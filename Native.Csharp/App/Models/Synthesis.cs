
namespace Native.Csharp.App.Models
{
    class Synthesis
    {
        public string Name { get; set; }
        public string Material { get; set; }
        public int SuccessRate { get; set; }
        public string Retain { get; set; }

        public void Add(string info)
        {
            string[] arr = info.Split(',');
            this.Material = arr[0];
            this.SuccessRate = int.Parse(arr[1]);
            this.Retain = arr[2];
        }
    }
}
