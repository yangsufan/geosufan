using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GeoUserManager
{
    class ControlGisSysSetting:Plugin.Interface.ControlRefBase
    {
        private Plugin.Application.IAppFormRef _hook;
        private UserControl _ControlSetting;

        //构造函数
        public ControlGisSysSetting()
        {
            base._Name = "GeoUserManager.ControlGisSysSetting";
            base._Caption = "配置管理";
            base._Visible = false;
            base._Enabled = false;
        }

        public override bool Visible
        {
            get
            {
                try
                {
                    if (_hook == null)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    if (_hook.CurrentSysName != base._Name)
                    {
                        base._Visible = false;
                        _ControlSetting.Visible= false;
                        ModData.v_AppPrivileges.StatusBar.Visible = false;                        
                        return false;
                    }

                    base._Visible = true;
                    _ControlSetting.Visible = true;
                    ModData.v_AppPrivileges.StatusBar.Visible = true;
                    return true;
                }
                catch
                {
                    base._Visible = false;
                    return false;
                }
            }
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_hook == null)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    if (_hook.CurrentSysName != base._Name)
                    {
                        base._Enabled = false;
                        _ControlSetting.Enabled = false;
                        ModData.v_AppPrivileges.StatusBar.Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    _ControlSetting.Enabled = true;
                    ModData.v_AppPrivileges.StatusBar.Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            _hook = hook as Plugin.Application.IAppFormRef;

            if (_hook == null) return;

            //设一下静态变量
            SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModData.v_ConfigPath, out SdeConfig.Server, out SdeConfig.Instance, out SdeConfig.Database, out SdeConfig.User, out SdeConfig.Password, out SdeConfig.Version, out SdeConfig.dbType);
            SdeConfig.Server = _hook.TempWksInfo.Server;
            SdeConfig.Instance = _hook.TempWksInfo.Service;
            SdeConfig.Database = _hook.TempWksInfo.DataBase;
            SdeConfig.User = _hook.TempWksInfo.User;
            SdeConfig.Password = _hook.TempWksInfo.PassWord;
            SdeConfig.Version = _hook.TempWksInfo.Version;
            SdeConfig.dbType = _hook.TempWksInfo.DBType;

            //权限控制
            ModData.v_AppPrivileges = new Plugin.Application.AppPrivileges(_hook.MainForm, _hook.ControlContainer, _hook.ListUserPrivilegeID, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath, _hook.ConnUser);
            //ModData.v_AppPrivileges = new Plugin.Application.AppPrivileges(_hook.MainForm, _hook.ControlContainer, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath, _hook.ConnUser);
            ModData.v_AppPrivileges.TempWksInfo = _hook.TempWksInfo;
            ModData.v_AppPrivileges.CurWksInfo = _hook.CurWksInfo;

            //加载受控制的权限项
            InitMainMenu();

            //配置管理 权限管理界面
            //_ControlSetting = new  SettingControl(this.Name, this.Caption);
            _ControlSetting = new UCRole(this.Name, this.Caption);
            _ControlSetting.Dock = DockStyle.Fill;

            _hook.MainForm.Controls.Add(_ControlSetting);
            _hook.MainForm.Controls.Add(ModData.v_AppPrivileges.StatusBar); ;
            //deleted by chulili 20110722 改为启动于数据源界面
            //_hook.MainForm.Controls.SetChildIndex(_ControlSetting,0);
            //end deleted by chulili
            ModData.v_AppPrivileges.UserInfo = "当前登陆: " + _hook.ConnUser.TrueName;
            _hook.MainForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
        }

        //在退出系统前如正在处理数据应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Plugin.Application.IAppPrivilegesRef pApp = ModData.v_AppPrivileges as Plugin.Application.IAppPrivilegesRef;
            if (pApp == null) return;
            if (pApp.CurrentThread != null)
            {
                pApp.CurrentThread.Suspend();
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "当前任务正在进行,是否终止退出?") == true)
                {
                    pApp.CurrentThread.Resume();
                    pApp.CurrentThread.Abort();
                }
                else
                {
                    pApp.CurrentThread.Resume();
                    e.Cancel = true;
                }
            }
        }

        //加载主界面上的菜单项
        private void InitMainMenu()
        {
            //得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModData.v_AppPrivileges.ControlContainer, xPath, ModData.v_AppPrivileges);
        }
    }
}
