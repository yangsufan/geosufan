using System;
using Fan.Plugin.Parse;
using Fan.Common;
using Fan.Plugin;

namespace GDBM
{
    public class frmMain : BaseRibbonForm
    {
        //等待窗体
        private frmLoadProgress _frmTemp;
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
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
            m_MainPluginUI = new PluginUI();
            m_MainPluginUI.SysLogInfoChnaged += new SysLogInfoChangedHandle(SysLogInfoChnaged);
            //加载界面信息
            InitialFrm();
           // InitialSysConfig();
            _frmTemp.Close();
        }
        PluginUI m_MainPluginUI = default(PluginUI);
        private void InitialSysConfig()
        {
            Mod.m_sysConfig = new SysConfig(Mod.m_SysDbOperate);
            this.Text = Mod.m_sysConfig.SystemName;
        }
        /// <summary>
        ///初始化界面
        /// </summary>
        private void InitialFrm()
        {
            _frmTemp.SysInfo = "获取系统功能插件中...";
            _frmTemp.RefreshLable();
            //从插件文件夹中获取插件接口对象
            PluginHandle pluginHandle = new PluginHandle(Mod.m_PluginFolderPath);
            PluginCollection pluginCol = pluginHandle.GetPluginFromDLL();
            //初始化主框架对象
            Mod.v_AppForm = new Fan.Plugin.Application.AppForm(this, pluginCol);
            //分类解析、获取插件
            m_MainPluginUI.IntialModuleCommon(Mod.m_LoginUser, pluginCol);
            //根据用户权限价值窗体
            m_MainPluginUI.LoadForm(Mod.v_AppForm as Fan.Plugin.Application.IApplicationRef);
            SysLogInfoChangedEvent newEvent = new SysLogInfoChangedEvent("加载数据...");
            SysLogInfoChnaged(null, newEvent);
            //根据XML加载插件界面
            m_MainPluginUI.LoadData(Mod.v_AppForm as Fan.Plugin.Application.IApplicationRef);
        }
        private void SysLogInfoChnaged(object sender, SysLogInfoChangedEvent e)
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
            this.IsMdiContainer = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new EventHandler(frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}
