using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace FileDBTool
{
    /// <summary>
    /// 根据组合条件查询数据
    /// </summary>
    public class ControlsQueryDataByCondition:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;
        DevComponents.AdvTree.Node v_DBNode = null;  //数据库数节点
        DevComponents.AdvTree.Node v_ProNode = null; //项目树节点

        string v_CondiStr = "";

        public ControlsQueryDataByCondition()
        {
            base._Name = "FileDBTool.ControlsQueryDataByCondition";
            base._Caption = "查询数据";
            base._Tooltip = "根据条件查询数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "根据条件查询数据";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree == null) return false ;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (ModData.v_ComboxTime == null) return false ;    //若时间列表框不存在

                string timeStr = ModData.v_ComboxTime.ControlText.Trim();//时间点选择
                if (m_Hook.ProjectTree.SelectedNode.ImageIndex == 1) return false;

                string whereStr = "";
                bool b = false;
                if (m_Hook.ProjectTree.SelectedNode != null)
                {
                    #region  根据时间和其他组合条件来查询
                    string treeNodeType = m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
                    if (treeNodeType == EnumTreeNodeType.DATABASE.ToString())
                    {
                        //根据时间进行查询
                        v_DBNode = m_Hook.ProjectTree.SelectedNode;
                        if (timeStr == "不选择" || timeStr == "")
                        {
                            whereStr = "";
                        }
                        else
                        {
                           // whereStr = "生产日期= #" + timeStr + "#";
                            whereStr = "生产日期=to_date('"+timeStr+"','yyyy-mm-dd')";

                        }
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.PROJECT.ToString())
                    {
                        //根据项目、时间进行组合查询

                        v_ProNode = m_Hook.ProjectTree.SelectedNode;
                        if (v_ProNode.Tag == null) return false;
                        v_DBNode = v_ProNode.Parent;
                        if (timeStr == "不选择" || timeStr == "")
                        {//没有加上时间条件
                            whereStr = "项目ID=" + long.Parse(v_ProNode.Tag.ToString());
                        }
                        else
                        {
                            whereStr = "项目ID=" + long.Parse(v_ProNode.Tag.ToString()) + " and 生产日期=to_date('" + timeStr + "','yyyy-mm-dd')";
                        }
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.DATAFORMAT.ToString())
                    {
                        //根据项目、数据类型（DLG\DEM\DOM\DRG）和时间来组合查询

                        v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent;
                        if (m_Hook.ProjectTree.SelectedNode.Tag == null || v_ProNode.Tag == null) return false;
                        long dataFormatID = long.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());   //数据格式ID
                        v_DBNode = v_ProNode.Parent;
                        if (timeStr == "不选择" || timeStr == "")
                        {//没有加上时间条件
                            whereStr = "项目ID=" + long.Parse(v_ProNode.Tag.ToString()) + " and 数据格式编号=" + dataFormatID;
                        }
                        else
                        {
                            whereStr = "项目ID=" + long.Parse(v_ProNode.Tag.ToString()) + " and 数据格式编号=" + dataFormatID + " and 生产日期= to_date('" + timeStr + "','yyyy-mm-dd')";
                        }
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.PRODUCT.ToString())
                    { //根据产品和时间来组合查询
                        v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent.Parent;
                        v_DBNode = v_ProNode.Parent;
                        if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                        long productID = long.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());  //产品ID

                        if (timeStr == "不选择" || timeStr == "")
                        {//没有加上时间条件
                            whereStr = "产品ID=" + productID;
                        }
                        else
                        {
                            whereStr = "产品ID=" + productID + " and 生产日期=to_date('" + timeStr + "','yyyy-mm-dd')";
                        }
                        b = true;
                    }
                    else if (treeNodeType == EnumTreeNodeType.PRODUCTPYPE.ToString())
                    {  //根据产品、产品类型（标准图幅、非标准图幅、属性表）和时间来组合查询


                        v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent.Parent.Parent;
                        v_DBNode = v_ProNode.Parent;
                        if (m_Hook.ProjectTree.SelectedNode.Tag == null || m_Hook.ProjectTree.SelectedNode.Parent.Tag == null) return false;
                        long prodID = long.Parse(m_Hook.ProjectTree.SelectedNode.Parent.Tag.ToString());//产品ID
                        long dataTypeID = long.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());  //产品类型id

                        if (timeStr == "不选择" || timeStr == "")
                        {//没有加上时间条件
                            whereStr = "产品ID=" + prodID + " and 数据类型编号=" + dataTypeID;
                        }
                        else
                        {
                            whereStr = "产品ID=" + prodID + " and 数据类型编号=" + dataTypeID + " and 生产日期=to_date('" + timeStr + "','yyyy-mm-dd')";
                        }
                        b = true;
                    }
                    else
                    {
                        return false;
                    }
                    if(v_DBNode==null) 
                    {
                        return false;
                    }
                    if (v_DBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString())
                    {
                        return false;
                    }
                    if (v_DBNode.Name != "文件连接")
                    {
                        return false;
                    }
                    if (whereStr != "")
                    {
                        v_CondiStr = "select * from ProductIndexTable where " + whereStr; //查询成果索引表

                    }
                    else
                    {
                        v_CondiStr = "select * from ProductIndexTable";
                    }

                    #endregion
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
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();

            try
            {
                //设置查询条件后，执行查询操作

               ///连接数据库

                /// //string ipStr = v_DBNode.Text.Trim();//ip
                //if (ipStr == "") return;
                //string ipStr = "//" + ipStr + "//MetaDataBase//MetaDataBase.mdb";//元数据库路径
                XmlElement dbElem = v_DBNode.Tag as XmlElement;
                if (dbElem == null) return;
                string ipStr = dbElem.GetAttribute("MetaDBConn");

                //string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
                //pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                pSysDB.SetDbConnection(ipStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                if(eError!=null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！");
                    pSysDB.CloseDbConnection();
                    return;
                }

                if(v_CondiStr=="")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置查询条件！");
                    pSysDB.CloseDbConnection();
                    return;
                }

                DataTable resultDT = pSysDB.GetSQLTable(v_CondiStr, out eError);
                if(eError!=null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询成果索引表出错！");
                    pSysDB.CloseDbConnection();
                    return;
                }
               

                ///将查询到的结果在界面上表现出来(查询结果包括数据信息和元信息)
                #region 将结果保存在DataTable中

                DataTable tempDT = ModDBOperator.CreateDataInfoTable();
                for(int i=0;i<resultDT.Rows.Count;i++)
                {
                    string dataID = "";        //数据ID
                    string DataName = "";      //数据文件名

                    string projectID = "";     //项目ID
                    string productID = "";     //产品ID
                    string projectName = "";   //项目名称
                    string productName = "";   //产品名称
                    string scale = "";         //比例尺

                    string rangNO = "";        //范围号

                    string dataFormatID = "";  //数据格式ID
                    string dataTypeID = "";      //数据类型ID
                    string dataFormat = "";    //数据格式名

                    string dataType = "";      //数据类型名

                    //string saveTime = "";      //存储时间
                    string savePath = "";      //存储路径
                    string fromDate = "";      //生产日期

                    dataID = resultDT.Rows[i]["数据ID"].ToString().Trim();
                    projectID = resultDT.Rows[i]["项目ID"].ToString().Trim();
                    productID = resultDT.Rows[i]["产品ID"].ToString().Trim();
                    scale = resultDT.Rows[i]["比例尺分母"].ToString().Trim();
                    rangNO = resultDT.Rows[i]["范围号"].ToString().Trim();
                    dataFormatID = resultDT.Rows[i]["数据格式编号"].ToString().Trim();
                    dataTypeID = resultDT.Rows[i]["数据类型编号"].ToString().Trim();
                    //saveTime = resultDT.Rows[i]["存储时间"].ToString().Trim();
                    savePath = resultDT.Rows[i]["存储位置"].ToString().Trim();
                    fromDate = resultDT.Rows[i]["生产日期"].ToString().Trim();

                    projectName =ModDBOperator.GetProjectName(long.Parse(projectID), pSysDB, out eError);
                    if(eError!=null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取项目名称出错！");
                        pSysDB.CloseDbConnection();
                        return;
                    }

                    productName = ModDBOperator.GetProductName(long.Parse(productID), pSysDB, out eError);
                    if(eError!=null||productName=="")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取产品名称出错！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    if(dataTypeID=="")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "为填写数据类型，请检查！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    DataName = GetDataName(int.Parse(dataTypeID),long.Parse(dataID), pSysDB, out eError);
                    if(eError!=null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据文件名称出错！");
                        pSysDB.CloseDbConnection();
                        return;
                    }

                    switch(int.Parse(dataFormatID))
                    {
                        case 0:
                            dataFormat = "DLG";
                            break;
                        case 1:
                            dataFormat = "DEM";
                            break;
                        case 2:
                            dataFormat = "DOM";
                            break;
                        case 3:
                            dataFormat = "DRG";
                            break;
                    }

                    switch (int.Parse(dataTypeID))
                    {
                        case 0:
                            dataType = "标准图幅数据";
                            break;
                        case 1:
                            dataType = "非标准图幅数据";
                            break;
                        case 2:
                            dataType = "控制点数据";
                            break;
                    }

                    DataRow newRow = tempDT.NewRow();
                    newRow["ID"] = dataID;
                    newRow["项目名称"] =projectName;
                    newRow["产品名称"] =productName;
                    newRow["数据文件名"] = DataName;
                    newRow["数据类型"] = dataType;
                    newRow["比例尺"] = scale;
                    newRow["范围号"] =rangNO;
                    newRow["存储位置"] = savePath;
                    //newRow["存储时间"] = saveTime;
                    newRow["生产日期"] = fromDate;
                    newRow["项目ID"] = projectID.ToString();
                    newRow["产品ID"] = productID.ToString();
                    tempDT.Rows.Add(newRow);
                }
                #endregion

                #region 将DataTable与DataGrid进行绑定
                //清空表格
                if(m_Hook.DataInfoGrid.DataSource!=null)
                {
                    m_Hook.DataInfoGrid.DataSource = null;
                }
                //绑定
                m_Hook.DataInfoGrid.DataSource = tempDT;
                m_Hook.DataInfoGrid.ReadOnly = true;
                m_Hook.DataInfoGrid.Visible = true;
                m_Hook.DataInfoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                for (int j = 0; j < m_Hook.DataInfoGrid.Columns.Count; j++)
                {
                    //m_Hook.DataInfoGrid.Columns[j].Width = (m_Hook.DataInfoGrid.Width - 20) / m_Hook.DataInfoGrid.Columns.Count;
                    m_Hook.DataInfoGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                }
                m_Hook.DataInfoGrid.RowHeadersWidth = 20;
                m_Hook.DataInfoGrid.Refresh();

                pSysDB.CloseDbConnection();
                #endregion
            }
            catch (Exception eR)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未知错误：" + eR.Message);
                pSysDB.CloseDbConnection();
                return;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }

        /// <summary>
        /// 根据数据ID获得数据文件名

        /// </summary>
        /// <param name="dataTypeID"></param>
        /// <param name="dataID"></param>
        /// <param name="sysTable"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetDataName(int dataTypeID,long dataID,SysCommon.DataBase.SysTable sysTable,out Exception eError)
        {
            eError = null;
            string dataName = "";
            string str = "";
            try
            {
                switch (dataTypeID)
                {
                    case 0:          //标准图幅
                        str = "select * from StandardMapMDTable where ID=" + dataID;
                        break;
                    case 1:          //非标准图幅

                        str = "select * from NonstandardMapMDTable where ID=" + dataID;
                        break; ;
                    case 2:          //控制点测量数据

                        str = "select * from ControlPointMDTable where ID=" + dataID;
                        break;

                }
                DataTable dt = sysTable.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    return "";
                }
                dataName = dt.Rows[0]["数据文件名"].ToString();
                return dataName;
            }
            catch (Exception ex)
            {
                eError = ex;
                return "";
            }
        }

    }
}
