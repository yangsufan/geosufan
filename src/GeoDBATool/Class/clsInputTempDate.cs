using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

//临时库入库操作类
namespace GeoDBATool
{
    public class clsInputTempDate
    {
        //删除FeatureClass中所有Feature
        public static void DeleteFeature(IFeatureClass pFeatureClass,string condtion,out Exception Error)
        {
            Error = null;
            if (pFeatureClass == null)
            {
                return;
            }
            IDataset dataset = pFeatureClass as IDataset;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceEdit workspaceEdit = workspace as IWorkspaceEdit;
            workspaceEdit.DisableUndoRedo();
            workspaceEdit.StartEditing(true);
            workspaceEdit.StartEditOperation();
            IFeatureCursor pFeatureCursor = null;
            if (condtion != "")
            {
                IQueryFilter pFilter = new QueryFilterClass();
                pFilter.WhereClause = condtion;
               pFeatureCursor=pFeatureClass.Update(pFilter, false);
            }
            else
            {
                pFeatureCursor = pFeatureClass.Search(null, false);
            }
            IFeature pFeature = pFeatureCursor.NextFeature();
            int count = 0;
            try
            {
                while (pFeature != null)
                {
                  // pFeatureCursor.DeleteFeature();
                   pFeature.Delete();
                   count++;
                   if (count == 500)
                   {
                       pFeatureCursor.Flush();
                       count = 0;
                   }
                   pFeature = pFeatureCursor.NextFeature();
                }
                if (count > 0)
                {
                    pFeatureCursor.Flush();
                }
            }
            catch (Exception ex)
            {
                Error = ex;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            workspaceEdit.StopEditOperation();
            workspaceEdit.StopEditing(true);
        }
        public static void DeleteFeatureClass(IFeatureClass pFeatureClass, string condtion,string FeatureClassName, out Exception Error)
        {
            Error = null;
            if (pFeatureClass == null)
            {
                return;
            }
            IDataset dataset = pFeatureClass as IDataset;
            IWorkspace workspace = dataset.Workspace;
            try
            {
                if (condtion != "")
                {
                    workspace.ExecuteSQL("delete from " + FeatureClassName +" where "+ condtion);
                }
                else
                {
                    workspace.ExecuteSQL("delete from " + FeatureClassName);
                }
            }
            catch(Exception ex)
            {
                Error = ex;
            }
        }

        //将源图层数据复制到目标图层数据
        public static bool CopySourceFeatureClass(IFeatureClass SourceFeatureClass, IFeatureClass TargetFeatureClass, IGeometry pGeometry, string strCondition, out Exception ErrorMsg)
        {
            bool flag = false;
            if (SourceFeatureClass == null || TargetFeatureClass == null)
            {
                ErrorMsg = null;
                return flag;
            }
            IFeatureCursor pFeatureCursor = null;
            int FeatureCount = 0;
            if (pGeometry != null)
            {
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pGeometry;
                switch (pGeometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPoint:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                        break;
                    default:
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                        break;
                }
                if (strCondition == "") { }
                else
                {
                    pSpatialFilter.WhereClause = strCondition;
                }
                FeatureCount = SourceFeatureClass.FeatureCount(pSpatialFilter);
                pFeatureCursor = SourceFeatureClass.Search(pSpatialFilter, false);
            }
            else
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.SubFields = "*";
                pQueryFilter.WhereClause = strCondition;
                FeatureCount = SourceFeatureClass.FeatureCount(pQueryFilter);
                pFeatureCursor = SourceFeatureClass.Search(pQueryFilter, false);
            }
            notcutExport(pFeatureCursor, TargetFeatureClass, FeatureCount, out ErrorMsg);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
            return flag;
        }
        //不剪裁输出
        public static void notcutExport(IFeatureCursor pCursor, IFeatureClass pToFeatureClass, int featurecount, out Exception error)
        {
            IFeature pFeature = pCursor.NextFeature();
            IFeatureCursor pFeatureCursor = pToFeatureClass.Insert(true);
            error = null;
            int iCount = 0;
            while (pFeature != null)
            {
                IFeatureBuffer pFeatureBuffer = pToFeatureClass.CreateFeatureBuffer();

                for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                {
                    string sFieldName = pFeature.Fields.get_Field(i).Name;

                    int iIndex = pFeatureBuffer.Fields.FindField(sFieldName);
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
                // 写入入库日期和入库人员
                int DateIndex = pFeatureBuffer.Fields.FindField("FromDate");
                int UserIndex = pFeatureBuffer.Fields.FindField("USERNAME");
                try
                {
                    if (DateIndex > -1 && UserIndex > -1)
                    {
                        pFeatureBuffer.set_Value(DateIndex, DateTime.Today.ToString("yyyy - MM - dd"));
                        if (Plugin.LogTable.user != "")
                        {
                            pFeatureBuffer.set_Value(UserIndex, Plugin.LogTable.user);
                        }
                    }
                }
                catch { }

                try
                {
                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pFeatureCursor.InsertFeature(pFeatureBuffer);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                if (iCount == 500)
                 {
                     pFeatureCursor.Flush();
                    iCount = 0;
                }
                iCount++;
                pFeature = pCursor.NextFeature();
            }
            if (iCount > 0) pFeatureCursor.Flush();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;
        }
    }
}
