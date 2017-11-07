//*********************************************************************************
//** 文件名：ModTableFun.cs
//** CopyRight (c) 2000-2007 武汉吉奥信息工程技术有限公司工程部
//** 创建人：chulili
//** 日  期：2011-03
//** 修改人：yjl
//** 日  期：
//** 描  述：与表相关的一些公共函数 
//**
//** 版  本：1.01
//*********************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
namespace GeoDBATool
{
    class ModTableFun
    {
        //函数功能：获取游标
        //输入参数：数据库连接  sql语句 //输出参数：根据sql语句打开的表的游标
        public static OleDbDataReader GetReader(OleDbConnection conn, string sqlstr)
        {
            OleDbCommand comm = conn.CreateCommand();
            comm.CommandText = sqlstr;
            OleDbDataReader myreader;
            try
            {
                myreader = comm.ExecuteReader();
                return myreader;
            }
            catch (System.Exception e)
            {
                e.Data.Clear();
                return null;
            }
        }
        //函数功能：删除表   输入参数：数据库连接  表名  输出参数：无
        public static void DropTable(OleDbConnection conn,string TableName)
        {
            if (isExist(conn, TableName))
            {
                try
                {
                    OleDbCommand mycomm = conn.CreateCommand();
                    mycomm.CommandText = "drop table " + TableName;
                    mycomm.ExecuteNonQuery();
                }
                catch
                { }
            }
        }
        //函数功能:执行DDL
        //yjl 20110824 add
        public static void ExecuteDDL(OleDbConnection conn, string ddl)
        {
          
                try
                {
                    OleDbCommand mycomm = conn.CreateCommand();
                    mycomm.CommandText = ddl;
                    mycomm.ExecuteNonQuery();
                }
                catch
                { }
            
        }
        //函数功能：判断表是否存在  
        //输入参数：数据库连接 表名  输出参数：布尔型
        public static bool isExist(OleDbConnection conn,string TableName)
        {
            OleDbCommand comm = conn.CreateCommand();
            comm.CommandText     = "select count(*) from " + TableName + " where 1=0";//yjl 20110824 modify.提高效率并且减少误判，因为如果列类型是OLE不支持的话也会抛出异常
            OleDbDataReader myreader;
            //根据错误保护判断表是否存在
            try
            {
                myreader = comm.ExecuteReader();
                myreader.Close();
                return true;
            }
            //报错则表示不存在
            catch(System.Exception e)
            {
                e.Data.Clear();
                return false;
            }

        }
        //判断表中是否存在某个字段】
        //输入参数：数据库连接  表名  字段名  输出参数：布尔型

        public static bool isFieldExist(OleDbConnection conn, string TableName, string FieldName)
        {
            OleDbCommand comm = conn.CreateCommand();
            comm.CommandText = "select " +FieldName+" from " + TableName + " where 1=0";
            OleDbDataReader myreader;
            //根据错误保护判断某个字段是否存在
            try
            {
                myreader = comm.ExecuteReader();
                myreader.Close();
                return true;
            }
            //报错则表示这个字段不存在
            catch (System.Exception e)
            {
                e.Data.Clear();
                return false;
            }
        }
        //函数功能：删除表的列   输入参数：数据库连接  表名  列串 如 "c1,c2" 输出参数：无
        //yjl20110824 add
        public static void DropColumn(OleDbConnection conn, string TableName,string colName)
        {

                try
                {
                    OleDbCommand mycomm = conn.CreateCommand();
                    mycomm.CommandText = "alter table " + TableName+" drop ("+colName+") cascade constraints";
                    mycomm.ExecuteNonQuery();
                }
                catch
                { }
            
        }
    }
}
