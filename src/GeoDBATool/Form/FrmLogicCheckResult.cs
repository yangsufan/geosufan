using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon;
using Microsoft.Office.Interop.Excel;
using DevComponents.DotNetBar.Controls;

//ygc 2012-12-27 逻辑检测错误展现

namespace GeoDBATool
{
    public partial class FrmLogicCheckResult : DevComponents.DotNetBar.Office2007Form
    {
        public FrmLogicCheckResult()
        {
            InitializeComponent();
        }
        public Dictionary<IFeatureClass, Dictionary<string, int>> m_DicErrorData
        {
            get;
            set;
        }
        private void FrmLogicCheckResult_Load(object sender, EventArgs e)
        {
            if (m_DicErrorData == null||m_DicErrorData .Count ==0) this.Close () ;
            IWorkspace pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
            Exception ex=null ;

            System.Data.DataTable CheckItem = InitializeCheckItemTable("检查项");
            foreach (IFeatureClass pFeatureClass in m_DicErrorData.Keys)
            {
                ListViewItem listItemLayer = new ListViewItem();
                listItemLayer.Tag = pFeatureClass;
                listItemLayer.Text = pFeatureClass.AliasName;
                Dictionary<string, int> tempDic = m_DicErrorData[pFeatureClass];
                foreach (string key in tempDic.Keys)
                {
                    Dictionary<string, string> dicField = GetFieldValue(pWorkspace, "OBJECTID='" + key + "'", "逻辑检查", out ex);
                    DataRow newRow = CheckItem.NewRow();
                    try
                    {
                        newRow["LayerName"] = pFeatureClass.AliasName;
                        newRow["OBJECTID"] = dicField["OBJECTID"];
                        newRow["CHECKNAME"] = dicField["CheckName"];
                        newRow["CONDITION"] = dicField["condition"];
                        newRow["REMARK"] = dicField["remark"];
                        newRow["ErrorCount"] = tempDic[key].ToString();
                        if (tempDic[key] == 0)
                        {
                            newRow["IsRight"] = "检查通过";
                        }
                        else if (tempDic[key] == -1)
                        {
                            newRow["IsRight"] = "检查未通过(检查条件不适合)";
                        }
                        else
                        {
                            newRow["IsRight"] = "检查未通过";
                        }
                    }
                    catch { }
                    CheckItem.Rows.Add(newRow);
                }
            }
            CheckItem.DefaultView.Sort = "IsRight desc";
            dgvCheckItem.DataSource = CheckItem;
            //表格属性微调
            dgvCheckItem.Columns["OBJECTID"].Visible = false;
            dgvCheckItem.Columns["CONDITION"].Visible = false;
            DataGridViewCellStyle ds = new DataGridViewCellStyle();
            ds.ForeColor =Color.Red ;
            for (int i = 0; i < dgvCheckItem.Rows.Count; i++)
            {
                if (dgvCheckItem.Rows[i].Cells["IsRight"].Value.ToString() == "检查未通过" || dgvCheckItem.Rows[i].Cells["IsRight"].Value.ToString() == "检查未通过(检查条件不适合)")
                {
                    dgvCheckItem.Rows[i].DefaultCellStyle = ds;
                }
            }
            for (int j = 0; j < dgvCheckItem.Columns.Count; j++)
            {
                dgvCheckItem.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvCheckItem.Columns[j].HeaderText = CheckItem.Columns[j].Caption;
                dgvCheckItem.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }
        //获取表中符合条件的第一行所有数据
        private Dictionary<string, string> GetFieldValue(IWorkspace pWorkspace, string Condition, string tableName, out Exception ex)
        {
            Dictionary<string, string> newdic = new Dictionary<string, string>();
            ex = null;
            if (pWorkspace == null) return null;
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            if (pFeatureWorkspace == null) return null;
            ITable pTable = pFeatureWorkspace.OpenTable(tableName);
            IQueryFilter pFilter = new QueryFilterClass();
            pFilter.WhereClause = Condition;
            try
            {
                ICursor pCursor = pTable.Search(pFilter, false);
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        newdic.Add(pRow.Fields.get_Field(i).AliasName, pRow.get_Value(i).ToString());
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
            catch (Exception e)
            {
                ex = e;
            }
            return newdic;

        }
        //初始化检查项列表
        private System.Data.DataTable InitializeCheckItemTable(string checkItemTableName)
        {
            System.Data.DataTable newTable=new System.Data.DataTable (checkItemTableName );
            DataColumn coulumn1 = new DataColumn();
            coulumn1.Caption = "图层名";
            coulumn1.ColumnName = "LayerName";
            coulumn1.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn1);
            DataColumn coulumn2 = new DataColumn();
            coulumn2.Caption = "OBJECTID";
            coulumn2.ColumnName = "OBJECTID";
            coulumn2.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn2);
            DataColumn coulumn3 = new DataColumn();
            coulumn3.Caption = "检查项名称";
            coulumn3.ColumnName = "CHECKNAME";
            coulumn3.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn3);
            DataColumn coulumn7 = new DataColumn();
            coulumn7.Caption = "是否通过检查";
            coulumn7.ColumnName = "IsRight";
            coulumn7.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn7);
            DataColumn coulumn6 = new DataColumn();
            coulumn6.Caption = "错误个数";
            coulumn6.ColumnName = "ErrorCount";
            coulumn6.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn6);
            DataColumn coulumn4 = new DataColumn();
            coulumn4.Caption = "检查条件";
            coulumn4.ColumnName = "CONDITION";
            coulumn4.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn4);
            DataColumn coulumn5 = new DataColumn();
            coulumn5.Caption = "检查项说明";
            coulumn5.ColumnName = "REMARK";
            coulumn5.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn5);
            return newTable;
        }

        private void dgvCheckItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCheckItem.CurrentRow == null) return;
            if (dgvCheckItem.CurrentRow.Cells["IsRight"].Value.ToString() != "检查未通过") return;
            string LayerName = dgvCheckItem.CurrentRow.Cells["LayerName"].Value.ToString();
            IFeatureClass pFeaureClass = GetFeatureClassByName(m_DicErrorData, LayerName);
            if (pFeaureClass == null) return;
            string Condition = dgvCheckItem.CurrentRow.Cells["CONDITION"].Value.ToString();

            System.Data.DataTable ErrorTable=InitializeErrorTable("错误信息");
            IQueryFilter pFilter = new QueryFilterClass();
            pFilter.WhereClause = Condition;
            IFeatureCursor pFeatureCursor = null;
            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = false;
            vProgress.EnableUserCancel(false);
            vProgress.MaxValue = pFeaureClass.FeatureCount(pFilter);
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            try
            {
                pFeatureCursor = pFeaureClass.Search(pFilter, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    int shengIndex=pFeature.Fields.FindField("sheng");
                    DataRow newRow = ErrorTable.NewRow();
                    newRow["LayerName"] = pFeaureClass.AliasName;
                    newRow["sheng"] = ModXZQ.GetXzqName ( Plugin.ModuleCommon.TmpWorkSpace,pFeature.get_Value(shengIndex).ToString().Substring (0,2));
                    int shiIndex = pFeature.Fields.FindField("shi");
                    newRow["shi"] = ModXZQ.GetXzqName ( Plugin.ModuleCommon.TmpWorkSpace,pFeature.get_Value(shiIndex).ToString ().Substring (0,4));
                    int xianIndex = pFeature.Fields.FindField("xian");
                    newRow["xian"] = ModXZQ.GetXzqName ( Plugin.ModuleCommon.TmpWorkSpace,pFeature.get_Value(xianIndex).ToString ().Substring (0,6));
                    int xiangIndex = pFeature.Fields.FindField("xiang");
                    newRow["xiang"] = ModXZQ.GetXzqName ( Plugin.ModuleCommon.TmpWorkSpace,pFeature.get_Value(xiangIndex).ToString ().Substring (0,8));
                    int cunIndex = pFeature.Fields.FindField("cun");
                    newRow["cun"] = ModXZQ.GetXzqName ( Plugin.ModuleCommon.TmpWorkSpace,pFeature.get_Value(cunIndex).ToString ().Substring (0,10));
                    int xbhIndex = pFeature.Fields.FindField("xbh");
                    newRow["xbh"] = pFeature.get_Value(xbhIndex).ToString ();
                    ErrorTable.Rows.Add(newRow);
                    pFeature = pFeatureCursor.NextFeature();
                    vProgress.ProgresssValue += 1;
                }
            }
            catch (Exception ex)
            {
                vProgress.Close();
            }
            vProgress.Close();
            dgvCheckResultData.DataSource = ErrorTable;
            DataGridViewCellStyle ds = new DataGridViewCellStyle();
            ds.ForeColor = Color.Red;
            for (int i = 0; i < dgvCheckResultData.Rows.Count; i++)
            {
                dgvCheckResultData.Rows[i].DefaultCellStyle = ds;
            }
            for (int j = 0; j < dgvCheckResultData.Columns.Count; j++)
            {
                dgvCheckResultData.Columns[j].HeaderText = ErrorTable.Columns[j].Caption;
                dgvCheckResultData.Columns[j].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvCheckResultData.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

        }
        private IFeatureClass GetFeatureClassByName(Dictionary<IFeatureClass, Dictionary<string, int>> dicErrorData,string LayerName)
        {
            IFeatureClass pFeatureClass = null;
            if (dicErrorData == null || dicErrorData.Count == 0) return pFeatureClass;
            foreach (IFeatureClass fs in dicErrorData.Keys)
            {
                if (fs.AliasName == LayerName)
                {
                    pFeatureClass = fs;
                    break;
                }
            }
            return pFeatureClass;
        }
        //初始化数据列表
        private System.Data.DataTable InitializeErrorTable(string TableName)
        {
            System.Data.DataTable newTable = new System.Data.DataTable(TableName);
            DataColumn coulumn1 = new DataColumn();
            coulumn1.Caption = "图层名";
            coulumn1.ColumnName = "LayerName";
            coulumn1.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn1);
            DataColumn coulumn2 = new DataColumn();
            coulumn2.Caption = "省";
            coulumn2.ColumnName = "sheng";
            coulumn2.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn2);
            DataColumn coulumn3 = new DataColumn();
            coulumn3.Caption = "市";
            coulumn3.ColumnName = "shi";
            coulumn3.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn3);
            DataColumn coulumn4 = new DataColumn();
            coulumn4.Caption = "县";
            coulumn4.ColumnName = "xian";
            coulumn4.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn4);
            DataColumn coulumn5 = new DataColumn();
            coulumn5.Caption = "乡";
            coulumn5.ColumnName = "xiang";
            coulumn5.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn5);
            DataColumn coulumn6 = new DataColumn();
            coulumn6.Caption = "村";
            coulumn6.ColumnName = "cun";
            coulumn6.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn6);
            DataColumn coulumn7 = new DataColumn();
            coulumn7.Caption = "小斑号";
            coulumn7.ColumnName = "xbh";
            coulumn7.DataType = System.Type.GetType("System.String");
            newTable.Columns.Add(coulumn7);
            return newTable;
        }

        private void btnExportDataTable_Click(object sender, EventArgs e)
        {
            if (dgvCheckItem.Rows.Count <=0) return;
            List<string> listFieldName = new List<string>();
            for (int i = 0; i < dgvCheckItem.Columns.Count; i++)
            {
                if (dgvCheckItem.Columns[i].Name != "OBJECTID")
                {
                    listFieldName.Add(dgvCheckItem.Columns[i].Name);
                }
            }
            ExportDataGridview(dgvCheckItem, listFieldName, "信息列表");
        }
        //导出数据
        public static bool ExportDataGridview(DataGridViewX gridView, List<string> lstFields, string defaultName)
        {
            //添加进度条 ygc 2012-10-8
            SysCommon.CProgress vProgress = new SysCommon.CProgress();
            vProgress.ShowDescription = true;
            vProgress.ShowProgressNumber = true;
            vProgress.TopMost = true;
            vProgress.EnableCancel = true;
            vProgress.EnableUserCancel(true);
            vProgress.ProgresssValue = 0;
            vProgress.Step = 1;
            vProgress.ShowProgress();
            vProgress.SetProgress("正在导出" + defaultName + "数据......");
            Microsoft.Office.Interop.Excel.Application excel = null;
            Workbook wb = null;
            try
            {
                if (gridView.Rows.Count == 0)
                    return false;
                //建立Excel对象
                excel = new Microsoft.Office.Interop.Excel.Application();
                wb = excel.Application.Workbooks.Add(true);
                excel.Visible = true;
                wb.Application.ActiveWindow.Caption = defaultName;

                //生成字段名称
                //for (int i = 0; i < gridView.ColumnCount; i++)
                //{
                //    if(!lstFields.Contains(gridView.Columns[i].HeaderText)) continue;
                //    excel.Cells[1, i + 1] = gridView.Columns[i].HeaderText;
                //}

                for (int i = 0; i < lstFields.Count; i++)
                {
                    //if (!lstFields.Contains(gridView.Columns[i].HeaderText)) continue;
                    excel.Cells[1, i + 1] = gridView.Columns[lstFields[i]].HeaderText;
                }

                vProgress.MaxValue = gridView.Columns.Count * gridView.Rows.Count;
                int t = 0;
                //填充数据
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    for (int j = 0; j < lstFields.Count; j++)
                    {
                        //if (!lstFields.Contains(gridView.Columns[j].HeaderText)) continue;
                        int intFieldIndex = gridView.Columns.IndexOf(gridView.Columns[lstFields[j]]);

                        if (gridView[intFieldIndex, i].ValueType == typeof(string))
                        {
                            if (gridView[intFieldIndex, i].Value != null)
                            {
                                excel.Cells[i + 2, j + 1] = "'" + gridView[intFieldIndex, i].Value.ToString();
                            }
                        }
                        else
                        {
                            if (gridView[intFieldIndex, i].Value != null)
                            {
                                excel.Cells[i + 2, j + 1] = gridView[intFieldIndex, i].Value.ToString();
                            }
                        }
                        t++;
                        vProgress.ProgresssValue = t;
                    }
                }
                vProgress.Close();
               Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
                fd.InitialFileName = defaultName;
                int result = fd.Show();
                if (result == 0) return true;
                string fileName = fd.InitialFileName;
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (fileName.IndexOf(".xls") == -1)
                    {
                        fileName += ".xls";
                    }
                    wb.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                return true;
            }
            catch
            {
                vProgress.Close();
                return false;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExportDetialTable_Click(object sender, EventArgs e)
        {
            if (dgvCheckResultData.Rows.Count <= 0) return;
            List<string> listFiledname = new List<string>();
            for (int i = 0; i < dgvCheckResultData.Columns.Count; i++)
            {
                listFiledname.Add(dgvCheckResultData .Columns [i].Name);
            }
            ExportDataGridview(dgvCheckResultData, listFiledname, "详细信息表");
        }
    }
}
