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

        // 攻击事件
        private void Attack(string groupPath, CqGroupMessageEventArgs e, int num = 0) {

            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            string res = "";

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (user.HP <= 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您已死亡：请复活后继续!");
                return ;
            }

            string pos = iniTool.IniReadValue(groupPath, fightIni, e.FromQQ.ToString(), "当前位置");

            string enemyName = iniTool.IniReadValue(devPath, mapIni, user.Pos, "怪物");

            if (enemyName == "")
            {
                return ;
            }

            string lastTime = iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), "攻击时间");

            if (lastTime != "")
            {
                DateTime lastTime1 = Convert.ToDateTime(lastTime);

                TimeSpan timeSpan = nowTime.Subtract(lastTime1);

                if (timeSpan.TotalSeconds < 16)
                {
                    user.isShowMessage = false;
                }
            }
         
            iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "攻击时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

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

            // 添加超频判断
            if (user.isShowMessage)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, res);
            }

        }

        // 战斗
        private string Fight(User user, Enemy enemy, CqGroupMessageEventArgs e, string groupPath) {

            Random random = new Random();

            int enemyhurt = user.Agg - enemy.Defense;

            string res = "";

            string isCrit = "";

            if (enemyhurt > 0)
            {
                int eDodge = random.Next(0, 100);

                if (enemy.Level - user.Level > eDodge)
                {
                    res += user.Name + " 攻击 " + enemy.Name +  "-(闪避了)，" + enemy.Name + "的血量 剩余：" + enemy.HP + Environment.NewLine;
                }
                else { 

                    int hurt1 = enemyhurt;

                    enemyhurt += Crit(user, enemyhurt);

                    if (enemyhurt > hurt1 + hurt1 * 0.25) {
                        isCrit = "-(受到暴击伤害)";
                    }

                    enemy.HP -= enemyhurt;

                    if (enemy.HP <= 0)
                    {
                        res += enemy.Name + " 被 " + user.Name + " 击败了!";

                        res += SetItem(enemy, e, groupPath);

                        iniTool.DeleteSection(groupPath, fightIni, e.FromQQ.ToString());

                        res += Environment.NewLine  + "经验增加：" + enemy.Exp.ToString() + Environment.NewLine;

                        res += "当前血量：" + user.HP;

                        if (user.isShowMessage)
                        {
                            Common.CqApi.SendGroupMessage(e.FromGroup, res);
                        }

                        eventManage.OnEnemyDeath(user, enemy, groupPath, e);

                        return "";
                    }
              
                    res += user.Name + " 攻击 " + enemy.Name + isCrit + "，" + enemy.Name + "的血量 -" + enemyhurt + " 剩余：" + enemy.HP + Environment.NewLine;
                }
            } else {

                res += user.Name + " 攻击 " + enemy.Name + "，" + enemy.Name + "的血量 -" + 0 + " 剩余：" + enemy.HP + Environment.NewLine;

            }

            int userhurt = enemy.Agg - user.Defense;

            if (userhurt > 0) {

                int uDodge = random.Next(0, 100);

                if (user.Dodge > uDodge)
                {
                    res += enemy.Name + " 攻击 " + user.Name + "-(闪避了)，" + user.Name + "的血量 剩余：" + user.HP;
                }
                else
                {
                    userhurt += Crit(user, userhurt, false);

                    user.HP -= userhurt;

                    if (user.HP <= 0)
                    {
                        res += user.Name + " 被 " + enemy.Name + " 击败了!";
                        iniTool.DeleteSection(devPath + "\\" + e.FromGroup.ToString(), fightIni, e.FromQQ.ToString());

                        // 记录用户血量
                        iniTool.WriteInt(groupPath, userInfoIni, e.FromQQ.ToString(), "血量", 0);

                        if (user.isShowMessage)
                        {
                            Common.CqApi.SendGroupMessage(e.FromGroup, res);
                        }

                        return "";
                    }

                    iniTool.WriteInt(groupPath, userInfoIni, e.FromQQ.ToString(), "血量", user.HP);

                    res += enemy.Name + " 攻击 " + user.Name + "，" + user.Name + "的血量 -" + userhurt + " 剩余：" + user.HP;
                }

            }
            else
            {
                res += enemy.Name + " 攻击 " + user.Name + "，" + user.Name + "的血量 -" + 0 + " 剩余：" + user.HP;
            }

            return res;
        }

        //设置获得物品
        private string SetItem(Enemy enemy, CqGroupMessageEventArgs e, string groupPath) {
            Random random = new Random();

            int rNum = random.Next(0, 100);

            string[] certainArr;

            string res = Environment.NewLine + "获得：";

            if (enemy.Certain != "") {
                certainArr = enemy.Certain.Split('*');
                int item = iniTool.ReadInt(groupPath, KnapsackIni, e.FromQQ.ToString(), certainArr[0], 0);
                iniTool.IniWriteValue(groupPath, KnapsackIni, e.FromQQ.ToString(), certainArr[0], (int.Parse(certainArr[1]) + item).ToString());
                res += certainArr[0] + "*" + certainArr[1];
            }

            int enemyCoin = iniTool.ReadInt(devPath, enemyIni, enemy.Name, "金币", 0);

            int myCoin = iniTool.ReadInt(groupPath, KnapsackIni, e.FromQQ.ToString(), "金币", 0);

            if (rNum <= enemy.Probability)
            {
                string items = iniTool.IniReadValue(devPath, enemyIni, enemy.Name, "掉落物品");

                string[] arr = items.Split('|');

                rNum = random.Next(0, arr.Count());

                arr = arr[rNum].Split('*');

                int item1 = iniTool.ReadInt(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[0], 0);

                iniTool.IniWriteValue(groupPath, KnapsackIni, e.FromQQ.ToString(), "金币", (myCoin + enemyCoin).ToString());
                iniTool.IniWriteValue(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[0], (int.Parse(arr[1]) + item1).ToString());

                if (enemy.Certain != "")
                {
                    res += ", 金币*" + enemyCoin.ToString() + ", " + arr[0] + "*" + arr[1];
                }
                else {
                    res += "金币*" + enemyCoin.ToString() + ", " + arr[0] + "*" + arr[1];
                }

                return res;
            }

            if (enemy.Certain == "")
            {
                iniTool.IniWriteValue(groupPath, KnapsackIni, e.FromQQ.ToString(), "金币", (myCoin + enemyCoin).ToString());

                return Environment.NewLine + "获得：金币*" + enemyCoin.ToString();
            }

            return res;
        }

        private int Crit(User user, int hurt, bool isUser = true) {

            Random random = new Random();

            int rNum = random.Next(0, 100);

            hurt = random.Next(0, hurt);

            if (rNum < 50) {
                if (isUser) {

                    rNum = random.Next(0, 100);

                    if (user.Crit >= rNum) {

                        return hurt;
                    }
                }

                hurt *= -1;
            }

            return hurt;
        }
    }
}
