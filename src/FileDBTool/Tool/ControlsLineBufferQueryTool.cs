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

namespace FileDBTool
{
    public class ControlsLineBufferQueryTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppFileRef _AppHk;

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
                if (_AppHk.MapControl.LayerCount == 0) return false;
                if (_AppHk.ProjectTree == null) return false;
                if (_AppHk.ProjectTree.SelectedNode == null) return false;
                if (_AppHk.ProjectTree.SelectedNode.DataKey == null) return false;
                if (_AppHk.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString()) return false;
                if (_AppHk.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString()) return false;
                if (_AppHk.ProjectTree.SelectedNode.ImageIndex == 1) return false;
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
            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppFileRef;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _tool = new LineBufferQueryToolClass(pAppForm.MainForm,_AppHk);
            _cmd = _tool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }


    public class LineBufferQueryToolClass : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        private Plugin.Application.IAppFileRef m_hook;
        private INewLineFeedback m_pNewLineFeedback;

        private Form m_mainFrm;

        //类的方法
        public LineBufferQueryToolClass(Form mainFrm,Plugin.Application.IAppFileRef hook)
        {
            m_mainFrm = mainFrm;
            m_hook = hook;
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
          
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, m_MapControl.ActiveView.Extent);
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

            frmBufferSet pFrmBufSet = new frmBufferSet(pPolyline as IGeometry, m_MapControl.Map);
            IGeometry pGeometry = pFrmBufSet.GetBufferGeometry();
            if (pGeometry == null || pFrmBufSet.Res == false) return;

            //==================================================================================================
            //执行查询数据操作
            ModDBOperator.QueryDataByGeometry(pGeometry, m_hook);
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
