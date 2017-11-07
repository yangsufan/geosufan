using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    /// <summary>
    /// 根据条件提取矢量数据
    /// </summary>
    public class ControlsStractDataByCondition: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;

        public ControlsStractDataByCondition()
        {
            base._Name = "GeoDBATool.ControlsStractDataByCondition";
            base._Caption = "数据提取";
            base._Tooltip = "根据条件提取数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "根据条件提取数据";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.MapControl == null) return false;
              //  if (m_Hook.MapControl.LayerCount == 0) return false;  //wgf 20110521屏蔽 原因：退出系统的时候会报异常
                //if (m_Hook.ProjectTree.SelectedNode == null) return false;

                //if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
                //if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                //XmlNode ProNode = m_Hook.ProjectTree.SelectedNode.Tag as XmlNode;
                //if (ProNode == null) return false;
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
            FrmDataStract dataStractFrm = new FrmDataStract(m_Hook);
            dataStractFrm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
