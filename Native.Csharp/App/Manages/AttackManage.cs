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
                    if (Int32.TryParse(arr[1], out int num))
                    {
                        Attack(groupPath, e, num);
                    }

                    return;
                }

                Attack(groupPath, e);

                return;
            }
        }

        private void Attack(string groupPath, CqGroupMessageEventArgs e, int num = 0) {
          
            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            string pos = iniTool.IniReadValue(groupPath, fightIni, e.FromQQ.ToString(), "当前位置");

            string enemyName = iniTool.IniReadValue(devPath, mapIni, user.Pos, "怪物");

            if (enemyName == "")
            {
                return ;
            }

            Enemy enemy = GetEnemy(enemyName);

            if ( pos.Equals(user.Pos) ) {
                string enemyHP = iniTool.IniReadValue(groupPath, fightIni, e.FromQQ.ToString(), "怪物血量");
                enemy.HP = int.Parse(enemyHP);
            }
           
            string res = "";

            if (Fight(user, enemy, e, res))
            {
                return ;
            };

            AddFight(user, enemy, e.FromQQ.ToString(), e.FromGroup.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, res);
  
        }

        private bool Fight(User user, Enemy enemy, CqGroupMessageEventArgs e, string res) {

            int enemyhurt = user.Agg - enemy.Defense;

            if (enemyhurt > 0)
            {
                enemy.HP -= enemyhurt;

                if (enemy.HP <= 0)
                {
                    res += enemy.Name + "被：" + user.Name + "击败了!";
                    iniTool.DeleteSection(devPath + "\\" + e.FromGroup.ToString(), fightIni, e.FromQQ.ToString());
                    Common.CqApi.SendGroupMessage(e.FromGroup, res);
                    return true;
                }
              
                res += user.Name + " 攻击 " + enemy.Name + "，" + enemy.Name + "的血量 -" + enemyhurt + "剩余：" + enemy.HP + Environment.NewLine;
                

            } else {
                res += user.Name + " 攻击 " + enemy.Name + "，" + enemy.Name + "的血量 -" + enemyhurt + "剩余：" + enemy.HP + Environment.NewLine;
            }

            int userhurt = enemy.Agg - user.Defense;

            if (userhurt > 0) {

                user.HP -= userhurt;

                if (user.HP <= 0)
                {
                    res += user.Name + "被：" + enemy.Name + "击败了!";
                    iniTool.DeleteSection(devPath + "\\" + e.FromGroup.ToString(), fightIni, e.FromQQ.ToString());
                    Common.CqApi.SendGroupMessage(e.FromGroup, res);
                    return true;
                }
                
                res += enemy.Name + " 攻击 " + user.Name + "，" + user.Name + "的血量 -" + userhurt + "剩余：" + user.HP;
              

            }
            else
            {
                res += enemy.Name + " 攻击 " + user.Name + "，" + user.Name + "的血量 -" + userhurt + "剩余：" + user.HP;
            }

            return false;
        }


    }
}
