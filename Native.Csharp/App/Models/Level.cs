namespace Native.Csharp.App.Models
{
    class Level
    {
        public int Name { get; set; }
        public int Agg { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Defense { get; set; }
        public int Exp { get; set; }
        public string Fame { get; set; }

        public void Add(string userInfo)
        {
            string[] arr = userInfo.Split(',');
            this.Agg = int.Parse(arr[0]);
            this.HP = int.Parse(arr[1]);
            this.MP = int.Parse(arr[2]);
            this.Defense = int.Parse(arr[3]);
            this.Exp = int.Parse(arr[4]);
            this.Fame = arr[5];
        }
    }
}
