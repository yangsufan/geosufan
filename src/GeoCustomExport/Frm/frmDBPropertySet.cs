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

namespace GeoCustomExport
{
    public partial class frmDBPropertySet : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace m_Workspace = null;
        public IWorkspace Workspace
        {
            get { return m_Workspace; }
        }
        private string m_connset = "";
        public string ConnSet
        {
            get { return m_connset; }
        }
        public frmDBPropertySet()
        {
            InitializeComponent();
        }
        public frmDBPropertySet(string frmName)
        {
            InitializeComponent();
            InitialFrm(frmName);
        }
        public frmDBPropertySet(string frmName, string connet)
        {
            InitializeComponent();
            InitialFrm(frmName);
            if (connet==null||connet=="") return;
            string[] arr = connet.Split(';');
            switch (arr[0])
            {
                case "SDE":
                    string[] conn = arr[1].Split('|');
                    comBoxType.SelectedIndex = 0;
                    txtServer.Text = conn[0];
                    txtInstance.Text = conn[1];
                    txtDB.Text = conn[2];
                    txtUser.Text = conn[3];
                    txtPassword.Text = conn[4];
                    txtVersion.Text = conn[5];
                    break;
                case "GDB":
                    comBoxType.SelectedIndex = 1;
                    txtDB.Text = arr[1];
                    break;
                case "PDB":
                    comBoxType.SelectedIndex = 2;
                    txtDB.Text = arr[1];
                    break;

            }
        }

        //public frmDBPropertySet(string frmName,string dbType)
        //{
        //    InitializeComponent();
        //    InitialFrm(frmName);

        //    switch (dbType)
        //    {
        //         case "ArcSDE(For Oracle)":
        //            comBoxType.SelectedIndex = 1;
        //            break;
        //         case "ESRI个人数据库(*.mdb)":
        //            comBoxType.SelectedIndex = 2;
        //            break;
        //        case "ESRI文件数据库(*.gdb)":
        //            comBoxType.SelectedIndex = 0;
        //            break;

        //    }
        //}

        private void InitialFrm(string frmName)
        {
            //cyf 20110628
            btnOK.Enabled = false;
            this.Text = frmName;
            comBoxType.SelectedIndex = 0;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Exception err=null;
            switch (comBoxType.Text)
            {
                case "ESRI文件数据库(*.gdb)":
                    SetWorkspace(txtDB.Text, "GDB", out err);
                    break;
                case "ArcSDE(For Oracle)":
                   SetWorkspace(txtServer.Text, txtInstance.Text, txtDB.Text, txtUser.Text, txtPassword.Text, txtVersion.Text, out err);
                    break;
                case "ESRI个人数据库(*.mdb)":
                    SetWorkspace(txtDB.Text,"PDB", out err);
                    break;
            }
            
            if (err != null)
            {
                MessageBox.Show("连接数据库失败:" + err.Message,"提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 
            if (comBoxType.Text == "ESRI个人数据库(*.mdb)")
            {
                comBoxType.Tag = "PDB";
            }
            else if (comBoxType.Text == "ESRI文件数据库(*.gdb)")
            {
                comBoxType.Tag = "GDB";
            }
            else if (comBoxType.Text == "ArcSDE(For Oracle)")
            {
                comBoxType.Tag = "SDE";
            }
            //end
            if (comBoxType.Text != "ArcSDE(For Oracle)")
            {
                btnDB.Visible = true;
                txtDB.Size = new Size(txtUser.Size.Width - btnDB.Size.Width, txtServer.Size.Height);
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtServer.Enabled = false;
            }
            else
            {
                btnDB.Visible = false;
                txtDB.Size = new Size(txtUser.Size.Width, txtServer.Size.Height);
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtServer.Enabled = true;
            }
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "ArcSDE(For Oracle)":

                    break;

                case "ESRI文件数据库(*.gdb)":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            txtDB.Text = pFolderBrowser.SelectedPath;
                        }
                        else
                        {
                            MessageBox.Show("请选择GDB格式文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;

                case "ESRI个人数据库(*.mdb)":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDB.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

       
        /// <summary>
        /// 设置PDB、GDB工作区
        /// </summary>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="wstype">工作区类型</param>
        /// <returns>输出错误Exception</returns>
        public bool SetWorkspace(string sFilePath,string type, out Exception eError)
        {
            eError = null;

            try
            {
                IPropertySet pPropSet = new PropertySetClass();
                switch (type)
                {
                    case "PDB":
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        m_Workspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        m_connset = "PDB;" + sFilePath;
                        break;
                    case "GDB":
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        m_Workspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        m_connset = "GDB;" + sFilePath;
                        break;
                }
                
                pPropSet = null;
                return true;
            }
            catch (Exception eX)
            {
                return false;
            }
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
                m_Workspace = pSdeFact.Open(pPropSet, 0);
                pPropSet = null;
                pSdeFact = null;
                m_connset = "SDE;" + sServer + "|" + sService + "|" + sDatabase + "|" + sUser + "|" + sPassword + "|" + strVersion;
                return true;
            }
            catch (Exception eX)
            {
             
                return false;
            }
        }


    }
}