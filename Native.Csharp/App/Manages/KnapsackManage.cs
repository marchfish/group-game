using System;
using System.Collections.Generic;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

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

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            string[] arr = e.Message.Split(' ');

            if (arr.Length > 1)
            {

                if (Int32.TryParse(arr[1], out int num))
                {

                    ShowPage(user, e, groupPath, KnapsackIni, num);

                    return;
                }

            }

            ShowPage(user, e, groupPath, KnapsackIni);

            return;
        }

        public void AddUserKnapsack(string userId, string groupPath) {
            iniTool.IniWriteValue(groupPath, KnapsackIni, userId, "金币", "5000");
            iniTool.IniWriteValue(groupPath, KnapsackIni, userId, "木棍", "1");
        }
    }
}
