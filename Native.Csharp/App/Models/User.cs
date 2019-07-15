namespace Native.Csharp.App.Models
{
   public class User
   {
        public string Name { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Agg { get; set; }
        public int Magic { get; set; }
        public int Crit { get; set; }
        public int Dodge { get; set; }
        public int Defense { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public string Fame { get; set; }
        public string Pos { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }

        public void Add(string userInfo)
        {
            string[] arr = userInfo.Split(',');
            this.Name = arr[0];
            this.HP = int.Parse(arr[1]);
            this.MP = int.Parse(arr[2]);
            this.Agg = int.Parse(arr[3]);
            this.Magic = int.Parse(arr[4]);
            this.Crit = int.Parse(arr[5]);
            this.Dodge = int.Parse(arr[6]);
            this.Defense = int.Parse(arr[7]);
            this.Level = int.Parse(arr[8]);
            this.Exp = int.Parse(arr[9]);
            this.Fame = arr[10];
            this.Pos = arr[11];
            this.MaxHP = int.Parse(arr[12]);
            this.MaxMP = int.Parse(arr[13]);
        }


    }


}
