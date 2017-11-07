using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    public class ControlsCreateRasterDBStruct : Plugin.Interface.CommandRefBase
    {
     private Plugin.Application.IAppGISRef m_Hook;

        public ControlsCreateRasterDBStruct()
        {
            base._Name = "GeoDBATool.ControlsCreateRasterDBStruct";
            base._Caption = "构建栅格数据库库体";
            base._Tooltip = "根据GeoOne Schema文件创建栅格数据库库体";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "根据GeoOne Schema文件创建栅格数据库库体";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
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
            FrmCreateRasterDB DBStructCreator = new FrmCreateRasterDB(m_Hook);
            DBStructCreator.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}