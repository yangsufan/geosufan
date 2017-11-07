using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Windows.Forms;
using System.Drawing;
//ZQ  20110923  add
namespace GeoDataManagerFrame
{
   public class DJAttributeQueryClass
    {
       public static IWorkspace m_Workspace;
       /// <summary>
       /// 初始化Tab表
       /// </summary>
       /// <param name="pDataGridView"></param>
       /// <param name="strTable"></param>
        public static void GetTable(DevComponents.DotNetBar.Controls.DataGridViewX pDataGridView, string strTable)
        {

            try
            {
                IFeatureWorkspace pFeatureWorkspace = m_Workspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable(strTable);
                IFields pFields = pTable.Fields;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    if (pFields.get_Field(i).Name.ToString() == "OBJECTID") { continue; }
                    pDataGridView.Rows.Add(SysCommon.ModField.GetChineseNameOfField(pFields.get_Field(i).Name.ToString()), "", pFields.get_Field(i).Name.ToString());
                }
                pDataGridView.Tag = strTable;
            }
            catch
            { }
        }
       /// <summary>
       /// 根据地籍号获取宗地图层的图形ID
       /// </summary>
       /// <param name="pFeatureClass"></param>
       /// <param name="strDJHVlaue"></param>
       /// <returns></returns>
        private static string GetFeatuerID(IFeatureClass pFeatureClass, string strDJHVlaue)
        {
            IQueryFilter pQueryFilter = new QueryFilterClass();
            string strFeatureID="";
            try
            {
                int pIndex = pFeatureClass.FindField("DJH");
                IField pField = pFeatureClass.Fields.get_Field(pIndex);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeString:
                        pQueryFilter.WhereClause = pField.Name + " = '" + strDJHVlaue + "'";
                        break;
                    default:
                        pQueryFilter.WhereClause = pField.Name + " = " + strDJHVlaue;
                        break;
                }
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    strFeatureID = pFeature.get_Value(pFeature.Fields.FindField("OBJECTID")).ToString();
                    pFeature = pFeatureCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                pFeatureCursor = null;
                return strFeatureID;
            }
            catch { return strFeatureID; }

        }
       /// <summary>
       /// 根据用户输入的表达式进行查询
       /// </summary>
       /// <param name="pLabel"></param>
       /// <param name="strTable"></param>
       /// <param name="StrQueryValue"></param>
       /// <param name="pFeatureClass"></param>
       /// <param name="pDataGridView"></param>
        public static void Query(DevComponents.DotNetBar.LabelX pLabel, string strTable, string StrQueryValue, IFeatureClass pFeatureClass, DevComponents.DotNetBar.Controls.DataGridViewX pDataGridView)
        {

            //进度条
            SysCommon.CProgress pgss = new SysCommon.CProgress("正在查询，请稍候...");
            pgss.EnableCancel = false;
            pgss.ShowDescription = false;
            pgss.FakeProgress = true;
            pgss.TopMost = true;
            pgss.ShowProgress();
            try
            {
                IFeatureWorkspace pFeatureWorkspace = m_Workspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable(strTable);
                string strFieldName = pLabel.Tag.ToString();
                string strLike = CommnClass.GetDescriptionOfWorkspace((pFeatureClass as IDataset).Workspace);
                pDataGridView.Columns["CumQueryName"].HeaderText = pLabel.Text.Substring(0, pLabel.Text.Length - 1);
                pDataGridView.Rows.Clear();
                IField pField = pTable.Fields.get_Field(pTable.FindField(strFieldName));
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = pField.Name + " Like '" + strLike + StrQueryValue + strLike + "'";
                ICursor pCursor = pTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();

                while (pRow != null)
                {
                    string strFieldVlaue = pRow.get_Value(pRow.Fields.FindField(strFieldName)).ToString();
                    string strDJHVlaue = pRow.get_Value(pRow.Fields.FindField("DJH")).ToString();
                    string strFeatureID = GetFeatuerID(pFeatureClass, strDJHVlaue);
                    pDataGridView.Rows.Add(strFieldVlaue, strDJHVlaue, strFeatureID);
                    pRow = pCursor.NextRow();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;

                pgss.Close();
            }
            catch
            {

            }
            finally
            {
                pgss.Close();
            }
        }
       /// <summary>
       /// 查询结果中用户双击选中的宗地信息
       /// </summary>
       /// <param name="pDataGridView"></param>
       /// <param name="strTableName"></param>
       /// <param name="strDJH"></param>
        public static void QueryResult(DevComponents.DotNetBar.Controls.DataGridViewX pDataGridView, string strTableName, string strDJH)
        {
            try
            {
                IFeatureWorkspace pFeatureWorkspace = m_Workspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable(strTableName);
                IQueryFilter pQueryFilter = new QueryFilterClass();
                int pIndex = pTable.FindField("DJH");
                IField pField = pTable.Fields.get_Field(pIndex);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeString:
                        pQueryFilter.WhereClause = pField.Name + " = '" + strDJH + "'";
                        break;
                    default:
                        pQueryFilter.WhereClause = pField.Name + " = " + strDJH;
                        break;
                }
                ICursor pCursor = pTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();
                if (pRow != null) { pDataGridView.Rows.Clear(); }
                while (pRow != null)
                {
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        if (pRow.Fields.get_Field(i).Name.ToString() == "OBJECTID") { continue; }
                        pDataGridView.Rows.Add(SysCommon.ModField.GetChineseNameOfField(pRow.Fields.get_Field(i).Name), pRow.get_Value(i).ToString(), pRow.Fields.get_Field(i).Name.ToString());
                    }
                    pRow = pCursor.NextRow();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            }
            catch { }
        }
       /// <summary>
       /// 根据地籍号获取相对应的宗地图形要素
       /// </summary>
       /// <param name="pFeatureClass"></param>
       /// <param name="strValue"></param>
       /// <returns></returns>
        public static IFeature  QueryFeature(IFeatureClass pFeatureClass,string strValue)
        {
            IFeatureCursor pFeatureCursor = null;
            IFeature pFeature =null;
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                int Index = pFeatureClass.Fields.FindField("DJH");
                switch (pFeatureClass.Fields.get_Field(Index).Type)
                {
                    case esriFieldType.esriFieldTypeString:
                        pQueryFilter.WhereClause = "DJH ='" + strValue + "'";
                        break;
                    default:
                        pQueryFilter.WhereClause = "DJH =" + strValue;
                        break;
                }
                pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
                pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    break;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                pFeatureCursor = null;
                return pFeature;
            }
            catch { return pFeature; }
        }
  
    }
}
