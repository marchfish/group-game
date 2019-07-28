
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;

namespace Native.Csharp.App.Manages
{
    class BusinessManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName == "")
            {
                return;
            }

            string[] arr = e.Message.Split(' ');

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString(), e);

            string startTime = iniTool.IniReadValue(groupPath, businessIni, "时间", "内容"); 

            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            if (arr[0] == "出售")
            {

                if (startTime == "")
                {
                    iniTool.IniWriteValue(groupPath, businessIni, "时间", "内容", DateTime.Now.ToString("yyyy-MM-dd"));
                }

                if (arr.Length > 2)
                {
                    if (arr[1] == "金币") {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "金币无法出售!");
                        return;
                    }

                    if (Int32.TryParse(arr[2], out int num))
                    {
                        if (num > 90000000)
                        {
                            Common.CqApi.SendGroupMessage(e.FromGroup, "您出售的价格过高!");
                            return;
                        }

                        if (num < 10)
                        {
                            Common.CqApi.SendGroupMessage(e.FromGroup, "出售价格不能小于10!");
                            return;
                        }

                        int myItem = GetKnapsackItemNum(arr[1], groupPath, e.FromQQ.ToString());

                        if (myItem == 0)
                        {
                            Common.CqApi.SendGroupMessage(e.FromGroup, "您没有该物品：" + arr[1]);
                            return;
                        }

                        DeleteKnapsackItemNum(arr[1], myItem, 1, groupPath, e.FromQQ.ToString());

                        Sell(arr[1], arr[2], groupPath, user, e);

                        Common.CqApi.SendGroupMessage(e.FromGroup, "物品成功上架：" + arr[1]);

                        return;
                    }

                    Common.CqApi.SendGroupMessage(e.FromGroup, "价格为数字");
                    return;
                }

                Common.CqApi.SendGroupMessage(e.FromGroup, "出售 物品名称 价格");
                return;
            }

            if (arr[0] == "购买商品")
            {

                if (arr.Length > 1)
                {
                    if (Int32.TryParse(arr[1], out int num))
                    {
                        Pay(arr[1], groupPath, user, e); 
                    }

                    return;
                }
                return;
            }

            if (arr[0] == "下架")
            {
                if (arr.Length > 1)
                {
                    if (Int32.TryParse(arr[1], out int num))
                    {
                        UnItem(groupPath, arr[1], e);
                    }

                    return;
                }
            }

            if (startTime == "") {

                iniTool.IniWriteValue(groupPath, businessIni, "时间", "内容", DateTime.Now.ToString("yyyy-MM-dd"));

                Common.CqApi.SendGroupMessage(e.FromGroup, "暂无物品上架");

                return ;

            }

            DateTime startTime1 = Convert.ToDateTime(startTime);

            TimeSpan timeSpan = nowTime.Subtract(startTime1);

            int day = timeSpan.Days + 1;

            if (day > 15) {

                iniTool.IniWriteValue(groupPath, businessIni, "时间", "内容", DateTime.Now.ToString("yyyy-MM-dd"));

                DeleteItemAll(groupPath);

                Common.CqApi.SendGroupMessage(e.FromGroup, "拍卖行更新，暂无物品上架");

                return;
            }

            List<string> items = iniTool.IniReadSectionKey(groupPath, businessIni, "商品");

            if (items.Count == 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "暂无物品上架");
                return;
            }

            string res = "[拍卖行商品]" + Environment.NewLine;

            foreach (string item in items)
            {
                BusinessItem businessItem = GetBusinessItem(groupPath, item);

                res += item + "、" + businessItem.ItemName + "：" + businessItem.Coin + "金币" + Environment.NewLine;

                res += "--出售者：" + businessItem.UserName + Environment.NewLine;

            }

            res += "输入：购买商品 商品编号";

            Common.CqApi.SendGroupMessage(e.FromGroup, res);

            return;
        }

        private void Pay(string itemNo, string groupPath, User user, CqGroupMessageEventArgs e)
        {
            BusinessItem businessItem = GetBusinessItem(groupPath, itemNo);

            if (businessItem.Coin == 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：没有编号为" + itemNo + "的商品!");
                return;
            }

            if (businessItem.UserId == e.FromQQ.ToString())
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：不能购买自己上架的物品!");
                return;
            }

            int myCoin = GetKnapsackItemNum("金币", groupPath, e.FromQQ.ToString());

            int useCoin = businessItem.Coin;

            if (myCoin < useCoin)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "购买失败：您没有足够的金币!");
                return;
            }

            SetKnapsackItemNum(businessItem.ItemName, 1, groupPath, e.FromQQ.ToString());

            DeleteKnapsackItemNum("金币", myCoin, useCoin, groupPath, e.FromQQ.ToString());

            DeleteItem(businessItem, groupPath);

            Common.CqApi.SendGroupMessage(e.FromGroup, "购买成功：" + businessItem.ItemName + ", -" + useCoin + "金币");

            UpdateItem(groupPath);

            return;
        }


        private void Sell(string itemName, string coin, string groupPath, User user, CqGroupMessageEventArgs e)
        {
            Random random = new Random();

            int rNum = random.Next(1000, 10000);

            string val = itemName + "|" + coin + "|" + user.Name + "|" + e.FromQQ.ToString();

            iniTool.IniWriteValue(groupPath, businessIni, "商品", rNum.ToString(), val);

            UpdateItem(groupPath);

            return;
        }

        private void UpdateItem(string groupPath) {
            List<string> items = iniTool.IniReadSectionKey(groupPath, businessIni, "商品");

            List<BusinessItem> businessItemlist = new List<BusinessItem>();

            int count = 1;

            foreach (string item in items)
            {
                BusinessItem businessItem = GetBusinessItem(groupPath, item);

                businessItem.No = count.ToString();

                businessItemlist.Add(businessItem);

                count++;
            }

            iniTool.DeleteSection(groupPath, businessIni, "商品");

            foreach (BusinessItem item in businessItemlist)
            {
                string val = item.ItemName + "|" + item.Coin + "|" + item.UserName + "|" + item.UserId;

                iniTool.IniWriteValue(groupPath, businessIni, "商品", item.No, val);
            }
        }

        private void DeleteItem(BusinessItem businessItem, string groupPath)
        {
            int coin = (int) Math.Round(businessItem.Coin - businessItem.Coin * 0.1);

            SetKnapsackItemNum("金币", coin, groupPath, businessItem.UserId);

            iniTool.DeleteSectionKey(groupPath, businessIni, "商品", businessItem.No);

            return;
        }

        private void DeleteItemAll(string groupPath)
        {
            List<string> items = iniTool.IniReadSectionKey(groupPath, businessIni, "商品");

            List<BusinessItem> businessItemlist = new List<BusinessItem>();

            int count = 1;

            foreach (string item in items)
            {
                BusinessItem businessItem = GetBusinessItem(groupPath, item);

                businessItem.No = count.ToString();

                businessItemlist.Add(businessItem);

                count++;
            }

            foreach (BusinessItem item in businessItemlist)
            {
                SetKnapsackItemNum(item.ItemName, 1, groupPath, item.UserId);
            }

            iniTool.DeleteSection(groupPath, businessIni, "商品");

            return;
        }

        private BusinessItem GetBusinessItem(string groupPath, string itemNo)
        {
            BusinessItem businessItem = new BusinessItem();

            string info = iniTool.IniReadValue(groupPath, businessIni, "商品", itemNo);

            if (info == "")
            {
                return businessItem;
            }

            businessItem.Add(info);

            businessItem.No = itemNo;

            return businessItem;
        }

        // 下架物品
        private void UnItem(string groupPath, string itemNo, CqGroupMessageEventArgs e)
        {
            BusinessItem businessItem = GetBusinessItem(groupPath, itemNo);

            if (businessItem.ItemName == "") {

                Common.CqApi.SendGroupMessage(e.FromGroup, "没有改编号的物品");

                return;
            }

            if (e.FromQQ.ToString() != businessItem.UserId)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "该物品不属于您");

                return;
            }

            SetKnapsackItemNum(businessItem.ItemName, 1, groupPath, e.FromQQ.ToString());

            iniTool.DeleteSectionKey(groupPath, businessIni, "商品", businessItem.No);

            UpdateItem(groupPath);

            Common.CqApi.SendGroupMessage(e.FromGroup, "成功下架商品：" + businessItem.ItemName);

            return;

        }

    }
}
