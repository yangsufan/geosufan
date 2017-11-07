using System;
using System.Collections.Generic;
using System.Text;

namespace SysCommon.Authorize
{
    public class User
    {
        /// <summary>
        /// 有效期  wgf 20111102
        /// </summary>
        private string U_UserDate;
        public string UserDate
        {
            get { return U_UserDate; }
            set { U_UserDate = value; }
        }
        /// <summary>
        /// 用户科室信息 ygc 20130319
        /// </summary>
        private string U_UserDepartment;
        public string UserDepartment
        {
            get { return U_UserDepartment; }
            set { U_UserDepartment = value; }
        }

        /// <summary>
        /// 编号
        /// </summary>
        private int U_ID;
        public int ID
        {
            get { return U_ID; }
            set { U_ID = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string U_NAME;
        public string Name
        {
            get { return U_NAME; }
            set { U_NAME = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string U_PWD;
        public string Password
        {
            get { return U_PWD; }
            set { U_PWD = value; }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        private string U_ROLE;
        public string Role
        {
            get { return U_ROLE; }
            set { U_ROLE = value; }
        }

        /// <summary>
        /// 用户角色类型
        /// </summary>
        private int U_RoleTypeID;
        public int RoleTypeID
        {
            get
            {
                return U_RoleTypeID;
            }
            set
            {
                U_RoleTypeID = value;
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        private string U_SEX;
        public string Sex
        {
            get { return U_SEX; }
            set { U_SEX = value; }
        }

        /// <summary>
        /// 职称
        /// </summary>
        private string U_JOB;
        public string Position
        {
            get { return U_JOB; }
            set { U_JOB = value; }
        }
        /// <summary>
        /// 职称
        /// </summary>
        private double U_ExportArea;
        public double  ExportArea
        {
            get { return U_ExportArea; }
            set { U_ExportArea = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        private string U_REMARK;
        public string Remark
        {
            get { return U_REMARK; }
            set { U_REMARK = value; }
        }

        private string _loginInfo;
        public string LoginInfo
        {
            get
            {
                return _loginInfo;
            }
            set
            {
                _loginInfo = value;
            }
        }


        //20110518  add
        /// <summary>
        /// 编号
        /// </summary>
        private string _idStr;
        public string IDStr
        {
            get { return _idStr; }
            set { _idStr = value; }
        }

        /// <summary>
        /// 用户名称（简称）
        /// </summary>
        //private string _name;
        //public string NameStr
        //{
        //    get { return _name; }
        //    set { _name = value; }
        //}

        /// <summary>
        /// 用户名称（真实名）
        /// </summary>
        private string _trueName;
        public string TrueName
        {
            get { return _trueName; }
            set { _trueName = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        //private string _password;
        //public string Password
        //{
        //    get { return _password; }
        //    set { _password = value; }
        //}

        /// <summary>
        /// 性别
        /// </summary>
        private int _sex;
        public int SexInt
        {
            get { return _sex; }
            set { _sex = value; }
        }

        /// <summary>
        /// 职称
        /// </summary>
        //private string _position;
        //public string Position
        //{
        //    get { return _position; }
        //    set { _position = value; }
        //}

        /// <summary>
        /// 备注
        /// </summary>
        //private string _remark;
        //public string Remark
        //{
        //    get { return _remark; }
        //    set { _remark = value; }
        //}
        //
    }
}
