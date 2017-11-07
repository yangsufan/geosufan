using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace GeoDBIntegration
{
    /*
     *  guozheng 2011-5-10 added 该类实现元数据表的解析生成建表的SQL语句（Oracle） 
     *     元数据表的结构根据输入的MDB文件获取
     *     版本：1.0.0
     */
    class ClsMetaTableOperation
    {
        private string m_MDBFilePath = string.Empty;///////////////mdb文件路径
        /// <summary>
        /// 模板文件路径
        /// </summary>
        public string MDBFileTemplatePath
        {
            set { this.m_MDBFilePath = value; }
            get { return this.m_MDBFilePath; }
        }
        private string m_sExPandField = string.Empty;//////////////增加的扩展字段
        /// <summary>
        /// 扩展字段,最后不带逗号（格式《ColumnName1 NUMBER,ColumnName2 VARCHAR2(100)》）
        /// </summary>
        public string sExPandField
        {
            get { return this.m_sExPandField; }
            set { this.m_sExPandField = value; }
        }
        

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClsMetaTableOperation()
        {
        }
        public ClsMetaTableOperation(string in_sMDBTemplateFile)
        {
            this.m_MDBFilePath = in_sMDBTemplateFile;
        }
        public ClsMetaTableOperation(string in_sMDBTemplateFile,string in_sExpandFields)
        {
            this.m_MDBFilePath = in_sMDBTemplateFile;
            this.m_sExPandField = in_sExpandFields;
        }

        /// <summary>
        /// 获取MDB模板文件中已存在的表名称  2011-5-10
        /// </summary>
        /// <param name="in_sMDBFilePath">MDB模板文件路径</param>
        /// <param name="ex">输出：错误信息</param>
        /// <returns>string[]表名列表，内部异常返回NULL</returns>
        public static string[] GetShemaTableNames(string in_sMDBFilePath, out Exception ex)
        {
            ex = null;
            OleDbConnection conn = new OleDbConnection();
            try
            {
                //获取数据表
                conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + in_sMDBFilePath + ";Persist Security Info=True";
                conn.Open();
                DataTable shemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                int n = shemaTable.Rows.Count;
                string[] strTable = new string[n];
                int m = shemaTable.Columns.IndexOf("TABLE_NAME");
                for (int i = 0; i < n; i++)
                {
                    DataRow m_DataRow = shemaTable.Rows[i];
                    strTable[i] = m_DataRow.ItemArray.GetValue(m).ToString();
                }
                return strTable;
            }
            catch (OleDbException eError)
            {
               // MessageBox.Show("指定的限制集无效: " + ex.Message);
                ex = eError;
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

        /// <summary>
        /// 获取建表语句
        /// </summary>
        /// <param name="ex">输出错误信息</param>
        /// <returns>SQL语句文本，内部异常返回NULL</returns>
        public string GetTableSql(out Exception ex)
        {
            if (string.IsNullOrEmpty(this.m_MDBFilePath))
            { ex = new Exception("没有指定模板文件"); return null; }
            string[] ExistTableNames = GetShemaTableNames(this.m_MDBFilePath, out ex);
            if (ex != null) return null;
            if (ExistTableNames.Length <= 0) { ex = new Exception("模板文件中没有找到表"); return null; }
            //////////////////////////取第一个表解析其结构构造SQL语句//////////////////////////////
            try
            {
                string TarGetTableName = ExistTableNames[0];
                SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.m_MDBFilePath + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out ex);
                if (ex != null) return null;
                DataTable GetTable = metaSysTable.GetTable(TarGetTableName, out ex);
                if (ex != null) return null;
                string SQL = GetSQLByTable(GetTable, TarGetTableName, out ex);
                return SQL;
            }
            catch (Exception eError)
            {
                ex = eError;
                return null;
            }
        }
        public string GetTableSql(string sTableName, out Exception ex)
        {

            if (string.IsNullOrEmpty(this.m_MDBFilePath))
            { ex = new Exception("没有指定模板文件"); return null; }
            try
            {
                string TarGetTableName = sTableName;
                SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.m_MDBFilePath + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out ex);
                if (ex != null) return null;
                DataTable GetTable = metaSysTable.GetTable(TarGetTableName, out ex);
                if (ex != null) return null;
                string SQL = GetSQLByTable(GetTable, TarGetTableName, out ex);
                return SQL;
            }
            catch (Exception eError)
            {
                ex = eError;
                return null;
            }
        }
        /// <summary>
        /// 根据Table对象解析生成SQL文本
        /// </summary>
        /// <param name="in_Table">输入Table对象</param>
        /// <param name="sTable">建立的元信息表的表名</param>
        /// <param name="ex">输出：异常信息</param>
        /// <returns>SQL文本</returns>
        private string GetSQLByTable(DataTable in_Table,string sTable, out Exception ex)
        {
            if (in_Table == null) { ex = new Exception("输入表为空"); return null; }
            ex = null;
            try
            {
                string  SQL = "CREATE TABLE " + sTable + "(";
                
                for (int i = 0; i < in_Table.Columns.Count; i++)
                {
                    DataColumn GetColumn = in_Table.Columns[i];
                    string sColumnName = GetColumn.ColumnName;

                    int iColumnLen = GetColumn.MaxLength;
                    if (iColumnLen <= 0) iColumnLen = 500;
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
                if (!string.IsNullOrEmpty(this.m_sExPandField))
                    SQL += this.m_sExPandField + ",";
                SQL = SQL.Substring(0, SQL.LastIndexOf(','));
                SQL += " )";
                return SQL;
            }
            catch (Exception eError)
            {
                ex = eError;
                return null;
            }
        }
    }
}
