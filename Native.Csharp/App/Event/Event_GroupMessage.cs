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
            string message = facade.iniTool.IniReadValue(Facade.devPath, "游戏菜单.ini", e.Message, "内容");

            if (message != "") {
                Common.CqApi.SendGroupMessage(e.FromGroup, System.Text.RegularExpressions.Regex.Unescape(message));
                return;
            }

            string[] arr = e.Message.Split(' ');

            BaseManage baseManage;

            string groupId = e.FromGroup.ToString();

            if (groupId.Equals("7829338")) {
                groupId = "872042330";
            }

            string groupPath = Facade.devPath + "\\" + groupId;


            if (facade.managesDit.TryGetValue(arr[0], out baseManage)) {
                baseManage.Request(sender, e, groupPath);
                return;
            }

            string action = facade.iniTool.IniReadValue(Facade.devPath, "基础配置.ini", "路由", arr[0]);

            if (action == "")
            {
                return;
            }

            if (facade.managesDit.TryGetValue(action, out baseManage))
            {
                baseManage.Request(sender, e, groupPath);
                return;
            }

        }
    }
}
