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

namespace GeoUtilities
{
    public class ControlsLineBufferQueryTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.AppGIS _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsLineBufferQueryTool()
        {
            base._Name = "GeoUtilities.ControlsLineBufferQueryTool";
            base._Caption = "线缓冲查询";
            base._Tooltip = "线缓冲查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "线缓冲查询";

            //base._Cursor=new Cursor(GetType(), "Resources.BufferQuery.cur");
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                if (_tool == null || _cmd == null || _AppHk == null) return false;
                if (_AppHk.MapControl == null) return false;
                return true;
            }
        }

        public override bool Checked
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.CurrentTool != this.Name) return false;
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
            _AppHk.MapControl.CurrentTool = _tool;
            Plugin.Application.AppGIS phook = _AppHk as Plugin.Application.AppGIS;
            SysCommon.BottomQueryBar pBar = phook.QueryBar;
            if (pBar.m_WorkSpace == null)
            {
                pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            }
            if (pBar.ListDataNodeKeys == null)
            {
                pBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
            }
            LineBufferQueryToolClass pTool = _cmd as LineBufferQueryToolClass;
            pTool.GetQueryBar(pBar);
            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.AppGIS ;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _tool = new LineBufferQueryToolClass(pAppForm.MainForm);
            _cmd = _tool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }


    public class LineBufferQueryToolClass : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;

        private INewLineFeedback m_pNewLineFeedback;

        private Form m_mainFrm;
        private frmQuery m_frmQuery;
        private enumQueryMode m_enumQueryMode;
        private SysCommon.BottomQueryBar _QuerBar;
        public void GetQueryBar(SysCommon.BottomQueryBar QueryBar)
        {
            _QuerBar = QueryBar;
        }
        //类的方法
        public LineBufferQueryToolClass(Form mainFrm)
        {
            m_mainFrm = mainFrm;
            base.m_category = "GeoCommon";
            base.m_caption = "LineBufferQuery";
            base.m_message = "线缓冲查询";
            base.m_toolTip = "线缓冲查询";
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
            m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, m_MapControl.ActiveView.Extent);
        }

        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumQueryMode = m_frmQuery.QueryMode;
            m_frmQuery = null;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;

            IPoint pPnt = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (m_pNewLineFeedback == null) //第一次按下
            {
                m_pNewLineFeedback = new NewLineFeedbackClass();

                IRgbColor pRGB = new RgbColorClass();
                ISimpleLineSymbol pSLnSym = m_pNewLineFeedback.Symbol as ISimpleLineSymbol;
                pRGB.Red = 255;
                pRGB.Blue = 0;
                pRGB.Green = 0;

                pSLnSym.Color = pRGB;
                pSLnSym.Style = esriSimpleLineStyle.esriSLSSolid;
                m_pNewLineFeedback.Display = m_MapControl.ActiveView.ScreenDisplay;
                m_pNewLineFeedback.Start(pPnt);
            }
            else                    //将点加入到工具中去
            {
                m_pNewLineFeedback.AddPoint(pPnt);
            }
        }

        //鼠标移动， 将工具也移动到点的位置
        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            base.OnMouseMove(Button, Shift, X, Y);

            if (m_pNewLineFeedback != null)
            {
                IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
                m_pNewLineFeedback.MoveTo(pPoint);
            }
        }

         //双击则创建该线，并弹出缓冲窗体
        public override void OnDblClick()
        {
            //获取折线 并获取当前视图的屏幕显示
            if (m_pNewLineFeedback == null) return;

            IPolyline pPolyline = m_pNewLineFeedback.Stop();
            m_pNewLineFeedback = null;
            pPolyline.Project(m_MapControl.Map.SpatialReference);
            
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuery(m_MapControl, m_enumQueryMode);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }

            frmBufferSet pFrmBufSet = new frmBufferSet(pPolyline as IGeometry, m_MapControl.Map, m_frmQuery);
            IGeometry pGeometry = pFrmBufSet.GetBufferGeometry();
            if (pGeometry == null || pFrmBufSet.Res == false) return;

           // m_frmQuery.Show();
           // m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, pGeometry);
            _QuerBar.m_pMapControl = m_MapControl;
            _QuerBar.EmergeQueryData(m_MapControl.ActiveView.FocusMap, pGeometry, pFrmBufSet.pesriSpatialRelEnum);
            try
            {
                DevComponents.DotNetBar.Bar pBar = _QuerBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                if (pBar != null)
                {
                    pBar.AutoHide = false;
                    //pBar.SelectedDockTab = 1;
                    int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                    pBar.SelectedDockTab = tmpindex;
                }
            }
            catch
            { }
        }

        public override void OnKeyUp(int keyCode, int Shift)
        {
            if (Shift != 2 && keyCode != 90) return;
            if (m_pNewLineFeedback == null) return;
            IPolyline pPolyline = m_pNewLineFeedback.Stop();

            m_pNewLineFeedback = null;
            if (pPolyline == null) return;
            IPointCollection pntCol = pPolyline as IPointCollection;

            m_pNewLineFeedback = new NewLineFeedbackClass();

            IRgbColor pRGB = new RgbColorClass();
            ISimpleLineSymbol pSLnSym = m_pNewLineFeedback.Symbol as ISimpleLineSymbol;
            pRGB.Red = 255;
            pRGB.Blue = 0;
            pRGB.Green = 0;

            pSLnSym.Color = pRGB;
            pSLnSym.Style = esriSimpleLineStyle.esriSLSSolid;
            m_pNewLineFeedback.Display = m_MapControl.ActiveView.ScreenDisplay;

            m_pNewLineFeedback.Start(pntCol.get_Point(0));
            for (int i = 1; i < pntCol.PointCount - 1; i++)
            {
                m_pNewLineFeedback.AddPoint(pntCol.get_Point(i));
            }
        }

        public override void Refresh(int hDC)
        {
            if (m_pNewLineFeedback != null)
            {
                m_pNewLineFeedback.Refresh(hDC);
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
