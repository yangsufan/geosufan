using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoProperties.UserControls
{
    public partial class FrmSaveSQLSolution : DevComponents.DotNetBar.Office2007Form
    {
        public FrmSaveSQLSolution(IWorkspace pWS)
        {
            m_pWorkspace = pWS;
            InitializeComponent();
        }
        //获取当前登录用户的用户名
        string user = (Plugin.ModuleCommon.AppUser == null) ? "" : Plugin.ModuleCommon.AppUser.Name;
        private IWorkspace m_pWorkspace
        {
            get;
            set;
        }
        public string m_Condition
        {
            get;
            set;
        }
        public string m_LayerID
        {
            get;
            set;
        }
        public string m_TableName
        {
            get;
            set;
        }
        private string _FrmText = "保存查询解决方案";
        public string m_FrmText
        {
            set 
            {
                _FrmText = value;
            }
        }
        private void FrmSaveSQLSolution_Load(object sender, EventArgs e)
        {
            RtxtCondition.Text = m_Condition;
            this.Text = _FrmText;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_pWorkspace == null) return;
            if (txtSolutionName.Text == "")
            {
                MessageBox.Show("请输入改解决方案的名称！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Information );
                txtSolutionName.Focus();
                return;
            }
            string SolutionName = txtSolutionName.Text.Trim();
            List<string> list = GetFiledValues(m_TableName, "SOLUTIONNAME", "LONGINUSER='" + user + "'");
            //判断方案名称是否重复 ygc 2012-9-7
            if (list.Contains(txtSolutionName.Text))
            {
                MessageBox.Show("该解决方案名已存在，请重新输入解决方案名称！","提示");
                return;
            }
            string Sdescription = RichTxtDescription.Text;
            if (Sdescription == "")
            {
                Sdescription += "无";
            }
            int t = CheckShared .Checked  == true ? 1 : 0;
            try
            {
                string Condition = GetNewString(m_Condition);
                if (Condition.Length > 3000)
                {
                    MessageBox.Show("条件语句过长，请精简后重试！","提示");
                    return;
                }
                if (Sdescription.Length > 300)
                {
                    MessageBox.Show("描述信息过长，请精简后重试","提示");
                    return;
                }
                bool flag = AddRow(m_TableName, m_LayerID, user, t, txtSolutionName.Text, Condition, Sdescription);

                if (flag)
                {
                    MessageBox.Show("保存解决方案成功！", "提示");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString ());
            }
            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
        private bool AddRow(string tableName, string layerID, string longinUser, int isshare, string solutionName, string condition, string description)
        {
            bool falg = false;
            if (m_pWorkspace == null) return falg;
            string SQLstring = "insert into " + tableName + " (layerid,longinuser,isshare,solutionname,condition,description) values('" + layerID + "','" + longinUser + "', " + isshare + " , '" + solutionName + "','" + condition + "','" + description + "')";
            try
            {
                m_pWorkspace.ExecuteSQL(SQLstring);
                falg = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return falg;
        }
        private List<string> GetFiledValues(string tableName, string Key, string condition)
        {
            List<string> newList = new List<string>();
            if (m_pWorkspace == null) return newList;
            DropTable("tempTable");
            m_pWorkspace.ExecuteSQL("create table tempTable as select " + Key + " from " + tableName + " where " + condition );
            IFeatureWorkspace pWs = m_pWorkspace as IFeatureWorkspace;
            ITable pTable = pWs.OpenTable("tempTable");
            ICursor pCursor = pTable.Search(null ,false);
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
                m_pWorkspace.ExecuteSQL("drop table "+tableName);
            }
            catch
            { }
        }
        private string GetNewString(string Condition)
        {
            string newstring="";
            if (Condition.Contains("'"))
            {
                newstring = Condition.Replace("'", "''");
                return newstring;
            }
            else
            {
                return Condition;
            }
        }
    }
}
