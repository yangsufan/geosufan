using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
namespace GeoDBATool
{
    public class ControlsCheckGridClear : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsCheckGridClear()
        {
            base._Name = "GeoDBATool.ControlsCheckGridClear";
            base._Caption = "清空检查出错信息";
            base._Tooltip = "清空检查出错信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "清空检查出错信息";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null || m_Hook.DataCheckGrid == null) return false;
                if (m_Hook.DataCheckGrid.Rows.Count > 0) return true;
                return false;
                
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

        private delegate void Clear();
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
        public override void OnClick()
        {
            ClearContent();
        }

        //清空更新信息
        private void ClearContent()
        {
            if (m_Hook.DataCheckGrid.Rows.Count > 0)
            {
                m_Hook.DataCheckGrid.DataSource = null;
                
            }
        }
    }
}
