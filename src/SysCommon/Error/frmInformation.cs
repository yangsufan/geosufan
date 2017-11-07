using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SysCommon.Error
{
    public partial class frmInformation : DevComponents.DotNetBar.Office2007Form
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
            if (null == SysCommon.Log.Module.SysLog)
                SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
            SysCommon.Log.Module.SysLog.Write(1, Pra);
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
            if (null == SysCommon.Log.Module.SysLog)
                SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
            SysCommon.Log.Module.SysLog.Write(1, Pra);
            //********************************************
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}