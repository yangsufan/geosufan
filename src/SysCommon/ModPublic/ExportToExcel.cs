using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using Microsoft.Office.Interop.Excel;

namespace SysCommon
{
    public class ExportToExcel
    {
        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //double LD = 0;
        //double FLD = 0;
        //private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        //导出结束时，关闭EXCEL进程
        //private static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        //{
        //    //IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

        //    int k = 0;
        //    GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k);   //得到本进程唯一标志k
        //    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
        //    p.Kill();     //关闭进程k
        //}
        /// <summary>
        /// 得到各个统计值的百分比
        /// </summary>
        /// <param name="listArea"></param>
        /// <returns></returns>
        private IList<string> ListPercent(IList<double> listArea)
        {
            IList<string> newList = new List<string>();
            double total = GetTotalArea(listArea);
            for (int i = 0; i < listArea.Count; i++)
            {

                double tmp = 0;
                if (total != 0)
                {
                    tmp = listArea[i] / total * 100;
                }

                newList.Add(Math.Round(tmp, 2).ToString() + "%");
            }
            return newList;
        }
        /// <summary>
        /// 获得总的面积
        /// </summary>
        /// <param name="listArea"></param>
        /// <returns></returns>
        private double GetTotalArea(IList<double> listArea)
        {
            double Total = 0;
            for (int i = 0; i < listArea.Count; i++)
            {
                Total = Total + listArea[i];
            }
            return Total;
        }
        /// <summary>
        /// 将数据加入到excel中
        /// </summary>
        /// <param name="listTypeName"></param>
        /// <param name="listArea"></param>
        /// <param name="filePath">导出文件路径</param>
        /// <param name="Name">表头中类型的名称</param>
        private void DoExport(IList<string> listTypeName, IList<double> listArea, string strTitle, string TypeName)
        {
            int RowNum = listArea.Count;
            int RowIndex = 1;
            IList<string> listPercent = null;
            listPercent = ListPercent(listArea);
            if (RowNum == 0)
            {
                return;
            }
            else if (RowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application excel = null;
                Workbook wb = null;
                try
                {
                    excel = new Microsoft.Office.Interop.Excel.Application();
                    if (excel == null)
                    {
                        MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                        return;
                    }
                    wb = excel.Application.Workbooks.Add(true);
                    excel.Visible = true;

                    excel.DefaultFilePath = "";
                    excel.DisplayAlerts = true;
                    excel.WindowState = XlWindowState.xlNormal;
                    excel.SheetsInNewWorkbook = 1;
                    //表头
                    excel.Cells[1, 1] = "序号";
                    excel.Cells[1, 2] = TypeName;
                    excel.Cells[1, 3] = "面积(平方米)";
                    excel.Cells[1, 4] = "占地百分比(%)";
                    for (int i = 0; i < RowNum; i++)
                    {
                        RowIndex++;
                        excel.Cells[RowIndex, 1] = i;
                        excel.Cells[RowIndex, 2] = listTypeName[i];
                        excel.Cells[RowIndex, 3] = listArea[i].ToString();
                        excel.Cells[RowIndex, 4] = listPercent[i];
                    }
                    ///弹出对话保存生成统计表的路径
                    Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                    fd.InitialFileName = strTitle + " 统计表";
                    int result = fd.Show();
                    if (result == 0) return;
                    string fileName = fd.InitialFileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.IndexOf(".xls") == -1)
                        {
                            fileName += ".xls";
                        }
                        ///保存生成的统计表
                        wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    GC.Collect();
                }
                catch { }
            }
        }
        //根据指定字段统计面积
        private IList<double> StatisticsArea(IList<string> ListTypeName, IFeatureCursor pFeatureCursor, string fieldName)
        {

            IList<double> newList = new List<double>();
            int indexArea = -1;

            double temp = 0;
            for (int i = 0; i < ListTypeName.Count; i++)
            {
                temp = 0;
                newList.Add(temp);
            }
            IFeature pFeature = pFeatureCursor.NextFeature();
            int FieldIndex = -1;
            if (pFeature != null)
            {
                IFields pFields = pFeature.Fields;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    IField pTmpFiled = pFields.get_Field(i);
                    if (pTmpFiled.Name.ToLower().Contains("shape") && pTmpFiled.Name.ToLower().Contains("area"))
                    {
                        indexArea = i;
                        break;
                    }
                }
                FieldIndex = pFeature.Fields.FindField(fieldName);
            }
            while (pFeature != null)
            {
                string strFieldvalue = pFeature.get_Value(FieldIndex).ToString();
                for (int i = 0; i < ListTypeName.Count; i++)
                {
                    if (strFieldvalue==ListTypeName[i])
                    {
                        temp = newList[i];
                        try
                        {
                            double dTempArra = 0;
                            double dTempLength = 0;
                            GetAreaAndLength(pFeature.ShapeCopy, ref dTempLength, ref dTempArra, pFeature.Shape.SpatialReference);
                            temp = temp + dTempArra;
                        }
                        catch
                        { }
                        newList[i] = temp;
                    }
                }
                pFeature = pFeatureCursor.NextFeature();
            }
            return newList;
        }
        //投影 获得投影面积
        private void GetAreaAndLength(IGeometry pGeo, ref double dblLength, ref double dblArea, ISpatialReference pOldSpatial)
        {
            dblLength = 0;
            dblArea = 0;

            IClone pClone = pGeo as IClone;
            IGeometry pNewGeo = pClone.Clone() as IGeometry;

            //做投影变换
            if (!(pOldSpatial is IProjectedCoordinateSystem))
            {
                pNewGeo.SpatialReference = pOldSpatial;
                //获得坐标系
                ISpatialReference pNewSpatial = SysCommon.Gis.ModGisPub.GetSpatialByX((pNewGeo.Envelope.XMin + pNewGeo.Envelope.XMax) / 2);
                if (pNewSpatial != null) pNewGeo.Project(pNewSpatial);

            }

            //计算值
            if (pNewGeo is IPolygon)
            {
                IPolygon pPolygon = pNewGeo as IPolygon;
                IArea parea = pNewGeo as IArea;

                dblLength = pPolygon.Length;
                dblArea = parea.Area;
            }
            else if (pNewGeo is IPolyline)
            {
                IPolyline pPolyline = pNewGeo as IPolyline;
                dblLength = pPolyline.Length;
            }
        }
        private IList<string> GetType(IFeatureClass pFeatureClass, string TypeName)
        {

            IWorkspace pWorkspace = null;
            string strTableName = "";
            try
            {
                pWorkspace = (pFeatureClass as IDataset).Workspace;
                strTableName = (pFeatureClass as IDataset).Name;
            }
            catch
            { }
            if (pWorkspace == null || strTableName.Equals(""))
            {
                return null;
            }
            IList<string> ListType = new List<string>();
            try
            {
                pWorkspace.ExecuteSQL("drop table tmptype_ldtb");
            }
            catch
            { }

            pWorkspace.ExecuteSQL("create table tmptype_ldtb as select distinct " + TypeName + " from " + strTableName);
            IFeatureWorkspace pFeaWKS = pWorkspace as IFeatureWorkspace;
            ITable pTable = null;
            try
            {
                pTable = pFeaWKS.OpenTable("tmptype_ldtb");
            }
            catch
            { }
            if (pTable != null)
            {
                ICursor pCursor = pTable.Search(null, false);
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    try
                    {
                        string strTmp = pRow.get_Value(0).ToString();
                        ListType.Add(strTmp);
                    }
                    catch
                    { }
                    pRow = pCursor.NextRow();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            }
            try
            {
                pWorkspace.ExecuteSQL("drop table tmptype_ldtb");
            }
            catch
            { }
            return ListType;


            //IList<string> ListType = new List<string>();
            //IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
            //IDataStatistics pData = new DataStatisticsClass();
            //pData.Field = TypeName;
            //pData.Cursor = pFeatureCursor as ICursor;
            //IEnumerator pEnumeraVar = pData.UniqueValues;
            //int count = pData.UniqueValueCount;
            //pEnumeraVar.Reset();
            //while (pEnumeraVar.MoveNext())
            //{
            //    if (pEnumeraVar.Current.ToString() != "")
            //    {
            //        ListType.Add(pEnumeraVar.Current.ToString());
            //    }
            //}
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            //return ListType;
        }
        /// <summary>
        /// 设定导出路径，导出成EXCEl文件
        /// </summary>
        /// <param name="listTypeName"></param>
        /// <param name="Area"></param>
        /// <param name="fileName">EXCEL文件名</param>
        /// <param name="TypeName">表头中统计类型名</param>
        //private void ExportExcel(IList<string> listTypeName, IList<double> Area, string fileName, string TypeName)
        //{
        //    if (result == DialogResult.OK)
        //    {
        //        string path = folderBrowerDialog.SelectedPath + "\\" + fileName + ".xls";
        //        DoExport(listTypeName, Area, path, TypeName);
        //        if (flag != 1)
        //        {
        //            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //            Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(path);
        //            excel.Visible = true;
        //        }
        //    }
        //}
        private void DropTable(IWorkspace pWks, string strTableName)
        {
            try
            {
                pWks.ExecuteSQL("drop table " + strTableName);

            }
            catch
            { }
        }
        public void ExportbySQL(IFeatureClass pFeatureclass, IFeatureCursor pFeaturecursor, string fileName, string TypeName, string fieldName)
        {

            if (pFeatureclass == null)
            {
                return;
            }

            int intOID = -1; string strTableName = ""; string strOIDname = "";
            string strAreaName = ""; int intGroupField = -1;
            IWorkspace pWorkspace = null;
            try
            {
                pWorkspace = (pFeatureclass as IDataset).Workspace;
                strTableName = (pFeatureclass as IDataset).Name;
            }
            catch
            { }

            DropTable(pWorkspace, "tmpFieldName_ldtb");
            pWorkspace.ExecuteSQL("create table tmpFieldName_ldtb as select " + fieldName + " as englishname,'中文描述等等等等等等等等等等等等等等等等' as chinesename from " + strTableName + " where 1=0");


            for (int i = 0; i < pFeatureclass.Fields.FieldCount; i++)
            {
                IField pTmpfield = pFeatureclass.Fields.get_Field(i);
                if (pTmpfield.Type == esriFieldType.esriFieldTypeOID)
                {
                    intOID = i; strOIDname = pTmpfield.Name;

                }
                if (pTmpfield.Name.ToLower().Contains("shape") && pTmpfield.Name.ToLower().Contains("area"))
                {
                    strAreaName = pTmpfield.Name;
                }
                if (pTmpfield.Name.ToLower().Equals(fieldName.ToLower()))
                {
                    intGroupField = i;
                    IDomain domain = pTmpfield.Domain;
                    ICodedValueDomain codeDomain = domain as ICodedValueDomain;
                    if (codeDomain != null)
                    {
                        for (int j = 0; j < codeDomain.CodeCount; j++)
                        {
                            string strdomainvalue = codeDomain.get_Value(j).ToString();
                            string strdomainname = codeDomain.get_Name(j).ToString();
                            pWorkspace.ExecuteSQL("insert into tmpFieldName_ldtb(englishname,chinesename) values('" + strdomainvalue + "','" + strdomainname + "')");

                        }
                    }
                }
            }
            DropTable(pWorkspace, "tmpoid_ldtb");
            pWorkspace.ExecuteSQL("create table tmpoid_ldtb as select " + strOIDname + " from " + strTableName + " where 1=0");
            pWorkspace.ExecuteSQL("alter table tmpoid_ldtb add mj number(38,8)");
            IFeature pTmpfea = pFeaturecursor.NextFeature();

            while (pTmpfea != null)
            {
                string strTmpoid = pTmpfea.get_Value(intOID).ToString();
                double dTempArra = 0;
                double dTempLength = 0;
                try
                {

                    GetAreaAndLength(pTmpfea.ShapeCopy, ref dTempLength, ref dTempArra, pTmpfea.Shape.SpatialReference);

                }
                catch
                { }
                pWorkspace.ExecuteSQL("insert into tmpoid_ldtb(" + strOIDname + ",mj) values(" + strTmpoid + "," + dTempArra + ")");

                pTmpfea = pFeaturecursor.NextFeature();
            }
            DropTable(pWorkspace, "tmpFeaturetable_ldtb");
            pWorkspace.ExecuteSQL("create table tmpFeaturetable_ldtb as select b." + strOIDname + ",a." + fieldName + ",b.mj from " + strTableName + " a, tmpoid_ldtb b where a." + strOIDname + "=b." + strOIDname);
            DropTable(pWorkspace, "tmpStatistic_ldtb0");
            pWorkspace.ExecuteSQL("create table tmpStatistic_ldtb0 as select " + fieldName + ",sum(mj) as summj from tmpFeaturetable_ldtb where " + strOIDname + " in(select " + strOIDname + " from tmpoid_ldtb) group by " + fieldName);
            DropTable(pWorkspace, "tmpStatistic_ldtb");
            try
            {
                pWorkspace.ExecuteSQL("create table tmpStatistic_ldtb as select * from tmpStatistic_ldtb0 order by to_number(" + fieldName + ") asc");
            }
            catch
            {
                DropTable(pWorkspace, "tmpStatistic_ldtb");
                pWorkspace.ExecuteSQL("create table tmpStatistic_ldtb as select * from tmpStatistic_ldtb0 order by " + fieldName + " asc");
            }

            pWorkspace.ExecuteSQL("alter table tmpStatistic_ldtb add chinesename varchar2(100)");
            pWorkspace.ExecuteSQL("alter table tmpStatistic_ldtb add perofmj number(38,8)");
            pWorkspace.ExecuteSQL("alter table tmpFieldName_ldtb add primary key(englishname)");
            pWorkspace.ExecuteSQL("update (select a.chinesename as aname,b.chinesename as bname from  tmpStatistic_ldtb a,tmpFieldName_ldtb b where a." + fieldName + "=b.englishname ) set aname=bname");
            DropTable(pWorkspace, "tmpsummj_ldtb");
            pWorkspace.ExecuteSQL("create table tmpsummj_ldtb as select sum(summj) as summj0 from tmpStatistic_ldtb");
            ITable pTable = null; double summj = 0.1;
            try
            {
                pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tmpsummj_ldtb");
            }
            catch
            { }
            ICursor pCursor = null;
            IRow pRow = null;
            try
            {
                pCursor = pTable.Search(null, false);
                pRow = pCursor.NextRow();
            }
            catch
            { }
            if (pRow != null)
            {
                try
                {
                    summj = double.Parse(pRow.get_Value(0).ToString());
                }
                catch
                { }
            }
            pWorkspace.ExecuteSQL("update tmpStatistic_ldtb set perofmj=round(summj/" + summj + ",2)");
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            pCursor = null; pRow = null;
            pTable = null;
            try
            {
                pTable = (pWorkspace as IFeatureWorkspace).OpenTable("tmpStatistic_ldtb");
            }
            catch
            { }
            try
            {
                pCursor = pTable.Search(null, false);
                pRow = pCursor.NextRow();
            }
            catch
            { }
            DropTable(pWorkspace, "tmpFieldName_ldtb");
            DropTable(pWorkspace, "tmpStatistic_ldtb0");
            DropTable(pWorkspace, "tmpFeaturetable_ldtb");
            DropTable(pWorkspace, "tmpsummj_ldtb");
            DropTable(pWorkspace, "tmpoid_ldtb");

            int intGoupField = -1; int intMJfield = -1; int intPercentField = -1;
            if (pRow != null)
            {
                intGroupField = pRow.Fields.FindField("chinesename");
                intMJfield = pRow.Fields.FindField("summj");
                intPercentField = pRow.Fields.FindField("perofmj");
            }
            else
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
                return;
            }

            Microsoft.Office.Interop.Excel.Application excel = null;
            Workbook wb = null;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                if (excel == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                    return;
                }
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = true;

                excel.DefaultFilePath = "";
                excel.DisplayAlerts = true;
                excel.SheetsInNewWorkbook = 1;
                //表头
                excel.Cells[1, 1] = "序号";
                excel.Cells[1, 2] = TypeName;
                excel.Cells[1, 3] = "面积(平方米)";
                excel.Cells[1, 4] = "占地百分比(%)";
                int RowIndex = 1;
                while (pRow != null)
                {
                    RowIndex++;
                    excel.Cells[RowIndex, 1] = RowIndex - 1;
                    excel.Cells[RowIndex, 2] = pRow.get_Value(intGroupField).ToString();
                    excel.Cells[RowIndex, 3] = pRow.get_Value(intMJfield).ToString();
                    excel.Cells[RowIndex, 4] = pRow.get_Value(intPercentField).ToString();
                    pRow = pCursor.NextRow();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
                ///弹出对话保存生成统计表的路径
                Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                fd.InitialFileName = fileName + " 统计表";
                int result = fd.Show();
                if (result == 0) return;
                string fileName0 = fd.InitialFileName;
                if (!string.IsNullOrEmpty(fileName0))
                {
                    if (fileName0.IndexOf(".xls") == -1)
                    {
                        fileName0 += ".xls";
                    }
                    ///保存生成的统计表
                    wb.SaveAs(fileName0, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                GC.Collect();
            }
            catch { }
            DropTable(pWorkspace, "tmpStatistic_ldtb");
        }
        /// <summary>
        /// 执行导出主函数
        /// </summary>
        /// <param name="pFeatureClass">目标FeatureClass</param>
        /// <param name="fileName">导出成EXCEL文件名</param>
        /// <param name="TypeName">表头中类型名称</param>
        public void Export(IFeatureClass pFeatureclass, IFeatureCursor pFeaturecursor, string fileName, string TypeName, string fieldName)
        {

            IList<string> liststring = GetType(pFeatureclass, fieldName);
            sortListString(ref liststring);
            IList<double> listArea = StatisticsArea(liststring, pFeaturecursor, fieldName);
            IList<string> ListTypeName = null;

            ListTypeName = GetDomainsName(liststring, fieldName, pFeatureclass);
            //ExportExcel(ListTypeName, listArea, fileName, TypeName);

            //string path = System.Windows.Forms.Application.StartupPath + "\\Temp\\" + fileName + ".xls";
            //if (!System.IO.Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\Temp"))
            //{
            //    System.IO.Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\Temp");
            //}
            DoExport(ListTypeName, listArea, fileName, TypeName);

            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(path);
            //excel.Visible = true;


        }
        /// <summary>
        /// 执行导出Table主函数
        /// </summary>
        /// <param name="pFeatureClass">目标FeatureClass</param>
        /// <param name="fileName">导出成EXCEL文件名</param>
        /// <param name="TypeName">表头中类型名称</param>
        public void XZQExport(IFeatureClass pFeatureclass, IFeatureCursor pFeaturecursor, string fileName, string TypeName, string fieldName)
        {

        }
        /// <summary>
        /// 林地规划和林地利用分布统计
        /// ygc 2012-8-15
        /// </summary>
        /// <param name="pFeatureclass"></param>
        /// <param name="pFeaturecursor"></param>
        /// <param name="fileName"></param>
        /// <param name="TypeName"></param>
        /// <param name="fieldName"></param>
        /// <param name="pDataGV"></param>
        public void XZQExportLD(IFeatureClass pFeatureclass, IFeatureCursor pFeaturecursor, string fileName, string TypeName, string fieldName)
        {
           

        }
        //排序
        private void sortListString(ref IList<string> pList)
        {
            if (pList == null)
            {
                return;
            }
            int index = -1;
            int intCurValue = 0;
            for (int i = 0; i < pList.Count - 1; i++)
            {
                intCurValue = 0;
                try
                {
                    intCurValue = int.Parse(pList[i]);
                }
                catch
                { }
                for (int j = i + 1; j < pList.Count; j++)
                {
                    int intTmp = 0;
                    try
                    {
                        intTmp = int.Parse(pList[j]);
                    }
                    catch
                    { }
                    if (intCurValue > intTmp)
                    {
                        index = j;
                        intCurValue = intTmp;
                    }
                }
                if (index != -1 && index != i)
                {
                    string strtmp = pList[i];
                    pList[i] = pList[index];
                    pList[index] = strtmp;
                }
            }
        }
        /// <summary>
        /// 得到属性域的值
        /// </summary>
        /// <param name="TypeName">表字段名称</param>
        /// <param name="pFeatureClass">特征类</param>
        /// <returns>属性域字段</returns>
        private IList<string> GetDomainsName(IList<string> listfieldvalue, string fieldName, IFeatureClass pFeatureClass)
        {
            IList<string> newList = new List<string>();
            for (int i = 0; i < listfieldvalue.Count; i++)
            {
                newList.Add(listfieldvalue[i]);
            }
            int indexField = pFeatureClass.Fields.FindField(fieldName);
            IField pField = pFeatureClass.Fields.get_Field(indexField);
            IDomain domain = pField.Domain;
            ICodedValueDomain codeDomain = domain as ICodedValueDomain;
            if (codeDomain == null) { return newList; }
            for (int i = 0; i < codeDomain.CodeCount; i++)
            {
                string strdomain = codeDomain.get_Value(i).ToString();
                for (int j = 0; j < listfieldvalue.Count; j++)
                {
                    if (strdomain.Equals(listfieldvalue[j]))
                    {
                        newList[j] = codeDomain.get_Name(i).ToString();
                    }
                }
            }
            return newList;
        }
        /// <summary>
        /// 执行导出主函数
        /// </summary>
        /// <param name="pFeatureClass">目标FeatureClass</param>
        /// <param name="fileName">导出成EXCEL文件名</param>
        /// <param name="TypeName">表头中类型名称</param>
        public void ExportLDFB(IFeatureClass pFeatureclass, IFeatureCursor pFeaturecursor, string fileName, string TypeName, string fieldName)
        {
            //if (flag != 1)
            //{
            //    result = folderBrowerDialog.ShowDialog();
            //}
            IList<string> liststring = GetType(pFeatureclass, fieldName);
            sortListString(ref liststring);
            IList<double> listArea = StatisticsArea(liststring, pFeaturecursor, fieldName);
            IList<string> ListTypeName = new List<string>();

            ListTypeName = GetDomainsName(liststring, fieldName, pFeatureclass);
            sortListString(ref liststring);

            //listString = SortList(GetType(pFeatureclass, fieldName));
            DoForestryExport(ListTypeName, listArea, fileName, TypeName, liststring);
        }
        /// <summary>
        /// 执行导出DataTable主函数
        /// </summary>
        /// <param name="pFeatureClass">目标FeatureClass</param>
        /// <param name="fileName">导出成EXCEL文件名</param>
        /// <param name="TypeName">表头中类型名称</param>
        public void XZQExportLDFB(IFeatureClass pFeatureclass, IFeatureCursor pFeaturecursor, string fileName, string TypeName, string fieldName,DevExpress.XtraGrid.Views.Grid.GridView pDataGV)
        {
       
        }

        //找出林地与非林地相关面积
        //ygc 2012-8-14
        private void TJLingDI(IList<double> listArea, IList<string> listTypeName, out double LDarea, out double FLDarea)
        {
            FLDarea = 0; LDarea = 0;

            for (int i = 0; i < listTypeName.Count; i++)
            {
                if (Convert.ToInt16(listTypeName[i])>= 111 && Convert.ToInt16(listTypeName[i]) <= 180)
                {
                    LDarea = LDarea + listArea[i];
                }
                else
                {
                    FLDarea = FLDarea + listArea[i];
                }
            }
        }
        private void ClassArea(IList<double> listArea, IList<string> listTypeName, out Dictionary<string, double> dicClassArea,string fileName)
        {
              dicClassArea = InitialDic(fileName);
            switch (fileName)
            {
                case "林地规划统计":
            for (int i = 0; i < listTypeName.Count; i++)
            {
                if (Convert.ToInt16(listTypeName[i]) >= 111 && Convert.ToInt16(listTypeName[i]) <= 114)
                {
                    dicClassArea["有林地"] = dicClassArea["有林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) == 120)
                {
                    dicClassArea["疏林地"] = dicClassArea["疏林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) >= 131 && Convert.ToInt16(listTypeName[i]) <= 132)
                {
                    dicClassArea["灌木林地"] = dicClassArea["灌木林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) >= 141 && Convert.ToInt16(listTypeName[i]) <= 180)
                {
                    dicClassArea["其他林地"] = dicClassArea["其他林地"] + listArea[i];
                }
            }
            break;
                case "林地利用分布统计":
            for (int i = 0; i < listTypeName.Count; i++)
            {
                if (Convert.ToInt16(listTypeName[i]) >= 111 && Convert.ToInt16(listTypeName[i]) <= 114)
                {
                    dicClassArea["有林地"] = dicClassArea["有林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) == 120)
                {
                    dicClassArea["疏林地"] = dicClassArea["疏林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) >= 131 && Convert.ToInt16(listTypeName[i]) <= 132)
                {
                    dicClassArea["灌木林地"] = dicClassArea["灌木林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) >= 141 && Convert.ToInt16(listTypeName[i]) <= 142)
                {
                    dicClassArea["未成林地"] = dicClassArea["未成林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) == 150)
                {
                    dicClassArea["苗圃地"] = dicClassArea["苗圃地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) >= 161 && Convert.ToInt16(listTypeName[i]) <= 163)
                {
                    dicClassArea["无立木林地"] = dicClassArea["无立木林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) >= 171 && Convert.ToInt16(listTypeName[i]) <= 174)
                {
                    dicClassArea["宜林地"] = dicClassArea["宜林地"] + listArea[i];
                }
                else if (Convert.ToInt16(listTypeName[i]) <= 180)
                {
                    dicClassArea["林业辅助生产用地"] = dicClassArea["林业辅助生产用地"] + listArea[i];
                }
            }
            break;
                case "林场用地分布统计":
            for (int i = 0; i < listTypeName.Count;i++ )
            {
                if (listTypeName[i] == "149101004") dicClassArea["落阵营林场"] = dicClassArea["落阵营林场"] + listArea[i];
                else if (listTypeName[i] == "149101003") dicClassArea["金沙滩林场"] = dicClassArea["金沙滩林场"] + listArea[i];
                else if (listTypeName[i] == "149101005") dicClassArea["九梁洼林场"] = dicClassArea["九梁洼林场"] + listArea[i];
                else if (listTypeName[i] == "149101006") dicClassArea["云西林场"] = dicClassArea["云西林场"] + listArea[i];
                else if (listTypeName[i] == "149101007") dicClassArea["御河林场"] = dicClassArea["御河林场"] + listArea[i];
                else if (listTypeName[i] == "149101008") dicClassArea["五旗林场"] = dicClassArea["五旗林场"] + listArea[i];
                else if (listTypeName[i] == "149103010") dicClassArea["王庄堡林场"] = dicClassArea["王庄堡林场"] + listArea[i];
                else if (listTypeName[i] == "149103012") dicClassArea["上寨林场"] = dicClassArea["上寨林场"] + listArea[i];
                else if (listTypeName[i] == "149201001") dicClassArea["大同市十里河林场"] = dicClassArea["大同市十里河林场"] + listArea[i];
                else if (listTypeName[i] == "149201002") dicClassArea["大同市恒山林场"] = dicClassArea["大同市恒山林场"] + listArea[i];
                else if (listTypeName[i] == "149201003") dicClassArea["大同市长城山林场"] = dicClassArea["大同市长城山林场"] + listArea[i];
                else if (listTypeName[i] == "149201004") dicClassArea["大同市桦林背林场"] = dicClassArea["大同市桦林背林场"] + listArea[i];
                else if (listTypeName[i] == "149201005") dicClassArea["大同市镇川矿柱林场"] = dicClassArea["大同市镇川矿柱林场"] + listArea[i];
                else if (listTypeName[i] == "149201006") dicClassArea["大同市窑山林场"] = dicClassArea["大同市窑山林场"] + listArea[i];
                else if (listTypeName[i] == "149201007") dicClassArea["大同市马铺山林场"] = dicClassArea["大同市马铺山林场"] + listArea[i];
                else if (listTypeName[i] == "149202001") dicClassArea["灵丘县林场"] = dicClassArea["灵丘县林场"] + listArea[i];
                else if (listTypeName[i] == "149202002") dicClassArea["广灵县白羊峪林场"] = dicClassArea["广灵县白羊峪林场"] + listArea[i];
                else if (listTypeName[i] == "149202003") dicClassArea["天镇县黑龙寺林场"] = dicClassArea["天镇县黑龙寺林场"] + listArea[i];
                else if (listTypeName[i] == "149301002" || listTypeName[i] == "149303001") dicClassArea["灵丘林场"] = dicClassArea["灵丘林场"] + listArea[i];
            }
                    break;
        }
        }
        private Dictionary<string, double> InitialDic(string fileName)
        {
            Dictionary<string, double> dicArea = new Dictionary<string, double>();
            switch (fileName)
            {
                case "林地规划统计":
                    dicArea.Add("有林地", 0);
                    dicArea.Add("疏林地", 0);
                    dicArea.Add("灌木林地", 0);
                    dicArea.Add("其他林地", 0);
                    break;
                case "林地利用分布统计":
                    dicArea.Add("有林地", 0);
                    dicArea.Add("疏林地", 0);
                    dicArea.Add("灌木林地", 0);
                    dicArea.Add("未成林地", 0);
                    dicArea.Add("苗圃地", 0);
                    dicArea.Add("无立木林地", 0);
                    dicArea.Add("宜林地", 0);
                    dicArea.Add("林业辅助生产用地", 0);
                    break;
                case "林场用地分布统计":
                    dicArea.Add("落阵营林场", 0);
                    dicArea.Add("金沙滩林场", 0);
                    dicArea.Add("九梁洼林场", 0);
                    dicArea.Add("云西林场", 0);
                    dicArea.Add("御河林场", 0);
                    dicArea.Add("五旗林场", 0);
                    dicArea.Add("王庄堡林场", 0);
                    dicArea.Add("上寨林场", 0);
                    dicArea.Add("大同市十里河林场", 0);
                    dicArea.Add("大同市恒山林场", 0);
                    dicArea.Add("大同市长城山林场", 0);
                    dicArea.Add("大同市桦林背林场", 0);
                    dicArea.Add("大同市窑山林场", 0);
                    dicArea.Add("大同市马铺山林场", 0);
                    dicArea.Add("灵丘县林场", 0);
                    dicArea.Add("广灵县白羊峪林场", 0);
                    dicArea.Add("天镇县黑龙寺林场", 0);
                    dicArea.Add("灵丘林场", 0);
                    dicArea.Add("大同市镇川矿柱林场",0);
                    break;
            }
            return dicArea;
        }
        /// <summary>
        /// 林用地分布统计
        /// </summary>
        /// <param name="listTypeName"></param>
        /// <param name="listArea"></param>
        /// <param name="filePath"></param>
        /// <param name="TypeName"></param>
        private void DoForestryExport(IList<string> listTypeName, IList<double> listArea, string filePath, string TypeName, IList<string> listString)
        {
            int RowNum = listArea.Count;
            int RowIndex = 1;
            double Total = GetTotalArea(listArea);
            double LDarea = 0, FLDarea = 0;
            TJLingDI(listArea, listString, out LDarea, out FLDarea);
            IList<string> listPercent = null;
            listPercent = ListPercent(listArea);
            if (RowNum == 0)
            {
                return;
            }

            else if (RowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application excel = null;
                Workbook wb = null;
                try
                {
                    excel = new Microsoft.Office.Interop.Excel.Application();
                    if (excel == null)
                    {
                        MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                        return;
                    }
                    wb = excel.Application.Workbooks.Add(true);
                    excel.Visible = true;
                    excel.DefaultFilePath = "";
                    excel.WindowState = XlWindowState.xlNormal;
                    excel.DisplayAlerts = true;
                    excel.SheetsInNewWorkbook = 1;
                    //表头
                    excel.Cells[1, 1] = "序号";
                    excel.Cells[1, 2] = "林地类型";
                    excel.Cells[1, 3] = TypeName;
                    excel.Cells[1, 4] = "面积(平方米)";
                    excel.Cells[1, 5] = "占地百分比(%)";
                    for (int i = 0; i < RowNum; i++)
                    {
                        RowIndex++;
                        excel.Cells[RowIndex, 1] = i;
                        if (Convert.ToInt16(listString[i]) >= 111 && Convert.ToInt16(listString[i]) <= 180)
                        {
                            excel.Cells[RowIndex, 2] = "林地";
                        }
                        else
                        {
                            excel.Cells[RowIndex, 2] = "非林地";
                        }
                        excel.Cells[RowIndex, 3] = listTypeName[i];
                        excel.Cells[RowIndex, 4] = listArea[i].ToString();
                        excel.Cells[RowIndex, 5] = listPercent[i];
                    }
                    excel.Cells[RowNum + 4, 2] = "林地总面积";
                    excel.Cells[RowNum + 5, 2] = "非林地总面积";
                    excel.Cells[RowNum + 4, 4] = LDarea.ToString();
                    excel.Cells[RowNum + 4, 5] = Math.Round((LDarea / Total) * 100, 2).ToString() + "%";
                    excel.Cells[RowNum + 5, 4] = FLDarea.ToString();
                    excel.Cells[RowNum + 5, 5] = Math.Round((FLDarea / Total) * 100, 2).ToString() + "%";
                    ///弹出对话保存生成统计表的路径
                    Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                    fd.InitialFileName = filePath + " 统计表";
                    int result = fd.Show();
                    if (result == 0) return;
                    string fileName = fd.InitialFileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.IndexOf(".xls") == -1)
                        {
                            fileName += ".xls";
                        }
                        ///保存生成的统计表
                        wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                    GC.Collect();
                }
                catch { }
            }
        }
    }
}
