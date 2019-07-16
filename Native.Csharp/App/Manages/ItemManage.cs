
using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class ItemManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            // 用户验证
            if (userName == "")
            {
                return;
            }

            // 获取用户信息
            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            string[] arr = e.Message.Split(' ');

            if (arr.Length > 1)
            {
                int itemNum = GetKnapsackItemNum(arr[1], groupPath, e.FromQQ.ToString());

                if (itemNum == 0) {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "您没有物品：" + arr[1]);
                    return;
                }

                string use = iniTool.IniReadValue(devPath, itemIni, arr[1], "效果");

                if (use == "") {
                    Common.CqApi.SendGroupMessage(e.FromGroup, arr[1] + " 不能使用");
                    return;
                }

                if (use == "恢复")
                {
                    RecoveryItem recoveryItem =  GetRecoveryItem(arr[1]);

                    user.HP += recoveryItem.HP;
                    user.MP += recoveryItem.MP;

                    if (user.HP > user.MaxHP) {
                        user.HP = user.MaxHP;
                    }

                    if (user.MP > user.MaxMP)
                    {
                        user.MP = user.MaxMP;
                    }

                    iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "血量", user.HP.ToString());
                    iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "蓝量", user.MP.ToString());

                    // 使用背包物品
                    SetKnapsackItemNum(arr[1], itemNum, 1, groupPath, e.FromQQ.ToString());

                    Common.CqApi.SendGroupMessage(e.FromGroup, arr[1] + " 使用成功");
                    return ;
                }

            }

            return ;

        }

        private RecoveryItem GetRecoveryItem(string ItemName)
        {
            RecoveryItem recoveryItem = new RecoveryItem();

            string itemInfo = "";

            foreach (string name in GameConfig.recoveryItem)
            {
                itemInfo += iniTool.IniReadValue(devPath, itemIni, ItemName, name) + ",";
            }

            recoveryItem.Add(itemInfo);

            return recoveryItem;
        }

    }
}
