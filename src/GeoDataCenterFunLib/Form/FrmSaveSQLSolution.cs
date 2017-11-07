using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataCenterFunLib
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
        public string m_layerName
        {
            get;
            set;
        }
        private void FrmSaveSQLSolution_Load(object sender, EventArgs e)
        {
            RtxtCondition.Text = m_Condition;
            this.Text = _FrmText;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (m_pWorkspace == null) return;
            if (txtSolutionName.Text.Trim() == "")
            {
                MessageBox.Show("请输入该解决方案的名称！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Information );
                txtSolutionName.Focus();
                return;
            }
            string SolutionName = txtSolutionName.Text.Trim();

            if (GetExist(m_TableName, "LONGINUSER='" + user + "' and SOLUTIONNAME='" + SolutionName + "'"))
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
                //string Condition = GetNewString(m_Condition); //为什么将单引号转成双引号？转换后，打开查询方案查不到数据了
                string Condition = m_Condition;  //changed by chulili 2012-01-11 取消单引号向双引号的转换
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
                bool flag = AddRow(m_TableName, m_LayerID, user, t, SolutionName, Condition, Sdescription, m_layerName);

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
        private bool AddRow(string tableName, string layerID, string longinUser, int isshare, string solutionName, string condition, string description,string Layername)
        {
            //bool falg = false;
            //if (m_pWorkspace == null) return falg;
            //string SQLstring = "insert into " + tableName + " (layerid,longinuser,isshare,solutionname,condition,description,layername) values('" + layerID + "','" + longinUser + "', " + isshare + " , '" + solutionName + "','" + condition + "','" + description + "','"+Layername+"')";
            //try
            //{
            //    m_pWorkspace.ExecuteSQL(SQLstring);
            //    falg = true;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //return falg;
            bool flag = false;
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_pWorkspace);
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            dicData.Add("layerid", layerID);
            dicData.Add("longinuser", longinUser);
            dicData.Add("isshare", isshare);
            dicData.Add("solutionname", solutionName);
            dicData.Add("condition", condition);
            dicData.Add("description", description);
            dicData.Add("layername", Layername);
            try
            {
                Exception exError = null;
                if (!sysTable.NewRow(tableName, dicData, out exError))
                {
                    flag = false;
                }
                else
                {
                    flag = true;
                }
            }
            catch (System.Exception ex)
            {
                flag = false;
            }
            
            dicData.Clear();
            dicData = null;
            sysTable = null;
            return flag;

        }
        private bool GetExist(string tableName, string condition)
        {
            //List<string> newList = new List<string>();
            //if (m_pWorkspace == null) return newList;
            //DropTable("tempTable");
            //m_pWorkspace.ExecuteSQL("create table tempTable as select " + Key + " from " + tableName + " where " + condition );
            //IFeatureWorkspace pWs = m_pWorkspace as IFeatureWorkspace;
            //ITable pTable = pWs.OpenTable("tempTable");
            //ICursor pCursor = pTable.Search(null ,false);
            //try
            //{
            //    if (pCursor != null)
            //    {
            //        IRow row = pCursor.NextRow();
            //        while (row != null)
            //        {
            //            newList.Add(row.get_Value(0).ToString());
            //            row = pCursor.NextRow();
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            //finally
            //{
            //    DropTable("tempTable");
            //    if (pCursor != null)
            //    {
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            //    }
            //}
            //return newList;
            bool bExist = false; 
            SysCommon.Gis.SysGisTable pTable = new SysCommon.Gis.SysGisTable(m_pWorkspace);
            try
            {
                if (pTable.ExistData(tableName, condition))
                {
                    bExist = true;
                }
            }
            catch (System.Exception ex)
            {
                bExist = false;
            }
            
            pTable = null;
            return bExist;

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
