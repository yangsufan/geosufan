using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fan.Common.Error
{
    public partial class frmErrorHandle : BaseForm
    {
        public frmErrorHandle(string strCaption, string strDes)
        {
            InitializeComponent();
            this.Text = strCaption;
            this.labelX.Text = strDes;
            //********************************************
            //guozheng added 系统运行日志
            // 1 是错误提示日志
            List<string> Pra = new List<string>();
            Pra.Add(strCaption);
            Pra.Add(strDes);
            if (null == Fan.Common.Log.Module.SysLog)
                Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
            Fan.Common.Log.Module.SysLog.Write(1, Pra);
            //********************************************
        }

        private void buttonX_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}