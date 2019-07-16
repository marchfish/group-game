using Native.Csharp.App.EventArgs;
using Native.Csharp.App.Models;

namespace Native.Csharp.App.Manages
{
    public delegate void RegisterUser(string userId, string groupId);
    public delegate void UserUpEquip(User user, Equip equip, string groupPath, string userId);
    public delegate void UserDownEquip(User user, Equip equip, string groupPath, string userId);
    public delegate void EnemyDeath(User user, Enemy enemy, string groupPath, CqGroupMessageEventArgs e);
    public delegate void IsUplevel(User user, string groupPath, CqGroupMessageEventArgs e);

    public class EventManage
    {
        public event RegisterUser RegisterUser;
        public event UserUpEquip UserUpEquip;
        public event UserDownEquip UserDownEquip;
        public event EnemyDeath EnemyDeath;
        public event IsUplevel IsUplevel;

        // 注册用户
        public void OnRegisterUser(string userId, string groupId)
        {
            RegisterUser?.Invoke(userId, groupId);
        }

        // 装备物品
        public void OnUserUpEquip(User user, Equip equip, string groupPath, string userId)
        {
            UserUpEquip?.Invoke(user, equip, groupPath, userId);
        }

        // 卸下装备
        public void OnUserDownEquip(User user, Equip equip, string groupPath, string userId)
        {
            UserDownEquip?.Invoke(user, equip, groupPath, userId);
        }

        // 击杀怪物
        public void OnEnemyDeath(User user, Enemy enemy, string groupPath, CqGroupMessageEventArgs e)
        {
            EnemyDeath?.Invoke(user, enemy, groupPath, e);
        }

        // 判断是否升级
        public void OnIsUplevel(User user, string groupPath, CqGroupMessageEventArgs e)
        {
            IsUplevel?.Invoke(user, groupPath, e);
        }
    }
}
