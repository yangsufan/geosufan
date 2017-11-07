using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace GeoDBIntegration
{
    public class ControlsSetAppConnection : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppDBIntegraRef m_Hook;
        public ControlsSetAppConnection()
        {
            base._Name = "GeoDBIntegration.ControlsSetAppConnection";
            base._Caption = "设置系统维护库";
            base._Tooltip = "配置系统维护库连接信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "设置系统维护库";

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

            //执行添加数据库操作
            //*******************************************************//
            //guozheng 2010-09-28 获取系统维护库连接信息，
            //读取系统维护库中的所有数据库信息挂接到界面上
            Exception ex = null;
            clsAddAppDBConnection addAppDB = new clsAddAppDBConnection();
            string sConnect = addAppDB.SetAppDBConInfo(out ex);
            if (!string.IsNullOrEmpty(sConnect))
            {
                addAppDB.JudgeAppDbConfiguration(sConnect, out ex);
                if (ex != null)
                {
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "系统维护库库体结构错误：" + ex.Message + ",\n是否重新配置系统维护库连接信息？"))
                    {
                        sConnect = addAppDB.SetAppDBConInfo(out ex);
                    }
                    else
                        return;
                }

                while (!addAppDB.refurbish(sConnect, out ex))
                {

                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "界面初始化化失败，\n原因:" + ex.Message + ",\n是否重新配置系统维护库连接信息？"))
                    {
                        sConnect = addAppDB.SetAppDBConInfo(out ex);
                        /////将连接字符串记录下来
                        ModuleData.v_AppConnStr = sConnect;
                        //清空用户信息
                        ModuleData.m_User = null;
                    }
                    else
                    {
                        ex = new Exception("取消操作");
                        break;
                    }
                }
                if (null != ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "设置系统维护库失败，\n原因：" + ex.Message);
                    return;
                }
                else
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功！");
                    /////将连接字符串记录下来
                    ModuleData.v_AppConnStr = sConnect;

                    //清空用户信息
                    ModuleData.m_User = null;
                }
            }
            //else
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接信息失败");
            //    return;
            //}

            ////******************************************************//

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            if (m_Hook == null) return;
        }
    }
}