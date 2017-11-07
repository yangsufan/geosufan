using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoSysUpdate
{
    public partial class ManageSolution : DevComponents.DotNetBar.Office2007Form
    {
        public ManageSolution()
        {
            InitializeComponent();
        }
        DataTable m_dt = null;
        private void btnDelete_Click(object sender, EventArgs e)
        {
            Exception ex=null;
            if (dataGridViewX1.CurrentRow == null)
            {
                MessageBox.Show("请选择要删除的方案！","提示");
                return;
            }
            string ID = dataGridViewX1.CurrentRow.Cells["ID"].Value.ToString();
            SysCommon.Gis.SysGisTable table = new SysCommon.Gis.SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
           bool t= table.DeleteRows("customstatistics", "id='" + ID + "'", out ex);
           if (t)
           {
               MessageBox.Show("成功删除统计方案！", "提示");
               dataGridViewX1.Rows.Remove(dataGridViewX1.CurrentRow);
               dataGridViewX1.Refresh();
              // RefreshDataGridview();
               return;
           }
           else
           {
               MessageBox.Show("删除统计方案失败！", "提示");
           }
        }

        private void ManageSolution_Load(object sender, EventArgs e)
        {
             m_dt = GetDataTable(GetSolutionTable());
            if (m_dt != null)
            {

                dataGridViewX1.DataSource = m_dt;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private ITable GetSolutionTable()
        {
            IWorkspace pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
            ITable pTable = null;
            DropTable(pWorkspace, "TempStatisticsTable");
            //统计数据SQL语句
            string SQLString = "create table TempStatisticsTable as select * from customstatistics";
            pWorkspace.ExecuteSQL(SQLString);
            pTable = (pWorkspace as IFeatureWorkspace).OpenTable("TempStatisticsTable");
            return pTable;
        }
        private System.Data.DataTable GetDataTable(ITable pip_Table)
        {
            System.Data.DataTable lc_TableData = new System.Data.DataTable("统计结果");
            if (pip_Table == null) return null;
            ICursor lip_Cursor = null;
            try
            {
                // 无数据返回空表
                if (pip_Table.RowCount(null) == 0) return null;
                // 给列赋值
                for (int index = 0; index < pip_Table.Fields.FieldCount; index++)
                {
                    DataColumn lc_DataColum = new DataColumn();
                    lc_DataColum.ColumnName = pip_Table.Fields.get_Field(index).AliasName;
                    lc_DataColum.Caption = pip_Table.Fields.get_Field(index).AliasName;
                    lc_DataColum.DataType = System.Type.GetType("System.String");
                    lc_TableData.Columns.Add(lc_DataColum);
                }
                // 循环拷贝数据
                lip_Cursor = pip_Table.Search(null, false);
                if (lip_Cursor == null)
                {
                    return null;
                }
                IRow lip_Row = lip_Cursor.NextRow();
                lc_TableData.BeginLoadData();
                while (lip_Row != null)
                {
                    DataRow lc_Row = lc_TableData.NewRow();
                    for (int i = 0; i < pip_Table.Fields.FieldCount; i++)
                    {
                        string values = lip_Row.get_Value(i).ToString();
                        lc_Row[i] = values;
                    }
                    lc_TableData.Rows.Add(lc_Row);
                    lip_Row = lip_Cursor.NextRow();
                }
                lc_TableData.EndLoadData();
                return lc_TableData;
            }
            catch (Exception ex)
            {
                return lc_TableData;
            }
            finally
            {
                if (lip_Cursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lip_Cursor);
                }
            }
        }
        private void DropTable(IWorkspace pWks, string TableName)
        {
            try
            {
                pWks.ExecuteSQL("drop table " + TableName);

            }
            catch
            { }
        }
    }
}
