using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SysCommon;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBIntegration
{
    /*
     * guozheng  2010-9-28 added
     * 该类实现刷新一个数据库的界面信息，
     * 通过传入的数据库信息在系统维护库中检索，将数据库的信息挂接到界面上
     */
    class clsRefurbishDBinfo
    {
        private SysCommon.DataBase.SysTable m_DBOper;/////系统维护库连接信息
        public SysCommon.DataBase.SysTable DBOper
        {
            get { return this.m_DBOper; }
            set { this.m_DBOper = value; }
        }

        #region 构造函数
        public clsRefurbishDBinfo()
        {
            this.m_DBOper = null;
        }
        public clsRefurbishDBinfo(SysCommon.DataBase.SysTable pTable)
        {
            this.m_DBOper = pTable;
        }
        public clsRefurbishDBinfo(SysCommon.enumDBConType DBConType, SysCommon.enumDBType DBType, string sConInfo)
        {
            Exception ex = null;
            this.m_DBOper = new SysCommon.DataBase.SysTable();
            this.m_DBOper.SetDbConnection(sConInfo, DBConType, DBType, out ex);
            if (ex != null) this.m_DBOper = null;
        }
        #endregion
        /// <summary>
        /// cyf 20110602 modify:刷新界面函数（获取信息）
        /// </summary>
        /// <param name="lDBID">刷新的数据库ID（为-1则刷新所有数据库信息）</param>
        /// <param name="ProjectNode">输出：刷新后的数据库工程树节点</param>
        /// <param name="ex">输出：错误信息</param>
        public void RefurbishDBinfo(long lDBID,out DevComponents.AdvTree.Node ProjectNode, out Exception ex)
        {
            ex = null;
            ProjectNode = null;
            //cyf 20110602
            //if (this.m_DBOper == null) { ex = new Exception("系统维护库连接信息未初始化"); return; }
            IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWS == null) { ex = new Exception("系统维护库连接失败！"); return; }
            //end
            string sql = string.Empty;
            //cyf 20110602 
            IQueryDef pQueryDef = pFeaWS.CreateQueryDef();
            pQueryDef.Tables = "DATABASEMD";
            pQueryDef.SubFields = "*";
            if (lDBID != -1)
            {
                pQueryDef.WhereClause = " ID=" + lDBID.ToString();
                //sql = "SELECT * FROM DATABASEMD WHERE ID=" + lDBID.ToString();
            }
            else
            {
                //sql = "SELECT * FROM DATABASEMD";
                ///////////如果是刷新所有的信息，删除工程树中的数据库节点,所有的Combox选择为空///////////
                try
                {
                    for (int i = 0; i < ModuleData.v_AppDBIntegra.ProjectTree.Nodes[0].Nodes.Count; i++)
                    {
                        ModuleData.v_AppDBIntegra.ProjectTree.Nodes[0].Nodes[i].Nodes.Clear();
                    }
                    ModuleData.v_AppDBIntegra.cmbAdressDB.DataSource = null;
                    ModuleData.v_AppDBIntegra.cmbDemDB.DataSource = null;
                    ModuleData.v_AppDBIntegra.cmbEntiDB.DataSource = null;
                    ModuleData.v_AppDBIntegra.cmbFeatureDB.DataSource = null;
                    ModuleData.v_AppDBIntegra.cmbFileDB.DataSource = null;
                    ModuleData.v_AppDBIntegra.cmbImageDB.DataSource = null;
                    ModuleData.v_AppDBIntegra.cmbImageDB.DataSource = null;
                }
                catch
                {
                }
            }
            //查询表格
            ICursor pCursor = null;
            try { pCursor = pQueryDef.Evaluate(); } catch { ex = new Exception("读取系统维护库信息失败"); return; }
            if (pCursor == null) { ex = new Exception("读取系统维护库信息失败"); return; }
            IRow pRow = pCursor.NextRow();
            //遍历每一条记录
            while (pRow != null)
            {
                long lDbID = -1;
                string sDbName = string.Empty;////数据库名称
                long lDbTypeID = -1;////数据库类型ID
                long lDbFormateID = -1;////数据库平台ID
                long lDbStateID = -1;////数据库状态ID
                string sConnectInfo = string.Empty;////数据库连接信息
                //cyf 20110623 modify 
                //long lscale = -1;////比例尺
                string lscale = "";
                //end
                string pParaDB = "";
                int idIndex=-1;          //ID字段索引
                int dbTypeIdIndex=-1;    //数据库类型ID索引
                int dbFormatIdIndex = -1;//数据格式ID索引
                int dbStateIdIndex = -1; //数据库状态ID索引
                int connInfoIndex = -1;  //连接信息ID索引
                int scaleIndex = -1;     //比例尺字段索引
                int dbNameIndex = -1;    //数据库名称ID索引
                int dbParaIndex = -1;         //参数索引
                //获取字段索引
                idIndex = pRow.Fields.FindField("ID");
                dbTypeIdIndex = pRow.Fields.FindField("DATABASETYPEID");
                dbFormatIdIndex = pRow.Fields.FindField("DATAFORMATID");
                dbStateIdIndex = pRow.Fields.FindField("DATABASSTATEID");
                connInfoIndex = pRow.Fields.FindField("CONNECTIONINFO");
                scaleIndex = pRow.Fields.FindField("SCALE");
                dbNameIndex = pRow.Fields.FindField("DATABASENAME");
                dbParaIndex = pRow.Fields.FindField("DBPARA");
                if (idIndex == -1 || dbTypeIdIndex == -1 || dbFormatIdIndex == -1 || dbStateIdIndex == -1 || connInfoIndex == -1 || scaleIndex == -1 || dbNameIndex == -1 || dbParaIndex == -1)
                {
                    ex = new Exception("获取信息失败！"); return; 
                }
                //////////获取必要信息//////////////
                try { lDbID = Convert.ToInt64(pRow.get_Value(idIndex).ToString()); } catch { return; }
                try { lDbTypeID = Convert.ToInt64(pRow.get_Value(dbTypeIdIndex).ToString());} catch { lDbTypeID = -1; }
                try { lDbFormateID = Convert.ToInt64(pRow.get_Value(dbFormatIdIndex).ToString()); } catch { lDbFormateID = -1; }
                try { lDbStateID = Convert.ToInt64(pRow.get_Value(dbStateIdIndex).ToString()); } catch { lDbStateID = -1; }
                try { sConnectInfo = pRow.get_Value(connInfoIndex).ToString();} catch { sConnectInfo = ""; }
                //cyf 20110623 modify 
                //try { lscale = Convert.ToInt64(pRow.get_Value(scaleIndex).ToString());} catch { lscale = -1; }
                try { lscale = pRow.get_Value(scaleIndex).ToString(); }
                catch { lscale = ""; }
                //end
                sDbName =pRow.get_Value(dbNameIndex).ToString();
                pParaDB = pRow.get_Value(dbParaIndex).ToString();

                RefurbishFrm(lDbID, lDbTypeID, lDbFormateID, lDbStateID, sDbName, sConnectInfo, lscale, pParaDB, out ProjectNode, out ex);
                pRow = pCursor.NextRow();
            }
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            //end
            #region 原有代码
            //DataTable gettable = null;
            //gettable = this.m_DBOper.GetSQLTable(sql, out ex);
            //if (ex!=null){ex=new Exception("读取系统维护库信息失败");return;}
            //if (gettable == null) { ex = new Exception("读取系统维护库信息失败"); return; }
            //for (int i = 0; i < gettable.Rows.Count; i ++)
            //{
            //    long lDbID = -1;
            //    string sDbName = string.Empty;////数据库名称
            //    long lDbTypeID = -1;////数据库类型ID
            //    long lDbFormateID = -1;////数据库平台ID
            //    long lDbStateID = -1;////数据库状态ID
            //    string sConnectInfo = string.Empty;////数据库连接信息
            //    long lscale = -1;////比例尺
            //    string pParaDB = "";
            //    //////////获取必要信息//////////////
            //    try  { lDbID = Convert.ToInt64(gettable.Rows[i]["ID"].ToString());   }
            //    catch { return; }
            //    try { lDbTypeID = Convert.ToInt64(gettable.Rows[i]["DATABASETYPEID"].ToString()); }
            //    catch { lDbTypeID = -1; }
            //    try { lDbFormateID = Convert.ToInt64(gettable.Rows[i]["DATAFORMATID"].ToString()); }
            //    catch { lDbFormateID = -1; }
            //    try { lDbStateID = Convert.ToInt64(gettable.Rows[i]["DATABASSTATEID"].ToString()); }
            //    catch { lDbStateID = -1; }
            //    try { sConnectInfo = gettable.Rows[i]["CONNECTIONINFO"].ToString(); }
            //    catch { sConnectInfo = ""; }
            //    try { lscale = Convert.ToInt64(gettable.Rows[i]["SCALE"].ToString()); }
            //    catch {lscale = -1; }
            //    sDbName = gettable.Rows[i]["DATABASENAME"].ToString();
            //    pParaDB = gettable.Rows[i]["DBPARA"].ToString();

            //    RefurbishFrm(lDbID, lDbTypeID, lDbFormateID, lDbStateID, sDbName, sConnectInfo,lscale,pParaDB,out ProjectNode, out ex);
            //}
            #endregion

        }
        /// <summary>
        /// cyf 20110602 modify:刷新界面主函数  ,刷新数据库工程节点，刷新xml配置文件，刷新combobox列表
        /// </summary>
        /// <param name="lDBid">数据库ID</param>
        /// <param name="lDBTypeID">数据库类型ID</param>
        /// <param name="lDBFormatID">数据库平台ID</param>
        /// <param name="DNStateID">数据库状态ID</param>
        /// <param name="sDBName">数据库名称</param>
        /// <param name="sDbConnetInfo">数据库连接信息</param>
        /// <param name="lScale">比例尺 cyf 20110623 比例尺字段类型变成字符串类型</param>
        /// <param name="ProjectNode">输出：刷新后的数据库工程树节点</param>
        /// <param name="ex">输出：错误信息</param>
        private void RefurbishFrm(long lDBid, long lDBTypeID, long lDBFormatID, long DNStateID, string sDBName, string sDbConnetInfo, string lScale, string pParaDB, out DevComponents.AdvTree.Node ProjectNode, out Exception ex)
        {
            ex = null;
            ProjectNode = null;
            /////////获取数据库类型、平台、状态文本//////
            string sDBType = string.Empty;
            string sDBFormat = string.Empty;
            string sDBState = string.Empty;
            string sDbpara = string.Empty;
            string sql = string.Empty;
            IQueryDef pQueryDes = (ModuleData.TempWks as IFeatureWorkspace).CreateQueryDef();

            try
            {
                //cyf 20110602
                ////数据库类型
                pQueryDes.Tables = "DATABASETYPEMD";
                pQueryDes.SubFields = "DATABASETYPE";
                pQueryDes.WhereClause = "ID=" + lDBTypeID.ToString();
                ICursor pCursor = null;
                pCursor = pQueryDes.Evaluate();
                if (pCursor == null) return;
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    sDBType = pRow.get_Value(0).ToString().Trim();
                }

                //sql = "SELECT DATABASETYPE  FROM DATABASETYPEMD WHERE ID=" + lDBTypeID.ToString();
                //DataTable gettable = this.m_DBOper.GetSQLTable(sql, out ex);
                //sDBType = gettable.Rows[0][0].ToString().Trim();
                //end

                //cyf 20110602
                ////数据库平台
                pQueryDes.Tables = "DATABASEFORMATMD";
                pQueryDes.SubFields = "DATABASEFORMAT";
                pQueryDes.WhereClause = "ID=" + lDBFormatID.ToString();
                pCursor = pQueryDes.Evaluate();
                if (pCursor == null) return;
                pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    sDBFormat = pRow.get_Value(0).ToString().Trim();
                }
                //sql = "SELECT DATABASEFORMAT  FROM DATABASEFORMATMD WHERE ID=" + lDBFormatID.ToString();
                //gettable = this.m_DBOper.GetSQLTable(sql, out ex);
                //sDBFormat = gettable.Rows[0][0].ToString().Trim();
                //end

                //cyf 20110602
                ////数据库状态
                pQueryDes.Tables = "DATABASESTATEMD";
                pQueryDes.SubFields = "DATABASESTATE";
                pQueryDes.WhereClause = "ID=" + DNStateID.ToString();
                pCursor = pQueryDes.Evaluate();
                if (pCursor == null) return;
                pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    sDBState = pRow.get_Value(0).ToString().Trim();
                }
                //sql = "SELECT DATABASESTATE  FROM DATABASESTATEMD WHERE ID=" + DNStateID.ToString();
                //gettable = this.m_DBOper.GetSQLTable(sql, out ex);
                //sDBState = gettable.Rows[0][0].ToString().Trim();
                //end

                //cyf 20110602
                ////数据库参数
                pQueryDes.Tables = "DATABASEMD";
                pQueryDes.SubFields = "DBPARA";
                pQueryDes.WhereClause = "ID=" + lDBid.ToString();
                pCursor = pQueryDes.Evaluate();
                if (pCursor == null) return;
                pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    sDbpara = pRow.get_Value(0).ToString().Trim();
                }
                //sql = "SELECT DBPARA FROM DATABASEMD WHERE ID=" + lDBid.ToString();
                //gettable = this.m_DBOper.GetSQLTable(sql, out ex);
                //sDbpara = gettable.Rows[0][0].ToString().Trim();
                //end
            }
            catch
            {
                sDBType = "";
                sDBFormat = "";
                sDBState = "";
                sDbpara = "";
            }
            /////////将信息更新到界面上//////
            DevComponents.AdvTree.Node CtrlDBTreeNode = null;
            CtrlDBTreeNode = getDbTreeNode(sDBType, out ex);
            //cyf 20110614 add
            if (CtrlDBTreeNode == null)
            {
                ex = new Exception("工程树获取节点失败,\n请检查在系统维护库中是否定义该类型的数据库");
                return;
            }
            //end
            //////更新树节点///////
            DevComponents.AdvTree.Node DbNode = null;  //cyf 20110614 数据库工程根节点 modify
            //获取数据库工程根节点
            DevComponents.AdvTree.NodeCollection nodes = CtrlDBTreeNode.Nodes;
            if (nodes == null) return;
            else
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node getNode = nodes[i];
                    if (getNode.Text == sDBName)
                    {
                        DbNode = getNode;
                        break;
                    }
                }
            }

            /////////////////////构建挂接信息//////////////////
            XmlDocument XmlDoc = new XmlDocument();
            XmlNode XmlInfoNode = XmlDoc.CreateNode(XmlNodeType.Element, "数据库信息", null);
            XmlAttribute addAttr = null;
            ////
            addAttr = XmlDoc.CreateAttribute("数据库工程名");
            addAttr.Value = sDBName;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库ID");
            addAttr.Value = lDBid.ToString();
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库连接信息");
            addAttr.Value = sDbConnetInfo;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////cyf 20110628
            addAttr = XmlDoc.CreateAttribute("数据库平台ID");
            addAttr.Value = lDBFormatID.ToString();
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库状态ID");
            addAttr.Value = DNStateID.ToString();
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库类型ID");
            addAttr.Value = lDBTypeID.ToString() ;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            //end
            ////
            addAttr = XmlDoc.CreateAttribute("数据库平台");
            addAttr.Value = sDBFormat;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库状态");
            addAttr.Value = sDBState;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库类型");
            addAttr.Value = sDBType;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("比例尺");
            //cyf 20110623 modify
            //string strScale = "";
            //if (lScale != "")
            //{
            //    strScale = lScale.ToString();
            //}
            addAttr.Value = lScale.ToString();
            //end
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            ////
            addAttr = XmlDoc.CreateAttribute("数据库参数");
            addAttr.Value = sDbpara;
            XmlInfoNode.Attributes.SetNamedItem(addAttr);
            /////////////////////////////////////////////////////
            if (DbNode == null)/////没有则插入新的节点
            {
                DbNode = new DevComponents.AdvTree.Node();
                DbNode.Text = sDBName;////数据库名称
                DbNode.Tag = XmlInfoNode as XmlElement;
                DbNode.DataKey = lDBid;
                DbNode.Image = ModuleData.v_AppDBIntegra.ProjectTree.ImageList.Images[2];  //cyf 20110711 添加图标
                CtrlDBTreeNode.Nodes.Add(DbNode);
            }
            else//////有则更新
            {
                DbNode.Text = sDBName;////数据库名称
                DbNode.Tag = XmlInfoNode as XmlElement;
                DbNode.DataKey = lDBid;
            }
            ProjectNode = DbNode;
            ///////////////////////////刷新combox//////////////
            // UpDataComBox(sDBType, out ex);

            //cyf 20110624 delete:没有必要刷新xml，进入子系统时会自动读取数据库刷新xml;
            //刷新子系统xml  陈亚飞添加 20101009
            //ModDBOperate.RefeshXml(lDBTypeID, lDBid.ToString(), sDBName, lScale, sDBFormat, sDbConnetInfo, pParaDB, out ex);
            //if (ex != null)
            //{
            //    return;
            //}
            //end
            /////////////////刷新按钮////////////////
            //cyf 20110614 modify
            XmlElement DBConnectInfo = null;// GetDBConInfoEle(lDBTypeID, lDBid);   //cyf 20110627 从xml获取连接信息，并将其瓜子啊树节点上
            UpDataButton(lDBid, sDBName,lDBTypeID, lDBFormatID,sDBType, sDBFormat, XmlInfoNode as XmlElement, DBConnectInfo);
        }

     
        /// <summary>
        /// 树图中获取数据库所属的父节点  cyf  20110614 ：modify
        /// </summary>
        /// <param name="sDBType">数据库类型</param>
        /// <param name="ex">输出：错误信息</param>
        /// <returns></returns>
        private DevComponents.AdvTree.Node getDbTreeNode(string sDBType, out Exception ex)
        {
            ex = null;
            string NodeName=string.Empty;
            //cyf 20110614 modify:
            NodeName = sDBType;
            //if (sDBType==enumInterDBType.成果文件数据库.ToString()) NodeName=enumInterDBType.成果文件数据库.ToString();//"添加文件成果数据库";
            //else if (sDBType == enumInterDBType.地理编码数据库.ToString()) NodeName =enumInterDBType.地理编码数据库.ToString();// "添加地理编码数据库";
            //else if (sDBType == enumInterDBType.地名数据库.ToString()) NodeName =enumInterDBType.地名数据库.ToString();// "添加地名数据库";
            //else if (sDBType == enumInterDBType.高程数据库.ToString()) NodeName =enumInterDBType.高程数据库.ToString();// "添加高程数据库";
            //else if (sDBType == enumInterDBType.框架要素数据库.ToString()) NodeName = enumInterDBType.框架要素数据库.ToString();//"添加框架要素数据库";
            //else if (sDBType == enumInterDBType.影像数据库.ToString()) NodeName = enumInterDBType.影像数据库.ToString();//"添加影像数据库";
            //else
            //{
            //    ex = new Exception("不支持的数据库类型");
            //    return null;
            //}
            //end
            ////////在工程树中找到这个节点//////
            DevComponents.AdvTree.NodeCollection rootnodes = ModuleData.v_AppDBIntegra.ProjectTree.Nodes;
            if (rootnodes == null)
            {
                ex = new Exception("工程树获取节点失败");
                return null;
            }
            for (int i = 0; i < rootnodes.Count; i++)
            {
                DevComponents.AdvTree.Node getnode = rootnodes[i];
                if (getnode.Text == "数据库管理工具")
                {
                    DevComponents.AdvTree.NodeCollection DesNodes = getnode.Nodes;
                    if (DesNodes == null) { ex = new Exception("工程树获取节点失败"); return null; }
                    for (int j = 0; j < DesNodes.Count; j++)
                    {
                        if (DesNodes[j].Text == NodeName)
                        {
                            return DesNodes[j];
                        }
                    }

                }
            }
            return null;
        }

        /// <summary>
        /// cyf 20110602 modify:根据输入信息更新数据库的Combox
        /// </summary>
        /// <param name="DBType">数据库类型</param>
        /// <param name="ex">输出：错误信息</param>
        public void UpDataComBox(string  sDBType, out Exception ex)
        {
            ex = null;
            DevComponents.DotNetBar.Controls.ComboBoxEx DBcombox = null;

            if (sDBType == enumInterDBType.成果文件数据库.ToString())
            {
                DBcombox = ModuleData.v_AppDBIntegra.cmbFileDB;
            }
            else if (sDBType == enumInterDBType.地理编码数据库.ToString())
            {
                DBcombox = ModuleData.v_AppDBIntegra.cmbEntiDB;
            }
            else if (sDBType == enumInterDBType.地名数据库.ToString())
            {
                DBcombox = ModuleData.v_AppDBIntegra.cmbAdressDB;
            }
            else if (sDBType == enumInterDBType.高程数据库.ToString())
            {
                DBcombox = ModuleData.v_AppDBIntegra.cmbDemDB;
            }
            else if (sDBType == enumInterDBType.框架要素数据库.ToString())
            {
                DBcombox = ModuleData.v_AppDBIntegra.cmbFeatureDB;
            }
            else if (sDBType == enumInterDBType.影像数据库.ToString())
            {
                DBcombox = ModuleData.v_AppDBIntegra.cmbImageDB;
            }
            else
            {
                ex = new Exception("不支持的数据库类型");
                return;
            }
            if (DBcombox == null) { ex = new Exception("不支持的数据库类型"); return; }
           //////从数据库获取table 挂接到combox//////
            ////获取数据库类型ID
            //cyf 20110602 modify
            if (ModuleData.TempWks == null)
            {
                ex = new Exception("系统维护库连接失败！");
                return;
            }
            IFeatureWorkspace pFeaWs = ModuleData.TempWks as IFeatureWorkspace;
            if (pFeaWs == null)
            {
                ex = new Exception("系统维护库连接失败！");
                return;
            }
            IQueryDef pQueryDes = pFeaWs.CreateQueryDef();
            pQueryDes.Tables = "databasetypemd";
            pQueryDes.SubFields = "ID";
            pQueryDes.WhereClause = "DATABASETYPE='" + sDBType + "'";
            long lDBType = -1;
            ICursor pCursor = pQueryDes.Evaluate();
            if (pCursor == null) return;
            IRow pRow = pCursor.NextRow();
            if (pRow != null)
            {
                lDBType = Convert.ToInt64(pRow.get_Value(0).ToString().Trim());
            }
            pQueryDes = pFeaWs.CreateQueryDef();
            pQueryDes.Tables = "databasemd";
            pQueryDes.SubFields = "ID,DATABASENAME";
            pQueryDes.WhereClause = "DATABASETYPEID=" + lDBType.ToString();
            pCursor = pQueryDes.Evaluate();
            if (pCursor == null) return;
            //创建表格
            DataTable gettable = new DataTable();
            gettable.Columns.Add("ID",Type.GetType("System.String"));
            gettable.Columns.Add("DATABASENAME", Type.GetType("System.String"));
            pRow = pCursor.NextRow();
            if (pRow != null)
            {
                DataRow pDTRow = gettable.NewRow();
                pDTRow[0] = pRow.get_Value(0).ToString().Trim();
                pDTRow[1] = pRow.get_Value(1).ToString().Trim();
                gettable.Rows.Add(pDTRow);
                pRow = pCursor.NextRow();
            }
            DBcombox.DataSource = null;
            DBcombox.DataSource = gettable;
            DBcombox.DisplayMember = "DATABASENAME";
            DBcombox.ValueMember = "ID";
            if (gettable.Rows.Count > 0)
            {
                DBcombox.SelectedIndex = 0;
            }
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            //end
            #region 原有代码
            //string sql = "SELECT ID FROM databasetypemd WHERE DATABASETYPE='" + sDBType+"'";
            //long lDBType = -1;
            //DataTable gettable = this.m_DBOper.GetSQLTable(sql, out ex);
            //if (ex != null)
            //{
            //    return;
            //}
            //lDBType = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //sql = "SELECT ID,DATABASENAME FROM databasemd WHERE DATABASETYPEID=" + lDBType.ToString(); 
            //gettable = this.m_DBOper.GetSQLTable(sql, out ex);
            //if (ex != null)
            //{
            //    return;
            //}
            //if (gettable == null) return;
            //DBcombox.DataSource = null;
            //DBcombox.DataSource = gettable;
            //DBcombox.DisplayMember = "DATABASENAME";
            //DBcombox.ValueMember = "ID";
            //if (gettable.Rows.Count > 0)
            //{
            //    DBcombox.SelectedIndex = 0;
            //}
            #endregion 
        }

        /// <summary>
        /// 刷新界面的Button   cyf 20110627 modify
        /// </summary>
        /// <param name="sDBName">数据库工程名</param>
        /// <param name="sDBType">数据库类型</param>
        /// <param name="sDBFormate">数据库平台</param>
        /// <param name="DBInFo">数据库信息</param>
        /// <param name="DBConInfo">数据库连接信息</param>
        public void UpDataButton(long lDBID, string sDBName,long lDBTypeID, long lDBFormatID, string sDBType, string sDBFormate, XmlElement DBInFo, XmlElement DBConInfo)
        {
            Exception ex = null;
            //cyf 20110627 delete:
            //enumInterDBType DBType = GetDBType(lDBTypeID, out ex); ; if (null != ex) return;
            //enumInterDBFormat DBFormat = GetDBFormate(lDBFormatID, out ex);; if (null != ex) return; 
            ClsDataBaseProject pDBPro = new ClsDataBaseProject(lDBID,lDBTypeID,lDBFormatID,sDBType,sDBFormate,sDBName, DBInFo, DBConInfo);
            //end
            try
            {
                ModuleData.v_DataBaseProPanel.AddDataBasePro(pDBPro);

            }
            catch
            {
            }

        }

        //cyf 20110627 modify
        private enumInterDBType GetDBType(long lDBTypeID, out Exception ex)
        {
            ex = null;
            if (lDBTypeID.ToString() == enumInterDBType.成果文件数据库.GetHashCode().ToString())
                return enumInterDBType.成果文件数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.地理编码数据库.GetHashCode().ToString())
                return enumInterDBType.地理编码数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.地名数据库.GetHashCode().ToString())
                return enumInterDBType.地名数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())
                return enumInterDBType.高程数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                return enumInterDBType.框架要素数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString())
                return enumInterDBType.影像数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.专题要素数据库.GetHashCode().ToString())
                return enumInterDBType.专题要素数据库;
            else if (lDBTypeID.ToString() == enumInterDBType.电子地图数据库.GetHashCode().ToString())
                return enumInterDBType.电子地图数据库;
            else
            {
                ex = new Exception("不支持的数据库类型");
                return enumInterDBType.框架要素数据库;
            }
        }
        //cyf 20110627  modify
        private enumInterDBFormat GetDBFormate(long lDBFormatID, out Exception ex)
        {
            ex = null;
            if (lDBFormatID.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                return enumInterDBFormat.ARCGISGDB;
            else if (lDBFormatID.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                return enumInterDBFormat.ARCGISPDB;
            else if (lDBFormatID.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                return enumInterDBFormat.ARCGISSDE;
            else if (lDBFormatID.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
                return enumInterDBFormat.FTP;
            else if (lDBFormatID.ToString() == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())
                return enumInterDBFormat.GEOSTARACCESS;
            else if (lDBFormatID.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
                return enumInterDBFormat.GEOSTARORACLE;
            else if (lDBFormatID.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
                return enumInterDBFormat.GEOSTARORSQLSERVER;
            else if (lDBFormatID.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
                return enumInterDBFormat.ORACLESPATIAL;
            else
            {
                ex = new Exception("不支持的数据库平台");
                return enumInterDBFormat.ARCGISSDE;
            }

        }

        /// <summary>
        /// 通过数据库的类型与ID，获取其连接信息的XML cyf 20110614 modify:
        /// </summary>
        /// <param name="lDBTypeID">数据库类型ID</param>
        /// <param name="lDBID">数据库ID</param>
        /// <returns>XmlElement</returns>
        private XmlElement GetDBConInfoEle(long lDBTypeID, long lDBID)
        {
            //cyf 20110614 modify
            XmlDocument DOC=new XmlDocument();
            string sXmlFile=string.Empty;
            if (lDBTypeID.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                sXmlFile = ModuleData.v_feaProjectXML;
            else if (lDBTypeID.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString())
                //cyf 20110608 modify
                //sXmlFile = ModuleData.v_ImageProjectXml;
                sXmlFile = ModuleData.v_feaProjectXML;
                //end
            else if (lDBTypeID.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())
                //cyf 20110608 modify
                //sXmlFile = ModuleData.v_DEMProjectXml;
                sXmlFile = ModuleData.v_feaProjectXML;
                //end
            else
                return null;/////////////其他库体待实现
            try
            {
                DOC.Load(sXmlFile);
                XmlElement DBConnectinfo = ProjectXml.GetProjectInfo(DOC, lDBID);
                return DBConnectinfo;
            }
            catch (Exception eError)
            {
                if (null == ModuleData.v_SysLog) ModuleData.v_SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModuleData.v_SysLog.Write(eError);
                return null;
            }
        }
    }
}
