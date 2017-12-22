using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fan.Common.Error
{
    public partial class frmInformation : BaseForm
    {
        public frmInformation(string strOkName, string strCancelName, string strDes)
        {
            InitializeComponent();
            this.buttonXOk.Text = strOkName;
            this.buttonXCancel.Text = strCancelName;
            this.labelX.Text = strDes;
        }

        private void buttonXOk_Click(object sender, EventArgs e)
        {
            //********************************************
            //guozheng added 系统运行日志
            // 1 是错误提示日志
            List<string> Pra = new List<string>();
            Pra.Add(this.labelX.Text);
            Pra.Add("OK");
            if (null == Fan.Common.Log.Module.SysLog)
                Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
            Fan.Common.Log.Module.SysLog.Write(1, Pra);
            //********************************************
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            //********************************************
            //guozheng added 系统运行日志
            // 1 是错误提示日志
            List<string> Pra = new List<string>();
            Pra.Add(this.labelX.Text);
            Pra.Add("CANCLE");
            if (null == Fan.Common.Log.Module.SysLog)
                Fan.Common.Log.Module.SysLog = new Fan.Common.Log.clsWriteSystemFunctionLog();
            Fan.Common.Log.Module.SysLog.Write(1, Pra);
            //********************************************
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}