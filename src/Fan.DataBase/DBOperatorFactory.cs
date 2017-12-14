using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fan.DataBase
{
   public class DBOperatorFactory
    {
        public DBOperatorFactory(DBConfig configset)
        {
            m_config = configset;
        }
        private DBConfig m_config = null;
        public DBConfig Config
        {
            get { return m_config; }
        }
        public IDBOperate GetDbOperate()
        {
            IDBOperate iDBOperate=null;
            switch (m_config.m_OperatorType)
            {
                case DBOperatorType.EsriOperator:
                    switch (m_config.m_ConnectType)
                    {
                        case DBType.ESRIGDB:
                            break;
                        case DBType.ESRIPDB:
                            break;
                        case DBType.ESRISDEOracle:
                            break;
                        case DBType.ESRISDESqlServer:
                            break;
                    }
                    break;
                case DBOperatorType.ODBC:
                    switch (m_config.m_ConnectType)
                    {
                        case DBType.ODBCMDB:
                            iDBOperate = new MDBOperate(m_config);
                            break;
                        case DBType.ODBCORACLE:
                            break;
                        case DBType.ODBCPOST:
                            break;
                        case DBType.ODBCSQL:
                            break;
                    }
                    break;
            }
            return iDBOperate;
        }
    }
}
