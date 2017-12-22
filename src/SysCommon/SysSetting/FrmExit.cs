using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Fan.Common
{
    public partial class FrmExit :BaseForm
    {
        public FrmExit()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
