using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fan.DataBase.Module
{
   public static class ColumnName
    {
        #region Table User
        public const string UserCode = "UserCode";
        public const string UserName = "UserName";
        public const string UserPassword = "UserPass";
        #endregion
        #region Table Role
        public const string RoleID = "RoleID";
        public const string RoleName = "RoleName";
        #endregion
        #region Table SysFunction
        public const string PID = "PID";
        public const string FName = "FName";
        public const string Caption = "Caption";
        public const string Visible = "Visible";
        public const string Enabled = "Enabled";
        public const string BackgroudLoad = "BackgroudLoad";
        public const string ControlType = "ControlType";
        public const string NewGroup = "NewGroup";
        public const string FID = "FID";
        public const string Tips = "Tips";
        public const string LevelID = "LevelID";
        #endregion
        #region Table SysConfig
        public const string ConfigCode = "ConfigCode";
        public const string ConfigName = "ConfigName";
        public const string ConfigValue = "ConfigValue";
        #endregion
    }
}
