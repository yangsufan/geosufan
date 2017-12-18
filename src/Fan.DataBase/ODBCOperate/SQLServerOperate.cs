using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fan.DataBase
{
   public class SQLServerOperate : ISQLServerOperate
    {
        public SQLServerOperate(DBConfig dbconfig)
        {

        }
        public bool AddRow(string tableName, IList<string> column, params string[] datarow)
        {
            throw new NotImplementedException();
        }
        public bool DeleteRow(string tableName, string strWhereCase)
        {
            throw new NotImplementedException();
        }
        public DataTable GetTable(string tableName, string whereStr)
        {
            throw new NotImplementedException();
        }
        public bool ImportTable(DataTable newDt)
        {
            throw new NotImplementedException();
        }
        public bool TestConnect()
        {
            throw new NotImplementedException();
        }
        public bool UpdateTable(string tableName, string strWhereCase, params string[] UpdateRows)
        {
            throw new NotImplementedException();
        }
    }
}
