using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;

namespace Native.Csharp.App.Manages
{
    class ShopManage : BaseManage
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

            if (arr[0] == "商店")
            {
                Show(user.Pos, e);
                return;
            }

            if (arr.Length > 1)
            {
                if (Int32.TryParse(arr[1], out int num))
                {
                    if (arr[0] == "购买")
                    {
                        if (arr.Length > 2 && Int32.TryParse(arr[2], out int num1)) {

                            Pay(arr[1], int.Parse(arr[2]), groupPath, user, e);
                            return;
                        }

                        Pay(arr[1], 1, groupPath, user, e);
                    }

                }

                return;
            }

            Show("商城", e);

            return;
        }

        private void Show(string shopName, CqGroupMessageEventArgs e) {

            List<string> items = iniTool.IniReadSectionKey(devPath, shopIni, shopName);

            if (items.Count == 0) {

                Common.CqApi.SendGroupMessage(e.FromGroup, "该位置没有商品");

                return;
            }

            string shopItems = "[" + shopName + "]" + Environment.NewLine;

            foreach (string item in items)
            {
                string name = iniTool.IniReadValue(devPath, shopIni, shopName, item);

                string[] arrItem = name.Split('*');

                shopItems += item + "：" + arrItem[0] + "--" + arrItem[1] + "金币" + Environment.NewLine;
            }

            shopItems += "输入：购买 物品编号";

            Common.CqApi.SendGroupMessage(e.FromGroup, shopItems);

            return;
        }

        private void Pay(string itemNo, int itemNum, string groupPath, User user, CqGroupMessageEventArgs e) {

            string shopName = "商城";

            string isShop = iniTool.IniReadValue(devPath, shopIni, user.Pos, "1");

            if (isShop != "") {
                shopName = user.Pos;
            }

            string name = iniTool.IniReadValue(devPath, shopIni, shopName, itemNo);

            if (name == "") {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：没有编号为"+ itemNo +"的物品!");
                return;
            }

            string itemInfo = iniTool.IniReadValue(devPath, shopIni, shopName, itemNo);

            string[] item = itemInfo.Split('*');

            int myCoin = GetKnapsackItemNum("金币", groupPath, e.FromQQ.ToString());

            if (myCoin < itemNum*int.Parse(item[1]))
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：您没有足够的金币!");
                return;
            }

            SetKnapsackItemNum(item[0], itemNum, groupPath, e.FromQQ.ToString());

            DeleteKnapsackItemNum("金币", myCoin, itemNum * int.Parse(item[1]), groupPath, e.FromQQ.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, "购买成功：" + item[0] + "*" + itemNum + ", -" + (itemNum * int.Parse(item[1])) + "金币");

            return;
        }
    }
}
