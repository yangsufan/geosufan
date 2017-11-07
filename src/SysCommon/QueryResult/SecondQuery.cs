using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace SysCommon
{
    public partial class SecondQuery : DevComponents.DotNetBar.Office2007Form
    {
        public SecondQuery()
        {
            InitializeComponent();
        }
        IList<string> listNodes = new List<string>();
        Dictionary<string, IList<string>> dicGroup = new Dictionary<string, IList<string>>();
        Dictionary<string, DataTable> dicDT = new Dictionary<string, DataTable>();
        IList<DataTable> ListDT = new List<DataTable>();
        public DataTable m_Tabel { get; set; }
        public DataRow m_Row { get; set; }
        public IFeature m_pFeature { get; set; }
        public IFeatureClass m_pFeatrueClass { get; set; }
        int frmHeight;
        int frmWidth;
        private void SecondQuery_Load(object sender, EventArgs e)
        {
            frmHeight = this.Height;
            frmWidth = this.Width;
            if (!GetGroup())
            {
                MessageBox.Show("配置文件有误！","警告");
                return;
            }
            InitializeGroups();
            InitializeDataGrid();
            try
            {
                for (int i = 0; i < m_pFeature.Fields .FieldCount; i++)
                {
                    //string temp = m_Tabel.Rows[i]["Name"].ToString();
                    string FieldName = m_pFeature.Fields.get_Field(i).Name;
                    string values = m_pFeature.get_Value(i).ToString();
                    if (FieldName != "Shape" && FieldName != "SHAPE")
                    {
                        for (int j = 0; j < listNodes.Count; j++)
                        {
                            if (dicGroup[listNodes[j]].Contains(FieldName))
                            {
                                ListDT[j].Rows.Add(new object[] { SysCommon.ModField.GetChineseNameOfField(FieldName), SysCommon.ModField .GetDomainValueOfFieldValue (m_pFeatrueClass ,FieldName ,values) });
                                break;
                            }
                            else if (listNodes[j] == "其他信息")
                            {
                                ListDT[ListDT.Count - 1].Rows.Add(new object[] { SysCommon.ModField.GetChineseNameOfField(FieldName), SysCommon.ModField.GetDomainValueOfFieldValue(m_pFeatrueClass, FieldName, values) });
                                break;
                            }
                        }
                    }
                }
             
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "");
            }
            DtToDic(ListDT);
            DataGridBindData();
        }
        //为DataGriedview绑定数据源
        private void DataGridBindData()
        {
            for (int i = 0; i < listNodes.Count; i++)
            {
                DevComponents.DotNetBar.Controls.DataGridViewX dgv = new DevComponents.DotNetBar.Controls.DataGridViewX();
                dgv = explorerBar1.Controls[i] as DataGridViewX;
                dgv.DataSource = dicDT[listNodes[i]];
                dgv = null;
            }
        }
        //初始化DataGridView
        private void InitializeDataGrid()
        {

            for (int i = 0; i < listNodes.Count; i++)
            {
                DataTable dt = new DataTable(listNodes[i]);
                dt.Columns.Add("Name", typeof(System.String));
                dt.Columns["Name"].ReadOnly = true;
                dt.Columns.Add("Value", typeof(System.String));
                dt.Columns["Value"].ReadOnly = true;
                //dicDT.Add(listNodes[i], dt);
                ListDT.Add(dt);
                //ListDT[i].TableName =listNodes[i];
                //dicDT.Add(listNodes[i],null);
                
            }
        }
        private void DtToDic(IList<DataTable> listDt)
        {
            for (int i = 0; i < ListDT.Count; i++)
            {
                dicDT.Add(listDt[i].TableName, listDt[i]);
            }
        }
        //得到分组数据类别
        private bool GetGroup()
        {
            string m_XmlPath = Application.StartupPath + "\\SecondQueryConfig.xml";
            if(m_XmlPath ==""){return false ;}
            if (!System.IO.File.Exists(m_XmlPath)) { return false ; }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(m_XmlPath);
            if (pXmldoc == null) { return false ; }
            XmlNodeList xNodeList = pXmldoc.SelectNodes("TypeNameConfig/TypeName");
            if (xNodeList.Count == 0) { return false ; }
            for (int i = 0; i < xNodeList.Count; i++)
            {
                string NodeName = xNodeList[i].Attributes["name"].Value.ToString();
                IList<string> listType = XNodeToList(xNodeList[i].ChildNodes);
                dicGroup.Add(NodeName, listType);
                listNodes.Add(NodeName);
            }
            return true;
            
        }
        //提取分组的因子
        private IList<string> XNodeToList(XmlNodeList xNodeList)
        {
            IList<string> newList = new List<string>();
            for (int i = 0; i < xNodeList.Count; i++)
            {
                string Value = xNodeList[i].Attributes["name"].Value.ToString();
                newList.Add(Value);
            }
            return newList;
        }
        //初始化控件的text
        private void InitializeGroups()
        {
            for (int i = 0; i < listNodes.Count; i++)
            {
                explorerBar1.Groups[i].Text = listNodes[i];
            }
        }

    }
}
