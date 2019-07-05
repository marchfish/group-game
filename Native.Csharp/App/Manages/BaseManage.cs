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
        protected MidirTool midirTool = Facade.facade.midirTool;
        protected string devPath = Facade.devPath;

        public abstract void Request(object sender, CqGroupMessageEventArgs e);
        
        protected void AddManage() {
            facade.AddManage(action, this);
        }
    }
}
