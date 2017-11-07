using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace GeoUtilities
{
    public class ControlsMapIdentifyTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsMapIdentifyTool()
        {
            base._Name = "GeoUtilities.ControlsMapIdentifyTool";
            base._Caption = "属性查询";
            base._Tooltip = "属性查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "选择地物进行查询";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerIdentify;
            //base._Image = "";
            //base._Category = "";
        }


        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                
                if (_AppHk.CurrentControl is ISceneControl) return false;  //为了只有效于2维控件
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }
               
        public override void OnClick()
        {
            if (_tool == null || _cmd == null || _AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            Plugin.Application.AppGIS phook = _AppHk as Plugin.Application.AppGIS;
            //SysCommon.BottomQueryBar pBar = phook.QueryBar;
           // if (pBar.m_WorkSpace == null)
           // {
           //     pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
          //  }
            ControlsMapIdentify pTool = _cmd as ControlsMapIdentify;
           // pTool.GetQueryBar(pBar);
            _AppHk.MapControl.CurrentTool = _tool;
            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _tool = new ControlsMapIdentify(pAppForm.MainForm);
            _cmd = _tool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }

    public class ControlsMapIdentify : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private Form m_mainFrm;
        private frmQuery m_frmQuery;
        private enumQueryMode m_enumQueryMode;
        private SysCommon.BottomQueryBar _QuerBar;
        private IPoint m_pPoint;
        private INewEnvelopeFeedback m_pNewEnvelope;
        public void GetQueryBar(SysCommon.BottomQueryBar QueryBar)
        {
            _QuerBar = QueryBar;
        }
        //类的方法
        public ControlsMapIdentify(Form mainFrm)
        {
            m_mainFrm = mainFrm;
            base.m_category = "GeoCommon";
            base.m_caption = "MapIdentify";
            base.m_message = "要素查询";
            base.m_toolTip = "要素查询";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.MapIdentify.cur");
            }
            catch
            {

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

            m_enumQueryMode = enumQueryMode.Top;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuery(m_MapControl,m_enumQueryMode);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed+=new FormClosedEventHandler(frmQuery_FormClosed);
            }

            //m_frmQuery.Show();
            //m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, null);
        }

        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumQueryMode = m_frmQuery.QueryMode;
            m_frmQuery = null;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;

            ESRI.ArcGIS.Geometry.IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            if (m_pNewEnvelope == null)
            {
                pPoint.SpatialReference = m_MapControl.Map.SpatialReference;
                m_pNewEnvelope = new NewEnvelopeFeedbackClass();

                m_pNewEnvelope.Display = m_MapControl.ActiveView.ScreenDisplay;

                m_pNewEnvelope.Start(pPoint);
            }

            m_pPoint = pPoint;
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            if (m_pNewEnvelope == null) return;

            IEnvelope pEnvelope = m_pNewEnvelope.Stop();
            int iPiexl = 3;
            double iMapTolerance = ConvertPixelsToMapUnits(m_MapControl.ActiveView, iPiexl);

            IGeometry pGeometry = null;
            if (pEnvelope == null || pEnvelope.IsEmpty)
            {
                ITopologicalOperator pTopo = (ITopologicalOperator)m_pPoint;
                if (pTopo != null) pGeometry = pTopo.Buffer(iMapTolerance);
            }
            else
            {
                pEnvelope.SpatialReference = m_MapControl.ActiveView.FocusMap.SpatialReference;
                pGeometry = pEnvelope;
            }
            m_pNewEnvelope = null;
            
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuery(m_MapControl, m_enumQueryMode);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }

            m_frmQuery.Show();
            m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, pGeometry);
           // _QuerBar.m_pMapControl = m_MapControl;
           // _QuerBar.EmergeQueryData(m_MapControl.ActiveView.FocusMap, pGeometry,esriSpatialRelEnum.esriSpatialRelIntersects);
        }

        private double ConvertPixelsToMapUnits(IActiveView pActiveView, int pixelUnits)
        {
            tagRECT deviceRECT = pActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
            int pixelExtent = deviceRECT.right - deviceRECT.left;
            double realWorldDisplayExtent = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            if (m_pNewEnvelope != null)
            {
                m_pNewEnvelope.MoveTo(pPoint);
            }
        }

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }

}
