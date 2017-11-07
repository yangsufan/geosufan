using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace SysCommon.Authorize
{
    public class AuthorizeClass
    {
        //简单加密
        public static string ComputerSecurity(string strPass)
        {
            Byte[] bytValue;
            Byte[] bytHash;

            bytValue = System.Text.Encoding.UTF8.GetBytes(strPass);
            System.Security.Cryptography.MD5CryptoServiceProvider vMD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            bytHash = vMD5.ComputeHash(bytValue);
            vMD5.Clear();

            string strRes = Convert.ToBase64String(bytHash);
            return strRes;
        }

        #region 序列化
        public static void Serialize(Hashtable _hashtable, string strFile)
        {
            System.IO.FileStream fs = new System.IO.FileStream(strFile, FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, _hashtable);
            }

            catch (System.Runtime.Serialization.SerializationException e)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(e);
                //******************************************
                MessageBox.Show("Failed to serialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }

        }


        #endregion

        #region 反序列化

        public static void Deserialize(ref Hashtable _hashtable, string strFile)
        {
            System.IO.FileStream fs = new System.IO.FileStream(strFile, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // Deserialize the hashtable from the file and assign the reference to the local variable.

                _hashtable = (Hashtable)formatter.Deserialize(fs);
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                //******************************************
                //guozheng added System Exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(e);
                //******************************************
                MessageBox.Show("Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }
        }
        #endregion

        //通过反序列化 获得链接信息
        public static void GetConnectInfo(string strFile, out string strServer, out string strSevice, out string strDatabase, out string strUser, out string strPass, out string strVersion, out string strType)
        {
            strServer = "";
            strSevice = "";
            strDatabase = "";
            strUser = "";
            strPass = "";
            strVersion = "";
            strType = "";

            if (!File.Exists(strFile)) return;

            Hashtable vTemp = new Hashtable();
            Deserialize(ref vTemp, strFile);

            if (vTemp == null) return;
            foreach (DictionaryEntry de in vTemp)
            {
                string strKey = de.Key.ToString().ToUpper();
                string strValue = de.Value.ToString();

                switch (strKey)
                {
                    case "SERVER":
                        strServer = strValue;
                        break;
                    case "SERVICE":
                        strSevice = strValue;
                        break;
                    case "DATABASE":
                        strDatabase = strValue;
                        break;
                    case "USER":
                        strUser = strValue;
                        break;
                    case "PASSWORD":
                        strPass = strValue;
                        break;
                    case "VERSION":
                        strVersion = strValue;
                        break;
                    case "DBTYPE":
                        strType = strValue;
                        break;
                }
            }
        }
    }
}
