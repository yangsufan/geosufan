using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataChecker
{
    public partial class FrmLineLengthCheck : DevComponents.DotNetBar.Office2007Form
    {
        double pMaxValue = 0;
        double pMinValue = 0;
        public double MaxValue
        {
            get 
            {
                return pMaxValue;
            }
            set 
            {
                pMaxValue=value;
            }
        }

        public double MinValue
        {
            get 
            {
                return pMinValue;
            }
            set 
            {
                pMinValue=value;
            }
        }
        public FrmLineLengthCheck()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //判断文本框输入是否合理，是否字符串，最小值是否小于最大值
            if (txtMin.Text == "" && txtMax.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请填写最大值或最小值的范围！");
                return;
            }
            try
            {
                if (txtMin.Text != "" && txtMax.Text != "")
                {
                    if (Convert.ToDouble(txtMin.Text.Trim()) > Convert.ToDouble(txtMax.Text.Trim()))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "最小值不能大于最大值！");
                        return;
                    }
                }
                pMaxValue = Convert.ToDouble(txtMax.Text.Trim());
                pMinValue=Convert.ToDouble(txtMin.Text.Trim());
            }
            catch (Exception eex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "'最大值'或'最小值'应为数字，请输入有效的数字！");
                return;
            }
           this.DialogResult = DialogResult.OK; 
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}