using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Fan.DataBase
{
   public class SQLServerOperate : ISQLServerOperate
    {
        public SQLServerOperate(DBConfig dbconfig)
        {
            m_Config = dbconfig;
            InitializeDbOperate();
        }
        private DBConfig m_Config = null;
        private string ConnectString = string.Empty;
        private SqlConnection m_con = null;
        private void InitializeDbOperate()
        {
            if (m_Config != null)
            {
                ConnectString = string.Format("Server={0};DataBase={1};uid={2};pwd={3}",
                    m_Config.Server,m_Config.Database,m_Config.User,m_Config.Password);
                try
                {
                    m_con = new SqlConnection(ConnectString);
                }
                catch (Exception ex)
                {
                    Log.LogManager.WriteSysLog(ex, string.Format("FuncionName:InitializeDbOperate"));
                }
            }
        }
        public bool AddRow(string tableName, IList<string> column, params string[] datarow)
        {
            if (column.Count <= 0 || datarow.Length <= 0) return false;
            SqlCommand command = m_con.CreateCommand();
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
                    tempRow = string.Format("{0},", key);
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
                Log.LogManager.WriteSysLog(ex, string.Format("FunctionName:SQLServerOperate.AddRow"), command.CommandText);
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
            SqlCommand command = m_con.CreateCommand();
            try
            {
                command.CommandText = string.Format("delete [{0}] where {1}", tableName, strWhereCase);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FunctionName:SQLServerOperate.DeleteRow"), command.CommandText);
                return false;
            }
            return true;
        }
        public DataTable GetTable(string tableName, string whereStr)
        {
            DataTable pReturenTable = new DataTable(tableName);
            if (m_con == null)
            {
                return pReturenTable;
            }
            SqlCommand command = m_con.CreateCommand();
            try
            {
                if (string.IsNullOrEmpty(whereStr))
                {
                    command.CommandText = string.Format("select * from [{0}]", tableName);
                }
                else
                {
                    command.CommandText = string.Format("select * from [{0}] where {1}", tableName, whereStr);
                }
                pReturenTable.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FuncionName:SQLServerOperate.GetTable"), command.CommandText);
                return pReturenTable;
            }
            return pReturenTable;
        }
        public bool ImportTable(DataTable newDt)
        {
            throw new NotImplementedException();
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
                    Log.LogManager.WriteSysLog(ex, string.Format("FuncionName:SQLServerOperate.TestConnect"));
                    return false;
                }
            }
            return true;
        }
        public bool UpdateTable(string tableName, string strWhereCase, params string[] UpdateRows)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                Log.LogManager.WriteSysInfo(string.Format("表名不能为空"));
                return false;
            }
            if (string.IsNullOrEmpty(strWhereCase) && UpdateRows.Length <= 0)
            {
                return false;
            }
            SqlCommand command = m_con.CreateCommand();
            try
            {
                string updateStr = string.Empty;
                foreach (string key in UpdateRows)
                {
                    updateStr = updateStr + string.Format("{0},", key);
                }
                if (updateStr.EndsWith(","))
                    updateStr = updateStr.Substring(0, updateStr.LastIndexOf(','));
                command.CommandText = string.Format("update [{0}] set {1} where {2}", tableName, updateStr, strWhereCase);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.LogManager.WriteSysLog(ex, string.Format("FunctionName:SQLServerOperate.UpdateTable"), command.CommandText);
                return false;
            }
            return true;
        }
        public DataTable GetTableByStoredProcedure(string spName, params string[] spParams)
        {
            DataTable returnDt = default(DataTable);
            return returnDt;
        }
        public bool ExecStoreProcedure(string spName, params string[] spParams)
        {
            return false;
        }
    }
}
