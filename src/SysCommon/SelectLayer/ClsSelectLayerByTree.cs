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
        private Dictionary<DevExpress.XtraTreeList.Nodes.TreeListNode, bool> m_Dic = new Dictionary<DevExpress.XtraTreeList.Nodes.TreeListNode, bool>();
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
        public void CheckNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)//
        {
            if (node.ParentNode != null)
            {
                node.ParentNode.Checked = true;
                CheckNode(node.ParentNode);
            }

        }
        //创建根节点 xisheng 20111128
        public void CreatRootNode(IMap pMap, string strLayerTreePath, DevExpress.XtraTreeList.TreeList advTreeLayerList,bool isTbClick)
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
                DevExpress.XtraTreeList.Nodes.TreeListNode treenodeRoot = new DevExpress.XtraTreeList.Nodes.TreeListNode();
                //treenodeRoot.n = "Root";
                //treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
               // treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                if (m_checkbox)
                {
                    //treenodeRoot.CheckBoxVisible = true;
                    treenodeRoot.Checked = true;
                }
                advTreeLayerList.Nodes.Add(treenodeRoot);

                //treenodeRoot = _Imagelist.Images["Root"];
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
        public void InitLayerTreeByLayerKeys(IMap pMap,DevExpress.XtraTreeList.Nodes.TreeListNode treenode, XmlNode xmlnode, bool isTbClick)
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
                    DevExpress.XtraTreeList.Nodes.TreeListNode treenodeChild = new DevExpress.XtraTreeList.Nodes.TreeListNode();
                    //treenodeChild.Name = sNodeKey;
                    //treenodeChild.Text = sNodeText;


                    //treenodeChild.DataKey = xmlElementChild;
                    treenodeChild.Tag = xmlElementChild.Name;
                    treenode.Nodes.Add(treenodeChild);

                    if (m_checkbox)
                    {
                        //treenodeChild.CheckBoxVisible = true;
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
        }
        //根据配置文件显示图层树
        public void InitLayerTreeByXmlNode(IMap pMap, DevExpress.XtraTreeList.Nodes.TreeListNode treenode, XmlNode xmlnode,bool isTbClick)
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
                DevExpress.XtraTreeList.Nodes.TreeListNode treenodeChild = new DevExpress.XtraTreeList.Nodes.TreeListNode();
                //treenodeChild.Name = sNodeKey;
                //treenodeChild.Text = sNodeText;


                //treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;
                treenode.Nodes.Add(treenodeChild);

                if (m_checkbox)
                {
                    //treenodeChild.CheckBoxVisible = true;
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
        public void RemoveChild(DevExpress.XtraTreeList.Nodes.TreeListNode node , ref bool remove)
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
        public void Remove(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                if (node.PrevNode.Nodes.Count == 1)// && node.PrevNode. != "Root")
                {
                    Remove(node.PrevNode);
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
        public void InitializeNodeImage(DevExpress.XtraTreeList.Nodes.TreeListNode treenode)
        {
            //switch (treenode.Tag.ToString())
            //{
            //    case "Root":
            //        treenode.Image =_Imagelist.Images["Root"];
            //        treenode.CheckBoxVisible = false;
            //        break;
            //    case "SDE":
            //        treenode.Image = _Imagelist.Images["SDE"];
            //        break;
            //    case "PDB":
            //        treenode.Image = _Imagelist.Images["PDB"];
            //        break;
            //    case "FD":
            //        treenode.Image = _Imagelist.Images["FD"];
            //        break;
            //    case "FC":
            //        treenode.Image = _Imagelist.Images["FC"];
            //        break;
            //    case "TA":
            //        treenode.Image = _Imagelist.Images["TA"];
            //        break;
            //    case "DIR":
            //        treenode.Image = _Imagelist.Images["DIR"];
            //        //treenode.CheckBoxVisible = false;
            //        break;
            //    case "DataDIR":
            //        treenode.Image = _Imagelist.Images["DataDIRHalfOpen"];
            //        break;
            //    case "DataDIR&AllOpened":
            //        treenode.Image = _Imagelist.Images["DataDIROpen"];
            //        break;
            //    case "DataDIR&Closed":
            //        treenode.Image = _Imagelist.Images["DataDIRClosed"];
            //        break;
            //    case "DataDIR&HalfOpened":
            //        treenode.Image = _Imagelist.Images["DataDIRHalfOpen"];
            //        break;
            //    case "Layer":
            //        XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
            //        if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
            //        {
            //            string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

            //            switch (strFeatureType)
            //            {
            //                case "esriGeometryPoint":
            //                    treenode.Image = _Imagelist.Images["_point"];
            //                    break;
            //                case "esriGeometryPolyline":
            //                    treenode.Image = _Imagelist.Images["_line"];
            //                    break;
            //                case "esriGeometryPolygon":
            //                    treenode.Image = _Imagelist.Images["_polygon"];
            //                    break;
            //                case "esriFTAnnotation":
            //                    treenode.Image = _Imagelist.Images["_annotation"];
            //                    break;
            //                case "esriFTDimension":
            //                    treenode.Image = _Imagelist.Images["_Dimension"];
            //                    break;
            //                case "esriGeometryMultiPatch":
            //                    treenode.Image = _Imagelist.Images["_MultiPatch"];
            //                    break;
            //                default:
            //                    treenode.Image = _Imagelist.Images["Layer"];
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            treenode.Image = _Imagelist.Images["Layer"];
            //        }
            //        break;
            //    case "RC":
            //        treenode.Image = _Imagelist.Images["RC"];
            //        break;
            //    case "RD":
            //        treenode.Image = _Imagelist.Images["RD"];
            //        break;
            //    case "SubType":
            //        treenode.Image = _Imagelist.Images["SubType"];
            //        break;
            //    default:
            //        break;
            //}//end switch
        }
        //获得所有选中的节点
        public void GetCheckedNodetoMap(DevExpress.XtraTreeList.Nodes.TreeListNode Node, List<DevExpress.XtraTreeList.Nodes.TreeListNode> pListNode)
        {
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node in Node.Nodes)
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
        public void GetNodeKey(DevExpress.XtraTreeList.Nodes.TreeListNode pNode, DevExpress.XtraEditors.LabelControl labelErr, int m_m, ref string NodeKey, ref string NodeText)
        {
            string connectkey = "";
            GetNodeKey(pNode, labelErr, m_m, ref NodeKey, ref NodeText, ref connectkey);

        }
        //通过NODE 得到NODYKEY
        public void GetNodeKey(DevExpress.XtraTreeList.Nodes.TreeListNode pNode, DevExpress.XtraEditors.LabelControl labelErr, int m_m, ref string NodeKey, ref string NodeText,ref string ConnectKey)
        {
            labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)pNode.Tag;
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
               // NodeText = pNode.GetValue;
            }
            if (xmlelement.HasAttribute("ConnectKey"))
            {
                ConnectKey = xmlelement.GetAttribute("ConnectKey");
            }
        }
        //通过NODE 得到NODYKEY
        public string GetNodeKey1(DevExpress.XtraTreeList.Nodes.TreeListNode Node, DevExpress.XtraEditors.LabelControl labelErr,int m_m)
        {
            labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)Node.Tag;
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
        public void ChangeParentCheck(DevExpress.XtraTreeList.Nodes.TreeListNode node,ref bool flag2)
        {
            flag2 = false;
            if (node.PrevNode != null)
            {
                if (node.Checked)
                {
                    node.PrevNode.Checked = true;
                    ChangeParentCheck(node.PrevNode, ref flag2 );
                }
            }
            flag2 = true;

        }
        /// <summary>
        /// 根据当前节点递归选中父节点 added by xs 2011.04.19
        /// </summary>
        /// <param name="node">当前节点</param>
        public void ChangeChildCheck(DevExpress.XtraTreeList.Nodes.TreeListNode node,ref bool flag)
        {

            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode item in node.Nodes)
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
