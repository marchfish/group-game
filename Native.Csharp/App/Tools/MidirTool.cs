using System.IO;

namespace Tools
{
    public class MidirTool
    {
        public void Midir(string midirPath)
        {
            if (Directory.Exists(midirPath) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(midirPath);
            }
        }
    }
}
