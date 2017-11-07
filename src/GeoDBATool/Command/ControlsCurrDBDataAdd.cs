using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    /// <summary>
    /// 加载目标数据  陈亚飞 添加 20101124
    /// </summary>
    public class ControlsCurrDBDataAdd : Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;
         private ControlsDBHistoryManage HisCommand = null;

        public ControlsCurrDBDataAdd()
        {
            base._Name = "GeoDBATool.ControlsCurrDBDataAdd";
            base._Caption = "加载目标数据库";
            base._Tooltip = "加载目标数据库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载目标数据库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
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
           

            frmDBPropertySet1 newFrm = new frmDBPropertySet1(m_Hook, EnumFeatureType.更新要素);
            if (newFrm.ShowDialog() == DialogResult.OK)
            {
                //清空图层
                //m_Hook.MapControl.ClearLayers();

                ModData.m_ObjWS = newFrm.OBJWS;
                if (ModData.m_ObjWS == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取目标工作空间失败！");
                    return;
                }
            }

            m_Hook.MapControl.ActiveView.Refresh();
            m_Hook.TOCControl.Update();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
