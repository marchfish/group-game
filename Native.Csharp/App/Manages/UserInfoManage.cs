using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class UserInfoManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string groupPath = devPath + "\\" + e.FromGroup;

            string userName = iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), "角色名");

            if (userName == "")
            {
                return;
            }
 
            string userInfo = "";

            foreach (string user in GameConfig.userInfo) {
                userInfo += user + "：" +  iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), user) + Environment.NewLine;
            }

            userInfo = userInfo.Substring(0, userInfo.Length - Environment.NewLine.Length);

            Common.CqApi.SendGroupMessage(e.FromGroup, userInfo);
            return;
        }
    }
}
