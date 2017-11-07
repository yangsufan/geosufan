using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

//*********************************************************************************
//** 文件名：frmDataUpload.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-25
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDataCenterFunLib
{
    public partial class frmNameRule :DevComponents.DotNetBar.Office2007Form
    {

        private TextBox tb;
        public frmNameRule(TextBox textbox)
        {
            InitializeComponent();
            tb = textbox;
        }

        private string prename;//获取和设置前缀文本框内容

        string[] array = new string[6];

        private void frmNameRule_Load(object sender, EventArgs e)
        {
            prename=tb.Text;
            LoadCombox();
            if (prename.Length > 14)
            {
                //写数据编码表
                AnalyseDataToArray(prename);
                ChangeComboxText();

            }
        }
        /// <summary>
        /// 根据textbox加载列表框text
        /// </summary>
        private void ChangeComboxText()
        {
            //加载业务大类代码
            string strExp = "select 描述,代码 from 业务大类代码表 where 代码='" + array[0] + "'";
            LoadComboxText(comboBoxBus, strExp);
            //加载行政代码
            strExp = "select 行政名称,行政代码 from 数据单元表 where 行政代码='"+array[3]+"'";
            LoadComboxText(comboBoxArea, strExp);
            //加载业务小类代码
            strExp = "select 描述,业务小类代码 from 业务小类代码表 where 业务小类代码='"+array[2]+"'and 业务大类代码='"+array[0]+"'";
            LoadComboxText(comboBoxType, strExp);
            //加载比例尺
            strExp = "select 描述,代码 from 比例尺代码表 where 代码='"+array[4]+"'";
            LoadComboxText(comboBoxScale, strExp);
             //加载年度
            comboBoxYear.Text = array[1];
        }
        /// <summary>
        ///  加载列表框text
        /// </summary>
        /// <param name="cb">列表框</param>
        /// <param name="str">需要执行的SQL语句</param>
        private void LoadComboxText(ComboBox cb, string str)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            DataTable dt = db.GetDataTableFromMdb(strCon, str);
            cb.Text=dt.Rows[0][0] + "(" + dt.Rows[0][1] + ")";
        }

        /// <summary>
        /// 加载列表框
        /// </summary>
        private void LoadCombox()
        {  
            //加载业务大类代码
            string strExp = "select 描述,代码 from 业务大类代码表";
            LoadData2(comboBoxBus, strExp);
            //加载行政代码
            strExp = "select 行政名称,行政代码 from 数据单元表 order by 行政代码";
            LoadData2(comboBoxArea, strExp);
            //加载业务小类代码
            //comboBoxType.Items.Add
            //加载比例尺
            strExp = "select 描述,代码 from 比例尺代码表";
            LoadData2(comboBoxScale, strExp);
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
                if (!cb.Items.Contains(list[i]))
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
                MessageBox.Show("描述（代码）格式不正确!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return strname;
            }
        }


        /// <summary>
        /// 将数据分析成字符串数组
        /// </summary>
        /// <param name="filename">数据名称</param>
        public void AnalyseDataToArray(string filename)
        {
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 字段名称 from 图层命名规则表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            for (int i = 0; i < arrName.Length; i++)
            {
                switch (arrName[i])
                {
                    case "业务大类代码":
                        array[0] = filename.Substring(0, 2);//业务大类代码
                        filename = filename.Remove(0, 2);
                        break;
                    case "年度":
                        array[1] = filename.Substring(0, 4);//年度
                        filename = filename.Remove(0, 4);
                        break;
                    case "业务小类代码":
                        array[2] = filename.Substring(0, 2);//业务小类代码
                        filename = filename.Remove(0, 2);
                        break;
                    case "行政代码":
                        array[3] = filename.Substring(0, 6);//行政代码
                        filename = filename.Remove(0, 6);
                        break;
                    case "比例尺":
                        array[4] = filename.Substring(0, 1);//比例尺
                        filename = filename.Remove(0, 1);
                        break;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            prename = "";//清空原有数据
            array[0] = GetCode(comboBoxBus.Text);//业务大类代码如DZ
            array[1] = comboBoxYear.Text;//年度如2009
            array[2] = GetCode(comboBoxType.Text);//专题代码如01
            array[3] = GetCode(comboBoxArea.Text);//区域如420683
            array[4] = GetCode(comboBoxScale.Text);//比例尺如G
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp = "select 命名规则 from 图层命名规则表";
            string strRegex = db.GetInfoFromMdbByExp(strCon, strExp);//正则表达式
            strExp="select 字段名称 from 图层命名规则表";
            string strname=db.GetInfoFromMdbByExp(strCon,strExp);
            try
            {
                string[] arrRegex = strRegex.Split('(', ')');//分离正则表达式
                string[] arrName = strname.Split('+');//分离字段名称

                Regex regex;

                for (int i = 0; i < arrName.Length; i++)
                {
                    regex = new Regex(arrRegex[2 * i + 1]);
                    switch (arrName[i])
                    {
                        case "业务大类代码":
                            if (!regex.IsMatch(array[0]))//匹配业务大类代码
                            {
                                MessageBox.Show("业务大类代码不符合命名规则!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else prename += array[0];
                            break;
                        case "年度":
                            if (!regex.IsMatch(array[1]))//匹配年度
                            {
                                MessageBox.Show("年度不符合命名规则!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else prename += array[1];
                            break;
                        case "业务小类代码":
                            if (!regex.IsMatch(array[2]))//匹配业务小类代码
                            {
                                MessageBox.Show("业务小类代码不符合命名规则!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else prename += array[2];
                            break;
                        case "行政代码":
                            if (!regex.IsMatch(array[3]))//匹配行政代码
                            {
                                MessageBox.Show("行政代码不符合命名规则!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else prename += array[3];
                            break;
                        case "比例尺":
                            if (!regex.IsMatch(array[4]))//匹配比例尺
                            {
                                MessageBox.Show("比例尺不符合命名规则!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else prename += array[4];
                            break;
                    }

                }
            }
            catch
            {
                MessageBox.Show("没有命名规则!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //生成前缀
            tb.Text = prename;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxType.Items.Clear();
           string  strExp = "select 描述,业务小类代码 from 业务小类代码表 where 业务大类代码='" + GetCode(comboBoxBus.Text) +"'";
            LoadData2(comboBoxType, strExp);
        }
    }
}