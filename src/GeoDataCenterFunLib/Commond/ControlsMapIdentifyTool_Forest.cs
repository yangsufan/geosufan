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

namespace GeoDataCenterFunLib
{
    public class ControlsMapIdentifyToolForest : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsMapIdentifyToolForest()
        {
            base._Name = "GeoDataCenterFunLib.ControlsMapIdentifyToolForest";
            base._Caption = "查询";
            base._Tooltip = "查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "选择地物进行查询";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerIdentify;
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Checked
        {
            get
            {
                if (_AppHk.CurrentTool != this.Name) return false;
                return true;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;

                if (_AppHk.CurrentControl is IMapControl2) 
                {
                    try
                    {
                        if (_AppHk.MapControl.LayerCount == 0)
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
                else
                    return false;

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
            _AppHk.MapControl.CurrentTool = _tool;
            _AppHk.CurrentTool = this.Name;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("使用二维工具:一般查询");//xisheng 日志记录
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _cmd = new ControlsMapIdentifyForest(pAppForm.MainForm);
            _tool = _cmd as ITool;
            
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }

    public class ControlsMapIdentifyForest : BaseTool
    {
        private IHookHelper m_hookHelper;
        public IMapControlDefault m_MapControl;

        private Form m_mainFrm;
        private frmQueryForest m_frmQuery;
        private enumQueryMode m_enumQueryMode;
        private IPoint m_pPoint;
        //类的方法
        public ControlsMapIdentifyForest(Form mainFrm)
        {
            m_mainFrm = mainFrm;
            base.m_category = "GeoCommon";
            base.m_caption = "MapIdentify";
            base.m_message = "要素查询";
            base.m_toolTip = "要素查询";
            base.m_name = base.m_category + "_" + base.m_caption;
            //try
            //{
            //    base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.MapIdentify.cur");
            //}
            //catch
            //{

            //}
            System.IO.Stream strm = GetType().Assembly.GetManifestResourceStream("GeoDataCenterFunLib.Resources.MapIdentify.cur");
            base.m_cursor = new System.Windows.Forms.Cursor(strm);
            strm.Close();
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

            m_enumQueryMode = enumQueryMode.Visiable ;
            //m_enumQueryMode = enumQueryMode.CurEdit; //changed by chulili 2011-04-15 默认当前编辑图层
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQueryForest(m_MapControl);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }
        }

        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmQuery = null;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;

            ESRI.ArcGIS.Geometry.IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            m_pPoint = pPoint;
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            int iPiexl = 3;
            double iMapTolerance = ConvertPixelsToMapUnits(m_MapControl.ActiveView, iPiexl);

            IGeometry pGeometry = null;

            ITopologicalOperator pTopo = (ITopologicalOperator)m_pPoint;
            if (pTopo != null) pGeometry = pTopo.Buffer(iMapTolerance);

            
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQueryForest(m_MapControl);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }
            //ygc 2012-8-28 将查询结果数据在主窗体下方显示
           // _QuerBar.m_pMapControl = m_MapControl;
           // _QuerBar.EmergeQueryData(m_MapControl.ActiveView.FocusMap, pGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
            m_frmQuery.Show();
            ///ZQ 20111119  modify
           m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, pGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
        }

        private double ConvertPixelsToMapUnits(IActiveView pActiveView, int pixelUnits)
        {
            tagRECT deviceRECT = pActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
            int pixelExtent = deviceRECT.right - deviceRECT.left;
            double realWorldDisplayExtent = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }

        //public override void OnMouseMove(int Button, int Shift, int X, int Y)
        //{
        //    IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

        //    if (m_pNewEnvelope != null)
        //    {
        //        m_pNewEnvelope.MoveTo(pPoint);
        //    }
        //}

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }

}
