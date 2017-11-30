using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GDBM
{
    public partial class frmLoadProgress : SysCommon.BaseForm
    {
        public string SysInfo
        {
            get { return labelXInfo.Text; }
            set { labelXInfo.Text = value; }
        }

        public void RefreshLable()
        {
            this.labelXInfo.Refresh();
        }
        public frmLoadProgress()
        {
            InitializeComponent();

            try //zhangqi 2012-08-06
            {
                if (System.IO.File.Exists(Application.StartupPath + "\\..\\Res\\Pic\\系统初始化.jpg"))
                {
                    this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\..\\Res\\Pic\\系统初始化.jpg");
                }
                else
                {
                    this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\..\\Res\\Pic\\系统初始化.png");
                }
            }
            catch { }

        }
    }
}