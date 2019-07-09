using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class EquipManage : BaseManage
    {
        private string iniName = "装备信息.ini";

        public EquipManage()
        {
            action = "装备";
            AddManage();
            eventManage.registerUser += AddUserEquip;
        }

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {
            string groupPath = devPath + "\\" + e.FromGroup;

            if (isUser(e.FromQQ.ToString(), e.FromGroup.ToString()))
            {
                string[] arr = e.Message.Split(' ');

                if (arr.Length > 1)
                {
                    iniTool.IniWriteValue(groupPath, iniName, e.FromQQ.ToString(), "武器", arr[1]);
                    Common.CqApi.SendGroupMessage(e.FromGroup, "装备成功");
                    return;
                }


                string equip = "";

                foreach (string userEquip in GameConfig.equip)
                {
                    equip += userEquip + "：" + iniTool.IniReadValue(groupPath, iniName, e.FromQQ.ToString(), userEquip) + Environment.NewLine;
                }

                equip = equip.Substring(0, equip.Length - Environment.NewLine.Length);

                Common.CqApi.SendGroupMessage(e.FromGroup, equip);
                return;
            }
        }

        //private void Equip()
        //{

        //}

        public void AddUserEquip(string userId, string groupId)
        {
            string groupPath = devPath + "\\" + groupId;

            for (int i = 0; i < GameConfig.equip.Length; i++)
            {
                iniTool.IniWriteValue(groupPath, iniName, userId, GameConfig.equip[i], GameConfig.equipDefault[i]);
            }
        }
    }
}
