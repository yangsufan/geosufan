using System;
using System.Collections.Generic;
using System.Text;
using SCHEMEMANAGERCLASSESLib;
using System.Data.OracleClient;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace GeoDBIntegration
{
    class ClsOracleSpatialDBCreater
    {
        /*
         * guozheng added 2010-5-...
         * 该类实现OracleSpatial空间库的库体创建
         * 需要获取Oracle 的服务名，用户名，密码以及Geoone库体配置方案文件
         * 
         */
        private string _Server = string.Empty;///////Oracle服务名
        public string Server
        {
            get { return this._Server; }
            set { this._Server = value; }
        }
        private string _User = string.Empty;////////Oracle用户名
        public string Usr
        {
            get { return this._User; }
            set { this._User = value; }
        }
        private string _Password = string.Empty;//////Oracle用户密码
        public string Password
        {
            get { return this._Password; }
            set { this._Password = value; }
        }

        private ClsDatabase m_DataBaseOper = null;////////数据库操作对象

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ServerName">Oracle服务名</param>
        /// <param name="UserID">Oracle用户名</param>
        /// <param name="Passw">Oracle用户密码</param>
        public ClsOracleSpatialDBCreater(string ServerName, string UserID, string Passw)
        {
            this._Password = Passw;
            this._Server = ServerName;
            this._User = UserID;
            OracleConnectionStringBuilder constrbuild = new OracleConnectionStringBuilder();
            constrbuild.DataSource = ServerName;
            constrbuild.UserID = UserID;
            constrbuild.Password = Passw;
            /////////////实例化数据库操作对象
            m_DataBaseOper = new ClsDatabase(constrbuild.ConnectionString);
        }
        /// <summary>
        /// 创建库体建构
        /// </summary>
        /// <param name="DbTemplatePath">库体配置方案模板路径</param>
        /// <param name="IsHisDb">是否为历史库体</param>
        /// <param name="ex"></param>
        public void CreateDataBase(string DbTemplatePath, bool IsHisDb, out Exception ex)
        {
            FrmProcessBar ProcFrm = new FrmProcessBar();
            ProcFrm.Show();
            ex = null;
            //***********************************************
            //guozheng added 保存表名和建表的SQL提供用户选择
            Dictionary<string, string> TableDic = new Dictionary<string, string>();
            //***********************************************
            ISchemeProject m_pProject = null;
            int m_DBScale = -1;/////比例尺信息
            ProcFrm.SetFrmProcessBarText("正在加载配置方案");
            #region 加载配置方案
            List<string> ltablename = new List<string>();
            try
            {
                m_pProject = new SchemeProjectClass();     //创建实例
                int index = DbTemplatePath.LastIndexOf('.');
                string lastName = DbTemplatePath.Substring(index + 1);
                if (lastName == "mdb")
                {
                    m_pProject.Load(DbTemplatePath, e_FileType.GO_SCHEMEFILETYPE_MDB);    //加载schema文件
                }
                else if (lastName == "gosch")
                {
                    m_pProject.Load(DbTemplatePath, e_FileType.GO_SCHEMEFILETYPE_GOSCH);    //加载schema文件
                }
                else
                {
                    ex = new Exception("数据库配置方案文件格式不规范，请检查");
                    ProcFrm.Close();
                    return;
                }

                ///如果加载成功则获取比例尺返回true，否则返回false
                if (m_pProject != null)
                {
                    string DBScale = m_pProject.get_MetaDataValue("Scale") as string;   //获取比例尺信息（总工程中的默认比例尺）
                    string[] DBScaleArayy = DBScale.Split(':');
                    m_DBScale = Convert.ToInt32(DBScaleArayy[1]);

                }
                else
                {
                    ex = new Exception("加载数据库配置方案文件：" + DbTemplatePath + "失败！");
                    ProcFrm.Close();
                    return;
                }
            }
            catch
            {
                ex = new Exception("加载数据库配置方案文件：" + DbTemplatePath + "失败！");
                ProcFrm.Close();
                return;
            }
            #endregion
            List<string> DataSpace = new List<string>();
            string sDataBaseName = string.Empty;
            string sNow = DateTime.Now.ToLongDateString();
            #region 获取字段信息
            try
            {
                IChildItemList pProjects = m_pProject as IChildItemList;
                //获取属性库集合信息
                ISchemeItem pDBList = pProjects.get_ItemByName("ATTRDB");
                IChildItemList pDBLists = pDBList as IChildItemList;
                //遍历属性库集合
                long DBNum = pDBLists.GetCount();
                for (int i = 0; i < DBNum; i++)
                {
                    int m_DSScale = 0;    //比例尺信息
                    #region 获取比例尺
                    //取得属性库信息
                    ISchemeItem pDB = pDBLists.get_ItemByIndex(i);
                    ///获取数据集的比例尺信息，如果获取失败则，取默认比例尺信息
                    IAttribute pa = pDB.AttributeList.get_AttributeByName("Scale") as IAttribute;
                    if (pa == null)
                    {
                        m_DSScale = m_DBScale;
                    }
                    else
                    {
                        string[] DBScaleArayy = pa.Value.ToString().Split(':');
                        m_DSScale = Convert.ToInt32(DBScaleArayy[1]);
                    }
                    #endregion
                    IChildItemList pDBs = pDB as IChildItemList;
                    string pDatasetName = pDB.Name;
                    DataSpace.Add(pDatasetName);
                    sDataBaseName = pDatasetName;
                    //////////////////////////////////////创建库/////////////////////
                    //创建地物类集合
                    //遍历属性表
                    long TabNum = pDBs.GetCount();
                    ProcFrm.SetFrmProcessBarMax(TabNum);
                    for (int j = 0; j < TabNum; j++)
                    {
                        //获取属性表信息
                        ISchemeItem pTable = pDBs.get_ItemByIndex(j);  //获取属性表对象
                        ProcFrm.SetFrmProcessBarValue(j);
                        ProcFrm.SetFrmProcessBarText("正在获取属性表");

                        string pFeatureClassName = pTable.Name;     //要素类名称
                        string pFeatureClassType = pTable.Value as string;   //要素类类型
                        string sTableName = pFeatureClassName;
                        string sTableType = pFeatureClassType;
                        //获得地物类的类型
                        string sField = string.Empty;
                        string sViewField = string.Empty;
                        sField += ModuleData.s_KeyFieldName + " NUMBER PRIMARY KEY,";
                        sViewField += ModuleData.s_KeyFieldName + ",";
                        ///////几何字段
                        sField = sField + ModuleData.s_GeometryFieldName + "  " + "MDSYS.SDO_GEOMETRY,";

                        if (pFeatureClassType == "ANNO")///////注记层不予处理
                            continue;

                        //遍历字段
                        IAttributeList pAttrs = pTable.AttributeList;
                        long FNum = pAttrs.GetCount();
                        int lfldcnt = pAttrs.GetCount();
                        int n = 0;
                        for (n = 0; n < lfldcnt; n++)
                        {
                            IAttribute pAttr = pAttrs.get_AttributeByIndex(n);
                            //获取扩展属性信息
                            IAttributeDes pAttrDes = pAttr.Description;
                            //以下变量用来定义字段的属性
                            string fieldName = pAttr.Name;      //记录字段名称
                            string fieldType = pAttr.Type.ToString();   //记录字段类型
                            int fieldLen = Convert.ToInt32(pAttrDes.InputWidth);     //记录字段长度
                            bool isNullable = pAttrDes.AllowNull;   //记录字段是否允许空值                
                            if (fieldLen <= 0)
                                fieldLen = 30;
                            int precision = Convert.ToInt32(pAttrDes.PrecisionEx);        //精度
                            bool required = bool.Parse(pAttrDes.Necessary.ToString());
                            ////////////////记录字段用于创建表///////////////////                            
                            string sFildType = string.Empty;
                            switch (fieldType)
                            {
                                case "GO_VALUETYPE_STRING":
                                    sFildType = "VARCHAR2(" + fieldLen.ToString() + ")";
                                    break;
                                case "GO_VALUETYPE_LONG":
                                    sFildType = "NUMBER";
                                    break;
                                case "GO_VALUETYPE_DATE":
                                    sFildType = "DATE";
                                    break;
                                case "GO_VALUETYPE_FLOAT":
                                    sFildType = "FLOAT";
                                    break;
                                case "GO_VALUETYPE_DOUBLE":
                                    sFildType = "NUMBER";
                                    break;
                                case "GO_VALUETYPE_BYTE":
                                    sFildType = "BLOB";
                                    break;
                                case "GO_VALUETYPE_BOOL":
                                    sFildType = "CHAR";
                                    break;
                                default:
                                    continue;
                                    break;
                            }

                            if (!string.IsNullOrEmpty(sFildType))
                            {
                                if (fieldType == "GO_VALUETYPE_BOOL")
                                {
                                    sFildType += " CHECK (" + fieldName + " IN('N','Y'))";
                                }
                                else
                                {
                                    //************************************
                                    //guozheng 2010-12-8 added 增加非空判断
                                    if (!isNullable)
                                        sFildType += " NOT NULL";
                                    //************************************
                                }
                                sField = sField + " " + fieldName + "  " + sFildType + ",";
                                sViewField += fieldName + ",";
                            }
                            else
                            {
                                continue;
                            }
                        }

                        string sMaxvalue = DateTime.MaxValue.ToLongDateString();
                        ///////////////////////////////创建表/////////////sTableName,sField
                        ProcFrm.SetFrmProcessBarText("正在组织表字段");
                        if (IsHisDb)/////若是历史库表后增加后缀
                        {
                            sTableName = sTableName.Trim() + "_GOH";///////增加后缀
                            ///////增加字段
                            sField = sField + "FromDate" + "  " + "VARCHAR2(30)" + " " + "DEFAULT('" + sNow + "'),";
                            sField = sField + "ToDate" + "  " + "VARCHAR2(30)" + " " + "DEFAULT('" + sMaxvalue + "'),";
                            sField = sField + "SourceOID" + "  " + "NUMBER" + ",";
                            sField = sField + "State" + "  " + "NUMBER" + ",";
                            sField = sField + "VERSION" + "  " + "NUMBER" + " " + "DEFAULT(0) NOT NULL,";

                        }
                        //////构建建表SQL语句
                        string CreateSQL = string.Empty;
                        if (sTableType != "ANNO")/////不是注记层
                        {
                            sField = sField.Substring(0, sField.LastIndexOf(","));
                            CreateSQL = "CREATE TABLE " + this._User.Trim() + "." + sTableName.Trim() + " " + "(" + sField + ")";
                        }
                        else////////注记层
                        {
                            continue;
                        }
                        //////////
                        ProcFrm.SetFrmProcessBarText("记录表" + sTableName);
                        //////////
                        if (!IsHisDb)
                        {
                            if (!TableDic.ContainsKey(sTableName.Trim()))
                            {
                                TableDic.Add(sTableName.Trim(), CreateSQL);
                            }
                        }
                        if (IsHisDb)
                        {
                            int index = sTableName.IndexOf("_GOH");
                            if (index < 0) { ex = new Exception("历史表名称不规范"); return; }
                            string userTableName = sTableName.Substring(0, index);
                            string strsql = "SELECT COUNT(*) FROM " + userTableName;
                            CreateTable(strsql, out ex);////判断用户表是否存在
                            if (ex != null)////用户表不存在，不需要建立历史表
                            {
                                continue;
                            }
                            InitialHisTable(sTableName, CreateSQL, sViewField, out ex);
                            //if (!TableDic.ContainsKey(sTableName.Trim()))
                            //{
                            //    TableDic.Add(sTableName.Trim(), CreateSQL);
                            //}
                        }
                        ////////////////////////////////////////////////////////////////                                               
                    }
                }
                ProcFrm.Close();
                if (!IsHisDb)
                {
                    ProcFrm.SetFrmProcessBarText("正在创建远程日志表");
                    /////////建立远程日志表//////
                    string LogSQL = "CREATE TABLE GO_DATABASE_UPDATELOG(OID NUMBER,STATE NUMBER,LAYERNAME VARCHAR2(30),LASTUPDATE DATE,VERSION NUMBER DEFAULT(0) NOT NULL,XMIN NUMBER,XMAX NUMBER,YMIN NUMBER,YMAX NUMBER)";
                    ltablename.Add("GO_DATABASE_UPDATELOG");
                    /////////远程更新日志
                    TableDic.Add("远程日志表:GO_DATABASE_UPDATELOG", LogSQL);
                    /////////数据库版本表
                    LogSQL = "CREATE TABLE go_database_version(VERSION NUMBER DEFAULT(0) NOT NULL,USERNAME VARCHAR2(30),VERSIONTIME DATE,DES VARCHAR2(30))";
                    ltablename.Add("go_database_version");
                    TableDic.Add("数据库版本表:go_database_version", LogSQL);
                    if (TableDic.Count > 0)
                    {
                        frmChooseTable fChooseTables = new frmChooseTable(TableDic, this._Server, this._User, this._Password);
                        if (DialogResult.OK == fChooseTables.ShowDialog())
                        {
                            Dictionary<string, string> getCreatedTables = fChooseTables.CreatedTable;
                        }
                        else
                        {
                            ex = new Exception("取消操作");
                            ProcFrm.Close();
                            return;
                        }

                    }

                }
                ProcFrm.Close();
            }
            catch (Exception dd)
            {
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", dd.Message);
                ex = dd;
                ProcFrm.Close();
            }

            #endregion
            System.Runtime.InteropServices.Marshal.ReleaseComObject(m_pProject);

        }
        /// <summary>
        /// 执行指定的SQL语句，可用于建表、删除、建用户等
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="ex"></param>
        public void CreateTable(string SQL, out Exception ex)
        {
            ex = null;
            if (this.m_DataBaseOper == null)
            {
                ex = new Exception("Oracle连接信息未初始化！");
                return;
            }
            try
            {
                this.m_DataBaseOper.ExecuteSql(SQL);
                return;
            }
            catch (Exception eRror)
            {
                ex = new Exception("建表失败！原因：" + eRror.Message);
                return;
            }
        }
        /// <summary>
        /// 初始化历史库体表（表不存在则建立，然后将用户库体中相应表中的数据加上时间戳复制到历史表中，SOURCID为原要素OBJECTID）
        /// </summary>
        /// <param name="sTableName">历史表名</param>
        /// <param name="SQL">建立历史表的Sql语句</param>
        /// <param name="HisFileds">用户表除SHAPE字段外的所有字段，用于创建视图</param>
        /// <param name="ex"></param>
        private void InitialHisTable(string sTableName, string SQL, string HisFileds, out Exception ex)
        {
            ex = null;

            if (this.m_DataBaseOper == null)
            {
                ex = new Exception("Oracle连接信息未初始化！");
                return;
            }
            int index = sTableName.IndexOf("_GOH");
            if (index < 0) { ex = new Exception("历史表名称不规范"); return; }
            string userTableName = sTableName.Substring(0, index);
            HisFileds = HisFileds.Remove(HisFileds.LastIndexOf(","), 1);/////用于创建视图
            #region 创建用户表视图
            string CreateView = "CREATE OR REPLACE VIEW " + userTableName + "_VIEW(" + HisFileds + ") AS SELECT " + HisFileds + " FROM " + userTableName;
            m_DataBaseOper.ExecuteSql(CreateView);
            #endregion
            string sNow = DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
            string strsql = "SELECT COUNT(*) FROM " + sTableName;
            CreateTable(strsql, out ex);////判断；历史表是否存在
            if (ex != null)////临时表不存在
            {
                CreateTable(SQL, out ex);/////建立历史表
                if (ex != null) return;
            }
            ex = null;
            //////////将用户库中的数据导入到历史表中////////////

            strsql = "SELECT * FROM " + userTableName + "_VIEW";
            DataTable gettable = new DataTable();
            gettable = m_DataBaseOper.GetSQLTable(strsql, out ex);
            if (ex != null) return;
            if (gettable != null)
            {
                FrmProcessBar Procbar = new FrmProcessBar();
                Procbar.SetChild();
                Procbar.SetFrmProcessBarMax((long)gettable.Rows.Count);
                for (int i = 0; i < gettable.Rows.Count; i++)
                {
                    Procbar.SetFrmProcessBarValue((long)i);
                    Procbar.Show();
                    Procbar.SetFrmProcessBarText("正在转储表：" + userTableName + ":中数据至:" + sTableName);
                    System.Windows.Forms.Application.DoEvents();
                    int iColCount = gettable.Columns.Count;

                    string sFields = string.Empty;
                    string sValue = string.Empty;
                    DataRow getrow = gettable.Rows[i];
                    long rec = 0;
                    //////获取历史表中记录数目
                    #region 获取记录数
                    string getrec = "SELECT COUNT(*) FROM " + sTableName;
                    DataTable rectable = m_DataBaseOper.GetSQLTable(getrec, out ex);
                    if (ex != null) return;
                    if (rectable != null)
                    {
                        try
                        {
                            rec = Convert.ToInt64(rectable.Rows[0][0].ToString()) + 1;
                        }
                        catch
                        {
                            rec = 1;
                        }
                    }
                    #endregion
                    long OID = -1;
                    OID = Convert.ToInt64(getrow[ModuleData.s_KeyFieldName].ToString());
                    //////获取字段和值
                    #region 获取字段和值（主码和几何字段除外）
                    for (int j = 0; j < iColCount; j++)
                    {
                        string sFieldName = gettable.Columns[j].ColumnName;
                        Type gettype = gettable.Columns[j].DataType;
                        string value = string.Empty;
                        if (sFieldName == ModuleData.s_KeyFieldName || sFieldName == ModuleData.s_GeometryFieldName) continue;
                        sFields += sFieldName + ",";
                        if (getrow[j] == null || string.IsNullOrEmpty(getrow[j].ToString()))
                            value = "NULL";
                        else
                        {
                            if (gettype.FullName == "System.String" || gettype.FullName == "System.Char" || gettype.FullName == "System.DateTime")
                            {
                                value = "'" + getrow[j].ToString() + "'";
                            }
                            else
                            {
                                value = getrow[j].ToString();
                            }
                        }
                        sValue += value + ",";

                    }
                    #endregion
                    sFields += ModuleData.s_KeyFieldName + ",";
                    sValue += rec.ToString() + ",";
                    sFields += "SourceOID" + ",";
                    sValue += OID.ToString() + ",";
                    sFields += "FromDate" + ",";
                    sValue += "'" + sNow + "',";
                    sFields += ModuleData.s_GeometryFieldName;
                    sValue += ModuleData.s_GetGeometry + "(" + OID.ToString() + ")";
                    ///////////
                    List<string> SQllist = new List<string>();
                    //   SQllist.Add("declare begin "+ModData.s_SaveShapePack.Trim()+"."+ModData.s_ShapePackSet+"MDSYS.SDO_GEOMETRY(SELECT "+ModData.s_GeometryFieldName+" FROM "+userTableName+" WHERE "+ModData.s_KeyFieldName+"="+OID.ToString()+")");
                    SQllist.Add("CREATE OR REPLACE FUNCTION " + ModuleData.s_GetGeometry + "(id IN NUMBER) RETURN MDSYS.SDO_GEOMETRY AS resgeo MDSYS.SDO_GEOMETRY;BEGIN SELECT " + ModuleData.s_GeometryFieldName + " INTO resgeo FROM " + userTableName + " WHERE " + ModuleData.s_KeyFieldName + "=id;RETURN resgeo;END;");
                    SQllist.Add("INSERT INTO " + sTableName + "(" + sFields + ")" + " VALUES(" + sValue + ")");
                    foreach (string sql in SQllist)
                    {
                        try
                        {
                            m_DataBaseOper.ExecuteSql(sql);

                        }
                        catch (Exception error)
                        {
                            ex = error;

                            continue;
                        }

                    }
                    rec += 1;
                }
                Procbar.Close();
            }

        }
    }
}
