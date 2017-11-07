using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Excel;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using ESRI.ArcGIS.Carto;

namespace GeoDataManagerFrame
{
    public class ExportLDFBToExcel
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]

        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        //FolderBrowserDialog folderBrowerDialog = new FolderBrowserDialog();
        IList<string> listString = new List<string>();
        double LD = 0;
        double FLD = 0;
        //int flag;
        //DialogResult result = new DialogResult();
        //导出结束时，关闭EXCEL进程
        private static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            //IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

            int k = 0;
            GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k);   //得到本进程唯一标志k
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
            p.Kill();     //关闭进程k
        }
        /// <summary>
        /// 得到各个统计值的百分比
        /// </summary>
        /// <param name="listArea"></param>
        /// <returns></returns>
        private IList<double> ListPercent(IList<double> listArea)
        {
            IList<double> newList = new List<double>();
            double total = GetTotalArea(listArea);
            for (int i = 0; i < listArea.Count; i++)
            {
                newList.Add(Math.Round((listArea[i] / total) * 100, 2));
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
        private void DoExport(IList<string> listTypeName, IList<double> listArea, string filePath, string TypeName)
        {
            int RowNum = listArea.Count;
            int RowIndex = 1;
            IList<double> listPercent = new List<double>();
            listPercent = ListPercent(listArea);
            if (RowNum == 0 || string.IsNullOrEmpty(filePath))
            {
                return;
            }
            else if (RowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                if (xlapp == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                    return;
                }
                xlapp.DefaultFilePath = "";
                xlapp.DisplayAlerts = true;
                xlapp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlapp.Workbooks.Add(true);
                //表头
                xlapp.Cells[1, 1] = "序号";
                xlapp.Cells[1, 2] = TypeName;
                xlapp.Cells[1, 3] = "面积(平方米)";
                xlapp.Cells[1, 4] = "占地百分比";
                for (int i = 0; i < RowNum; i++)
                {
                    RowIndex++;
                    xlapp.Cells[RowIndex, 1] = i;
                    xlapp.Cells[RowIndex, 2] = listTypeName[i];
                    xlapp.Cells[RowIndex, 3] = listArea[i].ToString();
                    xlapp.Cells[RowIndex, 4] = listPercent[i].ToString() + "%";
                }
                xlBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlBook = null;
                Kill(xlapp);
            }
        }
        /// <summary>
        /// 林用地分布统计
        /// </summary>
        /// <param name="listTypeName"></param>
        /// <param name="listArea"></param>
        /// <param name="filePath"></param>
        /// <param name="TypeName"></param>
        private void DoForestryExport(IList<string> listTypeName, IList<double> listArea, string filePath, string TypeName)
        {
            int RowNum = listArea.Count;
            int RowIndex = 1;
            double Total = GetTotalArea(listArea);
            TJLingDI(listArea, listString);
            IList<double> listPercent = new List<double>();
            listPercent = ListPercent(listArea);
            if (RowNum == 0 || string.IsNullOrEmpty(filePath))
            {
                return;
            }
            else if (RowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                if (xlapp == null)
                {
                    MessageBox.Show("无法创建excel对象，可能您的系统没有安装excel", "提示");
                    return;
                }
                xlapp.DefaultFilePath = "";
                xlapp.DisplayAlerts = true;
                xlapp.SheetsInNewWorkbook = 1;
                Microsoft.Office.Interop.Excel.Workbook xlBook = xlapp.Workbooks.Add(true);
                //表头
                xlapp.Cells[1, 1] = "序号";
                xlapp.Cells[1, 2] = "林地类型";
                xlapp.Cells[1, 3] = TypeName;
                xlapp.Cells[1, 4] = "面积(平方米)";
                xlapp.Cells[1, 5] = "占地百分比(%)";
                for (int i = 0; i < RowNum; i++)
                {
                    RowIndex++;
                    xlapp.Cells[RowIndex, 1] = i;
                    if (Convert.ToInt16(listString[i]) >= 111 && Convert.ToInt16(listString[i]) <= 180)
                    {
                        xlapp.Cells[RowIndex, 2] = "林地";
                    }
                    else
                    {
                        xlapp.Cells[RowIndex, 2] = "非林地";
                    }
                    xlapp.Cells[RowIndex, 3] = listTypeName[i];
                    xlapp.Cells[RowIndex, 4] = listArea[i].ToString();
                    xlapp.Cells[RowIndex, 5] = listPercent[i].ToString() + "%";
                }
                xlapp.Cells[RowNum + 4, 2] = "林地总面积";
                xlapp.Cells[RowNum + 5, 2] = "非林地总面积";
                xlapp.Cells[RowNum + 4, 4] = LD.ToString();
                xlapp.Cells[RowNum + 4, 5] = Math.Round((LD / Total) * 100, 2).ToString() + "%";
                xlapp.Cells[RowNum + 5, 4] = FLD.ToString();
                xlapp.Cells[RowNum + 5, 5] = Math.Round((FLD / Total) * 100, 2).ToString() + "%";
                xlBook.SaveAs(filePath, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlBook = null;
                Kill(xlapp);
            }
        }
        //根据指定字段统计面积
        private IList<double> StatisticsArea(IList<string> ListTypeName, IFeatureClass pFeatureClass, string fieldName)
        {
            IList<double> newList = new List<double>();
            IQueryFilter pQueryFilter = new QueryFilterClass();
            int indexField = -1;
            IFields pFields = pFeatureClass.Fields;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                IField pTmpFiled = pFields.get_Field(i);
                if (pTmpFiled.Name.ToLower().Contains("shape") && pTmpFiled.Name.ToLower().Contains("area"))
                {
                    indexField = i;
                    break;
                }
            }
            //pFeatureClass.FindField("Area"); 

            for (int i = 0; i < ListTypeName.Count; i++)
            {
                double temp = 0;

                pQueryFilter.WhereClause = fieldName + "='" + ListTypeName[i] + "'";
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(pQueryFilter, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    temp = temp + Convert.ToDouble(pFeature.get_Value(indexField));
                    pFeature = pFeatureCursor.NextFeature();
                }
                newList.Add(temp);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                pFeatureCursor = null;
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pQueryFilter);
            pQueryFilter = null;
            return newList;
        }
        //获取统计字段
        private IList<string> GetType(IFeatureClass pFeatureClass, string TypeName)
        {
            IList<string> ListType = new List<string>();
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
            IDataStatistics pData = new DataStatisticsClass();
            pData.Field = TypeName;
            pData.Cursor = pFeatureCursor as ICursor;
            IEnumerator pEnumeraVar = pData.UniqueValues;
            int count = pData.UniqueValueCount;
            pEnumeraVar.Reset();
            while (pEnumeraVar.MoveNext())
            {
                if (pEnumeraVar.Current.ToString() != "")
                {
                    ListType.Add(pEnumeraVar.Current.ToString());
                }
            }
            //手动释放Cursor
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            return ListType;
        }
        ///// <summary>
        ///// 设定导出路径，导出成EXCEl文件
        ///// </summary>
        ///// <param name="listTypeName"></param>
        ///// <param name="Area"></param>
        ///// <param name="fileName">EXCEL文件名</param>
        ///// <param name="TypeName">表头中统计类型名</param>
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
        private void ExportExcelForestry(IList<string> listTypeName, IList<double> Area, string fileName, string TypeName)
        {
            //if (result == DialogResult.OK)
            //{
                //string path = folderBrowerDialog.SelectedPath + "\\" + fileName + ".xls";
                DoForestryExport(listTypeName, Area, path, TypeName);
                //if (flag != 1)
                //{
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(path);
                    excel.Visible = true;
                //}
            //}
        }
        /// <summary>
        /// 执行导出主函数
        /// </summary>
        /// <param name="pFeatureClass">目标FeatureClass</param>
        /// <param name="fileName">导出成EXCEL文件名</param>
        /// <param name="TypeName">表头中类型名称</param>
        public void Export(IFeatureClass pFeatureClass, string fileName, string TypeName, string fieldName)
        {
            //if (flag != 1)
            //{
            //    result = folderBrowerDialog.ShowDialog();
            //}

            listString = GetType(pFeatureClass, fieldName);
            listString = SortList(listString);
            IList<double> listArea = StatisticsArea(listString, pFeatureClass, fieldName);
            IList<string> ListTypeName = new List<string>();

            ListTypeName = GetDomainsName(fieldName, pFeatureClass);
            listString = SortList(GetType(pFeatureClass, fieldName));
            DoForestryExport(listTypeName, Area, path, TypeName);
            //if (flag != 1)
            //{
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook book = excel.Application.Workbooks.Add(path);
            excel.Visible = true;
            //ExportExcelForestry(ListTypeName, listArea, fileName, TypeName);


            listString.Clear();
        }
        ///// <summary>
        ///// 批量专题导出Excel
        ///// </summary>
        ///// <param name="pFeatureClass"></param>
        ///// <param name="listFileName"></param>
        ///// <param name="TypeName"></param>
        //public void BatchSubjectExport(IFeatureClass pFeatureClass, IList<string> listFileName, string TypeName, string fieldName)
        //{
        //    flag = 1;
        //    result = folderBrowerDialog.ShowDialog();
        //    for (int i = 0; i < listFileName.Count; i++)
        //    {
        //        Export(pFeatureClass, listFileName[i], TypeName, fieldName);
        //    }
        //    flag = 0;
        //}
        ///// <summary>
        ///// 批量导出该行政去内各种类型Excel
        ///// </summary>
        ///// <param name="ListFeatureClass"></param>
        ///// <param name="VillageName">行政区名称</param>
        ///// <param name="fieldName">统计字段名称</param>
        //public void BatchStatisticalChart(IList<IFeatureClass> ListFeatureClass, string VillageName, string fieldName)
        //{

        //    flag = 1;
        //    result = folderBrowerDialog.ShowDialog();
        //    for (int i = 0; i < ListFeatureClass.Count; i++)
        //    {
        //        ExportByFeartureClassName(ListFeatureClass[i], VillageName, fieldName);
        //    }
        //    flag = 0;
        //}
        ///// <summary>
        ///// 根据FeatureClass别名进行统计导出Excel表
        ///// </summary>
        ///// <param name="pFeatureClass"></param>
        ///// <param name="VillageName"></param>
        ///// <param name="fieldName"></param>
        //private void ExportByFeartureClassName(IFeatureClass pFeatureClass, string VillageName, string fieldName)
        //{
        //    switch (pFeatureClass.AliasName)
        //    {
        //        case "DATASYS.DATONG_LDGH":
        //            string fileName = VillageName + "镇(乡)林地规划统计表";
        //            Export(pFeatureClass, fileName, "林地类型", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_LYYDFB":
        //            fileName = VillageName + "镇(乡)林业用地分布统计表";
        //            Export(pFeatureClass, fileName, "类型", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_BAOHU_DJ":
        //            fileName = VillageName + "镇(乡)林地保护等级分布统计表";
        //            Export(pFeatureClass, fileName, "林地保护等级级别", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_ZDGNQ":
        //            fileName = VillageName + "镇(乡)林地主导功能区分布统计表";
        //            Export(pFeatureClass, fileName, "主导功能区类型", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_LDLY":
        //            fileName = VillageName + "镇(乡)林地利用现状统计表";
        //            Export(pFeatureClass, fileName, "林地利用现状类型", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_LDJG":
        //            fileName = VillageName + "镇(乡)林地结构现状统计表";
        //            Export(pFeatureClass, fileName, "林地结构类型", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_ZHILIANG_DJ":
        //            fileName = VillageName + "镇(乡)林地质量等级现状统计表";
        //            Export(pFeatureClass, fileName, "林地质量等级类型", fieldName);
        //            fileName = "";
        //            break;
        //        case "DATASYS.DATONG_LC":
        //            fileName = VillageName + "镇(乡)林场统计表";
        //            Export(pFeatureClass, fileName, "林场名称", fieldName);
        //            fileName = "";
        //            break;
        //    }
        //}
        /// <summary>
        /// 得到属性域的值
        /// </summary>
        /// <param name="TypeName">表字段名称</param>
        /// <param name="pFeatureClass">特征类</param>
        /// <returns>属性域字段</returns>
        private IList<string> GetDomainsName(string fieldName, IFeatureClass pFeatureClass)
        {
            IList<string> newList = new List<string>();
            newList = listString;
            IDataset dataset = pFeatureClass as IDataset;
            IWorkspace workspace = dataset.Workspace;
            IWorkspaceDomains workspaceDomains = (IWorkspaceDomains)workspace;
            int indexField = pFeatureClass.Fields.FindField(fieldName);
            IField pField = pFeatureClass.Fields.get_Field(indexField);
            IDomain domain = pField.Domain;
            ICodedValueDomain codeDomain = (ICodedValueDomain)domain;
            for (int i = 0; i < codeDomain.CodeCount; i++)
            {
                for (int j = 0; j < listString.Count; j++)
                {
                    if (listString[j] == codeDomain.get_Value(i).ToString())
                    {
                        newList[j] = codeDomain.get_Name(i).ToString();
                    }
                }

            }
            return newList;
        }
        private void TJLingDI(IList<double> listArea, IList<string> listTypeName)
        {
            for (int i = 0; i < listTypeName.Count; i++)
            {
                if (Convert.ToInt16(listTypeName[i]) >= 111 && Convert.ToInt16(listTypeName[i]) <= 180)
                {
                    LD = LD + listArea[i];
                }
                else
                {
                    FLD = FLD + listArea[i];
                }
            }
        }
        private IList<string> SortList(IList<string> list)
        {
            bool isExchanged = false;
            for (int i = 0; i < list.Count - 1; i++)
            {
                isExchanged = false;
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (Convert.ToInt16(list[j]) < Convert.ToInt16(list[j - 1]))
                    {
                        string temp = list[j];
                        list[j] = list[j - 1];
                        list[j - 1] = temp;
                        isExchanged = true;
                    }
                }
                if (!isExchanged)//一遍比较过后如果没有进行交换则退出循环
                    break;
            }
            return list;
        }

    }
}
