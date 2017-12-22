using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Fan.Common;

namespace Fan.Plugin
{
    public static class Mod
    {
        public readonly static string m_SysXmlPath = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\Xml\\GeoDBConfigSys.Xml";
        public readonly static string m_ResPath = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\Pic";
        public readonly static string m_PluginFolderPath = System.Windows.Forms.Application.StartupPath + "\\..\\Bin";
        public readonly static string m_LogPath = System.Windows.Forms.Application.StartupPath + "\\..\\Log\\系统初始化";

        //连接信息
        public static string v_ConfigPath = System.Windows.Forms.Application.StartupPath + "\\conn.dat";
        public static string Server = "";
        public static string Instance = "";
        public static string Database="";
        public static string User = "";
        public static string Password = "";
        public static string Version = "";
        public static string dbType = "";

        public static User v_AppUser;
        public static XmlDocument v_SystemXml;
        public static Fan.Plugin.Application.AppForm v_AppForm;
        public static Dictionary<string, List<string>> m_Dic = new Dictionary<string, List<string>>();//记录图层比例尺 xisheng  20111117
        private static string _LogFilePath = System.Windows.Forms.Application.StartupPath + "\\..\\Log\\Fan.Plugin.txt";
        public static void WriteLocalLog(string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (!File.Exists(_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(_LogFilePath);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string strread = strLog + "     " + strTime + "\r\n";
            StreamWriter sw = new StreamWriter(_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }
    }
}
