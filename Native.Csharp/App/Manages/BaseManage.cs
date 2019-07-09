using Native.Csharp.App.EventArgs;
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

        protected bool isUser(string userId, string groupId) {

            string userName = iniTool.IniReadValue(devPath +"\\" + groupId, "用户信息.ini", userId, "角色名");

            if (userName != null ) {

                return true;

            }

            return false;
        }
    }
}
