using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;

using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace SysCommon.DataBase
{
    public interface IDataBase
    {
        //设置连接
        void SetDbConnection(string strConnectionString, enumDBConType DBConType, enumDBType DBType, out Exception eError);
        //关闭连接
        void CloseDbConnection();

        //在数据库中创建表
        bool CreateTable(string strTableName, Dictionary<string, string> dic, out Exception eError);

        //执行SQL语句新建、修改、删除数据
        bool UpdateTable(string strSQL, out Exception eError);

        //获取数据库中所有表名
        ArrayList GetTablesName();
        //获取数据库中所有基本表的信息
        DataTable GetTableSchema();
        //获取数据库中所有视图的信息
        DataTable GetViewSchema();
    }

    public interface ITable
    {
        //获取数据库中所有的表
        DataSet GetAllTables();

        //获取表
        DataTable GetTable(string tablename, out Exception eError);
        DataTable GetTable(string tablename, string condition, out Exception eError);
    }
    
    public interface ICommon
    {
    }
}
