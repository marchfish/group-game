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
                    User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

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

            List<string> items = iniTool.IniReadSection(devPath, shopIni);

            string shopItems = "[物品商城]" + Environment.NewLine;

            foreach (string item in items)
            {
                string name = iniTool.IniReadValue(devPath, shopIni, item, "名称");

                string coin = iniTool.IniReadValue(devPath, shopIni, item, "金币");

                shopItems += item + "：" + name + "--" + coin + "金币" + Environment.NewLine;
            }

            shopItems += "输入：购买 物品编号";

            Common.CqApi.SendGroupMessage(e.FromGroup, shopItems);

            return;
        }

        private void Pay(string itemNo, int itemNum, string groupPath, User user, CqGroupMessageEventArgs e) {

            string name = iniTool.IniReadValue(devPath, shopIni, itemNo, "名称");

            int coin = iniTool.ReadInt(devPath, shopIni, itemNo, "金币", 1);

            int myCoin = GetKnapsackItemNum("金币", groupPath, e.FromQQ.ToString());

            if (myCoin < itemNum*coin)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：您没有足够的金币!");
                return;
            }

            SetKnapsackItemNum(name, itemNum, groupPath, e.FromQQ.ToString());

            iniTool.WriteInt(groupPath, KnapsackIni, e.FromQQ.ToString(), "金币", myCoin - (itemNum * coin));

            Common.CqApi.SendGroupMessage(e.FromGroup, "购买成功：" + name + "*" + itemNum + ", -" + (itemNum * coin) + "金币");

            return;
        }
    }
}
