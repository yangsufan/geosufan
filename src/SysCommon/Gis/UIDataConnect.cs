using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
namespace SysCommon.Gis
{
    public partial class UIDataConnect : BaseControl
    {
        private string _Addtag = "Old";
        public string Addtag
        {
            get { return _Addtag; }
        }
        public UIDataConnect()
        {
            InitializeComponent();
        }
        private string m_strTitle = "数据连接";
        public string strTitle
        {
            get { return m_strTitle; }
            set { m_strTitle = value; }
        }
        public void BindAtt()
        {
            this.groupPanel.Text = m_strTitle;
        }
        private IWorkspace m_pWks=null;
        private string m_strDatabaseType;
        public string DatabaseType
        {
            get { return m_strDatabaseType; }
            set { m_strDatabaseType = value; }
        }
        private string  m_Server="";
        public string Server
        {
            get { return this.txtServer.Text.Trim(); }
            set { m_Server = value; }
        }
        private string m_Service = "";
        public string Service
        {
            get { return this.txtService.Text; }
            set { m_Service = value; }
        }
        private string m_DataBase = "";
        public string DataBase
        {
            get { return this.txtDataBase.Text; }
            set { m_DataBase = value; }
        }
        private string m_User = "";
        public string User
        {
            get { return this.txtUser.Text; }
            set { m_User = value; }
        }
        private string m_Password = "";
        public string Password
        {
            get { return this.txtPassWord.Text; }
            set { m_Password = value; }
        }
        private string m_Version = "";
        public string Version
        {
            get { return this.cboVersion.Text; }
            set { m_Version = value; }
        }
        public IWorkspace GetWks()
        {
            m_pWks = GetWksBySetting();
            return m_pWks;
        }
        public void IntiForm()
        {
            this.cboDataType.Properties.Items.Clear();
            this.cboDataType.Properties.Items.Add("ArcSDE(For Oracle)");
            this.cboDataType.Properties.Items.Add("ArcSDE(For MicorSoft SQLServer)");
            this.cboDataType.Properties.Items.Add("ESRI文件数据库(File Geodatabase)");
            this.cboDataType.Properties.Items.Add("ESRI个人数据库(Personal Geodatabase)");

            this.groupPanel.Text = m_strTitle;

            if (m_strDatabaseType == "ORACLE")
            {
                this.cboDataType.SelectedIndex = 0;
            }
            else if (m_strDatabaseType == "SQLSERVER")
            {
                this.cboDataType.SelectedIndex = 1;
            }
            else if (m_strDatabaseType == "FILE")
            {
                this.cboDataType.SelectedIndex = 2;
            }
            else if (m_strDatabaseType == "ACCESS")
            {
                this.cboDataType.SelectedIndex = 3;
            }
            else
            {
                this.cboDataType.SelectedIndex = 0;
            }

            this.txtServer.Text = m_Server;
            this.txtService.Text = m_Service;
            this.txtUser.Text = m_User;
            this.txtPassWord.Text = m_Password;
            this.txtDataBase.Text = m_DataBase;
            this.cboVersion.Text = m_Version;
        }
        private IWorkspace GetWksBySetting()
        {
            if(this.cboDataType.Tag==null) return null;
            string strType=this.cboDataType.Tag.ToString();

            IWorkspace pTempWks = null;
            Exception err=null;
            errorServer.Clear();
            errorService.Clear();
            errorUser.Clear();
            errorPassWord.Clear();
            errorVersion.Clear();
            errorDataBase.Clear();
            try
            {
                if (strType == "ARCSDE")
                {
                    if (txtServer.Text == "")
                    {
                        errorServer.SetError(btnNew, "必填！");
                    }
                    if (txtService.Text == "")
                    {
                        errorService.SetError(txtService, "必填！");
                    }
                    if (txtUser.Text == "")
                    {
                        errorUser.SetError(txtUser, "必填！");
                    }
                    if (txtPassWord.Text == "")
                    {
                        errorPassWord.SetError(txtPassWord, "必填！");
                    }
                    if (cboVersion.Text == "")
                    {
                        errorVersion.SetError(cboVersion, "必填！");
                    }
                    if (txtDataBase.Text == "")
                    {
                        errorDataBase.SetError(txtDataBase, "必填！");
                        return pTempWks;
                    }
                    pTempWks = SetWorkspace(this.txtServer.Text, this.txtService.Text, this.txtDataBase.Text, this.txtUser.Text, this.txtPassWord.Text, this.cboVersion.Text, out err);
                }
                else if (strType == "FGDB")
                {
                    if (txtServer.Text == "")
                    {
                        errorServer.SetError(btnNew, "必填！");
                        return pTempWks;
                    }
                    if (Directory.Exists(this.txtServer.Text))
                    {
                        pTempWks = SetWorkspace(this.txtServer.Text, WksType.FGDB, out err);
                    }
                    else
                    {
                        if (_Addtag == "New")
                        {
                            string path = txtServer.Text.Substring(0, txtServer.Text.LastIndexOf("\\") + 1);
                            string name = txtServer.Text.Substring(txtServer.Text.LastIndexOf("\\") + 1);
                            IWorkspaceFactory pworkspacefactory = new FileGDBWorkspaceFactoryClass();
                            IWorkspaceName workspaceName = pworkspacefactory.Create(path, name, null, 0);
                            ESRI.ArcGIS.esriSystem.IName pname = (ESRI.ArcGIS.esriSystem.IName)workspaceName;

                            pTempWks = SetWorkspace(this.txtServer.Text, WksType.FGDB, out err);
                        }
                    }
                }
                else if (strType == "PGDB")
                {
                    if (txtServer.Text == "")
                    {
                        errorServer.SetError(btnNew, "必填！");
                        return pTempWks;
                    }
                    if (File.Exists(this.txtServer.Text))
                    {
                        pTempWks = SetWorkspace(this.txtServer.Text, WksType.PGDB, out err);
                    }
                    else
                    {
                        if (_Addtag == "New")  
                        {
                            string path = txtServer.Text.Substring(0, txtServer.Text.LastIndexOf("\\") + 1);
                            string name = txtServer.Text.Substring(txtServer.Text.LastIndexOf("\\") + 1);
                            if (!Directory.Exists(path))
                            {
                                return null;
                            }
                            IWorkspaceFactory pworkspacefactory = new AccessWorkspaceFactoryClass();
                            IWorkspaceName workspaceName = pworkspacefactory.Create(path, name, null, 0);
                            ESRI.ArcGIS.esriSystem.IName pname = (ESRI.ArcGIS.esriSystem.IName)workspaceName;

                            pTempWks = SetWorkspace(this.txtServer.Text, WksType.PGDB, out err);
                        }
                    }
                }

                if (pTempWks == null && err!=null )
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "无法连接数据库：" + this.txtServer.Text + err.Message);
                    return null;
                }

            }
            catch (Exception  ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "无法连接数据库：" + this.txtServer.Text + " " + ex.Message);
            }

            return pTempWks;
        }
        enum WksType
        {
            ArcSDE,
            PGDB,
            FGDB
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
        private IWorkspace SetWorkspace(string sServer, string sService, string sDatabase, string sUser, string sPassword, string strVersion, out Exception eError)
        {
            eError = null;
            IWorkspace pWks = null;
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
                pWks = pSdeFact.Open(pPropSet, 0);
                pPropSet = null;
                pSdeFact = null;
                return pWks;
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }
        /// <summary>
        /// 设置PDB、GDB工作区
        /// </summary>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="wstype">工作区类型</param>
        /// <returns>输出错误Exception</returns>
        private IWorkspace SetWorkspace(string sFilePath, WksType wstype, out Exception eError)
        {
            eError = null;
            IWorkspace pWks = null;

            try
            {
                IPropertySet pPropSet = new PropertySetClass();
                switch (wstype)
                {
                    case WksType.PGDB:
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWks =pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        break;
                    case WksType.FGDB:
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWks = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        break;
                    case WksType.ArcSDE:
                        break;
                }
                pPropSet = null;
                return pWks;
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }
        public void GetVersionInfo(IWorkspace workspace)    
        {
            if (workspace.Type != esriWorkspaceType.esriRemoteDatabaseWorkspace) return;

            this.cboVersion.Properties.Items.Clear();
            IVersionedWorkspace versionedWorkspace = (IVersionedWorkspace)workspace;     
            IEnumVersionInfo enumVersionInfo = versionedWorkspace.Versions;        
            enumVersionInfo.Reset();        
            IVersionInfo version = (IVersionInfo)enumVersionInfo.Next();
            if (version != null)
            {
                this.cboVersion.Properties.Items.Clear();
            }
            while (version != null)        
            {
                this.cboVersion.Properties.Items.Add(version.VersionName);
                version = (IVersionInfo)enumVersionInfo.Next();       
            }    
        }
        private void cboDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDataType.Text == "")
            {
                this.cboDataType.Tag = null;
                return;
            }
            errorServer.Clear();
            errorService.Clear();
            errorUser.Clear();
            errorPassWord.Clear();
            errorVersion.Clear();
            errorDataBase.Clear();
            if (this.cboDataType.Text == "ArcSDE(For Oracle)")
            {
                this.cboDataType.Tag = "ARCSDE";
                this.txtDataBase.Enabled = true;
                this.btnServer.Enabled = false;
                this.btnNew.Enabled = false;    

                this.txtService.Enabled = true;
                this.txtUser.Enabled = true;
                this.txtPassWord.Enabled = true;
                this.cboVersion.Enabled = true;

                m_strDatabaseType = "ORACLE";
            }
            else if (this.cboDataType.Text == "ArcSDE空间数据库(For MicorSoft SQLServer)")
            {
                this.cboDataType.Tag = "ARCSDE";
                this.btnServer.Enabled = false;
                this.btnNew.Enabled = false;

                this.txtDataBase.Enabled = true;
                this.txtService.Enabled = true;
                this.txtUser.Enabled = true;
                this.txtPassWord.Enabled = true;
                this.cboVersion.Enabled = true;

                m_strDatabaseType = "SQLSERVER";
            }
            else if (this.cboDataType.Text == "ESRI文件数据库(File Geodatabase)")
            {
                this.cboDataType.Tag = "FGDB";

                this.btnServer.Enabled = true;
                this.btnNew.Enabled = true;

                this.txtDataBase.Enabled = false;
                this.txtService.Enabled = false;
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.cboVersion.Enabled = false;

                m_strDatabaseType = "FILE";
            }
            else if (this.cboDataType.Text == "ESRI个人数据库(Personal Geodatabase)")
            {
                this.cboDataType.Tag = "PGDB";
                this.btnServer.Enabled = true;
                this.btnNew.Enabled = true;

                this.txtDataBase.Enabled = false;
                this.txtService.Enabled = false;
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.cboVersion.Enabled = false;

                m_strDatabaseType = "ACCESS";
            }
        }
        private void buttonXTest_Click(object sender, EventArgs e)
        {
            if (m_pWks == null)
            {
                m_pWks = GetWksBySetting();
            }

            if (m_pWks == null) return;

            GetVersionInfo(m_pWks);
            if (m_pWks != null)
            {
                MessageBox.Show("连接成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            GetWks();
            if (m_pWks != null)
            {
                MessageBox.Show("连接成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnServer_Click(object sender, EventArgs e)
        {
            if (this.cboDataType.Tag == null) return;
            if (this.cboDataType.Tag.ToString() == "PGDB")
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.FileName = "";
                dlg.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == DialogResult.Cancel) return;

                this.txtServer.Text = dlg.FileName;
            }
            else if (this.cboDataType.Tag.ToString() == "FGDB")
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "打开ESRI文件数据库(File Geodatabase)";
                if (fd.ShowDialog() == DialogResult.Cancel) return;

                string strFileName = fd.SelectedPath;
                if (strFileName.Substring(strFileName.Length - 3).ToUpper() != "GDB")
                {
                    MessageBox.Show("选择的不是ESRI文件数据库(File Geodatabase)", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.txtServer.Text = strFileName;

            }
            this.buttonXTest.Enabled = true;
            _Addtag = "Old";
        }
        private void UIDataConnect_Load(object sender, EventArgs e)
        {
            IntiForm();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            if (this.cboDataType.Tag == null) return;
            if (this.cboDataType.Tag.ToString() == "PGDB")
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "";
                dlg.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                if (dlg.ShowDialog() == DialogResult.Cancel) return;

                this.txtServer.Text = dlg.FileName;
            }
            else if (this.cboDataType.Tag.ToString() == "FGDB")
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.FileName = "";
                dlg.Filter = "ESRI文件数据库(.gdb)|*.gdb";
                if (dlg.ShowDialog() == DialogResult.Cancel) return;

                this.txtServer.Text = dlg.FileName;

            }
            this.buttonXTest.Enabled=false;
            _Addtag = "New";
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (m_pWks == null)
            {
                m_pWks = GetWksBySetting();
            }

            if (m_pWks == null) return;
            IWorkspace pWks = m_pWks;
        }
    }
}
