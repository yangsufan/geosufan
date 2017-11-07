using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoCustomExport
{
    public partial class frmWait:DevComponents.DotNetBar.Office2007Form
    {
        //提示信息
        public string TipText
        {
            set { this.labText.Text = value; }
            get { return this.labText.Text; }
        }

        public frmWait(string text)
        {
            InitializeComponent();
            TipText = text;
            this.TopMost = true;
        }

        /// <summary>
        /// 设置提示信息
        /// </summary>
        /// <param name="text"></param>
        public void SetText(string text)
        {
            TipText = text;
            this.Refresh();
        }
    }
}