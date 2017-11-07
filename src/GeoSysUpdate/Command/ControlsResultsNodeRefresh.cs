using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
//zq 2012-03-21 add 成果目录树节点刷新
namespace GeoSysUpdate
{
   public  class ControlsResultsNodeRefresh: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ControlsResultsNodeRefresh()
        {
            base._Name = "GeoSysUpdate.ControlsResultsNodeRefresh";
            base._Caption = "节点刷新";
            base._Tooltip = "节点刷新";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "节点刷新";
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
                if (!Directory.Exists((selectNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
                {
                    if ((selectNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim() != "")
                    {
                        MessageBox.Show("请先测试能否连接上路径" + (selectNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim(), "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请先配置目录路径", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }
                RefreshResultsTree(selectNode);
            }
            MessageBox.Show("刷新完成","提示！",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void RefreshResultsTree(TreeNode rootNode)
        {
            if (rootNode == null) { return; }
            if (!Directory.Exists((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
            {
                return;
            }
            foreach (string d in Directory.GetFileSystemEntries((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
            {
                Dictionary<string, string> pDicTag = new Dictionary<string, string>();
                if (File.Exists(d))
                {
                    if (!ModStringpro.IsSameTreeNode(rootNode, Path.GetFileName(d), d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        TreeNode ResultsNode = new TreeNode(Path.GetFileName(d));  //创建文件类型新节点
                        pDicTag.Add("Path", d);
                        pDicTag.Add("Type", "File");
                        ResultsNode.Tag = pDicTag;
                        switch (Path.GetExtension(d).ToLower())
                        {
                            case ".mxd":
                                ResultsNode.ImageKey = "ArcMap";
                                ResultsNode.SelectedImageKey = "ArcMap";
                                break;
                            case ".doc":
                            case ".docx":
                                ResultsNode.ImageKey = "word";
                                ResultsNode.SelectedImageKey = "word";
                                break;
                            case ".xls":
                            case ".xlsx":
                                ResultsNode.ImageKey = "excel";
                                ResultsNode.SelectedImageKey = "excel";
                                break;
                            case ".jpg":
                            case ".bmp":
                            case ".png":
                            case ".jpeg":
                            case ".gif":
                                ResultsNode.ImageKey = "images";
                                ResultsNode.SelectedImageKey = "images";
                                break;
                            case ".txt":
                                ResultsNode.ImageKey = "text";
                                ResultsNode.SelectedImageKey = "text";
                                break;
                            case ".pdf":
                                ResultsNode.ImageKey = "pdf";
                                ResultsNode.SelectedImageKey = "pdf";
                                break;
                            default:
                                ResultsNode.ImageKey = "warning";
                                ResultsNode.SelectedImageKey = "warning";
                                break;
                        }
                        rootNode.Nodes.Add(ResultsNode);
                    }
                }
                else if (Directory.Exists(d) && d.ToLower().EndsWith("gdb"))
                {
                    if (!ModStringpro.IsSameTreeNode(rootNode, Path.GetFileName(d), d))
                    {
                        TreeNode GdbNode = new TreeNode(d.Substring(d.LastIndexOf("\\")+1));  //创建文件类型新节点
                        pDicTag.Add("Path", d);
                        pDicTag.Add("Type", "GDB");
                        GdbNode.Tag = pDicTag;
                        GdbNode.ImageKey = "PDB";
                        GdbNode.SelectedImageKey = "PDB";
                        rootNode.Nodes.Add(GdbNode);
                    }
                }
                else
                {
                    if (!ModStringpro.IsSameTreeNode(rootNode, Path.GetFileName(d), d))
                    {
                        TreeNode fileNode = new TreeNode(Path.GetFileName(d));  //创建文件夹类型新节点
                        pDicTag.Add("Path", d);
                        pDicTag.Add("Type", "Folder");
                        fileNode.Tag = pDicTag;
                        fileNode.ImageKey = "关闭";
                        fileNode.SelectedImageKey = "关闭";
                        rootNode.Nodes.Add(fileNode);
                        fileNode.Expand();
                        DirectoryInfo d1 = new DirectoryInfo(d);
                        if (d1.GetFileSystemInfos().Length != 0)
                        {
                            RefreshResultsTree(fileNode);
                            //DeleteFolder(d1.FullName);////递归创建节点
                        }
                    }

                }
            }
        }
        //判断当前节点下子节点是否存在相同的节点

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
            if (_AppHk.MapControl == null) return;
        }
    }
}
