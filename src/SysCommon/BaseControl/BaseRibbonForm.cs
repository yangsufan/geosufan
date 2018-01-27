using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace Fan.Common
{
    public partial class BaseRibbonForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public BaseRibbonForm()
        {
            InitializeComponent();
        }
       // public delegate void Progress;
        public virtual SysConfig GetMainSysConfig()
        {
            return null;
        }
        public virtual void SetLonginBarText(string strBarText)
        {
            if (string.IsNullOrEmpty(strBarText))
            {
                this.ItemLoginUser.Caption = strBarText;
            }
        }
    }
}