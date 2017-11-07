using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevComponents.AdvTree;

namespace FileDBTool
{
    /// <summary>
    /// 设置成果库连接信息
    /// </summary>
    public class ControlsConnectionSetting:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsConnectionSetting()
        {
            base._Name = "FileDBTool.ControlsConnectionSetting";
            base._Caption = "连接设置";
            base._Tooltip = "设置库体连接信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "设置库体连接信息";

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
                if (m_Hook.ProjectTree.SelectedNode.Name == "") return false;
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
            //设置成果库连接信息
            //获取树节点上工程节点的连接IP信息
            DevComponents.AdvTree.Node SelNode = m_Hook.ProjectTree.SelectedNode;
            string NodeType = SelNode.DataKey.ToString().Trim();
            if (EnumTreeNodeType.DATABASE.ToString()== NodeType)
            {
                if (SelNode.Name == "文件连接")
                {
                    XmlElement Ele = SelNode.Tag as XmlElement;
                    if(Ele==null)
                    {
                        frmConSet frmconset = new frmConSet("", "", "", "");
                        frmconset.ShowDialog();
                    }
                    else 
                    {
                        string ConType = Ele.GetAttribute("实例名");
                        if (null == ConType || "" == ConType)
                        {
                            string ip = Ele.GetAttribute("服务器");
                            string id = Ele.GetAttribute("用户");
                            string password = Ele.GetAttribute("密码");
                            string metaConn = Ele.GetAttribute("MetaDBConn");
                            frmConSet frmconset = new frmConSet(ip, id, password, metaConn);
                            frmconset.ShowDialog();
                        }
                    }
                }
                else if(SelNode.Name=="空间连接")
                {

                }
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
