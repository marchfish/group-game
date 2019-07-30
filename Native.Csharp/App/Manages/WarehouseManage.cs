
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

            if (arr.Length > 1) {
              
                if (Int32.TryParse(arr[1], out int num))
                {

                    ShowWarehouse(user, e, groupPath, num);

                    return;
                }

            }

            ShowWarehouse(user, e, groupPath);
        }

        private void ShowWarehouse(User user, CqGroupMessageEventArgs e, string groupPath, int page = 1)
        {
            List<string> items = iniTool.IniReadSectionKey(groupPath, warehouseIni, e.FromQQ.ToString());

            int nowPage = (int)Math.Ceiling(Convert.ToDouble(items.Count) / Convert.ToDouble(pageSize));

            page -= 1;

            if (nowPage < page+1 ) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "没有第 " + (page+1).ToString() + "页");
                return ;
            }

            int startPage = page * pageSize;

            if (page == 0) {
                startPage = 0; 
            }


            string res = "[" + user.Name + "] 共" + nowPage + "页 当前页数：" + (page + 1).ToString() + Environment.NewLine;

            for (int i = startPage; i < items.Count; i++ ) {

                if (i - startPage > 9) {
                    break ;
                }
                string itemNum = iniTool.IniReadValue(groupPath, warehouseIni, e.FromQQ.ToString(), items[i]);

                res += items[i] + "：" + itemNum + Environment.NewLine;
            }

            res += "输入：仓库 2";

            Common.CqApi.SendGroupMessage(e.FromGroup, res);
            return;

        }

        private void GetWarehouse(User user, CqGroupMessageEventArgs e, string groupPath)
        {
             
        }

        private void SetWarehouse(User user, CqGroupMessageEventArgs e, string groupPath, string itemName ,int nowItemNum, int useNum = 1)
        {
            int myItem = iniTool.ReadInt(groupPath, warehouseIni, e.FromQQ.ToString(), itemName, 0);

            iniTool.WriteInt(groupPath, warehouseIni, e.FromQQ.ToString(), itemName, useNum + myItem);

            DeleteKnapsackItemNum(itemName, nowItemNum, useNum, groupPath, e.FromQQ.ToString());

            Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] ：" + "成功存入 " + itemName + "*" + useNum.ToString());
        }
    }
}
