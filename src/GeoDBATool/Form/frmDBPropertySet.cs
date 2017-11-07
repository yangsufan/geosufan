using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDBATool
{
    public partial class frmDBPropertySet : DevComponents.DotNetBar.Office2007Form
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
        }

        public frmDBPropertySet(string frmName)
        {
            InitializeComponent();
            InitialFrm(frmName);
        }

        public frmDBPropertySet(string frmName,string dbType)
        {
            InitializeComponent();
            InitialFrm(frmName);

            switch (dbType)
            {
                case "ESRI文件数据库(*.gdb)":
                    comBoxType.SelectedIndex = 0;
                    break;
                case "ArcSDE(For Oracle)":
                    comBoxType.SelectedIndex = 1;
                    break;
                case "ESRI个人数据库(*.mdb)":
                    comBoxType.SelectedIndex = 2;
                    break;
            }
        }

        private void InitialFrm(string frmName)
        {
            //cyf 20110628
            btnOK.Enabled = false;
            this.Text = frmName;
            object[] TagDBType = new object[] { "ESRI文件数据库(*.gdb)", "ArcSDE(For Oracle)", "ESRI个人数据库(*.mdb)" };//"GDB", "SDE", "PDB"
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Exception err=null;
            SysCommon.Gis.SysGisDB dbsys = new SysCommon.Gis.SysGisDB();
            switch (comBoxType.Text)
            {
                case "ESRI文件数据库(*.gdb)":
                    dbsys.SetWorkspace(txtDB.Text, SysCommon.enumWSType.GDB, out err);
                    break;
                case "ArcSDE(For Oracle)":
                    dbsys.SetWorkspace(txtServer.Text, txtInstance.Text, txtDB.Text, txtUser.Text, txtPassword.Text, txtVersion.Text, out err);
                    break;
                case "ESRI个人数据库(*.mdb)":
                    dbsys.SetWorkspace(txtDB.Text, SysCommon.enumWSType.PDB, out err);
                    break;
            }
            
            if (err != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("连接数据库失败", "原因:" + err.Message);
                return;
            }

            dbsys.Dispose();
            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_Res = true;
            m_DbType = comBoxType.Tag.ToString().Trim();//.Text;  //cyf 20110628
            m_PropertySet = txtServer.Text.Trim() + "|" + txtInstance.Text.Trim() + "|" + txtDB.Text.Trim() + "|" + txtUser.Text.Trim() + "|" + txtPassword.Text.Trim() + "|" + txtVersion.Text.Trim();
            this.Close();
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
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件!");
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

        private void frmDBPropertySet_Load(object sender, EventArgs e)
        {
            Plugin.Application.IAppGISRef Hook = ModData.v_AppGIS;

            DevComponents.AdvTree.Node pCurNode = Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点
            if (pCurNode == null) return;
            if (pCurNode.Parent == null) return;
            string pProjectname = pCurNode.Parent.Name;

            System.Xml.XmlNode Projectnode = Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            if (Projectnode == null) return;
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            if (ProjectNodeElement.SelectSingleNode(".//历史库/连接信息") == null) return;
            System.Xml.XmlElement ProjectHisDBConnEle = ProjectNodeElement.SelectSingleNode(".//历史库/连接信息") as System.Xml.XmlElement;
            string HisDBType = ProjectHisDBConnEle.GetAttribute("类型");
            comBoxType.Text = HisDBType;
            if (HisDBType == "ESRI个人数据库(*.mdb)")
            {

                string path = ProjectHisDBConnEle.GetAttribute("数据库");

                this.txtDB.Text = path;
                btnDB.Tooltip = path;
            }
            else if (HisDBType == "ESRI文件数据库(*.gdb)")
            {
                string path = ProjectHisDBConnEle.GetAttribute("数据库");

                this.txtDB.Text = path;
                btnDB.Tooltip = path;
            }
            else
            {
                this.txtServer.Text = ProjectHisDBConnEle.GetAttribute("服务器");

                this.txtInstance.Text = ProjectHisDBConnEle.GetAttribute("服务名");

                this.txtDB.Text = ProjectHisDBConnEle.GetAttribute("数据库");

                this.txtUser.Text = ProjectHisDBConnEle.GetAttribute("用户");

                this.txtPassword.Text = ProjectHisDBConnEle.GetAttribute("密码");

                this.txtVersion.Text = ProjectHisDBConnEle.GetAttribute("版本");
            } 

        }
    }
}