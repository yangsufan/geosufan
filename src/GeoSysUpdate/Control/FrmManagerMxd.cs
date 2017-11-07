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
    public partial class FrmManagerMxd : DevComponents.DotNetBar.Office2007Form
    {
        public FrmManagerMxd(IWorkspace pWorkspace)
        {
            m_pWorkspace = pWorkspace;
            InitializeComponent();
        }
        //获取当前登录用户的用户名
        string user = (Plugin.ModuleCommon.AppUser == null) ? "" : Plugin.ModuleCommon.AppUser.Name;
        DataTable m_DataTable;
        private IWorkspace m_pWorkspace
        {
            get;
            set;
        }
        public string m_LayerId
        {
            get;
            set;
        }
        public string m_ID
        {
            get;
            set;
        }
        public string m_TableName
        {
            get;
            set;
        }
        public string m_Condition
        {
            get;
            set;
        }
        private string _DvCaption = "方案名称";
        public string m_DvCaption
        {
            set { _DvCaption = value; }
        }
        private DataTable ITableToDataTable(ITable pip_Table)
        {
            DataTable lc_TableData = new DataTable("显示方案");
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
                    lc_DataColum.Caption = pip_Table.Fields.get_Field(index).AliasName;
                    lc_DataColum.ColumnName = pip_Table.Fields.get_Field(index).AliasName;
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
        private void FrmManagerMxd_Load(object sender, EventArgs e)
        {
            dataGridViewX1.Columns["CmnSolutionName"].HeaderText = _DvCaption;
            m_DataTable = GetSelectData(SysCommon.ModSysSetting._MxdListTable);
            if (m_DataTable == null||m_DataTable .Rows.Count <=0)
            {
                MessageBox.Show("无已存显示方案！","提示");
                return;
            }
            dataGridViewX1.DataSource = m_DataTable;
        }
        private DataTable GetSelectData(string TableName)
        {
            DataTable  dt = null;
            DropTable("TempTable", m_pWorkspace);
            try
            {
                m_pWorkspace.ExecuteSQL("create table TempTable as select objectid as ID," + SysCommon.ModSysSetting._MxdListTable_NameField + " as SOLUTIONNAME," + SysCommon.ModSysSetting._MxdListTable_DescripField + " as DESCRIPTION," + SysCommon.ModSysSetting._MxdListTable_UserField + " as LOGINUSER from  " + SysCommon.ModSysSetting._MxdListTable + "  where (" + SysCommon.ModSysSetting._MxdListTable_UserField + "='" + user + "' or " + SysCommon.ModSysSetting._MxdListTable_ShareField + "='" + true.ToString() + "')");
                IFeatureWorkspace pWs = m_pWorkspace as IFeatureWorkspace;
                ITable pTable = pWs.OpenTable("TempTable");
                dt = ITableToDataTable(pTable);
            }
            catch
            { }
            finally
            {
                DropTable("TempTable", m_pWorkspace);
            }
            return dt;
        }
        private void DropTable(string TableName,IWorkspace pW)
        {
            try
            {
                pW.ExecuteSQL("Drop table "+TableName);
            }
            catch
            { }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.CurrentRow == null) return;
            string strID = dataGridViewX1.CurrentRow.Cells["ID"].Value.ToString();
            if (strID == "" || strID == null)
            {
                MessageBox.Show("请选择要打开的显示方案！","提示");
                return;
            }
            m_ID = strID;
            m_Condition = SysCommon.ModSysSetting._MxdListTable_IDField + "=" + m_ID;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnDeleteSQL_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.CurrentRow == null) return;
            string ID = dataGridViewX1.CurrentRow.Cells["ID"].Value.ToString();
            string userName = dataGridViewX1.CurrentRow.Cells["CmnLoginUser"].Value.ToString();
            if (userName != user)
            {
                MessageBox.Show("您不能删除其他用户的显示方案！","提示");
                return;
            }
            int index=dataGridViewX1 .CurrentRow.Index ;
            if (ID == null || ID == "")
            {
                MessageBox.Show("请选择要删除的解决方案！","提示");
                return;
            }
            if (m_pWorkspace == null) return;
            DialogResult result = MessageBox.Show("确定要删除该选择方案吗？","提示",MessageBoxButtons .OKCancel ,MessageBoxIcon.Question);
            if (result != DialogResult.OK) return;
            m_pWorkspace.ExecuteSQL("delete from " + SysCommon.ModSysSetting._MxdListTable +" where "+SysCommon.ModSysSetting._MxdListTable_IDField+"="+ID);
            DataGridViewRow row = dataGridViewX1.Rows[index];
            dataGridViewX1.Rows.Remove(row);
            dataGridViewX1.Refresh();
            //MessageBox.Show("成功删除显示方案！","提示");

        }
    }
}
