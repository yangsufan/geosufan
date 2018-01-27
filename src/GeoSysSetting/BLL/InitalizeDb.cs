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
    class InitalizeDb
    {
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="dicTempLate">数据库模板</param>
        /// <param name="mainConfig">系统配置</param>
        protected InitalizeDb(Dictionary<string,DBConfig> dicTempLate, SysConfig mainConfig)
        {
            
        }
    }
}
