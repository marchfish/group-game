using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;

namespace Native.Csharp.App.Manages
{
    class RankManage : BaseManage
    {
        public RankManage() {
            eventManage.Uplevel += UpdateRank;
            eventManage.UserUpEquip += UpAggRank;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {

            string[] arr = e.Message.Split(' ');

            if (arr[0] == "攻击排行")
            {
                string panks = "[攻击排行]" + Environment.NewLine;

                panks += iniTool.IniReadValue(groupPath, panksIni, "攻击排行", "内容");

                if (panks == "") {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "暂无排行");
                }
                Common.CqApi.SendGroupMessage(e.FromGroup, System.Text.RegularExpressions.Regex.Unescape(panks));
                return;
            }

            if (arr[0] == "防御排行")
            {
                string panks = "[防御排行]" + Environment.NewLine;

                panks += iniTool.IniReadValue(groupPath, panksIni, "防御排行", "内容");

                if (panks == "")
                {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "暂无排行");
                }
                Common.CqApi.SendGroupMessage(e.FromGroup, System.Text.RegularExpressions.Regex.Unescape(panks));
                return;
            }

            string levelpanks = "[等级排行]" + Environment.NewLine ;

            levelpanks += iniTool.IniReadValue(groupPath, panksIni, "排行", "内容");

            if (levelpanks == "")
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "暂无排行");
            }

            Common.CqApi.SendGroupMessage(e.FromGroup, System.Text.RegularExpressions.Regex.Unescape(levelpanks));
        }

        public void UpdateRank(User user1, string groupPath, CqGroupMessageEventArgs e)
        {
            List<User> userInfo = new List<User>();

            List<string> users = iniTool.IniReadSection(groupPath, userInfoIni);

            foreach (string user in users)
            {
                userInfo.Add(GetUser(user, e, groupPath));
            }

            for (int i = 0; i < userInfo.Count; i++) {

                for (int j = i+1; j < userInfo.Count; j++) {

                    if (userInfo[i].Level < userInfo[j].Level)
                    {
                        User temp = userInfo[i];

                        userInfo[i] = userInfo[j];

                        userInfo[j] = temp;

                    }

                }

            }

            iniTool.DeleteSection(groupPath, panksIni, "排行");

            iniTool.DeleteSection(groupPath, panksIni, "攻击排行");

            int count = 1;

            string panks = "";

            foreach (User u in userInfo)
            {
                if (count > 10 ) {
                    break ;
                }

                panks += u.Name + "：" + u.Level + "\\r\\n";

                count++;
            }

            panks += "注：前十名";

            iniTool.IniWriteValue(groupPath, panksIni, "排行", "内容", panks);


            for (int i = 0; i < userInfo.Count; i++)
            {

                for (int j = i + 1; j < userInfo.Count; j++)
                {

                    if (userInfo[i].Agg < userInfo[j].Agg)
                    {
                        User temp = userInfo[i];

                        userInfo[i] = userInfo[j];

                        userInfo[j] = temp;

                    }

                }

            }

            count = 1;

            panks = "";

            foreach (User u in userInfo)
            {
                if (count > 10)
                {
                    break;
                }

                panks += u.Name + "：" + u.Agg + "\\r\\n";

                count++;
            }

            panks += "注：前十名";

            iniTool.IniWriteValue(groupPath, panksIni, "攻击排行", "内容", panks);


            for (int i = 0; i < userInfo.Count; i++)
            {

                for (int j = i + 1; j < userInfo.Count; j++)
                {

                    if (userInfo[i].Defense < userInfo[j].Defense)
                    {
                        User temp = userInfo[i];

                        userInfo[i] = userInfo[j];

                        userInfo[j] = temp;

                    }

                }

            }

            count = 1;

            panks = "";

            foreach (User u in userInfo)
            {
                if (count > 10)
                {
                    break;
                }

                panks += u.Name + "：" + u.Defense + "\\r\\n";

                count++;
            }

            panks += "注：前十名";

            iniTool.IniWriteValue(groupPath, panksIni, "防御排行", "内容", panks);

            return ;
        }

        public void UpAggRank(User user1, Equip equip, string groupPath, CqGroupMessageEventArgs e)
        {
            List<User> userInfo = new List<User>();

            List<string> users = iniTool.IniReadSection(groupPath, userInfoIni);

            foreach (string user in users)
            {
                userInfo.Add(GetUser(user, e, groupPath));
            }

            for (int i = 0; i < userInfo.Count; i++)
            {

                for (int j = i + 1; j < userInfo.Count; j++)
                {

                    if (userInfo[i].Level < userInfo[j].Level)
                    {
                        User temp = userInfo[i];

                        userInfo[i] = userInfo[j];

                        userInfo[j] = temp;

                    }

                }

            }

            iniTool.DeleteSection(groupPath, panksIni, "攻击排行");

            int count = 1;

            string panks = "";

            for (int i = 0; i < userInfo.Count; i++)
            {

                for (int j = i + 1; j < userInfo.Count; j++)
                {

                    if (userInfo[i].Agg < userInfo[j].Agg)
                    {
                        User temp = userInfo[i];

                        userInfo[i] = userInfo[j];

                        userInfo[j] = temp;

                    }

                }

            }

            foreach (User u in userInfo)
            {
                if (count > 10)
                {
                    break;
                }

                panks += u.Name + "：" + u.Agg + "\\r\\n";

                count++;
            }

            panks += "注：前十名";

            iniTool.IniWriteValue(groupPath, panksIni, "攻击排行", "内容", panks);

            iniTool.DeleteSection(groupPath, panksIni, "防御排行");

            for (int i = 0; i < userInfo.Count; i++)
            {

                for (int j = i + 1; j < userInfo.Count; j++)
                {

                    if (userInfo[i].Defense < userInfo[j].Defense)
                    {
                        User temp = userInfo[i];

                        userInfo[i] = userInfo[j];

                        userInfo[j] = temp;

                    }

                }

            }

            count = 1;

            panks = "";

            foreach (User u in userInfo)
            {
                if (count > 10)
                {
                    break;
                }

                panks += u.Name + "：" + u.Defense + "\\r\\n";

                count++;
            }

            panks += "注：前十名";

            iniTool.IniWriteValue(groupPath, panksIni, "防御排行", "内容", panks);

            return;
        }
    }
}
