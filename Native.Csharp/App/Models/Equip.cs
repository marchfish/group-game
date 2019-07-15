namespace Native.Csharp.App.Models
{
   public class Equip
   {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Agg { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Magic { get; set; }
        public int Dodge { get; set; }
        public int Crit { get; set; }
        public int Defense { get; set; }
        public string Quality { get; set; }
        public string Type { get; set; }
        public string Describe { get; set; }

        public void Add(string userInfo)
        {
            string[] arr = userInfo.Split(',');
            this.Level = int.Parse(arr[0]);
            this.Agg = int.Parse(arr[1]);
            this.HP = int.Parse(arr[2]);
            this.MP = int.Parse(arr[3]);
            this.Magic = int.Parse(arr[4]);
            this.Dodge = int.Parse(arr[5]);
            this.Crit = int.Parse(arr[6]);
            this.Defense = int.Parse(arr[7]);
            this.Quality = arr[8];
            this.Type = arr[9];
            this.Describe = arr[10];
        }


    }


}
