using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SysCommon
{
    public static class LogManage
    {
        private static string m_sysLogFileName = Application.StartupPath + "\\..\\Log\\log.log";
        private static SysCommon.Log.ISysLog m_sysLog = new SysCommon.Log.SysLogClass(m_sysLogFileName);
        private static SysCommon.Log.ILog m_functionLog = new SysCommon.Log.FunctionLogClass();

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
    }
}
