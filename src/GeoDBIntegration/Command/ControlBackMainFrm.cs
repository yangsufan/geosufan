using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GeoDBIntegration
{
    /// <summary>
    /// 返回主界面类  20101019修改
    /// </summary>
    public class ControlBackMainFrm : Plugin.Interface.ControlRefBase, Plugin.Interface.ICommandRef
    {
      
        Plugin.Application.IAppFormRef m_Hook;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ControlBackMainFrm()
        {
            base._Name = "GeoDBIntegration.ControlBackMainFrm";
            base._Caption = "返回主界面";
            base._Visible = true;
            base._Enabled = true;
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFormRef;
            if (m_Hook == null) return;
        }

        #region ICommandRef成员
        string Plugin.Interface.ICommandRef.Name
        {
            get { return ""; }
        }

        string Plugin.Interface.ICommandRef.Caption
        {
            get { return ""; }
        }

        string Plugin.Interface.ICommandRef.Tooltip
        {
            get { return ""; }
        }
        bool Plugin.Interface.ICommandRef.WriteLog
        {
            get 
            {
                return false;
            }
            set 
            {
                
            }
        }
        System.Drawing.Image Plugin.Interface.ICommandRef.Image
        {
            get { return null; }
        }

        string Plugin.Interface.ICommandRef.Category
        {
            get { return ""; }
        }

        bool Plugin.Interface.ICommandRef.Checked
        {
            get { return false; }
        }

        bool Plugin.Interface.ICommandRef.Visible
        {
            get { return true; }
        }

        bool Plugin.Interface.ICommandRef.Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MainForm == null) return false;
                return true;
            }
        }

        string Plugin.Interface.ICommandRef.Message
        {

            get
            {
                return "";
            }
        }

        void Plugin.Interface.ICommandRef.ClearMessage()
        {
           
        }

        void Plugin.Interface.ICommandRef.OnClick()
        {
            //==================================================================================
            //  chenayfei  modify 20110215  返回主界面修改
            //
            //
            //执行返回主界面操作
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "GeoDBIntegration.ControlDBIntegrationTool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);

            //========================================================================
        }

        void Plugin.Interface.ICommandRef.OnCreate(Plugin.Application.IApplicationRef hook)
        {
            //m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //if (m_Hook == null) return;
        }

        #endregion
    }
}