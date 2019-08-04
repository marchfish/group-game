using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class EquipManage : BaseManage
    {
        public EquipManage()
        {
            eventManage.RegisterUser += AddUserEquip;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), groupPath);

            if (userName == "")
            {
                return;
            }

            string[] arr = e.Message.Split(' ');

            User user = GetUser(e.FromQQ.ToString(), e, groupPath);

            if (arr[0] == "卸下")
            {
                if (arr.Length > 1)
                {
                    string isEquip = iniTool.IniReadValue(devPath, equipIni, arr[1], "装备方式");

                    if (isEquip == "")
                    {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "该物品不是装备");
                        return;
                    }

                    Equip equipInfo = GetEquip(arr[1]);

                    string userNowEquip = iniTool.IniReadValue(groupPath, equipInfoIni, e.FromQQ.ToString(), equipInfo.Type);

                    if (!userNowEquip.Equals(equipInfo.Name)) {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 您没有装备：" + equipInfo.Name);

                        return;
                    }

                    eventManage.OnUserDownEquip(user, equipInfo, groupPath, e.FromQQ.ToString());

                    SetKnapsackItemNum(equipInfo.Name, 1, groupPath, e.FromQQ.ToString());

                    iniTool.IniWriteValue(groupPath, equipInfoIni, e.FromQQ.ToString(), equipInfo.Type, "无");

                    Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 成功卸下装备：" + equipInfo.Name);

                    return;
                }

                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 卸下 装备名称");
                return;
            }

            if (arr.Length > 1)
            {
                int itemNum = GetKnapsackItemNum(arr[1], groupPath, e.FromQQ.ToString());

                if (itemNum != 0)
                {
                    string isEquip = iniTool.IniReadValue(devPath, equipIni, arr[1], "装备方式");

                    if (isEquip == "") {

                        Common.CqApi.SendGroupMessage(e.FromGroup, "该物品不是装备");

                        return ;
                    }

                    Equip equipInfo = GetEquip(arr[1]);

                    if (equipInfo.Level > user.Level)
                    {
                        Common.CqApi.SendGroupMessage(e.FromGroup, "装备失败：您的等级不足" + equipInfo.Level);

                        return;
                    }

                    string userNowEquip = iniTool.IniReadValue(groupPath, equipInfoIni, e.FromQQ.ToString(), equipInfo.Type);

                    if (userNowEquip != "无")
                    {
                        if (userNowEquip == equipInfo.Name) {
                            itemNum += 1;
                        }

                        Equip nowEquipInfo = GetEquip(userNowEquip);

                        eventManage.OnUserDownEquip(user, nowEquipInfo, groupPath, e.FromQQ.ToString());

                        string knapsackEquipNum =  iniTool.IniReadValue(groupPath, KnapsackIni, e.FromQQ.ToString(), nowEquipInfo.Name);

                        if (knapsackEquipNum == "")
                        {
                            iniTool.WriteInt(groupPath, KnapsackIni, e.FromQQ.ToString(), nowEquipInfo.Name, 1);
                        }
                        else {
                            iniTool.WriteInt(groupPath, KnapsackIni, e.FromQQ.ToString(), nowEquipInfo.Name, 1+int.Parse(knapsackEquipNum));
                        }
                    }


                    eventManage.OnUserUpEquip(user, equipInfo, groupPath, e);

                    iniTool.IniWriteValue(groupPath, equipInfoIni, e.FromQQ.ToString(), equipInfo.Type, arr[1]);

                    // 更新背包物品
                    DeleteKnapsackItemNum(equipInfo.Name, itemNum, 1, groupPath, e.FromQQ.ToString());

                    Common.CqApi.SendGroupMessage(e.FromGroup, "装备成功： " + equipInfo.Name);

                    return ;
                }
                
                Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您没有该物品");
                
                return;
            }

            string equip = "[" + userName + "]" + Environment.NewLine;

            foreach (string userEquip in GameConfig.equipInfo)
            {
                equip += userEquip + "：" + iniTool.IniReadValue(groupPath, equipInfoIni, e.FromQQ.ToString(), userEquip) + Environment.NewLine;
            }

            equip = equip.Substring(0, equip.Length - Environment.NewLine.Length);

            Common.CqApi.SendGroupMessage(e.FromGroup, equip);
            return;

        }

        public void AddUserEquip(string userId, string groupPath)
        {
            for (int i = 0; i < GameConfig.equipInfo.Length; i++)
            {
                iniTool.IniWriteValue(groupPath, equipInfoIni, userId, GameConfig.equipInfo[i], GameConfig.equipDefault[i]);
            }
        }
    }
}
