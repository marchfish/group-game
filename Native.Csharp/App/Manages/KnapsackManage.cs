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
        private string iniName = "背包信息.ini";

        public KnapsackManage()
        {
            action = "背包";
            AddManage();
            eventManage.registerUser += AddUserKnapsack;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string groupPath = devPath + "\\" + e.FromGroup;

            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName != "")
            {
                string userKnapsack = "[" + userName + "]" + Environment.NewLine;

                List<string> items = iniTool.IniReadSectionKey(groupPath, iniName, e.FromQQ.ToString());

                foreach (string item in items)
                {
                    string res = iniTool.IniReadValue(groupPath, iniName, e.FromQQ.ToString(), item);

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
            iniTool.IniWriteValue(groupPath, iniName, userId, "金币", "5000");
            iniTool.IniWriteValue(groupPath, iniName, userId, "木棍", "1");
        }
    }
}
