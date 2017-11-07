using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DevComponents.DotNetBar;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace GeoDBATool
{
    public partial class frmSelHistoryDataVersion : DevComponents.DotNetBar.Office2007Form
    {
        private List<AxMapControl> LstArcGisMapControl;
        private AxMapControl ArcGisMapControl;
        private IMapControlDefault Mapcontrol;
        private DevComponents.DotNetBar.Bar BarHistory;
        private List<ILayer> LayerList;

        private bool m_Sel;                                //是否过滤选择要素

        public frmSelHistoryDataVersion(ArrayList arrayListFromDate,AxMapControl arcGisMapControl,DevComponents.DotNetBar.Bar barHistory,bool bSel)
        {
            InitializeComponent();
            InitialFrm(arrayListFromDate);
            ArcGisMapControl = arcGisMapControl;
            Mapcontrol = ArcGisMapControl.Object as IMapControlDefault;
            BarHistory = barHistory;
            BarHistory.DockTabChange+=new DotNetBarManager.DockTabChangeEventHandler(BarHistory_DockTabChange);
            m_Sel = bSel;
        }

        private void InitialFrm(ArrayList arrayListFromDate)
        {
            for (int i = 0; i < arrayListFromDate.Count; i++)
            {
                ListViewItem aItem=listViewEx.Items.Add(arrayListFromDate[i].ToString());
                aItem.ToolTipText = arrayListFromDate[i].ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (listViewEx.CheckedItems.Count == 0) return;

            LayerList = new List<ILayer>();
            for (int i = 0; i < Mapcontrol.Map.LayerCount; i++)
            {
                //IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
               //changed by xisheng 20110828
                ILayer mLayer = Mapcontrol.Map.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = mLayer as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        IFeatureLayer featLay = pComLayer.get_Layer(j) as IFeatureLayer;

                        if (featLay == null) continue;
                        if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                        LayerList.Add(featLay);
                    }
                }
                else
                { 
                    IFeatureLayer featLay = Mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                    if (featLay == null) continue;
                    if (!(featLay.FeatureClass as IDataset).Name.EndsWith("_GOH")) continue;
                    LayerList.Add(featLay);
                }
            }
            
            LstArcGisMapControl = new List<AxMapControl>();
            BarHistory.Items["dockItemHistoryData0"].Text = listViewEx.CheckedItems[0].Text;
            BarHistory.Items["dockItemHistoryData0"].Tooltip = listViewEx.CheckedItems[0].Text;
            ESRI.ArcGIS.Controls.AxMapControl axMapControlHistoryData=(BarHistory.Items["dockItemHistoryData0"] as DockContainerItem).Control.Controls[0] as AxMapControl;
            LstArcGisMapControl.Add(axMapControlHistoryData);
            for(int i=LayerList.Count-1;i>=0;i--)
            {
                IFeatureLayer layTemp = LayerList[i] as IFeatureLayer;
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                if (layTemp.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatureLayer = new FDOGraphicsLayerClass();
                }
                else
                {
                    (pFeatureLayer as IGeoFeatureLayer).Renderer = (layTemp as IGeoFeatureLayer).Renderer;
                }
                
                pFeatureLayer.FeatureClass = layTemp.FeatureClass;
                pFeatureLayer.Name = (layTemp.FeatureClass as IDataset).Name;
                               

                IFeatureLayerDefinition featLayDefRes = null;
                if (m_Sel)
                {
                    int fdIndex = layTemp.FeatureClass.Fields.FindField("SourceOID");
                    if (fdIndex == -1) continue;
                    IFeatureLayerDefinition featLayDefTemp = layTemp as IFeatureLayerDefinition;
                    IEnumIDs pEnumIDs = featLayDefTemp.DefinitionSelectionSet.IDs;
                    int ID = pEnumIDs.Next();
                    StringBuilder sb = new StringBuilder();
                    while (ID != -1)
                    {
                        IFeature pFeat = layTemp.FeatureClass.GetFeature(ID);
                        if (sb.Length != 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(pFeat.get_Value(fdIndex).ToString());
                        ID = pEnumIDs.Next();
                    }
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "SourceOID in (" + sb.ToString() + ")";
                    IFeatureSelection featSel = pFeatureLayer as IFeatureSelection;
                    featSel.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    featSel.SelectionChanged();

                    IFeatureLayerDefinition featLayDef = pFeatureLayer as IFeatureLayerDefinition;
                    IFeatureLayer selFeatLay = featLayDef.CreateSelectionLayer(pFeatureLayer.Name, true, "", "");
                    IGeoFeatureLayer pGeoFeatureLayer = layTemp as IGeoFeatureLayer;
                    if (pGeoFeatureLayer != null)
                    {
                        (selFeatLay as IGeoFeatureLayer).Renderer = pGeoFeatureLayer.Renderer;
                    }

                    axMapControlHistoryData.Map.AddLayer(selFeatLay);
                    featLayDefRes = selFeatLay as IFeatureLayerDefinition;
                }
                else
                {
                    axMapControlHistoryData.Map.AddLayer(pFeatureLayer);
                    featLayDefRes = pFeatureLayer as IFeatureLayerDefinition;
                }
                                
                featLayDefRes.DefinitionExpression = "FromDate<='" + listViewEx.CheckedItems[0].Text + "' and ToDate>'" + listViewEx.CheckedItems[0].Text + "'";
            }
            axMapControlHistoryData.ActiveView.Extent = ArcGisMapControl.ActiveView.Extent;
            axMapControlHistoryData.ActiveView.Refresh();

            for(int i=1;i<listViewEx.CheckedItems.Count;i++)
            {
                DockContainerItem dockItemHistoryData = new DockContainerItem("dockItemHistoryData" + i.ToString(), listViewEx.CheckedItems[i].Text);
                dockItemHistoryData.Tooltip = listViewEx.CheckedItems[i].Text;
                PanelDockContainer PanelTipHistoryData = new PanelDockContainer();
               
                dockItemHistoryData.Control = PanelTipHistoryData;
                BarHistory.Items.Add(dockItemHistoryData);
            }

            ArcGisMapControl.OnExtentUpdated+=new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(ArcGisMapControl_OnExtentUpdated);
            ArcGisMapControl.OnViewRefreshed+=new IMapControlEvents2_Ax_OnViewRefreshedEventHandler(ArcGisMapControl_OnViewRefreshed);

            this.Close();
        }

        private void BarHistory_DockTabChange(object sender, DockTabChangeEventArgs e)
        {
            DockContainerItem dockItemHistoryData = e.NewTab as DockContainerItem;
            if (dockItemHistoryData.Control.Controls.Count != 0) return;

            if (LayerList.Count == 0) return;
            frmArcgisMapControl newFrm = new frmArcgisMapControl();
            for (int j = LayerList.Count - 1; j >= 0; j--)
            {
                IFeatureLayer layTemp = LayerList[j] as IFeatureLayer;
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                if (layTemp.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatureLayer = new FDOGraphicsLayerClass();
                }
                else
                {
                    (pFeatureLayer as IGeoFeatureLayer).Renderer = (layTemp as IGeoFeatureLayer).Renderer;
                }

                pFeatureLayer.FeatureClass = layTemp.FeatureClass;
                pFeatureLayer.Name = (layTemp.FeatureClass as IDataset).Name;
                
                IFeatureLayerDefinition featLayDefRes = null;
                if (m_Sel)
                {
                    int fdIndex = layTemp.FeatureClass.Fields.FindField("SourceOID");
                    if (fdIndex == -1) continue;
                    IFeatureLayerDefinition featLayDefTemp = layTemp as IFeatureLayerDefinition;
                    IEnumIDs pEnumIDs = featLayDefTemp.DefinitionSelectionSet.IDs;
                    int ID = pEnumIDs.Next();
                    StringBuilder sb = new StringBuilder();
                    while (ID != -1)
                    {
                        IFeature pFeat = layTemp.FeatureClass.GetFeature(ID);
                        if (sb.Length != 0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(pFeat.get_Value(fdIndex).ToString());
                        ID = pEnumIDs.Next();
                    }
                    IQueryFilter queryFilter = new QueryFilterClass();
                    queryFilter.WhereClause = "SourceOID in (" + sb.ToString() + ")";
                    IFeatureSelection featSel = pFeatureLayer as IFeatureSelection;
                    featSel.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    featSel.SelectionChanged();

                    IFeatureLayerDefinition featLayDef = pFeatureLayer as IFeatureLayerDefinition;
                    IFeatureLayer selFeatLay = featLayDef.CreateSelectionLayer(pFeatureLayer.Name, true, "", "");
                    IGeoFeatureLayer pGeoFeatureLayer = layTemp as IGeoFeatureLayer;
                    if (pGeoFeatureLayer != null)
                    {
                        (selFeatLay as IGeoFeatureLayer).Renderer = pGeoFeatureLayer.Renderer;
                    }

                    newFrm.ArcGisMapControl.Map.AddLayer(selFeatLay);
                    featLayDefRes = selFeatLay as IFeatureLayerDefinition;
                }
                else
                {
                    newFrm.ArcGisMapControl.Map.AddLayer(pFeatureLayer);
                    featLayDefRes = pFeatureLayer as IFeatureLayerDefinition;
                }

                featLayDefRes.DefinitionExpression = "FromDate<='" + dockItemHistoryData.Text + "' and ToDate>'" + dockItemHistoryData.Text + "'";
            }
            newFrm.ArcGisMapControl.ActiveView.Extent = ArcGisMapControl.ActiveView.Extent;
            newFrm.ArcGisMapControl.Refresh();
            newFrm.ArcGisMapControl.Dock = DockStyle.Fill;
            dockItemHistoryData.Control.Controls.Add(newFrm.ArcGisMapControl);

            LstArcGisMapControl.Add(newFrm.ArcGisMapControl);
        }

        private void ArcGisMapControl_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (LstArcGisMapControl == null) return;
            if (LstArcGisMapControl.Count == 0) return;
            foreach (AxMapControl arcgisMapControl in LstArcGisMapControl)
            {
                arcgisMapControl.Extent = e.newEnvelope as ESRI.ArcGIS.Geometry.IEnvelope;
                arcgisMapControl.ActiveView.Refresh();
            }
        }

        private void ArcGisMapControl_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            ILayer aLay = e.layerOrElement as ILayer;
            if (aLay == null) return;
            if (LstArcGisMapControl == null) return;
            if (LstArcGisMapControl.Count == 0) return;
            foreach (AxMapControl arcgisMapControl in LstArcGisMapControl)
            {
                for (int i = 0; i < arcgisMapControl.LayerCount; i++)
                {
                    if (arcgisMapControl.get_Layer(i).Name == aLay.Name)
                    {
                        arcgisMapControl.get_Layer(i).Visible = aLay.Visible;
                        break;
                    }
                }
                arcgisMapControl.ActiveView.Refresh();
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}