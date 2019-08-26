
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class ChallengeManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), groupPath);

            if (userName == "")
            {
                return;
            }

            string[] arr = e.Message.Split(' ');

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            string startTime = iniTool.IniReadValue(groupPath, challengeInfoIni, "时间", "内容");

            if (startTime == "") {
                NewRank(e, groupPath);
                return;
            }

            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DateTime endTime = Convert.ToDateTime(startTime);

            int compNum = DateTime.Compare(nowTime, endTime);

            if (compNum > 0)
            {

                iniTool.IniWriteValue(groupPath, challengeInfoIni, "时间", "内容", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                Reward(e, groupPath);

                return;
            }

            if (arr[0] == "挑战") {
                Challenge(e, user, groupPath);
                return;
            }

            if (arr[0] == "排位奖励")
            {
                ShowReward(e, groupPath, startTime);
                return;
            }

            ShowRank(e, groupPath, startTime);
            return;
        }
        private void Challenge(CqGroupMessageEventArgs e, User user, string groupPath) {
            int userNum = iniTool.ReadInt(groupPath, challengeInfoIni, e.FromQQ.ToString(), "名次", 0);

            if (userNum == 1)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您已到达了巅峰！");
                return;
            }

            string pkTime = iniTool.IniReadValue(groupPath, challengeInfoIni, e.FromQQ.ToString(), "挑战时间");

            if (pkTime != "") {
                string isCd = IsCD(pkTime);

                if (isCd != "")
                {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "挑战冷却时间(秒)：" + isCd);
                    return;
                }
            }

            int myCoin = GetKnapsackItemNum("金币", groupPath, e.FromQQ.ToString());

            if (myCoin < 500) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 您的金币数量不足：500");
                return;
            }

            DeleteKnapsackItemNum("金币", myCoin, 500, groupPath, e.FromQQ.ToString());

            iniTool.IniWriteValue(groupPath, challengeInfoIni, e.FromQQ.ToString(), "挑战时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            if (userNum == 0) {
                for (int i = 1; i <= 10; i++)
                {
                    string name = iniTool.IniReadValue(groupPath, challengeInfoIni, "排名", i.ToString());
                    if (name.Equals("无")) {
                        iniTool.IniWriteValue(groupPath, challengeInfoIni, "排名", i.ToString(), user.Name + "|" + e.FromQQ.ToString());
                        iniTool.IniWriteValue(groupPath, challengeInfoIni, e.FromQQ.ToString(), "名次", i.ToString());
                        Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 挑战成功！ 当前排名：" + i.ToString() );
                        return;
                    }
                }

                PK(e, groupPath, user, userNum, 10);
                return;
            }

            PK(e, groupPath, user, userNum, userNum - 1);
            return;
        }

        private void PK(CqGroupMessageEventArgs e, string groupPath, User user, int userNum, int userNum1) {
            string userName1 = iniTool.IniReadValue(groupPath, challengeInfoIni, "排名", userNum1.ToString());
            string[] userName = userName1.Split('|');
            bool isOver = true;

            User user1 = GetUser(userName[1], e, groupPath);
            user.HP = user.MaxHP;
            user1.HP = user1.MaxHP;

            while (isOver) { 

            int hurt = user.Agg - user1.Defense;
            
            // 挑战者
            if (hurt > 0)
            {
                int uDodge = random.Next(0, 100);

                if (user1.Dodge < uDodge)
                {
                    int hurt1 = hurt;
                    hurt += Crit(user, hurt);
                    user1.HP -= hurt;
                }
            }
            else
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 挑战失败，排名无变化！");
                return;
            }

            // 被挑战者
            hurt = user1.Agg - user.Defense;
            if (hurt > 0)
            {
                int uDodge = random.Next(0, 100);

                if (user.Dodge < uDodge)
                {
                    int hurt1 = hurt;
                    hurt += Crit(user1, hurt);
                    user.HP -= hurt;
                }
            }
            else
            {
                iniTool.IniWriteValue(groupPath, challengeInfoIni, "排名", userNum1.ToString(), user.Name + "|" + e.FromQQ.ToString());
                iniTool.IniWriteValue(groupPath, challengeInfoIni, e.FromQQ.ToString(), "名次", userNum1.ToString());

                    if (userNum == 0)
                    {
                        iniTool.IniWriteValue(groupPath, challengeInfoIni, userName[1].ToString(), "名次", userNum.ToString());
                    }
                    else {
                        iniTool.IniWriteValue(groupPath, challengeInfoIni, "排名", userNum.ToString(), user1.Name + "|" + userName[1]);
                        iniTool.IniWriteValue(groupPath, challengeInfoIni, userName[1].ToString(), "名次", userNum.ToString());
                    }
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 挑战成功！ 当前排名：" + userNum1.ToString());
                return;
            }

            if (user.HP <= 0 || user1.HP <= 0) {
                    isOver = false;
            }
            }

            if (user.HP > user1.HP)
            {
                iniTool.IniWriteValue(groupPath, challengeInfoIni, "排名", userNum1.ToString(), user.Name + "|" + e.FromQQ.ToString());
                iniTool.IniWriteValue(groupPath, challengeInfoIni, e.FromQQ.ToString(), "名次", userNum1.ToString());

                if (userNum == 0)
                {
                    iniTool.IniWriteValue(groupPath, challengeInfoIni, userName[1].ToString(), "名次", userNum.ToString());
                }
                else {
                    iniTool.IniWriteValue(groupPath, challengeInfoIni, "排名", userNum.ToString(), user1.Name + "|" + userName[1]);
                    iniTool.IniWriteValue(groupPath, challengeInfoIni, userName[1].ToString(), "名次", userNum.ToString());
                }

                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 挑战成功！ 当前排名：" + userNum1.ToString());
                return;
            }
            else {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 挑战失败，排名无变化！");
                return;
            }

        }

        private void ShowRank(CqGroupMessageEventArgs e, string groupPath, string endTime) {
            string res = "[排位榜]" + Environment.NewLine;

            for (int i = 1; i <= 10; i++)
            {
                string name = iniTool.IniReadValue(groupPath, challengeInfoIni, "排名", i.ToString());

                if (!name.Equals("无")) {
                    string[] arr = name.Split('|');
                    res += i.ToString() + "、" + arr[0] + Environment.NewLine;
                    continue;
                }

                res += i.ToString() + "、" + name + Environment.NewLine;
            }

            res += "赛季结束日期：" + endTime;

            Common.CqApi.SendGroupMessage(e.FromGroup, res);
            return;
        }

        private void ShowReward(CqGroupMessageEventArgs e, string groupPath, string endTime)
        {
            string res = "[排位奖励]" + Environment.NewLine;

            for (int i = 1; i <= 10; i++)
            {
                string reward = iniTool.IniReadValue(devPath, challengeIni, "奖励", i.ToString());

                res += i.ToString() + "、" + reward.Replace("|", ",") + Environment.NewLine;
            }

            res += "赛季结束日期：" + endTime;

            Common.CqApi.SendGroupMessage(e.FromGroup, res);
            return;
        }

        //发放奖励
        private void Reward(CqGroupMessageEventArgs e, string groupPath)
        {
            string res = "[排位奖励]" + Environment.NewLine;

            for (int i = 1; i <= 10; i++)
            {
                string name = iniTool.IniReadValue(groupPath, challengeInfoIni, "排名", i.ToString());

                if (!name.Equals("无"))
                {
                    string[] arr = name.Split('|');

                    string rewards = iniTool.IniReadValue(devPath, challengeIni, "奖励", i.ToString());
                    string[] reward = rewards.Split('|');

                    foreach (string rs in reward) {
                        string[] r = rs.Split('*');
                        SetKnapsackItemNum(r[0], int.Parse(r[1]), groupPath, arr[1]);
                    }
                    continue;
                }
            }

            iniTool.DeleteAllSection(groupPath, challengeInfoIni);

            NewRank(e, groupPath);

            return;
        }

        // 新排位
        private void NewRank(CqGroupMessageEventArgs e, string groupPath) {
            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            DateTime dateTime = nowTime.AddMonths(1);

            iniTool.IniWriteValue(groupPath, challengeInfoIni, "时间", "内容", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            for (int i = 1; i <= 10; i++) {
                iniTool.IniWriteValue(groupPath, challengeInfoIni, "排名", i.ToString(), "无");
            }

            Common.CqApi.SendGroupMessage(e.FromGroup, "新赛季开始，暂无排名!");
            return;
        }

        private int Crit(User user, int hurt, bool isUser = true)
        {

            int rNum = random.Next(0, 100);

            hurt = random.Next(0, hurt);

            if (rNum < 50)
            {
                if (isUser)
                {

                    rNum = random.Next(0, 100);

                    if (user.Crit >= rNum)
                    {

                        return hurt;
                    }
                }

                hurt *= -1;
            }

            return hurt;
        }

        // 是否在冷却中
        private string IsCD(string pkTime)
        {
            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            if (pkTime != "")
            {
                DateTime pkTime1 = Convert.ToDateTime(pkTime);

                TimeSpan timeSpan = nowTime.Subtract(pkTime1);

                if (timeSpan.TotalSeconds < 3600)
                {
                    return (3600 - timeSpan.TotalSeconds).ToString();
                }
            }

            return "";
        }
    }
}
