using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using System.Data.OleDb;
using System.Xml;
using System.IO;

namespace GeoDBConfigFrame
{
    public partial class SubIndexScript : DevComponents.DotNetBar.Office2007Form
    {
        public string m_mypath = null;
        public string m_xmlPath;
        public XmlDocument m_xmldoc;
        public TreeNode m_tMapNode;
        private TreeNode m_LastDragNode = null;
        string m_Typecode = "";
        public SubIndexScript()
        {
            InitializeComponent();

            //初始化listview列表
            InitListView();


        }

        //private void btnOk_Click(object sender, EventArgs e)
        //{
            
        //    this.DialogResult = DialogResult.OK;
        //    this.Hide();
        //    this.Dispose(true);
        //}

        private void butnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
            this.Dispose(true);
        }

        public void InitListView()
        {

            listViewControl.View = View.Details;

            //从 数据单元表 中获取信息
            ListViewItem lItem;
            ListViewItem.ListViewSubItem lSubItem;
            ListViewItem.ListViewSubItem lSubItemSecond;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            m_mypath = dIndex.GetDbInfo();
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select 专题类型,描述,脚本文件 from 标准专题信息表";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();
                while (aReader.Read())
                {
                    lItem = new ListViewItem();
                    lItem.Text = aReader["专题类型"].ToString();

                    lSubItem = new ListViewItem.ListViewSubItem();
                    lSubItem.Text = aReader["描述"].ToString();
                    lItem.SubItems.Add(lSubItem);

                    lSubItemSecond = new ListViewItem.ListViewSubItem();
                    lSubItemSecond.Text = aReader["脚本文件"].ToString();
                    lItem.SubItems.Add(lSubItemSecond);

                    listViewControl.Items.Add(lItem);
                }
                //关闭reader对象     
                aReader.Close();

                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        private void listViewControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //listview节点发生变换 加载更新树视图
        private void listViewControl_MouseClick(object sender, MouseEventArgs e)
        {

            //节点发生变化
            ListView lvControl = (ListView)sender;
            if (lvControl.SelectedItems.Count == 0) return;
            m_Typecode = lvControl.SelectedItems[0].Text;
            string strIndexName = lvControl.SelectedItems[0].SubItems[2].Text;
            LoadTreeView(strIndexName);//加载树列表
        }

        private void LoadTreeView(string strIndexName)
        {
            try
            {
                //清空树控件
                treeViewControl.Nodes.Clear();

                //获取搅拌文件初始化树视图
                string strModFile = Application.StartupPath + "\\..\\Template\\" + strIndexName;

                //判断文件是否存在 
                if (File.Exists(strModFile))
                {
                    //加载xml文件
                    m_xmldoc = new XmlDocument();
                    m_xmldoc.Load(strModFile);
                    m_xmlPath = strModFile;

                    //根节点
                    TreeNode tparent;
                    tparent = new TreeNode();
                    tparent.Text = "地图文档";
                    tparent.Tag = 0;
                    treeViewControl.Nodes.Add(tparent);
                    treeViewControl.ExpandAll();
                    tparent.ImageIndex = 0;
                    tparent.SelectedImageIndex = 0;


                    TreeNode tNewNode;
                    string strTblName = "";
                    string strRootName = "";
                    string strSearchRoot = "//GisMap";
                    XmlNode xmlNodeRoot = m_xmldoc.SelectSingleNode(strSearchRoot);
                    XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                    strRootName = xmlElentRoot.GetAttribute("sItemName");

                    m_tMapNode = new TreeNode();
                    m_tMapNode.Text = strRootName;
                    m_tMapNode.Tag = 1;
                    tparent.Nodes.Add(m_tMapNode);
                    tparent.ExpandAll();
                    m_tMapNode.ImageIndex = 1;
                    m_tMapNode.SelectedImageIndex = 1;

                    //首先添加第一级子节点 SubGroup
                    string strSearch = "//SubGroup";
                    XmlNodeList xmlNdList = m_xmldoc.SelectNodes(strSearch);
                    foreach (XmlNode xmlChild in xmlNdList)
                    {
                        strTblName = "";
                        XmlElement xmlElent = (XmlElement)xmlChild;
                        strTblName = xmlElent.GetAttribute("sItemName");
                        tNewNode = new TreeNode();
                        tNewNode.Text = strTblName;
                        tNewNode.Tag = 2;
                        m_tMapNode.Nodes.Add(tNewNode);
                        m_tMapNode.ExpandAll();
                        tNewNode.ImageIndex = 2;
                        tNewNode.SelectedImageIndex = 2;

                        //添加最终子节点
                        AddLeafItem(tNewNode, xmlChild);
                    }

                    /*  //整个加载xml文件
                        treeViewControl.Nodes.Add(new TreeNode(xDoc.DocumentElement.Name));
                        TreeNode tNode = new TreeNode();
                        tNode = (TreeNode)treeViewControl.Nodes[0];  
                        addTreeNode(xDoc.DocumentElement, tNode);*/

                    treeViewControl.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        //添加叶子节点
        public void AddLeafItem(TreeNode treeNode, XmlNode xmlNode)
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string layer = "";
            strExp = "select 关键图层 from 标准专题信息表 where 专题类型='" + m_Typecode + "'";
            layer = db.GetInfoFromMdbByExp(constr, strExp);
            strExp = "select 描述 from 标准图层信息表 where 代码='" + layer + "'";
            layer = db.GetInfoFromMdbByExp(constr, strExp);

            if (treeNode != null && xmlNode != null)
            {
                TreeNode tNewNode;
                string strLayerDescribed = ""; //图层名称 地类图斑等
                string strFileName = "";
                XmlNodeList xmlNdList;
                xmlNdList = xmlNode.ChildNodes;
                foreach (XmlNode xmlChild in xmlNdList)
                {
                    strLayerDescribed = "";
                    XmlElement xmlElent = (XmlElement)xmlChild;
                    strLayerDescribed = xmlElent.GetAttribute("sDemo");  //描述
                    strFileName = xmlElent.GetAttribute("sItemName");    //名称

                    //修改sfile名称
                    tNewNode = new TreeNode();
                    tNewNode.Text = strLayerDescribed;
                    tNewNode.Name = strFileName;
                    tNewNode.Tag = 3;
                    treeNode.Nodes.Add(tNewNode);
                    tNewNode.ImageIndex = 3;
                    tNewNode.SelectedImageIndex = 3;
                    if (strFileName.Trim().CompareTo(layer.Trim()) == 0)
                        tNewNode.ForeColor = Color.Red;
                }
                treeNode.ExpandAll();
            }
        }

        //递归添加节点
        private void addTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList xNodeList;
            if (xmlNode.HasChildNodes)
            {
                xNodeList = xmlNode.ChildNodes;
                for (int x = 0; x <= xNodeList.Count - 1; x++)
                {
                    xNode = xmlNode.ChildNodes[x];
                    treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = treeNode.Nodes[x];
                    addTreeNode(xNode, tNode);
                }
            }
            else
                treeNode.Text = xmlNode.OuterXml.Trim();
        }

        private void treeViewControl_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeViewControl.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag.Equals(0))//根节点弹出单独菜单
                {
                    System.Drawing.Point ClickPoint = treeViewControl.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                    contextMenuSub.Show(ClickPoint);
                }
                else
                {

                    System.Drawing.Point ClickPoint = treeViewControl.PointToScreen(new System.Drawing.Point(e.X, e.Y));
                    contextMenuTree.Show(ClickPoint);
                    if (e.Node.Tag.Equals(3))//如果是是叶子节点
                    {
                        MenuItemEditLayer.Visible = true;
                        MenuItemMainLayer.Visible = true;
                        MenuItemCanceMainLayer.Visible = true;
                        toolStripSeparator2.Visible = true;
                        if (!e.Node.ForeColor.Equals(Color.Red))
                            MenuItemCanceMainLayer.Enabled = false;
                    }
                    else
                    {
                        MenuItemEditLayer.Visible = false;
                        MenuItemMainLayer.Visible = false;
                        MenuItemCanceMainLayer.Visible = false;
                        toolStripSeparator2.Visible = false;
                    }
                }
            }
        }

        //添加专题
        private void MenuItemAddSub_Click(object sender, EventArgs e)
        {
            SubAttForm dlg = new SubAttForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            { 
                string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
                string strExp = "select count(*) from 标准专题信息表 where 专题类型='"+dlg.strSubCode+"'";
                GeoDataCenterDbFun db=new GeoDataCenterDbFun();
                int count=db.GetCountFromMdb(constr,strExp);
                if (count > 0)
                {
                    MessageBox.Show("专题已存在，请修改专题类型！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //向当前listview添加一条记录
                ListViewItem lvItem;
                ListViewItem.ListViewSubItem lSubItem;
                ListViewItem.ListViewSubItem lSubItemSecond;
                lvItem = new ListViewItem();
                lvItem.Text = dlg.strSubCode;

                lSubItem = new ListViewItem.ListViewSubItem();
                lSubItem.Text = dlg.strSubName;
                lvItem.SubItems.Add(lSubItem);

                lSubItemSecond = new ListViewItem.ListViewSubItem();
                lSubItemSecond.Text = dlg.strIndexFile;
                lvItem.SubItems.Add(lSubItemSecond);

                listViewControl.Items.Add(lvItem);
                listViewControl.Refresh();

                //获取数值添加到“标准专题信息表”中
               
                strExp = "insert into 标准专题信息表 (专题类型,描述,脚本文件,配图方案文件) values('" + dlg.strSubCode + "','" + dlg.strSubName + "','" + dlg.strIndexFile + "','" + dlg.strMapSymIndexFile  + "')";
                OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
                try
                {
                    mycon.Open();

                    //插入记录    
                    int iRows = aCommand.ExecuteNonQuery();

                    //关闭连接,这很重要     
                    mycon.Close();
                }
                catch (System.Exception err)
                {
                    Console.WriteLine(err.Message);
                }

                //在\Template目录下生成对应文件
                string strModFile = Application.StartupPath + "\\..\\Template\\StandardBlank.xml";
                string strIndexFile = Application.StartupPath + "\\..\\Template\\" + dlg.strIndexFile;
                if (!File.Exists(strIndexFile))
                {
                    File.Copy(strModFile, strIndexFile, true);
                }

                //加载文件并修改GisMap ItemName=""
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(strIndexFile);
                string strSearchRoot = "//GisMap";
                XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
                XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                xmlElentRoot.SetAttribute("sItemName", dlg.strSubName);
                xmldoc.Save(strIndexFile);
                m_Typecode = dlg.strSubCode;
                LoadTreeView(dlg.strIndexFile);
            }
        }

        //修改专题
        private void MenuItemModifySub_Click(object sender, EventArgs e)
        {
            if (listViewControl.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择要修改的专题!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //获取数据库中信息 更改
            //获取数值添加到“标准专题信息表”中
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            //获取节点名称
            string strSubOldName = null;
            string strSubOldCode = null;
            string strSubOldIndexFile = null;
            string strSubOldMapFile = null;

            SubAttForm dlg = new SubAttForm();
            dlg.strSubCode = listViewControl.SelectedItems[0].Text;
            strSubOldCode = listViewControl.SelectedItems[0].Text;
            dlg.strSubName = listViewControl.SelectedItems[0].SubItems[1].Text;
            strSubOldName = listViewControl.SelectedItems[0].SubItems[1].Text;
            dlg.strIndexFile = listViewControl.SelectedItems[0].SubItems[2].Text;
            strSubOldIndexFile = listViewControl.SelectedItems[0].SubItems[2].Text;
            strExp="select 配图方案文件 from 标准专题信息表 where " + "专题类型 = '" + strSubOldCode + "' ";
            strSubOldMapFile= dlg.strMapSymIndexFile = db.GetInfoFromMdbByExp(constr, strExp);
            dlg.SetFormTextBoxAtt();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //没发生变化
                if (strSubOldCode.Equals(dlg.strSubCode) && strSubOldName.Equals(dlg.strSubName) && strSubOldIndexFile.Equals(dlg.strIndexFile)&&strSubOldMapFile.Equals(dlg.strMapSymIndexFile))
                    return;

                //修改listview
                listViewControl.SelectedItems[0].Text = dlg.strSubCode;
                listViewControl.SelectedItems[0].SubItems[1].Text = dlg.strSubName;
                listViewControl.SelectedItems[0].SubItems[2].Text = dlg.strIndexFile;
                listViewControl.Refresh();
            }

            
         //   strExp = "update 标准专题信息表 set 专题类型 = '" + dlg.strSubCode + "'," + "描述 = '" + dlg.strSubName + "' where " + "专题类型 = '" + strSubOldCode + "'";
            strExp = "update 标准专题信息表 set 专题类型 = '" + dlg.strSubCode + "'," + "描述 = '" + dlg.strSubName + "'," + "脚本文件 = '" + dlg.strIndexFile + "'," + "配图方案文件 = '" + dlg.strMapSymIndexFile + "' where " + "专题类型 = '" + strSubOldCode + "'";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //g更新记录    
                int iRows = aCommand.ExecuteNonQuery();

                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception err)
            {
                Console.WriteLine(err.Message);
            }
            strExp = "update 地图入库信息表 set 专题类型='" + dlg.strSubCode + "' where " + "专题类型 = '" + strSubOldCode + "'";
            db.ExcuteSqlFromMdb(constr,strExp);
            string strModFile = Application.StartupPath + "\\..\\Template\\StandardBlank.xml";
            string strIndexFile = Application.StartupPath + "\\..\\Template\\" + dlg.strIndexFile;
            if (strSubOldIndexFile.CompareTo(dlg.strIndexFile) != 0)
            {
                if (!File.Exists(strIndexFile))
                {
                    File.Copy(strModFile, strIndexFile, true);
                }
            }
            if (strSubOldName.CompareTo(dlg.strSubName) != 0)  //修改了专题描述才修改对应的xml文件
            {
                //加载文件并修改GisMap ItemName=""
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(strIndexFile);
                string strSearchRoot = "//GisMap";
                XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
                XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                xmlElentRoot.SetAttribute("sItemName", dlg.strSubName);
                xmldoc.Save(strIndexFile);
            }

        }

        //删除专题
        private void MenuItemDelSub_Click(object sender, EventArgs e)
        {
            if (listViewControl.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择要删除的专题!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strTip = "确定删除【" + listViewControl.SelectedItems[0].SubItems[1].Text + "】";
            if (MessageBox.Show(strTip, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;

            //获取数值添加到“标准专题信息表”中
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "delete  from 标准专题信息表 where 专题类型 = '" + listViewControl.SelectedItems[0].Text + "'";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //删除记录    
                int iRows = aCommand.ExecuteNonQuery();
             
                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception err)
            {
                Console.WriteLine(err.Message);
            }
            strExp = "delete  from 地图入库信息表 where 专题类型 = '" + listViewControl.SelectedItems[0].Text + "'";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            db.ExcuteSqlFromMdb(constr, strExp);
            //删除脚本文件
            string strIndexFile = Application.StartupPath + "\\..\\Template\\" + listViewControl.SelectedItems[0].SubItems[2].Text;
            if (File.Exists(strIndexFile))
            {
                File.Delete(strIndexFile);
            }

            //删除listview
            listViewControl.Items.Remove(listViewControl.SelectedItems[0]);

            treeViewControl.Nodes.Clear();
        }

        //添加组
        private void MenuItemTreeAddGroup_Click(object sender, EventArgs e)
        {
            AddGroupForm dlg = new AddGroupForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //修改树视图
                TreeNode tNewGroupNode = new TreeNode();
                tNewGroupNode.Name = "【" + dlg.m_strSubName + "】";
                tNewGroupNode.Text = "【" + dlg.m_strSubName + "】";
                tNewGroupNode.Tag = 2;
                tNewGroupNode.ImageIndex = 2;
                tNewGroupNode.SelectedImageIndex = 2;
                m_tMapNode.Nodes.Add(tNewGroupNode);
                m_tMapNode.ExpandAll();

                //修改xml文件
                string strSearchRoot = "//GisMap";
                XmlNode xmlNodeRoot = m_xmldoc.SelectSingleNode(strSearchRoot);
                XmlElement xmlElentRoot = (XmlElement)xmlNodeRoot;
                XmlElement xmlElemGroup = m_xmldoc.CreateElement("SubGroup");
                string strGroupName = "【" + dlg.m_strSubName + "】";
                xmlElemGroup.SetAttribute("sItemName", strGroupName);
                xmlElemGroup.SetAttribute("sType", "GROUP");
                xmlElentRoot.AppendChild(xmlElemGroup);


                //获取组内图层列表
                TreeNode tNewNode = null;
                foreach (TreeNode fCurNode in dlg.totreeView.Nodes)
                {
                    
                    XmlNode boolnode = m_xmldoc.SelectSingleNode("//Layer[@sItemName='" +  fCurNode.Text+ "']");
                    if (boolnode != null)
                    {
                        MessageBox.Show(fCurNode.Text+"图层已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        continue;
                    }
                    XmlElement xmlElemt = m_xmldoc.CreateElement("Layer");
                    xmlElemt.SetAttribute("sDemo", fCurNode.Text);
                    xmlElemt.SetAttribute("sItemName", fCurNode.Text);
                    

                    //添加 文件名称 业务大类代码  业务小类代码
                    string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                    OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
                    string strExp = "";
                    strExp = "select  * from 标准图层信息表 where 描述 = '" + fCurNode.Text + "'";
                    OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
                    strExp = "select 图层组成 from 标准专题信息表 where 专题类型='" + m_Typecode + "'";
                    GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                    string layer = db.GetInfoFromMdbByExp(constr, strExp);
                    try
                    {
                        mycon.Open();

                        //创建datareader   对象来连接到表单     
                        OleDbDataReader aReader = aCommand.ExecuteReader();
                        while (aReader.Read())
                        {
                            xmlElemt.SetAttribute("sFile", aReader["代码"].ToString());
                            xmlElemt.SetAttribute("sBigClass", aReader["业务大类代码"].ToString());
                            xmlElemt.SetAttribute("sSubClass", aReader["业务小类代码"].ToString());


                            if (layer == "")
                                layer = aReader["代码"].ToString();
                            else if(!GetExists(aReader["代码"].ToString(),layer))
                                layer += "/" + aReader["代码"].ToString();//更改图层组成
                        }

                        //关闭reader对象     
                        aReader.Close();

                        //关闭连接,这很重要     
                        mycon.Close();
                        //更新标准专题信息表
                        strExp = "update 标准专题信息表 set 图层组成='" + layer + "' where 专题类型='" + m_Typecode + "'";
                        db.ExcuteSqlFromMdb(constr, strExp);
                    }
                    catch (System.Exception err)
                    {

                    }
                    //更新树
                    tNewNode = new TreeNode();
                    tNewNode.Text = fCurNode.Text;
                    tNewNode.Name = fCurNode.Name;
                    tNewNode.Tag = 3;
                    tNewNode.ImageIndex = 3;
                    tNewNode.SelectedImageIndex = 3;
                    tNewGroupNode.Nodes.Add(tNewNode);
                    tNewGroupNode.ExpandAll();

                    xmlElemGroup.AppendChild(xmlElemt);

                }
                treeViewControl.Refresh();

                //更新xml文件
                m_xmldoc.Save(m_xmlPath);
            }
        }

        //检测图层组成中是否包含图层
        private bool GetExists(string layer,string layers)
        {
            bool exist=false;
            if (layers.Contains("/"))
            {
                string[] arrlayer = layers.Split('/');
                for (int ii = 0; ii < arrlayer.Length; ii++)
                {
                    if (arrlayer[ii].Trim() == layer)
                    {
                        exist = true;
                        break;
                    }
                }
            }
            else
            {
                if (layers.Trim() == layer)
                    exist = true;
            }
            return exist;
        }
        //添加图层
        private void MenuItemAddLayer_Click(object sender, EventArgs e)
        {
            string strCurSubName = null;
            TreeNode tparentNode = null;
            if (treeViewControl.SelectedNode.Tag.Equals(2))
            {
                strCurSubName = treeViewControl.SelectedNode.Text;
                tparentNode = treeViewControl.SelectedNode;
            }
            if (treeViewControl.SelectedNode.Tag.Equals(3))
            {
                strCurSubName = treeViewControl.SelectedNode.Parent.Text;
                tparentNode = treeViewControl.SelectedNode.Parent;
            }
            AddLayerForm dlg = new AddLayerForm(m_xmldoc, strCurSubName);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                

                //更新xml文件
                string strSearchRoot = "//SubGroup[@sItemName = '" + strCurSubName + "']";
                XmlNode xmlNodeRoot = m_xmldoc.SelectSingleNode(strSearchRoot);
                XmlElement xmlElemGroup = (XmlElement)xmlNodeRoot;
                XmlNode boolnode = m_xmldoc.SelectSingleNode("//Layer[@sItemName='" + dlg.m_strLayerName + "']");
                if (boolnode != null)
                {
                    MessageBox.Show("该图层已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                XmlElement xmlElemt = m_xmldoc.CreateElement("Layer");
                xmlElemt.SetAttribute("sDemo", dlg.m_strLayerDescri);
                xmlElemt.SetAttribute("sItemName", dlg.m_strLayerName);
                xmlElemt.SetAttribute("sDispScale", dlg.m_strScale);
                xmlElemt.SetAttribute("sDiaphaneity", dlg.m_strTransp);

                //添加 文件名称 业务大类代码  业务小类代码
                string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
                string strExp = "";
                strExp = "select  * from 标准图层信息表 where 描述 = '" + dlg.m_strLayerName + "'";
                OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
                strExp = "select 图层组成 from 标准专题信息表 where 专题类型='"+m_Typecode+"'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string layer = db.GetInfoFromMdbByExp(constr, strExp);
                try
                {
                    mycon.Open();

                    //创建datareader   对象来连接到表单     
                    OleDbDataReader aReader = aCommand.ExecuteReader();
                    while (aReader.Read())
                    {
                        xmlElemt.SetAttribute("sFile", aReader["代码"].ToString());
                        xmlElemt.SetAttribute("sBigClass", aReader["业务大类代码"].ToString());
                        xmlElemt.SetAttribute("sSubClass", aReader["业务小类代码"].ToString());
                        
                        if (layer == "")
                            layer = aReader["代码"].ToString();
                        else if (!GetExists(aReader["代码"].ToString(), layer))
                            layer += "/" + aReader["代码"].ToString();//更改图层组成
                    }

                    //关闭reader对象     
                    aReader.Close(); 
                    //关闭连接,这很重要     
                    mycon.Close();

                    //更新标准专题信息表
                    strExp = "update 标准专题信息表 set 图层组成='" + layer + "' where 专题类型='" + m_Typecode + "'";
                    db.ExcuteSqlFromMdb(constr, strExp);
                }

                catch (System.Exception err)
                {
                    MessageBox.Show(err.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                }
                //更新树
                TreeNode tNewNode = new TreeNode();
                tNewNode.Text = dlg.m_strLayerDescri;
                tNewNode.Name = dlg.m_strLayerName;
                tNewNode.Tag = 3;
                tNewNode.ImageIndex = 3;
                tNewNode.SelectedImageIndex = 3;
                tparentNode.Nodes.Add(tNewNode);
                tparentNode.ExpandAll();
                treeViewControl.Refresh();
               if(xmlElemGroup!=null)
                xmlElemGroup.AppendChild(xmlElemt);

                //更新xml文件
                m_xmldoc.Save(m_xmlPath);
            }
        }


        //更新标准图层信息表
        private void UpdateLayersInfo()
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string layer = "";
            string strSearchRoot = "//Layer";
            XmlNodeList xmlNodelist = m_xmldoc.SelectNodes(strSearchRoot);
            foreach (XmlNode node in xmlNodelist)
            {
                XmlElement xmlelment = node as XmlElement;
                layer += xmlelment.GetAttribute("sFile") + "/";
            }
            if(layer.Contains("/"))
                layer=layer.Substring(0,layer.LastIndexOf("/"));
            strExp = "update 标准专题信息表 set 图层组成='" + layer + "' where 专题类型='" + m_Typecode + "'";
            db.ExcuteSqlFromMdb(constr, strExp);
           
        }

        //删除节点
        private void MenuItemTreeDelLayer_Click(object sender, EventArgs e)
        {
          
            if (treeViewControl.SelectedNode == null)
                return;
            string strSelItemName = treeViewControl.SelectedNode.Text;
            if (treeViewControl.SelectedNode.Tag.Equals(2))
            {
                string strTip = strSelItemName + "包含的所有图层也将一起删除!";
                if (MessageBox.Show(strTip, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    string strSearchRoot = "//SubGroup[@sItemName='" + strSelItemName + "']";
                    XmlNode xmlNodeRoot = m_xmldoc.SelectSingleNode(strSearchRoot);
                    if (xmlNodeRoot != null)
                    {
                        xmlNodeRoot.RemoveAll();
                        xmlNodeRoot.ParentNode.RemoveChild(xmlNodeRoot);
                    }

                    //删除子节点 删除该节点
                    treeViewControl.Nodes.Remove(treeViewControl.SelectedNode);
                }
            }
            else
            {
                //修改对应的xml文件
                string strSearchRoot = "//Layer[@sItemName='" + strSelItemName + "']";
                XmlNode xmlNodeRoot = m_xmldoc.SelectSingleNode(strSearchRoot);
                if (xmlNodeRoot != null)
                {
                    xmlNodeRoot.RemoveAll();
                    xmlNodeRoot.ParentNode.RemoveChild(xmlNodeRoot);
                }

                //删除节点
                treeViewControl.Nodes.Remove(treeViewControl.SelectedNode);
            }
            UpdateLayersInfo();//更新图层信息表
            m_xmldoc.Save(m_xmlPath);
        }

        //鼠标拖动树节点
        private void treeViewControl_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void treeViewControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void treeViewControl_DragDrop(object sender, DragEventArgs e)
        {
            if (m_LastDragNode != null)
            {
                m_LastDragNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示   

                m_LastDragNode.ForeColor = SystemColors.WindowText;
                m_LastDragNode = null;
            }
            //获得拖放中的节点  
            TreeNode moveNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                moveNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            if (moveNode.Level != 3)
                return;
            //根据鼠标坐标确定要移动到的目标节点  
            Point pt;
            TreeNode targeNode;
            pt = ((TreeView)(sender)).PointToClient(new Point(e.X, e.Y));
            targeNode = treeViewControl.GetNodeAt(pt);

            //如果目标节点无子节点则添加为同级节点,反之添加到下级节点的未端  
            TreeNode NewMoveNode = (TreeNode)moveNode.Clone();
            switch (targeNode.Level)
            {
                case 3:
                    if (targeNode.Parent.Parent != moveNode.Parent.Parent)
                        return;
                    targeNode.Parent.Nodes.Insert(targeNode.Index, NewMoveNode);
                    break;
                case 2:
                    if (targeNode.Parent != moveNode.Parent.Parent)
                        return;
                    targeNode.Nodes.Insert(targeNode.Nodes.Count, NewMoveNode);
                    break;
            }

            //更新当前拖动的节点选择  
            treeViewControl.SelectedNode = NewMoveNode;

            //展开目标节点,便于显示拖放效果  
            targeNode.Expand();

            //移除拖放的节点  
            moveNode.Remove();
        }

        private void treeViewControl_DragOver(object sender, DragEventArgs e)
        {


            //当光标悬停在 TreeView 控件上时，展开该控件中的 TreeNode   
            TreeNode moveNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                moveNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            if (moveNode.Level != 3)
                return;

            Point p = this.treeViewControl.PointToClient(new Point(e.X, e.Y));

            TreeNode tn = this.treeViewControl.GetNodeAt(p);

            //设置拖放目标TreeNode的背景色   


            if (m_LastDragNode != null && tn != m_LastDragNode)
            {
                m_LastDragNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示   

                m_LastDragNode.ForeColor = SystemColors.WindowText;
            }
            else
            {
                moveNode.BackColor = SystemColors.Window;//取消上一个被放置的节点高亮显示   

                moveNode.ForeColor = SystemColors.WindowText;
            }

            if (m_LastDragNode != tn)
            {
                tn.BackColor = SystemColors.Highlight;

                tn.ForeColor = SystemColors.HighlightText;

            }
            m_LastDragNode = tn;




        }

        //编辑图层
        private void MenuItemEditLayer_Click(object sender, EventArgs e)
        {
            string strCurSubName = null;//组节点的text
            TreeNode tparentNode = null;
            TreeNode tNode = null;

            strCurSubName = treeViewControl.SelectedNode.Parent.Text;
            tparentNode = treeViewControl.SelectedNode.Parent;
            tNode = treeViewControl.SelectedNode;
            string stroldText = tNode.Name;//记录原有节点名称
            AddLayerForm dlg = new AddLayerForm(m_xmldoc, tNode);
            if (dlg.ShowDialog() == DialogResult.OK)
            {

                tNode.Text = dlg.m_strLayerDescri;
                tNode.Name = dlg.m_strLayerName;
                tNode.Tag = 3;
                tNode.ImageIndex = 3;
                tNode.SelectedImageIndex = 3;
                tparentNode.ExpandAll();
                treeViewControl.Refresh();

                //更新xml文件
                //删除原有节点
                string strSearch = "//Layer[@sItemName='" + stroldText + "']";
                XmlNode xmlNode = m_xmldoc.SelectSingleNode(strSearch);
                //xmlNode.ParentNode.RemoveChild(xmlNode);//
                //string strSearchRoot = "//SubGroup[@sItemName = '" + strCurSubName + "']";
                //XmlNode xmlNodeRoot = m_xmldoc.SelectSingleNode(strSearchRoot);
                //XmlElement xmlElemGroup = (XmlElement)xmlNodeRoot;
                //XmlElement xmlElemt= m_xmldoc.CreateElement("Layer");
                XmlElement xmlElemt = xmlNode as XmlElement;
                xmlElemt.SetAttribute("sDemo", tNode.Text);
                xmlElemt.SetAttribute("sDispScale", dlg.m_strScale);
                xmlElemt.SetAttribute("sDiaphaneity", dlg.m_strTransp);
                xmlElemt.SetAttribute("sItemName", tNode.Name);
                //xmlElemGroup.AppendChild(xmlElemt);

                //添加 文件名称 业务大类代码  业务小类代码
                string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
                string strExp = "";
                strExp = "select  * from 标准图层信息表 where 描述 = '" + tNode.Name + "'";
                OleDbCommand aCommand = new OleDbCommand(strExp, mycon);

                strExp = "select 图层组成 from 标准专题信息表 where 专题类型='" + m_Typecode + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string layer = db.GetInfoFromMdbByExp(constr, strExp);
                try
                {
                    mycon.Open();

                    //创建datareader   对象来连接到表单     
                    OleDbDataReader aReader = aCommand.ExecuteReader();
                    while (aReader.Read())
                    {
                        xmlElemt.SetAttribute("sFile", aReader["代码"].ToString());
                        xmlElemt.SetAttribute("sBigClass", aReader["业务大类代码"].ToString());
                        xmlElemt.SetAttribute("sSubClass", aReader["业务小类代码"].ToString());
                        
                        if (layer == "")
                            layer = aReader["代码"].ToString();
                        else if (!GetExists(aReader["代码"].ToString(), layer))
                            layer += "/" + aReader["代码"].ToString();//更改图层组成
                    }

                    //关闭reader对象     
                    aReader.Close();

                    //关闭连接,这很重要     
                    mycon.Close();

                    //更新标准专题信息表
                    strExp = "update 标准专题信息表 set 图层组成='" + layer + "' where 专题类型='" + m_Typecode + "'";
                    db.ExcuteSqlFromMdb(constr, strExp);
                }

                catch (System.Exception err)
                {

                }

                //更新xml文件
                m_xmldoc.Save(m_xmlPath);
            }
        }

        //设置成关键图层
        private void MenuItemMainLayer_Click(object sender, EventArgs e)
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string layer = "";
           // strExp = "select 关键图层 from 标准专题信息表 where 专题类型='" + m_Typecode + "'";
           //string  Mainlayer = db.GetInfoFromMdbByExp(constr, strExp);
            
            layer = treeViewControl.SelectedNode.Name;
            strExp = "select 代码 from 标准图层信息表 where 描述 ='" + layer + "'";
            layer = db.GetInfoFromMdbByExp(constr, strExp);
            strExp = "update 标准专题信息表 set 关键图层='" + layer + "' where 专题类型='" + m_Typecode + "'";
            db.ExcuteSqlFromMdb(constr, strExp);
            ChangeColor();
            treeViewControl.SelectedNode.ForeColor = Color.Red;
            MenuItemCanceMainLayer.Enabled = true;
            treeViewControl.ExpandAll();
            treeViewControl.Refresh();

        }
         //取消关键图层
        private void MenuItemCanceMainLayer_Click(object sender, EventArgs e)
        {
            if (treeViewControl.SelectedNode.ForeColor.Equals(Color.Red))
            {
                treeViewControl.SelectedNode.ForeColor = Color.Black;
                string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                strExp = "update 标准专题信息表 set 关键图层='' where 专题类型='" + m_Typecode + "'";
                db.ExcuteSqlFromMdb(constr, strExp);
            }
            MenuItemCanceMainLayer.Enabled = true;
            treeViewControl.Refresh();
        }
        
       //改变树节点颜色
        private void ChangeColor()
        {
            try
            {
                foreach (TreeNode node in treeViewControl.Nodes[0].Nodes)
                {
                    foreach (TreeNode child in node.Nodes)
                    {
                        foreach (TreeNode childnode in child.Nodes)
                        {
                            childnode.ForeColor = SystemColors.WindowText;
                        }
                    }
                }
            }
            catch
            { }
        }
    

    }
}