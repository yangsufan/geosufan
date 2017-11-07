using System;
using System.Collections.Generic;
using System.Text;

namespace SysCommon.Authorize
{
    public class User
    {
        /// <summary>
        /// 编号
        /// </summary>
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 用户名称（简称）
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

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
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 性别
        /// </summary>
        private int _sex;
        public int Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }

        /// <summary>
        /// 职称
        /// </summary>
        private string _position;
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
