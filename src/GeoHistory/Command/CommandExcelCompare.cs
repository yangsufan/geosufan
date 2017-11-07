using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace GeoHistory
{
    public class CommandExcelCompare : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        private Plugin.Application.IAppFormRef m_pAppForm;
        public CommandExcelCompare()
        {
            base._Name = "GeoHistory.CommandExcelCompare";
            base._Caption = "报表对比";
            base._Tooltip = "报表对比";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = false;
            base._Message = "报表对比";

        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return true;
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
            if (m_pAppForm == null)
            {
                return;
            }
            FormExcelCompare pfrm = new FormExcelCompare();
            pfrm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;


            m_pAppForm = _AppHk as Plugin.Application.IAppFormRef;
        }
        

    }
}
