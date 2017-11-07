using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
// zq add 2012-03-21   新建目录
namespace GeoSysUpdate
{
   public  class ControlsNewResultsTree: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ControlsNewResultsTree()
        {
            base._Name = "GeoSysUpdate.ControlsNewResultsTree";
            base._Caption = "新建目录";
            base._Tooltip = "新建目录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "新建目录";
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
                NewResultsTreeName(selectNode);
            }
            
        }
        private void NewResultsTreeName(TreeNode rootNode)
        {
            if (rootNode == null) { return; }
            if (!Directory.Exists((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
            {
                return;
            }
            frmResultsTreeName pfrmResultsTreeName = new frmResultsTreeName();
            pfrmResultsTreeName.femText = "创建目录";
            pfrmResultsTreeName.LabText = "创建名称：";
            if (pfrmResultsTreeName.ShowDialog() == DialogResult.OK)
            {
                string strNewName = pfrmResultsTreeName.ResultsTreeName;
                if (strNewName == "") return;
                string strPath = (rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim() + "\\" + strNewName;
                if (!ModStringpro.IsSameTreeNode(rootNode, strNewName, strPath))
                {
                    TreeNode fileNode = new TreeNode(Path.GetFileName(strPath));  //创建文件夹类型新节点
                    Dictionary<string, string> pDicTag = new Dictionary<string, string>();
                    pDicTag.Add("Path", strPath);
                    pDicTag.Add("Type", "Folder");
                    fileNode.Tag = pDicTag;
                    fileNode.ImageKey = "关闭";
                    fileNode.SelectedImageKey = "关闭";
                    rootNode.Nodes.Add(fileNode);
                    fileNode.Expand();
                    Directory.CreateDirectory(strPath);
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
