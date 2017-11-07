using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;

namespace GeoDBATool
{
    public partial class frmNameRule :DevComponents.DotNetBar.Office2007Form
    {

        private TextBox tb;
        public frmNameRule(TextBox textbox,IWorkspace pworkspace)
        {
            InitializeComponent();
            tb = textbox;
            m_TempWorkspace = pworkspace;
            mSystable = new SysGisTable(pworkspace);
        }
        private IWorkspace m_TempWorkspace = null;
        string mypath = ModData.v_DbInfopath;
        private string prename;//获取和设置前缀文本框内容
        private SysGisTable mSystable ;
        private string[] array = new string[6];

        private void frmNameRule_Load(object sender, EventArgs e)
        {
            prename = tb.Text;
            LoadCombox();

            if (prename != "")
            {
                AnalyseDataToArray(prename);
                ChangeComboxText();
            }
        }
        /// <summary>
        /// 根据textbox加载列表框text
        /// </summary>
        private void ChangeComboxText()
        {
             Exception err=null;
            Dictionary<string, object> dic = mSystable.GetRow("行政区字典表", "CODE='" + array[3] + "'", out err);
            if (dic != null)
            {
                comboBoxArea.SelectedItem = dic["NAME"] + "(" + dic["CODE"] + ")";
            }
            comboBoxYear.Text = array[1];
        }

        /// <summary>
        /// 加载列表框
        /// </summary>
        private void LoadCombox()
        {  
            Exception err=null;
            //加载行政代码
            List<Dictionary<string,object>> listdic= mSystable.GetRows("行政区字典表", "XZJB=3", out err);
            if (listdic != null)
            {
                foreach (Dictionary<string, object> dic in listdic)
                {
                    string str = dic["NAME"] + "(" + dic["CODE"] + ")";
                    if (!comboBoxArea.Items.Contains(str))
                        comboBoxArea.Items.Add(str);
                }
            }
            if (comboBoxArea.Items.Count > 0)
                comboBoxArea.SelectedIndex = 0;
            listdic = mSystable.GetRows("年度代码表", "", out err);
            if (listdic != null)
            {
                foreach (Dictionary<string, object> dic in listdic)
                {
                    string stryear = dic["CODE"].ToString();
                    if (!comboBoxYear.Items.Contains(stryear))
                        comboBoxYear.Items.Add(stryear);
                }
            }
            if (comboBoxYear.Items.Count > 0)
                comboBoxYear.SelectedIndex = 0;
        }

      

        /// <summary>
        /// 通过描述(代码)分离获得代码
        /// </summary>
        /// <param name="strname">描述(代码)</param>
        private string GetCode(string strname,out bool right)
        {
            try
            {
                string[] arr = strname.Split('(', ')');
                right = true;
                return arr[1];
            }
            catch
            {
                MessageBox.Show("描述（代码）格式不正确!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                right = false;
                return strname;
            }
        }


        /// <summary>
        /// 将数据分析成字符串数组
        /// </summary>
        /// <param name="filename">数据名称</param>
        public void AnalyseDataToArray(string filename)
        {
            try
            {
                array[3] = filename.Substring(4, filename.LastIndexOf("_") - 4);
                array[1] = filename.Substring(filename.LastIndexOf("_") + 1);
            }
            catch { }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool right=true;
            prename = "";//清空原有数据
            // array[0] = GetCode(comboBoxBus.Text);//业务大类代码如DZ
            array[1] = comboBoxYear.Text;//年度如2009
            // array[2] = GetCode(comboBoxType.Text);//专题代码如01
            array[3] = GetCode(comboBoxArea.Text,out right);//区域如420683
            if (!right)
                return;
            //array[4] = GetCode(comboBoxScale.Text);//比例尺如G

           Regex regex=new Regex(@"[1-9]\d{3}");
           if (!regex.IsMatch(array[1]))
           {
               MessageBox.Show("年度不规范!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
           }
            regex=new Regex(@"[1-9]\d*");
            if (!regex.IsMatch(array[3]))
            {
                MessageBox.Show("行政代码不规范!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            prename = "LYZY" + array[3] + "_" + array[1];

            //生成前缀
            tb.Text = prename;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNameRule_FormClosed(object sender, FormClosedEventArgs e)
        {
            mSystable = null;
        }

       
    }
}