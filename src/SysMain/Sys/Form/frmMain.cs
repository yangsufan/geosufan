using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using System.Data.OracleClient;

namespace GDBM
{
    public class frmMain : SysCommon.BaseRibbonForm
    {
        //等待窗体
        private frmLoadProgress _frmTemp;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private bool _Res=false;
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        public frmMain()
        {
            //让线程等待，知道窗口句柄创建完毕
            //while (!this.IsHandleCreated)
            //{
            //    ;
            //}
            InitializeComponent();

            //frmLogin frmLogin = new frmLogin();
            //string name = "";
            //if (frmLogin.ShowDialog() == DialogResult.OK)
            //{

                //等待窗体
                _frmTemp = new frmLoadProgress();
                _frmTemp.Show(this);

                Plugin.ModuleCommon.SysLogInfoChnaged += new SysCommon.SysLogInfoChangedHandle(SysLogInfoChnaged);
                //加载界面信息
                //int pGroupFuncID=21;//角色ID信息
                //name = frmLogin.txtUser.Text;
                //InitialFrm(name);
                InitialFrm();

                _frmTemp.Close();
                
            //}
            //else
            //{
            //    return;
            //}
        }
        public frmMain(string userName, string UserPassword, string userType)
        {
            //让线程等待，知道窗口句柄创建完毕
            //while (!this.IsHandleCreated)
            //{
            //    ;
            //}
            InitializeComponent();

            //frmLogin frmLogin = new frmLogin();
            //string name = "";
            //if (frmLogin.ShowDialog() == DialogResult.OK)
            //{

            //等待窗体
            _frmTemp = new frmLoadProgress();
            _frmTemp.Show(this);

            Plugin.ModuleCommon.SysLogInfoChnaged += new SysCommon.SysLogInfoChangedHandle(SysLogInfoChnaged);
            //加载界面信息
            //int pGroupFuncID=21;//角色ID信息
            //name = frmLogin.txtUser.Text;
            //InitialFrm(name);
            InitialFrm();

            _frmTemp.Close();
            //}
            //else
            //{
            //    return;
            //}

            // *=====================================================
            // *功能：建在完毕后根据不同的用户进入到不同的子界面去
            // *开发：陈亚飞
            // *时间：20110520
            string pSysName = "";           //子系统名称
            string pSysCaption = "";        //子系统Caption
            if (userType == UserTypeEnum.SuperAdmin.GetHashCode().ToString())
            {
                //超级管理员,配置管理子系统
                pSysName = "GeoUserManager.ControlGisSysSetting";
            }
            else if (userType == UserTypeEnum.Admin.GetHashCode().ToString())
            {
                //管理员，集成管理子系统
                pSysName = "GeoDBIntegration.ControlDBIntegrationTool";
            }
            else if (userType == UserTypeEnum.CommonUser.GetHashCode().ToString())
            {
                //普通用户，中心管理子系统
                pSysName = "GeoSysUpdate.ControlSysUpdate";
            }
            //根据Name获得子系统的caption
            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(Mod.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入数据中心管理子系统界面
            ModuleOperator.InitialForm(pSysName, pSysCaption);

            //功能日志 enter feature Db Log
            
            if (Mod.v_SysLog != null)
            {
                List<string> Pra = new List<string>();
                Mod.v_SysLog.Write("进入配置管理子系统", Pra, DateTime.Now);
            }
        }
        /// <summary>
        ///初始化界面
        /// </summary>
        /// <param name="pGroupFunc">组ID</param>
        private void InitialFrm()//(string name)
        {
            //读取系统权限XML
            XmlDocument docXml = null;
            //docXml = Mod.v_SystemXml;
            //if (docXml == null&& name == "Admin")
            //{
                docXml = new XmlDocument();
                if (!File.Exists(Mod.m_SysXmlPath)) return;
                docXml.Load(Mod.m_SysXmlPath);

                List<string> lstUserPrivilegeID = new List<string>();
                lstUserPrivilegeID = Mod.v_ListUserPrivilegeID;

                Plugin.ModuleCommon.ListUserPrivilegeID = Mod.v_ListUserPrivilegeID;
                Plugin.ModuleCommon.ListUserdataPriID = Mod.v_ListUserdataPriID;

            _frmTemp.SysInfo = "获取系统功能插件中...";
            _frmTemp.RefreshLable();

            //从插件文件夹中获取插件接口对象
            Plugin.Parse.PluginHandle pluginHandle = new Plugin.Parse.PluginHandle();
            pluginHandle.PluginFolderPath = Mod.m_PluginFolderPath;
            Plugin.Parse.PluginCollection pluginCol = pluginHandle.GetPluginFromDLL();
            Mod.m_PluginCol = pluginCol;
            //初始化主框架对象
            Mod.v_AppForm = new Plugin.Application.AppForm(this,null, docXml, null, null, pluginCol, Mod.m_ResPath);
            //获得数据库连接信息
            Plugin.Application.ICustomWks tempWks = new Plugin.Application.ICustomWks();
            tempWks.Wks = Mod.TempWks;
            tempWks.Server = Mod.Server;
            tempWks.Service = Mod.Instance;
            tempWks.Database = Mod.Database;
            tempWks.User = Mod.User;
            tempWks.PassWord = Mod.Password;
            tempWks.Version = Mod.Version;
            tempWks.DBType = Mod.dbType;
            Mod.v_AppForm.TempWksInfo = tempWks;

            Plugin.Application.ICustomWks curWks = new Plugin.Application.ICustomWks();
            curWks.Wks = Mod.CurWks;
            curWks.Server = Mod.CurServer;
            curWks.Service = Mod.CurInstance;
            curWks.Database = Mod.CurDatabase;
            curWks.User = Mod.CurUser;
            curWks.PassWord = Mod.CurPassword;
            curWks.Version = Mod.CurVersion;
            curWks.DBType = Mod.CurdbType;
            Mod.v_AppForm.CurWksInfo = curWks;
            //分类解析、获取插件
            Plugin.ModuleCommon.IntialModuleCommon(lstUserPrivilegeID,docXml, Mod.m_ResPath, pluginCol, Mod.m_LogPath);
            //根据XML加载插件界面
            Plugin.ModuleCommon.LoadFormByXmlNode(Mod.v_AppForm as Plugin.Application.IApplicationRef);
            SysCommon.SysLogInfoChangedEvent newEvent = new SysCommon.SysLogInfoChangedEvent("加载数据...");
            SysLogInfoChnaged(null, newEvent);
            //根据XML加载插件界面
            Plugin.ModuleCommon.LoadData(Mod.v_AppForm as Plugin.Application.IApplicationRef);
        }
        private void SysLogInfoChnaged(object sender, SysCommon.SysLogInfoChangedEvent e)
        {
            _frmTemp.SysInfo = "";
            _frmTemp.RefreshLable();
            _frmTemp.SysInfo = e.Information;
            _frmTemp.RefreshLable();
        }
        #region Windows 窗体设计代码
        /// <summary>
        /// 窗体设计
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.SuspendLayout();
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(523, 342);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.ResumeLayout(false);

        }
        #endregion
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (_Res)
            {
                Application.Exit();
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SysCommon.FrmExit pFrm = new SysCommon.FrmExit();
            DialogResult pRes = pFrm.ShowDialog();
            pFrm = null;
            if (pRes == DialogResult.Yes)
            {
                Plugin.LogTable.Writelog("退出系统");//yjl记录用户退出系统
                Application.ExitThread();
                Application.Exit();
                System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("GDBM");
                foreach (System.Diagnostics.Process pc in pro)
                {
                    pc.Kill();
                }
            }
            else if (pRes == DialogResult.No)
            {
                Plugin.LogTable.Writelog("切换子系统");
                string exepath = Application.StartupPath;
                string strExecutablePath = Application.ExecutablePath;
                Application.ExitThread();
                Application.Exit();
                string picPath;
                System.Diagnostics.Process p = new System.Diagnostics.Process(); 
                picPath = string.Concat(System.IO.Path.GetDirectoryName(strExecutablePath), "\\GDBM.exe");
                System.Diagnostics.Process.Start(picPath);
            }
            else
            {
                e.Cancel = true;
            }

        }
    }
}
