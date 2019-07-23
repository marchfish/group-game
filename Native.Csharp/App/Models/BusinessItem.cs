
namespace Native.Csharp.App.Models
{
    class BusinessItem
    {
        public string No { get; set; }
        public string ItemName { get; set; }
        public int Coin { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        public void Add(string itemInfo)
        {
            string[] arr = itemInfo.Split('|');
            this.ItemName = arr[0];
            this.Coin = int.Parse(arr[1]);
            this.UserName = arr[2];
            this.UserId = arr[3];
        }
    }
}
