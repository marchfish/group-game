using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class MoveManage : BaseManage
    {
        public MoveManage() {
            eventManage.EnemyDeath += MoveForEnemy;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (user.HP <= 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您已死亡：请复活后继续!");
                return;
            }

            string[] arr = e.Message.Split(' ');

            if (arr.Length > 1)
            {
                string moveName = arr[1] + "传送卷";

                int myItem = GetKnapsackItemNum(moveName, groupPath, e.FromQQ.ToString());

                if (myItem == 0) {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您没有：" + moveName);
                    return;
                }

                user.Pos = arr[1];

                iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "当前位置", user.Pos);

                DeleteKnapsackItemNum(moveName, myItem, 1, groupPath, e.FromQQ.ToString());

                Common.CqApi.SendGroupMessage(e.FromGroup, "您已传送至：" + user.Pos);

                return;
            }
        }

        public void MoveForEnemy(User user, Enemy enemy, string groupPath, CqGroupMessageEventArgs e)
        {
            if (enemy.Move != "") {
                user.Pos = enemy.Move;

                iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "当前位置", user.Pos);

                Common.CqApi.SendGroupMessage(e.FromGroup, "您已传送至：" + user.Pos);
            }
        }
    }
}
