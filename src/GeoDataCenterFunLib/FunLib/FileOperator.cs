using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataCenterFunLib
{
    public class FileOperator
    {
        public void OpenFile(string FileName)
        {
            string tmpfilename = FileName;
            tmpfilename.ToLower();
            if (tmpfilename.EndsWith(".xls"))
                OpenExcelFile(FileName);
            if (tmpfilename.EndsWith(".xlsx"))
                OpenExcelFile(FileName);
            if (tmpfilename.EndsWith(".doc"))
                OpenWordFile(FileName);
            if (tmpfilename.EndsWith(".docx"))
                OpenWordFile(FileName);

        }
        public void OpenWordFile(string filename)
        {
            Microsoft.Office.Interop.Word.Application xApp = new Microsoft.Office.Interop.Word.Application();
            object MissingValue = System.Reflection.Missing.Value;
            object pfilename = @filename;
            Microsoft.Office.Interop.Word.Document odoc = xApp.Documents.Open(ref pfilename, ref MissingValue, ref MissingValue, ref MissingValue, ref  MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref MissingValue, ref  MissingValue, ref MissingValue, ref MissingValue, ref MissingValue);
            xApp.Visible = true;
        }
        public void OpenExcelFile(string filename)
        {
            Microsoft.Office.Interop.Excel.Application xApp = new Microsoft.Office.Interop.Excel.Application();
            object MissingValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Workbook xBook = xApp.Workbooks._Open(filename, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            Microsoft.Office.Interop.Excel.Worksheet xSheet = (Microsoft.Office.Interop.Excel.Worksheet)xBook.Sheets[1];
            xApp.Visible = true; 
        }

    }
}
