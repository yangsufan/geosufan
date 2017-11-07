using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoUserManager
{
    public partial class frmEditPassword : DevComponents.DotNetBar.Office2007Form
    {
        public frmEditPassword()
        {
            InitializeComponent();
        }

        //pl
        SysCommon.Gis.SysGisDataSet m_pGisDb;
        public SysCommon.Gis.SysGisDataSet GisDB
        {
            set { m_pGisDb = value; }
        }
        //
        string m_intUserID;
        public string UserID
        {
            set { m_intUserID = value; }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_pGisDb == null) return;

            Exception Err;
            if (this.txtOldSec.Text.Trim() == "" || this.txtNewSec.Text.Trim()=="" || this.txtNewSec2.Text=="")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","新旧密码都不能为空。");
                return;
            }
            if (this.txtNewSec2.Text.Trim() != this.txtNewSec.Text.Trim())
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "两次输入的新密码不一致。");
                return;
            }
            
            //开始修改密码
            SysCommon.Gis.SysGisTable vGisTb = new SysCommon.Gis.SysGisTable(m_pGisDb);

            string strOldPass = vGisTb.GetFieldValue("USER_INFO", "UPWD", "USERID='" + m_intUserID.ToString()+"'", out Err).ToString();
            if (SysCommon.Authorize.AuthorizeClass.ComputerSecurity(this.txtOldSec.Text.Trim()) != strOldPass)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "输入的旧密码不正确。");
                return;
            }

            Dictionary<string, object> dicvalue = new Dictionary<string, object>();
            dicvalue.Add("UPWD", SysCommon.Authorize.AuthorizeClass.ComputerSecurity(this.txtNewSec.Text.Trim()));
            if (vGisTb.UpdateRow("USER_INFO", "USERID='" + m_intUserID.ToString()+"'", dicvalue, out Err))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改密码成功。");
                this.Close();
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统不能正确的修改密码。");
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}