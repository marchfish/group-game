using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class UserInfo : BaseManage
    {
        private string iniName = "用户信息.ini";

        public UserInfo()
        {
            action = "角色信息";
            AddManage();
        }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string groupPath = devPath + "\\" + e.FromGroup;

            string userID = iniTool.IniReadValue(groupPath, iniName, e.FromQQ.ToString(), "角色名");

            if (userID != "")
            {
                string userInfo = "";

                foreach (string user in GameConfig.userInfo) {
                    userInfo += user + "：" +  iniTool.IniReadValue(groupPath, iniName, e.FromQQ.ToString(), user) + Environment.NewLine;
                }

                userInfo = userInfo.Substring(0, userInfo.Length - Environment.NewLine.Length);

                Common.CqApi.SendGroupMessage(e.FromGroup, userInfo);
                return;
            }
        }
    }
}
