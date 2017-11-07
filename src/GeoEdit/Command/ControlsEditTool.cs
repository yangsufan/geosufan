using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoEdit
{
    public class ControlsEditTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef m_Hook;
        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsEditTool()
        {

            base._Name = "GeoEdit.ControlsEditTool";
            base._Caption = "要素编辑";
            base._Tooltip = "要素编辑";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "要素编辑";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
                return true;
            }
        }

        public override bool Checked
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentTool != this.Name) return false;
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
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppArcGISRef;
            if (m_Hook.MapControl == null) return;

            _tool = new ControlsEditSelFeature();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(m_Hook.MapControl);
        }

        public override void OnClick()
        {
            if (_tool == null || _cmd == null || m_Hook == null) return;
            if (m_Hook.MapControl == null) return;
            m_Hook.ArcGisMapControl.OnSelectionChanged+=new EventHandler(ArcGisMapControl_OnSelectionChanged);
            m_Hook.ArcGisMapControl.OnAfterScreenDraw+=new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterScreenDrawEventHandler(ArcGisMapControl_OnAfterScreenDraw);
            m_Hook.MapControl.Map.ClearSelection();

            m_Hook.MapControl.CurrentTool = _tool;
            m_Hook.CurrentTool = this.Name;
        }

        private void ArcGisMapControl_OnSelectionChanged(object sender, EventArgs e)
        {
            if (m_Hook.CurrentTool != this.Name)
            {
                MoData.v_bVertexSelectionTracker = false;
            }
        }

        private void ArcGisMapControl_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            //判断节点编辑中的选择节点，用于刷新画节点
            if (MoData.v_bVertexSelectionTracker == false) return;

            IEnumFeature pEnumFeature = m_Hook.MapControl.Map.FeatureSelection as IEnumFeature;
            if (pEnumFeature == null || m_Hook.MapControl.Map.SelectionCount!=1) return;
            pEnumFeature.Reset();
            IFeature pFeature = pEnumFeature.Next();
            if (pFeature == null) return;

            ModPublic.DrawEditSymbol(pFeature.Shape, m_Hook.MapControl.ActiveView);
        }
    }


//    描述: 选择对象，并当鼠标停留在选择的对象上，可以移动对象；当只选中一个对象时，
//          双击可以显示节点，并且可以移动节点;若只通过点击鼠标来选择对象，则只选中一个对象，
//          若拉框选择则在框范围内的对象都被选中
//          热键：Ctrl+C 复制、Ctrl+X 剪切、Ctrl+V 粘贴、Esc 取消清空或粘贴的内容、
//          Ctrl+R 逆向
    public class ControlsEditSelFeature : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private ControlsMoveSelFeature m_pControlsMoveSelFeature;

        //类的方法
        public ControlsEditSelFeature()
        {
            base.m_category = "GeoCommon";
            base.m_caption = "EditSelFeature";
            base.m_message = "选择编辑";
            base.m_toolTip = "选择编辑";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.EditSelect.cur");
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            //支持快捷键
            m_MapControl.KeyIntercept = 1;  //esriKeyInterceptArrowKeys
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;
            MoData.v_bVertexSelectionTracker = false;

            //设置点选择容差
            ISelectionEnvironment pSelectEnv = new SelectionEnvironmentClass();
            double Length = ModPublic.ConvertPixelsToMapUnits(m_hookHelper.ActiveView, pSelectEnv.SearchTolerance);

            IPoint pPoint = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            IGeometry pGeometry = pPoint as IGeometry;
            ITopologicalOperator pTopo = pGeometry as ITopologicalOperator;
            IGeometry pBuffer = pTopo.Buffer(Length);

            //仅与框架别界相交地物会被选取
            pGeometry = m_MapControl.TrackRectangle() as IGeometry;
            bool bjustone = true;
            if (pGeometry != null)
            {
                if (pGeometry.IsEmpty)
                {
                    pGeometry = pBuffer;
                }
                else
                {
                    bjustone = false;
                }
            }
            else
            {
                pGeometry = pBuffer;
            }

            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer=m_MapControl.Map.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while(pLayer!=null)
            {
                if (pLayer.Visible == false)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                if (pFeatureLayer.Selectable == false)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }

                GetSelctionSet(pFeatureLayer, pGeometry, bjustone, Shift);

                pLayer = pEnumLayer.Next();
            }

            //触发Map选择发生变化事件
            ISelectionEvents pSelectionEvents = m_hookHelper.FocusMap as ISelectionEvents;
            pSelectionEvents.SelectionChanged();
            //刷新
            m_hookHelper.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, m_hookHelper.ActiveView.Extent);

            //鼠标在选择集的范围内则TOOL置为移动工具
            if (ModPublic.MouseOnSelection(pPoint, m_hookHelper.ActiveView) == true)
            {
                m_pControlsMoveSelFeature = new ControlsMoveSelFeature();
                m_pControlsMoveSelFeature.OnCreate(m_hookHelper.Hook);
                m_MapControl.CurrentTool = m_pControlsMoveSelFeature as ITool;
            }
        }
        private void GetSelctionSet(IFeatureLayer pFeatureLayer, IGeometry pGeometry, bool bjustone, int Shift)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            //没开启编辑的不可选择
            IDataset pDataset = pFeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
            if (!pWorkspaceEdit.IsBeingEdited()) return;
            switch (Shift)
            {
                case 1:   //增加选择结果集
                    ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultAdd, bjustone);
                    break;
                case 4:   //减少选择结果集
                    ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultSubtract, bjustone);
                    break;
                case 2:
                    ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultXOR, bjustone);
                    break;
                default:   //新建选择结果集
                    ModPublic.GetSelctionSet(pFeatureLayer, pGeometry, pFeatureClass, esriSelectionResultEnum.esriSelectionResultNew, bjustone);
                    break;
            }
        }


        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {

        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            //鼠标在选择集的范围内则TOOL置为移动工具
            IPoint pPnt = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (ModPublic.MouseOnSelection(pPnt, m_hookHelper.ActiveView) == true)
            {
                if (m_pControlsMoveSelFeature == null)
                {
                    m_pControlsMoveSelFeature = new ControlsMoveSelFeature();
                    m_pControlsMoveSelFeature.OnCreate(m_hookHelper.Hook);
                    m_MapControl.CurrentTool = m_pControlsMoveSelFeature as ITool;
                }
                else
                {
                    m_MapControl.CurrentTool = m_pControlsMoveSelFeature as ITool;
                }
            }
            else
            {
                m_MapControl.CurrentTool = this as ITool;
            }
        }

        public override void OnDblClick()
        {
           
        }

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }

//    描述: 移动对象，当鼠标停留在选择的对象之外，可以选择对象；当只有一个对象时，双击可以
//          显示节点，并且可以移动节点，热键：Ctrl+C 复制、Ctrl+X 剪切、Ctrl+V 粘贴、Esc
//          取消清空或粘贴的内容、Ctrl+R 逆向
    public class ControlsMoveSelFeature : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private bool m_bMouseDown;
        private IPoint m_pPtStart;     //移动前鼠标坐标
        private ISet m_pMoveSet;
        private INewLineFeedback m_pNewLineFeedback;
        private IMoveGeometryFeedback m_pMoveGeometryFeedback;

        public ControlsMoveSelFeature()
        {
            base.m_category = "GeoCommon";
            base.m_caption = "MoveSelFeature";
            base.m_message = "移动选择要素";
            base.m_toolTip = "移动选择要素";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.EditMove.cur");
            }
            catch
            {

            }
        }

        #region Overriden Class Methods

        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1 || m_MapControl.Map.SelectionCount==0) return;
            MoData.v_bVertexSelectionTracker = false;

            m_pNewLineFeedback = new NewLineFeedbackClass();
            m_pMoveGeometryFeedback = new MoveGeometryFeedbackClass();
            IDisplayFeedback pDisplayFeedback = m_pMoveGeometryFeedback as IDisplayFeedback;
            pDisplayFeedback.Display = m_hookHelper.ActiveView.ScreenDisplay;

            m_pPtStart = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            m_pNewLineFeedback.Display = m_hookHelper.ActiveView.ScreenDisplay;
            m_pNewLineFeedback.Start(m_pPtStart);

            //只刷新选中的地物
            IEnumFeature pEnumFeature = m_MapControl.Map.FeatureSelection as IEnumFeature;
            IInvalidArea pInvalidArea = new InvalidAreaClass();
            pInvalidArea.Display = m_hookHelper.ActiveView.ScreenDisplay;
            pInvalidArea.Add(pEnumFeature);

            pEnumFeature.Reset();
            IFeature pFeature = pEnumFeature.Next();
            m_pMoveSet = new SetClass();
            while (pFeature != null)
            {
                //'判断选中的要素是否启动了编辑
                IFeatureClass pFeatureClass = pFeature.Class as IFeatureClass;
                IDataset pDataset = pFeatureClass as IDataset;
                IWorkspaceEdit pWSEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWSEdit != null)
                {
                    if (pWSEdit.IsBeingEdited())
                    {
                        m_pMoveGeometryFeedback.AddGeometry(pFeature.Shape);
                        m_pMoveSet.Add(pFeature);
                    }
                }
                pFeature = pEnumFeature.Next();
            }

            m_pMoveGeometryFeedback.Start(m_pPtStart);

            m_bMouseDown = true;
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;
            if (m_MapControl.Map.SelectionCount == 0 || m_pMoveGeometryFeedback == null || m_pNewLineFeedback == null || m_pPtStart == null) return;
            IPoint pPtFinal=m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            m_pNewLineFeedback.Stop();
            m_pMoveGeometryFeedback.MoveTo(pPtFinal);

            //发生移动则移动所有要素
            if (m_pPtStart.X != pPtFinal.X || m_pPtStart.Y != pPtFinal.Y)
            {
                MoveAllFeature(m_pMoveSet, m_pPtStart, pPtFinal, MoData.v_CurWorkspaceEdit, m_MapControl.Map);
                m_MapControl.ActiveView.Refresh();
            }

            m_bMouseDown = false;
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            //鼠标在选择集的范围外为选择功能
            IPoint pPnt = m_hookHelper.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (ModPublic.MouseOnSelection(pPnt, m_hookHelper.ActiveView) == false && m_bMouseDown == false)
            {//若光标不在选择的对象上，为选择功能
                ControlsEditSelFeature clsSelectFeature = new ControlsEditSelFeature();
                clsSelectFeature.OnCreate(m_hookHelper.Hook);
                clsSelectFeature.OnClick();
                m_MapControl.CurrentTool = clsSelectFeature as ITool;
                return;
            }

            //鼠标在选择要素节点上时为节点移动功能
            if(m_MapControl.Map.SelectionCount==1)
            {
                IEnumFeature pEnumFeature=m_MapControl.Map.FeatureSelection as IEnumFeature;
                pEnumFeature.Reset();
                IFeature pFeature=pEnumFeature.Next();
                if (ModPublic.MouseOnFeatureVertex(pPnt, pFeature, m_hookHelper.ActiveView) == true && MoData.v_bVertexSelectionTracker ==true)
                {
                    ControlsMoveVertex pControlsMoveVertex = new ControlsMoveVertex();
                    pControlsMoveVertex.OnCreate(m_hookHelper.Hook);
                    pControlsMoveVertex.OnClick();
                    m_MapControl.CurrentTool = pControlsMoveVertex as ITool;
                    return;
                }
            }

            //捕捉节点
            if (MoData.v_bSnapStart)
            {
                ModPublic.SnapPoint(pPnt, m_hookHelper.ActiveView);
            }

            if (Button != 1) return;
            //进行移动
            if (m_MapControl.Map.SelectionCount > 0 && m_pMoveGeometryFeedback != null && m_pNewLineFeedback!=null)
            {
                m_pMoveGeometryFeedback.MoveTo(pPnt);
                m_pNewLineFeedback.MoveTo(pPnt);
            }
        }

        public override void OnDblClick()
        {
            IEnumFeature pEnumFeature = m_MapControl.Map.FeatureSelection as IEnumFeature;
            if (pEnumFeature == null) return;
            pEnumFeature.Reset();
            IFeature pFeature = pEnumFeature.Next();
            if (pFeature == null) return;

            ModPublic.DrawEditSymbol(pFeature.Shape, m_hookHelper.ActiveView);
            MoData.v_bVertexSelectionTracker = true;
        }

        public override bool Deactivate()
        {
            return true;
        }

        #endregion

        //移动所有要素
        private void MoveAllFeature(ISet pALLMoveSet, IPoint pPtStart, IPoint pPtFinal, IWorkspaceEdit pCurWorkspaceEdit,IMap pMap)
        {
            pCurWorkspaceEdit.StartEditOperation();

            ILine pLine = new LineClass();
            pLine.PutCoords(pPtStart, pPtFinal);
            //将空间参考赋给移动的线
            pLine.SpatialReference = pMap.SpatialReference;

            pALLMoveSet.Reset();
            IFeatureEdit pFeatureEdit = pALLMoveSet.Next() as IFeatureEdit;
            while (pFeatureEdit != null)
            {
                pFeatureEdit.MoveSet(pALLMoveSet, pLine);
                pFeatureEdit = pALLMoveSet.Next() as IFeatureEdit;
            }

            pCurWorkspaceEdit.StopEditOperation();

        }
              
    }


    //描述: 移动节点,指针不在节点上双击鼠标时，回到选择状态
    public class ControlsMoveVertex : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private bool m_bMouseDown;

        private IFeature m_pFeature;
        private double m_dblTolearance;
        private int m_HitSegmentIndex;            //选中的节点
        private IVertexFeedback m_pVertexFeed;    //节点移动类

        private IPoint m_pSnapPoint;              //开启捕捉时移动以捕捉点为准

        public ControlsMoveVertex()
        {
            base.m_category = "GeoCommon";
            base.m_caption = "MoveVertex";
            base.m_message = "移动要素节点";
            base.m_toolTip = "移动要素节点";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.MoveVertex.cur");
            }
            catch
            {

            }
        }

        #region Overriden Class Methods

        public override bool Enabled
        {
            get
            {
                if (m_MapControl.Map.SelectionCount != 1) return false;
                IEnumFeature pSelected = m_MapControl.Map.FeatureSelection as IEnumFeature;
                IFeature pFeature = pSelected.Next();
                IFeatureClass pFeatClass=pFeature.Class as IFeatureClass;
                IDataset pDataset = pFeatClass as IDataset;
                IWorkspaceEdit pWSEdit = pDataset.Workspace as IWorkspaceEdit;
                //如果当前选中要素所在图层启动了编辑，且不是点状要素，则设置节点编辑可用
                if (pFeatClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeature.Shape.Dimension != esriGeometryDimension.esriGeometry0Dimension && pWSEdit.IsBeingEdited())
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {

            IEnumFeature pEnumFeature = m_MapControl.Map.FeatureSelection as IEnumFeature;
            if (pEnumFeature == null || m_MapControl.Map.SelectionCount != 1) return;
            pEnumFeature.Reset();
            IFeature pFeature = pEnumFeature.Next();
            if (pFeature == null) return;

            //画出图形各节点
            if (MoData.v_bVertexSelectionTracker == true)
            {
                ModPublic.DrawEditSymbol(pFeature.Shape, m_MapControl.ActiveView);
            }

            //设置点选容差 
            ISelectionEnvironment pSelectEnv = new SelectionEnvironmentClass();
            m_dblTolearance = ModPublic.ConvertPixelsToMapUnits(m_hookHelper.ActiveView, pSelectEnv.SearchTolerance);

            m_pFeature = pFeature;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1 || m_pFeature==null) return;

            IPoint pPnt = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            //获取选中的节点
            IHitTest pHitTest = m_pFeature.Shape as IHitTest;
            IPoint pHitPoint = new PointClass();
            double dblHitDistance=0; 
            int lPart=0;
            bool bRight=false;
            bool bHitTest=pHitTest.HitTest(pPnt,m_dblTolearance,esriGeometryHitPartType.esriGeometryPartVertex,pHitPoint,ref dblHitDistance,ref lPart,ref m_HitSegmentIndex,ref bRight);
            if (bHitTest == false) return;
            if (m_HitSegmentIndex == -1) return;   //未选中节点

            ISegmentCollection pSegmentCollection = m_pFeature.Shape as ISegmentCollection;
            m_pVertexFeed = new VertexFeedbackClass();
            m_pVertexFeed.Display = m_MapControl.ActiveView.ScreenDisplay;

            //检查是否点击第一点
            if (m_HitSegmentIndex == 0)
            {
                //如果是第一点且是Polygon，加最后一段 (FromPoint as anchor)
                if (m_pFeature.Shape is IPolygon)
                {
                    m_pVertexFeed.AddSegment(pSegmentCollection.get_Segment(pSegmentCollection.SegmentCount - 1), true);
                }
            }
            else
            {
                //如果不是第一点，并加上一段(FromPoint as anchor)
                m_pVertexFeed.AddSegment(pSegmentCollection.get_Segment(m_HitSegmentIndex - 1), true);
            }

            //检查是否点击最后一个节点
            if (m_HitSegmentIndex == pSegmentCollection.SegmentCount)
            {
                //如果是点击最后一个节点,并且是polygon，加第一段
                if (m_pFeature.Shape is IPolygon)
                {
                    m_pVertexFeed.AddSegment(pSegmentCollection.get_Segment(0), false);
                }
            }
            else
            {
                //如果不是最后一个节点，就加下一段(ToPoint as anchor)
                m_pVertexFeed.AddSegment(pSegmentCollection.get_Segment(m_HitSegmentIndex), false);
            }

            m_bMouseDown = true;
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (Button != 1 || m_pFeature == null || m_pVertexFeed == null || m_HitSegmentIndex == -1) return;

            IPoint pPnt = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            //如果开启捕捉则以捕捉到的点为准
            if (MoData.v_bSnapStart && m_pSnapPoint!=null)
            {
                pPnt=m_pSnapPoint;
            }

            ISegmentCollection pSegmentCollection = m_pFeature.Shape as ISegmentCollection;
            

            //检查是否点击第一点
            if (m_HitSegmentIndex == 0)
            {
                //如果是第一点且是Polygon，更新最后一段
                if (m_pFeature.Shape is IPolygon)
                {
                    pPnt.Z = pSegmentCollection.get_Segment(pSegmentCollection.SegmentCount - 1).ToPoint.Z;
                    pSegmentCollection.get_Segment(pSegmentCollection.SegmentCount - 1).ToPoint = pPnt;
                }
            }
            else
            {
                //如果不是第一点，更新上一段(FromPoint as anchor)
                pPnt.Z = pSegmentCollection.get_Segment(m_HitSegmentIndex - 1).ToPoint.Z;
                pSegmentCollection.get_Segment(m_HitSegmentIndex - 1).ToPoint = pPnt;
            }

            //检查是否点击最后一个节点
            if (m_HitSegmentIndex == pSegmentCollection.SegmentCount)
            {
                //如果是点击最后一个节点,并且是polygon，更新第一段
                if (m_pFeature.Shape is IPolygon)
                {
                    pPnt.Z = pSegmentCollection.get_Segment(0).FromPoint.Z;
                    pSegmentCollection.get_Segment(0).FromPoint = pPnt;
                }
            }
            else
            {
                //如果不是最后一个节点，更新下一段(ToPoint as anchor)
                pPnt.Z = pSegmentCollection.get_Segment(m_HitSegmentIndex).FromPoint.Z;
                pSegmentCollection.get_Segment(m_HitSegmentIndex).FromPoint = pPnt;
            }

            StoreFeature(m_pFeature, pSegmentCollection, MoData.v_CurWorkspaceEdit);

            m_MapControl.ActiveView.Refresh();
            m_pVertexFeed = null;
            m_bMouseDown = false;
            m_HitSegmentIndex = 0;
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            if (m_pFeature == null) return;
            IPoint pPnt = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            //鼠标在选择集的范围外为选择功能
            if (ModPublic.MouseOnSelection(pPnt, m_hookHelper.ActiveView) == false && m_bMouseDown==false)
            {//若光标不在选择的对象上，为选择功能
                ControlsEditSelFeature clsSelectFeature = new ControlsEditSelFeature();
                clsSelectFeature.OnCreate(m_hookHelper.Hook);
                clsSelectFeature.OnClick();
                m_MapControl.CurrentTool = clsSelectFeature as ITool;
                return;
            }
            //范围内为移动要素功能且不在要素节点上
            else if (ModPublic.MouseOnFeatureVertex(pPnt, m_pFeature, m_hookHelper.ActiveView) == false && m_bMouseDown == false)    
            {
                ControlsMoveSelFeature pControlsMoveSelFeature = new ControlsMoveSelFeature();
                pControlsMoveSelFeature.OnCreate(m_hookHelper.Hook);
                pControlsMoveSelFeature.OnClick();
                m_MapControl.CurrentTool = pControlsMoveSelFeature as ITool;
                return;
            }


            if (m_pVertexFeed==null) return;
            //捕捉节点
            if (MoData.v_bSnapStart)
            {
                m_pSnapPoint=ModPublic.SnapPoint(pPnt, m_hookHelper.ActiveView);
            }

            IHitTest pHitTest = m_pFeature.Shape as IHitTest;
            IPoint pHitPoint = new PointClass();
            double dblHitDistance = 0;
            int lPart = 0;
            int intHitSegmentIndex = 0;
            bool bRight = false;
            bool bHitTest = pHitTest.HitTest(pPnt, m_dblTolearance, esriGeometryHitPartType.esriGeometryPartVertex, pHitPoint, ref dblHitDistance, ref lPart, ref intHitSegmentIndex, ref bRight);

            if (m_pSnapPoint != null && MoData.v_bSnapStart)
            {
                m_pVertexFeed.MoveTo(m_pSnapPoint);
            }
            else
            {
                m_pVertexFeed.MoveTo(pPnt);
            }
        }

        public override void OnDblClick()
        {

        }

        public override bool Deactivate()
        {
            return true;
        }

        #endregion

        //存储Feature
        private void StoreFeature(IFeature pFeature, ISegmentCollection pSegcol, IWorkspaceEdit pCurWorkspaceEdit)
        {
            pCurWorkspaceEdit.StartEditOperation();

            pFeature.Shape = pSegcol as IGeometry;
            pFeature.Store();
            pCurWorkspaceEdit.StopEditOperation();
        }
    }

}
