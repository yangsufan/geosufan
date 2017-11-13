using System;
namespace SysCommon.Log
{
    public class clsWriteSystemFunctionLog
    {
        /*
         * guozheng added 2010-11-25
         *  该类实现系统运行日志的记录（目前实现异常记录和功能运用记录2010-11-25）
         *  需要记录新的日志类型，则重写一个继承于TextSystemFuntionLOG的子类，重写Write方法，
         *  并在该类的构造函数中为“写日志委托”注册这一事件
         */
        //写日志委托
        private delegate bool WriteLog(string in_sLogFileName, object in_oLogContent, object in_oLogParamter, DateTime in_LogTime);
        private event WriteLog eWritelog;
        //初始化日志委托
        private delegate bool InitialLog(string in_sLogFileName, DateTime in_LogTime);
        private event InitialLog eInitialLog;
        //结束日志委托
        private delegate bool Teminatelog(string in_sLogFileName, DateTime in_LogTime);
        private event InitialLog eTeminatelog;

        private string m_sLofFileName;//////系统运行日志文件名
        public string sLogFileName
        {
            get { return this.m_sLofFileName; }
            set { this.m_sLofFileName = value; }
        }
        /// <summary>
        /// 日志初始信息方法
        /// </summary>
        /// <returns></returns>
        public bool Initial()
        {
            if (null != this.eInitialLog)
            {
                return this.eInitialLog(this.m_sLofFileName, DateTime.Now);
            }
            return false;
        }
        /// <summary>
        /// 日志结束方法
        /// </summary>
        /// <returns></returns>
        public bool Teminate()
        {
            if (null != this.eInitialLog)
            {
                return this.eTeminatelog(this.m_sLofFileName, DateTime.Now);
            }
            return false;
        }
        /// <summary>
        /// 改造函数
        /// </summary>
        public clsWriteSystemFunctionLog()
        {
            ////
            this.eInitialLog += (new TextSystemFuntionLOG()).Initial;//////////日志初始化方法
            this.eTeminatelog += (new TextSystemFuntionLOG()).Terminate;///////日志结束方法
            ////
            
            this.eWritelog += (new TextExceptionLog()).Write;///////注册系统异常事件
            this.eWritelog += (new TextOperationLog()).Write;///////注册系统功能操作日志
            //*******************************************************************

            //需要增加新的日志记录类型，在此注册新的事件

            //*******************************************************************
            this.eWritelog += (new TextDialogboxLog()).Write;//////注册错误提示界面日志
            //***************************************************************************************************
            //guozheng 2010-11-22 系统运行日志目录
            try
            {
                if (!System.IO.Directory.Exists(SysCommon.Log.Module.v_sLogFilePath))
                {
                    System.IO.Directory.CreateDirectory(SysCommon.Log.Module.v_sLogFilePath);
                }
                if (!System.IO.File.Exists(SysCommon.Log.Module.v_sLogFilePath + "\\" + DateTime.Now.ToShortDateString() + ".log"))
                {
                    SysCommon.Log.TextSystemFuntionLOG SysLog = new SysCommon.Log.TextSystemFuntionLOG();
                    SysLog.CreateLogFile(SysCommon.Log.Module.v_sLogFilePath + "\\" + DateTime.Now.ToShortDateString() + ".log");
                    //SysLog.Initial(SysCommon.Log.Module.v_sLogFilePath + "\\" + DateTime.Now.ToShortDateString() + ".log", DateTime.Now);
                }
                this.m_sLofFileName = SysCommon.Log.Module.v_sLogFilePath + "\\" + DateTime.Now.ToShortDateString() + ".log";
            }
            catch
            {
            }
            //***************************************************************************************************
        }
        /// <summary>
        /// 执行写日志
        /// </summary>
        /// <param name="in_oLogContent">日志内容</param>
        /// <param name="in_oLogParamter">日志参数</param>
        /// <param name="LogTime">日志时间</param>
        /// <returns></returns>
        public bool Write(object in_oLogContent, object in_oLogParamter, DateTime LogTime)
        {
            if (this.eWritelog != null)
            {
                bool res= eWritelog(this.m_sLofFileName,in_oLogContent, in_oLogParamter, LogTime);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 执行写日志（无参数，默认当前时间）
        /// </summary>
        /// <param name="in_oLogContent">日志内容</param>
        /// <returns></returns>
        public bool Write(object in_oLogContent)
        {
            if (this.eWritelog != null)
            {
                bool res = eWritelog(this.m_sLofFileName, in_oLogContent, null, DateTime.Now);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 执行写日志（默认当前时间）
        /// </summary>
        /// <param name="in_oLogContent">日志内容</param>
        /// <param name="in_oLogParamter">日志参数</param>
        /// <returns></returns>
        public bool Write(object in_oLogContent, object in_oLogParamter)
        {
            if (this.eWritelog != null)
            {
                bool res = eWritelog(this.m_sLofFileName, in_oLogContent, in_oLogParamter, DateTime.Now);
                return true;
            }
            return false;
        }
      
    }
}