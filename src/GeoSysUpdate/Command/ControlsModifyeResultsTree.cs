using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GeoSysUpdate
{
    class ControlsModifyeResultsTree: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ControlsModifyeResultsTree()
        {
            base._Name = "GeoSysUpdate.ControlsModifyeResultsTree";
            base._Caption = "修改目录名称";
            base._Tooltip = "修改目录名称";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改目录名称";
            //base._Image = "";
            //base._Category = "";
        }
        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_hook.ResultsTree == null) return false;
                TreeNode selectNode = _hook.ResultsTree.SelectedNode;
                if (selectNode == null) return false;
                if (selectNode.Level == 0) return false;
                if ((selectNode.Tag as Dictionary<string, string>)["Type"].ToString().Trim() == "File" || (selectNode.Tag as Dictionary<string, string>)["Type"].ToString().Trim() == "GDB")
                {
                    return false;
                }
                return true;
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (_hook.ResultsTree== null) return;
            TreeNode selectNode = _hook.ResultsTree.SelectedNode;
            if (selectNode != null)
            {
                ModifyResultsTreeName(selectNode);
            }
            
        }
        private void ModifyResultsTreeName(TreeNode rootNode)
        {
            if (rootNode == null) { return; }
            if (!Directory.Exists((rootNode.Tag as Dictionary<string,string>)["Path"].ToString().Trim()))
            {
                return;
            }
            frmResultsTreeName pfrmResultsTreeName = new frmResultsTreeName();
            pfrmResultsTreeName.femText = "修改目录名称";
            pfrmResultsTreeName.LabText = "修改名称：";
            if (pfrmResultsTreeName.ShowDialog() == DialogResult.OK)
            {
                string strNewName = pfrmResultsTreeName.ResultsTreeName;
                if (strNewName == "") return;
                string strPath = Path.GetDirectoryName((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()) + "\\" + strNewName;
                if (!ModStringpro.IsSameTreeNode(rootNode.Parent, strNewName, strPath))
                {
                    try
                    {
                        Directory.Move((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim(), strPath);
                        rootNode.Text = strNewName;
                        Dictionary<string, string> pDicTag = new Dictionary<string, string>();
                        pDicTag.Add("Path", strPath);
                        pDicTag.Add("Type", "Folder");
                        rootNode.Tag = pDicTag;
                    }
                    catch { MessageBox.Show("修改失败！","提示！", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
            if (_AppHk.MapControl == null) return;
        }
    }
}