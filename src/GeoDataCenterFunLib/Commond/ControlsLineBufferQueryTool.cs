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

namespace GeoDataCenterFunLib
{
    public class ControlsLineBufferQueryTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;
        public ControlsLineBufferQueryTool()
        {
            base._Name = "GeoDataCenterFunLib.ControlsLineBufferQueryTool";
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
                try
                {
                    if (_AppHk.MapControl.Map.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
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
            Plugin.Application.IAppGisUpdateRef phook = _AppHk as Plugin.Application.IAppGisUpdateRef;
            SysCommon.BottomQueryBar pBar= phook.QueryBar;
            if (pBar.m_WorkSpace == null)
            {
                pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            }
            if (pBar.ListDataNodeKeys == null)
            {
                pBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
            }
            LineBufferQueryToolClass pTool = _cmd as LineBufferQueryToolClass;
            pTool.WriteLog = this.WriteLog; //ygc 2012-9-12 是否写日志
            pTool.GetQueryBar(pBar);
            _cmd.OnClick();
            if (_AppHk.CurrentControl is IMapControl2)
            {
                _AppHk.MapControl.CurrentTool = _tool;
            }
            else
            {
                _AppHk.PageLayoutControl.CurrentTool = _tool;
            }

            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _tool = new LineBufferQueryToolClass(pAppForm.MainForm);
            LineBufferQueryToolClass TempTool = _tool as LineBufferQueryToolClass;
            TempTool.WriteLog = WriteLog;
            _cmd = TempTool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }


    public class LineBufferQueryToolClass : BaseTool
    {
        private IHookHelper m_hookHelper;
        public IMapControlDefault m_MapControl;

        private INewLineFeedback m_pNewLineFeedback;
        private SysCommon.BottomQueryBar _QueryBar;
        private Form m_mainFrm;
        private frmQuery m_frmQuery;
        private enumQueryMode m_enumQueryMode;
        private frmBufferSet m_frmBufferSet = null;
        public void GetQueryBar(SysCommon.BottomQueryBar QueryBar)
        {
            _QueryBar = QueryBar;
        }
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

            m_enumQueryMode = enumQueryMode.Visiable ;//changed by chulili 20110707现不再有当前编辑图层选项
            //m_enumQueryMode = enumQueryMode.CurEdit ;   //changed by chulili 2011-04-15 默认当前编辑图层
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_frmQuery == null)
            {
               
                m_frmQuery = new frmQuery(m_MapControl, m_enumQueryMode);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }
try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.MapIdentify.cur");
            }
            catch
            {

            }
            //deleted by chulili 20110731
            //m_frmQuery.Show();
            //m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, null);
        }

        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumQueryMode = m_frmQuery.QueryMode;
            m_frmQuery = null;
            if (m_frmBufferSet!=null) m_frmBufferSet.setBufferGeometry(null);//added by chulili 20110731
            //m_MapControl.ActiveView.Refresh();
            //deleted by chulili 20110731
            //base.m_cursor = Cursors.Default;//鼠标回到默认状态 xisheng 20110722
            //m_MapControl.CurrentTool = null; 
            //end deleted by chulili
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
                if (m_frmBufferSet != null)
                {
                    SysCommon.ScreenDraw.list.Remove(m_frmBufferSet.BufferSetAfterDraw);
                    m_frmBufferSet.setBufferGeometry(null);
                    m_frmBufferSet = null;
                }
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
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("线缓冲查询");//xisheng 日志记录 0928;
            }
            //清除上次的所有元素
            (m_MapControl.Map as IGraphicsContainer).DeleteAllElements();
            m_frmBufferSet = new frmBufferSet(pPolyline as IGeometry, m_MapControl.Map, m_frmQuery);
            IGeometry pGeometry = m_frmBufferSet.GetBufferGeometry();
            if (pGeometry == null || m_frmBufferSet.Res == false) return;
            _QueryBar.m_pMapControl = m_MapControl;
            _QueryBar.EmergeQueryData(m_MapControl.ActiveView.FocusMap, pGeometry, m_frmBufferSet.pesriSpatialRelEnum);
            try
            {
                DevComponents.DotNetBar.Bar pBar = _QueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
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
            //m_frmQuery.Show();
           // m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, pGeometry,m_frmBufferSet.pesriSpatialRelEnum);
        }

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }
}
