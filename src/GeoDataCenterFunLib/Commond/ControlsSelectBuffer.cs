using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataCenterFunLib
{
    public class ControlsSelectBuffer : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        /// ZQ 2011 1125 modify
        private frmSelectBuffer m_frmSelectBuffer = null;
        public ControlsSelectBuffer()
        {
            base._Name = "GeoDataCenterFunLib.ControlsSelectBuffer";
            base._Caption = "选择要素缓冲查询";
            base._Tooltip = "选择要素缓冲查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "选择要素缓冲查询";
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
            /// ZQ 2011 1125 modify
            if (m_frmSelectBuffer == null)
            {
                m_frmSelectBuffer = new frmSelectBuffer(_AppHk.ArcGisMapControl, m_pAppForm.MainForm);
                m_frmSelectBuffer.WriteLog = this.WriteLog;
                m_frmSelectBuffer._pMapControl = _AppHk.MapControl;
                m_frmSelectBuffer.FormClosed += new System.Windows.Forms.FormClosedEventHandler(m_frmSelectBuffer_FormClosed);
                m_frmSelectBuffer.QueryBar = pBar;
                m_frmSelectBuffer.Show();
            }
            else
            { m_frmSelectBuffer.Activate(); }
        

        }
        //ZQ 20111125  modify
        private void m_frmSelectBuffer_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            m_frmSelectBuffer = null;
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
