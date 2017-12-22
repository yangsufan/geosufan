using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using Fan.DataBase;
using Fan.DataBase.Module;

namespace Fan.Common
{
    public class Role
    {
        public Role(string strRoleID, IDBOperate iDbOperate)
        {
            m_RoleID = strRoleID;
            m_DbOperate = iDbOperate;
        }
        #region Class Attribute 
        private string m_RoleID = string.Empty;
        public string RoleID
        {
            get { return m_RoleID; }
        }
        private string m_RoleName = string.Empty;
        public string RoleName
        {
            get { return m_RoleName; }
        }
        public DataTable RoleFunDt
        {
            get {
                DataTable Dt = default(DataTable);
                if (!string.IsNullOrEmpty(m_RoleID))
                {
                    Dt = m_DbOperate.GetTable(TableName.VRoleFunction, string.Format("{0}='{1}'", ColumnName.RoleID, m_RoleID));
                }
                return Dt;
            }
        }
        private IDBOperate m_DbOperate = default(IDBOperate);
        #endregion

        #region Class Function
        /// <summary>
        /// 更新角色功能权限，先删后新增
        /// </summary>
        /// <param name="listFunc"></param>
        /// <returns></returns>
        public string UpdateRoleFunction(List<string> listFunc)
        {
            if (listFunc.Count <= 0) return string.Format("更新列表不能为空");
            if (!m_DbOperate.DeleteRow(TableName.TRoleFunction, string.Format("{0}='{1}'", ColumnName.RoleID, m_RoleID)))
            {
                return string.Format("修改权限失败：删除权限失败");
            }
            foreach (string func in listFunc)
            {
                if (m_DbOperate.AddRow(TableName.TRoleFunction, new List<string> { ColumnName.RoleID,ColumnName.FID },m_RoleID,func)) continue;
                else return string.Format("修改权限失败");
            }
            return string.Empty;
        }
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dicRoleInfo">角色信息</param>
        /// <param name="listFunc">权限ID列表</param>
        /// <returns></returns>
        public string AddRole(Dictionary<string,string>dicRoleInfo,List<string> listFunc)
        {
            if (dicRoleInfo.Count <= 0 || listFunc.Count <= 0)
            {
                return string.Format("添加角色失败：新增角色信息不完全");
            }
            foreach (string key in dicRoleInfo.Keys)
            {
                switch (key)
                {
                    case ColumnName.RoleID:
                        m_RoleID = dicRoleInfo[key];
                        break;
                    case ColumnName.RoleName:
                        m_RoleName = dicRoleInfo[key];
                        break;
                }
            }
            DataTable pDt = m_DbOperate.GetTable(TableName.TRole, string.Format("{0}='{1}'", ColumnName.RoleID,m_RoleID));
            if (pDt != null && pDt.Rows.Count > 0)
            {
                return string.Format("添加角色失败：角色ID以存在");
            }
            if (!m_DbOperate.AddRow(TableName.TRole, new List<string> { ColumnName.RoleID, ColumnName.RoleName },
                m_RoleID, m_RoleName))
            {
                return string.Format("添加角色失败：写入数据失败");
            }
            return UpdateRoleFunction(listFunc);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <returns></returns>
        public string DeleteRole()
        {
            if (string.IsNullOrEmpty(m_RoleID)) return string.Format("删除角色失败：角色ID不能为空");
            if (!m_DbOperate.DeleteRow(TableName.TRoleFunction, string.Format("{0}='{1}'", ColumnName.RoleID, m_RoleID)))
            {
                return string.Format("删除角色失败：删除角色权限失败");
            }
            if (!m_DbOperate.DeleteRow(TableName.TRole, string.Format("{0}='{1}'", ColumnName.RoleID, m_RoleID)))
            {
                return string.Format("删除角色失败");
            }
            return string.Empty;
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="UpdateRole">角色信息</param>
        /// <returns></returns>
        public string UpdateRole(Dictionary<string,string> UpdateRole)
        {
            if (UpdateRole == null || UpdateRole.Count <= 0)
            {
                return string.Format("更新角色信息失败");
            }
            foreach (string key in UpdateRole.Keys)
            {
                switch (key)
                {
                    case ColumnName.RoleName:
                        m_RoleName = UpdateRole[key];
                        break;
                    case ColumnName.RoleID:
                        m_RoleID = UpdateRole[key];
                        break;
                }
            }
            if (!m_DbOperate.UpdateTable(TableName.TRole, string.Format("{0}='{1}'", ColumnName.RoleID, m_RoleID),
                string.Format("{0}='{1}'", ColumnName.RoleName, m_RoleName)))
            {
                return string.Format("更新角色信息失败：更新数据库失败");
            }
            return string.Empty;
        }
        #endregion
    }
}
