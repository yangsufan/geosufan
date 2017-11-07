using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

using SysCommon;

namespace GeoUtilities
{
    //获取配置系统的连接信息
    public class clsDBConnect : SysCommon.DataBase.SysTable
    {
        private string _DbType = "";//数据库类型  陈亚飞增加
        public string DBTYPE
        {
            get
            {
                return this._DbType;
            }
            set
            {
                this._DbType = value;
            }
        }
        private string _ConStr = "";//数据库连接字符串信息  陈亚飞增加
        public string CONNSTR
        {
            get
            {
                return this._ConStr;
            }
            set
            {
                this._ConStr = value;
            }
        }

        /// <summary>
        /// 获得数据库连接的参数  陈亚飞增加
        /// </summary>
        public void GetConInfo()
        {
            string fileStr = Application.StartupPath + "\\dbSet.txt";

            ///检查该文件是否存在
            if (File.Exists(fileStr))
            {
                StreamReader sr = new StreamReader(fileStr);
                _DbType = sr.ReadLine();//数据库类型
                _ConStr = sr.ReadLine();//数据库连接字符串
            }

        }
        /// <summary>
        /// 根据数据库类型给枚举变量赋值 陈亚飞增加
        /// </summary>
        public void GetDBType()
        {
            GetConInfo();
            if (_DbType == "") return;
            switch (_DbType.Trim())
            {
                case "ORACLE":
                    DBConType =enumDBConType.ORACLE;
                    DBType = enumDBType.ORACLE;
                    break;
                case "ACCESS":
                    DBConType = enumDBConType.OLEDB;
                    DBType = enumDBType.ACCESS;
                    break;
                case "SQL":
                    DBConType = enumDBConType.SQL;
                    DBType = enumDBType.SQLSERVER;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///连接数据库
        /// </summary>
        /// <param name="DBtype"></param>
        /// <param name="strConnectionString"></param>
        /// <param name="eError"></param>
        public void ConnectDataBse(out Exception eError)
        {
            eError = null;
            GetDBType();
            if (_ConStr == "")
                return;
            try
            {
                SetDbConnection(_ConStr, DBConType, DBType, out eError);
            }
            catch (Exception ex)
            {
                eError = ex;
                return;
            }
        }

        /// <summary>
        /// 获取GIS库连接
        /// </summary>
        /// <returns></returns>
        public IWorkspace GetWorkspace()
        {
            Exception eError = null;
            if (_ConStr == "" || _DbType == "") return null;

            IWorkspace pWorkspace = null;

            if (_DbType == "ORACLE")
            {
                string[] arr = _ConStr.Split(new char[] { ';' });//将普通表的连接字符串分解成各个参数
                string serviceStr = "";//获得服务名参数
                string userStr = "";//获得用户名参数
                string pswdStr = "";//获得密码参数
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] Strs = arr[i].Split(new char[] { '=' });//获得各个参数并分解
                    if (i == 0)
                    {
                        serviceStr = Strs[1];//服务名
                    }
                    else if (i == 2)
                    {
                        userStr = Strs[1];//其他参数
                    }
                    else if (i == 3)
                    {
                        pswdStr = Strs[1];//其他参数
                    }
                    else
                    {
                        continue;
                    }
                }
                string strInstance = "SDE:Oracle10g:" + serviceStr;
                string strUser = userStr;
                string strPassword = pswdStr + "@" + serviceStr;
                string strVersion = "SDE.DEFAULT";

                pWorkspace = SetWorkspace("", strInstance, "", strUser, strPassword, strVersion, out eError);
            }
            else if (_DbType == "ACCESS")
            {
                int index = _ConStr.IndexOf("Data Source=");
                string dbPath = _ConStr.Substring(index + 12);
                string[] strArr = dbPath.Split(new char[] { ';' });

                dbPath = strArr[0];
                pWorkspace = SetWorkspace(dbPath, enumWSType.PDB, out eError);
            }

            if (eError != null) return null;
            return pWorkspace;
        }

        /// <summary>
        /// 设置SDE工作区
        /// </summary>
        /// <param name="sServer">服务器名</param>
        /// <param name="sService">服务名</param>
        /// <param name="sDatabase">数据库名(SQLServer)</param>
        /// <param name="sUser">用户名</param>
        /// <param name="sPassword">密码</param>
        /// <param name="strVersion">SDE版本</param>
        /// <returns>输出错误Exception</returns>
        private IWorkspace SetWorkspace(string sServer, string sService, string sDatabase, string sUser, string sPassword, string strVersion, out Exception eError)
        {
            eError = null;
            IWorkspace pWorkspace = null;
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);

            try
            {
                pWorkspace = pSdeFact.Open(pPropSet, 0);
                pPropSet = null;
                pSdeFact = null;
                return pWorkspace;
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }

        /// <summary>
        /// 设置PDB、GDB工作区
        /// </summary>
        /// <param name="sFilePath">文件路径</param>
        /// <param name="wstype">工作区类型</param>
        /// <returns>输出错误Exception</returns>
        private IWorkspace SetWorkspace(string sFilePath, enumWSType wstype, out Exception eError)
        {
            eError = null;
            IWorkspace pWorkspace = null;
            try
            {
                IPropertySet pPropSet = new PropertySetClass();
                switch (wstype)
                {
                    case enumWSType.PDB:
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWorkspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        break;
                    case enumWSType.GDB:
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        pPropSet.SetProperty("DATABASE", sFilePath);
                        pWorkspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        break;
                }
                pPropSet = null;
                return pWorkspace;
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }
    }
}
