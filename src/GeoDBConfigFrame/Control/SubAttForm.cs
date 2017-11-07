using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBConfigFrame
{
    public partial class SubAttForm : DevComponents.DotNetBar.Office2007Form
    {
        public string strSubCode;
        public string strSubName;
        public string strIndexFile;
        public string strMapSymIndexFile;
        public SubAttForm()
        {
            InitializeComponent();

        }

        public void SetFormTextBoxAtt()
        {
            textSubCode.Text = strSubCode;
            textSubName.Text = strSubName;
            if (strIndexFile.Trim() != "")
            {
                textIndexFile.Text = strIndexFile.Substring(0, strIndexFile.LastIndexOf('.'));
            }
            if (strMapSymIndexFile.Trim() != "")
            {
                textBoxMapIndexFile.Text = strMapSymIndexFile;
            }
        }

        public void GetFormTextBoxAtt()
        {
            strSubCode = textSubCode.Text.Trim();
            strSubName = textSubName.Text.Trim();
            strIndexFile = textIndexFile.Text.Trim() + ".xml";
            strMapSymIndexFile = textBoxMapIndexFile.Text.Trim();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            //专题代码 
            if (textSubCode.Text.Trim().Equals(""))
            {
                MessageBox.Show("专题类型不能为空值!","提示");
                return;
            }
            //专题描述 不能为空
            if (textSubName.Text.Trim().Equals(""))
            {
                MessageBox.Show("专题描述不能为空值!", "提示");
                return;
            }
            GetFormTextBoxAtt();
            this.DialogResult = DialogResult.OK;
            this.Hide();
            this.Dispose(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
            this.Dispose(true);
        }

        //专题类型变化
        private void textSubCode_TextChanged(object sender, EventArgs e)
        {
           // textBoxMapIndexFile.Text = textSubCode.Text;//配图文件
            textIndexFile.Text = textSubCode.Text;//脚本文件
        }

        //打开对话框，取MXD得文件
        private void btnServer_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "配图文件(*.mxd)|*.mxd";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxMapIndexFile.Text = dlg.FileName;
            }
        }
    }
}