using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoDBIntegration
{
    class ControlsDBDataRemove : Plugin.Interface.CommandRefBase
    {
          private Plugin.Application.IAppDBIntegraRef m_Hook;
          public ControlsDBDataRemove()
        {
            base._Name = "GeoDBIntegration.ControlsDBDataRemove";
            base._Caption = "卸载数据库图层";
            base._Tooltip = "卸载数据库图层";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "卸载数据库图层";

        }
        public override bool Enabled
        {
            get
            {
             //   if (this.m_Hook.MapControl.LayerCount <= 0) return false;  //wgf 20110521 原因：退出系统会报异常错误
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
            ////////////////////卸载相应的图层///////////////////////////
            DevComponents.AdvTree.Node SelectNode = this.m_Hook.ProjectTree.SelectedNode;
            if (SelectNode == null) return;
            string LayerName = SelectNode.DataKeyString + "_" + SelectNode.Text;
            for (int i = 0; i < this.m_Hook.MapControl.LayerCount; i++)
            {
                ILayer GetLayer = this.m_Hook.MapControl.get_Layer(i);
                if (GetLayer.Name==LayerName)
                {
                    this.m_Hook.MapControl.DeleteLayer(i);
                    break;
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            if (m_Hook == null) return;
        }
    }
}
