using System.Data.OracleClient;
using System.Data;
namespace GeoStatistics
{
	 public static class ModOracle
    {
         public static string GetNewTableName(OracleConnection conn,string DefaultTableName)
         {
             string ResName = DefaultTableName;
             if (!IsExist(conn, ResName))
             {
                 return ResName;
             }
             for (int i = 0; i < 9999999;i++ )
             {
                 ResName = DefaultTableName + i.ToString();
                 if (!IsExist(conn, ResName))
                 {
                     return ResName;
                 }
             }
             return "";
         }
         public static OracleConnection GetOracleConnection(string strServer, string strDatabase, string strUser, string strPassword)
         {
             try
             {
                 string strOracleConnection = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + strServer + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + strDatabase + ")));Persist Security Info=True;User Id=" + strUser + "; Password=" + strPassword + "";
                 OracleConnection pOracleConnection = new OracleConnection(strOracleConnection);
                 pOracleConnection.Open();
                 return pOracleConnection;
             }
             catch (System.Exception ex)
             {             	
             }
             return null;
         }
         public static OracleDataReader GetReader(OracleConnection conn, string sqlstr)
         {
             if (conn == null)
             {
                 return null;
             }
             if (conn.State == ConnectionState.Closed)
             {
                 return null;
             }
             OracleCommand pCommand = conn.CreateCommand();
             pCommand.CommandText = sqlstr;
             OracleDataReader pReader = null;
             try
             {
                 pReader = pCommand.ExecuteReader();
                 return pReader;
             }
             catch (System.Exception ex)
             {
                 ex.Data.Clear();
                 return null;
             }
         }
         public static void DropTable(OracleConnection conn, string strTableName)
         {
             if (IsExist(conn, strTableName))
             {
                 try
                 {
                     OracleCommand pCommand = conn.CreateCommand();
                     pCommand.CommandText = "drop table "+strTableName;
                     pCommand.ExecuteNonQuery();
                 }
                 catch (System.Exception ex)
                 {
                 	
                 }
             }
         }
         public static bool IsExist(OracleConnection conn, string strTableName)
         {
             if (conn == null)
             {
                 return false;
             }
             if (conn.State == ConnectionState.Closed)
             {
                 return false;
             }
             OracleCommand pCommand = conn.CreateCommand();
             pCommand.CommandText = "select * from "+strTableName +" where 1=0";
             OracleDataReader pReader = null;
             try
             {
                 pReader = pCommand.ExecuteReader();
                 pReader.Close();
                 return true;
             }
             catch (System.Exception ex)
             {
                 ex.Data.Clear();
                 return false;
             }
         }
     }
}