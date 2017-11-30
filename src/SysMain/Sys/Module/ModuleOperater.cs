using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using SysCommon.Gis;
using SysCommon.Error;
using SysCommon.Authorize;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SysCommon;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.DataBase;
using System.Data.OracleClient;
using System.Data.OleDb;
namespace GDBM
{
    public static class ModuleOperator
    {

        /// <summary>
        /// 检查用户登陆
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="gisDb"></param>
        /// <returns></returns>
        public static bool AddSystemXML(string name, out Exception eError)
        {
            eError = null;
            return false;
            //eError = null;
            //User user;
            //try
            //{
            //    SysGisDB gisDb = new SysGisDB();
            //    GeoUtilities.clsDBConnect DBConn = new GeoUtilities.clsDBConnect();
            //    DBConn.GetConInfo();
            //    IWorkspace pWorkSpace = DBConn.GetWorkspace();
            //    gisDb.WorkSpace = pWorkSpace;
            //    SysGisTable sysTable = new SysGisTable(gisDb);
            //    Dictionary<string, object> dicData = sysTable.GetRow("ICE_USERINFO", "U_NAME='" + name + "'", out eError);
            //    if (dicData == null || dicData.Count == 0)
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","用户不存在！");
            //        return false;
            //    }
            //    if (dicData != null && dicData.Count > 0)
            //    {
            //        user = new User();
            //        user.ID = int.Parse(dicData["U_ID"].ToString());
            //        user.Name = dicData["U_NAME"].ToString();
            //        user.Password = dicData["U_PWD"].ToString();
            //        //user.Sex = int.Parse(dicData["U_SEX"].ToString());
            //        user.Position = dicData["U_JOB"].ToString();
            //        user.Remark = dicData["U_REMARK"].ToString();
            //        user.LoginInfo = dicData["LOGININFO"].ToString();
            //        //将用户信息和相应权限信息保存起来，在加载插件时用到
            //        Mod.v_AppUser = user;
            //        Mod.v_SystemXml = GetPrivilegeXml(user, gisDb, out eError);
            //        return true;
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    eError = ex;
            //    return false;
            //}
        }
        /// <summary>
        /// 权限文档合并
        /// </summary>
        /// <param name="user"></param>
        /// <param name="gisDb"></param>
        /// <returns></returns>
        private static XmlDocument GetPrivilegeXml(User user, SysGisDB gisDb,out Exception exError)
        {
            exError = null;
            Role role;
            try
            {
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("ICE_USERGROUPRELATION", "U_ID=" + user.ID, out exError);
                List<int> ids = null;
                if (lstDicData != null && lstDicData.Count > 0)
                {
                    ids = new List<int>();
                    foreach (Dictionary<string, object> dic in lstDicData)
                    {
                        foreach (string key in dic.Keys)
                        {
                            if (key.Equals("G_ID"))
                            {
                                ids.Add(int.Parse(dic[key].ToString()));
                            }
                        }
                    }
                    if (ids.Count > 0)
                    {
                        string strSql = "";
                        foreach (int id in ids)
                        {
                            if (string.IsNullOrEmpty(strSql))
                            {
                                strSql = id.ToString();
                            }
                            else
                            {
                                strSql += "," + id.ToString();
                            }
                        }
                        //获取当前用户的当前组id集
                        List<Dictionary<string, object>> lstDicDataRole = sysTable.GetRows("ICE_USERGROUPINFO", "G_ID IN (" + strSql + ")", out exError);
                        Dictionary<int, Role> dic = new Dictionary<int, Role>();
                        if (lstDicDataRole != null)
                        {
                            for (int i = 0; i < lstDicDataRole.Count; i++)
                            {
                                role = new Role();
                                role.ID = int.Parse(lstDicDataRole[i]["G_ID"].ToString());
                                role.Name = lstDicDataRole[i]["G_NAME"].ToString();
                                role.Type = lstDicDataRole[i]["G_TYPE"].ToString();
                                role.Privilege = lstDicDataRole[i]["G_PURVIEW"] as XmlDocument;
                                role.Remark = lstDicDataRole[i]["G_REMARK"].ToString();
                                dic.Add(int.Parse(lstDicDataRole[i]["G_ID"].ToString()), role);
                            }
                            if (dic.Count > 0)
                            {
                                XmlDocument allDoc = null;
                                foreach (int key in dic.Keys)
                                {
                                    //合并权限文档
                                    MergeXml(ref allDoc, dic[key].Privilege);
                                }
                                return allDoc;
                            }
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 合并XML文档
        /// </summary>
        /// <param name="allDoc"></param>
        /// <param name="doc"></param>
        private static void MergeXml(ref XmlDocument allDoc, XmlDocument doc)
        {
            try
            {
                if (doc == null) return;
                if (allDoc == null)
                {
                    allDoc = doc;
                }
                else
                {
                    string strXPath = @"//*[@Enabled='false' and @Visible='true'] | //*[@Enabled='true' and @Visible='false']|//*[@Enabled='false' and @Visible='false']";
                    XmlNodeList nodeList = allDoc.SelectNodes(strXPath);
                    if (nodeList != null && nodeList.Count > 0)
                    {
                        foreach (XmlNode node in nodeList)
                        {
                            strXPath = @"//" + node.Name + "[@Name='" + (node as XmlElement).GetAttribute("Name") + "']";
                            XmlNode pNode = doc.SelectSingleNode(strXPath);
                            if (pNode != null)
                            {
                                XmlElement pElement = pNode as XmlElement;
                                if (!node.Attributes["Enabled"].Value.Equals("true"))
                                {
                                    node.Attributes["Enabled"].Value = pElement.GetAttribute("Enabled");
                                }
                                if (!node.Attributes["Visible"].Value.Equals("true"))
                                {
                                    node.Attributes["Visible"].Value = pElement.GetAttribute("Visible");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //测试链接信息是否可用
        public static bool CanOpenConnect(string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "SDE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if(strType.ToUpper() == "PDB")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if(strType.ToUpper() == "GDB")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }

        /// <summary>
        /// 初始化子系统界面的选中状态   chenyafei  add 20110215  页面跳转
        /// </summary>
        /// <param name="pSysName">子系统name</param>
        /// <param name="pSysCaption">子系统caption</param>
        public static void InitialForm(string pSysName, string pSysCaption)
        {
            if (Plugin.ModuleCommon.DicTabs == null || Plugin.ModuleCommon.AppFrm == null) return;
            //初始化当前应用成素的名称和标题
            Plugin.ModuleCommon.AppFrm.CurrentSysName = pSysName;
            Plugin.ModuleCommon.AppFrm.Caption = pSysCaption;

            //显示选定的子系统界面
            bool bEnable = false;
            bool bVisible = false;
            if (Plugin.ModuleCommon.DicControls != null)
            {
                foreach (KeyValuePair<string, Plugin.Interface.IControlRef> keyValue in Plugin.ModuleCommon.DicControls)
                {
                    bEnable = keyValue.Value.Enabled;
                    bVisible = keyValue.Value.Visible;

                    Plugin.Interface.ICommandRef pCmd = keyValue.Value as Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == pSysName)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            //默认显示子系统界面的第一项
            int i = 0;
            foreach (KeyValuePair<DevExpress.XtraBars.Ribbon.RibbonPage, string> keyValue in Plugin.ModuleCommon.DicTabs)
            {
                //if (keyValue.Value == pSysName)
                //{
                //    i = i + 1;
                //    keyValue.Key.Visible = true;
                //    keyValue.Key.Enabled = true;
                //    if (i == 1)
                //    {
                //        //默认选中第一项
                //        keyValue.Key.Checked = true;
                //    }
                //}
                //else
                //{
                //    keyValue.Key.Visible = false;
                //    keyValue.Key.Enabled = false;
                //}
            }
        }

        /// <summary>
        /// 从XML获取数据库连接配置文件
        /// </summary>
        /// <param name="strPath"></param>
        public static bool GetSettingXml(string strPath)
        {
            if (string.IsNullOrEmpty(strPath)) return false;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(strPath);
                if (doc.DocumentElement != null)
                {
                    XmlElement pElement = doc.DocumentElement;
                    Mod.Server = pElement["server"].InnerText;
                    Mod.Instance = pElement["service"].InnerText;
                    Mod.Database = pElement["database"].InnerText;
                    Mod.User = pElement["user"].InnerText;
                    Mod.Password = pElement["password"].InnerText;
                    Mod.Version = pElement["version"].InnerText;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        //测试链接信息是否可用
        public static bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }
        /// <summary>
        /// 登陆控制
        /// </summary>
        /// <returns></returns>
        public static bool CheckLogin()
        {
            //判断配置文件是否存在
            bool blnCanConnect = false;
            if (File.Exists(Mod.v_ConfigPath))
            {
                SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(Mod.v_ConfigPath, out Mod.Server, out Mod.Instance, out Mod.Database, out Mod.User, out Mod.Password, out Mod.Version, out Mod.dbType);
                blnCanConnect = CanOpenConnect(vgisDb, Mod.dbType, Mod.Server, Mod.Instance, Mod.Database, Mod.User, Mod.Password, Mod.Version);
                Mod.TempWks = vgisDb.WorkSpace;
                Plugin.ModuleCommon.TmpWorkSpace = vgisDb.WorkSpace;
            }
            //无法连接则调配置窗体
            if ((!blnCanConnect))
            {
                //不存在，调数据配置窗体，并保存配置文件
                frmDBSet frmSet = new frmDBSet();
                if (frmSet.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    blnCanConnect = true;
                    Plugin.ModuleCommon.TmpWorkSpace = frmSet.WorkSpace;
                }
                else
                {
                    blnCanConnect = false;
                }
            }
            //存在，调用登陆窗体
            if (blnCanConnect)
            {
                frmSelectLogin pfrmSelectLogin = new frmSelectLogin();
                if (pfrmSelectLogin.ShowDialog() == DialogResult.OK)
                {
                    Mod.m_SystemType = pfrmSelectLogin.GetSystemType();
                    if (Mod.m_SystemType == SystemTypeEnum.ManagerSystem)
                    {
                        Mod.m_SysXmlPath=Application.StartupPath + "\\..\\Res\\Xml\\SysXmlManager.Xml";
                    }
                    else if (Mod.m_SystemType == SystemTypeEnum.ConfigSystem)
                    {
                        Mod.m_SysXmlPath=Application.StartupPath + "\\..\\Res\\Xml\\SysXmlConfig.Xml";
                    }
                    else
                    {
                        Mod.m_SysXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\SysXmlUpdate.Xml";
                    }
                    frmLogin frmLogin = new frmLogin(Mod.dbType);
                    if (frmLogin.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (frmLogin.LoginSuccess)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private static void  InitializeMetaDatabase()
        {
            string strOracleConnection = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + Mod.Database + ")));Persist Security Info=True;User Id=" + Mod.User + "; Password=" + Mod.Password + "";
            OracleConnection pOracleConnection = new OracleConnection(strOracleConnection);
            if (pOracleConnection.State == ConnectionState.Closed)
            {
                try
                {
                    pOracleConnection.Open();
                }
                catch {  return; }
                //MessageBox.Show("Oracle数据库连接失败！元数据库初始化失败，请设置正确的oracle连接信息。", "提示！");
            }
            OracleCommand pOracleCommand = pOracleConnection.CreateCommand();
            //创建中文分词器
            pOracleCommand.CommandText = "call ctx_ddl.create_preference('cnlex','CHINESE_LEXER')";
            try
            {
                pOracleCommand.ExecuteNonQuery();
            }
            catch { }
            if (pOracleConnection.State == ConnectionState.Closed)
            {
                pOracleConnection.Open();
            }
            //创建MetaData_XML表
            pOracleCommand.CommandText = "create table MetaData_XML( XMLID   NVARCHAR2(255) not null, MetaDataXML SYS.XMLTYPE not null,图幅号  VARCHAR2(100) not null,图名  NVARCHAR2(255),数据类型  NVARCHAR2(255) not null,数据生产单位 NVARCHAR2(255),数据生产时间  DATE,  影像类型   NVARCHAR2(255),影像分辨率   NVARCHAR2(255),入库时间  DATE)";
            try
            {
                pOracleCommand.ExecuteNonQuery();
            }
            catch { }
            if (pOracleConnection.State == ConnectionState.Closed)
            {
                pOracleConnection.Open();
            }
            //创建MetaData_XML表中MetaDataXML字段索引
            pOracleCommand.CommandText = "CREATE index source_index on MetaData_XML(MetaDataXML) indextype is ctxsys.context parameters ('DATASTORE CTXSYS.DIRECT_DATASTORE FILTER CTXSYS.NULL_FILTER LEXER cnlex')";
            try
            {
                pOracleCommand.ExecuteNonQuery();
            }
            catch { }
            if (pOracleConnection.State == ConnectionState.Closed)
            {
                pOracleConnection.Open();
            }
            //创建MetaData_TEMP表
            pOracleCommand.CommandText = "create table MetaData_TEMP( XMLID   NVARCHAR2(255) not null,  MetaDataXML CLOB)";
            try
            {
                pOracleCommand.ExecuteNonQuery();
            }
            catch { }
            if (pOracleConnection.State == ConnectionState.Closed)
            {
                pOracleConnection.Open();
            }
            //创建MetaData_TEMP表主键
            pOracleCommand.CommandText = "alter table MetaData_TEMP add constraint XMLID_pk primary key (XMLID) using index ";
            try
            {
                pOracleCommand.ExecuteNonQuery();
            }
            catch { }
            if (pOracleConnection.State == ConnectionState.Open)
            {
                pOracleConnection.Close();
            }
        }
        //根据配置文件初始化系统环境 added by chulili 20110531
        public static void InitSystemByXML(IWorkspace sourceWorkspace, IWorkspace targetWorkspace,bool iscover)
        {
            //xml模板位置固定，根据模板中规定的表名进行拷贝
            string xmlpath = Application.StartupPath + "\\..\\Template\\InitUserRoleConfig.Xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlpath);
            string strSearch = "//InitSystemRoot";
            XmlNode pInitSystemnode = doc.SelectSingleNode(strSearch);
            XmlNodeList pInitSystemlist = pInitSystemnode.ChildNodes;
            //遍历需要拷贝的表名
            foreach (XmlNode pnode in pInitSystemlist)
            {
                if (pnode.NodeType == XmlNodeType.Element)
                {
                    XmlElement pEle = pnode as XmlElement;
                    string strTableName = pEle.GetAttribute("Name");
                    try//判断源库中是否有该表，没有则跳过
                    {
                        IFeatureWorkspace ifwsource = sourceWorkspace as IFeatureWorkspace;
                        ifwsource.OpenTable(strTableName);

                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                    if (iscover)//如果覆盖，则先把目标库中该表删除
                    {
                        try//判断目标数据库中有没有该表，有则删除
                        {
                            IFeatureWorkspace ptmpwks = targetWorkspace as IFeatureWorkspace;
                            ESRI.ArcGIS.Geodatabase.ITable ptable = ptmpwks.OpenTable(strTableName);
                            IDataset pdataset = ptable as IDataset;
                            pdataset.Delete();
                            
                        }
                        catch (Exception e)
                        {
                        }
                        CopyPasteGDBData.CopyPasteGeodatabaseData(sourceWorkspace, targetWorkspace, strTableName, esriDatasetType.esriDTTable);
                    }
                    else
                    {
                        try//如果不覆盖，判断该表是否存在，不存在则拷贝表
                        {
                            IFeatureWorkspace ptmpwks = targetWorkspace as IFeatureWorkspace;
                            ptmpwks.OpenTable(strTableName);
                        }
                        catch (Exception e)
                        {

                                CopyPasteGDBData.CopyPasteGeodatabaseData(sourceWorkspace, targetWorkspace, strTableName, esriDatasetType.esriDTTable);
                            

                        }                        
                            
                    }
                }
            }
            if (Mod.dbType.ToUpper() == "ORACLE")
            {
                InitializeMetaDatabase();
            }
        }
        /// <summary>
        /// 检查用户登陆
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="gisDb"></param>
        /// <returns></returns>
        public static bool CheckLogin(string name, string password, ref SysGisDB gisDb, enumWSType wsType, out Exception eError)
        {
            eError = null;
            User user;
            bool result = false;
            try
            {
                if (gisDb == null)
                {
                    gisDb = new SysGisDB();
                    switch (wsType)
                    {
                        case enumWSType.SDE:
                            result = gisDb.SetWorkspace(Mod.Server, Mod.Instance, Mod.Database, Mod.User, Mod.Password, Mod.Version, out eError);
                            break;
                        case enumWSType.PDB:
                        case enumWSType.GDB:
                            result = gisDb.SetWorkspace(Mod.Server, wsType, out eError);
                            break;
                        default:
                            break;
                    }
                    if (!result) return false;
                }
                SysGisTable sysTable = new SysGisTable(gisDb);
                Dictionary<string, object> dicData = sysTable.GetRow("user_info", "NAME='" + name + "'", out eError);
                if (dicData != null && dicData.Count > 0)
                {
                    user = new User();
                    user.IDStr  = dicData["USERID"].ToString();
                    user.Name = dicData["NAME"].ToString();
                    user.TrueName = dicData["TRUTHNAME"].ToString();
                    user.Password = dicData["UPWD"].ToString();
                    user.SexInt = int.Parse(dicData["USEX"].ToString());
                    user.Position = dicData["UPOSITION"].ToString();
                    user.Remark = dicData["UREMARK"].ToString();
                    user.UserDate = "";
                    if (dicData.ContainsKey("ENDDATE"))
                    {
                        if (dicData["ENDDATE"] != null)
                        {
                            user.UserDate = dicData["ENDDATE"].ToString();
                        }
                    }
                    Dictionary<string, object> dicUser_role = sysTable.GetRow("user_role", "USERID='" + user.IDStr + "'", out eError);
                    if (dicUser_role.ContainsKey("ROLEID"))
                    {
                        string strRoleid = dicUser_role["ROLEID"].ToString();
                        Dictionary<string, object> dicRole = sysTable.GetRow("ROLE", "ROLEID='" + strRoleid + "'", out eError);
                        if (dicRole.ContainsKey("NAME"))
                        {
                            user.Role = dicRole["NAME"].ToString();
                        }
                        if (dicRole.ContainsKey("TYPEID"))
                        {
                            user.RoleTypeID =int.Parse( dicRole["TYPEID"].ToString());
                        }
                    }
                    string aa = SysCommon.Authorize.AuthorizeClass.ComputerSecurity(password);
                    if (user.Password.Equals(SysCommon.Authorize.AuthorizeClass.ComputerSecurity(password)))
                    {
                        if (user.UserDate != "")
                        {
                            string strDate = user.UserDate;
                            if (strDate.Contains(" "))
                            {
                                strDate = strDate.Substring(0, strDate.IndexOf(" "));
                            }
                            System.DateTime pDate = DateTime.ParseExact(strDate, "yyyy/MM/dd", null);
                            if(pDate.CompareTo(DateTime.Today)<0)
                            {
                                eError = new Exception("已超过用户有效日期");
                                return false;
                            }
                            
                        }
                        Mod.v_AppUser = user;
                        // ******************************************
                        // *修 改 者： chenyafei
                        // *修改日期： 20110602
                        // *功能描述： 保存用户对应的角色信息
                        if (!SaveRoleInfo(user)) return false;
                        XmlDocument docXml = new XmlDocument();
                        if (!File.Exists(Mod.m_SysXmlPath)) return false;
                        docXml.Load(Mod.m_SysXmlPath);
                        Mod.v_SystemXml = docXml;
                        Mod.v_ListUserPrivilegeID = GetUserPrivilege(user, gisDb, out eError);
                        Mod.v_ListUserdataPriID = GetUserDataPri(user, gisDb, out eError);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                eError = ex;
                return false;
            }
        }
        // ***************************************************************
        // *功能函数：将用户对应的角色信息保存起来
        // *添 加 者：陈亚飞
        // *修改日期：20110602
        // *返 回 值：true:成功;false:失败
        // *******************************************************************
        private static bool SaveRoleInfo(User user)
        {
            List<Role> RoleLst = new List<Role>();                               //声明用户对应的角色列表
            //进行联表查询
            IQueryDef pQueryDef = null;         //联表查询接口
            IFeatureWorkspace pFeaWS = Mod.TempWks as IFeatureWorkspace;  //工作空间
            if (pFeaWS == null) return false;
            pQueryDef = pFeaWS.CreateQueryDef();
            pQueryDef.Tables = "user_role,role,roletype";   //联表查询表格
            pQueryDef.SubFields = "role.ROLEID,role.NAME,role.TYPEID,roletype.ROLETYPE,role.PROJECTID";  //联表查询字段
            pQueryDef.WhereClause = "user_role.ROLEID=role.ROLEID and role.TYPEID=roletype.TYPEID and USERID='" + user.IDStr + "'";
            try
            {
                //执行查询并且返回游标
                ICursor pCursor = pQueryDef.Evaluate();
                if (pCursor == null) return false;
                IRow pRow = pCursor.NextRow();   //查询到的记录
                if (pRow == null) return false;
                while (pRow != null)
                {
                    Role pRole = new Role();  //角色信息
                    int roleidIndex = -1;       //角色ID所以
                    int roleNameIndex = -1;   //角色名称索引
                    int typeidIndex = -1;     //角色类型ID索引
                    int RoleTypeIndex = -1;   //角色类型索引
                    int projectIDIndex = -1;  //项目ID
                    roleidIndex = pRow.Fields.FindField("role.ROLEID");
                    roleNameIndex = pRow.Fields.FindField("role.NAME");
                    typeidIndex = pRow.Fields.FindField("role.TYPEID");
                    RoleTypeIndex = pRow.Fields.FindField("roletype.ROLETYPE");
                    projectIDIndex = pRow.Fields.FindField("role.PROJECTID");
                    if (roleidIndex == -1 || roleNameIndex == -1 || typeidIndex == -1 || RoleTypeIndex == -1 || projectIDIndex == -1) return false;
                    pRole.IDStr = pRow.get_Value(roleidIndex).ToString();  //角色ID
                    pRole.Name = pRow.get_Value(roleNameIndex).ToString(); //角色名称
                    pRole.TYPEID = pRow.get_Value(typeidIndex).ToString();//角色类型ID
                    pRole.Type = pRow.get_Value(RoleTypeIndex).ToString();   //角色类型
                    pRole.PROJECTID = pRow.get_Value(projectIDIndex).ToString();//项目ID
                    if (!RoleLst.Contains(pRole))
                    {
                        RoleLst.Add(pRole);
                    }
                    pRow = pCursor.NextRow();
                }
                //释放游标
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                //将角色信息保存起来
                Mod.v_LstRole = RoleLst;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        //获得用户数据权限id集合
        public static List<string> GetUserDataPri(User user, SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            string rolePrivilegeID;

            try
            {
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("user_role", "USERID='" + user.IDStr + "'", out exError);
                List<string> ids = null;
                if (lstDicData != null && lstDicData.Count > 0)
                {
                    ids = new List<string>();
                    foreach (Dictionary<string, object> dic in lstDicData)
                    {
                        foreach (string key in dic.Keys)
                        {
                            if (key.Equals("ROLEID"))
                            {
                                ids.Add(dic[key].ToString());
                            }
                        }
                    }
                    if (ids.Count > 0)
                    {
                        string strSql = "";
                        foreach (string id in ids)
                        {
                            if (string.IsNullOrEmpty(strSql))
                            {
                                strSql = "'" + id.ToString() + "'";
                            }
                            else
                            {
                                strSql += ",'" + id.ToString() + "'";
                            }
                        }
                        //获取当前用户的当前组id集
                        List<Dictionary<string, object>> lstDicDataRole = sysTable.GetRows("role_Data", "ROLEID IN (" + strSql + ")", out exError);
                        List<string> lstRolePrivilege = null;
                        if (lstDicDataRole != null && lstDicDataRole.Count > 0)
                        {
                            lstRolePrivilege = new List<string>();
                            for (int i = 0; i < lstDicDataRole.Count; i++)
                            {
                                rolePrivilegeID = lstDicDataRole[i]["DATAPRI_ID"].ToString();
                                lstRolePrivilege.Add(rolePrivilegeID);
                            }
                            Purge(ref lstRolePrivilege);
                            return lstRolePrivilege;

                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        //获取用户菜单权限的id集合
        public static List<string> GetUserPrivilege(User user, SysGisDB gisDb, out Exception exError)
        {
            exError = null;
            string rolePrivilegeID;

            try
            {
                SysGisTable sysTable = new SysGisTable(gisDb);
                List<Dictionary<string, object>> lstDicData = sysTable.GetRows("user_role", "USERID='" + user.IDStr + "'", out exError);
                List<string> ids = null;
                if (lstDicData != null && lstDicData.Count > 0)
                {
                    ids = new List<string>();
                    foreach (Dictionary<string, object> dic in lstDicData)
                    {
                        foreach (string key in dic.Keys)
                        {
                            if (key.Equals("ROLEID"))
                            {
                                ids.Add(dic[key].ToString());
                            }
                        }
                    }
                    if (ids.Count > 0)
                    {
                        string strSql = "";
                        foreach (string id in ids)
                        {
                            if (string.IsNullOrEmpty(strSql))
                            {
                                strSql = "'" + id.ToString() + "'";
                            }
                            else
                            {
                                strSql += ",'" + id.ToString() + "'";
                            }
                        }
                        //获取当前用户的当前组id集
                        List<Dictionary<string, object>> lstDicDataRole = sysTable.GetRows("role_pri", "ROLEID IN (" + strSql + ")", out exError);
                        List<string> lstRolePrivilege = null;
                        if (lstDicDataRole != null && lstDicDataRole.Count > 0)
                        {
                            lstRolePrivilege = new List<string>();
                            for (int i = 0; i < lstDicDataRole.Count; i++)
                            {
                                rolePrivilegeID = lstDicDataRole[i]["PRIVILEGE_ID"].ToString();
                                lstRolePrivilege.Add(rolePrivilegeID);
                            }
                            Purge(ref lstRolePrivilege);
                            return lstRolePrivilege;

                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 去除List中的重复项
        /// </summary>
        /// <param name="needToPurge"></param>
        public static void Purge(ref List<string> needToPurge)
        {
            for (int i = 0; i < needToPurge.Count - 1; i++)
            {
                string deststring = needToPurge[i];
                for (int j = i + 1; j < needToPurge.Count; j++)
                {
                    if (deststring.CompareTo(needToPurge[j]) == 0)
                    {
                        needToPurge.RemoveAt(j);
                        continue;
                    }
                }
            }
        }
    }
}
