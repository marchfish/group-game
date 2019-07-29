using System;
using System.Collections.Generic;
using Native.Csharp.App.EventArgs;

namespace Native.Csharp.App.Manages
{
    class KnapsackManage : BaseManage
    {
        public KnapsackManage()
        {
            eventManage.RegisterUser += AddUserKnapsack;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), groupPath);

            if (userName == "")
            {
                return;
            }

            string userKnapsack = "[" + userName + "]" + Environment.NewLine;

            List<string> items = iniTool.IniReadSectionKey(groupPath, KnapsackIni, e.FromQQ.ToString());

            foreach (string item in items)
            {
                string res = iniTool.IniReadValue(groupPath, KnapsackIni, e.FromQQ.ToString(), item);

                if (res != "")
                {
                    userKnapsack += item + "：" + res + Environment.NewLine;
                }
            }

            userKnapsack = SubRN(userKnapsack);

            Common.CqApi.SendGroupMessage(e.FromGroup, userKnapsack);
            return;
            
        }

        public void AddUserKnapsack(string userId, string groupPath) {
            iniTool.IniWriteValue(groupPath, KnapsackIni, userId, "金币", "5000");
            iniTool.IniWriteValue(groupPath, KnapsackIni, userId, "木棍", "1");
        }
    }
}
