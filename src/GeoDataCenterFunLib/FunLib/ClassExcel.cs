using System;
using System.Collections.Generic;

using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace GeoDataCenterFunLib
{
    public class ExcelHelper
    {
        public Excel._Application excelApp;
        private string fileName = string.Empty;
        public Excel.WorkbookClass wbclass;       

        public ExcelHelper(string _filename)
        {
            excelApp = new Excel.Application();
            object objOpt = System.Reflection.Missing.Value; 
            wbclass = (Excel.WorkbookClass)excelApp.Workbooks.Open(_filename, objOpt, false, objOpt, objOpt, objOpt, true, objOpt, objOpt, true, objOpt, objOpt, objOpt, objOpt, objOpt);
      
        }
        /**/
        /// <summary>
        /// 所有sheet的名称列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetSheetNames()
        {
            List<string> list = new List<string>();
            Excel.Sheets sheets = wbclass.Worksheets;
            string sheetNams = string.Empty;
            foreach (Excel.Worksheet sheet in sheets)
            {
                list.Add(sheet.Name);
            }
            return list;
        }
        public Excel.Worksheet GetWorksheetByName(string name)
        {
            Excel.Worksheet sheet = null;
            Excel.Sheets sheets = wbclass.Worksheets;
            foreach (Excel.Worksheet s in sheets)
            {
                if (s.Name == name)
                {
                    sheet = s;
                    break;
                }
            }
            return sheet;
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheetName">sheet名称</param>
        /// <returns></returns>
        public Array GetContent(string sheetName,string cell1,string cell2)
        {
            Excel.Worksheet sheet = GetWorksheetByName(sheetName);
            //获取A1 到AM24范围的单元格
            Excel.Range rang = sheet.get_Range(cell1, cell2);
            //读一个单元格内容
            //sheet.get_Range("A1", Type.Missing);
            //不为空的区域,列,行数目
            //   int l = sheet.UsedRange.Columns.Count;
            // int w = sheet.UsedRange.Rows.Count;
            //  object[,] dell = sheet.UsedRange.get_Value(Missing.Value) as object[,];
            System.Array values = (Array)rang.Cells.Value2;
            return values;
        }
        /// <summary>
        /// 根据sheetRange循环指定的列，得到该值对应的行
        /// </summary>
        /// <param name="range">范围</param>
        /// <param name="value">值</param>
        /// <param name="Column">列</param>
        /// <returns></returns>
        public int GetCRFromValue(Excel.Range range,string value, int Column)
        {
            for (int i = 0; i <= range.Rows.Count; i++)
            {
                string strtext=((Excel.Range)range.Cells[i+1,Column]).Text.ToString();
                if (strtext == value)
                {
                    return i+1;
                }
                
            }
            return -1;
        }

        public void Close()
        {
            IntPtr T = new IntPtr(excelApp.Hwnd);
            int k = 0;
            SysCommon.ModExcel.GetWindowThreadProcessId(T, out k);
            wbclass.Close(false, null, null);
            excelApp.Application.Quit();
            try
            {
                excelApp.Quit();
            }
            catch
            { }

            excelApp = null;
            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                p.Kill();
            }
            catch
            { }
        }
        

    }

}
