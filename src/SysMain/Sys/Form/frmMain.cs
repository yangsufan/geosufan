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
    public class frmMain : Fan.Common.BaseRibbonForm
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
            InitializeComponent();
            //等待窗体
            _frmTemp = new frmLoadProgress();
            _frmTemp.Show(this);
            Fan.Plugin.ModuleCommon.SysLogInfoChnaged += new Fan.Common.SysLogInfoChangedHandle(SysLogInfoChnaged);
            //加载界面信息
            InitialFrm();
            _frmTemp.Close();
        }
        /// <summary>
        ///初始化界面
        /// </summary>
        /// <param name="pGroupFunc">组ID</param>
        private void InitialFrm()
        {
            XmlDocument docXml = null;
            docXml = new XmlDocument();
            if (!File.Exists(Mod.m_SysXmlPath)) return;
            docXml.Load(Mod.m_SysXmlPath);
            List<string> lstUserPrivilegeID = new List<string>();

            lstUserPrivilegeID = Mod.v_ListUserPrivilegeID;
            Fan.Plugin.ModuleCommon.ListUserPrivilegeID = Mod.v_ListUserPrivilegeID;
            Fan.Plugin.ModuleCommon.ListUserdataPriID = Mod.v_ListUserdataPriID;
            _frmTemp.SysInfo = "获取系统功能插件中...";
            _frmTemp.RefreshLable();
            //从插件文件夹中获取插件接口对象
            Fan.Plugin.Parse.PluginHandle pluginHandle = new Fan.Plugin.Parse.PluginHandle();
            pluginHandle.PluginFolderPath = Mod.m_PluginFolderPath;
            Fan.Plugin.Parse.PluginCollection pluginCol = pluginHandle.GetPluginFromDLL();
            Mod.m_PluginCol = pluginCol;
            //初始化主框架对象
            Mod.v_AppForm = new Fan.Plugin.Application.AppForm(this, null, docXml, null, null, pluginCol, Mod.m_ResPath);
            //获得数据库连接信息
            Fan.Plugin.Application.ICustomWks tempWks = new Fan.Plugin.Application.ICustomWks();
            tempWks.Wks = Mod.TempWks;
            tempWks.Server = Mod.Server;
            tempWks.Service = Mod.Instance;
            tempWks.Database = Mod.Database;
            tempWks.User = Mod.User;
            tempWks.PassWord = Mod.Password;
            tempWks.Version = Mod.Version;
            tempWks.DBType = Mod.dbType;
            Mod.v_AppForm.TempWksInfo = tempWks;

            Fan.Plugin.Application.ICustomWks curWks = new Fan.Plugin.Application.ICustomWks();
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
            Fan.Plugin.ModuleCommon.IntialModuleCommon(lstUserPrivilegeID, docXml, Mod.m_ResPath, pluginCol, Mod.m_LogPath);
            //根据XML加载插件界面
            Fan.Plugin.ModuleCommon.LoadFormByXmlNode(Mod.v_AppForm as Fan.Plugin.Application.IApplicationRef);
            Fan.Common.SysLogInfoChangedEvent newEvent = new Fan.Common.SysLogInfoChangedEvent("加载数据...");
            SysLogInfoChnaged(null, newEvent);
            //根据XML加载插件界面
            Fan.Plugin.ModuleCommon.LoadData(Mod.v_AppForm as Fan.Plugin.Application.IApplicationRef);
        }
        private void SysLogInfoChnaged(object sender, Fan.Common.SysLogInfoChangedEvent e)
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 690);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
