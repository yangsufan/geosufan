using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    /// <summary>
    /// 新增 陈亚飞添加20101124
    /// </summary>
   public  class ControlsDBDataInsert: Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;

         public ControlsDBDataInsert()
        {
            base._Name = "GeoDBATool.ControlsDBDataInsert";
            base._Caption = "新增";
            base._Tooltip = "新增";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "新增";

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
            //新增操作
            //获得参照要素新增的要素
            Exception pError = null;
            ModData.m_CurOperType = EnumUpdateType.新增;
            if (ModData.m_orgMap == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请先加载参照图层");
                return;
            }
            //获得要新增的参照要素
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
