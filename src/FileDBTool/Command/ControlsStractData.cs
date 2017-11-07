using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace FileDBTool
{
    /// <summary>
    /// 数据下载 批量数据项的下载和提取
    /// </summary>
    public class ControlsStractData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsStractData()
        {
            base._Name = "FileDBTool.ControlsStractData";
            base._Caption = "成果数据提取";
            base._Tooltip = "将指定成果数据项提取出来";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "将指定成果数据项提取出来";

        }

        public override bool Enabled
        {
            get
            {
                bool b = false;
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree == null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.DataInfoGrid == null||m_Hook.DataInfoGrid.RowCount==0) return false;
                if (!m_Hook.DataInfoGrid.Columns.Contains("ID")) return false;
                if (!m_Hook.DataInfoGrid.Columns.Contains("数据文件名")) return false;
                if (!m_Hook.DataInfoGrid.Columns.Contains("存储位置")) return false;
                if (m_Hook.DataInfoGrid.RowCount == 0 && m_Hook.DataInfoGrid.Rows[0].Cells["ID"].FormattedValue.ToString()== "") return false;
                if (m_Hook.ProjectTree.SelectedNode.Text == EnumDataType.控制点数据.ToString()) return false;
                for (int i = 0; i < m_Hook.DataInfoGrid.RowCount; i++)
                {
                    if (m_Hook.DataInfoGrid.Rows[i].Selected)
                    {
                        string savePath = m_Hook.DataInfoGrid.Rows[i].Cells["存储位置"].FormattedValue.ToString().Trim();
                        if (savePath.Contains("标准图幅") || savePath.Contains("非标准图幅"))
                        {
                            b = true;
                            break;
                        }
                    }
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
            if (m_Hook.DataInfoGrid.Rows.Count <= 0) return;
            FolderBrowserDialog GetFileInfo = new FolderBrowserDialog();
            if (GetFileInfo.ShowDialog()==DialogResult.OK )
            {
                //System.IO.FileInfo fileinfo = new System.IO.FileInfo(GetFileInfo.FileName);
                string FileSavePath = GetFileInfo.SelectedPath;
                FileSavePath = FileSavePath.Replace("\\", "/");//文件保存路径
                
                //string FileSaveName = fileinfo.Name;           //文件保存名
               
                string serverStr = ""; //服务器
                string user = "";      //用户
                string password = "";  //密码

                #region 通过树节点获取IP地址
                DevComponents.AdvTree.Node DBNode = null;
                string treeNodeType = m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
                if (treeNodeType == "") return;
                DBNode = m_Hook.ProjectTree.SelectedNode;//数据库节点，根节点
                while(DBNode.Parent!=null)
                {
                    DBNode=DBNode.Parent;
                }
                 
                if (DBNode == null) return;
                if(DBNode.DataKey.ToString()!=EnumTreeNodeType.DATABASE.ToString()) return;
                if(DBNode.Name!="文件连接") return;
                XmlElement connElem = DBNode.Tag as XmlElement;
                if (connElem == null) return;
                serverStr = connElem.GetAttribute("服务器");
                user = connElem.GetAttribute("用户");
                password = connElem.GetAttribute("密码");

                if (serverStr == "" || user == "" || password == "") return;
                //string conStr = connElem.GetAttribute("MetaDBConn");//元数据库连接信息
                #endregion

                for (int i = 0; i < m_Hook.DataInfoGrid.RowCount; i++)
                {
                    if (false == m_Hook.DataInfoGrid.Rows[i].Selected)
                        continue;
                    string fileName = "";   //文件名
                    string filePath = "";   //文件FTP路径
                    fileName = m_Hook.DataInfoGrid.Rows[i].Cells["数据文件名"].FormattedValue.ToString().Trim();
                    filePath = m_Hook.DataInfoGrid.Rows[i].Cells["存储位置"].FormattedValue.ToString().Trim();
                    if (fileName == "" || filePath == "") return;
                   
                    string error = "";
                    //执行下载数据
                    FTP_Class clsFtp = new FTP_Class(serverStr, user, password);
                    string[] fNameArr = clsFtp.GetFileList(filePath,out error);
                    if(error!="Succeed")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取文件列表失败！");
                        return;
                    }
                    if (fNameArr == null)
                    {
                        //控制点数据导出，特殊处理
                        continue;
                    }
                    else
                    {
                        #region 其他数据下载
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
                                        return;
                                }
                                bool state = ModDBOperator.DownloadFile(serverStr, user, password, filePath, ttName, ttName, FileSavePath, out error);
                                if (!state)
                                {
                                    SysCommon.Error.frmInformation eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件\n'" + filePath + "\\" + ttName + "'\n下载失败。原因为：\n" + error + "！\n是否继续下载？");
                                    eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                                    if (eerorFrm.ShowDialog() == DialogResult.OK)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }

                        }
                        #endregion
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

