using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class LevelManage : BaseManage
    {
        public LevelManage (){
            eventManage.IsUplevel += Uplevel;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), groupPath);

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 您当前的等级为：" + user.Level);

            return;

        }

        // 等级提升
        public void Uplevel(User user, string groupPath, CqGroupMessageEventArgs e)
        {
            int count = 1;

            while (true) {

                Level nextlevel = GetLevel(user.Level + 1);

                if (user.Exp >= nextlevel.Exp)
                {
                    Level mylevel = GetLevel(user.Level);

                    user.Agg -= mylevel.Agg;
                    user.MaxHP -= mylevel.HP;
                    user.MaxMP -= mylevel.MP;
                    user.Defense -= mylevel.Defense;

                    user.Agg += nextlevel.Agg;
                    user.MaxHP += nextlevel.HP;
                    user.MaxMP += nextlevel.MP;
                    user.Defense += nextlevel.Defense;
                    user.Level = nextlevel.Name;
                    user.Fame = nextlevel.Fame;

                    SetUser(user, groupPath, e.FromQQ.ToString());

                    eventManage.OnUplevel(user, groupPath, e);

                    count++;
                }
                else
                {
                    if (count>1) {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "恭喜您等级提升为：" + user.Level);
                    }

                    break;
                }

            }
        }

        private void SetUser(User user, string groupPath, string userId) {

            iniTool.WriteInt(groupPath, userInfoIni, userId, "攻击力", user.Agg);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "最大血量", user.MaxHP);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "最大蓝量", user.MaxMP);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "防御力", user.Defense);
            iniTool.WriteInt(groupPath, userInfoIni, userId, "等级", user.Level);
            iniTool.IniWriteValue(groupPath, userInfoIni, userId, "称号", user.Fame);

        }
    }
}
