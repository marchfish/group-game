﻿using System;
using System.Collections.Generic;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    class MissionManage : BaseManage
    {
        public override void Request(object sender, CqGroupMessageEventArgs e, string groupPath)
        {
            string userName = GetUserName(e.FromQQ.ToString(), e.FromGroup.ToString());

            if (userName == "")
            {
                return;
            }

            User user = GetUser(e.FromQQ.ToString(), e.FromGroup.ToString());

            string[] arr = e.Message.Split(' ');

            if (arr[0] == "当前任务")
            {
                NowMission(user, e, groupPath);
                return;
            }

            if (arr.Length > 1)
            {

                if (arr[0] == "接受任务")
                {
                    SetMission(user, arr[1], e, groupPath);
                    return;
                }

                if (arr[0] == "提交任务") {

                    SubmitMission(user, arr[1], e, groupPath);
                    return;
                
                }

                string mission = iniTool.IniReadValue(devPath, missionIni, arr[1], "内容");

                Common.CqApi.SendGroupMessage(e.FromGroup, System.Text.RegularExpressions.Regex.Unescape(mission));

                return;
            }

            GetMissions(userName, user.Fame, e);

            return;
        }

        // 获取当前可接受的任务
        private void GetMissions(string userName, string missionName, CqGroupMessageEventArgs e) {

            string missions = "[" + userName + "] 您当前可接受的任务如下：" + Environment.NewLine;

            List<string> items = iniTool.IniReadSectionKey(devPath, missionIni, missionName);

            foreach (string item in items)
            {
                missions += item + Environment.NewLine;
            }

            missions += "输入：任务 你想要了解的任务 （即可查看详情）";

            Common.CqApi.SendGroupMessage(e.FromGroup, missions);
        }

        // 接受任务
        private void SetMission(User user, string missionName, CqGroupMessageEventArgs e, string groupPath)
        {
            string myMission = iniTool.IniReadValue(groupPath, missionHistoryIni, e.FromQQ.ToString(), missionName);

            if (myMission != "") {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您已接受了任务“" + missionName + "”");
                return ;
            }

            string mission = iniTool.IniReadValue(devPath, missionIni, user.Fame, missionName);

            if (mission == "") {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您不能接受 “" + missionName + "”" + " 任务");
                return;
            }

            iniTool.IniWriteValue(groupPath, missionHistoryIni, e.FromQQ.ToString(), missionName, "进行中");

            Common.CqApi.SendGroupMessage(e.FromGroup, missionName + "接受成功！");
        }

        // 提交任务
        private void SubmitMission(User user, string missionName, CqGroupMessageEventArgs e, string groupPath)
        {
            string myMission = iniTool.IniReadValue(groupPath, missionHistoryIni, e.FromQQ.ToString(), missionName);

            if (myMission == "")
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您没有接受任务“" + missionName + "”");
                return;
            }

            if (myMission == "已完成")
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "您已完成任务“" + missionName + "”");
                return;
            }

            string item = iniTool.IniReadValue(devPath, missionIni, missionName, "背包");

            string[] items = item.Split('*');

            int myItemNum = GetKnapsackItemNum(items[0], groupPath, e.FromQQ.ToString());

            if (myItemNum < int.Parse(items[1])) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "提交失败：您背包里没有任务需要的物品数量！");
                return;
            }

            if (! SetKnapsackItemNum(items[0], myItemNum, int.Parse(items[1]), groupPath, e.FromQQ.ToString())) {
                Common.CqApi.SendGroupMessage(e.FromGroup, "提交失败：请重试！");
                return;
            };

            string reward = iniTool.IniReadValue(devPath, missionIni, missionName, "奖励");
            int exp = iniTool.ReadInt(devPath, missionIni, missionName, "经验", 0);

            string[] rewards = reward.Split('|');

            foreach (string rew in rewards) {

                string[] temp = rew.Split('*');

                int myNum = GetKnapsackItemNum(temp[0], groupPath, e.FromQQ.ToString());

                iniTool.WriteInt(groupPath, KnapsackIni, e.FromQQ.ToString(), temp[0], myNum + int.Parse(temp[1]));

            }

            user.Exp += exp;

            iniTool.WriteInt(groupPath, userInfoIni, e.FromQQ.ToString(), "经验", user.Exp);

            iniTool.IniWriteValue(groupPath, missionHistoryIni, e.FromQQ.ToString(), missionName, "已完成");

            eventManage.OnIsUplevel(user, groupPath, e);

            string delivery = iniTool.IniReadValue(devPath, missionIni, missionName, "传送");

            if (delivery != "") {
                iniTool.IniWriteValue(groupPath, userInfoIni, e.FromQQ.ToString(), "当前位置", "delivery");
            }

            Common.CqApi.SendGroupMessage(e.FromGroup, "提交成功任务 “" + missionName + "”");
            return;
        
        }

        // 获取已经接受的任务
        private void NowMission(User user, CqGroupMessageEventArgs e, string groupPath)
        {
            string missions = "[" + user.Name + "] 您当前的任务如下：" + Environment.NewLine;

            List<string> items = iniTool.IniReadSectionKey(groupPath, missionHistoryIni, e.FromQQ.ToString());

            if (items.Count == 0) {
                Common.CqApi.SendGroupMessage(e.FromGroup, user.Name + " 您当前并未接受任务！");
                return ;
            }

            foreach (string item in items)
            {
                missions += item + ": " + iniTool.IniReadValue(groupPath, missionHistoryIni, e.FromQQ.ToString(), item) + Environment.NewLine;
            }

            missions += "提交任务指令： 提交任务 您想要提交的任务（即可提交任务）";

            Common.CqApi.SendGroupMessage(e.FromGroup, missions);
        }
    }
}
