using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace GeoDBATool
{
    /// <summary>
    /// FrmMain 的摘要说明。

    /// </summary>
    partial class frmSDEManager : MyControls.Forms.BaseForm
    {

        #region 声明与构造函数


        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }

        [DllImport("kernel32")]
        public static extern int SetSystemTime(ref SYSTEMTIME lpSystemTime);

        private MyControls.Control.TabPage userPage, statPage, layerPage, versionPage;
        private Hashtable dataItems;
        private DataBindItem currentItem;

        public frmSDEManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 数据绑定项

        /// </summary>
        private class DataBindItem
        {
            private string tableName;
            private int currentRowIndex;

            public DataBindItem(string tableName)
            {
                this.tableName = tableName;
                currentRowIndex = 0;
            }

            /// <summary>
            /// 获取数据绑定的数据表名称
            /// </summary>
            public string TableName
            {
                get { return tableName; }
            }

            /// <summary>
            /// 获取或设置当前索引

            /// </summary>
            public int CurrentRowIndex
            {
                get { return currentRowIndex; }
                set { currentRowIndex = value; }
            }
        }

        #endregion


        #region 初始化事件和方法

        private DataSet dataSet;

        private void frmSDEManager_Load(object sender, EventArgs e)
        {
            Initialize();

            int i = 1;

            Console.WriteLine("24 << : {0}", i << 18);
        }

        /// <summary>
        /// 初始化处理

        /// </summary>
        private void Initialize()
        {
            // 数据表架构定义初始化
            dataSet = new DataSet("SdeInfo");

            DataTable table = new DataTable("UserInfo");
            table.Columns.Add("ID", typeof(Int32));
            table.Columns.Add("计算机名", typeof(string));
            table.Columns.Add("开始连接时间", typeof(string));
            table.Columns.Add("客户端操作系统", typeof(string));
            table.Columns.Add("连接用户名", typeof(string));
            table.Columns.Add("已连接时间", typeof(string));
            table.DefaultView.Sort = "ID";
            dataSet.Tables.Add(table);

            table = new DataTable("StatInfo");
            table.Columns.Add("ID", typeof(Int32));
            table.Columns.Add("读操作个数", typeof(string));
            table.Columns.Add("写操作个数", typeof(string));
            table.Columns.Add("进程操作个数", typeof(string));
            table.Columns.Add("锁个数", typeof(string));
            table.Columns.Add("已加载的要素个数", typeof(string));
            table.Columns.Add("缓冲区数", typeof(string));
            table.Columns.Add("缓冲区要素个数", typeof(string));
            table.Columns.Add("缓冲区字节大小", typeof(string));
            table.DefaultView.Sort = "ID";
            dataSet.Tables.Add(table);

            table = new DataTable("LayerInfo");
            table.Columns.Add("ID", typeof(Int32));
            table.Columns.Add("图层名称", typeof(string));
            table.Columns.Add("存储类型", typeof(string));
            table.Columns.Add("访问权限", typeof(string));
            table.Columns.Add("空间数据类型", typeof(string));
            table.Columns.Add("是否有空间索引", typeof(string));
            table.Columns.Add("图层创建时间", typeof(string));
            table.Columns.Add("图层空间参考", typeof(string));
            table.Columns.Add("图层描述信息", typeof(string));
            table.DefaultView.Sort = "ID";
            dataSet.Tables.Add(table);

            table = new DataTable("VersionInfo");
            table.Columns.Add("ID", typeof(Int32));
            table.Columns.Add("版本名称", typeof(string));
            table.Columns.Add("访问方式", typeof(string));
            table.Columns.Add("版本创建时间", typeof(string));
            table.Columns.Add("父版本名称", typeof(string));
            table.Columns.Add("描述信息", typeof(string));
            table.DefaultView.Sort = "ID";
            dataSet.Tables.Add(table);

            // 数据网络表样式定义初始化
            if (dataSet.Tables.Contains("UserInfo"))
            {
                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = "UserInfo";
                tableStyle.AllowSorting = true;
                tableStyle.ReadOnly = true;
                tableStyle.RowHeadersVisible = false;

                MyControls.Control.DataGridTextColumn column1 = new MyControls.Control.DataGridTextColumn("ID", "进程ID", 80);
                MyControls.Control.DataGridTextColumn column2 = new MyControls.Control.DataGridTextColumn("计算机名", "计算机名", 120);
                MyControls.Control.DataGridTextColumn column3 = new MyControls.Control.DataGridTextColumn("开始连接时间", "开始连接时间", 190);
                MyControls.Control.DataGridTextColumn column4 = new MyControls.Control.DataGridTextColumn("客户端操作系统", "客户端操作系统", 140);
                MyControls.Control.DataGridTextColumn column5 = new MyControls.Control.DataGridTextColumn("连接用户名", "连接用户名", 100);
                MyControls.Control.DataGridTextColumn column6 = new MyControls.Control.DataGridTextColumn("已连接时间", "已连接时间", 140);

                tableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] { column1, column2, column3, column4, column5, column6 });
                dataGrid.TableStyles.Add(tableStyle);
            }

            if (dataSet.Tables.Contains("StatInfo"))
            {
                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = "StatInfo";
                tableStyle.AllowSorting = true;
                tableStyle.ReadOnly = true;
                tableStyle.RowHeadersVisible = false;

                MyControls.Control.DataGridTextColumn column1 = new MyControls.Control.DataGridTextColumn("ID", "进程ID", 80);
                MyControls.Control.DataGridTextColumn column2 = new MyControls.Control.DataGridTextColumn("读操作个数", "读操作个数", 90);
                MyControls.Control.DataGridTextColumn column3 = new MyControls.Control.DataGridTextColumn("写操作个数", "写操作个数", 90);
                MyControls.Control.DataGridTextColumn column4 = new MyControls.Control.DataGridTextColumn("进程操作个数", "进程操作个数", 100);
                MyControls.Control.DataGridTextColumn column5 = new MyControls.Control.DataGridTextColumn("锁个数", "锁个数", 80);
                MyControls.Control.DataGridTextColumn column6 = new MyControls.Control.DataGridTextColumn("已加载的要素个数", "已加载的要素个数", 120);
                MyControls.Control.DataGridTextColumn column7 = new MyControls.Control.DataGridTextColumn("缓冲区数", "缓冲区数", 100);
                MyControls.Control.DataGridTextColumn column8 = new MyControls.Control.DataGridTextColumn("缓冲区要素数", "缓冲区要素数", 100);
                MyControls.Control.DataGridTextColumn column9 = new MyControls.Control.DataGridTextColumn("缓冲区字节大小", "缓冲区字节大小", 100);

                tableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] { column1, column2, column3, column4, column5, column6, column7, column8, column9 });
                dataGrid.TableStyles.Add(tableStyle);
            }

            if (dataSet.Tables.Contains("LayerInfo"))
            {
                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = "LayerInfo";
                tableStyle.AllowSorting = true;
                tableStyle.ReadOnly = true;
                tableStyle.RowHeadersVisible = false;

                MyControls.Control.DataGridTextColumn column1 = new MyControls.Control.DataGridTextColumn("ID", "图层ID", 60);
                MyControls.Control.DataGridTextColumn column2 = new MyControls.Control.DataGridTextColumn("图层名称", "图层名称", 150);
                MyControls.Control.DataGridTextColumn column3 = new MyControls.Control.DataGridTextColumn("存储类型", "存储类型", 80);
                MyControls.Control.DataGridTextColumn column4 = new MyControls.Control.DataGridTextColumn("访问权限", "访问权限", 140);
                MyControls.Control.DataGridTextColumn column5 = new MyControls.Control.DataGridTextColumn("空间数据类型", "空间数据类型", 120);
                MyControls.Control.DataGridTextColumn column6 = new MyControls.Control.DataGridTextColumn("是否有空间索引", "有否空间索引", 90);
                MyControls.Control.DataGridTextColumn column7 = new MyControls.Control.DataGridTextColumn("图层创建时间", "图层创建日期", 100);
                MyControls.Control.DataGridTextColumn column8 = new MyControls.Control.DataGridTextColumn("图层空间参考", "图层空间参考", 100);
                MyControls.Control.DataGridTextColumn column9 = new MyControls.Control.DataGridTextColumn("图层描述信息", "图层描述信息", 100);

                tableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] { column1, column2, column3, column4, column5, column6, column7, column8, column9 });
                dataGrid.TableStyles.Add(tableStyle);
            }

            if (dataSet.Tables.Contains("VersionInfo"))
            {
                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = "VersionInfo";
                tableStyle.AllowSorting = true;
                tableStyle.ReadOnly = true;
                tableStyle.RowHeadersVisible = false;

                MyControls.Control.DataGridTextColumn column1 = new MyControls.Control.DataGridTextColumn("ID", "版本ID", 60);
                MyControls.Control.DataGridTextColumn column2 = new MyControls.Control.DataGridTextColumn("版本名称", "版本名称", 170);
                MyControls.Control.DataGridTextColumn column3 = new MyControls.Control.DataGridTextColumn("访问方式", "访问方式", 70);
                MyControls.Control.DataGridTextColumn column4 = new MyControls.Control.DataGridTextColumn("版本创建时间", "版本创建时间", 100);
                MyControls.Control.DataGridTextColumn column5 = new MyControls.Control.DataGridTextColumn("父版本名称", "父版本名称", 170);
                MyControls.Control.DataGridTextColumn column6 = new MyControls.Control.DataGridTextColumn("描述信息", "描述信息", 170);

                tableStyle.GridColumnStyles.AddRange(new DataGridColumnStyle[] { column1, column2, column3, column4, column5, column6 });
                dataGrid.TableStyles.Add(tableStyle);
            }

            // 面板定义初始化

            userPage = new MyControls.Control.TabPage("SDE连接信息");
            statPage = new MyControls.Control.TabPage("SDE统计信息");
            layerPage = new MyControls.Control.TabPage("SDE图层信息");
            versionPage = new MyControls.Control.TabPage("SDE版本信息");

            // 数据绑定哈希表

            dataItems = new Hashtable();

            // 数据绑定定义初始化

            DataBindItem dataItem = new DataBindItem("UserInfo");
            dataItems.Add(userPage, dataItem);
            dataItem = new DataBindItem("StatInfo");
            dataItems.Add(statPage, dataItem);
            dataItem = new DataBindItem("LayerInfo");
            dataItems.Add(layerPage, dataItem);
            dataItem = new DataBindItem("VersionInfo");
            dataItems.Add(versionPage, dataItem);

            toolBar.Pages = new MyControls.Control.TabPage[] { userPage, statPage, layerPage, versionPage };
        }

        #endregion

        #region 获取服务器信息方法


        /// <summary>
        /// 获取用户SDE连接信息
        /// </summary>
        private void Get_UserInfo()
        {
            DataTable changeTable = dataSet.Tables["UserInfo"].Clone();
            IntPtr userList;
            int userCount;
            string strNodeName;
            char[] nodeName;
            int iRc = Connection.SE_instance_get_users(m_strServer, m_strInstance, out userList, out userCount);
            if (iRc == 0)
            {
                DateTime dt = DateTime.Parse("1970-01-01 08:00:00"), dtStart;
                Connection.SE_INSTANCE_USER[] pUsers = new Connection.SE_INSTANCE_USER[userCount];
                IntPtr current = userList;
                DataRow newRow;
                for (int i = 0; i < userCount; i++)
                {
                    newRow = changeTable.NewRow();
                    pUsers[i] = new Connection.SE_INSTANCE_USER();
                    pUsers[i] = (Connection.SE_INSTANCE_USER)Marshal.PtrToStructure(current, typeof(Connection.SE_INSTANCE_USER));
                    Marshal.DestroyStructure(current, typeof(Connection.SE_INSTANCE_USER));

                    current = (IntPtr)((int)current + Marshal.SizeOf(pUsers[i]));

                    newRow["ID"] = pUsers[i].svrpid;

                    // 计算机名
                    nodeName = pUsers[i].nodename;
                    strNodeName = "";
                    for (int j = 0; j < nodeName.Length; j++)
                    {
                        if (nodeName[j] != '\0')
                        {
                            strNodeName += Char.ToString(nodeName[j]);
                        }
                        else
                            break;
                    }
                    newRow["计算机名"] = strNodeName;
                    dtStart = dt.AddSeconds(pUsers[i].cstime);
                    newRow["开始连接时间"] = dtStart.ToString("yy年MM月dd日 HH时mm分ss秒");

                    // 客户端操作系统

                    nodeName = pUsers[i].sysname;
                    strNodeName = "";
                    for (int j = 0; j < nodeName.Length; j++)
                    {
                        if (nodeName[j] != '\0')
                        {
                            strNodeName += Char.ToString(nodeName[j]);
                        }
                        else
                            break;
                    }
                    newRow["客户端操作系统"] = strNodeName;

                    // 连接用户名

                    nodeName = pUsers[i].username;
                    strNodeName = "";
                    for (int j = 0; j < nodeName.Length; j++)
                    {
                        if (nodeName[j] != '\0')
                        {
                            strNodeName += Char.ToString(nodeName[j]);
                        }
                        else
                            break;
                    }
                    newRow["连接用户名"] = strNodeName;


                    TimeSpan ts = DateTime.Now - dtStart;

                    int days = ts.Days, hours = ts.Hours, minutes = ts.Minutes, seconds = ts.Seconds;
                    string strTime;
                    if (days > 0)
                        strTime = String.Format("{0}天{1}小时{2}分{3}秒", days, hours, minutes, seconds);
                    else if (hours > 0)
                        strTime = String.Format("{0}小时{1}分{2}秒", hours, minutes, seconds);
                    else if (minutes > 0)
                        strTime = String.Format("{0}分{1}秒", minutes, seconds);
                    else if (seconds > 0)
                        strTime = String.Format("{0}秒", seconds);
                    else
                        strTime = "0秒";

                    newRow["已连接时间"] = strTime;

                    changeTable.Rows.Add(newRow);
                }

                changeTable.AcceptChanges();
                Connection.SE_instance_free_users(ref pUsers, userCount);
            }

            if (changeTable.Rows.Count > 0 && currentItem.TableName == changeTable.TableName)
            {
                Invoke(new DataGridRefreshDelegate(DataGridRefresh), new object[] { changeTable });
            }
            else
            {
                Invoke(new MethodInvoker(timer1.Start));
                //Cursor = Cursors.Default;
                Invoke(new SetCursorDelegate(SetCursor));
            }
        }

        /// <summary>
        /// 获取SDE连接统计信息
        /// </summary>
        private void Get_StatInfo()
        {
            DataTable changeTable = dataSet.Tables["StatInfo"].Clone();
            IntPtr statList;
            int statCount;
            int iRc = Connection.SE_instance_get_statistics(m_strServer, m_strInstance, out statList, out statCount);
            if (iRc == 0)
            {
                Connection.SE_INSTANCE_STATS[] pStats = new Connection.SE_INSTANCE_STATS[statCount];
                IntPtr current = statList;
                DataRow newRow;
                for (int i = 0; i < statCount; i++)
                {
                    newRow = changeTable.NewRow();
                    pStats[i] = new Connection.SE_INSTANCE_STATS();
                    pStats[i] = (Connection.SE_INSTANCE_STATS)Marshal.PtrToStructure(current, typeof(Connection.SE_INSTANCE_STATS));
                    Marshal.DestroyStructure(current, typeof(Connection.SE_INSTANCE_STATS));

                    current = (IntPtr)((int)current + Marshal.SizeOf(pStats[i]));

                    newRow["ID"] = pStats[i].pid;
                    newRow["读操作个数"] = String.Format("{0}", pStats[i].rcount);
                    newRow["写操作个数"] = String.Format("{0}", pStats[i].wcount);
                    newRow["进程操作个数"] = String.Format("{0}", pStats[i].opcount);
                    newRow["锁个数"] = String.Format("{0}", pStats[i].numlocks);
                    newRow["已加载的要素个数"] = String.Format("{0}", pStats[i].fb_partial);
                    newRow["缓冲区数"] = String.Format("{0}", pStats[i].fb_count);
                    newRow["缓冲区要素个数"] = String.Format("{0}", pStats[i].fb_fcount);
                    newRow["缓冲区字节大小"] = String.Format("{0}", pStats[i].fb_kbytes);

                    changeTable.Rows.Add(newRow);
                }

                changeTable.AcceptChanges();
                if (statCount > 0)
                    Connection.SE_instance_free_statistics(ref pStats, statCount);
            }

            if (changeTable.Rows.Count > 0 && currentItem.TableName == changeTable.TableName)
            {

                Invoke(new DataGridRefreshDelegate(DataGridRefresh), new object[] { changeTable });
            }
            else
            {
                Invoke(new MethodInvoker(timer1.Start));
                //Cursor = Cursors.Default;
                Invoke(new SetCursorDelegate(SetCursor));
            }
        }

        /// <summary>
        /// 获取SDE图层信息
        /// </summary>
        private void Get_LayerInfo()
        {
            DataTable changeTable = dataSet.Tables["LayerInfo"].Clone();

            Connection.SE_ERROR pError = new Connection.SE_ERROR();
            IntPtr pSdeConn;
            int iRc = Connection.SE_connection_create(m_strServer, m_strInstance, "SDE", m_strUser, m_strPwd, ref pError, out pSdeConn);
            if (iRc == 0)
            {
                try
                {
                    IntPtr layerList;
                    int layerCount;
                    iRc = Layer.SE_layer_get_info_list(pSdeConn, out layerList, out layerCount);

                    if (layerCount == 0)
                    {
                        Connection.SE_connection_free(pSdeConn);
                        Invoke(new MethodInvoker(timer1.Start));
                        Cursor = Cursors.Default;
                        return;
                    }

                    if (iRc == 0)
                    {
                        Int32[] iLayerList = new Int32[layerCount];
                        Marshal.Copy(layerList, iLayerList, 0, layerCount);
                        DataRow newRow;
                        string strLayerName = "";
                        int layerID, storageType, privileges, shapeType;
                        bool blnSpatialIndex;
                        for (int i = 0; i < layerCount; i++)
                        {
                            newRow = changeTable.NewRow();
                            IntPtr layerInfo = (IntPtr)iLayerList[i];

                            iRc = Layer.SE_layerinfo_get_id(layerInfo, out layerID);
                            if (iRc == 0)
                            {
                                newRow["ID"] = layerID;
                            }

                            char[] layer_name = new char[1808];
                            char[] layer_column = new char[256];
                            iRc = Layer.SE_layerinfo_get_spatial_column(layerInfo, layer_name, layer_column);
                            if (iRc == 0)
                            {
                                strLayerName = "";
                                for (int j = 0; j < layer_name.Length; j++)
                                {
                                    if (layer_name[j] != '\0')
                                    {
                                        strLayerName += Char.ToString(layer_name[j]);
                                    }
                                    else
                                        break;
                                }
                                newRow["图层名称"] = strLayerName;
                            }

                            iRc = Layer.SE_layerinfo_get_storage_type(layerInfo, out storageType);
                            if (iRc == 0)
                            {
                                switch (storageType)
                                {
                                    case 16777216:	// 1L << 24		SE_STORAGE_SDEBINARY_TYPE
                                        newRow["存储类型"] = "SDE二进制";
                                        break;
                                    case 268435456:	// 1L << 28		SE_STORAGE_LOB_TYPE
                                        newRow["存储类型"] = "Oracle LOB";
                                        break;
                                    case 33554432:	// 1L << 25		SE_STORAGE_WKB_TYPE
                                        newRow["存储类型"] = "普通二进制";
                                        break;
                                    case 134217728:	// 1L << 27		SE_STORAGE_SPATIAL_TYPE	
                                        newRow["存储类型"] = "SDO_GEOMETRY";
                                        break;
                                    default:
                                        newRow["存储类型"] = "其它类型";
                                        break;
                                }
                            }

                            iRc = Layer.SE_layerinfo_get_access(layerInfo, out privileges);
                            if (iRc == 0)
                            {
                                string strPrivileges = "";
                                if (privileges >= 16)
                                {
                                    strPrivileges = "删除";
                                    privileges -= 16;
                                }
                                if (privileges >= 8)
                                {
                                    strPrivileges = "插入/" + strPrivileges;
                                    privileges -= 8;
                                }
                                if (privileges >= 4)
                                {
                                    strPrivileges = "更新/" + strPrivileges;
                                    privileges -= 4;
                                }
                                if (privileges >= 2)
                                {
                                    strPrivileges = "查询/" + strPrivileges;
                                }
                                newRow["访问权限"] = strPrivileges;
                            }

                            iRc = Layer.SE_layerinfo_get_shape_types(layerInfo, out shapeType);
                            if (iRc == 0)
                            {
                                switch (shapeType)
                                {
                                    case 1:			// 1L			SE_NIL_TYPE_MASK
                                        newRow["空间数据类型"] = "空类型(Nil)";
                                        break;
                                    case 2:			// 1L << 1		SE_POINT_TYPE_MASK
                                    case 3:			// SE_POINT_TYPE_MASK + SE_NIL_TYPE_MASK
                                        newRow["空间数据类型"] = "简单点(Point)";
                                        break;
                                    case 4:			// 1L << 2		SE_LINE_TYPE_MASK
                                    case 8:			// 1L << 3		SE_SIMPLE_LINE_TYPE_MASK
                                        newRow["空间数据类型"] = "简单线(Line)";
                                        break;
                                    case 262157:	// SE_MULTIPART_TYPE_MASK(1L<<18) + SE_SIMPLE_LINE_TYPE_MASK + SE_LINE_TYPE_MASK + SE_NIL_TYPE_MASK
                                        newRow["空间数据类型"] = "多义线(Polyline)";
                                        break;
                                    case 16:		// 1L << 4		SE_AREA_TYPE_MASK	
                                        newRow["空间数据类型"] = "简单面(Area)";
                                        break;
                                    case 262161:	// SE_MULTIPART_TYPE_MASK(1L<<18) + SE_AREA_TYPE_MASK + SE_NIL_TYPE_MASK
                                        newRow["空间数据类型"] = "多面(Polygon)";
                                        break;
                                }
                                //								Console.WriteLine( "{0} :  {1}", strLayerName, shapeType );

                            }

                            blnSpatialIndex = Layer.SE_layerinfo_has_spatial_index(layerInfo);
                            if (blnSpatialIndex)
                                newRow["是否有空间索引"] = "有";
                            else
                                newRow["是否有空间索引"] = "无";

                            Layer.SE_TIME pTime = new Layer.SE_TIME();
                            iRc = Layer.SE_layerinfo_get_creation_date(layerInfo, ref pTime);
                            if (iRc == 0)
                            {
                                newRow["图层创建时间"] = String.Format("{0}年{1}月{2}日", pTime.tm_year + 1900, pTime.tm_mon + 1,
                                    pTime.tm_mday); ;
                            }

                            //空间参考、图层描述，是否自动锁定、是否允许标注

                            IntPtr coordinRef;                 //空间参考

                            string coordinSysdes;                 //空间坐标描述
                            iRc = Layer.SE_layerinfo_get_coordref(layerInfo, out coordinRef);
                            if (iRc == 0)
                            {
                                char[] coorDes = new char[1808];
                                int iRc2 = Layer.SE_coordref_get_description(coordinRef, coorDes);
                                if (iRc2 == 0)
                                {
                                    coordinSysdes = "";
                                    for (int k = 0; k < coorDes.Length; k++)
                                    {
                                        if (coorDes[k] != '\0')
                                        {
                                            coordinSysdes += Char.ToString(coorDes[k]);
                                        }
                                        else
                                            break;
                                    }
                                    newRow["图层空间参考"] = coordinSysdes;
                                }
                            }
                            else
                            {

                            }

                            string layerDes;     //图层描述
                            char[] lDes = new char[1808];
                            iRc = Layer.SE_layerinfo_get_description(layerInfo, lDes);
                            if (iRc == 0)
                            {
                                layerDes = "";
                                for (int k = 0; k < lDes.Length; k++)
                                {
                                    if (lDes[k] != '\0')
                                    {
                                        layerDes += Char.ToString(lDes[k]);
                                    }
                                    else
                                        break;
                                }
                                newRow["图层描述信息"] = layerDes;
                            }

                            changeTable.Rows.Add(newRow);
                        }

                        changeTable.AcceptChanges();
                        if (layerCount > 0)
                            Layer.SE_layer_free_info_list(layerCount, layerList);
                    }
                    Connection.SE_connection_free(pSdeConn);
                }
                catch (Exception ex)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    //********************************************************************
                    Console.WriteLine(ex.Message);
                    Connection.SE_connection_free(pSdeConn);
                }

                if (changeTable.Rows.Count > 0 && currentItem.TableName == changeTable.TableName)
                {
                    Invoke(new DataGridRefreshDelegate(DataGridRefresh), new object[] { changeTable });
                }
                else
                {
                    Invoke(new MethodInvoker(timer1.Start));
                    //Cursor = Cursors.Default;
                    Invoke(new SetCursorDelegate(SetCursor));
                }
            }
        }

        /// <summary>
        /// 获取SDE连接版本信息
        /// </summary>
        private void Get_VersionInfo()
        {
            DataTable changeTable = dataSet.Tables["VersionInfo"].Clone();
            Connection.SE_ERROR pError = new Connection.SE_ERROR();
            IntPtr pSdeConn;
            int iRc = Connection.SE_connection_create(m_strServer, m_strInstance, "SDE", m_strUser, m_strPwd, ref pError, out pSdeConn);
            if (iRc == 0)
            {
                try
                {
                    IntPtr versionList;
                    int versionCount;
                    iRc = Version.SE_version_get_info_list(pSdeConn, "", out versionList, out versionCount);

                    if (versionCount == 0)
                    {
                        Connection.SE_connection_free(pSdeConn);
                        Invoke(new MethodInvoker(timer1.Start));
                        Cursor = Cursors.Default;
                        return;
                    }

                    if (iRc == 0)
                    {
                        Int32[] iVersionList = new Int32[versionCount];
                        Marshal.Copy(versionList, iVersionList, 0, versionCount);
                        DataRow newRow;
                        string strVersionName = "";
                        int versionID, versionAccess;
                        for (int i = 0; i < versionCount; i++)
                        {
                            newRow = changeTable.NewRow();
                            IntPtr versionInfo = (IntPtr)iVersionList[i];

                            iRc = Version.SE_versioninfo_get_id(versionInfo, out versionID);
                            if (iRc == 0)
                            {
                                newRow["ID"] = versionID;
                            }

                            char[] version_name = new char[64];
                            iRc = Version.SE_versioninfo_get_name(versionInfo, version_name);
                            if (iRc == 0)
                            {
                                strVersionName = "";
                                for (int j = 0; j < version_name.Length; j++)
                                {
                                    if (version_name[j] != '\0')
                                    {
                                        strVersionName += Char.ToString(version_name[j]);
                                    }
                                    else
                                        break;
                                }
                                newRow["版本名称"] = strVersionName;
                            }

                            iRc = Version.SE_versioninfo_get_access(versionInfo, out versionAccess);
                            if (iRc == 0)
                            {
                                switch (versionAccess)
                                {
                                    case 0:			// 0		SE_VERSION_ACCESS_PUBLIC
                                        newRow["访问方式"] = "公有";
                                        break;
                                    case 1:			// 1		SE_VERSION_ACCESS_PROTECTED
                                        newRow["访问方式"] = "保护";
                                        break;
                                    case 2:			// 2		SE_VERSION_ACCESS_PRIVATE
                                        newRow["访问方式"] = "私有";
                                        break;
                                }
                            }

                            Layer.SE_TIME pTime = new Layer.SE_TIME();
                            iRc = Version.SE_versioninfo_get_creation_time(versionInfo, ref pTime);
                            if (iRc == 0)
                            {
                                newRow["版本创建时间"] = String.Format("{0}年{1}月{2}日", pTime.tm_year + 1900, pTime.tm_mon + 1,
                                    pTime.tm_mday); ;
                            }

                            version_name = new char[64];
                            iRc = Version.SE_versioninfo_get_parent_name(versionInfo, version_name);
                            if (iRc == 0)
                            {
                                strVersionName = "";
                                for (int j = 0; j < version_name.Length; j++)
                                {
                                    if (version_name[j] != '\0')
                                    {
                                        strVersionName += Char.ToString(version_name[j]);
                                    }
                                    else
                                        break;
                                }
                                newRow["父版本名称"] = strVersionName;
                            }

                            version_name = new char[64];
                            iRc = Version.SE_versioninfo_get_description(versionInfo, version_name);
                            if (iRc == 0)
                            {
                                strVersionName = "";
                                for (int j = 0; j < version_name.Length; j++)
                                {
                                    if (version_name[j] != '\0')
                                    {
                                        strVersionName += Char.ToString(version_name[j]);
                                    }
                                    else
                                        break;
                                }
                                newRow["描述信息"] = strVersionName;
                            }


                            changeTable.Rows.Add(newRow);
                        }

                        changeTable.AcceptChanges();
                        if (versionCount > 0)
                            Version.SE_version_free_info_list(versionCount, versionList);
                    }
                    Connection.SE_connection_free(pSdeConn);
                }
                catch (Exception ex)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(ex, null, DateTime.Now);
                    }
                    //********************************************************************
                    Console.WriteLine(ex.Message);
                    Connection.SE_connection_free(pSdeConn);
                }

                if (changeTable.Rows.Count > 0 && currentItem.TableName == changeTable.TableName)
                {
                    Invoke(new DataGridRefreshDelegate(DataGridRefresh), new object[] { changeTable });
                }
                else
                {
                    Invoke(new MethodInvoker(timer1.Start));
                    //Cursor = Cursors.Default;
                    Invoke(new SetCursorDelegate(SetCursor));
                }
            }
        }

        #endregion

        #region 数据集刷新方法


        private delegate void DataGridRefreshDelegate(DataTable changeTable);
        private void DataGridRefresh(DataTable changeTable)
        {
            currentItem.CurrentRowIndex = dataGrid.CurrentRowIndex;
            string strTableName = changeTable.TableName;
            DataTable table = dataSet.Tables[strTableName];
            table.BeginLoadData();
            foreach (DataRow dr in table.Rows)
            {
                DataRow[] row = changeTable.Select(String.Format("ID='{0}'", dr["ID"]));
                if (row.Length > 0)
                {
                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        if (!row[0][dataColumn.ColumnName].Equals(dr[dataColumn]))
                            dr[dataColumn] = row[0][dataColumn.ColumnName];
                    }
                    changeTable.Rows.Remove(row[0]);
                }
                else
                    dr.Delete();
            }

            table.AcceptChanges();

            if (changeTable.Rows.Count > 0)
                foreach (DataRow dr in changeTable.Rows)
                    table.ImportRow(dr);
            int currentIndex = currentItem.CurrentRowIndex;
            int RowCount = table.Rows.Count;
            if (RowCount > 0)
            {
                if (currentIndex > -1)
                {
                    if (currentIndex < RowCount)
                        dataGrid.CurrentRowIndex = currentIndex;
                    else
                    {
                        int index = currentIndex - 1;
                        while (index >= RowCount && index > -1)
                            index--;
                        if (index > -1)
                            dataGrid.CurrentRowIndex = index;
                        else
                            dataGrid.CurrentRowIndex = 0;
                    }
                }
                else
                    dataGrid.CurrentRowIndex = 0;
            }

            table.EndLoadData();
            table.DefaultView.Sort = "ID";
            timer1.Start();
            Cursor = Cursors.Default;
        }

        #endregion

        //鼠标状态,获取默认光标
        private delegate void SetCursorDelegate();
        private void SetCursor()
        {
            Cursor = Cursors.Default;
        }

        #region 其它事件与方法


        private string m_strServer, m_strInstance, m_strUser, m_strPwd;
        private void btnConnect_Click(object sender, System.EventArgs e)
        {
            string strServer = txtServer.Text.Trim();
            string strInstance = txtInstance.Text.Trim();
            string strUser = txtUser.Text.Trim();
            string strPwd = txtPwd.Text.Trim();

            this.m_strServer = strServer;
            this.m_strInstance = strInstance;
            this.m_strUser = strUser;
            this.m_strPwd = strPwd;

            Connection.SE_ERROR pError = new Connection.SE_ERROR();
            IntPtr pConn;
            Int32 iRc = Connection.SE_connection_create(strServer, strInstance, "", strUser, strPwd, ref pError, out pConn);
            if (iRc == 0)
            {
                // 获取服务器时间

                int serverTime = Connection.SE_connection_get_server_time(pConn);
                if (serverTime > 0)
                {
                    DateTime dt = DateTime.Parse("1970-01-01 00:00:00").AddSeconds(serverTime);
                    SYSTEMTIME curSysTime = new SYSTEMTIME();
                    curSysTime.wYear = Convert.ToInt16(dt.Year);
                    curSysTime.wMonth = Convert.ToInt16(dt.Month);
                    curSysTime.wDayOfWeek = Convert.ToInt16(dt.DayOfWeek);
                    curSysTime.wDay = Convert.ToInt16(dt.Day);
                    curSysTime.wHour = Convert.ToInt16(dt.Hour);
                    curSysTime.wMinute = Convert.ToInt16(dt.Minute);
                    curSysTime.wSecond = Convert.ToInt16(dt.Second);
                    curSysTime.wMilliseconds = Convert.ToInt16(dt.Millisecond);

                    // 设置当前系统时间
                    SetSystemTime(ref curSysTime);
                }
                Connection.SE_connection_free(pConn);
            }

            CheckInstanceStatus(strServer, strInstance);

        }

        private void CheckInstanceStatus(string strServer, string strInstance)
        {
            timer1.Stop();
            toolBar.Buttons = new MyControls.Control.ToolBarButton[0];
            foreach (DataTable table in dataSet.Tables)
                table.Rows.Clear();
            btnStopView.Enabled = false;
            blnStartTime = false;
            Connection.SE_INSTANCE_STATUS pStatus = new Connection.SE_INSTANCE_STATUS();
            int iRc = Connection.SE_instance_status(strServer, strInstance, ref pStatus);
            if (iRc == -101)
            {
                gbStatus.Enabled = btnStart.Enabled = true;
                btnStop.Enabled = btnPause.Enabled = btnResume.Enabled = btnView.Enabled = false;
            }
            else if (iRc == 0)
            {
                gbStatus.Enabled = true;
                btnStart.Enabled = false;

                if (pStatus.system_mode == 1)
                {
                    btnStop.Enabled = true;
                    btnPause.Enabled = true;
                    btnResume.Enabled = false;
                    btnView.Enabled = true;
                }
                else if (pStatus.system_mode == 2)
                {
                    btnStop.Enabled = false;
                    btnPause.Enabled = false;
                    btnResume.Enabled = true;
                    btnView.Enabled = false;
                }
                else
                {
                    btnStop.Enabled = false;
                    btnPause.Enabled = false;
                    btnResume.Enabled = false;
                    btnView.Enabled = false;
                }
            }
            else
                gbStatus.Enabled = false;
        }

        private Thread thread;
        private bool blnStartTime = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (blnStartTime)
            {
                if (thread != null && thread.IsAlive)
                    thread.Join();
                switch (currentItem.TableName)
                {
                    case "UserInfo":
                        thread = new Thread(new ThreadStart(Get_UserInfo));
                        thread.Start();
                        break;
                    case "StatInfo":
                        thread = new Thread(new ThreadStart(Get_StatInfo));
                        thread.Start();
                        break;
                    case "LayerInfo":
                        thread = new Thread(new ThreadStart(Get_LayerInfo));
                        thread.Start();
                        break;
                    case "VersionInfo":
                        thread = new Thread(new ThreadStart(Get_VersionInfo));
                        thread.Start();
                        break;
                }
            }
        }

        private void btnView_Click(object sender, System.EventArgs e)
        {
            blnStartTime = true;
            if (!timer1.Enabled)
                timer1.Start();
            btnView.Enabled = false;
            btnStopView.Enabled = true;
            btnDelSel.Enabled = true;
        }

        private void btnStopView_Click(object sender, System.EventArgs e)
        {
            blnStartTime = false;
            btnView.Enabled = true;
            btnStopView.Enabled = false;
        }

        private void btnKill_Click(object sender, System.EventArgs e)
        {
            if (dataGrid.CurrentRowIndex > -1)
            {
                Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 4, Convert.ToInt32(dataGrid.CurrentRow["ID"]));
            }
        }


        private void frmSDEManager_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            this.Update();
            blnStartTime = false;
            if (thread != null && thread.IsAlive)
            {
                thread.Join();
            }
        }

        private void btnStart_Click(object sender, System.EventArgs e)
        {
            //开始

            gbStatus.Enabled = false;
            Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 6, 0);//6
            CheckInstanceStatus(m_strServer, m_strInstance);
        }

        private void btnStop_Click(object sender, System.EventArgs e)
        {
            //关闭
            gbStatus.Enabled = false;
            Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 1, 0);//1
            CheckInstanceStatus(m_strServer, m_strInstance);
        }

        private void btnPause_Click(object sender, System.EventArgs e)
        {
            //暂停
            gbStatus.Enabled = false;
            Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 2, 0);//2
            CheckInstanceStatus(m_strServer, m_strInstance);
        }

        private void btnResume_Click(object sender, System.EventArgs e)
        {
            //回复
            gbStatus.Enabled = false;
            Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 3, 0);//3
            CheckInstanceStatus(m_strServer, m_strInstance);
        }

        private void btnDelSel_Click(object sender, EventArgs e)
        {
            //删除连接kill one
            gbStatus.Enabled = false;
            if (dataGrid.CurrentRow != null)
            {
                if (MessageBox.Show(this, String.Format("确定杀死进程ID为[{0}]的连接吗？", dataGrid.CurrentRow["ID"]), "提示",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 4, Convert.ToInt32(dataGrid.CurrentRow["ID"]));
                    CheckInstanceStatus(m_strServer, m_strInstance);
                }
            }
            //Connection.SE_ERROR pError = new Connection.SE_ERROR();
            //IntPtr pConn;
            //Int32 iRc = Connection.SE_connection_create(m_strServer, m_strInstance, "", m_strUser, m_strPwd, ref pError, out pConn);
            //if (iRc == 0)
            //{
            //    Connection.SE_connection_free(pConn);
            //CheckInstanceStatus(m_strServer, m_strInstance);
            //}
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            //删除 所有连接  kill all
            gbStatus.Enabled = false;
            if (MessageBox.Show(this, "确定删除所有连接吗？", "提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 5, 0);
                CheckInstanceStatus(m_strServer, m_strInstance);
                btnDelSel.Enabled = false;
            }
        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            CheckInstanceStatus(m_strServer, m_strInstance);
        }

        private void toolBar_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //若存在连接，则刷新列表

            // 清除工具栏按钮

            toolBar.Buttons = new MyControls.Control.ToolBarButton[0];
            MyControls.Control.TabPage currentPage = toolBar.Pages[toolBar.SelectedIndex];

            currentItem = dataItems[currentPage] as DataBindItem;
            dataGrid.DataSource = dataSet.Tables[currentItem.TableName];

            if (gbStatus.Enabled)
            {
                Cursor = Cursors.WaitCursor;
                switch (currentItem.TableName)
                {
                    case "UserInfo":
                        //						thread = new Thread( new ThreadStart( Get_UserInfo ) );
                        //						thread.Start();
                        timer1.Interval = 1000;
                        timer1.Start();
                        break;
                    case "StatInfo":
                        //						thread = new Thread( new ThreadStart( Get_StatInfo ) );
                        //						thread.Start();
                        timer1.Interval = 1000;
                        timer1.Start();
                        break;
                    case "LayerInfo":
                        blnStartTime = false;
                        if (thread != null && thread.IsAlive)
                            thread.Join();
                        thread = new Thread(new ThreadStart(Get_LayerInfo));
                        thread.Start();
                        timer1.Interval = 20000;
                        blnStartTime = true;
                        timer1.Start();
                        break;
                    case "VersionInfo":
                        blnStartTime = false;
                        if (thread != null && thread.IsAlive)
                            thread.Join();
                        thread = new Thread(new ThreadStart(Get_VersionInfo));
                        thread.Start();
                        timer1.Interval = 20000;
                        blnStartTime = true;
                        timer1.Start();
                        break;
                }
            }
        }

        private void dataGrid_CurrentRowIndexChanged(object sender, System.EventArgs e)
        {
            if (dataGrid.CurrentRowIndex > -1)
            {
                MyControls.Control.TabPage currentPage = toolBar.Pages[toolBar.SelectedIndex];
                if (currentPage == userPage)
                {
                    MyControls.Control.ToolBarButton button1 = new MyControls.Control.ToolBarButton("关闭连接", "关闭当前SDE连接", 0);
                    toolBar.Buttons = new MyControls.Control.ToolBarButton[] { button1 };
                }
                else if (currentPage == statPage)
                {
                }
            }
        }

        private void toolBar_ButtonClick(object sender, MyControls.Control.ButtonClickEventArgs e)
        {
            switch (e.Button.Text)
            {
                case "关闭连接":
                    if (MessageBox.Show(this, String.Format("确定关闭进程ID为[{0}]的连接吗？", dataGrid.CurrentRow["ID"]), "系统提示",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Cursor = Cursors.WaitCursor;
                        this.Update();
                        Connection.SE_instance_control(m_strServer, m_strInstance, m_strPwd, 4, Convert.ToInt32(dataGrid.CurrentRow["ID"]));
                    }
                    break;
            }
            Cursor = Cursors.Default;
        }

        private void frmSDEManager_Closed(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #endregion



    }
}

