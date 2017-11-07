using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;


using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF.BaseClasses;

using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using stdole;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Output;
using SysCommon.Error;
using ElementCommandTool;
using SysCommon.Gis;
namespace GeoPageLayout
{
    public partial class FrmPageLayout : DevComponents.DotNetBar.Office2007Form
    {

        IMap cMap = null;

        double height, width;//mapframe in page units
        private System.Drawing.Printing.PrintDocument kskdocument = new System.Drawing.Printing.PrintDocument();
        internal PrintPreviewDialog printPreviewDialogksk;
        internal PrintDialog printDialogksk;
        private short m_CurrentPrintPage;
        private ITrackCancel m_TrackCancel = new CancelTrackerClass();
        string cDir = "";
        string cMxdPath = "";
        string cMdbPath = "";
        string xzq = "";
        string cPath = Application.StartupPath + "\\..\\Template\\PageLayout.xml";
        List<string> cfnamelst = new List<string>();
        IMapDocument cMD = null;
        XmlDocument cXmlDoc = null;
        int cScale = 10000;
        private  IEnvelope pEnvelope = null;//pagelayout初始的范围
        private Dictionary<string, string> pgTextElements = null;
        public List<string> clistview =null;
        public int typeZHT = -1;////制图类型0标准1业务2动态范围3接合图表树
        private bool hasLegend = false;
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
        //标准图幅制图的构造函数
        public FrmPageLayout(IMap pMap)
        {
            InitializeComponent();
            try
            {
                cDir = createDir();
                //(pMap as IGraphicsContainer).DeleteAllElements();
                cMD = new MapDocumentClass();
                cMD.New(cDir + @"\" + pMap.get_Layer(0).Name.Replace(":", "：") + ".mxd");
                cMD.Open(cDir + @"\" + pMap.get_Layer(0).Name.Replace(":", "：") + ".mxd", "");
                cMD.ReplaceContents(pMap as IMxdContents);
                cMD.Save(true, false);

                //cMD.Close();
                //axPageLayoutControl1.LoadMxFile(cDir + @"\" + pMap.get_Layer(0).Name.Replace(":", "：") + ".mxd", "");
                axPageLayoutControl1.ActiveView.Refresh();

                IMapFrame pMapFrame = (IMapFrame)axPageLayoutControl1.GraphicsContainer.FindFrame(axPageLayoutControl1.ActiveView.FocusMap);

                IElement pMapEle = pMapFrame as IElement;
                (pMapEle as IElementProperties).Name = "地图";
                //IEnvelope pEvle = pMapEle.Geometry.Envelope;
                //pEnvelope = new EnvelopeClass();
                //pEnvelope.PutCoords(pEvle.XMin,pEvle.YMin,pEvle.XMax,pEvle.YMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private string createDir()
        {
            string datefilename = DateTime.Now.ToString().Replace(':', '_');
            string dstfilename = Application.StartupPath + "\\..\\OutputResults\\图形成果\\" + datefilename;
            if (!Directory.Exists(dstfilename))
                Directory.CreateDirectory(dstfilename);
            string Dir = dstfilename;
            return Dir;
        }

        //动态范围制图的构造函数，已不用
        public  void FrmPageLayout123(IMap pMap, IGeometry pExtent)
        {
            InitializeComponent();
            btnBZTF.Visible = false;
            try
            {
                IObjectCopy pOC = new ObjectCopyClass();
                IMap newMap = pOC.Copy(pMap) as IMap;
                procLyrScale(newMap);//yjl取消比例尺显示限制
                IMaps newMaps = new Maps();
               
                newMaps.Add(newMap);
                cMD = new MapDocumentClass();
                cMD.Open(Application.StartupPath + "\\..\\Template\\test.mxd", "");

                //if (cMD.get_Map(0).FeatureSelection.CanClear())
                //cMD.get_Map(0).ClearSelection();//若是选择集范围出图，将选择集清除
  
                axPageLayoutControl1.PageLayout = cMD.PageLayout;
                IMapLayers pMapLayers = axPageLayoutControl1.ActiveView.FocusMap as IMapLayers;
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pMapLayers.InsertLayer(pMap.get_Layer(i), false, pMapLayers.LayerCount);
                }
                //axPageLayoutControl1.PageLayout.ReplaceMaps(newMaps);
                IActiveView mapAV = axPageLayoutControl1.ActiveView;
                mapAV.Extent = pExtent.Envelope;
                int cScale = 10000;
                double width = pExtent.Envelope.Width * 100 / cScale;
                double height = pExtent.Envelope.Height * 100 / cScale;
                axPageLayoutControl1.Page.PutCustomSize(width + 15, height + 15);//centimeter
                IMap pgMap = axPageLayoutControl1.ActiveView.FocusMap as IMap;
                pgMap.ClipGeometry = pExtent;
                //axPageLayoutControl1.LoadMxFile(cDir + @"\" + pMap.get_Layer(0).Name.Replace(":", "：") + ".mxd", "");
                axPageLayoutControl1.ActiveView.Refresh();
                IMapFrame pMapFrame = (IMapFrame)axPageLayoutControl1.GraphicsContainer.FindFrame(axPageLayoutControl1.ActiveView.FocusMap);
                IElement pMapEle = pMapFrame as IElement;
                IEnvelope pppp=new EnvelopeClass();
                pppp.PutCoords(5,5,5+width,5+height);
                pMapEle.Geometry = pppp;
                (pMapEle as IElementProperties).Name = "地图";
                axPageLayoutControl1.ZoomToWholePage();
                //IEnvelope pEvle = pMapEle.Geometry.Envelope;
                //pEnvelope = new EnvelopeClass();
                //pEnvelope.PutCoords(pEvle.XMin, pEvle.YMin, pEvle.XMax, pEvle.YMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //动态范围制图的构造函数
        public  FrmPageLayout(IMap pMap, IGeometry pExtent)
        {
            InitializeComponent();
            btnBZTF.Visible = false;
            try
            {
                IObjectCopy pOC = new ObjectCopyClass();//20111007yjl add
                IMap newMap = pOC.Copy(pMap) as IMap;
                //procLyrScale(newMap);//deleted by chulili 20111118 保持比例尺限制//yjl取消比例尺显示限制
                IMapLayers pMapLayers = axPageLayoutControl1.ActiveView.FocusMap as IMapLayers;
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
                }
                IMap newMap2 = pMapLayers as IMap;
                ///ZQ 20111115  modify
                //GeoPageLayoutFn.updateMapSymbol(newMap2, pExtent);//计算范围图例
                IActiveView mapAV = pMapLayers as IActiveView;
                mapAV.Extent = pExtent.Envelope;
                IMap pgMap = pMapLayers as IMap;
                //procLyrScale(pgMap);  //deleted by chulili 20111118 保持比例尺限制
                pgMap.ClipGeometry = pExtent;
                IMapFrame pMapFrame = (IMapFrame)axPageLayoutControl1.GraphicsContainer.FindFrame(axPageLayoutControl1.ActiveView.FocusMap);

                IElement pMapEle = pMapFrame as IElement;
                (pMapEle as IElementProperties).Name = "地图";
                IPage pPage = axPageLayoutControl1.PageLayout.Page;
                pPage.IsPrintableAreaVisible = false;
                //pPage.IsPrintableAreaVisible = false;
                //pMapEle.Geometry = pPage.PrintableBounds;
                //IEnvelope pEvle = pMapEle.Geometry.Envelope;
                //pEnvelope = new EnvelopeClass();
                //pEnvelope.PutCoords(pEvle.XMin, pEvle.YMin, pEvle.XMax, pEvle.YMax);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //导入范围制专题图的构造函数
        public FrmPageLayout(IMap pMap, IGeometry pExtent,bool isTrue)
        {
            InitializeComponent();
            IObjectCopy pOC = new ObjectCopyClass();
            IMap pMC = pOC.Copy(pMap) as IMap;

            btnBZTF.Visible = false;
            try
            {
                GeoPageLayoutFn.updateMapSymbol(pMC, pExtent);
                if (isTrue)
                    pageLayoutExtentZTT(pMC, pExtent);
                else
                    pageLayoutExtentZTGHT(pMC, pExtent);
               
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void gcsReplaceSR(IMap pMap,IGeometry inGeometry)
        {

            if (pMap.SpatialReference is IGeographicCoordinateSystem)
            {
                IArea pArea = inGeometry as IArea;
                IPoint pCp = pArea.Centroid;
                int iX = (int)pCp.X;
                int iCenter = Convert.ToInt32(Math.Floor(iX / 6.0) * 6 + 3);
                ISpatialReference pSpa = GetSpatialByX(iCenter);
                if (pSpa == null)
                {
                    MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pMap.SpatialReference = pSpa;
                inGeometry.Project(pSpa);
            }
        }
        //宗地图的构造函数
        public FrmPageLayout(IMap pMap, IGeometry pExtent, IFeature inFeature)
        {
            InitializeComponent();
            btnBZTF.Visible = false;
            try
            {
                pageLayoutSelZDT(pMap, pExtent, inFeature);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //宗地图的构造函数-暂不使用
        public FrmPageLayout(IMaps pMaps, IGeometry pExtent, IFeature inFeature)
        {
            InitializeComponent();
            btnBZTF.Visible = false;
            try
            {
                pageLayoutSelZDT(pMaps, pExtent, inFeature);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //已知图幅号标准图幅制图的构造函数(inPoint参数是为了确定大比例尺坐标高位数)
        public FrmPageLayout(IMap pMap,string strMapNo,int iScale,SysCommon.CProgress pgss,IPoint inPoint,int type_ZT)
        {
            InitializeComponent();
            try
            {
          
                axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pMap.SpatialReference;
                IMapLayers pMapLayers = axPageLayoutControl1.ActiveView.FocusMap as IMapLayers;
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    IObjectCopy pOC = new ObjectCopyClass();
                    ILayer pLayer = pOC.Copy(pMap.get_Layer(i)) as ILayer;
                    pMapLayers.InsertLayer(pLayer, false, pMapLayers.LayerCount);
                }
                IActiveView mapAV = pMapLayers as IActiveView;
                IMap pgMap = pMapLayers as IMap;
                procLyrScale(pgMap);//yjl取消比例尺显示限制
                //axPageLayoutControl1.PageLayout = cMD.PageLayout;
                //axPageLayoutControl1.LoadMxFile(cDir + @"\" + pMap.get_Layer(0).Name.Replace(":", "：") + ".mxd", "");
                axPageLayoutControl1.ActiveView.Refresh();

                
                pgss.Close();
                if (iScale > 2000)//小比例尺
                {
                    if (!OutBZFFT2(iScale, strMapNo,type_ZT)) return;
                }
                else
                {
                    if (inPoint == null)
                    {
                        OutBZFFTBigScale2(iScale, strMapNo, "", "", type_ZT);
                    }
                    else
                    {
                        int gwx = (int)inPoint.X / 100000;//高位数
                        int gwy = (int)inPoint.Y / 100000;
                        OutBZFFTBigScale2(iScale, strMapNo, gwx.ToString(), gwy.ToString(), type_ZT);
                    }
 
                    
                }
                //更新范围图例
                GeoPageLayoutFn.updateMapSymbol(axPageLayoutControl1.ActiveView.FocusMap, axPageLayoutControl1.ActiveView.FocusMap.ClipGeometry);
                IMapFrame pMapFrame = (IMapFrame)axPageLayoutControl1.GraphicsContainer.FindFrame(axPageLayoutControl1.ActiveView.FocusMap);

                IElement pMapEle = pMapFrame as IElement;
                (pMapEle as IElementProperties).Name = "地图";
                double x = pMapEle.Geometry.Envelope.XMax;//地图框架的右上点坐标
                double y = pMapEle.Geometry.Envelope.YMax;
                double xmin = pMapEle.Geometry.Envelope.XMin;
                //AddScalebar(axPageLayoutControl1.PageLayout, axPageLayoutControl1.ActiveView.FocusMap);
                //标准制图里没有这样的指北针 也不是这样加图例的 所以去掉 chenxinwei Edit
                if (FrmSheetMapSet2.CheckedTuli && type_ZT != 1)//选择显示图例就显示 20110828 xisheng 
                {
                    if (iScale > 2000)//小比例尺
                    {
                        AddLegend(axPageLayoutControl1.PageLayout, axPageLayoutControl1.ActiveView.FocusMap, x + 1, y - 4, 8,2);
                        //AddNorthArrow(axPageLayoutControl1.PageLayout, axPageLayoutControl1.ActiveView.FocusMap, xmin, y);
                    }

                    else
                    {
                        AddLegend(axPageLayoutControl1.PageLayout, axPageLayoutControl1.ActiveView.FocusMap, x + 4, y, 8,2);
                        //AddNorthArrow(axPageLayoutControl1.PageLayout, axPageLayoutControl1.ActiveView.FocusMap, xmin, y);
                    }
                }
                if (type_ZT == 1&&hasLegend)
                {
                    AddLegend(axPageLayoutControl1.PageLayout, axPageLayoutControl1.ActiveView.FocusMap, x + 1, y, 4, 1);
                    delUnnecLegends(axPageLayoutControl1.PageLayout as IGraphicsContainer,x+1,y,4);
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void procLyrScale(IMap inMap)
        {
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        ILayer pFLayer = pCLayer.get_Layer(j);
                        pFLayer.MaximumScale = 0;
                        pFLayer.MinimumScale = 0;

                    }
                }
                else//不是grouplayer
                {
                    pLayer.MaximumScale = 0;
                    pLayer.MinimumScale = 0;
                }
            }
        }

        //for open a saved mxd
        public FrmPageLayout(string docpath)
        {
            InitializeComponent();
            cMD = new MapDocumentClass();
            cMD.Open(docpath, "");
            axPageLayoutControl1.PageLayout = cMD.PageLayout;
 
        }

        //传入制图对象yjl20110921
        public FrmPageLayout(IPageLayout inPageLayout)
        {
            InitializeComponent();
            axPageLayoutControl1.PageLayout = inPageLayout;
            axPageLayoutControl1.ZoomToWholePage();

        }

        private void FrmPageLayout_Load(object sender, EventArgs e)
        {
            //axTOCControl1.Refresh();
            //if(typeZHT==3)
            //
            (axToolbarControl1 as Control).MouseLeave += new EventHandler(axToolbarControl1_MouseLeave);
            if (axPageLayoutControl1.ActiveView.FocusMap.LayerCount == 0)
            {
                this.Dispose();
                return;
            }
            //初始化控件工具为选择元素工具
            string progID = "esriControls.ControlsSelectTool";
            int index = axToolbarControl1.Find(progID);
            if (index != -1)
            {
                IToolbarItem toolItem = axToolbarControl1.GetItem(index);
                ICommand _cmd = toolItem.Command;
                ITool _tool = (ITool)_cmd;
                axPageLayoutControl1.CurrentTool = _tool;
            }
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            pGCS.UnselectAllElements();//清除全选
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        #region 宗地图
        //宗地图
        private void pageLayoutSelZDT(IMap inMap, IGeometry inGeometry,IFeature inFeature)
        {
            if (inMap == null)
                return;

            //Plugin.ModuleCommon.TmpWorkSpace

            FrmZDMapSet tdlyXQT = new FrmZDMapSet();
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            if (tdlyXQT.ShowDialog() != DialogResult.OK)
                return;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\ZDTemplate.mxd");
            if (tlPageLayout == null)
            {

                return;
            }
            if (inFeature == null)
                return;
            //宗地信息
            //int idx1 = 0, idx2 = 0, idx3 = 0, idx4 = 0, idx5 = 0;
            //idx2 = inFeature.Fields.FindField("DJH");
            //idx3 = inFeature.Fields.FindField("QSXZ");
            //idx4 = inFeature.Fields.FindField("FZMJ");
            //idx5 = inFeature.Fields.FindField("JZWZDMJ");
            //string username = (inFeature.Class as IDataset).Name;
            //if (username.Contains("."))
            //    username = username.Substring(0, username.LastIndexOf('.') + 1);
            //IWorkspace pWorkspace = (inFeature.Class as IDataset).Workspace;
            ////IWorkspace pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
            //IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            //ITable pTable = pFeatureWorkspace.OpenTable(username + "CZDJ_QLR");
            //string djh = inFeature.get_Value(idx2).ToString();
            //IQueryFilter pQF = new QueryFilterClass();
            //pQF.WhereClause="DJH='"+djh+"'";
            //ICursor pCursor=pTable.Search(pQF, false);
            //IRow pRow = pCursor.NextRow();
           
            //idx1 = pTable.Fields.FindField("QLRMC");
            //if (pRow != null)
            //   pTextEles.Add("左上value1", pRow.get_Value(idx1).ToString());
            //pTextEles.Add("左上value2", inFeature.get_Value(idx2).ToString());
            //pTextEles.Add("左上value3", inFeature.get_Value(idx3).ToString());
            //pTextEles.Add("右上value4", inFeature.get_Value(idx4).ToString()+"平方米");
            //pTextEles.Add("右上value5", inFeature.get_Value(idx5).ToString() + "平方米");
            IGraphicsContainer tlGra = tlPageLayout as IGraphicsContainer;
            IPage tlPage = tlPageLayout.Page;
            double pageW = 0, pageH = 0;
            tlPage.QuerySize(out pageW, out pageH);//模板纸张大小
            tlGra.Reset();
            IElement tlEle = tlGra.Next();
            IElement tlRC = null;

            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    tlRC = tlEle;
                tlEle = tlGra.Next();
            }
            IEnvelope tlRCEnv = tlRC.Geometry.Envelope as IEnvelope;
            if (tlRCEnv == null)
                return;
            //RC和纸张的距离左下右上
            double[] rc2page = new double[4];
            rc2page[0] = tlRCEnv.XMin;
            rc2page[1] = tlRCEnv.YMin;
            rc2page[2] = pageW - tlRCEnv.XMax;
            rc2page[3] = pageH - tlRCEnv.YMax;
            IElement tlMF = tlGra.FindFrame((tlPageLayout as IActiveView).FocusMap) as IElement;
            IEnvelope tlMFEnv = tlMF.Geometry.Envelope as IEnvelope;
            if (tlMFEnv == null)
                return;
            //地图框架和纸张的距离
            double[] mf2page = new double[4];
            mf2page[0] = tlMFEnv.XMin;
            mf2page[1] = tlMFEnv.YMin;
            mf2page[2] = pageW - tlMFEnv.XMax;
            mf2page[3] = pageH - tlMFEnv.YMax;
            IObjectCopy pOC = new ObjectCopyClass();
            IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
            //IPageLayoutControl pPLC = new PageLayoutControlClass();//制图控件不能属于两个控件
            axPageLayoutControl1.PageLayout = workPageLayout;
            IPage workPage = workPageLayout.Page;
            IEnvelope workEnv = inGeometry.Envelope;
            workPage.StretchGraphicsWithPage = false;
            double workPW = workEnv.Width * 100 / inScale + mf2page[0] + mf2page[2],
                workPH = workEnv.Height * 100 / inScale + mf2page[1] + mf2page[3];
            workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
            IActiveView workActiveView = workPageLayout as IActiveView;
            pOC = new ObjectCopyClass();
            IMap newMap = pOC.Copy(inMap) as IMap;
            procLyrScale(newMap);//yjl取消比例尺显示限制
            IMap workMap = workActiveView.FocusMap;
            workMap.Name = pTextEles["主题"];
            workMap.SpatialReference = inMap.SpatialReference;
            IMapLayers pMapLayers = workMap as IMapLayers;
            for (int i = 0; i < newMap.LayerCount; i++)
            {
                pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
            }
            IActiveView workMapAV = workMap as IActiveView;
            workMapAV.Extent = inGeometry.Envelope;
            workMap.ClipGeometry = inGeometry.Envelope;
            workMap.MapScale = inScale;
            //ITopologicalOperator pTO = inGeometry as ITopologicalOperator;
            //IPolyline xzqBound = pTO.Boundary as IPolyline;
            //GeoPageLayoutFn.drawPolylineElement(xzqBound, workMap as IGraphicsContainer);
            IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
            pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
            pMapFrame.MapBounds = inGeometry.Envelope;
            IElement pMapEle = pMapFrame as IElement;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
            pMapEle.Geometry = pEnv;
            //添加图例
            //if (hasLegend)
            //{
            //    AddLegendZT(workPageLayout, workMap, workPW - mf2page[2], mf2page[1], 8, 2);
            //    //delUnnecLegends(workGra);//去除多余图例
            //}
            IElement workRC = null;//外框
            IElement workTable = null;//宗地表格
            workGra.Reset();
            tlEle = workGra.Next();
            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    workRC = tlEle;
                if (pEP.Name == "table")//外框
                    workTable = tlEle;
                tlEle = workGra.Next();
            }
            IEnvelope pEnv2 = new EnvelopeClass();
            pEnv2.PutCoords(rc2page[0], rc2page[1], workPW - rc2page[2], workPH - rc2page[3]);
            workRC.Geometry = pEnv2;
            //IEnvelope tlEnvTable = workTable.Geometry.Envelope;
            //IEnvelope pEnv3 = new EnvelopeClass();
            //pEnv3.PutCoords(rc2page[0], workPH - rc2page[3], workPW - rc2page[2], workPH - rc2page[3] + 3);
            //workTable.Geometry = pEnv3;
            //画里面的表格线
            //drawTableInnerLines(workGra, workTable);
            double[] offXY = new double[2];//0X位移2Y位移
            offXY[0] = pEnv2.XMax - tlRCEnv.XMax;
            offXY[1] = pEnv2.YMax - tlRCEnv.YMax;
            moveElements(workGra, offXY);
            updateTextEle(workGra, pTextEles);
            //axPageLayoutControl1.Object = pPLC;
            axPageLayoutControl1.ZoomToWholePage();
        }
        //宗地图,暂不使用
        private void pageLayoutSelZDT(IMaps inMaps, IGeometry inGeometry, IFeature inFeature)
        {
            if (inMaps == null)
                return;

            //Plugin.ModuleCommon.TmpWorkSpace

            FrmZDMapSet tdlyXQT = new FrmZDMapSet();
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            if (tdlyXQT.ShowDialog() != DialogResult.OK)
                return;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\ZDTemplate.mxd");
            if (tlPageLayout == null)
            {

                return;
            }
            if (inFeature == null)
                return;
            //宗地信息
            //int idx1 = 0, idx2 = 0, idx3 = 0, idx4 = 0, idx5 = 0;
            //idx2 = inFeature.Fields.FindField("DJH");
            //idx3 = inFeature.Fields.FindField("QSXZ");
            //idx4 = inFeature.Fields.FindField("FZMJ");
            //idx5 = inFeature.Fields.FindField("JZWZDMJ");
            //string username = (inFeature.Class as IDataset).Name;
            //if (username.Contains("."))
            //    username = username.Substring(0, username.LastIndexOf('.') + 1);
            //IWorkspace pWorkspace = (inFeature.Class as IDataset).Workspace;
            ////IWorkspace pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
            //IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            //ITable pTable = pFeatureWorkspace.OpenTable(username + "CZDJ_QLR");
            //string djh = inFeature.get_Value(idx2).ToString();
            //IQueryFilter pQF = new QueryFilterClass();
            //pQF.WhereClause="DJH='"+djh+"'";
            //ICursor pCursor=pTable.Search(pQF, false);
            //IRow pRow = pCursor.NextRow();

            //idx1 = pTable.Fields.FindField("QLRMC");
            //if (pRow != null)
            //   pTextEles.Add("左上value1", pRow.get_Value(idx1).ToString());
            //pTextEles.Add("左上value2", inFeature.get_Value(idx2).ToString());
            //pTextEles.Add("左上value3", inFeature.get_Value(idx3).ToString());
            //pTextEles.Add("右上value4", inFeature.get_Value(idx4).ToString()+"平方米");
            //pTextEles.Add("右上value5", inFeature.get_Value(idx5).ToString() + "平方米");
            IGraphicsContainer tlGra = tlPageLayout as IGraphicsContainer;
            IPage tlPage = tlPageLayout.Page;
            double pageW = 0, pageH = 0;
            tlPage.QuerySize(out pageW, out pageH);//模板纸张大小
            tlGra.Reset();
            IElement tlEle = tlGra.Next();
            IElement tlRC = null;

            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    tlRC = tlEle;
                tlEle = tlGra.Next();
            }
            IEnvelope tlRCEnv = tlRC.Geometry.Envelope as IEnvelope;
            if (tlRCEnv == null)
                return;
            //RC和纸张的距离左下右上
            double[] rc2page = new double[4];
            rc2page[0] = tlRCEnv.XMin;
            rc2page[1] = tlRCEnv.YMin;
            rc2page[2] = pageW - tlRCEnv.XMax;
            rc2page[3] = pageH - tlRCEnv.YMax;
            IElement tlMF = tlGra.FindFrame((tlPageLayout as IActiveView).FocusMap) as IElement;
            IEnvelope tlMFEnv = tlMF.Geometry.Envelope as IEnvelope;
            if (tlMFEnv == null)
                return;
            //地图框架和纸张的距离
            double[] mf2page = new double[4];
            mf2page[0] = tlMFEnv.XMin;
            mf2page[1] = tlMFEnv.YMin;
            mf2page[2] = pageW - tlMFEnv.XMax;
            mf2page[3] = pageH - tlMFEnv.YMax;
            IObjectCopy pOC = new ObjectCopyClass();
            IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
            //IPageLayoutControl pPLC = new PageLayoutControlClass();//制图控件不能属于两个控件
            axPageLayoutControl1.PageLayout = workPageLayout;
            IPage workPage = workPageLayout.Page;
            IEnvelope workEnv = inGeometry.Envelope;
            workPage.StretchGraphicsWithPage = false;
            double workPW = workEnv.Width * 100 / inScale + mf2page[0] + mf2page[2],
                workPH = workEnv.Height * 100 / inScale + mf2page[1] + mf2page[3];
            workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
            IActiveView workActiveView = workPageLayout as IActiveView;
            //pOC = new ObjectCopyClass();
            //IMaps newMaps = pOC.Copy(inMaps) as IMaps;
            workPageLayout.ReplaceMaps(inMaps);
            IMap workMap = workActiveView.FocusMap;

            for (int i = 0; i < inMaps.Count; i++)
            {
                procLyrScale(inMaps.get_Item(i));//yjl取消比例尺显示限制
            }
            
            //ITopologicalOperator pTO = inGeometry as ITopologicalOperator;
            //IPolyline xzqBound = pTO.Boundary as IPolyline;
            //GeoPageLayoutFn.drawPolylineElement(xzqBound, workMap as IGraphicsContainer);
            IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
            for (int i = 0; i < inMaps.Count; i++)
            {
                workActiveView.FocusMap = inMaps.get_Item(i);
                IActiveView pAcView = inMaps.get_Item(i) as IActiveView;
                workMap = inMaps.get_Item(i);
                if (workMap.Name == "宗地图")
                {

                    workMap.ClipGeometry = inGeometry.Envelope;

                }
                else
                {
                    ITopologicalOperator pTO = inFeature.ShapeCopy as ITopologicalOperator;
                    
                    workMap.ClipGeometry = pTO.Buffer(1);
                }
                pAcView.Extent = inGeometry.Envelope;
                workMap.MapScale = inScale;
                IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
                pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
                pMapFrame.MapBounds = inGeometry.Envelope;
                IElement pMapEle = pMapFrame as IElement;
                IEnvelope pEnv = new EnvelopeClass();
                pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
                pMapEle.Geometry = pEnv;
            }
            //添加图例
            //if (hasLegend)
            //{
            //    AddLegendZT(workPageLayout, workMap, workPW - mf2page[2], mf2page[1], 8, 2);
            //    //delUnnecLegends(workGra);//去除多余图例
            //}
            IElement workRC = null;//外框
            IElement workTable = null;//宗地表格
            workGra.Reset();
            tlEle = workGra.Next();
            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    workRC = tlEle;
                if (pEP.Name == "table")//外框
                    workTable = tlEle;
                tlEle = workGra.Next();
            }
            IEnvelope pEnv2 = new EnvelopeClass();
            pEnv2.PutCoords(rc2page[0], rc2page[1], workPW - rc2page[2], workPH - rc2page[3]);
            workRC.Geometry = pEnv2;
            //IEnvelope tlEnvTable = workTable.Geometry.Envelope;
            //IEnvelope pEnv3 = new EnvelopeClass();
            //pEnv3.PutCoords(rc2page[0], workPH - rc2page[3], workPW - rc2page[2], workPH - rc2page[3] + 3);
            //workTable.Geometry = pEnv3;
            //画里面的表格线
            //drawTableInnerLines(workGra, workTable);
            double[] offXY = new double[2];//0X位移2Y位移
            offXY[0] = pEnv2.XMax - tlRCEnv.XMax;
            offXY[1] = pEnv2.YMax - tlRCEnv.YMax;
            moveElements(workGra, offXY);
            updateTextEle(workGra, pTextEles);
            //axPageLayoutControl1.Object = pPLC;
            axPageLayoutControl1.ZoomToWholePage();
        }
        //画宗地上方表格的边线
        private void drawTableInnerLines(IGraphicsContainer inGra, IElement inRecEle)
        {
            IEnvelope pEnv = inRecEle.Geometry.Envelope;
            //画水平线
            double vGap=1,hLineW=1;
            IRgbColor pRGB=getRGB(0,0,0);//黑色 
            for (int i = 1; i < pEnv.Height / vGap; i++)
            {
                //ILine hLine = new LineClass();
                IPolyline hLine = new PolylineClass();
                IPoint FromPoint = new PointClass(); 
                FromPoint.PutCoords(pEnv.XMin, pEnv.YMax - vGap * i);
                IPoint ToPoint = new PointClass();
                ToPoint.PutCoords(pEnv.XMax, pEnv.YMax - vGap*i);
                //hLine.PutCoords(FromPoint, ToPoint);
                hLine.FromPoint = FromPoint;
                hLine.ToPoint = ToPoint;
                GeoPageLayoutFn.drawPolylineElement(hLine, inGra, pRGB, hLineW);
            }
            //画垂直线,暂只画中心线
            double midX = (pEnv.XMax + pEnv.XMin) / 2,
                midY = (pEnv.YMax + pEnv.YMin) / 2;
            //ILine vLine = new LineClass();
            IPolyline vLine = new PolylineClass();
            IPoint vFromPoint = new PointClass();
            vFromPoint.PutCoords(midX, pEnv.YMax);
            IPoint vToPoint = new PointClass();
            vToPoint.PutCoords(midX, pEnv.YMin);
            //vLine.PutCoords(vFromPoint, vToPoint);
            vLine.FromPoint = vFromPoint;
            vLine.ToPoint = vToPoint;
            GeoPageLayoutFn.drawPolylineElement(vLine, inGra, pRGB, hLineW);


        }
        #endregion

        #region 范围专题图
        private void pageLayoutExtentZTT(IMap inMap, IGeometry inGeometry)
        {
            if (inMap == null)
                return;
            FrmExtentZTMapSet tdlyXQT = new FrmExtentZTMapSet();
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            reSet:
            if (tdlyXQT.ShowDialog() != DialogResult.OK)
                return;
            gcsReplaceSR(inMap,inGeometry);//直接改变地图对象的参考  地图对象须经克隆过 以免影响主地图控件的对象
            bool hasLegend = tdlyXQT.HasLegend;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\ExtentTemplate.mxd");
            if (tlPageLayout == null)
                return;
            IGraphicsContainer tlGra = tlPageLayout as IGraphicsContainer;
            IPage tlPage = tlPageLayout.Page;
            double pageW = 0, pageH = 0;
            tlPage.QuerySize(out pageW, out pageH);//模板纸张大小
            tlGra.Reset();
            IElement tlEle = tlGra.Next();
            IElement tlRC = null;
            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    tlRC = tlEle;
                tlEle = tlGra.Next();
            }
            IEnvelope tlRCEnv = tlRC.Geometry.Envelope as IEnvelope;
            if (tlRCEnv == null)
                return;
            //RC和纸张的距离左下右上
            double[] rc2page = new double[4];
            rc2page[0] = tlRCEnv.XMin;
            rc2page[1] = tlRCEnv.YMin;
            rc2page[2] = pageW - tlRCEnv.XMax;
            rc2page[3] = pageH - tlRCEnv.YMax;
            IElement tlMF = tlGra.FindFrame((tlPageLayout as IActiveView).FocusMap) as IElement;
            IEnvelope tlMFEnv = tlMF.Geometry.Envelope as IEnvelope;
            if (tlMFEnv == null)
                return;
            //地图框架和纸张的距离
            double[] mf2page = new double[4];
            mf2page[0] = tlMFEnv.XMin;
            mf2page[1] = tlMFEnv.YMin;
            mf2page[2] = pageW - tlMFEnv.XMax;
            mf2page[3] = pageH - tlMFEnv.YMax;
            IObjectCopy pOC = new ObjectCopyClass();
            IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
            //IPageLayoutControl pPLC = new PageLayoutControlClass();
            //pPLC.PageLayout = workPageLayout;
            IPage workPage = workPageLayout.Page;

            IEnvelope workEnv = inGeometry.Envelope;
            //workEnv.Expand(1.2, 1.2, true);
            workPage.StretchGraphicsWithPage = false;
            double workPW = workEnv.Width * 100 / inScale + mf2page[0] + mf2page[2],
                workPH = workEnv.Height * 100 / inScale + mf2page[1] + mf2page[3];
            //判断如果纸张太小，说明比例尺和范围不协调，提示用户重设
            if (workPH < 20 || workPW < 20)
            {
                MessageBox.Show("出图界面纸张过小，请设置较大比例尺。", "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                goto reSet;//使用GOTO语句，打开设置界面，重设比例尺
            }
            workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
            IActiveView workActiveView = workPageLayout as IActiveView;
            pOC = new ObjectCopyClass();
            IMap newMap = pOC.Copy(inMap) as IMap;
            procLyrScale(newMap);//yjl取消比例尺显示限制
            IMap workMap = workActiveView.FocusMap;
            workMap.Name = pTextEles["主题"];
            workMap.SpatialReference = inMap.SpatialReference;
            IMapLayers pMapLayers = workMap as IMapLayers;
            for (int i = 0; i < newMap.LayerCount; i++)
            {
                pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
            }
            
            IActiveView workMapView = workMap as IActiveView;
            workMapView.Extent = workEnv;
            //workMap.ClipGeometry = inGeometry;
            //workMap.ClipBorder = createBorder(inGeometry, workActiveView);
            workMap.MapScale = inScale;
            //arcgis10新增了接口来处理ClipGeometry
            IMapClipOptions pMapClip = workMap as IMapClipOptions;
            pMapClip.ClipType = esriMapClipType.esriMapClipShape;
            pMapClip.ClipGeometry = inGeometry;
            pMapClip.ClipBorder = createBorder(inGeometry, workActiveView);
                
            //ITopologicalOperator pTO = inGeometry as ITopologicalOperator;
            //IPolyline xzqBound = pTO.Boundary as IPolyline;
            //GeoPageLayoutFn.drawPolylineElement(xzqBound, workMap as IGraphicsContainer);
            IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
            pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
            pMapFrame.MapBounds = workEnv;
            IElement pMapEle = pMapFrame as IElement;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
            pMapEle.Geometry = pEnv;
            //将导入的范围绘制到制图页面
            IObjectCopy pOCa = new ObjectCopyClass();
            IGeometry nwGeometry = pOCa.Copy(inGeometry) as IGeometry;
            if (nwGeometry.GeometryType != esriGeometryType.esriGeometryEnvelope)
            {
                IPointCollection pc = nwGeometry as IPointCollection;
                for (int i = 0; i < pc.PointCount - 1; i++)
                {
                    IPoint tmp = pc.get_Point(i);
                    double x = mf2page[0] + (tmp.X - workEnv.XMin) * 100 / inScale,
                        y = mf2page[1] + (tmp.Y - workEnv.YMin) * 100 / inScale;
                    tmp.PutCoords(x, y);
                    pc.UpdatePoint(i, tmp);
                }
            }
            else
            {
                IEnvelope pEnvelope = nwGeometry as IEnvelope;
                double XMax = mf2page[0] + (pEnvelope.XMax - workEnv.XMin) * 100 / inScale,
                    XMin = mf2page[0] + (pEnvelope.XMin - workEnv.XMin) * 100 / inScale,
                    YMax = mf2page[1] + (pEnvelope.YMax - workEnv.YMin) * 100 / inScale,
                    YMin = mf2page[1] + (pEnvelope.YMin - workEnv.YMin) * 100 / inScale;
                pEnvelope.PutCoords(XMin, YMin, XMax, YMax);
 
            }
                //ITransform2D nwGeoTran = nwGeometry as ITransform2D;
                //nwGeoTran.Move(mf2page[0]/100 - workEnv.XMin, mf2page[1]/100 - workEnv.YMin);
                //IPoint oriP = new PointClass();
                //oriP.PutCoords(mf2page[0], mf2page[1]);
                //nwGeoTran.Scale(oriP, inScale / 100, inScale / 100);
              //GeoPageLayoutFn.drawPolygonElement(nwGeometry, workGra, getRGB(255, 0, 0), 2);
            ////测试用
            //  IGeometry test = pOC.Copy(nwGeometry) as IGeometry;
            //  ITransform2D testTO = test as ITransform2D;
            //  testTO.Scale(test.Envelope.LowerLeft, 0.5, 0.5);
            //  GeoPageLayoutFn.drawPolygonElement(test, workGra, getRGB(0, 0, 255), 2);

            ////测试用end
            //添加图例
            if (hasLegend)
            {
                AddLegendZT(workPageLayout, workMap, workPW - mf2page[2], mf2page[1], 8, 2);
                //delUnnecLegends(workGra);//去除多余图例
            }
            IElement workRC = null;//外框
            workGra.Reset();
            tlEle = workGra.Next();
            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    workRC = tlEle;
                tlEle = workGra.Next();
            }
            IEnvelope pEnv2 = new EnvelopeClass();
            pEnv2.PutCoords(rc2page[0], rc2page[1], workPW - rc2page[2], workPH - rc2page[3]);
            workRC.Geometry = pEnv2;
            double[] offXY = new double[2];//0X位移2Y位移
            offXY[0] = pEnv2.XMax - tlRCEnv.XMax;
            offXY[1] = pEnv2.YMax - tlRCEnv.YMax;
            moveElements(workGra, offXY);
            updateTextEle(workGra, pTextEles);
            axPageLayoutControl1.PageLayout = workPageLayout;
            axPageLayoutControl1.ZoomToWholePage();
        }
        #endregion

        #region 导入范围输出总体规划图
        //导入范围输出总体规划图
        private void pageLayoutExtentZTGHT(IMap inMap, IGeometry inGeometry)
        {
            if (inMap == null)
                return;
            FrmTDLYGHMapSet tdlyXQT = new FrmTDLYGHMapSet();
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            if (tdlyXQT.ShowDialog() != DialogResult.OK)
                return;
            bool hasLegend = tdlyXQT.HasLegend;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\TDLYGHTTemplate.mxd");
            if (tlPageLayout == null)
                return;
            IGraphicsContainer tlGra = tlPageLayout as IGraphicsContainer;
            IPage tlPage = tlPageLayout.Page;
            double pageW = 0, pageH = 0;
            tlPage.QuerySize(out pageW, out pageH);//模板纸张大小
            tlGra.Reset();
            IElement tlEle = tlGra.Next();
            IElement tlRC = null;
            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    tlRC = tlEle;
                tlEle = tlGra.Next();
            }
            IEnvelope tlRCEnv = tlRC.Geometry.Envelope as IEnvelope;
            if (tlRCEnv == null)
                return;
            //RC和纸张的距离左下右上
            double[] rc2page = new double[4];
            rc2page[0] = tlRCEnv.XMin;
            rc2page[1] = tlRCEnv.YMin;
            rc2page[2] = pageW - tlRCEnv.XMax;
            rc2page[3] = pageH - tlRCEnv.YMax;
            IElement tlMF = tlGra.FindFrame((tlPageLayout as IActiveView).FocusMap) as IElement;
            IEnvelope tlMFEnv = tlMF.Geometry.Envelope as IEnvelope;
            if (tlMFEnv == null)
                return;
            //地图框架和纸张的距离
            double[] mf2page = new double[4];
            mf2page[0] = tlMFEnv.XMin;
            mf2page[1] = tlMFEnv.YMin;
            mf2page[2] = pageW - tlMFEnv.XMax;
            mf2page[3] = pageH - tlMFEnv.YMax;
            IObjectCopy pOC = new ObjectCopyClass();
            IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
            //IPageLayoutControl pPLC = new PageLayoutControlClass();
            //pPLC.PageLayout = workPageLayout;
            IPage workPage = workPageLayout.Page;
            IEnvelope workEnv = inGeometry.Envelope;
            workPage.StretchGraphicsWithPage = false;
            double workPW = workEnv.Width * 100 / inScale + mf2page[0] + mf2page[2],
                workPH = workEnv.Height * 100 / inScale + mf2page[1] + mf2page[3];
            workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
            IActiveView workActiveView = workPageLayout as IActiveView;
            pOC = new ObjectCopyClass();
            IMap newMap = pOC.Copy(inMap) as IMap;
            procLyrScale(newMap);//yjl取消比例尺显示限制
            IMap workMap = workActiveView.FocusMap;
            workMap.Name = pTextEles["主题"];
            workMap.SpatialReference = inMap.SpatialReference;
            IMapLayers pMapLayers = workMap as IMapLayers;
            for (int i = 0; i < newMap.LayerCount; i++)
            {
                pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
            }
            IActiveView workMapAV = workMap as IActiveView;
            workMapAV.Extent = inGeometry.Envelope;
            workMap.ClipGeometry = inGeometry.Envelope;
            workMap.MapScale = inScale;
            //ITopologicalOperator pTO = inGeometry as ITopologicalOperator;
            //IPolyline xzqBound = pTO.Boundary as IPolyline;
            //GeoPageLayoutFn.drawPolylineElement(xzqBound, workMap as IGraphicsContainer);
            IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
            pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
            pMapFrame.MapBounds = inGeometry.Envelope;
            IElement pMapEle = pMapFrame as IElement;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
            pMapEle.Geometry = pEnv;
            //更新范围图例
            GeoPageLayoutFn.updateMapSymbol(workMap, workMap.ClipGeometry);
                
            //添加图例
            if (hasLegend)
            {
                AddLegendZT(workPageLayout, workMap, workPW - mf2page[2], mf2page[1], 8, 2);
                //delUnnecLegends(workGra);//去除多余图例
            }
            IElement workRC = null;//外框
            workGra.Reset();
            tlEle = workGra.Next();
            while (tlEle != null)
            {
                IElementProperties pEP = tlEle as IElementProperties;
                if (pEP.Name == "RC")//外框
                    workRC = tlEle;
                tlEle = workGra.Next();
            }
            IEnvelope pEnv2 = new EnvelopeClass();
            pEnv2.PutCoords(rc2page[0], rc2page[1], workPW - rc2page[2], workPH - rc2page[3]);
            workRC.Geometry = pEnv2;
            double[] offXY = new double[2];//0X位移2Y位移
            offXY[0] = pEnv2.XMax - tlRCEnv.XMax;
            offXY[1] = pEnv2.YMax - tlRCEnv.YMax;
            moveElements(workGra, offXY);
            updateTextEle(workGra, pTextEles);
            CreateMeasuredGrid(workPageLayout);
            addCornerCoor(workGra, inGeometry);
            axPageLayoutControl1.PageLayout = workPageLayout;
            axPageLayoutControl1.ZoomToWholePage();
        }
        //生成地图公里网
        private void CreateMeasuredGrid(IPageLayout inPageLayout)
        {
            //PageLayout
            IActiveView pActiveView;
            IMap pMap;
            pActiveView = inPageLayout as IActiveView;
            pMap = pActiveView.FocusMap;
            IGraphicsContainer pGraCner = inPageLayout as IGraphicsContainer;
            IFrameProperties frameProperties = (IFrameProperties)pGraCner.FindFrame(pActiveView.FocusMap);
            //IStyleGallery pStyleGallery = new ServerStyleGalleryClass();
            //IStyleGalleryStorage pStyleGalleryStorage;
            //IEnumStyleGalleryItem pEnumStyleGalleryItem = new EnumServerStyleGalleryItemClass();
            //pStyleGalleryStorage = pStyleGallery as IStyleGalleryStorage;
            //pStyleGalleryStorage.AddFile(Application.StartupPath + @"\..\sTyles\pagelayout.ServerStyle");

            //pEnumStyleGalleryItem = pStyleGallery.get_Items("Borders", Application.StartupPath + @"\..\sTyles\pagelayout.ServerStyle", "");
            //pEnumStyleGalleryItem.Reset();
            //IStyleGalleryItem pEnumItem;
            //pEnumItem = pEnumStyleGalleryItem.Next();
            //while (pEnumItem != null)
            //{
            //    if (pEnumItem.Name == "图框")
            //    {
            //        frameProperties.Border = (IBorder)pEnumItem.Item;
            //        frameProperties.Border.Gap = 17;
            //        break;
            //    }
            //    pEnumItem = pEnumStyleGalleryItem.Next();
            //}

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pEnumStyleGalleryItem);
            //添加mapGrid到版式视图


            IMapGrids myMapGrids = (IMapGrids)pGraCner.FindFrame(pActiveView.FocusMap);
            myMapGrids.AddMapGrid(GeoPageLayoutFn.creategrid(true, inPageLayout));
            myMapGrids.AddMapGrid(GeoPageLayoutFn.creategrid(false, inPageLayout));
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);


        }
        //由范围生成极点数组
        private WKSPoint[] getPts(IGeometry inXZQ)
        {
            WKSPoint[] res = new WKSPoint[2];
            ISpatialReference pSpatialRefrence = inXZQ.SpatialReference;
            IEnvelope xzqEnv = inXZQ.Envelope;
            if (pSpatialRefrence is IProjectedCoordinateSystem)
            {
                IProjectedCoordinateSystem pPCS = pSpatialRefrence as IProjectedCoordinateSystem;
                WKSPoint pPointMin = new WKSPoint();
                pPointMin.X = xzqEnv.XMin;
                pPointMin.Y = xzqEnv.YMin;
                pPCS.Inverse(1, ref pPointMin);
                res[0] = pPointMin;
                WKSPoint pPointMax = new WKSPoint();
                pPointMax.X = xzqEnv.XMax;
                pPointMax.Y = xzqEnv.YMax;
                pPCS.Inverse(1, ref pPointMax);
                res[1] = pPointMax;
                return res;
            }
            else if (pSpatialRefrence is IGeographicCoordinateSystem)
            {
                WKSPoint pPointMin = new WKSPoint();
                pPointMin.X = xzqEnv.XMin;
                pPointMin.Y = xzqEnv.YMin;
                res[0] = pPointMin;
                WKSPoint pPointMax = new WKSPoint();
                pPointMax.X = xzqEnv.XMax;
                pPointMax.Y = xzqEnv.YMax;
                res[1] = pPointMax;
                return res;

            }
            else
                return null;

        }
        //更换角点坐标
        private void addCornerCoor(IGraphicsContainer inGra, IGeometry inGeometry)
        {

            WKSPoint[] pPts = getPts(inGeometry);
            if (pPts == null)
                return;
            inGra.Reset();
            IElement plele = inGra.Next();
            while (plele != null)
            {
                if (plele is ITextElement)
                {
                    ITextElement pTE = plele as ITextElement;
                    switch (pTE.Text)
                    {
                        case "左下角经度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[0].X, ref d, ref m, ref s);
                                pTE.Text = d + m + s;
                                break;
                            }
                        case "左下角\r\n纬度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[0].Y, ref d, ref m, ref s);
                                pTE.Text = d + "\r\n" + m + s;
                                break;
                            }
                        case "右下角经度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[1].X, ref d, ref m, ref s);
                                pTE.Text = d + m + s;
                                break;
                            }
                        case "右下角\r\n纬度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[0].Y, ref d, ref m, ref s);
                                pTE.Text = d + "\r\n" + m + s;
                                break;
                            }
                        case "左上角经度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[0].X, ref d, ref m, ref s);
                                pTE.Text = d + m + s;
                                break;
                            }
                        case "左上角\r\n纬度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[1].Y, ref d, ref m, ref s);
                                pTE.Text = d + "\r\n" + m + s;
                                break;
                            }
                        case "右上角经度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[1].X, ref d, ref m, ref s);
                                pTE.Text = d + m + s;
                                break;
                            }
                        case "右上角\r\n纬度":
                            {
                                string d = "", m = "", s = "";
                                degreesTostring(pPts[1].Y, ref d, ref m, ref s);
                                pTE.Text = d + "\r\n" + m + s;
                                break;
                            }


                    }
                }
                plele = inGra.Next();

            }


        }
        //更新地图标注
        private void updateTextEle(IGraphicsContainer inGra, Dictionary<string, string> inTexts)
        {
            foreach (KeyValuePair<string, string> kvp in inTexts)
            {
                IGraphicsContainer pgGC = inGra;
                pgGC.Reset();
                IElement pgEle = pgGC.Next();
                while (pgEle != null)
                {
                    if (pgEle is ITextElement)
                    {
                        if ((pgEle as IElementProperties).Name == kvp.Key || (pgEle as ITextElement).Text == kvp.Key)
                            (pgEle as ITextElement).Text = kvp.Value;

                    }
                    pgEle = pgGC.Next();
                }
            }
        }
        //移动周边元素
        private void moveElements(IGraphicsContainer inGra, double[] inOff)
        {
            inGra.Reset();
            IElement pEle = inGra.Next();
            while (pEle != null)
            {
                IElementProperties pEP = pEle as IElementProperties;
                if (pEP.Name.Contains("右下"))
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0], 0);
                //else if(pEP.Name.Contains("左下"))
                //    GeoPageLayoutFn.MoveElement(pEle, inOff[0], 0);
                else if (pEP.Name.Contains("左上"))
                    GeoPageLayoutFn.MoveElement(pEle, 0, inOff[1]);
                else if (pEP.Name.Contains("右上") || pEle is IPictureElement)
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0], inOff[1]);
                else if (pEP.Name == "副题")
                    GeoPageLayoutFn.MoveElement(pEle, 0, inOff[1]);
                else if (pEP.Name == "主题")
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0] / 2, inOff[1]);
                else if (pEP.Name == "比例尺")
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0] / 2, 0);
                pEle.Activate((inGra as IActiveView).ScreenDisplay);
                pEle = inGra.Next();
            }

        }
        //获取模板的制图对象
        private IPageLayout getTemplateGra(string inPath)
        {
            IPageLayout res = null;
            if (!File.Exists(inPath))
                return null;//1
            else
            {
                IMapDocument pMD = new MapDocumentClass();
                pMD.Open(inPath, "");
                res = pMD.PageLayout;
                pMD.Close();
                return res;//2
            }
        }
        #endregion 

        #region"Add Scale Bar"


        ///<summary>Add a Scale Bar to the Page Layout from the Map.</summary>
        ///
        ///<param name="pageLayout">An IPageLayout interface.</param>
        ///<param name="map">An IMap interface.</param>
        ///
        ///<remarks></remarks>
        public void AddScalebar(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map)
        {

            if (pageLayout == null || map == null)
            {
                return;
            }

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(0.2, 0.2, 1, 2); // Specify the location and size of the scalebar
            ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = "esriCarto.AlternatingScaleBar";

            // Create a Surround. Set the geometry of the MapSurroundFrame to give it a location
            // Activate it and add it to the PageLayout's graphics container
            ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = pageLayout as ESRI.ArcGIS.Carto.IGraphicsContainer; // Dynamic Cast
            ESRI.ArcGIS.Carto.IActiveView activeView = pageLayout as ESRI.ArcGIS.Carto.IActiveView; // Dynamic Cast
            ESRI.ArcGIS.Carto.IFrameElement frameElement = graphicsContainer.FindFrame(map);
            ESRI.ArcGIS.Carto.IMapFrame mapFrame = frameElement as ESRI.ArcGIS.Carto.IMapFrame; // Dynamic Cast
            ESRI.ArcGIS.Carto.IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null); // Dynamic Cast
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            element.Geometry = envelope;
            element.Activate(activeView.ScreenDisplay);
            graphicsContainer.AddElement(element, 0);
            ESRI.ArcGIS.Carto.IMapSurround mapSurround = mapSurroundFrame.MapSurround;


            ESRI.ArcGIS.Carto.IScaleBar markerScaleBar = ((ESRI.ArcGIS.Carto.IScaleBar)(mapSurround));
            markerScaleBar.LabelPosition = ESRI.ArcGIS.Carto.esriVertPosEnum.esriBelow;
            markerScaleBar.UseMapSettings();
        }
        #endregion


        ///<summary>Add a Legend to the Page Layout from the Map.</summary>
        ///
        ///<param name="pageLayout">An IPageLayout interface.</param>
        ///<param name="map">An IMap interface.</param>
        ///<param name="posX">A System.Double that is X coordinate value in page units for the start of the Legend.</param>
        ///<param name="posY">A System.Double that is Y coordinate value in page units for the start of the Legend. </param>
        ///<param name="legW">A System.Double that is length in page units of the Legend in both the X and Y direction.</param>
        /// 
        ///<remarks></remarks>
        public void AddLegend(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map, System.Double posX, System.Double posY, System.Double legW,int legendCol)
        {

            if (pageLayout == null || map == null)
            {
                return;
            }
            ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = pageLayout as ESRI.ArcGIS.Carto.IGraphicsContainer; // Dynamic Cast
            ESRI.ArcGIS.Carto.IMapFrame mapFrame = graphicsContainer.FindFrame(map) as ESRI.ArcGIS.Carto.IMapFrame; // Dynamic Cast
            ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = "esriCarto.Legend";
            ESRI.ArcGIS.Carto.IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame((ESRI.ArcGIS.esriSystem.UID)uid, null); // Explicit Cast


            ILegend2 pLegend = mapSurroundFrame.MapSurround as ILegend2;
            //将多边形移到上面
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.ShowDescriptions = false;
                pLegendItem.ShowHeading = false;
                pLegendItem.ShowLayerName = false;
                ILayer tmp = pLegendItem.Layer;
                if (tmp.Valid)
                {
                    if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pLegend.RemoveItem(i);
                        pLegend.InsertItem(0, pLegendItem);
                    }

                }
                else
                    pLegend.RemoveItem(i);


            }
            //将点移到下面
            for (int i = pLegend.ItemCount - 1; i >= 0; i--)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                ILayer tmp = pLegendItem.Layer;
                if (tmp.Valid)
                {
                    if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pLegend.RemoveItem(i);
                        pLegend.InsertItem(pLegend.ItemCount, pLegendItem);//remove导致itemcount时时变化，cautions
                    }
                }
                else
                    pLegend.RemoveItem(i);
            }


            pLegend.Title = "图 例";
            pLegend.AdjustColumns(legendCol);//yjl20110812
            pLegend.Refresh();
            //Get aspect ratio
            ESRI.ArcGIS.Carto.IQuerySize querySize = mapSurroundFrame.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX, (posY - legW / aspectRatio), (posX + legW), posY);
            
           
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.AnchorPoint = esriAnchorPointEnum.esriTopLeftCorner;
            pep.Name = "图例";

            element.Activate((pageLayout as IActiveView).ScreenDisplay);

            graphicsContainer.AddElement(element, 0);
        }
        public void AddLegendZT(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map, System.Double posX, System.Double posY, System.Double legW, int legendCol)
        {

            if (pageLayout == null || map == null)
            {
                return;
            }
            ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = pageLayout as ESRI.ArcGIS.Carto.IGraphicsContainer; // Dynamic Cast
            ESRI.ArcGIS.Carto.IMapFrame mapFrame = graphicsContainer.FindFrame(map) as ESRI.ArcGIS.Carto.IMapFrame; // Dynamic Cast
            ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = "esriCarto.Legend";
            ESRI.ArcGIS.Carto.IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame((ESRI.ArcGIS.esriSystem.UID)uid, null); // Explicit Cast


            ILegend2 pLegend = mapSurroundFrame.MapSurround as ILegend2;
            //将多边形移到上面
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.ShowDescriptions = false;
                pLegendItem.ShowHeading = false;
                pLegendItem.ShowLayerName = false;
                ILayer tmp = pLegendItem.Layer;
                if (tmp.Valid)
                {
                    if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pLegend.RemoveItem(i);
                        pLegend.InsertItem(0, pLegendItem);
                    }

                }
                else
                    pLegend.RemoveItem(i);


            }
            //将点移到下面
            for (int i = pLegend.ItemCount - 1; i >= 0; i--)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                ILayer tmp = pLegendItem.Layer;
                if (tmp.Valid)
                {
                    if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pLegend.RemoveItem(i);
                        pLegend.InsertItem(pLegend.ItemCount, pLegendItem);//remove导致itemcount时时变化，cautions
                    }
                }
                else
                    pLegend.RemoveItem(i);
            }


            pLegend.Title = "图 例";
            pLegend.AdjustColumns(legendCol);//yjl20110812
            pLegend.Refresh();
            //Get aspect ratio
            ESRI.ArcGIS.Carto.IQuerySize querySize = mapSurroundFrame.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;
           
            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX-legW, posY, posX, posY+(legW / aspectRatio));
            mapSurroundFrame.Border = createBorder(envelope, pageLayout as IActiveView);//边框，diff
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.Name = "图例";
            pep.AnchorPoint = esriAnchorPointEnum.esriBottomRightCorner;
            element.Activate((pageLayout as IActiveView).ScreenDisplay);//关键代码
            graphicsContainer.AddElement(element, 0);
           
            
        }

        //生成border
        private IBorder createBorder(IGeometry inGeometry, IActiveView inAV)
        {
            ISymbolBorder pSymbolBorder = new SymbolBorderClass();
            pSymbolBorder.GetGeometry(inAV.ScreenDisplay as IDisplay, inGeometry);
            ILineSymbol pLSymbol = new SimpleLineSymbolClass();
            pLSymbol.Color = getRGB(0, 0, 0);
            pLSymbol.Width = 1;
            pSymbolBorder.LineSymbol = pLSymbol;


            return pSymbolBorder as IBorder;
        }

        ///<summary>Add a North Arrow to the Page Layout from the Map.</summary>
        ///      
        ///<param name="pageLayout">An IPageLayout interface.</param>
        ///<param name="map">An IMap interface.</param>
        ///      
        ///<remarks></remarks>
        ///
        public void AddNorthArrow(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map,double inXMin,double inYMax)
        {

            if (pageLayout == null || map == null)
            {
                return;
            }
            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(inXMin+4, inYMax - 10, inXMin+13, inYMax - 5); //  Specify the location and size of the north arrow

            ESRI.ArcGIS.esriSystem.IUID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = "esriCarto.MarkerNorthArrow";

            // Create a Surround. Set the geometry of the MapSurroundFrame to give it a location
            // Activate it and add it to the PageLayout's graphics container
            ESRI.ArcGIS.Carto.IGraphicsContainer graphicsContainer = pageLayout as ESRI.ArcGIS.Carto.IGraphicsContainer; // Dynamic Cast
            ESRI.ArcGIS.Carto.IActiveView activeView = pageLayout as ESRI.ArcGIS.Carto.IActiveView; // Dynamic Cast
            ESRI.ArcGIS.Carto.IFrameElement frameElement = graphicsContainer.FindFrame(map);
            ESRI.ArcGIS.Carto.IMapFrame mapFrame = frameElement as ESRI.ArcGIS.Carto.IMapFrame; // Dynamic Cast
            ESRI.ArcGIS.Carto.IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid as ESRI.ArcGIS.esriSystem.UID, null); // Dynamic Cast
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            element.Geometry = envelope;
            element.Activate(activeView.ScreenDisplay);
            graphicsContainer.AddElement(element, 0);
            ESRI.ArcGIS.Carto.IMapSurround mapSurround = mapSurroundFrame.MapSurround;

            // Change out the default north arrow
            ESRI.ArcGIS.Carto.IMarkerNorthArrow markerNorthArrow = mapSurround as ESRI.ArcGIS.Carto.IMarkerNorthArrow; // Dynamic Cast
            ESRI.ArcGIS.Display.IMarkerSymbol markerSymbol = markerNorthArrow.MarkerSymbol;
            ESRI.ArcGIS.Display.ICharacterMarkerSymbol characterMarkerSymbol = markerSymbol as ESRI.ArcGIS.Display.ICharacterMarkerSymbol; // Dynamic Cast
            characterMarkerSymbol.CharacterIndex = 175; // change the symbol for the North Arrow
            markerNorthArrow.MarkerSymbol = characterMarkerSymbol;
        }


       //根据符号模板产生实际地图for autoPageLayout
        void createMap()
        {
               IGroupLayer tmpGP = new GroupLayerClass();
            cMap = new MapClass();
            tmpGP.Name = "森林资源现状图";
            cMap.AddLayer(tmpGP as ILayer);
            IMapLayers pMapLayers = cMap as IMapLayers;
            for(int iMD=0;iMD<cMD.get_Map(0).LayerCount;iMD++)
            {


            IWorkspaceFactory Pwf = new AccessWorkspaceFactoryClass();
            IWorkspace pws = (IWorkspace)(Pwf.OpenFromFile(cMdbPath, 0));
            IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pws;
         
            
            for (int i = 0; i < cfnamelst.Count; i++)
            {
                IFeatureClass tmpfeatureclass = null;
                //IFeatureLayer cFL = new FeatureLayerClass();
                string[] lname = cfnamelst[i].Split('_');

                if (cMD.get_Map(0).get_Layer(iMD).Name == lname[0])
                {
                    ILayer tmp=cMD.get_Map(0).get_Layer(iMD);
                       tmpfeatureclass=pFeatureWorkspace.OpenFeatureClass(lname[1]);
                       (tmp as IFeatureLayer).FeatureClass = tmpfeatureclass;
                       tmp.Visible = true;
                    if((tmp as IFeatureLayer).FeatureClass.FeatureCount(null)!=0)
                       pMapLayers.InsertLayerInGroup(tmpGP, tmp, false, (tmpGP as ICompositeLayer).Count);
                }
            }

            }
            
        }

        //根据符号模板产生实际地图for dynamicExtentPageLayout
        //void createMap2()
        //{
        //    IGroupLayer tmpGP = new GroupLayerClass();
        //    cMap = new MapClass();
        //    tmpGP.Name = "森林资源现状图";
        //    cMap.AddLayer(tmpGP as ILayer);
        //    IMapLayers pMapLayers = cMap as IMapLayers;
        //    for (int iMD = 0; iMD < cMD.get_Map(0).LayerCount; iMD++)
        //    {


        //        IWorkspaceFactory Pwf = new AccessWorkspaceFactoryClass();
        //        IWorkspace pws = (IWorkspace)(Pwf.OpenFromFile(cMdbPath, 0));
        //        IEnumDataset pED = pws.get_Datasets(esriDatasetType.esriDTFeatureClass); 
        //        IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pws;
        //        pED.Reset();
        //        IFeatureClass  pFc=pED.Next() as IFeatureClass;
        //        while (pFc != null)
        //        {
        //            IFeatureClass tmpfeatureclass = null;
        //            //IFeatureLayer cFL = new FeatureLayerClass();
        //            string[] lname = cfnamelst[i].Split('_');

        //            if (cMD.get_Map(0).get_Layer(iMD).Name == lname[0])
        //            {
        //                ILayer tmp = cMD.get_Map(0).get_Layer(iMD);
        //                tmpfeatureclass = pFeatureWorkspace.OpenFeatureClass(lname[1]);
        //                (tmp as IFeatureLayer).FeatureClass = tmpfeatureclass;
        //                tmp.Visible = true;
        //                if ((tmp as IFeatureLayer).FeatureClass.FeatureCount(null) != 0)
        //                    pMapLayers.InsertLayerInGroup(tmpGP, tmp, false, (tmpGP as ICompositeLayer).Count);
        //            }
        //        }

        //    }

        //}

        void setfnamelst()
        {
            cXmlDoc = new XmlDocument();
            
                
            if (cXmlDoc != null && File.Exists(cPath))
            {
                cXmlDoc.Load(cPath);


                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("SubMapType");
                for (int j = 0; j < clistview.Count; j++)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if ((xn as XmlElement).GetAttribute("ItemName").ToString() != "森林资源现状图") continue;
                        for (int i = 0; i < xn.ChildNodes.Count; i++)
                        {
                            
                                if (xn.ChildNodes[i].Attributes["sItemName"].Value == clistview[j])
                                    cfnamelst.Add(clistview[j]+ "_"+ xn.ChildNodes[i].Attributes["sFile"].Value);
                            
                        }


                    }
                }

                cXmlDoc = null;
            }
        }
        private void axPageLayoutControl1_OnPageLayoutReplaced(object sender, IPageLayoutControlEvents_OnPageLayoutReplacedEvent e)
        {
            //if (axPageLayoutControl1.ActiveView.FocusMap.LayerCount != null)
            //{
            //    if (MessageBox.Show("是否保存当前地图文档？", "提示", MessageBoxButtons.YesNo,
            //               MessageBoxIcon.Information) == DialogResult.Yes)
            //    {
            //        ICommand pCmd = new ControlsSaveAsDocCommandClass();
            //        pCmd.OnCreate(axPageLayoutControl1.Object);
            //        pCmd.OnClick();
            //    }
 
            //}
            
        }


        //inner function start

        void addText()
        {
            ITextElement pTextElement = new TextElementClass();//title
            ITextElement pTextElement2 = new TextElementClass();//scale
            ITextElement pTextElement3 = new TextElementClass();
            //ITextElement pTextElement4 = new TextElementClass();


            ITextSymbol pTextSymbol = new TextSymbolClass();
            stdole.StdFont mySedFont = new stdole.StdFontClass();
            mySedFont.Name = "Times New Roman";
            mySedFont.Size = 180;
            pTextSymbol.Font = mySedFont as IFontDisp;

            pTextElement.Text = xzq+"森林资源现状图";

            pTextElement.Symbol = pTextSymbol;
            double wd = 0, hi = 0;

            IElement pele = pTextElement as IElement;
            axPageLayoutControl1.Page.QuerySize(out wd, out hi);
            pele.Geometry = XYtoP(wd / 2, hi - 8.5);


            axPageLayoutControl1.GraphicsContainer.AddElement(pele, 0);

            pTextSymbol = new TextSymbolClass();
            mySedFont = new stdole.StdFontClass();
            mySedFont.Name = "Times New Roman";
            mySedFont.Size = 80;
            pTextSymbol.Font = mySedFont as IFontDisp;



            pTextElement2.Text = "1:"+cScale.ToString();
            pTextElement2.Symbol = pTextSymbol;
            (pTextElement2 as IElement).Geometry = XYtoP(wd / 2, 1.5);


            axPageLayoutControl1.GraphicsContainer.AddElement(pTextElement2 as IElement, 0);



        }



        private IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pColor;
            pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }


        private void addCornerCoor()
        {

            this.axPageLayoutControl1.GraphicsContainer.Reset();
            IElement plele = this.axPageLayoutControl1.GraphicsContainer.Next();
            int i = 1;
            while (plele != null)
            {
                if ((plele as IElementProperties3).Name == "1") //i++;
                {
                    this.axPageLayoutControl1.GraphicsContainer.DeleteElement(plele);
                    this.axPageLayoutControl1.GraphicsContainer.Reset();
                }
                plele = this.axPageLayoutControl1.GraphicsContainer.Next();

            }

            ////MessageBox.Show(i.ToString());
            IGeographicCoordinateSystem gcs = new GeographicCoordinateSystemClass();// pProjectedGrid.SpatialReference as IGeographicCoordinateSystem;
            IProjectedCoordinateSystem pcs = this.axPageLayoutControl1.ActiveView.FocusMap.SpatialReference as IProjectedCoordinateSystem;
            gcs = pcs.GeographicCoordinateSystem;
            this.axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = gcs;
            //IDisplayTransformation dt = (this.axPageLayoutControl1.ActiveView.FocusMap as IActiveView).ScreenDisplay.DisplayTransformation;   //(pEnvelope.UpperRight, pEnvelope.UpperRight.Y);
            //IEnvelope pEnvelope = dt.VisibleBounds;
            //get page coor of corner of mapframe
            IMapFrame mf = (IMapFrame)this.axPageLayoutControl1.GraphicsContainer.FindFrame(this.axPageLayoutControl1.ActiveView.FocusMap);

            this.axPageLayoutControl1.Page.Units = esriUnits.esriPoints;

            IEnvelope pEnvelope = (mf as IElement).Geometry.Envelope;
            double ddd = mf.MapScale;
            double borderwidth = 17;

            double MaxX1 = 0, MaxY1 = 0, MinX1 = 0, MinY1 = 0;
            MaxX1 = pEnvelope.XMax;
            MaxY1 = pEnvelope.YMax;
            MinX1 = pEnvelope.XMin;
            MinY1 = pEnvelope.YMin;



            string d = "", m = "", s = "";
            degreesTostring(MaxX1, ref d, ref m, ref s);

            //MessageBox.Show(d + m + s);

            drawLineElement(XYtoP(MinX1, MinY1), 0, borderwidth);
            drawLineElement(XYtoP(MaxX1, MinY1), 1, borderwidth);
            drawLineElement(XYtoP(MinX1, MaxY1), 2, borderwidth);
            drawLineElement(XYtoP(MaxX1, MaxY1), 3, borderwidth);



            pEnvelope = mf.MapBounds;
            double MaxX2 = 0, MaxY2 = 0, MinX2 = 0, MinY2 = 0;
            MaxX2 = pEnvelope.XMax;
            MaxY2 = pEnvelope.YMax;
            MinX2 = pEnvelope.XMin;
            MinY2 = pEnvelope.YMin;
            string du = "", fen = "", miao = "", du1 = "", fen1 = "", miao1 = "";
            degreesTostring(MinX2, ref du, ref fen, ref miao);
            string d1 = du + fen + miao;
            //string ms=fen + miao;
            degreesTostring(MaxX2, ref du, ref fen, ref miao);
            string d2 = du + " " + fen + miao;
            //string ms2=fen + miao;
            degreesTostring(MinY2, ref du, ref fen, ref miao);
            degreesTostring(MaxY2, ref du1, ref fen1, ref miao1);
            drawTextElement(XYtoP(MinX1, MinY1), 0, borderwidth, du, fen, miao, d1);
            drawTextElement(XYtoP(MaxX1, MinY1), 1, borderwidth, du, fen, miao, d2);
            drawTextElement(XYtoP(MinX1, MaxY1), 2, borderwidth, du1, fen1, miao1, d1);
            drawTextElement(XYtoP(MaxX1, MaxY1), 3, borderwidth, du1, fen1, miao1, d2);

            this.axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pcs;
            this.axPageLayoutControl1.Page.Units = esriUnits.esriCentimeters;
            //this.axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }
        IPoint XYtoP(double x, double y)
        {
            IPoint p = new PointClass();
            p.X = x;
            p.Y = y;
            return p;

        }

        void drawTextElement(IPoint p, int orient, double length, string d, string m, string s, string dms)
        {
            ITextElement pTextElement = new TextElementClass();
            ITextElement pTextElement2 = new TextElementClass();
            ITextElement pTextElement3 = new TextElementClass();
            //ITextElement pTextElement4 = new TextElementClass();


            ITextSymbol pTextSymbol = new TextSymbolClass();
            stdole.StdFont mySedFont = new stdole.StdFontClass();
            mySedFont.Name = "Times New Roman";
            mySedFont.Size = 7;

            IPoint p1 = new PointClass(), p3 = new PointClass();
            switch (orient)
            {
                case 3: p1.X = p.X + length / 2; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y + length - 5.5; break;
                case 2: p1.X = p.X - length / 2; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y + length - 5.5; break;
                case 1: p1.X = p.X + length / 2; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y - length + 0.5; break;
                case 0: p1.X = p.X - length / 2; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y - length + 0.5; break;

            }
            IRgbColor color = new RgbColorClass();
            color.Blue = 0;
            color.Green = 0;
            color.Red = 0;
            pTextSymbol.Font = mySedFont as IFontDisp;
            pTextSymbol.Color = color;
            pTextElement.Text = d;
            pTextElement.Symbol = pTextSymbol;
            (pTextElement as IElement).Geometry = (IGeometry)p1;
            //(pTextElement as IElementProperties3).AnchorPoint = esriAnchorPointEnum.esriBottomMidPoint;
            //(pTextElement as IElementProperties3).AutoTransform = true;
            (pTextElement as IElementProperties3).Name = "1";

            this.axPageLayoutControl1.GraphicsContainer.AddElement(pTextElement as IElement, 0);
            IElement pele = pTextElement as IElement;
            this.axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pele, null);
            //this.axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //add fenmiao
            pTextElement2.Symbol = pTextSymbol;
            pTextElement2.Text = m + s;


            (pTextElement2 as IElement).Geometry = (IGeometry)XYtoP(p1.X, p1.Y - 6);
            (pTextElement2 as IElementProperties3).Name = "1";
            this.axPageLayoutControl1.GraphicsContainer.AddElement(pTextElement2 as IElement, 0);
            //IElement pele=(this.axPageLayoutControl1.PageLayout as IGraphicsContainerSelect).SelectedElements.Next() ;
            //(this.axPageLayoutControl1.PageLayout as IGraphicsContainerSelect).SelectElement(pTextElement2 as IElement);
            //(pele as IElementProperties3).AnchorPoint = esriAnchorPointEnum.esriTopMidPoint;
            //(pele as IElement).Geometry = (IGeometry)XYtoP(p1.X,p1.Y-6);
            //(pele as IElementProperties3).AutoTransform = true;
            //(pele as IElementProperties3).Name = "1";
            //this.axPageLayoutControl1.GraphicsContainer.UpdateElement(pTextElement2 as IElement);
            pele = pTextElement2 as IElement;
            this.axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pele, null);
            //add longetude
            pTextElement3.Symbol = pTextSymbol;
            pTextElement3.Text = dms;
            //(pTextElement3 as IElementProperties3).AnchorPoint = esriAnchorPointEnum.esriBottomRightCorner;
            (pTextElement3 as IElement).Geometry = (IGeometry)XYtoP(p3.X + 2, p3.Y);
            (pTextElement3 as IElementProperties3).Name = "1";
            this.axPageLayoutControl1.GraphicsContainer.AddElement(pTextElement3 as IElement, 0);
            pele = pTextElement3 as IElement;
            this.axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pele, null);
            //pTextElement4.Symbol = pTextSymbol;
            //pTextElement4.Text = ms;
            //(pTextElement4 as IElementProperties3).AnchorPoint = esriAnchorPointEnum.esriCenterPoint;
            //(pTextElement4 as IElement).Geometry = (IGeometry)XYtoP(p3.X + 8, p3.Y);
            //this.axPageLayoutControl1.GraphicsContainer.AddElement(pTextElement4 as IElement, 0);


        }

        void drawLineElement(IPoint p, int orient, double length)
        {
            ILineElement pLineElement = new LineElementClass();

            IPolyline pPolyline = new PolylineClass();
            IPointCollection pc = pPolyline as IPointCollection;
            object Missing = Type.Missing;
            IPoint p1 = new PointClass(), p3 = new PointClass();
            switch (orient)
            {
                case 3: p1.X = p.X + length; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y + length; break;
                case 2: p1.X = p.X - length; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y + length; break;
                case 1: p1.X = p.X + length; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y - length; break;
                case 0: p1.X = p.X - length; p1.Y = p.Y; p3.X = p.X; p3.Y = p.Y - length; break;

            }
            pc.AddPoint(p1, ref Missing, ref Missing);
            pc.AddPoint(p, ref Missing, ref Missing);
            pc.AddPoint(p3, ref Missing, ref Missing);
            IRgbColor color = new RgbColorClass();
            color.Blue = 0;
            color.Green = 0;
            color.Red = 0;
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 0.2;
            pOutline.Color = color;
            (pLineElement as IElement).Geometry = pPolyline;
            pLineElement.Symbol = pOutline;
            (pLineElement as IElementProperties3).Name = "1";
            this.axPageLayoutControl1.GraphicsContainer.AddElement(pLineElement as IElement, 0);
            IElement pele = pLineElement as IElement;
            this.axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pele, null);

        }
        private void degreesTostring(double x, ref string deg, ref string min, ref string sec)
        {
            int degree = (int)Math.Floor(x);
            int minute = (int)Math.Floor((x - degree) * 60);
            int second = (int)Math.Floor(((x - degree) * 60 - minute) * 60);

            deg = degree + "°";
            min = minute + "′";
            sec = second + "″";

        }

        private void FrmPageLayout_FormClosed(object sender, FormClosedEventArgs e)
        {
           
            //cMD.Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //ITool ElementSelect = new ESRI.ArcGIS.Controls.ControlsSelectToolClass();
            //(ElementSelect as ICommand).OnCreate(axPageLayoutControl1.Object);
            //axPageLayoutControl1.CurrentTool = ElementSelect;

            string progID = "esriControls.ControlsSelectTool";
            int index = axToolbarControl1.Find(progID);
            if (index != -1)
            {
                IToolbarItem toolItem = axToolbarControl1.GetItem(index);
                ICommand _cmd = toolItem.Command;
                ITool _tool = (ITool)_cmd;
                axPageLayoutControl1.CurrentTool = _tool;
            }
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            ITool ElementRotate = new ESRI.ArcGIS.Controls.ControlsRotateElementToolClass();
            (ElementRotate as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementRotate;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewCircleToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;

        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewRectangleToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewPolygonToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;

        }

        private void btnEllipse_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewEllipseToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnZline_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewLineToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnRline_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewFreeHandToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ESRI.ArcGIS.Controls.ControlsNewMarkerToolClass();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnTextEle_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ElementCommandTool.TbTextNormal();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnMTextEle_Click(object sender, EventArgs e)
        {

            ITool ElementNew = new ElementCommandTool.TbTextLabel();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;

        }

        private void btnLegend_Click(object sender, EventArgs e)
        {
            ICommand ElementNew = new ElementCommandTool.AddLegendCommand() as ICommand;
            ElementNew.OnCreate(axPageLayoutControl1.Object);
            ElementNew.OnClick();

        }

        private void btnNorthArrow_Click(object sender, EventArgs e)
        {

            ITool ElementNew = new ElementCommandTool.AddNorthArrowTool();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnScaleBar_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ElementCommandTool.AddScaleBarTool();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnScalebarText_Click(object sender, EventArgs e)
        {
            ITool ElementNew = new ElementCommandTool.AddScaleTextTool();
            (ElementNew as ICommand).OnCreate(axPageLayoutControl1.Object);
            axPageLayoutControl1.CurrentTool = ElementNew;
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            ICommand ElementNew = new ElementCommandTool.AddPictureCommand() as ICommand;
            ElementNew.OnCreate(axPageLayoutControl1.Object);
            ElementNew.OnClick();
        }

        private void btnElementEdit_Click(object sender, EventArgs e)
        {
            ICommand ElementNew = new ElementCommandTool.ElementEditCommand() as ICommand;
            ElementNew.OnCreate(axPageLayoutControl1.Object);
            ElementNew.OnClick();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            try
            {
                printpreview();
            }
            catch (Exception exError)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
            }
        }


        void printpreview()
        {
            printPreviewDialogksk = new PrintPreviewDialog();
            //printPreviewDialogksk.SetDesktopBounds(10, 10, , this.Width);
            //printPreviewDialogksk.ClientSize = new System.Drawing.Size(800, 600);
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            printPreviewDialogksk.ClientSize = new System.Drawing.Size(workingRectangle.Width, workingRectangle.Height);

            printPreviewDialogksk.Location = new System.Drawing.Point(29, 29);
            printPreviewDialogksk.Name = "";
            printPreviewDialogksk.MinimumSize = new System.Drawing.Size(375, 250);
            printPreviewDialogksk.UseAntiAlias = true;
            kskdocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(document_PrintPage);
            m_CurrentPrintPage = 0;
            //if (axPageLayoutControl1.DocumentFilename == null) return;
            kskdocument.DocumentName = axPageLayoutControl1.DocumentFilename;
            printPreviewDialogksk.Document = kskdocument;
            printPreviewDialogksk.ShowDialog();
        }


        private void document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingTile;
            //axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingCrop;
            axPageLayoutControl1.Page.PageToPrinterMapping = esriPageToPrinterMapping.esriPageMappingScale;
            short dpi = (short)e.Graphics.DpiX;
            IEnvelope devBounds = new EnvelopeClass();
            IPage page = axPageLayoutControl1.Page;
            short printPageCount;
            printPageCount = axPageLayoutControl1.get_PrinterPageCount(0);
            m_CurrentPrintPage++;
            IPrinter printer = axPageLayoutControl1.Printer;
            page.GetDeviceBounds(printer, m_CurrentPrintPage, 0, dpi, devBounds);
            tagRECT deviceRect;
            double xmin, ymin, xmax, ymax;
            devBounds.QueryCoords(out xmin, out ymin, out xmax, out ymax);
            deviceRect.bottom = (int)ymax;
            deviceRect.left = (int)xmin;
            deviceRect.top = (int)ymin;
            deviceRect.right = (int)xmax;
            IEnvelope visBounds = new EnvelopeClass();
            page.GetPageBounds(printer, m_CurrentPrintPage, 0, visBounds);
            IntPtr hdc = e.Graphics.GetHdc();
            axPageLayoutControl1.ActiveView.Output(hdc.ToInt32(), dpi, ref deviceRect, visBounds, m_TrackCancel);
            e.Graphics.ReleaseHdc(hdc);
            if (m_CurrentPrintPage < printPageCount)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            
            try
            {
                printDialogksk = new PrintDialog();
                printDialogksk.AllowSomePages = true;
                //printDialogksk.ShowHelp = true;
                printDialogksk.Document = kskdocument;
                DialogResult result = printDialogksk.ShowDialog();
                if (result == DialogResult.OK) kskdocument.Print();
            }
            catch (Exception exError)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
            }

        }

        private void btnLayoutPageSet_Click(object sender, EventArgs e)
        {
            FormPrintPageSet layoutPageSet = new FormPrintPageSet(axPageLayoutControl1);
            layoutPageSet.Show();
        }

        private void btnTDLYXZ_Click(object sender, EventArgs e)
        {
            axTOCControl1.SetBuddyControl(null);
            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            axPageLayoutControl1.ActiveView.Refresh();
        }

        private void btnSelectaFeatureExtent_Click(object sender, EventArgs e)
        {
            ICommand  _cmd = new CommandSelOutmap();
            CommandSelOutmap TempCommand = _cmd as CommandSelOutmap;
            TempCommand.WriteLog = this.WriteLog;//ygc 2012-9-12 是否写日志
            TempCommand.OnCreate(axPageLayoutControl1.Object);
            TempCommand.OnClick();
        }

        private void btnRectangleExtent_Click(object sender, EventArgs e)
        {

            //ICommand _cmd = new CommandRectangleOutmap(pEnvelope);
            //_cmd.OnCreate(axPageLayoutControl1.Object);
            //axPageLayoutControl1.CurrentTool = _cmd as ITool;

        }

        private void btn5W_Click(object sender, EventArgs e)
        {

            OutBZFFT(50000,"E");
        }
        //标准分幅图制作
        private bool OutBZFFT(int scale,string strscale)
        {
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            
            FrmSheetMapSet frmSMS = new FrmSheetMapSet(scale, strscale);
            frmSMS.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            if (frmSMS.ShowDialog() != DialogResult.OK)
                return false;
            axTOCControl1.SetBuddyControl(null);
            DeleteAllEleExpMap();
            pgTextElements = frmSMS.MapTextElements;
          

            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1.Object as IPageLayoutControl;
            pDrawSheetMap.vScale = Convert.ToUInt32(pgTextElements["比例尺"].Substring(2));
            pDrawSheetMap.m_intPntCount = 3;
            pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            string[] astrMapNo = pgTextElements["G50G005005"].Split(' ');//若图幅号带空格 则去空格
            string realMapNo = "";
            foreach (string str in astrMapNo)
            {
                realMapNo += str;
            }
            pDrawSheetMap.m_strSheetNo = realMapNo;//图幅号

            //根据图幅号获得投影文件
            if (axPageLayoutControl1.ActiveView.FocusMap.SpatialReference is IProjectedCoordinateSystem)
            {
                pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            }
            else
            {
                ISpatialReference pSpa = GetSpatialByMapNO(pDrawSheetMap.m_strSheetNo);
                if (pSpa == null)
                {
                    MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                pDrawSheetMap.m_pPrjCoor = pSpa;
                axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pSpa;
            }

            pDrawSheetMap.DrawSheetMap();

            foreach (KeyValuePair<string, string> kvp in pgTextElements)
            {
                IGraphicsContainer pgGC = axPageLayoutControl1.GraphicsContainer;
                pgGC.Reset();
                IElement pgEle = pgGC.Next();
                while (pgEle != null)
                {
                    if (pgEle is ITextElement && (pgEle as ITextElement).Text == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;

                    }
                    pgEle = pgGC.Next();
                }
            }

            axTOCControl1.SetBuddyControl(axPageLayoutControl1.Object);
            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            if (pGCS.ElementSelectionCount != 0)
            {
                pGCS.UnselectAllElements();
 
            }
            //(axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Refresh();
            axPageLayoutControl1.Refresh();
            //cMD.Save(true, false);

            return true;
 
        }
        //标准分幅图制作 从接合图表调用//20110914yjl add typeZT专题类型0地形图1二调，以获取不同的模板
        private bool OutBZFFT2(int scale, string strMapNo,int typeZT)
        {
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            if (typeZT == 0)
            {
                FrmSheetMapSet frmSMS = new FrmSheetMapSet(scale, "", strMapNo);
                frmSMS.WriteLog = WriteLog;//2012-9-12 是否写日志
                if (frmSMS.ShowDialog() != DialogResult.OK)
                {
                    this.Dispose();
                    return false;
                }
                pgTextElements = frmSMS.MapTextElements;
                hasLegend = frmSMS.HasLegend;
            }
            else if (typeZT ==1)
            {
                FrmSheetMapSet_ZT frmSMS = new FrmSheetMapSet_ZT(scale, "", strMapNo);
                frmSMS.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                if (frmSMS.ShowDialog() != DialogResult.OK)
                {
                    this.Dispose();
                    return false;
                }
                pgTextElements = frmSMS.MapTextElements;
                hasLegend = frmSMS.HasLegend;
 
            }


            axTOCControl1.SetBuddyControl(null);
            DeleteAllEleExpMap();

            axPageLayoutControl1.ActiveView.FocusMap.Name = pgTextElements["图名"];

            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1.Object as IPageLayoutControl;
            pDrawSheetMap.vScale = Convert.ToUInt32(pgTextElements["比例尺"].Substring(2));
            pDrawSheetMap.m_intPntCount = 3;
            pDrawSheetMap.type_ZT = typeZT;//专题类型
            pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            string[] astrMapNo = pgTextElements["G50G005005"].Split(' ');//若图幅号带空格 则去空格
            string realMapNo = "";
            foreach (string str in astrMapNo)
            {
                realMapNo += str;
            }
            pDrawSheetMap.m_strSheetNo = realMapNo;//图幅号

            //根据图幅号获得投影文件
            if (axPageLayoutControl1.ActiveView.FocusMap.SpatialReference is IProjectedCoordinateSystem)
            {
                pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            }
            else
            {
                ISpatialReference pSpa = GetSpatialByMapNO(pDrawSheetMap.m_strSheetNo);
                if (pSpa == null)
                {
                    MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                pDrawSheetMap.m_pPrjCoor = pSpa;
                axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pSpa;
            }

            pDrawSheetMap.DrawSheetMap();

            foreach (KeyValuePair<string, string> kvp in pgTextElements)
            {
                IGraphicsContainer pgGC = axPageLayoutControl1.GraphicsContainer;
                pgGC.Reset();
                IElement pgEle = pgGC.Next();
                while (pgEle != null)
                {
                    if (pgEle is ITextElement && (pgEle as ITextElement).Text == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;

                    }
                    pgEle = pgGC.Next();
                }
            }

            axTOCControl1.SetBuddyControl(axPageLayoutControl1.Object);
            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            if (pGCS.ElementSelectionCount != 0)
            {
                pGCS.UnselectAllElements();

            }
            //(axPageLayoutControl1.ActiveView.FocusMap as IActiveView).Refresh();
            axPageLayoutControl1.Refresh();
            //cMD.Save(true, false);

            return true;

        }
        private void DeleteAllEleExpMap()
        {
            this.axPageLayoutControl1.GraphicsContainer.Reset();
            IElement plele = this.axPageLayoutControl1.GraphicsContainer.Next();

            while (plele != null)
            {
                if (!(plele is IMapFrame)) //i++;
                {
                    this.axPageLayoutControl1.GraphicsContainer.DeleteElement(plele);
                    this.axPageLayoutControl1.GraphicsContainer.Reset();
                }
                plele = this.axPageLayoutControl1.GraphicsContainer.Next();

            }
        }

        private void axPageLayoutControl1_OnMouseMove(object sender, IPageLayoutControlEvents_OnMouseMoveEvent e)
        {
            SSLblPageX.Text = Math.Round(e.pageX,2).ToString();
            SSLabelPageY.Text = Math.Round(e.pageY,2).ToString();
            
        }

        private void btn5K_Click(object sender, EventArgs e)
        {
            OutBZFFT(5000,"H");
        }

        private void btn1W_Click(object sender, EventArgs e)
        {
            OutBZFFT(10000, "G");
        }

        private void btn2W5_Click(object sender, EventArgs e)
        {
            OutBZFFT(25000, "F");
        }
        private void btn500_Click(object sender, EventArgs e)
        {
            OutBZFFTBigScale(500);
        }
        //从比例尺调用
        private bool OutBZFFTBigScale(int scale)
        {
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            FrmSheetMapSet2 frmSMS2 = new FrmSheetMapSet2(scale);
            frmSMS2.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            if (frmSMS2.ShowDialog() != DialogResult.OK)
                return false;
            axTOCControl1.SetBuddyControl(null);
            DeleteAllEleExpMap();
            pgTextElements = frmSMS2.MapTextElements;
          

            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1.Object as IPageLayoutControl;
            pDrawSheetMap.vScale = Convert.ToUInt32(pgTextElements["比例尺"].Substring(2));
            pDrawSheetMap.m_intPntCount = 3;
            pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            string[] MidXY = pgTextElements["G50G005005"].Trim().Split('-');
            string FullX = frmSMS2.GWX+ MidXY[1];
            string FullY = frmSMS2.GWY + MidXY[0];
            double dFullX = Convert.ToDouble(FullX)*1000;
            double dFullY = Convert.ToDouble(FullY)*1000;
            string oldTH = Convert.ToString(dFullX) + "-" + Convert.ToString(dFullY);
            pDrawSheetMap.m_strSheetNo = oldTH;//旧图幅号

            //根据图幅号获得投影文件
            if (axPageLayoutControl1.ActiveView.FocusMap.SpatialReference is IProjectedCoordinateSystem)
            {
                pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            }
            else
            {
                ISpatialReference pSpa = GetSpatialByMapNO(pDrawSheetMap.m_strSheetNo);
                if (pSpa == null)
                {
                    MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                pDrawSheetMap.m_pPrjCoor = pSpa;
                axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pSpa;
            }

            pDrawSheetMap.DrawSheetMap();

            foreach (KeyValuePair<string, string> kvp in pgTextElements)
            {
                IGraphicsContainer pgGC = axPageLayoutControl1.GraphicsContainer;
                pgGC.Reset();
                IElement pgEle = pgGC.Next();
                while (pgEle != null)
                {
                    if (pgEle is ITextElement && (pgEle as ITextElement).Text == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;

                    }
                    pgEle = pgGC.Next();
                }
            }

            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            if (axPageLayoutControl1.ActiveView.Selection != null)
            {
                axPageLayoutControl1.ActiveView.Selection = null;

            }
            axPageLayoutControl1.ActiveView.Refresh();
            //cMD.Save(true, false);

            return true;

        }
        //从图幅号调用type0地形图2地籍图
        private void OutBZFFTBigScale2(int scale, string strMapNo,string inGwx,string inGwy,int type)
        {
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;

            string  gwX = "", gwY = "";
            if (type == 0)
            {
                FrmSheetMapSet2 frmSMS2 = new FrmSheetMapSet2(scale, "", strMapNo);
                frmSMS2.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frmSMS2.GWX = inGwx;
                frmSMS2.GWY = inGwy;
                frmSMS2.ShowDialog();

                if (frmSMS2.isChecked != true)
                {
                    this.Dispose();
                    return;
                }
                gwX=frmSMS2.GWX;
                gwY=frmSMS2.GWY;
                pgTextElements = frmSMS2.MapTextElements;
            }
            else if (type == 2)
            {
                FrmSheetMapSetDJ frmSMS2 = new FrmSheetMapSetDJ(scale, "", strMapNo);
                frmSMS2.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frmSMS2.GWX = inGwx;
                frmSMS2.GWY = inGwy;
                frmSMS2.ShowDialog();

                if (frmSMS2.isChecked != true)
                {
                    this.Dispose();
                    return;
                }
                pgTextElements = frmSMS2.MapTextElements;
                gwX = frmSMS2.GWX;
                gwY = frmSMS2.GWY;
            }
           
            axTOCControl1.SetBuddyControl(null);
            DeleteAllEleExpMap();
            

            axPageLayoutControl1.ActiveView.FocusMap.Name = pgTextElements["图名"];
            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1.Object as IPageLayoutControl;
            pDrawSheetMap.vScale = Convert.ToUInt32(pgTextElements["比例尺"].Substring(2));
            pDrawSheetMap.m_intPntCount = 3;
            string[] MidXY = pgTextElements["G50G005005"].Trim().Split('-');
            string FullX = gwX + MidXY[1];
            string FullY = gwY + MidXY[0];
            double dFullX = Convert.ToDouble(FullX) * 1000;
            double dFullY = Convert.ToDouble(FullY) * 1000;
            string oldTH = Convert.ToString(dFullX) + "-" + Convert.ToString(dFullY);
            pDrawSheetMap.m_strSheetNo = oldTH;//旧图幅号

            //根据图幅号获得投影文件
            if (axPageLayoutControl1.ActiveView.FocusMap.SpatialReference is IProjectedCoordinateSystem)
            {
                pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            }
            else
            {
                ISpatialReference pSpa=GetSpatialByMapNO(pDrawSheetMap.m_strSheetNo);
                if (pSpa == null)
                {
                    MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pDrawSheetMap.m_pPrjCoor = pSpa;
                axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pSpa;
            }
           
            pDrawSheetMap.DrawSheetMap();

            foreach (KeyValuePair<string, string> kvp in pgTextElements)
            {
                IGraphicsContainer pgGC = axPageLayoutControl1.GraphicsContainer;
                pgGC.Reset();
                IElement pgEle = pgGC.Next();
                while (pgEle != null)
                {
                    if (pgEle is ITextElement && (pgEle as ITextElement).Text == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;

                    }
                    if (pgEle is ITextElement && (pgEle as IElementProperties).Name == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;
                    }
                    pgEle = pgGC.Next();
                }
            }

            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            if (axPageLayoutControl1.ActiveView.Selection != null)
            {
                axPageLayoutControl1.ActiveView.Selection = null;

            }
            axPageLayoutControl1.ActiveView.Refresh();
            //cMD.Save(true, false);

        }
        //xiaoshiniudao
        private void OutBZFFTBigScale3(int scale, string strMapNo, string inGwx, string inGwy, int type)
        {
            
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;

            string gwX = "", gwY = "";
            if (type == 0)
            {
                FrmSheetMapSet2 frmSMS2 = new FrmSheetMapSet2(scale, "", strMapNo);
                frmSMS2.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frmSMS2.GWX = inGwx;
                frmSMS2.GWY = inGwy;
                frmSMS2.ShowDialog();

                if (frmSMS2.isChecked != true)
                {
                    this.Dispose();
                    return;
                }
                gwX = frmSMS2.GWX;
                gwY = frmSMS2.GWY;
                pgTextElements = frmSMS2.MapTextElements;
            }
            else if (type == 2)
            {
                FrmSheetMapSetDJ frmSMS2 = new FrmSheetMapSetDJ(scale, "", strMapNo);
                frmSMS2.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                frmSMS2.GWX = inGwx;
                frmSMS2.GWY = inGwy;
                frmSMS2.ShowDialog();

                if (frmSMS2.isChecked != true)
                {
                    this.Dispose();
                    return;
                }
                pgTextElements = frmSMS2.MapTextElements;
                gwX = frmSMS2.GWX;
                gwY = frmSMS2.GWY;
            }

            axTOCControl1.SetBuddyControl(null);
            DeleteAllEleExpMap();


            axPageLayoutControl1.ActiveView.FocusMap.Name = pgTextElements["图名"];
            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1.Object as IPageLayoutControl;
            pDrawSheetMap.vScale = Convert.ToUInt32(pgTextElements["比例尺"].Substring(2));
            pDrawSheetMap.m_intPntCount = 3;
            string[] MidXY = pgTextElements["G50G005005"].Trim().Split('-');
            string FullX = gwX + MidXY[1];
            string FullY = gwY + MidXY[0];
            double dFullX = Convert.ToDouble(FullX) * 1000;
            double dFullY = Convert.ToDouble(FullY) * 1000;
            string oldTH = Convert.ToString(dFullX) + "-" + Convert.ToString(dFullY);
            pDrawSheetMap.m_strSheetNo = oldTH;//旧图幅号

            //根据图幅号获得投影文件
            if (axPageLayoutControl1.ActiveView.FocusMap.SpatialReference is IProjectedCoordinateSystem)
            {
                pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            }
            else
            {
                ISpatialReference pSpa = GetSpatialByMapNO(pDrawSheetMap.m_strSheetNo);
                if (pSpa == null)
                {
                    MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pDrawSheetMap.m_pPrjCoor = pSpa;
                axPageLayoutControl1.ActiveView.FocusMap.SpatialReference = pSpa;
            }

            pDrawSheetMap.DrawSheetMap();

            foreach (KeyValuePair<string, string> kvp in pgTextElements)
            {
                IGraphicsContainer pgGC = axPageLayoutControl1.GraphicsContainer;
                pgGC.Reset();
                IElement pgEle = pgGC.Next();
                while (pgEle != null)
                {
                    if (pgEle is ITextElement && (pgEle as ITextElement).Text == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;

                    }
                    if (pgEle is ITextElement && (pgEle as IElementProperties).Name == kvp.Key)
                    {
                        (pgEle as ITextElement).Text = kvp.Value;
                    }
                    pgEle = pgGC.Next();
                }
            }

            axTOCControl1.SetBuddyControl(axPageLayoutControl1);
            if (axPageLayoutControl1.ActiveView.Selection != null)
            {
                axPageLayoutControl1.ActiveView.Selection = null;

            }
            axPageLayoutControl1.ActiveView.Refresh();
            //cMD.Save(true, false);

        }
        private void btn1000_Click(object sender, EventArgs e)
        {
            OutBZFFTBigScale(1000);
        }

        private void btn2000_Click(object sender, EventArgs e)
        {
            OutBZFFTBigScale(2000);
        }

        private void btnExportAV_Click(object sender, EventArgs e)
        {
            SysCommon.CProgress pgss =null;
            try
            {
                FrmExportActiveViewSet fmES = new FrmExportActiveViewSet(axPageLayoutControl1.ActiveView.FocusMap.Name);
                fmES.ShowDialog();
                if (!fmES.isOK)
                    return;
                string fileName = fmES.FileName;
                int resolution = fmES.Resolution;
                int ratio = fmES.Retio;
                string fExt = System.IO.Path.GetExtension(fileName);
                pgss = new SysCommon.CProgress("正在导出" + fileName);
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                Application.DoEvents();
                BaseCommand pCmd = new CommandExportActiveView(resolution, ratio, fileName, fExt.Substring(1).ToUpper(), false);

                pCmd.OnCreate(axPageLayoutControl1.Object);
                pCmd.OnClick();
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("输出图片完成！保存在： " + fileName);
                }
            }
            catch { }
            finally
            {
                if(pgss!=null)
                    pgss.Close();
                Application.DoEvents();
            }
        }

        private void btnDeleteEle_Click(object sender, EventArgs e)
        {
           IGraphicsContainerSelect pGCS=axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
           if (pGCS.ElementSelectionCount == 0)
                return;
            if (MessageBox.Show("您确定要删除选择的元素吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;
            //IEnumElement pEnumElement = pSelection as IEnumElement;
            //IElement pElement = pEnumElement.Next();
            //while (pElement != null)
            //{
            //    axPageLayoutControl1.GraphicsContainer.DeleteElement(pElement);
            //    //axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pElement, null);
            //    pElement = pEnumElement.Next();
            //}
            //axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            axTOCControl1.SetBuddyControl(null);
            ControlsEditingClearCommand Clear = new ControlsEditingClearCommandClass();
            Clear.OnCreate(axPageLayoutControl1.Object);
            Clear.OnClick();
            axTOCControl1.SetBuddyControl(axPageLayoutControl1);

        }

        private void axPageLayoutControl1_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {  //右键菜单 修改元素属性（人性化）
                if((axPageLayoutControl1.PageLayout as IGraphicsContainerSelect).ElementSelectionCount == 1)
                {
                    IElement pEle = (axPageLayoutControl1.PageLayout as IGraphicsContainerSelect).SelectedElements.Next();
                    IDisplay pDisplay = (axPageLayoutControl1.PageLayout as IActiveView).ScreenDisplay as IDisplay;
                    IEnvelope pEnv = new EnvelopeClass();
                    pEle.QueryBounds(pDisplay, pEnv);
                    if (e.pageX > pEnv.XMin && e.pageX < pEnv.XMax && e.pageY < pEnv.YMax && e.pageY > pEnv.YMin)
                    {
                        if (pEle is IGraphicsComposite)
                            itmConvertGra.Enabled = true;
                        else
                            itmConvertGra.Enabled = false;
                        if (pEle is IGroupElement)
                            itmUngroup.Enabled = true;
                        else
                            itmUngroup.Enabled = false;
                        PublicClass.POINTAPI pos = new PublicClass.POINTAPI();
                        PublicClass.GetCursorPos(ref pos);//user32 api
                        //cMSElePro.Items[1].Visible = false;//组合
                        //if (pEle is IGroupElement)
                        //    cMSElePro.Items[2].Visible = true;//取消组合
                        //else
                        //    cMSElePro.Items[2].Visible = false;
                        cMSElePro.Show(new System.Drawing.Point(pos.x, pos.y));
                        return;//返回
                    }
                   
                }//属性If
                //全选元素菜单
                else
                {
                    if ((axPageLayoutControl1.PageLayout as IGraphicsContainerSelect).ElementSelectionCount > 1)
                    {
                        itmGroup.Enabled = true;
                        itmClearSelect.Enabled = true;
                    }
                    else
                    {
                        itmGroup.Enabled = false;
                        itmClearSelect.Enabled = false;
 
                    }
                    PublicClass.POINTAPI pos = new PublicClass.POINTAPI();
                    PublicClass.GetCursorPos(ref pos);//user32 api
                    cMSPageLayout.Show(new System.Drawing.Point(pos.x, pos.y));
                }
            }//右键if
            
           
        }
        

        private void 属性ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ICommand ElementNew = new ElementCommandTool.ElementEditCommand() as ICommand;
            ElementNew.OnCreate(axPageLayoutControl1.Object);
            ElementNew.OnClick();
        }

        private void FrmPageLayout_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (axPageLayoutControl1.ActiveView.FocusMap.LayerCount != 0)
            {
               DialogResult vResult= MessageBox.Show("是否保存当前地图文档？", "提示", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Information);
                if (vResult==DialogResult.Yes)
                {
                    string cDir = createDir();
                    string mapName = axPageLayoutControl1.ActiveView.FocusMap.Name.Replace("\n", "-");
                    IMapDocument cMD = new MapDocumentClass();
                    cMD.New(cDir + @"\" + mapName + ".mxd");
                    cMD.Open(cDir + @"\" + mapName + ".mxd", "");
                    cMD.ReplaceContents(axPageLayoutControl1.PageLayout as IMxdContents);
                    cMD.Save(true, false);
                    cMD.Close();
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("保存地图文档到：" + cDir + @"\" + mapName + ".mxd");
                    }
                }
                else if (vResult == DialogResult.No)
                {
                }
                else
                {
                    e.Cancel = true;
                }


            }
        }

        private void axToolbarControl1_OnItemClick(object sender, IToolbarControlEvents_OnItemClickEvent e)
        {
            if (e.index == 0)
            {
                if (axPageLayoutControl1.ActiveView.FocusMap.LayerCount > 0 && MessageBox.Show("您点击了打开新文档按钮。确定保存当前地图文档吗？", "提示", MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Information) == DialogResult.OK)
                {
                    ICommand pCmd = new ControlsSaveAsDocCommandClass();
                    pCmd.OnCreate(axPageLayoutControl1.Object);
                    pCmd.OnClick();
                    MessageBox.Show("接下来请选择要打开的文档。", "提示", MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
                }
              
            }
            
        }
        //inner function end


        /// <summary>
        /// 根据图幅号获得投影文件
        /// </summary>
        /// <param name="strMapNO"></param>
        /// <returns></returns>
        private ISpatialReference GetSpatialByMapNO(string strMapNO)
        {
            //double dblX = 0;
            //double dblY = 0;

            // GeoDrawSheetMap.basPageLayout.GetCoordinateFromNewCode(strMapNO, ref dblX, ref dblY);

            //dblX = dblX / 3600;
            //dblY = dblY / 3600;
            string strtfh = "";
            try
            {
                strtfh = strMapNO.Substring(1, 2);
                int tfh = Convert.ToInt32(strtfh);
                tfh = tfh * 6 - 180 - 3;
                strtfh = "Xian 1980 GK CM " + tfh.ToString() + "E.prj";
            }
            catch
            {

            }
            string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (sInstall == "")
            {
                sInstall = ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }   //added by chulili 2012-11-13  end
            sInstall = sInstall + "\\Coordinate Systems\\Projected Coordinate Systems\\Gauss Kruger\\Xian 1980\\";
            string strPrjFileName = sInstall + strtfh;
            if (!System.IO.File.Exists(strPrjFileName)) return null;

            ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
            return pSpaFac.CreateESRISpatialReferenceFromPRJFile(strPrjFileName);

        }
        /////// <summary>
        /////// 根据经度获得投影文件
        /////// </summary>
        /////// <param name="strMapNO"></param>
        /////// <returns></returns>
        //private ISpatialReference GetSpatialByMapNO(int iX)
        //{
        //    //double dblX = 0;
        //    //double dblY = 0;

        //    // GeoDrawSheetMap.basPageLayout.GetCoordinateFromNewCode(strMapNO, ref dblX, ref dblY);

        //    //dblX = dblX / 3600;
        //    //dblY = dblY / 3600;
        //    string strtfh = "";
        //    try
        //    {
        //        strtfh = iX.ToString();
        //        int tfh = Convert.ToInt32(strtfh);
        //        tfh = tfh * 6 - 180 - 3;
        //        strtfh = "Xian 1980 GK CM " + tfh.ToString() + "E.prj";
        //    }
        //    catch
        //    {

        //    }
        //    string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
        //    sInstall = sInstall + "\\Coordinate Systems\\Projected Coordinate Systems\\Gauss Kruger\\Xian 1980\\";
        //    string strPrjFileName = sInstall + strtfh;
        //    if (!System.IO.File.Exists(strPrjFileName)) return null;

        //    ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
        //    return pSpaFac.CreateESRISpatialReferenceFromPRJFile(strPrjFileName);

        //}
        public  ISpatialReference GetSpatialByX(double dblX)
        {
            string strPrjFileName = "";
            if (dblX < 112.5)
            {
                strPrjFileName = "Xian 1980 3 Degree GK CM 111E.prj";
            }
            else if (dblX < 115.5)
            {
                strPrjFileName = "Xian 1980 3 Degree GK CM 114E.prj";
            }
            else if (dblX < 118.5)
            {
                strPrjFileName = "Xian 1980 3 Degree GK CM 117E.prj";
            }
            else
            {
                strPrjFileName = "WGS 1984 PDC Mercator.prj";
            }


            //
            strPrjFileName = Application.StartupPath + "\\..\\Prj\\" + strPrjFileName;
            if (!System.IO.File.Exists(strPrjFileName)) return null;

            ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
            return pSpaFac.CreateESRISpatialReferenceFromPRJFile(strPrjFileName);

        }
        private string ReadRegistry(string p)
        {
            /// <summary> 
            /// 从注册表中取得指定软件的路径 

            /// </summary> 

            /// <param name="sKey"> </param> 

            /// <returns> </returns> 

            //Open the subkey for reading 

            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(p, true);

            if (rk == null) return "";

            // Get the data from a specified item in the key. 

            return (string)rk.GetValue("InstallDir");
        }

        private void btn25W_Click(object sender, EventArgs e)
        {
            OutBZFFT(250000, "E");
        }

        private void 取消组合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
                if (pGCS.ElementSelectionCount > 1)
                    return;
                IElement pEle = pGCS.SelectedElements.Next();
                if (pEle == null)
                    return;
                if (pEle is IGroupElement)
                {
                    IGraphicsContainer pGraCon = axPageLayoutControl1.GraphicsContainer;
                    pGraCon.DeleteElement(pEle);
                    axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    IGroupElement pGroupEle = pEle as IGroupElement;
                    for (int i = 0; i < pGroupEle.ElementCount; i++)
                    {
                        pGraCon.AddElement(pGroupEle.get_Element(i), 0);

                    }
                    axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
            }
            catch { }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string progID = "esriControls.ControlsSelectTool";
            int index = axToolbarControl1.Find(progID);
            if (index != -1)
            {
                IToolbarItem toolItem = axToolbarControl1.GetItem(index);
                ICommand _cmd = toolItem.Command;
                ITool _tool = (ITool)_cmd;
                axPageLayoutControl1.CurrentTool = _tool;
            }
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            pGCS.SelectAllElements();//全选元素
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string progID = "esriControls.ControlsSelectTool";
            int index = axToolbarControl1.Find(progID);
            if (index != -1)
            {
                IToolbarItem toolItem = axToolbarControl1.GetItem(index);
                ICommand _cmd = toolItem.Command;
                ITool _tool = (ITool)_cmd;
                axPageLayoutControl1.CurrentTool = _tool;
            }
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            pGCS.UnselectAllElements();//清除全选
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }
        private ILayer layerproperty = null;//TOC当前选择的图层
        private void axTOCControl1_OnMouseUp(object sender, ITOCControlEvents_OnMouseUpEvent e)
        {
            IBasicMap map = null;
            System.Object other = null;
            System.Object index = null;
            esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
            if (e.button == 2)//右键
            {
                axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layerproperty, ref other, ref index);
                if (item == esriTOCControlItem.esriTOCControlItemMap) return;
                if (layerproperty.GetType().FullName == "ESRI.ArcGIS.Carto.GroupLayerClass")
                {
                    return;
                }
                if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    cMSTOC.Show(axTOCControl1, e.x, e.y);
                }
 
            }
        }

        private void ItemSymbolSet_Click(object sender, EventArgs e)
        {

            ILayer pCurLayer = layerproperty;
            if (pCurLayer == null) return;
            if (pCurLayer is ESRI.ArcGIS.Carto.IFeatureLayer)
            {
                IFeatureLayer pLayer = pCurLayer as IFeatureLayer;
                if (pLayer == null) return;

                try
                {
                    GeoSymbology.frmSymbology frm = new GeoSymbology.frmSymbology(pLayer);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        ESRI.ArcGIS.Carto.IGeoFeatureLayer pGeoLayer = pLayer as ESRI.ArcGIS.Carto.IGeoFeatureLayer;
                        pGeoLayer.Renderer = frm.FeatureRenderer();
                        axPageLayoutControl1.ActiveView.Refresh();
                        axTOCControl1.Update();
                    }
                }
                catch
                {

                    return;
                }
            }
            else if (pCurLayer is ESRI.ArcGIS.Carto.RasterLayerClass)
            {
                GeoSymbology.frmDEMSymbology frm = new GeoSymbology.frmDEMSymbology(pCurLayer);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //yjl20110826 add
                    IRasterLayer pRasterLayer = pCurLayer as IRasterLayer;
                    IRasterRenderer pRR = pRasterLayer.Renderer;
                    pRasterLayer.Renderer = frm.RasterRenderer();
                    if (pRR.Updated)
                        pRR.Update();
                    axPageLayoutControl1.ActiveView.Refresh();
                    axTOCControl1.Update();
                }
            }
        }

        private void ItemLyrPro_Click(object sender, EventArgs e)
        {
            GeoProperties.LayerPropertiesTool cmd = new GeoProperties.LayerPropertiesTool();
            cmd.OnCreate(axPageLayoutControl1.Object);
            cmd.CurLayer = layerproperty;
            cmd.OnClick();
        }

        //处理图例，删除多余图例，默认组图层为一个专题
        private void delUnnecLegends(IGraphicsContainer inGra, System.Double posX, System.Double posY, System.Double legW)
        {
            ILegend pLegend = null;
            IGraphicsContainer pgGC = inGra;
            IElement pLegendEle = null;
            IMapSurroundFrame pMSF = null;
            pgGC.Reset();
            IElement pgEle = pgGC.Next();
            while (pgEle != null)
            {
                if (pgEle is IMapSurroundFrame)
                {
                    pMSF = pgEle as IMapSurroundFrame;
                    if (pMSF.MapSurround is ILegend)
                    {
                        pLegend = pMSF.MapSurround as ILegend;
                        pLegendEle = pgEle;
                        break;
                    }
                }
                pgEle = pgGC.Next();
            }
            if (pLegend == null)
                return;
            IMap pMap = pLegend.Map;
            if (pMap.LayerCount < 1)
                return;
            List<ILayer> lyrs = new List<ILayer>();
            for (int i = 1; i < pMap.LayerCount; i++)
            {
                ILayer pLyr = pMap.get_Layer(i);
                addLayer(pLyr, ref lyrs);
            }
            
                for (int i = 0; i < pLegend.ItemCount; i++)
                {
                    ILegendItem pLegendItem = pLegend.get_Item(i);
                    ILayer tmp = pLegendItem.Layer;
                    IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                    
                    if (pFeatureLayer != null)
                    {
                        IFeatureClass pFC = pFeatureLayer.FeatureClass;
                        if (pFC != null)
                        {
                            IDataset pDataset = pFC as IDataset;
                           
                            if (pDataset.Name.Contains("XZQH") || pDataset.Name.Contains("PDT"))
                            {
                                pLegend.RemoveItem(i);//去除行政区和坡度图
                                i--;
                                break;

                            }
                        }
                    }
                    foreach (ILayer lyr in lyrs)
                    {
                        if (lyr.Equals(tmp))
                        {
                            pLegend.RemoveItem(i);
                            i--;
                            break;
                        }
                    }//foreach
                }

            
            //Get aspect ratio
            if (pLegend.ItemCount == 0)
            {
                inGra.DeleteElement(pLegendEle);
                return;
            }
            ESRI.ArcGIS.Carto.IQuerySize querySize = pMSF.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX, (posY - legW / aspectRatio), (posX + legW), posY);
            pLegendEle.Geometry = envelope;
            pLegendEle.Activate((inGra as IActiveView).ScreenDisplay);

        }
        //得到组图层下的图层集合，递归
        private void addLayer(ILayer inGPLayer, ref List<ILayer> lyrLst)
        {
            if (inGPLayer is IGroupLayer)
            {
                ICompositeLayer pCL = inGPLayer as ICompositeLayer;
                for (int i = 0; i < pCL.Count; i++)
                {
                    ILayer tmp = pCL.get_Layer(i);
                    if (tmp is IGroupLayer)
                        addLayer(tmp, ref lyrLst);
                    else
                        lyrLst.Add(tmp);
                }
            }
            else
                lyrLst.Add(inGPLayer);
        }

        private void 转为图形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
                if (pGCS.ElementSelectionCount > 1)
                    return;
                IElement pEle = pGCS.SelectedElements.Next();
                if (pEle == null)
                    return;
                if (pEle is IGraphicsComposite)
                {
                    axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    IDisplay pDisplay = axPageLayoutControl1.ActiveView.ScreenDisplay as IDisplay;
                    IGraphicsContainer pGraCon = axPageLayoutControl1.GraphicsContainer;
                    IGraphicsComposite pGC = pEle as IGraphicsComposite;
                    IEnumElement pEnumEles = pGC.get_Graphics(pDisplay, pEle);
                    IElement tmpEle = pEnumEles.Next();
                    IGroupElement pGroupEle = new GroupElementClass();
                    while (tmpEle != null)
                    {
                        pGroupEle.AddElement(tmpEle);
                        tmpEle = pEnumEles.Next();
                    }
                    pGraCon.DeleteElement(pEle);
                    pGraCon.AddElement(pGroupEle as IElement, 0);
                    axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                }
            }
            catch { }
        }

        private void 组合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
                if (pGCS.ElementSelectionCount < 2)
                    return;
                IElement pEle = pGCS.SelectedElements.Next();
                IGroupElement pGroupEle = new GroupElementClass();
                IGraphicsContainer pGraCon = axPageLayoutControl1.GraphicsContainer;
                axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
                while (pEle != null)
                {
                    pGraCon.DeleteElement(pEle);
                    pGroupEle.AddElement(pEle);
                    pEle = pGCS.SelectedElements.Next();

                }
                pGraCon.AddElement(pGroupEle as IElement, 0);
                axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            catch { }
        }

        private void axToolbarControl1_OnMouseMove(object sender, IToolbarControlEvents_OnMouseMoveEvent e)
        {
            //toolTip1.Hide(sender as Control);
            int itm=axToolbarControl1.HitTest(e.x,e.y);
            string cmdName=axToolbarControl1.GetItem(itm).Command.Name;
            switch (cmdName)
            {
                case "ControlToolsGeneric_Undo":
                    toolTip1.Show("撤销", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsGeneric_FileOpenCommand":
                    toolTip1.Show("打开地图文档", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsGeneric_SaveAsDocCommand":
                    toolTip1.Show("保存地图文档", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsGeneric_Redo":
                    toolTip1.Show("重做", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_PageZoomIn":
                    toolTip1.Show("放大", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_PageZoomOut":
                    toolTip1.Show("缩小", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_PagePan":
                    toolTip1.Show("浏览", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_PageZoomInFixed":
                    toolTip1.Show("固定放大", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_PageZoomOutFixed":
                    toolTip1.Show("固定缩小", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_ZoomWholePage":
                    toolTip1.Show("全图", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_Zoom100Percent":
                    toolTip1.Show("实际大小", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_ZoomPageToLastExtentBack":
                    toolTip1.Show("上个视图", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsPageLayout_ZoomPageToLastExtentForward":
                    toolTip1.Show("下个视图", sender as Control,e.x,e.y+20);
                    break;
                case "ControlToolsGraphicElement_SelectTool":
                    toolTip1.Show("选择元素", sender as Control,e.x,e.y+20);
                    break;

            }
        }

        private void axToolbarControl1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(sender as Control);
        }

        private void btnManagerPageLayerout_Click(object sender, EventArgs e)
        {
            FrmManagerPageLayout frmMana = new FrmManagerPageLayout();
            if (frmMana.ShowDialog() != DialogResult.OK) return;
            string MXDPaht=frmMana .m_MXDFileName ;
            if (!File.Exists(MXDPaht)) return;
            IMapControlDefault pMapControl = new MapControlClass();
            pMapControl.Map = axPageLayoutControl1.ActiveView.FocusMap;
            Exception ex = null;
            try
            {
                ModGisPub.RenderLayerByMxd(MXDPaht, pMapControl, out ex);
                axPageLayoutControl1.ActiveView.Refresh();
                axTOCControl1.Update();
                File.Delete(MXDPaht);
            }
            catch
            {
 
            }
        }
    }
}