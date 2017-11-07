using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//?using GeoDataCenterFrame;
using Plugin;
using System.Threading;

namespace GeoDBConfigFrame
{
    public class ControlDataManagerFrame : Plugin.Interface.ControlRefBase
    {
        private Plugin.Application.IAppFormRef _hook;
        private FaceControl _ControlFace;

        //构造函数
        public ControlDataManagerFrame()
        {
            base._Name = "GeoDBConfigFrame.ControlDataManagerFrame";
            base._Caption = "数据配置系统";
            base._Visible = false;
            base._Enabled = false;
        }

        public override bool Visible
        {
            get
            {
                try
                {
                    if (_hook != null)
                    {
                        if (_hook.CurrentSysName != base._Name)
                        {
                            base._Visible = false;
                            _ControlFace.Visible = false;
                            ModFrameData.v_AppPrivileges.StatusBar.Visible = false;
                            return false;
                        }

                        base._Visible = true;
                        _ControlFace.Visible = true;
                        ModFrameData.v_AppPrivileges.StatusBar.Visible = true;
                        return true;
                    }
                    else
                    {
                        base._Visible = false;
                        return false;
                    }
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
                        _ControlFace.Enabled = false;
                        ModFrameData.v_AppPrivileges.StatusBar.Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    _ControlFace.Enabled = true;
                    ModFrameData.v_AppPrivileges.StatusBar.Enabled = true;
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
           // SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModFrameData.v_ConfigPath, out SdeConfig.Server, out SdeConfig.Instance, out SdeConfig.Database, out SdeConfig.User, out SdeConfig.Password, out SdeConfig.Version, out SdeConfig.dbType);
           // ModFrameData.v_AppPrivileges = new Plugin.Application.AppPrivileges(_hook.MainForm, _hook.ControlContainer, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath, _hook.ConnUser);
            ModFrameData.v_AppPrivileges = new Plugin.Application.AppPrivileges(_hook.MainForm, _hook.ControlContainer, _hook.ListUserPrivilegeID, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath, _hook.ConnUser);
            _ControlFace = new FaceControl(this.Name, this.Caption);
            _hook.MainForm.Controls.Add(_ControlFace);
            _hook.MainForm.Controls.Add(ModFrameData.v_AppPrivileges.StatusBar);
            ModFrameData.v_AppPrivileges.UserInfo = "当前登陆: " + _hook.ConnUser.TrueName;
            _hook.MainForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
        }

        //在退出系统前如正在处理数据应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Plugin.Application.IAppPrivilegesRef pApp = ModFrameData.v_AppPrivileges as Plugin.Application.IAppPrivilegesRef;
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

    }
}
