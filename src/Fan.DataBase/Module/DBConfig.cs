using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// DataBase Config
/// </summary>
namespace Fan.DataBase
{
    public class DBConfig
    {
        public DBConfig()
        {

        }
        #region Class property 
        private DBOperatorType m_OperatorType = DBOperatorType.UnKnowOperator;
        public DBOperatorType OperatorType
        {
            get { return m_OperatorType; }
        }
        private DBType m_ConnectType = DBType.DEFAULT;
        public DBType ConnectType
        {
            get { return m_ConnectType; }
        }
        private string m_Server = string.Empty;
        public string Server
        {
            get { return m_Server; }
        }
        private string m_Service = string.Empty;
        public string Service
        {
            get { return m_Service; }
        }
        private string m_Database = string.Empty;
        public string Database
        {
            get { return m_Database; }
        }
        private string m_User = string.Empty;
        public string User
        {
            get { return m_User; }
        }
        private string m_Password = string.Empty;
        public string Password
        {
            get { return m_Password; }
        }
        private string m_Version = string.Empty;
        public string Version
        {
            get { return m_Version; }
        }
        private string m_ServerPort = string.Empty;
        public string ServerPort
        {
            get { return m_ServerPort; }
        }
        #endregion
        #region Class Funcion
        public void ReadConfigFromFile(string fileName)
        {
            if (!File.Exists(fileName)) return;
            StreamReader sr = new StreamReader(fileName,Encoding.UTF8);
            string readStr = sr.ReadToEnd();
            ReadConfigFromStr(readStr);
        }
        /// <summary>
        /// 从加密字符串中读取配置信息
        /// </summary>
        /// <param name="strConfig"></param>
        public void ReadConfigFromStr(string strConfig)
        {
            string ConfigStr = Encryption.Decrypt(strConfig);
            string[] data = ConfigStr.Split(',');
            if (data.Length <= 0) return;
            int temp = 0;
            foreach (string dataitem in data)
            {
                string[] configitem = dataitem.Split('=');
                if (configitem.Length != 2) continue;
                switch (configitem[0])
                {
                    case "server":
                        m_Server = configitem[1];
                        break;
                    case "serverice":
                        m_Service = configitem[1];
                        break;
                    case "database":
                        m_Database = configitem[1];
                        break;
                    case "user":
                        m_User = configitem[1];
                        break;
                    case "password":
                        m_Password = configitem[1];
                        break;
                    case "version":
                        m_Version = configitem[1];
                        break;
                    case "serverport":
                        m_ServerPort = configitem[1];
                        break;
                    case "operatortype":
                        int.TryParse(configitem[1], out temp);
                        m_OperatorType = (DBOperatorType)temp;
                        break;
                    case "dbtype":
                        int.TryParse(configitem[1], out temp);
                        m_ConnectType = (DBType)temp;
                        break;
                }
            }
        }
        public void SetConfig(DBOperatorType operatorType, DBType connecttype,Dictionary<string,string> Dicconnect)
        {
            m_OperatorType = operatorType;
            m_ConnectType = connecttype;
            if (Dicconnect.Count == 0) return;
            foreach (string key in Dicconnect.Keys)
            {
                switch (key.ToLower())
                {
                    case "server":
                        m_Server = Dicconnect[key];
                        break;
                    case "serverice":
                        m_Service = Dicconnect[key];
                        break;
                    case "database":
                        m_Database = Dicconnect[key];
                        break;
                    case "user":
                        m_User = Dicconnect[key];
                        break;
                    case "password":
                        m_Password = Dicconnect[key];
                        break;
                    case "version":
                        m_Version = Dicconnect[key];
                        break;
                    case "serverport":
                        m_ServerPort = Dicconnect[key];
                        break;
                }
            }

        }
        public void SaveConfig(string fileName)
        {
            string writeStr = GetConfigStr();
            FileStream fs = new FileStream(fileName, FileMode.Create);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(writeStr);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        /// <summary>
        /// 获取数据库连接加密字符串
        /// </summary>
        /// <returns></returns>
        public string GetConfigStr()
        {
            string writeStr = string.Empty;
            writeStr = string.Format("operatortype={0},dbtype={1},server={2},serverice={3},database={4},user={5}," +
                "password={6},version={7},serverport={8}", (int)OperatorType, (int)ConnectType,
               Server, Service, Database, User, Password, Version, ServerPort);
            writeStr = Encryption.Encrypt(writeStr);
            return writeStr;
        }
        /// <summary>
        /// 获取数据库配置名称
        /// 如果是关系型数据库则返回服务器IP，如果是文件数据库则返回文件路径
        /// </summary>
        /// <returns></returns>
        public string GetConfigName()
        {
            if (m_ConnectType == DBType.ESRIGDB || m_ConnectType == DBType.ESRIPDB || m_ConnectType == DBType.ESRISHP || m_ConnectType == DBType.ODBCMDB)
                return m_Database;
            else if (m_ConnectType != DBType.DEFAULT) return string.Format("{0}:{1}",m_Server,m_Database);
            return string.Empty;
        }
        #endregion

    }
    public enum DBOperatorType
    {
        ODBC=1,
        EsriOperator=2,
        UnKnowOperator=0
    }
}
