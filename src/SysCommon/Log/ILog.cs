using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SysCommon.Log
{
    /// <summary>
    /// 日志记录接口
    /// </summary>
    interface ILog
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        bool InitialLog();
    }
    /// <summary>
    /// 功能日志，主要包括系统操作相关日志接口。写入数据库
    /// </summary>
    interface IFunctionLog : ILog
    {

        bool WriteLog(string FunctionName, params string[] LogStr);
    }
    /// <summary>
    /// 系统日志，主要记录系统运行状况。以文本的方式记录
    /// </summary>
    interface ISysLog : ILog
    {
        string logFileName
        {
            get;
        }
        /// <summary>
        /// 获取日志内容
        /// </summary>
        /// <returns></returns>
        string GetLogStr();
        /// <summary>
        /// 写入异常信息到日志
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        bool WriteLog(Exception ex);
        /// <summary>
        /// 写入异常信息到日志
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="strOperatorName"></param>
        /// <returns></returns>
        bool WriteLog(Exception ex, params string[] strContent);
    }
}
