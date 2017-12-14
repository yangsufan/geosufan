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
        private DBOperatorType OperatorType = DBOperatorType.UnKnowOperator;
        public DBOperatorType m_OperatorType
        {
            get { return OperatorType; }
        }
        private DBType ConnectType = DBType.DEFAULT;
        public DBType m_ConnectType
        {
            get { return ConnectType; }
        }
        private string Server = string.Empty;
        public string m_Server
        {
            get { return Server; }
        }
        private string Service = string.Empty;
        public string m_Service
        {
            get { return Service; }
        }
        private string Database = string.Empty;
        public string m_Database
        {
            get { return Database; }
        }
        private string User = string.Empty;
        public string m_User
        {
            get { return User; }
        }
        private string Password = string.Empty;
        public string m_Password
        {
            get { return Password; }
        }
        private string Version = string.Empty;
        public string m_Version
        {
            get { return m_Version; }
        }
        private string ServerPort = string.Empty;
        public string m_ServerPort
        {
            get { return ServerPort; }
        }
        #endregion
        #region Class Funcion
        public void ReadConfigFromFile(string fileName)
        {
            if (!File.Exists(fileName)) return;
            StreamReader sr = new StreamReader(fileName,Encoding.UTF8);
            string readStr = sr.ReadToEnd();
            string ConfigStr = Encryption.Decrypt(readStr);
            string[] data = ConfigStr.Split(',');
            if (data.Length <= 0) return;
            int temp = 0;
            foreach (string dataitem in data)
            {
                string[] configitem = dataitem.Split('=');
                if (configitem.Length !=2) continue;
                switch (configitem[0])
                {
                    case "server":
                        Server = configitem[1];
                        break;
                    case "serverice":
                        Service = configitem[1];
                        break;
                    case "database":
                        Database = configitem[1];
                        break;
                    case "user":
                        User = configitem[1];
                        break;
                    case "password":
                        Password = configitem[1];
                        break;
                    case "version":
                        Version = configitem[1];
                        break;
                    case "serverport":
                        ServerPort = configitem[1];
                        break;
                    case "operatortype":
                        int.TryParse(configitem[1],out temp);
                        OperatorType = (DBOperatorType)temp;
                        break;
                    case "dbtype":
                        int.TryParse(configitem[1],out temp);
                        ConnectType = (DBType)temp;
                        break;
                }
            }
        }
        public void SetConfig(DBOperatorType operatorType, DBType connecttype,Dictionary<string,string> Dicconnect)
        {
            OperatorType = operatorType;
            ConnectType = connecttype;
            if (Dicconnect.Count == 0) return;
            foreach (string key in Dicconnect.Keys)
            {
                switch (key.ToLower())
                {
                    case "server":
                        Server = Dicconnect[key];
                        break;
                    case "serverice":
                        Service = Dicconnect[key];
                        break;
                    case "database":
                        Database = Dicconnect[key];
                        break;
                    case "user":
                        User = Dicconnect[key];
                        break;
                    case "password":
                        Password = Dicconnect[key];
                        break;
                    case "version":
                        Version = Dicconnect[key];
                        break;
                    case "serverport":
                        ServerPort = Dicconnect[key];
                        break;
                }
            }

        }
        public void SaveConfig(string fileName)
        {
            string writeStr = string.Empty;
            writeStr = string.Format("operatortype={0},dbtype={1},server={2},serverice={3},database={4},user={5}," +
                "password={6},version={7},serverport={8}",(int)OperatorType,(int)ConnectType,
               Server, Service, Database, User, Password, Version, ServerPort);
            writeStr = Encryption.Encrypt(writeStr);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(writeStr);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
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
