using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using System.Drawing;
using DevComponents.AdvTree;
using stdole;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：地图制图主类
    /// </summary>
    public class GeoPageLayout
    {

        private IMap cMap = null;

        //private XmlDocument cXmlDoc = null;
        //IWorkspace srcWs = null;
        string cDir = "";//当前实例图形成果目录
        public int typePageLayout = -1;//制图类型0标准1业务2动态范围3接合图表树4行政区批量分幅5辖区图6批量辖区图
        public List<string> listview = new List<string>();
        public bool isNeed = false;
        public string ztlx = "";
        public string stcxzq = "";
        public int stcSclae = 10000;
        private IGeometry pGeometry = null;//制图形状
        private string StrMapNo = "";
        private IPoint pPoint = null;//为了大比例尺的坐标
        private int type_ZT = 0;//20110914 yjl add 专题类型
        private string pXZQMC = null;
        private string pZTMC = null;//专题名称
        private Node pXZQ = null;//批量辖区图的行政区节点
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
        //空构造函数
        public GeoPageLayout()
        { }
        //标准制图构造函数,未知图幅号
        public GeoPageLayout(IMap pMap)
        {
            cMap = pMap;

        }
        //标准制图构造函数,已知图幅号,支持带空格的图幅号和不带的
        public GeoPageLayout(IMap pMap, string strMapNo, int iScale, IPoint inPoint, int typeZT)
        {
            cMap = pMap;
            StrMapNo = strMapNo;
            stcSclae = iScale;
            pPoint = inPoint;
            type_ZT = typeZT;//专题类型
        }
        //动态范围制图构造函数
        public GeoPageLayout(IMap pMap, IGeometry fGeometry)
        {
            cMap = pMap;
            pGeometry = fGeometry;
        }

        //行政区批量输出构造函数
        public GeoPageLayout(IMap pMap, int inScale, string inZTMC, Node inXZQ)
        {
            cMap = pMap;
            //pGeometry = xzqGeometry;
            stcSclae = inScale;
            pXZQ = inXZQ;
            pZTMC = inZTMC;
        }
        //行政区批量输出分幅构造函数
        public GeoPageLayout(IMap pMap, IGeometry xzqGeometry, int inScale, string xzqName, int typeZT)
        {
            cMap = pMap;
            pGeometry = xzqGeometry;
            stcSclae = inScale;
            pXZQMC = xzqName;
            type_ZT = typeZT;
        }


        //制图主函数
        public void MapOut()
        {
            if (typePageLayout == 0)//标准分幅 pagelayout
            {
                SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                Application.DoEvents();
                FrmPageLayout pPl = new FrmPageLayout(cMap);//制图主窗体
                pPl.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pPl.typeZHT = 0;
                pPl.Show();
                pgss.Close();
                Application.DoEvents();
            }
            //if (typePageLayout == 1)//only for open a saved mxd with a data mdb
            //{
            //    FrmPageLayout pPl = new FrmPageLayout(cDir + "\\" + ztlx + ".mxd");
            //    pPl.ShowDialog();

            //}
            if (typePageLayout == 2)//动态范围 pagelayout
            {
                SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                Application.DoEvents();
                pgss.Close();
                Application.DoEvents();
            }
            if (typePageLayout == 3)//接合图表树和已知图幅号的标准分幅
            {
                try
                {
                    Application.DoEvents();
                    SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
                    pgss.EnableCancel = false;
                    pgss.ShowDescription = false;
                    pgss.FakeProgress = true;
                    pgss.TopMost = true;
                    pgss.ShowProgress();
                    Application.DoEvents();

                    FrmPageLayout pPl = new FrmPageLayout(cMap, StrMapNo, stcSclae, pgss, pPoint, type_ZT);
                    pPl.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                    pPl.typeZHT = 3;
                    pPl.Show();

                    Application.DoEvents();
                }
                catch
                {
                }
            }
            if (typePageLayout == 4)//批量分幅图
            {

                BatPageLayoutTDLYFFT(cMap, pXZQMC, stcSclae, type_ZT);

            }
            if (typePageLayout == 5)//单个辖区图
            {

                pageLayoutTDLYXQT(cMap, pGeometry, stcSclae);
            }
            if (typePageLayout == 6)//批量辖区图
            {

                batPageLayoutTDLYXQT(cMap, pZTMC, stcSclae, pXZQ);
            }

        }//fn_MapOut

        #region 范围制图
        private void pageLayoutExtent(IMap pMap, IGeometry pExtent)
        {
            try
            {
                IPageLayout pPageLayout = new PageLayoutClass();
                IActiveView pActiveView = pPageLayout as IActiveView;
                IGraphicsContainer pGra = pPageLayout as IGraphicsContainer;
                IMapLayers pMapLayers = pActiveView.FocusMap as IMapLayers;
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pMapLayers.InsertLayer(pMap.get_Layer(i), false, pMapLayers.LayerCount);
                }
                IActiveView mapAV = pMapLayers as IActiveView;
                mapAV.Extent = pExtent.Envelope;
                IMap pgMap = pMapLayers as IMap;
                //pgMap.ClipGeometry = pExtent;
                //pgMap.ClipBorder = createBorder(pExtent, pActiveView);

                //arcgis10新增了接口来处理ClipGeometry
                IMapClipOptions pMapClip = pMapLayers as IMapClipOptions;
                pMapClip.ClipType = esriMapClipType.esriMapClipShape;
                pMapClip.ClipGeometry = pExtent;
                pMapClip.ClipBorder = createBorder(pExtent, pActiveView);

                IMapFrame pMapFrame = (IMapFrame)pGra.FindFrame(pActiveView.FocusMap);
                IElement pMapEle = pMapFrame as IElement;
                (pMapEle as IElementProperties).Name = "地图";
                IPage pPage = pPageLayout.Page;
                pPage.IsPrintableAreaVisible = false;
                pMapEle.Geometry = pPage.PrintableBounds;
                FrmPageLayout fmPL = new FrmPageLayout(pPageLayout);
                fmPL.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                fmPL.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion


        #region 批量分幅森林资源现状图
        //由比例尺获得经纬度幅差
        private void getDeltaJW(int inScale, ref double diffX, ref double diffY)
        {
            switch (inScale)
            {
                case 500000:
                    diffX = 3 * 3600;
                    diffY = 2 * 3600;
                    break;
                case 250000:
                    diffX = 3 * 3600 / 2;
                    diffY = 1 * 3600;
                    break;
                case 100000:
                    diffX = 1 * 3600 / 2;
                    diffY = 1 * 3600 / 3;
                    break;
                case 50000:
                    diffX = 1 * 3600 / 4;
                    diffY = 1 * 3600 / 6;
                    break;
                case 25000:
                    diffX = 1 * 3600 / 8;
                    diffY = 1 * 3600 / 12;
                    break;
                case 10000:
                    diffX = 1 * 3600 / 16;
                    diffY = 1 * 3600 / 24;
                    break;
                case 5000:
                    diffX = 1 * 3600 / 32;
                    diffY = 1 * 3600 / 48;
                    break;
            }

        }
        //由极点数组生成图幅号数组
        private List<string> getTFHLst(WKSPoint[] inJD, int inScale, IGeometry inXZQ)
        {

            ISpatialReference pSR = inXZQ.SpatialReference;
            IGeographicCoordinateSystem pGCS = new GeographicCoordinateSystemClass();
            pGCS = (pSR as IProjectedCoordinateSystem).GeographicCoordinateSystem;
            Dictionary<string, IGeometry> res = new Dictionary<string, IGeometry>();
            double difX = 0, difY = 0;
            getDeltaJW(inScale, ref difX, ref difY);
            double minX = Math.Floor(inJD[0].X * 3600 / difX) * difX;//图幅号的极值
            double minY = Math.Floor(inJD[0].Y * 3600 / difY) * difY;
            double maxX = Math.Floor(inJD[1].X * 3600 / difX) * difX;
            double maxY = Math.Floor(inJD[1].Y * 3600 / difY) * difY;
            for (double i = minX; i <= maxX; i += difX)
            {
                for (double j = minY; j <= maxY; j += difY)
                {
                    IEnvelope pEnv = new EnvelopeClass();
                    pEnv.PutCoords(i / 3600, j / 3600, (i + difX) / 3600, (j + difY) / 3600);
                    pEnv.SpatialReference = pGCS;
                    string mapNo = "";
                    long lScale = inScale;
                    GeoDrawSheetMap.basPageLayout.GetNewCodeFromCoordinate(ref mapNo, (long)i, (long)(j + 10), lScale);
                    res.Add(mapNo, pEnv as IGeometry);

                }
            }//for i
            //和行政区做求交运算,先将行政区反算到大地坐标

            inXZQ.Project(pGCS);
            inXZQ.SpatialReference = pGCS;
            IRelationalOperator pRO = inXZQ as IRelationalOperator;
            List<string> tfh = new List<string>();
            (cMap as IGraphicsContainer).DeleteAllElements();
            foreach (KeyValuePair<string, IGeometry> kvp in res)
            {
                if (pRO.Overlaps(kvp.Value))
                {
                    //drawPolygonElement(kvp.Value, cMap as IGraphicsContainer);
                    tfh.Add(kvp.Key);
                }
                else if (pRO.Contains(kvp.Value))
                {
                    //drawPolygonElement(kvp.Value, cMap as IGraphicsContainer);
                    tfh.Add(kvp.Key);

                }
            }
            return tfh;
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
        //行政区批量输出森林资源现状分幅图
        private void BatPageLayoutTDLYFFT(IMap pMap, string inXZQMC, int iScale, int inTypeZT)
        {
            FormProgress fmPgs = null;
            SysCommon.CProgress pgss = null;
            try
            {
                cDir = createDir(inXZQMC + pMap.Name + "批量分幅图");
                if (cDir == "")
                    return;
                IGeometry xzq = pGeometry;
                WKSPoint[] wksP = getPts(xzq);
                if (wksP == null)
                    return;
                pgss = new SysCommon.CProgress("正在计算" + inXZQMC + "范围的图幅号...");
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                Application.DoEvents();
                List<string> TFHs = getTFHLst(wksP, stcSclae, xzq);
                pgss.Close();
                Application.DoEvents();
                if (TFHs.Count == 0)
                    return;
                List<string> strMapNos = TFHs;
                Dictionary<string, string> pgTextElements = null;
                bool hasLegend = false;
                if (inTypeZT == 0)
                {
                    FrmSheetMapSet frmSMS = new FrmSheetMapSet(iScale, "", strMapNos[0]);
                    frmSMS.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                    if (frmSMS.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    pgTextElements = frmSMS.MapTextElements;
                    hasLegend = frmSMS.HasLegend;
                }
                else if (inTypeZT == 1)
                {
                    FrmSheetMapSet_ZT frmSMS = new FrmSheetMapSet_ZT(iScale, "", strMapNos[0]);
                    frmSMS.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                    if (frmSMS.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    pgTextElements = frmSMS.MapTextElements;
                    hasLegend = frmSMS.HasLegend;

                }
                fmPgs = new FormProgress();
                ProgressBar pgsBar = fmPgs.progressBar1;
                pgsBar.Minimum = 1;
                pgsBar.Maximum = strMapNos.Count;
                pgsBar.Step = 1;
                fmPgs.TopLevel = true;
                fmPgs.Text = "正在批量输出" + inXZQMC + "森林资源现状分幅图";
                fmPgs.Show();

                foreach (string strMapNo in strMapNos)
                {

                    fmPgs.lblOut.Text = "总共" + strMapNos.Count + "幅，正在输出第" + pgsBar.Value + "幅，图幅号为：" + strMapNo;
                    pgTextElements["G50G005005"] = strMapNo;
                    IObjectCopy pOC = new ObjectCopyClass();
                    IMap newMap = pOC.Copy(pMap) as IMap;
                    procLyrScale(newMap);//yjl取消比例尺显示限制
                    IMaps newMaps = new Maps();
                    newMaps.Add(newMap);
                    IPageLayoutControl pPLC = new PageLayoutControlClass();
                    pPLC.PageLayout.ReplaceMaps(newMaps);
                    //axPageLayoutControl1.PageLayout = cMD.PageLayout;
                    if (iScale > 2000)//小比例尺
                    {
                        GeoPageLayoutFn.QuietPageLayoutTDLYFFT(pPLC, pgTextElements, inTypeZT);
                    }
                    IMapFrame pMapFrame = (IMapFrame)pPLC.GraphicsContainer.FindFrame(pPLC.ActiveView.FocusMap);
                    IElement pMapEle = pMapFrame as IElement;
                    (pMapEle as IElementProperties).Name = "地图";
                    double x = pMapEle.Geometry.Envelope.XMax;//地图框架的右上点坐标
                    double y = pMapEle.Geometry.Envelope.YMax;
                    double xmin = pMapEle.Geometry.Envelope.XMin;
                    //更新范围图例
                    GeoPageLayoutFn.updateMapSymbol(pPLC.ActiveView.FocusMap, pPLC.ActiveView.FocusMap.ClipGeometry);
                    if (type_ZT == 1 && hasLegend)
                    {
                        AddLegend(pPLC.PageLayout, pPLC.ActiveView.FocusMap, x + 1, y, 4, 1);

                    }
                    IMapDocument cMD = new MapDocumentClass();
                    string savePath = cDir + @"\" + strMapNo + ".mxd";
                    cMD.New(savePath);
                    cMD.Open(savePath, "");
                    cMD.ReplaceContents(pPLC.PageLayout as IMxdContents);
                    cMD.Save(true, false);
                    cMD.Close();
                    if (fmPgs.IsDisposed)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("用户取消批量森林资源现状分幅图输出！已输出的图保存在：" + cDir);
                        }
                        break;
                    }

                    pgsBar.PerformStep();
                    Application.DoEvents();

                }//foreach
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("批量森林资源现状分幅图输出完成！保存路径：" + cDir);
                }
                //MessageBox.Show("批量森林资源现状分幅图输出完成！保存路径：" + cDir, "提示", MessageBoxButtons.OK,
                //         MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (pgss != null)
                    pgss.Close();
                if (fmPgs != null && !fmPgs.IsDisposed)
                    fmPgs.Dispose();


            }
        }
        #endregion

        #region 现状辖区图单幅和批量
        //输出森林资源现状辖区图-单幅
        private void pageLayoutTDLYXQT(IMap inMap, IGeometry inGeometry, int inScale)
        {
            if (inMap == null)
                return;
            FrmTDLYMapSet_XQT tdlyXQT = new FrmTDLYMapSet_XQT(inScale, "");
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            tdlyXQT.XZQname = pXZQMC;//行政区名称
            if (tdlyXQT.ShowDialog() != DialogResult.OK)
                return;
            bool hasLegend = tdlyXQT.HasLegend;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\TDLYXQTTemplate.mxd");
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

               //进度条
            SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
            pgss.EnableCancel = false;
            pgss.ShowDescription = false;
            pgss.FakeProgress = true;
            pgss.TopMost = true;
            pgss.ShowProgress();
            Application.DoEvents();
            try
            {
                IObjectCopy pOC = new ObjectCopyClass();
                IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
                //IPageLayoutControl pPLC = new PageLayoutControlClass();
                //pPLC.PageLayout = workPageLayout;
                //扩大行政区范围2厘米
                IEnvelope workEnv = new EnvelopeClass();
                IEnvelope pGeoEnv = inGeometry.Envelope;
                double xmin = pGeoEnv.XMin - 0.02 * inScale,
                    ymin = pGeoEnv.YMin - 0.02 * inScale,
                    xmax = pGeoEnv.XMax + 0.02 * inScale,
                    ymax = pGeoEnv.YMax + 0.02 * inScale;
                workEnv.PutCoords(xmin, ymin, xmax, ymax);
                workEnv.SpatialReference = inGeometry.SpatialReference;
                //根据范围和比例尺及模板边距，重设纸张大小
                IPage workPage = workPageLayout.Page;
                workPage.StretchGraphicsWithPage = false;
                double workPW = workEnv.Width * 100 / inScale + mf2page[0] + mf2page[2],
                    workPH = workEnv.Height * 100 / inScale + mf2page[1] + mf2page[3];
                workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
                IActiveView workActiveView = workPageLayout as IActiveView;
                pOC = new ObjectCopyClass();
                IMap newMap = pOC.Copy(inMap) as IMap;
                procLyrScale(newMap);//yjl取消比例尺显示限制
                IMap workMap = workActiveView.FocusMap;
                workMap.Name = pTextEles["主题"];//
                workMap.SpatialReference = inMap.SpatialReference;
                IMapLayers pMapLayers = workMap as IMapLayers;
                for (int i = 0; i < newMap.LayerCount; i++)
                {
                    pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
                }
                IActiveView workMapAV = workMap as IActiveView;

                workMapAV.Extent = workEnv;
                //workMap.ClipGeometry = pExtent;
                workMap.MapScale = inScale;
                
               
                IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
                IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
                pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentScale;
                pMapFrame.MapBounds = workEnv;
                IElement pMapEle = pMapFrame as IElement;
                IEnvelope pEnv = new EnvelopeClass();
                pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
                pMapEle.Geometry = pEnv;

                //晕线部分
                IObjectCopy cpGeo = new ObjectCopyClass();
                IGeometry xzqPg = cpGeo.Copy(inGeometry) as IGeometry;

                IPointCollection pPC = xzqPg as IPointCollection;
                //xzqBound.SpatialReference = null;

                ITransform2D pT2D = xzqPg as ITransform2D;
                pT2D.Scale(pPC.get_Point(0), (double)100 / inScale, (double)100 / inScale);
                //纸面坐标减去地图坐标得到平移量
                double movX = (pEnv.XMin + (pPC.get_Point(0).X - workMapAV.Extent.XMin) * 100 / inScale) - pPC.get_Point(0).X,
                   movY = (pEnv.YMin + (pPC.get_Point(0).Y - workMapAV.Extent.YMin) * 100 / inScale) - pPC.get_Point(0).Y;
                pT2D.Move(movX, movY);
                xzqPg.SpatialReference = pMapEle.Geometry.SpatialReference;
                ////遮盖相邻行政区元素
                //ITopologicalOperator pTO1 = pMapEle.Geometry as ITopologicalOperator;
                //IGeometry pSurround = pTO1.Difference(xzqPg);
                //GeoPageLayoutFn.drawPolygonElement(pSurround, workGra, getRGB(255, 255, 255), false, getRGB(0, 0, 0), 1.5);
                //晕线
                ITopologicalOperator pTO = xzqPg as ITopologicalOperator;
                IPolyline xzqBound = pTO.Boundary as IPolyline;
                GeoPageLayoutFn.drawPolylineElement(xzqBound, workPageLayout as IGraphicsContainer, getRGB(0, 0, 255), 2);
          
                //更新范围图例
                GeoPageLayoutFn.updateMapSymbol(workMap, workActiveView.Extent);
                
                //添加图例
                if (hasLegend)
                {
                    AddLegend(workPageLayout, workMap, workPW - mf2page[2] + 1, workPH - mf2page[3], 4, 1);
                    IMapSurroundFrame pLegend = getMapSurroundFrame(workGra, "图例");
                    IObjectCopy pLC = new ObjectCopyClass();
                    ILegendFormat pLF = pLC.Copy((pLegend.MapSurround as ILegend).Format) as ILegendFormat;
                    pLC = new ObjectCopyClass();
                    ILegendClassFormat pLCF = pLC.Copy((pLegend.MapSurround as ILegend).get_Item(0).LegendClassFormat) as ILegendClassFormat;
                    delUnnecLegends(pLegend);//去除多余图例
                    (pLegend.MapSurround as ILegend).Format = pLF;
                    resizeLegend(workActiveView, pLCF, pLegend, workPW - mf2page[2] + 1, workPH - mf2page[3], 4);
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
                FrmPageLayout fmPL = new FrmPageLayout(workPageLayout);
                fmPL.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                fmPL.Show();
            }
            catch { }
            finally 
            {
                pgss.Close();
            }


        }
        //输出森林资源现状辖区图-批量
        private void batPageLayoutTDLYXQT(IMap inMap, string inZTMC, int inScale, Node inXZQnode)
        {
            FormProgress fmPgs = null;
            try
            {
                cDir = createDir(inXZQnode.Text + inZTMC + "批量辖区图");
                if (cDir == "")
                    return;
                if (inMap == null)
                    return;
                if (!inXZQnode.HasChildNodes)
                    return;
                FrmTDLYMapSet_XQT tdlyXQT = new FrmTDLYMapSet_XQT(inScale, "");
                tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12是否写日志
                if (tdlyXQT.ShowDialog() != DialogResult.OK)
                    return;
                bool hasLegend = tdlyXQT.HasLegend;
                Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
                string title = pTextEles["主题"];
                IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\TDLYXQTTemplate.mxd");
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

                fmPgs = new FormProgress();
                ProgressBar pgsBar = fmPgs.progressBar1;
                pgsBar.Minimum = 1;
                pgsBar.Maximum = inXZQnode.Nodes.Count;
                pgsBar.Step = 1;
                fmPgs.TopLevel = true;
                fmPgs.Text = "正在批量输出" + inXZQnode.Text + "森林资源现状辖区图";
                fmPgs.Show();
                foreach (Node advNode in inXZQnode.Nodes)
                {
                    fmPgs.lblOut.Text = "总共" + pgsBar.Maximum + "个行政区，正在输出第" + pgsBar.Value + "个，行政区名称为：" + advNode.Text;
                    IGeometry inGeometry = ModGetData.getExtentByXZQ(advNode);
                    IObjectCopy pOC = new ObjectCopyClass();
                    IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
                    //IPageLayoutControl pPLC = new PageLayoutControlClass();
                    //pPLC.PageLayout = workPageLayout;
                    //扩大行政区范围2厘米
                    IEnvelope workEnv = new EnvelopeClass();
                    IEnvelope pGeoEnv = inGeometry.Envelope;
                    double xmin = pGeoEnv.XMin - 0.02 * inScale,
                        ymin = pGeoEnv.YMin - 0.02 * inScale,
                        xmax = pGeoEnv.XMax + 0.02 * inScale,
                        ymax = pGeoEnv.YMax + 0.02 * inScale;
                    workEnv.PutCoords(xmin, ymin, xmax, ymax);
                    workEnv.SpatialReference = inGeometry.SpatialReference;
                    //根据范围和比例尺及模板边距，重设纸张大小
                    IPage workPage = workPageLayout.Page;
                    workPage.StretchGraphicsWithPage = false;
                    double workPW = workEnv.Width * 100 / inScale + mf2page[0] + mf2page[2],
                        workPH = workEnv.Height * 100 / inScale + mf2page[1] + mf2page[3];
                    workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
                    IActiveView workActiveView = workPageLayout as IActiveView;
                    IMap newMap = null;
                    bool isSpecial = ModGetData.IsMapSpecial();

                    if (isSpecial)//如果找特定专题
                    {
                        newMap = new MapClass();
                        ModGetData.AddMapOfByXZQ(newMap, "TDLY", inZTMC, inMap, advNode.Text);
                        if (newMap.LayerCount == 0)//若是空地图则前进
                        {
                            if (this.WriteLog)
                            {
                                Plugin.LogTable.Writelog(advNode.Text + "没有找到图层！");
                            }
                            continue;
                        }
                        ModuleMap.LayersComposeEx(newMap);//图层排序
                    }
                    else
                    {
                        IObjectCopy pOC1 = new ObjectCopyClass();
                        newMap = pOC1.Copy(inMap) as IMap;//复制地图
                    }
                    if (newMap.LayerCount == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                        return;

                    }

                    string xzqdmFD = "";
                    //构造晕线和渲染图层
                    IFeatureClass xzqFC = ModGetData.getFCByXZQ(advNode, ref xzqdmFD);
                    if (xzqFC != null && xzqdmFD != null)
                    {
                        ILayer hachureLyr = GeoPageLayoutFn.createHachureLyr(xzqFC, xzqdmFD, advNode.Name);
                        if (hachureLyr != null)
                        {
                            IMapLayers pMapLayers1 = newMap as IMapLayers;
                            IGroupLayer pGroupLayer = newMap.get_Layer(0) as IGroupLayer;
                            if (pGroupLayer != null)
                            {
                                pMapLayers1.InsertLayerInGroup(pGroupLayer, hachureLyr, false, 0);
                            }

                        }
                    }
                    pTextEles["主题"] = advNode.Text + title;
                    //fmPgs.lblOut.Text = "输出行政区：" + advNode.Text;
                    procLyrScale(newMap);//yjl取消比例尺显示限制
                    IMap workMap = workActiveView.FocusMap;
                    workMap.Name = "森林资源现状辖区图";
                    workMap.SpatialReference = inMap.SpatialReference;
                    IMapLayers pMapLayers = workMap as IMapLayers;
                    for (int i = 0; i < newMap.LayerCount; i++)
                    {
                        pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
                    }
                    IActiveView workMapAV = workMap as IActiveView;

                    workMapAV.Extent = workEnv;
                    //workMap.ClipGeometry = pExtent;
                    workMap.MapScale = inScale;


                    IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
                    IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
                    pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
                    pMapFrame.MapBounds = workEnv;
                    IElement pMapEle = pMapFrame as IElement;
                    IEnvelope pEnv = new EnvelopeClass();
                    pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
                    pMapEle.Geometry = pEnv;

                    //晕线部分
                    IObjectCopy cpGeo = new ObjectCopyClass();
                    IGeometry xzqPg = cpGeo.Copy(inGeometry) as IGeometry;

                    IPointCollection pPC = xzqPg as IPointCollection;
                    //xzqBound.SpatialReference = null;

                    ITransform2D pT2D = xzqPg as ITransform2D;
                    pT2D.Scale(pPC.get_Point(0), (double)100 / inScale, (double)100 / inScale);
                    //纸面坐标减去地图坐标得到平移量
                    double movX = (pEnv.XMin + (pPC.get_Point(0).X - workMapAV.Extent.XMin) * 100 / inScale) - pPC.get_Point(0).X,
                       movY = (pEnv.YMin + (pPC.get_Point(0).Y - workMapAV.Extent.YMin) * 100 / inScale) - pPC.get_Point(0).Y;
                    pT2D.Move(movX, movY);
                    xzqPg.SpatialReference = pMapEle.Geometry.SpatialReference;
                    ////遮盖相邻行政区元素
                    //ITopologicalOperator pTO1 = pMapEle.Geometry as ITopologicalOperator;
                    //IGeometry pSurround = pTO1.Difference(xzqPg);
                    //GeoPageLayoutFn.drawPolygonElement(pSurround, workGra, getRGB(255, 255, 255), false, getRGB(0, 0, 0), 1.5);
                    //晕线
                    ITopologicalOperator pTO = xzqPg as ITopologicalOperator;
                    IPolyline xzqBound = pTO.Boundary as IPolyline;
                    GeoPageLayoutFn.drawPolylineElement(xzqBound, workPageLayout as IGraphicsContainer, getRGB(0, 0, 255), 2);

                    //更新范围图例
                    GeoPageLayoutFn.updateMapSymbol(workMap, workActiveView.Extent);
                    
                    //添加图例
                    if (hasLegend)
                    {
                        AddLegend(workPageLayout, workMap, workPW - mf2page[2] + 1, workPH - mf2page[3], 4, 1);
                        IMapSurroundFrame pLegend = getMapSurroundFrame(workGra, "图例");
                        IObjectCopy pLC = new ObjectCopyClass();
                        ILegendFormat pLF = pLC.Copy((pLegend.MapSurround as ILegend).Format) as ILegendFormat;
                        pLC = new ObjectCopyClass();
                        ILegendClassFormat pLCF = pLC.Copy((pLegend.MapSurround as ILegend).get_Item(0).LegendClassFormat) as ILegendClassFormat;
                        delUnnecLegends(pLegend);//去除多余图例
                        (pLegend.MapSurround as ILegend).Format = pLF;
                        resizeLegend(workActiveView, pLCF, pLegend, workPW - mf2page[2] + 1, workPH - mf2page[3], 4);
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
                    workPageLayout.ZoomToWhole();


                    IMapDocument cMD = new MapDocumentClass();
                    string savePath = cDir + @"\" + advNode.Text + ".mxd";
                    cMD.New(savePath);
                    cMD.Open(savePath, "");
                    cMD.ReplaceContents(workPageLayout as IMxdContents);
                    cMD.Save(true, false);
                    cMD.Close();
                    cMD = null;
                    if (fmPgs.IsDisposed)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("用户取消批量森林资源现状辖区图输出！已输出的图保存在：" + cDir);
                        }
                        break;
                    }

                    pgsBar.PerformStep();
                    Application.DoEvents();
                }//foreach
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("批量森林资源现状辖区图输出完成！保存路径：" + cDir);
                }
                //MessageBox.Show("批量森林资源现状辖区图输出完成！保存路径：" + cDir, "提示", MessageBoxButtons.OK,
                         //MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (fmPgs != null && !fmPgs.IsDisposed)
                    fmPgs.Dispose();
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
                        ITextElement pTE=pgEle as ITextElement;
                        if ((pgEle as IElementProperties).Name == kvp.Key || (pgEle as ITextElement).Text == kvp.Key)
                        {
                            if (kvp.Value == "")
                            {
                                inGra.DeleteElement(pgEle);
                                inGra.Reset();
                            }
                            else
                            {
                                pTE.Text = kvp.Value;
                            }
                        }
                        if ((pgEle as IElementProperties).Name == "副题")
                        {
                            if(inTexts.ContainsKey("副题"))
                            {
                                if (inTexts["副题"] == "")
                                {
                                    inGra.DeleteElement(pgEle);
                                    inGra.Reset();
                                }
                                else
                                    pTE.Text = inTexts["副题"];
                            }
                        }

                    }
                    pgEle = pgGC.Next();
                }
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
        //生成border
        private IBorder createBorder(IGeometry inGeometry, IActiveView inAV)
        {
            ISymbolBorder pSymbolBorder = new SymbolBorderClass();
            pSymbolBorder.GetGeometry(inAV.ScreenDisplay as IDisplay, inGeometry.Envelope);
            ILineSymbol pLSymbol = new SimpleLineSymbolClass();
            pLSymbol.Color = getRGB(0, 0, 0);
            pLSymbol.Width = 1;
            pSymbolBorder.LineSymbol = pLSymbol;


            return pSymbolBorder as IBorder;
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
                else if (pEP.Name.Contains("左上"))
                    GeoPageLayoutFn.MoveElement(pEle, 0, inOff[1]);
                else if (pEP.Name.Contains("右上") || pEle is IPictureElement)
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0], inOff[1]);
                else if (pEP.Name == "规划标题")
                    GeoPageLayoutFn.MoveElement(pEle, 0, inOff[1]);
                else if (pEP.Name == "主题" || pEP.Name == "副题")
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0] / 2, inOff[1]);
                else if (pEP.Name == "比例尺")
                    GeoPageLayoutFn.MoveElement(pEle, inOff[0] / 2, 0);
                inGra.UpdateElement(pEle);
                pEle = inGra.Next();
            }

        }
        //十进制经纬度转字符串
        private void degreesTostring(double x, ref string deg, ref string min, ref string sec)
        {
            int degree = (int)Math.Floor(x);
            int minute = (int)Math.Floor((x - degree) * 60);
            int second = (int)Math.Floor(((x - degree) * 60 - minute) * 60);

            deg = degree + "°";
            min = minute + "′";
            sec = second + "″";

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
        #endregion

   

        #region 批量范围分幅输出mxd矢量
        public void pageLayoutExtentBatFFT(IMap pMap, Dictionary<string, List<int>> inDic, IFeatureClass inExtentFC, string inPath)
        {
            if (inDic.Count == 0)
                return;
            if (pMap.LayerCount == 0)
                return;
            if (inExtentFC == null)
                return;
            IFeatureClass jhtb = null;
            try
            {
                jhtb = ModGetData.GetFeatureClassByNodeKey(ModGetData.AttrValue("JHTB1W", "NodeKey"));
            }
            catch(Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "根据配置文件未找到1万接合图表层！");
            }
            if (jhtb == null)
                return;
            IObjectCopy pOC = new ObjectCopyClass();
            IMap newMap = pOC.Copy(pMap) as IMap;
            IMapLayers newMapLyrs = newMap as IMapLayers;
            for (int i = 0; i < newMap.LayerCount; i++)
            {
                ILayer pLyr = newMap.get_Layer(i);
                if (pLyr.Name.Contains("影像"))
                {
                    newMap.DeleteLayer(pLyr);
                    newMapLyrs.InsertLayer(pLyr, false, newMapLyrs.LayerCount);
                }
            }
            IEnumerator<string> enm = inDic.Keys.GetEnumerator();
            enm.Reset();
            string key = "";
            if(enm.MoveNext())
                key=enm.Current;
            FrmSheetMapSetExtent_ZT tdlyXQT = new FrmSheetMapSetExtent_ZT(10000, "", key);
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            tdlyXQT.Jhtb = jhtb;
            IList<string> fdLst = new List<string>();
            for (int fdx = 0; fdx < inExtentFC.Fields.FieldCount; fdx++)
            {
                if (inExtentFC.Fields.get_Field(fdx).Type != esriFieldType.esriFieldTypeGeometry)
                {
                    fdLst.Add(inExtentFC.Fields.get_Field(fdx).Name);
                }

            }
            tdlyXQT.LstFields = fdLst;//字段集合

            List<ILayer> lyrs = new List<ILayer>();
            IList<ILayer> pgLyrs = new List<ILayer>();//面层集合

            for (int i = 0; i < newMap.LayerCount; i++)
            {
                ILayer pLyr = newMap.get_Layer(i);
                addLayer(pLyr, ref lyrs);
            }
            foreach (ILayer lyr in lyrs)
            {
                if (lyr is IFeatureLayer)
                {
                    IFeatureLayer pFL = lyr as IFeatureLayer;
                    if (pFL.FeatureClass == null)
                        continue;
                    if (pFL.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        pgLyrs.Add(lyr);
                }
            }
            tdlyXQT.LstPolygonLyrs = pgLyrs;

            tdlyXQT.ExtentFeature = inExtentFC.Search(null, false).NextFeature();

            if (tdlyXQT.ShowDialog() != DialogResult.OK)//用户界面弹出
                return;
            if (tdlyXQT.LstResPolygonLyrs != null)
                pgLyrs = tdlyXQT.LstResPolygonLyrs;//面层集合，为空则用户未设置
            //设置面层符号透明
            if (pgLyrs != null)
            {
                foreach (ILayer lyr in pgLyrs)
                {
                    setNoSymbol(lyr);
                }
            }
            FormProgress fmPgs = null;
            try
            {
                string fcName = (inExtentFC as IDataset).Name;


                ILayer extenLayer = makeLayer(inExtentFC);
                extenLayer.Name = "红线数据";
                IMapLayers pMapLayers = newMap as IMapLayers;
                pMapLayers.InsertLayer(extenLayer, false, 0);
                fmPgs = new FormProgress();
                ProgressBar pgsBar = fmPgs.progressBar1;
                pgsBar.Minimum = 1;
                pgsBar.Maximum = inDic.Count + 1;
                pgsBar.Step = 1;
                fmPgs.TopLevel = true;
                fmPgs.Text = "正在批量输出" + fcName + "文件包含的范围红线图";
                fmPgs.Show();
                //int i = 1;
                string OID = inExtentFC.OIDFieldName;
                foreach (KeyValuePair<string, List<int>> kvp in inDic)
                {
                    fmPgs.lblOut.Text = "总共" + inDic.Count.ToString() + "个文档，正在输出第" + pgsBar.Value + "个";
                    string lyrDef = OID + " IN(";
                    tdlyXQT.MapTextElements["新图幅号"] = kvp.Key;
                    if (tdlyXQT.MapTextElements["新旧图幅号"] == "old")
                    {
                        string whr = "MAPNUMBER='", tfh = kvp.Key;
                        if (tfh.Contains(" "))
                            whr += tfh + "'";
                        else
                            whr += tfh.Insert(3, " ").Insert(5, " ") + "'";
                        tdlyXQT.MapTextElements["图幅号"] = getValueOfFeature(whr, "MAPNUMBER_OLD", tdlyXQT.Jhtb);
                    }
                    else
                        tdlyXQT.MapTextElements["图幅号"] = kvp.Key;
                    foreach (int oid in kvp.Value)
                    {
                        lyrDef += oid.ToString() + ",";


                    }
                    lyrDef = lyrDef.Substring(0, lyrDef.Length - 1);
                    lyrDef += ")";
                    IFeatureLayerDefinition pFLD = extenLayer as IFeatureLayerDefinition;
                    pFLD.DefinitionExpression = lyrDef;
                    pageLayoutExtentZTFFTforBat(newMap, tdlyXQT, inPath, kvp.Value, inExtentFC);
                    //i++;
                    if (fmPgs.IsDisposed)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("用户取消批量" + fcName + "文件包含的范围红线图输出！已输出的图保存在：" + inPath);
                        }
                        break;
                    }

                    pgsBar.PerformStep();
                    Application.DoEvents();
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("批量范围图输出完成！保存路径：" + inPath);
                }
                //MessageBox.Show("批量范围图输出完成！保存路径：" + cDir, "提示", MessageBoxButtons.OK,
                //         MessageBoxIcon.Information);
            }
            catch { }
            finally
            {
                if (fmPgs != null && !fmPgs.IsDisposed)
                    fmPgs.Dispose();
            }
        }
        //单个范围输出针对批量
        private void pageLayoutExtentZTFFTforBat(IMap inMap, FrmSheetMapSetExtent_ZT inFmSet, string inPath
            , List<int> oids, IFeatureClass inFC)
        {
            FrmSheetMapSetExtent_ZT tdlyXQT = inFmSet;
            IList<string> lblFds = tdlyXQT.LstResFields;//标注集合，为空则用户未设置
            IList<ILayer> pgLyrs = null;
            if (tdlyXQT.LstResPolygonLyrs != null)
                pgLyrs = tdlyXQT.LstResPolygonLyrs;
            else
                pgLyrs = tdlyXQT.LstPolygonLyrs;
            string subHeadFd = tdlyXQT.ResSubHeadFields;//副标题字段
            bool hasSubHead = tdlyXQT.HasSubHead;//是否显示副标题
            bool hasLegend = tdlyXQT.HasLegend;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            string subHead = "";
            if (hasSubHead)
            {
                string whr = "", tfh = pTextEles["图幅号"];
                if (pTextEles["新旧图幅号"] == "new")
                {
                    whr = "MAPNUMBER='";
                    if (tfh.Contains(" "))
                        whr += tfh + "'";
                    else
                        whr += tfh.Insert(3, " ").Insert(5, " ") + "'";
                }
                else
                    whr = "MAPNUMBER_OLD='" + tfh + "'";
               

                subHead = getValueOfFeature(whr, "MAPNAME", tdlyXQT.Jhtb);
                pTextEles["副题"] = subHead + "\r\n" + pTextEles["图幅号"];
            }
            else
                pTextEles["副题"] = pTextEles["图幅号"];
            IPageLayout workPageLayout = new PageLayoutClass();
            IPageLayoutControl pPLC = new PageLayoutControlClass();
            pPLC.PageLayout = workPageLayout;
            
            IActiveView workActiveView = workPageLayout as IActiveView;
            //pOC = new ObjectCopyClass();
            //IMap newMap = pOC.Copy(inMap) as IMap;
            IMap newMap = inMap;//地图引用，而非拷贝！！！！
            procLyrScale(newMap);//yjl取消比例尺显示限制
            IMap workMap = workActiveView.FocusMap;
            workMap.Name = pTextEles["主题"];
            workMap.SpatialReference = inMap.SpatialReference;
            IMapLayers pMapLayers = workMap as IMapLayers;
            for (int i = 0; i < newMap.LayerCount; i++)
            {
                pMapLayers.InsertLayer(newMap.get_Layer(i), false, pMapLayers.LayerCount);
            }
            //生成分幅图样式
            pageLayoutTDLYFFT(pPLC, pTextEles, 1);
            IGeometry inGeometry=workMap.ClipGeometry;
            IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
            IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
            IElement pMapEle = pMapFrame as IElement;
            IEnvelope pEnv = pMapEle.Geometry.Envelope;
            //更新范围图例--更改了渲染对象
            GeoPageLayoutFn.updateMapSymbol(inMap, inGeometry);
            double x = pMapEle.Geometry.Envelope.XMax;//地图框架的右上点坐标
            double y = pMapEle.Geometry.Envelope.YMax;
            //添加图例
           
            if (hasLegend)
            {
                AddLegend(pPLC.PageLayout, pPLC.ActiveView.FocusMap, x + 1, y, 6, 1);
                IMapSurroundFrame pLegend = getMapSurroundFrame(workGra, "图例");
                IObjectCopy pLC = new ObjectCopyClass();
                ILegendFormat pLF = pLC.Copy((pLegend.MapSurround as ILegend).Format) as ILegendFormat;
                pLC = new ObjectCopyClass();
                ILegendClassFormat pLCF = pLC.Copy((pLegend.MapSurround as ILegend).get_Item(0).LegendClassFormat) as ILegendClassFormat;
                if (pgLyrs != null)
                {
                    if (pgLyrs.Count != 0)
                        delUnnecLegends(pLegend, pgLyrs);//去除透明图例
                }
                sortLegend(pLegend);
                (pLegend as IElement).Activate(workActiveView.ScreenDisplay);//激活图例
                delUnnecLegendItemInfo(pLegend);
                (pLegend.MapSurround as ILegend).Format = pLF;

                resizeLegend(workPageLayout as IActiveView,pLCF, pLegend, x + 1, y, 6);

            }
            //else
            //   workGra.DeleteElement(pLegend as IElement);
           
           
            bool hasBL = false;
            if (tdlyXQT.HasBootLine && inFC.ShapeType == esriGeometryType.esriGeometryPolygon)//与引线有关
                hasBL = true;
            foreach (int oid in oids)
            {
                IFeature pFeature = inFC.GetFeature(oid);
                IEnvelope pEnvFea = pFeature.ShapeCopy.Envelope;

                //IPoint fromPoint = new PointClass();
                //fromPoint.X = pEnv.XMin + ((pEnvFea.XMax+pEnvFea.XMin)/2 - inGeometry.Envelope.XMin) * 100 / inScale;
                //fromPoint.Y = pEnv.YMin + ((pEnvFea.YMax + pEnvFea.YMin) / 2 - inGeometry.Envelope.YMin) * 100 / inScale;




                IPoint p = new PointClass();
                p.X = pEnv.XMin + (pFeature.ShapeCopy.Envelope.XMax - inGeometry.Envelope.XMin) * 100 / inScale;
                p.Y = pEnv.YMin + (pFeature.ShapeCopy.Envelope.YMin - inGeometry.Envelope.YMin) * 100 / inScale;

                IPoint fromPoint = null;
                if (hasBL)
                {
                    IArea pArea = pFeature.ShapeCopy as IArea;
                    fromPoint = pArea.LabelPoint;
                    fromPoint.X = pEnv.XMin + (fromPoint.X - inGeometry.Envelope.XMin) * 100 / inScale;
                    fromPoint.Y = pEnv.YMin + (fromPoint.Y - inGeometry.Envelope.YMin) * 100 / inScale;
                    IPolyline pLine = new PolylineClass();//引线
                    pLine.FromPoint = fromPoint;
                    pLine.ToPoint = p;
                    GeoPageLayoutFn.drawPolylineElement(pLine, workGra, getRGB(255, 0, 0), 1);
                }

                string strEle = "";
                //设置标注
                if (lblFds != null)
                {
                    foreach (string fd in lblFds)
                    {
                        int fdx = inFC.FindField(fd);
                        strEle += pFeature.get_Value(fdx).ToString() + "\r\n";
                    }
                    strEle = strEle.Substring(0, strEle.Length - 2);
                    GeoPageLayoutFn.drawTextElement(p, workGra, strEle);
                }
                //设置副标题
               
            }//foreach oid
           
            //if (!tdlyXQT.HasNorthArrow)
            //    delNorthArrow(workGra);
            updateTextEle(workGra, pTextEles);
            IMapDocument cMD = new MapDocumentClass();
            string savePath = inPath + "\\";
            if (pTextEles["副题"] != "")
                savePath += pTextEles["副题"].Replace("\r\n","") + ".mxd";
            else if (pTextEles["主题"] != "")
                savePath += pTextEles["主题"] + ".mxd";
            else
                savePath += "noname.mxd";
            cMD.New(savePath);
            cMD.Open(savePath, "");
            workPageLayout.ZoomToWhole();
            //FrmPageLayout fmPl = new FrmPageLayout(workPageLayout);
            //fmPl.Show();
            cMD.ReplaceContents(workPageLayout as IMxdContents);
            cMD.Save(true, false);
            cMD.Close();
        }
        //生成分幅样式
        private  void pageLayoutTDLYFFT(IPageLayoutControl axPageLayoutControl1, Dictionary<string, string> pgTextElements, int typeZT)
        {
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1 as IPageLayoutControl;
            pDrawSheetMap.vScale = Convert.ToUInt32(pgTextElements["比例尺"].Substring(2));
            pDrawSheetMap.m_intPntCount = 3;
            pDrawSheetMap.type_ZT = typeZT;//专题类型
            pDrawSheetMap.m_pPrjCoor = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            string[] astrMapNo = pgTextElements["新图幅号"].Split(' ');//若图幅号带空格 则去空格
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
                    if (pgEle is ITextElement)
                    {
                        if((pgEle as ITextElement).Text == kvp.Key||(pgEle as IElementProperties).Name==kvp.Key)
                        {
                            if(kvp.Value!="")
                               (pgEle as ITextElement).Text = kvp.Value;
                            else
                            {
                                pgGC.DeleteElement(pgEle);
                                pgGC.Reset();
                            }

                        }


                    }
                    pgEle = pgGC.Next();
                }
            }

            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            if (pGCS.ElementSelectionCount != 0)
            {
                pGCS.UnselectAllElements();

            }
        }

        // 根据图幅号获得投影文件   
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
                strtfh = "CGCS 2000 GK CM " + tfh.ToString() + "E.prj";
            }
            catch
            {

            }
            string strPrjFileName = Application.StartupPath + "\\..\\Prj\\CGCS2000\\" + strtfh;
            if (!System.IO.File.Exists(strPrjFileName)) return null;

            ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
            return pSpaFac.CreateESRISpatialReferenceFromPRJFile(strPrjFileName);

        }
        //根据某字段获取另一字段的值
        private string getValueOfFeature(string whereclause, string desField,IFeatureClass inFC)
        {
            string res = "";
            IQueryFilter pQF = new QueryFilterClass();
            pQF.WhereClause = whereclause;
            ITable pTable = inFC as ITable;
            ICursor pCursor = pTable.Search(pQF, false);
            IRow pRow = pCursor.NextRow();
            int fdx = pTable.FindField(desField);
            if (fdx != -1 && pRow != null)
            {
                res = pRow.get_Value(fdx).ToString();

            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            pCursor = null;
            pRow = null;
            return res;

        }
        #endregion

        #region 批量范围输出mxd矢量
        public void pageLayoutExtentBat(IMap pMap, Dictionary<IGeometry, List<int>> inDic,IFeatureClass inExtentFC,string inPath)
        {
            if (inDic.Count == 0)
                return;
            if (pMap.LayerCount == 0)
                return;
            if (inExtentFC == null)
                return;
            IObjectCopy pOC = new ObjectCopyClass();
            IMap newMap = pOC.Copy(pMap) as IMap;
            IMapLayers newMapLyrs = newMap as IMapLayers;
            for (int i = 0; i < newMap.LayerCount; i++)
            {
                ILayer pLyr = newMap.get_Layer(i);
                if (pLyr.Name.Contains("影像"))
                {
                    newMap.DeleteLayer(pLyr);
                    newMapLyrs.InsertLayer(pLyr, false, newMapLyrs.LayerCount);
                }
            }
            FrmExtentZTMapSetBat tdlyXQT = new FrmExtentZTMapSetBat();
            tdlyXQT.WriteLog = WriteLog; //ygc 2012-9-12 是否写日志
            IList<string> fdLst = new List<string>();
            for (int fdx = 0; fdx < inExtentFC.Fields.FieldCount; fdx++)
            {
                if (inExtentFC.Fields.get_Field(fdx).Type != esriFieldType.esriFieldTypeGeometry)
                {
                    fdLst.Add(inExtentFC.Fields.get_Field(fdx).Name);
                }
 
            }
            tdlyXQT.LstFields = fdLst;//字段集合
          
            List<ILayer> lyrs = new List<ILayer>();
            IList<ILayer> pgLyrs=new List<ILayer>();//面层集合

            for (int i = 0; i < newMap.LayerCount; i++)
            {
                ILayer pLyr = newMap.get_Layer(i);
                addLayer(pLyr, ref lyrs);
            }
            foreach (ILayer lyr in lyrs)
            {
                if(lyr is IFeatureLayer)
                {
                    IFeatureLayer pFL=lyr as IFeatureLayer;
                    if (pFL.FeatureClass == null)
                        continue;
                    if(pFL.FeatureClass.ShapeType==esriGeometryType.esriGeometryPolygon)
                        pgLyrs.Add(lyr);
                }
            }
            tdlyXQT.LstPolygonLyrs=pgLyrs;
            
            tdlyXQT.ExtentFeature = inExtentFC.Search(null,false).NextFeature();

            if (tdlyXQT.ShowDialog() != DialogResult.OK)//用户界面弹出
                return;
            if (tdlyXQT.LstResPolygonLyrs != null)
            {
                pgLyrs = tdlyXQT.LstResPolygonLyrs;//面层集合，为空则用户未设置
                //设置面层符号透明
                if (pgLyrs != null)
                {
                    foreach (ILayer lyr in pgLyrs)
                    {
                        setNoSymbol(lyr);
                    }
                }
            }
            FormProgress fmPgs = null;
            try
            {
                string fcName = (inExtentFC as IDataset).Name;
                
               
                ILayer extenLayer = makeLayer(inExtentFC);
                extenLayer.Name = "红线数据";
                IMapLayers pMapLayers = newMap as IMapLayers;
                pMapLayers.InsertLayer(extenLayer, false, 0);
                fmPgs = new FormProgress();
                ProgressBar pgsBar = fmPgs.progressBar1;
                pgsBar.Minimum = 1;
                pgsBar.Maximum = inDic.Count+1;
                pgsBar.Step = 1;
                fmPgs.TopLevel = true;
                fmPgs.Text = "正在批量输出" + fcName + "文件包含的范围红线图";
                fmPgs.Show();
                //int i = 1;
                string OID = inExtentFC.OIDFieldName;
                foreach (KeyValuePair<IGeometry, List<int>> kvp in inDic)
                {
                    fmPgs.lblOut.Text = "总共" + inDic.Count.ToString() + "个文档，正在输出第" + pgsBar.Value + "个";
                    string lyrDef = OID+" IN(";
                    foreach (int oid in kvp.Value)
                    {
                        lyrDef += oid.ToString()+",";


                    }
                    lyrDef = lyrDef.Substring(0, lyrDef.Length - 1);
                    lyrDef += ")";
                    IFeatureLayerDefinition pFLD = extenLayer as IFeatureLayerDefinition;
                    pFLD.DefinitionExpression = lyrDef;
                    pageLayoutExtentZTforBat(newMap, kvp.Key, tdlyXQT,inPath,kvp.Value,inExtentFC);
                    //i++;
                    if (fmPgs.IsDisposed)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("用户取消批量" + fcName + "文件包含的范围红线图输出！已输出的图保存在：" + inPath);
                        }
                        break;
                    }

                    pgsBar.PerformStep();
                    Application.DoEvents();
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("批量范围图输出完成！保存路径：" + inPath);
                }
                //MessageBox.Show("批量范围图输出完成！保存路径：" + cDir, "提示", MessageBoxButtons.OK,
                //         MessageBoxIcon.Information);
            }
            catch { }
            finally
            {
                if (fmPgs != null && !fmPgs.IsDisposed)
                    fmPgs.Dispose();
            }
        }
        //识别纸张
        private esriPageFormID getPageID(short portrait, int w)
        {
            esriPageFormID PFI = esriPageFormID.esriPageFormCUSTOM;
            if (portrait==1)
            {
                switch (w)
                {
                    case 841:
                        PFI = esriPageFormID.esriPageFormA0;
                        break;
                    case 594:
                        PFI = esriPageFormID.esriPageFormA1;
                        break;
                    case 420:
                        PFI = esriPageFormID.esriPageFormA2;
                        break;
                    case 297:
                        PFI = esriPageFormID.esriPageFormA3;
                        break;
                    case 210:
                        PFI = esriPageFormID.esriPageFormA4;
                        break;
                    case 148:
                        PFI = esriPageFormID.esriPageFormA5;
                        break;

                }

            }
            else
            {
                switch (w)
                {
                    case 1189:
                        PFI = esriPageFormID.esriPageFormA0;
                        break;
                    case 841:
                        PFI = esriPageFormID.esriPageFormA1;
                        break;
                    case 594:
                        PFI = esriPageFormID.esriPageFormA2;
                        break;
                    case 420:
                        PFI = esriPageFormID.esriPageFormA3;
                        break;
                    case 297:
                        PFI = esriPageFormID.esriPageFormA4;
                        break;
                    case 210:
                        PFI = esriPageFormID.esriPageFormA5;
                        break;

                }
 
            }
            return PFI;
        }
        //构造范围红线图层
        private ILayer makeLayer(IFeatureClass inFC)
        {
            IFeatureLayer pLayer = new FeatureLayerClass();
            pLayer.FeatureClass = inFC;
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            //颜色对象
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.UseWindowsDithering = false;
            ISymbol pSymbol = (ISymbol)pFillSymbol;
            //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pRGBColor.Red = 255;
            pRGBColor.Green = 0;
            pRGBColor.Blue = 0;
            pLineSymbol.Color = pRGBColor;

            pLineSymbol.Width = 2;
            //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pFillSymbol.Outline = pLineSymbol;
            pRGBColor.Transparency = 0;
            pFillSymbol.Color = pRGBColor;
            ISimpleRenderer pSimpleRenderer=new SimpleRendererClass();
            if (inFC.ShapeType == esriGeometryType.esriGeometryPolyline)
                pSimpleRenderer.Symbol = pLineSymbol as ISymbol;
            else if (inFC.ShapeType == esriGeometryType.esriGeometryPolygon)
                pSimpleRenderer.Symbol = pFillSymbol as ISymbol;
            IGeoFeatureLayer pGeoLyr = pLayer as IGeoFeatureLayer;
            pGeoLyr.Renderer = pSimpleRenderer as IFeatureRenderer;
            return pLayer as ILayer;

        }
        //单个范围输出针对批量
        private void pageLayoutExtentZTforBat(IMap inMap, IGeometry inGeometry,FrmExtentZTMapSetBat inFmSet,string inPath
            ,List<int> oids,IFeatureClass inFC)
        {
            FrmExtentZTMapSetBat tdlyXQT=inFmSet;
            IList<string> lblFds = tdlyXQT.LstResFields;//标注集合，为空则用户未设置
            IList<ILayer> pgLyrs=null;
            if (tdlyXQT.LstResPolygonLyrs != null)
                pgLyrs = tdlyXQT.LstResPolygonLyrs;
            else
                pgLyrs = tdlyXQT.LstPolygonLyrs;
            string subHeadFd = tdlyXQT.ResSubHeadFields;//副标题字段
            bool hasSubHead = tdlyXQT.HasSubHead;//是否显示副标题
            bool hasLegend = tdlyXQT.HasLegend;
            Dictionary<string, string> pTextEles = tdlyXQT.MapTextElements;
            int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\BatExtentTemplate.mxd");
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
            esriPageFormID pfi = getPageID(workPage.Orientation, (int)(workPW * 10));
            if (pfi != esriPageFormID.esriPageFormCUSTOM)
                workPage.FormID = pfi;
            else
                workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
            IActiveView workActiveView = workPageLayout as IActiveView;
            //pOC = new ObjectCopyClass();
            //IMap newMap = pOC.Copy(inMap) as IMap;
            IMap newMap = inMap;//地图引用，而非拷贝！！！！
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
            //更新范围图例--更改了渲染对象
            GeoPageLayoutFn.updateMapSymbol(inMap, inGeometry);
            IMapSurroundFrame pLegend = getMapSurroundFrame(workGra, "图例");
            //添加图例
            if (hasLegend)
            {
                //AddLegend2(workPageLayout, workMap, workPW - mf2page[2] + 0.5, workPH - mf2page[3], 3, 1,pgLyrs);
                if (pgLyrs != null)
                {
                    if (pgLyrs.Count != 0)
                        delUnnecLegends(pLegend, pgLyrs);//去除透明图例
                }
                //delUnnecLegendItemInfo(pLegend);
                sortLegend(pLegend);
                (pLegend as IElement).Activate(workActiveView.ScreenDisplay);//激活图例

            }
            else
                workGra.DeleteElement(pLegend as IElement);
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
            string subHead = "";
            bool hasBL=false;
            if (tdlyXQT.HasBootLine && inFC.ShapeType == esriGeometryType.esriGeometryPolygon)//与引线有关
                hasBL = true;
            foreach (int oid in oids)
            {
                IFeature pFeature = inFC.GetFeature(oid);
                IEnvelope pEnvFea = pFeature.ShapeCopy.Envelope;
                
                //IPoint fromPoint = new PointClass();
                //fromPoint.X = pEnv.XMin + ((pEnvFea.XMax+pEnvFea.XMin)/2 - inGeometry.Envelope.XMin) * 100 / inScale;
                //fromPoint.Y = pEnv.YMin + ((pEnvFea.YMax + pEnvFea.YMin) / 2 - inGeometry.Envelope.YMin) * 100 / inScale;


              

                IPoint p = new PointClass();
                p.X = pEnv.XMin + (pFeature.ShapeCopy.Envelope.XMax - inGeometry.Envelope.XMin) * 100 / inScale;
                p.Y = pEnv.YMin + (pFeature.ShapeCopy.Envelope.YMin - inGeometry.Envelope.YMin) * 100 / inScale;

                IPoint fromPoint = null;
                if (hasBL)
                {
                    IArea pArea = pFeature.ShapeCopy as IArea;
                    fromPoint = pArea.LabelPoint;
                    fromPoint.X = pEnv.XMin + (fromPoint.X - inGeometry.Envelope.XMin) * 100 / inScale;
                    fromPoint.Y = pEnv.YMin + (fromPoint.Y - inGeometry.Envelope.YMin) * 100 / inScale;
                    IPolyline pLine = new PolylineClass();//引线
                    pLine.FromPoint = fromPoint;
                    pLine.ToPoint = p;
                    GeoPageLayoutFn.drawPolylineElement(pLine, workGra, getRGB(255, 0, 0), 1);
                }
               
                string strEle = "";
                //设置标注
                if (lblFds != null)
                {
                    foreach (string fd in lblFds)
                    {
                        int fdx = inFC.FindField(fd);
                        strEle += pFeature.get_Value(fdx).ToString() + "\r\n";
                    }
                    strEle = strEle.Substring(0, strEle.Length - 2);
                    GeoPageLayoutFn.drawTextElement(p, workGra, strEle);
                }
                //设置副标题
                if (hasSubHead)
                {
                    int idx = inFC.FindField(subHeadFd);
                    subHead += pFeature.get_Value(idx).ToString() + ",";
                }
            }//foreach oid
            if (subHead != "")
            {
                subHead = subHead.Substring(0, subHead.Length - 1);
                pTextEles["副题"] = subHead;
            }
            moveElements(workGra, offXY);
            updateTextEle(workGra, pTextEles);
            if (!tdlyXQT.HasNorthArrow)
                delNorthArrow(workGra);
            IMapDocument cMD = new MapDocumentClass();
            string savePath = inPath+"\\";
            if (hasSubHead && pTextEles["副题"] != "")
                savePath += pTextEles["副题"] + ".mxd";
            else if (pTextEles["主题"] != "")
                savePath += pTextEles["主题"] + ".mxd";
            else
                savePath += "noname.mxd";
            cMD.New(savePath);
            cMD.Open(savePath, "");
            workPageLayout.ZoomToWhole();
            //FrmPageLayout fmPl = new FrmPageLayout(workPageLayout);
            //fmPl.Show();
            cMD.ReplaceContents(workPageLayout as IMxdContents);
            cMD.Save(true, false);
            cMD.Close();
        }
        //设置面层符号透明
        private void setNoSymbol(ILayer inLayer)
        {
            IGeoFeatureLayer pGFL = inLayer as IGeoFeatureLayer;
            if (pGFL == null)
                return;
            IFeatureLayer pFL = pGFL as IFeatureLayer;
            IFeatureClass inFC = pFL.FeatureClass;
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            //颜色对象
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.UseWindowsDithering = false;
            ISymbol pSymbol = (ISymbol)pFillSymbol;
            //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pRGBColor.Red = 255;
            pRGBColor.Green = 255;
            pRGBColor.Blue = 255;
            pRGBColor.Transparency = 0;
            pLineSymbol.Color = pRGBColor;
            
            pLineSymbol.Width = 4;
            //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            pFillSymbol.Outline = pLineSymbol;
            
            pFillSymbol.Color = pRGBColor;
            ISimpleRenderer pSimpleRenderer = new SimpleRendererClass();
            if (inFC.ShapeType == esriGeometryType.esriGeometryPolyline)
                pSimpleRenderer.Symbol = pLineSymbol as ISymbol;
            else if (inFC.ShapeType == esriGeometryType.esriGeometryPolygon)
                pSimpleRenderer.Symbol = pFillSymbol as ISymbol;
            pGFL.Renderer = pSimpleRenderer as IFeatureRenderer;
        }
        #endregion

        #region 批量范围输出mxd栅格
        public void pageLayoutExtentRasterBat(IMap pMap, Dictionary<IGeometry, List<int>> inDic, IFeatureClass inExtentFC, string inPath)
        {
            if (inDic.Count == 0)
                return;
            if (pMap.LayerCount == 0)
                return;
            if (inExtentFC == null)
                return;
            IObjectCopy pOC = new ObjectCopyClass();
            IMap newMap = pOC.Copy(pMap) as IMap;
            FrmExtentZTRasterMapSetBat tdlyXQT = new FrmExtentZTRasterMapSetBat();
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12  是否写日志
            tdlyXQT.SourceMap = newMap;
            IList<string> fdLst = new List<string>();
            for (int fdx = 0; fdx < inExtentFC.Fields.FieldCount; fdx++)
            {
                if (inExtentFC.Fields.get_Field(fdx).Type != esriFieldType.esriFieldTypeGeometry)
                    fdLst.Add(inExtentFC.Fields.get_Field(fdx).Name);

            }
            tdlyXQT.LstFields = fdLst;//字段集合
          
           
            if (tdlyXQT.ShowDialog() != DialogResult.OK)//用户界面弹出
                return;
            if (tdlyXQT.DesMap.LayerCount == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("未找到栅格图层！","");
            }
            newMap = tdlyXQT.DesMap;
            FormProgress fmPgs = null;
            try
            {
                string fcName = (inExtentFC as IDataset).Name;


                ILayer extenLayer = makeLayer(inExtentFC);
                extenLayer.Name = "红线数据";
                IMapLayers pMapLayers = newMap as IMapLayers;
                pMapLayers.InsertLayer(extenLayer, false, 0);
                fmPgs = new FormProgress();
                ProgressBar pgsBar = fmPgs.progressBar1;
                pgsBar.Minimum = 1;
                pgsBar.Maximum = inDic.Count + 1;
                pgsBar.Step = 1;
                fmPgs.TopLevel = true;
                fmPgs.Text = "正在批量输出" + fcName + "文件包含的范围红线图";
                fmPgs.Show();
                int i = 1;
                string OID = inExtentFC.OIDFieldName;
                foreach (KeyValuePair<IGeometry, List<int>> kvp in inDic)
                {
                    fmPgs.lblOut.Text = "总共" + inDic.Count.ToString() + "个文档，正在输出第" + pgsBar.Value + "个";
                    string lyrDef = OID + " IN(";
                    foreach (int oid in kvp.Value)
                    {
                        lyrDef += oid.ToString() + ",";


                    }
                    lyrDef = lyrDef.Substring(0, lyrDef.Length - 1);
                    lyrDef += ")";
                    IFeatureLayerDefinition pFLD = extenLayer as IFeatureLayerDefinition;
                    pFLD.DefinitionExpression = lyrDef;
                    //string path = inPath + "\\" + i.ToString() + ".mxd";
                    pageLayoutExtentRasterZTforBat(newMap, kvp.Key, tdlyXQT, inPath, kvp.Value, inExtentFC);
                    i++;
                    if (fmPgs.IsDisposed)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("用户取消批量" + fcName + "文件包含的范围红线图输出！已输出的图保存在：" + inPath);
                        }
                        break;
                    }

                    pgsBar.PerformStep();
                    Application.DoEvents();
                }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("批量范围图输出完成！保存路径：" + inPath);
                }
                //MessageBox.Show("批量范围图输出完成！保存路径：" + cDir, "提示", MessageBoxButtons.OK,
                //         MessageBoxIcon.Information);
            }
            catch { }
            finally
            {
                if (fmPgs != null && !fmPgs.IsDisposed)
                    fmPgs.Dispose();
            }
        }
       
        //单个范围输出针对批量
        private void pageLayoutExtentRasterZTforBat(IMap inMap, IGeometry inGeometry, FrmExtentZTRasterMapSetBat inFmSet, string inPath
            , List<int> oids, IFeatureClass inFC)
        {
            FrmExtentZTRasterMapSetBat tdlyXQT = inFmSet;
            IList<string> lblFds = tdlyXQT.LstResFields;//标注集合，为空则用户未设置
            
            //int inScale = Convert.ToInt32(pTextEles["比例尺"].Split(':')[1]);
            int inScale = 10000;
            IPageLayout tlPageLayout = getTemplateGra(Application.StartupPath + "\\..\\Template\\BatExtentRasterTemplate.mxd");
            if (tlPageLayout == null)
                return;
           
            IObjectCopy pOC = new ObjectCopyClass();
            IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
            //IPageLayoutControl pPLC = new PageLayoutControlClass();
            //pPLC.PageLayout = workPageLayout;
            IPage workPage = workPageLayout.Page;
            IEnvelope workEnv = inGeometry.Envelope;
            double workPW = workEnv.Width * 100 / inScale,
               workPH = workEnv.Height * 100 / inScale;
            workPage.StretchGraphicsWithPage = false;
            esriPageFormID pfi = getPageID(workPage.Orientation, (int)(workPW * 10));
            if (pfi != esriPageFormID.esriPageFormCUSTOM)
                workPage.FormID = pfi;
            else
                workPage.PutCustomSize(workPW, workPH);//重设纸张大小单位厘米
            IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
            IActiveView workActiveView = workPageLayout as IActiveView;
            //pOC = new ObjectCopyClass();
            //IMap newMap = pOC.Copy(inMap) as IMap;
            IMap newMap = inMap;//地图引用，而非拷贝！！！！
            procLyrScale(newMap);//yjl取消比例尺显示限制
            IMap workMap = workActiveView.FocusMap;
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

            IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
            pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
            pMapFrame.MapBounds = inGeometry.Envelope;
            IElement pMapEle = pMapFrame as IElement;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(0, 0, workPW, workPH);
            pMapEle.Geometry = pEnv;

            bool hasBL = false;
            if (tdlyXQT.HasBootLine && inFC.ShapeType == esriGeometryType.esriGeometryPolygon)//与引线有关
                hasBL = true;
            string mxdname = "";
            int idx = inFC.FindField("DKBH");
            foreach (int oid in oids)
            {
                IFeature pFeature = inFC.GetFeature(oid);
                IPoint p = new PointClass();
                p.X = (pFeature.ShapeCopy.Envelope.XMax - inGeometry.Envelope.XMin) * 100 / inScale;
                p.Y = (pFeature.ShapeCopy.Envelope.YMin - inGeometry.Envelope.YMin) * 100 / inScale;
                IPoint fromPoint = null;
                if (hasBL)
                {
                    IArea pArea = pFeature.ShapeCopy as IArea;
                    fromPoint = pArea.LabelPoint;
                    fromPoint.X = pEnv.XMin + (fromPoint.X - inGeometry.Envelope.XMin) * 100 / inScale;
                    fromPoint.Y = pEnv.YMin + (fromPoint.Y - inGeometry.Envelope.YMin) * 100 / inScale;
                    IPolyline pLine = new PolylineClass();//引线
                    pLine.FromPoint = fromPoint;
                    pLine.ToPoint = p;
                    GeoPageLayoutFn.drawPolylineElement(pLine, workGra, getRGB(255, 0, 0), 1);
                }

                string strEle = "";
                //设置标注
                if (lblFds != null)
                {
                    foreach (string fd in lblFds)
                    {
                        int fdx = inFC.FindField(fd);
                        strEle += pFeature.get_Value(fdx).ToString() + "\r\n";
                    }
                    strEle = strEle.Substring(0, strEle.Length - 2);
                    GeoPageLayoutFn.drawTextElement(p, workGra, strEle);
                }
                
                if (idx != -1)
                    mxdname += pFeature.get_Value(idx).ToString() + ",";
                else
                    mxdname += oid + ",";
              
            }//foreach oid
            if (mxdname != "")
                mxdname = mxdname.Substring(0, mxdname.Length - 1);
            IMapDocument cMD = new MapDocumentClass();
            string savePath = inPath +"\\";
            if (mxdname != "")
                savePath += mxdname + ".mxd"; 

            cMD.New(savePath);
            cMD.Open(savePath, "");
            workPageLayout.ZoomToWhole();
            //FrmPageLayout fmPl = new FrmPageLayout(workPageLayout);
            //fmPl.Show();
            cMD.ReplaceContents(workPageLayout as IMxdContents);
            cMD.Save(true, false);
            cMD.Close();
        }

        #endregion

        #region 辖区总规图
      
        public void pageLayoutZTGHTXQT(IMap inMap, IGeometry inGeometry,string xzqName)
        {
            if (inMap == null)
                return;
            FrmTDLYGHMapSet tdlyXQT = new FrmTDLYGHMapSet();
            tdlyXQT.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            tdlyXQT.XZQname = xzqName;
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
            //进度条
            SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
            pgss.EnableCancel = false;
            pgss.ShowDescription = false;
            pgss.FakeProgress = true;
            pgss.TopMost = true;
            pgss.ShowProgress();
            Application.DoEvents();
            try
            {
                IObjectCopy pOC = new ObjectCopyClass();
                IPageLayout workPageLayout = pOC.Copy(tlPageLayout) as IPageLayout;
                //IPageLayoutControl pPLC = new PageLayoutControlClass();
                //pPLC.PageLayout = workPageLayout;

                //扩大行政区范围2厘米
                IEnvelope workEnv = new EnvelopeClass();
                IEnvelope pGeoEnv = inGeometry.Envelope;
                double xmin = pGeoEnv.XMin - 0.02 * inScale,
                    ymin = pGeoEnv.YMin - 0.02 * inScale,
                    xmax = pGeoEnv.XMax + 0.02 * inScale,
                    ymax = pGeoEnv.YMax + 0.02 * inScale;
                workEnv.PutCoords(xmin, ymin, xmax, ymax);
                workEnv.SpatialReference = inGeometry.SpatialReference;
                //根据范围和比例尺及模板边距，重设纸张大小
                IPage workPage = workPageLayout.Page;
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
                workMapAV.Extent = workEnv;
                //workMap.ClipGeometry = pExtent;
                workMap.MapScale = inScale;
                IGraphicsContainer workGra = workPageLayout as IGraphicsContainer;
                IMapFrame pMapFrame = (IMapFrame)workGra.FindFrame(workActiveView.FocusMap);
                pMapFrame.ExtentType = esriExtentTypeEnum.esriExtentBounds;
                pMapFrame.MapBounds = workEnv;
                IElement pMapEle = pMapFrame as IElement;
                IEnvelope pEnv = new EnvelopeClass();
                pEnv.PutCoords(mf2page[0], mf2page[1], workPW - mf2page[2], workPH - mf2page[3]);
                pMapEle.Geometry = pEnv;

                //晕线部分
                IObjectCopy cpGeo = new ObjectCopyClass();
                IGeometry xzqPg = cpGeo.Copy(inGeometry) as IGeometry;

                IPointCollection pPC = xzqPg as IPointCollection;
                //xzqBound.SpatialReference = null;

                ITransform2D pT2D = xzqPg as ITransform2D;
                pT2D.Scale(pPC.get_Point(0), (double)100 / inScale, (double)100 / inScale);
                //纸面坐标减去地图坐标得到平移量
                double movX = (pEnv.XMin + (pPC.get_Point(0).X - workMapAV.Extent.XMin) * 100 / inScale) - pPC.get_Point(0).X,
                   movY = (pEnv.YMin + (pPC.get_Point(0).Y - workMapAV.Extent.YMin) * 100 / inScale) - pPC.get_Point(0).Y;
                pT2D.Move(movX, movY);
                xzqPg.SpatialReference = pMapEle.Geometry.SpatialReference;
                ////遮盖相邻行政区元素
                //ITopologicalOperator pTO1 = pMapEle.Geometry as ITopologicalOperator;
                //IGeometry pSurround = pTO1.Difference(xzqPg);
                //GeoPageLayoutFn.drawPolygonElement(pSurround, workGra, getRGB(255, 255, 255), false, getRGB(0, 0, 0), 1.5);
                //晕线
                ITopologicalOperator pTO = xzqPg as ITopologicalOperator;
                IPolyline xzqBound = pTO.Boundary as IPolyline;
                GeoPageLayoutFn.drawPolylineElement(xzqBound, workPageLayout as IGraphicsContainer, getRGB(0, 0, 255), 2);

                //更新范围图例
                GeoPageLayoutFn.updateMapSymbol(workMap, workActiveView.Extent);

                //添加图例
                if (hasLegend)
                {
                    GeoPageLayoutFn.AddLegendZT(workPageLayout, workMap, workPW - mf2page[2], mf2page[1], 8, 2);
                    IMapSurroundFrame pLegend = getMapSurroundFrame(workGra, "图例");
                    IObjectCopy pLC = new ObjectCopyClass();
                    ILegendFormat pLF = pLC.Copy((pLegend.MapSurround as ILegend).Format) as ILegendFormat;
                    pLC = new ObjectCopyClass();
                    ILegendClassFormat pLCF = pLC.Copy((pLegend.MapSurround as ILegend).get_Item(0).LegendClassFormat) as ILegendClassFormat;
                    delUnnecLegends(pLegend);//去除多余图例
                    (pLegend.MapSurround as ILegend).Format = pLF;
                     resizeLegendGH(workActiveView, pLCF, pLegend,workPW - mf2page[2], mf2page[1], 8,2);

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
                FrmPageLayout fmPL = new FrmPageLayout(workPageLayout);
                fmPL.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                fmPL.Show();
            }
            catch { }
            finally
            {
                pgss.Close();
            }
        }
        #endregion

        private IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pColor;
            pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }
        //处理图例，删除多余图例，默认组图层为一个专题
        private void delUnnecLegends(IMapSurroundFrame inMSF)
        {
            ILegend2 pLegend = inMSF.MapSurround as ILegend2;
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
                if (tmp.Name == "晕线")
                {
                    pLegend.RemoveItem(i);
                    break;
                }
            }
            foreach (ILayer lyr in lyrs)
            {
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
                                break;

                            }
                        }
                    }
                    if (lyr.Equals(tmp))
                    {
                        pLegend.RemoveItem(i);
                        break;
                    }
                    
                   
                }

            }


        }
        //处理图例，删除透明图例
        private void delUnnecLegends(IMapSurroundFrame inMSF, IList<ILayer> lyrs)
        {
            ILegend2 pLegend = inMSF.MapSurround as ILegend2;
          
            if (pLegend == null)
                return;
          
            foreach (ILayer lyr in lyrs)
            {
                for (int i = 0; i < pLegend.ItemCount; i++)
                {
                    ILegendItem pLegendItem = pLegend.get_Item(i);
                    ILayer tmp = pLegendItem.Layer;

                    if (lyr.Equals(tmp))
                    {
                        pLegend.RemoveItem(i);
                        break;
                    }
                }
            }
        }
        //处理图例，去掉图例项的图层名和标头
        private void delUnnecLegendItemInfo(IMapSurroundFrame inMSF)
        {
            ILegend2 pLegend = inMSF.MapSurround as ILegend2;
           
            if (pLegend == null)
                return;

      
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.ShowDescriptions = false;
                pLegendItem.ShowHeading = false;
                pLegendItem.ShowLayerName = false;
               
            }
            
        }
        //处理图例，图例比率变化，重设图例的大小
        private void resizeLegend(IActiveView inAV,ILegendClassFormat inLCF, IMapSurroundFrame inMSF, double posX, double posY, double legW)
        {
            ILegend2 pLegend = inMSF.MapSurround as ILegend2;
            pLegend.Refresh();
            if (pLegend == null)
                return;
            pLegend.Format.DefaultPatchHeight = 12;
            pLegend.Format.DefaultPatchWidth = 24;
            pLegend.Format.VerticalPatchGap = 10;
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.LegendClassFormat.LabelSymbol=inLCF.LabelSymbol;
            }

            IElement element = inMSF as IElement;
            //Get aspect ratio
            ESRI.ArcGIS.Carto.IQuerySize querySize = inMSF.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX, (posY - legW / aspectRatio), (posX + legW), posY);
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.AnchorPoint = esriAnchorPointEnum.esriTopLeftCorner;
            element.Activate(inAV.ScreenDisplay);
            inAV.Refresh();

        }
        //处理图例，图例比率变化，重设图例的大小--规划辖区图
        private void resizeLegendGH(IActiveView inAV, ILegendClassFormat inLCF, IMapSurroundFrame inMSF, double posX, double posY, double legW,int col)
        {
            ILegend2 pLegend = inMSF.MapSurround as ILegend2;
            pLegend.Refresh();
            if (pLegend == null)
                return;
            pLegend.Format.DefaultPatchHeight = 12;
            pLegend.Format.DefaultPatchWidth = 24;
            pLegend.Format.VerticalPatchGap = 10;
            pLegend.Format.TitleSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.LegendClassFormat.LabelSymbol = inLCF.LabelSymbol;
            }
            pLegend.AdjustColumns(col);
            IElement element = inMSF as IElement;
            //Get aspect ratio
            ESRI.ArcGIS.Carto.IQuerySize querySize = inMSF.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX - legW, posY, posX, (posY + legW / aspectRatio));
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.AnchorPoint = esriAnchorPointEnum.esriBottomRightCorner;
            element.Activate(inAV.ScreenDisplay);
            inAV.Refresh();

        }
        //处理图例，对图例项排序面线点,同时删除图例项的图层名和标头
        private void sortLegend(IMapSurroundFrame inMSF)
        {
            ILegend2 pLegend = inMSF.MapSurround as ILegend2;

            if (pLegend == null)
                return;
            //将多边形移到上面
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.ShowDescriptions = false;
                pLegendItem.ShowHeading = false;
                pLegendItem.ShowLayerName = false;

                ILayer tmp = pLegendItem.Layer;
                if (!tmp.Valid)
                {
                    pLegend.RemoveItem(i);
                }
                IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                if (pFeatureLayer != null)
                {

                    if (pFeatureLayer.FeatureClass != null)
                    {
                        if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            pLegend.RemoveItem(i);
                            pLegend.InsertItem(0, pLegendItem);
                        }
                    }
                }
            }
            //将点移到下面
            for (int i = pLegend.ItemCount - 1; i >= 0; i--)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                ILayer tmp = pLegendItem.Layer;
                IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                if (pFeatureLayer != null)
                {
                    if (pFeatureLayer.FeatureClass != null)
                    {
                        if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        {
                            pLegend.RemoveItem(i);
                            pLegend.InsertItem(pLegend.ItemCount, pLegendItem);//remove导致itemcount时时变化，cautions
                        }
                    }
                }

            }
        }
        //寻找图例对象
        private IMapSurroundFrame getMapSurroundFrame(IGraphicsContainer inGra,string inName)
        {
            IMapSurroundFrame res = null;
            IGraphicsContainer pgGC = inGra;
            pgGC.Reset();
            IElement pgEle = pgGC.Next();
            while (pgEle != null)
            {
                if (pgEle is IMapSurroundFrame)
                {
                    switch(inName)
                    {
                        case "图例":
                            {
                                IMapSurroundFrame pMSF = pgEle as IMapSurroundFrame;
                                if (pMSF.MapSurround is ILegend)
                                    res = pMSF;
                                break;
                            }
                    }
                }
                pgEle = pgGC.Next();
            }
            return res;
 
        }
        //删除模板上的指北针
        private void delNorthArrow(IGraphicsContainer inGra)
        {
            IGraphicsContainer pgGC = inGra;
            pgGC.Reset();
            IElement pgEle = pgGC.Next();
            while (pgEle != null)
            {
                if (pgEle is IMapSurroundFrame)
                {
                    IMapSurroundFrame pMSF = pgEle as IMapSurroundFrame;
                    if (pMSF.MapSurround is INorthArrow)
                    {
                        pgGC.DeleteElement(pgEle);
                        break;
                    }
                }
                pgEle = pgGC.Next();
            }
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
        //创建图形结果目录"\\..\\OutputResults\\图形成果\\"
        private string createDir(string inPostfix)
        {
            string datefilename = DateTime.Now.ToString().Replace(':', '_');
            string dstfilename = Application.StartupPath + "\\..\\OutputResults\\图形成果\\" + inPostfix;
            if (Directory.Exists(dstfilename))
            {
                if (MessageBox.Show("已经批量输出过该行政区，是否覆盖？", "提示", MessageBoxButtons.YesNo,
                         MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Directory.Delete(dstfilename, true);

                }
                else
                    return "";
            }

            Directory.CreateDirectory(dstfilename);
            string Dir = dstfilename;
            return Dir;

        }
        //去掉图层的比例尺显示限制
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
        //添加图例
        public void AddLegend(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map, System.Double posX, System.Double posY, System.Double legW, int legendCol)
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
                if (!tmp.Valid)
                {
                    pLegend.RemoveItem(i);
                }
                IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                if (pFeatureLayer != null)
                {

                    if (pFeatureLayer.FeatureClass != null)
                    {
                        if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                        {
                            pLegend.RemoveItem(i);
                            pLegend.InsertItem(0, pLegendItem);
                        }
                    }
                }
            }
            //将点移到下面
            for (int i = pLegend.ItemCount - 1; i >= 0; i--)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                ILayer tmp = pLegendItem.Layer;
                IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                if (pFeatureLayer != null)
                {
                    if (pFeatureLayer.FeatureClass != null)
                    {
                        if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                        {
                            pLegend.RemoveItem(i);
                            pLegend.InsertItem(pLegend.ItemCount, pLegendItem);//remove导致itemcount时时变化，cautions
                        }
                    }
                }

            }


            pLegend.Title = "图 例";
            pLegend.AdjustColumns(legendCol);//yjl20110812
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            
            //Get aspect ratio
            ESRI.ArcGIS.Carto.IQuerySize querySize = mapSurroundFrame.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX, (posY - legW / aspectRatio), (posX + legW), posY);
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.Name = "图例";
            pep.AnchorPoint = esriAnchorPointEnum.esriTopLeftCorner;
            element.Activate((pageLayout as IActiveView).ScreenDisplay);//关键代码
            graphicsContainer.AddElement(element, 0);
            (graphicsContainer as IActiveView).Refresh();
        }
        //添加图例--批量范围矢量
        public void AddLegend2(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map, System.Double posX, System.Double posY, System.Double legW, int legendCol,IList<ILayer> lyrs)
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
            ILegendFormat pLegendFormat = pLegend.Format;
            ITextSymbol titleSymbol = pLegendFormat.TitleSymbol;
            //去除不需要输出的图层的图例
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                ILayer tmp = pLegendItem.Layer;
                foreach (ILayer lyr in lyrs)
                {
                    if(lyr.Equals(tmp))
                        pLegend.RemoveItem(i);

                }
                if (!tmp.Valid)
                {
                    pLegend.RemoveItem(i);
                }
            }
            //将多边形移到上面
            for (int i = 0; i < pLegend.ItemCount; i++)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                pLegendItem.ShowDescriptions = false;
                pLegendItem.ShowHeading = false;
                pLegendItem.ShowLayerName = false;
                ILayer tmp = pLegendItem.Layer;
                IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                if (pFeatureLayer.FeatureClass != null)
                {
                    if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pLegend.RemoveItem(i);
                        pLegend.InsertItem(0, pLegendItem);
                    }
                }
            }
            //将点移到下面
            for (int i = pLegend.ItemCount - 1; i >= 0; i--)
            {
                ILegendItem pLegendItem = pLegend.get_Item(i);
                ILayer tmp = pLegendItem.Layer;
                IFeatureLayer pFeatureLayer = tmp as IFeatureLayer;
                if (pFeatureLayer.FeatureClass != null)
                {
                    if ((tmp as IFeatureLayer).FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pLegend.RemoveItem(i);
                        pLegend.InsertItem(pLegend.ItemCount, pLegendItem);//remove导致itemcount时时变化，cautions
                    }
                }

            }


            pLegend.Title = "图 例";
            pLegend.AdjustColumns(legendCol);//yjl20110812
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast

            //Get aspect ratio
            ESRI.ArcGIS.Carto.IQuerySize querySize = mapSurroundFrame.MapSurround as ESRI.ArcGIS.Carto.IQuerySize; // Dynamic Cast
            System.Double w = 0;
            System.Double h = 0;
            querySize.QuerySize(ref w, ref h);
            System.Double aspectRatio = w / h;

            ESRI.ArcGIS.Geometry.IEnvelope envelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            envelope.PutCoords(posX, (posY - legW / aspectRatio), (posX + legW), posY);
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.Name = "图例";
            pep.AnchorPoint = esriAnchorPointEnum.esriTopLeftCorner;
            element.Activate((pageLayout as IActiveView).ScreenDisplay);//关键代码
            graphicsContainer.AddElement(element, 0);
            (graphicsContainer as IActiveView).Refresh();
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
    }

    /// <summary>
    /// 作者：yjl
    /// 日期：20110909
    /// 说明：地图制图常用函数类fromVB。net
    /// </summary>
    public static class GeoPageLayoutFn
    {
        
        //移动制图模板的元素
        public static void MoveElement(IElement inElement, double inOffX, double inOffY)
        {
            //if (inElement.Geometry.GeometryType == esriGeometryType.esriGeometryPoint)
            //{
            //    IPoint pPoint = inElement.Geometry as IPoint;
            //    pPoint.X += inOffX;
            //    pPoint.Y += inOffY;
            //    inElement.Geometry = pPoint;
            //}
            //else if (inElement.Geometry.GeometryType == esriGeometryType.esriGeometryLine)
            //{
            //    ILine pLine = inElement.Geometry as ILine;

            //    pLine.FromPoint.X += inOffX;
            //    pLine.FromPoint.Y += inOffY;
            //    pLine.ToPoint.X += inOffX;
            //    pLine.ToPoint.Y += inOffY;
            //}
            //else if (inElement.Geometry.GeometryType == esriGeometryType.esriGeometryPolyline)
            //{
                //IPointCollection pPC = inElement.Geometry as IPointCollection;
                //for (int i = 0; i < pPC.PointCount; i++)
                //{
                //    IPoint p = pPC.get_Point(i);
                //    p.X += inOffX;
                //    p.Y += inOffY;
                //    pPC.UpdatePoint(i, p);
                //}
                ITransform2D pT2D = inElement.Geometry as ITransform2D;
                pT2D.Move(inOffX, inOffY);
                inElement.Geometry = pT2D as IGeometry;
                
            //}
            //else
            //{
            //    IEnvelope pEnv = inElement.Geometry.Envelope;
            //    pEnv.Offset(inOffX, inOffY);
            //    inElement.Geometry = pEnv;
            //}
        }
        //打开制图模板获取模板元素
        public static void OpenTemplateElement(IPageLayoutControl inPLC)
        {
            IMapDocument pMD = new MapDocumentClass();
            pMD.Open(Application.StartupPath + @"\..\Template\TDLY10000.mxd", "");
            //首先删除原有的元素
            IGraphicsContainer pOriGC = inPLC.PageLayout as IGraphicsContainer;
            pOriGC.Reset();
            IElement pEle = pOriGC.Next();
            while (pEle != null)
            {
                if (!(pEle is IMapFrame))
                {
                    pOriGC.DeleteElement(pEle);
                    pOriGC.Reset();//删除之后要重置，否则删除不完

                }
                pEle = pOriGC.Next();
            }//while
            //添加制图模板的元素
            IGraphicsContainer pMbGC = pMD.PageLayout as IGraphicsContainer;
            pMbGC.Reset();
            pEle = pMbGC.Next();
            while (pEle != null)
            {
                if (pEle is ITextElement)
                {
                    pOriGC.AddElement(pEle, 0);

                }
                pEle = pMbGC.Next();
            }//while
        }
        //后台生成森林资源现状标准分幅图--对制图控件进行操作
        public static void QuietPageLayoutTDLYFFT(IPageLayoutControl axPageLayoutControl1, Dictionary<string, string> pgTextElements, int typeZT)
        {
            ISpatialReference pSpatialRefrence = axPageLayoutControl1.ActiveView.FocusMap.SpatialReference;
            GeoDrawSheetMap.clsDrawSheetMap pDrawSheetMap = new GeoDrawSheetMap.clsDrawSheetMap();
            pDrawSheetMap.vPageLayoutControl = axPageLayoutControl1 as IPageLayoutControl;
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
                    pgEle = pgGC.Next();
                }
            }

            IGraphicsContainerSelect pGCS = axPageLayoutControl1.PageLayout as IGraphicsContainerSelect;
            if (pGCS.ElementSelectionCount != 0)
            {
                pGCS.UnselectAllElements();

            }
        }

        /// <summary>
        /// 根据图幅号获得投影文件
        /// </summary>
        /// <param name="strMapNO"></param>
        /// <returns></returns>
        private static ISpatialReference GetSpatialByMapNO(string strMapNO)
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
                strtfh = "CGCS 2000 GK CM " + tfh.ToString() + "E.prj";
            }
            catch
            {

            }
            string strPrjFileName = Application.StartupPath + "\\..\\Prj\\CGCS2000\\" + strtfh;
            if (!System.IO.File.Exists(strPrjFileName)) return null;

            ISpatialReferenceFactory pSpaFac = new SpatialReferenceEnvironmentClass();
            return pSpaFac.CreateESRISpatialReferenceFromPRJFile(strPrjFileName);

        }
        //添加文字注记
        public static void drawTextElement(IPoint p, IGraphicsContainer pGra,string strEle)
        {
            ITextElement pTextElement = new TextElementClass();
            ITextSymbol pTextSymbol = new TextSymbolClass();
            stdole.StdFont mySedFont = new stdole.StdFontClass();
            mySedFont.Name = "黑体";
            mySedFont.Size = 10;
            mySedFont.Bold = true;
            
            IRgbColor color = new RgbColorClass();
            color.Blue = 0;
            color.Green = 0;
            color.Red = 255;
            pTextSymbol.Font = mySedFont as IFontDisp;
            pTextSymbol.Color = color;
            pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            pTextElement.Text = strEle;
            pTextElement.Symbol = pTextSymbol;
            (pTextElement as IElement).Geometry = (IGeometry)p;
            //(pTextElement as IElementProperties3).AnchorPoint = esriAnchorPointEnum.esriBottomMidPoint;
            //(pTextElement as IElementProperties3).AutoTransform = true;
            //(pTextElement as IElementProperties3).Name = "1";

            pGra.AddElement(pTextElement as IElement, 0);
           
        }

        //在mapcontrol上画多边形
        public static void drawPolygonElement(IGeometry pPolygon, IGraphicsContainer pGra)
        {
            if (pPolygon == null)
                return;
            //pPolygon.Project((pGra as IActiveView).FocusMap.SpatialReference);
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IFillShapeElement pPolygonElement = null;
            if (pPolygon.GeometryType == esriGeometryType.esriGeometryEnvelope)
                pPolygonElement = new RectangleElementClass();
            else if (pPolygon.GeometryType == esriGeometryType.esriGeometryPolygon)
                pPolygonElement = new PolygonElementClass();
            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 0;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 255;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 2;
                //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;
                pRGBColor.Transparency = 0;
                pFillSymbol.Color = pRGBColor;
                //pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                (pPolygonElement as IElement).Geometry = pPolygon;
                pPolygonElement.Symbol = pFillSymbol;
                pGra.AddElement(pPolygonElement as IElement, 0);



            }
            catch (Exception ex)
            {
                //MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pFillSymbol = null;
            }
        }
        //重载在mapcontrol上画多边形，需符号参数
        public static void drawPolygonElement(IGeometry pPolygon, IGraphicsContainer pGra, IRgbColor fillClr, bool transparency, IRgbColor inRgbColor, double inWidth)
        {
            if (pPolygon == null)
                return;
            //pPolygon.Project((pGra as IActiveView).FocusMap.SpatialReference);
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IFillShapeElement pPolygonElement = null;
            if (pPolygon.GeometryType == esriGeometryType.esriGeometryEnvelope)
                pPolygonElement = new RectangleElementClass();
            else if (pPolygon.GeometryType == esriGeometryType.esriGeometryPolygon)
                pPolygonElement = new PolygonElementClass();
            try
            {
                //颜色对象
                fillClr.UseWindowsDithering = false;
                inRgbColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                pLineSymbol.Color = inRgbColor;

                pLineSymbol.Width = inWidth;
                //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;
                if(transparency)
                    fillClr.Transparency = 0;
                pFillSymbol.Color = fillClr;
                //pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                (pPolygonElement as IElement).Geometry = pPolygon;
                pPolygonElement.Symbol = pFillSymbol;
                pGra.AddElement(pPolygonElement as IElement, 0);



            }
            catch (Exception ex)
            {
                //MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pFillSymbol = null;
            }
        }
        //在mapcontrol上画线
        public static void drawPolylineElement(IGeometry pPolyline, IGraphicsContainer pGra)
        {
            if (pPolyline == null)
                return;
            //pPolyline.Project((pGra as IMap).SpatialReference);
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            ILineElement pLineEle = new LineElementClass();
            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                pRGBColor.Red = 0;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 255;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 2;
                pLineEle.Symbol = pLineSymbol as ILineSymbol;
                (pLineEle as IElement).Geometry = pPolyline;
                pGra.AddElement(pLineEle as IElement, 0);



            }
            catch (Exception ex)
            {
                //MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pLineSymbol = null;
            }
        }
        //在mapcontrol上画线重载需要颜色和宽度参数
        public static void drawPolylineElement(IGeometry pPolyline, IGraphicsContainer pGra,IRgbColor inRgbColor,double inWidth)
        {
            if (pPolyline == null)
                return;
            //pPolyline.Project((pGra as IMap).SpatialReference);
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            ILineElement pLineEle = new LineElementClass();
            try
            {
                pLineSymbol.Color = inRgbColor;

                pLineSymbol.Width = inWidth;
                pLineEle.Symbol = pLineSymbol as ILineSymbol;
                (pLineEle as IElement).Geometry = pPolyline;
                pGra.AddElement(pLineEle as IElement, 0);



            }
            catch (Exception ex)
            {
                //MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pLineSymbol = null;
            }
        }
        //由经纬度得到图幅号---未经测试
        public static bool GetNewCodeFromCoordinate(out string tfh, double x, double y, long vScale)
        {
            tfh = "";
            string b;
            int l;
            int r;
            int c; string th; int YInt; double XPlus;
            double YPlus;
            YInt = Convert.ToInt32(y / 4 / 3600) + 1;
            b = Convert.ToString(YInt + 64);
            l = Convert.ToInt32(x / 6 / 3600) + 31; switch (vScale)
            {
                case 500000:
                    th = "B";
                    XPlus = 3 * 3600;
                    YPlus = 2 * 3600;
                    break;
                case 250000:
                    th = "C";
                    XPlus = 3 / 2 * 3600;
                    YPlus = 1 * 3600;
                    break;
                case 100000:
                    th = "D";
                    XPlus = 1 / 2 * 3600;
                    YPlus = 1 / 3 * 3600;
                    break;
                case 50000:
                    th = "E";
                    XPlus = 1 / 4 * 3600;
                    YPlus = 1 / 6 * 3600;
                    break;
                case 25000:
                    th = "F";
                    XPlus = 1 / 8 * 3600;
                    YPlus = 1 / 12 * 3600;
                    break;
                case 10000:
                    th = "G";
                    XPlus = 1 / 16 * 3600;
                    YPlus = 1 / 24 * 3600;
                    break;
                case 5000:
                    th = "H";
                    XPlus = 1 / 32 * 3600;
                    YPlus = 1 / 48 * 3600;
                    break;
                default:
                    return false;
            }
            r = Convert.ToInt32(4 / (YPlus / 3600) - Convert.ToInt32(((y / 3600) - Convert.ToInt32((y / 3600) / 4) * 4) / (YPlus / 3600)));
            c = Convert.ToInt32(((x / 3600) - Convert.ToInt32((x / 3600) / 6) * 6) / (XPlus / 3600)) + 1;
            tfh = b + l + th + String.Format("00#", r) + String.Format("00#", c);
            return true;
        }
        public static IMapGrid creategrid(bool is_zuoyou, IPageLayout inPageLayout)
        {
            IActiveView pActiveView = inPageLayout as IActiveView;
            IMapGrid myMapGrid;
            IMeasuredGrid myMeasuredGrid = new MeasuredGridClass();
            IProjectedGrid pProjectedGrid = (IProjectedGrid)myMeasuredGrid;
            pProjectedGrid.SpatialReference = pActiveView.FocusMap.SpatialReference;
            myMeasuredGrid.FixedOrigin = true;
            myMeasuredGrid.Units = esriUnits.esriKilometers;
            myMeasuredGrid.XIntervalSize = 1;
            myMeasuredGrid.XOrigin = 0;
            myMeasuredGrid.YIntervalSize = 1;
            myMeasuredGrid.YOrigin = 0;

            myMapGrid = (IMapGrid)myMeasuredGrid;


            IRgbColor rgbColor = new RgbColor();
            rgbColor.Red = 0;
            rgbColor.Green = 0;
            rgbColor.Blue = 0;
            IColor color = rgbColor as IColor;

            //网格线的符号样式
            ICartographicLineSymbol pLineSymbol;
            pLineSymbol = new CartographicLineSymbolClass();
            pLineSymbol.Cap = esriLineCapStyle.esriLCSButt;
            pLineSymbol.Width = 0;
            myMapGrid.LineSymbol = pLineSymbol;
            //刻度线符号样式
            pLineSymbol = new CartographicLineSymbolClass();
            pLineSymbol.Cap = esriLineCapStyle.esriLCSSquare;
            pLineSymbol.Join = esriLineJoinStyle.esriLJSBevel;
            pLineSymbol.Width = 0.2;
            pLineSymbol.Color = color;
            myMapGrid.TickMarkSymbol = null;
            myMapGrid.TickLineSymbol = pLineSymbol;
            myMapGrid.TickLength = 17;
            myMapGrid.SetTickVisibility(false, false, false, false);


            ISimpleMapGridBorder simpleMapGridBorder = new SimpleMapGridBorderClass();
            ISimpleLineSymbol simpleLineSymbol = new SimpleLineSymbolClass();
            simpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            simpleLineSymbol.Color = color;
            simpleLineSymbol.Width = 0.4;
            simpleMapGridBorder.LineSymbol = simpleLineSymbol as ILineSymbol;
            myMapGrid.Border = simpleMapGridBorder as IMapGridBorder;

            myMapGrid.Border = (IMapGridBorder)simpleMapGridBorder;

            //创建标签，并设置其属性
            IMixedFontGridLabel mGrdLab = new MixedFontGridLabelClass();
            IFormattedGridLabel myMapFormattedGridLabel = mGrdLab as IFormattedGridLabel;
            IGridLabel myGridLabel = (IGridLabel)myMapFormattedGridLabel;

            if (is_zuoyou)
            {
                mGrdLab.NumGroupedDigits = 1;
            }
            else
            {
                mGrdLab.NumGroupedDigits = 2;
            }
            IRgbColor glColor = new RgbColorClass();
            glColor.Red = 0;
            glColor.Green = 0;
            glColor.Blue = 0;
            mGrdLab.SecondaryColor = glColor;
            stdole.StdFont mySedFont = new stdole.StdFontClass();
            mySedFont.Name = "宋体";
            mySedFont.Size = 7;
            mGrdLab.SecondaryFont = (stdole.IFontDisp)mySedFont;
            stdole.StdFont myFont = new stdole.StdFontClass();
            myFont.Name = "宋体";
            myFont.Size = 10;
            myFont.Bold = true;
            myGridLabel.Font = (stdole.IFontDisp)myFont;

            IColor myColor = new RgbColorClass();
            myColor.RGB = Convert.ToInt32(0);
            myGridLabel.LabelOffset = 0;
            myGridLabel.Color = myColor;


            myGridLabel.set_LabelAlignment(esriGridAxisEnum.esriGridAxisLeft, true);
            myGridLabel.set_LabelAlignment(esriGridAxisEnum.esriGridAxisTop, true);
            myGridLabel.set_LabelAlignment(esriGridAxisEnum.esriGridAxisRight, true);
            myGridLabel.set_LabelAlignment(esriGridAxisEnum.esriGridAxisBottom, true);
            INumericFormat myNumericFormat = new NumericFormatClass();
            myNumericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignLeft;
            if (!is_zuoyou)
            {
                myNumericFormat.AlignmentOption = esriNumericAlignmentEnum.esriAlignRight;
                myNumericFormat.AlignmentWidth = 4;
            }
            myNumericFormat.RoundingOption = esriRoundingOptionEnum.esriRoundNumberOfDecimals;
            myNumericFormat.RoundingValue = 0;
            myNumericFormat.ShowPlusSign = false;
            myNumericFormat.UseSeparator = false;
            myMapFormattedGridLabel.Format = (INumberFormat)myNumericFormat;
            myMapGrid.LabelFormat = myGridLabel;
            if (is_zuoyou)
            {
                myMapGrid.SetLabelVisibility(true, false, true, false);
            }
            else
            {
                myMapGrid.SetLabelVisibility(false, true, false, true);
            }

            return myMapGrid;
        }
        //更改图层的唯一值渲染，只保留范围内的符号
        public static void updateLayerSymbol(ILayer inLayer, IGeometry inGeometry)
        {
            if (inLayer is IGroupLayer)//如果是组图层，递归
            {
                ICompositeLayer pCL = inLayer as ICompositeLayer;
                for (int i = 0; i < pCL.Count; i++)
                {
                    updateLayerSymbol(pCL.get_Layer(i), inGeometry);

                }
            }
            else
            {
                IFeatureLayer pFeaLyr = inLayer as IFeatureLayer;
                if (pFeaLyr == null)
                    return;
                IGeoFeatureLayer pGeoFeaLayer = pFeaLyr as IGeoFeatureLayer;
                IFeatureRenderer pFeaRenderer = pGeoFeaLayer.Renderer;
                IUniqueValueRenderer pUVRenderer = pFeaRenderer as IUniqueValueRenderer;
                if (pUVRenderer == null)
                    return;
                if (pUVRenderer.FieldCount != 1)
                    return;
                IFeatureClass pFeaCls = pGeoFeaLayer.FeatureClass;
                if (pFeaCls == null)
                    return;
                ISpatialFilter pSpaFlr = new SpatialFilterClass();
                pSpaFlr.Geometry = inGeometry;
                pSpaFlr.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                ICursor pCursor = pFeaCls.Search(pSpaFlr, false) as ICursor;
                IFeatureRenderer pNewFeaRenderer = new UniqueValueRendererClass();
                IUniqueValueRenderer pNewUVRenderer = pNewFeaRenderer as IUniqueValueRenderer;
                IDataStatistics pDataStatistics = new DataStatisticsClass();
                pDataStatistics.Cursor = pCursor;
                pDataStatistics.Field = pUVRenderer.get_Field(0);
                pNewUVRenderer.FieldCount = 1;
                pNewUVRenderer.set_Field(0, pUVRenderer.get_Field(0));
                System.Collections.IEnumerator pEnumerator = pDataStatistics.UniqueValues;
                while (pEnumerator.MoveNext())
                {
                    object obj = pEnumerator.Current;
                    string vle = obj.ToString();
                    for (int i = 0; i < pUVRenderer.ValueCount; i++)
                    {
                        string orivle = pUVRenderer.get_Value(i);
                        if (orivle.Contains(vle))
                        {
                            pNewUVRenderer.AddValue(orivle, pUVRenderer.get_Heading(orivle), pUVRenderer.get_Symbol(orivle));
                            pNewUVRenderer.set_Label(orivle, pUVRenderer.get_Label(orivle));
                        }
                    }
                    pNewUVRenderer.UseDefaultSymbol = false;//不显示其他值
                    pGeoFeaLayer.Renderer = pNewFeaRenderer;
                }
            }//else

        }
        //更改地图各图层的唯一值渲染，只保留范围内的符号
        public static void updateMapSymbol(IMap pMap, IGeometry inGeometry)
        {
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                updateLayerSymbol(pMap.get_Layer(i), inGeometry);

            }
 
        }
        //添加图例
        public static void AddLegendZT(ESRI.ArcGIS.Carto.IPageLayout pageLayout, ESRI.ArcGIS.Carto.IMap map, System.Double posX, System.Double posY, System.Double legW, int legendCol)
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
            envelope.PutCoords(posX - legW, posY, posX, posY + (legW / aspectRatio));
            mapSurroundFrame.Border = createBorder(envelope, pageLayout as IActiveView);//边框，diff
            ESRI.ArcGIS.Carto.IElement element = mapSurroundFrame as ESRI.ArcGIS.Carto.IElement; // Dynamic Cast
            element.Geometry = envelope;
            IElementProperties3 pep = element as IElementProperties3;
            pep.AnchorPoint = esriAnchorPointEnum.esriBottomRightCorner;
            pep.Name = "图例";
            element.Activate((pageLayout as IActiveView).ScreenDisplay);//关键代码
            graphicsContainer.AddElement(element, 0);
           
            
        }
        //生成border
        private static IBorder createBorder(IGeometry inGeometry, IActiveView inAV)
        {
            ISymbolBorder pSymbolBorder = new SymbolBorderClass();
            pSymbolBorder.GetGeometry(inAV.ScreenDisplay as IDisplay, inGeometry.Envelope);
            ILineSymbol pLSymbol = new SimpleLineSymbolClass();
            pLSymbol.Color = getRGB(0, 0, 0);
            pLSymbol.Width = 1;
            pSymbolBorder.LineSymbol = pLSymbol;


            return pSymbolBorder as IBorder;
        }
        //创建颜色
        private static IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pColor;
            pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }
        //获得模板制图对象
        //获取模板的制图对象
        public static  IPageLayout GetTemplateGra(string inPath)
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
        //构造晕线图层
         public static ILayer createHachureLyr(IFeatureClass inFC, string inFd, string inCode)
        {
            IFeatureLayer pFeaLyr = new FeatureLayerClass();
            pFeaLyr.FeatureClass = inFC;
            IFeatureRenderer pNewFeaRenderer = new UniqueValueRendererClass();
            IUniqueValueRenderer pNewUVRenderer = pNewFeaRenderer as IUniqueValueRenderer;
            pNewUVRenderer.FieldCount = 1;
            pNewUVRenderer.set_Field(0, inFd);
            ISymbol pSym = createFillSymbol(getRGB(0,0,0), true, getRGB(0, 0, 255), 2);
            pNewUVRenderer.AddValue(inCode, "", pSym);
            pNewUVRenderer.set_Label(inCode, "晕线");
            ISymbol pDftSym = createFillSymbol(getRGB(255, 255, 255), false, getRGB(255, 255, 255), 0);
            pNewUVRenderer.DefaultSymbol = pDftSym;
            pNewUVRenderer.DefaultLabel = "";
            pNewUVRenderer.UseDefaultSymbol = true;
            IGeoFeatureLayer pGFL = pFeaLyr as IGeoFeatureLayer;
            pGFL.Renderer = pNewFeaRenderer;
            pFeaLyr.Name = "晕线";
            return pFeaLyr as ILayer;
            
        }
        //创建简单面符号
         private static ISymbol createFillSymbol(IRgbColor fillClr, bool transparency, IRgbColor inRgbColor, double inWidth)
         {
             ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
             ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
             ISymbol pSymbol = (ISymbol)pFillSymbol;
             try
             {
                 //颜色对象
                 fillClr.UseWindowsDithering = false;
                 inRgbColor.UseWindowsDithering = false;
                 
                 //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
                 pLineSymbol.Color = inRgbColor;

                 pLineSymbol.Width = inWidth;
                 //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                 pFillSymbol.Outline = pLineSymbol;
                 if (transparency)
                     fillClr.NullColor = transparency;
                 pFillSymbol.Color = fillClr;
                 //pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;




             }
             catch (Exception ex)
             {
                 //MessageBox.Show("绘制范围出错:" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 pFillSymbol = null;
             }
             return pSymbol;
 
         }


    }
}
