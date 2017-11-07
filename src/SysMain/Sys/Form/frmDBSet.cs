using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using SysCommon;
using SysCommon.Gis;
using SysCommon.Error;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDatabaseManager
{
    public partial class frmDBSet : DevComponents.DotNetBar.Office2007Form
    {
        private enumWSType wsType;                 //工作库数据库类型
        public enumWSType WsType
        {
            set { wsType = value; }
            get { return wsType; }
        }

        private SysGisDataSet gisDataSet;           //数据库操作类

        /// <summary>
        /// 工作库
        /// </summary>
        private IWorkspace _Workspace;
        public IWorkspace WorkSpace
        {
            get { return _Workspace; }
            set { _Workspace = value; }
        }

        /// <summary>
        /// 现势库
        /// </summary>
        private IWorkspace _CurWorkspace;
        public IWorkspace CurWorkSpace
        {
            get { return _CurWorkspace; }
            set { _CurWorkspace = value; }
        }

        //string server;              //服务器
        //string service ;            //服务名
        //string dataBase ;           //数据库
        //string user ;               //用户名
        //string password ;           //密码
        //string version ;            //版本

        public frmDBSet()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 得到工作空间
        /// </summary>
        /// <param name="eError"></param>
        /// <returns></returns>
        public IWorkspace GetWorkspace(string server,string service,string dataBase,string user,string password,string version,enumWSType type,out Exception eError)
        {
            eError = null;
            bool result = false;
            
            if (gisDataSet == null)
            {
                gisDataSet = new SysGisDataSet();
            }

            try
            {
                switch (type)
                {
                    case enumWSType.SDE:
                        result=gisDataSet.SetWorkspace(server, service, dataBase, user, password, version, out eError);
                        break;
                    case enumWSType.PDB:
                    case enumWSType.GDB:
                        result=gisDataSet.SetWorkspace(server, wsType,out eError);
                        break;
                    default:
                        break;
                }
                if (result)
                {
                    return gisDataSet.WorkSpace;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }

        private void frmDBSet_Load(object sender, EventArgs e)
        {
            //判断现实库的连接
            try
            {
                if (File.Exists(Mod.v_ConfigPath))
                {
                    //工作库
                    string strServer, strType, strInstance, strDatabase, strUser, strPassword, strVersion;
                    SysCommon.Authorize.AuthorizeClass.GetConnectInfo(Mod.v_ConfigPath, out strServer, out strInstance, out strDatabase, out strUser, out strPassword, out strVersion, out strType);
                    this.ConSet.DatabaseType = strType;
                    this.ConSet.Server = strServer;
                    this.ConSet.Service = strInstance;
                    this.ConSet.DataBase = strDatabase;
                    this.ConSet.User = strUser;
                    this.ConSet.Password = strPassword;
                    this.ConSet.Version = strVersion;
                }
            }
            catch 
            {
                
            }
        }

        private void buttonXOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            if (_Workspace == null)
            {
                _Workspace = this.ConSet.GetWks();
            }
            if (_Workspace == null)
            {
                MessageBox.Show("无法连接指定数据库，请检查连接信息。","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //获得正式库连接信息
            string strServer = "";
            string strSevice = "";
            string strDatabase = "";
            string strUser = "";
            string strPass = "";
            string strVersion = "";
            string strType = "";

            enumWSType WsCurType = enumWSType.SDE;
            //if (_CurWorkspace == null)
            //{
            //    SysCommon.Authorize.AuthorizeClass.GetCurWks(_Workspace, out strServer, out strSevice, out strDatabase, out strUser, out strPass, out strVersion, out strType);

            //    if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQUSERVER")
            //    {
            //        WsCurType = enumWSType.SDE;
            //    }
            //    else if (strType.ToUpper() == "ACCESS")
            //    {
            //        WsCurType = enumWSType.PDB;
            //    }
            //    else if (strType.ToUpper() == "FILE")
            //    {
            //        WsCurType = enumWSType.GDB;
            //    }

            //    Exception Err = null;
            //    _CurWorkspace = GetWorkspace(strServer, strSevice, strDatabase, strUser, strPass, strVersion, WsCurType, out Err);
            //}

            if (_Workspace != null)
            {
                //进行序列化
                System.Collections.Hashtable conset = new System.Collections.Hashtable();
                ////工作库
                conset.Add("dbtype", this.ConSet.DatabaseType);
                conset.Add("server", this.ConSet.Server);
                conset.Add("service", this.ConSet.Service);
                conset.Add("database", this.ConSet.DataBase);
                conset.Add("user", this.ConSet.User);
                conset.Add("password", this.ConSet.Password);
                conset.Add("version", this.ConSet.Version);

                SysCommon.Authorize.AuthorizeClass.Serialize(conset, Mod.v_ConfigPath);

                //设一下工作库
                Mod.Server = this.ConSet.Server;
                Mod.Instance = this.ConSet.Service;
                Mod.Database = this.ConSet.DataBase;
                Mod.User = this.ConSet.User;
                Mod.Password = this.ConSet.Password;
                Mod.Version = this.ConSet.Version;
                Mod.dbType = this.ConSet.DatabaseType;
                Mod.TempWks = _Workspace;

                //初始化系统配置 added by chulili 20110531
                IWorkspaceFactory pWorkspaceFactory = null;
                IWorkspace pWorkspace = null;
                ESRI.ArcGIS.esriSystem.IPropertySet pPropertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
                //获取初始化系统配置的表格模板
                string dataPath = Application.StartupPath + "\\..\\Template\\DbInfoTemplate.gdb";
                pPropertySet.SetProperty("DATABASE", dataPath);
                pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                if (pWorkspace != null)
                {

                    try//加保护，xisheng 20110817 若连接失败弹出信息。不要挂掉、
                    { //是否覆盖：false  如果原库有系统配置表，则不覆盖
                        ModuleOperator.InitSystemByXML(pWorkspace, _Workspace, false);
                    }
                    catch(Exception err)
                    {

                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统维护表初始化失败！");
                        _Workspace = null;//连接失败后，将原来配置库设置为空 xisheng 20110817 
                        return;
                    }
                }//end  added by chulili 20110531

                this.DialogResult = DialogResult.OK;

            }
            else
            {
                if (eError != null)
                {
                    ErrorHandle.ShowInform("提示", eError.Message);
                }
            }
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}