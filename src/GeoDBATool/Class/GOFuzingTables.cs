using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    /// <summary>
    /// 创建更新管理支撑表

    /// </summary>
    public class GOFuzingTables
    {
        protected IWorkspace _Workspace = null;
        protected IWorkspace _TempleteWorkspace = null;

        public GOFuzingTables()
        { }

        #region 构造函数获得工作空间连接

        /// <summary>
        /// 如果是SDE库体则获取工作空间

        /// </summary>
        /// <param name="propertySet">工作空间连接属性</param>
        public GOFuzingTables(IPropertySet propertySet)
        {
            IWorkspaceFactory pWorkspaceFactory = new SdeWorkspaceFactoryClass();
            this._Workspace = pWorkspaceFactory.Open(propertySet, 0);
            this._TempleteWorkspace = OpenTempleteWorkSpace();

        }

        /// <summary>
        /// 如果是文件库则需要创建辅助库体的pdb
        /// 重载构造函数

        /// </summary>
        /// <param name="MTpath">本地辅助库存放路径</param>
        public GOFuzingTables(string MTpath)
        {
            //创建PDB工作空间
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();

            FileInfo finfo = new FileInfo(MTpath);
            string outputDBPath = finfo.DirectoryName;
            string outputDBName = finfo.Name;

            if (finfo.Exists)
            {
                SysCommon.Error.frmInformation frmInfo = new SysCommon.Error.frmInformation("是", "否", "存在同名的辅助库，是否进行替代？");
                if (frmInfo.ShowDialog() == DialogResult.OK)
                {
                    finfo.Delete();
                }
                else
                {
                    return;
                }
            }

            //outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
            ESRI.ArcGIS.esriSystem.IName pName = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
            this._Workspace = (IWorkspace)pName.Open();
            this._TempleteWorkspace = OpenTempleteWorkSpace();
        }

        public void Dispose()
        {
            if (this._Workspace != null)
            {
                Marshal.ReleaseComObject(this._Workspace);
            }
            if (this._TempleteWorkspace != null)
            {
                Marshal.ReleaseComObject(this._TempleteWorkspace);
            }
        }

        /// <summary>
        /// 打开本地模板工作空间
        /// </summary>
        /// <returns>返回模板工作空间</returns>
        private IWorkspace OpenTempleteWorkSpace()
        {
            string PDBPath = Application.StartupPath + "\\..\\CacheDB\\TempleteDatabase.mdb";
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();

            return pWorkspaceFactory.OpenFromFile(PDBPath, 0);
        }
        #endregion


        /// <summary>
        /// 根据模板创建指定的要素类（从模板中复制要素类）

        /// </summary>
        /// <param name="FCName">要素类名称</param>
        /// <param name="IncludeFeature">是否导入源要素类中的要素</param>
        /// <returns>是否创建成功</returns>
        public bool CreateDefaultFeatureClass(string FCName, bool IncludeFeature)
        {
            IFeatureWorkspace pDesFeatureWorkspace = this._Workspace as IFeatureWorkspace;
            IFeatureClass pDesFeatureClass = null;

            if (pDesFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureWorkspace pSourFeatureWorkspace = this._TempleteWorkspace as IFeatureWorkspace;
            if (pSourFeatureWorkspace == null)
            {

                return false;
            }

            #region 创建目标图层
            //获取源要素类
            IFeatureClass pSourFeatureClass = pSourFeatureWorkspace.OpenFeatureClass(FCName);
            if (pSourFeatureClass == null)
            {

                return false;
            }

            //获取源要素类后判断要素类的类型

            if (pSourFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)   //如果是注记层
            {
                IFeatureWorkspaceAnno pFWSAnno = pDesFeatureWorkspace as IFeatureWorkspaceAnno;
                IAnnoClass pAnnoClass = pSourFeatureClass.Extension as IAnnoClass;
                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.ReferenceScale = pAnnoClass.ReferenceScale;
                pGLS.Units = pAnnoClass.ReferenceScaleUnits;

                pDesFeatureClass = pFWSAnno.CreateAnnotationClass(pSourFeatureClass.AliasName, pSourFeatureClass.Fields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", null, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
            }
            else    //如果是普通要素类
            {
                pDesFeatureClass = pDesFeatureWorkspace.CreateFeatureClass(FCName, pSourFeatureClass.Fields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
            }

            #endregion

            if (pDesFeatureClass == null)
            {

                return false;
            }

            //导入要素
            if (IncludeFeature)  //如果需要复制要素
            {
                Exception eError = null;

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                IFeatureCursor pFeatureCursor = pSourFeatureClass.Search(null, false);

                IFields pFields = pSourFeatureClass.Fields;

                for (int i = 0; i < pFields.FieldCount; i++)
                {

                    IField pField = pFields.get_Field(i);
                    if (pField.Editable)
                    {
                        pDic.Add(pField.Name, pField.Name);
                    }
                }

                SysCommon.Gis.SysGisDataSet pSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                pSysGisDataSet.WorkSpace = this._Workspace;

                pSysGisDataSet.NewFeatures(FCName, pFeatureCursor, pDic, null, true, true, out eError);

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            return true;
        }

        public bool CreateDefaultFeatureClass(string FCName, bool IncludeFeature, bool AddFID)
        {
            IFeatureWorkspace pDesFeatureWorkspace = this._Workspace as IFeatureWorkspace;
            IFeatureClass pDesFeatureClass = null;

            if (pDesFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureWorkspace pSourFeatureWorkspace = this._TempleteWorkspace as IFeatureWorkspace;
            if (pSourFeatureWorkspace == null)
            {

                return false;
            }

            #region 创建目标图层
            //获取源要素类
            IFeatureClass pSourFeatureClass = pSourFeatureWorkspace.OpenFeatureClass(FCName);
            if (pSourFeatureClass == null)
            {

                return false;
            }

            //获取源要素类后判断要素类的类型

            if (pSourFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)   //如果是注记层
            {
                IFeatureWorkspaceAnno pFWSAnno = pDesFeatureWorkspace as IFeatureWorkspaceAnno;
                IAnnoClass pAnnoClass = pSourFeatureClass.Extension as IAnnoClass;
                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.ReferenceScale = pAnnoClass.ReferenceScale;
                pGLS.Units = pAnnoClass.ReferenceScaleUnits;


                ///添加特殊字段
                ///
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;

                pFieldEdit.Editable_2 = true;
                pFieldEdit.Name_2 = "GOFID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

                IField pField2 = new FieldClass();
                IFieldEdit pFieldEdit2 = pField2 as IFieldEdit;

                pFieldEdit2.Editable_2 = true;
                pFieldEdit2.Name_2 = "pro_id";   //项目ID
                pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeInteger;

                IFields pFields = pSourFeatureClass.Fields;

                if (AddFID)
                {
                    IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                    pFieldsEdit.AddField(pField);
                    pFieldsEdit.AddField(pField2);
                    pDesFeatureClass = pFWSAnno.CreateAnnotationClass(pSourFeatureClass.AliasName, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", null, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
                    pFieldsEdit.DeleteField(pField);
                    pFieldsEdit.DeleteField(pField2);
                }
                else
                {
                    pDesFeatureClass = pFWSAnno.CreateAnnotationClass(pSourFeatureClass.AliasName, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", null, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
                }



            }
            else    //如果是普通要素类
            {

                ///添加特殊字段
                ///
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;

                pFieldEdit.Editable_2 = true;
                pFieldEdit.Name_2 = "GOFID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

                IField pField2 = new FieldClass();
                IFieldEdit pFieldEdit2 = pField2 as IFieldEdit;

                pFieldEdit2.Editable_2 = true;
                pFieldEdit2.Name_2 = "pro_id";   //项目ID
                pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeInteger;

                IFields pFields = pSourFeatureClass.Fields;

                if (AddFID)
                {
                    IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                    pFieldsEdit.AddField(pField);
                    pFieldsEdit.AddField(pField2);
                    pDesFeatureClass = pDesFeatureWorkspace.CreateFeatureClass(FCName, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
                    pFieldsEdit.DeleteField(pField);
                    pFieldsEdit.DeleteField(pField2);

                }
                else
                {
                    pDesFeatureClass = pDesFeatureWorkspace.CreateFeatureClass(FCName, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
                }


            }

            #endregion

            if (pDesFeatureClass == null)
            {

                return false;
            }

            //导入要素
            if (IncludeFeature)  //如果需要复制要素
            {
                Exception eError = null;

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                IFeatureCursor pFeatureCursor = pSourFeatureClass.Search(null, false);

                IFields pFields = pSourFeatureClass.Fields;

                for (int i = 0; i < pFields.FieldCount; i++)
                {

                    IField pField = pFields.get_Field(i);
                    if (pField.Editable)
                    {
                        pDic.Add(pField.Name, pField.Name);
                    }
                }

                SysCommon.Gis.SysGisDataSet pSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                pSysGisDataSet.WorkSpace = this._Workspace;

                pSysGisDataSet.NewFeatures(FCName, pFeatureCursor, pDic, null, true, true, out eError);

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            return true;
        }

        /// <summary>
        /// 创建默认的更新时使用的一系列表

        /// </summary>
        /// <param name="FIDIsAutoIncrease">标识FID记录表记录中的GOFID字段是否自动增值</param>
        ///<param name="commonTableCreate">标识是否创建除FID记录表以外的其他表</param>
        /// <returns></returns>
        public bool CreateDefaultTables(bool FIDIsAutoIncrease, bool commonTableCreate)
        {
            Exception eError = null;
            IWorkspace pWorkspace = this._Workspace;
            try
            {
                if (pWorkspace.Type == esriWorkspaceType.esriLocalDatabaseWorkspace)   //如果是本地文件库
                {
                    if (commonTableCreate)
                    {
                        //创建更新任务表

                        pWorkspace.ExecuteSQL("create table ICE_CASE (CASE_ID varchar(50),U_ID integer,G_ID integer,PRO_ID integer,CREATEDATE date,ENDDATE date,REMARK varchar(255))");

                        //创建任务关联表

                        pWorkspace.ExecuteSQL("create table ICE_CASEJOIN (JOIN_GROUP text,U_ID integer,G_ID integer,PRO_ID integer,CREATEDATE date,ENDDATE date,C_REMARK varchar(255))");

                        //创建跨区要素表

                        pWorkspace.ExecuteSQL("create table ICE_CORSSFEATURE (GOFID integer primary key,CASE_ID varchar(50),PRO_ID integer)");

                        //创建数据源表
                        pWorkspace.ExecuteSQL("create table ICE_DATASOURCE (MAP_ID varchar(50),CASE_NAME varchar(50),PRO_NAME varchar(50),USER_TYPE varchar(100),CREATEDATE date,ENDDATE date,REMARK varchar(200))");

                        //创建工程信息表

                        pWorkspace.ExecuteSQL("create table ICE_PROJECTINFO (PRO_ID AUTOINCREMENT primary key,PRO_NAME varchar(50),MASTER_ID integer,PROINFO Memo,CREATEDATE date,ENDDATE date,P_REMARK varchar(200),STATE integer)");

                        //创建更新地图信息表

                        pWorkspace.ExecuteSQL("create table ICE_UPDATEMAPINFO (MAP_ID varchar(20),CASE_ID varchar(50),PRO_ID integer,STATE varchar(50),U_NAME varchar(50))");

                        //创建用户组信息表
                        pWorkspace.ExecuteSQL("create table ICE_USERGROUPINFO (G_ID AUTOINCREMENT primary key,G_NAME varchar(50),G_TYPE varchar(50),G_PURVIEW OLEObject,G_REMARK varchar(200))");

                        //创建用户组关系表
                        pWorkspace.ExecuteSQL("create table ICE_USERGROUPRELATION (U_ID integer,G_ID integer)");

                        //创建用户信息表

                        pWorkspace.ExecuteSQL("create table ICE_USERINFO (U_ID AUTOINCREMENT primary key,U_NAME varchar(50),U_PWD varchar(50),U_SEX integer,U_JOB varchar(50),U_REMARK varchar(200),LOGININFO Memo)");

                        //创建更新日志表

                        pWorkspace.ExecuteSQL("create table UPDATELOG (GOFID integer,STATE integer,OID integer,EID integer,STATUS integer,SAVE integer,NEW integer,LAYERNAME varchar(50),LASTUPDATE varchar(50),PROJECTID integer,CASEID varchar(50))");

                        //创建DID与FID关系表

                        pWorkspace.ExecuteSQL("create table DID与FID关系表 (DID integer,FID integer)");

                        pWorkspace.ExecuteSQL("create table UPDATERES (PRO_ID integer,FCNAME varchar(50),OID integer)");
                    }

                    if (FIDIsAutoIncrease)
                    {    //创建FID记录表

                        pWorkspace.ExecuteSQL("create table FID记录表 (GOFID AUTOINCREMENT,FCNAME varchar(50),OID integer)");
                    }
                    else
                    {
                        //创建FID记录表

                        pWorkspace.ExecuteSQL("create table FID记录表 (GOFID integer,FCNAME varchar(50),OID integer)");
                    }


                }
                else     //如果是SDE
                {

                    //首先删除已有的表对象
                    //pWorkspace.ExecuteSQL("drop table ICE_CASE");
                    //pWorkspace.ExecuteSQL("drop table ICE_CASEJOIN");
                    //pWorkspace.ExecuteSQL("drop table ICE_CORSSFEATURE");

                    //pWorkspace.ExecuteSQL("drop trigger ICE_PROJECTINFO_TG");
                    //pWorkspace.ExecuteSQL("drop sequence ICE_PROJECTINFO_SEQ");
                    //pWorkspace.ExecuteSQL("drop table ICE_PROJECTINFO");
                    //pWorkspace.ExecuteSQL("drop table ICE_UPDATEMAPINFO");
                    //pWorkspace.ExecuteSQL("drop table ICE_USERGROUPINFO");
                    //pWorkspace.ExecuteSQL("drop table ICE_USERGROUPRELATION");

                    //pWorkspace.ExecuteSQL("drop trigger ICE_USERINFO_TG");
                    //pWorkspace.ExecuteSQL("drop sequence ICE_USERINFO_SEQ");
                    //pWorkspace.ExecuteSQL("drop table ICE_USERINFO");
                    //pWorkspace.ExecuteSQL("drop table UPDATELOG");

                    //pWorkspace.ExecuteSQL("drop trigger FID记录表_TG");
                    //pWorkspace.ExecuteSQL("drop sequence FID记录表_SEQ");
                    //pWorkspace.ExecuteSQL("drop table FID记录表");
                    //pWorkspace.ExecuteSQL("drop table DID与FID关系表");

                    if (commonTableCreate)
                    {

                        //创建更新任务表

                        pWorkspace.ExecuteSQL("create table ICE_CASE(CASE_ID    NVARCHAR2(50),U_ID       INTEGER,G_ID       INTEGER,PRO_ID     INTEGER,CREATEDATE DATE,ENDDATE    DATE,REMARK     NVARCHAR2(255))");

                        //创建任务关联表

                        pWorkspace.ExecuteSQL("create table ICE_CASEJOIN(JOIN_GROUP NVARCHAR2(500),U_ID       INTEGER,G_ID       INTEGER,PRO_ID     INTEGER,CREATEDATE DATE,ENDDATE    DATE,C_REMARK   NVARCHAR2(255))");

                        //创建跨区要素表

                        pWorkspace.ExecuteSQL("create table ICE_CORSSFEATURE(GOFID    INTEGER,CASE_ID  NVARCHAR2(50),PRO_ID   INTEGER)");

                        //创建数据源表
                        pWorkspace.ExecuteSQL("create table ICE_DATASOURCE(MAP_ID     NVARCHAR2(20),CASE_NAME  NVARCHAR2(50),PRO_NAME   NVARCHAR2(50),USER_TYPE  NVARCHAR2(100),CREATEDATE DATE,ENDDATE    DATE,REMARK     NVARCHAR2(200))");

                        //创建工程信息表//需要创建触发器
                        pWorkspace.ExecuteSQL("create table ICE_PROJECTINFO(PRO_ID     INTEGER not null,PRO_NAME   NVARCHAR2(50),MASTER_ID  INTEGER,PROINFO    NVARCHAR2(500),CREATEDATE DATE,ENDDATE    DATE,P_REMARK   NVARCHAR2(200))");
                        pWorkspace.ExecuteSQL("create sequence ICE_PROJECTINFO_SEQ minvalue 1 maxvalue 9999999999 start with 1 increment by 1 cache 30 order");
                        pWorkspace.ExecuteSQL("create or replace trigger ICE_PROJECTINFO_TG before insert on ICE_PROJECTINFO for each row begin if :NEW.PRO_ID=0 or :NEW.PRO_ID is null then SELECT ICE_PROJECTINFO_SEQ.NEXTVAL   INTO   :NEW.PRO_ID   FROM   DUAL; end if; end;");

                        //创建更新地图信息表

                        pWorkspace.ExecuteSQL("create table ICE_UPDATEMAPINFO(MAP_ID   NVARCHAR2(20),CASE_ID  NVARCHAR2(50),PRO_ID   INTEGER,STATE    NVARCHAR2(50),U_NAME   NVARCHAR2(50))");

                        //创建用户组信息表//需要创建触发器
                        pWorkspace.ExecuteSQL("create table ICE_USERGROUPINFO(G_ID      INTEGER not null,G_NAME    NVARCHAR2(50),G_TYPE    NVARCHAR2(50),G_PURVIEW BLOB,G_REMARK  NVARCHAR2(200))");
                        pWorkspace.ExecuteSQL("create sequence ICE_USERGROUPINFO_SEQ minvalue 1 maxvalue 9999999999 start with 1 increment by 1 cache 30 order");
                        pWorkspace.ExecuteSQL("create or replace trigger ICE_USERGROUPINFO_TG before insert on  ICE_USERGROUPINFO for each row begin if :NEW.G_ID=0 or :NEW.G_ID is null then SELECT  ICE_USERGROUPINFO_SEQ.NEXTVAL   INTO   :NEW.G_ID   FROM   DUAL; end if; end;");

                        //创建用户组关系表
                        pWorkspace.ExecuteSQL("create table ICE_USERGROUPRELATION(U_ID     INTEGER,G_ID     INTEGER)");

                        //创建用户信息表//需要创建触发器
                        pWorkspace.ExecuteSQL("create table ICE_USERINFO(U_ID      INTEGER not null,U_NAME    NVARCHAR2(50),U_PWD     NVARCHAR2(50),U_SEX     INTEGER,U_JOB     NVARCHAR2(50),U_REMARK  NVARCHAR2(200),LOGININFO NVARCHAR2(500))");
                        pWorkspace.ExecuteSQL("create sequence ICE_USERINFO_SEQ minvalue 1 maxvalue 9999999999 start with 1 increment by 1 cache 30 order");
                        pWorkspace.ExecuteSQL("create or replace trigger ICE_USERINFO_TG before insert on ICE_USERINFO for each row begin if :NEW.U_ID=0 or :NEW.U_ID is null then SELECT ICE_USERINFO_SEQ.NEXTVAL   INTO   :NEW.U_ID   FROM   DUAL; end if; end;");

                        //创建更新日志表

                        pWorkspace.ExecuteSQL("create table UPDATELOG(GOFID      INTEGER,STATE      INTEGER,OID        INTEGER,EID        INTEGER,STATUS     INTEGER,SAVE       INTEGER,NEW        INTEGER,LAYERNAME  NVARCHAR2(50),LASTUPDATE NVARCHAR2(50),PROJECTID  INTEGER,CASEID     NVARCHAR2(50))");

                        //创建DID与FID关系表

                        pWorkspace.ExecuteSQL("create table DID与FID关系表(DID      INTEGER,FID      INTEGER)");

                        pWorkspace.ExecuteSQL("create table UPDATERES(PRO_ID INTEGER,FCNAME NVARCHAR2(50),OID INTEGER)");
                    }

                    if (FIDIsAutoIncrease)
                    {   //创建FID记录表//创建触发器

                        pWorkspace.ExecuteSQL("create table FID记录表(GOFID  INTEGER not null,FCNAME NVARCHAR2(50),OID    INTEGER)");
                        pWorkspace.ExecuteSQL("create sequence FID记录表_SEQ minvalue 1 maxvalue 9999999999 start with 1 increment by 1 cache 30 order");
                        pWorkspace.ExecuteSQL("create or replace trigger FID记录表_TG before insert on FID记录表 for each row begin if :NEW.GOFID=0 or :NEW.GOFID is null then SELECT FID记录表_SEQ.NEXTVAL   INTO   :NEW.GOFID   FROM   DUAL; end if; end;");
                    }
                    else
                    {
                        pWorkspace.ExecuteSQL("create table FID记录表(GOFID  INTEGER,FCNAME NVARCHAR2(50),OID    INTEGER)");
                    }
                }
            }
            catch (Exception ex)
            {
                //*******************************************************************
                //Exception Log
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************
                eError = ex;

            }

            pWorkspace = null;

            if (eError != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class GOFuzingSpatialTables : GOFuzingTables
    {
        /// <summary>
        /// 目标库体类型
        /// </summary>
        private string _DesDBType = null;
        private IPropertySet _PropertySet = null;

        public IPropertySet PropertySet
        {
            get { return _PropertySet; }
            set { _PropertySet = value; }
        }

        public string DesDBType
        {
            get { return _DesDBType; }
            set { _DesDBType = value; }
        }

        /// <summary>
        /// 构造函数

        /// </summary>
        /// <param name="propertySet">保存的连接设置</param>
        /// <param name="DBType">目标的类型（PDB、SDE、GDB）</param>
        /// <param name="pDesProperty">目标的连接设置</param>
        public GOFuzingSpatialTables(IPropertySet propertySet, string DBType, IPropertySet pDesProperty)
        {
            this._DesDBType = DBType;
            this._PropertySet = pDesProperty;
            IWorkspaceFactory pWorkspaceFactory = new SdeWorkspaceFactoryClass();
            this._Workspace = pWorkspaceFactory.Open(propertySet, 0);
            this._TempleteWorkspace = OpenTempleteWorkSpace();
        }

        /// <summary>
        /// 构造函数

        /// </summary>
        /// <param name="MTpath">保存的连接设置</param>
        /// <param name="DBType">目标的类型（PDB、SDE、GDB）</param>
        /// <param name="pDesProperty">目标的连接设置</param>
        public GOFuzingSpatialTables(string MTpath, string DBType, IPropertySet pDesProperty)
        {
            this._DesDBType = DBType;
            this._PropertySet = pDesProperty;
            IWorkspaceFactory pWorkspaceFactory = null;
            //if (DBType == "PDB")
            //{
            //    //创建PDB工作空间
            //    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            //}
            //else if (DBType == "GDB")
            //{
            pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
            //}

            FileInfo finfo = new FileInfo(MTpath);
            string outputDBPath = finfo.DirectoryName;
            string outputDBName = finfo.Name;
            if (Directory.Exists(MTpath))
            {
                SysCommon.Error.frmInformation frmInfo = new SysCommon.Error.frmInformation("是", "否", "存在同名的工作库实例，是否进行替代？");
                if (frmInfo.ShowDialog() == DialogResult.OK)
                {
                    Directory.Delete(MTpath, true);
                }
                else
                {
                    return;
                }
            }

            //outputDBName = outputDBName.Substring(0, outputDBName.Length - 4);
            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
            ESRI.ArcGIS.esriSystem.IName pName = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
            this._Workspace = (IWorkspace)pName.Open();
            this._TempleteWorkspace = OpenTempleteWorkSpace();
        }

        /// <summary>
        /// 打开更新目标工作空间
        /// </summary>
        /// <returns>返回模板工作空间</returns>
        private IWorkspace OpenTempleteWorkSpace()
        {
            if (this._DesDBType == "PDB")
            {
                IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                return pWorkspaceFactory.Open(this._PropertySet, 0);
            }
            else if (this._DesDBType == "GDB")
            {
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                return pWorkspaceFactory.Open(this._PropertySet, 0);
            }
            else if (this._DesDBType == "SDE")
            {
                IWorkspaceFactory pWorkspaceFactory = new SdeWorkspaceFactoryClass();
                return pWorkspaceFactory.Open(this._PropertySet, 0);
            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// 在指定数据集下创建要素类
        /// </summary>
        /// <param name="FCName"></param>
        /// <param name="DSName"></param>
        /// <param name="IncludeFeature"></param>
        /// <returns></returns>
        public bool CreateFeatureClassUnderDS(string FCName, string DSName, bool IncludeFeature, bool CreateDS)
        {
            IFeatureWorkspace pDesFeatureWorkspace = this._Workspace as IFeatureWorkspace;
            IFeatureClass pDesFeatureClass = null;

            if (pDesFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureWorkspace pSourFeatureWorkspace = this._TempleteWorkspace as IFeatureWorkspace;
            if (pSourFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureDataset pDesFeatureDataset = null;
            IFeatureDataset pSrcFeatureDataset = null;
            if (CreateDS)
            {
                //创建数据集

                pSrcFeatureDataset = pSourFeatureWorkspace.OpenFeatureDataset(DSName);
                IGeoDataset pGeoDataset = pSrcFeatureDataset as IGeoDataset;
                pDesFeatureDataset = pDesFeatureWorkspace.CreateFeatureDataset(DSName, pGeoDataset.SpatialReference);
            }
            else
            {
                pDesFeatureDataset = pDesFeatureWorkspace.OpenFeatureDataset(DSName);
            }

            #region 创建目标图层
            //获取源要素类
            IFeatureClass pSourFeatureClass = pSourFeatureWorkspace.OpenFeatureClass(FCName);
            if (pSourFeatureClass == null)
            {

                return false;
            }

            //获取源要素类后判断要素类的类型

            if (pSourFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)   //如果是注记层
            {
                IFeatureWorkspaceAnno pFWSAnno = pDesFeatureWorkspace as IFeatureWorkspaceAnno;
                IAnnoClass pAnnoClass = pSourFeatureClass.Extension as IAnnoClass;
                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.ReferenceScale = pAnnoClass.ReferenceScale;
                pGLS.Units = pAnnoClass.ReferenceScaleUnits;

                pDesFeatureClass = pFWSAnno.CreateAnnotationClass(pSourFeatureClass.AliasName, pSourFeatureClass.Fields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", pDesFeatureDataset, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
            }
            else    //如果是普通要素类
            {
                pDesFeatureClass = pDesFeatureDataset.CreateFeatureClass(FCName, pSourFeatureClass.Fields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
            }

            #endregion

            if (pDesFeatureClass == null)
            {

                return false;
            }

            //导入要素
            if (IncludeFeature)  //如果需要复制要素
            {
                Exception eError = null;

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                IFeatureCursor pFeatureCursor = pSourFeatureClass.Search(null, false);

                IFields pFields = pSourFeatureClass.Fields;

                for (int i = 0; i < pFields.FieldCount; i++)
                {

                    IField pField = pFields.get_Field(i);
                    if (pField.Editable)
                    {
                        pDic.Add(pField.Name, pField.Name);
                    }
                }

                SysCommon.Gis.SysGisDataSet pSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                pSysGisDataSet.WorkSpace = this._Workspace;

                pSysGisDataSet.NewFeatures(FCName, pFeatureCursor, pDic, null, true, true, out eError);

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            return true;
        }

        public bool CreateFeatureClassUnderDS(string FCName, string DSName, bool IncludeFeature, bool CreateDS, string TempDBID)
        {
            IFeatureWorkspace pDesFeatureWorkspace = this._Workspace as IFeatureWorkspace;
            IFeatureClass pDesFeatureClass = null;

            if (pDesFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureWorkspace pSourFeatureWorkspace = this._TempleteWorkspace as IFeatureWorkspace;
            if (pSourFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureDataset pDesFeatureDataset = null;
            IFeatureDataset pSrcFeatureDataset = null;
            if (CreateDS)
            {
                //创建数据集

                pSrcFeatureDataset = pSourFeatureWorkspace.OpenFeatureDataset(DSName);
                IGeoDataset pGeoDataset = pSrcFeatureDataset as IGeoDataset;
                pDesFeatureDataset = pDesFeatureWorkspace.CreateFeatureDataset(DSName + TempDBID, pGeoDataset.SpatialReference);
            }
            else
            {
                pDesFeatureDataset = pDesFeatureWorkspace.OpenFeatureDataset(DSName + TempDBID);
            }

            #region 创建目标图层
            //获取源要素类
            IFeatureClass pSourFeatureClass = pSourFeatureWorkspace.OpenFeatureClass(FCName);
            if (pSourFeatureClass == null)
            {

                return false;
            }

            //获取源要素类后判断要素类的类型

            if (pSourFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)   //如果是注记层
            {
                IFeatureWorkspaceAnno pFWSAnno = pDesFeatureWorkspace as IFeatureWorkspaceAnno;
                IAnnoClass pAnnoClass = pSourFeatureClass.Extension as IAnnoClass;
                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.ReferenceScale = pAnnoClass.ReferenceScale;
                pGLS.Units = pAnnoClass.ReferenceScaleUnits;

                ///添加特殊字段
                ///
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;

                pFieldEdit.Editable_2 = true;
                pFieldEdit.Name_2 = "GOFID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

                IFields pFields = pSourFeatureClass.Fields;
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                pFieldsEdit.AddField(pField);

                pDesFeatureClass = pFWSAnno.CreateAnnotationClass(FCName + TempDBID, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", pDesFeatureDataset, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
                pFieldsEdit.DeleteField(pField);

            }
            else    //如果是普通要素类
            {
                ///添加特殊字段
                ///
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;

                pFieldEdit.Editable_2 = true;
                pFieldEdit.Name_2 = "GOFID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;

                IFields pFields = pSourFeatureClass.Fields;
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                pFieldsEdit.AddField(pField);


                pDesFeatureClass = pDesFeatureDataset.CreateFeatureClass(FCName + TempDBID, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
                pFieldsEdit.DeleteField(pField);

            }

            #endregion

            if (pDesFeatureClass == null)
            {

                return false;
            }

            //导入要素
            if (IncludeFeature)  //如果需要复制要素
            {
                Exception eError = null;

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                IFeatureCursor pFeatureCursor = pSourFeatureClass.Search(null, false);

                IFields pFields = pSourFeatureClass.Fields;

                for (int i = 0; i < pFields.FieldCount; i++)
                {

                    IField pField = pFields.get_Field(i);
                    if (pField.Editable)
                    {
                        pDic.Add(pField.Name, pField.Name);
                    }
                }

                SysCommon.Gis.SysGisDataSet pSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                pSysGisDataSet.WorkSpace = this._Workspace;

                pSysGisDataSet.NewFeatures(FCName, pFeatureCursor, pDic, null, true, true, out eError);

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            return true;
        }

        public bool CreateFeatureClassUnderDS(string FCName, string DSName, bool IncludeFeature, bool CreateDS, string TempDBID, bool AddFID)
        {
            IFeatureWorkspace pDesFeatureWorkspace = this._Workspace as IFeatureWorkspace;
            IFeatureClass pDesFeatureClass = null;

            if (pDesFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureWorkspace pSourFeatureWorkspace = this._TempleteWorkspace as IFeatureWorkspace;
            if (pSourFeatureWorkspace == null)
            {

                return false;
            }

            IFeatureDataset pDesFeatureDataset = null;
            IFeatureDataset pSrcFeatureDataset = null;
            if (CreateDS)
            {
                //创建数据集

                pSrcFeatureDataset = pSourFeatureWorkspace.OpenFeatureDataset(DSName);
                IGeoDataset pGeoDataset = pSrcFeatureDataset as IGeoDataset;
                pDesFeatureDataset = pDesFeatureWorkspace.CreateFeatureDataset(DSName + TempDBID, pGeoDataset.SpatialReference);
            }
            else
            {
                pDesFeatureDataset = pDesFeatureWorkspace.OpenFeatureDataset(DSName + TempDBID);
            }

            #region 创建目标图层
            //获取源要素类
            IFeatureClass pSourFeatureClass = pSourFeatureWorkspace.OpenFeatureClass(FCName);
            if (pSourFeatureClass == null)
            {

                return false;
            }

            //获取源要素类后判断要素类的类型

            if (pSourFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)   //如果是注记层
            {
                IFeatureWorkspaceAnno pFWSAnno = pDesFeatureWorkspace as IFeatureWorkspaceAnno;
                IAnnoClass pAnnoClass = pSourFeatureClass.Extension as IAnnoClass;
                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.ReferenceScale = pAnnoClass.ReferenceScale;
                pGLS.Units = pAnnoClass.ReferenceScaleUnits;

                ///添加特殊字段
                ///
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;

                pFieldEdit.Editable_2 = true;
                pFieldEdit.Name_2 = "GOFID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;



                IField pField2 = new FieldClass();
                IFieldEdit pFieldEdit2 = pField2 as IFieldEdit;

                pFieldEdit2.Editable_2 = true;
                pFieldEdit2.Name_2 = "pro_id";   //项目ID
                pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeInteger;

                IFields pFields = pSourFeatureClass.Fields;

                if (AddFID)
                {

                    IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                    pFieldsEdit.AddField(pField);
                    pFieldsEdit.AddField(pField2);
                    pDesFeatureClass = pFWSAnno.CreateAnnotationClass(FCName + TempDBID, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", pDesFeatureDataset, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
                    pFieldsEdit.DeleteField(pField);
                    pFieldsEdit.DeleteField(pField2);

                }
                else
                {
                    pDesFeatureClass = pFWSAnno.CreateAnnotationClass(FCName + TempDBID, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.ShapeFieldName, "", pDesFeatureDataset, null, pAnnoClass.AnnoProperties, pGLS, pAnnoClass.SymbolCollection, true);
                }

            }
            else    //如果是普通要素类
            {
                ///添加特殊字段
                ///
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = pField as IFieldEdit;

                pFieldEdit.Editable_2 = true;
                pFieldEdit.Name_2 = "GOFID";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger;


                IField pField2 = new FieldClass();
                IFieldEdit pFieldEdit2 = pField2 as IFieldEdit;

                pFieldEdit2.Editable_2 = true;
                pFieldEdit2.Name_2 = "pro_id";   //项目ID
                pFieldEdit2.Type_2 = esriFieldType.esriFieldTypeInteger;

                IFields pFields = pSourFeatureClass.Fields;

                if (AddFID)
                {
                    IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                    pFieldsEdit.AddField(pField);
                    pFieldsEdit.AddField(pField2);
                    pDesFeatureClass = pDesFeatureDataset.CreateFeatureClass(FCName + TempDBID, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
                    pFieldsEdit.DeleteField(pField);
                    pFieldsEdit.DeleteField(pField2);
                }
                else
                {
                    pDesFeatureClass = pDesFeatureDataset.CreateFeatureClass(FCName + TempDBID, pFields, pSourFeatureClass.CLSID, pSourFeatureClass.EXTCLSID, pSourFeatureClass.FeatureType, pSourFeatureClass.ShapeFieldName, "");
                }


            }

            #endregion

            if (pDesFeatureClass == null)
            {

                return false;
            }

            //导入要素
            if (IncludeFeature)  //如果需要复制要素
            {
                Exception eError = null;

                Dictionary<string, string> pDic = new Dictionary<string, string>();
                IFeatureCursor pFeatureCursor = pSourFeatureClass.Search(null, false);

                IFields pFields = pSourFeatureClass.Fields;

                for (int i = 0; i < pFields.FieldCount; i++)
                {

                    IField pField = pFields.get_Field(i);
                    if (pField.Editable)
                    {
                        pDic.Add(pField.Name, pField.Name);
                    }
                }

                SysCommon.Gis.SysGisDataSet pSysGisDataSet = new SysCommon.Gis.SysGisDataSet();
                pSysGisDataSet.WorkSpace = this._Workspace;

                pSysGisDataSet.NewFeatures(FCName, pFeatureCursor, pDic, null, true, true, out eError);

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            }
            return true;
        }
    }
}
