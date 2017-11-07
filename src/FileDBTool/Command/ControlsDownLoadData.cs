using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace FileDBTool
{
    /// <summary>
    /// 数据下载  单个数据项的下载和提取
    /// </summary>
    public class ControlsDownLoadData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsDownLoadData()
        {
            base._Name = "FileDBTool.ControlsDownLoadData";
            base._Caption = "数据下载";
            base._Tooltip = "数据下载";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据下载";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree == null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATAITEM.ToString())
                {
                    if (m_Hook.ProjectTree.SelectedNode.Parent.Text == EnumDataType.控制点数据.ToString())
                        return false;
                    else
                    {
                        return true;
                    }
                    
                }
                
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
            FolderBrowserDialog GetFileInfo = new FolderBrowserDialog();
            if (GetFileInfo.ShowDialog() == DialogResult.OK)
            {
                //System.IO.FileInfo fileinfo = new System.IO.FileInfo(GetFileInfo.FileName);
                //string FileSavePath = fileinfo.DirectoryName;
                string FileSavePath = GetFileInfo.SelectedPath;
                FileSavePath = FileSavePath.Replace("\\", "/");//文件保存路径

                string serverStr = ""; //服务器
                string user = "";      //用户
                string password = "";  //密码
                string fileName = "";   //文件名
                string filePath = "";   //文件FTP路径

                DevComponents.AdvTree.Node DBNode = null;//数据库节点
                string treeNodeType = m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
                if (m_Hook.ProjectTree.SelectedNode.Name == "") return;
                filePath = m_Hook.ProjectTree.SelectedNode.Name;  //数据文件存储路径
                if (m_Hook.ProjectTree.SelectedNode.Text == "") return;
                fileName = m_Hook.ProjectTree.SelectedNode.Text;    //数据文件名
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return;

                //if (File.Exists(FileSavePath + "/" + fileName))
                //{
                //    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "存在同名文件，是否替换？"))
                //    {
                //        File.Delete(FileSavePath + "/" + fileName);
                //    }
                //    else
                //        return;
                //}
                //获得FTP地址
                DBNode = m_Hook.ProjectTree.SelectedNode;//数据库节点，根节点
                while (DBNode.Parent != null)
                {
                    DBNode = DBNode.Parent;
                }
                if (DBNode == null) return;
                if (DBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString()) return;
                if (DBNode.Name != "文件连接") return;
                XmlElement connElem = DBNode.Tag as XmlElement;
                if (connElem == null) return;
                serverStr = connElem.GetAttribute("服务器");
                user = connElem.GetAttribute("用户");
                password = connElem.GetAttribute("密码");

                if (serverStr == "" || user == "" || password == "") return;
                string error = "";
                FTP_Class clsFtp = new FTP_Class(serverStr, user, password);
                string[] fNameArr = clsFtp.GetFileList(filePath, out error);
                if (error != "Succeed")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取文件列表失败！");
                    return;
                }
                for (int j = 0; j < fNameArr.Length; j++)
                {
                    string ttName = fNameArr[j];
                    if (ttName.Contains(fileName.Substring(0, fileName.LastIndexOf('.'))))
                    {
                        if (File.Exists(FileSavePath + "/" + ttName))
                        {
                            if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "存在同名文件，是否替换？"))
                            {
                                File.Delete(FileSavePath + "/" + ttName);
                            }
                            else
                            {
                                return;
                            }
                        }
                        bool state = ModDBOperator.DownloadFile(serverStr, user, password, filePath, ttName, ttName, FileSavePath, out error);
                        if (!state)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "下载数据失败，数据文件为：\n'" + filePath + "\\" + ttName + "'");
                            return;
                        }
                    }
                }
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "下载完成！");
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}
