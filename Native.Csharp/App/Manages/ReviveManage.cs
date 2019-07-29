
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class ReviveManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), groupPath);

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            if (user.HP > 0) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您没有死亡！");
                return;
            }

            Revive(user, groupPath, e);
        }

        // 复活
        private void Revive(User user, string groupPath, CqGroupMessageEventArgs e)
        {
            int myCoin = GetKnapsackItemNum("金币", groupPath, e.FromQQ.ToString());

            int cost = iniTool.ReadInt(devPath, reviveIni, user.Fame, "金币", 1000);

            if (myCoin < cost) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您的金币不足：" + cost.ToString());
                return;
            }

            if (DeleteKnapsackItemNum("金币", myCoin, cost, groupPath, e.FromQQ.ToString()))
            {
                string pos = iniTool.IniReadValue(devPath, reviveIni, user.Fame, "位置");

                if (pos != "")
                {
                    user.Pos = pos;
                    iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "当前位置", user.Pos);
                }

                user.HP = user.MaxHP;
                iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "血量", user.HP.ToString());

                Common.CqApi.SendGroupMessage(e.FromGroup, user.Name + "成功复活, -" + cost + "金币, 当前位置：" + user.Pos);
                return;
            }
        }
    }
}
