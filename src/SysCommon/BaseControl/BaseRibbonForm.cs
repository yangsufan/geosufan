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
       /// <summary>
       /// 获取系统配置信息
       /// </summary>
       /// <returns></returns>
        public virtual SysConfig GetMainSysConfig()
        {
            return null;
        }
        /// <summary>
        /// 设置登录标注
        /// </summary>
        /// <param name="strBarText"></param>
        public virtual void SetLonginBarText(string strBarText)
        {
            if (!string.IsNullOrEmpty(strBarText))
            {
                this.ItemLoginUser.Caption = strBarText;
            }
        }
    }
}