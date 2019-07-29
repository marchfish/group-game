using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class VipManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), "角色名");

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString(), e);

            string[] arr = e.Message.Split(' ');

            if (arr[0] == "购买会员")
            {
                buyVip(e, groupPath);
                return;
            }

            if (arr[0] == "结束挂机")
            {
                endOnHook(user, e, groupPath);
                return;
            }

            if (!user.isVip)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "您的会员已到期！" + Environment.NewLine + "重新购买请输入：购买会员");
                return;
            }

            if (arr[0] == "挂机") {

                if (arr.Length > 1)
                {
                    if (Int32.TryParse(arr[1], out int num))
                    {
                        setOnHook(num, user, e);
                        return;
                    }
                }

                Common.CqApi.SendGroupMessage(e.FromGroup, "挂机 1 (每分钟增加10点经验)" + Environment.NewLine + "挂机 2 (每分钟增加5个金币)");

                return;
            }

            if (arr[0] == "保护")
            {

                if (arr.Length > 1)
                {
                    if (Int32.TryParse(arr[1], out int num))
                    {
                        user.Protect = num;

                        setProtect(user, e, groupPath);
                        return;
                    }
                }

                showProtect(user, e);

                return;
            }

            if (arr[0] == "会员时间")
            {

                Vip vip = GetVipInfo(e);

                Common.CqApi.SendGroupMessage(e.FromGroup, "您的会员到期时间为：" + vip.endTime);

                return;
            }
        }

        private void setOnHook(int onHookType, User user, CqGroupMessageEventArgs e)
        {
            Vip vip = GetVipInfo(e);

            if (vip.OnHookTime != "")
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您已经处于挂机中，挂机时间：" + vip.OnHookTime);
                return;
            }

            string type = onHookType == 2 ? "金币" : "经验" ; 

            vip.OnHookTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            vip.OnHookType = type;

            SetVipInfo(vip, e);

            Common.CqApi.SendGroupMessage(e.FromGroup, "挂机成功，时间：" + vip.OnHookTime);
            return;
        }

        private void endOnHook(User user, CqGroupMessageEventArgs e, string groupPath)
        {
            Vip vip = GetVipInfo(e);

            if (vip.OnHookTime == "")
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您未处于挂机中");
                return;
            }

            DateTime startTime = Convert.ToDateTime(vip.OnHookTime);

            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            TimeSpan timeSpan = nowTime.Subtract(startTime);

            int mTime = (int)Math.Round( timeSpan.TotalMinutes );

            if (vip.OnHookType == "金币")
            {
                int coin = iniTool.ReadInt(devPath, vipIni, "挂机", "金币", 5);

                SetKnapsackItemNum("金币", coin * mTime, groupPath, e.FromQQ.ToString());

                Common.CqApi.SendGroupMessage(e.FromGroup, "共挂机：" + mTime + "分钟， 获得金币：" + mTime * coin);
            }
            else
            {
                int exp = iniTool.ReadInt(devPath, vipIni, "挂机", "经验", 10);

                user.Exp += exp * mTime;

                iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "经验", user.Exp.ToString());

                Common.CqApi.SendGroupMessage(e.FromGroup, "共挂机：" + mTime + "分钟， 获得经验：" + mTime * exp);

                eventManage.OnIsUplevel(user, groupPath, e);
            }

            vip.OnHookTime = "";
            vip.OnHookType = "";

            SetVipInfo(vip, e);

            return;
        }

        private void showProtect(User user, CqGroupMessageEventArgs e)
        {
            string res = "您当前的保护为：" + user.Protect + Environment.NewLine;

            res += "输入：保护 50";

            Common.CqApi.SendGroupMessage(e.FromGroup, res);

            return;
        }

        private void setProtect(User user, CqGroupMessageEventArgs e, string groupPath)
        {
            if (user.Protect > user.MaxHP * 0.6)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "设置失败：保护不得超过最大生命的60%");

                return;
            }

            iniTool.WriteInt(groupPath, vipInfoIni, e.FromQQ.ToString(), "保护", user.Protect);

            Common.CqApi.SendGroupMessage(e.FromGroup, "设置成功，当前保护为：" + user.Protect);

            return;
        }

        private void buyVip(CqGroupMessageEventArgs e, string groupPath)
        {
            int coin = GetKnapsackItemNum("金币", groupPath, e.FromQQ.ToString());

            if (coin < 50000) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：您背包里的金币不足50000");

                return;
            }

            Vip vip = GetVipInfo(e);

            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DateTime endTime = Convert.ToDateTime(vip.endTime);
            DateTime dateTime;

            dateTime = nowTime.AddMonths(1);

            int compNum = DateTime.Compare(nowTime, endTime);

            if (compNum < 0)
            {
                dateTime = endTime.AddMonths(1);
            }

            vip.endTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            SetVipInfo(vip, e);

            DeleteKnapsackItemNum("金币", coin, 50000, groupPath, e.FromQQ.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, "购买成功，您的会员到期时间为：" + vip.endTime);

            return;
        }
    }
}
