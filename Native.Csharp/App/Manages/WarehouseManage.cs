
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;

namespace Native.Csharp.App.Manages
{
    class WarehouseManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = iniTool.IniReadValue(groupPath, userInfoIni, e.FromQQ.ToString(), "角色名");

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            string[] arr = e.Message.Split(' ');

            if (arr[0] == "存") {

                if (!user.isVip)
                {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "您不是会员！请先购买会员");
                    return;
                }

                if (arr.Length > 1)
                {
                    int myItemNum = GetKnapsackItemNum(arr[1], groupPath, e.FromQQ.ToString());

                    if (myItemNum == 0) {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "存入失败：您没有物品 " + arr[1]);
                        return;
                    }

                    if (arr.Length > 2) {
                        if (Int32.TryParse(arr[2], out int num))
                        {
                            if (myItemNum < num) {
                                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "存入失败：您没有足够的数量 " + arr[1]);
                                return;
                            }

                            SetWarehouse(user, e, groupPath, arr[1], myItemNum, num);

                            return;
                        }
                    }

                    SetWarehouse(user, e, groupPath, arr[1], myItemNum, myItemNum);
                    return;
                }
            }

            if (arr[0] == "取")
            {
                if (arr.Length > 1)
                {
                    int wItemNum = GetWarehouseItemNum(arr[1], groupPath, e.FromQQ.ToString());

                    if (wItemNum == 0)
                    {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "取出失败：您仓库中没有物品 " + arr[1]);
                        return;
                    }

                    int useNum = wItemNum;

                    if (arr.Length > 2)
                    {
                        if (Int32.TryParse(arr[2], out int num))
                        {
                            if (wItemNum < num)
                            {
                                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "取出失败：您仓库中没有足够的数量 " + arr[1]);
                                return;
                            }
                            useNum = num;
                        }
                    }

                    SetKnapsackItemNum(arr[1], useNum, groupPath, e.FromQQ.ToString());

                    DeleteWarehouseItemNum(arr[1], wItemNum, useNum, groupPath, e.FromQQ.ToString());

                    Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "取出成功：" + arr[1] + "*" + useNum.ToString());

                    return;
                }
            }

            if (arr.Length > 1) {
              
                if (Int32.TryParse(arr[1], out int num))
                {

                    ShowPage(user, e, groupPath, warehouseIni, num);

                    return;
                }

            }

            ShowPage(user, e, groupPath, warehouseIni);
        }

        private void GetWarehouse(User user, CqGroupMessageEventArgs e, string groupPath, string itemName, int useNum)
        {
            
        }

        // 存储物品
        private void SetWarehouse(User user, CqGroupMessageEventArgs e, string groupPath, string itemName ,int nowItemNum, int useNum = 1)
        {
            int myItem = iniTool.ReadInt(groupPath, warehouseIni, e.FromQQ.ToString(), itemName, 0);

            iniTool.WriteInt(groupPath, warehouseIni, e.FromQQ.ToString(), itemName, useNum + myItem);

            DeleteKnapsackItemNum(itemName, nowItemNum, useNum, groupPath, e.FromQQ.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "成功存入 " + itemName + "*" + useNum.ToString());
        }

        // 判断是否有该物品并返回数量 
        private int GetWarehouseItemNum(string itemName, string groupPath, string userId)
        {
            return iniTool.ReadInt(groupPath, warehouseIni, userId, itemName, 0);
        }

        // 使用（减少）新更物品数量
        private bool DeleteWarehouseItemNum(string itemName, int nowNum, int useNum, string groupPath, string userId)
        {
            if (useNum > nowNum)
            {
                return false;
            }

            if (nowNum == useNum)
            {
                iniTool.DeleteSectionKey(groupPath, warehouseIni, userId, itemName);
            }
            else
            {
                iniTool.WriteInt(groupPath, warehouseIni, userId, itemName, nowNum - useNum);
            }

            return true;
        }
    }
}
