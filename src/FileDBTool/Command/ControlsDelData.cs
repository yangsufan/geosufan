using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

namespace FileDBTool
{
    /// <summary>
    /// 删除成果数据文件、相关的元数据信息、成果索引表中相应的索引
    /// </summary>
    public class ControlsDelData:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsDelData()
        {
            base._Name = "FileDBTool.ControlsDelData";
            base._Caption = "成果数据删除";
            base._Tooltip = "删除成果数据项及其相关的元数据信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除成果数据项及其相关的元数据信息";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (null != m_Hook.ProjectTree.SelectedNode)
                {
                    if (EnumTreeNodeType.DATAITEM.ToString() != m_Hook.ProjectTree.SelectedNode.DataKey.ToString() && m_Hook.DataInfoGrid.RowCount == 0)
                        return false;
                    if (EnumTreeNodeType.DATAITEM.ToString() != m_Hook.ProjectTree.SelectedNode.DataKey.ToString() && !m_Hook.DataInfoGrid.Columns.Contains("ID"))
                        return false;
                    if (EnumTreeNodeType.DATAITEM.ToString() != m_Hook.ProjectTree.SelectedNode.DataKey.ToString() && !m_Hook.DataInfoGrid.Columns.Contains("数据文件名"))
                        return false;
                    if (EnumTreeNodeType.DATAITEM.ToString() != m_Hook.ProjectTree.SelectedNode.DataKey.ToString() && !m_Hook.DataInfoGrid.Columns.Contains("存储位置"))
                        return false;
                    if (EnumTreeNodeType.DATAITEM.ToString() != m_Hook.ProjectTree.SelectedNode.DataKey.ToString() && !m_Hook.DataInfoGrid.Columns.Contains("生产日期"))
                        return false;
                }
                else
                {
                    if (m_Hook.DataInfoGrid.RowCount == 0 || !m_Hook.DataInfoGrid.Columns.Contains("ID") || !m_Hook.DataInfoGrid.Columns.Contains("数据文件名") || !m_Hook.DataInfoGrid.Columns.Contains("存储位置") || !m_Hook.DataInfoGrid.Columns.Contains("生产日期"))
                        return false;
                }
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
            SysCommon.Error.frmInformation eerorFrm = new SysCommon.Error.frmInformation("是", "否", "删除数据将无法恢复，确定吗？");
            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
            if (eerorFrm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            //执行成果数据删除操作（在列表中选中要删除的数据项，进行删除）

            Exception ex=null;
            #region 获取连接信息
            DevComponents.AdvTree.Node mDBNode = m_Hook.ProjectTree.SelectedNode;
            DevComponents.AdvTree.Node Deltreenode = null;
            string ipStr="";
            string ip="";
            string id="";
            string password="";
            string ConnStr="";
            int datatype = -1;
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();    //属性库连接类
            
            while (mDBNode.Parent != null)
            {
                mDBNode = mDBNode.Parent;
            }
            if (mDBNode.Name == "文件连接")
            {
                Deltreenode = mDBNode;
                System.Xml.XmlElement dbElem = mDBNode.Tag as System.Xml.XmlElement;
                if (dbElem == null) return;
                ipStr = dbElem.GetAttribute("MetaDBConn");
                ip = dbElem.GetAttribute("服务器");
                id = dbElem.GetAttribute("用户");
                password = dbElem.GetAttribute("密码");
                //ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
                ConnStr = ipStr;
                pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out ex);
            }
            else return;
            #endregion
            if (EnumTreeNodeType.DATAITEM.ToString() == m_Hook.ProjectTree.SelectedNode.DataKey.ToString())
            {
              #region 在树节点上选择要删除的数据
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return;
                if (m_Hook.ProjectTree.SelectedNode.Tag.ToString() == "") return;
                long dataID = int.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());//数据
                string DataType = m_Hook.ProjectTree.SelectedNode.Parent.Tag.ToString();;
                try
                {
                    datatype = Convert.ToInt32(DataType);
                }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示！", "获取数据类型失败！");
                    return;
                }
                if (2 != datatype)////数据类型不为控制点控制点数据

                {
                    string FilePath = m_Hook.ProjectTree.SelectedNode.Name;
                    string FileName = m_Hook.ProjectTree.SelectedNode.Text.Trim();
                    bool Delstate = ModDBOperator.DelDataItem(FilePath, FileName, ip, id, password, out ex);
                    if (!Delstate)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "文件库中删除文件:"+FilePath+"/"+FileName+"失败！"+"\n请确认文件是否存在或是否为占用状态！");
                        return;
                    }
                    m_Hook.ProjectTree.SelectedNode.Remove();
                    ModDBOperator.DelDataItem(dataID, datatype, pSysDB, out ex);
                    if (null != ex)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "元信息数据库中删除数据文件记录失败!");
                        return;
                    }
                }
                else/////数据类型为控制点数据

                {
                    ModDBOperator.DelDataItem(dataID, datatype, pSysDB, out ex);
                    if (null != ex)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "元信息数据库中删除数据文件记录失败!");
                        return;
                    }
                    m_Hook.ProjectTree.SelectedNode.Remove();
                }
              #endregion
            }
            else if (m_Hook.DataInfoGrid.Rows.Count > 0)
            {
                #region 其他所有节点
                DevComponents.AdvTree.Node Selnode = m_Hook.ProjectTree.SelectedNode;
                if (null == Selnode.DataKey) return;
                string ID="";
                long lid=-1;
                string FileName="";
                string FilePath="";
                string type="";
                
                FrmProcessBar frmbar = new FrmProcessBar();
                frmbar.SetFrmProcessBarText("删除操作:");
                frmbar.Show();
                List<int> delrowlist = new List<int>();
                if (EnumTreeNodeType.PRODUCTPYPE.ToString() == Selnode.DataKey.ToString())
                {
                    #region 选中的是产品类型节点(标准、非标准、控制点数据)
                    type = Selnode.Text;
                    switch (type)
                    {
                        case "标准图幅":
                            datatype = 0;
                            break;
                        case "非标准图幅":
                            datatype = 1;
                            break;
                        case "控制点数据":
                            datatype = 2;
                            break;
                        default:
                            datatype = -1;
                            break;
                    }
                    if (datatype == -1) return;
                    frmbar.SetFrmProcessBarMax((long)m_Hook.DataInfoGrid.Rows.Count);
                    for (int i = 0; i < m_Hook.DataInfoGrid.Rows.Count; i++)
                    {
                        frmbar.SetFrmProcessBarValue((long)i);
                        Application.DoEvents(); 
                        if (false == m_Hook.DataInfoGrid.Rows[i].Selected) continue;
                        try
                        {
                            ID = m_Hook.DataInfoGrid.Rows[i].Cells["ID"].FormattedValue.ToString().Trim();
                            if (string.IsNullOrEmpty(ID))
                                continue;
                            lid = Convert.ToInt64(ID);
                            FileName = m_Hook.DataInfoGrid.Rows[i].Cells["数据文件名"].FormattedValue.ToString().Trim();
                            FilePath = m_Hook.DataInfoGrid.Rows[i].Cells["存储位置"].FormattedValue.ToString().Trim();
                        }
                        catch 
                        {
                            eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件:"+FileName+"删除失败,是否继续删除？");
                            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                            if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                continue;
                            }
                            else
                            {
                                frmbar.Close();
                                return;
                            }
                        }
                        
                        #region 在文件库中删除数据文件

                        //////////////////在文件库中删除数据文件////////////////////////////////
                        frmbar.SetFrmProcessBarText("正在删除文件：" + FileName);
                        Application.DoEvents(); 
                        if (2 != datatype)////数据类型为控制点控制点数据不执行文件的删除操作

                        {
                            bool Delstate = ModDBOperator.DelDataItem(FilePath, FileName, ip, id, password, out ex);
                            if (!Delstate)
                            {
                                eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件\n'" + FilePath + "/" + FileName + "'\n删除失败。\n" + "请确认文件是否存在或是否为占用状态" + "！\n是否继续删除？");
                                eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                                if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    continue;
                                }
                                else
                                {
                                    frmbar.Close();
                                    return;
                                }
                            }
                        }
                        #endregion
                        #region 在元数据表中删除成果信息
                        ///////////////////在元数据表中删除成果信息/////////////////////////
                        ModDBOperator.DelDataItem(lid, datatype, pSysDB, out ex);
                        if (null != ex)
                        {
                            eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件\n'" + FileName + "'\n元信息删除失败。\n" + "请确认元信息库的连接信息。" + "！\n是否继续删除？");
                            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                            if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                continue;
                            }
                            else
                            {
                                frmbar.Close();
                                return;
                            }
                        }
                       // m_Hook.DataInfoGrid.Rows.Remove(m_Hook.DataInfoGrid.Rows[i]);
                       // i = i - 1;
                        delrowlist.Add(i);
                        #endregion
                        #region 在树节点中删除文件节点

                        /////////////////////在树节点中删除文件节点///////////////////////////
                        ModDBOperator.DelNodeByNameAndText(Deltreenode, FilePath, FileName, out ex);
                        if (null != ex)
                        {
                            eerorFrm = new SysCommon.Error.frmInformation("是", "否", "树节点删除失败。原因为：\n" + ex.Message + "！\n是否继续删除？");
                            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                            if (eerorFrm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                frmbar.Close();
                                return;
                            }
                            //else
                            //    return;
                        }
                        #endregion
                    }
                    #endregion
                }
                else  //除产品类型节点以外的所有节点
                {                 
                    frmbar.SetFrmProcessBarMax((long)m_Hook.DataInfoGrid.Rows.Count);                  
                    for (int i = 0; i < m_Hook.DataInfoGrid.Rows.Count; i++)
                    {
                        #region 获取必要信息
                        if (false == m_Hook.DataInfoGrid.Rows[i].Selected) continue;
                        frmbar.SetFrmProcessBarValue((long)i); 
                        int DataType = -1;//产品的类型（标准，非标准，属性）
                        try
                        {
                            
                            ID = m_Hook.DataInfoGrid.Rows[i].Cells["ID"].FormattedValue.ToString().Trim();
                            if (string.IsNullOrEmpty(ID))
                                continue;
                            lid = Convert.ToInt64(ID);
                            FileName = m_Hook.DataInfoGrid.Rows[i].Cells["数据文件名"].FormattedValue.ToString().Trim();
                            FilePath = m_Hook.DataInfoGrid.Rows[i].Cells["存储位置"].FormattedValue.ToString().Trim();
                            type = m_Hook.DataInfoGrid.Rows[i].Cells["数据类型"].FormattedValue.ToString().Trim();
                            if ("标准图幅数据" == type)
                            {
                                datatype = EnumDataType.标准图幅.GetHashCode();
                            }
                            else if ("非标准图幅数据" == type)
                            {
                                datatype = EnumDataType.非标准图幅.GetHashCode();
                            }
                            else if ("控制点数据" == type)
                            {
                                datatype = EnumDataType.控制点数据.GetHashCode();
                            }

                        }
                        catch 
                        {
                            eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件:"+FileName+"删除失败。原因为：数据文件的ID获取失败\n" +"是否继续删除？");
                            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                            if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                continue;
                            }
                            else
                            {
                                frmbar.Close();
                                return;
                            }
                        }
                    #endregion
                        #region 在元信息表中删除
                        frmbar.SetFrmProcessBarText("正在删除文件："+FileName);
                        if (ex != null)
                        {
                            ex = new Exception("连接元数据库失败！连接地址为：" + ConnStr);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        if (lid != -1 && datatype != -1)
                        {
                            ModDBOperator.DelDataItem(lid, datatype, pSysDB, out ex);
                            if (null != ex)
                            {
                                eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件元信息删除失败。\n" + "请检查元信息库连接信息" + "！\n是否继续删除？");
                                eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                                if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    continue;
                                }
                                else
                                {
                                    frmbar.Close();
                                    return;
                                }
                            }
                        }

                        #endregion
                        #region 在文件库中删除

                        if (2 != datatype)////数据类型为控制点控制点数据则不进行文件的删除操作
                        {
                            bool Delstate = ModDBOperator.DelDataItem(FilePath, FileName, ip, id, password, out ex);
                            if (!Delstate)
                            {
                                eerorFrm = new SysCommon.Error.frmInformation("是", "否", "数据文件\n'" + FilePath + "/" + FileName + "'\n删除失败。\n" + " 请确认文件是否存在或是否为占用状态。"+ "\n是否继续删除？");
                                eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                                if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {

                                    continue;
                                }
                                else
                                {
                                    frmbar.Close();
                                    return;
                                }
                            }
                        }
                        #endregion                     
                        #region 记录DataGrid中要删除的行
                        delrowlist.Add(i);
                        #endregion
                        #region 在树节点中删除文件节点

                        ModDBOperator.DelNodeByNameAndText(Deltreenode, FilePath, FileName, out ex);
                        if (null != ex)
                        {
                            eerorFrm = new SysCommon.Error.frmInformation("是", "否", "树节点删除失败。原因为：\n" + ex.Message + "！\n是否继续删除？");
                            eerorFrm.Owner = (m_Hook as Plugin.Application.IAppFormRef).MainForm;
                            if (eerorFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                continue;
                            }
                            else
                            {
                                frmbar.Close();
                                return;
                            }
                        }
                        #endregion 
                    }                                    
                }
                /////数据列表中移除相应的行
                if (null != delrowlist)
                {
                    for (int j = 0; j < delrowlist.Count; j++)
                    {
                        m_Hook.DataInfoGrid.Rows.Remove(m_Hook.DataInfoGrid.Rows[delrowlist[j]-j]);
                    }
                }
                frmbar.Close();
                #endregion
            }
            #region 图层处理
            if (ModData.v_AppFileDB.MapControl.LayerCount > 0 && datatype!=2)
            {
                IGroupLayer Glayer = null;
                for (int i = 0; i < ModData.v_AppFileDB.MapControl.LayerCount; i++)
                {
                    ILayer getlayer = ModData.v_AppFileDB.MapControl.get_Layer(i);
                    if (getlayer.Name == "项目范围图")
                    {
                        Glayer = getlayer as IGroupLayer;
                    }
                }
                if (null != Glayer)
                {
                    ICompositeLayer comlayer = Glayer as ICompositeLayer;
                    string layername = null;
                    switch (datatype)
                    {
                        case 0:
                            layername = "MapFrame_";
                            break;
                        case 1:
                            layername = "Range_";
                            break;
                        default:
                            layername = "_";
                            break;
                    }
                    if (comlayer != null)
                    {
                        for (int i = 0; i < comlayer.Count; i++)
                        {
                            ILayer orglayer = comlayer.get_Layer(i);
                            string lname = orglayer.Name;
                            if (lname.Contains(layername))
                            {
                                Glayer.Delete(orglayer);
                                ModData.v_AppFileDB.TOCControl.Update();
                                ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
                            }
                        }
                    }

                }
            }
            if (datatype == EnumDataType.控制点数据.GetHashCode())
            {
                IGraphicsContainer pGra = ModData.v_AppFileDB.MapControl.Map as IGraphicsContainer;
                pGra.DeleteAllElements();
                ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
            }
                #endregion              
           
            
            //刷新时间列表框

            ModDBOperator.LoadComboxTime(ConnStr, out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载时间列表框失败，" + ex.Message);
                return;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
    }
}