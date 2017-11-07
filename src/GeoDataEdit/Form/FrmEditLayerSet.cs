using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataEdit
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110811
    /// 说明：地图编辑图层设置窗体
    /// </summary>
    public partial class FrmEditLayerSet : DevComponents.DotNetBar.Office2007Form
    {
        private IMap inMap = null;
        private IMapControlDefault pMapCtl;
        private IWorkspace preW = null;//combolbox更改图层时，若工作空间也更改了 则保存上一个工作空间
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public FrmEditLayerSet(IMapControlDefault inMapCTL)
        {
            InitializeComponent();
            pMapCtl = inMapCTL;
            inMap = inMapCTL.Map;
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        if(!(pCLayer.get_Layer(j) is IFeatureLayer))
                           continue;
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        //added by chulili 20110729 错误保护
                        if (pFLayer == null)
                        { continue; }
                        //end added by chulili
                        //if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        cboLayers.Items.Add((pFLayer.FeatureClass as IDataset).Name + "(" + pFLayer.Name + ")"); 

                    }
                }
                else//不是grouplayer
                {
                    if(!(pLayer is IFeatureLayer))
                        continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer; 
                    //added by chulili 20110729 错误保护
                    if (pFLayer == null)
                    { continue; }
                    //end added by chulili 
                    //if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    cboLayers.Items.Add((pFLayer.FeatureClass as IDataset).Name + "(" + pLayer.Name+")"); 
                }
            }
            //if (cboLayers.Items.Count > 0)
            //    cboLayers.SelectedIndex = 0;
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\EditLayer.xml";
            if (!File.Exists(cPath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("EditLayerInfo");
                int i= cboLayers.FindStringExact(xnl.Item(0).Attributes["FCName"].Value + "(" + xnl.Item(0).Attributes["LayerName"].Value + ")");
                if (i > -1)
                {
                    cboLayers.SelectedIndex = i;
                    preW = ((getEditLayer.isExistLayer(inMap) as IFeatureLayer).FeatureClass as IDataset).Workspace;
                }
            }
            
            cXmlDoc = null;
        }

        private void btnXOK_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFL=getEditLayer.isExistLayer(inMap) as IFeatureLayer;
            if (pFL != null)
            {
                IFeatureSelection pFS = pFL as IFeatureSelection;
                pMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                pFS.Clear();
                pMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\EditLayer.xml";
            if (!File.Exists(cPath))
            {
                return;
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置" + pFL.Name + "为编辑图层");
            }
             XmlDocument cXmlDoc = new XmlDocument();

             if (cXmlDoc != null)
             {
                 cXmlDoc.Load(cPath);
                 string[] nm = cboLayers.Text.Split('(',')');
                 XmlNodeList xnl = cXmlDoc.GetElementsByTagName("EditLayerInfo");
                 xnl.Item(0).Attributes["LayerName"].Value = nm[1];
                 xnl.Item(0).Attributes["FCName"].Value = nm[0];
             }

             cXmlDoc.Save(cPath);
             IWorkspace curW=((getEditLayer.isExistLayer(inMap) as IFeatureLayer).FeatureClass as IDataset).Workspace;
             if(preW!=null&&!curW.Equals(preW))
             {
                 IWorkspaceEdit preWE=preW as IWorkspaceEdit;
                 bool hasEdit=false;
                 preWE.HasEdits(ref hasEdit);
                 if(hasEdit)
                 {
                     if (MessageBox.Show("您更改了工作空间，是否保存之前的编辑？", "提示", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information) == DialogResult.Yes)
                         preWE.StopEditing(true);
                     else
                     {
                         preWE.StopEditing(false);
                         pMapCtl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                     }
                 }
             }
             IWorkspaceEdit iWE =  curW as IWorkspaceEdit;
             if (!iWE.IsBeingEdited())
                 iWE.StartEditing(false);
        }

        private void FrmEditLayerSet_Load(object sender, EventArgs e)
        {
            
        }

        private void cboLayers_TextChanged(object sender, EventArgs e)
        {
            btnXOK.Enabled = true;
            pMapCtl.CurrentTool = null;
            if (cboLayers.Text == "")
                btnXOK.Enabled = false;
        }

        private void btnXCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("退出设置编辑涂层");
            }
        }

        private void FrmEditLayerSet_Load_1(object sender, EventArgs e)
        {

        }

    }
}
