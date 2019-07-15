using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.App.Configs;

namespace Native.Csharp.App.Manages
{
    class RegisterManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string[] arr = e.Message.Split(' ');

            if (arr.Length < 2)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "请输入：注册用户 你的名字");
                return;
            }

            // 读取ini
            string userID =  iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), "角色名");

            if (userID != "")
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您已经注册了用户：" + userID);
                return;
            }

            for (int i=0; i< GameConfig.userInfo.Length; i++ )
            {
                if(i == 0){
                    // 写入ini
                    iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "角色名", arr[1]);
                    continue ;
                }

                iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), GameConfig.userInfo[i], GameConfig.userInfoDefault[i]);
            }


            // 触发订阅
            eventManage.OnRegisterUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            // 发送消息(响应)
            Common.CqApi.SendGroupMessage(e.FromGroup, arr[1] +  " 注册成功，开始冒险吧！");
        }
    }
}
