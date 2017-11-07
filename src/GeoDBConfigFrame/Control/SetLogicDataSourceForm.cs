using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;

//*********************************************************************************
//** 文件名：frmDataUpload.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-18
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDBConfigFrame
{

    public partial class SetLogicDataSourceForm : DevComponents.DotNetBar.Office2007Form
    {
        public SetLogicDataSourceForm()
        {
            InitializeComponent();
        }
        GetDataTreeInitIndex m_dIndex = new GetDataTreeInitIndex();
        private void SetLogicDataSourceForm_Load(object sender, EventArgs e)
        {
            string mypath = m_dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 数据源名称 from 物理数据源表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int i = 0; i < list.Count; i++)
            {
                comboBoxDsName.Items.Add(list[i]);
            }
            if (comboBoxDsName.Items.Count > 0)
            {
                comboBoxDsName.SelectedIndex = 0;
            }
            
            strExp = "select 行政代码 from 数据单元表";
            list = new List<string>();
            list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int i = 0; i < list.Count; i++)
            {
                comboBoxAreaCode.Items.Add(list[i]);
            }
            if (comboBoxAreaCode.Items.Count > 0)
            {
                comboBoxAreaCode.SelectedIndex = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxAreaCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAreaCode.Text != "")
            {
                string mypath = m_dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select 行政名称 from 数据单元表 where 行政代码='"+comboBoxAreaCode.Text+"'";
                 GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                 txtAreaName.Text = db.GetInfoFromMdbByExp(strCon, strExp);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (comboBoxDsName.Text == "")
            {
                MessageBox.Show("数据源为空，无法删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string mypath = m_dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "delete * from 逻辑数据源表 where 行政代码='"+comboBoxAreaCode.Text+"' and 数据源名称='"+comboBoxDsName.Text+"'";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            db.ExcuteSqlFromMdb(strCon, strExp);
            MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {

            if (comboBoxDsName.Text == "")
            {
                MessageBox.Show("请先配置物理数据源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string mypath = m_dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select count(*) from 逻辑数据源表 where 行政代码='" + comboBoxAreaCode.Text + "' and 数据源名称='" + comboBoxDsName.Text + "'";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            int i=db.GetCountFromMdb(strCon, strExp);
            if (i != 0)
            {
                MessageBox.Show("数据源已配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            strExp = "insert into 逻辑数据源表(行政代码,数据源名称) values('" + comboBoxAreaCode.Text + "','" + comboBoxDsName.Text + "')";
            db.ExcuteSqlFromMdb(strCon,strExp);
            MessageBox.Show("新建成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}