﻿using System;
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
        public string Items { get; set; }
        public int Probability { get; set; }
        public int Exp { get; set; }
        public string Certain { get; set; }
        public string Type { get; set; }
        public string Move { get; set; }

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
            this.Items = arr[7];
            this.Probability = int.Parse(arr[8]);
            this.Exp = int.Parse(arr[9]);
            this.Certain = arr[10];
            this.Type = arr[11];
            this.Move = arr[12];
        }
    }
}
