using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;



namespace GeoDataEdit
{
    public partial class MapDataEdit : DevComponents.DotNetBar.Office2007Form
    {

        public MapDataEdit()
        {
            InitializeComponent();
            axMapControl1.Map.Name = "地图数据";
        }
        #region Public function
        private bool InEdit(ILayer player)
        {
            // Check edit conditions before allowing edit to stop
            CreateShape.m_CurrentLayer = player;
            if (CreateShape.m_CurrentLayer == null) return false;
            try
            {
                IFeatureLayer pFeatureLayer = (IFeatureLayer)CreateShape.m_CurrentLayer;
                if (pFeatureLayer.FeatureClass == null) return false;
                IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
                if (pDataset == null) return false;
                IWorkspaceEdit pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;
                if (pWorkspaceEdit.IsBeingEdited()) return true;

                return false;
            }
            catch
            {
                return false;
            }

        }
        #endregion
        #region  Events
        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            CoordinateStatusBar.Text = string.Format("{0} {1} {2}", e.mapX.ToString("#######.###"), e.mapY.ToString("#######.###"), axMapControl1.MapUnits.ToString().Substring(4));
        }
    
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            ITOCControl pTOCControl = axTOCControl1.Object as ITOCControl;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap map = null; ILayer layer = null;
            object other = null; object index = null;
            pTOCControl.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);
            if (e.button == 2)
            {
                switch (item)
                {
                    case esriTOCControlItem.esriTOCControlItemMap:
                        this.contextMenuTOCMap.Show(this.axTOCControl1, e.x, e.y);
                        break;
                }
            }
        }
        private void axMapControl1_OnKeyDown(object sender, IMapControlEvents2_OnKeyDownEvent e)
        {
            if (e.shift == 2 && e.keyCode == 90)
            { ;}
        }
        private void MapDataEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            CreateShape.IsEditing();
            this.Dispose();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

        }
        private void barMapTool_ItemClick(object sender, EventArgs e)
        {
            ExecuteDevCommand(sender);
        }
        private void contextMenuTOCMap_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ExecuteWinCommand(e.ClickedItem);
        }
        private void barEdit_ItemClick(object sender, EventArgs e)
        {
            ExecuteDevCommand(sender);
        }
      
        #endregion

        #region Command  Tools
        private void ExecuteDevCommand(object sender)
        {
            if (sender == null) return;
            DevComponents.DotNetBar.BaseItem vItem = sender as DevComponents.DotNetBar.BaseItem;
            string strToolStripName = vItem.Name.ToLower();
           ESRI.ArcGIS.SystemUI.ICommand  pCommand = null;

            switch (strToolStripName)
            {
                case "bttdefaulttool":
                    pCommand = new Tools.ControlsDefaultTool();
                    break;
                case "bttselection"://打开文档
                    pCommand = new ControlsSelectFeaturesToolClass();
                    break;
                case "bttclearselection":
                    pCommand = new ControlsClearSelectionCommandClass();
                    break;
                case "bttzoomintool":
                    pCommand = new ControlsMapZoomInToolClass();
                    break;
                case "bttzoomouttool":
                    pCommand = new ControlsMapZoomOutToolClass();
                    break;
                case "bttpantool":
                    pCommand = new ControlsMapPanToolClass();
                    break;
                case "bttzoominfixedcommand":
                    pCommand = new ControlsMapZoomInFixedCommandClass();
                    break;
                case "bttzoomoutfixedcommand":
                    pCommand = new ControlsMapZoomOutFixedCommandClass();
                    break;
                case "bttrefreshviewcommand":
                    pCommand = new ControlsMapRefreshViewCommandClass();
                    break;
                case "bttfullextentcommand":
                    pCommand = new ControlsMapFullExtentCommandClass();
                    break;
                case "bttzoomtolastextentbackcommand":
                    pCommand = new ControlsMapZoomToLastExtentBackCommandClass();
                    break;
                case "bttzoomtolastextentforwardcommand":
                    pCommand = new ControlsMapZoomToLastExtentForwardCommandClass();
                    break;
                case "bttstartedit":
                    if (CreateShape.m_CurrentLayer != null)
                    {
                        pCommand = new Command.ControlsStartEditCommand();
                    }
                    break;
                case "bttsaveedit":
                    if (CreateShape.m_CurrentLayer != null)
                    {
                        pCommand = new Command.ControlsSaveEditCommand(axMapControl1);
                    }
                    break;
                case "bttstopedit":
                    if (CreateShape.m_CurrentLayer != null)
                    {
                        axMapControl1.CurrentTool = null;
                        axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
                        pCommand = new Command.ControlsStopEditCommand(axMapControl1);
                    }
                    break;
                case "buttsketchup":
                    if (CreateShape.m_CurrentLayer != null && CreateShape.pWorkspaceEdit != null)
                    {
                        if(CreateShape.pWorkspaceEdit.IsBeingEdited())
                        {
                            axMapControl1.CurrentTool = null;
                            pCommand = new Tools.SketChupTool(axMapControl1);
                        }
                    }
                    break;
                case"bttnundo":
                    if (CreateShape.m_CurrentLayer != null && CreateShape.pWorkspaceEdit != null)
                    {
                        if (CreateShape.pWorkspaceEdit.IsBeingEdited())
                        {
                            pCommand = new Command.ControlsUndoEditCommand(axMapControl1);
                        }
                    }
                    break;
                case"bttnredo":
                    if (CreateShape.m_CurrentLayer != null && CreateShape.pWorkspaceEdit != null)
                    {
                        if (CreateShape.pWorkspaceEdit.IsBeingEdited())
                        {
                            pCommand = new Command.ControlsRedoEditCommand(axMapControl1);
                        }
                    }
                    break;
                case"bttndelete":
                    if (CreateShape.m_CurrentLayer != null && CreateShape.pWorkspaceEdit != null)
                    {
                        if (CreateShape.pWorkspaceEdit.IsBeingEdited())
                        {
                            pCommand = new Command.ControlsDeleteSelectedFeaturesCommand(axMapControl1);
                        }
                    }
                    break;
                case"bttadddata":
                    pCommand = new Command.ControlsAddDataCommand(axMapControl1);
                    break;
                case"bttopendoc":
                    pCommand = new Command.ControlsOpenMxdDocCommand(axMapControl1);
                    break;
                case "bttsave":
                    pCommand = new Command.ControlsSaveMxdDocCommand();
                    break;
                case "bttsaveas":
                    pCommand = new Command.ControlsSaveasMxdDocCommand();
                    break;
                case "bttcut":
                    pCommand = new ControlsEditingCutCommandClass();
                    break;
                case "bttcopy":
                    pCommand = new ControlsEditingCopyCommandClass();
                    break;
                case "bttpast":
                    pCommand = new ControlsEditingPasteCommandClass();
                    break;
                case "bttdelete":
                    pCommand = new ControlsEditingClearCommandClass();
                    break;
                case"bttexitmxddoc":
                    pCommand = new Command.ControlsExitMxdDocCommand(this);
                    break;
            }
            if (pCommand == null) return;
            pCommand.OnCreate(this.axMapControl1.Object);
            if (pCommand is ITool)
            {
                this.axMapControl1.CurrentTool = pCommand as ITool;
            }
            else
            {
                try
                {
                    pCommand.OnClick();
                }
                catch { MessageBox.Show("未启动编辑","提示！"); }
            }
        }
        private void ExecuteWinCommand(ToolStripItem vToolStripItem)
        {
            if (vToolStripItem == null) return;
            string strToolStripName = vToolStripItem.Name.ToLower();

            ESRI.ArcGIS.SystemUI.ICommand  pCommand = null;
            switch (strToolStripName)
            {
                case "bttopenalllayer":
                    pCommand = new Command.ControlsOpenAllLayerCommand();
                    break;
                case "bttcolsealllayer":
                    pCommand = new Command.controlsColseAllLayerCommand();
                    break;
                case "bttexpandalllayer":
                    pCommand = new Command.ControlsExpandAllLayerCommand();
                    break;
                case "bttcollapsealllayer":
                    pCommand = new Command.ControlsCollapseAllLayerCommand();
                    break;
                case "bttdeletealllayer":
                    pCommand = new Command.ControlsDeleteAllLayerCommand();
                    break;
            }

            if (pCommand == null) return;
            pCommand.OnCreate(this.axMapControl1.Object);
            if (pCommand is ITool)
            {
                this.axMapControl1.CurrentTool = pCommand as ITool;
            }
            else
            {
                pCommand.OnClick();
            }
            this.axTOCControl1.Update();
        }

        #endregion

     

        

      

       




































    }
}
