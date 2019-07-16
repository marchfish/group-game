
namespace Native.Csharp.App.Models
{
    public class RecoveryItem
    {
        public int Name { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public string Describe { get; set; }
        public string Effect { get; set; }

        public void Add(string itemInfo)
        {
            string[] arr = itemInfo.Split(',');
            this.HP = int.Parse(arr[0]);
            this.MP = int.Parse(arr[1]);
            this.Describe = arr[2];
            this.Effect = arr[3];
        }
    }
}
