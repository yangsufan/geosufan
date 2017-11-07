using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;


using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;
//using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
//using ESRI.ArcGIS.Animation;
//  三维绘制及三维坐标计算函数   张琪    
namespace GeoArcScene3DAnalyse
{
    public class Cls3DMarkDraw
    {
     /// <summary>
     /// 设置RGB函数   20110609
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
        /// 判断两点要素是否在同一位置    张琪 20110622
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
           if ( Point1.Y != Point2.Y)
           {
               TheSame = false;
           }
           if ( Point1.Z != Point2.Z)
           {
               TheSame = false;
           }
           return TheSame;

       }
        /// <summary>
        /// 在场景中绘制点要素  20110609
        /// </summary>
        /// <param name="pGeometry">点要素</param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="Size">符号大小</param>
        /// <returns></returns>
       public static IElement DrawPoint(IGeometry pGeometry, int r, int g, int b, double Size)
       {
           IElement markerElement = new MarkerElementClass();
           ISimpleMarker3DSymbol pSimpleMarker3DSymbol = new SimpleMarker3DSymbolClass();
           pSimpleMarker3DSymbol.Style = esriSimple3DMarkerStyle.esriS3DMSSphere;
           pSimpleMarker3DSymbol.ResolutionQuality = 1;
           IMarkerSymbol pMarkerSymbol = pSimpleMarker3DSymbol as IMarkerSymbol;
           pMarkerSymbol.Size = Size;
           pMarkerSymbol.Color = getRGB(r, g, b);
           markerElement.Geometry = pGeometry;
           IMarkerElement pMarkerElement = markerElement as IMarkerElement;
           pMarkerElement.Symbol = pMarkerSymbol;
           return markerElement;
       }
       /// <summary>
       /// 在三维场景中由两点生成线的绘制   20110609
       /// </summary>
       /// <param name="Point1">前一个点要素</param>
       /// <param name="Point2">后一点要素</param>
       /// <param name="r"></param>
       /// <param name="g"></param>
       /// <param name="b"></param>
       /// <param name="Width">线符号宽</param>
       /// <returns></returns>
       public static IElement PointToPolyline(IPoint Point1, IPoint Point2, int r, int g, int b, double Width)
       {

           ILine line = new LineClass();
           line.PutCoords(Point1, Point2);
           object missing = Type.Missing;
           ISegmentCollection segColl = new PolylineClass();
           segColl.AddSegment(line as ISegment, ref missing, ref missing);
           IPolyline pPolyline = new PolylineClass();
           pPolyline = segColl as IPolyline;
           IZAware pZAware = new PolylineClass();
           pZAware = pPolyline as IZAware;
          
           pZAware.ZAware = true;
          
           IElement lineElement = new LineElementClass();
           ISimpleLine3DSymbol pSimpleLine3DSymbol = new SimpleLine3DSymbolClass();
           pSimpleLine3DSymbol.Style = esriSimple3DLineStyle.esriS3DLSWall;
           pSimpleLine3DSymbol.ResolutionQuality = 1;
           ILineSymbol pLineSymbol = pSimpleLine3DSymbol as ILineSymbol;
           pLineSymbol.Color = getRGB(r, g, b);
           pLineSymbol.Width = Width;
           lineElement.Geometry = pZAware as IGeometry;
           ILineElement lineElement2 = lineElement as ILineElement;
           lineElement2.Symbol = pLineSymbol;
           return lineElement;
       }
      /// <summary>
      /// 由点集合生成面要素   20110609
      /// </summary>
      /// <param name="pPointCollection">点集合</param>
      /// <param name="PointMove">移动的点</param>
      /// <returns></returns>
       public static IPolygon PointToPolygon(IPointCollection pPointCollection, IPoint PointMove)
       {
           IPointCollection pMovePointCollection = new PolygonClass();
           object pBefore = Type.Missing;
           object pAfter = Type.Missing;
           for (int i = 0; i < pPointCollection.PointCount;i++ )
           {
               IPoint pPoint = pPointCollection.get_Point(i);
               pMovePointCollection.AddPoint(pPoint, ref pBefore, ref pAfter);
           }
           pMovePointCollection.AddPoint(PointMove, ref pBefore, ref pAfter);
           IZAware pZAware = pMovePointCollection as IZAware;
           pZAware.ZAware = true;
           IPolygon pPolygon = pMovePointCollection as IPolygon;
           return pPolygon;
       }
        /// <summary>
        /// 三维场景中绘制线要素  20110609
        /// </summary>
        /// <param name="pGeometry">线要素</param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="Width">线符号宽</param>
        /// <returns></returns>
       public static IElement DrawPolylineZ(IGeometry pGeometry, int r, int g, int b, double Width)
       {
           IPolyline pPolyline = (IPolyline)pGeometry;
           IZAware pZAware = new PolylineClass();
           pZAware = pPolyline as IZAware;
           //ISimpleLine3DSymbol pSimpleLine3DSymbol = new SimpleLine3DSymbolClass();
           pZAware.ZAware = true;
           IElement lineElement = new LineElementClass();
           ISimpleLine3DSymbol pSimpleLine3DSymbol = new SimpleLine3DSymbolClass();
           pSimpleLine3DSymbol.Style = esriSimple3DLineStyle.esriS3DLSWall;
           pSimpleLine3DSymbol.ResolutionQuality = 1;
           ILineSymbol pLineSymbol = pSimpleLine3DSymbol as ILineSymbol;
           pLineSymbol.Color = getRGB(r, g, b);
           pLineSymbol.Width = Width;
           lineElement.Geometry = pZAware as IGeometry;
           ILineElement lineElement2 = lineElement as ILineElement;
           lineElement2.Symbol = pLineSymbol;
           return lineElement;
       }
        /// <summary>
        /// 三维场景中绘制面要素 20110609
        /// </summary>
        /// <param name="pGeometry">面要素</param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="Z"></param>
        /// <param name="Width"></param>
        /// <returns></returns>
       public static IElement DrawPolygonZ(IGeometry pGeometry, int r, int g, int b, int Z, double Width)
       {
           IPolygon pEnvPolygon = (IPolygon)pGeometry;
           IZAware pZAware = (IZAware)pEnvPolygon;
           ((IZAware)pZAware).ZAware = true;
           IZ pZ = (IZ)pEnvPolygon;
           pZ.SetConstantZ(Z);
           IElement pElement;
           IPolygonElement pPolygonElement = new PolygonElementClass();
           pElement = pPolygonElement as IElement;
           IFillShapeElement pFillShapeElement = pPolygonElement as IFillShapeElement;

           ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
           pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSCross;
           pSimpleFillSymbol.Color = getRGB(r, g, b);
           ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbolClass();
           pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSDashDotDot;
           pSimpleLineSymbol.Color = getRGB(r,g,b);
           pSimpleLineSymbol.Color.Transparency = 10;
           pSimpleLineSymbol.Width = Width;
           ISymbol pSymbol = pSimpleLineSymbol as ISymbol;
           pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;
           pSimpleFillSymbol.Outline = pSimpleLineSymbol;
           pElement.Geometry = pZ as IGeometry;
           pFillShapeElement.Symbol = pSimpleFillSymbol;

           return pElement;
       }

        /// <summary>
        /// 三维要数闪烁
        /// </summary>
        /// <param name="pGeometry">输入的要素</param>
        /// <param name="pSceneGraph"></param>
       public static void FlashGeometry(IGeometry pGeometry, ISceneGraph pSceneGraph)
       {
           IDisplay3D pDisplay3D = (IDisplay3D)pSceneGraph;
           ISpatialReference pSpatialReference = pSceneGraph.Scene.SpatialReference;
           pGeometry.SpatialReference = pSpatialReference;
           pDisplay3D.AddFlashFeature(pGeometry);

           pDisplay3D.FlashFeatures();

       }
        /// <summary>
        /// 空间距离计算
        /// </summary>
        /// <param name="pToPoint"></param>
        /// <param name="pForPoint"></param>
        /// <returns></returns>
       public static double SpaceDistance(IPoint pToPoint,IPoint pForPoint)
       {
           double ToX, ToY, ToZ, ForX, ForY, ForZ, X, Y, Z, SpaceLength;
            ToX = pToPoint.X;
            ToY = pToPoint.Y;
            ToZ = pToPoint.Z;
            ForX = pForPoint.X;
            ForY = pForPoint.Y;
            ForZ = pForPoint.Z;
            X = ToX - ForX;
            Y = ToY - ForY;
            Z = ToZ - ForZ;
            SpaceLength = Math.Sqrt(X * X + Y * Y + Z * Z);
            return SpaceLength;
       }
        /// <summary>
        /// 垂直距离计算
        /// </summary>
        /// <param name="pToPoint"></param>
        /// <param name="pForPoint"></param>
        /// <returns></returns>
       public static double VerticalDistance(IPoint pToPoint, IPoint pForPoint)
       {
           double ToZ, ForZ, VerticalLength;
           ToZ = pToPoint.Z;
           ForZ = pForPoint.Z;
           VerticalLength =Math.Abs(ToZ - ForZ);
           return VerticalLength;

       }
       /// <summary>
       /// 水平距离计算
       /// </summary>
       /// <param name="pToPoint"></param>
       /// <param name="pForPoint"></param>
       /// <returns></returns>
       public static double LevelDistance(IPoint pToPoint, IPoint pForPoint)
       {
           double LevelLength, SpaceLength, VerticalLength;
           SpaceLength = SpaceDistance(pToPoint, pForPoint);
           VerticalLength = VerticalDistance(pToPoint, pForPoint);
           LevelLength = Math.Sqrt(SpaceLength * SpaceLength + VerticalLength * VerticalLength);
           return LevelLength;
       }
        /// <summary>
       /// 删除指定名称的  Element  张琪    20110628
        /// </summary>
        /// <param name="pScene"></param>
        /// <param name="sName"></param>
       public static void DeleteAllElementsWithName(IScene pScene,string  sName)
       {
           IElement pElement;
           IGraphicsContainer3D pGCon3D = pScene.BasicGraphicsLayer as IGraphicsContainer3D;
           if (pGCon3D.ElementCount >0)
           {
               pGCon3D.Reset();
               pElement = pGCon3D.Next() as IElement;
               while (pElement!=null)
               {
                    IElementProperties pElemProps = pElement as IElementProperties;
                   if (pElemProps.Name == sName)
                   {
                       pGCon3D.DeleteElement(pElement );
                          pGCon3D.Reset();
                   }
                   pElement = pGCon3D.Next() as IElement;
               }
           }
       }
        /// <summary>
        /// 绘制三维对象 张琪  20110628
        /// </summary>
        /// <param name="pGeometry">三维对象</param>
        /// <param name="pColor">颜色</param>
        /// <param name="lSize">绘制的大小</param>
        /// <param name="sName">名称</param>
        /// <param name="pScene"></param>
        /// <param name="pGroup"></param>
       public static void AddSimpleGraphic(IGeometry pGeometry, IRgbColor pColor, int lSize, string sName, IScene pScene, IGroupElement pGroup)
       {
           if (pGeometry ==null)
           {
               return ;
           }
           IElement pElement= null;
           //ISymbol pSym;
           //根据绘制的对象类型对IElement进行不同的定义
           switch (pGeometry.GeometryType.ToString())
           {
               case "esriGeometryPoint":
                   pElement = new MarkerElementClass();
                   IMarkerElement pPointElement = pElement as IMarkerElement;
                   ISimpleMarkerSymbol pMSym = new SimpleMarkerSymbolClass();
                   pMSym.Color =pColor;
                   pMSym.Size= lSize;
                   pMSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                   IMarkerSymbol pMarkerSymbol = pMSym as IMarkerSymbol;
                   pPointElement.Symbol=pMarkerSymbol;
                   break;
               case"esriGeometryPolyline":
                   pElement = new LineElementClass();
                   ILineElement pLineElement = pElement as ILineElement;
                   ISimpleLineSymbol pLSym = new SimpleLineSymbolClass();
                   pLSym.Width = lSize;
                   pLSym.Style= esriSimpleLineStyle.esriSLSSolid;
                   pLSym.Color = pColor;
                   ILineSymbol pLineSymbol = pLSym as ILineSymbol;
                   pLineElement.Symbol = pLineSymbol;
                   break;
               case"esriGeometryPolygon":
                   ISimpleLineSymbol  pOutlineSym =new SimpleLineSymbolClass();
                   pOutlineSym.Color = pColor;
                   pOutlineSym.Width =lSize;
                   pOutlineSym.Style = esriSimpleLineStyle.esriSLSSolid;
                   if( sName == "_ReferancePlane_")
                   {
                       pColor.Transparency =Convert.ToByte(Convert.ToInt32( 255/2));
                   }
                   pElement =new PolygonElementClass();
                   IFillShapeElement pPolygonElement = pElement as IFillShapeElement;
                   IFillSymbol  pFSym = new SimpleFillSymbolClass();
                   pFSym.Color =pColor;
                   ILineSymbol pLineSymbol1 = pOutlineSym as ILineSymbol;
                   pFSym.Outline =pLineSymbol1;
                    pPolygonElement.Symbol = pFSym;
                   break;
               case "esriGeometryMultiPatch":
                   pElement = new MultiPatchElementClass();
                   IFillShapeElement pFElement = pElement as IFillShapeElement;
                   IFillSymbol pFSymbol = pFElement.Symbol;
                   if (sName == "_ReferancePlane_")
                   {
                       pColor.Transparency = Convert.ToByte(Convert.ToInt32(255 / 2));
                   }
                   pFSymbol.Color = pColor;
                   pFElement.Symbol = pFSymbol;
                   break;
               default:
                   break;
           }
           pElement.Geometry = pGeometry;
           IElementProperties pElemProps = pElement as IElementProperties;
           pElemProps.Name= sName;
           if (pGroup == null)
           {
               IGraphicsContainer3D pGCon3D = pScene.BasicGraphicsLayer as IGraphicsContainer3D;
               pGCon3D.AddElement(pElement);
           }
           else
           {
               pGroup.AddElement(pElement);
           }
       }























    }
}
