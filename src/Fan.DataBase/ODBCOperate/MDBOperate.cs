using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace Fan.DataBase
{
    public class MDBOperate : IMDBOperate
    {
        public MDBOperate(DBConfig dbconfig)
        {
            m_Config = dbconfig;
            InitializeDbOperate();
        }
        private DBConfig m_Config = null;
        private string ConnectString = string.Empty;
        private OleDbConnection m_con = null;
        public DataTable GetTable(string tableName, string whereStr)
        {
            DataTable pReturenTable = new DataTable(tableName);
            if (m_con == null)
            {
                return pReturenTable;
            }
            OleDbCommand command = m_con.CreateCommand();
            try
            {
                if (string.IsNullOrEmpty(whereStr))
                {
                    command.CommandText = string.Format("select * from [{0}]", tableName);
                }
                else
                {
                    command.CommandText = string.Format("select * from [{0}] where {1}",tableName,whereStr);
                }
                pReturenTable.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FuncionName:GetTable"), command.CommandText);
                return pReturenTable;
            }
           return pReturenTable;
        }
        public bool ImportTable(DataTable newDt)
        {
            return false;
        }
        private void InitializeDbOperate()
        {
            if (m_Config != null)
            {
                if (string.IsNullOrEmpty(m_Config.Password))
                {
                    ConnectString = string.Format("Provider = Microsoft.Jet.OLEDB.4.0; Data Source ={0}", m_Config.Database);
                }
                else
                {
                    ConnectString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1}",
                        m_Config.Database,m_Config.Password);
                }
                try
                {
                    m_con = new OleDbConnection(ConnectString);
                }
                catch (Exception ex)
                {
                    Log.LogManager.WriteSysLog(ex, string.Format("FuncionName:SetDbConnection"));
                }
            }
        }
        public bool UpdateTable(string tableName, string strWhereCase,params string[] UpdateRows)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                Log.LogManager.WriteSysInfo(string.Format("表名不能为空"));
                return false;
            }
            if (string.IsNullOrEmpty(strWhereCase)&& UpdateRows.Length<=0)
            {
                return false;
            }
            OleDbCommand command = m_con.CreateCommand();
            try
            {
                string updateStr = string.Empty;
                foreach (string key in UpdateRows)
                {
                    updateStr = updateStr+string.Format("{0},",key);
                }
                if (updateStr.EndsWith(","))
                    updateStr = updateStr.Substring(0, updateStr.LastIndexOf(','));
                command.CommandText = string.Format("update [{0}] set {1} where {2}",tableName,updateStr,strWhereCase);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FunctionName:MDBOperate.UpdateTable"), command.CommandText);
                return false;
            }
            return true;
        }
        public bool TestConnect()
        {
            if (m_con != null)
            {
                try
                {
                    m_con.Open();
                }
                catch (Exception ex)
                {
                    Log.LogManager.WriteSysLog(ex, string.Format("FuncionName:TestConnect"));
                    return false;
                }
            }
            return true;
        }
        public bool AddRow(string tableName,IList<string> column, params string[] datarow)
        {
            if (column.Count <= 0 || datarow.Length <= 0) return false;
            OleDbCommand command = m_con.CreateCommand();
            try
            {
                string tempStr = string.Empty;
                foreach (string col in column)
                {
                    tempStr = string.Format("{0},", col);
                }
                if (tempStr.EndsWith(",")) tempStr = tempStr.Substring(0, tempStr.LastIndexOf(','));
                string tempRow = string.Empty;
                foreach (string key in datarow)
                {
                    tempRow= string.Format("{0},", key);
                }
                if (tempRow.EndsWith(",")) tempRow = tempRow.Substring(0, tempRow.LastIndexOf(','));
                command.CommandText = string.Format("insert into [{0}] ({1}) values (2)", tableName, tempStr, tempRow);
                if (command.ExecuteNonQuery() <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FunctionName:MDBOperate.AddRow"), command.CommandText);
                return false;
            }
            return true;
        }
        public bool DeleteRow(string tableName, string strWhereCase)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(strWhereCase))
            {
                return false;
            }
            OleDbCommand command = m_con.CreateCommand();
            try
            {
                command.CommandText = string.Format("delete [{0}] where {1}", tableName, strWhereCase);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FunctionName:MDBOperate.DeleteRow"), command.CommandText);
                return false;
            }
            return true;
        }
    }
}
