using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Xml;

namespace FileDBTool
{
    /// <summary>
    /// 控制点数据导出
    /// </summary>
    public class ControlsOutputControlData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsOutputControlData()
        {
            base._Name = "FileDBTool.ControlsOutputControlData";
            base._Caption = "控制点数据导出";
            base._Tooltip = "控制点数据导出";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "控制点数据导出";

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
                //if (m_Hook.ProjectTree.SelectedNode.Text != EnumDataType.控制点数据.ToString()) return false;
                for (int i = 0; i < m_Hook.DataInfoGrid.RowCount; i++)
                {
                    if (m_Hook.DataInfoGrid.Rows[i].Selected)
                    {
                        string savePath = m_Hook.DataInfoGrid.Rows[i].Cells["存储位置"].FormattedValue.ToString().Trim();
                        if (savePath.Contains("控制点数据"))
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
            Exception eError = null;

            #region 通过树节点获取源数据库的连接地址
            DevComponents.AdvTree.Node DBNode = null;
            string treeNodeType = m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
            if (treeNodeType == "") return;
            DBNode = m_Hook.ProjectTree.SelectedNode;//数据库节点，根节点
            while (DBNode.Parent != null)
            {
                DBNode = DBNode.Parent;
            }

            if (DBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString()) return;
            if (DBNode.Name != "文件连接") return;
            XmlElement connElem = DBNode.Tag as XmlElement;
            if (connElem == null) return;
            string conStr = connElem.GetAttribute("MetaDBConn");
            
            #endregion

            SaveFileDialog saveDial = new SaveFileDialog();
            saveDial.Title = "保存数据";
            saveDial.Filter = "控制点数据(*.mdb)|*.mdb";
            if (saveDial.ShowDialog() == DialogResult.OK)
            {
                FrmProcessBar frmbar = new FrmProcessBar();
                frmbar.Show();

                string pFileName = saveDial.FileName;
                FileInfo pFileInfo = new FileInfo(ModData.v_TempControlDB);
                FileInfo desFileInfo = pFileInfo.CopyTo(pFileName, true);         //文件存储路径加名称
                string path = desFileInfo.DirectoryName;                            //文件存储路径                           

                //连接目标表
                SysCommon.DataBase.SysTable desSysTable = new SysCommon.DataBase.SysTable();
                desSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pFileName + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + pFileName);
                    desSysTable.CloseDbConnection();
                    return;
                }
                //获得目标表格
                DataTable desTable = desSysTable.GetTable("metadata", out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message + "在库中不存在表格'metadata'，请检查！");
                    desSysTable.CloseDbConnection();
                    return;
                }

                //insert 语句
                string insertStr = "insert into metadata (";
                for (int i = 1; i < desTable.Columns.Count; i++)
                {
                    //第一列为自动编号ID，从第二列开始
                    insertStr += desTable.Columns[i].ColumnName + ",";
                }
                insertStr = insertStr.Substring(0, insertStr.Length - 1) + ")";
                insertStr += " values (";

                //连接源数据库
                SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
                ////pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + conStr + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + conStr);
                    pSysTable.CloseDbConnection();
                    desSysTable.CloseDbConnection();
                    return;
                }
                //源数据表格
                DataTable ordDt = pSysTable.GetTable("ControlPointMDTable", out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询控制点元信息出错！");
                    pSysTable.CloseDbConnection();
                    desSysTable.CloseDbConnection();
                    return;
                }
                pSysTable.CloseDbConnection();

                if (desTable.Columns.Count + 5 != ordDt.Columns.Count)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标数据库与源数据库字段结构不一致！");
                    desSysTable.CloseDbConnection();
                    return;
                }
                #region 遍历要导出的数据
                //设置进度条最大值
                frmbar.SetFrmProcessBarMax(m_Hook.DataInfoGrid.SelectedRows.Count);
                int pValue = 0;
                for (int i = 0; i < m_Hook.DataInfoGrid.RowCount; i++)
                {
                    if (m_Hook.DataInfoGrid.Rows[i].Selected)
                    {
                        string savePath=m_Hook.DataInfoGrid.Rows[i].Cells["存储位置"].FormattedValue.ToString().Trim();
                        if (!savePath.Contains("控制点数据"))
                        {
                            //其他数据导出
                            //pValue++;
                            //frmbar.SetFrmProcessBarValue(pValue);
                            continue;
                        }
                        frmbar.SetFrmProcessBarText("正在进行下载数据：" + m_Hook.DataInfoGrid.Rows[i].Cells["数据文件名"].FormattedValue.ToString().Trim());

                        if (m_Hook.DataInfoGrid.Rows[i].Cells["ID"].FormattedValue.ToString().Trim() == "") return;
                        long dataID = Convert.ToInt64(m_Hook.DataInfoGrid.Rows[i].Cells["ID"].FormattedValue.ToString().Trim());
                        //执行导出操作
                        OutPutOneRacord(dataID, insertStr, ordDt, desSysTable, path, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            desSysTable.CloseDbConnection();
                            frmbar.Dispose();
                            frmbar.Close();
                            return;
                        }
                        pValue++;
                        frmbar.SetFrmProcessBarValue(pValue);
                    }
                }
                #endregion

                frmbar.Dispose();
                frmbar.Close();

            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }

        /// <summary>
        /// 导出一条记录
        /// </summary>
        /// <param name="dataID">数据ID</param>
        /// <param name="insertStr">插入字符串</param>
        /// <param name="orgTable">源表格</param>
        /// <param name="desSysTable">目标表格</param>
        /// <param name="path">存储路径</param>
        /// <param name="eError"></param>
        private void OutPutOneRacord(long dataID,string insertStr,DataTable orgTable,SysCommon.DataBase.SysTable desSysTable,string path,out Exception eError)
        {
            eError = null;
            string insertallStr = insertStr;
            string pointName = "";//控制点名

            DataRow[] drs = orgTable.Select("ID =" +dataID);
            if (drs.Length == 0)
            {
                eError = new Exception("未找到ID为：'" + dataID + "'的记录");
                return;
            }
            DataRow dr = drs[0];
            for (int i = 5; i < dr.Table.Columns.Count - 1; i++)
            {
                Type columType = dr.Table.Columns[i].DataType;
                string columValue = dr[i].ToString();
                string str = ModDBOperator.GetSQlEX(columType, columValue);
                insertallStr += str + ",";
            }
            insertallStr = insertallStr.Substring(0, insertallStr.Length - 1) + ")";
            //导出元数据
            desSysTable.UpdateTable(insertallStr, out eError);
            if(eError!=null)
            {
                return;
            }

            //创建数据文件
            pointName = dr[5].ToString();
            string pDataName = path + "\\" + pointName + ".dwg";       //数据文件名
            if(File.Exists(pDataName))
            {
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "已存在同名文件，是否替换？"))
                {
                    File.Delete(pDataName);
                }
                else 
                {
                    return;
                }
            }
            if(dr[dr.Table.Columns.Count - 1].ToString()!="")
            {
                byte[] picByte = dr[dr.Table.Columns.Count - 1] as byte[];
                if (picByte.Length > 0)
                {
                    FileStream pFStream = new FileStream(pDataName, FileMode.CreateNew, FileAccess.Write);
                    pFStream.Write(picByte, 0, picByte.Length);
                    MemoryStream meStream = new MemoryStream(picByte, true);
                    meStream.Write(picByte, 0, picByte.Length);
                }
            }
        }
    }
}

