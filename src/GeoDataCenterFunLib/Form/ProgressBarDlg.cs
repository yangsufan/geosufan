using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataCenterFunLib
{
    public partial class ProgressBarDlg : Form
    {
        public ProgressBarDlg()
        {
            InitializeComponent();
            myprogressBar.MaxValue = 100;
            myprogressBar.Value = myprogressBar.MinValue = 0;
            Mytimer.Interval = 50;
        }

        private void ProgressBarDlg_Load(object sender, EventArgs e)
        {
            this.Owner.Enabled =false;//设置主窗体不可用
            Mytimer.Enabled = true;
        }

        private void ProgressBarDlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mytimer.Enabled = false;
            this.Owner.Enabled = true;//设置主窗体可用
        }

        private void Mytimer_Tick(object sender, EventArgs e)
        {
            if (myprogressBar.Value < myprogressBar.MaxValue)
                myprogressBar.Value += 1;
            else if (myprogressBar.Value == myprogressBar.MaxValue)
                myprogressBar.Value = 0;
        }
    }
}