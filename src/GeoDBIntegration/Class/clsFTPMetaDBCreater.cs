using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GeoDBIntegration
{
    /*
     *guozheng 2010-10-9 added
     * 该类实现使用输入的连接字符串在Oracle服务器上建立文件库的元信息表格结构
     * 对成果文件库的元信息库的操作可以在该类中实现
     */
    class clsFTPMetaDB
    {
        private SysCommon.DataBase.SysTable m_DataBaseOper;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sConnectInfo">Oracle连接字符串</param>
        public clsFTPMetaDB(string sConnectInfo)
        {
            Exception ex=null;
            m_DataBaseOper = new SysCommon.DataBase.SysTable();
            m_DataBaseOper.SetDbConnection(sConnectInfo, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out ex);
            if (ex != null)
            {
                this.m_DataBaseOper = null;
            }
        }
        /// <summary>
        /// 创建元信息库体结构
        /// </summary>
        /// <param name="ex">返回错误信息</param>
        public void Creat(out Exception ex)
        {
            ex = null;
            if (this.m_DataBaseOper == null) { ex = new Exception("元信息Oracle数据库连接信息未初始化"); return; }
            string sql = string.Empty;
            if (!System.IO.File.Exists(ModuleData.v_FTPDbSchameFile))
            {
                ex = new Exception("FTP元信息库模板文件:"+ModuleData.v_FTPDbSchameFile+" 并不存在");
                return;
            }
            try
            {
                SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ModuleData.v_FTPDbSchameFile + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out ex);
                if (ex != null) return;
                List<string> TableNames = new List<string>();
                TableNames.Add("ControlPointMDTable");////////控制点元信息表
                TableNames.Add("DataFormatTable");////////////数据格式表
                TableNames.Add("DataTypeTable");//////////////数据类型表
                TableNames.Add("NonstandardMapMDTable");//////非标准图幅元信息表
                TableNames.Add("ProductIndexTable");//////////产品索引表
                TableNames.Add("ProductMDTable");/////////////产品元信息表
                TableNames.Add("ProjectMDTable");/////////////项目元信息表
                TableNames.Add("StandardMapMDTable");/////////标准图幅元信息表
                this.m_DataBaseOper.StartTransaction();
                foreach (string sTable in TableNames)
                {
                    DataTable GetTable = metaSysTable.GetTable(sTable, out ex);
                    if (ex != null) return;
                    string SQL = string.Empty;/////////////在Oracle中建立表的SQL语句
                    SQL += "CREATE TABLE " + sTable+"(";
                    for (int i = 0; i < GetTable.Columns.Count; i++)
                    {
                        DataColumn GetColumn = GetTable.Columns[i];
                        string sColumnName = GetColumn.ColumnName;
                        
                        int iColumnLen=GetColumn.MaxLength;
                        if (iColumnLen < 0) iColumnLen = 100;
                        if (sColumnName == "存储位置") iColumnLen = 500;

                        if (GetColumn.DataType.FullName == "System.String")
                        {
                            SQL += sColumnName;
                            SQL += " VARCHAR2(" + iColumnLen.ToString() + "),";
                        }
                        else if (GetColumn.DataType.FullName == "System.Decimal" || GetColumn.DataType.FullName == "System.Int32" || GetColumn.DataType.FullName == "System.Int64" || GetColumn.DataType.FullName == "System.Int16")
                        {
                            SQL += sColumnName;
                            SQL += " NUMBER,";
                        }
                        else if (GetColumn.DataType.FullName == "System.Double" || GetColumn.DataType.FullName == "System.Single") 
                        {
                            SQL += sColumnName;
                            SQL += " NUMBER,";
                        }
                        else if (GetColumn.DataType.FullName == "System.DateTime")
                        {
                            SQL += sColumnName;
                            SQL += " DATE,";
                        }
                        else if (GetColumn.DataType.FullName == "System.Byte[]")
                        {
                            SQL += sColumnName;
                            SQL += " BLOB,";
                        }
                    }
                    SQL = SQL.Substring(0, SQL.LastIndexOf(','));
                    SQL += " )";
                    if (!m_DataBaseOper.UpdateTable(SQL, out ex))
                    {
                        ex = new Exception("表：" +sTable+" 创建失败，\n原因：" +ex.Message);
                        this.m_DataBaseOper.EndTransaction(false);
                        return;
                    }
                }
            }
            catch(Exception eError)
            {
                ex = eError;
                return;
            }
            try
            {
                //////插入数据格式记录，图幅数据类型记录///////////////////////////////////////////////////////////////////
                string[] insertsql = new string[4];
                insertsql[0] = "INSERT INTO DataFormatTable(数据格式编号,数据格式) VALUES(0,'DLG')";
                insertsql[1] = "INSERT INTO DataFormatTable(数据格式编号,数据格式) VALUES(1,'DOM')";
                insertsql[2] = "INSERT INTO DataFormatTable(数据格式编号,数据格式) VALUES(2,'DEM')";
                insertsql[3] = "INSERT INTO DataFormatTable(数据格式编号,数据格式) VALUES(3,'DRG')";
                for (int i = 0; i < insertsql.Length; i++)
                {
                    this.m_DataBaseOper.UpdateTable(insertsql[i], out ex);
                    if (ex != null)
                    {
                        ex = new Exception("数据格式表初始化失败，\n原因：" + ex.Message);
                        this.m_DataBaseOper.EndTransaction(false);
                        return;
                    }
                }
                ////////
                insertsql = new string[3];
                insertsql[0] = "INSERT INTO DataTypeTable(图幅类型编号,图幅类型) VALUES(0,'标准图幅')";
                insertsql[1] = "INSERT INTO DataTypeTable(图幅类型编号,图幅类型) VALUES(1,'非标准图幅')";
                insertsql[2] = "INSERT INTO DataTypeTable(图幅类型编号,图幅类型) VALUES(2,'属性信息')";
                for (int i = 0; i < insertsql.Length; i++)
                {
                    this.m_DataBaseOper.UpdateTable(insertsql[i], out ex);
                    if (ex != null)
                    {
                        ex = new Exception("图幅类型表初始化失败，\n原因：" + ex.Message);
                        this.m_DataBaseOper.EndTransaction(false);
                        return;
                    }
                }
                ////////
                this.m_DataBaseOper.EndTransaction(true);
            }
            catch(Exception eError)
            {
                ex = new Exception("成果文件库元信息库初始化失败，\n原因：" + eError.Message);
                return;
            }

        }
    }
}
