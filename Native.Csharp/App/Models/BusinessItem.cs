
namespace Native.Csharp.App.Models
{
    class BusinessItem
    {
        public string No { get; set; }
        public string ItemName { get; set; }
        public int Coin { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int Num { get; set; }

        public void Add(string itemInfo)
        {
            string[] arr = itemInfo.Split('|');
            string[] item = arr[0].Split('*');

            this.ItemName = item[0];
            this.Coin = int.Parse(arr[1]);
            this.UserName = arr[2];
            this.UserId = arr[3];
            this.Num = int.Parse(item[1]);
        }
    }
}
