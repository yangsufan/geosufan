using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataCenterFunLib
{
    public partial class ProgressBarStatDlg : Form
    {
        public ProgressBarStatDlg()
        {
            InitializeComponent();
            myprogressBar.MaxValue = 100;
            myprogressBar.Value = myprogressBar.MinValue = 0;
            Mytimer.Interval = 50;
        }
        private string _value;

        public string Value
        {
            get{return _value;}
            set{_value=value;}
        }

        private void ProgressBarStatDlg_Load(object sender, EventArgs e)
        {
            this.Owner.Enabled = false;//设置主窗体不可用
            Mytimer.Enabled = true;
        }

        private void ProgressBarStatDlg_FormClosed(object sender, FormClosedEventArgs e)
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
            if (labelPro.Text != _value)
                labelPro.Text = _value;
        }
        
    }
}