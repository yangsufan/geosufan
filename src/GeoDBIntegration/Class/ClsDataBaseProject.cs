using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBIntegration
    
{
    //**********************************************************//
    /* guozheng 2011-5-16 added 
     *  作者：guozheng 2011-5-16
     *  修改：
     *  功能：该类定义了数据库工程类的基本内容，包括数据库的名称，类型，信息，连接信息XMl文本等
     *         之后可以实现数据库的相关操作，例如数据库的卸载等
     *  版本：V1.0.0
     */
    //**********************************************************//
    public class ClsDataBaseProject
    {
        private enumInterDBType m_DBType = enumInterDBType.框架要素数据库;///////////////数据库类型
        public enumInterDBType DBType
        {
            get { return this.m_DBType; }
            set { this.m_DBType = value; }
        }

        private enumInterDBFormat m_DBFormate = enumInterDBFormat.ARCGISSDE;/////////数据库平台
        public enumInterDBFormat DBFormate
        {
            get { return this.m_DBFormate; }
            set { this.m_DBFormate = value; }
        }
        //cyf 20110627 add:数据库类型从数据库中读出来
         private long m_DBTypeID = -1;       //数据库类型ID
        public long DBTypeID
        {
            get { return m_DBTypeID; }
            set { m_DBTypeID = value; }
        }

        private long m_DBFormatID = -1;       //数据库平台ID
        public long DBFormatID
        {
            get { return m_DBFormatID; }
            set { m_DBFormatID = value; }
        }

        private string m_DBTypeStr = "";       //数据库类型
        public string DBTypeStr
        {
            get { return m_DBTypeStr; }
            set { m_DBTypeStr = value; }
        }

        private string m_DBFormatStr = "";       //数据库平台
        public string DBFormatStr
        {
            get { return m_DBFormatStr; }
            set { m_DBFormatStr = value; }
        }
        //end

        private string m_sDBName = string.Empty;//////////////数据库名称
        public string sDbName
        {
            get { return this.m_sDBName; }
            set { this.m_sDBName = value; }
        }

        private XmlElement m_pDBInfoEle = null;//////////////数据库信息
        public XmlElement pDBInfoEle
        {
            get { return this.m_pDBInfoEle; }
            set { this.m_pDBInfoEle = value; }
        }

        private XmlElement m_pDBConnectInfoEle = null;///////数据库连接信息
        public XmlElement pDBConnectInfoEle
        {
            get { return this.m_pDBConnectInfoEle; }
            set { this.m_pDBConnectInfoEle = value; }
        }

        long m_lDBID = -1;
        public long lDBID
        {
            get { return this.m_lDBID; }
            set { this.m_lDBID = value; }
        }
        /// <summary>
        /// 构造函数  cyf 20110627 modify
        /// </summary>
        /// <param name="in_DbType">数据库类型</param>
        /// <param name="in_DbFormat">数据库平台</param>
        /// <param name="in_sDBName">数据库名称</param>
        /// <param name="in_DBinfoEle">数据库信息</param>
        /// <param name="in_DBConnectInFoEle">数据库连接信息</param>
        public ClsDataBaseProject(long in_lDBID, long in_DbTypeID, long in_DbFormatID,string in_DbType, string in_DbFormat, string in_sDBName, XmlElement in_DBinfoEle, XmlElement in_DBConnectInFoEle)
        {
            this.m_lDBID = in_lDBID;
            //cyf 20110627 add
            this.m_DBTypeID=in_DbTypeID;
            this.m_DBFormatID=in_DbFormatID;
            this.m_DBFormatStr = in_DbFormat;
            this.m_DBTypeStr = in_DbType;
            //end
            this.m_sDBName = in_sDBName;
            this.m_pDBInfoEle = in_DBinfoEle;
            this.m_pDBConnectInfoEle = in_DBConnectInFoEle;
        }
    }
}
