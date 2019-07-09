namespace Native.Csharp.App.Models
{
   public class User
   {
        public string Name { get; set; }
        public string HP { get; set; }
        public string MP { get; set; }
        public string Agg { get; set; }
        public string Magic { get; set; }
        public string Crit { get; set; }
        public string Dodge { get; set; }
        public string Defense { get; set; }
        public string Level { get; set; }
        public string Exp { get; set; }
        public string Fame { get; set; }
        public string Pos { get; set; }

        public void Add(string userInfo)
        {
            string[] arr = userInfo.Split(',');
            this.Name = arr[0];
            this.HP = arr[1];
            this.MP = arr[2];
            this.Agg = arr[3];
            this.Magic = arr[4];
            this.Crit = arr[5];
            this.Dodge = arr[6];
            this.Defense = arr[7];
            this.Level = arr[8];
            this.Exp = arr[9];
            this.Fame = arr[10];
            this.Pos = arr[11];
        }


    }


}
