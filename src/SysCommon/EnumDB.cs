using System;
using System.Collections.Generic;
using System.Text;

namespace SysCommon
{
    //WorkSpace类型
    public enum enumWSType
    {
        SDE = 1,                   //ArcSDE
        PDB = 2,                   //Personal Geodatabase
        GDB = 3,                   //File Geodatabase
        SHP=4                      //shape

    }

    //数据库联接方式
    public enum enumDBConType
    {
        ODBC = 1,
        OLEDB = 2,
        SQL = 3,
        ORACLE = 4
    }
    //数据库类型
    public enum enumDBType
    {
        ACCESS = 1,
        ORACLE = 2,
        SQLSERVER = 3
    }
}
