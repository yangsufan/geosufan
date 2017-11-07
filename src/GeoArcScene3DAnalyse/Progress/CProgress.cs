using System;
using System.Collections.Generic;
using System.Text;

namespace GeoArcScene3DAnalyse
{
    /// <summary>
    /// 进度条内容改变的类型
    /// </summary>
    internal enum ProgressAction
    {
        enumChangeDesc,
        enumPerformStep,
        enumProgressValue,
        enumClose,
        enumEnableCancel,
        enumMakeWindowStateNormal,//dwt add 20110322 添加将窗口变成normal状态
        enumMakeWindowStateMinSize//dwt add 20110322 添加将窗口变成minsize状态
    }
    /// <summary>
    /// 进度条的进度相关信息
    /// </summary>
    internal struct ProgressInfo
    {
        public ProgressAction action;
        public object actionValue;
    }
    /// <summary>
    /// 进度条功能封装
    /// </summary>
    public class CProgress
    {
        public delegate void UserCancel(ref bool bEnableCancel);
        public event UserCancel UserCanceled;

        private Progress.FormProgress m_FormProgress = null;
        private System.Threading.Thread m_Thread = null;
        private long m_QuitFlag = 0;
        private long m_ShowFlag = 0;//表示是否调用了ShowProgress方法。
        private long m_AskCancelFlag = 0; //用户询问取消的标志。
        private System.Threading.ManualResetEvent m_WaitEvent = new System.Threading.ManualResetEvent(true);
        private string _Caption;
        private bool _TopMost;
        private bool _EnableCancel;
        private int _MaxValue;
        private int _Step;
        private bool _ShowDescription;
        private bool _ShowProgressNumber;
        private bool _FakeProgress;
        private bool _AutoCircle;

        #region 各种属性
        /// <summary>
        /// 进度条窗口的标题，进度窗口显示后设置无效
        /// </summary>
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool TopMost
        {
            get { return _TopMost; }
            set { _TopMost = value; }
        }
        /// <summary>
        /// 获取标志进度条是否在等待询问是否要取消
        /// </summary>
        public bool UserAskCancel
        {
            get
            {
                return (System.Threading.Interlocked.Read(ref m_AskCancelFlag) == 1);
            }
        }
        /// <summary>
        /// 当UserAskCancel标志为True时，通过此方法通知进度条是否真的要取消。
        /// </summary>
        public void EnableUserCancel(bool bEnableCancel)
        {
            //add by liyonghua 20101220,(bug3065),start
            //如果点击“否”，即bEnableCancel等于false时，进度不取消，继续进行
            if (bEnableCancel == false)
            {
                m_AskCancelFlag = 0;
                return;
            }
            //add by liyonghua 20101220,(bug3065),end
            System.Threading.Interlocked.Exchange(ref m_AskCancelFlag, 0);
            if (FakeProgress)
                return;
            if (null == m_FormProgress)
                return;

            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumEnableCancel;
            vInfo.actionValue = null;
            m_FormProgress.AppendProgressAction(vInfo, true);

        }

        /// <summary>
        /// 是否显示取消按钮,进度窗口显示后设置无效
        /// </summary>
        public bool EnableCancel
        {
            get { return _EnableCancel; }
            set { _EnableCancel = value; }
        }
        /// <summary>
        /// 设置获得进度条最大值，进度窗口显示后设置无效
        /// </summary>
        public int MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; }
        }
        /// <summary>
        /// 设置获得PerformStep方法增加的进度值，进度窗口显示后设置无效
        /// </summary>
        public int Step
        {
            get { return _Step; }
            set { _Step = value; }
        }

        /// <summary>
        /// 是否在进度条左上角显示描述信息
        /// </summary>
        public bool ShowDescription
        {
            get { return _ShowDescription; }
            set { _ShowDescription = value; }
        }

        /// <summary>
        /// 在进度条右上角显示进度百分比
        /// </summary>
        public bool ShowProgressNumber
        {
            get { return _ShowProgressNumber; }
            set { _ShowProgressNumber = value; }
        }

        /// <summary>
        /// 获取是否用户点击了取消按钮
        /// </summary>
        public bool IsUserCanceled
        {
            get
            {
                return (System.Threading.Interlocked.Read(ref m_QuitFlag) == 1);
            }
        }
        /// <summary>
        /// 设置获得是否是假进度条，如果是假进度条则，则程序在0到100之间不停的变化，每隔1秒刷新一个进度。
        /// </summary>
        public bool FakeProgress
        {
            get { return _FakeProgress; }
            set { _FakeProgress = value; }
        }
        /// <summary>
        /// 设置获取进度条的值。
        /// </summary>
        public int ProgresssValue
        {
            get
            {
                if (m_FormProgress == null)
                    return 0;
                return m_FormProgress.ProgressValue;
            }
            set
            {
                SetProgress(value);
            }

        }

        //dwt add 20110322 将进度条窗口最小化
        public void MakeProgressWindowMinSizeState()
        {
            if (null == m_FormProgress)
            {
                return;
            }
            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumMakeWindowStateMinSize;
            m_FormProgress.AppendProgressAction(vInfo, true);
        }
        //dwt add 20110322 将进度条窗口normal回来
        public void MakeProgressWindowNormalState()
        {
            if (null == m_FormProgress)
            {
                return;
            }
            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumMakeWindowStateNormal;
            m_FormProgress.AppendProgressAction(vInfo, true);
        }

        /// <summary>
        /// 自动循环，当PerformStep到达最大值时是否自动跳转到0；
        /// </summary>
        public bool AutoCircle
        {
            get { return _AutoCircle; }
            set { _AutoCircle = value; }
        }
        #endregion
        ~CProgress()
        {
            Close();
        }
        public CProgress()
        {
            MaxValue = 100;
            Step = 1;
            EnableCancel = false;
            ShowDescription = false;
            ShowProgressNumber = false;
            TopMost = true;
        }
         
        public CProgress(string strCaption)
        {
            this.Caption = strCaption;
            MaxValue = 100;
            Step = 1;
            EnableCancel = false;

            ShowDescription = false;
            ShowProgressNumber = false;
        }
       
        /// <summary>
        /// 显示进度条，
        /// </summary>
        public void ShowProgress()
        {
            if (null != m_Thread)
                return;


            System.Threading.Interlocked.Exchange(ref m_ShowFlag, 1); 
            System.Threading.Interlocked.Exchange(ref m_QuitFlag, 0); 

            //开始一个线程。
            System.Threading.ThreadStart start = new System.Threading.ThreadStart(ThreadFun);
            m_Thread = new System.Threading.Thread(start);
            m_Thread.Start();
            
        }
        /// <summary>
        /// 增加一个指定的步伐
        /// </summary>
        /// <param name="nStepLen"></param>
        public void PerformStep(int nStepLen)
        {
            if (FakeProgress)
                return;
            if (!EnterProgress())
                return;

            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumPerformStep;
            vInfo.actionValue = nStepLen;
            m_FormProgress.AppendProgressAction(vInfo, true);
        }
        /// <summary>
        /// 进度条增加一步
        /// </summary>
        public void PerformStep()
        {
            PerformStep(1);
        }

        /// <summary>
        /// 进入进度条，此函数返回True或者False来决定是否可以进入进度条，或者阻塞主线程。
        /// </summary>
        /// <returns></returns>
        private bool EnterProgress()
        {
            if (null == m_FormProgress)
            {
                //这里的值如果不为0则表示已经启动了显示进度条窗口的线程。
                if (System.Threading.Interlocked.Read(ref m_ShowFlag) == 0)
                    return false;

                //如果窗口对象为空则睡眠等待一下。
                while (null == m_FormProgress)
                    System.Threading.Thread.Sleep(10);
            }
            m_WaitEvent.WaitOne();
           return true;
        }
        /// <summary>
        /// 关闭进度条。
        /// </summary>
        public void Close()
        {
            if (!EnterProgress())
                return;

            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumClose;
            vInfo.actionValue = null;
            m_FormProgress.AppendProgressAction(vInfo, true);
        }
        /// <summary>
        /// 设置进度条的值和描述信息
        /// </summary>
        /// <param name="nProgress"></param>
        /// <param name="strDesc"></param>
        public void SetProgress(int nProgress,string strDesc)
        {
            if (FakeProgress)
                return;

            if (!EnterProgress())
                return;

            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumProgressValue;
            vInfo.actionValue = nProgress;
            m_FormProgress.AppendProgressAction(vInfo, false);

            vInfo.action = ProgressAction.enumChangeDesc;
            vInfo.actionValue = strDesc;
            m_FormProgress.AppendProgressAction(vInfo, true);
        }
        /// <summary>
        /// 设置进度条的值。
        /// </summary>
        /// <param name="nProgress"></param>
        public void SetProgress(int nProgress)
        {
            if (FakeProgress)
                return;

            if (!EnterProgress())
                return;

            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumProgressValue;
            vInfo.actionValue = nProgress;
            m_FormProgress.AppendProgressAction(vInfo,true);
        }

        /// <summary>
        /// 设置进度条的描述信息。
        /// </summary>
        /// <param name="nProgress"></param>
        public void SetProgress(string strDesc)
        {
            if (!EnterProgress())
                return;

            ProgressInfo vInfo = new ProgressInfo();
            vInfo.action = ProgressAction.enumChangeDesc;
            vInfo.actionValue = strDesc;
            m_FormProgress.AppendProgressAction(vInfo, true);
        }

        //线程函数用于在工作线程中启动进度条
        private void ThreadFun()
        {
            m_FormProgress = new Progress.FormProgress();
            m_FormProgress.Text = Caption;
            m_FormProgress.ButtonCancel.Visible = EnableCancel;
            m_FormProgress.labelDesc.Visible = ShowDescription;
            m_FormProgress.m_AutoCircle = AutoCircle;
            m_FormProgress.TopMost = TopMost;

            if (FakeProgress)
            {
                m_FormProgress.progressBar1.Maximum =   100;
                m_FormProgress.progressBar1.Step = 1;
                m_FormProgress.labelProgress.Visible = false;
            }
            else
            {
                m_FormProgress.progressBar1.Maximum = (MaxValue > 1) ? MaxValue : 100;
                m_FormProgress.progressBar1.Step = (Step > 0) ? Step : 1;
                m_FormProgress.labelProgress.Visible = ShowProgressNumber;
            }
            m_FormProgress.UserCanceled += new UserCancel(m_FormProgress_UserCanceled);
            if(!m_FormProgress.ShowMe(FakeProgress)) //如果返回False则表示退出的理由是点了取消按钮
                System.Threading.Interlocked.Exchange(ref m_QuitFlag, 1);

            //启动标志位0；
            System.Threading.Interlocked.Exchange(ref m_ShowFlag, 0);
            
            m_FormProgress.Dispose();
            m_FormProgress = null;
            m_Thread = null;

            
        }

        void m_FormProgress_UserCanceled(ref bool bEnableCancel)
        {
            if (null == UserCanceled)
            {   //如果用户没有订阅消息则设置一个内部的标志。
                System.Threading.Interlocked.Exchange(ref m_AskCancelFlag, 1);
                bEnableCancel = false;
                return;
            }

            //设置标志，阻塞主线程
            m_WaitEvent.Reset();
            bool bTopMost = m_FormProgress.TopMost;
            m_FormProgress.TopMost = false;
            UserCanceled(ref bEnableCancel);
            m_FormProgress.TopMost = bTopMost;
            //清空主线程标志。
            m_WaitEvent.Set();
        }

        
    }

   
}
