using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace SysCommon.Log
{
    interface ISystemFunctionLog
    {
        bool Initial(string in_sFileName, DateTime in_LogTime);////////////////////////日志初始化（写入开始日期时间等信息）
        bool Write(string in_sFileName, object in_oLogContent, object in_oParameter, DateTime in_LogTime);/////////////////////////写日志（根据不同的记录类型：例如异常、操作等）
        bool Terminate(string in_sFileName, DateTime in_LogTime);/////////////////////日志结束
    }
}
