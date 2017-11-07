using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBATool
{
   public class clsMergeFeatures
    {
       /// <summary>
       /// 要素融合 陈亚飞添加
       /// </summary>
       /// <param name="pFeatureClass">需要融合的图层</param>
       /// <param name="newOID">融合后保存的要素OID</param>
       /// <param name="oldOIDLst">需要融合的要素OID</param>
       public void MergeFeatures(IFeatureClass pFeatureClass, int newOID, List<int> oldOIDLst)
       {
           IGeometry tempGeo = null;
           for (int i = 0; i < oldOIDLst.Count; i++)
           {
               int oldOID = oldOIDLst[i];
               IFeature pFeature = pFeatureClass.GetFeature(oldOID);
               if (tempGeo != null)
               {
                   ITopologicalOperator pTop = tempGeo as ITopologicalOperator;
                   tempGeo = pTop.Union(pFeature.Shape);
                   //融合后将图形简单化
                   pTop = tempGeo as ITopologicalOperator;
                   pTop.Simplify();
               }
               else
               {
                   tempGeo = pFeature.Shape;
               }
           }

           IFeature newFea = pFeatureClass.GetFeature(newOID);
           //将融合后的图形赋值给新的要素
           newFea.Shape = tempGeo;
          
           //将新生成的要素存储
           newFea.Store();

           //融合后删除被融合的要素
           for (int j = 0; j < oldOIDLst.Count; j++)
           {
               if (oldOIDLst[j] != newOID)
               {
                   IFeature delFeature = pFeatureClass.GetFeature(oldOIDLst[j]);
                   delFeature.Delete();
               }
           }
       }
   }
}
