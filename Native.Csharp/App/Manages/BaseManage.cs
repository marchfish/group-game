using Native.Csharp.App.Configs;
using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;
using System;
using System.Collections.Generic;
using Tools;

namespace Native.Csharp.App.Manages
{
    abstract class BaseManage
    {
        protected IniTool iniTool = Facade.facade.iniTool;
        protected string devPath = Facade.devPath;
        protected EventManage eventManage = Facade.facade.eventManage;
        protected int pageSize = 10;

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
        protected string vipIni = "会员配置.ini";
        protected string vipInfoIni = "会员信息.ini";
        protected string warehouseIni = "仓库信息.ini";

        public abstract void Request(object sender, CqGroupMessageEventArgs e, string groupPath);

        // 判断是否是用户并获取用户名
        protected string GetUserName(string userId, string groupPath) {

            string userName = iniTool.IniReadValue(groupPath, userInfoIni, userId, "角色名");

            if (userName != "" ) {
                return userName;
            }

            return "";
        }

        // 获取用户所有信息
        protected User GetUser(string userId, CqGroupMessageEventArgs e, string groupPath)
        {
            User user = new User();

            string userInfo = "";

            foreach (string name in GameConfig.userInfo) {
                userInfo += iniTool.IniReadValue(groupPath, userInfoIni, userId, name) + ",";
            }

            user.Add(userInfo);

            user.isShowMessage = true;

            // 获取vip信息
            Vip vip = GetVipInfo(e, groupPath);

            if (vip.Level == 0)
            {
                vip = GetVipInfo(e, groupPath, true);
                SetVipInfo(vip, e, groupPath);
            }

            DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            DateTime endTime = Convert.ToDateTime(vip.endTime);

            int compNum = DateTime.Compare(nowTime, endTime);

            if (compNum < 0) {
                user.isVip = true;
            }

            if (vip.OnHookTime != "") {
                user.isOnHook = true;
            }

            user.Protect = vip.Protect;

            user.SuccessRate = vip.SuccessRate;


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

        // 获取Vip信息
        protected Vip GetVipInfo(CqGroupMessageEventArgs e, string groupPath, bool isInit = false)
        {
            Vip vip = new Vip();

            string vipInfo = "";

            if (isInit) {

                foreach (string v in GameConfig.vipInfo)
                {
                    vipInfo += iniTool.IniReadValue(devPath, vipIni, "初始值", v) + ",";
                }

                vip.Add(vipInfo);

                DateTime nowTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                DateTime dateTime = nowTime.AddMonths(int.Parse(vip.endTime));

                vip.endTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

                return vip; 
            }

            string time = iniTool.IniReadValue(groupPath, vipInfoIni, e.FromQQ.ToString(), "到期时间");

            if (time == "") {
                return vip;
            }

            foreach (string v in GameConfig.vipInfo)
            {
                vipInfo += iniTool.IniReadValue(groupPath, vipInfoIni, e.FromQQ.ToString(), v) + ",";
            }

            vip.Add(vipInfo);

            return vip;
        }

        // 设置Vip信息
        protected void SetVipInfo(Vip vip, CqGroupMessageEventArgs e, string groupPath)
        {
            string userId = e.FromQQ.ToString();

            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "等级", vip.Level.ToString());
            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "到期时间", vip.endTime);
            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "保护", vip.Protect.ToString());
            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "成功率", vip.SuccessRate.ToString());
            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "挂机时间", vip.OnHookTime);
            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "挂机类型", vip.OnHookType);
            iniTool.IniWriteValue(groupPath, vipInfoIni, userId, "次数", vip.Number.ToString());
        }

        // 保存战斗信息
        protected void AddFight(User user, Enemy enemy, string userId, string groupPath)
        {
            iniTool.IniWriteValue(groupPath, fightIni, userId, "角色名", user.Name);
            iniTool.IniWriteValue(groupPath, fightIni, userId, "怪物", enemy.Name);
            iniTool.IniWriteValue(groupPath, fightIni, userId, "怪物血量", enemy.HP.ToString());
            iniTool.IniWriteValue(groupPath, fightIni, userId, "当前位置", user.Pos);
        }

        // 显示分页结果
        protected void ShowPage(User user, CqGroupMessageEventArgs e, string groupPath, string iniName, int page = 1)
        {
            List<string> items = iniTool.IniReadSectionKey(groupPath, iniName, e.FromQQ.ToString());

            int nowPage = (int)Math.Ceiling(Convert.ToDouble(items.Count) / Convert.ToDouble(pageSize));

            page -= 1;

            if (nowPage < page + 1)
            {
                Common.CqApi.SendGroupMessage(e.FromGroup, "[" + user.Name + "的" + iniName.Substring(0, 2) + "] ：" + "第" + (page + 1).ToString() + "页 没有任何物品");
                return;
            }

            int startPage = page * pageSize;

            string res = "[" + user.Name + "的" + iniName.Substring(0, 2) + "] 共" + nowPage + "页 当前页数：" + (page + 1).ToString() + Environment.NewLine;

            for (int i = startPage; i < items.Count; i++)
            {

                if (i - startPage > 9)
                {
                    break;
                }
                string itemNum = iniTool.IniReadValue(groupPath, iniName, e.FromQQ.ToString(), items[i]);

                res += items[i] + "：" + itemNum + Environment.NewLine;
            }

            Common.CqApi.SendGroupMessage(e.FromGroup, SubRN(res));
            return;

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
