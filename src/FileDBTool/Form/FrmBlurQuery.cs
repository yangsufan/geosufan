using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace FileDBTool
{
    /// <summary>
    /// 模糊查询
    /// </summary>
    public partial class FrmBlurQuery : DevComponents.DotNetBar.Office2007Form
    {
        Plugin.Application.IAppFileRef m_Hook = null;
        string ConnStr = "";               //元信息表字符串
        List<string> m_FieldLst = null;     //字段集合
        Dictionary<string, Type> m_FieldDic = null; 

        public FrmBlurQuery(Plugin.Application.IAppFileRef hook, DevComponents.AdvTree.Node pDBNode)
        {
            InitializeComponent();

            m_Hook=hook;

            //获得根节点,数据库节点
            DevComponents.AdvTree.Node mDBNode = pDBNode;
            XmlElement dbElem = mDBNode.Tag as XmlElement;
            if (dbElem == null) return;
            string ipStr = dbElem.GetAttribute("MetaDBConn");

           // ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
            ConnStr = ipStr;
            //初始化界面
            IntialCom();
        }

        private void IntialCom()
        {
             //初始化匹配方式下拉列表框
            cmbMatch.Items.Clear();
            cmbMatch.Items.AddRange(new object[] { "完全匹配", "首字母匹配", "任意匹配" });
            cmbMatch.SelectedIndex = 0;
            //初始化检索范围下拉列表框
            cmbType.Items.Clear();
            cmbType.Items.AddRange(new object[] { "标准图幅", "非标准图幅", "控制点数据" });
            cmbType.SelectedIndex = 0;
        }

        

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //初始化字段信息
            cmbField.Items.Clear();
            cmbField.Items.Add("所有字段");
            m_FieldDic= AddField();
            cmbField.SelectedIndex = 0;
        }

        private Dictionary<string,Type> AddField()
        {
            Exception eError=null;
            Dictionary<string, Type> fieldDic = new Dictionary<string, Type>();

            if (ConnStr == "") return null;
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();
            pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！");
                pSysDB.CloseDbConnection();
                return null;
            }
            string pTableName = "";
            if (cmbType.Text == "标准图幅")
            {
                pTableName = "StandardMapMDTable";
            }
            else if (cmbType.Text == "非标准图幅")
            {
                pTableName = "NonstandardMapMDTable";
            }
            else if (cmbType.Text == "控制点数据")
            {
                pTableName = "ControlPointMDTable";
            }
            DataTable pTable = pSysDB.GetTable(pTableName, out eError);
            if(eError!=null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "获取源数据信息表失败！");
                pSysDB.CloseDbConnection();
                return null;
            }
            pSysDB.CloseDbConnection();
            for (int i = 0; i < pTable.Columns.Count; i++)
            {
                if (pTable.Columns[i].ColumnName.ToString().Trim() == "ID")
                {
                    continue;
                }
                if (pTable.Columns[i].ColumnName.ToString().Trim() == "图形数据")
                {
                    continue;
                }

                if (!cmbField.Items.Contains(pTable.Columns[i].ColumnName.ToString().Trim()))
                {
                    cmbField.Items.Add(pTable.Columns[i].ColumnName.ToString().Trim());
                    fieldDic.Add(pTable.Columns[i].ColumnName.ToString().Trim(), pTable.Columns[i].DataType);
                }
            }
            return fieldDic;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Hide();
            Exception eError = null;
            string fieldVale = txtValue.Text.ToString();       //字段值
            if (fieldVale == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请填写关键字！");
                return;
            }
            string strMatch = cmbMatch.Text.Trim();       //匹配类型

            #region 申明表格并进行绑定
            DataTable resalutDT = ModDBOperator.CreateDataInfoTable();
            //清空表格
            if (m_Hook.DataInfoGrid.DataSource != null)
            {
                m_Hook.DataInfoGrid.DataSource = null;
            }
            //绑定
            m_Hook.DataInfoGrid.DataSource = resalutDT;
            m_Hook.DataInfoGrid.ReadOnly = true;
            m_Hook.DataInfoGrid.Visible = true;
            m_Hook.DataInfoGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            for (int j = 0; j < m_Hook.DataInfoGrid.Columns.Count; j++)
            {
                //m_Hook.DataInfoGrid.Columns[j].Width = (m_Hook.DataInfoGrid.Width - 20) / m_Hook.DataInfoGrid.Columns.Count;
                m_Hook.DataInfoGrid.Columns[j].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
            m_Hook.DataInfoGrid.RowHeadersWidth = 20;
            #endregion

            #region 连接数据库
            if (ConnStr == "") return;
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();
            pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！");
                pSysDB.CloseDbConnection();
                return;
            }
            #endregion
            #region 首先查询成果索引表，获得符合条件的数据ID
            string dataIDStandarStr = "";        //标准图幅过滤条件
            string dataIDNonStandarStr = "";     //非标准图幅过滤条件
            string dataIDControlStr = "";       //控制点过滤条件
            string wherestr = GetWhereStr();//过滤条件
            string str2 = "";
            if (wherestr != "")
            {
                str2 = "select * from ProductIndexTable where " + wherestr; //查询成果索引表
                DataTable tempDt=pSysDB.GetSQLTable(str2, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询成果索引表失败！");
                    pSysDB.CloseDbConnection();
                    return;
                }
                for(int i=0;i<tempDt.Rows.Count;i++)
                {
                    int dataType = Convert.ToInt32(tempDt.Rows[i]["数据类型编号"].ToString()) ;
                    long pDataId = Convert.ToInt64(tempDt.Rows[i]["数据ID"].ToString());
                    if(dataType==0)
                    {
                        //标准图幅
                        dataIDStandarStr += pDataId + ",";
                    }
                    else if (dataType == 1)
                    {
                        //非标准图幅
                        dataIDNonStandarStr += pDataId + ",";
                    }
                    else if (dataType == 2)
                    {
                        //控制点数据
                        dataIDControlStr += pDataId + ",";
                    }
                }
                if(dataIDStandarStr !="")
                {
                    dataIDStandarStr = dataIDStandarStr.Substring(0, dataIDStandarStr.Length - 1);
                }
                if (dataIDNonStandarStr != "")
                {
                    dataIDNonStandarStr = dataIDNonStandarStr.Substring(0, dataIDNonStandarStr.Length - 1);
                }
                if (dataIDControlStr != "")
                {
                    dataIDControlStr = dataIDControlStr.Substring(0, dataIDControlStr.Length - 1);
                }
            }
            #endregion
            //再次进行过滤查询表格
            string pTableName = "";
            string restrainStr = "";
            if (cmbType.Text == "标准图幅")
            {
                pTableName = "StandardMapMDTable";
                if(dataIDStandarStr!="")
                {
                    restrainStr = dataIDStandarStr;
                }
            }
            else if (cmbType.Text == "非标准图幅")
            {
                pTableName = "NonstandardMapMDTable";
                if (dataIDNonStandarStr!= "")
                {
                    restrainStr = dataIDNonStandarStr;
                }
            }
            else if (cmbType.Text == "控制点数据")
            {
                pTableName = "ControlPointMDTable";
                if (dataIDControlStr!= "")
                {
                    restrainStr = dataIDControlStr;
                }
            }
            //DataTable mTable = pSysDB.GetTable(pTableName, out eError);
            //if (eError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "获取源数据信息表失败！");
            //    return;
            //}

            try
            {
                if (cmbField.Text == "所有字段")
                {
                    if (m_FieldDic == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "没有可用的字段！");
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    foreach (KeyValuePair<string, Type> pField in m_FieldDic)
                    {
                        string fName = pField.Key.Trim();
                        Type fType = pField.Value;
                        string pSQL = GetSQL(fName, fieldVale, fType, strMatch, out eError);
                        if (eError != null)
                        {
                            pSysDB.CloseDbConnection();
                            return;
                        }
                        string str1 = "";
                        if(m_Hook.ProjectTree.SelectedNode.DataKey.ToString()==EnumTreeNodeType.DATABASE.ToString())
                        {
                            //如果是数据库节点没有任何限制条件
                            str1 = "select * from " + pTableName + " where " + pSQL;
                        }
                        else 
                        {
                            //如果是其他的节点则肯定 有限制条件
                            if (restrainStr != "")
                            {
                                //查询到数据
                                str1 = "select * from " + pTableName + " where ID in (" + restrainStr + ") and " + pSQL;

                            }
                            else 
                            {
                               //查不到数据
                                return;
                            }
                        }
                        //str1 = "select * from " + pTableName + " where ID in (" + restrainStr + ") and 数据文件名 like '%3%'";
                        DataTable pTable = pSysDB.GetSQLTable(str1, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "获取源数据信息表失败！");
                            return;
                        }
                        //DataRow[] pRowArr = mTable.Select(pSQL);

                        showResault(pSysDB, pTable, resalutDT, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eError.Message);
                            pSysDB.CloseDbConnection();
                            return;
                        }
                    }
                }
                else
                {
                    string fieldName = cmbField.Text.Trim();
                    Type fieldType = m_FieldDic[fieldName];
                    string selSQL = GetSQL(fieldName, fieldVale, fieldType, strMatch, out eError);
                    if (eError != null)
                    {
                        pSysDB.CloseDbConnection();
                        return;
                    }
                    string str1 = "";
                    if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString())
                    {
                        //如果是数据库节点没有任何限制条件
                        str1 = "select * from " + pTableName + " where " + selSQL;
                    }
                    else
                    {
                        //如果是其他的节点则肯定 有限制条件
                        if (restrainStr != "")
                        {
                            //查询到数据
                            str1 = "select * from " + pTableName + " where " + selSQL + " and ID in (" + restrainStr + ")";

                        }
                        else
                        {
                            //查不到数据
                            m_Hook.DataInfoGrid.Update();
                            m_Hook.DataInfoGrid.Refresh();
                            return;
                        }
                    }
                    DataTable pTable = pSysDB.GetSQLTable(str1, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "获取源数据信息表失败！");
                        return;
                    }
                    //DataRow[] pRowsArr = mTable.Select(selSQL);

                    showResault(pSysDB, pTable, resalutDT, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eError.Message);
                        pSysDB.CloseDbConnection();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
            }
            pSysDB.CloseDbConnection();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 查询表格
        /// </summary>
        /// <param name="pSysDB"></param>
        /// <param name="pRowsArr"></param>
        /// <param name="resaultTable"></param>
        /// <param name="eError"></param>
        private void showResault(SysCommon.DataBase.SysTable pSysDB, DataTable pTable, DataTable resaultTable, out Exception eError)
        {
            eError = null;
            bool b = false;
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string dataID = "";
                string projectName = "";
                string productName = "";
                string DataName = "";
                string RangeNo = "";
                string pSacle = "";
                string pPath = "";
                string pTime = "";
                string productType="";
                string projectID = "";
                string productID = "";

                dataID = pTable.Rows[i]["ID"].ToString().Trim();
                for(int j=0;j<m_Hook.DataInfoGrid.Rows.Count;j++)
                {
                    if (m_Hook.DataInfoGrid.Rows[j].Cells[0].FormattedValue.ToString().Trim() == dataID)
                    {
                        b = true;
                        break;
                    }
                }
                if(b) 
                {
                    continue;
                }
                DataName = pTable.Rows[i]["数据文件名"].ToString().Trim();
                productID = pTable.Rows[i]["产品ID"].ToString().Trim();
                pPath = pTable.Rows[i]["存储位置"].ToString().Trim();
                pTime = pTable.Rows[i]["生产日期"].ToString().Trim();
                if (cmbType.Text == "标准图幅")
                {
                    RangeNo = pTable.Rows[i]["图幅号"].ToString().Trim();
                    productType = "标准图幅数据";
                }
                else if (cmbType.Text == "非标准图幅")
                {
                    RangeNo = pTable.Rows[i]["块图号"].ToString().Trim();
                    productType = "非标准图幅数据";
                }
                else if (cmbType.Text == "控制点数据")
                {
                    productType = "控制点数据";
                }
                //查询产品信息表
                string str = "select * from ProductMDTable where ID=" + long.Parse(productID);
                DataTable productTable = pSysDB.GetSQLTable(str, out eError);
                if(eError!=null)
                {
                    eError = new Exception("查询产品信息表出错！");
                    return;
                }
                if(productTable.Rows.Count==0) 
                {
                    eError = new Exception("查询产品信息表出错！");
                    return;
                }
                productName = productTable.Rows[0]["产品名称"].ToString().Trim();
                projectID = productTable.Rows[0]["项目ID"].ToString().Trim();
                pSacle = productTable.Rows[0]["比例尺分母"].ToString().Trim();
                //查询项目信息表
                str = "select * from ProjectMDTable where ID=" + long.Parse(projectID);
                DataTable projectTable = pSysDB.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    eError = new Exception("查询项目信息表出错！");
                    return;
                }
                if (productTable.Rows.Count == 0)
                {
                    eError = new Exception("查询项目信息表出错！");
                    return;
                }
                projectName = projectTable.Rows[0]["项目名称"].ToString().Trim();

                //添加行
                DataRow newRow = resaultTable.NewRow();
                newRow[0]=dataID;
                newRow[1]=projectName;
                newRow[2] = productName;
                newRow[3] = DataName;
                newRow[4] = RangeNo;
                newRow[5] = pSacle;
                newRow[6] = pPath;
                newRow[7] = pTime;
                newRow[8] = productType;
                newRow[9] =projectID;
                newRow[10] =productID;
                resaultTable.Rows.Add(newRow);

                //刷新表格
                m_Hook.DataInfoGrid.Update();
                m_Hook.DataInfoGrid.Refresh();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult=DialogResult.Cancel;
            this.Close();
        }

        private string GetSQL(string fieldName,string fieldValue,Type fieldType,string strMatch,out Exception eError)
        {
            eError = null;
            try
            {
                if (fieldType.FullName != "System.String")
                {
                    //fieldName = "CStr(" + fieldName + ")";//////Access数据库方式
                    fieldName = "LOWER(" + fieldName + ")";//////Oracle数据库方式
                }
                //在数据库中access:?；*;SQL:%,*；在程序后者能够都用：%,*
                string pSQL = "";
                switch (strMatch)
                {
                    case "完全匹配":
                        pSQL = fieldName + " like '" + fieldValue + "'";
                        break;
                    case "首字母匹配":
                        pSQL = fieldName + " like '" + fieldValue +"%'";
                        break;
                    case "任意匹配":
                        pSQL = fieldName + " like '%" + fieldValue + "%'";
                        break;
                    default:
                        break;
                }
                return pSQL;
            }
            catch (System.Exception ex)
            {
                eError = ex;
                return "";
            }
        }

        /// <summary>
        /// 获得节点的限制条件字符串
        /// </summary>
        /// <returns></returns>
        private string GetWhereStr()
        {
            DevComponents.AdvTree.Node v_ProNode = null;
            string whereStr = "";
            string treeNodeType =m_Hook.ProjectTree.SelectedNode.DataKey.ToString();
            if (treeNodeType == EnumTreeNodeType.PROJECT.ToString())
            {
                //根据项目合查询
                v_ProNode = m_Hook.ProjectTree.SelectedNode;
                if (v_ProNode.Tag == null) return "";
                whereStr = "项目ID=" + long.Parse(v_ProNode.Tag.ToString());
            }
            else if (treeNodeType == EnumTreeNodeType.DATAFORMAT.ToString())
            {
                //根据项目、数据类型（DLG\DEM\DOM\DRG）和时间来组合查询
                v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null || v_ProNode.Tag == null) return "";
                long dataFormatID = long.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());   //数据格式ID
                whereStr = "项目ID=" + long.Parse(v_ProNode.Tag.ToString()) + " and 数据格式编号=" + dataFormatID;
            }
            else if (treeNodeType == EnumTreeNodeType.PRODUCT.ToString())
            { //根据产品和时间来组合查询
                v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent.Parent;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return "";
                long productID = long.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());  //产品ID
                whereStr = "产品ID=" + productID;

            }
            else if (treeNodeType == EnumTreeNodeType.PRODUCTPYPE.ToString())
            {  //根据产品、产品类型（标准图幅、非标准图幅、属性表）和时间来组合查询

                v_ProNode = m_Hook.ProjectTree.SelectedNode.Parent.Parent.Parent;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null || m_Hook.ProjectTree.SelectedNode.Parent.Tag == null) return "";
                long prodID = long.Parse(m_Hook.ProjectTree.SelectedNode.Parent.Tag.ToString());//产品ID
                long dataTypeID = long.Parse(m_Hook.ProjectTree.SelectedNode.Tag.ToString());  //产品类型id
                whereStr = "产品ID=" + prodID + " and 数据类型编号=" + dataTypeID;
              
            }
            return whereStr;
        }
    }
}