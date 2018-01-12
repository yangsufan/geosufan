using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fan.DataBase;
using System.Data;
using Fan.DataBase.Module;


namespace Fan.Common
{
    public class SysConfig
    {
        public SysConfig(IDBOperate dBOperate)
        {
            m_Dboperate = dBOperate;
            InitializeConfig();
        }
        #region Class Attribute
        private IDBOperate m_Dboperate=null;
        private string m_SystemName = string.Empty;
        public string SystemName
        {
            get { return m_SystemName; }
        }
        private DBConfig m_TempDbConfig = null;
        /// <summary>
        /// 临时数据库连接配置
        /// </summary>
        public DBConfig TempDbConfig
        {
            get { return m_TempDbConfig; }
        }
        private DBConfig m_HisDbConfig = null;
        /// <summary>
        /// 历史数据库连接配置
        /// </summary>
        public DBConfig HisDbConfig
        {
            get { return m_HisDbConfig; }
        }
        private DBConfig m_OfficialDbConfig = null;
        /// <summary>
        /// 正式数据库连接配置
        /// </summary>
        public DBConfig OfficialDbConfig
        {
            get { return m_OfficialDbConfig; }
        }
        #endregion
        #region Class Function
        private void InitializeConfig()
        {
            if (m_Dboperate == null)
            {
                LogManage.WriteLog("初始化系统设置失败：无数据库连接信息！");
                return;
            }
            DataTable pConfigDt = m_Dboperate.GetTable(TableName.TSysConfig,string.Empty);
            if (pConfigDt == null || pConfigDt.Rows.Count == 0)
            {
                LogManage.WriteLog("初始化系统设置失败：无法获取系统设置数据");
            }
            foreach (DataRow pRow in pConfigDt.Rows)
            {
                if (pRow[ColumnName.ConfigCode].ToString().Trim() == ColumnName.SystemName)
                {
                    m_SystemName = pRow[ColumnName.ConfigValue].ToString();
                }
                else if (pRow[ColumnName.ConfigCode].ToString().Trim() == ColumnName.TempDbConfigCode)
                {
                    m_TempDbConfig = new DBConfig();
                    m_TempDbConfig.ReadConfigFromStr(pRow[ColumnName.ConfigValue].ToString());
                }
                else if (pRow[ColumnName.ConfigCode].ToString().Trim() == ColumnName.HisDbConfigCode)
                {
                    m_HisDbConfig = new DBConfig();
                    m_HisDbConfig.ReadConfigFromStr(pRow[ColumnName.ConfigValue].ToString());
                }
                else if (pRow[ColumnName.ConfigCode].ToString().Trim() == ColumnName.OfficDbConfigCode)
                {
                    m_OfficialDbConfig = new DBConfig();
                    m_OfficialDbConfig.ReadConfigFromStr(pRow[ColumnName.ConfigValue].ToString());
                }
            }
        }
        public string UpdateSysConfig(string ConfigCode, string ConfigValue,string ConfigName="")
        {
            if (m_Dboperate == null)
            {
                return string.Format("无数据库操作信息");
            }
            if (string.IsNullOrEmpty(ConfigCode) || string.IsNullOrEmpty(ConfigValue))
            {
                return string.Format("未设置系统配置更新项");
            }
            DataTable pConfigDt = m_Dboperate.GetTable(TableName.TSysConfig, string.Empty);
            if (pConfigDt == null || pConfigDt.Rows.Count == 0)
            {
                return string.Format("无法获取系统配置信息");
            }
            DataRow[] pSelectRows = pConfigDt.Select(string.Format("{0}='{1}'",ColumnName.ConfigCode, ConfigCode));
            if (pSelectRows.Length <= 0)
            {
                if (!m_Dboperate.AddRow(TableName.TSysConfig, new List<string> { ColumnName.ConfigCode, ColumnName.ConfigName, ColumnName.ConfigValue },
                    ConfigCode, ConfigName, ConfigValue))
                {
                    return string.Format("更新表失败，请查看日志信息");
                }
            }
            else
            {
                bool flag = false;
                if (string.IsNullOrEmpty(ConfigName))
                {
                    flag=m_Dboperate.UpdateTable(TableName.TSysConfig, string.Format("{0}='{1}'", ColumnName.ConfigCode, ConfigCode),
                        ColumnName.ConfigValue, ConfigValue);
                }
                else
                {
                    flag = m_Dboperate.UpdateTable(TableName.TSysConfig, string.Format("{0}='{1}'", ColumnName.ConfigCode, ConfigCode),
                        ColumnName.ConfigValue, ConfigValue,
                        ColumnName.ConfigName,ConfigName);
                }
                if(!flag) return string.Format("更新表失败，请查看日志信息");
            }
            return string.Empty;
        }
        #endregion
    }
}
