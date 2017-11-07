using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SysCommon.Gis;
using SysCommon.Error;
using SysCommon.Authorize;
using System.Xml;
using System.IO;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20110314  add content:添加角色 
    /// </summary>
    public partial class AddGroup : DevComponents.DotNetBar.Office2007Form
    {
        bool m_BeUpdate = false;                //添加(true 修改)
        bool m_beSucced = true;
        public bool beSucceed
        {
            get { return m_beSucced; }
            set { m_beSucced = value; }
        }
        
        public AddGroup(bool pBeUpdate)
        {
            InitializeComponent();
            m_BeUpdate = pBeUpdate;
            //初始化角色列表框
            intialForm();
        }

        /// <summary>
        /// 初始化角色类型列表框
        /// </summary>
        private void intialForm()
        {
            SysCommon.DataBase.SysTable pSysDB=null;
            Exception outError = null;
            //连接数据库
            ModDBOperate.ConnectDB(out pSysDB, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                m_beSucced = false;
                return;
            }
            #region 初始化角色类型列表
            string selStr = "";//初始化角色类型列表的字符串
            selStr = "select * from roletypeinfo WHERE ROLETYPEID<> 3";   //查询角色类型表,3为系统管理员
            DataTable pTable = pSysDB.GetSQLTable(selStr, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色基本信息表失败,原因：" + outError.Message);
                pSysDB.CloseDbConnection();
                m_beSucced = false;
                return;
            }

            //遍历表格记录，将角色类型信息加载到ComboBox中
            for (int j = 0; j < pTable.Rows.Count; j++)
            {
                string roleID = "";     //角色ID
                string roleName = "";   //角色类型名称
                roleID = pTable.Rows[j][0].ToString().Trim();
                roleName = pTable.Rows[j][2].ToString().Trim();
                ComboBoxItem pItem = new ComboBoxItem(roleName, roleID);
                cmbGroupType.Items.Add(pItem);
            }
            if (cmbGroupType.Items.Count > 0)
            {
                cmbGroupType.SelectedIndex = 0;
            }
            #endregion

            if (m_BeUpdate)
            {
                #region 修改，初始化角色名列表框
                string str = "select * from rolebaseinfo where roletypeid<>3";  //查询角色类型表 3为系统管理员，不能修改
                DataTable RoleTable = pSysDB.GetSQLTable(str, out outError);
                if (outError != null || RoleTable == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色基本信息表失败！");
                    pSysDB.CloseDbConnection();
                    m_beSucced = false;
                    return;
                }
                if (RoleTable.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "还未添加过角色，请检查");
                    pSysDB.CloseDbConnection();
                    m_beSucced = false;
                    return;
                }
                for (int i = 0; i < RoleTable.Rows.Count; i++)
                {
                    int pRoleID = -1;   //角色ID
                    string pRoleName = ""; //角色名称
                    int pRoleTypeID = -1;   //角色类型ID
                    pRoleID = Convert.ToInt32(RoleTable.Rows[i][0].ToString().Trim());
                    pRoleName = RoleTable.Rows[i][1].ToString().Trim();
                    pRoleTypeID = Convert.ToInt32(RoleTable.Rows[i][2].ToString().Trim());
                    ComboBoxItem pItem = new ComboBoxItem(pRoleName, pRoleID);
                    cmbRoleName.Items.Add(pItem);
                }
                if (cmbRoleName.Items.Count > 0)
                {
                    cmbRoleName.SelectedIndex = 0;
                }
                #endregion
            }
            //else
            //{
               
            //}
            pSysDB.CloseDbConnection();
        }

      

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            Exception pError = null;

            try
            {
                #region 对界面进行控制
                if (txtComment.Text.Length >= 200)
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "备注内容太长！");
                    return;
                }
                if (string.IsNullOrEmpty(this.txtRole.Text.Trim()))
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "用户组名不能为空！");
                    return;
                }
                if (txtRole.Text.IndexOf(" ") != -1)
                {
                    errorProvider1.SetError(txtRole, "组名不能有空格");
                    return;
                }

                #endregion

                SysCommon.DataBase.SysTable pSysDB = null;
                //连接系统维护库
               ModDBOperate.ConnectDB(out pSysDB, out pError);
                if (pError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", pError.Message);
                    return;
                }
                int pRoleID = -1;       //角色ID
                int pRoleTypeID = -1;     //角色类型ID
                pRoleTypeID = Convert.ToInt32((cmbGroupType.SelectedItem as ComboBoxItem).Value.ToString());
                if(pRoleTypeID == -1)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色类型ID失败！");
                    pSysDB.CloseDbConnection();
                    return;
                }

                //判断是否存在同名的角色
                string tempStr = "select * from rolebaseinfo where ROLENAME='" + txtRole.Text.Trim() + "'";
                DataTable mTable = pSysDB.GetSQLTable(tempStr, out pError);
                if (pError != null || mTable == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色基本信息表失败");
                    pSysDB.CloseDbConnection();
                    return;
                }
               

                if (m_BeUpdate)
                {
                    //判断是否存在同名角色
                    if (mTable.Rows.Count > 1)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "存在同名的角色！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    #region 对界面进行控制,角色名称保护
                    if (string.IsNullOrEmpty(cmbRoleName.Text.Trim()))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "用户组名不能为空！");
                        return;
                    }
                    if (cmbRoleName.Text.Trim().IndexOf(" ") != -1)
                    {
                        errorProvider1.SetError(txtRole, "组名不能有空格");
                        return;
                    }
                   
                    #endregion

                    //判断是否存在同名的角色
                    //if (cmbRoleName.Items.Contains(txtRole.Text.Trim()))
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "存在同名的角色！");
                    //    pSysDB.CloseDbConnection();
                    //    return;
                    //}

                    #region 修改角色信息，更新角色基本信息表

                    pRoleID = Convert.ToInt32((cmbRoleName.SelectedItem as ComboBoxItem).Value.ToString().Trim()); 
                    string updateStr = "update rolebaseinfo set ROLENAME='"+txtRole.Text.Trim()+"', ROLETYPEID="+pRoleTypeID+", REMARK='"+txtComment.Text.Trim()+"' where ROLEID="+pRoleID;
                    pSysDB.UpdateTable(updateStr, out pError);
                    if (pError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改角色基本信息表失败！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    pSysDB.CloseDbConnection();

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改角色信息成功！");
                    #endregion
                }
                else
                {
                    //判断是否存在同名角色
                    if (mTable.Rows.Count > 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "存在同名的角色！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    #region 添加角色信息,往角色表里面插入记录
                  
                    //查询角色ID的最大值
                    string selStr = "select Max(ROLEID) from rolebaseinfo";
                    DataTable tempTable = pSysDB.GetSQLTable(selStr, out pError);
                    if (pError != null || tempTable == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色基本信息表失败");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    if (tempTable.Rows.Count == 0)
                    {
                        pRoleID = 1;
                    }
                    else
                    {
                        if (tempTable.Rows[0][0].ToString().Trim() == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色类型ID失败！");
                            //pSysDB.CloseDbConnection();
                            //return;
                            pRoleID = 1;
                        }
                        else
                        {
                            pRoleID = Convert.ToInt32(tempTable.Rows[0][0].ToString()) + 1;
                        }
                    }
                    if (pRoleID == -1)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色ID失败！");
                        return;
                    }
                    string str = "insert into rolebaseinfo(ROLEID,ROLENAME,ROLETYPEID,REMARK) values (";
                    str += pRoleID + ",'" + txtRole.Text.Trim() + "'," + pRoleTypeID + ",'" + txtComment.Text.Trim() + "')";
                    //更新数据库
                    pSysDB.UpdateTable(str, out pError);
                    if (pError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新角色基本信息表失败！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    pSysDB.CloseDbConnection();

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "添加角色成功！");
                    #endregion
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
               
            }
        }

        private void AddGroup_Load(object sender, EventArgs e)
        {
            if (m_BeUpdate)
            {
                this.Text = "修改角色";
                btnAddRole.Text = "修改角色";
                this.cmbRoleName.Enabled = true;
                //this.txtComment.Text = _role.Remark;
            }
            else
            {
                this.Text = "添加角色";
                btnAddRole.Text = "添加角色";
                this.cmbRoleName.Enabled = false;
                this.txtRole.Focus();
            }
            
        }

        private void txtRole_TextChanged(object sender, EventArgs e)
        {
            if (txtRole.Text.IndexOf(" ") != -1)
            {
                errorProvider1.SetError(txtRole, "组名不能有空格");
                return;
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void cmbRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据角色名称初始化角色类型

            txtRole.Text = cmbRoleName.Text.Trim();
            SysCommon.DataBase.SysTable pSysDB = null;
            Exception outError = null;
            //连接数据库
            ModDBOperate.ConnectDB(out pSysDB, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                return;
            }

            string selStr = "";//初始化角色类型列表的字符串
            selStr = "select * from roletypeinfo inner join rolebaseinfo on rolebaseinfo.ROLETYPEID=roletypeinfo.ROLETYPEID and rolebaseinfo.ROLEID=" + Convert.ToInt32((cmbRoleName.SelectedItem as ComboBoxItem).Value.ToString());   //查询角色类型表
            DataTable pTable = pSysDB.GetSQLTable(selStr, out outError);
            if (outError != null||pTable==null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色类型表失败,原因：" + outError.Message);
                pSysDB.CloseDbConnection();
                return;
            }
            if (pTable.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色类型表失败,原因：" + outError.Message);
                pSysDB.CloseDbConnection();
                return;
            }
            //遍历表格记录，将角色类型信息加载到ComboBox中
            //for (int j = 0; j < pTable.Rows.Count; j++)
            //{
                string roleTypeID = "";     //角色类型ID
                string roleTypeName = "";   //角色类型
                roleTypeID = pTable.Rows[0]["ROLETYPEID"].ToString().Trim();
                roleTypeName = pTable.Rows[0]["ROLEREMARK"].ToString().Trim();
                //ComboBoxItem pItem = new ComboBoxItem(roleTypeName, roleTypeID);
                //cmbGroupType.SelectedItem = pItem;
                cmbGroupType.Text = roleTypeName;
                //cmbGroupType.Items.Add(pItem);
            //}
            //if (cmbGroupType.Items.Count > 0)
            //{
            //    cmbGroupType.SelectedIndex = 0;
            //}

            //初始化备注列表框
            selStr = "select * from rolebaseinfo where ROLEID=" + Convert.ToInt32((cmbRoleName.SelectedItem as ComboBoxItem).Value.ToString().Trim());
            pTable = pSysDB.GetSQLTable(selStr, out outError);
            if (outError != null || pTable==null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色类型表失败,原因：" + outError.Message);
                pSysDB.CloseDbConnection();
                return;
            }
            if (pTable.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "该用户不存在！");
                pSysDB.CloseDbConnection();
                return;
            }
            txtComment.Text = pTable.Rows[0][3].ToString().Trim();

            pSysDB.CloseDbConnection();
        }
    }
}