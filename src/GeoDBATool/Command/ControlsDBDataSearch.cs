using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;

namespace GeoDBATool
{
   public class ControlsDBDataSearch: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;
         private Plugin.Application.IAppArcGISRef  m_Hook2;

        public ControlsDBDataSearch()
        {
            base._Name = "GeoDBATool.ControlsDBDataSearch";
            base._Caption = "数据检索";
            base._Tooltip = "数据检索";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据检索";

        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook2.CurrentControl is ISceneControl) return false;
                    if (m_Hook2.MapControl.LayerCount == 0)
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
               
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook2 as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook2 as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            FrmQueryAll pFrmQueryAll = new FrmQueryAll(m_Hook2);
            pFrmQueryAll.ShowDialog();


        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            //m_Hook = hook as Plugin.Application.IAppGISRef;
            m_Hook2 = hook as Plugin.Application.IAppArcGISRef;
            if (m_Hook2.MapControl==null) return;
        }
    }
}
