using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace Fan.DataManagerView
{
    public class ControlCenterFrame : Plugin.Interface.ControlRefBase
    {
        private Plugin.Application.IAppFormRef _hook;
        //private SetControl _ControlSet;

        //构造函数
        public ControlCenterFrame()
        {
            base._Name = "GeoDataManagerFrame.ControlCenterFrame";
            base._Caption = "数据中心管理系统";
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
                        //if (_hook.CurrentSysName != base._Name)
                        //{
                        //    base._Visible = false;
                        //    //_ControlSet.Visible = false;
                        //    //ModFrameData.v_AppGisUpdate.StatusBar.Visible = false;
                        //    return false;
                        //}

                        //base._Visible = true;
                        //_ControlSet.Visible = true;
                        //ModFrameData.v_AppGisUpdate.StatusBar.Visible = true;
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
                    //if (_hook.CurrentSysName != base._Name)
                    //{
                    //    base._Enabled = false;
                    //    //_ControlSet.Enabled = false;
                    //    //ModFrameData.v_AppGisUpdate.StatusBar.Enabled = false;
                    //    return false;
                    //}

                    base._Enabled = true;
                    //_ControlSet.Enabled = true;
                    //ModFrameData.v_AppGisUpdate.StatusBar.Enabled = true;
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

            if (_hook == null)
                return;
            //plugin app.cs

            //(窗体，控制容器，系统功能树节点，数据树视图xml节点，数据连接信息xml节点，系统插件集合，)
            //ModFrameData.v_AppGisUpdate = new Plugin.Application.AppGidUpdate(_hook.MainForm, _hook.ControlContainer, _hook.ListUserPrivilegeID, 
                //_hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath, _hook.ConnUser);
            //ModFrameData.v_AppGisUpdate.MyDocument = new System.Drawing.Printing.PrintDocument();
      
            //_ControlSet = new SetControl(this.Name, this.Caption);

            //_hook.MainForm.Controls.Add(_ControlSet);
            //_hook.MainForm.Controls.Add(ModFrameData.v_AppGisUpdate.StatusBar);  //创建框架底部状态栏
            //20110518 当前登录用户
            //ModFrameData.v_AppGisUpdate.UserInfo = "当前登陆: " + _hook.ConnUser.TrueName;  //在底部状态栏中显示登录信息
            //_hook.MainForm.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
            //20110518  cyf  添加
  /*          _ControlSet.EnabledChanged += new EventHandler(_ControlSet_EnabledChanged); */ // Enable事件，用来触发数据库工程树图界面的初始化

        }

        //在退出系统前如正在处理数据应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           //Plugin.Application.IAppGisUpdateRef pApp = ModFrameData.v_AppGisUpdate as Plugin.Application.IAppGisUpdateRef;
            //if (pApp == null) return;
            //if (pApp.CurrentThread != null)
            //{
            //    if (pApp.CurrentThread.ThreadState != ThreadState.Stopped)
            //    {
            //        pApp.CurrentThread.Suspend();
            //        if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "当前任务正在进行,是否终止退出?") == true)
            //        {
            //            pApp.CurrentThread.Resume();
            //            pApp.CurrentThread.Abort();
            //        }
            //        else
            //        {
            //            pApp.CurrentThread.Resume();
            //            e.Cancel = true;
            //        }
            //    }
            //}
        }

        private void _ControlSet_EnabledChanged(object sender, EventArgs e)
        {
            //if (_ControlSet.Enabled)
            //{
            //}
            //else
            //{
               
            //}
        }

    }
}
