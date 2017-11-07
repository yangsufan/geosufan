using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace SysCommon
{
    public static class modCalArea
    {
        private static mdlPublic.LLAreaPro m_pLLAreaPrama;
        public static void CalCulateAllipsoidArea(IFeatureClass pFeaClass,string strAreaField,double dCenterLine)
        {
            if (pFeaClass == null) { return; }
            try
            {
                int Fieldindex = pFeaClass.Fields.FindField(strAreaField);
                IFeatureCursor pFeatureCursor = pFeaClass.Update(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    IGeometry pGeometry = pFeature.ShapeCopy;
                    if (pGeometry != null)
                    {
                        if (!pGeometry.IsEmpty)
                        {
                            pFeature.set_Value(Fieldindex, modCalArea.GetArea(pGeometry as IPolygon, dCenterLine));
                            pFeatureCursor.UpdateFeature(pFeature);

                        }
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                pFeatureCursor.Flush();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            catch { }
        }
       public static double GetArea(ESRI.ArcGIS.Geometry.IPolygon pPolygon, double dblcenter)
       {
           double Area = 0;
           if (pPolygon == null) { return Area; }
           try
           {
               if (m_pLLAreaPrama.m_A == 0)
               {
                   mdlPublic.SetLLAreaPrama(ref m_pLLAreaPrama, 1000);
               }
               ITopologicalOperator pTopologicalOperator = null;
               IGeometryCollection pGeometryCollection = pPolygon as IGeometryCollection;
               int nRingCount = pGeometryCollection.GeometryCount;
               if (nRingCount == 1)//单环的多边形
               {
                   Area = GetRYPolygonArea(pPolygon, dblcenter);
               }
               else//多环的多边形计算总面积
               {
                   double fArea = 0;
                   double fTempArea = 0;
                   for (int i = 0; i < nRingCount; i++)
                   {
                       IRing pRing = pGeometryCollection.get_Geometry(i) as IRing;
                       IPolygon pTempPolygon = MakePolygonFromRing(pRing) as IPolygon;
                       if (pTempPolygon == null) { return Area; }
                       int nAreaSng;
                       //凌海峰 2008-0405：下面的代码是有些问题的，请以后修改，Polygon本身就有办法判断内外环的，这里暂时不修改了
                       SimplyPolygon(pTempPolygon, out nAreaSng);
                       fTempArea = GetRYPolygonArea(pTempPolygon, dblcenter);
                       fArea = fArea + nAreaSng * Math.Abs(fTempArea);
                   }
                   Area = Math.Abs(fArea);
               }
               return Area;
           }
           catch { return Area; }
       }


       public static double GetRYPolygonArea(ESRI.ArcGIS.Geometry.IPolygon pXYPolygon, double dblcenter)
       {
           double RYPolygonArea = 0;
           if (pXYPolygon == null || pXYPolygon.IsEmpty) { return RYPolygonArea; }
           //转换成经纬度
           IPolygon pLBPolygon = pXYPolygon;
           double fMinL;
           int nStartIndex;//最左边的点索引值
           if (GetMinLIndex(pLBPolygon,out fMinL,out nStartIndex) == false)
           { return RYPolygonArea; }
           try
           {
               IPointCollection pPointCollection = pLBPolygon as IPointCollection;
               int nPointCount = pPointCollection.PointCount;
               IPoint pPoint1, pPoint2;
               double fArea = 0;
               int nCurIndex = nStartIndex;
               fArea = 0;
               dblcenter = 3600 / mdlPublic.RHO * dblcenter;
               for (int i = 0; i < nPointCount; i++)
               {
                   double dYushu1 = Math.IEEERemainder(nCurIndex, nPointCount);
                   double dYushu2 = Math.IEEERemainder(nCurIndex + 1, nPointCount);
                   if (dYushu1 < 0)
                   {
                       dYushu1 = dYushu1 + nPointCount;
                   }
                   if (dYushu2 < 0)
                   {
                       dYushu2 = dYushu2+ nPointCount;
                   }
                   pPoint1 = pPointCollection.get_Point(Convert.ToInt32(dYushu1));
                   pPoint2 = pPointCollection.get_Point(Convert.ToInt32(dYushu2));
                   double dblX1, dblX2, dblY1, dblY2;
                   modXY2LB.ComputeXYGeo(pPoint1.Y, pPoint1.X, out dblY1, out dblX1, dblcenter);
                   modXY2LB.ComputeXYGeo(pPoint2.Y, pPoint2.X, out dblY2, out dblX2, dblcenter);
                   double fTempArea;
                   //将所有的换算成弧度
                   //检查是否首先需要换算成整数
                   dblX1 = dblX1 / mdlPublic.RHO;
                   dblX2 = dblX2 / mdlPublic.RHO;
                   dblY1 = dblY1 / mdlPublic.RHO;
                   dblY2 = dblY2 / mdlPublic.RHO;
                   fTempArea = GetRYTXArea(ref dblX1,ref dblY1,ref dblX2,ref dblY2, m_pLLAreaPrama);
                   fArea = fArea + fTempArea;
                   nCurIndex = nCurIndex + 1;

               }
               RYPolygonArea = fArea;
               return RYPolygonArea;
           }
           catch { return RYPolygonArea; }

       }

       private static double GetRYTXArea(ref double fL1, ref double fB1, ref double fL2,ref double fB2, mdlPublic.LLAreaPro pLLAreaPro)
       {
           try
           {
               double resArea = 0;
               double fB_ = 0;
               fB_ = (fB1 - fB2) / 2;
               double fB = 0;
               fB = (fB1 + fB2) / 2;
               double fL = 0;
               fL = (fL1 + fL2) / 2;
               double dblArea, dblVal1, dblval2, dblval3, dblval4, dblval5, RadDiff;
               //这里注意使用大数乘以小数 再乘大数
               RadDiff = 2 * pLLAreaPro.m_Globeb * fL;
               dblVal1 = pLLAreaPro.m_Globeb * RadDiff * pLLAreaPro.m_A * Math.Sin(fB_) * Math.Cos(fB);
               dblval2 = pLLAreaPro.m_Globeb * RadDiff * pLLAreaPro.m_B * Math.Sin(3 * fB_) * Math.Cos(3 * fB);
               dblval3 = pLLAreaPro.m_Globeb * RadDiff * pLLAreaPro.m_C * Math.Sin(5 * fB_) * Math.Cos(5 * fB);
               dblval4 = pLLAreaPro.m_Globeb * RadDiff * pLLAreaPro.m_D * Math.Sin(7 * fB_) * Math.Cos(7 * fB);
               dblval5 = pLLAreaPro.m_Globeb * RadDiff * pLLAreaPro.m_E * Math.Sin(9 * fB_) * Math.Cos(9 * fB);

               resArea = dblVal1 - dblval2 + dblval3 - dblval4 + dblval5;
               return resArea;
           }
           catch (Exception err)
           {
               return 0;
           }

       }

       //获得一个环的形状
       private static ESRI.ArcGIS.Geometry.Polygon MakePolygonFromRing(ESRI.ArcGIS.Geometry.IRing pRing)
       {
           ESRI.ArcGIS.Geometry.Polygon pResPolygon = null;

           ESRI.ArcGIS.Geometry.IPolygon pPolygon = null;
           try
           {
               pPolygon = new ESRI.ArcGIS.Geometry.PolygonClass();
               ESRI.ArcGIS.Geometry.IGeometryCollection pGeometryCollection;
               pGeometryCollection = pPolygon as ESRI.ArcGIS.Geometry.IGeometryCollection;
               object missing = Type.Missing;
               pGeometryCollection.AddGeometry(pRing, ref missing, ref missing);
               pResPolygon = pPolygon as ESRI.ArcGIS.Geometry.Polygon;
               return pResPolygon;
           }
           catch (Exception err)
           {
               pResPolygon = null;
               pPolygon = null;
           }
           return null;
       }
       private static bool GetMinLIndex(IPolygon pLBPolygon, out double fMinL, out int nIndex)
       {
           bool MinLIndex = false; nIndex = 0;
           IPointCollection pPointCollection = pLBPolygon as IPointCollection;
               int nCount = pPointCollection.PointCount;
               IPoint pPoint = pPointCollection.get_Point(0);
               fMinL = pPoint.X;
           try
           {
               
              
               for (int i = 0; i < nCount; i++)
               {
                   pPoint = pPointCollection.get_Point(i);
                   if (fMinL > pPoint.X)
                   {
                       fMinL = pPoint.X;
                       nIndex = i;
                   }
               }
               MinLIndex = true;
               return MinLIndex;
           }
           catch { return MinLIndex = false; }
       }

       private static void SimplyPolygon(IPolygon pTempPolygon, out int nAreaSng)
       {
           IArea pArea = pTempPolygon as IArea;
           nAreaSng = 0;
             if (pArea.Area < 0)
             {
                 nAreaSng = -1;
             }
             else if (pArea.Area > 0)
             {
                 nAreaSng = 1;
             }
             ITopologicalOperator pTopologicalOperator = pTempPolygon as ITopologicalOperator;
             pTopologicalOperator.Simplify(); 
       }

     
    }
}
