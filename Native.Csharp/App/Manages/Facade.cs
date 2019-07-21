using System;
using System.Collections.Generic;
using Tools;

namespace Native.Csharp.App.Manages
{
    class Facade
    {
        public static Facade facade;

        // 应用路径
        public static string devPath = System.Windows.Forms.Application.StartupPath + "\\dev\\com.guangyingart.demo";

        public readonly Dictionary<string, BaseManage> managesDit = new Dictionary<string, BaseManage>();

        // 工具
        public IniTool iniTool = new IniTool();
        public MidirTool midirTool = new MidirTool();

        // 委托管理
        public EventManage eventManage = new EventManage(); 

        // 初始化类
        public Facade()
        {
            facade = this;
            
            managesDit.Add("注册用户", new RegisterManage());
            managesDit.Add("背包", new KnapsackManage());
            managesDit.Add("角色信息", new UserInfoManage());
            managesDit.Add("装备", new EquipManage());
            managesDit.Add("当前位置", new MapManage());
            managesDit.Add("攻击", new AttackManage());
            managesDit.Add("任务", new MissionManage());
            managesDit.Add("使用", new ItemManage());
            managesDit.Add("等级", new LevelManage());
            managesDit.Add("商城", new ShopManage());
            managesDit.Add("排行", new RankManage());
            managesDit.Add("复活", new ReviveManage());
            managesDit.Add("传送", new MoveManage()); 
            managesDit.Add("回收", new RecycleManage()); 
            managesDit.Add("拍卖行", new BusinessManage());
            managesDit.Add("查看", new FindManage());
        }

        public void AddManage(String manageName, BaseManage manage)
        {
            managesDit.Add(manageName, manage);
        }

    }
}
