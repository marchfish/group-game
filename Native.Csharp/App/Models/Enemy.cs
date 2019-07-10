using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Models
{
    public class Enemy
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Describe { get; set; }
        public int Agg { get; set; }
        public int HP { get; set; }
        public int Defense { get; set; }
        public string Attribute { get; set; }
        public int Coin { get; set; }
        public string items { get; set; }
        public int Exp { get; set; }
        public string Type { get; set; }

        public void Add(string userInfo)
        {
            string[] arr = userInfo.Split(',');
            this.Level = int.Parse(arr[0]);
            this.Describe = arr[1];
            this.Agg = int.Parse(arr[2]);
            this.HP = int.Parse(arr[3]);
            this.Defense = int.Parse(arr[4]);
            this.Attribute = arr[5];
            this.Coin = int.Parse(arr[6]);
            this.items = arr[7];
            this.Exp = int.Parse(arr[8]);
            this.Type = arr[9];
        }
    }
}
