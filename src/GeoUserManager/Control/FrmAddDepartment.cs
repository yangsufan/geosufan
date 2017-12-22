using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Fan.Common.Gis;
using Fan.Common.Error;

namespace GeoUserManager
{
    public partial class FrmAddDepartment : DevComponents.DotNetBar.Office2007Form
    {
        public FrmAddDepartment()
        {
            InitializeComponent();
        }
        //是否属于更新
        public bool m_IsUpdate
        {
            get;
            set;
        }
        //窗体名称
        public string m_FrmText
        {
            get;
            set;
        }
        //按钮名称
        public string m_BtnText
        {
            get;
            set;
        }
        //记录科室ID
        public string m_id
        {
            get;
            set;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtDepartmentName.Text == "")
            {
                MessageBox.Show("请输入科室名称！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            //if (txtFunction.Text == "")
            //{
            //    MessageBox.Show("请输入相应科室职能！","提示",MessageBoxButtons .OK ,MessageBoxIcon .Error);
            //    return;
            //}
            Exception Exerror=null ;
            Dictionary<string, object> newdic = new Dictionary<string, object>();
            SysGisTable ksTable = new SysGisTable(Fan.Plugin.ModuleCommon.TmpWorkSpace);
            newdic.Add("DEPARTMENTNAME",txtDepartmentName .Text);
            //if (m_id != null && m_id != "")
            //{
            //    newdic.Add("DEPARTMENTID",m_id);
            //}
            newdic.Add("DEPARTMENTINFO",txtFunction.Text);
            newdic.Add("DEPARTMENTMARK",txtRemark .Text);
            //如果是更新
            if (m_IsUpdate)
            {
                if (m_id != null && m_id != "")
                {
                    if (!ksTable.UpdateRow("USER_DEPARTMENT", "DEPARTMENTID='" + m_id + "'", newdic, out Exerror))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！" + Exerror.Message);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("更新科室成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("更新科室失败！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Information);
                    return;
                }
            }
            //新建科室
            else
            {
                newdic.Add("DEPARTMENTID", Guid.NewGuid().ToString());
                if (!ksTable.ExistData("USER_DEPARTMENT", "DEPARTMENTNAME='" + txtDepartmentName.Text + "'"))
                {
                    if (!ksTable.NewRow("USER_DEPARTMENT", newdic, out Exerror))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "添加失败！" + Exerror.Message);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("添加科室成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("系统已存在该科室","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FrmAddDepartment_Load(object sender, EventArgs e)
        {
            if (m_IsUpdate)
            {
                this.Text = m_FrmText;
                btnOk.Text = m_BtnText;
                SysGisTable ksTable = new SysGisTable(Fan.Plugin.ModuleCommon.TmpWorkSpace);
                Exception error=null ;
                Dictionary<string, object> newdic = new Dictionary<string, object>();
                newdic = ksTable.GetRow("USER_DEPARTMENT", "DEPARTMENTID='" + m_id + "'",out error);
                if (newdic != null && newdic.Count != 0)
                {
                    txtDepartmentName.Text = newdic["DEPARTMENTNAME"].ToString();
                    txtFunction.Text = newdic["DEPARTMENTINFO"].ToString();
                    txtRemark.Text = newdic["DEPARTMENTMARK"].ToString();
                }
            }
        }
    }
}
