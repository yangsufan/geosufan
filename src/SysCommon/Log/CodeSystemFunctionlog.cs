using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Fan.Common.Log
{
    public static  class Module
    {
        public static string v_sLogFilePath = Application.StartupPath+"\\..\\Log\\系统运行日志";
        public static clsWriteSystemFunctionLog SysLog;
    }
    public abstract class SystemFuntionLOGInitial
    {
        protected string m_LogFileName;/////////////系统运行日志文件名
        public string LogFileName
        {
            get { return m_LogFileName;}
            set { this.m_LogFileName = value; }
        }
        /// <summary>
        /// 创建一个日志文件
        /// </summary>
        /// <param name="in_sFileName"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool CreateLogFile(string in_sFileName)
        {
            if (File.Exists(in_sFileName))
            {
                return false;
            }
            else
            {
                try
                {
                    FileStream FS= File.Create(in_sFileName);
                    FS.Close();
                    this.m_LogFileName = in_sFileName;
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        /// <summary>
        /// 删除一个日志文件
        /// </summary>
        /// <param name="in_sFileName"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool DeleteLogFile(string in_sFileName)
        {
            if (!File.Exists(in_sFileName))
            {
                return false;
            }
            else
            {
                try
                {
                    File.Delete(in_sFileName);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }

    public class TextSystemFuntionLOG : SystemFuntionLOGInitial, ISystemFunctionLog
    {
        #region ISystemFunctionLog接口成员
        public TextSystemFuntionLOG()
        {

        }
        /// <summary>
        /// 日志初始化
        /// </summary>
        /// <returns></returns>
        public bool Initial(string in_sFileName, DateTime in_LogTime)
        {
            if (string.IsNullOrEmpty(in_sFileName))
            {
                throw new Exception("没有制定系统运行日志路径");
            }
            try
            {
                FileStream Fs = new FileStream(in_sFileName, FileMode.Append);
                StreamWriter SW = new StreamWriter(Fs);
                DateTime InitialTime = in_LogTime;
                if (InitialTime == null) InitialTime = DateTime.Now;
                SW.WriteLine();
                SW.Write("==================================================================================================");
                SW.WriteLine();
                SW.Write("SystemFunctionLog_Initial Time:" + InitialTime.ToString("G") + "  Host:" + System.Net.Dns.GetHostName() + " IPAddress:" + System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).GetValue(0).ToString());
                SW.WriteLine();
                SW.Write("==================================================================================================");
                SW.WriteLine();
                SW.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 日志终结
        /// </summary>
        /// <returns></returns>
        public bool Terminate(string in_sFileName, DateTime in_LogTime)
        {
            if (string.IsNullOrEmpty(in_sFileName))
            {
                throw new Exception("没有制定系统运行日志路径");
            }
            try
            {
                FileStream Fs = new FileStream(in_sFileName, FileMode.Append);
                StreamWriter SW = new StreamWriter(Fs);
                DateTime TerminateTime = in_LogTime;
                if (TerminateTime == null) TerminateTime = DateTime.Now;
                SW.WriteLine();
                SW.Write("==================================================================================================");
                SW.WriteLine();
                SW.Write("SystemFunctionLog_Terminate Time:" + TerminateTime.ToString("G"));
                SW.WriteLine();
                SW.Write("==================================================================================================");
                SW.WriteLine();
                SW.Close();
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public bool Write(string in_sFileName, object in_oLogContent, object in_oParameter, DateTime in_LogTime)/////////////////////////写日志（根据不同的记录类型：例如异常、操作等）
        {
            /*
             * 
             * add type of log here in the child class 
             * 
             */
            return false;
        }
        #endregion
    }
    /// <summary>
    /// Text的异常日志
    /// </summary>
    public class TextExceptionLog : TextSystemFuntionLOG
    {

        public TextExceptionLog()
        {
        }
        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <returns></returns>
        public new bool Write(string in_sFileName, object in_oLogContent, object in_oParameter, DateTime in_LogTime)
        {
            //if (in_oLogContent.GetType().ToString()!= "System.Exception") 
            // if (in_oLogContent.GetType().ToString()!="System.Runtime.InteropServices.COMException")
            Exception ex = in_oLogContent as Exception;
            if (ex == null)
                return false;
            if (string.IsNullOrEmpty(in_sFileName))
            {
                //throw new Exception("没有制定系统运行日志路径");
                return false;
            }
            try
            {
                DateTime RecTime = in_LogTime;
                if (RecTime == null) RecTime = DateTime.Now;
                FileStream Fs = new FileStream(in_sFileName, FileMode.Append);
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
                if (in_oParameter != null)
                {
                    List<string> list_Parameter = in_oParameter as List<string>;
                    if (list_Parameter != null)
                    {
                        foreach (string sParameter in list_Parameter)
                        {
                            SW.WriteLine();
                            SW.Write(" Parameters:" + sParameter);
                        }
                    }
                }
                SW.WriteLine();
                SW.Write("***********************************************************************");
                SW.Flush();
                SW.Close();
                SW.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
   
    }
    /// <summary>
    /// Text的功能操作日志
    /// </summary>
    public class TextOperationLog : TextSystemFuntionLOG
    {
        public TextOperationLog()
        {
        }
        /// <summary>
        /// 写功能操作日志
        /// </summary>
        /// <returns></returns>
        public new bool Write(string in_sFileName, object in_oLogContent, object in_oParameter, DateTime in_LogTime)
        {
            if (in_oLogContent.GetType() != Type.GetType("System.String")) return false;
            if (string.IsNullOrEmpty(in_sFileName))
            {
                //throw new Exception("没有制定系统运行日志路径");
                return false;
            }
            try
            {
                string sContent = in_oLogContent as string;
                if (string.IsNullOrEmpty(sContent)) return false;
                FileStream Fs = new FileStream(in_sFileName, FileMode.Append);
                StreamWriter SW = new StreamWriter(Fs);
                DateTime RecTime = in_LogTime;
                if (RecTime == null) RecTime = DateTime.Now;
                SW.WriteLine();
                SW.Write(RecTime.ToString("G") + ":  功能操作：" + sContent );
                if (in_oParameter != null)
                {
                    List<string> list_Parameter = in_oParameter as List<string>;
                    if (list_Parameter != null)
                    {
                        foreach (string sParameter in list_Parameter)
                        {
                            SW.WriteLine();
                            SW.Write(" Parameters:" + sParameter);
                        }
                    }
                }
                SW.WriteLine();
                SW.Write("***********************************************************************");
                SW.Flush();
                SW.Close();
                SW.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
    /// <summary>
    /// 界面提示日志
    /// </summary>
    public class TextDialogboxLog : TextSystemFuntionLOG
    {
        public TextDialogboxLog()
        {
         }
        public new bool Write(string in_sFileName, object in_oLogContent, object in_oParameter, DateTime in_LogTime)
        {
            //// 1 预留给界面提示日志类型
            if (in_oLogContent.GetType().ToString() != "System.Int32")
                return false;
            int Logtype =(int) in_oLogContent;
            if (Logtype != 1) return false;

            try
            {
                DateTime RecTime = in_LogTime;
                if (RecTime == null) RecTime = DateTime.Now;
                FileStream Fs = new FileStream(in_sFileName, FileMode.Append);
                StreamWriter SW = new StreamWriter(Fs);
                SW.WriteLine();
                SW.Write(RecTime.ToString("G") + ":  界面提醒：");
                if (in_oParameter != null)
                {
                    List<string> list_Parameter = in_oParameter as List<string>;
                    if (list_Parameter != null)
                    {
                        foreach (string sParameter in list_Parameter)
                        {
                            SW.WriteLine();
                            SW.Write(" Parameters:" + sParameter);
                        }
                    }
                }
                SW.WriteLine();
                SW.Write("***********************************************************************");
                SW.Flush();
                SW.Close();
                SW.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}