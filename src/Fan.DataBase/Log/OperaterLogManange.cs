using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Fan.DataBase.Log
{
    public class OperaterLogManange : IOperaterLog
    {
        public bool InitialLog()
        {
            throw new NotImplementedException();
        }
        public bool WriteErrorLog(Exception ex, params string[] StrMessage)
        {
            throw new NotImplementedException();
        }
        public bool WriteInfo(params string[] StrInfos)
        {
            throw new NotImplementedException();
        }
        public DataTable GetLogTable()
        {
            return default(DataTable);
        }
    }
}
