using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fan.DataBase.Log
{
    public class SysLogManange : ISysLog
    {
        public SysLogManange(string LogFileName)
        {
            m_LogFileName = LogFileName;
            InitialLog();
        }
        private string m_LogFileName = string.Empty;
        public bool InitialLog()
        {
            if (string.IsNullOrEmpty(m_LogFileName)) return false;
            if (File.Exists(m_LogFileName))
            {
                File.Delete(m_LogFileName);
            }
            File.Create(m_LogFileName);
            return true;
        }
        public bool WriteErrorLog(Exception ex, params string[] StrMessage)
        {
            try
            {
                DateTime RecTime = DateTime.Now;
                FileStream Fs = new FileStream(m_LogFileName, FileMode.Append);
                StreamWriter SW = new StreamWriter(Fs);
                SW.WriteLine();
                SW.Write(RecTime.ToString("G") + "  :异常：");
                SW.WriteLine();
                SW.Write(" HelpLink:" + ex.HelpLink);
                SW.WriteLine();
                SW.Write(" Source:" + ex.Source);
                SW.WriteLine();
                SW.Write(" Message:" + ex.Message);
                SW.WriteLine();
                SW.Write(" StackTrace:" + ex.StackTrace);
                SW.WriteLine();
                if (StrMessage.Length > 0)
                {

                    foreach (string sParameter in StrMessage)
                    {
                        SW.WriteLine();
                        SW.Write("Parameters:" + sParameter);
                    }
                    SW.WriteLine();
                }
                SW.Write("***********************************************************************");
                SW.Flush();
                SW.Close();
                SW.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool WriteInfo(params string[] StrInfos)
        {
            try
            {
                DateTime RecTime = DateTime.Now;
                FileStream Fs = new FileStream(m_LogFileName, FileMode.Append);
                StreamWriter SW = new StreamWriter(Fs);
                SW.WriteLine();
                SW.Write(RecTime.ToString("G") + "  :异常：");
                SW.WriteLine();
                if (StrInfos.Length > 0)
                {

                    foreach (string sParameter in StrInfos)
                    {
                        SW.WriteLine();
                        SW.Write(string.Format("Log:", sParameter));
                    }
                    SW.WriteLine();
                }
                SW.Write("***********************************************************************");
                SW.Flush();
                SW.Close();
                SW.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public string GetLogStr()
        {
            string str = string.Empty;
            try
            {
                DateTime RecTime = DateTime.Now;
                FileStream Fs = new FileStream(m_LogFileName, FileMode.Open);
                StreamReader sr = new StreamReader(Fs);
                str = sr.ReadToEnd();
            }
            catch (Exception e)
            {
            }
            return str;
        }
    }
}
