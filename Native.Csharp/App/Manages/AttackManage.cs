using System;
using System.Linq;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class AttackManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName == "")
            {
                return;
            }

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

        private void Attack(string groupPath, CqGroupMessageEventArgs e, int num = 0) {

            string res = "";

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

            if (num > 1)
            {
                for (int i = 0; i < num; i++)
                {
                    res = Fight(user, enemy, e, groupPath);

                    if (res == "")
                    {
                        return;
                    };
                }
            }
            else {
                res = Fight(user, enemy, e, groupPath);
            }

            if (res == "")
            {
                return ;
            };

            AddFight(user, enemy, e.FromQQ.ToString(), e.FromGroup.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, res);
  
        }

        private string Fight(User user, Enemy enemy, CqGroupMessageEventArgs e, string groupPath) {

            int enemyhurt = user.Agg - enemy.Defense;

            string res = "";

            if (enemyhurt > 0)
            {
                enemy.HP -= enemyhurt;

                if (enemy.HP <= 0)
                {
                    res += enemy.Name + " 被 " + user.Name + " 击败了!";

                    res += Environment.NewLine + SetItem(enemy, e, groupPath);

                    iniTool.DeleteSection(groupPath, fightIni, e.FromQQ.ToString());

                    eventManage.OnEnemyDeath(user, enemy, groupPath, e.FromQQ.ToString());

                    Common.CqApi.SendGroupMessage(e.FromGroup, res);
                    return "";
                }
              
                res += user.Name + " 攻击 " + enemy.Name + "，" + enemy.Name + "的血量 -" + enemyhurt + " 剩余：" + enemy.HP + Environment.NewLine;
                

            } else {
                res += user.Name + " 攻击 " + enemy.Name + "，" + enemy.Name + "的血量 -" + enemyhurt + " 剩余：" + enemy.HP + Environment.NewLine;
            }

            int userhurt = enemy.Agg - user.Defense;

            if (userhurt > 0) {

                user.HP -= userhurt;

                if (user.HP <= 0)
                {
                    res += user.Name + " 被 " + enemy.Name + " 击败了!";
                    iniTool.DeleteSection(devPath + "\\" + e.FromGroup.ToString(), fightIni, e.FromQQ.ToString());
                    Common.CqApi.SendGroupMessage(e.FromGroup, res);
                    return "";
                }
                
                res += enemy.Name + " 攻击 " + user.Name + "，" + user.Name + "的血量 -" + userhurt + " 剩余：" + user.HP;
              

            }
            else
            {
                res += enemy.Name + " 攻击 " + user.Name + "，" + user.Name + "的血量 -" + userhurt + " 剩余：" + user.HP;
            }

            return res;
        }

        //设置获得物品
        private string SetItem(Enemy enemy, CqGroupMessageEventArgs e, string groupPath) {
            Random random = new Random();

            int rNum = random.Next(0, 100);

            if (rNum <= enemy.Probability)
            {
                string items = iniTool.IniReadValue(devPath, enemyIni, enemy.Name, "掉落物品");

                int enemyCoin = iniTool.ReadInt(devPath, enemyIni, enemy.Name, "金币", 0);

                string[] arr = items.Split('|');

                rNum = random.Next(0, arr.Count());

                arr = arr[rNum].Split('*');

                int item = iniTool.ReadInt(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[0], 0);

                int myCoin = iniTool.ReadInt(groupPath, KnapsackIni, e.FromQQ.ToString(), "金币", 0);

                iniTool.IniWriteValue(groupPath, KnapsackIni, e.FromQQ.ToString(), "金币", (myCoin + enemyCoin).ToString());
                iniTool.IniWriteValue(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[0], (int.Parse(arr[1]) + item).ToString());

                return "获得：金币*" + enemyCoin.ToString() + ", " + arr[0] + "*" + arr[1] + ", 经验增加:" + enemy.Exp.ToString();
            }

            return null;
        }

    }
}
