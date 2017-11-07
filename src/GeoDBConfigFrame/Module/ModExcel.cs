using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
namespace GeoDBConfigFrame
{
    public static class ModExcel
    {
        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            //IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 

            int k = 0;
            GetWindowThreadProcessId(new IntPtr(excel.Hwnd), out k);   //得到本进程唯一标志k
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
            p.Kill();     //关闭进程k
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        public static bool ExportTableToExcel(IWorkspace pWorkSpace, string strTableName, string ExcelFileName)
        {
            return ExportTableToExcel( pWorkSpace, strTableName, ExcelFileName,null);
        }
        public static bool ExportTableToExcel(IWorkspace pWorkSpace,string strTableName,string ExcelFileName,SysCommon.CProgress vProgress)
        {
            if (pWorkSpace == null)
            {
                return false;
            }
            if (strTableName == "")
            {
                return false;
            }
            if (ExcelFileName == "")
            {
                return false;
            }
            IFeatureWorkspace pFeaWorkSpace = pWorkSpace as IFeatureWorkspace;
            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            //建立Excel对象
            excel = new Microsoft.Office.Interop.Excel.Application();
            wb = excel.Application.Workbooks.Add(true);
            excel.Visible = false;
            wb.Application.ActiveWindow.Caption = strTableName;
            int iRow = 1;

            ITable pTable = null;
            try
            {
                pTable = pFeaWorkSpace.OpenTable(strTableName);
            }
            catch (System.Exception ex)
            {            	
            }
            if (pTable == null)
            {
                return false;
            }
            WriteTableStruToExcel(excel, pTable, iRow);
            iRow = iRow + 1;
            ICursor pCursor = null;
            pCursor = pTable.Search(null, false);
            int RowCnt = pTable.RowCount(null)+1;
            if (vProgress != null)
            {
                vProgress.MaxValue = RowCnt;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在导出记录...");
            }            
            if (pCursor == null)
            {
                return false;
            }
            IRow pRow = pCursor.NextRow();
            while (pRow != null)
            {
                if (vProgress != null)
                {
                    if (vProgress.UserAskCancel)
                    {
                        wb.Saved = false;
                        excel.Workbooks.Close();
                        excel.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                        ModExcel.Kill(excel);
                        GC.Collect();
                        return false;
                    }
                    vProgress.PerformStep();
                }               
                
                WriteRowToExcel(excel, pRow, iRow);
                pRow = pCursor.NextRow();
                iRow = iRow + 1;
            }
            try
            {
                wb.SaveAs(ExcelFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (System.Exception ex)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                ModExcel.Kill(excel);
                GC.Collect();
                return false;
            }
            excel.Workbooks.Close();
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            ModExcel.Kill(excel);
            GC.Collect();
            return true;
        }
        //将图层信息写到导出的flexcell中
        private static void WriteTableStruToExcel(Microsoft.Office.Interop.Excel.Application pExcel, ITable pTable, int iRow)
        {
            if (pExcel == null)
                return;
            if (pTable == null)
                return;
            for (int i = 0; i < pTable.Fields.FieldCount; i++)
            {
                pExcel.Cells[iRow, i + 1] = pTable.Fields.get_Field(i).AliasName;
            }
        }
        //将Row信息写到导出的excel中
        private static void WriteRowToExcel(Microsoft.Office.Interop.Excel.Application pExcel, IRow pRow, int iRow)
        {
            if (pExcel == null)
                return;
            if (pRow == null)
                return;
            //写属性值
            for (int i = 0; i < pRow.Fields.FieldCount; i++)
            {
                esriFieldType pType = pRow.Fields.get_Field(i).Type;
                //Geometry字段单独处理
                if (pType == esriFieldType.esriFieldTypeGeometry)
                {
                    pExcel.Cells[iRow, i + 1] = "SHAPE";
                }
                else if (pType == esriFieldType.esriFieldTypeString)
                {
                    pExcel.Cells[iRow, i + 1] = "'" + pRow.get_Value(i).ToString();
                }
                else
                {
                    pExcel.Cells[iRow, i + 1] = pRow.get_Value(i).ToString();
                }
            }
        }
    }
}