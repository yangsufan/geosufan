using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace GeoDBATool
{

    public class GeoFIDCreator
    {
        private string _DatasetName = null;    //需要生成FID的要素类集合
        private IWorkspace _DestinationWorkspace = null;   //目标工作空间
        private IFeatureWorkspace _FeatureWorkspace = null;
        private string _ConnectionString = null;    //数据库连接字符串
        private string _DBType = null;

        /// <summary>
        /// 需要生成FID的要素类集合
        /// </summary>
        public string DatasetName
        {
            get { return _DatasetName; }
            set { _DatasetName = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DestinationWorkspace">目标工作空间</param>
        /// <param name="ConnectionString">数据库连接字符串</param>
        public GeoFIDCreator(IWorkspace DestinationWorkspace, string ConnectionString,FIDDBType TableType)
        {
            //未类成员变量赋值
            this._DestinationWorkspace = DestinationWorkspace;
            this._FeatureWorkspace = (IFeatureWorkspace)DestinationWorkspace;
            this._ConnectionString = ConnectionString;
            switch (TableType)
            {
                case FIDDBType.oracle:

                    this._DBType = "oracle";
                    break;
                case FIDDBType.access:
                    this._DBType = "access";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 创建FID记录表
        /// </summary>
        /// <param name="FIDTableName">FID记录表名</param>
        /// <returns></returns>
        public bool CreateFID(string FIDTableName)
        {
            IEnumDataset pEnumDataset = null;
            IDataset pDT = null;
            DbConnection DBConn = null;

            try
            {

                if (this._DBType == "oracle")
                {
                    DBConn = new OracleConnection(this._ConnectionString);
                    DBConn.Open();
                    if (!CreateTable(DBConn as OracleConnection)) return false;
                }
                else if (this._DBType == "access")
                {
                    DBConn = new OleDbConnection(this._ConnectionString);
                    DBConn.Open();
                }


                //根据名称获得数据集
                if (this._DatasetName == null)     //如果空间数据库中没有数据集，只有目标要素类
                {
                    pEnumDataset = _DestinationWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);

                    pDT = pEnumDataset.Next();

                    while (pDT != null)
                    {
                        IFeatureClass pFeatureClass = this._FeatureWorkspace.OpenFeatureClass(pDT.Name);

                        CreatreFIDByFeatureClass(pFeatureClass, FIDTableName, DBConn);

                        pDT = pEnumDataset.Next();
                    }

                }
                else     //如果指定了数据集
                {
                    IEnumDataset pSubEnumDataset = null;
                    pEnumDataset = _DestinationWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);

                    IDataset pDataset = pEnumDataset.Next();
                    while (pDataset != null)
                    {
                        //从要素类名称中去掉用户名称（SDE中是带有用户名的），如果存在用户名的话\
                        string pFeatureClassName = pDataset.Name;
                        string[] pTemp = pFeatureClassName.Split('.');

                        if (pTemp.Length == 2)
                        {
                            pFeatureClassName = pTemp[1];
                        }
                        else
                        {
                            pFeatureClassName = pTemp[0];
                        }

                        if (pFeatureClassName == this._DatasetName)
                        {
                            pSubEnumDataset = pDataset.Subsets;
                            pDT = pSubEnumDataset.Next();
                            while (pDT != null)
                            {
                                IFeatureClass pFeatureClass = this._FeatureWorkspace.OpenFeatureClass(pDT.Name);

                                CreatreFIDByFeatureClass(pFeatureClass, FIDTableName, DBConn);

                                pDT = pSubEnumDataset.Next();
                            }
                        }
                        pDataset = pEnumDataset.Next();
                    }


                }

                DBConn.Close();
            }
            catch(Exception e)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据输入的要素类，遍历其中的要素，生成FID到指定的FID表
        /// </summary>
        /// <param name="pFeatureClass">要素类对象</param>
        /// <param name="FIDTableName">FID表名</param>
        /// <param name="pConnectionString">数据库连接字符串</param>
        private void CreatreFIDByFeatureClass(IFeatureClass pFeatureClass, string FIDTableName, DbConnection DBConn)
        {
            IDataset pDataset = pFeatureClass as IDataset;
            string FeatureClassName = pDataset.Name;


            //从要素类名称中去掉用户名称（SDE中是带有用户名的），如果存在用户名的话\
            string[] pTemp = FeatureClassName.Split('.');

            if (pTemp.Length == 2)
            {
                FeatureClassName = pTemp[1];
            }
            else
            {
                FeatureClassName = pTemp[0];
            }



            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {

                //写入记录
                string SQL = "insert into " + FIDTableName + "(FCNAME,OID) values('" + FeatureClassName + "'," + pFeature.OID + ")";


                
                if (this._DBType == "oracle")
                {
                    DbCommand _DbCom = new OracleCommand(SQL, (OracleConnection)DBConn);
                    _DbCom.ExecuteNonQuery();
                }
                else if (this._DBType == "access")
                {
                    DbCommand _DbCom = new OleDbCommand(SQL, (OleDbConnection)DBConn);
                    _DbCom.ExecuteNonQuery();
                }

                pFeature = pFeatureCursor.NextFeature();
            }


            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
        }

        private bool CreateTable(OracleConnection DBConn)
        {
            try
            {
                string sqlCreate = "create table FID记录表(GOFID  INTEGER not null,FCNAME NVARCHAR2(50),OID    INTEGER)";
                OracleCommand DbCom = new OracleCommand(sqlCreate, DBConn);
                DbCom.ExecuteNonQuery();
                sqlCreate = "create sequence FID记录表_SEQ minvalue 1 maxvalue 9999999999 start with 1 increment by 1 cache 30 order";
                DbCom = new OracleCommand(sqlCreate, DBConn);
                DbCom.ExecuteNonQuery();
                sqlCreate = "create or replace trigger FID记录表_TG before insert on FID记录表 for each row begin if :NEW.GOFID=0 or :NEW.GOFID is null then SELECT FID记录表_SEQ.NEXTVAL   INTO   :NEW.GOFID   FROM   DUAL; end if; end;";
                DbCom = new OracleCommand(sqlCreate, DBConn);
                DbCom.ExecuteNonQuery();

                DbCom = null;
                return true;
            }
            catch(Exception e)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return false;
            }
        }
    }

    public enum FIDDBType
    { 
        oracle,access
    }
}
