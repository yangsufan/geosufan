using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加
    /// </summary>
    public partial class frmDataMerge : DevComponents.DotNetBar.Office2007Form
    {
        IFeatureClass m_FeaCLs=null;
        List<int> m_LstOID = null;
        Plugin.Application.IAppGISRef m_hook = null;
        public frmDataMerge(IFeatureClass pFeatureClass, List<int> OIDLst, Plugin.Application.IAppGISRef phook)
        {
            InitializeComponent();

            m_FeaCLs = pFeatureClass;
            m_LstOID = OIDLst;
            m_hook = phook;
            InitTree();
            DataTable DTSource=AddDataToTable(int.Parse(AdvTreeFeaCls.Nodes[0].Nodes[0].Text.Trim()));
            if (dtGridAttribute.DataSource != null)
            {
                dtGridAttribute.DataSource = null;
            }
            dtGridAttribute.DataSource = DTSource;
            for (int i = 0; i < dtGridAttribute.ColumnCount; i++)
            {
                dtGridAttribute.Columns[i].Width = (dtGridAttribute.Width-40) / 2;
            }
            dtGridAttribute.RowHeadersWidth = 20;
            dtGridAttribute.Update();
        }
        /// <summary>
        /// 初始化树图
        /// </summary>
        private void InitTree()
        {
            string FCName = (m_FeaCLs as IDataset).Name;//图层名
            DevComponents.AdvTree.Node praNode = new DevComponents.AdvTree.Node();
            praNode.Text = FCName.Trim();
            praNode.Expanded = true;
            praNode.Tag=m_FeaCLs;
            AdvTreeFeaCls.Nodes.AddRange(new DevComponents.AdvTree.Node[] { praNode });//添加根结点
            for (int i = 0; i < m_LstOID.Count; i++)
            {
                DevComponents.AdvTree.Node subNode = new DevComponents.AdvTree.Node();
                subNode.Text = m_LstOID[i].ToString().Trim();//OID
                praNode.Nodes.AddRange(new DevComponents.AdvTree.Node[] { subNode });//添加子节点
            }
        }
        /// <summary>
        /// 创建临时表格作为DTGrid的数据源
        /// </summary>
        private DataTable CreateTable()
        { 
            DataTable DT=new DataTable();
            DT.Columns.Add("字段名", System.Type.GetType("System.String"));
            DT.Columns.Add("字段值", System.Type.GetType("System.String"));
            return DT;
        }
        /// <summary>
        /// 想临时表格里面填充数据
        /// </summary>
        /// <param name="pOID"></param>
        /// <returns></returns>
        private DataTable AddDataToTable(int pOID)
        {
            DataTable tempDT = CreateTable();
            IFeature pFeature = m_FeaCLs.GetFeature(pOID);
            for (int i = 0; i < pFeature.Fields.FieldCount; i++)
            {
                DataRow newRow = tempDT.NewRow();
                string pFieldName = pFeature.Fields.get_Field(i).Name;//字段名
                string pFieldValue = pFeature.get_Value(i).ToString().Trim();//字段值
                newRow[0] = pFieldName;
                newRow[1] = pFieldValue;
                tempDT.Rows.Add(newRow);
            }
            return tempDT;
        }

        private void AdvTreeFeaCls_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            int pOID = int.Parse(e.Node.Text.Trim());
            DataTable DTSource = AddDataToTable(pOID);
            if (dtGridAttribute.DataSource != null)
            {
                dtGridAttribute.DataSource = null;
            }
            dtGridAttribute.DataSource = DTSource;
            for (int i = 0; i < dtGridAttribute.ColumnCount; i++)
            {
                dtGridAttribute.Columns[i].Width = (dtGridAttribute.Width-40) / 2;
            }
            dtGridAttribute.RowHeadersWidth = 20;
            dtGridAttribute.Update();
        }

        private void AdvTreeFeaCls_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
                SysCommon.Error.frmInformation pfrmInformation = new SysCommon.Error.frmInformation("是", "否", "是否进行要素融合！");
                if (pfrmInformation.ShowDialog() == DialogResult.OK)
                {
                    int pNewOID = int.Parse(e.Node.Text.Trim());
                    IDataset pDS = m_FeaCLs as IDataset;
                    IWorkspace pWs = pDS.Workspace as IWorkspace;
                    IWorkspaceEdit pWSE = pWs as IWorkspaceEdit;
                    if (!pWSE.IsBeingEdited())
                    {
                        pWSE.StartEditing(false);
                    }
                    clsMergeFeatures pclsMergeFeatures = new clsMergeFeatures();
                    pclsMergeFeatures.MergeFeatures(e.Node.Parent.Tag as IFeatureClass, pNewOID, m_LstOID);
                    pWSE.StopEditing(true);

                    this.Close();

                    //刷新图面
                   IActiveView pActiveView=m_hook.MapControl.Map as IActiveView;
                    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

                }
            }
            catch(Exception er)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                //********************************************************************
            }
        }
    }
}