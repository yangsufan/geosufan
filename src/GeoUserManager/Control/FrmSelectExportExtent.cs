using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GeoUserManager
{
    public partial class SelectExportExtent : DevComponents.DotNetBar.Office2007Form
    {
        private string m_XZQtreePath = Application.StartupPath + "\\..\\Res\\Xml\\XZQ.xml";
        public SelectExportExtent(List<string > lisCode)
        {
            InitializeComponent();
            m_ListXian = lisCode;
            InitialTreeView(m_XZQtreePath);
            //ChangeChecked(advXZQ .Nodes[0]);
        }
        public List<string> m_ListXian
        {
            get;
            set;
        }
        public List<string> m_ListName
        {
            get;
            set;
        }
        //ygc 20130419 根据行政区XML文件初始化树图
        private void InitialTreeView(string XZQXmlPath)
        {
            advXZQ.Nodes.Clear();
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(XZQXmlPath);
                //changed by chulili 准确找到行政区节点
                string strSearch = "//XZQTree";
                XmlNode vChinaNode = cXmlDoc.SelectSingleNode(strSearch);
                string xnlattr = vChinaNode.Attributes["ItemName"].Value;
                DevComponents.AdvTree.Node vRootNode = new DevComponents.AdvTree.Node();
                vRootNode.Text = xnlattr;
                vRootNode.Name = xnlattr;
                vRootNode.Tag = vChinaNode.Name;
                vRootNode.ImageIndex = 38;
                vRootNode.DataKey = vChinaNode as object;
                vRootNode.CheckState = CheckState.Checked;
                advXZQ.Nodes.Add(vRootNode);//行政区节点
                DevComponents.AdvTree.Node tNode1 = advXZQ.Nodes[0];
                if (!vChinaNode.HasChildNodes)
                    return;
                vRootNode.Expand();
                foreach (XmlNode vSubXmlNode in vChinaNode.ChildNodes)//全国0//改为省节点
                {
                    //shduan 20110731
                    string strXZQName = vSubXmlNode.Attributes["ItemName"].Value;
                    string strXZQCode = vSubXmlNode.Attributes["XzqCode"].Value;
                    DevComponents.AdvTree.Node vNode = new DevComponents.AdvTree.Node();
                    vNode.CheckBoxVisible = true;
                    vNode.Text = strXZQName;
                    vNode.Name = strXZQCode;
                    vNode.Tag = vSubXmlNode.Name;
                    vNode.DataKey = vSubXmlNode as object;
                    //added by chulili 20110818 设置节点图标 
                    switch (vSubXmlNode.Name)
                    {
                        case "Province":
                            vNode.ImageIndex = 35;
                            //vNode.SelectedImageIndex = 35;
                            break;
                        case "City":
                            vNode.ImageIndex = 37;
                            //vNode.SelectedImageIndex = 37;
                            break;
                        case "County":
                            vNode.ImageIndex = 36;
                            //vNode.SelectedImageIndex = 36;
                            break;
                        case "Town":
                            vNode.ImageIndex = 34;
                            //vNode.SelectedImageIndex = 36;
                            break;
                        case "Village":
                            //vNode.ImageIndex = 36;
                            //vNode.SelectedImageIndex = 36;
                            break;

                    }
                    //end added by chulili 20110818
                    tNode1.Nodes.Add(vNode);
                    getTreeNodeFromXMLNode(vSubXmlNode, vNode);
                    try
                    {
                        if (vSubXmlNode.Attributes["Expand"].Value == "1")
                            vNode.Expand();
                    }
                    catch
                    {
                    }

                }
                //tNode1.Expand();
            }
            cXmlDoc = null;
        }
        //递归遍历xmlnode初始化treenode
        //shduan 20110731
        private void getTreeNodeFromXMLNode(XmlNode vXmlNode, DevComponents.AdvTree.Node vNode)
        {
            foreach (XmlNode vSubXmlNode in vXmlNode.ChildNodes)//全国0
            {
                if (vSubXmlNode.Name != "Town")
                {
                    string strXZQName = vSubXmlNode.Attributes["ItemName"].Value;
                    string strXZQCode = vSubXmlNode.Attributes["XzqCode"].Value;
                    DevComponents.AdvTree.Node vSubNode = new DevComponents.AdvTree.Node();
                    vSubNode.CheckBoxVisible = true;
                    if (m_ListXian !=null &&m_ListXian.Contains(strXZQCode))
                    {
                        vSubNode.CheckState = CheckState.Checked;
                        if (vNode.CheckState == CheckState.Unchecked)
                        {
                            vNode.CheckState = CheckState.Checked;
                        }
                        if (vNode.Parent != null && vNode.Parent.CheckState != CheckState.Checked)
                        {
                            vNode.Parent.CheckState = CheckState.Checked;
                        }
                    }
                    vSubNode.Text = strXZQName;
                    vSubNode.Name = strXZQCode;
                    vSubNode.Tag = vSubXmlNode.Name;
                    vSubNode.DataKey = vSubXmlNode as object;
                    //added by chulili 20110818 设置节点图标 
                    switch (vSubXmlNode.Name)
                    {
                        case "Province":
                            vSubNode.ImageIndex = 35;
                            //vSubNode.SelectedImageIndex = 35;
                            break;
                        case "City":
                            vSubNode.ImageIndex = 37;
                            //vSubNode.SelectedImageIndex = 37;
                            break;
                        case "County":
                            vSubNode.ImageIndex = 36;
                            //vSubNode.SelectedImageIndex = 36;
                            break;
                        case "Town":
                            vSubNode.ImageIndex = 34;
                            //vNode.SelectedImageIndex = 36;
                            break;
                        case "Village":
                            vSubNode.ImageIndex = 34;
                            //vNode.SelectedImageIndex = 36;
                            break;
                    }
                    //end added by chulili 20110818
                    vNode.Nodes.Add(vSubNode);
                    getTreeNodeFromXMLNode(vSubXmlNode, vSubNode);
                    try
                    {
                        if (vSubXmlNode.Attributes["Expand"].Value == "1")
                            vSubNode.Expand();
                    }
                    catch
                    {
                    }
                }
            }
            if (vNode.Tag.ToString() == "XZQTree") //如果行政区代码为0就就展开，若不是则不展开，防止递归展开所以子节点 xisheng 20110803
                vNode.Expand();
        }
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        //ygc 20130420 根据县级选择状态修改下一级状态
        private void ChangeChecked(DevComponents.AdvTree.Node vNode)
        {
            if (vNode.HasChildNodes)
            {
                for (int i = 0; i < vNode.Nodes.Count; i++)
                {
                    if (vNode.Nodes[i].HasChildNodes)
                    {
                        vNode.Nodes[i].CheckState = vNode.CheckState;
                        ChangeChecked(vNode.Nodes[i]);
                    }
                    else
                    {
                        vNode.Nodes[i].CheckState = vNode.CheckState;
                    }
                }
            }
        }
        //ygc 20130420 改变上一级状态
        private void ChangeChild(DevComponents.AdvTree.Node vNode)
        {
            bool flag = false;
            if (vNode.Parent != null)
            {
                //直接改变父节点状态
                if (vNode.CheckState == CheckState.Checked)
                {
                    vNode.Parent.CheckState = vNode.CheckState;
                }
                else
                {
                    //循环判断改父节点下所以子节点是否都未勾选
                    for (int i = 0; i < vNode.Parent.Nodes.Count; i++)
                    {
                        if (vNode.Parent.Nodes[i].CheckState == CheckState.Checked)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        vNode.Parent.CheckState = vNode.CheckState;
                    }
                }
            }
        }

        private void advXZQ_AfterCheck(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            DevComponents.AdvTree.Node vNode = advXZQ.SelectedNode;
            if (vNode != null)
            {
                ChangeChecked(vNode);
                //改变父节点状态
                if (vNode.Parent != null)
                {
                    ChangeChild(vNode);
                    if (vNode.Parent.Parent != null)
                    {
                        ChangeChild(vNode.Parent);
                    }
                }
            }
           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (advXZQ.Nodes[0] == null) return;
            if (!advXZQ.Nodes[0].HasChildNodes) return;
            m_ListXian = new List<string>();
            m_ListName = new List<string>();
            for (int i = 0; i < advXZQ.Nodes[0].Nodes[0].Nodes.Count; i++)
            {
                for (int j = 0; j < advXZQ.Nodes[0].Nodes[0].Nodes[i].Nodes.Count; j++)
                {
                    if (advXZQ.Nodes[0].Nodes[0].Nodes[i].Nodes[j].CheckState == CheckState.Checked)
                    {
                        m_ListXian.Add(advXZQ.Nodes[0].Nodes[0].Nodes[i].Nodes[j].Name);
                        m_ListName.Add(advXZQ.Nodes[0].Nodes[0].Nodes[i].Nodes[j].Text);
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
