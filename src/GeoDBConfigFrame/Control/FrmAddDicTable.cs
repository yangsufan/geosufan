using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBConfigFrame
{
    public partial class FrmAddDicTable : DevComponents.DotNetBar.Office2007Form
    {
        private string _TableName = "";
        public string TableName
        {
            get { return _TableName; }
        }
        public FrmAddDicTable()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.textBoxTableName.Text != "")
            {
                _TableName = this.textBoxTableName.Text;
            }
            else
            {
                
                MessageBox.Show("请填写字典表名称!");
                return;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
