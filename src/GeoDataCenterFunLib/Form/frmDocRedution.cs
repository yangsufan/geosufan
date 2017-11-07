using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

//*********************************************************************************
//** 文件名：frmDocRedution.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-16
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDataCenterFunLib
{
    public partial class frmDocRedution : DevComponents.DotNetBar.Office2007Form
    {
        public frmDocRedution()
        {
            InitializeComponent();
        }

        bool m_first;//是否第一次加载列表框
        int[] m_state ={ 0, 0, 0, 0,0};//4个选项列表框选择状态
        public static TreeNode Node;//数据单元树返回的节点

        private void frmDocRedution_Load(object sender, EventArgs e)
        {
            Node = null;
            //初始化进度条
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在加载文档数据");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            m_first = true;

            LoadGridView();
            //加载虚拟目录
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 虚拟目录名 from 文档数据源信息表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int i = 0; i < list.Count; i++)
            {
                if (!comboBoxCatalog.Items.Contains(list[i]))
                    comboBoxCatalog.Items.Add(list[i]);
            }
            if (comboBoxCatalog.Items.Count > 0)
                comboBoxCatalog.SelectedIndex = 0;
          
            vProgress.Close();
            this.Activate();
        }
        private void LoadGridView()
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select * from 文档入库信息表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                datagwSource.Rows.Add(new object[] { true, dt.Rows[i]["文档名称"] });
            }
        }
        //年度选择状态
        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!m_first)
            {
                m_state[0] = comboBoxYear.SelectedIndex;
                ChangeGridView();
            }

        }
        //行政区选择状态
        private void comboBoxArea_TextChanged(object sender, EventArgs e)
        {
              ChangeGridView();
        }

        //比例尺选择状态
        private void comboBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_first)
            {
                m_state[2] = comboBoxScale.SelectedIndex;
                ChangeGridView();
            }
        }

        //专题类型
        private void comboBoxSub_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!m_first)
            {
                m_state[4] = comboBoxSub.SelectedIndex;
                ChangeGridView();
            }
        }

        //目录选择状态
        private void comboBoxCatalog_SelectedIndexChanged(object sender, EventArgs e)
        {

                ChangeGridView();
        }

       
        //第一次加载窗体时加载所有列表框
        private void comboBoxYear_Click(object sender, EventArgs e)
        {
            if (m_first)
            {
                LoadCombox();
                m_first = false;
            }

        }

        //加载列表框
        private void LoadCombox()
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string str_Exp = "select 年度 from 数据编码表";
            GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
            List<string> list = dbfun.GetDataReaderFromMdb(strCon, str_Exp);
            comboBoxYear.Items.Add("所有年度");
            for (int i = 0; i < list.Count; i++)
            {
                if (!comboBoxYear.Items.Contains(list[i]))
                    comboBoxYear.Items.Add(list[i]);

            }
            if (comboBoxYear.Items.Count > 0)
                comboBoxYear.SelectedIndex = 0;
            //str_Exp = "select 行政名称,行政代码 from 数据单元表";
            //DataTable dt = dbfun.GetDataTableFromMdb(strCon, str_Exp);
            //comboBoxArea.Items.Add("所有行政区");
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    comboBoxArea.Items.Add(dt.Rows[i]["行政名称"] + "(" + dt.Rows[i]["行政代码"] + ")");
            //}
            //if (comboBoxArea.Items.Count > 0)
            //    comboBoxArea.SelectedIndex = 0;
            str_Exp = "select 描述,代码 from 比例尺代码表";
            DataTable dt = dbfun.GetDataTableFromMdb(strCon, str_Exp);
            comboBoxScale.Items.Add("所有比例尺");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxScale.Items.Add(dt.Rows[i]["描述"] + "(" + dt.Rows[i]["代码"] + ")");
            }
            if (comboBoxScale.Items.Count > 0)
                comboBoxScale.SelectedIndex = 0;
            str_Exp = "select 描述,专题类型 from 标准专题信息表";
            dt = dbfun.GetDataTableFromMdb(strCon, str_Exp);
            comboBoxSub.Items.Add("所有专题类型");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxSub.Items.Add(dt.Rows[i]["描述"] + "(" + dt.Rows[i]["专题类型"] + ")");
            }
            if (comboBoxSub.Items.Count > 0)
                comboBoxSub.SelectedIndex = 0;

        }
        //全选按钮
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < datagwSource.Rows.Count; i++)
            {
                this.datagwSource.Rows[i].Cells[0].Value = true;
            }

        }
        //反选按钮
        private void btnInverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < datagwSource.Rows.Count; i++)
            {
                if ((bool)datagwSource.Rows[i].Cells[0].EditedFormattedValue == false)
                {
                    this.datagwSource.Rows[i].Cells[0].Value = true;
                    //datagwSource.Rows[i].Selected = true;
                }
                else
                {
                    this.datagwSource.Rows[i].Cells[0].Value = false;
                    //datagwSource.Rows[i].Selected = false;
                }
            }
        }

        //下载文档
        private void btn_Export_Click(object sender, EventArgs e)
        {
            bool flag = false;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun(); 
            string strExp="select 路径 from 文档数据源信息表 where 虚拟目录名='"+comboBoxCatalog.Text+"'";
            string path=db.GetInfoFromMdbByExp(strCon,strExp);
            foreach (DataGridViewRow row in datagwSource.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue == true)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                MessageBox.Show("没有选中行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SysCommon.CProgress vProgress = new SysCommon.CProgress("正在下载选中文档");
            try
            {
                FolderBrowserDialog dlg =new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //初始化进度条
                   
                    vProgress.EnableCancel = false;
                    vProgress.ShowDescription = true;
                    vProgress.FakeProgress = true;
                    vProgress.TopMost = true;
                    vProgress.ShowProgress();

                    string cellvalue = "";
                    foreach (DataGridViewRow row in datagwSource.Rows)
                    {
                        if ((bool)row.Cells[0].EditedFormattedValue == true)
                        {
                            cellvalue = row.Cells[1].Value.ToString().Trim();
                            strExp = "select * from 文档入库信息表 where 文档名称='"+cellvalue+"' and 文档虚拟目录='"+comboBoxCatalog.Text+"'";
                            DataTable dt= db.GetDataTableFromMdb(strCon, strExp);
                            string dir = path + "\\" + dt.Rows[0]["年度"] + dt.Rows[0]["专题类型"] + dt.Rows[0]["行政代码"] + "\\" + cellvalue + "." + dt.Rows[0]["文档类型"];
                            string filepath=dlg.SelectedPath+ "\\" + cellvalue + "." + dt.Rows[0]["文档类型"];
                            if (File.Exists(dir))
                            {
                                File.Copy(dir, filepath, true);
                            }
                        }
                    }
                    vProgress.Close();
                    MessageBox.Show("下载成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                vProgress.Close();
                this.Activate();
            }
        }

        //删除文档
        private void btn_Del_Click(object sender, EventArgs e)
        {
            
            bool flag = false;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun(); 
            string strExp="select 路径 from 文档数据源信息表 where 虚拟目录名='"+comboBoxCatalog.Text+"'";
            string path=db.GetInfoFromMdbByExp(strCon,strExp);
            foreach (DataGridViewRow row in datagwSource.Rows)
            {
                if ((bool)row.Cells[0].EditedFormattedValue == true)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                MessageBox.Show("没有选中行!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string cellvalue = "";
                foreach (DataGridViewRow row in datagwSource.Rows)
                {
                    if ((bool)row.Cells[0].EditedFormattedValue == true)
                    {
                        cellvalue = row.Cells[1].Value.ToString().Trim();
                        strExp = "select * from 文档入库信息表 where 文档名称='" + cellvalue + "' and 文档虚拟目录='" + comboBoxCatalog.Text + "'";
                        DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                        string dir = path + "\\" + dt.Rows[0]["年度"] + dt.Rows[0]["专题类型"] + dt.Rows[0]["行政代码"] + "\\" + cellvalue + "." + dt.Rows[0]["文档类型"];
                        if (File.Exists(dir))
                        {
                            File.Delete(dir);
                            dir = dir.Substring(0,dir.LastIndexOf("\\"));
                            if (Directory.GetFiles(dir).Length == 0)//如果该目录中没有其他文档，则删除该目录
                                Directory.Delete(dir);
                            strExp = "delete * from 文档入库信息表 where 文档名称='" + cellvalue + "' and 文档虚拟目录='" + comboBoxCatalog.Text + "'";
                            db.ExcuteSqlFromMdb(strCon, strExp);
                        }
                    }
                }
                datagwSource.Rows.Clear();
                LoadGridView();//重新加载数据
                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //列表框状态改变
        private void ChangeGridView()
        {
            datagwSource.Rows.Clear();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string str_Exp = "select 文档名称 from 文档入库信息表 where 文档虚拟目录='"+comboBoxCatalog.Text+"' ";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            if (Node != null && Convert.ToInt32(Node.Tag) != 0)//如果选择的不是所有行政区
            {
                switch (Convert.ToInt32(Node.Tag))
                {
                    case 1:
                        string code = GetCode(comboBoxArea.Text).Substring(0, 3);
                        str_Exp += "and 行政代码 like '" + code + "___'";
                        break;
                    case 2:
                        code = GetCode(comboBoxArea.Text).Substring(0, 4);
                        str_Exp += "and 行政代码 like '" +code + "__'";
                        break;
                    case 3:
                        str_Exp += "and 行政代码='" + GetCode(comboBoxArea.Text) + "'";
                        break;
                }
                
                
            }
            if (comboBoxScale.SelectedIndex > 0)//如果选择的不是所有比例尺
                str_Exp += "and 比例尺 ='" + GetCode(comboBoxScale.Text) + "'";
            if (comboBoxSub.SelectedIndex > 0)//如果选择的不是所有专题类型
                str_Exp += "and 专题类型='" + GetCode(comboBoxSub.Text) + "'";
            if (comboBoxYear.SelectedIndex > 0)//如果选择的不是所有年度
                str_Exp += "and 年度='" + comboBoxYear.Text + "'";

            List<string> list = db.GetDataReaderFromMdb(strCon, str_Exp);
            for (int i = 0; i < list.Count; i++)
            {
                datagwSource.Rows.Add(new object[] { true, list[i] });
            }
        }

        /// <summary>
        /// 通过描述(代码)分离获得代码
        /// </summary>
        /// <param name="strname">描述(代码)</param>
        private string GetCode(string strname)
        {
            try
            {
                string[] arr = strname.Split('(', ')');
                return arr[1];
            }
            catch
            {
                return strname;
            }
        }

        private void comboBoxArea_Click(object sender, EventArgs e)
        {
            GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            frmDataUnitTree frm = new frmDataUnitTree();//初始化数据单元树窗体
            frm.Location = new Point(this.Location.X + 45, this.Location.Y +140);
            frm.flag = 2;
            frm.ShowDialog();
            if (Node != null)//传回的Node不是NULL
            {
                if (Convert.ToInt32(Node.Tag) != 0)
                {

                    string strExp = "select 行政代码 from 数据单元表 where 行政名称='" + Node.Text + "'  and 数据单元级别='" + Node.Tag + "'";
                    string code = dbfun.GetInfoFromMdbByExp(strCon, strExp);
                    comboBoxArea.Text = Node.Text + "(" + code + ")";//为数据单元box显示数据
                }
                else
                {
                    comboBoxArea.Text = Node.Text;//为数据单元box显示数据
                }


            }
            
        }
    }
}