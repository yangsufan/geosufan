using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FileDBTool
{
    /// <summary>
    /// 删除连接信息
    /// </summary>
   public class ControlsDelConnection: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

       public ControlsDelConnection()
        {
            base._Name = "FileDBTool.ControlsDelConnection";
            base._Caption = "删除连接";
            base._Tooltip = "删除连接信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除连接信息";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString()) return false;

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
            try
            {
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "是否删除连接信息？"))
                {
                    string ipStr = m_Hook.ProjectTree.SelectedNode.Text.Trim();
                    string type = m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
                    XmlElement aElem = m_Hook.ProjectTree.SelectedNode.Tag as XmlElement;
                    if (aElem == null) return;
                    string pServer = aElem.GetAttribute("服务器");
                    string pUser = aElem.GetAttribute("用户");
                    string pPassword = aElem.GetAttribute("密码");
                    string pInstance = aElem.GetAttribute("实例名");
                    string pVersion = aElem.GetAttribute("版本");
                    string pMetaStr = aElem.GetAttribute("MetaDBConn");

                    //删除xml信息
                    XmlDocument xml = new XmlDocument();
                    xml.Load(ModData.v_CoonectionInfoXML);
                    //XmlNodeList pNodes = xml.SelectNodes(".//DBCatalog//DataBase[@NodeName='" + name + "' && @NodeText='" + ipStr + "' && @NodeType='"+type+"']");
                    XmlNodeList pNodes = xml.SelectNodes(".//连接信息[@服务器='" + pServer + "' and @用户='" + pUser + "' and @密码='" + pPassword + "' and @实例名='" + pInstance + "' and @ 版本='" + pVersion + "' and @MetaDBConn='" + pMetaStr + "']");//
                    XmlNode mNode = null;
                    if (pNodes.Count > 0)
                    {
                        mNode = pNodes.Item(0);
                    }
                    if (mNode == null) return;
                    xml.ChildNodes[0].RemoveChild(mNode.ParentNode);
                    //mNode.ParentNode.RemoveChild(mNode);
                    xml.Save(ModData.v_CoonectionInfoXML);

                    //移除树节点信息
                    m_Hook.ProjectTree.SelectedNode.Remove();
                }
            }
            catch(Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
                return;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
