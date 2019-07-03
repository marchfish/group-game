using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    class Facade
    {
        public static Facade facade;

        public Dictionary<string, BaseManage> managesDit = new Dictionary<string, BaseManage>();

        // 初始化类
        public Facade()
        {
            facade = this;

            // 注册管理
            RegisterManage registerManage = new RegisterManage();
            // 背包管理
            KnapsackManage knapsackManage = new KnapsackManage();


        }

        public void AddManage(String manageName, BaseManage manage)
        {
            managesDit.Add(manageName, manage);
        }

    }
}
