
namespace Native.Csharp.App.Models
{
    class Vip
    {
        public int Level { get; set; }
        public string endTime { get; set; }
        public int Protect { get; set; }
        public int SuccessRate { get; set; }
        public int Number { get; set; }

        public void Add(string userInfo)
        {
            string[] arr = userInfo.Split(',');
            this.Level = int.Parse(arr[0]);
            this.endTime = arr[1];
            this.Protect = int.Parse(arr[2]);
            this.SuccessRate = int.Parse(arr[3]);
            this.Number = int.Parse(arr[4]);
        }
    }
}
