using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GeoLayerTreeLib.LayerManager
{
    public partial class FormSetLayerAtt : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.Node _node = null;    //用户所要更改的节点
        private string _xmlpath = "";
        private UcDataLib _pUC = null;//added by chulili 20110909 添加控件变量，用来调用卸载节点的函数，节点修改了，老节点对应的图层要从视图中卸载掉
        
        public bool GetLoad()
        {
            return chkLoad.Checked;
        }
        public FormSetLayerAtt()
        {
            InitializeComponent();
        }
        public FormSetLayerAtt(UcDataLib pUC, string xmlpath, DevComponents.AdvTree.Node pnode)
        {
            InitializeComponent();
            _pUC = pUC;
            _xmlpath = xmlpath;
            _node = pnode;
        }
        private void checkBoxMinScale_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMinScale.Enabled = checkBoxMinScale.Checked;
        }

        private void checkBoxMaxScale_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMaxScale.Enabled = checkBoxMaxScale.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            if (_pUC != null)
            {
                _pUC.RemoveNodeFromMap(_node);
            }
            //打开xml文档
            XmlDocument layerxmldoc = new XmlDocument();
            layerxmldoc.Load(_xmlpath);
            //查找xml中的对应节点
            string strTag = _node.Tag.ToString();
            string NodeName = strTag;
            if (strTag.Contains("DataDIR"))
            {
                NodeName = "DataDIR";
            }
            string strSearch = "//" + NodeName + "[@NodeKey='" + _node.Name + "']";
            XmlNode pxmlnode = layerxmldoc.SelectSingleNode(strSearch);

            SetAttriOfXmlnode(_node, layerxmldoc);
            layerxmldoc.Save(_xmlpath);
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }
        //为xml节点赋数据源，递归调用
        private bool SetAttriOfXmlnode(DevComponents.AdvTree.Node pNode, XmlDocument pXmldoc)
        {
            //判断参数是否有效
            if (pNode == null)
                return false;
            string strTag = pNode.Tag.ToString();
            string strNodeName = strTag;
            if (strNodeName.Contains("DataDIR"))
            {
                strNodeName = "DataDIR";
            }
            switch (strNodeName)
            {
                case "Root":
                case "DIR":
                case "DataDIR":
                    
                    if (pNode.Nodes.Count > 0)
                    {
                        for (int i = 0; i < pNode.Nodes.Count; i++)
                        {
                            DevComponents.AdvTree.Node pTmpNode = pNode.Nodes[i];
                            SetAttriOfXmlnode(pTmpNode, pXmldoc);
                        }
                        //string strNodeKey = pNode.Name;
                        //XmlNode pNewXmlNode = pXmldoc.SelectSingleNode("//" + strNodeName + "[@NodeKey='" + strNodeKey + "']");
                        //if (pNewXmlNode != null)
                        //{
                        //    pNode.DataKey = pNewXmlNode as object;
                        //}
                    }
                    
                    break;
                case "Layer":
                    try
                    {
                        string strSearch = "//" + strNodeName + "[@NodeKey='" + pNode.Name + "']";
                        XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
                        if (pXmlnode == null)
                        {
                            return false;
                        }
                        if (!(pXmlnode is XmlElement))
                        {
                            return false;
                        }
                        XmlElement pNodeEle = pXmlnode as XmlElement;
                        if (chkLoad.Checked)
                        {
                            pNodeEle.SetAttribute("Load","1");
                        }
                        else
                        {
                            pNodeEle.SetAttribute("Load", "0");
                        }
                        if (chkView.Checked)
                        {
                            pNodeEle.SetAttribute("View","1");
                        }
                        else
                        {
                            pNodeEle.SetAttribute("View", "0");
                        }
                        string strDataType = pNodeEle.GetAttribute("DataType");
                        XmlNode nodeShow = pXmlnode["AboutShow"];
                        //为AboutShow节点写属性
                        XmlElement eleShow = nodeShow as XmlElement ;
                        if (nodeShow == null)
                        {
                            eleShow = pXmldoc.CreateElement("AboutShow");
                            nodeShow = pXmlnode.AppendChild(eleShow as XmlNode);
                        }
                        if (eleShow != null)
                        {
                            if (strDataType.ToUpper().Equals("FC"))
                            {
                                eleShow.SetAttribute("IsEdit", chkEdit.Checked.ToString());
                                eleShow.SetAttribute("IsQuery", chkQuery.Checked.ToString());
                                eleShow.SetAttribute("IsSelect", chkSelected.Checked.ToString());
                            }
                            if (checkBoxMaxScale.Checked)
                            {
                                eleShow.SetAttribute("MaxScale", this.txtMaxScale.Text );                                
                            }
                            if (checkBoxMinScale.Checked)
                            {
                                eleShow.SetAttribute("MinScale", this.txtMinScale.Text );
                            }

                        }
                        //added by chulili 20110630 非常重要，只有这样关联上，图层节点才能在视图浏览中正确显示
                        //pNode.DataKey = pXmlnode as object;
                        ModuleMap.SetDataKey(pNode, pXmlnode);
                    }
                    catch (Exception e)
                    {
                        string strinfo = e.Message;
                    }
                    break;
            }
            return true;
        }
    }
}
