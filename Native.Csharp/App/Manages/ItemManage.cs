
using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class ItemManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            // 用户验证
            if (userName == "")
            {
                return;
            }

            // 获取用户信息
            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString(), e);

            if (user.HP <= 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您已死亡：请复活后继续!");
                return;
            }

            string[] arr = e.Message.Split(' ');

            if (arr.Length > 1)
            {
                int itemNum = GetKnapsackItemNum(arr[1], groupPath, e.FromQQ.ToString());

                if (itemNum == 0) {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "您没有物品：" + arr[1]);
                    return;
                }

                int count = 1;

                if (arr.Length > 2)
                {
                    if (Int32.TryParse(arr[2], out int num))
                    {
                        if (itemNum < num)
                        {
                            Common.CqApi.SendGroupMessage(e.FromGroup, "您没有足够的物品数量：" + arr[1]);
                            return;
                        }

                        count = num;
                    }
                }

                string use = iniTool.IniReadValue(devPath, itemIni, arr[1], "效果");

                if (use == "") {
                    Common.CqApi.SendGroupMessage(e.FromGroup, arr[1] + " 不能使用");
                    return;
                }

                if (use == "恢复")
                {

                    RecoveryItem recoveryItem =  GetRecoveryItem(arr[1]);

                    user.HP += recoveryItem.HP * count;
                    user.MP += recoveryItem.MP * count;

                    if (user.HP > user.MaxHP) {
                        user.HP = user.MaxHP;
                    }

                    if (user.MP > user.MaxMP)
                    {
                        user.MP = user.MaxMP;
                    }

                    iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "血量", user.HP.ToString());
                    iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "蓝量", user.MP.ToString());

                    // 使用背包物品
                    DeleteKnapsackItemNum(arr[1], itemNum, 1, groupPath, e.FromQQ.ToString());

                    Common.CqApi.SendGroupMessage(e.FromGroup, "使用成功：" + arr[1] + "*" + count + Environment.NewLine + "当前血量：" + user.HP);
                    return ;
                }

            }

            return ;

        }

    }
}
