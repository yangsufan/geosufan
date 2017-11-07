using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GeoSysUpdate
{
    /// <summary>
    /// 上传数据gdb
    /// </summary>
    class ContrlosUploadGDB : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ContrlosUploadGDB()
        {
            base._Name = "GeoSysUpdate.ContrlosUploadGDB";
            base._Caption = " 上传数据(GDB)";
            base._Tooltip = "上传数据(GDB)";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "上传数据(GDB)";
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
                if ((selectNode.Tag as Dictionary<string, string>)["Type"].ToString().Trim() != "Folder")
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
            if (_hook.ResultsTree == null) return;
            TreeNode selectNode = _hook.ResultsTree.SelectedNode;
            if (selectNode != null)
            {
                UploadingResults(selectNode);
            }

        }
        private void UploadingResults(TreeNode rootNode)
        {
            if (rootNode == null) { return; }
            if (!Directory.Exists((rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim()))
            {
                return;
            }
            System.Windows.Forms.FolderBrowserDialog sOpenFileD = new System.Windows.Forms.FolderBrowserDialog();
            sOpenFileD.RootFolder = Environment.SpecialFolder.Desktop;
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                if (!sOpenFileD.SelectedPath.ToLower().EndsWith("gdb"))
                {
                    MessageBox.Show("请选择gdb数据库文件夹", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string strPathName = sOpenFileD.SelectedPath;
                string strDirtoryName = strPathName.Substring(strPathName.LastIndexOf("\\") + 1);
                Dictionary<string, string> pDic = new Dictionary<string, string>();
                if (Directory.Exists(strPathName.ToString()))
                {
                    string strPath = (rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim() + "\\" + strDirtoryName;
                    if (!ModStringpro.IsSameTreeNode(rootNode, strDirtoryName, strPath))
                    {
                        SysCommon.CProgress vProgress = new SysCommon.CProgress();
                        vProgress.ShowDescription = true;
                        vProgress.ShowProgressNumber = true;
                        vProgress.TopMost = true;
                        vProgress.EnableCancel = true;
                        vProgress.EnableUserCancel(true);
                        vProgress.ShowProgress();
                        Application.DoEvents();
                        vProgress.SetProgress("正在上传GDB数据.....");
                        try
                        {
                            ModStringpro.CopyDirectory(strPathName, (rootNode.Tag as Dictionary<string, string>)["Path"].ToString(),vProgress);
                            vProgress.Close();
                        }
                        catch
                        {
                            pDic.Add(strDirtoryName, "文件上传失败");
                            vProgress.Close();
                            return;
                        }
                        DirectoryInfo fi = new DirectoryInfo(strPath);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        TreeNode ResultsNode = new TreeNode(strDirtoryName);  //创建文件类型新节点
                        Dictionary<string, string> pDicTag = new Dictionary<string, string>();
                        pDicTag.Add("Path", strPath);
                        pDicTag.Add("Type", "GDB");
                        ResultsNode.Tag = pDicTag;
                        ResultsNode.ImageKey = "PDB";
                        ResultsNode.SelectedImageKey = "PDB";

                        rootNode.Nodes.Add(ResultsNode);
                        rootNode.Expand();
                        pDic.Add(strDirtoryName, "数据上传成功");
                    }
                    else
                    {
                        pDic.Add(strDirtoryName, "目录中已经存在");

                    }
                }

                frmUploadingList pfrmUploadingList = new frmUploadingList(pDic);
                pfrmUploadingList.ShowDialog();
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