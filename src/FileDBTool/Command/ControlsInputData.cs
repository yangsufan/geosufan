using System;
using System.Collections.Generic;
using System.Text;

namespace FileDBTool
{
    /// <summary>
    /// 执行成果数据入库
    /// </summary>
    public class ControlsInputData :Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsInputData()
        {
            base._Name = "FileDBTool.ControlsInputData";
            base._Caption = "成果数据入库";
            base._Tooltip = "成果数据入库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "成果数据入库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //若没有选中产品节点的子节点（成果数据类型节点），则不可用
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.PRODUCTPYPE.ToString()) return false;

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
            //执行成果数据入库操作（包括数据入库、元数据入库、在成果索引表中增加相应记录）。需记录入库时间，以便管理历史
            //=================================================================================================================
            //陈亚飞添加
            //将信息写入数据库，成果索引库
            string text = "数据入库";
            FrmProjectSetting frmProSetting = new FrmProjectSetting(m_Hook.ProjectTree.SelectedNode, text);
            frmProSetting.ShowDialog();


            /////执行数据入库//////////
            //string Path = "";
            //string IP = "";
            //string ID = "";
            //string Password = "";
            //string ProjectID = "";
            //string Filename = "";
            //Exception ex = null;
            //ModDBOperator.GetTreePathProInfo(m_Hook.ProjectTree, out ProjectID, out Path, out IP, out ID, out Password, out ex);
            //if (null != ex)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", ex.Message);
            //    return;
            //}
            //System.Windows.Forms.OpenFileDialog UoloadFrm = new System.Windows.Forms.OpenFileDialog();
            ///////////////////////////////////////////////////
            ////////////////加入数据入库界面//////////////////
            ////////////////加入数据入库界面///////////////////
            //////////////////////////////////////////////////
            //string fname = "";//文件名
            //string fPath = "";//文件路径
            //if (System.Windows.Forms.DialogResult.OK == UoloadFrm.ShowDialog())
            //{
            //    string FileName = UoloadFrm.FileName;
            //    System.IO.FileInfo Finfo = new System.IO.FileInfo(FileName);
            //    fname = Finfo.Name;
            //    fPath = Finfo.DirectoryName;
            //    string error = "";
            //    if (!ModDBOperator.UpLoadFile(IP, ID, Password, fPath, fname, Path, fname, out error))
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", error);
            //    }

            //}
           
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}


