using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace GeoHistory
{
    public partial class FormExcelCompare : DevComponents.DotNetBar.Office2007Form
    {
        private string _ConfigPath = Application.StartupPath + "\\..\\res\\xml\\ExcelCompareConfig.xml";
        private string _ReportType = "";
        public FormExcelCompare()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cmbType.Text == "")
            {
                MessageBox.Show("请选择报表类型!");
                return;
            }
            if (this.txtBoxOldExcel.Text == "")
            {
                MessageBox.Show("请选择旧报表!");
                return;
            }
            if (this.txtBoxNewExcel.Text == "")
            {
                MessageBox.Show("请选择新报表!");
                return;
            }

            progressBarX1.Visible = true;
            progressBarX1.Maximum = 10;
            progressBarX1.Minimum = 1;
            progressBarX1.Value = 1;
            progressBarX1.Step = 1;

            string strOldPath = this.txtBoxOldExcel.Text;
            string strNewPath = this.txtBoxNewExcel.Text;
            string strType = this.cmbType.Text;
            _ReportType = strType;
            lblTips.Text = "获取起始行和起始列...";
            Application.DoEvents();
            int intStartRowID = GetStartRowID(strType);
            if (intStartRowID < 0)
            {
                MessageBox.Show("找不到起始行，请检查配置文件!");
                return;
            }
            int intStartColID = GetStartColID(strType);
            if (intStartColID < 0)
            {
                MessageBox.Show("找不到起始列，请检查配置文件!");
                return;
            }
            lblTips.Text = "打开对比的新旧报表...";
            Application.DoEvents();
            progressBarX1.PerformStep();  //1  
            Microsoft.Office.Interop.Excel.Application excelOld = new Microsoft.Office.Interop.Excel.Application();
            object MissingValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Workbook xBookOld = excelOld.Workbooks._Open(strOldPath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            Microsoft.Office.Interop.Excel.Worksheet SheetOld = (Microsoft.Office.Interop.Excel.Worksheet)xBookOld.Sheets[1];
            progressBarX1.PerformStep(); //2
            Microsoft.Office.Interop.Excel.Application excelNew = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xBookNew = excelNew.Workbooks._Open(strNewPath, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue, MissingValue);
            Microsoft.Office.Interop.Excel.Worksheet SheetNew = (Microsoft.Office.Interop.Excel.Worksheet)xBookNew.Sheets[1];
            progressBarX1.PerformStep(); //3
            SheetOld.Columns.EntireColumn.AutoFit();   //自动调整列宽
            SheetNew.Columns.EntireColumn.AutoFit();   //自动调整列宽
            lblTips.Text = "获取报表表头...";
            Application.DoEvents();
            bool bSame = CompareExcelHeader(excelOld, excelNew, intStartRowID-1);
            progressBarX1.PerformStep(); //4
            if (!bSame)
            {
                MessageBox.Show("新旧报表表头不一致，请确认!");
                lblTips.Text = "新旧报表表头不一致";
                progressBarX1.Visible = false;
                return;
            }
            string strResPath = this.txtBoxResPath.Text;

            Microsoft.Office.Interop.Excel.Application excel = null;
            Microsoft.Office.Interop.Excel.Workbook wb = null;
            //建立Excel对象
            excel = new Microsoft.Office.Interop.Excel.Application();
            wb = excel.Application.Workbooks.Add(true);
            excel.Visible = false;
            wb.Application.ActiveWindow.Caption = strResPath;
            lblTips.Text = "拷贝报表表头...";
            Application.DoEvents();
            bool bRes = WriteHeaderOfResExcel(excel,excelNew, intStartRowID);
            progressBarX1.PerformStep(); //5
            lblTips.Text = "对比新旧报表内容...";
            Application.DoEvents();
            DoCompareEx(excel, excelOld, excelNew, intStartRowID, intStartColID);
            progressBarX1.PerformStep(); //6
            progressBarX1.PerformStep(); //7
            lblTips.Text = "保存对比结果...";
            Application.DoEvents();

            try
            {
                wb.SaveAs(strResPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (System.Exception ex)
            {
            }

            progressBarX1.PerformStep(); //8
            xBookNew.Saved = false;
            xBookOld.Saved = false;
            SheetOld = null;
            SheetNew = null;
            xBookNew.Close(false, MissingValue, MissingValue);
            xBookOld.Close(false, MissingValue, MissingValue);

            CloseExcel(excel);
            CloseExcel(excelOld);
            CloseExcel(excelNew);
            progressBarX1.PerformStep(); //9
            lblTips.Text = "打开对比结果...";
            Application.DoEvents();
            try
            {
                System.Diagnostics.Process.Start(strResPath);
            }
            catch (System.Exception ex)
            {
            }
            lblTips.Text = "对比完成";
            progressBarX1.Visible = false;
            //Ope
            this.DialogResult = DialogResult.OK;



        }
        private void CloseExcel(Microsoft.Office.Interop.Excel.Application excel)
        {
            excel.Workbooks.Close();
            excel.Application.Quit();
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            SysCommon.ModExcel.Kill(excel);
            GC.Collect();
        }
        //基本对比函数，新旧报表左侧标题必须一样
        private void DoCompare(Microsoft.Office.Interop.Excel.Application excelRes,Microsoft.Office.Interop.Excel.Application excelOld,Microsoft.Office.Interop.Excel.Application excelNew,int iStartRowID,int iStartColID)
        {
            Microsoft.Office.Interop.Excel.Worksheet sheetNew = excelNew.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            Microsoft.Office.Interop.Excel.Worksheet sheetOld = excelOld.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

            int iNullRowCnt = 0;
            int iNullColCnt = 0;
            object MissingValue=System.Type.Missing;
            for (int i = iStartRowID; i <= sheetNew.UsedRange.Rows.Count; i++)
            {
                object objTmp = ((Microsoft.Office.Interop.Excel.Range)sheetOld.UsedRange.Cells[i, 1]).Text;

                string strTmp = "";// excelNew.Cells[i, 1].ToString();
                if (objTmp != null)
                {
                    strTmp = objTmp.ToString();
                }
                if (strTmp == "")
                {
                    iNullRowCnt++;
                    if (iNullRowCnt > 50)
                    {
                        break;
                    }
                }
                else
                {
                    iNullRowCnt = 0;
                }
                bool isSame=true;
                if (iStartColID > 1)
                {
                    string strLast = "";
                    object objLast = ((Microsoft.Office.Interop.Excel.Range)sheetNew.UsedRange.Cells[i, iStartColID - 1]).Text;
                    if (objLast != null) { strLast = objLast.ToString(); }
                    for (int j = 1; j < iStartColID; j++)
                    {
                        object objOld = ((Microsoft.Office.Interop.Excel.Range)sheetOld.UsedRange.Cells[i, j]).Text;
                        object objNew = ((Microsoft.Office.Interop.Excel.Range)sheetNew.UsedRange.Cells[i, j]).Text;

                        
                        string strOld = "";
                        string strNew = "";
                        if (objOld != null) { strOld = objOld.ToString(); }
                        if (objNew != null) { strNew = objNew.ToString(); }
                        if (strOld.Trim() != strNew.Trim())
                        {
                            isSame = false;
                        }
                        else
                        {
                            excelRes.Cells[i, j] = strOld;
                            if (strOld == "" && i > 1 && strLast != "")
                            {
                                excelRes.get_Range(excelRes.Cells[i-1,j],excelRes.Cells[i,j]).Merge(MissingValue);
                            }
                        }
                    }
                    if (isSame)
                    {
                        for (int j = iStartColID; j <= sheetNew.UsedRange.Columns.Count; j++)
                        {
                            object objTmpOld = ((Microsoft.Office.Interop.Excel.Range)sheetOld.UsedRange.Cells[i, j]).Text;
                            object objTmpNew = ((Microsoft.Office.Interop.Excel.Range)sheetNew.UsedRange.Cells[i, j]).Text;
                            object objTmp1 = ((Microsoft.Office.Interop.Excel.Range)sheetNew.UsedRange.Cells[iStartRowID-1, j]).Text;
                            string strTmpOld = ""; ;
                            string strTmpNew = "";
                            string strTmp1 = "";

                            if (objTmpOld != null) { strTmpOld = objTmpOld.ToString(); }
                            if (objTmpNew != null) { strTmpNew = objTmpNew.ToString(); }
                            if (objTmp1 != null) { strTmp1 = objTmp1.ToString(); }
                            if (strTmp1 == "" && strTmpOld=="" && strTmpNew=="")
                            {
                                iNullColCnt++;
                                if (iNullColCnt > 50)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                iNullColCnt = 0;
                            }
                            string strRes = "";
                            try
                            {
                                double dOld = 0;
                                if (strTmpOld != "")
                                {
                                    dOld=double.Parse(strTmpOld);
                                }
                                double dNew = 0;
                                if (strTmpNew != "")
                                {
                                    dNew=double.Parse(strTmpNew);
                                }
                                double dRes = dNew - dOld;
                                strRes = dRes.ToString("f2");
                                if (dRes > 0)
                                {
                                    excelRes.Cells[i, j] ="'+"+ strRes;
                                }
                                else if(dRes<0)
                                {
                                    excelRes.Cells[i, j] ="'"+ strRes;
                                }
                            }
                            catch
                            { }
                        }
                    }
                }
            }
            
        }
        //升级版对比函数，如果左侧标题不对应，也可以做新旧对比
        private void DoCompareEx(Microsoft.Office.Interop.Excel.Application excelRes, Microsoft.Office.Interop.Excel.Application excelOld, Microsoft.Office.Interop.Excel.Application excelNew, int iStartRowID, int iStartColID)
        {
            Microsoft.Office.Interop.Excel.Worksheet sheetNew = excelNew.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            Microsoft.Office.Interop.Excel.Worksheet sheetOld = excelOld.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            Microsoft.Office.Interop.Excel.Worksheet sheetRes = excelRes.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            int iNullRowCnt = 0;
            int iNullColCnt = 0;
            object MissingValue = System.Type.Missing;
            int RowIDOld = iStartRowID;
            int RowIDNew = iStartRowID;
            int RowIDRes = iStartRowID;

            Dictionary<int, int> DicSame = new Dictionary<int, int>();
            List<int> ListOld = new List<int>();//已经处理过的旧ID
            List<int> ListNew = new List<int>();//已经处理过的新ID

            while (RowIDNew <= sheetNew.UsedRange.Rows.Count || RowIDOld <= sheetOld.UsedRange.Rows.Count)
            {

                string strTmp = GetValueOfLeftHeader(sheetOld, RowIDOld, 1);
                if (strTmp == "")
                {
                    iNullRowCnt++;
                    if (iNullRowCnt > 50)
                    {
                        break;
                    }
                }
                else
                {
                    iNullRowCnt = 0;
                }
                bool isSame = true;
                if (iStartColID > 1)
                {
                    string strLast = GetValueOfLeftHeader(sheetNew, RowIDNew, iStartColID - 1);
                    for (int j = 1; j < iStartColID; j++)
                    {
                        string strOld = GetValueOfLeftHeader(sheetOld, RowIDOld, j);
                        string strNew = GetValueOfLeftHeader(sheetNew, RowIDNew, j);

                        if (strOld.Trim() != strNew.Trim())
                        {
                            isSame = false;
                            break;
                        }
                    }
                    if (isSame)
                    {
                        WriteResOfSameRow(excelRes, sheetOld, sheetNew, RowIDRes, RowIDOld, RowIDNew, iStartRowID, iStartColID);
                        RowIDNew++;
                        RowIDOld++;
                        RowIDRes++;

                    }
                    else
                    {
                        int iFindIDFromOld = GetEqualRowID(sheetNew, sheetOld, RowIDNew,RowIDOld ,iStartColID, ListOld);
                        if (iFindIDFromOld > RowIDOld)
                        {
                            if (!ListOld.Contains(iFindIDFromOld))//找到一样的行
                            {
                                WriteResOfSameRow(excelRes, sheetOld, sheetNew, RowIDRes, iFindIDFromOld, RowIDNew, iStartRowID, iStartColID);
                                ListOld.Add(iFindIDFromOld);
                                ListNew.Add(RowIDNew);
                                RowIDRes++;
                                RowIDNew++;
                            }
                        }
                        //else if (iFindIDFromOld>0)//找到一样的行，但是是处理过的
                        //{
                        //    RowIDNew++;
                        //}
                        else//找不到一样的行，New表里面的内容是多的
                        {
                            WriteResOfNew(excelRes, sheetNew, RowIDRes, RowIDNew, iStartRowID, iStartColID);
                            ListNew.Add(RowIDNew);
                            RowIDNew++;
                            RowIDRes++;
                        }
                        while (ListOld.Contains(RowIDOld))
                        {
                            RowIDOld++;
                        }
                        while (ListNew.Contains(RowIDNew))
                        {
                            RowIDNew++;
                        }
                        int iFindIDFromNew = GetEqualRowID(sheetOld, sheetNew, RowIDOld,RowIDNew ,iStartColID, ListNew);
                        if (iFindIDFromNew >= RowIDNew)
                        {
                            if (!ListNew.Contains(iFindIDFromNew))
                            {
                                WriteResOfSameRow(excelRes, sheetOld, sheetNew, RowIDRes, RowIDOld, RowIDNew, iStartRowID, iStartColID);
                                ListNew.Add(iFindIDFromNew);
                                ListOld.Add(RowIDOld);
                                if (iFindIDFromNew == RowIDNew)
                                {
                                    RowIDNew++;
                                }
                                RowIDOld++;
                                RowIDRes++;
                            }
 
                        }
                        //else if (iFindIDFromNew > 0)//找到一样的行，但是是处理过的
                        //{
                        //    RowIDOld++;
                        //}
                        else
                        {
                            WriteResOfOld(excelRes, sheetOld, RowIDRes, RowIDOld, iStartRowID, iStartColID);
                            ListOld.Add(RowIDOld);//找不到一样的行，Old表里面的内容是多的
                            RowIDOld++;
                            RowIDRes++;
                        }
                    }
                }
                while (ListOld.Contains(RowIDOld))
                {
                    RowIDOld++;
                }
                while (ListNew.Contains(RowIDNew))
                {
                    RowIDNew++;
                }
            }
            MergeCellOfLeftHeader(sheetRes, iStartRowID, iStartColID, RowIDRes);
        }
        private void MergeCellOfLeftHeader(Microsoft.Office.Interop.Excel.Worksheet sheet,int iStartRowID,int iStartColID,int RowCnt)
        {
            for (int i = RowCnt; i > iStartRowID; i--)
            {
                for (int j = 1; j < iStartColID; j++)
                {
                    string strCur = GetValueOfLeftHeader(sheet, i, j);
                    string strPre = GetValueOfLeftHeader(sheet, i - 1, j);
                    bool bMerge = false;
                    if (strCur.Trim() == strPre.Trim() && strCur!="")
                    {
                        bMerge = true;
                    }
                    if (j > 1 && bMerge)
                    {
                        object objMerge = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[i, j - 1]).MergeCells;
                        if (objMerge.ToString() != "True")
                        {
                            bMerge = false;
                        }
                    }
                    if (bMerge)
                    {
                        sheet.Cells[i, j] = "";
                        sheet.get_Range(sheet.Cells[i-1, j], sheet.Cells[i, j]).Merge(false);
                    }
                }
            }
            sheet.get_Range(sheet.Cells[iStartRowID,1],sheet.Cells[RowCnt,iStartColID-1]).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        }
        //为新表多的行写对比结果
        private void WriteResOfNew(Microsoft.Office.Interop.Excel.Application excelRes, Microsoft.Office.Interop.Excel.Worksheet sheetNew, int RowIDRes, int RowIDNew, int iStartRowID, int iStartColID)
        {
            int iNullColCnt = 0;
            for (int j = 1; j < iStartColID; j++)
            {
                string strNew = GetValueOfLeftHeader(sheetNew, RowIDNew, j);

                excelRes.Cells[RowIDRes, j] = strNew;

            }
            for (int j = iStartColID; j <= sheetNew.UsedRange.Columns.Count; j++)
            {
                string strTmpNew = GetValueOfLeftHeader(sheetNew, RowIDNew, j);
                string strTmp1 = GetValueOfLeftHeader(sheetNew, iStartRowID -1, j);
                if (strTmp1 == ""  && strTmpNew == "")
                {
                    iNullColCnt++;
                    if (iNullColCnt > 50)
                    {
                        break;
                    }
                }
                else
                {
                    iNullColCnt = 0;
                }
                string strRes = "";
                try
                {
                    double dNew = 0;
                    if (strTmpNew != "")
                    {
                        dNew = double.Parse(strTmpNew);
                    }

                    strRes = dNew.ToString("f2");
                    if (dNew > 0)
                    {
                        excelRes.Cells[RowIDRes, j] = "'+" + strRes;
                    }
                    else if (dNew < 0)
                    {
                        excelRes.Cells[RowIDRes, j] = "'" + strRes;
                    }
                }
                catch
                { }
            }
        }
        //为旧表多的行写对比结果
        private void WriteResOfOld(Microsoft.Office.Interop.Excel.Application excelRes, Microsoft.Office.Interop.Excel.Worksheet sheetOld, int RowIDRes, int RowIDOld, int iStartRowID, int iStartColID)
        {
            int iNullColCnt = 0;
            for (int j = 1; j < iStartColID; j++)
            {
                string strOld = GetValueOfLeftHeader(sheetOld, RowIDOld, j);
                excelRes.Cells[RowIDRes, j] = strOld;

            }
            for (int j = iStartColID; j <= sheetOld.UsedRange.Columns.Count; j++)
            {
                string strTmpOld = GetValueOfLeftHeader(sheetOld, RowIDOld, j);
                string strTmp1 = GetValueOfLeftHeader(sheetOld, iStartRowID - 1, j);

                if (strTmp1 == "" && strTmpOld == "")
                {
                    iNullColCnt++;
                    if (iNullColCnt > 50)
                    {
                        break;
                    }
                }
                else
                {
                    iNullColCnt = 0;
                }
                string strRes = "";
                try
                {
                    double dOld = 0;
                    if (strTmpOld != "")
                    {
                        dOld = double.Parse(strTmpOld);
                        dOld = dOld * (-1);
                    }
                    strRes = dOld.ToString("f2");
                    if (dOld > 0)
                    {
                        excelRes.Cells[RowIDRes, j] = "'+" + strRes;
                    }
                    else if (dOld < 0)
                    {
                        excelRes.Cells[RowIDRes, j] = "'" + strRes;
                    }
                }
                catch
                { }
            }
        }
        //读左侧的表头
        private string GetValueOfLeftHeader(Microsoft.Office.Interop.Excel.Worksheet sheet,int RowID,int ColID)
        {
            object objCell = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[RowID, ColID]).MergeCells;
            string strValue = "";
            object objValue = null;
            if (objCell.ToString() == "True")//是合并单元格
            {
                int StartRow = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[RowID, ColID]).MergeArea.Row;
                int StartCol = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[RowID, ColID]).MergeArea.Column;
                objValue = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[StartRow, StartCol]).Text;
            }
            else
            {
                objValue = ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[RowID, ColID]).Text;
            }            
            if (objValue != null)
            {
                strValue = objValue.ToString();
            }
            return strValue;
        }
        //新旧表左侧一样的行，写对比结果
        private void WriteResOfSameRow(Microsoft.Office.Interop.Excel.Application excelRes, Microsoft.Office.Interop.Excel.Worksheet sheetOld, Microsoft.Office.Interop.Excel.Worksheet sheetNew, int RowIDRes, int RowIDOld, int RowIDNew,int iStartRowID,  int iStartColID)
        {
            int iNullColCnt = 0;
            for (int j = 1; j < iStartColID; j++)
            {
                
                string strOld = GetValueOfLeftHeader(sheetOld, RowIDOld, j);
                string strNew = GetValueOfLeftHeader(sheetNew, RowIDNew, j); 

                excelRes.Cells[RowIDRes, j] = strOld;

            }
            for (int j = iStartColID; j <= sheetNew.UsedRange.Columns.Count; j++)
            {
                string strTmpOld = GetValueOfLeftHeader(sheetOld, RowIDOld, j);
                string strTmpNew = GetValueOfLeftHeader(sheetNew, RowIDNew, j);
                string strTmp1 = GetValueOfLeftHeader(sheetNew, iStartRowID - 1, j);

                if (strTmp1 == "" && strTmpOld == "" && strTmpNew == "")
                {
                    iNullColCnt++;
                    if (iNullColCnt > 50)
                    {
                        break;
                    }
                }
                else
                {
                    iNullColCnt = 0;
                }
                string strRes = "";
                try
                {
                    double dOld = 0;
                    if (strTmpOld != "")
                    {
                        dOld = double.Parse(strTmpOld);
                    }
                    double dNew = 0;
                    if (strTmpNew != "")
                    {
                        dNew = double.Parse(strTmpNew);
                    }
                    double dRes = dNew - dOld;
                    strRes = dRes.ToString("f2");
                    if (dRes > 0)
                    {
                        excelRes.Cells[RowIDRes, j] = "'+" + strRes;
                    }
                    else if (dRes < 0)
                    {
                        excelRes.Cells[RowIDRes, j] = "'" + strRes;
                    }
                }
                catch
                { }
            }
        }
        //从excelFrom里面找，与excel的第iRowID行内容一致的
        //ListExceptID中放置不参与比较的行号
        //iStartRowID是开始查找的行，与其他地方该变量的含义不同
        private int GetEqualRowID(Microsoft.Office.Interop.Excel.Worksheet sheet, Microsoft.Office.Interop.Excel.Worksheet sheetFrom, int iRowID,int iStartRowID,int iStartColID,List<int> ListExceptID)
        {
            //为加快速度，仅仅查找最近的5行
            for (int i = iStartRowID; i < iStartRowID+5; i++)//i < sheetFrom.UsedRange.Rows.Count
            {
                bool bSame=true;
                for (int j = 1; j < iStartColID; j++)
                {
                    string strValue = GetValueOfLeftHeader(sheet, iRowID, j);
                    string strFrom = GetValueOfLeftHeader(sheetFrom, i, j);
                    if (strValue.Trim() != strFrom.Trim())
                    {
                        bSame = false;
                        break;
                    }
                }
                if (bSame)
                {
                    return i;
                }
            }

            return -1; 
        }
        private bool WriteHeaderOfResExcel(Microsoft.Office.Interop.Excel.Application excelTo,Microsoft.Office.Interop.Excel.Application excelFrom, int iStartRowID)
        {
            if (excelTo == null)
            {
                return false;
            }
            if (excelFrom == null)
            {
                return false;
            }
            try
            {
                Microsoft.Office.Interop.Excel.Worksheet fromSheet = excelFrom.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                Microsoft.Office.Interop.Excel.Worksheet toSheet = excelTo.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                object MissingValue = Type.Missing;
                int ColCnt = fromSheet.UsedRange.Columns.Count;
                // 选择
                fromSheet.Select(MissingValue);
                // 复制.
                
                fromSheet.get_Range(fromSheet.Cells[1, 1], fromSheet.Cells[iStartRowID - 1, ColCnt]).Copy(MissingValue);

                toSheet.Activate();
                toSheet.Select(MissingValue);

                // 粘贴格式.
                toSheet.get_Range(toSheet.Cells[1, 1], toSheet.Cells[iStartRowID - 1, ColCnt]).PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteFormats, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, MissingValue, MissingValue);
                // 粘贴数据.
                toSheet.get_Range(toSheet.Cells[1, 1], toSheet.Cells[iStartRowID - 1, ColCnt]).PasteSpecial(Microsoft.Office.Interop.Excel.XlPasteType.xlPasteValues, Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, MissingValue, MissingValue);

                fromSheet.get_Range(fromSheet.Cells[1, 1], fromSheet.Cells[1, 1]).Copy(MissingValue);
            }
            catch 
            {
                return false;
            }
            return true;


        }
        private bool CompareExcelHeader(Microsoft.Office.Interop.Excel.Application excelOld,Microsoft.Office.Interop.Excel.Application excelNew,int HeaderEndRowID)
        {
            if (excelOld==null)
            {
                return false;
            }
            if (excelNew==null)
            {
                return false;
            }
            Microsoft.Office.Interop.Excel.Worksheet sheetOld=excelOld.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            Microsoft.Office.Interop.Excel.Worksheet sheetNew=excelNew.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
            for (int i = 1; i <= HeaderEndRowID; i++)
            {
                for (int j = 1; j < sheetNew.UsedRange.Columns.Count; j++)
                {
                    string strOld = GetValueOfLeftHeader(sheetOld, i, j);
                    string strNew = GetValueOfLeftHeader(sheetNew, i, j);
                    if (strOld.Trim() != strNew.Trim())
                    {
                        return false;
                    }
                }
            }
            return true;




        }
        private void btnSelectOld_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = false;
            sOpenFileD.Title = "选择数据源";
            sOpenFileD.Filter = "Excel 97-2003 工作薄 (*.xls)|*.xls|Excel 工作薄(*.xlsx)|*.xlsx";

            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                this.txtBoxOldExcel.Text = sOpenFileD.FileName;
            }
        }

        private void btnSelectNew_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = false;
            sOpenFileD.Title = "选择数据源";
            sOpenFileD.Filter = "Excel 97-2003 工作薄 (*.xls)|*.xls|Excel 工作薄(*.xlsx)|*.xlsx";

            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                this.txtBoxNewExcel.Text = sOpenFileD.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FormExcelCompare_Load(object sender, EventArgs e)
        {
            InitReportTypes();
        }
        private void InitReportTypes()
        {
            this.cmbType.Items.Clear();
            if (!File.Exists(_ConfigPath))
            {
                return;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_ConfigPath);
            string strSearch = "//ExcelCompareConfig";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode != null)
            {
                XmlNodeList pNodeList = pNode.ChildNodes;
                for (int i = 0; i < pNodeList.Count; i++)
                {
                    XmlNode pTmpNode = pNodeList[i];
                    try
                    {
                        string strType = pTmpNode.Attributes["NodeText"].Value.ToString();
                        this.cmbType.Items.Add(strType);
                    }
                    catch
                    { }
                }
            }
            pXmldoc = null;
        }
        private int GetStartColID(string strType)
        {
            if (!File.Exists(_ConfigPath))
            {
                return -1;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_ConfigPath);
            string strSearch = "//ExcelCompareConfig//Type[@NodeText='" + strType + "']";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            int iStartColID = -1;
            if (pNode != null)
            {
                try
                {
                    string strStartCol = pNode.Attributes["StartColID"].Value.ToString();
                    iStartColID = int.Parse(strStartCol);
                }
                catch
                { }
            }
            pXmldoc = null;
            return iStartColID;
        }
        private int GetStartRowID(string strType)
        {
             if (!File.Exists(_ConfigPath))
            {
                return -1;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_ConfigPath);
            string strSearch = "//ExcelCompareConfig//Type[@NodeText='"+strType+"']";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            int iStartRowID=-1;
            if (pNode != null)
            {
                try
                {
                    string strStartRow = pNode.Attributes["StartRowID"].Value.ToString();
                    iStartRowID=int.Parse(strStartRow);
                }
                catch
                { }
            }
            pXmldoc = null;
            return iStartRowID;
        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            SaveFileDialog pDlg = new SaveFileDialog();
            pDlg.Filter = "Excel WorkBook (*.xls)|*.xls";
            if (pDlg.ShowDialog() == DialogResult.Cancel)
                return;
            this.txtBoxResPath.Text = pDlg.FileName;
            pDlg = null;
        }

    }
}
