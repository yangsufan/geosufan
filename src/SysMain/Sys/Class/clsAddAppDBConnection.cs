using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OracleClient;
using System.Data;

namespace GDBM
{
    /*
     * guozheng 2010-10-8
     * 该类实现系统维护库的连接字符串(Oracle)的获取、设置
     * 以及使用连接字符串进行系统主界面的刷新
     * 
     */ 
    class clsAddAppDBConnection
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public clsAddAppDBConnection()
        {
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <returns>返回已用连接字符串</returns>
        public string GetAppDBConInfo(out Exception ex)
        {
            ex = null;
            string sConnect = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            sConnect = string.Empty;
            if (File.Exists(Mod.v_AppDBConectXml))
            {
                #region 存在记录则读取连接信息
                XmlDoc.Load(Mod.v_AppDBConectXml);
                XmlElement ele = XmlDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
                if (ele != null)
                {
                    try
                    {
                        sConnect = ele.GetAttribute("连接字符串");                       
                    }
                    catch
                    {
                        ex = new Exception("系统维护库连接信息配置文件不规范");
                        sConnect = string.Empty;
                    }
                }
                #endregion
            }
            else
            {
                #region 不存在则建立一个空的文件
                XmlNode ele = XmlDoc.CreateNode(XmlNodeType.Element, "系统维护库连接信息", null);
                XmlAttribute addAttr = null;
                ////
                addAttr = XmlDoc.CreateAttribute("类型");
                addAttr.Value = "ORACLE";
                ele.Attributes.SetNamedItem(addAttr);
                ////
                addAttr = XmlDoc.CreateAttribute("连接字符串");
                addAttr.Value = "";
                ele.Attributes.SetNamedItem(addAttr);
                ////
                XmlDoc.AppendChild(ele);
                try
                {
                    XmlDoc.Save(Mod.v_AppDBConectXml);
                }
                catch
                {
                    ex = new Exception("系统维护库连接信息保存失败");
                    sConnect = string.Empty;
                }
                #endregion
            }
            return sConnect;
        }
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <returns>返回设置的连接字符串</returns>
        public string  SetAppDBConInfo(out Exception ex)
        {
            ex = null;
            frmSetAppDB SetApp = new frmSetAppDB();
            if (File.Exists(Mod.v_AppDBConectXml))
            {
                #region 存在记录则读取连接信息
                XmlDocument XmlDoc = new XmlDocument();
                string sConnect = string.Empty;//////////////连接字符串
                XmlDoc.Load(Mod.v_AppDBConectXml);
                XmlElement ele = XmlDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
                if (ele != null)
                {
                    try
                    {
                        sConnect = ele.GetAttribute("连接字符串");
                        OracleConnectionStringBuilder pConnectionStringBuilder = new OracleConnectionStringBuilder(sConnect);
                        SetApp.Server = pConnectionStringBuilder.DataSource;
                        SetApp.User = pConnectionStringBuilder.UserID;
                        SetApp.Password = pConnectionStringBuilder.Password;
                    }
                    catch
                    {
                        ex = new Exception("系统维护库连接信息配置文件不规范");
                        sConnect = string.Empty;
                    }
                }
                #endregion
            }
            if (SetApp.ShowDialog() == DialogResult.Yes)
            {
                string sServer = SetApp.Server;
                string sUser = SetApp.User;
                string sPassword = SetApp.Password;
                string sConnect = "Data Source=" + sServer + ";User ID=" + sUser + ";Password=" + sPassword;//Persist Security Info=True;
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(Mod.v_AppDBConectXml);
                XmlElement ele = XmlDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
                if (ele != null)
                {
                    try
                    {
                        ele.SetAttribute("连接字符串", sConnect);
                        XmlDoc.Save(Mod.v_AppDBConectXml);
                    }
                    catch
                    {
                        ex = new Exception("系统维护库连接信息配置文件不规范");
                        sConnect = string.Empty;
                    }
                }
                return sConnect;

            }
            else
            {
                ////按了取消退出程序
                Application.Exit();
                return null;
            }


        }

        /// <summary>
        /// 判断连接信息中的系统维护库的库体机构是否完整
        /// </summary>
        /// <param name="sConnetInfo">Oracle数据库连接信息</param>
        /// <param name="ex"></param>
        public void JudgeAppDbConfiguration(string sConnetInfo, out Exception ex)
        {
            ex = null;
            OracleConnection OracleCon = null;
            try
            {
                 OracleCon = new OracleConnection(sConnetInfo);
                 OracleCon.Open();
            }
            catch(Exception eError)
            {
                ex = new Exception("连接系统维护库失败");
                if (OracleCon.State == ConnectionState.Open)
                    OracleCon.Close();
                return;
            }
            try
            {
                //判断数据库类型表是否存在
                if (OracleCon.State == ConnectionState.Closed)
                    OracleCon.Open();
                string SQL = "SELECT COUNT(*) FROM DATABASETYPEMD";
                OracleCommand COm = new OracleCommand(SQL, OracleCon);
                COm.ExecuteNonQuery();
            }
            catch
            {
                ex = new Exception("数据库类型表(DATABASETYPEMD)不存在！");
                if (OracleCon.State == ConnectionState.Open)
                    OracleCon.Close();
                return;
            }
            try
            {
                //判断数据库平台表是否存在
                if (OracleCon.State == ConnectionState.Closed)
                    OracleCon.Open();
                string SQL = "SELECT COUNT(*) FROM DATABASEFORMATMD";
                OracleCommand COm = new OracleCommand(SQL, OracleCon);
                COm.ExecuteNonQuery();
            }
            catch
            {
                ex = new Exception("数据库平台表(DATABASEFORMATMD)不存在！");
                if (OracleCon.State == ConnectionState.Open)
                    OracleCon.Close();
                return;
            }
        
            try
            {
                //判断数据库状态表是否存在
                if (OracleCon.State == ConnectionState.Closed)
                    OracleCon.Open();
                string SQL = "SELECT COUNT(*) FROM DATABASESTATEMD";
                OracleCommand COm = new OracleCommand(SQL, OracleCon);
                COm.ExecuteNonQuery();
            }
            catch
            {
                ex = new Exception("数据库状态表(DATABASESTATEMD)不存在！");
                if (OracleCon.State == ConnectionState.Open)
                    OracleCon.Close();
                return;
            }
            try
            {
                //判断数据库元信息表是否存在
                if (OracleCon.State == ConnectionState.Closed)
                    OracleCon.Open();
                string SQL = "SELECT COUNT(*) FROM DATABASEMD ";
                OracleCommand COm = new OracleCommand(SQL, OracleCon);
                COm.ExecuteNonQuery();
            }
            catch
            {
                ex = new Exception("数据库元信息表(DATABASEMD )不存在！");
                if (OracleCon.State == ConnectionState.Open)
                    OracleCon.Close();
                return;
            }
            if (OracleCon.State == ConnectionState.Open)
                OracleCon.Close();
        }
    }
}
