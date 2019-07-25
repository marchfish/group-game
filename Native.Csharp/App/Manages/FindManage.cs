
using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;

namespace Native.Csharp.App.Manages
{
    class FindManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string[] arr = e.Message.Split(' ');

            if (arr.Length > 1)
            {
                string propItem = iniTool.IniReadValue(devPath, itemIni, arr[1], "效果");

                if (propItem != "")
                {
                    RecoveryItem recoveryItem = GetRecoveryItem(arr[1]);

                    string propItemInfo = "[" + arr[1] + "]" + Environment.NewLine;

                    foreach (string name in GameConfig.recoveryItem)
                    {
                        propItemInfo += name + "=" + iniTool.IniReadValue(devPath, itemIni, arr[1], name) + Environment.NewLine;
                    }

                    propItemInfo = SubRN(propItemInfo);

                    Common.CqApi.SendGroupMessage(e.FromGroup, propItemInfo);

                    return;
                }

                string equipItem = iniTool.IniReadValue(devPath, equipIni, arr[1], "装备方式");

                if (equipItem != "")
                {
                    Equip equip = GetEquip(arr[1]);

                    string equipInfo = "[" + arr[1] + "]" + Environment.NewLine;

                    foreach (string eq in GameConfig.equip)
                    {
                        equipInfo += eq +"=" + iniTool.IniReadValue(devPath, equipIni, arr[1], eq) + Environment.NewLine;
                    }

                    equipInfo = SubRN(equipInfo);

                    Common.CqApi.SendGroupMessage(e.FromGroup, equipInfo);

                    return;
                }

                Common.CqApi.SendGroupMessage(e.FromGroup, "不能查看：" + arr[1]);

                return;
            }
        }
    }
}
