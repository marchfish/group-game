using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class UserInfoManage : BaseManage
    {
        public UserInfoManage()
        {
            eventManage.UserUpEquip += UserUpEquip;
            eventManage.UserDownEquip += UserDownEquip;
            eventManage.EnemyDeath += IncreaseEXP;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), "角色名");

            if (userName == "")
            {
                return;
            }
 
            string userInfo = "";

            foreach (string user in GameConfig.userInfo) {
                if (user == "最大血量" || user  == "最大蓝量")
                {
                    continue ;
                }
                userInfo += user + "：" +  iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), user) + Environment.NewLine;
            }

            userInfo = SubRN( userInfo );

            Common.CqApi.SendGroupMessage(e.FromGroup, userInfo);
            return;
        }

        // 用户装备
        public void UserUpEquip(User user, Equip equip, string groupPath, string userId)
        {
            user.Agg += equip.Agg;
            user.HP += equip.HP;
            user.MP += equip.MP;
            user.Magic += equip.Magic;
            user.Dodge += equip.Dodge;
            user.Crit += equip.Crit;
            user.Defense += equip.Defense;

            iniTool.WriteInt(groupPath, userInfoIni, userId, "攻击力", user.Agg);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "血量", user.HP);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "蓝量", user.MP);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "魔法力", user.Magic);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "闪避率", user.Dodge);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "暴击率", user.Crit);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "防御力", user.Defense);
        }

        // 卸下装备
        public void UserDownEquip(User user, Equip equip, string groupPath, string userId)
        {
            user.Agg -= equip.Agg;
            user.HP -= equip.HP;
            user.MP -= equip.MP;
            user.Magic -= equip.Magic;
            user.Dodge -= equip.Dodge;
            user.Crit -= equip.Crit;
            user.Defense -= equip.Defense;

            iniTool.WriteInt(groupPath, userInfoIni, userId, "攻击力", user.Agg);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "血量", user.HP);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "蓝量", user.MP);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "魔法力", user.Magic);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "闪避率", user.Dodge);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "暴击率", user.Crit);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "防御力", user.Defense);
        }

        // 增加经验
        public void IncreaseEXP(User user, Enemy enemy, string groupPath, string userId)
        {
            user.Exp += enemy.Exp;

            iniTool.WriteInt(groupPath, userInfoIni, userId, "经验", user.Exp);
        }
    }
}
