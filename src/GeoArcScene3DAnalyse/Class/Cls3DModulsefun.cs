using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;

namespace GeoArcScene3DAnalyse
{

    public class Cls3DModulsefun
    {
        /// <summary>
        /// 创建RasterDataset文件   张琪    20110614
        /// </summary>
        /// <param name="sDir">保存路径</param>
        /// <param name="sName">文件名</param>
        /// <param name="sFormat"></param>
        /// <param name="pOrigin">点</param>
        /// <param name="nCol"></param>
        /// <param name="nRow"></param>
        /// <param name="cellsizeX"></param>
        /// <param name="cellsizeY"></param>
        /// <param name="ePixelType"></param>
        /// <param name="pSR"></param>
        /// <param name="bPerm"></param>
        /// <returns></returns>
        public IRasterDataset CreateRasterSurf(String sDir, String sName, String sFormat, ESRI.ArcGIS.Geometry.IPoint pOrigin, int nCol, int nRow, Double cellsizeX, Double cellsizeY, rstPixelType ePixelType, ISpatialReference2 pSR, bool bPerm)
        {
            IWorkspaceFactory prWksFac = new RasterWorkspaceFactoryClass();
            IWorkspace pWorkspace = prWksFac.OpenFromFile(sDir, 0);
            IRasterWorkspace2 pRasterWorkspace2 = pWorkspace as IRasterWorkspace2;
            int numbands = 1;
            IRasterDataset pRasterDataset = pRasterWorkspace2.CreateRasterDataset(sName, sFormat, pOrigin, nCol, nRow, cellsizeX, cellsizeY, numbands, ePixelType, pSR, bPerm);
            return pRasterDataset;
        }
        /// <summary>
        /// 获取栅格要数集的像素值    张琪    20110614
        /// </summary>
        /// <param name="pRasterDataset">栅格要数集</param>
        /// <param name="band">波段值</param>
        /// <returns></returns>
        public IRawPixels GetRawPixels(IRasterDataset pRasterDataset, int band)
        {
            try
            {
                IRasterBandCollection pRasterBandCollection = pRasterDataset as IRasterBandCollection;
                IRasterBand pRasterBnad = pRasterBandCollection.Item(band);
                IRawPixels pRawPixels = pRasterBnad as IRawPixels;
                return pRawPixels;
            }
            catch
            {
                return null;
            }
        }
        /// <summary> 
        /// 打开指定路径下的栅格数据      张琪    20110614
        /// </summary>
        /// <param name="sDir">栅格数据路径</param>
        /// <param name="sName">栅格数据名</param>
        /// <returns></returns>
        public IRasterDataset OpenRasterDataset(string sDir, string sName)
        {
            try
            {
                IWorkspaceFactory pWorkspFcefactory = new RasterWorkspaceFactoryClass();
                IRasterWorkspace pRasterWorkspace = pWorkspFcefactory.OpenFromFile(sDir, 0) as IRasterWorkspace;
                IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(sName);
                return pRasterDataset;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// TIN数据转栅格数据并进行坡度分析      张琪    20110614
        /// </summary>
        /// <param name="pTinAdvanced"></param>
        /// <param name="pRastConvType"></param>
        /// <param name="sDir"></param>
        /// <param name="sName"></param>
        /// <param name="ePixelType"></param>
        /// <param name="cellsize"></param>
        /// <param name="pExtent"></param>
        /// <param name="bPerm"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public IRasterDataset TinToRaster(ITinAdvanced pTinAdvanced, esriRasterizationType pRastConvType, String sDir, String sName, rstPixelType ePixelType, Double cellsize, IEnvelope pExtent, bool bPerm, String strType)
        {
            try
            {
                ESRI.ArcGIS.Geometry.IPoint pOrigin = pExtent.LowerLeft;
                pOrigin.X = pOrigin.X - (cellsize * 0.5);
                pOrigin.Y = pOrigin.Y - (cellsize * 0.5);
                int nCol, nRow;
                nCol = Convert.ToInt32(Math.Round(pExtent.Width / cellsize)) + 1;
                nRow = Convert.ToInt32(Math.Round(pExtent.Height / cellsize)) + 1;
                IGeoDataset pGeoDataset = pTinAdvanced as IGeoDataset;
                ISpatialReference2 pSpatialReference2 = pGeoDataset.SpatialReference as ISpatialReference2;
                IRasterDataset pRasterDataset = CreateRasterSurf(sDir, sName, strType, pOrigin, nCol, nRow, cellsize, cellsize, ePixelType, pSpatialReference2, bPerm);
                IRawPixels pRawPixels = GetRawPixels(pRasterDataset, 0);
                object pCache = pRawPixels.AcquireCache();
                ITinSurface pTinSurface = pTinAdvanced as ITinSurface;
                IGeoDatabaseBridge2 pbridge2 = (IGeoDatabaseBridge2)new GeoDatabaseHelperClass();

                IRasterProps pRasterProps = pRawPixels as IRasterProps;
                //float nodataFloat;
                //int nodataInt;
                double dZMin = pTinAdvanced.Extent.ZMin;
                object vNoData;
                if (ePixelType.ToString() == "PT_FLOAT")
                {
                    vNoData = (dZMin - 1).ToString();
                }
                else
                {
                    vNoData = Convert.ToInt32((dZMin - 1));
                }
                pRasterProps.NoDataValue = vNoData;
                IPnt pOffset = new DblPntClass();
                int lMaxBlockX = 2048;
                if (nCol < lMaxBlockX)
                {
                    lMaxBlockX = nCol;
                }
                int lMaxBlockY = 2048;
                if (nRow < lMaxBlockY)
                {
                    lMaxBlockY = nRow;
                }
                IPnt pBlockSize = new DblPntClass();
                pBlockSize.X = lMaxBlockX;
                pBlockSize.Y = lMaxBlockY;
                IPixelBlock3 pPixelBlock = pRawPixels.CreatePixelBlock(pBlockSize) as IPixelBlock3;
                object blockArray = pPixelBlock.get_PixelDataByRef(0);
                ITrackCancel pCancel = new CancelTrackerClass();
                pCancel.CancelOnClick = false;
                pCancel.CancelOnKeyPress = true;
                int lBlockCount = Convert.ToInt32(Math.Round((nCol / lMaxBlockX) + 0.49) * Math.Round((nRow / lMaxBlockY) + 0.49));
                ESRI.ArcGIS.Geometry.IPoint pBlockOrigin = new ESRI.ArcGIS.Geometry.PointClass();
                int lColOffset, lRowOffset;

                for (lRowOffset = 0; lRowOffset < (nRow - 1); )
                {
                    for (lColOffset = 0; lColOffset < (nCol - 1); )
                    {
                        if ((nCol - lColOffset) < lMaxBlockX)
                        {
                            pBlockSize.X = (nCol - lColOffset);
                            pPixelBlock = pRawPixels.CreatePixelBlock(pBlockSize) as IPixelBlock3;
                            blockArray = pPixelBlock.get_PixelDataByRef(0);
                        }
                        pBlockOrigin.X = pOrigin.X + (lColOffset * cellsize) + (cellsize * 0.5);
                        pBlockOrigin.Y = pOrigin.Y + ((nRow - lRowOffset) * cellsize) - (cellsize * 0.5);
                        pbridge2.QueryPixelBlock(pTinSurface, pBlockOrigin.X, pBlockOrigin.Y, cellsize, cellsize, pRastConvType, vNoData, ref blockArray);
                        //pTinSurface.QueryPixelBlock(pBlockOrigin.X, pBlockOrigin.Y, cellsize, cellsize, pRastConvType, vNoData, blockArray);
                        pOffset.X = lColOffset;
                        pOffset.Y = lRowOffset;
                        pPixelBlock.set_PixelData(0, (System.Object)blockArray);
                        pRawPixels.Write(pOffset, pPixelBlock as IPixelBlock);
                        if (lBlockCount > 1)
                        {
                            if (!pCancel.Continue())
                            {
                                break;
                            }
                            else if (pTinAdvanced.ProcessCancelled)
                            {
                                break;
                            }
                        }
                        lColOffset = lColOffset + lMaxBlockX;
                    }
                    bool bReset = false;
                    if (pBlockSize.X != lMaxBlockX)
                    {
                        pBlockSize.X = lMaxBlockX;
                        bReset = true;
                    }
                    if ((nRow - lRowOffset) < lMaxBlockY)
                    {
                        pBlockSize.Y = (nRow - lRowOffset);
                        bReset = true;
                    }
                    if (bReset)
                    {
                        pPixelBlock.set_PixelData(0, blockArray);
                        pPixelBlock = pRawPixels.CreatePixelBlock(pBlockSize) as IPixelBlock3;
                        blockArray = pPixelBlock.get_PixelDataByRef(0);
                    }
                    lRowOffset = lRowOffset + lMaxBlockY;
                }

                pRawPixels.ReturnCache(pCache);
                pCache = null;
                pRawPixels = null;
                pPixelBlock = null;
                pRasterProps = null;
                blockArray = 0;
                pRasterDataset = OpenRasterDataset(sDir, sName);
                if (lBlockCount == 1)
                {
                    pTinAdvanced.TrackCancel = null;
                }
                return pRasterDataset;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据输入的要素在SceneControl中绘制元素     张琪   20110621
        /// </summary>
        /// <param name="pSceneControl"></param>
        /// <param name="pGeom">几何要素</param>
        /// <param name="pSym"></param>
        public void AddGraphic(ISceneControl pSceneControl, IGeometry pGeom, ISymbol pSym)
        {
            if (pGeom == null)
            {
                return;
            }
            IElement pElement = null;
            switch (pGeom.GeometryType.ToString())
            {
                case "esriGeometryPoint"://点要素
                    pElement = new MarkerElementClass();
                    IMarkerElement pPointElement = pElement as IMarkerElement;
                    if (pSym != null)
                    {
                        IMarkerSymbol pMarker3DSymbol = pSym as IMarkerSymbol;
                        pPointElement.Symbol = pMarker3DSymbol as IMarkerSymbol;
                    }
                    break;
                case "esriGeometryPolyline"://线要素
                    pElement = new LineElementClass();
                    ILineElement pLineElement = pElement as ILineElement;
                    if (pSym != null)
                    {
                        ILineSymbol pLineSymbol = pSym as ILineSymbol;
                        pLineElement.Symbol = pLineSymbol;
                    }
                    break;
                case "esriGeometryPolygon"://面要素
                    pElement = new PolygonElementClass();
                    IFillShapeElement pFillElement = pElement as IFillShapeElement;
                    if (pSym != null)
                    {
                        IFillSymbol pFillSymbol = pSym as IFillSymbol;
                        pFillElement.Symbol = pFillSymbol;
                    }
                    break;
                case "esriGeometryMultiPatch"://多面体要素
                    pElement = new MultiPatchElementClass();
                    IFillShapeElement pMultiPatchElement = pElement as IFillShapeElement;
                    if (pSym != null)
                    {
                        IFillSymbol pFillSymbol = pSym as IFillSymbol;
                        pMultiPatchElement.Symbol = pFillSymbol as IFillSymbol;
                    }
                    break;
            }
            pElement.Geometry = pGeom;
            IGraphicsContainer3D pGCon3D = pSceneControl.Scene.BasicGraphicsLayer as IGraphicsContainer3D;
            pGCon3D.AddElement(pElement);//在SceneControl中绘制要素
            IGraphicsSelection pGS = pGCon3D as IGraphicsSelection;
            pSceneControl.Scene.SceneGraph.RefreshViewers();

        }
        /// <summary>
        /// 生成可视与不可视域的多面体    张琪    20110621
        /// </summary>
        /// <param name="bIsVis">是否可视</param>
        /// <param name="pObsPt">观察点</param>
        /// <param name="pTarPt">目标点</param>
        /// <param name="pVisLine">可视线要素</param>
        /// <param name="pInVisLine">不可视线要素</param>
        /// <param name="pVisPatch">可视多面体</param>
        /// <param name="pInVisPatch">不可视多面体</param>
        /// <param name="dTargetHeight"></param>
        public void CreateVerticalLOSPatches(bool bIsVis, ESRI.ArcGIS.Geometry.IPoint pObsPt, ESRI.ArcGIS.Geometry.IPoint pTarPt, IPolyline pVisLine, IPolyline pInVisLine, IGeometryCollection pVisPatch, IGeometryCollection pInVisPatch, double dTargetHeight)
        {
            IGeometryCollection pGeomColl = pVisLine as IGeometryCollection;//存储可视域线要素
            IMultiPatch pVisMPatch = pVisPatch as IMultiPatch;
            IMultiPatch pInVisMPatch = pInVisPatch as IMultiPatch;//生成不可视域要素
            dTargetHeight = pTarPt.Z;
            double dist1 = 0;
            double dist2;
            IPointCollection pPc;
            IClone pClone;
            ESRI.ArcGIS.Geometry.IPoint pLastVisPoint = null;
            IPointCollection pVisFan = new TriangleFanClass();//用于存储可视域多面体要素
            object before = Type.Missing;
            object after = Type.Missing;
            for (int i = 0; i < pGeomColl.GeometryCount; i++)//遍历可视域线要素
            {
                pPc = pGeomColl.get_Geometry(i) as IPointCollection;
                if (i == 0)//当为第一个可视域线要素是先要存储观察点要素
                {
                    pClone = pObsPt as IClone;
                    pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                    pClone = pPc.get_Point(0) as IClone;
                    pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                    ESRI.ArcGIS.Geometry.IPoint pStartPoint = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;

                }
                pClone = pPc as IClone;
                pVisFan.AddPointCollection(pClone.Clone() as IPointCollection);//将可视域线要素的点集合存储于pVisFan中

                if (i == pGeomColl.GeometryCount - 1)//当为可视域最后一个线要素时
                {
                    IVector3D pV = new Vector3DClass();
                    ESRI.ArcGIS.Geometry.IPoint p1;
                    pClone = pObsPt as IClone;
                    p1 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                    p1.Z = 0;
                    ESRI.ArcGIS.Geometry.IPoint p2;
                    pClone = pPc.get_Point(pPc.PointCount - 1) as IClone;
                    p2 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                    p2.Z = 0;
                    pV.ConstructDifference(p1, p2);
                    dist1 = pV.Magnitude;
                    pLastVisPoint = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                    if (pInVisLine == null)
                    {
                        if (pTarPt.Z > pPc.get_Point(pPc.PointCount - 1).Z)//当被观察点高程高于最后一点要素时则到被观点都是可视的
                        {
                            pClone = pTarPt as IClone;
                            pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);

                        }
                    }

                }

                pVisPatch.AddGeometry(pVisFan as IGeometry, ref before, ref after);//根据获得的点要素集生成TriangleFanClass
            }
            if (pInVisLine != null)//当不可视域的线要素不为空时
            {
                pGeomColl = pInVisLine as IGeometryCollection;
                IPointCollection pInVisRing = new RingClass();//用于存储不可视域点要素集并生成RingClass
                for (int i = 0; i < pGeomColl.GeometryCount; i++)
                {
                    pPc = pGeomColl.get_Geometry(i) as IPointCollection;
                    pClone = pPc.get_Point(0) as IClone;
                    pInVisRing.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                    pClone = pPc as IClone;
                    pInVisRing.AddPointCollection(pClone.Clone() as IPointCollection);
                    if (i == pGeomColl.GeometryCount - 1)
                    {
                        IVector3D pV = new Vector3DClass();
                        pClone = pObsPt as IClone;
                        ESRI.ArcGIS.Geometry.IPoint p1 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                        p1.Z = 0;
                        pClone = pPc.get_Point(pPc.PointCount - 1) as IClone;
                        ESRI.ArcGIS.Geometry.IPoint p2 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                        p2.Z = 0;
                        pV.ConstructDifference(p1, p2);
                        dist2 = pV.Magnitude;
                        if (dist1 < dist2)
                        {
                            pClone = pObsPt as IClone;
                            p1 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                            p1.Z = 0;
                            pClone = pPc.get_Point(0) as IClone;
                            p2 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                            p2.Z = 0;
                            pV.ConstructDifference(p1, p2);
                            double theDist1;
                            theDist1 = pV.Magnitude;
                            double slope = (pObsPt.Z - pPc.get_Point(0).Z) / theDist1;
                            pClone = pPc.get_Point(pPc.PointCount - 1) as IClone;
                            ESRI.ArcGIS.Geometry.IPoint pEndPoint = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                            p2 = pClone.Clone() as ESRI.ArcGIS.Geometry.IPoint;
                            p2.Z = 0;
                            pV.ConstructDifference(p1, p2);
                            double theDist2 = pV.Magnitude;
                            double deltaZ = theDist2 * slope;
                            double theHeight = pObsPt.Z - deltaZ;
                            pEndPoint.Z = theHeight;
                            pClone = pEndPoint as IClone;
                            pInVisRing.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                            if (bIsVis)//为True时说明不可视域线要素空间范围内存在可视区域
                            {
                                pVisFan = new TriangleFanClass();
                                pClone = pObsPt as IClone;
                                pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                                pClone = pTarPt as IClone;
                                pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                                pVisPatch.AddGeometry(pVisFan as IGeometry, ref before, ref after);
                            }
                            else
                            {
                                dTargetHeight = pEndPoint.Z;
                            }
                        }
                        else
                        {
                            if (bIsVis)
                            {
                                if (pTarPt.Z > pLastVisPoint.Z)
                                {
                                    pVisFan = new TriangleFanClass();
                                    pClone = pObsPt as IClone;
                                    pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                                    pClone = pTarPt as IClone;
                                    pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                                    pClone = pLastVisPoint as IClone;
                                    pVisFan.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                                    pVisPatch.AddGeometry(pVisFan as IGeometry, ref before, ref after);
                                }
                            }
                        }
                    }
                    pClone = pPc.get_Point(0) as IClone;
                    pInVisRing.AddPoint(pClone.Clone() as IPoint, ref before, ref after);
                    pInVisPatch.AddGeometry(pInVisRing as IGeometry, ref before, ref after);//获取每段不可视域线要素点集合并生成RingClass
                    pInVisMPatch.PutRingType(pInVisRing as IRing, esriMultiPatchRingType.esriMultiPatchRing);
                }
            }
        }
        public IRasterDataset TinToRaster2(ITinAdvanced pTinAdvanced, esriRasterizationType pRastConvType, String sDir, String sName, rstPixelType ePixelType, Double cellsize, IEnvelope pExtent, bool bPerm, String strType)
        {
            try
            {
                ESRI.ArcGIS.Geometry.IPoint pOrigin = pExtent.LowerLeft;
                pOrigin.X = pOrigin.X - (cellsize * 0.5);
                pOrigin.Y = pOrigin.Y - (cellsize * 0.5);
                int nCol, nRow;
                nCol = Convert.ToInt32(Math.Round(pExtent.Width / cellsize)) + 1;
                nRow = Convert.ToInt32(Math.Round(pExtent.Height / cellsize)) + 1;
                IGeoDataset pGeoDataset = pTinAdvanced as IGeoDataset;
                ISpatialReference2 pSpatialReference2 = pGeoDataset.SpatialReference as ISpatialReference2;
                IRasterDataset pRasterDataset = CreateRasterSurf(sDir, sName, strType, pOrigin, nCol, nRow, cellsize, cellsize, ePixelType, pSpatialReference2, bPerm);
                IRawPixels pRawPixels = GetRawPixels(pRasterDataset, 0);
                IPnt pBlockSize = new DblPntClass();
                pBlockSize.X = nCol;
                pBlockSize.Y = nRow;
                IPixelBlock3 pPixelBlock = pRawPixels.CreatePixelBlock(pBlockSize) as IPixelBlock3;
                object blockArray = pPixelBlock.get_PixelDataByRef(0);
                ITinSurface pTinSurface = pTinAdvanced as ITinSurface;
                IGeoDatabaseBridge2 pbridge2 = (IGeoDatabaseBridge2)new GeoDatabaseHelperClass();

                IRasterProps pRasterProps = pRawPixels as IRasterProps;
                object nodataFloat;
                object nodataInt;
                pOrigin.X = pOrigin.X + (cellsize * 0.5);
                pOrigin.Y = pOrigin.Y + (cellsize * nRow) - (cellsize * 0.5);
            
                if (ePixelType.ToString() == "PT_FLOAT")
                {
                    nodataFloat = pRasterProps.NoDataValue;
                    pTinSurface.QueryPixelBlock(pOrigin.X, pOrigin.Y, cellsize, cellsize, pRastConvType, nodataFloat, blockArray);
                }
                else
                {
                    nodataInt = pRasterProps.NoDataValue;
                    pTinSurface.QueryPixelBlock(pOrigin.X, pOrigin.Y, cellsize, cellsize, pRastConvType, nodataInt, blockArray);
                }

                if (pTinAdvanced.ProcessCancelled == false)
                {
                    return null;
                }
                IPnt pOffset = new DblPntClass();
                pOffset.X = 0;
                pOffset.Y = 0;
                pRawPixels.Write(pOffset, pPixelBlock  as IPixelBlock);
                if (!bPerm && ePixelType.ToString() == "PT_FLOAT")
                {
                    IRasterBand pBand = pRawPixels as IRasterBand;
                    IRasterStatistics pStats = pBand.Statistics;
                    pStats.Recalculate();
                }
                if (bPerm)
                {
                    pRawPixels = null;
                    pPixelBlock = null;
                    pRasterProps = null;
                    blockArray = 0;
                    pRasterDataset = OpenRasterDataset(sDir, sName);
                }
                return pRasterDataset;


            }      
            
            catch
            {
                return null;
            }
        }
 
    }
}
