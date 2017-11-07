using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using SysCommon.Gis;
using Microsoft.Office.Interop.Excel;

namespace GeoDataManagerFrame
{
    public partial class frmFeatureClassMetaInput : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_Workspace = null;
        public frmFeatureClassMetaInput(IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_Workspace = pWorkspace;
        }
        #region  文件操作
        private void bttOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
                sOpenFileD.CheckFileExists = true;
                sOpenFileD.CheckPathExists = true;
                sOpenFileD.Multiselect = true;
                sOpenFileD.Title = "选择数据源";
                sOpenFileD.Filter = "Excel 97-2003 工作薄 (*.xls)|*.xls|Excel 工作薄(*.xlsx)|*.xlsx";
                int m = 1;
                if (sOpenFileD.ShowDialog() == DialogResult.OK)
                {
                    string[] strFileName = sOpenFileD.FileNames;
                    for (int j = 0; j < strFileName.Length; j++)
                    {
                        for (int i = 0; i < dataGrid.RowCount - 1; i++)
                        {
                            if (dataGrid.Rows[i].Cells["CmnPath"].Value == null) { continue; }
                            if (strFileName[j].ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                            {
                                if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    break;
                                }
                                else
                                {
                                    m = 0;
                                    break;
                                }
                            }

                        }
                        if (m == 1)
                        {
                            dataGrid.Rows.Add(true, strFileName[j].ToString(), null, "未上传");
                        }
                    }
                }

                if (dataGrid.RowCount - 1 > 0)
                {
                    bttRemove.Enabled = true;
                    bttAllRemove.Enabled = true;
                }
            }
            catch { }
        }

        private void bttOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
                int m = 1;
                if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] pFilePath = null;
                    pFilePath = Directory.GetFiles(pFolderBrowserDialog.SelectedPath.ToString(), "*.xls", SearchOption.TopDirectoryOnly);
                    m = 1;
                    if (pFilePath.Length == 0)
                    {
                        for (int i = 0; i < dataGrid.RowCount - 1; i++)
                        {
                            if (dataGrid.Rows[i].Cells["CmnPath"].Value == null) { continue; }
                            if (pFolderBrowserDialog.SelectedPath.ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                            {
                                if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    break;
                                }
                                else
                                {
                                    m = 0;
                                    break;
                                }
                            }
                        }
                        if (m == 1)
                        {
                            dataGrid.Rows.Add(true, pFolderBrowserDialog.SelectedPath.ToString(), null, "未上传");
                        }
                    }
                    else
                    {
                        for (int j = 0; j < pFilePath.Length; j++)
                        {
                            for (int i = 0; i < dataGrid.RowCount - 1; i++)
                            {
                                if (dataGrid.Rows[i].Cells["CmnPath"].Value == null) { continue; }
                                if (pFilePath[j].ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                                {
                                    if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        m = 0;
                                        break;
                                    }
                                }
                            }
                            if (m == 1)
                            {
                                dataGrid.Rows.Add(true, pFilePath[j].ToString(), null, "未上传");
                            }
                        }
                    }

                }

                if (dataGrid.RowCount - 1 > 0)
                {
                    bttRemove.Enabled = true;
                    bttAllRemove.Enabled = true;
                }
            }
            catch { }
        }

        private void bttRemove_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection pSelectedRowC = dataGrid.SelectedRows;
            if (pSelectedRowC.Count == 0) { return; }
            try
            {
                /////删除用户选中的项
                for (int i = 0; i < pSelectedRowC.Count; i++)
                {
                    dataGrid.Rows.RemoveAt(pSelectedRowC[i].Index);
                }
                if (dataGrid.RowCount == 0)
                {
                    bttRemove.Enabled = false;
                    bttAllRemove.Enabled = false;
                }
            }
            catch { }
        }

        private void bttAllRemove_Click(object sender, EventArgs e)
        {
            dataGrid.Rows.Clear();
            bttRemove.Enabled = false;
            bttAllRemove.Enabled = false;
        }

        private void btnAllSelected_Click(object sender, EventArgs e)
        {
            if (dataGrid.RowCount-1 > 0)
            {
                for (int i = 0; i < dataGrid.RowCount-1; i++)
                {
                    dataGrid.Rows[i].Cells["CmnSelect"].Value = true;
                }
            }
        }

        private void btnOtherSelected_Click(object sender, EventArgs e)
        {
            if (dataGrid.RowCount - 1 > 0)
            {
                for (int i = 0; i < dataGrid.RowCount-1; i++)
                {
                    if (dataGrid.Rows[i].Cells["CmnSelect"].Value.ToString() == true.ToString())
                    {
                        dataGrid.Rows[i].Cells["CmnSelect"].Value = false;
                    }
                    else
                    {
                        dataGrid.Rows[i].Cells["CmnSelect"].Value = true;
                    }
                }
            }
        }
       private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose(true);

        }
        #endregion
       #region 操作函数
       /// <summary>
       /// 获取管理系统中所有的要素集名称
       /// </summary>
       /// <returns></returns>
       private List<string> GetAllFeatureClassName()
       {
           ///存储所用导入管理系统中的要素集名称
           List<string> LstFeatureClassName = new List<string>();
           try
           {
               SysGisTable sysTable = new SysGisTable(m_Workspace);
               Exception eError;
               ///存储所用导入管理系统中数据库的链接信息
               List<object> Lstconninfostr = new List<object>();
               ///获取所用导入管理系统中数据库的链接信息
               Lstconninfostr = sysTable.GetFieldValues("DATABASEMD", "CONNECTIONINFO", null, out eError);
               for (int i = 0; i < Lstconninfostr.Count; i++)
               {
                   try
                   {
                       string conninfostr = Lstconninfostr[i].ToString();
                       int index6 = conninfostr.LastIndexOf("|");
                       string strDatasets = conninfostr.Substring(index6 + 1);
                       ///获取该链接中已导入的管理系统的要素集名称
                       string[] strTemp = strDatasets.Split(new char[] { ',' });
                       try
                       {
                           for (int j = 0; j < strTemp.Length; j++)
                           {
                               if (strTemp[j] != "")
                               {
                                   if (!LstFeatureClassName.Contains(strTemp[j].ToString()))
                                   {
                                   LstFeatureClassName.Add(strTemp[j].ToString());
                                   }
                               }
                           }

                       }
                       catch { }
                   }
                   catch { }

               }
               return LstFeatureClassName;
           }
           catch { return LstFeatureClassName; }


       }

      /// <summary>
      /// 根据导入的Excel初始化字典表
      /// </summary>
      /// <param name="strExcelPath">Excel文件路径</param>
      /// <param name="dicData"></param>
       private void InitializeDic(string strExcelPath, ref Dictionary<string, object> dicData)
       {
       
           System.Reflection.Missing miss = System.Reflection.Missing.Value;
           Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application();
           if (xlsApp == null)
           {
               MessageBox.Show("无法创建Excel对象，可能您的机器未安装Excel");
               return;
           }
           try
           {
               Workbook xlsWrkBook = xlsApp.Workbooks.Open(strExcelPath, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss, miss);
               Worksheet xlsWrkSht = xlsWrkBook.Worksheets[1] as Worksheet;
               for (int row = 1; row < xlsWrkSht.Rows.Count; row++)
               {
                   ///Excel中第二列为字段名
                   Range pRangeCoum = xlsWrkSht.get_Range(xlsWrkSht.Cells[row, 2], xlsWrkSht.Cells[row, 2]);
                   ///第三列为字段值
                   Range pRangeRow = xlsWrkSht.get_Range(xlsWrkSht.Cells[row, 3], xlsWrkSht.Cells[row, 3]);
                   string strValue;
                    ///当读到字段名为空时则默认为数据读取完成
                   if (pRangeCoum.Value2 == null)
                   {
                       break;
                   }

                   if (pRangeRow.Value2 == null)
                   {

                       //continue;
                       strValue = "";
                   }
                   else
                   {
                       strValue = pRangeRow.Value2.ToString();
                   }
                   dicData.Add(pRangeCoum.Value2.ToString(), strValue);
               }
               xlsApp.Quit();
               System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWrkSht);
               System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWrkBook);
           }
           catch { }
           finally
           {
               System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp);
               GC.Collect();
           }

           

       }
       /// <summary>
       /// 对准备上传或更新的数据进行检查
       /// </summary>
       private void CheckerData()
       {
           try
           {
               for (int i = 0; i < dataGrid.RowCount - 1; i++)
               {
                   if (dataGrid.Rows[i].Cells["CmnSelect"].Value.ToString() == true.ToString() && dataGrid.Rows[i].Cells["CmnFeatureClassName"].Value==null)
                   {
                       dataGrid.Rows[i].Cells["CmnSelect"].Value = false;
                       dataGrid.Rows[i].Cells["CmnState"].Value = "数据库名称不能为空";
                   }
               }
           }
           catch { }
       }

       #endregion
       private void btnOK_Click(object sender, EventArgs e)
        {
            if (dataGrid.RowCount-1 == 0)
            {
                MessageBox.Show("请输入数据库属性信息！","提示！");
                return;
            }
            Exception exError = null;
            SysGisTable sysTable = new SysGisTable(m_Workspace);
           SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
           vProgress.EnableCancel = false;//设置进度条
           vProgress.ShowDescription = true;
           vProgress.FakeProgress = true;
           vProgress.TopMost = true;
           vProgress.MaxValue = dataGrid.RowCount-1;
           vProgress.ShowProgress();
           try
           {
               CheckerData();
               for (int i = 0; i < dataGrid.RowCount-1; i++)
               {
                   vProgress.ProgresssValue = i + 1;
                   if (dataGrid.Rows[i].Cells["CmnSelect"].Value.ToString() == true.ToString())
                   {
                       string strSource = dataGrid.Rows[i].Cells["CmnPath"].Value.ToString();
                       string strFeatureClassName = dataGrid.Rows[i].Cells["CmnFeatureClassName"].Value.ToString();
                       Dictionary<string, object> dicData = new Dictionary<string, object>();
                       ////判断上传的文件是否存在
                       if (System.IO.File.Exists(strSource))
                       {
                          
                           //不存在则添加
                           if (!sysTable.ExistData("METADATA_LIB", "数据库名称='" + strFeatureClassName + "'"))
                           {
                               vProgress.SetProgress("正在上传数据：" + System.IO.Path.GetFileName(strSource));
                               dicData.Add("数据库名称", strFeatureClassName);
                               InitializeDic(strSource, ref dicData);
                               if (sysTable.NewRow("METADATA_LIB", dicData, out exError))
                               {
                                   dataGrid.Rows[i].Cells["CmnSelect"].Value = false;
                                   dataGrid.Rows[i].Cells["CmnState"].Value = "数据录入完成";
                               }
                               else
                               {
                                   dataGrid.Rows[i].Cells["CmnState"].Value = "数据录入失败，请检查Excel文件格式是否正确";
                               }
                           }
                           else
                           {
                               dataGrid.Rows[i].Cells["CmnState"].Value = "该记录已存在，可进行更新";

                           }
                       }
                       else
                       {
                           dataGrid.Rows[i].Cells["CmnState"].Value = "数据库属性信息文件不存在";
                       }
                 

                   }
               }
               vProgress.Close();
               sysTable = null;


           }
           catch { vProgress.Close(); }
        }
       
        private void btnUpdata_Click(object sender, EventArgs e)
        {
            List<int> LstIndex = new List<int>();
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;//设置进度条
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            try
            {
                CheckerData();
                for (int i = 0; i < dataGrid.RowCount-1; i++)
                {
                    if (dataGrid.Rows[i].Cells["CmnSelect"].Value.ToString() == true.ToString())
                    {
                        LstIndex.Add(i);
                    }

                }
                if (LstIndex.Count == 0)
                {
                    MessageBox.Show("请选择需要更新的数据！","提示！");
                    return;
                }
                Exception exError = null;
                SysGisTable sysTable = new SysGisTable(m_Workspace);
                vProgress.MaxValue = LstIndex.Count;
                vProgress.ShowProgress();
                for (int j = 0; j < LstIndex.Count;j++ )
                {
                    vProgress.ProgresssValue = j + 1;
                    string strSource = dataGrid.Rows[LstIndex[j]].Cells["CmnPath"].Value.ToString();
                    string strFeatureClassName = dataGrid.Rows[LstIndex[j]].Cells["CmnFeatureClassName"].Value.ToString();
                    Dictionary<string, object> dicData = new Dictionary<string, object>();
                    ////判断上传的文件是否存在
                    if (System.IO.File.Exists(strSource))
                    {
                       
                        //已存在更新
                        if (sysTable.ExistData("METADATA_LIB", "数据库名称='" + strFeatureClassName + "'"))
                        {
                            vProgress.SetProgress("正在更新数据：" + System.IO.Path.GetFileName(strSource));
                            dicData.Add("数据库名称", strFeatureClassName);
                            InitializeDic(strSource, ref dicData);
                            if (sysTable.UpdateRow("METADATA_LIB", null, dicData, out exError))
                            {
                                dataGrid.Rows[LstIndex[j]].Cells["CmnSelect"].Value = false;
                                dataGrid.Rows[LstIndex[j]].Cells["CmnState"].Value = "数据更新完成";
                            }
                            else
                            {
                                dataGrid.Rows[LstIndex[j]].Cells["CmnState"].Value = "数据更新失败，请检查Excel文件格式是否正确";
                            }
                        }
                        else
                        {
                            dataGrid.Rows[LstIndex[j]].Cells["CmnState"].Value = "不存在该记录，请先上传该记录";
                        }
                    }
                    else
                    {
                        dataGrid.Rows[LstIndex[j]].Cells["CmnState"].Value = "数据库属性信息文件不存在";
                    }
                }
                vProgress.Close();
            }
            catch { vProgress.Close(); }
        }
       
        /// <summary>
        /// 初始化数据库名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFeatureClassMetaInput_Load(object sender, EventArgs e)
        {
            try
            {
                ///获取所用的数据库名称
                List<string> LstFeatureClassName = GetAllFeatureClassName();
                DataGridViewComboBoxColumn pComboBoxColumn = dataGrid.Columns[2] as DataGridViewComboBoxColumn;
                for (int i = 0; i < LstFeatureClassName.Count; i++)
                {
                    pComboBoxColumn.Items.Add(LstFeatureClassName[i].ToString());
                }
                
                dataGrid.Refresh();
            }
            catch { }
        }

    }
}
