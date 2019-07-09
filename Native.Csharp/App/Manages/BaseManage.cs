using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Native.Csharp.App.Manages
{
    abstract class BaseManage
    {
        protected String action = "base";
        protected Facade facade = Facade.facade;
        protected IniTool iniTool = Facade.facade.iniTool;
        protected string devPath = Facade.devPath;
        protected EventManage eventManage = Facade.facade.eventManage;

        public abstract void Request(object sender, CqGroupMessageEventArgs e);
        
        protected void AddManage() {
            facade.AddManage(action, this);
        }

        // 判断是否是用户获取用户名
        protected string GetUserName(string userId, string groupId) {

            string userName = iniTool.IniReadValue(devPath +"\\" + groupId, "用户信息.ini", userId, "角色名");

            if (userName != "" ) {
                return userName;
            }

            return "";
        }

        // 获取用户所有信息
        protected User GetUser(string userId, string groupId)
        {
            User user = new User();

            string userName = iniTool.IniReadValue(devPath + "\\" + groupId, "用户信息.ini", userId, "角色名");

            string userInfo = "";

            foreach (string name in GameConfig.userInfo) {
                userInfo += iniTool.IniReadValue(devPath + "\\" + groupId, "用户信息.ini", userId, name) + ",";
            }

            user.Add(userInfo);

            return user;
        }


        // 删除结尾换行符
        protected string SubRN(string str) {

            if (!str.Equals("")) {
                str = str.Substring(0, str.Length - Environment.NewLine.Length);
            }

            return str;
        }
    }
}
