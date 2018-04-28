using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 表名和视图名称常数类
 */
namespace Fan.DataBase.Module
{
   public static class TableName
    {
        #region TableName
        public const string TUser = "User";
        public const string TRole = "Role";
        public const string TSysFunction = "SysFunction";
        public const string TRoleFunction = "RoleFunction";
        public const string TUserRole = "UserRole";
        public const string TSysConfig = "SysConfig";
        #endregion

        #region  ViewName 
        public const string VUserInfo = "v_UserInfo";
        public const string VRoleFunction = "v_RoleFunction";
        #endregion

    }
}
