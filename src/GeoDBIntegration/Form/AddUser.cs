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
    /// chenyafei  20100311 add  content :添加用户窗体
    /// </summary>
    public partial class AddUser : DevComponents.DotNetBar.Office2007Form
    {
        User _user = null;                      //用户类
        bool m_BeSuccedd = true;               //标志加载界面是否成功
        public bool BeSuccedd
        {
            get
            {
                return m_BeSuccedd;
            }
            set
            {
                m_BeSuccedd = value;
            }
        }

        string m_UserName = "";                //更新前的用户名，用来进行比较是否发生变化
        string m_Password = "";                //更新前的密码，用来比较是否发生变化
        string m_OldRoleName = "";             //更新前的角色名称 ,用来进行比较
        string m_Sex = "";                     //更新前的性别
        string m_Position = "";                //更新前的职称
        string m_Remark = "";                  //更新前的备注

        //bool m_isUpdate = false;                  //判断当前是否为更新

        //public User user
        //{
        //    get { return _user; }
        //    set { _user = value; }
        //}

        public AddUser(User user)
        {
            InitializeComponent();
            _user = user;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="beUpdate"></param>
        //public AddUser(bool beUpdate)
        //{
        //    InitializeComponent();
        //    //m_isUpdate = beUpdate;

        //}

        /// <summary>
        /// chenyafei 20110311 add:初始化界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser_Load(object sender, EventArgs e)
        {
            //初始化界面---------下拉列表框
            InitilComboBox();
           
            if (_user!=null)
            {
                //修改用户界面
                this.btnAddUser.Text = "更新用户";
                this.Text = "修改用户信息";
                this.txtTrueName.Text = _user.Name;
                this.txtPassword.Text = _user.Password;
                this.cmbRole.Text = _user.Role;
                this.comboSex.Text = _user.Sex;
                this.txtPosition.Text = _user.Position;
                this.txtComment.Text = _user.Remark;

                //保存旧的用户信息，用来比较基本信息和和角色是否发生变化
                m_UserName = _user.Name;
                m_Password = _user.Password;
                m_OldRoleName = _user.Role; //保存旧的角色名
                m_Sex = _user.Sex;
                m_Position = _user.Position;
                m_Remark = _user.Remark;
                //cyf 20110602 modify
                ///////////////////////////////权限判断，普通用户不能改变自己的角色，系统管理员不能改变角色/////////
                //if (_user.RoleTypeID == EnumRoleType.系统管理员.GetHashCode() || _user.RoleTypeID == EnumRoleType.普通用户.GetHashCode())
                //{
                //    this.cmbRole.Enabled = false;
                //}
                //end
            }
            else
            {
                //添加用户界面
                this.btnAddUser.Text = "添加用户";
                this.Text="添加用户";
            }
            this.txtTrueName.Focus();
            
        }

        /// <summary>
        /// chenyafei 20110311 add content:初始化界面
        /// </summary>
        private void InitilComboBox()
        {
            Exception outError = null;
            #region 初始化性别列表框
            ComboBoxItem item = new ComboBoxItem("男", 0);
            this.comboSex.Items.Add(item);
            item = new ComboBoxItem("女", 1);
            this.comboSex.Items.Add(item);
            this.comboSex.SelectedIndex = 0;
            #endregion

            #region 初始化角色列表框
            //连接系统维护库
            SysCommon.DataBase.SysTable pSysDB = null;
            ModDBOperate.ConnectDB(out pSysDB, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                m_BeSuccedd = false;
                return;
            }
            //查询角色基本信息表
            string selStr = string.Empty;
            if (this._user == null)
                selStr = "select * from rolebaseinfo WHERE rolebaseinfo.ROLETYPEID<>3";   //查询角色字符串,3为系统管理员角色，系统管理员角色不能添加
            else
            {
                //cyf 20110602 delete
                //if (this._user.RoleTypeID==EnumRoleType.系统管理员.GetHashCode())
                //    selStr = "select * from rolebaseinfo";
                //else
                //    selStr = "select * from rolebaseinfo WHERE rolebaseinfo.ROLETYPEID<>3";
                //end
            }
            DataTable pTable = pSysDB.GetSQLTable(selStr, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色基本信息表失败,原因：" + outError.Message);
                m_BeSuccedd = false;
                pSysDB.CloseDbConnection();
                return;
            }
            pSysDB.CloseDbConnection();
            //遍历表格记录，将角色信息加载到ComboBox中
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string roleID = "";     //角色ID
                string roleName = "";   //角色名称
                roleID = pTable.Rows[i][0].ToString().Trim();
                roleName = pTable.Rows[i][1].ToString().Trim();
                ComboBoxItem pItem = new ComboBoxItem(roleName, roleID);
                cmbRole.Items.Add(pItem);
            }
            if (cmbRole.Items.Count > 0)
            {
                cmbRole.SelectedIndex = 0;
            }
            #endregion
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
      
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Exception exError = null;

            try
            {
                #region 进行界面控制
                if (txtComment.Text.Length > 200)
                {
                    ErrorHandle.ShowInform("提示", "备注内容太长！");
                    txtComment.Focus();
                    return;
                }
                if (txtTrueName.Text.IndexOf(" ") != -1)
                {
                    errorProvider.SetError(txtTrueName, "用户名不能有空格");
                    return;
                }
                //验证
                if (string.IsNullOrEmpty(this.txtTrueName.Text))
                {
                    errorProvider.SetError(txtTrueName, "用户真实名不能为空！");
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtPassword.Text))
                {
                    errorProvider.SetError(txtPassword, "密码不能为空！");
                    return;
                }
                else if (this.comboSex.Text == "")
                {
                    errorProvider.SetError(comboSex, "请选择用户角色！");
                    return;
                }
                else if (this.comboSex.Text == "请选择")
                {
                    errorProvider.SetError(comboSex, "请选择性别！");
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtPosition.Text))
                {
                    errorProvider.SetError(txtPosition, "姓名不能为空！");
                    return;
                }
                if (txtTrueName.Text == "Admin")
                {
                    ErrorHandle.ShowInform("提示", "此用户已存在,请重新输入！");
                    txtTrueName.Focus();
                    return;
                }
                #endregion

                //l连接系统维护库
                SysCommon.DataBase.SysTable pSysDB = null;
                ModDBOperate.ConnectDB(out pSysDB, out exError);
                if (exError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
                    return;
                }

                #region 首先要判断用户名不能重复

                //用户名不能与超级管理员同名
                string[] arr = ModuleData.v_AppConnStr.Split(';');
                if (arr.Length != 3)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接字符串不正确！");
                    return;
                }

                if (txtTrueName.Text.Trim() == arr[1].Substring(arr[1].IndexOf('=') + 1))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "存在同名的用户！");
                    pSysDB.CloseDbConnection();
                    return;
                }

                //不能与数据库中的用户同名
                string str = "select * from userbaseinfo where USERNAME='" + txtTrueName.Text.Trim() + "'";
                DataTable pUserTable = pSysDB.GetSQLTable(str, out exError);
                if (exError != null || pUserTable == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户基本信息表失败！");
                    pSysDB.CloseDbConnection();
                    return;
                }

                #endregion

                //判断是更新还是添加
                if (_user != null)
                {
                    // （修改用户） 判断是否存在同名用户
                    if (pUserTable.Rows.Count > 1)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "存在同名的用户！");
                        pSysDB.CloseDbConnection();
                        return;
                    }

                    #region 用户修改,1、需要修改用户基本信息表，2、需要修改用户角色关系表,3、更新用户登录信息
                    pSysDB.StartTransaction();
                    //更新用户信息

                    #region 更新用户信息表
                    //若用户的基本信息发生了变更，则更新用户基本信息表
                    int pUserID = _user.ID;   //用户ID
                    int pRoleTypeID = _user.RoleTypeID;
                    string updateStr = "";
                    if (m_UserName != txtTrueName.Text.Trim() || m_Password != txtPassword.Text.Trim() || comboSex.Text.Trim() != m_Sex || txtPosition.Text.Trim() != m_Position || txtComment.Text.Trim() != m_Remark)
                    {
                        //只要其中有一项基本信息发生了变化，就更injiben信息表
                        updateStr = "update userbaseinfo set UserName='" + txtTrueName.Text.Trim() + "', Password='" + txtPassword.Text.Trim() + "', Sex='" + comboSex.Text.Trim() + "', Job='" + txtPosition.Text.Trim() + "', Remark='" + txtComment.Text.Trim() + "' where UserID=" + pUserID;
                        pSysDB.UpdateTable(updateStr, out exError);
                        if (exError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新用户信息表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                    }
                    #endregion
                    if (cmbRole.Text.Trim() != m_OldRoleName)
                    {
                        //说明用户对应的角色也进行了更新
                        #region 更新用户角色关系表
                        if ((cmbRole.SelectedItem as ComboBoxItem).Value == null || (cmbRole.SelectedItem as ComboBoxItem).Value.ToString().Trim() == "")
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色ID失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        int pRoleID = -1; //角色ID  Convert.ToInt32((cmbRole.SelectedItem as ComboBoxItem).Value.ToString().Trim()); 
                        string mStr = "select * from rolebaseinfo where ROLENAME='" + cmbRole.Text.Trim() + "'";
                        DataTable tTable = pSysDB.GetSQLTable(mStr, out exError);
                        if (exError != null || tTable == null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色信息表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        if (tTable.Rows.Count == 0)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色ID失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        pRoleID = Convert.ToInt32(tTable.Rows[0][0].ToString().Trim());
                        //首先查询用户角色关系表，看是否存在这个记录
                        //******************************************************************
                        //guozheng added 2011-3-24
                        string sSQL = "SELECT ROLEID FROM rolebaseinfo WHERE ROLETYPEID=" + _user.RoleTypeID;
                        DataTable GetTable = pSysDB.GetSQLTable(sSQL, out exError);
                        if (exError != null || GetTable == null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户角色关系表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        if (GetTable.Rows.Count < 0)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有找到该用户对应的角色信息"); return;
                        }
                        //******************************************************************
                        string tempStr = "select * from userrolerelationinfo where ROLEID=" + GetTable.Rows[0][0].ToString() + " and USERID=" + pUserID;
                        DataTable temTable = pSysDB.GetSQLTable(tempStr, out exError);
                        if (exError != null || temTable == null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户角色关系表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        if (temTable.Rows.Count == 0)
                        {
                            //若不存在，则插入关系记录
                            updateStr = "insert into userrolerelationinfo(USERID,ROLEID) values (" + pUserID + "," + pRoleID + ")";
                        }
                        else
                        {
                            //若存在，则更新关系记录
                            updateStr = "update userrolerelationinfo set ROLEID=" + pRoleID + " where USERID=" + pUserID;
                        }

                        pSysDB.UpdateTable(updateStr, out exError);
                        if (exError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新用户角色关系表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        #endregion

                        //获得角色类型
                        tempStr = "select * from rolebaseinfo where ROLEID=" + pRoleID;
                        temTable = pSysDB.GetSQLTable(tempStr, out exError);
                        if (exError != null || temTable == null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询角色基本信息表失败！");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        if (temTable.Rows.Count == 0)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在该角色");
                            pSysDB.EndTransaction(false);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        pRoleTypeID = Convert.ToInt32(temTable.Rows[0][2].ToString().Trim());
                    }
                   
                    pSysDB.EndTransaction(true);
                    pSysDB.CloseDbConnection();
                    #region 更新用户登录信息
                    ModuleData.m_User = new User();
                    ModuleData.m_User.ID = pUserID;
                    ModuleData.m_User.Name = txtTrueName.Text.Trim();
                    ModuleData.m_User.Password = txtPassword.Text.Trim();
                    ModuleData.m_User.Sex = comboSex.Text.Trim();
                    ModuleData.m_User.Position = txtPosition.Text.Trim();
                    ModuleData.m_User.Remark = txtComment.Text.Trim();
                    ModuleData.m_User.Role = cmbRole.Text.Trim();
                    ModuleData.m_User.RoleTypeID = pRoleTypeID;
                    #endregion
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改用户信息成功！");
                    #endregion
                }
                else
                {
                    //（新增用户）判断是否存在同名用户
                    if (pUserTable.Rows.Count > 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "存在同名的用户！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    //添加用户信息
                    #region 添加用户，1、需要添加用户基本信息表，2、需要添加用户角色关系表
                    pSysDB.StartTransaction();

                    #region 获得用户ID
                    int pUserID = -1;
                    string seleStr = "select Max(UserID) from userbaseinfo";
                    DataTable tempTable = pSysDB.GetSQLTable(seleStr, out exError);
                    if (exError != null || tempTable == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户信息表失败！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    if (tempTable.Rows.Count == 0)
                    {
                        pUserID = 1;
                    }
                    else
                    {
                        if (tempTable.Rows[0][0].ToString().Trim() == "")
                        {
                            pUserID = 1;
                        }
                        else
                        {
                            pUserID = Convert.ToInt32(tempTable.Rows[0][0].ToString().Trim()) + 1;
                        }
                    }
                    if (pUserID == -1)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取用户ID失败！");
                        return;
                    }
                    #endregion

                    #region 更新用户基本信息表
                    string insertStr = "insert into userbaseinfo(UserID,UserName,Password,Sex,Job,Remark) values(" + pUserID + ",'" + txtTrueName.Text.Trim() + "','" + txtPassword.Text.Trim() + "','" + comboSex.Text.Trim() + "','" + txtPosition.Text.Trim() + "','" + txtComment.Text.Trim() + "')";

                    pSysDB.UpdateTable(insertStr, out exError);
                    if (exError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新用户表信息失败！");
                        pSysDB.EndTransaction(false);
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    #endregion
                    #region 更新用户角色表

                    //获得角色ID
                    int pRoleID = -1;   //角色ID
                    if ((cmbRole.SelectedItem as ComboBoxItem).Value == null || (cmbRole.SelectedItem as ComboBoxItem).Value.ToString().Trim() == "")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取角色ID失败！");
                        pSysDB.EndTransaction(false);
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    pRoleID = Convert.ToInt32((cmbRole.SelectedItem as ComboBoxItem).Value.ToString().Trim());

                    //插入数据到角色关系表里面
                    string inseStr = "insert into userrolerelationinfo(UserID,RoleID) values(" + pUserID + "," + pRoleID + ")";
                    pSysDB.UpdateTable(inseStr, out exError);
                    if (exError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新用户角色关系表失败！");
                        pSysDB.EndTransaction(false);
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    #endregion
                    pSysDB.EndTransaction(true);
                    pSysDB.CloseDbConnection();

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示 ", "添加用户信息成功！");

                    #endregion
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                exError = ex;
            }
        }
    }
}