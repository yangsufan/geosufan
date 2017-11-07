using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;

//订单查询功能    ZQ  20110727  add
namespace GeoUtilities
{
    public partial class frmOrderTaskQuery : DevComponents.DotNetBar.Office2007Form
    {
        //订单存储的数据库地址
        private string DDanPath = Application.StartupPath + "\\..\\OrderTask\\订单数据库.mdb";
        private IMapControlDefault m_MapControl;
        private IWorkspace m_WS = null;
        public frmOrderTaskQuery(IMapControlDefault pMapControl, IWorkspace pWs)
        {
            InitializeComponent();
            initialization();
            m_MapControl = pMapControl;
            m_WS = pWs;
        }
        #region 界面初始化    ZQ   20110727
        private void initialization()
        {
            object[] TagDBType = new object[] { "编号", "制表人", "订单状态", "制表日期" };
            cmboxTJ.Items.AddRange(TagDBType);
            cmboxTJ.SelectedIndex = 0;
            object[] TagZTType = new object[] { "未批准", "已批准" };
            cmboxGJZ.Items.AddRange(TagZTType);
            cmboxGJZ.SelectedIndex = 0;

        }

        #endregion 
        #region   基本操作 ZQ   20110727   
        private void cmboxTJ_SelectedIndexChanged(object sender, EventArgs e)
        {
           switch (cmboxTJ.SelectedItem.ToString())
           {
               case "编号":
               case "制表人":
                   txtGJZ.Visible = true;
                   cmboxGJZ.Visible = false;
                   dateTimeGJZ.Visible = false;
                   break;
               case "订单状态":
                   txtGJZ.Visible = false;
                   cmboxGJZ.Visible = true;
                   dateTimeGJZ.Visible = false;
                   break;
               case "制表日期":
                   txtGJZ.Visible = false;
                   cmboxGJZ.Visible = false;
                   dateTimeGJZ.Visible = true;
                   break;  
           }
        }
        private void bttnClear_Click(object sender, EventArgs e)
        {
            if(lstBoxQuery.Items.Count>0)
            {
                lstBoxQuery.Items.Clear();
            }
        }

        private void bttQX_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion
        #region 订单查询   ZQ  20110727 
        private DataTable GetAccesssTable(string strSQL)
        {
            DataTable pDataTable = null;
            try
            {
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DDanPath);
                OleDbCommand pOleDbCommand = con.CreateCommand();
                pOleDbCommand.CommandText = strSQL;
                try
                {
                    con.Open();
                }
                catch { MessageBox.Show("数据库连接失败！", "提示！"); }
                OleDbDataAdapter pOleDbDataAdapter = new OleDbDataAdapter(pOleDbCommand.CommandText, con);
                DataSet pDataSet = new DataSet();
                pOleDbDataAdapter.Fill(pDataSet, "订单表");
                pDataTable = pDataSet.Tables["订单表"];
                con.Close();
                return pDataTable;
            }
            catch
            {
                return pDataTable=null;
            }
        }
        private void SQLQuery()
        {
            lstBoxQuery.Items.Clear();
            string strSQL = "";
            switch (cmboxTJ.SelectedItem.ToString())
            {
                case "编号":
                case "制表人":
                    strSQL = "select 编号 from 订单表 where " + cmboxTJ.SelectedItem.ToString() + " like '%" + txtGJZ.Text.Trim() + "%'";
                    break;
                case "订单状态":
                    strSQL = "select 编号 from 订单表 where " + cmboxTJ.SelectedItem.ToString() + " ='" + cmboxGJZ.SelectedItem.ToString() + "'";
                    break;
                case "制表日期":
                    strSQL = "select 编号 from 订单表 where " + cmboxTJ.SelectedItem.ToString() + " =DateValue('" + dateTimeGJZ.Text + "')";
                    break;
            }
            DataTable pDataTable = GetAccesssTable(strSQL);
            if (pDataTable == null)
            { lstBoxQuery.Items.Add("未查到匹配结果！"); return; }
            for (int i = 0; i < pDataTable.Rows.Count; i++)
            {
                lstBoxQuery.Items.Add(pDataTable.Rows[i][0].ToString());
            }
        }
  
        private void bttQuery_Click(object sender, EventArgs e)
        {
            if (txtGJZ.Visible == true && txtGJZ.Text == "") { return; }
            if (dateTimeGJZ.Visible == true && dateTimeGJZ.Text == "") { return; }
            SQLQuery();
           
        } 
        private void txtGJZ_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (txtGJZ.Visible == true && txtGJZ.Text == "") { return; }
                SQLQuery();
            }
        }
        #endregion
        #region  查看订单详细信息    ZQ   20110727    add
        private void lstBoxQuery_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstBoxQuery.SelectedItem.ToString() == "") return;
                string strSQL = lstBoxQuery.SelectedItem.ToString();
                List<string> strValue = new List<string>();
                strSQL = "select * from 订单表 where 编号='" + strSQL + "'";
                DataTable pDataTable = GetAccesssTable(strSQL);
                if (pDataTable == null) return;
                try
                {
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("编号")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("制表日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("数据内容")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("提供依据")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("索取单位")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("处理部门")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("协议编号")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("特殊说明")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("数据范围")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("主要用途")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("国地信")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("密级")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("SN")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("PN")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("格式")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("数据量")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("涉及图幅数")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("技术服务费用")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("用户确认签字")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("用户确认签字日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("用户接收签字")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("用户接收签字日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("联系人")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("联系电话")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("邮政编码")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("Email")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("联系地址")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("备注")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("部门负责人")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("部门负责人签字日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("技术加工人")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("技术加工人签字日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("成果检查人")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("成果检查人签字日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("成果接收人")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("成果接收人签字日期")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("订单状态")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("制表人")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("DLG")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("DEM")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("DOM")].ToString());
                    strValue.Add(pDataTable.Rows[0][pDataTable.Columns.IndexOf("图幅号")].ToString());
                }
                catch { }
                this.WindowState = FormWindowState.Minimized;
                frmOrderTask pfrmOrderTask = new frmOrderTask(m_MapControl, "订单信息", strValue, m_WS);
                pfrmOrderTask.ShowDialog();
            }
            catch { }
        }
        #endregion


    }
}
