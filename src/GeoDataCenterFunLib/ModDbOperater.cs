using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataCenterFunLib
{
    public static class ModDBOperator
    {
        public static string _FlashTagName = "query123";
        #region  实现闪烁效果的代码
        //闪烁要素
        public static void FlashFeature(IFeature pFeature, IActiveView pActiveView)
        {
            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            int interval = 150;
            if (pFeature.Shape == null) return;
            switch (pFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                    FlashLineAndHiLight(pActiveView, pFeature.Shape, interval);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    FlashPolygon(pActiveView, pFeature.Shape, interval);
                    break;
                case esriGeometryType.esriGeometryPoint:
                    FlashPoint(pActiveView, pFeature.Shape, interval);
                    break;
                default:
                    break;
            }
            pScreenDisplay.FinishDrawing();
        }

        //闪烁要素
        public static void FlashFeature(IGeometry pGeometry, IActiveView pActiveView, int interval)
        {
            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            if (pGeometry == null) return;
            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryLine:

                    FlashLineAndHiLight(pActiveView, pGeometry, interval);//yjl0729,add,闪烁线要素
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    FlashPolygon(pActiveView, pGeometry, interval);
                    break;
                case esriGeometryType.esriGeometryPoint:
                    FlashPoint(pActiveView, pGeometry, interval);
                    break;
                default:
                    break;
            }
            pScreenDisplay.FinishDrawing();
        }
        //闪烁图形，主要是指十字丝的闪烁Yjl0729,add
        public static void FlashGeometry(IGeometry pGeometry, IActiveView pActiveView, int interval)
        {
            IScreenDisplay pScreenDisplay = pActiveView.ScreenDisplay;
            pScreenDisplay.StartDrawing(pScreenDisplay.hDC, (short)esriScreenCache.esriNoScreenCache);
            if (pGeometry == null) return;
            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryLine:
                    FlashLine(pActiveView, pGeometry, interval);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    FlashPolygon(pActiveView, pGeometry, interval);
                    break;
                case esriGeometryType.esriGeometryPoint:
                    FlashPoint(pActiveView, pGeometry, interval);
                    break;
                default:
                    break;
            }
            pScreenDisplay.FinishDrawing();
        }

        public static void FlashFeature(IFeature pFeature, IMapControlDefault pMapControl)
        {
            IActiveView pActiveView = pMapControl.ActiveView;

            IEnvelope pEnvSmall = pFeature.Shape.Envelope;
            IEnvelope pEnvScreen = pActiveView.Extent;

            IPoint pPointCenter = new PointClass();
            pPointCenter.PutCoords((pEnvSmall.XMax + pEnvSmall.XMin) / 2, (pEnvSmall.YMax + pEnvSmall.YMin) / 2);

            IPoint pPointXMax = new PointClass();
            pPointXMax.PutCoords(pEnvScreen.XMax, pPointCenter.Y);


            IPoint pPointXMaxSmall = new PointClass();
            pPointXMaxSmall.PutCoords(pEnvSmall.XMax, pPointCenter.Y);


            IPoint pPointXMin = new PointClass();
            pPointXMin.PutCoords(pEnvScreen.XMin, pPointCenter.Y);
            IPoint pPointXMinSmall = new PointClass();
            pPointXMinSmall.PutCoords(pEnvSmall.XMin, pPointCenter.Y);

            IPoint pPointYMax = new PointClass();
            pPointYMax.PutCoords(pPointCenter.X, pEnvScreen.YMax);
            IPoint pPointYMaxSmall = new PointClass();
            pPointYMaxSmall.PutCoords(pPointCenter.Y, pEnvSmall.YMax);

            IPoint pPointYMin = new PointClass();
            pPointYMin.PutCoords(pPointCenter.X, pEnvScreen.YMin);
            IPoint pPointYMinSmall = new PointClass();
            pPointYMinSmall.PutCoords(pPointCenter.Y, pEnvSmall.YMin);

            for (int i = 0; i < 13; i++)
            {
                IPolyline pLineXMax = new PolylineClass();
                pLineXMax.FromPoint = pPointXMax;
                IPoint pPointTo = new PointClass();
                double xMax = pPointXMax.X + Convert.ToInt32((pPointXMaxSmall.X - pPointXMax.X) * i / 13);

                pPointTo.PutCoords(xMax, pPointCenter.Y);
                pLineXMax.ToPoint = pPointTo;

                IPolyline pLineXMin = new PolylineClass();
                pLineXMin.FromPoint = pPointXMin;
                double xMin = pPointXMin.X + (pPointXMinSmall.X - pPointXMin.X) * i / 13;

                pPointTo.PutCoords(xMin, pPointCenter.Y);
                pLineXMin.ToPoint = pPointTo;

                IPolyline pLineYMin = new PolylineClass();
                pLineYMin.FromPoint = pPointYMin;
                double yMin = pPointYMin.Y + (pPointYMinSmall.Y - pPointYMin.Y) * i / 13;
                pPointTo.PutCoords(pPointYMin.X, yMin);
                pLineYMin.ToPoint = pPointTo;

                IPolyline pLineYMax = new PolylineClass();
                pLineYMax.FromPoint = pPointYMax;
                double yMax = pPointYMax.Y + (pPointYMaxSmall.Y - pPointYMax.Y) * i / 13;
                pPointTo.PutCoords(pPointYMax.X, yMax);
                pLineYMax.ToPoint = pPointTo;

                IGeometryCollection pGeoColl = new PolylineClass();
                object obj = Type.Missing;

                IPath pPath = new PathClass();
                pPath.FromPoint = pLineXMax.FromPoint;
                pPath.ToPoint = pLineXMax.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                pPath = new PathClass();
                pPath.FromPoint = pLineXMin.FromPoint;
                pPath.ToPoint = pLineXMin.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                pPath = new PathClass();
                pPath.FromPoint = pLineYMax.FromPoint;
                pPath.ToPoint = pLineYMax.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                pPath = new PathClass();
                pPath.FromPoint = pLineYMin.FromPoint;
                pPath.ToPoint = pLineYMin.ToPoint;
                pGeoColl.AddGeometry(pPath, ref obj, ref obj);

                FlashGeometry(pGeoColl as IGeometry, pActiveView,5);
            }

            FlashFeature(pFeature.Shape, pActiveView, 100);
        }

        //闪烁线
        private static void FlashLine(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            tagPOINT tagPOINT = new tagPOINT();
            WKSPoint WKSPoint = new WKSPoint();

            tagPOINT.x = (int)1;
            tagPOINT.y = (int)1;
            pDisplay.DisplayTransformation.TransformCoords(ref WKSPoint, ref tagPOINT, 1, 6);

            pLineSymbol = new SimpleLineSymbolClass();
            //if (pActiveView.FocusMap.MapScale != 0)
            //{
            //    pLineSymbol.Width = WKSPoint.X * 10000 / pActiveView.FocusMap.MapScale;
            //}
            //else
            //{
            //    pLineSymbol.Width = WKSPoint.X / 2;
            //}
            pLineSymbol.Width=WKSPoint.X;

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;//148
            pRGBColor.Red = 32;//32
            pRGBColor.Blue = 0;

            pSymbol = pLineSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolyline(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPolyline(pGeometry);
        }
        //闪烁线并高亮显示
        private static void FlashLineAndHiLight(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            tagPOINT tagPOINT = new tagPOINT();
            WKSPoint WKSPoint = new WKSPoint();

            tagPOINT.x = (int)1;
            tagPOINT.y = (int)1;
            pDisplay.DisplayTransformation.TransformCoords(ref WKSPoint, ref tagPOINT, 1, 6);

            pLineSymbol = new SimpleLineSymbolClass();
            //if (pActiveView.FocusMap.MapScale != 0)
            //{
            //    pLineSymbol.Width = WKSPoint.X * 10000 / pActiveView.FocusMap.MapScale;
            //}
            //else
            //{
            //    pLineSymbol.Width = WKSPoint.X / 2;
            //}
            pLineSymbol.Width = WKSPoint.X;

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;//148
            pRGBColor.Red = 32;//32
            pRGBColor.Blue = 0;

            pSymbol = pLineSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolyline(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPolyline(pGeometry);
			pLineSymbol.Width = 3;
            IElement pEle = new LineElementClass();//yjl高亮显示
            pEle.Geometry = pGeometry;
            (pEle as ILineElement).Symbol = pSymbol as ILineSymbol;
            (pEle as IElementProperties).Name = _FlashTagName;
            (pActiveView as IGraphicsContainer).AddElement(pEle, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pEle, null);
            //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, pEle, null);
        }
        //闪烁多边形
        private static void FlashPolygon(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleFillSymbol pFillSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Outline = null;

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;
            pRGBColor.Red = 32;
            pRGBColor.Blue = 0;
            pFillSymbol.Color = pRGBColor as IColor ;//added by chulili 20110805
            pSymbol = pFillSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolygon(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPolygon(pGeometry);
            IElement pEle = new PolygonElementClass();//yjl高亮显示
            pEle.Geometry = pGeometry;
            (pEle as IFillShapeElement).Symbol = pSymbol as IFillSymbol;
            (pEle as IElementProperties).Name = _FlashTagName;
            (pActiveView as IGraphicsContainer).AddElement(pEle, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pEle, null);
            //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, pEle, null);
        }

        //闪烁点
        private static void FlashPoint(IActiveView pActiveView, IGeometry pGeometry, int interval)
        {
            IScreenDisplay pDisplay = pActiveView.ScreenDisplay;
            ISimpleMarkerSymbol pMarkerSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            pMarkerSymbol = new SimpleMarkerSymbolClass();
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;

            pRGBColor = new RgbColorClass();
            pRGBColor.Green = 148;
            pRGBColor.Red = 32;
            pRGBColor.Blue = 0;

            pSymbol = pMarkerSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPoint(pGeometry);
            System.Threading.Thread.Sleep(interval);
            pDisplay.DrawPoint(pGeometry);
            IElement pEle = new MarkerElementClass();//yjl高亮显示
            pEle.Geometry = pGeometry;
            (pEle as IMarkerElement).Symbol = pSymbol as ISimpleMarkerSymbol;
            (pEle as IElementProperties).Name = _FlashTagName;
            (pActiveView as IGraphicsContainer).AddElement(pEle, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, pEle, null);
            //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, pEle, null);
        }
        #endregion
    }
}
