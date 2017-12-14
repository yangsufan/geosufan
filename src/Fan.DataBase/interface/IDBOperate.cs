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
    public interface IDBOperate
    {
        /// <summary>
        /// 根据条件获取表数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="whereStr">查询条件</param>
        /// <returns></returns>
        DataTable GetTable(string tableName, string whereStr);
        /// <summary>
        /// 导入表数据
        /// </summary>
        /// <param name="newDt">导入表</param>
        /// <returns></returns>
        bool ImportTable(DataTable newDt);
        /// <summary>
        /// 更新表数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="strWhereCase">更新条件</param>
        /// <param name="UpdateRows">更新行</param>
        /// <returns></returns>
        bool UpdateTable(string tableName,string strWhereCase, params string[] UpdateRows);
        /// <summary>
        /// 添加数据行到指定表
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="column">数据列名称</param>
        /// <param name="datarow">数据行</param>
        /// <returns></returns>
        bool AddRow(string tableName, IList<string> column, params string[] datarow);
        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="strWhereCase">删除条件</param>
        /// <returns></returns>
        bool DeleteRow(string tableName, string strWhereCase);
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <returns></returns>
        bool TestConnect();
    }
    /// <summary>
    /// OLEDB数据库操作接口
    /// </summary>
    public interface IODBCOperate : IDBOperate
    {



    }
    /// <summary>
    /// MDB操作接口
    /// </summary>
    public interface IMDBOperate : IODBCOperate
    {

    }
    /// <summary>
    /// SQLsever 数据库操作接口
    /// </summary>
    public interface ISQLServerOperate : IODBCOperate
    {

    }
    /// <summary>
    /// Oracle数据库操作接口
    /// </summary>
    public interface IORACLEOperate : IODBCOperate
    {

    }
    /// <summary>
    /// Postgres数据库操作接口
    /// </summary>
    public interface IPOSTOperate : IODBCOperate
    {

    }
    /// <summary>
    /// ESRI数据操作接口
    /// </summary>
    public interface IESRIOperate : IDBOperate
    {

    }
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DBType
    {
        ODBCMDB=1,
        ODBCSQL=2,
        ODBCORACLE=3,
        ODBCPOST=8,
        ESRISDEOracle=4,
        ESRISDESqlServer=9,
        ESRIPDB=5,
        ESRIGDB=6,
        ESRISHP=7,
        DEFAULT=0
    }
}
