using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SysCommon.Authorize
{
    public class Role
    {
        /// <summary>
        /// 编号
        /// </summary>
        /// 

        
        private string _TypeID;
      
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        //20110518 add
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
        /// 组名称
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 角色对应的项目组ID
        /// </summary>
        private string _ProjectID;
        public string PROJECTID
        {
            get { return _ProjectID; }
            set { _ProjectID = value; }
        }

        /// <summary>
        /// 组类型ID
        /// </summary>
        public string TYPEID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

        /// <summary>
        /// 组操作类型
        /// </summary>
        private string _type;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        /// <summary>
        /// 权限
        /// </summary>
        private XmlDocument _privilege;
        public XmlDocument Privilege
        {
            get { return _privilege; }
            set { _privilege = value; }
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
        /// <summary>
        /// 角色类型ID（0:管理员；1：普通用户）
        /// </summary>
        private int _RoleTypeID = -1;
        public int RoleTypeID
        {
            get { return this._RoleTypeID; }
            set { this._RoleTypeID = value; }
        }

     
        /// <summary>
        /// 组名称
        /// </summary>
        //private string _name;
        //public string Name
        //{
        //    get { return _name; }
        //    set { _name = value; }
        //}

        /// <summary>
        /// 权限
        /// </summary>
        //private XmlDocument _privilege;
        //public XmlDocument Privilege
        //{
        //    get { return _privilege; }
        //    set { _privilege = value; }
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
        //end
    }
}
