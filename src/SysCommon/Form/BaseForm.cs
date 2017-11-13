using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
/*
 * 类名：基础窗体类
 * 日期：2017-11-10
 * 描述：所有基础窗体基类
 */
namespace SysCommon
{
    public partial class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        public BaseForm()
        {
            InitializeComponent();
            this.ShowIcon = false;
        }
    }
}
