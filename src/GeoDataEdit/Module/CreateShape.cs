using System;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
//第三方控件DotNetBar
using DevComponents.DotNetBar;


namespace GeoDataEdit
{
    /// <summary>
    /// CreateShape 的摘要说明。
    /// </summary>
    public  class CreateShape 
    {
        static AxMapControl m_MapControl;            //设置地图控件对象
        public static ILayer m_CurrentLayer;    //获取当前编辑图层 
        //IPointCollection m_PntCol;         //临时记录未结束前画的点序
        private static string m_strOperator = "";

        //画点线面时的使用状态，画线面时为真
   

        public static IWorkspaceEdit pWorkspaceEdit;
        /// <summary>
        /// 开始编辑
        /// </summary>
        /// <param name="player"></param>
        public static void StartEditing(ILayer player, string strOperator)
        {
            // 开始编辑前检查编辑状态
            m_strOperator = strOperator;
            m_CurrentLayer = player;
            if (m_CurrentLayer == null)  return;

                if ((IGeoFeatureLayer)m_CurrentLayer == null) return;
                IFeatureLayer pFeatureLayer = (IFeatureLayer)m_CurrentLayer;
                IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
             
                if (pDataset == null) return;

                // Start editing, making sure that undo/redo are enabled
                pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;
                if (!pWorkspaceEdit.IsBeingEdited())
                {
                    try
                    {
                        pWorkspaceEdit.StartEditing(true);
                        pWorkspaceEdit.StartEditOperation();
                        pWorkspaceEdit.EnableUndoRedo();
                    }
                    catch
                    {
                        if (pWorkspaceEdit.IsBeingEdited())
                        {
                            pWorkspaceEdit.StopEditOperation();
                            pWorkspaceEdit.StopEditing(true);
                            pWorkspaceEdit.StartEditing(true);
                            pWorkspaceEdit.StartEditOperation();
                            pWorkspaceEdit.EnableUndoRedo();
                        }
                       
                    }
                    
                  
                }
        }

        //停止编辑
        public static void StopEditing(AxMapControl axmap, ILayer player)
        {
            m_strOperator = "";
            if (pWorkspaceEdit == null) return;
            // Check edit conditions before allowing edit to stop
            m_CurrentLayer = player;
            m_MapControl = axmap;
            if (m_CurrentLayer == null) return;
            IFeatureLayer pFeatureLayer = (IFeatureLayer)m_CurrentLayer;
            if (pFeatureLayer.FeatureClass == null) return;
            IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
            if (pDataset == null) return;

            // If the current document has been edited then prompt the user to save changes
            //IWorkspaceEdit pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;            

            if (pWorkspaceEdit.IsBeingEdited())
            {
                bool bHasEdits = false;
                pWorkspaceEdit.HasEdits(ref bHasEdits);
                bool bSave = false;
                if (bHasEdits)
                {
                    DialogResult result;
                    result = MessageBoxEx.Show( "你想保存你的编辑吗？", "提示！", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (DialogResult.Yes == result)
                    {
                        bSave = true;                       
                    }
                }
                pWorkspaceEdit.StopEditing(bSave);
            }

            m_MapControl.Map.ClearSelection();
            m_MapControl.ActiveView.Refresh();
        }
        public static void IsEditing()
        {
            try
            {
                if (pWorkspaceEdit.IsBeingEdited())
                {
                    bool bHasEdits = false;
                    pWorkspaceEdit.HasEdits(ref bHasEdits);
                    bool bSave = false;
                    if (bHasEdits)
                    {
                        DialogResult result;
                        result = MessageBoxEx.Show("你想保存你的编辑吗？", "提示！", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (DialogResult.Yes == result)
                        {
                            bSave = true;
                        }
                    }
                    pWorkspaceEdit.StopEditing(bSave);
                }
                m_MapControl.Map.ClearSelection();
                m_MapControl.ActiveView.Refresh();
            }
            catch { }
            
        }
        //保存编辑
        public static void SaveEditing(AxMapControl axmap, ILayer player)
        {
            // Check edit conditions before allowing edit to stop
            m_CurrentLayer = player;
            m_MapControl = axmap;
            if (m_CurrentLayer == null) return;
            IFeatureLayer pFeatureLayer = (IFeatureLayer)m_CurrentLayer;
            if (pFeatureLayer.FeatureClass == null) return;
            IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
            if (pDataset == null) return;

            // If the current document has been edited then prompt the user to save changes
            IWorkspaceEdit pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;
            
            bool bHasEdits = false;
            pWorkspaceEdit.HasEdits(ref bHasEdits);
            if (bHasEdits)
            {
                pWorkspaceEdit.StopEditOperation();
                pWorkspaceEdit.StopEditing(bHasEdits);
            }
        }

       //撤销
        public static void UndoEdit(AxMapControl axmap, ILayer player)
        {
            // Check that editing is possible
            m_CurrentLayer = player;
            m_MapControl = axmap;
            if (m_CurrentLayer == null) return;
            IFeatureLayer pFeatureLayer = (IFeatureLayer)m_CurrentLayer;
            IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
            if (pDataset == null) return;

            /// If edits have taken place then roll-back the last one
            IWorkspaceEdit pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;
            bool bHasUndos = false;
            pWorkspaceEdit.HasUndos(ref bHasUndos);
            if (bHasUndos)
                pWorkspaceEdit.UndoEditOperation();

            IActiveView pActiveView = (IActiveView)m_MapControl.Map;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
            pActiveView.Refresh();
        }
        //重做
        public static void RedoEdit(AxMapControl axmap, ILayer player)
        {
            // Check that editing is possible
            m_CurrentLayer = player;
            m_MapControl = axmap;
            if (m_CurrentLayer == null) return;
            IFeatureLayer pFeatureLayer = (IFeatureLayer)m_CurrentLayer;
            IDataset pDataset = (IDataset)pFeatureLayer.FeatureClass;
            if (pDataset == null) return;

            /// If edits have taken place then roll-back the last one
            IWorkspaceEdit pWorkspaceEdit = (IWorkspaceEdit)pDataset.Workspace;
            bool bHasUndos = false;
            pWorkspaceEdit.HasRedos(ref bHasUndos);
            if (bHasUndos)
                pWorkspaceEdit.RedoEditOperation();

            IActiveView pActiveView = (IActiveView)m_MapControl.Map;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
            pActiveView.Refresh();
        }


        private static IFeatureCursor GetSelectedFeatures(ILayer player)
        {
            m_CurrentLayer = player;
            if (m_CurrentLayer == null) return null;

            // If there are no features selected let the user know
            IFeatureSelection pFeatSel = (IFeatureSelection)m_CurrentLayer;
            ISelectionSet pSelectionSet = pFeatSel.SelectionSet;
            if (pSelectionSet.Count == 0)
            {
                MessageBoxEx.Show("在 '" + m_CurrentLayer.Name + "' 图层没有要素被选择", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            // Otherwise get all of the features back from the selection
            ICursor pCursor;
            pSelectionSet.Search(null, false, out pCursor);
            return (IFeatureCursor)pCursor;
        }

        public static void DeleteSelectedFeatures(AxMapControl axmap, ILayer player)
        {
            m_CurrentLayer = player;
            m_MapControl = axmap;
            if (m_CurrentLayer == null) return;

            // If there are no features currently selected then nothing to do
            IFeatureCursor pFeatureCursor = GetSelectedFeatures(player);
            if (pFeatureCursor == null) return;

            m_MapControl.Map.ClearSelection();

            // Loop over the selected features deleting each in turn
            IWorkspaceEdit pWorkspaceEdit = GetWorkspaceEdit();
            pWorkspaceEdit.StartEditOperation();
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                pFeature.Delete();
                pFeature = pFeatureCursor.NextFeature();
            }
            pWorkspaceEdit.StopEditOperation();

            IActiveView pActiveView = (IActiveView)m_MapControl.Map;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, pActiveView.Extent);
            pActiveView.Refresh();
        }
        private static IWorkspaceEdit GetWorkspaceEdit()                            //获取当前编辑空间
        {
            if (m_CurrentLayer == null) return null;

            IFeatureLayer m_FeatureLayer = (IFeatureLayer)m_CurrentLayer;
            IFeatureClass m_FeatureClass = m_FeatureLayer.FeatureClass;
            IDataset m_Dataset = (IDataset)m_FeatureClass;
            if (m_Dataset == null) return null;
            return (IWorkspaceEdit)m_Dataset.Workspace;
        }



    }
}
