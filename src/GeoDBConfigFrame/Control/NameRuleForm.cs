using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;

namespace GeoDBConfigFrame
{
    public partial class NameRuleForm : DevComponents.DotNetBar.Office2007Form
    {
        public NameRuleForm()
        {
            InitializeComponent();
        }

        private string strfildname;//得到字段名称
        
        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
            this.Dispose(true);
        }

        //添加字段
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddField frm = new AddField();
            frm.flag = 1;
            frm.ShowDialog();
        }

        //编辑字段
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择一项进行编辑!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            AddField frm = new AddField();
            frm.flag = 2;
            frm.fleldname = listView.SelectedItems[0].Text.Trim();
            frm.ShowDialog();

        }

        //上移
        private void btnUp_Click(object sender, EventArgs e)
        {

            if (listView.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择一项进行移动!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(listView.SelectedItems[0].Index==0)
                return;
            listView.BeginUpdate();
            ListViewItem item=listView.SelectedItems[0];
            int index = listView.SelectedItems[0].Index;
            listView.Items.RemoveAt(index);
            listView.Items.Insert(index - 1, item);
            listView.EndUpdate();
            ChangeIndex();
        }

        //下移
        private void btnDown_Click(object sender, EventArgs e)
        {

            if (listView.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择一项进行移动!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (listView.SelectedItems[0].Index ==listView.Items.Count-1)
                return;
            listView.BeginUpdate();
            ListViewItem item = listView.SelectedItems[0];
            int index = listView.SelectedItems[0].Index;
            listView.Items.RemoveAt(index);
            listView.Items.Insert(index +1, item);
            listView.EndUpdate();
            ChangeIndex();
        }

        //删除
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择一项进行删除!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = listView.SelectedItems[0].Index;//取得选中项索引
            DialogResult result=MessageBox.Show("是否确定删除此项?", "提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string name = listView.Items[index].Text;
                string strExp = "delete * from 图层命名初始化表 where 字段名称='" +name+"'";
                db.ExcuteSqlFromMdb(strCon, strExp);
                listView.Items.RemoveAt(index);
                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ChangeIndex()
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp = "";
            foreach(ListViewItem item in listView.Items)
            {
                int index = item.Index + 1;
                strExp = "update 图层命名初始化表 set 排序='" +index+"' where 字段名称='" + item.Text + "'";
                db.ExcuteSqlFromMdb(strCon, strExp);
            }
        }
        private void NameRuleForm_Load(object sender, EventArgs e)
        {
            LoadListView();
        }
        private void LoadListView()
        {
           listView.Items.Clear();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
          //  string strExp = "select * from 图层命名初始化表 order by 排序";
            string strExp = "select 字段名称,描述,字段类型,字段长度,缺省,排序 from 图层命名初始化表 order by 排序";
            DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listView.Items.Add(dt.Rows[i]["字段名称"].ToString());
                listView.Items[i].SubItems.Add(dt.Rows[i]["描述"].ToString());
                listView.Items[i].SubItems.Add(dt.Rows[i]["字段类型"].ToString());
                listView.Items[i].SubItems.Add(dt.Rows[i]["字段长度"].ToString());
                listView.Items[i].SubItems.Add(dt.Rows[i]["缺省"].ToString());
            }

        }

        private void NameRuleForm_Activated(object sender, EventArgs e)
        {
            LoadListView();
        }

        private void listView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView.SelectedItems.Count != 1)
            {
                MessageBox.Show("请选择一项进行编辑!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            AddField frm = new AddField();
            frm.flag = 2;
            frm.fleldname = listView.SelectedItems[0].Text.Trim();
            frm.ShowDialog();
        }

        //生成正则表达式
        private void btnGet_Click(object sender, EventArgs e)
        {
            textBoxExp.Text = "";
            textBoxExample.Text = "";
            strfildname = "";
            string type;//字段类型
           string Length;//字段长度
            string strregex="";//表达式
            string strName = "";//字段名称
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp = "";
            foreach(ListViewItem item in listView.Items)
            {
                type=item.SubItems[2].Text;
                Length =item.SubItems[3].Text;
                strName = item.Text;
                switch (strName)
                {
                    case "业务大类代码":
                        strregex += "(^[A-Z]{"+Length+"}$)";
                        break;
                    case "年度":
                        strregex += "(^20[0-9]{2}$)";
                        break;
                    case "业务小类代码":
                        strregex += "(^[0-9]{"+Length+"}$)";
                        break;
                    case "行政代码":
                        int ln=(Convert.ToInt32(Length))-1;
                        strregex += "(^[1-9][0-9]{"+ln+"}$)";
                        break;
                    case "比例尺":
                        strregex += "(^[B-I]{"+Length+"}$)";
                        break;
                }
                strfildname += item.Text+"+";
                textBoxExample.Text += item.SubItems[4].Text;
                int index = item.Index + 1;
                strExp = "update 图层命名初始化表 set 排序='" +  index+ "' where 字段名称='" + item.Text + "'";
                db.ExcuteSqlFromMdb(strCon,strExp);

            }
            textBoxExp.Text = strregex;
            strfildname = strfildname.Substring(0, strfildname.LastIndexOf("+"));
        }

        //插入数据库
        private void btnOk_Click(object sender, EventArgs e)
        {
            string Tambole="";
            strfildname = "";
            string type;//字段类型
            string Length;//字段长度
            string strregex = "";//表达式
            string strName = "";//字段名称
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp = "";
            foreach (ListViewItem item in listView.Items)
            {
                type = item.SubItems[2].Text;
                Length = item.SubItems[3].Text;
                strName = item.Text;
                switch (strName)
                {
                    case "业务大类代码":
                        strregex += "(^[A-Z]{" + Length + "}$)";
                        break;
                    case "年度":
                        strregex += "(^20[0-9]{2}$)";
                        break;
                    case "业务小类代码":
                        strregex += "(^[0-9]{" + Length + "}$)";
                        break;
                    case "行政代码":
                        int ln = (Convert.ToInt32(Length)) - 1;
                        strregex += "(^[1-9][0-9]{" + ln + "}$)";
                        break;
                    case "比例尺":
                        strregex += "(^[B-I]{" + Length + "}$)";
                        break;
                }
                strfildname += item.Text + "+";
                Tambole += item.SubItems[4].Text;
                int index = item.Index + 1;
                strExp = "update 图层命名初始化表 set 排序='" + index + "' where 字段名称='" + item.Text + "'";
                db.ExcuteSqlFromMdb(strCon, strExp);

            }
            strfildname = strfildname.Substring(0, strfildname.LastIndexOf("+"));
                strExp = "delete * from  图层命名规则表";
                db.ExcuteSqlFromMdb(strCon, strExp);
                strExp = "insert into 图层命名规则表(命名规则,示例,字段名称) values('" + strregex + "','" + Tambole + "','" + strfildname + "')";
                db.ExcuteSqlFromMdb(strCon, strExp);
                MessageBox.Show("命名规则已改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }
    }
}