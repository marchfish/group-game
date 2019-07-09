using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Manages
{
    public delegate void RegisterUser(string userId, string groupId);

    public class EventManage
    {
        public event RegisterUser registerUser;

        public void OnRegisterUser(string userId, string groupId) {

            registerUser?.Invoke(userId, groupId);

        }


    }
}
