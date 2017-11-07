using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

//ygc 2012-9-13 自定义显示
namespace GeoDataManagerFrame
{
    public partial class FrmDefinitionQuery : DevComponents.DotNetBar.Office2007Form
    {
        IMapControlDefault m_MapControl = null;
        public FrmDefinitionQuery(IMapControlDefault mapcontrol)
        {
            m_MapControl = mapcontrol;
            InitializeComponent();
        }
        private IFeatureLayerDefinition m_pFeatureLayerDefinition;
        private IFeatureLayer m_pCurrentLayer;
        private string LayerID;
        private string m_Condition = "";
       
        private void btnSaveSolution_Click(object sender, EventArgs e)
        {
            if (LayerID == null || LayerID == "")
            {
                MessageBox.Show("请先选择查看的图层！","提示");
                return;
            }
            if (RichTxtCondition.Text == "")
            {
                MessageBox.Show("未选择查看条件！","提示");
                return;
            }
            FrmSaveSQLSolution newFrm = new FrmSaveSQLSolution(m_pCurrentLayer.FeatureClass.FeatureDataset.Workspace);
            newFrm.m_TableName = "DEFINITIONMAPSHOW";
            newFrm.m_LayerID = LayerID;
            newFrm.m_FrmText = "保存方案";
            newFrm.m_Condition = RichTxtCondition.Text.Trim();
            newFrm.ShowDialog();

        }

        private void btnOpenSolution_Click(object sender, EventArgs e)
        {
            if (LayerID == null || LayerID == "")
            {
                MessageBox.Show("请先选择查看的图层！","提示");
                return;
            }
            FrmOpenSQLCondition newfrm = new FrmOpenSQLCondition(m_pCurrentLayer.FeatureClass.FeatureDataset.Workspace);
            newfrm.m_TableName = "DEFINITIONMAPSHOW";
            newfrm.m_LayerId = LayerID;
            newfrm.m_FrmText = "打开方案";
            newfrm.m_DvCaption = "方案名称";
            if (DialogResult.OK != newfrm.ShowDialog()) return;
            RichTxtCondition.Text = newfrm.m_Condition;
        }

        private void btnSetCondition_Click(object sender, EventArgs e)
        {
            if (m_pCurrentLayer == null)
            {
                MessageBox.Show("未选择查看图层！","提示");
                return;
            }
            
                frmQuery newFrm = new frmQuery(m_pCurrentLayer, "");
                if (DialogResult.OK != newFrm.ShowDialog()) return;
                m_Condition = newFrm.m_sWhereClause;
                RichTxtCondition.Text = m_Condition;
        }

        private void txtLayerName_Click(object sender, EventArgs e)
        {
            Plugin.SelectLayerByTree frm = new Plugin.SelectLayerByTree(1);
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.m_NodeKey.Trim() != "")
                {
                    m_pCurrentLayer = new FeatureLayerClass();
                    IFeatureClass pFeatureClass = null;
                    pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath, frm.m_NodeKey);
                    if (pFeatureClass == null) return;
                    m_pCurrentLayer.FeatureClass = pFeatureClass;
                    LayerID = frm.m_NodeKey;
                }

                if (m_pCurrentLayer.FeatureClass != null)
                {

                    txtLayerName.Text = frm.m_NodeText;
                    m_pCurrentLayer.Name = frm.m_NodeText;// xisheng 20111122 自定义查询无名称BUG修改
                }
            }
            else
            {
                return;
            }
            ILayer pLayer = GetLayerByName(m_MapControl, m_pCurrentLayer.Name);
            if (pLayer == null) return;
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pLayer;
            IFeatureLayerDefinition pDefinition = (IFeatureLayerDefinition)pFeatureLayer;
            if (pDefinition.DefinitionExpression != null || pDefinition.DefinitionExpression != "")
            {
                RichTxtCondition.Text = pDefinition.DefinitionExpression;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //刷新地图
            if (m_MapControl != null)
            {
                ILayer m_layer = GetLayerByName(m_MapControl, m_pCurrentLayer.Name);
                if (m_layer == null)
                {
                    MessageBox.Show("在地图中未找到选择图层，请重新选择查看图层！","提示");
                    return;
                }
                IFeatureLayer pFeatureLayer = (IFeatureLayer)m_layer;
                m_pFeatureLayerDefinition = (IFeatureLayerDefinition)pFeatureLayer;
                m_pFeatureLayerDefinition.DefinitionExpression = RichTxtCondition.Text.Trim();
                m_MapControl.Refresh(esriViewDrawPhase.esriViewBackground, m_layer, null);
            }
            this.Close();
        }
        private ILayer GetLayerByName(IMapControlDefault mapControl, string LayerName)
        {
            ILayer pLayer = null;
            for (int i = 0; i < mapControl.LayerCount; i++)
            {
                if (mapControl.get_Layer(i).Name == LayerName)
                {
                    pLayer = mapControl.get_Layer(i);
                    break;
                }
            }
                return pLayer;
        }
    }
}
