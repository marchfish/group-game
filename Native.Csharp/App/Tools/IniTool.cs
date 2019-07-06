using Native.Csharp.App.Manages;
using System.Runtime.InteropServices;
using System.Text;

namespace Tools
{
    public class IniTool
    {
        /// <summary>
        /// ini文件类
        /// </summary>
        private string m_FileName;

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileInt(
            string lpAppName,
            string lpKeyName,
            int nDefault,
            string lpFileName
            );

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
            );

        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName
            );

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="aFileName">Ini文件路径</param>
        public IniTool(string aFileName)
        {
            this.m_FileName = aFileName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IniTool()
        { }

        /// <summary>
        /// [扩展]读Int数值
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        //public int ReadInt(string section, string name, int def)
        //{
        //    return GetPrivateProfileInt(section, name, def, this.m_FileName);
        //}

        public int ReadInt(string filePath, string iniName, string section, string name, int def)
        {
            return GetPrivateProfileInt(section, name, def, filePath + "\\" + iniName);
        }

        /// <summary>
        /// [扩展]读取string字符串
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="def">默认值</param>
        /// <returns></returns>
        //public string ReadString(string section, string name, string def)
        //{
        //    StringBuilder vRetSb = new StringBuilder(2048);
        //    GetPrivateProfileString(section, name, def, vRetSb, 2048, this.m_FileName);
        //    return vRetSb.ToString();
        //}
        public string ReadString(string filePath, string iniName, string section, string name, string def)
        {
            StringBuilder vRetSb = new StringBuilder(2048);
            GetPrivateProfileString(section, name, def, vRetSb, 2048, filePath + "\\" + iniName);
            return vRetSb.ToString();
        }

        /// <summary>
        /// [扩展]写入Int数值，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="Ival">写入值</param>
        //public void WriteInt(string section, string name, int Ival)
        //{
        //    WritePrivateProfileString(section, name, Ival.ToString(), this.m_FileName);
        //}

        /// <summary>
        /// [扩展]写入Int数值，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="filePath">文件夹路径</param>
        /// <param name="iniName">ini文件名</param>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="Ival">写入值</param>
        public void WriteInt(string filePath, string iniName, string section, string name, int Ival)
        {
            Facade.facade.midirTool.Midir(filePath);
            WritePrivateProfileString(section, name, Ival.ToString(), filePath + '\\' + iniName);
        }

        /// <summary>
        /// [扩展]写入String字符串，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="strVal">写入值</param>
        //public void WriteString(string section, string name, string strVal)
        //{
        //    WritePrivateProfileString(section, name, strVal, this.m_FileName);
        //}

        /// <summary>
        /// [扩展]写入String字符串，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="filePath">文件夹路径</param>
        /// <param name="iniName">ini文件名</param>
        /// <param name="section">节</param>
        /// <param name="name">键</param>
        /// <param name="strVal">写入值</param>
        public void WriteString(string filePath, string iniName, string section, string name, string strVal)
        {
            Facade.facade.midirTool.Midir(filePath);
            WritePrivateProfileString(section, name, strVal, filePath + '\\' + iniName);
        }
        /// <summary>
        /// 删除指定的 节
        /// </summary>
        /// <param name="section"></param>
        public void DeleteSection(string section)
        {
            WritePrivateProfileString(section, null, null, this.m_FileName);
        }
        public void DeleteSection(string filePath, string section)
        {
            WritePrivateProfileString(section, null, null, filePath);
        }
        /// <summary>
        /// 删除全部 节
        /// </summary>
        public void DeleteAllSection()
        {
            WritePrivateProfileString(null, null, null, this.m_FileName);
        }
        public void DeleteAllSection(string filePath)
        {
            WritePrivateProfileString(null, null, null, filePath);
        }
        /// <summary>
        /// 读取指定 节-键 的值
        /// </summary>
        /// <param name="section"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        //public string IniReadValue(string section, string name)
        //{
        //    StringBuilder strSb = new StringBuilder(256);
        //    GetPrivateProfileString(section, name, "", strSb, 256, this.m_FileName);
        //    return strSb.ToString();
        //}
        public string IniReadValue(string filePath, string iniName, string section, string name)
        {
            StringBuilder strSb = new StringBuilder(256);
            GetPrivateProfileString(section, name, "", strSb, 256, filePath + "\\" + iniName);
            return strSb.ToString();
        }

        /// <summary>
        /// 写入指定值，如果不存在 节-键，则会自动创建
        /// </summary>
        /// <param name="section"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        //public void IniWriteValue(string section, string name, string value)
        //{
        //    WritePrivateProfileString(section, name, value, this.m_FileName);
        //}
        public void IniWriteValue(string filePath, string iniName, string section, string name, string value)
        {
            Facade.facade.midirTool.Midir(filePath);
            WritePrivateProfileString(section, name, value, filePath + '\\' + iniName);
        }
    }
}
