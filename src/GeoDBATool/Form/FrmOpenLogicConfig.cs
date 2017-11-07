using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;

namespace GeoDBATool
{
    public partial class FrmOpenLogicConfig : DevComponents.DotNetBar.Office2007Form
    {
        public FrmOpenLogicConfig()
        {
            InitializeComponent();
        }
       public IWorkspace m_Workspace
        {
            get;
            set;
        }
        private void FrmOpenLogicConfig_Load(object sender, EventArgs e)
        {
            if (m_Workspace == null) this.Close();
            InitialGridView(m_Workspace,"逻辑检查");
        }

        private void dgvLogicCheckConfig_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLogicCheckConfig.CurrentRow == null) return;
            txtName.Text  = dgvLogicCheckConfig.CurrentRow.Cells["CHECKNAME"].Value.ToString();
            txtCondition.Text  = dgvLogicCheckConfig.CurrentRow.Cells["CONDITION"].Value.ToString();
            txtRemark.Text  = dgvLogicCheckConfig.CurrentRow.Cells["REMARK"].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "新增")
            {
                txtName.Text = "";
                txtCondition.Text = "";
                txtRemark.Text = "";
                txtName.Enabled = true;
                txtCondition.Enabled = true;
                txtRemark.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnClose.Enabled = false;
                dgvLogicCheckConfig.Enabled = false;
                btnAdd.Text = "确定";
            }
            else if (btnAdd.Text == "确定")
            {
                if (txtName.Text == "")
                {
                    MessageBox.Show("请输入检查条件名称!","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                    return;
                }
                if (txtCondition.Text == "")
                {
                    MessageBox.Show("请输入检查条件!","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                    return;
                }
                string CheckName = txtName.Text;
                string Condition = txtCondition.Text;
                string Remark = txtRemark.Text;
                Exception  ex=null ;
                try
                {
                    IFeatureWorkspace pFeaureWorkspace = m_Workspace as IFeatureWorkspace;
                    ITable pTable = pFeaureWorkspace.OpenTable("逻辑检查");
                    Dictionary <string ,object > newdic=new Dictionary<string,object> ();
                    newdic .Add ("CheckName",CheckName);
                    newdic .Add ("condition",Condition);
                    newdic .Add ("Remark",Remark );
                   bool flag= ModGisPub.NewRow(pTable,newdic,out ex);
                   if (flag)
                   {
                       MessageBox.Show("成功新建检查条件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       InitialGridView(m_Workspace, "逻辑检查");
                   }
                   else
                   {
                       MessageBox.Show("创建检查条件失败:"+ex.ToString (),"提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                       return;
                   }
                   txtCondition.Text = "";
                   txtRemark.Text = "";
                   txtName.Enabled = false;
                   txtCondition.Enabled = false;
                   txtRemark.Enabled = false;
                   btnUpdate.Enabled = true;
                   btnDelete.Enabled = true;
                   btnClose.Enabled = true;
                   dgvLogicCheckConfig.Enabled = true;
                   btnAdd.Text = "新增";

                }
                catch
                { }
            }
        }
        private void InitialGridView(IWorkspace pWorkspace, string TableName)
        {
            DataTable ConfigTable=null ;
            try
            {
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable(TableName);
                ConfigTable = ModGisPub.ITableToDataTable(pTable, "逻辑检查配置");
                dgvLogicCheckConfig.DataSource = ConfigTable;
            }
            catch { }
            //数据表格微调
            if (ConfigTable == null) return;
            for (int i = 0; i < dgvLogicCheckConfig.Columns.Count; i++)
            {
                switch (dgvLogicCheckConfig.Columns[i].HeaderText)
                {
                    case "OBJECTID":
                        dgvLogicCheckConfig.Columns[i].Visible = false;
                        break;
                    case "CHECKNAME":
                        dgvLogicCheckConfig.Columns[i].HeaderText = "检查项名称";
                        break;
                    case "CONDITION":
                        dgvLogicCheckConfig.Columns[i].HeaderText = "检查条件";
                        break;
                    case "REMARK":
                        dgvLogicCheckConfig.Columns[i].HeaderText = "检查说明";
                        break;
                }

                dgvLogicCheckConfig.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvLogicCheckConfig.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLogicCheckConfig.CurrentRow == null) return;
            string OID = dgvLogicCheckConfig.CurrentRow.Cells["OBJECTID"].Value.ToString();
            string condition = "OBJECTID='" + OID + "'";
            if (MessageBox.Show("是否确定删除选中检查项？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes) return;
            if(m_Workspace ==null) return ;
            Exception ex=null ;
            IFeatureWorkspace pFeatureWorkspace=m_Workspace as IFeatureWorkspace ;
            ITable pTable = pFeatureWorkspace.OpenTable("逻辑检查");
            bool flag= ModGisPub.DelRow(pTable, condition, out ex);
            if (flag)
            {
                MessageBox.Show("成功删除该检查项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCondition.Text = "";
                txtName.Text = "";
                txtRemark.Text = "";
                InitialGridView(m_Workspace, "逻辑检查");
            }
            else
            {
                MessageBox.Show("删除改检查项失败"+ex.ToString (),"提示", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvLogicCheckConfig.CurrentRow == null) return;
            if (btnUpdate.Text == "修改")
            {
                txtCondition.Enabled = true;
                txtName.Enabled = true;
                txtRemark.Enabled = true;
                btnClose.Enabled = false;
                btnDelete.Enabled = false;
                btnAdd.Enabled = false;
                dgvLogicCheckConfig.Enabled = false;
                btnUpdate.Text = "确定";
            }
            else if (btnUpdate.Text == "确定")
            {
                string CheckName = txtName.Text;
                string Condition = txtCondition.Text;
                string Remark = txtRemark.Text;
                Exception ex = null;
                string OID = dgvLogicCheckConfig.CurrentRow.Cells["OBJECTID"].Value.ToString();
                string con = "OBJECTID='" + OID + "'";
                try
                {
                    IFeatureWorkspace pFeaureWorkspace = m_Workspace as IFeatureWorkspace;
                    ITable pTable = pFeaureWorkspace.OpenTable("逻辑检查");
                    Dictionary<string, object> newdic = new Dictionary<string, object>();
                    newdic.Add("CheckName", CheckName);
                    newdic.Add("condition", Condition);
                    newdic.Add("Remark", Remark);
                    bool flag = ModGisPub.UpdateRow(pTable ,con ,newdic ,out ex);
                    if (flag)
                    {
                        MessageBox.Show("成功修改该检查项", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InitialGridView(m_Workspace, "逻辑检查");
                    }
                    else
                    {
                        MessageBox.Show("修改该检查项失败！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                        return;
                    }
                    txtCondition.Text = "";
                    txtName.Text = "";
                    txtRemark.Text = "";
                    txtCondition.Enabled = false;
                    txtName.Enabled = false;
                    txtRemark.Enabled = false;
                    btnClose.Enabled = true;
                    btnDelete.Enabled = true;
                    btnAdd.Enabled = true;
                    dgvLogicCheckConfig.Enabled = true;
                    btnUpdate.Text = "修改";
                }
                catch
                { }
            }
        }
    }
}
