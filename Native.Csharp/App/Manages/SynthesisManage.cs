
using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Native.Csharp.App.Manages
{
    class SynthesisManage : BaseManage
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

            if (arr.Length > 1)
            {
                Synthesis(arr[1], user, e, groupPath);
                return;
            }

            ShowSynthesis(user, e, groupPath);
            return;
        }

        // 显示合成
        private void ShowSynthesis(User user, CqGroupMessageEventArgs e, string groupPath) {

            List<string> items = iniTool.IniReadSectionKey(devPath, synthesisIni, user.Pos);

            if (items.Count == 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 该位置没有商品");
                return;
            }

            string synthesisItems = "[" + user.Pos + "]" + Environment.NewLine;

            foreach (string item in items)
            {
                string name = iniTool.IniReadValue(devPath, synthesisIni, user.Pos, item);

                Synthesis synthesis = GetSynthesis(name, e, groupPath);

                synthesisItems += item + "：" + name + " 所需材料（" + synthesis.Material.Replace("|", ",") + "）" + Environment.NewLine;
                synthesisItems += "成功率：" + synthesis.SuccessRate + Environment.NewLine + "失败保留：" + synthesis.Retain + Environment.NewLine;
            }

            synthesisItems += "输入：合成 编号";

            Common.CqApi.SendGroupMessage(e.FromGroup, synthesisItems);

            return;
        }

        // 合成
        private void Synthesis(string synthesisName, User user, CqGroupMessageEventArgs e, string groupPath)
        {
            synthesisName = iniTool.IniReadValue(devPath, synthesisIni, user.Pos, synthesisName);

            Synthesis synthesis = GetSynthesis(synthesisName, e, groupPath);

            if (synthesis.SuccessRate == 0)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 该物品不能合成 " + synthesis.Name);
                return;
            }

            string[] allItems = synthesis.Material.Split('|');

            bool isExistence = IsExistenceItems(user, e, groupPath, allItems);

            if (!isExistence) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 合成失败：您背包里没有合成需要的物品数量！");
                return;
            }
       
            bool isDeleteItems = DeleteKnapsackItems(user, e, groupPath, allItems);

            if (!isDeleteItems)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 请稍后再试！");
                return;
            }

            if (user.isVip) {
                synthesis.SuccessRate += 20;
            }

            bool isSuccess = IsSuccess(synthesis.SuccessRate);

            if (!isSuccess) {
                string[] retain = synthesis.Retain.Split('*');

                SetKnapsackItemNum(retain[0], 1, groupPath, e.FromQQ.ToString());

                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 合成失败：长相问题!");
                return;
            }

            SetKnapsackItemNum(synthesis.Name, 1, groupPath, e.FromQQ.ToString());
        
            Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "] 合成成功：" + synthesis.Name);
        }

        // 获取Vip信息
        private Synthesis GetSynthesis(string synthesisName, CqGroupMessageEventArgs e, string groupPath)
        {
            Synthesis synthesis = new Synthesis();

            string isItem = iniTool.IniReadValue(devPath, synthesisIni, synthesisName, "合成材料");

            if (isItem == "") {

                return synthesis;
            }

            string synthesisInfo = "";

            foreach (string s in GameConfig.synthesis)
            {
                synthesisInfo += iniTool.IniReadValue(devPath, synthesisIni, synthesisName, s) + ",";

            }

            synthesis.Name = synthesisName;

            synthesis.Add(synthesisInfo);

            return synthesis;
        }
    }
}
