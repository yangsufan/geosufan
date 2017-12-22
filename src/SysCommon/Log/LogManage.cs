using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fan.Common
{
    public static class LogManage
    {
        private static string m_sysLogFileName = Application.StartupPath + "\\..\\Log\\log.log";
        private static Fan.Common.Log.ISysLog m_sysLog = new Fan.Common.Log.SysLogClass(m_sysLogFileName);
        private static Fan.Common.Log.ILog m_functionLog = new Fan.Common.Log.FunctionLogClass();

        public static void WriteErrorLog(Exception ex)
        {
            if (m_sysLog != null)
            {
                m_sysLog.WriteLog(ex); 
            }
        }

        public static void WriteErrorLog(Exception ex, params string[] strContent)
        {
            if (m_sysLog != null)
            {
                m_sysLog.WriteLog(ex, strContent);
            }
        }
        public static void WriteLog(string strLog)
        {
            if (m_sysLog != null)
            {
                m_sysLog.WriteLog(strLog);
            }
        }
    }
}
