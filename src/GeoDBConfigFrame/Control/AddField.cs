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
    public partial class AddField :  DevComponents.DotNetBar.Office2007Form
    {
        public AddField()
        {
            InitializeComponent();
        }
        private int m_flag=1;//1表示添加字段，2表示编辑字段
        private string m_fleldname;//获取字段名称
        private int id;
        public int flag
        {
            set { m_flag = value; }
        }
        public string fleldname
        {
            set { m_fleldname = value; }
        }

        private void AddField_Load(object sender, EventArgs e)
        {
            
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp;
            if (m_flag == 1)//添加字段
                comboBoxType.SelectedIndex = 0;
            else//编辑字段
            {
                strExp = "select * from 图层命名初始化表 where 字段名称='" + m_fleldname + "'";
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                comBoxName.Text = m_fleldname;
                textBoxDescribe.Text = dt.Rows[0]["描述"].ToString();
                comboBoxType.Text = dt.Rows[0]["字段类型"].ToString();
                textBoxLength.Text = dt.Rows[0]["字段长度"].ToString();
                checkBoxChange.Checked = (bool)(dt.Rows[0]["可变"]);
                textBoxDefault.Text = dt.Rows[0]["缺省"].ToString();
                id = Convert.ToInt32(dt.Rows[0]["ID"]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (comBoxName.Text == "")
            {
                MessageBox.Show("字段名称不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (textBoxDefault.Text == "")
            {
                MessageBox.Show("缺省值不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp;
            if (m_flag == 1)
            {
                strExp = "select max(排序) from 图层命名初始化表";
                int index = Convert.ToInt32(db.GetInfoFromMdbByExp(strCon,strExp)) + 1;
                strExp = string.Format("insert into 图层命名初始化表(字段名称,描述,字段类型,字段长度,缺省,可变,排序) values('{0}','{1}','{2}','{3}','{4}',{5},'{6}')",
                    comBoxName.Text, textBoxDescribe.Text, comboBoxType.Text, textBoxLength.Text, textBoxDefault.Text, checkBoxChange.Checked,index.ToString());
                db.ExcuteSqlFromMdb(strCon, strExp);
                MessageBox.Show("添加字段成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                strExp = string.Format("update 图层命名初始化表 set 字段名称='{0}',描述='{1}',字段类型='{2}',字段长度='{3}',缺省='{4}',可变={5} where ID={6}",
                     comBoxName.Text, textBoxDescribe.Text, comboBoxType.Text, textBoxLength.Text, textBoxDefault.Text, checkBoxChange.Checked,id);
                db.ExcuteSqlFromMdb(strCon, strExp);
                MessageBox.Show("编辑字段成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }    
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxChange_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxChange.Checked == true)
            {
                textBoxLength.Enabled = false;
                textBoxLength.Text = "";
            }
            else
                textBoxLength.Enabled = true;
        }

    }
}