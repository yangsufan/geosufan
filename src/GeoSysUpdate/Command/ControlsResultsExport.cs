using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GeoSysUpdate
{
    public class ControlsResultsExport : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        private Plugin.Application.AppGidUpdate _hook;
        public ControlsResultsExport()
        {
            base._Name = "GeoSysUpdate.ControlsResultsExport";
            base._Caption = "导出成果";
            base._Tooltip = "导出成果";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导出成果";
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
                if ((selectNode.Tag as Dictionary<string, string>)["Type"].ToString().Trim() == "Root") return false; 
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
                Dictionary<string,string> pDic =selectNode.Tag as Dictionary<string,string>;
                if (pDic != null)
                {

                    FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
                    if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (CopyResults(pDic["Path"].ToString().Trim(),  pFolderBrowserDialog.SelectedPath, pDic["Type"].ToString().Trim()))
                        {
                            MessageBox.Show("导出成功！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        MessageBox.Show("导出失败！！", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
               
            }

        }
        /// <summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        private bool CopyResults(string dir, string strCopyPath, string strTpye)
        {
            switch (strTpye)
            {
                case "File":
                    if (File.Exists(dir))
                    {
                        FileInfo fi = new FileInfo(dir);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        try
                        {
                            fi.CopyTo(strCopyPath + "\\" + fi.Name, true);
                        }
                        catch { return false; }
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case "Folder":
                    if (Directory.Exists(dir))
                    {
                        SysCommon.CProgress vProgress = new SysCommon.CProgress();
                        vProgress.ShowDescription = true;
                        vProgress.ShowProgressNumber = true;
                        vProgress.TopMost = true;
                        vProgress.EnableCancel = true;
                        vProgress.EnableUserCancel(true);
                        vProgress.ShowProgress();
                        Application.DoEvents();
                        vProgress.SetProgress("正在导出目录.....");
                        string flag = ModStringpro.CopyFolder(dir, strCopyPath, vProgress);
                        vProgress.Close();
                        if (flag != "success")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case "GDB":
                    if (Directory.Exists(dir))
                    {
                        SysCommon.CProgress vProgress = new SysCommon.CProgress();
                        vProgress.ShowDescription = true;
                        vProgress.ShowProgressNumber = true;
                        vProgress.TopMost = true;
                        vProgress.EnableCancel = true;
                        vProgress.EnableUserCancel(true);
                        vProgress.ShowProgress();
                        Application.DoEvents();
                        vProgress.SetProgress("正在导出GDB数据.....");
                        try
                        {
                            ModStringpro.CopyDirectory(dir, strCopyPath, vProgress);
                            vProgress.Close();
                        }
                        catch
                        {
                            vProgress.Close();
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    break;
            }
            return true;

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