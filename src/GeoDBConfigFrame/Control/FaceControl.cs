using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
//?using GeoDataCenterFrame;
using System.Xml;
using System.IO;
using System.Data.OleDb;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoDBConfigFrame
{
    public partial class FaceControl : UserControl
    {
        //右键菜单集合  
        //目前右键菜单会死机
      // private Dictionary<string, DevComponents.DotNetBar.ContextMenuBar> _dicContextMenu;
        public OleDbDataAdapter m_Adapter;
        public DataTable m_dataTable;
        public string m_connstr;
        public string m_TableName;
        public string m_strLogFilePath = Application.StartupPath + "\\..\\Log\\DataManagerLog.txt";

        //地图浏览工具栏容器
        private Control _MapToolControl;
        public FaceControl(string strName, string strCation)
        {
            InitializeComponent();

          
            //初始化配置对应视图控件
            InitialMainViewControl();
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            this.Name = strName;
            this.Tag = strCation;
            ModFrameData.v_AppPrivileges.MainUserControl = this;
            ModFrameData.v_AppPrivileges.DataTabIndexTree = DataIndexTree;
            ModFrameData.v_AppPrivileges.GridCtrl  = gridControl;
            ModFrameData.v_AppPrivileges.tipRichBox = tipRichBox;
            //日志文件路径
            ModFrameData.v_AppPrivileges.strLogFilePath = m_strLogFilePath;
           // ModFrameData.v_AppPrivileges.MainUserControl = this;

            //根据sys配置文件添加菜单和工具栏
            InitialFrmDefControl();      
        }

        public void InitialMainViewControl()
        {
            frmBarManager newfrmBarManager = new frmBarManager();
            newfrmBarManager.TopLevel = false;
            newfrmBarManager.Dock = DockStyle.Fill;
            newfrmBarManager.Show();
            this.Controls.Add(newfrmBarManager);

            //初始化树节点
            InitIndexTree();
      

            //提示
            //根据配置文件获取要创建的树信息。

            //加载设置数据索引窗口
            DevComponents.DotNetBar.Bar barIndexView = newfrmBarManager.CreateBar("barIndexView", enumLayType.FILL);
            barIndexView.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelIndexView = newfrmBarManager.CreatePanelDockContainer("PanelIndexView", barIndexView);
            PanelIndexView.Controls.Add(this.tabControlIndex);
            this.tabControlIndex.Dock = DockStyle.Fill;


            //加载设置视图窗口
            DevComponents.DotNetBar.Bar barMapView = newfrmBarManager.CreateBar("barMapView", enumLayType.FILL);
            barMapView.CanHide = false;
            DevComponents.DotNetBar.PanelDockContainer PanelMapView = newfrmBarManager.CreatePanelDockContainer("PanelMapView", barMapView);
            PanelMapView.Controls.Add(this.tabControlData);
            this.tabControlData.Dock = DockStyle.Fill;
            _MapToolControl = PanelMapView as Control;

            //布局设置
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().Dock(barIndexView, barMapView, eDockSide.Right);
            newfrmBarManager.MainDotNetBarManager.FillDockSite.GetDocumentUIManager().SetBarWidth(barIndexView, this.Width / 5);

            //加载设置提示窗体
            //用户组配置
      /*      PanelDockContainer PanelTipData = new PanelDockContainer();
            PanelTipData.Controls.Add(this.tipRichBox);
            this.tipRichBox.Dock = DockStyle.Fill;
            DockContainerItem dockItemData = new DockContainerItem("dockItemData", "提示");
            dockItemData.Control = PanelTipData;
            newfrmBarManager.ButtomBar.Items.Add(dockItemData);   */
        }

        //初始化索引树节点
        public void InitIndexTree()
        {
            this.DataIndexTree.Nodes.Clear();
            TreeNode tparent;
            tparent = new TreeNode();
            tparent.Text = "数据配置";
            tparent.Tag = 0;
            tparent.ImageIndex = 0;
            this.DataIndexTree.Nodes.Add(tparent);
            this.DataIndexTree.ExpandAll();
  

            //加入子节点
            TreeNode tNewNode;
            GetDataTreeInitIndex  dIndex = new GetDataTreeInitIndex();
        
           //遍历获取itemName信息 
            string  strTblName = "";
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc != null)
            {
                if (File.Exists(dIndex.m_strInitXmlPath))
                {
                    xmldoc.Load(dIndex.m_strInitXmlPath);

                    //修改根节点节点名称
                    string strRootName = "";
                    string strSearchRoot = "//Rootset";
                    XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
                    XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                    strRootName = xmlElentRoot.GetAttribute("ItemName");
                    tparent.Text = strRootName;

                    //首先添加第一级子节点 Childset
                    string strSearch = "//Childset";
                    XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                    XmlNodeList xmlNdList;
                    xmlNdList = xmlNode.ChildNodes;
                    foreach (XmlNode xmlChild in xmlNdList)
                    {
                        strTblName = "";
                        XmlElement xmlElent = (XmlElement)xmlChild;
                        strTblName = xmlElent.GetAttribute("ItemName");

                        tNewNode = new TreeNode();
                        tNewNode.Text = strTblName;
                        tNewNode.Tag = 1;
                        tNewNode.ImageIndex =0;
                        tparent.Nodes.Add(tNewNode);
                        tparent.ExpandAll();

                        //添加最终子节点
                        AddLeafItem(tNewNode, xmlChild);
                    }
                }
            }  
        }

        //添加叶子节点
        public void AddLeafItem(TreeNode treeNode, XmlNode xmlNode)
        {
            if (treeNode != null && xmlNode != null)
            {
                 TreeNode tNewNode;
                 string strTblName = "";
    
                 XmlNodeList xmlNdList;
                 xmlNdList = xmlNode.ChildNodes;
                 foreach (XmlNode xmlChild in xmlNdList)
                 {
                     strTblName = "";
                     XmlElement xmlElent = (XmlElement)xmlChild;
                     strTblName = xmlElent.GetAttribute("ItemName");
                     tNewNode = new TreeNode();
                     tNewNode.Text = strTblName;
                     tNewNode.Tag = 2;
                     tNewNode.ImageIndex = 1;
                     treeNode.Nodes.Add(tNewNode);              
                }
                treeNode.ExpandAll();
            }
        }

        //初始化框架插件控件界面
        //根据sys配置文件添加菜单和工具栏
        public void InitialFrmDefControl()
        {
            ////得到Xml的System节点,根据XML加载插件界面
            string xPath = ".//System[@Name='" + this.Name + "']";
            Plugin.ModuleCommon.LoadButtonViewByXmlNode(ModFrameData.v_AppPrivileges.ControlContainer, xPath, ModFrameData.v_AppPrivileges);

            ////右键菜单
         //   _dicContextMenu = ModFrameData.v_AppPrivileges.DicContextMenu;
            //初始化地图浏览工具栏
            //Plugin.Application.IAppFormRef pAppFrm = ModFrameData.v_AppPrivileges as Plugin.Application.IAppFormRef;
            //XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar9']");
            //if (barXmlNode == null || _MapToolControl == null) return;
            ////DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null, false) as Bar;
            //DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            //if (mapToolBar != null)
            //{
            //    mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            //    mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            //    mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Left;
            //    mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.Office2003;
            //    mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            //}


            //初始化地图浏览工具栏
           Plugin.Application.IAppFormRef pAppFrm = ModFrameData.v_AppPrivileges as Plugin.Application.IAppFormRef;
            XmlNode barXmlNode = pAppFrm.SystemXml.SelectSingleNode(".//ToolBar[@Name='ControlMapToolBar4']");
            if (barXmlNode == null || _MapToolControl == null)
                return;
             DevComponents.DotNetBar.Bar mapToolBar = Plugin.ModuleCommon.LoadButtonView(_MapToolControl, barXmlNode, pAppFrm, null) as Bar;
            if (mapToolBar != null)
            {
                mapToolBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
                mapToolBar.DockOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
                mapToolBar.DockSide = DevComponents.DotNetBar.eDockSide.Left;
                mapToolBar.GrabHandleStyle = DevComponents.DotNetBar.eGrabHandleStyle.None;
                mapToolBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
                mapToolBar.RoundCorners = false;
                mapToolBar.SendToBack();
            }
        }



        //=================================
        //作者：席胜 
        //时间：2011-02-21
        //说明：左键点选树节点响应
        //=================================
        public void DataIndexTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //判断树节点级别
            if (DataIndexTree.SelectedNode != e.Node)
            {
                if (DataIndexTree.SelectedNode != null)
                {
                    DataIndexTree.SelectedNode.ForeColor = Color.Black;

                }

                DataIndexTree.SelectedNode = e.Node;
                e.Node.ForeColor = Color.Red;
                switch (DataIndexTree.SelectedNode.Tag.ToString())
                {
                    case "0":
                        DataIndexTree.SelectedNode.SelectedImageIndex = 0;
                        break;
                    case "1":
                        DataIndexTree.SelectedNode.SelectedImageIndex = 0;
                        break;
                    case "2":
                        DataIndexTree.SelectedNode.SelectedImageIndex = 2;
                        break;
                }
            }
            string  strItemName = "";
            string  strTblName = "";
            strItemName = e.Node.Text;
            
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            strTblName = dIndex.GetTblNameByItemName(strItemName);
            m_TableName = strTblName;
            //根据tblName获取对应的数据信息填充到列表中
            InitDataInfoList(strTblName);  

            //修改tab页面的显示名称
             this.tabItemData.Text = strItemName;
            
        }
        public bool getEditable()
        {
            TreeNode pSelectNode = DataIndexTree.SelectedNode;
            if (pSelectNode == null)
                return false;
            if (pSelectNode.Level == 2)
            {
                if (pSelectNode.Parent.Index == 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        //将ITable转换为DataTable
        private DataTable Transfer(ITable table)
        {
            DataTable dt = new DataTable();
            try
            {
                IQueryFilter queryFilter = new QueryFilterClass();
                ICursor pCursor = table.Search(queryFilter, true);
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    DataColumn dataColumn = null;
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                       
                        if (pRow.Fields.get_Field(i).AliasName != "OBJECTID")
                        {
                            dataColumn = dt.Columns.Add(pRow.Fields.get_Field(i).AliasName);
                            dataColumn.ReadOnly = true;
                        }
                        else
                        {
                            dt.Columns.Add(pRow.Fields.get_Field(i).AliasName);
                            //dataColumn.Unique = true;
                        }
                    }
                    while (pRow != null)
                    {
                        DataRow pDataRow = dt.NewRow();
                        for (int j = 0; j < pCursor.Fields.FieldCount; j++)
                        {
                            pDataRow[j] = pRow.get_Value(j);
                        }
                        dt.Rows.Add(pDataRow);
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("转换出错，" + ex.Message, "提示");
            }
            return dt;
        }

          //重载，added by xisheng 2011.06.16
        public void InitDataInfoList(string strTblName, bool boolreturn)
        {
            if (strTblName.Equals(""))
                return;
            IWorkspace pTmpWorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            if (pTmpWorkSpace == null)
            {
                return;
            }
            ModDBOperate.boolreturn = boolreturn;
            DataTable pDatatable = ModDBOperate.GetQueryTable(pTmpWorkSpace as IFeatureWorkspace, strTblName, "");
            m_dataTable = pDatatable;
            this.gridControl.DataSource = null;
            this.gridControl.DataSource = pDatatable;
        }

        //根据tblName获取对应的数据信息填充到listview中
        public void InitDataInfoList(string strTblName)
        {
            if (strTblName.Equals(""))
                return;
            IWorkspace pTmpWorkSpace = Plugin.ModuleCommon.TmpWorkSpace;
            if (pTmpWorkSpace == null)
            {
                return;
            }
           /* SysCommon.Gis.SysGisTable pSystable = new SysCommon.Gis.SysGisTable(pTmpWorkSpace);
            Exception eError = null;
            ITable pTable = pSystable.OpenTable(strTblName, out eError);
            if (pTable == null)
            {
                return;
            }
            DataTable pDatatable = new DataTable();
            DataRow pRow = pDatatable.NewRow();
            DataColumn pColumn = new DataColumn();

            this.gridControl.DataSource = null;
            this.gridControl.DataSource = pTable;*/
            DataTable pDatatable = ModDBOperate.GetQueryTable(pTmpWorkSpace as IFeatureWorkspace, strTblName, "");
            m_dataTable = pDatatable;
            this.gridControl.DataSource = null;
            this.gridControl.DataSource = pDatatable;
            
            //目前直接读取mdb文件，需要修改为配置模式
/*            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            //string  mypath = dIndex.GetDbValue("dbServerPath");
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            m_connstr = constr;
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select * from " + strTblName;  
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);     
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();
                DataTable dt = new DataTable();
                OleDbDataAdapter da = new OleDbDataAdapter(strExp, constr);
                m_Adapter = null;
                m_dataTable = null;
                m_Adapter = da;
                m_dataTable = dt;
                da.Fill(dt);
                this.gridControl.DataSource = null;
                this.gridControl.DataSource = dt;
                
                
                //关闭reader对象     
                aReader.Close();

                //关闭连接,这很重要     
                mycon.Close();    

            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            */

            //获取数据库中的内容


        }

        //单元格鼠标点击
        private void gridControl_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            //右键点击的时候弹出右键菜单
            //changed by chulili 2011-02-22 gridControl中的坐标(e.X,e.Y)是相对于本单元格的坐标，所以要进行转换
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex <= -1)
                    return;
  
                if (gridControl.Rows[e.RowIndex].Selected !=true)
                {
                    gridControl.ClearSelection();
                    if (e.RowIndex > -1)
                    {
                        gridControl.Rows[e.RowIndex].Selected = true;   //改变选中行到指定行
                        DataGridViewRow cr = gridControl.Rows[e.RowIndex];
                        if (e.ColumnIndex > -1)
                        {
                            gridControl.CurrentCell = cr.Cells[e.ColumnIndex];    //added by chulili 2011-02-25改变行首符号
                        }
                    }                    
                    
                }
                Point p = new Point();
                if (e.RowIndex > -1)
                    p.Y = e.Y + e.RowIndex * this.gridControl.Rows[0].Height + this.gridControl.ColumnHeadersHeight;
                else
                    p.Y = e.Y;

                if (e.ColumnIndex > -1)
                    p.X = e.X + e.ColumnIndex * this.gridControl.Columns[0].Width + this.gridControl.RowHeadersWidth;
                else
                    p.X = e.X;

                Point ClickPoint = this.gridControl.PointToScreen(p);

               //? contextMenuStrip.Show(ClickPoint);
            }
        }

        private void MenuItemAddRcd_Click(object sender, EventArgs e)
        {   //added by chulili 2011-02-24 添加记录功能

            TableForm myTableForm = new TableForm(gridControl, m_connstr, m_TableName);
            myTableForm.InitForm("ADD");
            myTableForm.ShowDialog();
            OleDbCommandBuilder builder = new OleDbCommandBuilder(m_Adapter);
            //将数据提交到数据库更新

            try
            {
                m_Adapter.Update(m_dataTable);
                //提交后重新获取数据，避免ID列的值与数据库中不一致
                m_dataTable.Clear();
                m_Adapter.Fill(m_dataTable);
                gridControl.DataSource = null;
                gridControl.DataSource = m_dataTable;
                //gridControl.Update();
            }
            catch (System.Exception m)
            {
                Console.WriteLine(m.Message);
            }

            

        }

        private void MenuItemModifyRcd_Click(object sender, EventArgs e)
        {   //added by chulili 2011-02-24 修改记录功能
            if (gridControl.SelectedRows.Count ==0)
            {
                MessageBox.Show("未选中记录！");
                return;
            }
            if (gridControl.SelectedRows.Count > 1)
            {
                MessageBox.Show("仅可修改一行！");
                return;
            }

            TableForm myTableForm = new TableForm(gridControl, m_connstr, m_TableName);
            myTableForm.InitForm("MODIFY");
            myTableForm.ShowDialog();

            InitDataInfoList(m_TableName);

            //OleDbCommandBuilder builder = new OleDbCommandBuilder(m_Adapter);
            //////将数据提交到数据库更新

            //try
            //{
            //    m_Adapter.Update(m_dataTable);
            //    //提交后重新获取数据，避免ID列的值与数据库中不一致
            //    m_dataTable.Clear();
            //    m_Adapter.Fill(m_dataTable);
            //    gridControl.DataSource = null;
            //    gridControl.DataSource = m_dataTable;
            //}
            //catch (System.Exception m)
            //{
            //    Console.WriteLine(m.Message);
            //}

                     

        }

        private void MenuItemDelRcd_Click(object sender, EventArgs e)
        {   //added by chulili 2011-02-24删除指定记录功能
            int k = gridControl.SelectedRows.Count;
            if (gridControl.SelectedRows.Count ==0)
            {
                MessageBox.Show("未选中记录！");
                return;
            }
            if (this.gridControl.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("确定要删除选中的记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            OleDbConnection mycon = new OleDbConnection(m_connstr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            mycon.Open();
            int i = 0, j = 0;
            if (gridControl.Rows.Count > 0)
            {
                for (j = k; j >= 1; j--)//从下往上删，避免沙漏效应
                {
                    strExp = "delete from  " + m_TableName + " where";
                    for (i = 0; i < gridControl.ColumnCount; i++)
                    {
                        if (gridControl.SelectedRows[j - 1].Cells[i].Value.ToString().Equals(""))
                            strExp = strExp + " (" + gridControl.Columns[i].Name + "='' or " + gridControl.Columns[i].Name + " is null) and";
                        else 
                            strExp = strExp + " " + gridControl.Columns[i].Name + "='" + gridControl.SelectedRows[j-1].Cells[i].Value.ToString() + "' and";
                    }
                    strExp = strExp.Substring(0, strExp.Length - 3);
                    aCommand.CommandText = strExp;
                    try
                    {
                        aCommand.ExecuteNonQuery();
                    }
                    catch (System.Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                 
                    
                }
            }
            else
            {
                gridControl.Rows.Clear();
            }
            mycon.Close();
            InitDataInfoList(m_TableName);
            //OleDbCommandBuilder builder = new OleDbCommandBuilder(m_Adapter);
            ////将数据提交到数据库更新

            //try
            //{
            //    m_Adapter.Update(m_dataTable);
            //    gridControl.Update();
            //}
            //catch (System.Exception m)
            //{
            //    Console.WriteLine(m.Message);
            //}

            
        }

        private void MenuItemDelAll_Click(object sender, EventArgs e)
        {   //added by chulili 2011-02-25删除全部记录功能
            if (MessageBox.Show("确定要全部删除吗？删除后不可恢复！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            OleDbConnection mycon = new OleDbConnection(m_connstr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "delete from " + m_TableName ;
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            mycon.Open();
            aCommand.ExecuteNonQuery();
            mycon.Close();
            InitDataInfoList(m_TableName);
            // int k = gridControl.Rows.Count;
            // if (gridControl.Rows.Count > 0)
            // {
            //     for (int i = k; i >= 1; i--)//从下往上删，避免沙漏效应
            //     {
            //         gridControl.Rows.RemoveAt(gridControl.Rows[i-1].Index);
                     
            //     }
            // }
            // else
            // {
            //     gridControl.Rows.Clear();
            // }
            ////将改动提交到数据库
            // OleDbCommandBuilder builder = new OleDbCommandBuilder(m_Adapter);
            // try
            // {
            //     m_Adapter.Update(m_dataTable);
            //     gridControl.Update();
            // }
            // catch (System.Exception m)
            // {
            //     Console.WriteLine(m.Message);
            // }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetailLog frm = new frmDetailLog(m_strLogFilePath);
            frm.Show();
        }
        /// <summary>
        /// 增加双击启动编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridControl.DataSource == null)
                return;
            //如果未选中记录。。。
            if (gridControl.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选中记录！");
                return;
            }
            //如果选中多行记录。。。
            if (gridControl.SelectedRows.Count > 1)
            {
                MessageBox.Show("仅可修改一行！");
                return;
            }
            int idx = gridControl.SelectedRows[0].Index;//yjl20111103 add
            //初始化修改记录对话框
            //初始化添加记录对话框
            TableForm myTableForm = new TableForm(gridControl, m_connstr, m_TableName);
            myTableForm.InitForm("MODIFY");
            myTableForm.ShowDialog();
            //记录修改后再次初始化dataview控件
            InitDataInfoList(m_TableName);
            ////ZQ   20111017   add  及时更新数据字典
            switch (m_TableName)
            {
                case "属性对照表":
                    SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
                    break;
                case "标准图层代码表":
                    SysCommon.ModField.InitLayerNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicLayerName);
                    break;
                //default:
                //    ///ZQ 20111020 add 增加重启提示
                //    MessageBox.Show("添加的记录只有应用系统重启以后才能生效！","提示！");
                //    break;
            }
            //yjl20111103 add 
            if (gridControl.Rows.Count > idx)
            {
                //取消默认的第1行选中
                gridControl.Rows[0].Selected = false;
                //设置选择行
                gridControl.Rows[idx].Selected = true;
                //设置当前行
                gridControl.CurrentCell = gridControl.Rows[idx].Cells[0];
            }
            /////

            Plugin.LogTable.Writelog("修改记录");//xisheng 2011.07.09 增加日志
        }

    }
}