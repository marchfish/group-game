namespace Native.Csharp.App.Configs
{
    public class GameConfig
    {
        // 角色信息
        public static string[] userInfo = {"角色名", "血量", "蓝量", "攻击力", "魔法力", "暴击率", "闪避率", "防御力", "等级", "经验", "称号", "当前位置", "最大血量", "最大蓝量"};
        public static string[] userInfoDefault = {"", "100", "100", "10", "10", "0", "0", "5", "1", "0", "新手", "新手村", "100", "100" };

        // 装备
        public static string[] equipInfo = {"武器", "头盔", "衣服", "耳环", "手镯", "戒指",  "鞋子", "法宝"};
        public static string[] equipDefault = { "无", "无", "无", "无", "无", "无", "无", "无" };

        // 战斗配置
        public static string[] fight = {"角色名", "怪物", "怪物血量", "当前位置"};

        // 怪物
        public static string[] enemy = {"等级", "描述", "攻击力", "血量", "防御力", "属性", "金币", "掉落物品", "掉落机率", "经验", "必爆", "类型"};

        // 装备信息
        public static string[] equip = {"等级", "攻击", "血量", "蓝量", "魔力", "闪避", "暴击", "防御", "品质", "装备方式", "描述"};

        // 道具物品
        public static string[] recoveryItem = { "血量", "蓝量", "描述", "效果"};
    }
}
