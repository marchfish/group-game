using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using Native.Csharp.App.Manages;

namespace Native.Csharp.App.Event
{
    class Event_GroupMessage : IReceiveGroupMessage
    {
        Facade facade = new Facade();

        public void ReceiveGroupMessage(object sender, CqGroupMessageEventArgs e)
        {
            BaseManage baseManage;
            if (facade.managesDit.TryGetValue(e.Message, out baseManage)) {
                baseManage.Request(sender, e);
                return;
            }
        }
    }
}
