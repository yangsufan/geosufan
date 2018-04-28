using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fan.Common;
using Fan.DataBase;
/*
 * 初始化数据库业务类,主要用于将模板数据库结构复制到正式库、临时库以及历史库中
 * ygc
 * 2018-01-19
 */
namespace Fan.SysSetting
{
    public  class InitalizeDb
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="dicTempLate">数据库模板信息</param>
        /// <param name="mainConfig">系统配置</param>
        public InitalizeDb(Dictionary<string,DBConfig> dicTempLate, SysConfig mainConfig)
        {
            if (dicTempLate.Count == 0 || mainConfig == null)
            {
                LogManage.WriteLog(string.Format("初始化数据库失败，数据库配置信息有误"));
                return;
            }
            m_dicTemplate = dicTempLate;
            m_sysConfig = mainConfig;
        }
        private Dictionary<string, DBConfig> m_dicTemplate = default(Dictionary<string, DBConfig>);
        private SysConfig m_sysConfig = default(SysConfig);
        /// <summary>
        /// 开始进行初始化操作
        /// </summary>
        /// <returns></returns>
        public bool Do_Initalize()
        {
            IDBOperate pHisDBOp =new DBOperatorFactory(m_sysConfig.HisDbConfig).GetDbOperate();
            IDBOperate pOfficalDBOp = new DBOperatorFactory(m_sysConfig.OfficialDbConfig).GetDbOperate();
            IDBOperate pTempDBOp = new DBOperatorFactory(m_sysConfig.TempDbConfig).GetDbOperate();
            foreach (string key in m_dicTemplate.Keys)
            {
                IDBOperate pTemplateDBOp = new DBOperatorFactory(m_dicTemplate[key]).GetDbOperate();
            }
            return false;
        }
    }
}
