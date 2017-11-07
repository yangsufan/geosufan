using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GeoDataCenterFunLib
{
    public partial class frmBindSource : DevComponents.DotNetBar.Office2007Form
    {
        public frmBindSource(int ii)
        {
            InitializeComponent();
            index = ii;
        }

        private int index=0;//判断是文档库还是影像库
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string cellValue;
            if (e.RowIndex >= 0)
            {
                try
                {
                    if (dataGridView.Columns[e.ColumnIndex].GetType() == typeof(DataGridViewButtonColumn))
                    {
                        cellValue = dataGridView.Rows[e.RowIndex].Cells["Column5"].Value.ToString();
                        if (index == 0)
                        {
                           
                            frmSelectRaster frm = new frmSelectRaster();
                            frm.value = cellValue;
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                               if(frm.value.Trim()!="")
                                dataGridView.Rows[e.RowIndex].Cells["Column5"].Value = frm.value;
                            }
                        }
                        else
                        {
                            frmSelectDoc frm = new frmSelectDoc();
                            frm.value = cellValue;
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                if (frm.value.Trim() != "")
                                dataGridView.Rows[e.RowIndex].Cells["Column5"].Value = frm.value;
                            }
                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    //ErrorMessage(ex.Message);
                    return;
                }
            }

        }

        //加载列表
        private void frmBindRasterSource_Load(object sender, EventArgs e)
        {

            if (index == 0)
            {
                this.Text = "影像库挂接";
                this.dataGridView.Columns["Column5"].HeaderText = "影像库";
                LoadGirdViewRatster();
            }
            else
            {
                this.Text = "文档库挂接";
                this.dataGridView.Columns["Column5"].HeaderText = "文档库";
                LoadGirdViewDoc();
            }
           
        }
        private void LoadGirdViewRatster()
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select * from 地图入库信息表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
            DataGridViewButtonCell cell = new DataGridViewButtonCell();
            cell.Value ="...";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strExp = "select 描述 from  标准专题信息表 where 专题类型='" + dt.Rows[i]["专题类型"].ToString() + "'";
                string name = db.GetInfoFromMdbByExp(strCon, strExp);
                dataGridView.Rows.Add(new object[] { dt.Rows[i]["专题类型"].ToString(),name, dt.Rows[i]["年度"].ToString(), dt.Rows[i]["行政名称"].ToString(), dt.Rows[i]["比例尺"].ToString(), dt.Rows[i]["影像库"].ToString(),cell});
            }

        }

        private void LoadGirdViewDoc()
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select * from 地图入库信息表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
            DataGridViewButtonCell cell = new DataGridViewButtonCell();
            cell.Value = "...";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strExp = "select 描述 from  标准专题信息表 where 专题类型='" + dt.Rows[i]["专题类型"].ToString() + "'";
                string name = db.GetInfoFromMdbByExp(strCon, strExp);
                dataGridView.Rows.Add(new object[] { dt.Rows[i]["专题类型"].ToString(), name, dt.Rows[i]["年度"].ToString(), dt.Rows[i]["行政名称"].ToString(), dt.Rows[i]["比例尺"].ToString(), dt.Rows[i]["文档库"].ToString(), cell });
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string strtype = "";
                string stryear = "";
                string strareaname = "";
                string strscale = "";
                string sourepath = "";

                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    sourepath = dataGridView.Rows[i].Cells["Column5"].Value.ToString();//数据源
                    if (sourepath == "") continue;
                    strtype = dataGridView.Rows[i].Cells["Column1"].Value.ToString();//专题类型
                    stryear = dataGridView.Rows[i].Cells["Column2"].Value.ToString();//年度
                    strareaname = dataGridView.Rows[i].Cells["Column4"].Value.ToString();//行政单元
                    strscale = dataGridView.Rows[i].Cells["Column7"].Value.ToString();//比例尺

                    if (index == 0)
                    {

                        GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                        string mypath = dIndex.GetDbInfo();
                        string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                        string strExp = string.Format("update 地图入库信息表 set 影像库='{0}' where 专题类型='{1}' and 年度='{2}' and 行政名称='{3}' and 比例尺='{4}' ",
                            sourepath, strtype, stryear, strareaname, strscale);
                        GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                        db.ExcuteSqlFromMdb(strCon, strExp);
                    }
                    else
                    {
                        GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                        string mypath = dIndex.GetDbInfo();
                        string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                        string strExp = string.Format("update 地图入库信息表 set 文档库='{0}' where 专题类型='{1}' and 年度='{2}' and 行政名称='{3}' and 比例尺='{4}' ",
                            sourepath, strtype, stryear, strareaname, strscale);
                        GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                        db.ExcuteSqlFromMdb(strCon, strExp);
                    }
                }
                MessageBox.Show("信息已保存！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}