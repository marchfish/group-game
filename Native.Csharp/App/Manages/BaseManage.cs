using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    abstract class BaseManage
    {
        protected String action = "base";
        protected Facade facade = Facade.facade;

        public abstract void Request(object sender, CqGroupMessageEventArgs e);
        
        protected void AddManage() {
            facade.AddManage(action, this);
        }
    }
}
