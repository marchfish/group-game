using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
