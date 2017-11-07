using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 删除指定项目及相关信息
    /// </summary>
    public class ControlsDelProject: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsDelProject()
        {
            base._Name = "FileDBTool.ControlsDelProject";
            base._Caption = "删除项目";
            base._Tooltip = "删除项目及其相关的所有信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除项目及其相关的所有信息";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //若没有选中项目节点，则不可用
                if (EnumTreeNodeType.PROJECT.ToString()!=m_Hook.ProjectTree.SelectedNode.DataKey.ToString()) return false;
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
            //执行删除项目操作

            SysCommon.Error.frmInformation eerorFrm = new SysCommon.Error.frmInformation("是", "否", "删除项目将删除项目下的所有产品和数据文件，\n 确定吗？");
            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
            if (eerorFrm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            ///////////////////////// 删除服务器上所有的文件夹目录////////////////////
            DevComponents.AdvTree.Node SelNode = m_Hook.ProjectTree.SelectedNode;
            DevComponents.AdvTree.Node ConNode = m_Hook.ProjectTree.SelectedNode.Parent;
            if (null == ConNode) return;
            string DelProName = SelNode.Text;
            System.Xml.XmlElement Ele = ConNode.Tag as System.Xml.XmlElement;
            if (null == Ele)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", "获取工程元信息失败！");
                return;
            }
            string ip = Ele.GetAttribute("服务器");
            string id = Ele.GetAttribute("用户");
            string password = Ele.GetAttribute("密码");
            long ProjectID = int.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());//项目的ID 
            string ipStr = Ele.GetAttribute("MetaDBConn");
            //string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
            string ConnStr = ipStr;
            clsProject DelPro = new clsProject(DelProName, ip, id, password);
            string error = "";
            bool DelState=DelPro.DeleteProject(SelNode, ConnStr,ProjectID,out error);
            if (!DelState)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", error);
                return;
            }
            ///////////////////////// 删除元信息表中记录信息////////////////////
            
            Exception ex = null;
            ModDBOperator.DelProject(ProjectID, ConnStr, out ex);
            if (null != ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", ex.Message);
            }
            else
            {
                m_Hook.ProjectTree.SelectedNode.Remove();
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目删除成功");
            }
            m_Hook.DataInfoGrid.DataSource = null;    
            m_Hook.MetaDataGrid.DataSource = null;
            //刷新时间列表框
            Exception eError = null;
            ModDBOperator.LoadComboxTime(ConnStr, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载时间列表框失败，" + eError.Message);
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
