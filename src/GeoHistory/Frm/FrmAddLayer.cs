using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;

namespace GeoHistory
{
    public partial class FrmAddLayer : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.AdvTree _ProjectTree = null;
        private IFeatureClass _OldFeatureClass = null;
        private IFeatureClass _NewFeatureClass = null;
        public IFeatureClass NewFeatureClass
        {
            get { return _NewFeatureClass; }
        }
        public IFeatureClass OldFeatureClass
        {
            get { return _OldFeatureClass; }
        }
        public FrmAddLayer(DevComponents.AdvTree.AdvTree pProjectTree)
        {
            _ProjectTree = pProjectTree;
            InitializeComponent();
            txtBoxOldLayer.Text = "点击选择历史图层";
            txtBoxNewLayer.Text = "点击选择现状图层";
        }

        private void txtBoxOldLayer_Click(object sender, EventArgs e)
        {
            this.advTreeOldLayer.Width = this.txtBoxOldLayer.Width;
            this.advTreeOldLayer.Visible = true;
            this.advTreeOldLayer.Focus();
        }

        private void txtBoxNewLayer_Click(object sender, EventArgs e)
        {
            this.advTreeNewLayer.Width = this.txtBoxNewLayer.Width;
            this.advTreeNewLayer.Visible = true;
            this.advTreeNewLayer.Focus();
        }
        private void DealSelectOldNode()
        {
            this.txtBoxOldLayer.Text = "";
            if (this.advTreeOldLayer.SelectedNode == null)
            {
                return;
            }
            if (this.advTreeOldLayer.SelectedNode.DataKeyString != "FC")
            {
                return;
            }
            DevComponents.AdvTree.Node DBNode = null; //数据库树节点
            //获取数据库节点
            DBNode = advTreeOldLayer.SelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }

            //获取数据集节点
            DevComponents.AdvTree.Node DtSetNode = null;
            DtSetNode = advTreeOldLayer.SelectedNode;
            while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "FD")
            {
                DtSetNode = DtSetNode.Parent;
            }

            XmlElement elementTemp = (DBNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;
            IWorkspace TempWorkSpace =GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            IFeatureClass pFeatureClass = null;
            if (TempWorkSpace != null)
            {
                pFeatureClass = (TempWorkSpace as IFeatureWorkspace).OpenFeatureClass(advTreeOldLayer.SelectedNode.Name );
            }
            if (pFeatureClass != null)
            {
                _OldFeatureClass  = pFeatureClass;
                this.txtBoxOldLayer.Text = pFeatureClass.AliasName;
            }
            this.advTreeOldLayer.Visible = false;
        }
        private void DealSelectNewNode()
        {
            this.txtBoxNewLayer.Text = "";
            if (this.advTreeNewLayer.SelectedNode == null)
            {
                return;
            }
            if (this.advTreeNewLayer.SelectedNode.DataKeyString != "FC")
            {
                return;
            }
            DevComponents.AdvTree.Node DBNode = null; //数据库树节点
            //获取数据库节点
            DBNode = advTreeNewLayer.SelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }

            //获取数据集节点
            DevComponents.AdvTree.Node DtSetNode = null;
            DtSetNode = advTreeNewLayer.SelectedNode;
            while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "FD")
            {
                DtSetNode = DtSetNode.Parent;
            }

            XmlElement elementTemp = (DBNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;
            IWorkspace TempWorkSpace = GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            IFeatureClass pFeatureClass = null;
            if (TempWorkSpace != null)
            {
                pFeatureClass = (TempWorkSpace as IFeatureWorkspace).OpenFeatureClass(advTreeNewLayer.SelectedNode.Text);
            }
            if (pFeatureClass != null)
            {
                _NewFeatureClass = pFeatureClass;
                this.txtBoxNewLayer.Text = pFeatureClass.AliasName;
            }
            this.advTreeNewLayer.Visible = false;
        }
        private void advTreeOldLayer_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DealSelectOldNode();
        }

        private void advTreeNewLayer_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            DealSelectNewNode();
        }
        //根据XML节点获取连接信息
        private object GetDBInfoByXMLNode(XmlElement dbElement, string strPath)
        {
            try
            {
                string strType = dbElement.GetAttribute("类型");
                string strServer = dbElement.GetAttribute("服务器");
                string strInstance = dbElement.GetAttribute("服务名");
                string strDB = dbElement.GetAttribute("数据库");
                if (strPath != "")
                {
                    strDB = strPath + strDB;
                }
                string strUser = dbElement.GetAttribute("用户");
                string strPassword = dbElement.GetAttribute("密码");
                string strVersion = dbElement.GetAttribute("版本");

                IPropertySet pPropSet = null;
                switch (strType.Trim().ToLower())
                {
                    case "pdb":
                        pPropSet = new PropertySetClass();
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        if (!File.Exists(strDB))
                        {
                            FileInfo filePDB = new FileInfo(strDB);
                            pAccessFact.Create(filePDB.DirectoryName, filePDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace pdbWorkspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        return pdbWorkspace;

                    case "gdb":
                        pPropSet = new PropertySetClass();
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        if (!Directory.Exists(strDB))
                        {
                            DirectoryInfo dirGDB = new DirectoryInfo(strDB);
                            pFileGDBFact.Create(dirGDB.Parent.FullName, dirGDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace gdbWorkspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        return gdbWorkspace;

                    case "sde":
                        pPropSet = new PropertySetClass();
                        IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                        pPropSet.SetProperty("SERVER", strServer);
                        pPropSet.SetProperty("INSTANCE", strInstance);
                        pPropSet.SetProperty("DATABASE", strDB);
                        pPropSet.SetProperty("USER", strUser);
                        pPropSet.SetProperty("PASSWORD", strPassword);
                        pPropSet.SetProperty("VERSION", strVersion);
                        IWorkspace sdeWorkspace = pSdeFact.Open(pPropSet, 0);
                        pSdeFact = null;
                        return sdeWorkspace;

                    case "access":
                        System.Data.Common.DbConnection dbCon = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDB);
                        dbCon.Open();
                        return dbCon;

                    //case "oracle":
                    //    string strOracle = "Data Source=" + strDB + ";Persist Security Info=True;User ID=" + strUser + ";Password=" + strPassword + ";Unicode=True";
                    //    System.Data.Common.DbConnection dbConoracle = new OracleConnection(strOracle);
                    //    dbConoracle.Open();
                    //    return dbConoracle;

                    //case "sql":
                    //    string strSql = "Data Source=" + strDB + ";Initial Catalog=" + strInstance + ";User ID=" + strUser + ";Password=" + strPassword;
                    //    System.Data.Common.DbConnection dbConsql = new SqlConnection(strSql);
                    //    dbConsql.Open();
                    //    return dbConsql;

                    default:
                        break;
                }

                return null;
            }
            catch (Exception e)
            {
                //*******************************************************************
                //guozheng added
                //if (ModData.SysLog != null)
                //{
                //    ModData.SysLog.Write(e, null, DateTime.Now);
                //}
                //else
                //{
                //    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                //    ModData.SysLog.Write(e, null, DateTime.Now);
                //}
                //********************************************************************
                return null;
            }
        }

        private void txtBoxOldLayer_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBoxNewLayer_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmAddLayer_Load(object sender, EventArgs e)
        {
            InitProjectTree();
        }
        private void InitProjectTree()
        {
            this.advTreeOldLayer.Nodes.Clear();
            this.advTreeNewLayer.Nodes.Clear();
            for (int i = 0; i < _ProjectTree.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pNode = _ProjectTree.Nodes[i];
                DevComponents.AdvTree.Node pCopyNode =pNode.DeepCopy();
                this.advTreeNewLayer.Nodes.Add(pCopyNode);
                DevComponents.AdvTree.Node pCopyOldNode = pNode.DeepCopy();
                this.advTreeOldLayer.Nodes.Add(pCopyOldNode);
            }
            ClearFromHisTree(advTreeOldLayer);
            ClearFromCurTree(advTreeNewLayer);

        }
        private void ClearFromHisTree(DevComponents.AdvTree.AdvTree pTree)
        {
            for (int i = pTree.Nodes.Count - 1; i >= 0; i--)
            {
                DevComponents.AdvTree.Node pNode = pTree.Nodes[i];
                if (pNode.DataKeyString == "DB" && pNode.Text != "历史库")
                {
                    pNode.Remove();
                }
                else if (pNode.Nodes.Count > 0 && pNode.DataKeyString != "Layer")
                {
                    ClearFromHisNode(pNode);
                }
            }
        }
        private void ClearFromHisNode(DevComponents.AdvTree.Node pNode)
        {
            for (int i = pNode.Nodes.Count - 1; i >= 0; i--)
            {
                DevComponents.AdvTree.Node pTmpNode = pNode.Nodes[i];
                if (pTmpNode.DataKeyString == "DB" && pTmpNode.Text != "历史库")
                {
                    pTmpNode.Remove();
                }
                else if (pTmpNode.Nodes.Count > 0 && pTmpNode.DataKeyString != "Layer")
                {
                    ClearFromHisNode(pTmpNode);
                }
            }
        }
        private void ClearFromCurTree(DevComponents.AdvTree.AdvTree pTree)
        {
            for (int i = pTree.Nodes.Count - 1; i >= 0; i--)
            {
                DevComponents.AdvTree.Node pNode = pTree.Nodes[i];
                if (pNode.DataKeyString == "DB" && pNode.Text != "现势库")
                {
                    pNode.Remove();
                }
                else if (pNode.Nodes.Count > 0 && pNode.DataKeyString != "Layer")
                {
                    ClearFromCurNode(pNode);
                }
            }
        }
        private void ClearFromCurNode(DevComponents.AdvTree.Node pNode)
        {
            for (int i = pNode.Nodes.Count-1; i >=0; i--)
            {
                DevComponents.AdvTree.Node pTmpNode = pNode.Nodes[i];
                if (pTmpNode.DataKeyString == "DB" && pTmpNode.Text != "现势库")
                {
                    pTmpNode.Remove();
                }
                else if (pTmpNode.Nodes.Count > 0 && pTmpNode.DataKeyString != "Layer")
                {
                    ClearFromCurNode(pTmpNode);
                }
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_OldFeatureClass == null)
            {
                MessageBox.Show("请选择要加载的历史图层!");
                return;
            }
            if (_NewFeatureClass == null)
            {
                MessageBox.Show("请选择要加载的现状图层!");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
