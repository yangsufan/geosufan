using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBATool
{
    public partial class FrmProcessBar : DevComponents.DotNetBar.Office2007Form
    {
        public FrmProcessBar(long max)
        {
            InitializeComponent();
            this.progressBar.Maximum =(int) max;
            Point point = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width)/2, (Screen.PrimaryScreen.WorkingArea.Height - this.Size.Height)/2);//窗体位置，居中显示加个1/2就行了
            this.StartPosition = FormStartPosition.Manual;//窗体其实位置类型，manual由location指定
            this.Location = point;
           // Application.DoEvents(); 
        }

        public FrmProcessBar()
        {
            InitializeComponent();         
            Point point = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Size.Height) / 2);//窗体位置，居中显示加个1/2就行了
            this.StartPosition = FormStartPosition.Manual;//窗体其实位置类型，manual由location指定
            this.Location = point;
        }
        public void SetFrmProcessBarValue(long value)
        {
            if (value <= this.progressBar.Maximum)
                this.progressBar.Value = (int)value;
           // Application.DoEvents(); 
        }

        public void SetFrmProcessBarText(string task)
        {
            this.Text = task;
           // Application.DoEvents(); 
        }

        public void SetFrmProcessBarMax(long max)
        {
            this.progressBar.Maximum = (int)max;
            //Application.DoEvents(); 
        }

        public void SetChild()/////设置为子窗体
        {
            Point point = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Size.Height) / 2 + this.Height);//窗体位置，居中显示加个1/2就行了
            this.StartPosition = FormStartPosition.Manual;//窗体其实位置类型，manual由location指定
            this.Location = point;
        }
    }
    
}