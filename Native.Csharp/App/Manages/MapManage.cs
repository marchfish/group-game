using Native.Csharp.App.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class MapManage : BaseManage
    {
        private string iniName = "地图配置.ini";

        public override void Request(object sender, CqGroupMessageEventArgs e)
        {

            string groupPath = devPath + "\\" + e.FromGroup;

            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName != "")
            {
                string position = iniTool.IniReadValue(groupPath, "用户信息.ini", e.FromQQ.ToString(), "当前位置");

                if (e.Message == "上" || e.Message == "下" || e.Message == "左" || e.Message == "右")
                {
                    string nextPos = iniTool.IniReadValue(devPath, iniName, position, e.Message);
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
        }

        public void GetMap(string position, CqGroupMessageEventArgs e) {

            string map = "";

            List<string> items = iniTool.IniReadSectionKey(devPath, iniName, position);

            map += "[" + position + "]" + Environment.NewLine;

            foreach (string item in items)
            {
                string res = iniTool.IniReadValue(devPath, iniName, position, item);

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
