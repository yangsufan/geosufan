using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using System.Windows.Forms;
using System.IO;

namespace GeoDBATool
{
    /// <summary>
    /// 数据加载
    /// </summary>
    public class ControlsInitTmpDBStructure : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;
        private ControlsDBHistoryManage HisCommand = null;

        public ControlsInitTmpDBStructure()
        {
            base._Name = "GeoDBATool.ControlsInitTmpDBStructure";
            base._Caption = "初始化临时库";
            base._Tooltip = "初始化临时库结构";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "初始化临时库结构";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (!(m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD")) return false;
                try
                {
                    DevComponents.AdvTree.Node DBNode = null; //数据库树节点
                    //获取数据库节点
                    DBNode = m_Hook.ProjectTree.SelectedNode;
                    while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
                    {
                        DBNode = DBNode.Parent;
                    }
                    if (DBNode.DataKeyString == "DB")
                    {
                        if (DBNode.Text == "现势库")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    return false;
                }
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        //cyf 2011065 modify
        public override void OnClick()
        {
            Exception err = null;
            /////获取工程项目名称
            DevComponents.AdvTree.Node vSelectedNode = null;
            vSelectedNode = m_Hook.ProjectTree.SelectedNode;
            DevComponents.AdvTree.Node ProjectNode = null;
            ProjectNode = vSelectedNode;
            while (ProjectNode.Parent != null)
            {
                ProjectNode = ProjectNode.Parent;
            }
            //cyf 20110625 add:
            DevComponents.AdvTree.Node DBNode = null; //数据库树节点
            //获取数据库节点
            DBNode = vSelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }
            if (DBNode.DataKeyString != "DB")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库节点失败!");
                return;
            }

            DevComponents.AdvTree.Node DtSetNode = vSelectedNode; //数据集树节点
            if (DBNode.Text != "现势库")
            {
                return;
            }
            //XmlElement elementTemp = (DBNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;
            // IWorkspace TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            //ygc 2013-01-19修改临时库创建的位置
            frmXZDBPropertySet frm = new frmXZDBPropertySet();
            //frm.GetPropertySetStr = textSource.Text;
            IWorkspace TempWorkSpace=null;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //textSource.Text = frm.GetPropertySetStr;
                TempWorkSpace = frm.m_pworkspace;
            }
          
            if (TempWorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败!");
                return;
            }

            SysCommon.Gis.SysGisDataSet sysGisDataset = new SysCommon.Gis.SysGisDataSet(TempWorkSpace);
            //cyf 20110625 modify
            IFeatureDataset featureDataset = null;        //数据集
            if (vSelectedNode.DataKeyString == "FD")
            {
                featureDataset = sysGisDataset.GetFeatureDataset(vSelectedNode.Text, out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败，请检查该数据集是否存在!");
                    return;
                }
            }
            Exception pError = null;
            if (featureDataset != null)
            {
                bool bRes=InitialTmpDBstructure(featureDataset);
                if (bRes)
                {
                    MessageBox.Show("初始化临时库结构成功!");
                    //根据工程树图xml刷新工程树图界面
                    if (File.Exists(ModData.v_projectDetalXML))
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.Load(ModData.v_projectDetalXML);
                        ModData.v_AppGIS.DBXmlDocument = xml;
                        ModData.v_AppGIS.ProjectTree.Nodes.Clear();
                        ModDBOperator.RefreshProjectTree(ModData.v_AppGIS.DBXmlDocument, ModData.v_AppGIS.ProjectTree, out pError);
                    }
                }
            }

        }
        private bool InitialTmpDBstructure(IFeatureDataset pFeatureDataset)
        {
            if (pFeatureDataset == null)
            {
                return false;
            }
            IWorkspace pTagetWorkspace = pFeatureDataset.Workspace;
            if (pTagetWorkspace == null)
            {
                return false;
            }

            IFeatureDataset tagetFeatureDataset = null;
            string pFeaDatasetName = pFeatureDataset.Name;
            if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureDataset, pFeaDatasetName + "_GOT"))
            {
                tagetFeatureDataset = (pTagetWorkspace as IFeatureWorkspace).CreateFeatureDataset(pFeaDatasetName + "_GOT", (pFeatureDataset as IGeoDataset).SpatialReference);
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "该临时库已经进行了初始化!");
                return false;
            }


            IEnumDataset pEnumDs = pFeatureDataset.Subsets;
            pEnumDs.Reset();
            IDataset pDs = pEnumDs.Next();
            while (pDs != null)
            {
                IFeatureClass pFeatureClass = pDs as IFeatureClass;
                if (pFeatureClass != null)
                {
                    if (!(pTagetWorkspace as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, pDs.Name + "_GOT"))
                    {
                        Exception err = null;
                        ModHistory.CreateTmpFeaClsStructure(tagetFeatureDataset, pFeatureClass, pDs.Name + "_GOT", out err);
                        if (err != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建临时库库体结构失败!\n" + err.Message);
                            return false;
                        }
                    }
                }
                pDs = pEnumDs.Next();
            }
            return true;
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
