using System;
using System.Collections.Generic;
using System.Text;

/*-----------------------------------------------------------
 added by xisheng 20110805 缓冲坐标菜单文件 ControlsXYBufferSet.cs
 -----------------------------------------------------------*/
namespace GeoDataCenterFunLib
{
    public class ControlsXYBufferSet : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        frmXYBufferset m_frm = null;
        public ControlsXYBufferSet()
        {
            base._Name = "GeoDataCenterFunLib.ControlsXYBufferSet";
            base._Caption = "坐标缓冲查询";
            base._Tooltip = "坐标缓冲查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "坐标缓冲查询";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            Plugin.Application.IAppGisUpdateRef phook = _AppHk as Plugin.Application.IAppGisUpdateRef;
            SysCommon.BottomQueryBar pBar = phook.QueryBar;
            if (pBar.m_WorkSpace == null)
            {
                pBar.m_WorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            }
            if (pBar.ListDataNodeKeys == null)
            {
                pBar.ListDataNodeKeys = Plugin.ModuleCommon.ListUserdataPriID;
            }
            if (_AppHk == null) 
                return;
            if (m_frm != null)
            {
                m_frm = null;
            }
            m_frm = new frmXYBufferset(_AppHk.MapControl, m_pAppForm.MainForm);
            m_frm.WriteLog = this.WriteLog;
            m_frm.QueryBar = pBar;
            m_frm.Show();
           
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            if (_AppHk.MapControl == null) return;
            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
       
    }
}
