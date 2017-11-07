using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
namespace FileDBTool
{
    class ControlsGetProjectRange:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;
        public ControlsGetProjectRange()
        {
            base._Name = "FileDBTool.ControlsGetProjectRange";
            base._Caption = "获取底图";
            base._Tooltip = "获取项目的底图范围";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "获取项目的底图范围";

        }

        public override bool Enabled
        {
            get
            {
                //if (m_Hook == null) return false;
                //if (m_Hook.CurrentThread != null) return false;
                //if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                //if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.PROJECT.ToString())
                //    return false;
                return false;
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
          ///////执行获取底图操作/////////////   
            DevComponents.AdvTree.Node mDBNode = m_Hook.ProjectTree.SelectedNode;
            DevComponents.AdvTree.Node ProjectNode = m_Hook.ProjectTree.SelectedNode;
            string ipStr = "";
            string ConnStr = "";
            long ProjectId = -1;
            Exception ex=null;
            ///////获取工程项目节点
            while (EnumTreeNodeType.PROJECT.ToString()!= ProjectNode.DataKey.ToString())
            {
                if (ProjectNode.Parent == null)
                    return;
                ProjectNode = ProjectNode.Parent;
            }
            /////获取连接节点
            while (mDBNode.Parent != null)
            {
                mDBNode = mDBNode.Parent;
            }
            if (mDBNode.Name == "文件连接")
            {
                System.Xml.XmlElement dbElem = mDBNode.Tag as System.Xml.XmlElement;
                if (dbElem == null) return;
                ipStr = dbElem.GetAttribute("MetaDBConn");
                ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
                try
                {
                    string pId = ProjectNode.Tag.ToString();
                    ProjectId = Convert.ToInt64(pId);
                }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示","获取工程项目ID失败!");
                    return;
                }
                /////////////////进行底图的获取/////////////////////
               // ModDBOperator.AddProjectMapRange(ProjectId, ipStr, out ex);
                if (null != ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示0", "获取底图失败！");
                }
                //////////////////////////////////////////////////////////////////////////////

            }


        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
       
    }
}
