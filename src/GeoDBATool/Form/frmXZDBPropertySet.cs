using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoDBATool
{
    public partial class frmXZDBPropertySet : DevComponents.DotNetBar.Office2007Form
    {
        private bool m_Res;
        public bool Res
        {
            get
            {
                return m_Res;
            }
        }

        private string m_DbType;
        public string DBType
        {
            get { return m_DbType; }
        }

        private string m_PropertySet;
        public string GetPropertySetStr
        {
            get
            {
                return m_PropertySet;
            }
            set
            {
                m_PropertySet = value;
            }
        }

        public  IWorkspace m_pworkspace;
        public frmXZDBPropertySet()
        {
            InitializeComponent();
            btnOK.Enabled = false;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Exception err = null;
            if (txtServer.Text.Trim() == "")
            {
                MessageBox.Show("服务器名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtInstance.Text.Trim() == "")
            {
                MessageBox.Show("服务端口不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtDB.Text.Trim() == "")
            {
                MessageBox.Show("数据库名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtUser.Text.Trim() == "")
            {
                MessageBox.Show("用户名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("密码不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtVersion.Text.Trim() == "")
            {
                MessageBox.Show("版本号不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SetWorkspace(txtServer.Text, txtInstance.Text, txtDB.Text, txtUser.Text, txtPassword.Text, txtVersion.Text, out err);

            if (err != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败!详细信息:"+err.Message);
                return;
            }
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","连接成功");
            btnOK.Enabled = true;
        }


        /// <summary>
        /// 设置SDE工作区
        /// </summary>
        /// <param name="sServer">服务器名</param>
        /// <param name="sService">服务名</param>
        /// <param name="sDatabase">数据库名(SQLServer)</param>
        /// <param name="sUser">用户名</param>
        /// <param name="sPassword">密码</param>
        /// <param name="strVersion">SDE版本</param>
        /// <returns>输出错误Exception</returns>
        public bool SetWorkspace(string sServer, string sService, string sDatabase, string sUser, string sPassword, string strVersion, out Exception eError)
        {
            eError = null;
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);

            try
            {
                m_pworkspace = pSdeFact.Open(pPropSet, 0);
                pPropSet = null;
                pSdeFact = null;
                return true;
            }
            catch (Exception eX)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eX);
                //********************************
                eError = eX;
                return false;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            m_Res = true;
            m_PropertySet = txtServer.Text.Trim() + "|" + txtInstance.Text.Trim() + "|" + txtDB.Text.Trim() + "|" + txtUser.Text.Trim() + "|" + txtPassword.Text.Trim() + "|" + txtVersion.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }




        private void frmDBPropertySet_Load(object sender, EventArgs e)
        {

            if (m_PropertySet!=null && m_PropertySet != "")
            {
                string[] array = m_PropertySet.Split("|".ToCharArray());
                this.txtServer.Text = array[0];

                this.txtInstance.Text = array[1];

                this.txtDB.Text = array[2];

                this.txtUser.Text = array[3];

                this.txtPassword.Text = array[4];

                this.txtVersion.Text = array[5];
            }


        }
    }
}