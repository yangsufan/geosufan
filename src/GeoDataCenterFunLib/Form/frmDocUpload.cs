using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

//*********************************************************************************
//** 文件名：frmDocUpload.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-15
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDataCenterFunLib
{
    public partial class frmDocUpload : DevComponents.DotNetBar.Office2007Form
    {
        public frmDocUpload()
        {
            InitializeComponent();
        }
        int i = 0;
        string m_soucedir="";
        public static TreeNode Node;//数据单元树返回的节点
        
        private void frmDocUpload_Load(object sender, EventArgs e)
        {
             string strExp = "";
            comboBoxOpen.SelectedIndex = 0;
            //加载行政代码
            strExp = "select 描述,专题类型 from 标准专题信息表";
            LoadData2(comboBoxType, strExp);
            //加载比例尺
            strExp = "select 描述,代码 from 比例尺代码表";
            LoadData2(comboBoxScale, strExp);
            //加载入库目录
            strExp="select 虚拟目录名 from 文档数据源信息表";
            LoadData(comboBoxSource, strExp);
            //加载年度
            strExp = "select 年度 from 数据编码表";
            LoadData(comboBoxYear, strExp);
        }

        /// <summary>
        /// 加载列表框，年度，数据源
        /// </summary>
        /// <param name="cb">列表框</param>
        /// <param name="str">需要执行的SQL语句</param>
        private void LoadData(ComboBox cb, string str)
        {
            List<string> list = new List<string>();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            list = db.GetDataReaderFromMdb(strCon, str);
            for (int i = 0; i < list.Count; i++)
            {
                if(!cb.Items.Contains(list[i]))
                cb.Items.Add(list[i]);
            }
            if (cb.Items.Count != 0)
            {
                cb.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 加载列表框,名称在前，代码在后
        /// </summary>
        /// <param name="cb">列表框</param>
        /// <param name="str">需要执行的SQL语句</param>
        private void LoadData2(ComboBox cb, string str)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            DataTable dt = db.GetDataTableFromMdb(strCon, str);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb.Items.Add(dt.Rows[i][0] + "(" + dt.Rows[i][1] + ")");
            }
            if (cb.Items.Count != 0)
            {
                cb.SelectedIndex = 0;
            }
 
        }
        private void btnServer_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dg = new FolderBrowserDialog();
            if (dg.ShowDialog() == DialogResult.OK)
            {
                comboBoxSource.Text = dg.SelectedPath;
                m_soucedir = dg.SelectedPath;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (m_soucedir==comboBoxSource.Text&&!Directory.Exists(@comboBoxSource.Text))
            {
                MessageBox.Show("数据源路径不存在!", "提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "Word文档|*.doc|PDF文档|*.pdf";
            OpenFile.Multiselect = true;
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {

                foreach (string file in OpenFile.FileNames)
                {
                    for (int j = 0; j < i; j++)
                    {
                        string strExist = listViewDoc.Items[j].Text.Trim();
                       
                        if (strExist.CompareTo(file) == 0)
                        {
                            MessageBox.Show("文件已存在于列表中", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                    }
                    listViewDoc.Items.Add(file);
                    listViewDoc.Items[i].SubItems.Add("等待入库");
                    listViewDoc.Items[i].Checked = true;
                    i++;
                }
            }

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewDoc.Items)
            {
                if (item.Checked)
                {
                    listViewDoc.Items.Remove(item);
                    i--;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listViewDoc.Items.Clear();
            i = 0;
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
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            string physicdir="";
            string strExp;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            if (comboBoxScale.Text == "" || comboBoxArea.Text == "" || comboBoxYear.Text == "" || comboBoxType.Text == "")
            {
                MessageBox.Show("各选项不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string strscale = GetCode(comboBoxScale.Text); //获得比例尺代码
            string strType = GetCode(comboBoxType.Text);//获得专题类型
            string strArea = GetCode(comboBoxArea.Text);//获得区域代码
            string strAreaname = comboBoxArea.Text.Substring(0, comboBoxArea.Text.LastIndexOf('('));//获得区域名称

            //年度 专题 行政区代码 要按照规则替换
            string dir = comboBoxYear.Text +strType +strArea;
            if (m_soucedir != comboBoxSource.Text)
            {
                strExp = "select 路径 from 文档数据源信息表 where 虚拟目录名='" + comboBoxSource.Text + "'";
                physicdir = db.GetInfoFromMdbByExp(strCon, strExp) + @"\" + dir;//路径
            }
            else
            {
                physicdir = comboBoxSource.Text + @"\" + dir;//路径
            }
            if (!Directory.Exists(physicdir))
            {
                Directory.CreateDirectory(physicdir);
            }
            foreach (ListViewItem item in listViewDoc.Items)
            {
                try
                {
                    //string[] str = item.Text.Split('.');
                    string strfile = item.Text.Substring(item.Text.LastIndexOf("\\") + 1);
                    string[] strBuffer = strfile.Split('.');
                    string strFileName = strBuffer[0].ToString();
                    string strFileType = strBuffer[1].ToString();

                    if (item.Checked && item.SubItems[1].Text == "等待入库")
                    {
                        item.SubItems[1].Text = "正在入库";
                        listViewDoc.Refresh();
                        if (File.Exists(item.Text))
                        {
                            File.Copy(item.Text, physicdir + "\\" + strfile, true);
                        }
                        strExp = string.Format("insert into 文档入库信息表 (行政代码,行政名称,年度,比例尺,专题类型,文档名称,文档类型,文档虚拟目录,处理) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                            strArea, strAreaname, comboBoxYear.Text, strscale, strType, strFileName, strFileType, comboBoxSource.Text, comboBoxOpen.SelectedIndex.ToString());
                        db.ExcuteSqlFromMdb(strCon, strExp);
                        item.SubItems[1].Text = "入库完成";
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    item.SubItems[1].Text = "入库失败";
                }

            }
            MessageBox.Show("操作已完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxArea_Click(object sender, EventArgs e)
        {
            
            GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据

            frmDataUnitTree frm = new frmDataUnitTree();//初始化数据单元树窗体
            frm.Location = new Point(this.Location.X + 58, this.Location.Y +160);
            frm.flag = 3;
            frm.ShowDialog();
            if (Node != null)//传回的Node不是NULL
            {
                if (Convert.ToInt32(Node.Tag) != 0)
                {
                    string strExp = "select 行政代码 from 数据单元表 where 行政名称='" + Node.Text + "'  and 数据单元级别='" + Node.Tag + "'";
                    string code = dbfun.GetInfoFromMdbByExp(strCon, strExp);
                    comboBoxArea.Text = Node.Text + "(" + code + ")";//为数据单元box显示数据
                }
            }
        }

        //全选按钮
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewDoc.Items.Count; i++)
            {
                listViewDoc.Items[i].Checked = true;
            }

        }
        //反选按钮
        private void btnInverse_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listViewDoc.Items.Count; i++)
            {
                if (listViewDoc.Items[i].Checked == false)
                {
                    listViewDoc.Items[i].Checked = true;
                    //datagwSource.Rows[i].Selected = true;
                }
                else
                {
                    listViewDoc.Items[i].Checked = false;
                    //datagwSource.Rows[i].Selected = false;
                }
            }
        }

        private void listViewDoc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
    }
}