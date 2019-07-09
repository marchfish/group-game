using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Native.Csharp.App.Manages
{
    class Facade
    {
        public static Facade facade;

        // 应用路径
        public static string devPath = System.Windows.Forms.Application.StartupPath + "\\dev\\com.guangyingart.demo";

        public Dictionary<string, BaseManage> managesDit = new Dictionary<string, BaseManage>();

        // 工具
        public IniTool iniTool = new IniTool();
        public MidirTool midirTool = new MidirTool();

        // 委托管理
        public EventManage eventManage = new EventManage(); 

        // 初始化类
        public Facade()
        {
            facade = this;

            // 注册用户
            RegisterManage registerManage = new RegisterManage();
            // 背包信息
            KnapsackManage knapsackManage = new KnapsackManage();
            // 用户信息
            UserInfoManage userInfoManage = new UserInfoManage();
            // 装备信息
            EquipManage equipManage = new EquipManage();
            // 地图管理
            MapManage mapManage = new MapManage();
            // 攻击
            AttackManage attackManage = new AttackManage();
        }

        public void AddManage(String manageName, BaseManage manage)
        {
            managesDit.Add(manageName, manage);
        }

    }
}
