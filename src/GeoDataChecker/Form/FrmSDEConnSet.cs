using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataChecker
{
    /// <summary>
    /// 设置SDE连接信息
    /// </summary>
    public partial class FrmSDEConnSet : DevComponents.DotNetBar.Office2007Form
    {
        string m_SDEParaStr = "";
        /// <summary>
        /// SDE参数连接信息
        /// </summary>
        public string SDEParaStr
        {
            get
            {
                return m_SDEParaStr;
            }
            set
            {
                m_SDEParaStr = value;
            }
        }
        public FrmSDEConnSet()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtUser.Text.Trim()==""||txtPassword.Text.Trim()=="")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请完整填写用户名或密码！");
                return;
            }

            m_SDEParaStr = txtServer.Text.Trim() + ";" + txtInstance.Text.Trim() + ";" + txtUser.Text.Trim() + ";" + txtPassword.Text.Trim() + ";" + txtVersion.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}