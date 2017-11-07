using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Carto;

namespace SysCommon.SelectLayer
{
    public class ClsSelectLayerByTree
    {
        private System.Xml.XmlDocument _LayerTreeXmldoc = null;
        private bool m_checkbox = false;
        private System.Windows.Forms.ImageList _Imagelist = null;
        private Dictionary<DevComponents.AdvTree.Node, bool> m_Dic = new Dictionary<DevComponents.AdvTree.Node, bool>();
        private List<string> _ListDataNodeKeys = null;
        public List<string> ListDataNodeKeys
        {
            get
            {
                return _ListDataNodeKeys;
            }
            set
            {
                _ListDataNodeKeys = value;
            }
        }

        private List<string> _ListLayerKeys = null;
        public List<string> ListLayerKeys
        {
            get
            {
                return _ListLayerKeys;
            }
            set
            {
                _ListLayerKeys = value;
            }
        }

        public System.Windows.Forms.ImageList Imagelist
        {
            get
            {
                return _Imagelist;
            }
            set
            {
                _Imagelist = value;
            }
        }
        public bool CheckBox
        {
            get
            {
                return m_checkbox;
            }
            set
            {
                m_checkbox = value;
            }
        }
        /// <summary>
        /// 选中父节点
        /// </summary>
        /// <param name="node"></param>
        /// 
        public void CheckNode(DevComponents.AdvTree.Node node)//
        {
            if (node.Parent != null)
            {
                node.Parent.Checked = true;
                CheckNode(node.Parent);
            }

        }
        //创建根节点 xisheng 20111128
        public void CreatRootNode(IMap pMap, string strLayerTreePath, DevComponents.AdvTree.AdvTree advTreeLayerList,bool isTbClick)
        {
            //初始化图层树列表
            if (File.Exists(strLayerTreePath))
            {
                if (_LayerTreeXmldoc == null)
                {
                    _LayerTreeXmldoc = new XmlDocument();
                }
                _LayerTreeXmldoc.Load(strLayerTreePath);
                advTreeLayerList.Nodes.Clear();

                //获取Xml的根节点并作为根节点加到UltraTree上
                XmlNode xmlnodeRoot = _LayerTreeXmldoc.DocumentElement;
                XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

                xmlelementRoot.SetAttribute("NodeKey", "Root");
                string sNodeText = xmlelementRoot.GetAttribute("NodeText");

                //创建并设定树的根节点
                DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
                treenodeRoot.Name = "Root";
                treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
                treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                if (m_checkbox)
                {
                    treenodeRoot.CheckBoxVisible = true;
                    treenodeRoot.Checked = true;
                }
                advTreeLayerList.Nodes.Add(treenodeRoot);

                treenodeRoot.Image = _Imagelist.Images["Root"];
                if (_ListLayerKeys == null)
                {
                    InitLayerTreeByXmlNode(pMap, treenodeRoot, xmlnodeRoot, isTbClick);
                }
                else
                {
                    InitLayerTreeByLayerKeys(pMap, treenodeRoot, xmlnodeRoot, isTbClick);
                }
            }
        }
        //根据配置文件显示图层树
        public void InitLayerTreeByLayerKeys(IMap pMap, DevComponents.AdvTree.Node treenode, XmlNode xmlnode, bool isTbClick)
        {

            bool haschild = false;
            XmlNodeList pNodeList = xmlnode.SelectNodes("//Layer");
            if (pNodeList != null)
            {
                for (int iChildIndex = 0; iChildIndex < pNodeList.Count; iChildIndex++)
                {
                    if (iChildIndex == pNodeList.Count - 1)
                    {
                        m_Dic.Add(treenode, true);
                    }
                    XmlElement xmlElementChild = pNodeList[iChildIndex] as XmlElement;
                    if (xmlElementChild == null)
                    {
                        continue;
                    }
                    else if (xmlElementChild.Name == "ConfigInfo")
                    {
                        continue;
                    }
                    //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
                    string sNodeKey = xmlElementChild.GetAttribute("NodeKey");
                    if (_ListLayerKeys != null)
                    {
                        if (!_ListLayerKeys.Contains(sNodeKey))
                        {
                            continue;
                        }
                    }
                    string sNodeText = xmlElementChild.GetAttribute("NodeText");

                    ILayer player = null;
                    //判断Map中是否存在
                    if (pMap != null && xmlElementChild.Name == "Layer")
                    {
                        player = SysCommon.ModuleMap.GetLayerByNodeKey(null, pMap, sNodeKey, null);
                        if (!isTbClick)//判断是不是点击同步了
                        {
                            if (player == null) continue;
                        }

                    }

                    haschild = true;
                    DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                    treenodeChild.Name = sNodeKey;
                    treenodeChild.Text = sNodeText;


                    treenodeChild.DataKey = xmlElementChild;
                    treenodeChild.Tag = xmlElementChild.Name;
                    treenode.Nodes.Add(treenodeChild);

                    if (m_checkbox)
                    {
                        treenodeChild.CheckBoxVisible = true;
                        if (xmlElementChild.Name == "Layer" && player != null)
                        {
                            treenodeChild.Checked = true;

                            CheckNode(treenodeChild);//选中父节点
                        }
                    }
                    if (!m_checkbox)
                    {
                        InitializeNodeImage(treenodeChild);
                    }
                }
            }

            
            //if (!haschild) //没有子节点移除父节点
            //{
            //    Remove(treenode);

            //}

        }
        //根据配置文件显示图层树
        public void InitLayerTreeByXmlNode(IMap pMap, DevComponents.AdvTree.Node treenode, XmlNode xmlnode,bool isTbClick)
        {

            bool haschild = false;

            for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
            {
                if (iChildIndex == xmlnode.ChildNodes.Count - 1)
                {
                    m_Dic.Add(treenode, true);
                }
                XmlElement xmlElementChild = xmlnode.ChildNodes[iChildIndex] as XmlElement;
                if (xmlElementChild == null)
                {
                    continue;
                }
                else if (xmlElementChild.Name == "ConfigInfo")
                {
                    continue;
                }
                //用Xml子节点的"NodeKey"和"NodeText"属性来构造树子节点
                string sNodeKey = xmlElementChild.GetAttribute("NodeKey");
                if (_ListDataNodeKeys != null)
                {
                    if (!_ListDataNodeKeys.Contains(sNodeKey))
                    {
                        continue;
                    }
                }
                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                ILayer player = null;
                //判断Map中是否存在
                if (pMap != null && xmlElementChild.Name == "Layer")
                {
                    player = SysCommon.ModuleMap.GetLayerByNodeKey(null, pMap, sNodeKey, null);
                    if (!isTbClick)//判断是不是点击同步了
                    {
                        if (player == null) continue;
                    }

                }

                haschild = true;
                DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                treenodeChild.Name = sNodeKey;
                treenodeChild.Text = sNodeText;


                treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;
                treenode.Nodes.Add(treenodeChild);

                if (m_checkbox)
                {
                    treenodeChild.CheckBoxVisible = true;
                    if (xmlElementChild.Name == "Layer" && player != null)
                    {
                        treenodeChild.Checked = true;

                        CheckNode(treenodeChild);//选中父节点
                    }
                }

                //递归
                if (xmlElementChild.Name != "Layer")
                {
                    InitLayerTreeByXmlNode(pMap,treenodeChild, xmlElementChild as XmlNode,isTbClick );
                }
                if (!m_checkbox)
                {
                    InitializeNodeImage(treenodeChild);
                }
            }
            //if (!haschild) //没有子节点移除父节点
            //{
            //    Remove(treenode);

            //}

        }
        public void RemoveChild(DevComponents.AdvTree.Node node,ref bool remove)
        {
            remove = false;
            try
            {
                if (node.Nodes.Count > 0)
                {
                    for (int i = 0; i < node.Nodes.Count; i++)
                    {
                        RemoveChild(node.Nodes[i],ref remove );
                        if (remove) i--;
                    }

                }
                else if (node.Tag.ToString() != "Layer")
                {
                    node.Remove();
                    remove = true;
                }
            }
            catch
            { }
        }
        /// <summary>
        /// 移除节点后，判断其父节点是否也需要移除
        /// </summary>
        /// <param name="node"></param>
        public void Remove(DevComponents.AdvTree.Node node)
        {
            try
            {
                if (node.Parent.Nodes.Count == 1 && node.Parent.Name != "Root")
                {
                    Remove(node.Parent);
                }
                else
                {
                    node.Remove();
                }
            }
            catch
            { }
        }
        /// <summary>
        /// 通过传入节点的tag，选择对应的图标        
        /// </summary>
        /// <param name="treenode"></param>
        public void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        {
            switch (treenode.Tag.ToString())
            {
                case "Root":
                    treenode.Image =_Imagelist.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "SDE":
                    treenode.Image = _Imagelist.Images["SDE"];
                    break;
                case "PDB":
                    treenode.Image = _Imagelist.Images["PDB"];
                    break;
                case "FD":
                    treenode.Image = _Imagelist.Images["FD"];
                    break;
                case "FC":
                    treenode.Image = _Imagelist.Images["FC"];
                    break;
                case "TA":
                    treenode.Image = _Imagelist.Images["TA"];
                    break;
                case "DIR":
                    treenode.Image = _Imagelist.Images["DIR"];
                    //treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = _Imagelist.Images["DataDIRHalfOpen"];
                    break;
                case "DataDIR&AllOpened":
                    treenode.Image = _Imagelist.Images["DataDIROpen"];
                    break;
                case "DataDIR&Closed":
                    treenode.Image = _Imagelist.Images["DataDIRClosed"];
                    break;
                case "DataDIR&HalfOpened":
                    treenode.Image = _Imagelist.Images["DataDIRHalfOpen"];
                    break;
                case "Layer":
                    XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
                    if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                treenode.Image = _Imagelist.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                treenode.Image = _Imagelist.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                treenode.Image = _Imagelist.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                treenode.Image = _Imagelist.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                treenode.Image = _Imagelist.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                treenode.Image = _Imagelist.Images["_MultiPatch"];
                                break;
                            default:
                                treenode.Image = _Imagelist.Images["Layer"];
                                break;
                        }
                    }
                    else
                    {
                        treenode.Image = _Imagelist.Images["Layer"];
                    }
                    break;
                case "RC":
                    treenode.Image = _Imagelist.Images["RC"];
                    break;
                case "RD":
                    treenode.Image = _Imagelist.Images["RD"];
                    break;
                case "SubType":
                    treenode.Image = _Imagelist.Images["SubType"];
                    break;
                default:
                    break;
            }//end switch
        }
        //获得所有选中的节点
        public void GetCheckedNodetoMap(DevComponents.AdvTree.Node Node, List<DevComponents.AdvTree.Node> pListNode)
        {
            foreach (DevComponents.AdvTree.Node node in Node.Nodes)
            {
                if (node.Checked)
                {
                    if (node.Nodes.Count > 0)
                        GetCheckedNodetoMap(node,pListNode );
                    else
                    {
                        if (node.Tag.ToString() == "Layer")//不是叶子节点
                            pListNode.Add(node);
                    }
                }
            }
        }

        //通过NODE 得到NODYKEY
        public void GetNodeKey(DevComponents.AdvTree.Node pNode, DevComponents.DotNetBar.LabelX labelErr, int m_m, ref string NodeKey, ref string NodeText)
        {
            string connectkey = "";
            GetNodeKey(pNode, labelErr, m_m, ref NodeKey, ref NodeText, ref connectkey);

        }
        //通过NODE 得到NODYKEY
        public void GetNodeKey(DevComponents.AdvTree.Node pNode, DevComponents.DotNetBar.LabelX labelErr, int m_m, ref string NodeKey, ref string NodeText,ref string ConnectKey)
        {
            labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)pNode.DataKey;
            XmlElement xmlelement = xmlnode as XmlElement;
            string strDataType = "";
            if (xmlelement.HasAttribute("DataType"))
            {
                strDataType = xmlnode.Attributes["DataType"].Value;
            }
            if (strDataType == "RD" || strDataType == "RC")//是影像数据 返回
            {
                labelErr.Text = "请选择矢量数据进行操作!";
                return;
            }
            if (m_m == 1)
            {
                if (xmlelement.HasAttribute("IsQuery"))
                {
                    if (xmlelement["IsQuery"].Value == "False")
                    {
                        labelErr.Text = "该图层不可查询!";
                        return;
                    }
                }
            }
            if (xmlelement.HasAttribute("NodeKey"))
            {
                NodeKey = xmlelement.GetAttribute("NodeKey");
                NodeText = pNode.Text;
            }
            if (xmlelement.HasAttribute("ConnectKey"))
            {
                ConnectKey = xmlelement.GetAttribute("ConnectKey");
            }
        }
        //通过NODE 得到NODYKEY
        public string GetNodeKey1(DevComponents.AdvTree.Node Node, DevComponents.DotNetBar.LabelX labelErr,int m_m)
        {
            labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)Node.DataKey;
            XmlElement xmlelement = xmlnode as XmlElement;

            if (m_m == 1)
            {
                if (xmlelement.HasAttribute("IsQuery"))
                {
                    if (xmlelement["IsQuery"].Value == "False")
                    {
                        labelErr.Text = "该图层不可查询!";
                        return "";
                    }
                }
            }
            if (xmlelement.HasAttribute("NodeKey"))
            {
                return xmlelement.GetAttribute("NodeKey");

            }
            else
                return "";
        }
        /// <summary>
        /// 根据当前节点递归选中父节点 added by xs 2011.04.18
        /// </summary>
        /// <param name="node">当前节点</param>
        public void ChangeParentCheck(DevComponents.AdvTree.Node node,ref bool flag2)
        {
            flag2 = false;
            if (node.Parent != null)
            {
                if (node.Checked)
                {
                    node.Parent.Checked = true;
                    ChangeParentCheck(node.Parent,ref flag2 );
                }
            }
            flag2 = true;

        }

        /// <summary>
        /// 根据当前节点递归选中父节点 added by xs 2011.04.19
        /// </summary>
        /// <param name="node">当前节点</param>
        public void ChangeChildCheck(DevComponents.AdvTree.Node node,ref bool flag)
        {

            foreach (DevComponents.AdvTree.Node item in node.Nodes)
            {
                flag = false;
                if (item.Nodes.Count == 0) flag = true;
                item.Checked = node.Checked;
                if (!flag)
                    ChangeChildCheck(item,ref flag);
            }


        }

    }
}
