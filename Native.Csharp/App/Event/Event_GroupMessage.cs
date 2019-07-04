using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Interface;
using Native.Csharp.App.Manages;

namespace Native.Csharp.App.Event
{
    class Event_GroupMessage : IReceiveGroupMessage
    {
        Facade facade = new Facade();
        String message = "注册用户" + Environment.NewLine + "游戏介绍" + Environment.NewLine + "游戏功能";

        public void ReceiveGroupMessage(object sender, CqGroupMessageEventArgs e)
        {
            if (e.Message == "当前位置") {

                IniTool iniTool = new IniTool(System.Windows.Forms.Application.StartupPath + "\\dev\\test.ini");

                message = iniTool.IniReadValue("地图配置", "出生地");

                iniTool.WriteInt("test", "Name", 890);
                iniTool.WriteString("test", "Name", "名字");

                Common.CqApi.SendGroupMessage(e.FromGroup, message);
                return;

            }

            BaseManage baseManage;
            if (facade.managesDit.TryGetValue(e.Message, out baseManage)) {
                baseManage.Request(sender, e);
                return;
            }
        }
    }
}
