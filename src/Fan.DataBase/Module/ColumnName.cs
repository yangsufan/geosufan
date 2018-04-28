using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 列名常数类
 */
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
        #region 系统配置项名称
        public const string SystemName = "SysName";
        public const string TempDbConfigCode = "TempDbConfig";
        public const string HisDbConfigCode = "HisDbConfig";
        public const string OfficDbConfigCode = "OfficialDbConfig";
        #endregion

        #region 历史库和临时库相关前、后缀
        public const string His_prex = "his_";
        public const string Temp_prex = "temp_";
        #endregion
        #region 历史库和临时库日期列
        /// <summary>
        /// 更新时间
        /// </summary>
        public const string UpdateTime = "updatetime";
        /// <summary>
        /// 创建时间
        /// </summary>
        public const string CreateTime = "createtime";
        /// <summary>
        /// 有效日期
        /// </summary>
        public const string VaDate = "vadate";
        /// <summary>
        /// 失效日期
        /// </summary>
        public const string InVaDate = "invadate";
        #endregion
    }
}
