using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fan.DataBase.Log
{
   public static class LogManager
    {
        private static string m_sysLogFileName = Application.StartupPath + "\\..\\Log\\log.log";
        private static ILog m_SysLog = new SysLogManange(m_sysLogFileName);
        private static ILog m_OperatorLog = new OperaterLogManange();
        public static bool WriteSysLog(Exception ex, params string[] contect)
        {
            return m_SysLog.WriteErrorLog(ex, contect);
        }
        public static bool WriteSysInfo(params string[] infos)
        {
            return m_SysLog.WriteInfo(infos);
        }
    }
}
