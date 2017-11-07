using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    /// <summary>
    /// 设置属性连接信息
    /// </summary>
    public class ControlsDBsConnectionProperties : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsDBsConnectionProperties()
        {
            base._Name = "GeoDBATool.ControlsDBsConnectionProperties";
            base._Caption = "设置数据库关联属性";
            base._Tooltip = "设置数据库关联属性";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "设置数据库关联属性";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110625 nodify
                if (!(m_Hook.ProjectTree.SelectedNode.DataKeyString == "DB")) return false;
                //end
                if (m_Hook.MapControl == null) return false;
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
            frmDBsConnSet pForm = new frmDBsConnSet(m_Hook);
            pForm.ShowDialog();
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
