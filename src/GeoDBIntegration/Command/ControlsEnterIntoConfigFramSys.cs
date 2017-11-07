using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;



namespace GeoDBIntegration
{
    // *=======================================================================
    // *类功能：实现从数据库集成管理子系统页面跳转到配置管理子系统页面
    // *开发者：陈亚飞
    // *时  间：20110518
    // *=========================================================================
    public class ControlsEnterIntoConfigFramSys: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;  
        //构造函数
        public ControlsEnterIntoConfigFramSys()
        {
            base._Name = "GeoDBIntegration.ControlsEnterIntoConfigFramSys";
            base._Caption = "配置管理";
            base._Tooltip = "配置管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "进入配置管理子系统";
        }

        //设置按钮的Enable属性
        public override bool Enabled
        {
            get 
            {
                return true;
            }
        }

        //设置按钮的message
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

        //清楚按钮的message
        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        //按钮的click事件
        public override void OnClick()
        {

            string pSysName = "";  //配置管理子系统Name
            string pSysCaption = ""; //配置管理子系统Caption

            pSysName = "GeoUserManager.ControlGisSysSetting";
            //根据Name获得子系统的caption
            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入数据中心管理子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);

            //功能日志 enter feature Db Log
            if (ModuleData.v_SysLog != null)
            {
                List<string> Pra = new List<string>();
                ModuleData.v_SysLog.Write("进入配置管理子系统", Pra, DateTime.Now);
            }
        }

        //创建一个按钮
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
        }

    }
}