using Native.Csharp.App.EventArgs;
using System;

namespace Native.Csharp.App.Manages
{
    class VipManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            DateTime dateTime = nowTime.AddMonths(1);

            Common.CqApi.SendGroupMessage(e.FromGroup, dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
