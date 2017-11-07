using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

namespace GeoCustomExport
{
    public class ImportToFC
    {

        public static DevComponents.DotNetBar.Controls.ProgressBarX progressStep = null;
        public static Dictionary<string, string> m_DicFields = new Dictionary<string, string>();
        public static void CopyFeatureToFeatureClass(IFeatureClass pSourceClass, IFeatureClass pTargetfeatureclass, IGeometry pgeometry, string strcut, string strCondtion)
        {
            IFeatureCursor pFeatureCursor = null;
            int featurecount = 0;
            if (pgeometry != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();

                pSpatialFilter.Geometry = pgeometry;

                //pSpatialFilter.GeometryField = pfeaturelayer.FeatureClass.ShapeFieldName;
                switch (pgeometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        break;

                    case esriGeometryType.esriGeometryPolyline:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;

                    case esriGeometryType.esriGeometryPolygon:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                    default:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                }

                if (strCondtion == "")//过滤条件
                { }
                else
                {
                    pSpatialFilter.WhereClause = strCondtion;
                }

                featurecount = pSourceClass.FeatureCount(pSpatialFilter);
                pFeatureCursor = pSourceClass.Search(pSpatialFilter, false);
            }
            else
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = "*";
                pQueryFilter.WhereClause = strCondtion;
                featurecount = pSourceClass.FeatureCount(pQueryFilter);
                pFeatureCursor = pSourceClass.Search(pQueryFilter, false);
            }

            //剪裁输出
            if (strcut == "True")
            {
                cutExport(pFeatureCursor, pTargetfeatureclass, pgeometry, featurecount);
            }
            //不剪裁输出
            else
            {
                notcutExport(pFeatureCursor, pTargetfeatureclass, featurecount);
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;

        }
        //for shp
        public static void CopyFeatureToFeatureClassShp(IFeatureClass pSourceClass, IFeatureClass pTargetfeatureclass, IGeometry pgeometry, string strcut, string strCondtion)
        {

            IFeatureCursor pFeatureCursor = null;
            int featurecount = 0;
            if (pgeometry != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();

                pSpatialFilter.Geometry = pgeometry;

                //pSpatialFilter.GeometryField = pfeaturelayer.FeatureClass.ShapeFieldName;
                switch (pgeometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        break;

                    case esriGeometryType.esriGeometryPolyline:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;

                    case esriGeometryType.esriGeometryPolygon:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                    default:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                }

                if (strCondtion == "")//过滤条件
                { }
                else
                {
                    pSpatialFilter.WhereClause = strCondtion;
                }

                featurecount = pSourceClass.FeatureCount(pSpatialFilter);
                pFeatureCursor = pSourceClass.Search(pSpatialFilter, false);
            }
            else
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = "*";
                pQueryFilter.WhereClause = strCondtion;
                featurecount = pSourceClass.FeatureCount(pQueryFilter);
                pFeatureCursor = pSourceClass.Search(pQueryFilter, false);
            }

            //剪裁输出
            if (strcut == "True")
            {
                cutExportShp(pFeatureCursor, pTargetfeatureclass, pgeometry, featurecount);
            }
            //不剪裁输出
            else
            {
                notcutExportShp(pFeatureCursor, pTargetfeatureclass, featurecount);
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;

        }

        //不剪裁输出
        public static void notcutExport(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex =-1;
                    if (m_DicFields.Keys.Contains(sFieldName))
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(m_DicFields[sFieldName]);
                    }
                    else
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(sFieldName);
                    }
                    if (iIndex == -1) continue;
                    if ((pFeatureBuffer.Fields.get_Field(iIndex).Editable == true) && pFeatureBuffer.Fields.get_Field(iIndex).Type != esriFieldType.esriFieldTypeGeometry )
                    {
                        pFeatureBuffer.set_Value(iIndex, pFeature.get_Value(i));
                    }
                }
                pFeatureBuffer.Shape = pFeature.ShapeCopy;
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                progressStep.PerformStep();
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
        //剪裁输出
        public static void cutExport(IFeatureCursor pfeaturecursor, IFeatureClass pfeatureclass, IGeometry pgeometry, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pfeaturecursor.NextFeature();
            if (pFeature == null) return;

            IFeatureCursor pFeatureCursor = pfeatureclass.Insert(true);

            ISegmentCollection pSegmentCol = new PolygonClass();
            if (pgeometry.GeometryType == esriGeometryType.esriGeometryEnvelope)
            {
                IEnvelope pEnvelope = new EnvelopeClass();
                pEnvelope = pgeometry as IEnvelope;
                pSegmentCol.SetRectangle(pEnvelope);
                pgeometry = pSegmentCol as IGeometry;
            }
            else if (pgeometry.GeometryType == esriGeometryType.esriGeometryCircularArc)
            {
                ICircularArc pCircularArc = new CircularArcClass();
                pCircularArc = pgeometry as ICircularArc;
                object obj = System.Reflection.Missing.Value;
                pSegmentCol.AddSegment((ISegment)pCircularArc, ref obj, ref obj);
                pgeometry = pSegmentCol as IGeometry;
            }

            ITopologicalOperator pTopoOp = (ITopologicalOperator)pgeometry;
            IGeometry pBndGeom = pTopoOp.Boundary;

            esriGeometryDimension iDimension;

            IGeometry pAimGeometry = pFeature.ShapeCopy;
            if (pAimGeometry.Dimension < pgeometry.Dimension) iDimension = pAimGeometry.Dimension;
            else iDimension = pgeometry.Dimension;

            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pfeatureclass.CreateFeatureBuffer();
                IFeature pAimFeature = (IFeature)pFeatureBuffer;

                pAimGeometry = pFeature.ShapeCopy;
                IRelationalOperator pRelOpeator = (IRelationalOperator)pAimGeometry;

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = -1;
                    if (m_DicFields.Keys.Contains(sFieldName))
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(m_DicFields[sFieldName]);
                    }
                    else
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(sFieldName);
                    }

                    if (iIndex == -1) continue;
                    IField pFld = pAimFeature.Fields.get_Field(iIndex);
                    if ((iIndex > -1) && (pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        pAimFeature.set_Value(iIndex, pFeature.get_Value(i));
                    }
                }

                //此处有错误，暂时加保护 xisheng 20111128
                try
                {
                    if (pAimGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                    {
                        pAimFeature.Shape = pFeature.ShapeCopy;
                    }
                    //判断是否相交或者包含关系,如果是则进行空间切割
                    else
                    {
                        bool bCross = false;
                        bool bContain = false;
                        try
                        {
                            bCross = pRelOpeator.Crosses(pBndGeom);    //changed by chulili 20111213 这句话可能报错，不要直接放在IF条件里                        
                        }
                        catch
                        { }
                        try
                        {
                            bContain = pRelOpeator.Contains(pBndGeom);  //changed by chulili 20111213 这句话可能报错
                        }
                        catch
                        { }
                        if (bCross || bContain)
                        {
                            pTopoOp = (ITopologicalOperator)pFeature.ShapeCopy;
                            pTopoOp.Simplify();
                            pAimFeature.Shape = pTopoOp.Intersect(pgeometry, iDimension);
                        }
                        else
                        {
                            pAimFeature.Shape = pFeature.ShapeCopy;
                        }
                    }
                }
                catch { }

                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount = iCount + 1;
                progressStep.PerformStep();
                pFeature = pfeaturecursor.NextFeature();
            }

            if (iCount > 0) pFeatureCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }

        //不剪裁输出forShp
        public static void notcutExportShp(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    //string sFieldName = pFeature.Fields.get_Field(i).Name;

                    //int iIndex = pFeatureBuffer.Fields.FindField(toShpField[sFieldName]);
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = -1;
                    if (m_DicFields.Keys.Contains(sFieldName))
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(m_DicFields[sFieldName]);
                    }
                    else
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(sFieldName);
                    }

                    try
                    {
                        if ((iIndex > -1) && (pFeatureBuffer.Fields.get_Field(iIndex).Editable == true))
                        {
                            pFeatureBuffer.set_Value(iIndex, pFeature.get_Value(i));
                        }

                    }
                    catch
                    { }
                }
                pFeatureBuffer.Shape = pFeature.ShapeCopy;
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                progressStep.PerformStep();
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
        //剪裁输出forShp
        public static void cutExportShp(IFeatureCursor pfeaturecursor, IFeatureClass pfeatureclass, IGeometry pgeometry, int featurecount)
        {
            progressStep.Minimum = 0;
            progressStep.Maximum = featurecount;
            progressStep.Step = 1;
            IFeature pFeature = pfeaturecursor.NextFeature();
            if (pFeature == null) return;

            IFeatureCursor pFeatureCursor = pfeatureclass.Insert(true);

            ISegmentCollection pSegmentCol = new PolygonClass();
            if (pgeometry.GeometryType == esriGeometryType.esriGeometryEnvelope)
            {
                IEnvelope pEnvelope = new EnvelopeClass();
                pEnvelope = pgeometry as IEnvelope;
                pSegmentCol.SetRectangle(pEnvelope);
                pgeometry = pSegmentCol as IGeometry;
            }
            else if (pgeometry.GeometryType == esriGeometryType.esriGeometryCircularArc)
            {
                ICircularArc pCircularArc = new CircularArcClass();
                pCircularArc = pgeometry as ICircularArc;
                object obj = System.Reflection.Missing.Value;
                pSegmentCol.AddSegment((ISegment)pCircularArc, ref obj, ref obj);
                pgeometry = pSegmentCol as IGeometry;
            }

            ITopologicalOperator pTopoOp = (ITopologicalOperator)pgeometry;
            IGeometry pBndGeom = pTopoOp.Boundary;

            esriGeometryDimension iDimension;

            IGeometry pAimGeometry = pFeature.ShapeCopy;
            if (pAimGeometry.Dimension < pgeometry.Dimension) iDimension = pAimGeometry.Dimension;
            else iDimension = pgeometry.Dimension;

            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pfeatureclass.CreateFeatureBuffer();
                IFeature pAimFeature = (IFeature)pFeatureBuffer;

                pAimGeometry = pFeature.ShapeCopy;
                IRelationalOperator pRelOpeator = (IRelationalOperator)pAimGeometry;

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                   
                    //IField pFld = pAimFeature.Fields.get_Field(i);
                    //if ((pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                    //{
                    //    try
                    //    {
                    //        pAimFeature.set_Value(i, pFeature.get_Value(i));
                    //    }
                    //    catch
                    //    { }
                    //}

                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = -1;
                    if (m_DicFields.Keys.Contains(sFieldName))
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(m_DicFields[sFieldName]);
                    }
                    else
                    {
                        iIndex = pFeatureBuffer.Fields.FindField(sFieldName);
                    }

                    if (iIndex == -1) continue;
                    IField pFld = pAimFeature.Fields.get_Field(iIndex);
                    if ((iIndex > -1) && (pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        pAimFeature.set_Value(iIndex, pFeature.get_Value(i));
                    }
                }

                if (pAimGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                {
                    pAimFeature.Shape = pFeature.ShapeCopy;
                }
                //判断是否相交或者包含关系,如果是则进行空间切割
                else
                {
                    bool bCross = false;
                    bool bContain = false;
                    try
                    {
                        bCross = pRelOpeator.Crosses(pBndGeom);//changed by chulili 20111213 这句话可能报错，不要直接放在IF条件里                       
                    }
                    catch
                    { }
                    try
                    {
                        bContain = pRelOpeator.Contains(pBndGeom);//changed by chulili 20111213 这句话可能报错  
                    }
                    catch
                    { }
                    if (bCross || bContain)
                    {
                        pTopoOp = (ITopologicalOperator)pFeature.ShapeCopy;
                        pTopoOp.Simplify();
                        pAimFeature.Shape = pTopoOp.Intersect(pgeometry, iDimension);
                    }
                    else
                    {
                        pAimFeature.Shape = pFeature.ShapeCopy;
                    }
                }
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                if (iCount == 500)
                {
                    pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount = iCount + 1;
                progressStep.PerformStep();
                pFeature = pfeaturecursor.NextFeature();
            }

            if (iCount > 0) pFeatureCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }


        //注记复制样式
        public static void CopyAnnoPropertyToFC(IFeatureClass pSourceFeatureClass, IFeatureClass pToFeatureClass)
        {
            IAnnoClass pSourceAnnoClass = (IAnnoClass)pSourceFeatureClass.Extension;
            IAnnoClass pTargerAnnoClass = (IAnnoClass)pToFeatureClass.Extension;

            IAnnotateLayerPropertiesCollection pSourceAnnoProperCollection = pSourceAnnoClass.AnnoProperties;
            IClone pAnnoCollection = (IClone)pSourceAnnoProperCollection;

            ISymbolCollection pSourceSymbolCollection = pSourceAnnoClass.SymbolCollection;
            IClone pAnnoSymbol = (IClone)pSourceSymbolCollection;

            IAnnoClassAdmin2 pAnnoClassAdmin = (IAnnoClassAdmin2)pTargerAnnoClass;

            pAnnoClassAdmin.ReferenceScale = pSourceAnnoClass.ReferenceScale;
            pAnnoClassAdmin.ReferenceScaleUnits = pSourceAnnoClass.ReferenceScaleUnits;

            pAnnoClassAdmin.AnnoProperties = (IAnnotateLayerPropertiesCollection)pAnnoCollection.Clone();
            pAnnoClassAdmin.SymbolCollection = (ISymbolCollection)pAnnoSymbol.Clone();

            pAnnoClassAdmin.UpdateProperties();
            pAnnoClassAdmin.UpdateOnShapeChange = true;

        }
    }
}
