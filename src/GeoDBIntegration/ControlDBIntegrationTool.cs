using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using System.IO;

namespace GeoDBIntegration
{
    /// <summary>
    /// 陈亚飞编写  20100927
    /// </summary>
    public class ControlDBIntegrationTool: Plugin.Interface.ControlRefBase
    {
        private Plugin.Application.IAppFormRef _hook;
        private UserControlDBIntegra _ControlDBIntegra;

        //构造函数

        public ControlDBIntegrationTool()
        {
            base._Name = "GeoDBIntegration.ControlDBIntegrationTool";
            base._Caption = "数据库集成管理子系统";
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
                            _ControlDBIntegra.Visible = false;
                            ModuleData.v_AppDBIntegra.StatusBar.Visible = false;
                            return false;
                        }
                        //ModDBOperate.WriteLog("_Visible start");
                        base._Visible = true;
                        //ModDBOperate.WriteLog("_ControlDBIntegra.Visible  start");
                        _ControlDBIntegra.Visible = true;
                        //ModDBOperate.WriteLog("_ControlDBIntegra.Visible end");
                        ModuleData.v_AppDBIntegra.StatusBar.Visible = true;
                        //ModDBOperate.WriteLog("v_AppDBIntegra.StatusBar.Visible end");
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
                    if (_hook != null)
                    {
                        if (_hook.CurrentSysName != base._Name)
                        {
                            base._Enabled = false;
                            _ControlDBIntegra.Enabled = false;
                            ModuleData.v_AppDBIntegra.StatusBar.Enabled = false;
                            return false;
                        }

                        base._Enabled = true;
                        _ControlDBIntegra.Enabled = true;
                        ModuleData.v_AppDBIntegra.StatusBar.Enabled = true;
                        return true;
                    }
                    else
                    {
                        base._Enabled = false;
                        return false;
                    }
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
            ModuleData.v_AppDBIntegra = new Plugin.Application.AppDBIntegra(_hook.MainForm, _hook.ControlContainer, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath,_hook.ConnUser,_hook.LstRoleInfo);

            _ControlDBIntegra = new UserControlDBIntegra(this.Name, this.Caption);

            _ControlDBIntegra.Show();

            _hook.MainForm.Controls.Add(_ControlDBIntegra);
            _hook.MainForm.Controls.SetChildIndex(_ControlDBIntegra, 0);//added by chulili 20110722该界面位于顶层
            _hook.MainForm.Controls.Add(ModuleData.v_AppDBIntegra.StatusBar);
            //added by chulili 20110722设置状态栏
            ModuleData.v_AppDBIntegra.UserInfo = "当前登陆: " + _hook.ConnUser.TrueName;
            ModuleData.v_AppDBIntegra.CurScaleVisible = false;
            ModuleData.v_AppDBIntegra.RefScaleVisible = false;
            //end added by chulili
            _hook.MainForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
            //ModuleData.v_AppDBIntegra.RefScaleCmb.SelectedIndexChanged += new EventHandler(RefScaleCmb_SelectedIndexChanged);
            //ModuleData.v_AppDBIntegra.CurScaleCmb.SelectedIndexChanged += new EventHandler(CurScaleCmb_SelectedIndexChanged);
            //ModuleData.v_AppDBIntegra.RefScaleCmb.key

            //添加回车事件自定义比例尺
            //DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = ModuleData.v_AppDBIntegra.CurScaleCmb.ComboBoxEx;
            //vComboEx.KeyDown += new KeyEventHandler(vComboEx_KeyDown);
           
        }

        //在退出系统前如正在处理数据应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Plugin.Application.IAppSMPDRef pApp = ModuleData.v_AppDBIntegra as Plugin.Application.IAppSMPDRef;
            if (pApp == null)
            {
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Teminate();
                return;
            }
            if (pApp.CurrentThread != null)
            {
                pApp.CurrentThread.Suspend();
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "当前任务正在进行,是否终止退出?") == true)
                {
                    pApp.CurrentThread.Abort();
                }
                else
                {
                    pApp.CurrentThread.Resume();
                    e.Cancel = true;
                }
            }
        }
        //响应回车时间 改变当前显示比例尺
        void vComboEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = sender as DevComponents.DotNetBar.Controls.ComboBoxEx;
            string strScale = vComboEx.Text;
            double dblSacle = 0;

            try
            {
                if (double.TryParse(strScale, out dblSacle))
                {
                    ModuleData.v_AppDBIntegra.MapControl.Map.MapScale = dblSacle;
                    ModuleData.v_AppDBIntegra.MapControl.ActiveView.Refresh();
                }
                else
                {
                    vComboEx.Text = ModuleData.v_AppDBIntegra.MapControl.Map.MapScale.ToString();
                }
            }
            catch
            {
            }
        }
        //参考比例尺事件　陈亚飞添加

        private void RefScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    ModuleData.v_AppDBIntegra.MapControl.Map.ReferenceScale = Convert.ToDouble(ModuleData.v_AppDBIntegra.RefScaleCmb.SelectedItem.ToString().Trim());
            //}
            //catch
            //{
            //    ModuleData.v_AppDBIntegra.RefScaleCmb.Text = ModuleData.v_AppDBIntegra.MapControl.Map.ReferenceScale.ToString("0");
            //}
            //ModuleData.v_AppDBIntegra.MapControl.ActiveView.Refresh();
        }
        //当前比例尺事件 陈亚飞添加

        private void CurScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModuleData.v_AppDBIntegra.MapControl.Map.MapScale = Convert.ToDouble(ModuleData.v_AppDBIntegra.CurScaleCmb.SelectedItem.ToString().Trim());
            }
            catch
            {
                ModuleData.v_AppDBIntegra.CurScaleCmb.Text = ModuleData.v_AppDBIntegra.MapControl.Map.MapScale.ToString("0");
            }
            ModuleData.v_AppDBIntegra.MapControl.ActiveView.Refresh();
        }
    }
    
}
