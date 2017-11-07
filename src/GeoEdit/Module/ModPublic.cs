using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

namespace GeoEdit
{
    public static class ModPublic
    {
        #region 将MapControl上的图层按照工作空间进行存储
        public static Dictionary<IWorkspace, List<IFeatureLayer>> GetAllLayersFromMap(IMapControlDefault pMapControl)
        {
            if (pMapControl.Map.LayerCount == 0) return null;
            Dictionary<IWorkspace, List<IFeatureLayer>> dicValue = new Dictionary<IWorkspace, List<IFeatureLayer>>();
            List<IFeatureLayer> pListLays;

            UID pUID = new UIDClass();
            try
            {
                //矢量数据处理
                pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
                IEnumLayer pEnumLayer = pMapControl.Map.get_Layers(pUID, true);
                pEnumLayer.Reset();
                ILayer pLayer = pEnumLayer.Next();
                while (pLayer != null)
                {
                    IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                    IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                    if (!dicValue.ContainsKey(pDataset.Workspace))
                    {
                        pListLays = new List<IFeatureLayer>();
                        pListLays.Add(pFeatureLayer);
                        dicValue.Add(pDataset.Workspace, pListLays);
                    }
                    else
                    {
                        dicValue[pDataset.Workspace].Add(pFeatureLayer);
                    }
                    pLayer = pEnumLayer.Next();
                }
            }
            catch (Exception ex)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(ex);
                //******************************************
                return null;
            }


            return dicValue;
        }
        #endregion


        #region 选择编辑要素时用到的函数方法
        public static double ConvertPixelsToMapUnits(IActiveView pActiveView, int pixelUnits)
        {
            tagRECT deviceRECT = pActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
            int pixelExtent = deviceRECT.right - deviceRECT.left;
            double realWorldDisplayExtent = pActiveView.ScreenDisplay.DisplayTransformation.VisibleBounds.Width;
            double sizeOfOnePixel = realWorldDisplayExtent / pixelExtent;
            return pixelUnits * sizeOfOnePixel;
        }


        //选择要素
        public static void GetSelctionSet(IFeatureLayer pFeatureLayer, IGeometry pGeometry, IFeatureClass pFeatureClass, esriSelectionResultEnum pselecttype, bool bjustone)
        {
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.GeometryField = pFeatureClass.ShapeFieldName;

            switch (pFeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
                default:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
            }

            IFeatureSelection pFeaSelection = pFeatureLayer as IFeatureSelection;
            pFeaSelection.SelectFeatures(pSpatialFilter as IQueryFilter, pselecttype, bjustone);
            pFeaSelection.SelectionChanged();
        }


        /// <summary>
        /// 在要素上面绘制一个可拖拽的符号
        /// </summary>
        /// <param name="geometry"></param>
        public static void DrawEditSymbol(IGeometry geometry, IActiveView pActiveView)
        {
            IColor pColor = SysCommon.Gis.ModGisPub.GetRGBColor(38, 115, 0);
            ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
            pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSSquare;
            pSimpleMarkerSymbol.Color = pColor;
            pSimpleMarkerSymbol.Size = 2.0;

            pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, -1);
            pActiveView.ScreenDisplay.SetSymbol(pSimpleMarkerSymbol as ISymbol);
            IPointCollection pointCol = geometry as IPointCollection;
            for (int i = 0; i < pointCol.PointCount; i++)
            {
                pActiveView.ScreenDisplay.DrawPoint(pointCol.get_Point(i));
            }

            pActiveView.ScreenDisplay.FinishDrawing();
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphicSelection, null, pActiveView.Extent);
        }

        //判断鼠标是否在已选择的记录集上
        public static bool MouseOnSelection(IPoint pPnt, IActiveView pActiveView)
        {
            //设置点选择容差
            ISelectionEnvironment pSelectEnv = new SelectionEnvironmentClass();
            double Length = ConvertPixelsToMapUnits(pActiveView, pSelectEnv.SearchTolerance);

            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer = pActiveView.FocusMap.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                if (MouseOnSelection(pLayer as IFeatureLayer, pPnt, pActiveView, Length) == true)
                {
                    return true;
                }
                pLayer = pEnumLayer.Next();
            }

            return false;
        }
        private static bool MouseOnSelection(IFeatureLayer pFeatureLay, IPoint pPnt, IActiveView pActiveView, double Length)
        {
            IFeatureSelection pFeatureSelection = pFeatureLay as IFeatureSelection;
            if (pFeatureSelection == null) return false;
            ISelectionSet pSelectionSet = pFeatureSelection.SelectionSet;
            if (pSelectionSet.Count == 0) return false;
            ICursor pCursor = null;
            pSelectionSet.Search(null, false, out pCursor);
            IFeatureCursor pFeatureCursor = pCursor as IFeatureCursor;
            if (pFeatureCursor == null) return false;
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {
                IGeometry4 pGeom = pFeature.ShapeCopy as IGeometry4;
                //给pGeom一个确定的空间参考
                pGeom.Project(pActiveView.FocusMap.SpatialReference);
                if (pGeom.IsEmpty) continue;

                IProximityOperator pObj = pGeom as IProximityOperator;
                double dblDist = pObj.ReturnDistance(pPnt);
                if (dblDist < Length)
                {
                    return true;
                }

                pFeature = pFeatureCursor.NextFeature();
            }

            return false;
        }

        //判断鼠标是否在已选择要素的节点上
        public static bool MouseOnFeatureVertex(IPoint pPnt, IFeature pFeature, IActiveView pActiveView)
        {
            //设置点选择容差
            ISelectionEnvironment pSelectEnv = new SelectionEnvironmentClass();
            double Length = ConvertPixelsToMapUnits(pActiveView, pSelectEnv.SearchTolerance);

            IPointCollection pointCol = pFeature.Shape as IPointCollection;
            for (int i = 0; i < pointCol.PointCount; i++)
            {
                IGeometry4 pGeom = pointCol.get_Point(i) as IGeometry4;
                //给pGeom一个确定的空间参考
                pGeom.Project(pActiveView.FocusMap.SpatialReference);
                if (pGeom.IsEmpty) return false;

                IProximityOperator pObj = pGeom as IProximityOperator;
                double dblDist = pObj.ReturnDistance(pPnt);
                if (dblDist < Length)
                {
                    return true;
                }
            }

            return false;
        }

        //获取MAP中的所有开启编辑的图层
        public static List<ILayer> LoadAllEditLyr(IMap pMap)
        {
            List<ILayer> listLay = new List<ILayer>();
            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer = pMap.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
                if (pDataset == null)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }
                if (pDataset.Workspace == null)
                {
                    pLayer = pEnumLayer.Next();
                    continue;
                }
                IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit.IsBeingEdited())
                {
                    listLay.Add(pLayer);
                }

                pLayer = pEnumLayer.Next();
            }

            return listLay;
        }
        #endregion

        #region 实现捕捉功能

        private static IPointCollection m_PointCollection;                           //捕捉点集合
        private static Dictionary<IPointCollection, string> m_dicPointCollection;     //用于分类渲染
        private static IPoint m_LastSnapPnt;                                         //存储上次捕捉的点并在下次捕捉时重画以刷新界面
        //捕捉节点
        public static IPoint SnapPoint(IPoint pPnt, IActiveView pActiveView)
        {
            if (MoData.v_dicSnapLayers == null) return null;

            //鼠标点要位于当前的视图范围之内才进行捕捉
            if (pPnt.X < pActiveView.Extent.XMax && pPnt.X > pActiveView.Extent.XMin && pPnt.Y < pActiveView.Extent.YMax && pPnt.Y > pActiveView.Extent.YMin)
            {
                return SnapPnt(pPnt, pActiveView);
            }

            return null;
        }
        public static IPoint SnapPnt(IPoint pPnt, IActiveView pActiveView)
        {
            m_dicPointCollection = new Dictionary<IPointCollection, string>();
            m_PointCollection = new MultipointClass();

            //将查找的要素存储到FeatureCache
            double dCacheradius = ConvertPixelDistanceToMapDistance(pActiveView, MoData.v_CacheRadius);
            List<IFeature> listFeats = GetFeats(pActiveView, pPnt, dCacheradius);

            //直接利用地图距离得到捕捉半径
            double dSearchDist = ConvertPixelDistanceToMapDistance(pActiveView, MoData.v_SearchDist);
            //用于计算其它各点与鼠标点的距离,获取各捕捉点
            IProximityOperator pProximity = pPnt as IProximityOperator;
            foreach (KeyValuePair<ILayer, ArrayList> keyValue in MoData.v_dicSnapLayers)
            {
                bool bVertexPoint = Convert.ToBoolean(keyValue.Value[0]);       //节点捕捉
                bool bPortPoint = Convert.ToBoolean(keyValue.Value[1]);         //端点捕捉
                bool bIntersectPoint = Convert.ToBoolean(keyValue.Value[2]);    //相交点捕捉
                bool bMidPoint = Convert.ToBoolean(keyValue.Value[3]);          //中点捕捉
                bool bNearestPoint = Convert.ToBoolean(keyValue.Value[4]);      //最近点捕捉

                IFeatureLayer pFeatLay = keyValue.Key as IFeatureLayer;
                if (pFeatLay == null) continue;
                if (bVertexPoint)
                {
                    GetNodeCollection(listFeats, pFeatLay.FeatureClass, pProximity, dSearchDist);
                }

                if (bPortPoint)
                {
                    GetPortPntCollection(listFeats, pFeatLay.FeatureClass, pProximity, dSearchDist);
                }

                if (bIntersectPoint)
                {
                    GetIntersectPntCollection(listFeats, pFeatLay.FeatureClass, pProximity, dSearchDist);
                }

                if (bMidPoint)
                {
                    GetMidPntCollection(listFeats, pFeatLay.FeatureClass, pProximity, dSearchDist);
                }

                if (bNearestPoint)
                {
                    GetNearestPntCollection(listFeats, pFeatLay.FeatureClass, pPnt, dSearchDist);
                }
            }

            //找到距离鼠标点最近的点
            IPoint pClosedPnt = GetClosedPnt(m_PointCollection, dSearchDist, pProximity);
            if (pClosedPnt == null) return null;

            pPnt.PutCoords(pClosedPnt.X, pClosedPnt.Y);

            //存储上次捕捉的点并在下次捕捉时重画以刷新界面(否则MAPCONTROL上会画出多个捕捉点)
            if (m_LastSnapPnt != null)
            {

                DrawRectangle(pActiveView, m_LastSnapPnt, m_dicPointCollection);
            }
            DrawRectangle(pActiveView, pPnt, m_dicPointCollection);
            m_LastSnapPnt = pPnt;

            return pClosedPnt;
        }
        //从一个点集中获得距离鼠标最近的点
        private static IPoint GetClosedPnt(IPointCollection pPointCollection, double dSearchDist, IProximityOperator pProximity)
        {
            IPoint pClosedPnt = null;
            double dClosedDistance = dSearchDist;
            for (int i = 0; i < pPointCollection.PointCount; i++)
            {
                IPoint pPoint = pPointCollection.get_Point(i);
                double dScreenDist = pProximity.ReturnDistance(pPoint);
                if (dScreenDist < dClosedDistance)
                {
                    dClosedDistance = dScreenDist;
                    pClosedPnt = pPoint;
                }
            }

            return pClosedPnt;
        }

        //在MAPCONTROL上表现出捕捉点
        private static void DrawRectangle(IActiveView pActiveView, IPoint pPoint, Dictionary<IPointCollection, string> dicPointCollection)
        {
            pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            esriSimpleMarkerStyle pStyle = esriSimpleMarkerStyle.esriSMSCircle;
            foreach (KeyValuePair<IPointCollection, string> keyValue in dicPointCollection)
            {
                for (int i = 0; i < keyValue.Key.PointCount; i++)
                {
                    if (pPoint.X == keyValue.Key.get_Point(i).X && pPoint.Y == keyValue.Key.get_Point(i).Y)
                    {
                        switch (keyValue.Value)
                        {
                            case "PortPnt":
                                pStyle = esriSimpleMarkerStyle.esriSMSCircle;
                                break;
                            case "MidPnt":
                                pStyle = esriSimpleMarkerStyle.esriSMSDiamond;
                                break;
                            case "Node":
                                pStyle = esriSimpleMarkerStyle.esriSMSSquare;
                                break;
                            case "IntersectPnt":
                                pStyle = esriSimpleMarkerStyle.esriSMSX;
                                break;
                            case "NearestPnt":
                                pStyle = esriSimpleMarkerStyle.esriSMSCircle;
                                break;
                        }
                        break;
                    }
                }
            }

            pActiveView.ScreenDisplay.SetSymbol(SetSnapSymbol(pStyle) as ISymbol);
            pActiveView.ScreenDisplay.DrawPoint(pPoint);
            pActiveView.ScreenDisplay.FinishDrawing();
        }
        //设置画捕捉时候的符号
        private static ISimpleMarkerSymbol SetSnapSymbol(esriSimpleMarkerStyle pStyle)
        {
            ISimpleMarkerSymbol pMarkerSymbol = new SimpleMarkerSymbolClass();
            ISymbol pSymbol = pMarkerSymbol as ISymbol;

            IRgbColor pRgbColor = new RgbColorClass();
            pRgbColor.Transparency = 0;
            //采用异或方式绘制，擦除以前画的符号
            pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
            pMarkerSymbol.Color = pRgbColor;
            pMarkerSymbol.Style = pStyle;

            //设置轮廓线样式
            pRgbColor.Red = 255;
            pRgbColor.Blue = 0;
            pRgbColor.Green = 0;
            pRgbColor.Transparency = 255;
            pMarkerSymbol.Outline = true;
            pMarkerSymbol.OutlineColor = pRgbColor;
            pMarkerSymbol.OutlineSize = 1;
            pMarkerSymbol.Size = 4.0;

            return pMarkerSymbol;
        }

        //把像素(屏幕)距离转化成为地图上的距离
        public static double ConvertPixelDistanceToMapDistance(IActiveView pActiveView, double pPixelDistance)
        {
            tagPOINT tagPOINT = new tagPOINT();
            tagPOINT.x = Convert.ToInt32(pPixelDistance);
            tagPOINT.y = Convert.ToInt32(pPixelDistance);

            WKSPoint pWKSPoint = new WKSPoint();
            pActiveView.ScreenDisplay.DisplayTransformation.TransformCoords(ref pWKSPoint, ref tagPOINT, 1, 6);

            return pWKSPoint.X;
        }

        //获取查找到的要素
        private static List<IFeature> GetFeats(IActiveView pActiveView, IPoint pPnt, double dRadius)
        {
            List<IFeature> listFeats = new List<IFeature>();

            //生成缓冲区
            IGeometry pGeometry = pPnt as IGeometry;
            ITopologicalOperator pTop = pGeometry as ITopologicalOperator;
            pGeometry = pTop.Buffer(dRadius);

            UID pUID = new UIDClass();
            pUID.Value = "{40A9E885-5533-11d0-98BE-00805F7CED21}";   //UID for IFeatureLayer
            IEnumLayer pEnumLayer = pActiveView.FocusMap.get_Layers(pUID, true);
            pEnumLayer.Reset();
            ILayer pLay = pEnumLayer.Next();
            while (pLay != null)
            {
                if (MoData.v_dicSnapLayers.ContainsKey(pLay) && pLay.Visible == true)
                {
                    IFeatureLayer pFeatLay = pLay as IFeatureLayer;
                    ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                    pSpatialFilter.Geometry = pGeometry;
                    pSpatialFilter.GeometryField = "SHAPE";
                    switch (pFeatLay.FeatureClass.ShapeType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                            break;
                        default:
                            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                            break;
                    }
                    IFeatureCursor pFeatCur = pFeatLay.FeatureClass.Search(pSpatialFilter, false);
                    IFeature pFeat = pFeatCur.NextFeature();
                    while (pFeat != null)
                    {
                        listFeats.Add(pFeat);
                        pFeat = pFeatCur.NextFeature();
                    }
                    Marshal.ReleaseComObject(pFeatCur);
                }

                pLay = pEnumLayer.Next();
            }

            return listFeats;
        }

        // 端点捕捉
        private static void GetPortPntCollection(List<IFeature> listFeats, IFeatureClass pFeatureClass, IProximityOperator pProximity, double dSearchDist)
        {
            IPointCollection pPntColTemp = new MultipointClass();

            for (int i = 0; i < listFeats.Count; i++)
            {
                IFeature pFeature = listFeats[i];
                //判断该Feature图层
                if (pFeature.Class.ObjectClassID != pFeatureClass.ObjectClassID) continue;

                if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline || pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    double dScreenSearchDist = pProximity.ReturnDistance(pFeature.Shape);
                    if (dScreenSearchDist < 1.5 * dSearchDist)
                    {
                        IGeometryCollection pGeoCollection = pFeature.Shape as IGeometryCollection;
                        for (int j = 0; j < pGeoCollection.GeometryCount; j++)
                        {
                            IGeometry pGeom = pGeoCollection.get_Geometry(j);
                            IPointCollection pPntCol = pGeom as IPointCollection;
                            m_PointCollection.AddPointCollection(pPntCol);
                            pPntColTemp.AddPointCollection(pPntCol);
                        }
                    }
                }
            }

            m_dicPointCollection.Add(pPntColTemp, "PortPnt");
        }

        //中点捕捉
        private static void GetMidPntCollection(List<IFeature> listFeats, IFeatureClass pFeatureClass, IProximityOperator pProximity, double dSearchDist)
        {
            IPointCollection pPntColTemp = new MultipointClass();

            for (int i = 0; i < listFeats.Count; i++)
            {
                IFeature pFeature = listFeats[i];
                //判断该Feature图层
                if (pFeature.Class.ObjectClassID != pFeatureClass.ObjectClassID) continue;

                if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline || pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    double dScreenSearchDist = pProximity.ReturnDistance(pFeature.Shape);
                    if (dScreenSearchDist < 1.5 * dSearchDist)
                    {
                        IGeometryCollection pGeoCollection = pFeature.Shape as IGeometryCollection;
                        for (int j = 0; j < pGeoCollection.GeometryCount; j++)
                        {
                            IGeometry pGeom = pGeoCollection.get_Geometry(j);
                            ISegmentCollection pSegColl = pGeom as ISegmentCollection;

                            for (int k = 0; k < pSegColl.SegmentCount; k++)
                            {
                                ISegment pSeg = pSegColl.get_Segment(k);
                                IPoint pMidPoint = new PointClass();
                                double x = (pSeg.FromPoint.X + pSeg.ToPoint.X) / 2;
                                double y = (pSeg.FromPoint.Y + pSeg.ToPoint.Y) / 2;
                                pMidPoint.PutCoords(x, y);
                                object befor = Type.Missing;
                                object after = Type.Missing;
                                m_PointCollection.AddPoint(pMidPoint, ref befor, ref after);
                                pPntColTemp.AddPoint(pMidPoint, ref befor, ref after);

                            }
                        }
                    }
                }
            }

            m_dicPointCollection.Add(pPntColTemp, "MidPnt");
        }

        //节点捕捉
        private static void GetNodeCollection(List<IFeature> listFeats, IFeatureClass pFeatureClass, IProximityOperator pProximity, double dSearchDist)
        {
            IPointCollection pPntColTemp = new MultipointClass();

            for (int i = 0; i < listFeats.Count; i++)
            {
                IFeature pFeature = listFeats[i];
                //判断该Feature图层
                if (pFeature.Class.ObjectClassID != pFeatureClass.ObjectClassID) continue;

                double dScreenSearchDist = pProximity.ReturnDistance(pFeature.Shape);
                if (dScreenSearchDist < 1.5 * dSearchDist)
                {
                    if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        IPoint pPoint = pFeature.Shape as IPoint;
                        object befor = Type.Missing;
                        object after = Type.Missing;
                        m_PointCollection.AddPoint(pPoint, ref befor, ref after);
                        pPntColTemp.AddPoint(pPoint, ref befor, ref after);
                    }
                    else
                    {
                        IPointCollection pTempPtcln = pFeature.Shape as IPointCollection;
                        m_PointCollection.AddPointCollection(pTempPtcln);
                        pPntColTemp.AddPointCollection(pTempPtcln);
                    }
                }
            }

            m_dicPointCollection.Add(pPntColTemp, "Node");
        }

        //相交点捕捉
        private static void GetIntersectPntCollection(List<IFeature> listFeats, IFeatureClass pFeatureClass, IProximityOperator pProximity, double dSearchDist)
        {
            IPointCollection pPntColTemp = new MultipointClass();

            List<IFeature> listFeatsTemp = new List<IFeature>();

            for (int i = 0; i < listFeats.Count; i++)
            {
                IFeature pFeature = listFeats[i];
                if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline || pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    double dScreenSearchDist = pProximity.ReturnDistance(pFeature.Shape);
                    if (dScreenSearchDist < 1.5 * dSearchDist)
                    {
                        listFeatsTemp.Add(pFeature);
                    }
                }
            }

            //收集线两两相交点，收集的交点有重复，但是不影响结果
            foreach (IFeature pFeat in listFeatsTemp)
            {
                IPointCollection pPntCol = GetAllIntersect(pFeat, listFeatsTemp);
                m_PointCollection.AddPointCollection(pPntCol);
                pPntColTemp.AddPointCollection(pPntCol);
            }

            m_dicPointCollection.Add(pPntColTemp, "IntersectPnt");
        }
        //得到所有交点
        private static IPointCollection GetAllIntersect(IFeature pFeat, List<IFeature> listFeats)
        {
            IPointCollection pItersectCol = new MultipointClass();

            IPointCollection pPntColTemp = pFeat.Shape as IPointCollection;
            IPolyline pPolyline = pPntColTemp as IPolyline;

            foreach (IFeature pFeatTemp in listFeats)
            {
                IGeometry pGeometry = pFeat.Shape;
                if (!pFeat.Equals(pFeatTemp))
                {
                    IPointCollection pItersectColTemp = GetIntersection(pFeatTemp.Shape, pPolyline);
                    if (pItersectColTemp != null)
                    {
                        pItersectCol.AddPointCollection(pItersectColTemp);
                    }
                }
            }

            return pItersectCol;
        }
        private static IPointCollection GetIntersection(IGeometry pIntersect, IPolyline pPolyline)
        {
            if (pIntersect.SpatialReference.SpatialReferenceImpl != pPolyline.SpatialReference.SpatialReferenceImpl)
            {
                pPolyline.Project(pIntersect.SpatialReference);
            }

            ITopologicalOperator pTopoOp = pIntersect as ITopologicalOperator;
            pTopoOp.Simplify();
            IGeometry pGeomResult = pTopoOp.Intersect(pPolyline, esriGeometryDimension.esriGeometry0Dimension);
            if (pGeomResult == null) return null;
            IPointCollection pPointCollection = pGeomResult as IPointCollection;
            return pPointCollection;
        }

        //最近点捕捉
        private static void GetNearestPntCollection(List<IFeature> listFeats, IFeatureClass pFeatureClass, IPoint pPnt, double dSearchDist)
        {
            IPointCollection pPntColTemp = new MultipointClass();

            for (int i = 0; i < listFeats.Count; i++)
            {
                IFeature pFeature = listFeats[i];
                if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolyline || pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPolygon)
                {
                    IProximityOperator pProximity = pFeature.Shape as IProximityOperator;
                    IPoint pNearestPnt = pProximity.ReturnNearestPoint(pPnt, esriSegmentExtension.esriNoExtension);
                    object befor = Type.Missing;
                    object after = Type.Missing;
                    m_PointCollection.AddPoint(pNearestPnt, ref befor, ref after);
                    pPntColTemp.AddPoint(pNearestPnt, ref befor, ref after);
                }
            }

            m_dicPointCollection.Add(pPntColTemp, "NearestPnt");
        }
        #endregion



        #region 要素融合时操作核心日志记录表相关函数
        /// <summary>
        /// 从mapcontrol上获取图幅结合表,首先获得“范围”图层组
        /// </summary>
        /// <param name="pMapcontrol"></param>
        /// <param name="strGroupLayerName">GroupLayer名称</param>
        /// <param name="strLayerName">GroupLayer下的某个图层名称</param>
        /// <returns></returns>
        public static ILayer GetLayerOfGroupLayer(IMapControlDefault pMapcontrol, string strGroupLayerName, string strLayerName)
        {
            ILayer pLayer = null;
            IGroupLayer pGroupLayer = GetGroupLayer(pMapcontrol, strGroupLayerName);//获得范围图层组
            ICompositeLayer pCompositeLayer = pGroupLayer as ICompositeLayer;
            if (pCompositeLayer.Count == 0) return null;
            for (int i = 0; i < pCompositeLayer.Count; i++)
            {
                ILayer mLayer = pCompositeLayer.get_Layer(i);
                if (mLayer.Name == strLayerName)
                {
                    pLayer = mLayer;
                    break;
                }
            }
            return pLayer;//获得图幅结合表
        }

        /// <summary>
        /// 根据图层组名获取组图层
        /// </summary>
        /// <param name="pMapcontrol"></param>
        /// <param name="strName">图层组名称</param>
        /// <returns></returns>
        public static IGroupLayer GetGroupLayer(IMapControlDefault pMapcontrol, string strName)
        {
            IGroupLayer pGroupLayer = new GroupLayerClass();
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pLayer = pMapcontrol.Map.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    if (pLayer.Name == strName)
                    {
                        pGroupLayer = pLayer as IGroupLayer;
                        break;
                    }
                }
            }
            return pGroupLayer;
        }

        /// <summary>
        /// 从坐标字符串得到范围Polygon
        /// </summary>
        /// <param name="strCoor">坐标字符串,格式为X@Y,以逗号分割</param>
        /// <returns></returns>
        public static IPolygon GetPolygonByCol(string strCoor)
        {
            try
            {
                object after = Type.Missing;
                object before = Type.Missing;
                IPolygon polygon = new PolygonClass();
                IPointCollection pPointCol = (IPointCollection)polygon;
                string[] strTemp = strCoor.Split(',');
                for (int index = 0; index < strTemp.Length; index++)
                {
                    string CoorLine = strTemp[index];
                    string[] coors = CoorLine.Split('@');

                    double X = Convert.ToDouble(coors[0]);
                    double Y = Convert.ToDouble(coors[1]);

                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(X, Y);
                    pPointCol.AddPoint(pPoint, ref before, ref after);
                }

                polygon = (IPolygon)pPointCol;
                polygon.Close();

                if (IsValidateGeometry(polygon)) return polygon;
                return null;
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                return null;
            }
        }

        // 从范围Polygon得到对应的坐标字符串
        public static string GetColByPolygon(IPolygon polygon)
        {
            if (polygon == null) return "";
            IPointCollection pPointCol = (IPointCollection)polygon;

            try
            {
                StringBuilder sb = new StringBuilder();
                for (int index = 0; index < pPointCol.PointCount; index++)
                {
                    IPoint pPoint = pPointCol.get_Point(index);

                    string X = Convert.ToString(pPoint.X);
                    string Y = Convert.ToString(pPoint.Y);

                    if (sb.Length != 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(X + "@" + Y);
                }

                return sb.ToString();
            }
            catch (Exception eError)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
                //******************************************
                return "";
            }
        }

        // 检测一个几何体是否非法
        private static bool IsValidateGeometry(IGeometry pgeometry)
        {
            // 获取此Geometry的原始点数
            IPointCollection pOrgPointCol = (IPointCollection)pgeometry;

            // 获取此Geometry的原始Part数
            IGeometryCollection pOrgGeometryCol = (IGeometryCollection)pgeometry;

            // 对目标进行克隆和对应的处理
            IClone pClone = (IClone)pgeometry;
            IGeometry pGeometryTemp = (IPolygon)pClone.Clone();
            ITopologicalOperator pTopo = (ITopologicalOperator)pGeometryTemp;
            pTopo.Simplify();

            // 得到新的Geometry
            pGeometryTemp = (IPolygon)pTopo;

            // 获取新的Geometry的点数
            IPointCollection pObjPointCol = (IPointCollection)pGeometryTemp;

            // 获取新的Geometry的Part数
            IGeometryCollection pObjGeometryCol = (IGeometryCollection)pGeometryTemp;

            // 进行比较
            if (pOrgPointCol.PointCount != pObjPointCol.PointCount) return false;

            if (pOrgGeometryCol.GeometryCount != pObjGeometryCol.GeometryCount) return false;

            return true;
        }
        #endregion
    }
}
