using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace GeoDBATool
{
    public partial class frmBrowseHistorySelFeatures : DevComponents.DotNetBar.Office2007Form
    {
        private Dictionary<string, Plugin.Interface.ICommandRef> v_dicCommands;
        private Dictionary<string, Plugin.Interface.IToolRef> v_dicTools;

        public frmBrowseHistorySelFeatures(List<IFeatureLayer> layerList)
        {
            InitializeComponent();

            this.Controls.Remove(this.axMapControl);

            //初始化配置对应视图控件
            InitialMainViewControl();

            InitialFrm(layerList);
        }

         //初始化配置对应视图控件
        private void InitialMainViewControl()
        {
            dotNetBarManager.ToolbarTopDockSite.Controls.Add(barBrowse);
            dotNetBarManager.ToolbarLeftDockSite.Controls.Add(barMap);

            //加载设置数据视图
            DevComponents.DotNetBar.Bar barMapControl =CreateBar("barMapControl", enumLayType.FILL);
            barMapControl.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelMap = CreatePanelDockContainer("PanelMap", barMapControl);
            DevComponents.DotNetBar.DockContainerItem MapContainerItem = CreateDockContainerItem("TreeContainerItem", "数据视图", PanelMap, barMapControl);
            PanelMap.Controls.Add(this.axMapControl);
            this.axMapControl.Dock = DockStyle.Fill;
        }

        private DevComponents.DotNetBar.Bar CreateBar(string strName, enumLayType layType)
        {
            DevComponents.DotNetBar.Bar bar = new DevComponents.DotNetBar.Bar();
            bar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            bar.AlwaysDisplayDockTab = true;
            bar.CanCustomize = false;
            bar.CanDockBottom = false;
            bar.CanDockDocument = true;
            bar.CanDockLeft = false;
            bar.CanDockRight = false;
            bar.CanDockTop = false;
            bar.CanHide = true;
            bar.CanUndock = false;
            bar.DockTabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Top;
            bar.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            bar.SelectedDockTab = 0;
            bar.Stretch = true;
            bar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            bar.TabStop = false;
            bar.Name = strName;

            DevComponents.DotNetBar.DockSite dockSite = null;
            switch (layType)
            {
                case enumLayType.BOTTOM:
                    dockSite = dotNetBarManager.BottomDockSite;
                    break;
                case enumLayType.FILL:
                    dockSite = dotNetBarManager.FillDockSite;
                    break;
                case enumLayType.LEFT:
                    dockSite = dotNetBarManager.LeftDockSite;
                    break;
                case enumLayType.RIGHT:
                    dockSite = dotNetBarManager.RightDockSite;
                    break;
                case enumLayType.TOP:
                    dockSite = dotNetBarManager.TopDockSite;
                    break;
            }
            if (dockSite != null)
            {
                dockSite.Controls.Add(bar);
                dockSite.DocumentDockContainer.Orientation = DevComponents.DotNetBar.eOrientation.Vertical;
                DevComponents.DotNetBar.DocumentBarContainer aDocumentBarContainer = new DevComponents.DotNetBar.DocumentBarContainer(bar, bar.Size.Width, bar.Size.Height);
                dockSite.DocumentDockContainer.Documents.Add(((DevComponents.DotNetBar.DocumentBaseContainer)(aDocumentBarContainer)));
            }
            return bar;
        }

        private DevComponents.DotNetBar.PanelDockContainer CreatePanelDockContainer(string panelName, DevComponents.DotNetBar.Bar bar)
        {
            DevComponents.DotNetBar.PanelDockContainer panelDockContainer = new DevComponents.DotNetBar.PanelDockContainer();
            panelDockContainer.Name = panelName;
            panelDockContainer.Style.Alignment = System.Drawing.StringAlignment.Center;
            panelDockContainer.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            panelDockContainer.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            panelDockContainer.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            panelDockContainer.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            panelDockContainer.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            panelDockContainer.Style.GradientAngle = 90;
            panelDockContainer.Dock = DockStyle.Fill;
            bar.Controls.Add(panelDockContainer);

            return panelDockContainer;
        }

        private DevComponents.DotNetBar.DockContainerItem CreateDockContainerItem(string ItemName, string ItemText, DevComponents.DotNetBar.PanelDockContainer panelDockContainer, DevComponents.DotNetBar.Bar bar)
        {
            DevComponents.DotNetBar.DockContainerItem dockContainerItem = new DevComponents.DotNetBar.DockContainerItem();
            dockContainerItem.Control = panelDockContainer;
            dockContainerItem.Name = ItemName;
            dockContainerItem.Text = ItemText;

            bar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] { dockContainerItem });

            return dockContainerItem;
        }

        private void InitialFrm(List<IFeatureLayer> layerList)
        {
            foreach (IFeatureLayer aFeatLay in layerList)
            {
                IFeatureSelection featSel = aFeatLay as IFeatureSelection;
                int fdIndex=aFeatLay.FeatureClass.Fields.FindField("SourceOID");
                if(fdIndex==-1) continue;
                IEnumIDs pEnumIDs = featSel.SelectionSet.IDs;
                int ID = pEnumIDs.Next();
                StringBuilder sb=new StringBuilder();
                while (ID != -1)
                {
                    IFeature pFeat = aFeatLay.FeatureClass.GetFeature(ID);
                    if(sb.Length!=0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(pFeat.get_Value(fdIndex).ToString());
                    ID = pEnumIDs.Next();
                }

                IQueryFilter queryFilter=new QueryFilterClass();
                queryFilter.WhereClause="SourceOID in ("+sb.ToString()+")";
                IFeatureLayerDefinition featLayDef = aFeatLay as IFeatureLayerDefinition;
                featLayDef.DefinitionExpression = "";
                featSel.SelectFeatures(queryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                featSel.SelectionChanged();
                                
                IFeatureLayer selFeatLay = featLayDef.CreateSelectionLayer(aFeatLay.Name, true, "", "");
                IGeoFeatureLayer pGeoFeatureLayer = aFeatLay as IGeoFeatureLayer;
                if (pGeoFeatureLayer != null)
                {
                    (selFeatLay as IGeoFeatureLayer).Renderer = pGeoFeatureLayer.Renderer;
                }
                this.axMapControl.Map.AddLayer(selFeatLay);
            }
            SysCommon.Gis.ModGisPub.LayersCompose(this.axMapControl.Object as IMapControlDefault);

            InitialSliderItem(this.axMapControl.Object as IMapControlDefault);

            InitialbarMap();
        }

        private void InitialSliderItem(IMapControlDefault mapcontrol)
        {
            comboBoxItem.Items.Clear();

            ArrayList arrayListFromDate = new ArrayList();
            //计算各历史图层的fromdate字段唯一值
            for (int i = 0; i < mapcontrol.Map.LayerCount; i++)
            {
                IFeatureLayer featLay = mapcontrol.Map.get_Layer(i) as IFeatureLayer;
                ITable table = featLay.FeatureClass as ITable;
                IDataStatistics statistics = new DataStatisticsClass();
                statistics.Cursor = table.Search(null, false);
                statistics.Field = "FromDate";
                IEnumerator enumDate = statistics.UniqueValues;
                enumDate.Reset();
                while (enumDate.MoveNext())
                {
                    if (!arrayListFromDate.Contains(enumDate.Current))
                    {
                        arrayListFromDate.Add(enumDate.Current);
                    }
                }
            }

            //组合形成时间段
            arrayListFromDate.Sort();
            for (int i = 0; i < arrayListFromDate.Count; i++)
            {
                comboBoxItem.Items.Add(arrayListFromDate[i]);
            }
            comboBoxItem.SelectedIndex = arrayListFromDate.Count - 1;
            sliderItem.Maximum = arrayListFromDate.Count;
            sliderItem.Minimum = 1;
            sliderItem.Value = arrayListFromDate.Count;
            sliderItem.Tag = arrayListFromDate;
        }

        private void InitialbarMap()
        {
            Plugin.Interface.IToolRef tool = null;
            Plugin.Interface.ICommandRef cmd = null;
            Plugin.Application.IAppArcGISRef appArcGISRef = new Plugin.Application.AppGIS();
            appArcGISRef.MapControl = axMapControl.Object as IMapControlDefault; ;

            v_dicCommands = new Dictionary<string, Plugin.Interface.ICommandRef>();
            v_dicTools = new Dictionary<string, Plugin.Interface.IToolRef>();

            tool = new GeoUtilities.ControlsDefaultTool();
            tool.OnCreate(appArcGISRef);
            v_dicTools.Add("btnDefault", tool);

            tool = new GeoUtilities.ControlsMapZoomInTool();
            tool.OnCreate(appArcGISRef);
            v_dicTools.Add("btnMapZoomIn", tool);

            tool = new GeoUtilities.ControlsMapZoomOutTool();
            tool.OnCreate(appArcGISRef);
            v_dicTools.Add("btnMapZoomOut", tool);

            tool = new GeoUtilities.ControlsMapPanTool();
            tool.OnCreate(appArcGISRef);
            v_dicTools.Add("btnMapPan", tool);

            cmd = new GeoUtilities.ControlsMapZoomInFixedCommand();
            cmd.OnCreate(appArcGISRef);
            v_dicCommands.Add("btnMapZoomInFixed", cmd);

            cmd = new GeoUtilities.ControlsMapZoomOutFixedCommand();
            cmd.OnCreate(appArcGISRef);
            v_dicCommands.Add("btnMapZoomOutFixed", cmd);

            cmd = new GeoUtilities.ControlsMapRefreshViewCommand();
            cmd.OnCreate(appArcGISRef);
            v_dicCommands.Add("btnMapRefreshView", cmd);

            cmd = new GeoUtilities.ControlsMapFullExtentCommand();
            cmd.OnCreate(appArcGISRef);
            v_dicCommands.Add("btnMapFullExtent", cmd);

            cmd = new GeoUtilities.ControlsMapZoomToLastExtentBackCommand();
            cmd.OnCreate(appArcGISRef);
            v_dicCommands.Add("btnMapZoomToLastExtentBack", cmd);

            cmd = new GeoUtilities.ControlsMapZoomToLastExtentForwardCommand();
            cmd.OnCreate(appArcGISRef);
            v_dicCommands.Add("btnMapZoomToLastExtentForward", cmd);

            tool = new GeoUtilities.ControlsMapIdentifyTool();
            tool.OnCreate(appArcGISRef);
            v_dicTools.Add("btnMapIdentify", tool);

            tool = new GeoUtilities.ControlsMapMeasureTool();
            tool.OnCreate(appArcGISRef);
            v_dicTools.Add("btnMapMeasure", tool);
        }

        private void barMap_ItemClick(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.ButtonItem aButtonItem = sender as DevComponents.DotNetBar.ButtonItem;
            if (aButtonItem == null) return;

            if (v_dicCommands.ContainsKey(aButtonItem.Name))
            {
                Plugin.Interface.ICommandRef pCommandRef = v_dicCommands[aButtonItem.Name] as Plugin.Interface.ICommandRef;
                pCommandRef.OnClick();
            }

            if (v_dicTools.ContainsKey(aButtonItem.Name))
            {
                Plugin.Interface.IToolRef pToolRef = v_dicTools[aButtonItem.Name] as Plugin.Interface.IToolRef;
                pToolRef.OnClick();
            }
        }

        private void sliderItem_ValueChanged(object sender, EventArgs e)
        {
            comboBoxItem.SelectedIndex = sliderItem.Value - 1;

            ChangeLayersDef();
        }
        private void ChangeLayersDef()
        {
            for (int i = 0; i < axMapControl.Map.LayerCount; i++)
            {
                IFeatureLayer featLay = axMapControl.Map.get_Layer(i) as IFeatureLayer;
                IFeatureLayerDefinition featLayDef = featLay as IFeatureLayerDefinition;
                featLayDef.DefinitionExpression = "FromDate<='" + comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString() + "' and ToDate>'" + comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString() + "'";
            }

            axMapControl.ActiveView.Refresh();
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            sliderItem.Value = comboBoxItem.SelectedIndex + 1;
            ChangeLayersDef();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            barHistoryDataCompare.Items.Clear();
            barHistoryDataCompare.Hide();
            dotNetBarManager.RightDockSite.Controls.Add(barHistoryDataCompare);
            dotNetBarManager.RightDockSite.DocumentDockContainer = new DevComponents.DotNetBar.DocumentDockContainer(new DevComponents.DotNetBar.DocumentBaseContainer[] {
            ((DevComponents.DotNetBar.DocumentBaseContainer)(new DevComponents.DotNetBar.DocumentBarContainer(barHistoryDataCompare, 664, 126)))}, DevComponents.DotNetBar.eOrientation.Vertical);
            barHistoryDataCompare.Size = new System.Drawing.Size(362, 228);
            DevComponents.DotNetBar.DockContainerItem dockItemHistoryData = new DevComponents.DotNetBar.DockContainerItem("dockItemHistoryData0", "历史数据对比浏览");
            DevComponents.DotNetBar.PanelDockContainer PanelTipHistoryData = new DevComponents.DotNetBar.PanelDockContainer();
            frmArcgisMapControl newFrmArcgisMapControl = new frmArcgisMapControl();
            newFrmArcgisMapControl.ArcGisMapControl.Dock = DockStyle.Fill;
            PanelTipHistoryData.Controls.Add(newFrmArcgisMapControl.ArcGisMapControl);
            dockItemHistoryData.Control = PanelTipHistoryData;
            barHistoryDataCompare.Items.Add(dockItemHistoryData);
            barHistoryDataCompare.Show();
            barHistoryDataCompare.AutoHide = true;
            barHistoryDataCompare.Width = 600;
            ArrayList arrayListFromDate = sliderItem.Tag as ArrayList;
            if (arrayListFromDate == null) return;
            frmSelHistoryDataVersion newFrm = new frmSelHistoryDataVersion(arrayListFromDate, this.axMapControl, barHistoryDataCompare,true);
            newFrm.ShowDialog(this);
        }

        private void btnStract_Click(object sender, EventArgs e)
        {
            ArrayList arrayListFromDate = sliderItem.Tag as ArrayList;
            if (arrayListFromDate == null) return;
            frmStractHistorySet newFrm = new frmStractHistorySet(axMapControl.Map, arrayListFromDate, comboBoxItem.Items[comboBoxItem.SelectedIndex].ToString(), true);
            newFrm.ShowDialog(this);
        }
    }
}