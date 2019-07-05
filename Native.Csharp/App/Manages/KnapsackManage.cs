using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.App.EventArgs;

namespace Native.Csharp.App.Manages
{
    class KnapsackManage : BaseManage
    {
        public KnapsackManage() : base("背包管理") { }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            // TODO

            // 发送消息(响应)
            Common.CqApi.SendGroupMessage(e.FromGroup, action + "响应：" + e.Name);
        }
    }
}
