using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GDBM
{
    public partial class frmSelectLogin :Fan.Common.BaseForm
    {
        public frmSelectLogin()
        {
            InitializeComponent();
        }

        private void btnDataManage_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            
        }

        private void btnUpdata_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            
        }

        private void btnDBIntegration_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            

        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnDataManage_MouseEnter(object sender, EventArgs e)
        {
            btnDataManage.Image = global::GDBM.Properties.Resources.btn4;
        }

        private void btnDataManage_MouseLeave(object sender, EventArgs e)
        {
            btnDataManage.Image = global::GDBM.Properties.Resources.btn1;
        }

        private void btnDBIntegration_MouseEnter(object sender, EventArgs e)
        {
            btnDBIntegration.Image = global::GDBM.Properties.Resources.btn5;
        }

        private void btnDBIntegration_MouseLeave(object sender, EventArgs e)
        {
            btnDBIntegration.Image = global::GDBM.Properties.Resources.btn2;
        }

        private void btnUpdata_MouseEnter(object sender, EventArgs e)
        {
            btnUpdata.Image = global::GDBM.Properties.Resources.btn6;
        }

        private void btnUpdata_MouseLeave(object sender, EventArgs e)
        {
            btnUpdata.Image = global::GDBM.Properties.Resources.btn3;
        }

        private void frmSelectLogin_Load(object sender, EventArgs e)
        {
            double oldLeft = btnDataManage.Left;
            double oldWidth = btnDataManage.Width;
            btnDataManage.Width = global::GDBM.Properties.Resources.btn1.Width;
            int newLeft =(int)( oldLeft + oldWidth / 2 - btnDataManage.Width / 2);
            btnDataManage.Left = newLeft;

            oldLeft = btnUpdata.Left;
            oldWidth = btnUpdata.Width;
            btnUpdata.Width = global::GDBM.Properties.Resources.btn3.Width;
            newLeft = (int)(oldLeft + oldWidth / 2 - btnUpdata.Width / 2);
            btnUpdata.Left = newLeft;

            oldLeft = btnDBIntegration.Left;
            oldWidth = btnDBIntegration.Width;
            btnDBIntegration.Width = global::GDBM.Properties.Resources.btn2.Width;
            newLeft = (int)(oldLeft + oldWidth / 2 - btnDBIntegration.Width / 2);
            btnDBIntegration.Left = newLeft;
        }

    }
}
