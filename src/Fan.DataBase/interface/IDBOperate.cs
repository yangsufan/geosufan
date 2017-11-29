using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Fan.DataBase
{
    /// <summary>
    /// 数据库操作接口
    /// </summary>
    interface IDBOperate
    {

    }
    /// <summary>
    /// OLEDB数据库操作接口
    /// </summary>
    interface IODBCOperate : IDBOperate
    {
        /// <summary>
        /// 设置连接
        /// </summary>
        /// <param name="ConntectStr">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        bool SetDbConnection(string ConntectStr);
        DataTable GetTable(string tableName,string whereStr);
        bool ImportTable(DataTable newDt);
        bool UpdateTable(DataRow[] updateRows);
    }
    /// <summary>
    /// MDB操作接口
    /// </summary>
    interface IMDBOperate : IODBCOperate
    {

    }
    /// <summary>
    /// SQLsever 数据库操作接口
    /// </summary>
    interface ISQLServerOperate : IODBCOperate
    {

    }
    /// <summary>
    /// Oracle数据库操作接口
    /// </summary>
    interface IORACLEOperate : IODBCOperate
    {

    }
    /// <summary>
    /// Postgres数据库操作接口
    /// </summary>
    interface IPOSTOperate : IODBCOperate
    {

    }
    /// <summary>
    /// ESRI数据操作接口
    /// </summary>
    interface IESRIOperate : IDBOperate
    {

    }
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DBType
    {
        ODBCMDB=0,
        ODBCSQL=1,
        ODBCORACL=2,
        ESRISDE=3,
        ESRIPDB=4,
        ESRIGDB=5,
        ESRISHP=6
    }
}
