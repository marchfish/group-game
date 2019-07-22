﻿using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using Tools;

namespace Native.Csharp.App.Manages
{
    abstract class BaseManage
    {
        protected IniTool iniTool = Facade.facade.iniTool;
        protected string devPath = Facade.devPath;
        protected EventManage eventManage = Facade.facade.eventManage;

        // 现有的ini文件
        protected string userInfoIni = "用户信息.ini";
        protected string KnapsackIni = "背包信息.ini";
        protected string fightIni = "战斗信息.ini";
        protected string equipInfoIni = "装备信息.ini";
        protected string equipIni = "装备配置.ini";
        protected string enemyIni = "怪物配置.ini";
        protected string mapIni = "地图配置.ini";
        protected string missionIni = "任务配置.ini";
        protected string missionHistoryIni = "任务信息.ini";
        protected string itemIni = "道具配置.ini";
        protected string levelIni = "等级配置.ini";
        protected string shopIni = "商城配置.ini";
        protected string panksIni = "排行信息.ini";
        protected string reviveIni = "复活配置.ini";
        protected string recycleIni = "回收配置.ini";
        protected string businessIni = "拍卖行信息.ini";

        public abstract void Request(object sender, CqGroupMessageEventArgs e, string groupPath);

        // 判断是否是用户并获取用户名
        protected string GetUserName(string userId, string groupId) {

            string userName = iniTool.IniReadValue(devPath +"\\" + groupId, userInfoIni, userId, "角色名");

            if (userName != "" ) {
                return userName;
            }

            return "";
        }

        // 获取用户所有信息
        protected User GetUser(string userId, string groupId)
        {
            User user = new User();

            string userInfo = "";

            foreach (string name in GameConfig.userInfo) {
                userInfo += iniTool.IniReadValue(devPath + "\\" + groupId, userInfoIni, userId, name) + ",";
            }

            user.Add(userInfo);

            return user;
        }

        // 获取怪物所有信息
        protected Enemy GetEnemy(string enemyName)
        {
            Enemy enemy = new Enemy();

            string enemyInfo = "";

            foreach (string e in GameConfig.enemy)
            {
                enemyInfo += iniTool.IniReadValue(devPath, enemyIni, enemyName, e) + ",";
            }

            enemy.Name = enemyName;

            enemy.Add(enemyInfo);

            return enemy;
        }

        // 获取装备全部信息
        protected Equip GetEquip(string equipName)
        {
            Equip equip = new Equip();

            string equipInfo = "";

            foreach (string e in GameConfig.equip)
            {
                equipInfo += iniTool.IniReadValue(devPath, equipIni, equipName, e) + ",";
            }

            equip.Name = equipName;

            equip.Add(equipInfo);

            return equip;
        }

        // 判断背包中是否有该物品并返回数量 
        protected int GetKnapsackItemNum( string itemName, string groupPath, string userId) {
            return iniTool.ReadInt(groupPath, KnapsackIni, userId, itemName, 0);
        }

        // 使用（减少）新更背包中的物品数量
        protected bool DeleteKnapsackItemNum(string itemName,int nowNum, int useNum, string groupPath, string userId)
        {
            if (useNum > nowNum) {
                return false;
            }

            if (nowNum == useNum)
            {
                iniTool.DeleteSectionKey(groupPath, KnapsackIni, userId, itemName);
            }
            else
            {
                iniTool.WriteInt(groupPath, KnapsackIni, userId, itemName, nowNum - useNum);
            }

            return true;
        }

        // 增加（购买）新更背包中的物品数量
        protected bool SetKnapsackItemNum(string itemName, int setNum, string groupPath, string userId)
        {
            int itemNum = GetKnapsackItemNum(itemName, groupPath, userId);

            iniTool.WriteInt(groupPath, KnapsackIni, userId, itemName, itemNum + setNum);
           
            return true;
        }

        // 获取药品信息
        protected RecoveryItem GetRecoveryItem(string ItemName)
        {
            RecoveryItem recoveryItem = new RecoveryItem();

            string itemInfo = "";

            foreach (string name in GameConfig.recoveryItem)
            {
                itemInfo += iniTool.IniReadValue(devPath, itemIni, ItemName, name) + ",";
            }

            recoveryItem.Add(itemInfo);

            return recoveryItem;
        }

        // 获取等级信息
        protected Level GetLevel(int levelName)
        {
            Level level = new Level();

            string levelInfo = "";

            foreach (string e in GameConfig.level)
            {
                levelInfo += iniTool.IniReadValue(devPath, levelIni, levelName.ToString(), e) + ",";
            }

            level.Name = levelName;

            level.Add(levelInfo);

            return level;
        }

        // 保存战斗信息
        protected void AddFight(User user, Enemy enemy, string userId, string groupId)
        {
            iniTool.IniWriteValue(devPath + "\\" + groupId, fightIni, userId, "角色名", user.Name);
            iniTool.IniWriteValue(devPath + "\\" + groupId, fightIni, userId, "怪物", enemy.Name);
            iniTool.IniWriteValue(devPath + "\\" + groupId, fightIni, userId, "怪物血量", enemy.HP.ToString());
            iniTool.IniWriteValue(devPath + "\\" + groupId, fightIni, userId, "当前位置", user.Pos);
        }

        // 删除结尾换行符
        protected string SubRN(string str) {

            if (!str.Equals("")) {
                str = str.Substring(0, str.Length - Environment.NewLine.Length);
            }

            return str;
        }
    }
}
