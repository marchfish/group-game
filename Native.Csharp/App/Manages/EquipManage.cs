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
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName == "")
            {
                return;
            }

            string[] arr = e.Message.Split(' ');

            if (arr.Length > 1)
            {
                if (arr[1] == "金币") {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "金币无法装备");
                    return;
                }

                string item = iniTool.IniReadValue(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[1]);

                if (item != "")
                {
                    Equip equipInfo = GetEquipInfo(arr[1]);

                    string userNowEquip = iniTool.IniReadValue(groupPath, equipIni, e.FromQQ.ToString(), equipInfo.Type);

                    User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

                    if (userNowEquip != "无")
                    {
                        Equip nowEquipInfo = GetEquipInfo(userNowEquip);

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

                    eventManage.OnUserUpEquip(user, equipInfo, groupPath, e.FromQQ.ToString());

                    iniTool.IniWriteValue(groupPath, equipIni, e.FromQQ.ToString(), equipInfo.Type, arr[1]);

                    if (int.Parse(item) == 1)
                    {
                        iniTool.DeleteSectionKey(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[1]);
                    }
                    else {
                        iniTool.WriteInt(groupPath, KnapsackIni, e.FromQQ.ToString(), arr[1], int.Parse(item) - 1);
                    }

                    Common.CqApi.SendGroupMessage(e.FromGroup, "装备成功： " + equipInfo.Name);
                }
                else {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您没有该物品");
                }

                return;
            }

            string equip = "[" + userName + "]" + Environment.NewLine;

            foreach (string userEquip in GameConfig.equip)
            {
                equip += userEquip + "：" + iniTool.IniReadValue(groupPath, equipIni, e.FromQQ.ToString(), userEquip) + Environment.NewLine;
            }

            equip = equip.Substring(0, equip.Length - Environment.NewLine.Length);

            Common.CqApi.SendGroupMessage(e.FromGroup, equip);
            return;

        }

        public void AddUserEquip(string userId, string groupId)
        {
            string groupPath = devPath + "\\" + groupId;

            for (int i = 0; i < GameConfig.equip.Length; i++)
            {
                iniTool.IniWriteValue(groupPath, equipIni, userId, GameConfig.equip[i], GameConfig.equipDefault[i]);
            }
        }
    }
}
