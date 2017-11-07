using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSysSetting
{
    class ControlsCatalogOutport: Plugin.Interface.CommandRefBase
    {
        private GeoSysSetting.SubControl.UCDataSourceManger ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlsCatalogOutport()
        {
            base._Name = "GeoSysSetting.ControlsCatalogOutport";
            base._Caption = "目录导出";
            base._Tooltip = "将系统中的目录导出为XML";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "目录导出";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return true;
                //根据时间刷新一下改空间是否显示
                if (_hook.Visible == false && this.ucCtl != null)
                {
                    this.ucCtl.Visible = false;
                }
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            Exception eError;

            //if (ucCtl == null)
            //{
            //    ucCtl = new SubControl.UCDataSourceManger();
            //    ucCtl.Dock = DockStyle.Fill;
            //    ucCtl.m_TmpWorkSpace = _hook.TempWksInfo.Wks;
                
                
            //    _hook.MainForm.Controls.Add(ucCtl);
            //}

            //ucCtl.Visible = true;
            //_hook.MainForm.Controls.SetChildIndex(ucCtl, 0);
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }

    }
}
