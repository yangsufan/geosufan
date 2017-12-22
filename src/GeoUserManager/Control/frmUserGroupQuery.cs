using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Fan.Common.Gis;
using Fan.Common.Error;
using Fan.Common.Authorize;

namespace GeoUserManager
{
    public partial class frmUserGroupQuery : DevComponents.DotNetBar.Office2007Form
    {
        public string GroupName = "";
        public frmUserGroupQuery()
        {
            InitializeComponent();
            comboBoxGroupType.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            if (comboBoxGroupType.SelectedItem.ToString() != "")
            {
                GroupName = comboBoxGroupType.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}