using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GeoSysUpdate
{
    class ContrlosUploadingResults: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ContrlosUploadingResults()
        {
            base._Name = "GeoSysUpdate.ContrlosUploadingResults";
            base._Caption = " 上传成果";
            base._Tooltip = "上传成果";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "上传成果";
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
            if (_hook.ResultsTree== null) return;
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
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = true;
            sOpenFileD.Title = "选择文件";
            sOpenFileD.Filter = "制图文件(*.mxd)|*.mxd|Excel工作薄 (*.xls;*.xlsx)|*.xls;*.xlsx|Word(*.doc;*.docx)|*.doc;*.docx|Text 文档(*.txt)|*.txt|PDF 文档(*.pdf)|*.pdf|图片文件 (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp";
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                Dictionary<string, string> pDic = new Dictionary<string, string>();
                 string[] strFileName = sOpenFileD.FileNames;
                 SysCommon.CProgress vProgress = new SysCommon.CProgress();
                 vProgress.ShowDescription = true;
                 vProgress.ShowProgressNumber = true;
                 vProgress.TopMost = true;
                 vProgress.EnableCancel = true;
                 vProgress.EnableUserCancel(true);
                 vProgress.MaxValue = strFileName.Length;
                 vProgress.ProgresssValue = 0;
                 vProgress.Step = 1;
                 vProgress.ShowProgress();
                 vProgress.SetProgress("正在上传.....");
                 for (int j = 0; j < strFileName.Length; j++)
                 {
                     vProgress.SetProgress("正在上传：" + Path.GetFileName(strFileName[j].ToString()) + "成果......");
                     vProgress.ProgresssValue = vProgress.ProgresssValue + 1;
                     if (vProgress.UserAskCancel)
                     {
                         vProgress.Close();
                         break;
                     }
                     if (File.Exists(strFileName[j].ToString()))
                     {
                         string strPath = (rootNode.Tag as Dictionary<string, string>)["Path"].ToString().Trim() + "\\" + Path.GetFileName(strFileName[j].ToString());
                         if (!ModStringpro.IsSameTreeNode(rootNode, Path.GetFileName(strPath), strPath))
                         {
                             if (!ModStringpro.copyDirectory(strFileName[j].ToString(), strPath))
                             {
                                 pDic.Add(Path.GetFileName(strFileName[j].ToString()),"文件上传失败"); 
                                 continue;
                             }
                             FileInfo fi = new FileInfo(strPath);
                             if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                 fi.Attributes = FileAttributes.Normal;
                             TreeNode ResultsNode = new TreeNode(Path.GetFileName(strPath));  //创建文件类型新节点
                             Dictionary<string, string> pDicTag = new Dictionary<string, string>();
                             pDicTag.Add("Path", strPath);
                             pDicTag.Add("Type", "File");
                             ResultsNode.Tag = pDicTag;
                             switch (Path.GetExtension(strPath).ToLower())
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
                                 case ".img":
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
                             }

                             rootNode.Nodes.Add(ResultsNode);
                             rootNode.Expand();
                             pDic.Add(Path.GetFileName(strFileName[j].ToString()), "文件上传成功"); 
                         }
                         else
                         {
                             pDic.Add(Path.GetFileName(strFileName[j].ToString()), "目录中已经存在"); 
                              
                         }
                     }
                 }
                 vProgress.Close();
                 Application.DoEvents();
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