using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataManagerFrame
{
    public partial class FrmModifyName : DevComponents.DotNetBar.Office2007Form
    {
        
        UCResultDataManager m_UCResultDataManager = null;
        public string m_Path = "";
        public string strModifyValue
        {
            set
            {
                txtName.Text = value;
            }
        }
        public FrmModifyName(UCResultDataManager pUCResultDataManager,string strPath)
        {
         
            m_UCResultDataManager = pUCResultDataManager;
            m_Path = strPath;
            InitializeComponent();
        }

        private void bttCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose(true);
        }

        private void bttSure_Click(object sender, EventArgs e)
        {
            string strNewPath = System.IO.Path.GetDirectoryName(m_Path) + "\\" + txtName.Text + System.IO.Path.GetExtension(m_Path);
            if (System.IO.File.Exists(strNewPath))
            {
                MessageBox.Show("该文件已经存在请重命名！", "提示！");
                txtName.Text = System.IO.Path.GetFileNameWithoutExtension(m_Path);
                return;
            }
            m_UCResultDataManager.m_ModifyName = txtName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
            this.Dispose(true);
          
        }

       
    }
}
