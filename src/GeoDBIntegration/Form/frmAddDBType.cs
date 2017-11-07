using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;

namespace GeoDBIntegration
{
    /// <summary>
    /// yjl 20120808  add   ：添加数据库类型
    /// </summary>
    public partial class frmAddDBType : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.AdvTree ProjectTree;  //数据库工程树
        public bool Success = false;
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public frmAddDBType(DevComponents.AdvTree.AdvTree projectTree)
        {
            InitializeComponent();

            if (projectTree.SelectedNode == null) return;
            txtDBType.Text = projectTree.SelectedNode.Text.Trim();  //工程名

            ProjectTree = projectTree;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Exception pError = null;

            //首先检查工程名臣是否符合规范
            if (!CheckProjectName())
            {
                return;
            }

            if (checkUnique())
            {
                //新建数据库类型                
                if (ModuleData.TempWks == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统维护库连接失败！");
                    return;
                }
                try
                {
                    Exception ex;
                    string sMax = getNextMaxCode(ModuleData.TempWks,out ex);
                    if (ex == null)
                    {
                        Dictionary<string, object> dicData = new Dictionary<string, object>();
                        dicData.Add("ID", sMax);
                        dicData.Add("databasetype", txtDBType.Text);
                        SysGisTable sysTable = new SysGisTable(ModuleData.TempWks);
                        sysTable.NewRow("DATABASETYPEMD", dicData, out ex);
                        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "添加数据库类型成功！");
                        //ProjectTree.Nodes.Clear();
                        (ModuleData.v_AppDBIntegra.MainUserControl as UserControlDBIntegra).InitProjectTree();
                        Success = true;
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("添加数据库类型【" + txtDBType.Text + "】成功！");
                        }
                        this.Close();
                    }
                    else
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    }
                }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "添加数据库类型出错！");
                    return;
                }
                //end
                //刷新界面
            }
            
        }
        //检查数据库类型名字的唯一性
        private bool checkUnique()
        {
            IQueryDef pQueryDes = (ModuleData.TempWks as IFeatureWorkspace).CreateQueryDef();
            try
            {
                //cyf 20110602
                ////数据库类型
                pQueryDes.Tables = "DATABASETYPEMD";
                pQueryDes.SubFields = "DATABASETYPE";
                pQueryDes.WhereClause = "DATABASETYPE='" + txtDBType.Text + "'";
                ICursor pCursor = null;
                pCursor = pQueryDes.Evaluate();
                if (pCursor == null) return true;
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "已存在此数据库类型！");
                    return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }
        //获取下个最大的数据库类型ID值
        private string getNextMaxCode(IWorkspace pW, out Exception ex)
        {
            string res = ""; ex = null;
            IFeatureWorkspace pFW = pW as IFeatureWorkspace;
            IQueryDef pQD = pFW.CreateQueryDef();
            pQD.SubFields = "max(ID)";
            pQD.Tables = "DATABASETYPEMD";
            ICursor pCsr = null;
            try
            {
                pCsr = pQD.Evaluate();
                IRow pR = pCsr.NextRow();
                string sCode = pR.get_Value(0).ToString();//DBNull在转换除字符串外的 类型会报错
                if (sCode == "")
                {

                    res = 1.ToString();
                }
                else
                {
                    res = (Convert.ToInt32(sCode) + 1).ToString();
                }
            }
            catch (Exception e)
            {
                ex = e;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCsr);
            }

            return res;
        }

        private void txtProjectName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ("'".Contains(e.KeyChar.ToString()))
            {
                e.KeyChar = Char.MinValue;
                DevComponents.DotNetBar.SuperTooltipInfo info = new DevComponents.DotNetBar.SuperTooltipInfo();
                info.HeaderText = "不能包含字符 '";
                superTooltip.SetSuperTooltip(txtDBType, info);
            }
        }

        private void txtProjectName_TextChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 检查工程名称名称
        /// </summary>
        /// <returns>返回工程名称是否合法</returns>
        private bool CheckProjectName()
        {
            string ProjectName = this.txtDBType.Text.Trim();
            if ("" == ProjectName)
                return false;
            string s_nonlicet = string.Empty;
            if (ProjectName.Contains("<")) s_nonlicet += "<,";
            if (ProjectName.Contains("\"")) s_nonlicet += "\",";
            if (ProjectName.Contains("%")) s_nonlicet += "%,";
            if (ProjectName.Contains("?")) s_nonlicet += "?,";
            if (ProjectName.Contains("^")) s_nonlicet += "^,";
            if (ProjectName.Contains("\'")) s_nonlicet += "\',";
            if (ProjectName.Contains("&")) s_nonlicet += "&,";
            if (ProjectName.Contains("$")) s_nonlicet += "$,";
            if (ProjectName.Contains("#")) s_nonlicet += "#,";
            if (ProjectName.Contains("@")) s_nonlicet += "@,";
            if (ProjectName.Contains("~")) s_nonlicet += "~,";
            if (ProjectName.Contains("*")) s_nonlicet += "*,";
            if (ProjectName.Contains("!")) s_nonlicet += "!,";
            if (ProjectName.Contains(">")) s_nonlicet += ">,";
            if (ProjectName.Contains("/")) s_nonlicet += "/,";
            if (ProjectName.Contains("\\")) s_nonlicet += "\\,";
            if (ProjectName.Contains(";")) s_nonlicet += ";,";
            if (s_nonlicet != string.Empty)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "非法字符：" + s_nonlicet);
                return false;
            }
            else
                return true;

        }
    }
}