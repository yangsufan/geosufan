using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace GeoDBIntegration
{
    /// <summary>
    /// chenyafei  20110311  add  content ： 用户登录
    /// </summary>
    public partial class LoginForm : DevComponents.DotNetBar.Office2007Form
    {
        private SysCommon.Authorize.User m_User = null;   //登录的用户信息
        public SysCommon.Authorize.User LoginUser
        {
            get
            {
                return m_User;
            }
            set
            {
                m_User = value;
            }
        }
      
        public LoginForm()
        {
            InitializeComponent();

            //初始化界面
        }
        // 登录系统
        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception outError = null;
            //界面控制
            if (txtUser.Text.Trim() == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "用户名不能为空");
                return;
            }
            else if (txtPassword.Text.Trim() == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "密码不能为空");
                return;
            }

            if (ModuleData.v_AppConnStr.Trim() == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接字符串为空！");
                return;
            }
            string[] arr = ModuleData.v_AppConnStr.Split(';');
            if (arr.Length != 3)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接字符串不正确！");
                return;
            }
            ///////////////////////获取登陆的用户信息///////////////////
            #region 连接数据库
            SysCommon.DataBase.SysTable pSysDB = null;
            //连接数据库
            ModDBOperate.ConnectDB(out pSysDB, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", outError.Message);
                return;
            }
            #endregion

            #region 查询用户基本信息表,用户和密码判断
            string selStr = "select * from userbaseinfo where UserName='" + this.txtUser.Text.Trim() + "' and PASSWORD='" + this.txtPassword.Text.Trim() + "'";
            DataTable UserTable = pSysDB.GetSQLTable(selStr, out outError);
            if (outError != null || UserTable == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户基本信息表失败！");
                pSysDB.CloseDbConnection();
                return;
            }
            if (UserTable.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "无效的用户或者密码！");
                pSysDB.CloseDbConnection();
                return;
            }
            #endregion
            int pUserID = -1;   //用户ID
            string pSex = "";   //性别
            string pJob = "";   //职称
            string pRemark = "";  //备注
            string pRoleName = "";   //角色名称
            int pRoleTypeID = -1; //角色类型：1管理员、2作业员

            pUserID = Convert.ToInt32(UserTable.Rows[0][0].ToString().Trim());
            pSex = UserTable.Rows[0][3].ToString().Trim();
            pJob = UserTable.Rows[0][4].ToString().Trim();
            pRemark = UserTable.Rows[0][5].ToString().Trim();

            #region 查询用户角色
            string str = "select * from userrolerelationinfo inner join rolebaseinfo on UserID=" + pUserID + " and userrolerelationinfo.RoleID=rolebaseinfo.RoleID";
            //string str = "select * from userrolerelationinfo a inner join (select * from rolebaseinfo inner join roletypeinfo on rolebaseinfo.roletypeid=roletypeinfo.roletypeid) b on a.UserID=" + pUserID + "and a.RoleID=b.RoleID";

            DataTable pRoleRelTable = pSysDB.GetSQLTable(str, out outError);
            if (outError != null || pRoleRelTable == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户角色关系表失败！");
                pSysDB.CloseDbConnection();
                return;
            }
            if (pRoleRelTable.Rows.Count == 0)
            {
                pRoleName = "";
                pRoleTypeID = -1;
            }
            else
            {
                pRoleName = pRoleRelTable.Rows[0]["ROLENAME"].ToString().Trim();
                pRoleTypeID = Convert.ToInt32(pRoleRelTable.Rows[0]["ROLETYPEID"].ToString().Trim());  //角色类型ID
            }

            #endregion

            pSysDB.CloseDbConnection();

            //若用户和密码正确,则登录系统，保存用户相关信息

            m_User = new SysCommon.Authorize.User();
            m_User.ID = pUserID;
            m_User.Name = this.txtUser.Text.Trim(); 
            m_User.Password = this.txtPassword.Text.Trim();
            m_User.Sex = pSex;
            m_User.Position = pJob;
            m_User.Remark = pRemark;
            m_User.Role = pRoleName;
            m_User.RoleTypeID = pRoleTypeID;
            /////////////////////////////////////////////////////////////        
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// guozheng 2011-3-24 added 判断是不是第一次登陆（第一次登陆只有系统管理员，将系统管理员账号显示在界面上）
        /// </summary>
        private void JudgeInitialize()
        {
            Exception outError=null;
            SysCommon.DataBase.SysTable pSysDB = null;
            //连接数据库
            ModDBOperate.ConnectDB(out pSysDB, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接Oracle数据库异常："+outError.Message+"请检查Oracle数据库运行状态");
                return;
            }
            /////读取用户表，若只有一条记录则认为是系统管理员/////
            string sSQL = "SELECT * FROM userbaseinfo";
            DataTable GetUserTable = pSysDB.GetTable("userbaseinfo", out outError);
            if (GetUserTable.Rows.Count == 1)
            {
                ////////将系统管理员的信息显示在界面上////////
                this.txtUser.Text = GetUserTable.Rows[0]["USERNAME"].ToString();
                this.txtPassword.Text = GetUserTable.Rows[0]["PASSWORD"].ToString();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //////判断是不是第一次启动////
            JudgeInitialize();
            this.txtUser.Focus();
        }
    }
}
