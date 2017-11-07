using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    /// <summary>
    /// 修改  陈亚飞20101124添加
    /// </summary>
    public class ControlsDBDataUpdate: Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;

         public ControlsDBDataUpdate()
        {
            base._Name = "GeoDBATool.ControlsDBDataUpdate";
            base._Caption = "修改";
            base._Tooltip = "修改";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改";

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
            //修改操作
            //获得参照要素
            Exception pError = null;
            ModData.m_CurOperType = EnumUpdateType.修改;
            if (ModData.m_orgMap == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请先加载参照图层");
                return;
            }
            //获得参照要素
            ModData.m_OrgFeature = ClsUpdate.getFea(ModData.m_orgMap, EnumFeatureType.参照要素, out pError);
            if (pError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", pError.Message);
                return;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
