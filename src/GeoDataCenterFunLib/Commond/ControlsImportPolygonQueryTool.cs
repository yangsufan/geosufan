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
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDataCenterFunLib
{
    public class ControlsImportPolygonQueryTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsImportPolygonQueryTool()
        {
            base._Name = "GeoDataCenterFunLib.ControlsImportPolygonQueryTool";
            base._Caption = "导入范围查询";
            base._Tooltip = "导入范围查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "导入范围查询";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerIdentify;
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
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
            SysCommon.BottomQueryBar pBar = phook.QueryBar;
            if (pBar.m_WorkSpace == null)
            {
                pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            }
            if (pBar.ListDataNodeKeys == null)
            {
                pBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
            }
            ImportPolygonQueryToolClass pTool = _cmd as ImportPolygonQueryToolClass;
            pTool.GetQueryBar(pBar);
            _cmd.OnClick();
            //if (_AppHk.CurrentControl is IMapControl2)
            //{
            //    _AppHk.MapControl.CurrentTool = _tool;
            //}
            //else
            //{
            //    _AppHk.PageLayoutControl.CurrentTool = _tool;
            //}

            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
            _tool = new ImportPolygonQueryToolClass(pAppForm.MainForm, _AppHk.MapControl.Map );
            ImportPolygonQueryToolClass tempTool = _tool as ImportPolygonQueryToolClass;
            tempTool.WriteLog = this.WriteLog;
            _cmd = tempTool as ICommand;
            _cmd.OnCreate(_AppHk.MapControl);
        }
    }

    public class ImportPolygonQueryToolClass : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        private SysCommon.BottomQueryBar _QueryBar;
        private INewPolygonFeedback m_pNewPolygonFeedback;

        private Form m_mainFrm;
        private frmQuery m_frmQuery;
        private enumQueryMode m_enumQueryMode;
        //private IScreenDisplay m_pScreenDisplay;
        //private IActiveViewEvents_Event m_pActiveViewEvents;
        //IActiveView m_pActiveView = null;
        private IGeometry  m_Polygon;
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
        public ImportPolygonQueryToolClass(Form mainFrm, ESRI.ArcGIS.Carto.IMap pMap)
        {
            m_mainFrm = mainFrm;
            base.m_category = "GeoCommon";
            base.m_caption = "ImportPolygonQuery";
            base.m_message = "导入范围查询";
            base.m_toolTip = "导入范围查询";
            base.m_name = base.m_category + "_" + base.m_caption;
            // m_pActiveView = pMap as IActiveView;

            ////m_pActiveViewEvents = m_pActiveView as IActiveViewEvents_Event;IActiveView
            //m_pScreenDisplay = m_pActiveView.ScreenDisplay;
          
            //try
            //{
            //    m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);

            //}
            //catch
            //{
            //}
        }
        ///ZQ 2011  1128
        //被擦去时画出
        internal void ImportPolygonQueryAfterDraw(IDisplay Display, esriViewDrawPhase phase)
        //private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            if(m_frmQuery!=null && !m_frmQuery.IsDisposed)
            {
                if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR(null);
            }
        }
        /// <summary>
        /// ZQ 2011 1129  modify
        /// </summary>
        /// <param name="pPolygon"></param>
        private void drawgeometryXOR(IGeometry pPolygon)
        {
         
            //联动
            //this.sliderBuffer.Value = Convert.ToInt32(dblBuffLen.Text);
            //获得缓冲范围

            IScreenDisplay pScreenDisplay = m_MapControl.ActiveView.ScreenDisplay;
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 255;
                pRGBColor.Green = 170;
                pRGBColor.Blue = 0;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 1.0;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

                pFillSymbol.Color = pRGBColor;
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

                pScreenDisplay.StartDrawing(pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawPolygon(pPolygon);
                    m_Polygon = pPolygon;
                }
                //存在已画出的多边形
                else
                {
                    if (m_Polygon != null)
                    {
                        pScreenDisplay.DrawPolygon(m_Polygon);
                    }
                }

                pScreenDisplay.FinishDrawing();

            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制缓冲范围出错:" + ex.Message, "提示");
                pFillSymbol = null;
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
            //m_pActiveViewEvents = m_MapControl.ActiveView  as IActiveViewEvents_Event;
            m_enumQueryMode = enumQueryMode.Visiable ;//changed by chulili 20110707现不再有当前编辑图层选项
            //m_enumQueryMode = enumQueryMode.CurEdit;  //changed by chulili 2011-04-15 默认当前编辑图层

            
        }
        #region
        /// <summary>
        /// 从文件路径获得一个PolyGon
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns></returns>
        private IPolygon GetPolyGonFromFile(string path)
        {
            IPolygon pGon = null;
            if (path.EndsWith(".mdb"))
            {
                string errmsg = "";
                IWorkspaceFactory pwf = new AccessWorkspaceFactoryClass();
                IWorkspace pworkspace = pwf.OpenFromFile(path, 0);
                IEnumDataset pEnumdataset = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumdataset.Reset();
                IDataset pDataset = pEnumdataset.Next();
                while (pDataset != null)
                {
                    IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                    if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    {
                        pDataset = pEnumdataset.Next();
                        continue;
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {
                            pGon = pFeature.Shape as IPolygon;
                            break;
                        }
                        else
                        {
                            pDataset = pEnumdataset.Next();
                            continue;
                        }
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {

                            IPolyline pPolyline = pFeature.Shape as IPolyline;
                            pGon = GetPolygonFormLine(pPolyline);
                            if (pGon.IsClosed == false)
                            {
                                errmsg = "选择的要素不能构成封闭多边形！";
                                pGon = null;
                                pDataset = pEnumdataset.Next();
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                }
                if (pGon == null)
                {
                    IEnumDataset pEnumdataset1 = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                    pEnumdataset1.Reset();
                    pDataset = pEnumdataset1.Next();
                    while (pDataset != null)
                    {
                        IFeatureDataset pFeatureDataset = pDataset as IFeatureDataset;
                        IEnumDataset pEnumDataset2 = pFeatureDataset.Subsets;
                        pEnumDataset2.Reset();
                        IDataset pDataset1 = pEnumDataset2.Next();
                        while (pDataset1 != null)
                        {
                            if (pDataset1 is IFeatureClass)
                            {

                                IFeatureClass pFeatureclass = pDataset1 as IFeatureClass;
                                if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                                {
                                    pDataset1 = pEnumDataset2.Next();
                                    continue;
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {
                                        pGon = pFeature.Shape as IPolygon;
                                        break;
                                    }
                                    else
                                    {
                                        pDataset1 = pEnumDataset2.Next();
                                        continue;
                                    }
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {

                                        IPolyline pPolyline = pFeature.Shape as IPolyline;
                                        pGon = GetPolygonFormLine(pPolyline);
                                        if (pGon.IsClosed == false)
                                        {
                                            errmsg = "选择的要素不能构成封闭多边形！";
                                            pGon = null;
                                            pDataset1 = pEnumDataset2.Next();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (pGon != null)
                            break;
                        pDataset = pEnumdataset1.Next();
                    }
                }
                if (pGon == null)
                {
                    if (errmsg != "")
                    {
                        MessageBox.Show(errmsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请选择一个包含面要素和线要素的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return pGon;
                }
            }
            else if (path.EndsWith(".shp"))
            {
                IWorkspaceFactory pwf = new ShapefileWorkspaceFactoryClass();
                string filepath = System.IO.Path.GetDirectoryName(path);
                string filename = path.Substring(path.LastIndexOf("\\") + 1);
                IFeatureWorkspace pFeatureworkspace = (IFeatureWorkspace)pwf.OpenFromFile(filepath, 0);
                IFeatureClass pFeatureclass = pFeatureworkspace.OpenFeatureClass(filename);
                if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature != null)
                    {
                        pGon = pFeature.Shape as IPolygon;
                    }
                }
                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature != null)
                    {

                        IPolyline pPolyline = pFeature.Shape as IPolyline;
                        pGon = GetPolygonFormLine(pPolyline);
                        if (pGon.IsClosed == false)
                        {
                            MessageBox.Show("选择的线要素不能构成封闭多边形！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return null;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择一个面或者线要素文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }


            }
            else if (path.EndsWith(".txt"))
            {
                string txtpath = path;
                System.IO.StreamReader smRead = new System.IO.StreamReader(txtpath, System.Text.Encoding.Default); //设置路径  
                string line;

                IPointCollection pc = pGon as IPointCollection;
                double x, y;
                while ((line = smRead.ReadLine()) != null)
                {
                    if (line.IndexOf(",") > 0)
                    {
                        try
                        {
                            x = double.Parse(line.Substring(0, line.IndexOf(",")));
                            y = double.Parse(line.Substring(line.IndexOf(",") + 1));
                        }
                        catch
                        {
                            MessageBox.Show("文本文件格式不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            smRead.Close();
                            return null;
                        }
                        IPoint tmpPoint = new ESRI.ArcGIS.Geometry.Point();
                        tmpPoint.X = x;
                        tmpPoint.Y = y;
                        object ep = System.Reflection.Missing.Value;

                        pc.AddPoint(tmpPoint, ref ep, ref ep);
                    }

                }
                smRead.Close();
                ICurve pCurve = pGon as ICurve;
                if (pCurve.IsClosed == false)
                {
                    MessageBox.Show("导入点坐标不能构成封闭多边形！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return null;
                }

            }
            else if (path.EndsWith("gdb"))
            {
                string errmsg = "";
                IWorkspaceFactory pwf = new FileGDBWorkspaceFactoryClass();
                IWorkspace pworkspace = pwf.OpenFromFile(path.Substring(0,path.LastIndexOf("\\")), 0);
                IEnumDataset pEnumdataset = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumdataset.Reset();
                IDataset pDataset = pEnumdataset.Next();
                while (pDataset != null)
                {
                    IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                    if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    {
                        pDataset = pEnumdataset.Next();
                        continue;
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {
                            pGon = pFeature.Shape as IPolygon;
                            break;
                        }
                        else
                        {
                            pDataset = pEnumdataset.Next();
                            continue;
                        }
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {

                            IPolyline pPolyline = pFeature.Shape as IPolyline;
                            pGon = GetPolygonFormLine(pPolyline);
                            if (pGon.IsClosed == false)
                            {
                                errmsg = "选择的要素不能构成封闭多边形！";
                                pGon = null;
                                pDataset = pEnumdataset.Next();
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                }
                if (pGon == null)
                {
                    IEnumDataset pEnumdataset1 = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                    pEnumdataset1.Reset();
                    pDataset = pEnumdataset1.Next();
                    while (pDataset != null)
                    {
                        IFeatureDataset pFeatureDataset = pDataset as IFeatureDataset;
                        IEnumDataset pEnumDataset2 = pFeatureDataset.Subsets;
                        pEnumDataset2.Reset();
                        IDataset pDataset1 = pEnumDataset2.Next();
                        while (pDataset1 != null)
                        {
                            if (pDataset1 is IFeatureClass)
                            {

                                IFeatureClass pFeatureclass = pDataset1 as IFeatureClass;
                                if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                                {
                                    pDataset1 = pEnumDataset2.Next();
                                    continue;
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {
                                        pGon = pFeature.Shape as IPolygon;
                                        break;
                                    }
                                    else
                                    {
                                        pDataset1 = pEnumDataset2.Next();
                                        continue;
                                    }
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {

                                        IPolyline pPolyline = pFeature.Shape as IPolyline;
                                        pGon = GetPolygonFormLine(pPolyline);
                                        if (pGon.IsClosed == false)
                                        {
                                            errmsg = "选择的要素不能构成封闭多边形！";
                                            pGon = null;
                                            pDataset1 = pEnumDataset2.Next();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (pGon != null)
                            break;
                        pDataset = pEnumdataset1.Next();
                    }
                }
                if (pGon == null)
                {
                    if (errmsg != "")
                    {
                        MessageBox.Show(errmsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请选择一个包含面要素和线要素的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return pGon;
                }
            }
            return pGon;
        }
        #endregion

        //cast the polyline object to the polygon xisheng 20110926 
        private IPolygon GetPolygonFormLine(IPolyline pPolyline)
        {
            ISegmentCollection pRing;
            IGeometryCollection pPolygon = new PolygonClass();
            IGeometryCollection pPolylineC = pPolyline as IGeometryCollection;
            object o = Type.Missing;
            for (int i = 0; i < pPolylineC.GeometryCount; i++)
            {
                pRing = new RingClass();
                pRing.AddSegmentCollection(pPolylineC.get_Geometry(i) as ISegmentCollection);
                pPolygon.AddGeometry(pRing as IGeometry, ref o, ref o);
            }
            IPolygon polygon = pPolygon as IPolygon;
            return polygon;
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.Filter = "个人数据库(*.mdb)|*.mdb|shp数据|*.shp|文本文件|*.txt";
            dlg.Filter = "shp数据|*.shp|个人数据库(*.mdb)|*.mdb|文件数据库(*.gdb)|gdb";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            IPolygon pGon = new PolygonClass();
            pGon = GetPolyGonFromFile(dlg.FileName);
            if (pGon == null) return;
            ITopologicalOperator pTopo = pGon as ITopologicalOperator;
            if (pTopo != null) pTopo.Simplify();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("导入范围查询，范围文件路径为" + dlg.FileName);//xisheng 日志记录 0928;
            }
            //Zq  20111011  add 将当前视图定位到导入的范围并闪烁显示
            m_MapControl.Extent = pGon.Envelope;
            (m_MapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
            m_MapControl.ActiveView.ScreenDisplay.UpdateWindow();
            m_MapControl.FlashShape(pGon as IGeometry, 3, 200, null);
            ///ZQ 2011 1129 modify
            drawgeometryXOR(pGon as IGeometry);
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuery(m_MapControl, m_enumQueryMode);
                m_frmQuery.Owner = m_mainFrm;
                m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
            }
            ///ZQ 2011  1128 将绘制事件插入口袋中
            SysCommon.ScreenDraw.list.Add(ImportPolygonQueryAfterDraw);
            //m_frmQuery.Show();
            ///ZQ 20111119  modify
           // m_frmQuery.FillData(m_MapControl.ActiveView.FocusMap, pGon as IGeometry,esriSpatialRelEnum.esriSpatialRelIntersects );
            _QueryBar.m_pMapControl = m_MapControl;
            _QueryBar.EmergeQueryData(m_MapControl.ActiveView.FocusMap, pGon as IGeometry, esriSpatialRelEnum.esriSpatialRelIntersects);
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
          
        }
        //被擦去时画出
        //private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        //{
        //    if (m_frmQuery.IsDisposed == true) return;
        //    if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR(null, m_MapControl.ActiveView.ScreenDisplay);
        //}
        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumQueryMode = m_frmQuery.QueryMode;
            m_frmQuery = null;
            SysCommon.ScreenDraw.list.Remove(ImportPolygonQueryAfterDraw);
            //m_MapControl.ActiveView.Refresh();  //changed by chulili  20111011 改为仅刷新图元
            (m_MapControl.Map as IActiveView).PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        //工具不可用时释放窗体等变量
        public override bool Deactivate()
        {
            return true;
        }
        #endregion
    }
}
