using System;
using System.Collections.Generic;
using System.Text;

namespace Fan.Common
{
    #region 系统登陆时信息提示 委托事件定义
    //定义委托
    public delegate void SysLogInfoChangedHandle(object sender, SysLogInfoChangedEvent e);
    //构建InfoChangedEvent信息
    public class SysLogInfoChangedEvent : EventArgs
    {
        //  改变信息字符串
        private string _information;
        public string Information
        {
            get
            {
                return _information;
            }
        }

        //  默认构造函数
        public SysLogInfoChangedEvent()
        {
            _information = string.Empty;
        }

        //  实际构造函数
        public SysLogInfoChangedEvent(string information)
        {
            _information = information;
        }
    }

    #endregion
}
