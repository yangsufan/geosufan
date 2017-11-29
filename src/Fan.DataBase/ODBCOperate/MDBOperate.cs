using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace Fan.DataBase
{
    class MDBOperate : IMDBOperate
    {
        private string ConnectString = string.Empty;
        private OleDbConnection m_con = null;
        public DataTable GetTable(string tableName, string whereStr)
        {
            DataTable pReturenTable = new DataTable(tableName);
            if (m_con == null)
            {
                return pReturenTable;
            }
            try
            {
                OleDbCommand command = m_con.CreateCommand();
                if (string.IsNullOrEmpty(whereStr))
                {
                    command.CommandText = string.Format("select * from {0}", tableName);
                }
                else
                {
                    command.CommandText = string.Format("select * from {0} where {1}",tableName,whereStr);
                }
                pReturenTable.Load(command.ExecuteReader());
            }
            catch (Exception ex)
            {
                SysCommon.LogManage.WriteErrorLog(ex, string.Format("FuncionName:GetTable"));
                return pReturenTable;
            }
           return pReturenTable;
        }
        public bool ImportTable(DataTable newDt)
        {
            return false;
        }

        public bool SetDbConnection(string ConntectStr)
        {
            if (string.IsNullOrEmpty(ConntectStr))
            {
                return false;
            }
            else
            {
                try
                {
                    m_con = new OleDbConnection(ConntectStr);
                }
                catch (Exception ex)
                {
                    SysCommon.LogManage.WriteErrorLog(ex,string.Format("FuncionName:SetDbConnection"));
                    return false;
                }
            }
            return true;
        }
        public bool UpdateTable(DataRow[] updateRows)
        {
            throw new NotImplementedException();
        }
    }
}
