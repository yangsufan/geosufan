using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using System.Data.OracleClient;
using ESRI.ArcGIS.Display;
using SysCommon.Authorize;

namespace GeoDBIntegration
{
    public partial class FrmGetTaskLayerGuide : DevComponents.DotNetBar.Office2007Form
    {
        private Plugin.Application.IAppDBIntegraRef m_AppGIS;               //主功能应用APP
        private Plugin.Application.IAppFormRef m_AppForm;               //主功能应用APP
        private IGeometry m_Geometry;
        private IWorkspace m_SDEWs = null;//////////////////////用户库所在Workspace
        private IWorkspace m_SourceWs = null;/////////////////任务范围数据源的Workspace
        private IFeatureClass m_SDETaskLayer = null;/////////////任务的范围图层（SDE上的）
        private IFeatureClass m_SourceTaskLayer = null;//////////源的任务范围图层
        string m_sSDERangeLayerName = "RANGE";
        private string m_DBProjectID = string.Empty;/////////////数据库工程ID
        private string m_iRoleId ="";/////////////////////////////作业员对应的角色ID
        //cyf 20110603 add
        //private string pUserId = "";                         //作业员ID
        //end

        public IGeometry DrawGeometry
        {
            set
            {
                m_Geometry = value;
            }
        }
        public FrmGetTaskLayerGuide(Plugin.Application.IAppDBIntegraRef pAppGIS, IWorkspace in_SDEWs, string in_DBID)
        {
            InitializeComponent();
            //cyf  201100603  add;
            m_AppGIS = pAppGIS;
            if (m_AppGIS == null) return;
            m_AppForm = m_AppGIS as Plugin.Application.IAppFormRef;
            if (m_AppForm == null) return;
            //end
            this.cmb_Type.Items.Add("SDE");
            this.cmb_Type.Items.Add("PDB");
            this.cmb_Type.Items.Add("GDB");
            this.cmb_Type.SelectedIndex = 1;
            this.m_SDEWs = in_SDEWs;
            this.radioInputNewLayer.Checked = true;
            this.m_DBProjectID = in_DBID;
            this.RadioSDELayer.Checked = true;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void RadioBtnSelectRange_Click(object sender, EventArgs e)
        {

        }

        private void RadioBtnSelectRange_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 数据源类型改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmb_Type.Text == "SDE")
            {
                this.txtServer.Enabled = true;
                this.txt_servername.Enabled = true;
                this.txtDataBase.Enabled = true;
                this.btnServer.Enabled = false;
                this.txtVersion.Enabled = true;
                this.txtVersion.Text = "SDE.DEFAULT";
                this.txtUser.Enabled = true;
                this.txtPassWord.Enabled = true;
            }
            else if (this.cmb_Type.Text == "GDB" || this.cmb_Type.Text == "PDB")
            {
                this.txtServer.Enabled = false;
                this.txt_servername.Enabled = false;
                this.txtDataBase.Enabled = true;
                this.btnServer.Enabled = true;
                this.txtVersion.Enabled = false;
                this.txtVersion.Text = "";
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
            }
        }

        private void RadioSDELayer_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioInputNewLayer.Checked == true)
                this.groupBox1.Enabled = true;
            else
                this.groupBox1.Enabled = false;
        }

        private void radioInputNewLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioInputNewLayer.Checked == true)
                this.groupBox1.Enabled = true;
            else
                this.groupBox1.Enabled = false;
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            switch (this.cmb_Type.Text.Trim())
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = pFolderBrowser.SelectedPath;
                    }
                    break;

                case "PDB":
                    OpenFileDialog saveFile = new OpenFileDialog();
                    saveFile.Title = "打开PDB数据";
                    saveFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = saveFile.FileName;
                    }
                    break;
                default:
                    break;
            }
        }






        private void btn1_Next_Click(object sender, EventArgs e)
        {
            //tabControl1.SelectedIndex = 1;
            Exception ex=null;
            if (this.RadioSDELayer.Checked)
            {
                ///////SDE中已经存在的范围图层///////
                try
                {
                    this.m_SDETaskLayer = (this.m_SDEWs as IFeatureWorkspace).OpenFeatureClass(this.m_sSDERangeLayerName);
                    tabControl1.SelectedIndex = 2;
                    IFeatureLayer SDERangeLayer = new FeatureLayerClass();
                    SDERangeLayer.FeatureClass = this.m_SDETaskLayer;
                    this.axMapControl2.AddLayer(SDERangeLayer as ILayer);
                    //////////渲染///////////////
                    if (this.axMapControl2.LayerCount > 0)
                    {
                        IFeatureLayer GetFirstLayer = this.axMapControl2.get_Layer(0) as IFeatureLayer;
                        SetDataUpdateSymbol(GetFirstLayer);
                    }
                    this.axMapControl2.Refresh();
                    //cyf 20110603  modify：加载项目信息
                    GetProjectInfo(out ex);
                    if (ex != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取项目信息失败：" + ex.Message);
                        return;
                    }
                    //GetAllUser(ModuleData.v_AppConnStr, out ex);
                    //if (ex != null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取作业员信息失败：" + ex.Message);
                    //    return;
                    //}
                    //end
                    
                }
                catch (Exception eError)
                {
                    if (null == ModuleData.v_SysLog) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModuleData.v_SysLog.Write(eError);
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "SDE中获取任务范围图层：" + this.m_sSDERangeLayerName + " 失败");
                    return;
                }
            }
            else
            {
                ///////数据源中导入的图层///////
                this.m_SourceWs = GetDBInfo(this.cmb_Type.Text.Trim(), this.txtDataBase.Text, this.txtServer.Text, this.txt_servername.Text, this.txtUser.Text, this.txtPassWord.Text, this.txtVersion.Text, out ex) as IWorkspace;
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据源失败：" + ex.Message);
                    return;
                }
                if (this.m_SourceWs == null) return;
                tabControl1.SelectedIndex = 1;
                ////////将源数据中的图层显示在列表中///////
                IEnumDataset pEnumDataSet = this.m_SourceWs.get_Datasets(esriDatasetType.esriDTFeatureClass);
                IFeatureClass GetLayerInSource = pEnumDataSet.Next() as IFeatureClass;
                while (GetLayerInSource != null)
                {
                    if (GetLayerInSource.ShapeType != esriGeometryType.esriGeometryPolygon)
                    {
                        ////////只加载多边形的图层
                        GetLayerInSource = pEnumDataSet.Next() as IFeatureClass;
                        continue;
                    }
                    this.combox_SelectLayer.Items.Add((GetLayerInSource as IDataset).Name);
                    GetLayerInSource = pEnumDataSet.Next() as IFeatureClass;
                }
                if (this.combox_SelectLayer.Items.Count > 0) this.combox_SelectLayer.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// 根据条件创建工作空间（引用原有代码）
        /// </summary>
        /// <param name="strType">类型：SDE,PDB,GDB</param>
        /// <param name="strDB">数据库路径</param>
        /// <param name="strServer">服务名</param>
        /// <param name="strInstance">实例名</param>
        /// <param name="strUser">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <param name="strVersion">版本</param>
        /// <param name="ex">输出：错误信息</param>
        /// <returns>IWorkspace对象，出错返回NULL</returns>
        public static object GetDBInfo(string strType, string strDB, string strServer, string strInstance, string strUser, string strPassword, string strVersion, out Exception ex)
        {
            ex = null;
            try
            {
                IPropertySet pPropSet = null;
                switch (strType.Trim().ToLower())
                {
                    case "pdb":
                        pPropSet = new PropertySetClass();
                        AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();
                        if (!File.Exists(strDB))
                        {
                            FileInfo filePDB = new FileInfo(strDB);
                            pAccessFact.Create(filePDB.DirectoryName, filePDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace pdbWorkspace = pAccessFact.Open(pPropSet, 0);
                        pAccessFact = null;
                        return pdbWorkspace;

                    case "gdb":
                        pPropSet = new PropertySetClass();
                        FileGDBWorkspaceFactoryClass pFileGDBFact = new FileGDBWorkspaceFactoryClass();
                        if (!Directory.Exists(strDB))
                        {
                            DirectoryInfo dirGDB = new DirectoryInfo(strDB);
                            pFileGDBFact.Create(dirGDB.Parent.FullName, dirGDB.Name, null, 0);
                        }
                        pPropSet.SetProperty("DATABASE", strDB);
                        IWorkspace gdbWorkspace = pFileGDBFact.Open(pPropSet, 0);
                        pFileGDBFact = null;
                        return gdbWorkspace;

                    case "sde":
                        pPropSet = new PropertySetClass();
                        IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                        pPropSet.SetProperty("SERVER", strServer);
                        pPropSet.SetProperty("INSTANCE", strInstance);
                        pPropSet.SetProperty("DATABASE", strDB);
                        pPropSet.SetProperty("USER", strUser);
                        pPropSet.SetProperty("PASSWORD", strPassword);
                        pPropSet.SetProperty("VERSION", strVersion);
                        IWorkspace sdeWorkspace = pSdeFact.Open(pPropSet, 0);
                        pSdeFact = null;
                        return sdeWorkspace;

                    case "access":
                        System.Data.Common.DbConnection dbCon = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strDB);
                        dbCon.Open();
                        return dbCon;

                    //case "oracle":
                    //    string strOracle = "Data Source=" + strDB + ";Persist Security Info=True;User ID=" + strUser + ";Password=" + strPassword + ";Unicode=True";
                    //    System.Data.Common.DbConnection dbConoracle = new OracleConnection(strOracle);
                    //    dbConoracle.Open();
                    //    return dbConoracle;

                    //case "sql":
                    //    string strSql = "Data Source=" + strDB + ";Initial Catalog=" + strInstance + ";User ID=" + strUser + ";Password=" + strPassword;
                    //    System.Data.Common.DbConnection dbConsql = new SqlConnection(strSql);
                    //    dbConsql.Open();
                    //    return dbConsql;

                    default:
                        break;
                }

                return null;
            }
            catch (Exception e)
            {
                //******************************************
                //系统运行日志
                if (ModuleData.v_SysLog == null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(e);
                //******************************************
                ex = e;
                return null;
            }
        }

        private void btn2_Pre_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void axMapControl1_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {

        }

        /// <summary>
        /// 源图层列表中图层改变，图层显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combox_SelectLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sGetLayerName = this.combox_SelectLayer.Text;
            try
            {
                this.axMapControl1.ClearLayers();
                IFeatureClass Getlayer = (this.m_SourceWs as IFeatureWorkspace).OpenFeatureClass(sGetLayerName);
                IFeatureLayer pFeaLayer = new FeatureLayerClass();
                pFeaLayer.FeatureClass = Getlayer;
                ILayer AddLayer = pFeaLayer as ILayer;
                this.axMapControl1.AddLayer(AddLayer);
                this.m_SourceTaskLayer = Getlayer;
                this.axMapControl1.Refresh();
                this.m_SourceTaskLayer = Getlayer;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 将源图层的要素导入到SDE中，进入下一步分配的阶段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn2_next_Click(object sender, EventArgs e)
        {
            Exception ex=null;
            if (this.m_SourceTaskLayer == null) return;
            if (this.m_SDETaskLayer == null)
            {
                try
                {
                    this.m_SDETaskLayer = (this.m_SDEWs as IFeatureWorkspace).OpenFeatureClass(this.m_sSDERangeLayerName);
                }
                catch(Exception eError)
                {
                    if (null == ModuleData.v_SysLog) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModuleData.v_SysLog.Write(eError);
                }
            }
            //////////首先清空已有的范围要素//////
            //OracleConnection Con =Con = new OracleConnection(ModuleData.v_AppConnStr);
            try
            {
                //cyf 20110603 modify
                /////////////////清空分配信息/////////////////           
                //if (Con.State == ConnectionState.Closed) Con.Open();
                //string sDeleteSql = "DELETE FROM updateinfo WHERE PROJECTID=" + this.m_DBProjectID;
                //OracleCommand Comm = new OracleCommand(sDeleteSql, Con);
                //Comm.ExecuteNonQuery();
                string sDeleteSql = "DELETE FROM updateinfo WHERE PROJECTID=" + this.m_DBProjectID;
                if (ModuleData.TempWks == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                    return;
                }
                ModuleData.TempWks.ExecuteSQL(sDeleteSql);
                //end
                //cyf 20110603 delete:取消注册版本
                ////////////////////判断是否建立了版本////////////////////
                //IVersionedObject pVersion = this.m_SDETaskLayer as IVersionedObject;
                //if (!pVersion.IsRegisteredAsVersioned) pVersion.RegisterAsVersioned(true);
                ////////////////////////////////////////////////
                //end
                IFeatureCursor pFeaCur = this.m_SDETaskLayer.Search(null, false);
                IFeature pGetFea = pFeaCur.NextFeature();
                IWorkspaceEdit SDEWs = (this.m_SDETaskLayer as IDataset).Workspace as IWorkspaceEdit;
                //SDEWs.StartEditing(false);
                //SDEWs.StartEditOperation();
                bool bSave = false;
                while (pGetFea != null)
                {
                    pGetFea.Delete();
                    pGetFea.Store();
                    bSave = true;
                    pGetFea = pFeaCur.NextFeature();
                }
                //SDEWs.StopEditOperation();
                //SDEWs.StopEditing(bSave);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCur);
                ///////////////////////////
            }
            catch (Exception eError)
            {
                if (ModuleData.v_SysLog == null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog(); ModuleData.v_SysLog.Write(eError);
            }
            //finally
            //{
            //    if (Con.State == ConnectionState.Open) Con.Close();
            //}
            ////////要素转移////////////
            try
            {
                //cyf 20110603 delete:取消注册版本
                ////////////////////判断是否建立了版本////////////////////
                //IVersionedObject pVersion = this.m_SDETaskLayer as IVersionedObject;
                //if (!pVersion.IsRegisteredAsVersioned) pVersion.RegisterAsVersioned(true);
                ////////////////////////////////////////////////
                //end
                IFeatureCursor pFeaCur = this.m_SourceTaskLayer.Search(null, false);
                IFeature pGetFea = pFeaCur.NextFeature();
               // int RangeId = 0;
                IWorkspaceEdit SDEWs = (this.m_SDETaskLayer as IDataset).Workspace as IWorkspaceEdit;
                //SDEWs.StartEditing(false);
                //SDEWs.StartEditOperation();
                bool bSave = false;
                while (pGetFea != null)
                {
                    //RangeId += 1;
                    IFeature NewFeature = this.m_SDETaskLayer.CreateFeature();
                    NewFeature.Shape = pGetFea.ShapeCopy;
                    int FieldIndex = NewFeature.Fields.FindField("RANGEID");
                    if (FieldIndex > 0) NewFeature.set_Value(FieldIndex, NewFeature.OID);
                    FieldIndex = NewFeature.Fields.FindField("assign");
                    if (FieldIndex > 0) NewFeature.set_Value(FieldIndex, 0);
                    NewFeature.Store();
                    pGetFea = pFeaCur.NextFeature();
                    bSave = true;
                }
                //SDEWs.StopEditOperation();
                //SDEWs.StopEditing(bSave);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCur);
            }
            catch (Exception eError)
            {
                if (ModuleData.v_SysLog == null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog(); ModuleData.v_SysLog.Write(eError);
            }
            tabControl1.SelectedIndex = 2;
            /////////
            IFeatureLayer SDERangeLayer = new FeatureLayerClass();
            SDERangeLayer.FeatureClass = this.m_SDETaskLayer;
            this.axMapControl2.AddLayer(SDERangeLayer as ILayer);            
            
            //cyf 20110603 modify:加载项目信息
            GetProjectInfo(out ex);
            //GetAllUser(ModuleData.v_AppConnStr, out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取项目信息失败：" + ex.Message);
                return;
            }
            //////////渲染///////////////
            if (this.axMapControl2.LayerCount > 0)
            {
                IFeatureLayer GetFirstLayer = this.axMapControl2.get_Layer(0) as IFeatureLayer;
                SetDataUpdateSymbol(GetFirstLayer);
            }
            this.axMapControl2.Refresh();
        }

        /*
        /// <summary>
        ///读取系统维护哭中的作业员信息现在在列表中提供范围分配
        /// </summary>
        /// <param name="in_OracleDBConnectstr"></param>
        /// <param name="ex"></param>
        private void GetAllUser(string in_OracleDBConnectstr, out Exception ex)
        {
            ex = null;
            if (string.IsNullOrEmpty(in_OracleDBConnectstr)) { ex = new Exception("系统维护库连接信息不能为空"); return; }
            ///////////////读取数据库中的用户信息显示在Combox上///////////
            OracleConnection Con = new OracleConnection(in_OracleDBConnectstr);
            try
            {
                Con.Open();
                string SQL = "SELECT ROLETYPEID FROM roletypeinfo WHERE ROLETYPE=1";/////1为普通用户
                OracleDataAdapter DataAdapter = new OracleDataAdapter(SQL, Con);
                DataTable GetTable = new DataTable();
                DataAdapter.Fill(GetTable);
                m_iRoleId = Convert.ToInt32(GetTable.Rows[0][0].ToString());
                SQL = "SELECT U.USERID,U.USERNAME, U.SEX,U.JOB FROM userbaseinfo U JOIN userrolerelationinfo R  ON (U.USERID=R.userid)JOIN roletypeinfo X ON(R.ROLEID=X.ROLETYPEID) WHERE X.ROLETYPE=" + m_iRoleId.ToString();
                DataAdapter = new OracleDataAdapter(SQL, Con);
                GetTable = new DataTable();
                DataAdapter.Fill(GetTable);
                this.comboBox_USER.DataSource = GetTable;
                this.comboBox_USER.DisplayMember = "USERNAME";
                this.comboBox_USER.ValueMember = "USERID";
            }
            catch (Exception eError)
            {
                ex = eError;
                if (ModuleData.v_SysLog == null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(eError);
                return;
            }
            finally
            {
                if (Con.State == ConnectionState.Open) Con.Close();
            }
        }
        */


        /// <summary>
        /// cyf 20110603 add:读取系统维护哭中的作业员信息现在在列表中提供范围分配
        /// </summary>
        /// <param name="proIDStr">项目ID</param>
        /// <param name="ex">异常</param>
        private void GetAllUser(string proIDStr,out Exception ex)
        {
            ex = null;
            if (ModuleData.TempWks==null) { ex = new Exception("系统维护库连接信息不能为空"); return; }
            ///////////////读取数据库中的用户信息显示在Combox上///////////
            try
            {
                //查询表格，获取用户信息
                IFeatureWorkspace pFeaWs = ModuleData.TempWks as IFeatureWorkspace;
                if (pFeaWs != null)
                {
                    IQueryDef pQueryDef = pFeaWs.CreateQueryDef();
                    pQueryDef.Tables = "role,user_role,user_info";
                    pQueryDef.SubFields = "user_info.USERID,user_info.NAME,role.ROLEID";
                    pQueryDef.WhereClause = "role.ROLEID=user_role.ROLEID and  user_role.USERID=user_info.USERID and role.TYPEID='"+EnumRoleType.作业员.GetHashCode().ToString()+"' and  " + "role.PROJECTID='" + proIDStr + "'";//+ EnumRoleType.作业员.GetHashCode().ToString() + "'
                    ICursor pCursor = pQueryDef.Evaluate();
                    if (pCursor == null)
                    {
                        ex = new Exception("查询用户信息失败！");
                        return;
                    }
                      //创建临时的表，用来报讯查询到的项目信息
                    DataTable mTable = new DataTable();
                    mTable.Columns.Add("USERID", Type.GetType("System.String"));
                    mTable.Columns.Add("USERNAME", Type.GetType("System.String"));

                    IRow pRow = pCursor.NextRow();
                    //遍历查询到的所有的行，将项目信息加载在下拉列表框中
                    while (pRow != null)
                    {
                        string mUserIDStr = pRow.get_Value(0).ToString().Trim();        //项目ID
                        string mUserNameStr = pRow.get_Value(1).ToString().Trim();      //项目名称

                        //往临时表中插入数据
                        DataRow pDtRow = mTable.NewRow();
                        pDtRow[0] = mUserIDStr;
                        pDtRow[1] = mUserNameStr;
                        mTable.Rows.Add(pDtRow);

                        pRow = pCursor.NextRow();
                    }
                    //释放游标
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    //在下拉列表框中显示
                    this.comboBox_USER.DataSource = mTable;
                    this.comboBox_USER.DisplayMember = "USERNAME";
                    this.comboBox_USER.ValueMember = "USERID";

                    if (this.comboBox_USER.Items.Count > 0)
                    {
                        this.comboBox_USER.SelectedIndex = 0;
                    }
                }
            } catch (Exception eError)
            {
                ex = eError;
                if (ModuleData.v_SysLog == null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(eError);
                return;
            }
            
        }
        
        /// <summary>
        /// cyf 20110603 add :获取登录用户的项目信息
        /// </summary>
        /// <param name="outError">异常</param>
        private void GetProjectInfo(out Exception outError)
        {
            outError = null;
            if (ModuleData.TempWks == null) { outError = new Exception("系统维护库连接信息不能为空"); return; }
            ///////////////读取数据库中的用户信息显示在Combox上///////////
            try
            {
                //查询表格，获取用户信息
                IFeatureWorkspace pFeaWs = ModuleData.TempWks as IFeatureWorkspace;
                if (pFeaWs != null)
                {
                    //获得登录用户对应的工程项目信息
                    List<Role> LstRole = m_AppForm.LstRoleInfo;
                    if (LstRole == null)
                    {
                        outError = new Exception("查询项目信息表失败！");
                        return;
                    }
                    string proIDWhere = "";           //项目ID过滤字符串
                    foreach (Role pRole in LstRole)
                    {
                        string proIDStr = pRole.PROJECTID;   //项目ID
                        proIDWhere += "'" + proIDStr + "',";
                    }
                    if (proIDWhere != "")
                    {
                        proIDWhere = proIDWhere.Substring(0, proIDWhere.Length - 1);
                    }

                    IQueryDef pQueryDef = pFeaWs.CreateQueryDef();
                    pQueryDef.Tables = "projectgroup";
                    pQueryDef.SubFields = "PROJECTID,PROJECTNAME";
                    if (proIDWhere != "")
                    {
                        pQueryDef.WhereClause = "PROJECTID in (" + proIDWhere + ")";
                    }
                    else
                    {
                        outError = new Exception("获取登录用户的项目信息失败！");
                        return;
                    }
                    ICursor pCursor = pQueryDef.Evaluate();
                    if (pCursor == null)
                    {
                        outError = new Exception("查询项目信息失败！");
                        return;
                    }
                    //创建临时的表，用来报讯查询到的项目信息
                    DataTable mTable = new DataTable();
                    mTable.Columns.Add("PROJECTID", Type.GetType("System.String"));
                    mTable.Columns.Add("PROJECTNAME", Type.GetType("System.String"));

                    IRow pRow = pCursor.NextRow();
                    //遍历查询到的所有的行，将项目信息加载在下拉列表框中
                    while (pRow != null)
                    {
                        string mProIDStr = pRow.get_Value(0).ToString().Trim();        //项目ID
                        string mProNameStr = pRow.get_Value(1).ToString().Trim();      //项目名称

                        //往临时表中插入数据
                        DataRow pDtRow = mTable.NewRow();
                        pDtRow[0] = mProIDStr;
                        pDtRow[1] = mProNameStr;
                        mTable.Rows.Add(pDtRow);

                        pRow = pCursor.NextRow();
                    }
                    //释放游标
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    //在下拉列表框中显示
                    this.cmbProject.DataSource = mTable;
                    this.cmbProject.DisplayMember = "PROJECTNAME";
                    this.cmbProject.ValueMember = "PROJECTID";

                    if (this.cmbProject.Items.Count > 0)
                    {
                        this.cmbProject.SelectedIndex = 0;
                    }
                }
            } catch(Exception eError)
            {
                outError = eError;
                if (ModuleData.v_SysLog == null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(eError);
                return;
            }
        }

        /// <summary>
        /// 范围的分配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //cyf  20110603 add
           if( ModuleData.TempWks == null)
           {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                return;
           }
            IFeatureWorkspace pFeaWs=ModuleData.TempWks as IFeatureWorkspace;
            if(pFeaWs==null) 
            {
                 SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                return;
            }
            //end
            if (this.axMapControl2.LayerCount <= 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请加载范围图层");
                return;
            }
            if (string.IsNullOrEmpty(this.comboBox_USER.Text.Trim()))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个接受作业范围的作业员");
                this.comboBox_USER.Focus(); 
                return;
            }
            try
            {
                IFeatureLayer GetLayer = this.axMapControl2.get_Layer(0) as IFeatureLayer;
                IFeatureClass GetFeaCls = GetLayer.FeatureClass;
                IFeatureSelection fealSel = GetLayer as IFeatureSelection;
                string sUserName = this.comboBox_USER.Text;
                string sUserID = this.comboBox_USER.SelectedValue.ToString();
                //cyf 20110603 delete:
                ////////////////////判断是否建立了版本////////////////////
                //IVersionedObject pVersion = this.m_SDETaskLayer as IVersionedObject;
                //if (!pVersion.IsRegisteredAsVersioned) pVersion.RegisterAsVersioned(true);
                //end
                ////////////////////////////////////////////////
                //cyf  20110603  modify
                ///////系统维护库相关//////
                //OracleConnection Con = new OracleConnection(ModuleData.v_AppConnStr);
                IWorkspaceEdit SDEWs = (this.m_SDETaskLayer as IDataset).Workspace as IWorkspaceEdit;
                int iUserFieldIndex = m_SDETaskLayer.Fields.FindField("USERNAME");
                //SDEWs.StartEditing(false);
                //SDEWs.StartEditOperation();
                //bool bSave = false;
                 if (fealSel.SelectionSet.Count != 0)
                 {
                     //Con.Open();
                     IEnumIDs pEnumIDs = fealSel.SelectionSet.IDs;
                     int id = pEnumIDs.Next();
                     while (id != -1)
                     {
                         IFeature GetFea = GetFeaCls.GetFeature(id);
                         string sRangeID=string.Empty;
                         int iAssign = 0;
                         int Index=GetFea.Fields.FindField("RANGEID");
                         if (Index<0) sRangeID="null";
                         else sRangeID=GetFea.get_Value(Index).ToString();
                         Index = GetFea.Fields.FindField("assign");
                         try
                         {
                             iAssign = Convert.ToInt32(GetFea.get_Value(Index).ToString());
                         }
                         catch
                         {
                             iAssign = 0;
                         }
                         ///////将任务分配信息写入数据库/////
                         //cyf 20110603 add:获取updateinfo的记录的最大值
                         int oidMax = 0;
                         ITable pTable = pFeaWs.OpenTable("updateinfo");
                         if (pTable.RowCount(null) == 0)
                         {
                             oidMax = 1;
                         }
                         else
                         {
                             IDataStatistics pDtSta = new DataStatisticsClass();
                             pDtSta.Field = "OBJECTID";
                             pDtSta.Cursor = pTable.Search(null, false);
                             IStatisticsResults pStaRes = null;
                             pStaRes = pDtSta.Statistics;
                             oidMax = (int)pStaRes.Maximum;
                             oidMax = oidMax + 1;
                         }
                         //end
                         string SQL = string.Empty;
                         if (iAssign == 0)
                             SQL = "INSERT INTO updateinfo(OBJECTID,PROJECTID ,RANGEID,ROLEID,USERID ) values(" + oidMax + "," + this.m_DBProjectID + "," + sRangeID + ",'" + this.m_iRoleId.ToString() + "','" + sUserID + "')";
                         else
                             SQL = "UPDATE updateinfo SET PROJECTID=" + this.m_DBProjectID + "," + "RANGEID=" + sRangeID + "," + "ROLEID='" + this.m_iRoleId.ToString() + "'," + "USERID='" + sUserID + "' WHERE "+"PROJECTID=" + this.m_DBProjectID + " AND " + "RANGEID=" + sRangeID ;
                        //cyf 20110603 modify
                             ModuleData.TempWks.ExecuteSQL(SQL);
                         //end
                         //OracleCommand Com = new OracleCommand(SQL, Con);
                         //Com.ExecuteNonQuery();
                         id = pEnumIDs.Next();
                         GetFea.set_Value(Index, 1);
                         //*********记录分配的用户**************
                         if (iUserFieldIndex >= 0) GetFea.set_Value(iUserFieldIndex, this.comboBox_USER.Text.Trim());
                         GetFea.Store();
                         //bSave = true;
                     }
                     //SDEWs.StopEditOperation();
                     //SDEWs.StopEditing(bSave);
                     SysCommon.Error.ErrorHandle.ShowInform("提示", "分配完成");
                     SetDataUpdateSymbol(GetLayer);
                     this.axMapControl2.Refresh();
                 }
            }
            catch(Exception eError)
            {
                if (ModuleData.v_SysLog != null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(eError);
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "分配错误：" + eError.Message);
            }
            
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 界面刷新后，渲染已分配的要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axMapControl2_OnAfterDraw(object sender, IMapControlEvents2_OnAfterDrawEvent e)
        {
            try
            {
               //IFeatureLayer GetLayer = this.axMapControl2.get_Layer(0) as IFeatureLayer;
               //if (GetLayer.FeatureClass.Fields.FindField("assign") < 0) return;
               //SetDataUpdateSymbol(GetLayer);
            }
            catch
            {
            }
        }
        private void SetDataUpdateSymbol(IFeatureLayer pFeatureLayer)
        {
            //if (pFeatureLayer == null || strFieldName == string.Empty) return;
            //Dictionary<string, string> dicFieldValue = new Dictionary<string, string>();
            //Dictionary<string, ISymbol> dicFieldSymbol = new Dictionary<string, ISymbol>();

            //ISymbol pSymbol = null;
            //dicFieldValue.Add("1", "已分配");
            //pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 35, 254, 7);
            //dicFieldSymbol.Add("1", pSymbol);

            ////dicFieldValue.Add("2", "修改");
            ////pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 38, 254, 7);
            ////dicFieldSymbol.Add("2", pSymbol);

            ////dicFieldValue.Add("3", "删除");
            ////pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 254, 7, 7); ;
            ////dicFieldSymbol.Add("3", pSymbol);

            //dicFieldValue.Add("0", "未分配");
            //pSymbol = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 178, 178, 178); ;
            //dicFieldSymbol.Add("0", pSymbol);

            //SetLayerUniqueValueRenderer(pFeatureLayer, strFieldName, dicFieldValue, dicFieldSymbol, false);
            string sCurrentUserName = this.comboBox_USER.Text.Trim();//////////////当前选中用户
            string sUserFieldName = "USERNAME";
            string sAssignFieldName = "assign";
            int iUserIndex = pFeatureLayer.FeatureClass.Fields.FindField(sUserFieldName);
            int iAssignIndex = pFeatureLayer.FeatureClass.Fields.FindField(sAssignFieldName);
            if (iUserIndex < 0 || iAssignIndex < 0) return;/////没有找到渲染的图层
            ISymbol pSymbolNull = null;//未变化
            pSymbolNull = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 178, 178, 178);

            ISymbol pSymbolOtherUser= null;//其他用户渲染
            pSymbolOtherUser = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 35, 255, 255);

            ISymbol pSymbolCurrentUser = null;//其他用户渲染
            pSymbolCurrentUser = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, 38, 254, 7);

            IFeatureClass pFeatCls = pFeatureLayer.FeatureClass;
            IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
            pUniqueValueRenderer.FieldCount = 2;/////////////////////////////////////////
            pUniqueValueRenderer.FieldDelimiter = "|";
            pUniqueValueRenderer.set_Field(0, sUserFieldName);          //设置渲染色
            pUniqueValueRenderer.set_Field(1, sAssignFieldName);
            pUniqueValueRenderer.DefaultSymbol = pSymbolNull;
            pUniqueValueRenderer.UseDefaultSymbol = true;////////////////////////////////
            string[] GetUnirueValues = GetUniqueValue(pFeatureLayer.FeatureClass, sUserFieldName);
            if (GetUnirueValues.Length <= 0) return;
            ///////////////获取随即色//////////////
            //IRandomColorRamp pColorRandom = new RandomColorRampClass();
            //pColorRandom.StartHue = 40;
            //pColorRandom.EndHue = 120;
            //pColorRandom.MinValue = 65;
            //pColorRandom.MaxValue = 90;
            //pColorRandom.MinSaturation = 25;
            //pColorRandom.MaxSaturation = 45;
            //pColorRandom.Size = GetUnirueValues.Length;
            //pColorRandom.Seed = 23;
            //bool IsCreateRamp = false;
            //pColorRandom.CreateRamp(out IsCreateRamp);
            //IEnumColors pEnumColors = pColorRandom.Colors;
            foreach (string GetUserName in GetUnirueValues)
            {
                ///////渲染其他用户
                //ISymbol pSymbolUser = null;//未变化
               // pSymbolUser = CreateSymbol(pFeatureLayer.FeatureClass.ShapeType, pEnumColors.Next());
                if (GetUserName == sCurrentUserName) continue;
                pUniqueValueRenderer.AddValue(GetUserName + "|1", "", pSymbolOtherUser);
               // pUniqueValueRenderer.AddValue(GetVersion.ToString() + "|1", "", pSymbol1);
            }
            //////渲染当前用户////
            pUniqueValueRenderer.AddValue(sCurrentUserName + "|1", "", pSymbolCurrentUser);
            IGeoFeatureLayer pGeoFeatLay = pFeatureLayer as IGeoFeatureLayer;
            if (pGeoFeatLay != null) pGeoFeatLay.Renderer = pUniqueValueRenderer as IFeatureRenderer;

        }
        /// <summary>
        /// 创建符号
        /// </summary>
        /// <param name="pGeometryType"></param>
        /// <param name="intR"></param>
        /// <param name="intG"></param>
        /// <param name="intB"></param>
        /// <returns></returns>
        private ISymbol CreateSymbol(esriGeometryType pGeometryType, int intR, int intG, int intB)
        {
            ISymbol pSymbol = null;
            ISimpleLineSymbol pSimpleLineSymbol = null;
            IColor pColor = GetRGBColor(intR, intG, intB);
            switch (pGeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleLineSymbol.Color = GetRGBColor(156, 156, 156);
                    pSimpleLineSymbol.Width = 0.01;
                    ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                    pSimpleFillSymbol.Outline = pSimpleLineSymbol;
                    pSimpleFillSymbol.Color = pColor;
                    pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    pSymbol = pSimpleFillSymbol as ISymbol;
                    break;
                case esriGeometryType.esriGeometryPoint:
                    ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                    pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSimpleMarkerSymbol.Color = pColor;
                    pSimpleMarkerSymbol.Size = 2;
                    pSymbol = pSimpleMarkerSymbol as ISymbol;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleLineSymbol.Color = pColor;
                    pSimpleLineSymbol.Width = 0.1;
                    pSymbol = pSimpleLineSymbol as ISymbol;
                    break;
            }

            return pSymbol;
        }
        /// <summary>
        /// 创建符号
        /// </summary>
        /// <param name="pGeometryType"></param>
        private ISymbol CreateSymbol(esriGeometryType pGeometryType, IColor in_Color)
        {
            ISymbol pSymbol = null;
            ISimpleLineSymbol pSimpleLineSymbol = null;
            IColor pColor = in_Color;
            switch (pGeometryType)
            {
                case esriGeometryType.esriGeometryPolygon:
                    pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleLineSymbol.Color = GetRGBColor(156, 156, 156);
                    pSimpleLineSymbol.Width = 0.01;
                    ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                    pSimpleFillSymbol.Outline = pSimpleLineSymbol;
                    pSimpleFillSymbol.Color = pColor;
                    pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                    pSymbol = pSimpleFillSymbol as ISymbol;
                    break;
                case esriGeometryType.esriGeometryPoint:
                    ISimpleMarkerSymbol pSimpleMarkerSymbol = new SimpleMarkerSymbolClass();
                    pSimpleMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pSimpleMarkerSymbol.Color = pColor;
                    pSimpleMarkerSymbol.Size = 2;
                    pSymbol = pSimpleMarkerSymbol as ISymbol;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSimpleLineSymbol = new SimpleLineSymbolClass();
                    pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                    pSimpleLineSymbol.Color = pColor;
                    pSimpleLineSymbol.Width = 0.1;
                    pSymbol = pSimpleLineSymbol as ISymbol;
                    break;
            }

            return pSymbol;
        }


        /// <summary>
        /// 设置图层渲染UniqueValueRenderer
        /// </summary>
        /// <param name="pFeatLay">渲染图层</param>
        /// <param name="strFieldName">渲染字段</param>
        /// <param name="dicFieldValue">渲染值对(字段值,渲染名称)</param>
        /// <param name="dicFieldSymbol">渲染Symbol对(字段值,Symbol)</param>
        private void SetLayerUniqueValueRenderer(IFeatureLayer pFeatLay, string strFieldName, Dictionary<string, string> dicFieldValue, Dictionary<string, ISymbol> dicFieldSymbol, bool bUseDefaultSymbol)
        {
            if (pFeatLay == null || strFieldName == string.Empty || dicFieldValue == null || dicFieldSymbol == null) return;
            IFeatureClass pFeatCls = pFeatLay.FeatureClass;
            IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();
            pUniqueValueRenderer.FieldCount = 1;
            pUniqueValueRenderer.set_Field(0, strFieldName);
            if (bUseDefaultSymbol == true)
            {
                pUniqueValueRenderer.UseDefaultSymbol = true;
            }
            else
            {
                pUniqueValueRenderer.UseDefaultSymbol = false;
            }
            foreach (KeyValuePair<string, string> keyValue in dicFieldValue)
            {
                if (dicFieldSymbol.ContainsKey(keyValue.Key))
                {
                    pUniqueValueRenderer.AddValue(keyValue.Key, "", dicFieldSymbol[keyValue.Key]);
                    pUniqueValueRenderer.set_Label(keyValue.Key, keyValue.Value);
                }
            }

            IGeoFeatureLayer pGeoFeatLay = pFeatLay as IGeoFeatureLayer;
            if (pGeoFeatLay != null) pGeoFeatLay.Renderer = pUniqueValueRenderer as IFeatureRenderer;
        }
        /// <summary>
        /// 获取RGB
        /// </summary>
        /// <param name="lngR"></param>
        /// <param name="lngG"></param>
        /// <param name="lngB"></param>
        /// <returns></returns>
        private IRgbColor GetRGBColor(int lngR, int lngG, int lngB)
        {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = lngR;
            rgbColor.Green = lngG;
            rgbColor.Blue = lngB;
            rgbColor.UseWindowsDithering = false;

            return rgbColor;
        }

        private void btn_test_Click(object sender, EventArgs e)
        {

        }
         /// <summary>
         /// 获取一个要素集某个字段的唯一值
         /// </summary>
         /// <param name="pFeatureClass">IFeatureClass对象</param>
         /// <param name="strFld">获取唯一值的字段</param>
         /// <returns>string[]唯一值列表</returns>
　　     public static string[] GetUniqueValue(IFeatureClass pFeatureClass,string strFld)
　　     {
　　         //得到IFeatureCursor游标
　　         IFeatureCursor pCursor=pFeatureClass.Search(null,false);   　　
　　         //coClass对象实例生成
　　         IDataStatistics pdata=new DataStatisticsClass();
             pdata.Field = strFld;
             pdata.Cursor = pCursor as ICursor;   　　
　　         //枚举唯一值
             IEnumerator pEnumVar = pdata.UniqueValues; 　　
　　         //记录总数
             int RecordCount = pdata.UniqueValueCount;  　　
　　         //字符数组
　　         string[] strValue=new string[RecordCount]; 
　　         pEnumVar.Reset();   　　 
　　         int i=0; 　　
　　         while(pEnumVar.MoveNext())
　　         {
　　            strValue[i++]=pEnumVar.Current.ToString();
　　         } 　　 
　　         return strValue;
          }

        /// <summary>
        /// 取消一个区域的分配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancleAss_Click(object sender, EventArgs e)
        {
            if (this.axMapControl2.LayerCount <= 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请加载范围图层");
                return;
            }
            try
            {
                IFeatureLayer GetLayer = this.axMapControl2.get_Layer(0) as IFeatureLayer;
                IFeatureClass GetFeaCls = GetLayer.FeatureClass;
                IFeatureSelection fealSel = GetLayer as IFeatureSelection;
                string sUserName = this.comboBox_USER.Text;
                string sUserID = this.comboBox_USER.SelectedValue.ToString();
                //cyf  20110603 modify
                ////////////////////判断是否建立了版本////////////////////
                //IVersionedObject pVersion = this.m_SDETaskLayer as IVersionedObject;
                //if (!pVersion.IsRegisteredAsVersioned) pVersion.RegisterAsVersioned(true);
                ////////////////////////////////////////////////
                //end
                //cyf 20110603 modify
                //OracleConnection Con = new OracleConnection(ModuleData.v_AppConnStr);
                IWorkspaceEdit SDEWs = (this.m_SDETaskLayer as IDataset).Workspace as IWorkspaceEdit;
                int iUserFieldIndex = m_SDETaskLayer.Fields.FindField("USERNAME");
                //SDEWs.StartEditing(false);
                //SDEWs.StartEditOperation();
                //bool bSave = false;
                if (fealSel.SelectionSet.Count != 0)
                {
                    //Con.Open();
                    IEnumIDs pEnumIDs = fealSel.SelectionSet.IDs;
                    int id = pEnumIDs.Next();
                    while (id != -1)
                    {
                        //////////将原有分配信息清空
                        IFeature GetFea = GetFeaCls.GetFeature(id);
                        string sRangeID = string.Empty;
                        int iAssign = 0;
                        int Index = GetFea.Fields.FindField("RANGEID");
                        if (Index < 0) sRangeID = "null";
                        else sRangeID = GetFea.get_Value(Index).ToString();
                        Index = GetFea.Fields.FindField("assign");
                        if (Index >= 0) GetFea.set_Value(Index, 0);
                        ///////删除已有的分配记录/////
                        string SQL = string.Empty;
                        SQL = "DELETE FROM updateinfo WHERE PROJECTID=" + this.m_DBProjectID + " AND " + "RANGEID=" + sRangeID;
                        //cyf 20110603 modify
                        ModuleData.TempWks.ExecuteSQL(SQL);
                        //end
                        //OracleCommand Com = new OracleCommand(SQL, Con);
                        //Com.ExecuteNonQuery();
                        id = pEnumIDs.Next();
                        //*********清空分配的用户**************
                        if (iUserFieldIndex >= 0) GetFea.set_Value(iUserFieldIndex, "");
                        GetFea.Store();
                        //bSave = true;
                    }
                    //SDEWs.StopEditOperation();
                    //SDEWs.StopEditing(bSave);
                    //end
                    SysCommon.Error.ErrorHandle.ShowInform("提示", "取消分配完成");
                    SetDataUpdateSymbol(GetLayer);
                    this.axMapControl2.Refresh();
                }
            }
            catch (Exception eError)
            {
                if (ModuleData.v_SysLog != null) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(eError);
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "取消分配错误：" + eError.Message);
            }
        }

        private void comboBox_USER_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110603 add:获得用户对应的角色ID
            string pUserIdStr = this.comboBox_USER.SelectedValue.ToString().Trim();  //用户ID
            IFeatureWorkspace pFeaWs = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWs == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                return;
            }
            IQueryDef pQueryDef = pFeaWs.CreateQueryDef();
            pQueryDef.Tables = "user_role,role";
            pQueryDef.SubFields = "user_role.ROLEID";
            pQueryDef.WhereClause = "user_role.USERID='" + pUserIdStr + "' and role.TYPEID='"+EnumRoleType.作业员.GetHashCode().ToString()+"'";
            ICursor pCursor = pQueryDef.Evaluate();
            if (pCursor == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询用户信息失败！");
                return;
            }
            IRow pRow = pCursor.NextRow();
            if (pRow != null)
            {
                m_iRoleId = pRow.get_Value(0).ToString();
            }
            //end
            //////////渲染///////////////
            if (this.axMapControl2.LayerCount > 0)
            {
                IFeatureLayer GetFirstLayer = this.axMapControl2.get_Layer(0) as IFeatureLayer;
                SetDataUpdateSymbol(GetFirstLayer);
            }
            this.axMapControl2.Refresh();
        }
        
        //cyf  20110603  add
        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            Exception outError=null;
            string proIdStr = this.cmbProject.SelectedValue.ToString().Trim();  //项目信息ID
            //获取并加载用户信息
            GetAllUser(proIdStr, out outError);
            if (outError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "加载作业员信息失败！原因：" + outError.Message);
                return;
            }
        }
    }

         

}