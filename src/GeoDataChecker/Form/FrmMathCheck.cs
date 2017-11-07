using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataChecker
{
    public partial class FrmMathCheck : DevComponents.DotNetBar.Office2007Form
    {
        string prjFileName = "";
        public string PRJFNAME
        {
            get 
            {
                return prjFileName;
            }
            set 
            {
                prjFileName=value;
            }
        }
        public FrmMathCheck()
        {
            InitializeComponent();
        }
       
        private void btnPrj_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.CheckFileExists = true;
            OpenFile.CheckPathExists = true;
            OpenFile.Title = "选择空间参考文件";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtPrj.Text = OpenFile.FileName;
            }

            prjFileName = txtPrj.Text.Trim();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(prjFileName=="")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择空间参考标准文件！");
                return;
            }
            this.DialogResult=DialogResult.OK;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}