using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
///zq 2011-1223 修改密码
namespace GeoDataManagerFrame
{
    public partial class frmModifyPassword :DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace m_Workspace;
        private Plugin.Application.IAppFormRef m_AppFormRef;
        public frmModifyPassword(Plugin.Application.IAppFormRef pAppFormRef, IWorkspace pWorkspace)
        {
            m_AppFormRef = pAppFormRef;
            m_Workspace = pWorkspace;
            InitializeComponent();
        }

        private void bttCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtOldPassword_TextChanged(object sender, EventArgs e)
        {
            errModifyPassword.Clear();
            if (txtNewPassword.Text != "" || txtPassword.Text != "")
            {
                txtPassword.Text = "";
                txtNewPassword.Text = "";
                bttCommit.Enabled = false;
            }
            txtNewPassword.Enabled = true;
            txtPassword.Enabled = false;
        }
        private void txtOldPassword_Leave(object sender, EventArgs e)
        {
            if (txtOldPassword.Text.Trim() == "") { return; }
            string strOldPassword = SysCommon.Authorize.AuthorizeClass.ComputerSecurity(txtOldPassword.Text.Trim());
            ///判断输入的密码是否与当前密码一致
            if ( strOldPassword!=m_AppFormRef.ConnUser.Password )
            {
                errModifyPassword.SetError(txtOldPassword, "输入密码错误");
                txtNewPassword.Enabled = false;
            }
            else 
            { 
                txtNewPassword.Enabled = true;
                txtNewPassword.Focus(); 
            }
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "")
            {
                txtPassword.Text = "";
                bttCommit.Enabled = false;
            }
            errModifyPassword.Clear();
            txtPassword.Enabled = true;
            bttCommit.Enabled = false;
        }
        private void txtNewPassword_Leave(object sender, EventArgs e)
        {
            if (txtNewPassword.Text.Trim() == "")
            {
                errModifyPassword.SetError(txtNewPassword, "密码不能空或空格");
                txtPassword.Enabled = false;
            }
            else
            {
                txtPassword.Enabled = true;
                txtPassword.Focus();
            }
        }
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            errModifyPassword.Clear();
            bttCommit.Enabled = true;
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            string strNewPassword = SysCommon.Authorize.AuthorizeClass.ComputerSecurity(txtNewPassword.Text.Trim());
            string strPassword = SysCommon.Authorize.AuthorizeClass.ComputerSecurity(txtPassword.Text.Trim());
            ///判断新密码输入的是否一致
            if (strNewPassword != strPassword)
            {
                errModifyPassword.SetError(txtPassword, "输入确认密码错误");
                txtPassword.Text = "";
                bttCommit.Enabled = false;
            }
            else
            {
                bttCommit.Enabled = true;
              
            }
        }

        private void bttCommit_Click(object sender, EventArgs e)
        {
            Exception exError = null;
            SysGisTable sysTable = new SysGisTable(m_Workspace);
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            string strPassword = SysCommon.Authorize.AuthorizeClass.ComputerSecurity(txtNewPassword.Text.Trim());
            //查找当前用户名是否存在
            if (sysTable.ExistData("USER_INFO", "NAME='" + m_AppFormRef.ConnUser.Name + "'"))
            {
                dicData.Add("UPWD", strPassword);
                if (sysTable.UpdateRow("USER_INFO", "NAME='" + m_AppFormRef.ConnUser.Name + "'", dicData, out exError))
                {
                    ///记录新的密码
                    m_AppFormRef.ConnUser.Password = strPassword;
                    MessageBox.Show("密码修改成功", "提示！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("密码修改失败","提示！");
                }
                
            }
            
        }


  

      
    }
}
