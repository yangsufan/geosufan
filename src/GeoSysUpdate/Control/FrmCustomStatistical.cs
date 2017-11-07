using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GeoSysUpdate
{
    public partial class FrmCustomStatistical : DevComponents.DotNetBar.Office2007Form
    {
        public FrmCustomStatistical()
        {
            InitializeComponent();
        }
        public string m_SolutionName
        {
            get;
            set;
        }
        public string m_StatisticsField
        {
            get;
            set;
        }
        public string m_ClassField
        {
            get;
            set;
        }
        public IFeatureClass m_pFeatureClass
        {
            get;
            set;
        }
        public DialogResult result
        {
            get;
            set;
        }
        private List<string> ListStatistics;
        private List<string> ListClassField;
        public string m_StaticsUnit
        { get; set; }
        private IWorkspace m_pWorkspace = null;
        private string LayerId = "";
        private void txtLayerName_Click(object sender, EventArgs e)
        {
            Plugin.SelectLayerByTree frm = new Plugin.SelectLayerByTree(1);
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm.m_NodeKey.Trim() != "")
                {
                    m_pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, frm._LayerTreePath, frm.m_NodeKey);
                }
                LayerId = frm.m_NodeKey;
                m_pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
                if (m_pFeatureClass != null)
                {

                    txtLayerName.Text = frm.m_NodeText;
                }
                InitializeComBox();
            }
        }
        //初始化下拉框
        //ygc 2012-8-16
        private void InitializeComBox()
        {
            if (m_pFeatureClass == null)
            {
                MessageBox.Show("无查询结果数据！", "提示");
                return;
            }
            ListStatistics = new List<string>();
            ListClassField = new List<string>();
            //清空已有元素--ygc 2012-9-4
            CBoxStatisticsField.Items.Clear(); ;
            CBoxClassField.Items.Clear();
            IFields pFields = m_pFeatureClass.Fields;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeDouble || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeSingle || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeInteger)
                {
                    ListStatistics.Add(pFields.get_Field(i).Name.ToString());
                    CBoxStatisticsField.Items.Add(SysCommon.ModField.GetChineseNameOfField(pFields.get_Field(i).Name.ToString()));
                }
                ListClassField.Add(pFields.get_Field(i).Name.ToString());
                CBoxClassField.Items.Add(SysCommon.ModField.GetChineseNameOfField(pFields.get_Field(i).Name.ToString()));
            }
            if (CBoxStatisticsField.Items.Count > 0)
            {
                CBoxStatisticsField.SelectedIndex = 0;
            }
            if (CBoxClassField.Items.Count > 0)
            {
                CBoxClassField.SelectedIndex = 0;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close ();
            GC.Collect ();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtSolutionName.Text == "")
            {
                MessageBox.Show("请输入解决方案名称！", "提示", MessageBoxButtons.OK);
                return;
            }
            m_SolutionName = txtSolutionName.Text;
            if (txtLayerName.Text == "")
            {
                MessageBox.Show("请选择统计的目标图层！","提示",MessageBoxButtons .OK);
                return;
            }
            if (CBoxClassField.Text == "")
            {
               MessageBox.Show("请选择分类字段！","提示",MessageBoxButtons.OK);
               return;
            }
            if (CBoxStatisticsField.Text == "")
            {
                MessageBox.Show("请选择统计字段!","提示",MessageBoxButtons .OK);
                return;
            }
            result = DialogResult.OK;
            this.Close();
        }

        private void CBoxStatisticsField_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_StatisticsField = "";
            int index = CBoxStatisticsField.SelectedIndex;
            m_StatisticsField = ListStatistics[index];
        }

        private void CBoxClassField_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_ClassField = "";
            int index = CBoxClassField.SelectedIndex;
            m_ClassField = ListClassField[index];
        }

        private void btnSaveSolution_Click(object sender, EventArgs e)
        {
            if (m_pFeatureClass == null) return;
            string LayerName = txtLayerName.Text.ToString();
            if (LayerName == "")
            {
                MessageBox.Show("请选择统计的图层！","提示");
                return;
            }
            string SolutionName = txtSolutionName.Text.ToString();
            if (SolutionName == "")
            {
                MessageBox.Show("请输入该统计方案名称！","提示");
                return;
            }
            List<string> listName = GetFiledValues("CUSTOMSTATISTICS", "STATISTICSNAME");
            if (listName.Contains(SolutionName))
            {
                MessageBox.Show("该方案名称已存在！","提示");
                return ;
            }
            bool t = AddRow("CUSTOMSTATISTICS", LayerId, LayerName, m_StatisticsField, SolutionName, m_ClassField, m_StaticsUnit);
            if (t)
            {
                MessageBox.Show("保存统计方案成功！", "提示");
            }
            else
            {
                MessageBox.Show("保存统计方案失败！","提示");
            }
        
        }
        private bool AddRow(string tableName, string layerID, string layername, string  statisticfield, string solutionName, string classfiyfield, string statisticunit)
        {
            bool falg = false;
            if (m_pWorkspace == null) return falg;
            string SQLstring = "insert into " + tableName + " (LAYERID,LAYERNAME,STATISTICSFIELD,STATISTICSNAME,CLASSIFYFIELD,STATISTICSUNIT) values('" + layerID + "','" + layername + "', '" + statisticfield + "' , '" + solutionName + "','" + classfiyfield + "','" + statisticunit + "')";
            try
            {
                m_pWorkspace.ExecuteSQL(SQLstring);
                falg = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return falg;
            }
            return falg;
        }
        private List<string> GetFiledValues(string tableName, string Key)
        {
            List<string> newList = new List<string>();
            if (m_pWorkspace == null) return newList;
            DropTable("tempTable");
            m_pWorkspace.ExecuteSQL("create table tempTable as select " + Key + " from " + tableName);
            IFeatureWorkspace pWs = m_pWorkspace as IFeatureWorkspace;
            ITable pTable = pWs.OpenTable("tempTable");
            ICursor pCursor = pTable.Search(null, false);
            try
            {
                if (pCursor != null)
                {
                    IRow row = pCursor.NextRow();
                    while (row != null)
                    {
                        newList.Add(row.get_Value(0).ToString());
                        row = pCursor.NextRow();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                DropTable("tempTable");
                if (pCursor != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
            }
            return newList;
        }
        private void DropTable(string tableName)
        {
            try
            {
                m_pWorkspace.ExecuteSQL("drop table " + tableName);
            }
            catch
            { }
        }
    }
}
