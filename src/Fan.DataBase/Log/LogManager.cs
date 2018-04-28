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
        /// <summary>
        /// 系统日志，主要记录捕获到的系统出错的信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="contect"></param>
        /// <returns></returns>
        public static bool WriteSysLog(Exception ex, params string[] contect)
        {
            return m_SysLog.WriteErrorLog(ex, contect);
        }
        /// <summary>
        /// 写系统信息，主要记录系统操作的相关信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static bool WriteSysInfo(params string[] infos)
        {
            return m_SysLog.WriteInfo(infos);
        }
    }
}
