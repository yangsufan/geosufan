﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;


namespace GeoDataManagerFrame
{
    // *=======================================================================
    // *类功能：实现从数据库中心管理子系统页面跳转到配置管理子系统页面
    // *开发者：陈亚飞
    // *时  间：20110518
    // *=========================================================================
    public class ControlsEnterIntoConfigFramSys: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppDBIntegraRef m_Hook;  
        //构造函数
        public ControlsEnterIntoConfigFramSys()
        {
            base._Name = "GeoDataManagerFrame.ControlsEnterIntoConfigFramSys";
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
                //===========================================================
                //cyf add  20110520
                //根据用户信息判断能否进行子系统
                  if (!File.Exists(ModFrameData.v_AppDBConectXml)) return false;
                  XmlDocument xmlDoc = new XmlDocument();
                  xmlDoc.Load(ModFrameData.v_AppDBConectXml);
                  XmlNode pNode = xmlDoc.DocumentElement.SelectSingleNode(".//用户信息");
                  if (pNode == null) return false;
                  XmlElement pElem = pNode as XmlElement;
                  if (pElem == null) return false;
                  string pType = pElem.GetAttribute("type").Trim();//用户类型
                //若不是集成管理的用户，则不能进入子系统
                  if (pType != "1"&&pType!="0") return false;
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

            string pSysName = "";  //数据配置管理子系统Name
            string pSysCaption = ""; //数据配置管理子系统Caption

            pSysName = "GeoDBConfigFrame.ControlDataManagerFrame";
            //根据Name获得子系统的caption
            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModFrameData.m_SysXmlPath);
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
            if (ModFrameData.v_SysLog != null)
            {
                List<string> Pra = new List<string>();
               ModFrameData.v_SysLog.Write("进入配置管理子系统", Pra, DateTime.Now);
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