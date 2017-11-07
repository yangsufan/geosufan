using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.OleDb;
using System.Data.OracleClient;

namespace FileDBTool
{
    /// <summary>
    /// 批量数据入库
    /// </summary>
    public class ControlsInputBatchData : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsInputBatchData()
        {
            base._Name = "FileDBTool.ControlsInputBatchData";
            base._Caption = "批量数据入库";
            base._Tooltip = "批量数据入库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "批量数据入库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //若没有选中产品节点的子节点（成果数据类型节点），则不可用


                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.PRODUCTPYPE.ToString()) return false;

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
            //执行批量数据入库操作（包括数据入库、元数据入库、在成果索引表中增加相应记录）。需记录入库时间，以便管理历史


            //=================================================================================================================
            //陈亚飞添加


            string error = "";
            Exception eError = null;

            string conStr = "";               //元数据库连接信息
            string severStr = "";             //ip地址
            string user = "";                 //用户
            string Password = "";             //密码
            string desPath = "";              //存储路径
            //DateTime saveTime = DateTime.Now; //存储时间
            int dataTypeID;                   //数据类型编号
            long productID;                   //产品ID
            long projectID;                   //项目ID
            long productScale = 0;              //比例尺


            string rangeNO = "";                   //产品图幅号


            int dataFormatID;                 //数据格式ID
            long dataID = 0;                      //数据ID


            //获得数据库根节点
            DevComponents.AdvTree.Node DBNode = m_Hook.ProjectTree.SelectedNode;
            while (DBNode.Parent != null)
            {
                DBNode = DBNode.Parent;
            }
            if (DBNode == null)
            {
                return;
            }
            //获得产品类型节点
            DevComponents.AdvTree.Node productTypeNode = null;
            productTypeNode = m_Hook.ProjectTree.SelectedNode;
            while (productTypeNode.DataKey.ToString() != EnumTreeNodeType.PRODUCTPYPE.ToString())
            {
                productTypeNode = productTypeNode.Parent;
            }
            if (productTypeNode == null) return;
            dataTypeID = int.Parse(productTypeNode.Tag.ToString());
            productID = long.Parse(productTypeNode.Parent.Tag.ToString());
            string productName = productTypeNode.Parent.Text.Trim();//产品名称
            int index1 = productName.LastIndexOf('_');
            if (index1 == -1) return;
            string scaleStr = productName.Substring(index1 + 1);
            productScale = long.Parse(scaleStr);


            //获得元数据库的连接字符串和ftp连接地址
            if (DBNode.Tag == null) return;
            XmlElement conElem = DBNode.Tag as XmlElement;
            conStr = conElem.GetAttribute("MetaDBConn");
            severStr = conElem.GetAttribute("服务器");
            user = conElem.GetAttribute("用户");
            Password = conElem.GetAttribute("密码");
            desPath = m_Hook.ProjectTree.SelectedNode.Name;


            //连接数据库
            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            //pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + conStr + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
           //****************************************************************************************************************************
            //guozheng 2010-10-12 改为Oracle连接方式
            pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            //***************************************************************************************************************************
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！\n连接地址为：" + conStr);
                pSysTable.CloseDbConnection();
                return;
            }

            string mStr = "select * from ProductMDTable where ID=" + productID;
            DataTable mTable = pSysTable.GetSQLTable(mStr, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询产品信息表出错！");
                pSysTable.CloseDbConnection();
                return;
            }
            if (mTable.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "找不到产品ID为：'" + productID + "'的产品");
                pSysTable.CloseDbConnection();
                return;
            }
            projectID = long.Parse(mTable.Rows[0]["项目ID"].ToString());
            dataFormatID = int.Parse(mTable.Rows[0]["数据格式编号"].ToString());


            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            if (fbDialog.ShowDialog() == DialogResult.OK)
            {
                string path = fbDialog.SelectedPath;
                string[] fileNames = Directory.GetFiles(path);  //获得目录下所有文件


                List<string> dataFile = new List<string>();     //数据文件
                List<string> metaFile = new List<string>();     //元数据文件


                DataTable metaTable = null;                     //元信息表格



                #region 遍历文件，将数据文件保存起来
                for (int i = 0; i < fileNames.Length; i++)
                {
                    int index = fileNames[i].LastIndexOf('.');
                    if (fileNames[i].Substring(index) == ".mdb")
                    {
                        //元数据文件


                        if (!metaFile.Contains(fileNames[i]))
                        {
                            metaFile.Add(fileNames[i]);
                        }
                    }
                    else if (fileNames[i].Substring(index).ToUpper() == ".DWG" || fileNames[i].Substring(index).ToUpper() == ".SHP" || fileNames[i].Substring(index).ToUpper() == ".IMG" || fileNames[i].Substring(index).ToUpper() == ".TIF")
                    {
                        //数据文件
                        if (!dataFile.Contains(fileNames[i]))
                        {
                            dataFile.Add(fileNames[i]);
                        }
                    }

                }
                #endregion

                #region 清空错误日志信息
                string desErrPath=Application.StartupPath+"\\FileErrLog.mdb";
                if(File.Exists(desErrPath))
                {
                    File.Delete(desErrPath);
                }
                #endregion

                //边路产品元信息，获取比例尺信息
                DataTable productDT=pSysTable.GetTable("ProductMDTable", out eError);


                if (dataTypeID != EnumDataType.控制点数据.GetHashCode())
                {
                    #region 插入元信息表的字段组合
                    long lNewDataID = -1;
                    string insertstrn = "";
                    if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                    {
                       
                        #region 组合标准图幅字段名

                        insertstrn = "insert into StandardMapMDTable (";
                        DataTable stdTable = pSysTable.GetTable("StandardMapMDTable", out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysTable.CloseDbConnection();
                        }
                        for (int i = 0; i < stdTable.Columns.Count; i++)
                        {
                            insertstrn += stdTable.Columns[i].ColumnName + ",";
                        }
                        insertstrn = insertstrn.Substring(0, insertstrn.Length - 1) + ")";
                        #endregion

                    }
                    else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                    {
                        
                        #region 组合非标准图幅字段名
                        insertstrn = "insert into NonstandardMapMDTable (";
                        DataTable stdTable = pSysTable.GetTable("NonstandardMapMDTable", out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            pSysTable.CloseDbConnection();
                        }
                        for (int i = 0; i < stdTable.Columns.Count; i++)
                        {
                            insertstrn += stdTable.Columns[i].ColumnName + ",";
                        }
                        insertstrn = insertstrn.Substring(0, insertstrn.Length - 1) + ")";

                        #endregion
                    }
                    #endregion

                    //针对标准图幅和非标准图幅数据
                    #region 遍历数据文件 ,进行数据和元信息的入库

                    foreach (string aDataf in dataFile)
                    {
                        string pDataFName = aDataf;                     //文件全名（带路径）


                        FileInfo Finfo = new FileInfo(pDataFName);
                        string fname = Finfo.Name;                      //文件名称
                        string fPath = Finfo.DirectoryName;             //文件路径
                        int index = fname.LastIndexOf('.');
                        string pureFName = fname.Substring(0, index);    //不带扩展名的文件名

                        string insertstr = "";                          //插入SQL
                        string metaFName = pureFName + ".mdb";          //对应的元数据文件名

                        //******************************************************************
                        //guozheng 2010-10-12
                        //////////////////////////获取新增数据的ID//////////////////////////
                        if (dataTypeID == EnumDataType.标准图幅.GetHashCode())
                        {
                            lNewDataID = GetNewTableID("StandardMapMDTABLE", pSysTable, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                return;
                            }
                        }
                        else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                        {
                            lNewDataID = GetNewTableID("NonstandardMapMDTABLE", pSysTable, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                return;
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////
                        //***************************************************************************

                        if (metaFile.Contains(fPath + "\\" + metaFName))
                        {
                            //数据有元信息

                            #region 从元信息文件中读取元信息
                            SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                            try
                            {
                                metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fPath + "\\" + metaFName + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                                if (eError != null)
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元信息文件失败！\n连接地址为：" + conStr);
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                                metaTable = metaSysTable.GetTable("metadata", out eError);
                                if (eError != null)
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message + "在库中不存在表格'metadata'，请检查！");
                                    metaSysTable.CloseDbConnection();
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                                if (metaTable.Rows.Count == 0)
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "元信息文件为空，请检查！");
                                    metaSysTable.CloseDbConnection();
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                                metaSysTable.CloseDbConnection();
                            }
                            catch (Exception ex)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                                metaSysTable.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            #endregion

                            string orgScale = metaTable.Rows[0][1].ToString().Trim();//源数据的元信息比例尺
                            if(orgScale!=productScale.ToString().Trim())
                            {
                                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "数据'"+pureFName+"'的比例尺与产品的比例尺信\n息不一致,是否继续导入？"))
                                {
                                    //将出错信息导入
                                    if (!File.Exists(desErrPath))
                                    {
                                        File.Copy(ModData.v_tempErrLog, desErrPath);
                                    }
                                    //连接错误表格
                                    SysCommon.DataBase.SysTable pErrTable = new SysCommon.DataBase.SysTable();
                                    pErrTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + desErrPath + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                                    if (eError != null)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接错误日志表失败！\n连接地址为：" + desErrPath);
                                        pSysTable.CloseDbConnection();
                                        return;
                                    }
                                    string errDes="数据比例尺与产品比例尺信息不一致";
                                    string inserStr = "insert into Errorlog (FileName,ErrDes) values ('" + pureFName + "','" + errDes + "')";
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

                            # region 插入元信息

                            insertstr = insertstrn + " values (" +lNewDataID.ToString()+","+
                        productID + "," + dataFormatID + ",'" + fname + "','" + desPath + "'," + productScale;
                            try
                            {

                                for (int j = 2; j < metaTable.Columns.Count; j++)
                                {
                                    Type columnType = metaTable.Columns[j].DataType;
                                    string columnValue = metaTable.Rows[0][j].ToString();
                                    if (j == 2 && columnValue == "")
                                    {
                                        //图幅号不能为空
                                        columnValue = pureFName;
                                    }
                                    string str = ModDBOperator.GetSQl(columnType, columnValue);
                                    insertstr += "," + str;
                                }
                                insertstr += ")";
                                pSysTable.StartTransaction();      //开启事物


                                //向数据元数据表中插入新的数据元信息
                                #region 若文件库中已存在同名文件，试图删除此文件的元信息记录
                                ////若文件库中已存在同名文件，试图删除此文件的元信息记录
                                if (ModDBOperator.IsFTPExistFile(severStr, user, Password, desPath, fname, out eError))
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
                                        SQL = "SELECT ID FROM " + Tablename + " WHERE 数据文件名='" + fname + "'" + " AND  存储位置='" + desPath + "'";
                                        DataTable table = pSysTable.GetSQLTable(SQL, out eError);
                                        if (null != table)
                                        {
                                            DataID = Convert.ToInt64(table.Rows[0][0].ToString());
                                        }
                                        SQL = "DELETE FROM " + Tablename + " WHERE 数据文件名='" + fname + "'" + " AND  存储位置='" + desPath + "'";
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
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入数据元信息出错！\n原因："+eError.Message);
                                    pSysTable.EndTransaction(false);
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
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
                            //else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                            //{
                            //    mStr = "select * from ControlPointMDTable order by ID desc";
                            //}
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
                            DateTime fromDate = Convert.ToDateTime(mTable.Rows[0]["生产日期"].ToString());
                            insertstr += projectID + "," + productID + "," + productScale + "," +
                                dataFormatID + "," + dataTypeID + "," + dataID + ",'" + rangeNO + "',to_date('" + fromDate.ToShortDateString() + "','yyyy-mm-dd'),'" + desPath + "')";
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

                            #region 上传元信息文件

                            ////////////////判断文件库上是否有重名文件///////
                            //if (ModDBOperator.IsFTPExistFile(severStr, user, Password, desPath, metaFName, out eError))
                            //{
                            //     ModDBOperator.DelNodeByNameAndText(m_Hook.ProjectTree.SelectedNode, fPath, metaFName, out eError);
                            //}
                            if (!ModDBOperator.UpLoadFile(severStr, user, Password, fPath, metaFName, desPath, metaFName, out error))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", error);
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            #endregion

                            pSysTable.EndTransaction(true);
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

                                    insertstr += "产品ID,数据文件名,数据格式编号,存储位置,图幅比例尺,图幅号)";
                                    insertstr += " values (" +
                                     productID + ",'" + fname + "'," + dataFormatID + ",'" + desPath + "',"+productScale +",'"+ pureFName + "')";

                                }
                                else if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                                {
                                    insertstr = "insert into NonstandardMapMDTable (";

                                    insertstr += "产品ID,数据文件名,数据格式编号,存储位置,图幅比例尺,块图号)";

                                    insertstr = " values (" +
                                     productID + ",'" + fname + "'," + dataFormatID + ",'" + desPath + "'," + productScale + ",'" + pureFName + "')";
                                }

                                pSysTable.StartTransaction();      //开启事物


                                //向数据元数据表中插入新的数据元信息


                                pSysTable.UpdateTable(insertstr, out eError);
                                if (eError != null)
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "插入数据元信息出错！");
                                    pSysTable.EndTransaction(false);
                                    pSysTable.CloseDbConnection();
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
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
                            //else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                            //{
                            //    mStr = "select * from ControlPointMDTable order by ID desc";
                            //}
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
                            DateTime fromDate = DateTime.Now;
                            //insertstr += projectID + "," + productID + "," + productScale + "," +
                            //    dataFormatID + "," + dataTypeID + "," + dataID + ",'" + rangeNO + "',#" + fromDate + "#,'" + desPath + "')";
                            insertstr += projectID + "," + productID + "," + productScale + "," +
                               dataFormatID + "," + dataTypeID + "," + dataID + ",'" + rangeNO + "',to_date('" + fromDate.ToShortDateString() +"','yyyy-mm-dd')"+ ",'" + desPath + "')";
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

                            pSysTable.EndTransaction(true);
                        }

                        #region 上传数据文件
                        for (int i = 0; i < fileNames.Length; i++)
                        {
                            string fItem = fileNames[i];
                            FileInfo tFInfo = new FileInfo(fItem);
                            string ttName = tFInfo.Name;
                            if (ttName.Contains(pureFName))
                            {
                                if (!ModDBOperator.UpLoadFile(severStr, user, Password, fPath, ttName, desPath, ttName, out error))
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", error);
                                    pSysTable.CloseDbConnection();
                                }
                            }
                        }
                        #endregion

                        #region 非标准图幅数据试图入库图幅结合表
                        if (dataTypeID == EnumDataType.非标准图幅.GetHashCode())
                        {
                            string RangeMDBFile = fPath + "\\" + pureFName + "_Range.mdb";
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
                                    ModDBOperator.CreateMapRangeByMDB(RangeMDBFile, Mappath, FeatureClassName, out eError);
                                    if (null != eError)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误提示", "文件：" + RangeMDBFile + "中获取非标准图幅范围信息失败，\n确认文件中数据格式是否正确.");
                                    }
                                }
                            }
                        }

                        #endregion
                        //将新插入的信息挂在树节点上

                        ModDBOperator.DelNodeByNameAndText(m_Hook.ProjectTree.SelectedNode, desPath, fname, out eError);
                        ModDBOperator.AppendNode(m_Hook.ProjectTree.SelectedNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), fname, dataID, desPath, 4);

                    }
                    #endregion
                }
                else if (dataTypeID == EnumDataType.控制点数据.GetHashCode())
                {
                    FrmProcessBar frmbar = new FrmProcessBar();
                    frmbar.Show();

                    //针对控制点控制点数据


                    #region 组合控制点数据的字段名

                    DataTable stdTable = pSysTable.GetTable("ControlPointMDTable", out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        pSysTable.EndTransaction(false);
                        pSysTable.CloseDbConnection();
                    }

                    string insertstr2 = "insert into ControlPointMDTable (ID,";
                    for (int i = 1; i < stdTable.Columns.Count; i++)
                    {
                        insertstr2 += stdTable.Columns[i].ColumnName + ",";
                    }
                    insertstr2 = insertstr2.Substring(0, insertstr2.Length - 1) + ")";
                    insertstr2 += " values (";

                    //插入成果索引表的字字段名
                    string insertstr1 = "insert into ProductIndexTable(项目ID,产品ID,比例尺分母,数据格式编号,数据类型编号,数据ID,生产日期,存储位置) values(";
                    
                    #endregion

                    #region 遍历多个控制点数据库
                    foreach (string metaScpFile in metaFile)
                    {
                        string pMetaFName = metaScpFile;                                           //控制点数据全名（带路径）
                        FileInfo Finfo = new FileInfo(pMetaFName);
                        string metaFName = Finfo.Name;                                             //控制点数据名称


                        string fPath = Finfo.DirectoryName;                                        //文件路径
                        string pureFName = metaFName.Substring(0, metaFName.LastIndexOf('.'));     //不带扩展名的控制点数据名
                        //string fname = pureFName + ".dwg";                                       //对应的数据文件名

                        string insertstr = "";                                                     //插入SQL


                        #region 从元信息文件中读取元信息
                        SysCommon.DataBase.SysTable metaSysTable = new SysCommon.DataBase.SysTable();
                        try
                        {
                            metaSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pMetaFName + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元信息文件失败！\n连接地址为：" + conStr);
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            metaTable = metaSysTable.GetTable("metadata", out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message + "在库中不存在表格'metadata'，请检查！");
                                metaSysTable.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            if (metaTable.Rows.Count == 0)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "元信息文件为空，请检查！");
                                metaSysTable.CloseDbConnection();
                                pSysTable.CloseDbConnection();
                                return;
                            }
                            metaSysTable.CloseDbConnection();
                        }
                        catch (Exception eX)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eX.Message);
                            metaSysTable.CloseDbConnection();
                            pSysTable.CloseDbConnection();
                            return;
                        }
                        #endregion


                        pSysTable.StartTransaction();      //开启事物


                        #region 遍历属性库，进行控制点数据的入库

                        frmbar.SetFrmProcessBarMax(metaTable.Rows.Count);

                        for (int q = 0; q < metaTable.Rows.Count; q++)
                        {
                            string fname = metaTable.Rows[q][1].ToString().Trim() + ".dwg";                //控制点名
                            string mName = metaTable.Rows[q][1].ToString().Trim();

                            frmbar.SetFrmProcessBarText("正在进行控制点数据'" + mName + "'入库：");
                            byte[] dataBt = null;// new byte[60000];                                     //该字段用过来存储数据文件
                            if (dataFile.Contains(fPath + "\\" + fname))
                            {
                                FileInfo tempFile = new FileInfo(fPath + "\\" + fname);

                                //该控制点数据存在数据文件
                                dataBt = new byte[tempFile.Length];//把图片转化为二进制流,System.Text.Encoding.Default.GetBytes()
                                FileStream fs = new FileStream(fPath + "\\" + fname, FileMode.Open, FileAccess.Read);
                                fs.Read(dataBt, 0, dataBt.Length);
                            }
                            //desPath = "";

                            # region 插入元信息
                            //**************************************************************
                            //guozheng added 
                            long lNewID = -1;////////////////////插入控制点表适用的ID
                            Exception ex = null;
                            lNewID = GetNewTableID("ControlPointMDTable", pSysTable, out ex);
                            if (null != ex)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取控制点表ID失败，\n原因：" + ex.Message);
                                frmbar.Close();
                                return;
                            }
                            string sSqlControlPoint=insertstr2 + lNewID.ToString() + ",";
                            //***************************************************************

                            try
                            {
                                insertstr = sSqlControlPoint + productID + ",'" + mName + "'," + dataFormatID + ",'" + desPath + "'";


                                for (int j = 1; j < metaTable.Columns.Count; j++)
                                {
                                    Type columnType = metaTable.Columns[j].DataType;
                                    string columnValue = metaTable.Rows[q][j].ToString();

                                    string str = ModDBOperator.GetSQl(columnType, columnValue);
                                    insertstr += "," + str;
                                }
                                //
                                if (dataBt != null)
                                {
                                    insertstr += ",:dataF)";
                                }
                                else
                                {
                                    insertstr += ",null)";
                                }

                                //向数据元数据表中插入新的数据元信息
                                //判断信息库中是否已存在同名的属性信息，存在进行删除
                                #region 判断信息库中是否已存在同名的属性信息，存在进行删除
                                string Condition = "存储位置=" + "'" + desPath + "'";
                                if (ModDBOperator.IsTableFildExist("ControlPointMDTable", pSysTable, "控制点名", mName, Condition, out eError))
                                {
                                    string SQL = "";
                                    try
                                    {
                                        long DataID = -1;
                                        SQL = "SELECT ID FROM ControlPointMDTable WHERE 控制点名='" + mName + "'" + " AND  存储位置='" + desPath + "'";
                                        DataTable table = pSysTable.GetSQLTable(SQL, out eError);
                                        if (null == eError && null != table)
                                        {
                                            DataID = Convert.ToInt64(table.Rows[0][0].ToString());
                                        }
                                        SQL = "DELETE FROM ControlPointMDTable WHERE 控制点名='" + mName + "'" + " AND  存储位置='" + desPath + "'";
                                        pSysTable.UpdateTable(SQL, out eError);
                                        if (null != eError)
                                            pSysTable.EndTransaction(false);
                                        if (DataID != -1)
                                        {
                                            SQL = "DELETE FROM ProductIndexTable WHERE  存储位置='" + desPath + "'" + " AND  数据ID=" + DataID + " AND 数据类型编号=2";
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
                            }
                            catch (Exception eXX)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eXX.Message);
                                pSysTable.EndTransaction(false);
                                pSysTable.CloseDbConnection();
                                frmbar.Dispose();
                                frmbar.Close();
                                return;
                            }
                            #endregion

                            #region 插入成果索引表



                            mStr = "select * from ControlPointMDTable order by ID desc";
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

                            DateTime fromDate = Convert.ToDateTime(mTable.Rows[0]["生产日期"].ToString());
                            //insertstr = insertstr1 + projectID + "," + productID + "," +productScale+","+
                            //    dataFormatID + "," + dataTypeID + "," + dataID + ",#" + fromDate + "#,'" + desPath + "')";
                            insertstr = insertstr1 + projectID + "," + productID + "," + productScale + "," +
                               dataFormatID + "," + dataTypeID + "," + dataID + ",to_date('" + fromDate.ToShortDateString() +"','yyyy-mm-dd')"+ ",'" + desPath + "')";

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

                            //将新插入的信息挂在树节点上

                            ModDBOperator.DelNodeByNameAndText(m_Hook.ProjectTree.SelectedNode, desPath, mName, out eError);
                            ModDBOperator.AppendNode(m_Hook.ProjectTree.SelectedNode.Nodes, EnumTreeNodeType.DATAITEM.ToString(), mName, dataID, desPath, 4);
                            frmbar.SetFrmProcessBarValue(q + 1);
                        }
                        #endregion

                        pSysTable.EndTransaction(true);
                    }

                    frmbar.Dispose();
                    frmbar.Close();
                    #endregion
                }

                //刷新时间列表框


                ModDBOperator.LoadComboxTime(conStr, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "加载时间列表框失败，" + eError.Message);
                    pSysTable.CloseDbConnection();
                    return;
                }
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "导入成功！");
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }

        /// <summary>
        /// 在插入项目表的时候，获取一个可用的最大ID
        /// </summary>
        /// <param name="ex">输出：错误信息</param>
        /// <returns></returns>
        private long GetNewTableID(string sTableName,SysCommon.DataBase.SysTable pSysTable, out Exception ex)
        {
            ex = null;
            long lid = -1;
            if (pSysTable == null) { ex = new Exception("元信息库连接信息未初始化"); return -1; }
            //////////从数据库中中查询ID最大值加1返回，没有记录返回1///////
            string sql = "SELECT COUNT(*) FROM " + sTableName;
            DataTable gettable = pSysTable.GetSQLTable(sql, out ex);
            if (ex != null) return -1;
            try
            {
                int count = Convert.ToInt32(gettable.Rows[0][0].ToString());
                if (count == 0) return 1;
                else
                {
                    sql = "SELECT MAX(ID) FROM " + sTableName;
                    gettable = pSysTable.GetSQLTable(sql, out ex);
                    if (ex != null) return -1;
                    lid = Convert.ToInt32(gettable.Rows[0][0].ToString());
                    lid += 1;
                    return lid;
                }
            }
            catch (Exception eError)
            {
                ex = eError;
                return -1;
            }
        }
    }
}
