namespace Native.Csharp.App.Configs
{
    public class GameConfig
    {
        // 角色信息
        public static string[] userInfo = { "角色名", "血量", "蓝量", "攻击力", "魔法力", "暴击率", "闪避率", "防御力", "等级", "经验", "称号", "当前位置" };
        public static string[] userInfoDefault = { "", "100", "100", "10", "10", "0", "0", "5", "1", "0", "新手", "新手村" };

        // 装备信息
        public static string[] equip = { "武器", "头盔", "衣服", "耳环", "手镯", "戒指",  "鞋子", "法宝"};
        public static string[] equipDefault = { "无", "无", "无", "无", "无", "无", "无", "无" };

        // 战斗配置
        public static string[] fight = { "角色名", "角色血量", "角色蓝量", "怪物", "怪物血量", "当前位置"};


    }
}
