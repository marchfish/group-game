
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class RecycleManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), groupPath);

            if (userName == "")
            {
                return;
            }

            string[] arr = e.Message.Split(' ');

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            if (arr.Length > 2)
            {
                if (Int32.TryParse(arr[2], out int num))
                {
                    Recycle(user, arr[1], num, groupPath, e);
                }
                else {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "请输入正确的数量");
                }

                return ; 
            }


            if (arr.Length > 1)
            {
                Recycle(user, arr[1], 0, groupPath, e);

                return;
            }

            Common.CqApi.SendGroupMessage(e.FromGroup, "请输入要回收的物品");
        }

        private void Recycle(User user, string itemName, int itemNum, string groupPath, CqGroupMessageEventArgs e)
        {
            int myItemNum = GetKnapsackItemNum(itemName, groupPath, e.FromQQ.ToString());

            if (myItemNum == 0) {

                Common.CqApi.SendGroupMessage(e.FromGroup, "您没有该物品：" + itemName);

                return;
            }

            int coin = iniTool.ReadInt(devPath, recycleIni, itemName, "金币", 0);

            if (coin == 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "该物品不能回收：" + itemName);

                return;
            }

            if (itemNum == 0) {

                SetKnapsackItemNum("金币", coin * myItemNum, groupPath, e.FromQQ.ToString());
                DeleteKnapsackItemNum(itemName, myItemNum, myItemNum, groupPath, e.FromQQ.ToString());

                Common.CqApi.SendGroupMessage(e.FromGroup, "回收成功 +" + coin * myItemNum + "金币");

                return;

            }

            if (myItemNum < itemNum)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您背包中没有这么多：" + itemName);

                return;
            }

            SetKnapsackItemNum("金币", coin * itemNum, groupPath, e.FromQQ.ToString());

            DeleteKnapsackItemNum(itemName, myItemNum, itemNum, groupPath, e.FromQQ.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, "回收成功 +" + coin * itemNum + "金币");

            return;
        }
    }
}
