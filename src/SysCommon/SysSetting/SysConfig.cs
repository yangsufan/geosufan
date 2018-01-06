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
        private string m_SystemCode = "SysName";
        public string SystemCode
        {
            get { return m_SystemCode; }
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
                if (pRow[ColumnName.ConfigCode].ToString().Trim() == m_SystemCode)
                {
                    m_SystemName = pRow[ColumnName.ConfigValue].ToString();
                }
            }
        }
        #endregion
    }
}
