using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBATool
{
    /// <summary>
    /// SDE数据库服务管理
    /// </summary>
    public class ControlsArcSDEManager: Plugin.Interface.CommandRefBase
    {
        //string WorkState = "";//任务状态
        private Plugin.Application.IAppGISRef m_Hook;
        //private XmlDocument m_CaseXmlDoc;                                              //选中的作业工程xml
        public ControlsArcSDEManager()
        {
            base._Name = "GeoDBATool.ControlsArcSDEManager";
            base._Caption = "ArcSDE连接信息管理";
            base._Tooltip = "ArcSDE连接信息管理";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "ArcSDE连接信息管理";

        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;
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
            //**********************************************
            //guozheng added
            if (ModData.SysLog != null)
            {
                ModData.SysLog.Write("ArcSDE连接信息管理", null, DateTime.Now);
            }
            else
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write("ArcSDE连接信息管理", null, DateTime.Now);
            }
            //*********************************************

            frmSDEManager pFrmSDEManager = new frmSDEManager();
            pFrmSDEManager.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
