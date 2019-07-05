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
        public RegisterManage() : base("注册管理") { }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            // TODO

            // 发送消息(响应)
            Common.CqApi.SendGroupMessage(e.FromGroup, action + "响应：" + e.Name);
        }
    }
}
