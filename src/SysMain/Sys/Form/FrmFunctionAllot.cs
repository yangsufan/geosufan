using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Collections;
using ESRI.ArcGIS.esriSystem;
using System.Data.OracleClient;
using System.Data.OleDb;

namespace GeoDatabaseManager
{
    public partial class FrmFunctionAllot : DevComponents.DotNetBar.Office2007Form
    {
        Dictionary<string,int> dicUserGroup = new Dictionary<string,int>();//用户组信息（id和Name）
        XmlDocument docXml = new XmlDocument();
        XmlNode SysNode = null;
        private string pDbType = "";//数据库类型
       private string pConStr = "";//数据库连接字符串


        public FrmFunctionAllot()
        {
            InitializeComponent();
            GetConInfo();
            if(loadRoleTable()==false ) return ;
            this.comboBoxRole.SelectedIndex = 0;

            //读取系统权限XML
            if (!File.Exists(Mod.m_SysXmlPath)) return;
            docXml.Load(Mod.m_SysXmlPath);
            SysNode = docXml.SelectSingleNode(".//Root//Main//System");

            ReadXMl();
        }

        /// <summary>
        /// 获得数据库连接的参数
        /// </summary>
        private void GetConInfo()
        {
            StreamReader sr = new StreamReader(Mod.filestr);
            pDbType = sr.ReadLine();
            pConStr = sr.ReadLine();
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="pSysTable">要连接的数据库</param>
        /// <param name="DBtype">数据库类型</param>
        /// <param name="strConnectionString">数据库连接字符串</param>
       private void  SetDbCon(SysCommon.DataBase.SysTable pSysTable, string DBtype, string strConnectionString, out Exception eError)
        {
            eError = null;
            if (strConnectionString == "")
                return;
            try
            {
                if (DBtype.Trim() == "ACCESS")
                {
                    pSysTable.SetDbConnection(strConnectionString+";Mode=Share Deny None;Persist Security Info=False", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                }
                else if (DBtype.Trim() == "ORACLE")
                {
                    pSysTable.SetDbConnection(strConnectionString, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                }
                else if (DBtype.Trim() == "SQL")
                {
                    pSysTable.SetDbConnection(strConnectionString, SysCommon.enumDBConType.SQL, SysCommon.enumDBType.SQLSERVER, out eError);
                }
            }
            catch (Exception ex)
            {
                eError = ex;
                return;
            }
        }
        //加载角色权限表
        private bool loadRoleTable()
        {
            Exception eError = null;

             SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
             SetDbCon(pSysTable, pDbType ,pConStr, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("信息提示", eError.Message);
                return false ;
            }
            DataTable tempTable = pSysTable.GetTable("ice_UserGroupinfo", out eError);
            if (tempTable == null||tempTable.Rows.Count==0) return false ;
            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                int pUserGroupID = int.Parse(tempTable.Rows[i][0].ToString());//组ID
                string pUserGroupName = tempTable.Rows[i][1].ToString();//组名称
                dicUserGroup.Add(pUserGroupName,pUserGroupID);
                this.comboBoxRole.Items.Add(pUserGroupName);
            }
            return true;
        }

        private void ReadXMl()
        {
            ReadMainXMl(SysNode);
            ReadToolBarXML(SysNode);
            ReadContextMenuXML(SysNode);
        }
        /// <summary>
        /// 从XML中读取主菜单节点
        /// </summary>
        /// <param name="SysNode"></param>
        private void ReadMainXMl(XmlNode SysNode)
        {
            this.node1.Text = "主菜单";//node1显示的名称
            this.node1.Expanded = true;//node的扩展属性
            this.node1.CheckBoxVisible = false;//node1是否显示checkBox
            this.node1.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Hidden;
            this.node1.FullRowBackground = true;
            //this.advTreeMainPurview.Nodes.AddRange(new DevComponents.AdvTree.Node[] { this.node1 });//将node1添加到树上
            #region 通过读取xml文件动态的将主菜单节点添加到node1下面
            XmlNodeList mainFuncNodeList = SysNode.SelectNodes(".//RibbonTabItem");//获得第一级节点集合
            XmlElement nodeToElem = null;
            string pCaption = "";//Caption属性值
            string pName = "";//Name属性值

            #region 添加主菜单节点以及子节点
            //遍历xml文件中第一级子节点，并将相关信息附加到界面树节点上
            foreach (XmlNode oneFunc in mainFuncNodeList)
            {
                //添加第一级子节点
                //RibbonTabItem子节点
                nodeToElem = oneFunc as XmlElement;//将节点转化为元素类型，以便获取节点的属性
                pCaption = nodeToElem.GetAttribute("Caption").Trim();//节点的Caption属性
                pName = nodeToElem.GetAttribute("Name").Trim();//节点的Name属性
                DevComponents.AdvTree.Node nodeFunc = new DevComponents.AdvTree.Node();//声明一个node变量
                //给node变量的属性赋值
                nodeFunc.Text = pCaption + "," + pName;//将xml中读出来的属性赋给改node的Text属性
                nodeFunc.CheckBoxVisible = true;
                nodeFunc.Checked = false;
                nodeFunc.Expanded = true;
                nodeFunc.NodeClick += new System.EventHandler(TreeNodeClick);//声明node的nodeclick事件
                this.node1.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nodeFunc });//将改node附加在node1节点后面
                XmlNodeList nodeRibbonBar = oneFunc.SelectNodes(".//RibbonBar");//获得第二级节点集合
                //遍历xml文件中第二级子节点，并将相关信息附加到界面树节点上
                foreach (XmlNode oneNodeRibbon in nodeRibbonBar)
                {
                    //添加第二级子节点，同上
                    //RibbonBar子节点
                    nodeToElem = oneNodeRibbon as XmlElement;
                    pCaption = nodeToElem.GetAttribute("Caption").Trim();
                    pName = nodeToElem.GetAttribute("Name").Trim();
                    DevComponents.AdvTree.Node subNodeRibb = new DevComponents.AdvTree.Node();//声明一个node变量
                    //给node变量的属性赋值
                    subNodeRibb.Text = pCaption + "," + pName;
                    subNodeRibb.CheckBoxVisible = true;
                    subNodeRibb.Checked = false;
                    subNodeRibb.Expanded = true;
                    subNodeRibb.NodeClick += new System.EventHandler(TreeNodeClick);//nodeclick事件
                    nodeFunc.Nodes.AddRange(new DevComponents.AdvTree.Node[] { subNodeRibb });//将该节点附加在第一级节点后面
                    XmlNodeList nodeSubItemList = oneNodeRibbon.SelectNodes(".//SubItems");//从xml文件中获取第三级节点集合
                    //遍历xml文件中第三级子节点，并将相关信息附加到界面树节点上
                    foreach (XmlNode nodeSubItem in nodeSubItemList)
                    {
                        //添加第三级子节点
                        // ButtonItem子节点
                        //获得节点的属性值
                        nodeToElem = nodeSubItem as XmlElement;
                        pCaption = nodeToElem.GetAttribute("Caption").Trim();
                        pName = nodeToElem.GetAttribute("Name").Trim();
                        DevComponents.AdvTree.Node nodeButtonItem = new DevComponents.AdvTree.Node();//声明一个node节点
                        //给node变量的属性赋值
                        nodeButtonItem.Text = pCaption + "," + pName;
                        nodeButtonItem.CheckBoxVisible = true;
                        nodeButtonItem.Expanded = true;
                        nodeButtonItem.Checked = false;
                        nodeButtonItem.NodeClick += new System.EventHandler(TreeNodeClick);//nodeclick事件
                        //将改节点附加在第二级结点后面
                        subNodeRibb.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nodeButtonItem });//添加ButtonItem节点
                    }
                }
            }
            #endregion
            #endregion
        }
        /// <summary>
        /// 从XML中读取工具栏节点
        /// </summary>
        /// <param name="SysNode"></param>
        private void ReadToolBarXML(XmlNode SysNode)
        {
            this.node2.Text = "工具栏";
            this.node2.Expanded = true;
            this.node2.CheckBoxVisible = false;
            this.node2.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Hidden;
            this.node2.FullRowBackground = true;
            //this.advTreeToolbar.Nodes.AddRange(new DevComponents.AdvTree.Node[] { this.node2 });
            XmlNodeList toolBarNodeList = SysNode.SelectNodes(".//ToolBar");
            XmlElement nodeToElem = null;
            string pCaption = "";//Caption属性值
            string pName = "";//Name属性值

            #region 添加工具栏节点以及子节点
            foreach (XmlNode toolBarNode in toolBarNodeList)
            {//RibbonTabItem子节点
                nodeToElem = toolBarNode as XmlElement;
                pCaption = nodeToElem.GetAttribute("Caption").Trim();
                pName = nodeToElem.GetAttribute("Name").Trim();
                DevComponents.AdvTree.Node toolBarNd = new DevComponents.AdvTree.Node();
                toolBarNd.Text = pCaption + "," + pName;
                toolBarNd.CheckBoxVisible = true;
                toolBarNd.Expanded = true;
                toolBarNd.Checked = false;
                toolBarNd.NodeClick+=new System.EventHandler(TreeNodeClick);//定义nodeclick事件
                this.node2.Nodes.AddRange(new DevComponents.AdvTree.Node[] { toolBarNd });
                XmlNodeList nodeButtonList = toolBarNode.SelectNodes(".//SubItems");
                foreach (XmlNode nodeButton in nodeButtonList)
                {//RibbonBar子节点
                    nodeToElem = nodeButton as XmlElement;
                    pCaption = nodeToElem.GetAttribute("Caption").Trim();
                    pName = nodeToElem.GetAttribute("Name").Trim();
                    DevComponents.AdvTree.Node nodeBtn = new DevComponents.AdvTree.Node();//添加toolBar节点
                    nodeBtn.Text = pCaption + "," + pName;
                    nodeBtn.CheckBoxVisible = true;
                    nodeBtn.Expanded = true;
                    nodeBtn.Checked = false;
                    nodeBtn.NodeClick+=new System.EventHandler(TreeNodeClick);//定义nodeclick事件
                    toolBarNd.Nodes.AddRange(new DevComponents.AdvTree.Node[] { nodeBtn });//添加Button节点
                }
            }
            #endregion
        }
        /// <summary>
        /// 从XML中读取右键菜单节点
        /// </summary>
        /// <param name="SysNode"></param>
        private void ReadContextMenuXML(XmlNode SysNode)
        {
            this.node3.Text = "右键菜单";
            this.node3.Expanded = true;
            this.node3.CheckBoxVisible = false;
            this.node3.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Hidden;
            this.node3.FullRowBackground = true;
            //this.advTreeContextMenuBar.Nodes.AddRange(new DevComponents.AdvTree.Node[] { this.node3 });
            XmlNodeList contextMenuNodeList = SysNode.SelectNodes(".//ContextMenuBar");
            XmlElement nodeToElem = null;
            string pCaption = "";//Caption属性值
            string pName = "";//Name属性值

            #region 添加右键菜单节点以及子节点
            foreach (XmlNode contextMenuNode in contextMenuNodeList)
            {//contextMenuBar子节点
                nodeToElem = contextMenuNode as XmlElement;
                pCaption = nodeToElem.GetAttribute("Caption").Trim();
                pName = nodeToElem.GetAttribute("Name").Trim();
                DevComponents.AdvTree.Node menuBarnode = new DevComponents.AdvTree.Node();
                menuBarnode.Text = pCaption + "," + pName;
                menuBarnode.CheckBoxVisible = true;
                menuBarnode.Expanded = true;
                menuBarnode.Checked = false;
                menuBarnode.NodeClick += new System.EventHandler(TreeNodeClick);//nodeclick事件
                this.node3.Nodes.AddRange(new DevComponents.AdvTree.Node[] { menuBarnode });
                XmlNodeList nodeButtonList = contextMenuNode.SelectNodes(".//SubItems");//添加contextMenuBar节点
                foreach (XmlNode oneNodeButton in nodeButtonList)
                {//ButtonItem子节点
                    nodeToElem = oneNodeButton as XmlElement;
                    pCaption = nodeToElem.GetAttribute("Caption").Trim();
                    pName = nodeToElem.GetAttribute("Name").Trim();
                    DevComponents.AdvTree.Node subNodeBtn = new DevComponents.AdvTree.Node();
                    subNodeBtn.Text = pCaption + "," + pName;
                    subNodeBtn.CheckBoxVisible = true;
                    subNodeBtn.Expanded = true;
                    subNodeBtn.Checked = false;
                    subNodeBtn.NodeClick += new System.EventHandler(TreeNodeClick);//nodeclick事件
                    menuBarnode.Nodes.AddRange(new DevComponents.AdvTree.Node[] { subNodeBtn });//添加ButtonItem节点
                }
            }
            #endregion
        }

        /// <summary>
        /// 点击node节点，设置check属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeNodeClick(object sender, EventArgs e)
        {
            DevComponents.AdvTree.Node pNode = sender as DevComponents.AdvTree.Node;//获得点击的节点
            pNode.Checked = !pNode.Checked;//将节点的选中状态置反
            DevComponents.AdvTree.Node nodeParent = new DevComponents.AdvTree.Node();//声明父节点变量
            DevComponents.AdvTree.NodeCollection  nodeChilds = new DevComponents.AdvTree.NodeCollection();//声明子节点集合变量
            if (pNode.Checked == true)
            {
                #region 若子节点被选中，则其所在的父节点均被选中
                nodeParent = pNode.Parent;
                while (nodeParent != null && nodeParent.CheckBoxVisible == true)
                {
                    if (nodeParent.Checked == false)
                    {
                        nodeParent.Checked = true;
                    }
                    nodeParent = nodeParent.Parent;
                }
                #endregion
                #region 若父节点被选中,则所有的子节点均被选中
                setNodeChildVisble(pNode, true);
                #endregion
            }
            else if (pNode.Checked == false)
            {
                //若父节点为false,则其所有的子节点均为false;
                setNodeChildVisble(pNode, false);
            }
        }
        /// <summary>
        /// 根据父节点的visible属性，设置子节点的visible属性
        /// </summary>
        /// <param name="pNode">父节点</param>
        /// <param name="visible">父节点visible属性值</param>
        private void setNodeChildVisble(DevComponents.AdvTree.Node pNode,bool visible)
        {
            DevComponents.AdvTree.NodeCollection nodeChilds = new DevComponents.AdvTree.NodeCollection();//声明子节点集合变量
            nodeChilds = pNode.Nodes;
            //含有子节点时,将子节点全部选中
            if (nodeChilds.Count > 0)
            {
                for (int i = 0; i < nodeChilds.Count; i++)
                {
                    DevComponents.AdvTree.Node nodeChild = nodeChilds[i];
                    nodeChild.Checked = visible;
                    setNodeChildVisble(nodeChild, visible);
                }
            }
        }
        /// <summary>
        /// 根据advTree的节点的visible属性值，修改XML的节点的visible属性和Enable属性---针对主菜单
        /// </summary>
        private void UpdateXmlBaseOnMainTree()
        {
            //设置RibbonTab的visible和enable属性
            DevComponents.AdvTree.NodeCollection pNodeColl = this.node1.Nodes;//获得“主菜单”界面树节点node1的子节点集合
            for (int i = 0; i < pNodeColl.Count; i++)
            {
                DevComponents.AdvTree.Node pNode = pNodeColl[i];//获得主菜单界面上树节点的第一级子节点
                int index = pNode.Text.IndexOf(",");//获得“主菜单”界面树节点第一级子节点的名称所包含的逗号的索引值，以便获得该节点的名称
                string pName = pNode.Text.Substring(index + 1);//通过逗号的索引值获得界面树节点的名称Name，以便用来跟xml读取的节点比较
                XmlNodeList RibbonTabNodeList = SysNode.SelectNodes(".//RibbonTabItem");//获得xml文件中主菜单节点的集合
                //遍历xml文件中主菜单子节点
                foreach (XmlNode RibbonTabNode in RibbonTabNodeList)
                {
                    XmlElement RibbonTabElem = RibbonTabNode as XmlElement;//将节点类型进行转换，以便获得节点的属性值
                    string mName = RibbonTabElem.GetAttribute("Name").Trim();//获得节点的Name属性值
                    if (pName == mName)//将界面树中的节点的Name和xml中节点的Name属性值进行比较,找到与界面树中的节点匹配的xml文件中的对应节点
                    {
                        //根据界面树中节点的勾选情况，设置xml文件中对应RibbonTab节点的RibbonBar的visible和enable属性
                        if (pNode.Checked == true)
                        {
                            RibbonTabElem.SetAttribute("Enabled", "true");
                            RibbonTabElem.SetAttribute("Visible", "true");
                        }
                        else if (pNode.Checked == false)
                        {
                            RibbonTabElem.SetAttribute("Enabled", "false");
                            RibbonTabElem.SetAttribute("Visible", "false");
                            #region 若父节点的visible和enable属性为false,则下面的所有的子节点的visible和enable属性也为false
                            XmlNodeList fNodeList = RibbonTabNode.SelectNodes(".//RibbonBar");//获得xml文件中RibbonBar节点的子节点集合
                            //遍历子节点，并将所有子节点的visible和enable属性设置为false
                            foreach (XmlNode fNode in fNodeList)
                            {
                                XmlElement fElme = fNode as XmlElement;
                                fElme.SetAttribute("Enabled", "false");
                                fElme.SetAttribute("Visible", "false");
                                XmlNodeList ffNodeList = fNode.SelectNodes(".//SubItems");//获得xml文件中SubItems节点的子节点集合
                                //遍历子节点，并将所有子节点的visible和enable属性设置为false
                                foreach (XmlNode ffNode in ffNodeList)
                                {
                                    XmlElement ffElem = ffNode as XmlElement;
                                    ffElem.SetAttribute("Enabled", "false");
                                    ffElem.SetAttribute("Visible", "false");
                                }
                            }
                            break;
                            #endregion
                        }
                        #region 设置RibbonBar的visible和enable属性
                        DevComponents.AdvTree.NodeCollection ppNodeColl = pNode.Nodes;//获得“主菜单”界面树节点第二级子节点集合
                        //遍历第二级子节点
                        for (int j = 0; j < ppNodeColl.Count; j++)
                        {
                            DevComponents.AdvTree.Node ppNode = ppNodeColl[j];
                            int pIndex = ppNode.Text.IndexOf(",");//同理获得该节点的逗号索引
                            string ppName = ppNode.Text.Substring(pIndex + 1);//通过索引获得节点的名称
                            XmlNodeList RibbonBarNodeList = RibbonTabNode.SelectNodes(".//RibbonBar");//读取XML文件，获得相应第二级节点的节点集合
                            //遍历xml文件中的第二级节点
                            foreach (XmlNode RibbonBar in RibbonBarNodeList)
                            {
                                XmlElement RibbonBarElem = RibbonBar as XmlElement;
                                string mmName = RibbonBarElem.GetAttribute("Name").Trim();//获得二级节点的Name属性
                                //通过名称找到xml里面相应的节点
                                if (ppName == mmName)
                                {
                                    //根据界面上的勾选情况，设置xml文件中节点的属性值
                                    if (ppNode.Checked == true)
                                    {
                                        RibbonBarElem.SetAttribute("Enabled", "true");
                                        RibbonBarElem.SetAttribute("Visible", "true");
                                    }
                                    else if (ppNode.Checked == false)
                                    {
                                        RibbonBarElem.SetAttribute("Enabled", "false");
                                        RibbonBarElem.SetAttribute("Visible", "false");
                                        #region 若父节点的visible和enable属性为false,则下面的所有的子节点的visible和enable属性也为false
                                       //读xml文件并将其子节点的属性设置未false
                                        XmlNodeList fffNodeList = RibbonBar.SelectNodes(".//SubItems");
                                        //设置ButtonItem的visible和enable属性
                                        foreach (XmlNode fffNode in fffNodeList)
                                        {
                                            XmlElement fffElem = fffNode as XmlElement;
                                            fffElem.SetAttribute("Enabled", "false");
                                            fffElem.SetAttribute("Visible", "false");
                                        }
                                        break;
                                        #endregion
                                    }
                                    #region 设置ButtonItem的visible和enable属性
                                    DevComponents.AdvTree.NodeCollection pppNodeColl = ppNode.Nodes;//获得“主菜单”界面树节点第三级子节点集合
                                    //遍历界面树中第三级子节点
                                    for (int k = 0; k < pppNodeColl.Count; k++)
                                    {
                                        DevComponents.AdvTree.Node pppNode = pppNodeColl[k];
                                        int ppIndex = pppNode.Text .IndexOf(",");
                                        string pppName = pppNode.Text.Substring(ppIndex + 1);//获得界面上子节点的名称
                                        XmlNodeList ButtonNodeList = RibbonBar.SelectNodes(".//SubItems");//读取xml文件中第三级子节点集合
                                        foreach (XmlNode ButtonNode in ButtonNodeList)
                                        {
                                            XmlElement ButtonElem = ButtonNode as XmlElement;
                                            string mmmName = ButtonElem.GetAttribute("Name").Trim();//获得xml文件中节点的属性值
                                            //通过比较界面上的名称和xml文件汇总节点的名称，在xml文件中找到与界面上节点对应节点，
                                            //并根据界面的勾选情况，设置xml文件中节点的属性值
                                            if (pppName == mmmName)
                                            {
                                                //设置SubItems节点的属性
                                                if (pppNode.Checked == true)
                                                {
                                                    ButtonElem.SetAttribute("Enabled", "true");
                                                    ButtonElem.SetAttribute("Visible", "true");
                                                }
                                                else if (pppNode.Checked == false)
                                                {
                                                    ButtonElem.SetAttribute("Enabled", "false");
                                                    ButtonElem.SetAttribute("Visible", "false");
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    #endregion
                                    break;
                                }
                                
                            }
                        }
                        #endregion
                        break;
                    }
                }
               
            }
        }
        /// <summary>
        /// 根据advTree的节点的visible属性值，修改XML的节点的visible属性和Enable属性---针对工具栏
        /// </summary>
        private void UpdateXmlBaseOnToolBarTree()
        {
            //设置ToolBar的visible和enable属性
            DevComponents.AdvTree.NodeCollection pNodeColl = this.node2.Nodes;//node1的所有子节点
            for (int i = 0; i < pNodeColl.Count; i++)
            {
                DevComponents.AdvTree.Node pNode = pNodeColl[i];
                int index = pNode.Text .IndexOf(",");//树节点上逗号的索引
                string pName = pNode.Text.Substring(index + 1);//树节点的Name
                XmlNodeList ToolBarNodeList = SysNode.SelectNodes(".//ToolBar");//获得ToolBar节点集合
                foreach (XmlNode ToolBarNode in ToolBarNodeList)
                {//设置ToolBar的属性
                    XmlElement ToolBarElem = ToolBarNode as XmlElement;//将ToolBar节点转换为ToolBar元素
                    string mName = ToolBarElem.GetAttribute("Name").Trim();//通过ToolBar元素获得ToolBar的Name属性值
                    if (pName == mName)//如果界面上树节点的名称Name和ToolBar的Name属性值一样，则给该ToolBar节点的其他属性赋值
                    {
                        if (pNode.Checked == true)
                        {
                            ToolBarElem.SetAttribute("Enabled", "true");
                            ToolBarElem.SetAttribute("Visible", "true");
                        }
                        else if (pNode.Checked == false)
                        {
                            ToolBarElem.SetAttribute("Enabled", "false");
                            ToolBarElem.SetAttribute("Visible", "false");
                            #region 若父节点ToolBar的visible和enable属性为false,则下面的所有的子节点的visible和enable属性也为false
                            XmlNodeList fNodeList = ToolBarNode.SelectNodes(".//SubItems");
                            //设置SubItems的visible和enable属性
                            foreach (XmlNode fNode in fNodeList)
                            {
                                XmlElement fElme = fNode as XmlElement;
                                fElme.SetAttribute("Enabled", "false");
                                fElme.SetAttribute("Visible", "false");
                            }
                            break;
                            #endregion
                        }
                        #region 设置SubItems的visible和enable属性
                        DevComponents.AdvTree.NodeCollection ppNodeColl = pNode.Nodes;//pNode的所有子节点
                        for (int j = 0; j < ppNodeColl.Count; j++)
                        {
                            DevComponents.AdvTree.Node ppNode = ppNodeColl[j];
                            int pIndex = ppNode.Text.IndexOf(",");
                            string ppName = ppNode.Text.Substring(pIndex + 1);
                            XmlNodeList ButtonNodeList = ToolBarNode.SelectNodes(".//SubItems");
                            foreach (XmlNode ButtonNode in ButtonNodeList)
                            {
                                XmlElement ButtonElem = ButtonNode as XmlElement;
                                string mmName = ButtonElem.GetAttribute("Name").Trim();
                                //通过名称找到xml里面相应的节点
                                if (ppName == mmName)
                                {
                                    //设置SubItems节点的属性
                                    if (ppNode.Checked == true)
                                    {
                                        ButtonElem.SetAttribute("Enabled", "true");
                                        ButtonElem.SetAttribute("Visible", "true");
                                    }
                                    else if (ppNode.Checked == false)
                                    {
                                        ButtonElem.SetAttribute("Enabled", "false");
                                        ButtonElem.SetAttribute("Visible", "false");
                                    }
                                    break;
                                }
                            }
                        }
                        #endregion
                        break;
                    }
                }

            }
        }
        /// <summary>
        /// 根据advTree的节点的visible属性值，修改XML的节点的visible属性和Enable属性---针对右键菜单
        /// </summary>
        private void UpdateXmlBaseOnContextMenuTree()
        {
            //设置ContextMenu的visible和enable属性
            DevComponents.AdvTree.NodeCollection pNodeColl = this.node3.Nodes;//node1的所有子节点
            for (int i = 0; i < pNodeColl.Count; i++)
            {
                DevComponents.AdvTree.Node pNode = pNodeColl[i];
                int index = pNode.Text .IndexOf(",");//获得树节点上逗号的索引
                string pName = pNode.Text .Substring(index + 1);//获得树节点的Name
                XmlNodeList ContextMenuNodeList = SysNode.SelectNodes(".//ContextMenuBar");//获得ContextMenuBar节点链
                foreach (XmlNode ContextMenuNode in ContextMenuNodeList)
                {//设置ToolBar的属性
                    XmlElement ContextMenuElem = ContextMenuNode as XmlElement;//将ContextMenu节点转换为ContextMenu元素
                    string mName = ContextMenuElem.GetAttribute("Name").Trim();//通过ContextMenu元素获得ContextMenu的Name属性值
                    if (pName == mName)//如果界面上树节点的名称Name和ContextMenu的Name属性值一样，则给该ContextMenu节点的其他属性赋值
                    {
                        if (pNode.Checked == true)
                        {
                            ContextMenuElem.SetAttribute("Enabled", "true");
                            ContextMenuElem.SetAttribute("Visible", "true");
                        }
                        else if (pNode.Checked == false)
                        {
                            ContextMenuElem.SetAttribute("Enabled", "false");
                            ContextMenuElem.SetAttribute("Visible", "false");
                            #region 若父节点ContextMenuElem的visible和enable属性为false,则下面的所有的子节点的visible和enable属性也为false
                            XmlNodeList fNodeList = ContextMenuNode.SelectNodes(".//SubItems");
                            //设置SubItems的visible和enable属性
                            foreach (XmlNode fNode in fNodeList)
                            {
                                XmlElement fElme = fNode as XmlElement;
                                fElme.SetAttribute("Enabled", "false");
                                fElme.SetAttribute("Visible", "false");
                            }
                            break;
                            #endregion
                        }
                        #region 设置SubItems的visible和enable属性
                        DevComponents.AdvTree.NodeCollection ppNodeColl = pNode.Nodes;//pNode的所有子节点
                        for (int j = 0; j < ppNodeColl.Count; j++)
                        {
                            DevComponents.AdvTree.Node ppNode = ppNodeColl[j];
                            int pIndex = ppNode.Text .IndexOf(",");
                            string ppName = ppNode.Text .Substring(pIndex + 1);
                            XmlNodeList ButtonNodeList = ContextMenuNode.SelectNodes(".//SubItems");
                            foreach (XmlNode ButtonNode in ButtonNodeList)
                            {
                                XmlElement ButtonElem = ButtonNode as XmlElement;
                                string mmName = ButtonElem.GetAttribute("Name").Trim();
                                //通过名称找到xml里面相应的节点
                                if (ppName == mmName)
                                {
                                    //设置SubItems节点的属性
                                    if (ppNode.Checked == true)
                                    {
                                        ButtonElem.SetAttribute("Enabled", "true");
                                        ButtonElem.SetAttribute("Visible", "true");
                                    }
                                    else if (ppNode.Checked == false)
                                    {
                                        ButtonElem.SetAttribute("Enabled", "false");
                                        ButtonElem.SetAttribute("Visible", "false");
                                    }
                                    break;
                                }
                            }
                        }
                        #endregion
                        break;
                    }
                }

            }
        }

        //保存设置
        private void btnSave_Click(object sender, EventArgs e)
        {
            UpdateXmlBaseOnMainTree();
            UpdateXmlBaseOnToolBarTree();
            UpdateXmlBaseOnContextMenuTree();

            string roleName = this.comboBoxRole.Text.Trim();//用户组名称
            int roleID = dicUserGroup[roleName];//用户组ID

            #region 访问数据库，将BLOB字段插入到数据库中
            byte[] XmlBt = System.Text.Encoding.Default.GetBytes(docXml.InnerXml);//把xmlDocument转化为二进制流
            string updateStr = "update ice_UserGroupinfo set G_purview=:G_purview where G_id =" + roleID;
            if (pDbType == "ORACLE")
            {
                OracleConnection pConn = new OracleConnection(pConStr);
                try
                {
                    pConn.Open();

                    #region 将记录插入角色权限表中
                    OracleCommand OracleCmd = new OracleCommand(updateStr, pConn);
                    //OracleCmd.CommandType = CommandType.Text;
                    OracleCmd.CommandText = updateStr;//正常SQL语句插入数据库

                    OracleCmd.Parameters.Add("G_purview", System.Data.OracleClient.OracleType.Blob, XmlBt.Length);
                    OracleCmd.Parameters[0].Value = XmlBt;

                    OracleCmd.ExecuteNonQuery();
                    #endregion
                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
                    return;
                }
            }
            if (pDbType == "ACCESS")
            {
                OleDbConnection pOleConn = new OleDbConnection(pConStr);
                try
                {
                    pOleConn.Open();
                    #region 将角色信息插入用户组权限表中
                    OleDbCommand oledbCmd = new OleDbCommand(updateStr, pOleConn);
                    //oledbCmd.CommandText = updateStr;//正常SQL语句插入数据库
                    oledbCmd.Parameters.Add("G_purview", System.Data.OleDb.OleDbType.Binary, XmlBt.Length);
                    oledbCmd.Parameters[0].Value = XmlBt;
                    oledbCmd.ExecuteNonQuery();
                    #endregion
                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
                    return;
                }
            }
            #endregion

            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功");

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Application.Exit();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Application.Exit();
            this.Close();
        }
    }
}