using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class RegisterManage : BaseManage
    {
        public RegisterManage (){
            action = "注册用户";
            AddManage();
        }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string[] arr = e.Message.Split(' ');

            if (arr.Length < 2)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, action + "请输入：注册用户 你的名字");
                return;
            }

            // 读取ini
            iniTool.IniReadValue(devPath, "ini文件名", e.Message, "内容");

            // 写入ini
            iniTool.WriteInt(devPath + "\\" + e.FromGroup, "要写入的文件名.ini", "section名", "节点名", 1);

            // 发送消息(响应)
            Common.CqApi.SendGroupMessage(e.FromGroup, "功能暂未开发...");
        }
    }
}
