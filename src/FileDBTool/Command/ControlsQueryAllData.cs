using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 查找指定数据
    /// </summary>
    public class ControlsQueryAllData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;
        DevComponents.AdvTree.Node v_DBNode = null;  //数据库数节点
        DevComponents.AdvTree.Node v_ProNode = null; //项目树节点
        int m_DataTypeID=0;

        public ControlsQueryAllData()
        {
            base._Name = "FileDBTool.ControlsQueryAllData";
            base._Caption = "查找指定数据";
            base._Tooltip = "查找指定数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "查找指定数据";

        }

        public override bool Enabled
        {
            get
            {
                return false;
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.ImageIndex == 1) return false;//若未连接上，则不能查询
                string whereStr = "";
                bool b = false;
                if (m_Hook.ProjectTree.SelectedNode != null)
                {
                    #region  根据时间和其他组合条件来查询
                    string treeNodeType = m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
                    if (treeNodeType == EnumTreeNodeType.DATABASE.ToString())
                    {
                        //根据时间进行查询
                        v_DBNode = m_Hook.ProjectTree.SelectedNode;
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.PROJECT.ToString())
                    {
                        //根据项目、时间进行组合查询
                        v_ProNode = m_Hook.ProjectTree.SelectedNode;
                        if (v_ProNode.Tag == null) return false;
                        v_DBNode = v_ProNode.Parent;
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.DATAFORMAT.ToString())
                    {
                        //根据项目、数据类型（DLG\DEM\DOM\DRG）和时间来组合查询
                        v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent;
                        if (m_Hook.ProjectTree.SelectedNode.Tag == null || v_ProNode.Tag == null) return false;
                        v_DBNode = v_ProNode.Parent;
                       
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.PRODUCT.ToString())
                    { //根据产品和时间来组合查询
                        v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent.Parent;
                        v_DBNode = v_ProNode.Parent;
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.PRODUCTPYPE.ToString())
                    {  //根据产品、产品类型（标准图幅、非标准图幅、属性表）和时间来组合查询

                        v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent.Parent.Parent;
                        v_DBNode = v_ProNode.Parent;
                        b = true;
                    }
                    else
                    {
                        return false;
                    }
                    if (v_DBNode == null)
                    {
                        return false;
                    }
                    if (v_DBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString())
                    {
                        return false;
                    }
                    if (v_DBNode.Name != "文件连接")
                    {
                        return false;
                    }

                    #endregion
                }
                return b;
               
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
           FrmQueryAll pFrmQueryAll=new FrmQueryAll();
           pFrmQueryAll.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }


    
    }
}
