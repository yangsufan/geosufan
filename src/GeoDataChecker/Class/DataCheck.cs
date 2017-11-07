using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Xml;

using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    #region 框架接口、类
    //数据检查参数设置
    public interface IDataCheckParaSet
    {

    }
    public interface IArcgisDataCheckParaSet : IDataCheckParaSet
    {
        //检查数据来接
        IWorkspace Workspace { get;set;}

        //参数数据连接
        DbConnection DbConnPara { get;set;}
        string DbConnParaGeoOne { get;set;}

        //检查结果日志输出连接MDB格式
        DbConnection DbConnRes { get;set;}

        //检查结果日志表名
        string ErrResTableName { get;set;}

        //检查功能组合结构XML
        XmlDocument XmlDocDataCheckSet { get;set;}

    }
    public class ArcgisDataCheckParaSet : IArcgisDataCheckParaSet
    {
        #region IArcgisDataCheckParaSet 成员

        private IWorkspace workspace;
        public IWorkspace Workspace
        {
            get
            {
                return workspace;
            }
            set
            {
                workspace = value;
            }
        }

        private DbConnection dbConnPara;
        public DbConnection DbConnPara
        {
            get
            {
                return dbConnPara;
            }
            set
            {
                dbConnPara = value;
            }
        }

        private string dbConnParaGeoOne;
        public string DbConnParaGeoOne
        {
            get
            {
                return dbConnParaGeoOne;
            }
            set
            {
                dbConnParaGeoOne = value;
            }
        }

        //检查结果日志输出连接MDB格式
        private DbConnection dbConnRes;
        public DbConnection DbConnRes
        {
            get
            {
                return dbConnRes;
            }
            set
            {
                dbConnRes = value;
            }
        }

        private string errResTableName;
        public string ErrResTableName
        {
            get
            {
                return errResTableName;
            }
            set
            {
                errResTableName = value;
            }
        }

        private XmlDocument xmlDocDataCheckSet;
        public XmlDocument XmlDocDataCheckSet 
        { 
            get
            {
                return xmlDocDataCheckSet;
            }
            set
            {
                xmlDocDataCheckSet = value;
            }
        }
        #endregion
    }

    //数据检查接口
    public interface IDataCheckHook
    {
    }

    public interface IArcgisDataCheckHook : IDataCheckHook
    {
        IDataCheckParaSet DataCheckParaSet { get;}
    }

    public class ArcgisDataCheck : IArcgisDataCheckHook
    {
        private IDataCheckParaSet ArcgisDataCheckParaSet;
        public ArcgisDataCheck(IDataCheckParaSet arcgisDataCheckParaSet)
        {
            ArcgisDataCheckParaSet = arcgisDataCheckParaSet;
        }

        #region IArcgisDataCheckHook 成员

        public IDataCheckParaSet DataCheckParaSet
        {
            get
            {
                return ArcgisDataCheckParaSet;
            }
        }
        #endregion
    }

    #endregion

    #region 检查功能实现接口、类
    public interface IDataCheck : ICheckEvent
    {
        
        void OnCreate(IDataCheckHook hook);

        void OnDataCheck();
    }

    public interface ICheckEvent
    {
        event DataErrTreatHandle DataErrTreat;
        event ProgressChangeHandle ProgressShow;
       
    }

    public interface IDataCheckLogic : IDataCheck
    {
        void DataCheckLogic_DataErrTreat(object sender, DataErrTreatEvent e);
        void DataCheck_ProgressShow(object sender, ProgressChangeEvent e);
    }

    public interface IDataCheckRealize : IDataCheck
    {
        void DataCheckRealize_DataErrTreat(object sender, DataErrTreatEvent e);
         void DataCheck_ProgressShow(object sender, ProgressChangeEvent e);
    }
    #endregion
}
