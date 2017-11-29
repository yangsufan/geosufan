using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SysCommon.Progress
{
    partial class FormProgress : BaseForm
    {
        public event CProgress.UserCancel UserCanceled;

        private int m_nUpdateFlag = 0;
        public long m_ProgressValue = 0;

        public bool m_AutoCircle = false;

        private Timer m_UpdateUITimer = new Timer();
        private Timer m_FakeTimer =  null;

        public Queue<ProgressInfo> m_Queue = new Queue<ProgressInfo>();

        public FormProgress()
        {
            InitializeComponent();
        }
        

       /// <summary>
       /// 添加一个进度条动作
       /// </summary>
       /// <param name="vInfo"></param>
        public void AppendProgressAction(ProgressInfo vInfo,bool bUpdate)
        {
            lock (m_Queue)
            {
                m_Queue.Enqueue(vInfo);
            }

            //设置信号量1表示发生了改变

            if(bUpdate)
                System.Threading.Interlocked.Exchange(ref m_nUpdateFlag, 1);
        }

        /// <summary>
        /// 显示进度条窗口

        /// </summary>
        /// <returns></returns>
        public bool ShowMe(bool bFake)
        {
            if (bFake)
            {//如果是假的。

                m_FakeTimer = new Timer();
                m_FakeTimer.Interval = 100;
                m_FakeTimer.Tick += new EventHandler(m_FakeTimer_Tick);
                m_FakeTimer.Start();
            }
            m_UpdateUITimer.Interval = 10;
            m_UpdateUITimer.Tick += new EventHandler(m_UpdateUITimer_Tick);
            m_UpdateUITimer.Start();
            return  (ShowDialog() != DialogResult.Cancel);
        }

        void m_FakeTimer_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt16(progressBar1.Text) < progressBar1.Properties.Maximum)
                progressBar1.PerformStep();
            else
                progressBar1.Text = progressBar1.Properties.Minimum.ToString();

        }
        public int ProgressValue
        {
            get
            {
                return Convert.ToInt32(System.Threading.Interlocked.Read(ref m_ProgressValue));
            }
        }
        void m_UpdateUITimer_Tick(object sender, EventArgs e)
        { 
            //检查是否存在更新的内容
            if (1 != System.Threading.Interlocked.CompareExchange(ref m_nUpdateFlag, 0, 1))
                return;
             
            lock (m_Queue)
            {
                while (m_Queue.Count > 0)
                {
                    ProgressInfo vInfo = m_Queue.Dequeue();
                    switch (vInfo.action)
                    {
                        //dwt add 20110322 添加进度条的行为处理
                        case ProgressAction.enumMakeWindowStateNormal:
                            this.WindowState = FormWindowState.Normal;
                            return;
                        case ProgressAction.enumMakeWindowStateMinSize:
                            this.WindowState = FormWindowState.Minimized;
                            return;
                        //dwt end 20110322 添加进度条的行为处理

                        case ProgressAction.enumEnableCancel:
                            //允许Cancel了。

                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            return;
                        case ProgressAction.enumClose:
                            //设置退出的理由是OK，这个是因为这是由外部要求退出的。

                            this.DialogResult = DialogResult.OK;
                            this.Close();
                            return;
                        case ProgressAction.enumChangeDesc:
                            labelDesc.Text = vInfo.actionValue.ToString();
                            labelDesc.Refresh();
                            break;
                        case ProgressAction.enumPerformStep:
                            int nStep = Convert.ToInt32(vInfo.actionValue);
                            if ((Convert.ToInt16(progressBar1.Text)  + nStep)<= progressBar1.Properties.Maximum)
                            {
                                if (1 == nStep)
                                    progressBar1.PerformStep();
                                else
                                    progressBar1.Text += nStep;
                                labelProgress.Text = Convert.ToInt32((100 * Convert.ToInt16(progressBar1.Text) / progressBar1.Properties.Maximum)).ToString() + "%";
                            }
                            else if (m_AutoCircle)
                                progressBar1.Text = progressBar1.Properties.Minimum.ToString();

                            break;
                        case ProgressAction.enumProgressValue:
                            int nValue = Convert.ToInt32(vInfo.actionValue);
                            if (nValue >= progressBar1.Properties.Minimum && nValue <= progressBar1.Properties.Maximum)
                            {
                                progressBar1.Text = nValue.ToString();
                                labelProgress.Text = Convert.ToInt32((100 * Convert.ToInt16(progressBar1.Text) / progressBar1.Properties.Maximum)).ToString() + "%";
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            System.Threading.Interlocked.Exchange(ref m_ProgressValue,Convert.ToInt16(progressBar1.Text));

        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            bool bEnableCancel = true;
            UserCanceled(ref bEnableCancel);
            if (!bEnableCancel)
                return;

            this.DialogResult = DialogResult.Cancel;
            Close();

        }

        private void labelProgress_Click(object sender, EventArgs e)
        {

        }
        
    }
}
