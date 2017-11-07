using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDBATool
{
    public class ControlsLogicCheck : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsLogicCheck()
        {
            base._Name = "GeoDBATool.ControlsLogicCheck";
            base._Caption = "森林数据逻辑检查";
            base._Tooltip = "森林数据逻辑检查";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "森林数据逻辑检查";

        }

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
            FrmLogicCheckSet newfrm = new FrmLogicCheckSet();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14 写日志
            }
            newfrm.Show();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
