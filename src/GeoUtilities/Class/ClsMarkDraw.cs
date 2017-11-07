using System;
using System.Collections.Generic;
using System.Text;


using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesRaster;
//ZQ    20110808   add 
namespace GeoUtilities
{
    public class ClsMarkDraw
    {

        /// <summary>
        /// 设置RGB函数   20110808
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns></returns>
        public static IRgbColor getRGB(int r, int g, int b)
        {
            IRgbColor pRgbColor = new RgbColorClass();
            pRgbColor.Red = r;
            pRgbColor.Green = g;
            pRgbColor.Blue = b;
            return pRgbColor;
        }
        /// <summary>
        /// 判断两点要素是否在同一位置    张琪 201108208
        /// </summary>
        /// <param name="Point1"></param>
        /// <param name="Point2"></param>
        /// <returns></returns>
        public static bool PointIsSame(ESRI.ArcGIS.Geometry.IPoint Point1, ESRI.ArcGIS.Geometry.IPoint Point2)
        {
            bool TheSame = true;
            if (Point1.X != Point2.X)
            {
                TheSame = false;
            }
            if (Point1.Y != Point2.Y)
            {
                TheSame = false;
            }
            if (Point1.Z != Point2.Z)
            {
                TheSame = false;
            }
            return TheSame;

        }
        /// <summary>
        /// 删除指定名称的  Element  张琪    20110628
        /// </summary>
        /// <param name="pScene"></param>
        /// <param name="sName"></param>
        public static void DeleteAllElementsWithName(IMap pMap, string sName)
        {
            try
            {
                IElement pElement;
                IGraphicsContainer pGCon = pMap.BasicGraphicsLayer as IGraphicsContainer;
                pGCon.Reset();
                pElement = pGCon.Next() as IElement;
                while (pElement != null)
                {
                    IElementProperties pElemProps = pElement as IElementProperties;
                    if (pElemProps.Name == sName)
                    {
                        pGCon.DeleteElement(pElement);
                        pGCon.Reset();
                    }
                    pElement = pGCon.Next() as IElement;
                }
            }
            catch { }
        }
        /// <summary>
        /// 绘制对象 张琪  20110628
        /// </summary>
        /// <param name="pGeometry">对象</param>
        /// <param name="pColor">颜色</param>
        /// <param name="lSize">绘制的大小</param>
        /// <param name="sName">名称</param>
        /// <param name="pScene"></param>
        /// <param name="pGroup"></param>
        public static void AddSimpleGraphic(IGeometry pGeometry, IRgbColor pColor, int lSize, string sName, IMap pMap, IGroupElement pGroup)
        {
            if (pGeometry == null)
            {
                return;
            }
            IElement pElement = null;
            //ISymbol pSym;
            //根据绘制的对象类型对IElement进行不同的定义
            switch (pGeometry.GeometryType.ToString())
            {
                case "esriGeometryPoint":
                    pElement = new MarkerElementClass();
                    IMarkerElement pPointElement = pElement as IMarkerElement;
                    ISimpleMarkerSymbol pMSym = new SimpleMarkerSymbolClass();
                    pMSym.Color = pColor;
                    pMSym.Size = lSize;
                    pMSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    IMarkerSymbol pMarkerSymbol = pMSym as IMarkerSymbol;
                    pPointElement.Symbol = pMarkerSymbol;
                    break;
                case "esriGeometryPolyline":
                    pElement = new LineElementClass();
                    ILineElement pLineElement = pElement as ILineElement;
                    ISimpleLineSymbol pLSym = new SimpleLineSymbolClass();
                    pLSym.Width = lSize;
                    pLSym.Style = esriSimpleLineStyle.esriSLSSolid;
                    pLSym.Color = pColor;
                    ILineSymbol pLineSymbol = pLSym as ILineSymbol;
                    pLineElement.Symbol = pLineSymbol;
                    break;
                case "esriGeometryPolygon":
                    ISimpleLineSymbol pOutlineSym = new SimpleLineSymbolClass();
                    pOutlineSym.Color = pColor;
                    pOutlineSym.Width = lSize;
                    pOutlineSym.Style = esriSimpleLineStyle.esriSLSSolid;
                    if (sName == "_ReferancePlane_")
                    {
                        pColor.Transparency = Convert.ToByte(Convert.ToInt32(255 / 2));
                    }
                    pElement = new PolygonElementClass();
                    IFillShapeElement pPolygonElement = pElement as IFillShapeElement;
                    IFillSymbol pFSym = new SimpleFillSymbolClass();
                    pFSym.Color = pColor;
                    ILineSymbol pLineSymbol1 = pOutlineSym as ILineSymbol;
                    pFSym.Outline = pLineSymbol1;
                    pPolygonElement.Symbol = pFSym;
                    break;
                default:
                    break;
            }
            pElement.Geometry = pGeometry;
            IElementProperties pElemProps = pElement as IElementProperties;
            pElemProps.Name = sName;
            if (pGroup == null)
            {
                IGraphicsContainer pGCon = pMap.BasicGraphicsLayer as IGraphicsContainer;
                pGCon.AddElement(pElement,0);
            }
            else
            {
                pGroup.AddElement(pElement);
            }
        }
    }
}
