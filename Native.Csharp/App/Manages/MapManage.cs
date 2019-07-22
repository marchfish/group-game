using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class MapManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (user.HP <= 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，您已死亡：请复活后继续!");
                return;
            }

            string position = iniTool.IniReadValue(groupPath, "用户信息.ini", e.FromQQ.ToString(), "当前位置");

            if (e.Message == "上" || e.Message == "下" || e.Message == "左" || e.Message == "右" || e.Message == "前" || e.Message == "后")
            {
                string nextPos = iniTool.IniReadValue(devPath, mapIni, position, e.Message);
                if (nextPos != "")
                {
                    iniTool.IniWriteValue(groupPath, "用户信息.ini", e.FromQQ.ToString(), "当前位置", nextPos);

                    GetMap(nextPos, e);
                }
                else {
                    Common.CqApi.SendGroupMessage(e.FromGroup, "对不起，前方无路可走。");
                }
                return;
            }

            GetMap(position, e);

            return;
        }

        public void GetMap(string position, CqGroupMessageEventArgs e) {

            string map = "";

            List<string> items = iniTool.IniReadSectionKey(devPath, mapIni, position);

            map += "[" + position + "]" + Environment.NewLine;

            foreach (string item in items)
            {
                string res = iniTool.IniReadValue(devPath, mapIni, position, item);

                if (res != "")
                {
                    map += item + "：" + res + Environment.NewLine;
                }

            }

            map = SubRN(map);

            Common.CqApi.SendGroupMessage(e.FromGroup, map);
        }
    }
}
