using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Fan.DataBase.Log
{
    /// <summary>
    /// 日志操作接口
    /// </summary>
    interface ILog
    {
        bool InitialLog();
        bool WriteErrorLog(Exception ex, params string[] StrMessage);
        bool WriteInfo(params string[] StrInfos);
    }
    /// <summary>
    /// 系统日志接口
    /// </summary>
    interface ISysLog : ILog
    {
        string GetLogStr();
    }
    /// <summary>
    /// 操作日志接口
    /// </summary>
    interface IOperaterLog : ILog
    {
        DataTable GetLogTable();
    }
}
