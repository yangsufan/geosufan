using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;

namespace GeoDBIntegration
{
    /// <summary>
    /// 子系统启动界面  陈亚飞添加20100928
    /// </summary>
    public partial class parentMainForm : DevComponents.DotNetBar.Office2007RibbonForm
    {

        public parentMainForm(string xmlPath)
        {
            InitializeComponent();

            //ModuleData.v_MainForm = pForm;
            //ModuleData.v_SubForm = this;

            ModDBOperate.IntialSysFrm(xmlPath, this);
        }

        private void parentMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

    