using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.App.EventArgs;

namespace Native.Csharp.App.Manages
{
    class KnapsackManage : BaseManage
    {
        public KnapsackManage()
        {
            eventManage.registerUser += AddUserKnapsack;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string groupPath = devPath + "\\" + e.FromGroup;

            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName != "")
            {
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
        }

        public void AddUserKnapsack(string userId, string groupId) {
            string groupPath = devPath + "\\" + groupId;
            iniTool.IniWriteValue(groupPath, KnapsackIni, userId, "金币", "5000");
            iniTool.IniWriteValue(groupPath, KnapsackIni, userId, "木棍", "1");
        }
    }
}
