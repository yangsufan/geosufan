using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;

namespace GeoDataManagerFrame
{
    public partial class FrmSaveMxd : DevComponents.DotNetBar.Office2007Form
    {
        public FrmSaveMxd(IWorkspace pWS)
        {
            m_pWorkspace = pWS;
            InitializeComponent();
        }
        //获取当前登录用户的用户名
        public string user = (Plugin.ModuleCommon.AppUser == null) ? "" : Plugin.ModuleCommon.AppUser.Name;
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
        public string m_Name
        {
            get;
            set;
        }
        public string m_Description
        {
            get;
            set;
        }
        public bool m_Share
        {
            get;
            set;
        }
        public Dictionary<string, object> _Dic = null;
        private void FrmSaveSQLSolution_Load(object sender, EventArgs e)
        {
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
            m_Name = txtSolutionName.Text;
            m_Description = RichTxtDescription.Text.Trim();
            if (this.CheckShared.Checked)
            {
                m_Share = true;
            }
            else
            {
                m_Share = false;
            }
            SysGisTable sysTable = new SysGisTable(m_pWorkspace);
            Exception eError = null;
            m_Condition = SysCommon.ModSysSetting._MxdListTable_NameField + "='" + this.txtSolutionName + "' and " + SysCommon.ModSysSetting._MxdListTable_UserField + "='" + user + "'";
            object objValue = sysTable.GetFieldValue(SysCommon.ModSysSetting._MxdListTable, SysCommon.ModSysSetting._MxdListTable_MapField, m_Condition, out eError);
            sysTable = null;
            //判断方案名称是否重复 ygc 2012-9-7
            if (objValue!=null)
            {
                DialogResult pRes= MessageBox.Show("该显示方案名已存在，是否覆盖?","询问",MessageBoxButtons.YesNo,MessageBoxIcon.Question );
                if (pRes == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
