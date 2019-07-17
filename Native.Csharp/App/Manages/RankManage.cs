using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System.Collections.Generic;

namespace Native.Csharp.App.Manages
{
    class RankManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            UpdateRank(groupPath, e);
        }

        public void UpdateRank(string groupPath, CqGroupMessageEventArgs e)
        {
            List<User> userInfo = new List<User>();

            List<string> users = iniTool.IniReadSection(groupPath, userInfoIni);

            foreach (string user in users)
            {
                userInfo.Add(GetUser(user, e.FromGroup.ToString()));
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

            foreach (User u in userInfo)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, u.Name + "：" + u.Level );
            }

          
        }  
    }
}
