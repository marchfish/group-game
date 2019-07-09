using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class AttackManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string groupPath = devPath + "\\" + e.FromGroup;

            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName != "")
            {
                string[] arr = e.Message.Split(' ');

                if (arr.Length > 1)
                {
                    //return;
                }

                Attack(groupPath, userName, e);

                return;
            }
        }

        private void Attack(string groupPath, string userName, CqGroupMessageEventArgs e) {

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            string pos = iniTool.IniReadValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "当前位置");

            if ( !pos.Equals(user.Pos) ) {
                iniTool.IniWriteValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "角色名", user.Name);
                iniTool.IniWriteValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "角色血量", user.HP);
                iniTool.IniWriteValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "角色蓝量", user.MP);
                iniTool.IniWriteValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "怪物", "野猪");
                iniTool.IniWriteValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "怪物血量", "30");
                iniTool.IniWriteValue(groupPath, "战斗配置.ini", e.FromQQ.ToString(), "当前位置", user.Pos);
            }

            Common.CqApi.SendGroupMessage(e.FromGroup, user.Name + "攻击了野猪，野猪 HP-10" );

            user = null;
        }


    }
}
