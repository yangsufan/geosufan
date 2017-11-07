using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace FileDBTool
{
    /// <summary>
    /// 工程的新建修改、产品的新建修改、数据入库，数据属性修改
    /// </summary>
    public partial class FrmProjectSetting : DevComponents.DotNetBar.Office2007Form
    {
        DevComponents.AdvTree.Node v_DBNode = null;  //数据库节点，根节点

        DevComponents.AdvTree.Node v_SelNode = null; //选中的节点

        DevComponents.AdvTree.Node v_dataFormatNode = null;//数据格式节点
        SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();//数据库操作类
        DataTable newTable = null;//数据入库元信息表格

        DataTable alterTable = null;//数据修改元信息表格

        public FrmProjectSetting(DevComponents.AdvTree.Node mNode,string text)
        {
            InitializeComponent();

            this.cmbScale.Visible = false;
            this.groupPanel1.Visible = false;
            dateTimePicker1.Visible = false;
            this.Text = text;
            splitContainer1.Panel1Collapsed = true;
            splitContainer1.Panel2Collapsed = false;
            buttonX4.Enabled = true;
            buttonX2.Enabled = true;
            if (this.Text == "数据入库")
            {
                splitContainer1.Panel1Collapsed = false;
                buttonX4.Enabled = true;
                buttonX2.Enabled = true;
                buttonX3.Enabled = true;
                if(listViewData.Items.Count==0)
                {
                    buttonX2.Enabled = false;
                    buttonX3.Enabled = false;
                    buttonX4.Enabled = false;
                }
            }
            //if (mNode.Text.Trim() == EnumDataType.控制点数据.ToString())
            //{
                //splitContainer1.Panel2Collapsed = true;
                
            //}
            v_SelNode = mNode;
            intialForm(mNode);
        }

        //初始化界面

        private void intialForm(DevComponents.AdvTree.Node mNode)
        {
            Exception eError=null;
            DataTable newItemTable = null;   //新建记录表

            DataTable alterItemTable = null; //修改记录表

            string str = "";

            //清空资源
            if (dataGridViewMeta.DataSource == null)
            {
                dataGridViewMeta.DataSource = null;
            }
            //获得数据库节点信息

            v_DBNode = mNode;      
            while(v_DBNode.Parent!=null)
            {
                v_DBNode=v_DBNode.Parent;
            }
            if (v_DBNode==null)
            {
                return;
            }
            if (v_DBNode.Tag == null) return;
            XmlElement conElem = v_DBNode.Tag as XmlElement;  //连接信息节点
            string conStr = conElem.GetAttribute("MetaDBConn");

            //连接数据库
            //****************************************************************************************************************************************
            //guozheng 2010-10-11 改为Oracle连接方式 

           // pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + conStr + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            if(eError!=null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + conStr);
                pSysTable.CloseDbConnection();
                return;
            }
            
             if (mNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString()) 
            {
                //数据连接返回
                return;
            }
            else if(mNode.DataKey.ToString()==EnumTreeNodeType.DATABASE.ToString())
            {
                //新建项目
                buttonX2.Enabled = false;
                buttonX4.Enabled = false;
                newItemTable = pSysTable.GetTable("ProjectMDTable", out eError);
                this.btnOK.Text = "保 存";
                
            }
            else if(mNode.DataKey.ToString()==EnumTreeNodeType.PROJECT.ToString())
            {
                //修改项目
                buttonX2.Enabled = false;
                buttonX4.Enabled = false;
                if (mNode.Tag == null) return;
                long projectID = long.Parse(mNode.Tag.ToString());
                str = "select * from ProjectMDTable where ID=" + projectID;
                alterItemTable = pSysTable.GetSQLTable(str, out eError);
                this.btnOK.Text = "保 存";

            }
            else if(mNode.DataKey.ToString()==EnumTreeNodeType.DATAFORMAT.ToString())
            {
                //新建产品
                buttonX2.Enabled = false;
                buttonX4.Enabled = false;
                newItemTable = pSysTable.GetTable("ProductMDTable", out eError);
                this.btnOK.Text = "保 存";
            }
            else if(mNode.DataKey.ToString()==EnumTreeNodeType.PRODUCT.ToString())
            {
                //修改产品
                buttonX2.Enabled = false;
                buttonX4.Enabled = false;
                if (mNode.Tag == null) return;
                long productID = long.Parse(mNode.Tag.ToString());
                int dataFormatID=int.Parse(mNode.Parent.Tag.ToString());  //数据格式编号
                str = "select * from ProductMDTable where ID=" + productID + " and 数据格式编号=" + dataFormatID;
                this.btnOK.Text = "保 存";
                alterItemTable = pSysTable.GetSQLTable(str, out eError);
            }
            else if (mNode.DataKey.ToString() == EnumTreeNodeType.DATAITEM.ToString())
            {
                #region 数据项（修改数据项）
                DevComponents.AdvTree.Node productNode = null;//产品节点
                productNode = mNode;
                while (productNode.DataKey.ToString() != EnumTreeNodeType.PRODUCT.ToString())
                {
                    productNode = productNode.Parent;
                }
                if (productNode == null) return;
                long productID = long.Parse(productNode.Tag.ToString());//产品ID
                int dataTypeID = int.Parse(mNode.Parent.Tag.ToString());//数据类型编号
                long dataID=long.Parse(mNode.Tag.ToString());
                if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                {
                    str = "select * from StandardMapMDTable where ID=" + dataID;
                }
                else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                {
                    str = "select * from NonstandardMapMDTable where ID=" + dataID;
                }
                else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                {
                    buttonX2.Enabled = false;
                    buttonX4.Enabled = false;
                    str = "select * from ControlPointMDTable where ID=" + dataID;
                }
                alterItemTable = pSysTable.GetSQLTable(str, out eError);
                #endregion
                this.btnOK.Text = "保 存";
            }
            else
            {
                #region 数据入库(新的数据项)
                this.groupPanel1.Visible = true;

                DevComponents.AdvTree.Node productTypeNode = null;//产品类型节点
                productTypeNode = mNode;
                while (productTypeNode.DataKey.ToString() != EnumTreeNodeType.PRODUCTPYPE.ToString())
                {
                    productTypeNode = productTypeNode.Parent;
                }
                if (productTypeNode == null) return;
                int dataTypeID = int.Parse(productTypeNode.Tag.ToString());//数据类型编号
                if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                {
                    str = "select * from StandardMapMDTable";
                }
                else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                {
                    str = "select * from NonstandardMapMDTable";
                }
                else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                {
                    //str = "select * from ControlPointMDTable";
                    splitContainer1.Panel2Collapsed = true;
                }
                if (str != "")
                {
                    newItemTable = pSysTable.GetSQLTable(str, out eError);
                }
                
                #endregion
                this.btnOK.Text = "导 入";
            }
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询元数据表失败！");
                pSysTable.CloseDbConnection();
                return;
            }

            newTable = newItemTable;
            alterTable = alterItemTable;
            # region 初始化datagridview
            dataGridViewMeta.Columns.Add("字段名称", "字段名称");
            dataGridViewMeta.Columns.Add("字段值", "字段值");
            dataGridViewMeta.Columns[0].ReadOnly = true;
            dataGridViewMeta.Visible = true;
            dataGridViewMeta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            for (int j = 0; j < dataGridViewMeta.Columns.Count; j++)
            {
                dataGridViewMeta.Columns[j].Width = (dataGridViewMeta.Width - 20) / dataGridViewMeta.Columns.Count;
            }
            dataGridViewMeta.RowHeadersWidth = 20;
            #endregion

            if (newItemTable != null)
            {
                #region 对于新建的记录(项目、产品、数据)，显示字段名

                NewRecordDataBinding(newItemTable);
                #endregion
            }
            if (alterItemTable != null)
            {
                #region 对于修改的记录(项目、产品、数据)，显示字段名和字段值

                
                AlterRecordDataBinding(alterItemTable);
                #endregion
            }
            if (this.dataGridViewMeta.Columns.Count > 1)
            {
                for (int i = 1; i < this.dataGridViewMeta.Columns.Count; i++)
                {
                    (this.dataGridViewMeta.Columns[i] as DataGridViewTextBoxColumn).MaxInputLength = 30;

                }
            }
        
            //将表格绑定到界面上

            //dataGridViewMeta.DataSource = dt;
            ////dataGridViewMeta.Rows[0].Cells[0].ValueType=typeof()
            //dataGridViewMeta.Columns[0].ReadOnly = true;
            //dataGridViewMeta.Visible = true;
            //dataGridViewMeta.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //for (int j = 0; j < dataGridViewMeta.Columns.Count; j++)
            //{
            //    dataGridViewMeta.Columns[j].Width = (dataGridViewMeta.Width - 20) / dataGridViewMeta.Columns.Count;
            //}
            //dataGridViewMeta.RowHeadersWidth = 20;
            dataGridViewMeta.Refresh();
            pSysTable.CloseDbConnection();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;

            //获得元数据库的连接字符串和ftp连接地址
            if (v_DBNode.Tag == null) return;
            XmlElement conElem = v_DBNode.Tag as XmlElement;
            string conStr = conElem.GetAttribute("MetaDBConn");
            string severStr = conElem.GetAttribute("服务器"); //ip地址
            string user = conElem.GetAttribute("用户");    //用户
            string Password = conElem.GetAttribute("密码");//密码

            //连接数据库
            //****************************************************************************************
            //guozheng 2010-10-11 改为Oracle连接方式
            //pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + conStr + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            //********************************************************************************************
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + conStr);
                pSysTable.CloseDbConnection();
                return;
            }
            
            //定义特殊字段。

            int dataFormatID = -1;//数据格式编号  产品元信息

            string savePath = "";//存储位置
            
            #region 遍历dataGridview，组合插入数据库的字段和值的字符串,数据入库情况除外的其他情况

            string productName = "";  //产品名称
            string productNO = "";//编号
            string projectName = "";  //项目名称
            string projectNO = "";//项目编号
            DateTime startTime = DateTime.Now.Date;//项目开始时间
            long Scal = 0;//元数据比例尺

            DateTime endTime = DateTime.Now.Date;    //项目结束时间
            //插入数据库的字段组合和对应的值组合

            StringBuilder nameStrBuild = new StringBuilder();
            StringBuilder valueStrBuild = new StringBuilder();
            //修改数据库的SQL语句
            string updateStr = "";
            //插入数据库的SQL语句
            string nameStr = "";
            string valueStr = "";
            if (this.Text.Trim() != "数据入库")
            {
                
                string produName = "";
                for (int i = 0; i < dataGridViewMeta.Rows.Count; i++)
                {
                    string columnName = dataGridViewMeta.Rows[i].Cells[0].FormattedValue.ToString().Trim();
                    if (columnName == "") continue;
                    string columnValue = "";

                    if (this.dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Contains("'"))
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText=  "'"+columnName+"'使用非法字符:  ''',请检查！";
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;

                    #region 界面判断和控制，有些是必填项

                    if (columnName == "产品名称")
                    {
                        productName = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (productName == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "产品名称不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText= "产品名称不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText=string.Empty;
                        continue; 
                    }
                    if (columnName == "底图")
                    {
                        string MapPath = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (MapPath == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "产品名称不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "底图不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        if(MapPath.Substring(MapPath.LastIndexOf('.')+1)!="mxd")
                        {
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "底图必须为mxd文件！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    if (columnName == "图幅结合表")
                    {
                        string MapFramePath = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (MapFramePath == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "产品名称不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "图幅结合表路径不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        if (MapFramePath.Substring(MapFramePath.LastIndexOf('.') + 1) != "mdb")
                        {
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "图幅结合表必须为pdb文件！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    if (columnName == "产品编号")
                    {
                         productNO = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (productNO == "")
                        {
                           // SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "产品编号不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText ="产品编号不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText =string.Empty;
                    }
                    if (columnName == "项目名称")
                    {
                        projectName = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (projectName == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目名称不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText ="项目名称不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText =string.Empty;
                    }
                    if (columnName == "项目编号")
                    {
                        projectNO = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (projectNO == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目编号不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "项目编号不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    if (columnName == "开始时间")
                    {
                        if (dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim() == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目开始时间不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "'项目开始时间'不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                        startTime = DateTime.Parse(dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString());
                    }
                    if (columnName == "结束时间")
                    {
                        if (dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim() == "")
                        {
                           // SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目结束时间不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "'项目结束时间'不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                        endTime = DateTime.Parse(dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString());
                    }
                    if (columnName == "测绘面积（平方米）")
                    {
                        string value = "";
                        double Area = 0;
                        try
                        {
                            value = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                            Area = Convert.ToDouble(value);
                            if (Area < 0)
                            {
                                dataGridViewMeta.Rows[i].Cells[1].ErrorText = "'测绘面积（平方米）'输入值应当是大于0的数值";
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                        }
                        catch
                        { 

                        }
                    }
                    if (columnName == "比例尺分母")
                    {
                        string value = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                        if (value == "")
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目编号不能为空！");
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "比例尺分母不能为空！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        try
                        {
                           Scal = Convert.ToInt64(value);
                            if (Scal < 0)
                            {
                                dataGridViewMeta.Rows[i].Cells[1].ErrorText = "'图幅比例尺'输入值应当是大于0的数值";
                                pSysTable.CloseDbConnection();
                                return;
                            }
                        }
                        catch
                        {
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = "无效的字符串！";
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    if (columnName == "图幅比例尺")
                    {
                        string value = "";
                        try
                        {
                            value = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString().Trim();
                            Scal = Convert.ToInt64(value);
                            if (Scal < 0)
                            {
                                dataGridViewMeta.Rows[i].Cells[1].ErrorText = "'图幅比例尺'输入值应当是大于0的数值";
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                        }
                        catch
                        {
                        }
                    }
                    #endregion
                    columnValue = dataGridViewMeta.Rows[i].Cells[1].FormattedValue.ToString();
                    Type columnType = dataGridViewMeta.Rows[i].Cells[1].ValueType;
                    nameStrBuild.Append(columnName + ",");
                    StringBuilder tempStr = new StringBuilder();
                    string tempUpStr = "";
                    GetSQL(columnType, columnName, columnValue, out tempStr, out tempUpStr, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    valueStrBuild.Append(tempStr);
                    updateStr += tempUpStr;
                }
                updateStr = "set " + updateStr;
                updateStr = updateStr.Substring(0, updateStr.Length - 1);
                nameStr = nameStrBuild.ToString().Substring(0, nameStrBuild.ToString().Length - 1);     //字段名组合

                valueStr = valueStrBuild.ToString().Substring(0, valueStrBuild.ToString().Length - 1);  //字段值组合

            }
            else 
            {
                //数据入库的情况进行特殊的处理，在这里不处理，放在在后面处理

            }
           
            #endregion 
           
            string insertstr = "";
            pSysTable.StartTransaction();/////开启数据库事务//6.7郭正修改 bug GO-3854 
            if (v_SelNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString())
            {
                //数据连接返回
                return;
            }
            else if (v_SelNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString())
            {
                ///////项目名称非法字符判断
                if (!CheckProjectName(projectName))
                {
                    return;
                }
                ///////项目编号非法字符判断
                if (!CheckProjectName(projectNO))
                {
                    return;
                }
                #region 新建项目
                if (startTime > endTime)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目结束时间不能早于开始时间！");
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (projectName.Contains("'"))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", " 项目文件名使用非法字符:  '''");
                    pSysTable.CloseDbConnection();
                    return;
                }
                #region 插入项目元信息

                nameStr += ",存储位置";
                valueStr += ",'" + projectName + "'";
                long lNewID = this.GetNewTableID("ProjectMDTable", out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "生成项目ID失败,\n原因"+eError.Message);
                    pSysTable.CloseDbConnection();
                    return;
                }
                nameStr += ",ID";
                valueStr += "," + lNewID.ToString();
                insertstr = "insert into ProjectMDTable (" + nameStr + ") values (" + valueStr + ")";
   
                #region 判断项目名称和编号是否重复

                #region 判断项目名称是否重复
                //////////////////////////////判断项目名称是否重复/////////////////////////////
                bool isEx = ModDBOperator.IsTableFildExist("ProjectMDTable", pSysTable, "项目名称", projectName, null, out  eError);
                if (null != eError)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "读取工程元数据库失败！");
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (isEx)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "已存在同名工程项目，请重新制定项目名称");
                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion

                #region 判断项目编号是否重复
                //////////////////////////////判断项目编号是否重复/////////////////////////////
                isEx = ModDBOperator.IsTableFildExist("ProjectMDTable", pSysTable, "项目编号", projectNO, null, out  eError);
                if (null != eError)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "读取工程元数据库失败！");
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (isEx)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "已存在相同项目编号，请重新制定项目编号");
                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion
                #endregion
                ///////////////////////////////////////////////////////////////////////////////////
               
                pSysTable.UpdateTable(insertstr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入元信息出错！");
                    pSysTable.EndTransaction(false);///新建项目事务回滚
                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion

                #region 创建项目文件夹

                //if (projectName.Trim() ==string.Empty)
                //{
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "项目名称不能为空！");
                //    pSysTable.EndTransaction(false);///新建项目事务回滚
                //    pSysTable.CloseDbConnection();                    
                //    return;
                //}
                
                clsProject CreatPro = new clsProject(projectName, severStr, user, Password);
                bool CreatState = CreatPro.Create();
                if (!CreatState)
                {
                    string errorchar = string.Empty;
                    if (projectName.Contains("."))
                        errorchar = ".";
                    if (projectName.Contains(">"))
                        errorchar = ">";
                    if (projectName.Contains("<"))
                        errorchar = "<";
                    if (projectName.Contains("/"))
                        errorchar = "/";
                    if (projectName.Contains("?"))
                        errorchar = "?";
                    if (projectName.Contains("~"))
                        errorchar = "~";
                    if (projectName.Contains("#"))
                        errorchar = "#";
                    if (projectName.Contains("*"))
                        errorchar = "*";
                    if (projectName.Contains("|"))
                        errorchar = "|";
                    if (projectName.Contains("\\"))
                        errorchar = "\\";
                    if (projectName.Contains("\""))
                        errorchar = "\"";
                    if (projectName.Contains(":"))
                        errorchar = ":";
                  
                    if (errorchar==string.Empty)
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "创建项目文件夹出错，请检查FTP服务根目录是否存在同名文件夹：" + projectName);
                    else
                         SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "创建项目出错！\n 项目文件名使用非法字符:  '" + errorchar+"'");
                    pSysTable.EndTransaction(false);///新建项目事务回滚
                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion

                #region 将项目信息附加在节点上

                string pStr = "select * from ProjectMDTable order by ID desc";
                DataTable projectDT=pSysTable.GetSQLTable(pStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询项目元信息表失败！");
                    pSysTable.CloseDbConnection();
                    pSysTable.EndTransaction(false);///新建项目事务回滚
                    return;
                }
                if (projectDT.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "项目元信息表为空！");
                    pSysTable.CloseDbConnection();
                    pSysTable.EndTransaction(false);///新建项目事务回滚
                    return;
                }
                long pID = long.Parse(projectDT.Rows[0]["ID"].ToString());//项目ID
                DevComponents.AdvTree.Node proNode= ModDBOperator.AppendNode(v_SelNode.Nodes, EnumTreeNodeType.PROJECT.ToString(), projectName, pID, projectName, 3);
                #endregion

                #region 将数据格式子节点附加在项目节点上
                XmlDocument dataFormatInfoXml = new XmlDocument();
                dataFormatInfoXml.Load(ModData.v_ProjectInfoXML);
                XmlNodeList Nodelist = dataFormatInfoXml.SelectSingleNode(".//项目目录").ChildNodes;
                #region 遍历项目xml文件，在项目节点后附加数据格式节点（DLG、DEM、DOM、DRG）

                foreach (XmlNode formatPathNode in Nodelist)
                {
                    string dataFormatName = formatPathNode.InnerText.ToString();   //数据格式名称
                    if (dataFormatName == EnumDataFormat.DLG.ToString())
                    {
                        dataFormatID = 0;
                    }
                    else if (dataFormatName == EnumDataFormat.DEM.ToString())
                    {
                        dataFormatID = 1;
                    }
                    else if (dataFormatName == EnumDataFormat.DOM.ToString())
                    {
                        dataFormatID = 2;
                    }
                    else if (dataFormatName == EnumDataFormat.DRG.ToString())
                    {
                        dataFormatID = 3;
                    }
                    if (dataFormatID == -1) continue;
                    string dataFormatPath = projectName + "/" + dataFormatName;              //数据格式路径
                    //执行附加节点操作
                    DevComponents.AdvTree.Node dataFormatNode = null;//数据格式树节点

                    dataFormatNode =ModDBOperator.AppendNode(proNode.Nodes, EnumTreeNodeType.DATAFORMAT.ToString(), dataFormatName, dataFormatID, dataFormatPath, 3);
                    
                }
                #endregion
                #endregion
                #endregion
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "新建成功！");
            }
            else if (v_SelNode.DataKey.ToString() == EnumTreeNodeType.PROJECT.ToString())
            {               
                #region 修改项目      
                if (startTime > endTime)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "项目结束时间不能早于开始时间！");
                    pSysTable.CloseDbConnection();
                    return;
                }
                updateStr = "update ProjectMDTable " + updateStr + " where ID=" + long.Parse(v_SelNode.Tag.ToString());
                //进行插入或修改操作

                pSysTable.UpdateTable(updateStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "修改元信息出错！");
                    pSysTable.EndTransaction(false);
                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改成功！");
            }
            else if (v_SelNode.DataKey.ToString() == EnumTreeNodeType.DATAFORMAT.ToString())
            {
                ///////产品名称非法字符判断
                if (!CheckProjectName(productName))
                {
                    return;
                }
                ///////产品编号非法字符判断
                if (!CheckProjectName(productNO))
                {
                    return;
                }
                # region 新建产品
                nameStr += ",产品名称";
                valueStr += ",'" + productName + "_" + Scal.ToString().Trim() + "'";
                long projID = long.Parse(v_SelNode.Parent.Tag.ToString());
                if (v_SelNode.Text.Trim() == EnumDataFormat.DLG.ToString())
                {
                    dataFormatID = 0;
                    savePath = v_SelNode.Parent.Name + "/" + EnumDataFormat.DLG.ToString() + "/" + productName+"_"+Scal.ToString().Trim();
                }
                else if (v_SelNode.Text.Trim() == EnumDataFormat.DEM.ToString())
                {
                    dataFormatID = 1;
                    savePath = v_SelNode.Parent.Name + "/" + EnumDataFormat.DEM.ToString() + "/" + productName + "_" + Scal.ToString().Trim();
                }
                else if (v_SelNode.Text.Trim() == EnumDataFormat.DOM.ToString())
                {
                    dataFormatID = 2;
                    savePath = v_SelNode.Parent.Name + "/" + EnumDataFormat.DOM.ToString() + "/" + productName + "_" + Scal.ToString().Trim();
                }
                else if (v_SelNode.Text.Trim() == EnumDataFormat.DRG.ToString())
                {
                    dataFormatID = 3;
                    savePath = v_SelNode.Parent.Name + "/" + EnumDataFormat.DRG.ToString() + "/" + productName + "_" + Scal.ToString().Trim();
                }
                if (dataFormatID == -1) return;

                nameStr += ",数据格式编号,存储位置,项目ID";
                valueStr += "," + dataFormatID + ",'" + savePath + "'," + projID;
                //*********************************************************************************
                //guozheng 2010-10-12 Oracle没有
                long lNewProductId = this.GetNewTableID("ProductMDTABLE", out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "生成产品ID失败,\n原因" + eError.Message);
                    pSysTable.CloseDbConnection();
                    return;
                }
                nameStr += ",ID";
                valueStr += "," + lNewProductId.ToString();
                //*********************************************************************************
                insertstr = "insert into ProductMDTable (" + nameStr + ") values (" + valueStr + ")";
                ////////////////////////////////////////////////////////////////////////////////////
                #region 判断产品的编号和名称是否重复
                #region 判断工程下的产品名称是否重复
                /////////////////////////////////判断工程下的产品名称是否重复//////////////
                DevComponents.AdvTree.Node Projectnode = v_SelNode;
                while (Projectnode.DataKey.ToString() != EnumTreeNodeType.PROJECT.ToString())
                {
                    if (null == Projectnode.Parent) return;
                    Projectnode = Projectnode.Parent;
                }
                long projectID = long.Parse(Projectnode.Tag.ToString());
                string Condition = "项目ID=" + projectID;
                bool isEx = ModDBOperator.IsTableFildExist("ProductMDTable", pSysTable, "产品名称", productName, Condition, out  eError);
                if (null != eError)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "读取产品元数据库失败！");
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (isEx)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "工程下已存在同名产品名称，请重新指定产品名称");
                    pSysTable.CloseDbConnection();
                    return;
                }
                /////////////////////////////////////////////////////////////////////////////
                #endregion
               
                #region 判断工程下的产品编号是否重复
                /////////////////////////////////判断工程下的产品编号是否重复//////////////
                Projectnode = v_SelNode;
                while (Projectnode.DataKey.ToString() != EnumTreeNodeType.PROJECT.ToString())
                {
                    if (null == Projectnode.Parent) return;
                    Projectnode = Projectnode.Parent;
                }
                projectID = long.Parse(Projectnode.Tag.ToString());
                Condition = "项目ID=" + projectID;
                isEx = ModDBOperator.IsTableFildExist("ProductMDTable", pSysTable, "产品编号", productNO, Condition, out  eError);
                if (null != eError)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "读取产品元数据库失败！");
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (isEx)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "工程下已存在相同的产品编号，请重新指定产品编号");
                    pSysTable.CloseDbConnection();
                    return;
                }
                /////////////////////////////////////////////////////////////////////////////
                #endregion
                #endregion
                ////////////////////////////////////////////////////////////////////////////////////
                #region 进行插入新的产品信息
                pSysTable.UpdateTable(insertstr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "掺入产品元信息出错！");
                    pSysTable.EndTransaction(false);////回滚新建产品数据库事务

                    pSysTable.CloseDbConnection();
                    return;
                }
                string selStr = "select * from ProductMDTable order by ID desc";
                DataTable mTable = pSysTable.GetSQLTable(selStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询产品ID出错！");
                    pSysTable.EndTransaction(false);////回滚新建产品数据库事务

                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion
                #region 创建产品目录以及子目录并将节点附加在树图上

                long productID = long.Parse(mTable.Rows[0]["ID"].ToString());
                if (!ModDBOperator.CreatProduct(productName+"_"+Scal.ToString().Trim(), productID, v_SelNode, out eError))
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eError.Message);
                    string errorchar = string.Empty;
                    if (productName.Contains("."))
                        errorchar = ".";
                    if (productName.Contains(">"))
                        errorchar = ">";
                    if (productName.Contains("<"))
                        errorchar = "<";
                    if (productName.Contains("?"))
                        errorchar = "?";
                    if (productName.Contains("~"))
                        errorchar = "~";
                    if (productName.Contains("#"))
                        errorchar = "#";
                    if (productName.Contains("*"))
                        errorchar = "*";
                    if (productName.Contains("|"))
                        errorchar = "|";
                    if (productName.Contains("\\"))
                        errorchar = "\\";
                    if (productName.Contains("\""))
                        errorchar = "\"";
                    if (errorchar == string.Empty)
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "创建产品出错！\n 产品文件名无效，请检查。");
                    else
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "创建产品出错！\n 产品文件名使用非法字符:  '" + errorchar + "'");

                    pSysTable.EndTransaction(false);////回滚新建产品数据库事务
                    pSysTable.CloseDbConnection();
                    return;
                }
                 #endregion             
                #endregion
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "新建成功！");
            }
            else if (v_SelNode.DataKey.ToString() == EnumTreeNodeType.PRODUCT.ToString())
            {               
                #region 修改产品
                updateStr = "update ProductMDTable " + updateStr + " where ID=" + long.Parse(v_SelNode.Tag.ToString());
                //进行插入或修改操作

                pSysTable.UpdateTable(updateStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "修改元信息出错！");
                    pSysTable.EndTransaction(false);
                    pSysTable.CloseDbConnection();
                    return;
                }
                #endregion
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改成功！");
            }
            else if (v_SelNode.DataKey.ToString() == EnumTreeNodeType.DATAITEM.ToString())
            {
                #region 数据项（修改数据项）
                DevComponents.AdvTree.Node productTypeNode = null;//产品类型节点
                productTypeNode = v_SelNode;
                while (productTypeNode.DataKey.ToString() != EnumTreeNodeType.PRODUCTPYPE.ToString())
                {
                    productTypeNode = productTypeNode.Parent;
                }
                if (productTypeNode == null) return;
                int dataTypeID = int.Parse(productTypeNode.Tag.ToString());//数据类型编号
                #region 标准图幅，非标准图幅，控制点数据

                if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                {
                    updateStr = "update StandardMapMDTable " + updateStr + " where ID=" + long.Parse(v_SelNode.Tag.ToString());
                    //进行插入或修改操作

                    pSysTable.UpdateTable(updateStr, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eError.Message);
                        pSysTable.EndTransaction(false);
                        pSysTable.CloseDbConnection();
                        return;
                    }
                }
                else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                {
                    updateStr = "update NonstandardMapMDTable " + updateStr + " where ID=" + long.Parse(v_SelNode.Tag.ToString());
                    //进行插入或修改操作

                    pSysTable.UpdateTable(updateStr, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "修改元信息出错！");
                        pSysTable.EndTransaction(false);
                        pSysTable.CloseDbConnection();
                        return;
                    }
                }
                else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                {
                    updateStr = "update ControlPointMDTable " + updateStr + " where ID=" + long.Parse(v_SelNode.Tag.ToString());
                    //进行插入或修改操作

                    pSysTable.UpdateTable(updateStr, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "修改元信息出错！");
                        pSysTable.EndTransaction(false);
                        pSysTable.CloseDbConnection();
                        return;
                    }
                }
                #endregion
                #endregion
                //刷新时间列表框
                ModDBOperator.LoadComboxTime(conStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载时间列表框失败，" + eError.Message);
                    pSysTable.EndTransaction(false);
                    pSysTable.CloseDbConnection();
                    return;
                }
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改成功！");
            }
            else
            {                
                #region 数据入库(新的数据项)
                if (listViewData.Items.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("操作提示","请选择要上传的数据文件!");
                    pSysTable.CloseDbConnection();
                    return;
                }

                //遍历要入库的文件，进行数据入库

                #region 清空错误日志信息
                string desErrPath = Application.StartupPath + "\\FileErrLog.mdb";
                if (File.Exists(desErrPath))
                {
                    File.Delete(desErrPath);
                }
                #endregion

                for (int i = 0; i < listViewData.Items.Count; i++)
                {
                   
                    string ex = "";
                    FileInfo fileInfo = new FileInfo(listViewData.Items[i].Text.Trim());
                    string orgDataPath = fileInfo.DirectoryName;                              //元数据路径
                    string dataName = fileInfo.Name;                                          //数据文件名
                    string lastFName = dataName.Substring(dataName.LastIndexOf('.') + 1);     //文件后缀名
                    string pureName = dataName.Substring(0, dataName.LastIndexOf('.'));       //不带后缀的文件名
                    string metaFName = pureName + ".mdb";                                     //元数据文件名
                    string nameStr1 = "";                                                     //字段名组合
                    string valueStr1 = "";                                                    //字段值组合

                    DevComponents.AdvTree.Node productTypeNode = null;                        //产品类型节点
                    long projectID;                                                           //项目编号
                    int dataTypeID ;                                                          //数据类型编号
                    long productID ;                                                          //产品编号
                    DateTime fromDate;                                                        //生产日期
                    long productScale = 0;
                    long dataID;

                    //获得产品类型节点
                    productTypeNode = v_SelNode;
                    while (productTypeNode.DataKey.ToString() != EnumTreeNodeType.PRODUCTPYPE.ToString())
                    {
                        productTypeNode = productTypeNode.Parent;
                    }
                    if (productTypeNode == null) return;

                    dataTypeID = int.Parse(productTypeNode.Tag.ToString());
                    productID = long.Parse(productTypeNode.Parent.Tag.ToString());
                    dataFormatID = int.Parse(productTypeNode.Parent.Parent.Tag.ToString());
                    projectID = long.Parse(productTypeNode.Parent.Parent.Parent.Tag.ToString());
                    savePath = v_SelNode.Name;                              //存储位置
                    string productNm = productTypeNode.Parent.Text.Trim();   //产品名称
                    int index1 = productNm.LastIndexOf('_');
                    if (index1 == -1) return;
                    string scaleStr = productNm.Substring(index1 + 1);
                    productScale = long.Parse(scaleStr);// 产品比例尺

                    DataTable mTable = null;
                    string mStr = "";
                   
                    #region 执行数据入库
                    if (dataTypeID != EnumDataType.控制点数据.GetHashCode())
                    {
                        #region 标准图幅、非标准图幅数据入库
                        long lNewDataID = -1; ;                              //数据ID                       
                        string rangeNO = "";                      //图幅号
                     //   long lNewDataID = -1;                     //数据ID
                        //**********************************************************
                        //guozheng 2010-10-12 
                        if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                        {
                            lNewDataID = GetNewTableID("StandardMapMDTABLE", out eError);
                        }
                        else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                        {
                            lNewDataID = GetNewTableID("NonstandardMapMDTABLE", out eError);
                        }
                        else return;
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "生成产品ID失败,\n原因" + eError.Message);
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        //***********************************************************
                        if (listViewData.Items[i].Tag != null)
                        {
                            long DataScale = -1;
                            string sDataScale = string.Empty;
                            #region 组合数据入库的SQL语句
                            StringBuilder nameStrBuild1 = new StringBuilder();                        //字段名组合

                            StringBuilder valueStrBuild1 = new StringBuilder();                       //字段值组合


                            string updateStr1 = "";                                                   //修改数据库的SQL语句
                            DataGridViewRow[] dgRows = listViewData.Items[i].Tag as DataGridViewRow[];
                            for (int j = 0; j < dgRows.Length; j++)
                            {
                                string columnName = "";                                               //字段名

                                string columnValue = "";                                              //字段值

                                Type columnType = null;                                               //字段类型

                                DataGridViewRow dgRow = dgRows[j];
                                if (dgRow.Cells[0].Value == null) continue;

                                columnName = dgRow.Cells[0].Value.ToString();
                                if (columnName == "") continue;
                                ///////获取成果数据的比例尺
                                if (columnName == "图幅比例尺" || columnName == "块图比例尺")
                                {
                                    if (dgRow.Cells[1].Value != null)
                                    {
                                        sDataScale = dgRow.Cells[1].Value.ToString();
                                        try
                                        {
                                            DataScale = long.Parse(sDataScale);
                                        }
                                        catch
                                        {
                                            DataScale = -1;
                                        }
                                    }
                                }
                                //////
                                if (dgRow.Cells[1].Value != null)
                                {
                                    columnValue = dgRow.Cells[1].Value.ToString();
                                }
                                if(columnValue.Contains("'"))
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","属性值不能包含“'”");
                                    //pSysTable.EndTransaction(false);
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                                columnType = dgRow.Cells[1].ValueType;

                                nameStrBuild1.Append(columnName + ",");
                                StringBuilder tStr = new StringBuilder();
                                GetSQL(columnType, columnName, columnValue, out tStr, out updateStr1, out eError);
                                if (eError != null)
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    //pSysTable.EndTransaction(false);
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                                valueStrBuild1.Append(tStr);
                            }
                            if (nameStrBuild1.ToString() == "" || valueStrBuild1.ToString() == "") continue;
                            nameStr1 = nameStrBuild1.ToString().Substring(0, nameStrBuild1.ToString().Length - 1);
                            valueStr1 = valueStrBuild1.ToString().Substring(0, valueStrBuild1.ToString().Length - 1);

                            #endregion
                            #region 增加判断：入库成果数据比例尺是否符合产品比例尺
                            if (DataScale != productScale)
                            {
                                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "数据'" + dataName + "'的比例尺与产品的比例尺信\n息不一致,是否继续导入？"))
                                {
                                    //将出错信息导入
                                    if (!File.Exists(desErrPath))
                                    {
                                        File.Copy(ModData.v_tempErrLog, desErrPath);
                                    }
                                    //连接错误表格
                                    SysCommon.DataBase.SysTable pErrTable = new SysCommon.DataBase.SysTable();
                                    //*****************************************************************************
                                    //guozheng 2010-10-11 
                                    //改为Oracle连接方式
                                    //pErrTable.SetDbConnection(desErrPath, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                                    //*******************************************************************************
                                    pErrTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + desErrPath + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                                    if (eError != null)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接错误日志表失败！\n连接地址为：" + desErrPath);
                                        pSysTable.CloseDbConnection();
                                        return;
                                    }
                                    string errDes = "数据比例尺与产品比例尺信息不一致";
                                    string inserStr = "insert into Errorlog (FileName,ErrDes) values ('" + dataName + "','" + errDes + "')";
                                    pErrTable.UpdateTable(inserStr, out eError);
                                    if (eError != null)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入错误日志信息出错！");
                                    }
                                    pErrTable.CloseDbConnection();
                                    continue;
                                }
                                else
                                {
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                            }




                            #endregion

                            #region 将数据元信息插入数据库中;

                            nameStr1 += ",产品ID,存储位置,数据文件名,数据格式编号";
                            valueStr1 += "," + productID + ",'" + savePath + "','" + dataName + "'," + dataFormatID;
                            if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                            {
                                nameStr1 += ",ID";
                                valueStr1 += "," + lNewDataID;
                                insertstr = "insert into StandardMapMDTable (" + nameStr1 + ") values (" + valueStr1 + ")";
                            }
                            else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                            {
                                nameStr1 += ",ID";
                                valueStr1 += "," + lNewDataID;
                                insertstr = "insert into NonstandardMapMDTable (" + nameStr1 + ") values (" + valueStr1 + ")";
                            }
                           // 向数据元数据表中插入新的数据元信息
                            #region 判断元信息数据表中是否存在同名记录，存在试图执行删除
                            if (ModDBOperator.IsFTPExistFile(severStr, user, Password, savePath, dataName, out eError))
                            {
                                string SQL = "";
                                string Tablename = "";
                                int dataFormat = -1;
                                if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                                {
                                    Tablename = "StandardMapMDTable";
                                    dataFormat = 0;
                                }
                                else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                                {
                                    Tablename = "NonstandardMapMDTable";
                                    dataFormat = 1;
                                }
                                try
                                {
                                    long DataID = -1;
                                    SQL = "SELECT ID FROM " + Tablename + " WHERE 数据文件名='" + dataName + "'" + " AND  存储位置='" + savePath + "'";
                                    DataTable table = pSysTable.GetSQLTable(SQL, out eError);
                                    if (null != table)
                                    {
                                        DataID = Convert.ToInt64(table.Rows[0][0].ToString());
                                    }
                                    SQL = "DELETE FROM " + Tablename + " WHERE 数据文件名='" + dataName + "'" + " AND  存储位置='" + savePath + "'";
                                    pSysTable.UpdateTable(SQL, out eError);
                                    if (DataID != -1)
                                    {
                                        SQL = "DELETE FROM ProductIndexTable WHERE 数据ID=" + DataID + " AND " + "数据类型编号=" + dataFormat;
                                        pSysTable.UpdateTable(SQL, out eError);
                                    }
                                }
                                catch
                                {
                                }
                            }
                            #endregion
                            pSysTable.UpdateTable(insertstr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入数据元信息出错！");
                                pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                pSysTable.CloseDbConnection();
                                return;
                            }
                            #endregion

                            #region 向成果索引表中插入数据


                            #region 查询元数据信息表，获得字段值

                            if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                            {
                                mStr = "select * from StandardMapMDTable order by ID desc";
                            }
                            else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                            {
                                mStr = "select * from NonstandardMapMDTable order by ID desc";
                            }
                            mTable = pSysTable.GetSQLTable(mStr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询数据元信息表出错！");
                                pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "数据元信息表为空！");
                                pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                pSysTable.CloseDbConnection();
                                return;
                            }
                            dataID = long.Parse(mTable.Rows[0]["ID"].ToString());
                            fromDate = DateTime.Parse(mTable.Rows[0]["生产日期"].ToString());
                           
                            if (dataTypeID == 0)
                            {
                                //productScale = long.Parse(mTable.Rows[0]["图幅比例尺"].ToString());
                                rangeNO = mTable.Rows[0]["图幅号"].ToString();
                            }
                            else if (dataTypeID == 1)
                            {
                                //productScale = long.Parse(mTable.Rows[0]["块图比例尺"].ToString());
                                rangeNO = mTable.Rows[0]["块图号"].ToString();
                            }
                            #endregion

                            string insertIndexTableStr = "";//定义插入成果索引表的字符串

                            insertIndexTableStr += "insert into ProductIndexTable(项目ID,产品ID,比例尺分母,范围号,数据格式编号,数据类型编号,数据ID,生产日期,存储位置) values(";
                            insertIndexTableStr += projectID + "," + productID + ",'" + productScale + "','" + rangeNO + "'," + dataFormatID + "," + dataTypeID + "," + dataID + ",to_date('" + fromDate.ToShortDateString() +"','yyyy-mm-dd')"+ ",'" + savePath + "')";
                            //执行插入操作
                            pSysTable.UpdateTable(insertIndexTableStr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入成果索引表信息出错！");
                                pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                pSysTable.CloseDbConnection();
                                return;
                            }
                            #endregion

                            #region 若存在元信息文件，则也上传元信息文件
                            if (File.Exists(orgDataPath + "\\" + metaFName))
                            {

                                if (!ModDBOperator.UpLoadFile(severStr, user, Password, orgDataPath, metaFName, v_SelNode.Name, metaFName, out ex))
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", ex);
                                    pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            //若数据没有元信息

                            # region 插入元信息

                            try
                            {
                                if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                                {

                                    insertstr = "insert into StandardMapMDTable (";

                                    insertstr += "产品ID,数据文件名,数据格式编号,存储位置,图幅比例尺,图幅号,ID)";
                                    insertstr += " values (" +
                                     productID + ",'" + dataName + "'," + dataFormatID + ",'" + savePath + "',"+productScale+",'" + pureName + "',"+lNewDataID.ToString()+")";

                                }
                                else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                                {
                                    insertstr = "insert into NonstandardMapMDTable (";

                                    insertstr += "产品ID,数据文件名,数据格式编号,存储位置,块图比例尺,块图号,ID)";

                                    insertstr += " values (" +
                                     productID + ",'" + dataName + "'," + dataFormatID + ",'" + savePath + "'," + productScale + ",'" + pureName + "',"+lNewDataID.ToString()+")";
                                }

                                //向数据元数据表中插入新的数据元信息
                                #region 判断元信息数据表中是否存在同名记录，存在试图执行删除
                                if (ModDBOperator.IsFTPExistFile(severStr, user, Password, savePath, dataName, out eError))
                                {
                                    string SQL = "";
                                    string Tablename = "";
                                    int dataFormat = -1;
                                    if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                                    {
                                        Tablename = "StandardMapMDTable";
                                        dataFormat = 0;
                                    }
                                    else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                                    {
                                        Tablename = "NonstandardMapMDTable";
                                        dataFormat = 1;
                                    }
                                    try
                                    {
                                        long DataID = -1;
                                        SQL = "SELECT ID FROM " + Tablename + " WHERE 数据文件名='" + dataName + "'" + " AND  存储位置='" + savePath + "'";
                                        DataTable table = pSysTable.GetSQLTable(SQL, out eError);
                                        if (null != table)
                                        {
                                            DataID = Convert.ToInt64(table.Rows[0][0].ToString());
                                        }
                                        SQL = "DELETE FROM " + Tablename + " WHERE 数据文件名='" + dataName + "'" + " AND  存储位置='" + savePath + "'";
                                        pSysTable.UpdateTable(SQL, out eError);
                                        if (DataID != -1)
                                        {
                                            SQL = "DELETE FROM ProductIndexTable WHERE 数据ID=" + DataID + " AND " + "数据类型编号=" + dataFormat;
                                            pSysTable.UpdateTable(SQL, out eError);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                #endregion
                                pSysTable.UpdateTable(insertstr, out eError);
                                if (eError != null)
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入数据元信息出错！");
                                    pSysTable.EndTransaction(false);
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                            }
                            catch (Exception eex)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eex.Message);
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            #endregion

                            #region 插入成果索引表


                            insertstr = "insert into ProductIndexTable(项目ID,产品ID,比例尺分母,数据格式编号,数据类型编号,数据ID,范围号,生产日期,存储位置) values(";

                            if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                            {
                                mStr = "select * from StandardMapMDTable order by ID desc";
                            }
                            else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                            {
                                mStr = "select * from NonstandardMapMDTable order by ID desc";
                            }
                            else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                            {
                                mStr = "select * from ControlPointMDTable order by ID desc";
                            }
                            mTable = pSysTable.GetSQLTable(mStr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询数据元信息表出错！");
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "数据元信息表为空！");
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            dataID = long.Parse(mTable.Rows[0]["ID"].ToString());   //数据ID

                            if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                            {
                                //productScale = long.Parse(mTable.Rows[0]["图幅比例尺"].ToString());
                                rangeNO = mTable.Rows[0]["图幅号"].ToString();
                            }
                            else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                            {
                                //productScale = long.Parse(mTable.Rows[0]["块图比例尺"].ToString());
                                rangeNO = mTable.Rows[0]["块图号"].ToString();
                            }
                            //DateTime fromDate = Convert.ToDateTime(mTable.Rows[0]["生产日期"].ToString());
                            fromDate = DateTime.Now;
                            //insertstr += projectID + "," + productID + "," + productScale + "," +
                            //    dataFormatID + "," + dataTypeID + "," + dataID + ",'" + rangeNO + "',#" + fromDate + "#,'" + savePath + "')";
                            //*****************************************************************************************************************
                            //guozheng 2010-10-12 改为oracle日期入库sql
                            insertstr += projectID + "," + productID + "," + productScale + "," +
                               dataFormatID + "," + dataTypeID + "," + dataID + ",'" + rangeNO + "',to_date('" + fromDate.ToShortDateString() + "','yyyy-mm-dd')" + ",'" + savePath + "')";
                            //*****************************************************************************************************************
                            //执行插入操作
                            pSysTable.UpdateTable(insertstr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入成果索引表信息出错！");
                                pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                pSysTable.CloseDbConnection();
                                return;
                            }
                            #endregion

                        }

                        #region 上传数据文件
                        string[] fileNameArr = Directory.GetFiles(orgDataPath);  //获得目录下所有文件

                        for (int j = 0; j < fileNameArr.Length; j++)
                        {
                            string fItem = fileNameArr[j];
                            FileInfo tFInfo = new FileInfo(fItem);
                            string ttName = tFInfo.Name;
                            if (ttName.Contains(pureName))
                            {
                                //if (ModDBOperator.IsFTPExistFile(severStr, user, Password, v_SelNode.Name, ttName, out eError))
                                //{
                                //    ModDBOperator.DelNodeByNameAndText(ModData.v_AppFileDB.ProjectTree.SelectedNode, v_SelNode.Name, ttName, out eError);
                                //}
                                //上传数据文件(包含数据文件名的所有数据作为一组数据上传)
                                if (!ModDBOperator.UpLoadFile(severStr, user, Password, orgDataPath, ttName, v_SelNode.Name, ttName, out ex))
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", ex);
                                    pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                            }

                        }
                        #endregion
                        #region 非标准图幅数据试图入库图幅结合表
                        if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                        {
                            string RangeMDBFile = orgDataPath + "\\" + pureName + "_Range.mdb";
                            if (File.Exists(RangeMDBFile))
                            {
                                string Mappath = null;
                                string SQL = "SELECT 图幅结合表 FROM  ProjectMDTable WHERE  ID=" + projectID;
                                string FeatureClassName = "Range_" + productScale;
                                DataTable table = pSysTable.GetSQLTable(SQL, out eError);
                                if (table != null)
                                {
                                    if (table.Rows.Count > 0)
                                        Mappath = table.Rows[0][0].ToString();
                                }
                                if (!string.IsNullOrEmpty(Mappath))
                                {
                                    ModDBOperator.CreateMapRangeByMDB(RangeMDBFile, Mappath,FeatureClassName, out eError);
                                    if (null != eError)
                                       SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", "文件：" + RangeMDBFile + "中获取非标准图幅范围信息失败，\n确认文件中数据格式是否正确.");
                                }
                            }
                        }

                        #endregion

                        #region 将新插入的信息挂在树节点上
                        ModDBOperator.DelNodeByNameAndText(ModData.v_AppFileDB.ProjectTree.SelectedNode, savePath, dataName, out eError);
                        ModData.v_AppFileDB.ProjectTree.Refresh();
                        ModDBOperator.AppendNode(ModData.v_AppFileDB.ProjectTree.SelectedNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), dataName, dataID, savePath, 4);
                        ModData.v_AppFileDB.ProjectTree.Refresh();
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region 控制点数据入库                                                             

                        FrmProcessBar frmbar = new FrmProcessBar();
                        frmbar.Show();

                        #region 从元信息文件中读取元信息
                        SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                        DataTable metaTable = null;                     //元信息表格

                        try
                        {
                            metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + listViewData.Items[i].Text.Trim() + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元信息文件失败！\n连接地址为：" + conStr);
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            metaTable = metaSysTable.GetTable("metadata", out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                metaSysTable.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            if (metaTable.Rows.Count == 0)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "元信息文件为空，请检查！");
                                metaSysTable.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            metaSysTable.CloseDbConnection();
                        }
                        catch (Exception eex)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eex.Message);
                            metaSysTable.CloseDbConnection();
                            pSysTable.CloseDbConnection();
                            frmbar.Dispose();
                            frmbar.Close();
                            return;
                        }
                        #endregion

                        //插入元数据表的字段名
                        DataTable stdTable = pSysTable.GetTable("ControlPointMDTable", out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysTable.CloseDbConnection();
                            frmbar.Dispose();
                            frmbar.Close();
                        }

                        string insertstr2 = "insert into ControlPointMDTable (ID,";
                        for (int j = 1; j < stdTable.Columns.Count; j++)
                        {
                            insertstr2 += stdTable.Columns[j].ColumnName + ",";
                        }
                        insertstr2 = insertstr2.Substring(0, insertstr2.Length - 1) + ")";
                        insertstr2 += " values (";
                        //插入成果索引表的字段名

                        string insertstr1 = "insert into ProductIndexTable(项目ID,产品ID,比例尺分母,数据格式编号,数据类型编号,数据ID,生产日期,存储位置) values(";

                        #region 遍历属性库，进行控制点数据的入库

                        frmbar.SetFrmProcessBarMax(metaTable.Rows.Count);

                        for (int q = 0; q < metaTable.Rows.Count; q++)
                        {
                            string fname = metaTable.Rows[q][1].ToString().Trim() + ".dwg";                //控制点名
                            string mName = metaTable.Rows[q][1].ToString().Trim();

                            frmbar.SetFrmProcessBarText("正在进行控制点数据'" + mName + "'入库：");
                            byte[] dataBt = null;// new byte[60000];                                   //插入SQL
                            if (File.Exists(orgDataPath + "\\" + fname))
                            {
                                FileInfo tempFileInfo = new FileInfo(orgDataPath + "\\" + fname);
                                //该控制点数据存在数据文件

                                dataBt = new byte[tempFileInfo.Length];
                                FileStream fs = new FileStream(orgDataPath + "\\" + fname, FileMode.Open, FileAccess.Read);
                                fs.Read(dataBt, 0, dataBt.Length);
                            }

                            # region 插入元信息
                            //**************************************************************
                            //guozheng added 
                            long lNewID = -1;////////////////////插入控制点表适用的ID
                            lNewID = GetNewTableID("ControlPointMDTable", out eError);
                            if (null != eError)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取控制点表ID失败，\n原因：" + eError.Message);
                                frmbar.Close();
                                return;
                            }
                            string sSqlControlPoint = insertstr2 + lNewID.ToString() + ",";
                            //***************************************************************
                            try
                            {

                                insertstr = sSqlControlPoint + productID + ",'" + mName + "'," + dataFormatID + ",'" + savePath + "'";

                                for (int j = 1; j < metaTable.Columns.Count; j++)
                                {
                                    Type columnType = metaTable.Columns[j].DataType;
                                    string columnValue = metaTable.Rows[q][j].ToString();

                                    string str = ModDBOperator.GetSQl(columnType, columnValue);
                                    insertstr += "," + str;
                                }
                                if (dataBt != null)
                                {
                                    insertstr += ",:dataF)";
                                }
                                else
                                {
                                    insertstr += ",null)";
                                }

                                //向数据元数据表中插入新的数据元信息
                                #region 判断信息库中是否已存在同名的属性信息，存在进行删除
                                string Condition = "存储位置=" + "'" + savePath + "'";
                                if (ModDBOperator.IsTableFildExist("ControlPointMDTable", pSysTable, "控制点名", mName, Condition, out eError))
                                {
                                    string SQL = "";
                                    try
                                    {
                                        long DataID = -1;
                                        SQL = "SELECT ID FROM ControlPointMDTable WHERE 控制点名='" + mName + "'" + " AND  存储位置='" + savePath + "'";
                                        DataTable table = pSysTable.GetSQLTable(SQL, out eError);
                                        if (null == eError && null != table)
                                        {
                                            DataID = Convert.ToInt64(table.Rows[0][0].ToString());
                                        }
                                        SQL = "DELETE FROM ControlPointMDTable WHERE 控制点名='" + mName + "'" + " AND  存储位置='" + savePath + "'";
                                        pSysTable.UpdateTable(SQL, out eError);
                                        if (null != eError)
                                            pSysTable.EndTransaction(false);
                                        if (DataID != -1)
                                        {
                                            SQL = "DELETE FROM ProductIndexTable WHERE  存储位置='" + savePath + "'" + " AND  数据ID=" + DataID + " AND 数据类型编号=2";
                                            pSysTable.UpdateTable(SQL, out eError);
                                            if (null != eError)
                                                pSysTable.EndTransaction(false);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                #endregion
                                OracleConnection OracleConn = pSysTable.DbConn as OracleConnection;
                                OracleTransaction OracleTransection = pSysTable.DBTransaction as OracleTransaction;
                                //oledbTransection.Begin();
                                OracleCommand OracleCmd = new OracleCommand(insertstr, OracleConn, OracleTransection);
                                if (dataBt != null)
                                {
                                    OracleCmd.Parameters.Add("dataF", OracleType.Blob, (int)dataBt.Length);
                                    OracleCmd.Parameters["dataF"].Value = dataBt;
                                }
                                //else
                                //{
                                //    oledbCmd.Parameters["@dataF"].Value = null;
                                //}
                                OracleCmd.ExecuteNonQuery();
                                //oledbTransection.Commit();
                            }
                            catch (Exception eex)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eex.Message);
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            #endregion

                            #region 插入成果索引表


                            #region 查询元数据信息表，获得相应的字段值

                            mStr = "select * from ControlPointMDTable order by ID desc";
                            mTable = pSysTable.GetSQLTable(mStr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询数据元信息表出错！");
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            if (mTable.Rows.Count == 0)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "数据元信息表为空！");
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            dataID = long.Parse(mTable.Rows[0]["ID"].ToString());   //数据ID
                            fromDate = Convert.ToDateTime(mTable.Rows[0]["生产日期"].ToString());
                            #endregion


                            //insertstr = insertstr1 + projectID + "," + productID + "," +productScale+","+
                            //    dataFormatID + "," + dataTypeID + "," + dataID + ",#" + fromDate + "#,'" + savePath + "')";
                            insertstr = insertstr1 + projectID + "," + productID + "," + productScale + "," +
                                dataFormatID + "," + dataTypeID + "," + dataID + ",to_date('" + fromDate.ToShortDateString() +"','yyyy-mm-dd')"+ ",'" + savePath + "')";
                            //执行插入操作
                            pSysTable.UpdateTable(insertstr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入成果索引表信息出错！");
                                pSysTable.EndTransaction(false);////回滚数据入库数据库事务

                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            #endregion

                            //将新插入的信息挂在树节点上
                            ModDBOperator.DelNodeByNameAndText(v_SelNode, savePath, mName, out eError);
                            ModDBOperator.AppendNode(v_SelNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), mName, dataID, savePath, 4);
                            frmbar.SetFrmProcessBarValue(q + 1);
                        #endregion
                        }
                        #endregion

                        frmbar.Dispose();
                        frmbar.Close();
                    }
                    #endregion
                }
                #endregion
                //刷新时间列表
                ModDBOperator.LoadComboxTime(conStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载时间列表框失败，" + eError.Message);
                    pSysTable.CloseDbConnection();
                    return;
                }
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "导入成功！");
            }

            pSysTable.EndTransaction(true);///事务提交
            pSysTable.CloseDbConnection();

            
            this.Close();
            ModData.v_AppFileDB.ProjectTree.SelectedNode.Expanded = false;
            ModData.v_AppFileDB.ProjectTree.SelectedNode.Expand();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            //还需要进行文件过滤

            v_dataFormatNode = v_SelNode;  //产品类型节点（DLG、DEM、DOM、DRG）

            while (v_dataFormatNode.DataKey.ToString() != EnumTreeNodeType.DATAFORMAT.ToString())
            {
                v_dataFormatNode = v_dataFormatNode.Parent;
            }
            if (v_dataFormatNode == null)
            {
                return;
            }

            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择上传的数据";
            if (v_dataFormatNode.Text.Trim() == EnumDataFormat.DLG.ToString())
            {
                //DLG数据
                if (v_SelNode.Text.Trim() != EnumDataType.控制点数据.ToString())
                {
                    OpenFile.Filter = "dwg数据(*.dwg)|*.dwg|SHP数据(*.shp)|*.shp";
                }
                else
                {
                    OpenFile.Filter = "控制点数据(*.mdb)|*.mdb";
                }
            }
            else if (v_dataFormatNode.Text.Trim() == EnumDataFormat.DEM.ToString())
            {
                OpenFile.Filter = "DEM数据(*.dem)|*.dem";
            }
            else if (v_dataFormatNode.Text.Trim() == EnumDataFormat.DOM.ToString())
            {
                OpenFile.Filter = "影像数据(*.img)|*.img|影像数据(*.tif)|*.tif";
            }
            else if (v_dataFormatNode.Text.Trim() == EnumDataFormat.DRG.ToString())
            {
                OpenFile.Filter = "DRG数据(*.*)|*.*";
            }
            //OpenFile.Filter = "dwg数据(*.dwg)|*.dwg|影像数据(*.img)|*.img|控制点数据(*.mdb)|*.mdb";
            OpenFile.Multiselect = true;
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string[] fileNames = OpenFile.FileNames;//打开的文件夹名

                for (int i = 0; i < fileNames.Length; i++)
                {
                    ListViewItem aItem = listViewData.Items.Add(fileNames[i], fileNames[i], "");
                    aItem.ToolTipText = fileNames[i];
                }
            }

            if (listViewData.Items.Count == 0)
            {
                buttonX2.Enabled = false;
                buttonX3.Enabled = false;
                buttonX4.Enabled = false;
            }
            else 
            {
                buttonX2.Enabled = true;
                buttonX3.Enabled = true;
                buttonX4.Enabled = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dataGridViewMeta.CurrentCell.Value = dateTimePicker1.Value;
            dateTimePicker1.Visible = false;

        }

        private void dataGridViewMeta_Scroll(object sender, ScrollEventArgs e)
        {
            dateTimePicker1.Visible = false;
            this.MapRangeBox.Visible = false;
            this.ProjectRangeBox.Visible = false;
            this.cmbScale.Visible = false;
        }

        private void dataGridViewMeta_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dateTimePicker1.Visible = false;
            this.MapRangeBox.Visible = false;
            this.ProjectRangeBox.Visible = false;
            this.cmbScale.Visible = false;
        }
        private void dataGridViewMeta_CurrentCellChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Visible = false;
            if (dataGridViewMeta.CurrentCell == null) return;
            //将dataGridView输入日期与dataTimePicker相关联

            if (this.dataGridViewMeta.CurrentCell.ColumnIndex == 1)
            {
                if (this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[1].ValueType.FullName == "System.DateTime")
                {
                    
                    if (this.Text != "数据入库")
                    {
                        Rectangle rect = dataGridViewMeta.GetCellDisplayRectangle(dataGridViewMeta.CurrentCell.ColumnIndex, dataGridViewMeta.CurrentCell.RowIndex, false);
                        
                        Rectangle rect1 = dataGridViewMeta.GetCellDisplayRectangle(dataGridViewMeta.CurrentCell.ColumnIndex, dataGridViewMeta.CurrentCell.RowIndex, false);
                        try
                        {
                            dateTimePicker1.Value = Convert.ToDateTime(this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[1].Value);
                        }
                        catch
                        {
                            dateTimePicker1.Value = DateTime.Now.Date;
                        }                        
                        dateTimePicker1.Left = rect.Left;
                        dateTimePicker1.Top = rect.Top + 47;
                        dateTimePicker1.Width = rect.Width;
                        dateTimePicker1.Height = rect.Height;
                    }
                    else
                    {
                        Rectangle rect1 = dataGridViewMeta.GetCellDisplayRectangle(dataGridViewMeta.CurrentCell.ColumnIndex, dataGridViewMeta.CurrentCell.RowIndex, false);
                        dateTimePicker1.Value = DateTime.Now.Date;
                        dateTimePicker1.Left = rect1.Left + 200;
                        dateTimePicker1.Top = rect1.Top + 47;
                        dateTimePicker1.Width = rect1.Width;
                        dateTimePicker1.Height = rect1.Height;
                    }

                   

                    dateTimePicker1.Visible = true;
                }
                else
                {
                    dateTimePicker1.Visible = false;
                }
                if (this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0].FormattedValue .ToString()== "图幅结合表")
                {
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择项目范围库";
                    OpenFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    OpenFile.Multiselect = false;
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[1].Value = OpenFile.FileName;
                    }

                }
                if (this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0].FormattedValue.ToString() == "底图")
                {
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择项目索引图";
                    OpenFile.Filter = "mxd数据(*.mxd)|*.mxd";
                    OpenFile.Multiselect = false;
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[1].Value = OpenFile.FileName;
                    }

                }

                if (this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0].FormattedValue.ToString() == "比例尺分母")
                {
                    Rectangle rect = dataGridViewMeta.GetCellDisplayRectangle(dataGridViewMeta.CurrentCell.ColumnIndex, dataGridViewMeta.CurrentCell.RowIndex, false);
                    this.cmbScale.Items.Clear();
                    this.cmbScale.Left = rect.Left;
                    this.cmbScale.Top = rect.Top + 47;
                    this.cmbScale.Width = rect.Width;
                    this.cmbScale.Height = rect.Height;
                    this.cmbScale.Visible = true;

                    this.cmbScale.Items.Clear();
                    this.cmbScale.Items.AddRange(new object[] { "500", "1000", "2000", "5000", "10000", "20000", "50000" });
                    this.cmbScale.SelectedIndex = 0;
                }
                else 
                {
                    this.cmbScale.Visible =false;
                }
               
            }
            else
            {
                dateTimePicker1.Visible = false;
            }


            if (this.dataGridViewMeta.CurrentCell.ColumnIndex == 1)
            {
                if (null == this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0])
                    return;
                if (null == this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0].Value)
                    return;
                #region 获取范围信息表中的范围号增加到新建产品的DataGridView的下拉列表中
                //////////////获取范围信息表中的范围号增加到新建产品的DataGridView的下拉列表MapRangeBox中//////////////////////////
                if (this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0].Value.ToString().Trim() == "范围号")
                {
                    Rectangle rect = dataGridViewMeta.GetCellDisplayRectangle(dataGridViewMeta.CurrentCell.ColumnIndex, dataGridViewMeta.CurrentCell.RowIndex, false);
                    this.MapRangeBox.Items.Clear();
                    this.MapRangeBox.Left = rect.Left;
                    this.MapRangeBox.Top = rect.Top + 47;
                    this.MapRangeBox.Width = rect.Width;
                    this.MapRangeBox.Height = rect.Height;
                    /////////////////获取元数据库中的范围号/////////
                    Exception eError = null;
                    if (v_DBNode.Tag == null) return;
                    XmlElement conElem = v_DBNode.Tag as XmlElement;  //连接信息节点
                    string conStr = conElem.GetAttribute("MetaDBConn");
                    //*************************************************************************************
                    //guozheng 2010-10-11 
                    //改为Oracle连接方式
                    //pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + conStr + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                    pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                    //****************************************************************************************
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + conStr);
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    string str = "select 范围号 from ProductRange";
                    DataTable table = pSysTable.GetSQLTable(str, out eError);
                    if (null != eError)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "读取范围信息表发生错误!");
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    if (table.Rows.Count > 0)
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            this.MapRangeBox.Items.Add(table.Rows[i][0].ToString().Trim());
                        }
                    }
                    pSysTable.CloseDbConnection();
                    this.MapRangeBox.Visible = true;
                }
                else
                {
                    this.MapRangeBox.Visible = false;
                }
                #endregion
                #region 获取产品的范围信息表
                if (this.dataGridViewMeta.Rows[this.dataGridViewMeta.CurrentCell.RowIndex].Cells[0].Value.ToString().Trim() == "项目范围号")
                {
                    Rectangle rect = dataGridViewMeta.GetCellDisplayRectangle(dataGridViewMeta.CurrentCell.ColumnIndex, dataGridViewMeta.CurrentCell.RowIndex, false);
                    this.ProjectRangeBox.Items.Clear();
                    this.ProjectRangeBox.Left = rect.Left;
                    this.ProjectRangeBox.Top = rect.Top;
                    this.ProjectRangeBox.Width = rect.Width;
                    this.ProjectRangeBox.Height = rect.Height;
                    /////////////////获取元数据库中的范围号/////////
                    Exception eError = null;
                    if (v_DBNode.Tag == null) return;
                    XmlElement conElem = v_DBNode.Tag as XmlElement;  //连接信息节点
                    string conStr = conElem.GetAttribute("MetaDBConn");
                    //****************************************************************************
                    //guozheng 2010-10-11 
                    //改为Oracle连接方式
                   // pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + conStr + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                    pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                    //*****************************************************************************
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + conStr);
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    string str = "select 范围号 from ProjectRange";
                    DataTable table = pSysTable.GetSQLTable(str, out eError);
                    if (null != eError)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "读取范围信息表发生错误!");
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    if (table.Rows.Count > 0)
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            this.ProjectRangeBox.Items.Add(table.Rows[i][0].ToString().Trim());
                        }
                    }
                    pSysTable.CloseDbConnection();
                    this.ProjectRangeBox.Visible = true;
                }

                else
                {
                    this.ProjectRangeBox.Visible = false;
                }
                #endregion
            }
            else
            {
                this.MapRangeBox.Visible = false;
                this.ProjectRangeBox.Visible = false;
            }

        }

        private void MapRangeBox_SelectedValueChanged(object sender, EventArgs e)
        {
            dataGridViewMeta.CurrentCell.Value = this.MapRangeBox.Text;
        }
        private void ProjectRangeBox_SelectedValueChanged(object sender, EventArgs e)
        {
            dataGridViewMeta.CurrentCell.Value = this.ProjectRangeBox.Text;
        }

        private void dataGridViewMeta_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridViewMeta.Rows[e.RowIndex].ErrorText = string.Empty;
        }

        private void dataGridViewMeta_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int i = e.RowIndex;
                Type type = this.dataGridViewMeta.Rows[i].Cells[1].ValueType;
                string columnValue = e.FormattedValue.ToString();
                #region 判断是否符合输入格式
                if (type.FullName == "System.String")
                {
                   
                }
                else if (type.FullName == "System.Decimal")
                {
                    try
                    {
                        System.Convert.ToDouble(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的数字类型";
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        return;
                    }
                }
                else if (type.FullName == "System.Int32")
                {
                    try
                    {
                        System.Convert.ToInt32(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的数字类型";
                        //dataGridViewMeta.Rows[i].Cells[1].Value = 0;
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        return;
                    }
                }
                else if (type.FullName == "System.Int16")
                {
                    try
                    {
                        System.Convert.ToInt16(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的数字类型";
                        //dataGridViewMeta.Rows[i].Cells[1].Value = 0;
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        return;
                    }
                }
                else if (type.FullName == "System.Int64")
                {
                    try
                    {
                        System.Convert.ToInt64(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的数字类型";
                        //dataGridViewMeta.Rows[i].Cells[1].Value = 0;
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        return;
                    }
                }
                else if (type.FullName == "System.Double")
                {
                    try
                    {
                        System.Convert.ToDouble(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的数字类型";
                        //dataGridViewMeta.Rows[i].Cells[1].Value = 0;
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        // e.Cancel = true;
                        return;
                    }
                }
                else if (type.FullName == "System.Single")
                {
                    try
                    {
                        System.Convert.ToSingle(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;
                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的数字类型";
                        //dataGridViewMeta.Rows[i].Cells[1].Value = 0;
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        return;
                    }
                }
                else if (type.FullName == "System.DateTime")
                {
                    try
                    {
                        dataGridViewMeta.Rows[i].Cells[1].Value = System.Convert.ToDateTime(columnValue);
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = string.Empty;

                    }
                    catch
                    {
                        dataGridViewMeta.Rows[i].Cells[1].ErrorText = "请填写正确的时间";
                        // dataGridViewMeta.Rows[i].Cells[1].Value = 0;
                        dataGridViewMeta.Rows[i].Cells[1].Selected = true;
                        return;
                    }

                }
                #endregion
            }
            dataGridViewMeta.Rows[e.RowIndex].ErrorText = string.Empty;
            e.Cancel = false;
        }


        //新建记录表格绑定
        private void NewRecordDataBinding(DataTable newItemTable)
        {
            if (newItemTable == null) return;
            for (int i = 0; i < newItemTable.Columns.Count; i++)
            {

                //DataRow newRow = dt.NewRow();
                if (newItemTable.Columns[i].ColumnName == "ID") continue;
                if (newItemTable.Columns[i].ColumnName == "数据格式编号") continue;
                if (newItemTable.Columns[i].ColumnName == "存储位置") continue;
                if (newItemTable.Columns[i].ColumnName == "项目ID") continue;
                if (newItemTable.Columns[i].ColumnName == "产品ID") continue;
                if (newItemTable.Columns[i].ColumnName == "存储位置") continue;
                if (newItemTable.Columns[i].ColumnName == "数据文件名") continue;
                //if (newItemTable.Columns[i].ColumnName == "图幅号") continue;
                //if (newItemTable.Columns[i].ColumnName == "块图号") continue;
                //if (newItemTable.Columns[i].ColumnName == "图幅比例尺") continue;
                //if (newItemTable.Columns[i].ColumnName == "块图比例尺") continue;
                //if (newItemTable.Columns[i].ColumnName == "存储时间") continue;

                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridViewMeta);
                newRow.Cells[0].Value = newItemTable.Columns[i].ColumnName;
                newRow.Cells[1].ValueType = newItemTable.Columns[i].DataType;

                dataGridViewMeta.Rows.Add(newRow);
                //newRow["字段名称"] = newItemTable.Columns[i].ColumnName;
                //dt.Rows.Add(newRow);
            }
        }

        //修改记录表格绑定
        private void AlterRecordDataBinding(DataTable alterItemTable)
        {
            if (alterItemTable == null) return;
            if (alterItemTable.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "该节点元信息不存在");
                pSysTable.CloseDbConnection();
                return;
            }
            for (int i = 0; i < alterItemTable.Columns.Count; i++)
            {
                //DataRow newRow = dt.NewRow();
                if (alterItemTable.Columns[i].ColumnName == "ID") continue;
                if (alterItemTable.Columns[i].ColumnName == "数据格式编号") continue;
                if (alterItemTable.Columns[i].ColumnName == "存储位置") continue;
                if (alterItemTable.Columns[i].ColumnName == "项目ID") continue;
                if (alterItemTable.Columns[i].ColumnName == "产品ID") continue;
                if (alterItemTable.Columns[i].ColumnName == "存储位置") continue;
                if (alterItemTable.Columns[i].ColumnName == "数据文件名") continue;
                //if (alterItemTable.Columns[i].ColumnName == "图幅号") continue;
                //if (alterItemTable.Columns[i].ColumnName == "块图号") continue;
                if (alterItemTable.Columns[i].ColumnName == "图形数据") continue;
                if (alterItemTable.Columns[i].ColumnName == "比例尺分母") continue;
                //if (alterItemTable.Columns[i].ColumnName == "图幅比例尺") continue;
                //if (alterItemTable.Columns[i].ColumnName == "块图比例尺") continue;


                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridViewMeta);
                newRow.Cells[0].Value = alterItemTable.Columns[i].ColumnName;
                newRow.Cells[1].ValueType = alterItemTable.Columns[i].DataType;
                if (alterItemTable.Columns[i].ColumnName.Trim() == "开始时间" || alterItemTable.Columns[i].ColumnName.Trim() == "结束时间" || alterItemTable.Columns[i].ColumnName.Trim() == "生产日期")/////6.8郭正修改 gub GO-3858
                {
                    try
                    {
                        newRow.Cells[1].Value = Convert.ToDateTime(alterItemTable.Rows[0][alterItemTable.Columns[i].ColumnName].ToString()).ToShortDateString();
                        //newRow.Cells[1].Value = alterItemTable.Rows[0][alterItemTable.Columns[i].ColumnName].ToString().Substring(0, alterItemTable.Rows[0][alterItemTable.Columns[i].ColumnName].ToString().IndexOf(':') - 2);
                    }
                    catch
                    {
                        newRow.Cells[1].Value = null;
                    }
                }
                else
                {
                    newRow.Cells[1].Value = alterItemTable.Rows[0][alterItemTable.Columns[i].ColumnName].ToString();
                }
                
                if (newRow.Cells[0].Value.ToString() == "项目编号" || newRow.Cells[0].Value.ToString() == "项目名称")
                {
                    newRow.Cells[1].ReadOnly = true;
                    newRow.Cells[1].Style.ForeColor = Color.Gray;
                    newRow.Cells[1].Style.SelectionForeColor = Color.Gray;/////6.07 郭正修改 bug GO-3857
                }
                if (newRow.Cells[0].Value.ToString() == "产品编号" || newRow.Cells[0].Value.ToString() == "产品名称")
                {
                    newRow.Cells[1].ReadOnly = true;
                    newRow.Cells[1].Style.ForeColor = Color.Gray;
                    newRow.Cells[1].Style.SelectionForeColor = Color.Gray;/////6.07 郭正修改 bug GO-3857
                   
                }

                dataGridViewMeta.Rows.Add(newRow);
            }
        }

        //导入数据信息，并进行绑定
        private void ImportDataBinding(DataTable alterItemTable, out Exception eError)
        {
            eError = null;
            if (alterItemTable.Columns.Count - 1 != dataGridViewMeta.Rows.Count)
            {
                eError = new Exception("字段数目不匹配，请检查！");
                return;
            }
            //int i = 2;
            for (int j = 0; j < dataGridViewMeta.Rows.Count; j++)
            {
               
                dataGridViewMeta.Rows[j].Cells[1].Value = alterItemTable.Rows[0][j+1].ToString();
                dataGridViewMeta.Rows[j].Cells[1].ValueType = alterItemTable.Columns[j + 1].DataType;
                //i++;
                if (dataGridViewMeta.Rows[j].Cells[1].ValueType.FullName == "System.DateTime")
                {
                    try
                    {
                        dataGridViewMeta.Rows[j].Cells[1].Value = Convert.ToDateTime(dataGridViewMeta.Rows[j].Cells[1].Value.ToString()).ToShortDateString();
                    }
                    catch
                    {
                    }
                }
            }
            dataGridViewMeta.Update();
        }

        /// <summary>
        /// 获得插入和修改数据库的SQL语句
        /// </summary>
        /// <param name="columnType">字段类型</param>
        /// <param name="columnName">字段名</param>
        /// <param name="columnValue">字段值</param>
        /// <param name="valueStrBuild">插入字符串</param>
        /// <param name="updateStr">修改字符串</param>
        private void GetSQL(Type columnType, string columnName, string columnValue, out StringBuilder valueStrBuild,out string updateStr,out Exception eError)
        {
            valueStrBuild = new StringBuilder();
            updateStr = "";
            eError = null;
            if (columnType.FullName == "System.String")
            {
                valueStrBuild.Append("'" + columnValue.Trim() + "',");
                updateStr += columnName + "='" + columnValue.Trim() + "',";
            }
            else if(columnType.FullName == "System.Decimal")
            {
                try
                {
                    System.Convert.ToDouble(columnValue);
                }
                catch
                {
                    eError = new Exception("'" + columnName + "’为数字型，请填写数字！");
                    return;
                }
                valueStrBuild.Append(columnValue + ",");
                updateStr += columnName + "=" + columnValue + ",";
            }
            else if (columnType.FullName == "System.Int32")
            {
                try
                {
                    System.Convert.ToInt32(columnValue);
                }
                catch
                {
                    eError = new Exception("'" + columnName + "’为数字型，请填写数字！");
                    return;
                }
                valueStrBuild.Append(columnValue + ",");
                updateStr += columnName + "=" + columnValue + ",";
            }
            else if (columnType.FullName == "System.Int16")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                try
                {
                    System.Convert.ToInt16(columnValue);
                }
                catch
                {
                    eError = new Exception("'" + columnName + "’为数字型，请填写数字！");
                    return;
                }
                valueStrBuild.Append(columnValue + ",");
                updateStr += columnName + "=" + columnValue + ",";
            }
            else if (columnType.FullName == "System.Int64")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                try
                {
                    System.Convert.ToInt64(columnValue);
                }
                catch
                {
                    eError = new Exception("'" + columnName + "’为数字型，请填写数字！");
                    return;
                }
                valueStrBuild.Append(columnValue + ",");
                updateStr += columnName + "=" + columnValue + ",";
            }
            else if (columnType.FullName == "System.Double")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                try
                {
                    System.Convert.ToDouble(columnValue);
                }
                catch
                {
                    eError = new Exception("'" + columnName + "’为数字型，请填写数字！");
                    return;
                }

                valueStrBuild.Append(columnValue + ",");
                updateStr += columnName + "=" + columnValue + ",";
            }
            else if (columnType.FullName == "System.Single")
            {
                if (columnValue == "")
                {
                    columnValue = "0";
                }
                try
                {
                    System.Convert.ToSingle(columnValue);
                }
                catch
                {
                    eError = new Exception("'" + columnName + "’为数字型，请填写数字！");
                    return;
                }
                valueStrBuild.Append(columnValue + ",");
                updateStr += columnName + "=" + columnValue + ",";
            }
            else if (columnType.FullName == "System.DateTime")
            {
                if (columnValue == "")
                {
                    columnValue = DateTime.MinValue.ToShortDateString();
                }
                //valueStrBuild.Append("#" + columnValue + "#,");
                //updateStr += columnName + "=#" + columnValue + "#,";
                //*********************************************************
                //guozheng 2010-10-12 
                //改为Oracle入库方式
                valueStrBuild.Append("to_date('"+columnValue+"','yyyy-mm-dd'),");
                updateStr += columnName + "=to_date('"+columnValue+"','yyyy-mm-dd'),";
                //************************************************************

            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            //将表格信息挂在listviewData上

            if (dataGridViewMeta.Rows.Count > 0)
            {
                DataGridViewRow[] dgRows = new DataGridViewRow[dataGridViewMeta.Rows.Count];
                for (int i = 0; i < dataGridViewMeta.Rows.Count; i++)
                {
                    if (dataGridViewMeta.Rows[i].Cells[0].FormattedValue.ToString() == "" || dataGridViewMeta.Rows[i].Cells[0].Value == null) continue;
                    dgRows[i] = dataGridViewMeta.Rows[i];

                }
                if (listViewData.FocusedItem == null)
                {
                    listViewData.Items[0].Tag = dgRows;
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个与元信息关联的数据项！");
                    //return;
                }
                else
                {
                    listViewData.FocusedItem.Tag = dgRows;
                }
            }
           
        }

        private void listViewData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {  
            if (dataGridViewMeta.Rows.Count > 0)
            {
                dataGridViewMeta.Rows.Clear();
            }

            if (e.Item.Tag != null)
            {
                DataGridViewRow[] pDataItemRowCol = e.Item.Tag as DataGridViewRow[];
                dataGridViewMeta.Rows.AddRange(pDataItemRowCol);
            }
            else
            {
                NewRecordDataBinding(newTable);
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            //删除选中项

            foreach(ListViewItem lvItem in listViewData.SelectedItems )
            {
                listViewData.Items.Remove(lvItem);
            }
            listViewData.Update();

            if (listViewData.Items.Count == 0)
            {
                buttonX2.Enabled = false;
                buttonX3.Enabled = false;
                buttonX4.Enabled = false;
            }
            else
            {
                buttonX2.Enabled = true;
                buttonX3.Enabled = true;
                buttonX4.Enabled = true;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Exception eError=null;
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择要导入的元信息文件";
            OpenFile.Filter = "mdb数据(*.mdb)|*.mdb";
            OpenFile.Multiselect = false;
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = OpenFile.FileName;//打开的文件夹名


                DataTable metaTable = null;
                #region 打开元数据信息库
                SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元信息文件失败！\n连接地址为：" + fileName);
                    return; 
                }
                metaTable = metaSysTable.GetTable("metadata", out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message+",在库中未找到表格‘metadata’！");
                    metaSysTable.CloseDbConnection();
                    return;
                }
                if (metaTable == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "在库中未找到表格‘metadata’！");
                    metaSysTable.CloseDbConnection();
                    return;

                }
                if (metaTable.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "元信息文件没有记录，请检查！");
                    metaSysTable.CloseDbConnection();
                    return;
                }
                metaSysTable.CloseDbConnection();
                #endregion 

                try
                {
                   //读取元数据信息表，并将信息显示在界面上 
                    ImportDataBinding(metaTable, out eError);
                    if(eError!=null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    return;
                }
            }

            
            //if (this.Text == "数据入库")
            //{
            //    //将表格信息挂在listviewData上
            //    if (dataGridViewMeta.Rows.Count > 0)
            //    {
            //        DataGridViewRow[] dgRows = new DataGridViewRow[dataGridViewMeta.Rows.Count];
            //        for (int i = 0; i < dataGridViewMeta.Rows.Count; i++)
            //        {
            //            if (dataGridViewMeta.Rows[i].Cells[0].FormattedValue.ToString() == "" || dataGridViewMeta.Rows[i].Cells[0].Value == null) continue;
            //            dgRows[i] = dataGridViewMeta.Rows[i];

            //        }
            //        if (listViewData.FocusedItem == null)
            //        {
            //            listViewData.Items[0].Tag = dgRows;
            //            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个与元信息关联的数据项！");
            //            //return;
            //        }
            //        else
            //        {
            //            listViewData.FocusedItem.Tag = dgRows;
            //        }
            //    }
            //}
            
        }

        private void dateTimePicker1_VisibleChanged(object sender, EventArgs e)
        {
            if (dateTimePicker1.Visible == true)
            {
                dataGridViewMeta.CurrentCell.Value = dateTimePicker1.Value;
                
            }
        }

        private void dataGridViewMeta_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

            dataGridViewMeta.Rows[e.RowIndex].ErrorText = string.Empty;
            e.Cancel = false;
        }

        private void dataGridViewMeta_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void cmbScale_SelectedValueChanged(object sender, EventArgs e)
        {
            if (dataGridViewMeta.CurrentCell != null)
            {
                dataGridViewMeta.CurrentCell.Value = this.cmbScale.Text;
            }
            
        }

        private void dataGridViewMeta_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        /// <summary>
        /// 检查字符串中非法字符,有非法字符返回false
        /// </summary>
        /// <returns></returns>
        private bool CheckProjectName(string CheckName)
        {
            string s_nonlicet = string.Empty;
            if (null == CheckName)
                return false;
            string ProjectName = CheckName;         
            if (ProjectName.Contains("<")) s_nonlicet += "<,";
            if (ProjectName.Contains("\"")) s_nonlicet += "\",";
            if (ProjectName.Contains("%")) s_nonlicet += "%,";
            if (ProjectName.Contains("?")) s_nonlicet += "?,";
            if (ProjectName.Contains("^")) s_nonlicet += "^,";
            if (ProjectName.Contains("\'")) s_nonlicet += "\',";
            if (ProjectName.Contains("&")) s_nonlicet += "&,";
            if (ProjectName.Contains("$")) s_nonlicet += "$,";
            if (ProjectName.Contains("#")) s_nonlicet += "#,";
            if (ProjectName.Contains("@")) s_nonlicet += "@,";
            if (ProjectName.Contains("~")) s_nonlicet += "~,";
            if (ProjectName.Contains("*")) s_nonlicet += "*,";
            if (ProjectName.Contains("!")) s_nonlicet += "!,";
            if (ProjectName.Contains(">")) s_nonlicet += ">,";
            if (ProjectName.Contains("/")) s_nonlicet += "/,";
            if (ProjectName.Contains("\\")) s_nonlicet += "\\,";
            if (ProjectName.Contains(";")) s_nonlicet += ";,";
            if (s_nonlicet != string.Empty)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "非法字符：" + s_nonlicet);
             /////   state = "非法字符：" + s_nonlicet;
                return false;
            }
            else
                return true;

        }

        /// <summary>
        /// 在插入项目表的时候，获取一个可用的最大ID
        /// </summary>
        /// <param name="ex">输出：错误信息</param>
        /// <returns></returns>
        private long GetNewTableID(string sTableName, out Exception ex)
        {
            ex = null;
            long lid = -1;
            if (this.pSysTable == null) { ex = new Exception("元信息库连接信息未初始化"); return -1; }
            //////////从数据库中中查询ID最大值加1返回，没有记录返回1///////
            string sql = "SELECT COUNT(*) FROM " + sTableName;
            DataTable gettable = this.pSysTable.GetSQLTable(sql, out ex);
            if (ex != null) return -1;
            try
            {
                int count = Convert.ToInt32(gettable.Rows[0][0].ToString());
                if (count == 0) return 1;
                else
                {
                    sql = "SELECT MAX(ID) FROM " + sTableName;
                    gettable = this.pSysTable.GetSQLTable(sql, out ex);
                    if (ex != null) return -1;
                    lid = Convert.ToInt32(gettable.Rows[0][0].ToString());
                    lid += 1;
                    return lid;
                }
            }
            catch(Exception eError)
            {
                ex = eError;
                return -1;
            }
        }
 
    }
}