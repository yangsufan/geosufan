using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    /// <summary>
    /// 数据检查父类
    /// 各类检查功能实现类从此类继承
    /// </summary>
    public class GOGISErrorChecker
    {
        public delegate void EventHandle(object sender,ErrorEventArgs ErrorArg);
        public delegate void ProgressHandle(object sender, int CurStep, int MaxValue);

        public event EventHandle FindErr;
        public event ProgressHandle ProgressStep;

        protected virtual void OnErrorFind(object sender,ErrorEventArgs e)
        {
            if (FindErr!=null)
            {
                this.FindErr(sender,e); 
            }
        }

        protected virtual void OnProgressStep(object sender, int CurStep, int MaxValue)
        {
            if (FindErr != null)
            {
                this.ProgressStep(sender,CurStep, MaxValue);
            }
        }
    }
}
