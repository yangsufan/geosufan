using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDBATool
{
    /// <summary>
    /// 加载参照数据库  陈亚飞添加 20101124
    /// </summary>
    public class ControlsDBReferDataAdd: Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;
         private ControlsDBHistoryManage HisCommand = null;

         public ControlsDBReferDataAdd()
        {
            base._Name = "GeoDBATool.ControlsDBReferDataAdd";
            base._Caption = "加载参照数据库";
            base._Tooltip = "加载参照数据库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载参照数据库";

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
            //******************************************************************************8
            //guozheng 
            if (ModData.UpDataCompareFrm != null)
            {
                ModData.UpDataCompareFrm.Show();
                ModData.UpDataCompareFrm.Activate();
                return;
            }
            //*******************************************************************************
            frmDBPropertySet1 newFrm = new frmDBPropertySet1(m_Hook, EnumFeatureType.参照要素);
            newFrm.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
