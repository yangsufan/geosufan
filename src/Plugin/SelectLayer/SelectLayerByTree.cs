using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

//xisheng 20111119 增加选择图层的功能。
namespace Fan.Plugin
{
    public partial class SelectLayerByTree : Fan.Common.BaseForm

    {
        public string _LayerTreePath = System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\查询图层树.xml"; //图层目录文件路径
        private XmlDocument _LayerTreeXmldoc = null;
        private Dictionary<string, string> _DicLayerList = null;
        private int m_m=0;//设置分类，是查询还是统计，判断是不是需要判断图层是否加载或者可查询

        public string m_NodeKey { get; set; } //设置图层NodeKey属性。、
        public string m_NodeText { get; set; }//图层名

       public SelectLayerByTree()
        {
            InitializeComponent();
        }

        public SelectLayerByTree(int m)
        {
            InitializeComponent();
            m_m = m;
        }
        
        private void SelectLayerByTree_Load(object sender, EventArgs e)
        {
            if (Fan.Common.ModField._DicFieldName.Keys.Count == 0)
            {
                Fan.Common.ModField.InitNameDic(Fan.Plugin.ModuleCommon.TmpWorkSpace, Fan.Common.ModField._DicFieldName, "属性对照表");
            }
            if (_DicLayerList == null)
            {
                _DicLayerList = new Dictionary<string, string>();
            }
            //初始化图层树列表
            if (File.Exists(_LayerTreePath))
            {
                if (_LayerTreeXmldoc == null)
                {
                    _LayerTreeXmldoc = new XmlDocument();
                }
                _LayerTreeXmldoc.Load(_LayerTreePath);
                advTreeLayerList.Nodes.Clear();

                //获取Xml的根节点并作为根节点加到UltraTree上
                XmlNode xmlnodeRoot = _LayerTreeXmldoc.DocumentElement;
                XmlElement xmlelementRoot = xmlnodeRoot as XmlElement;

                xmlelementRoot.SetAttribute("NodeKey", "Root");
                string sNodeText = xmlelementRoot.GetAttribute("NodeText");

                //创建并设定树的根节点
                DevExpress.XtraTreeList.Nodes.TreeListNode treenodeRoot = new DevExpress.XtraTreeList.Nodes.TreeListNode();
                //treenodeRoot.Name = "Root";
                //treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
                //treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                this.advTreeLayerList.Nodes.Add(treenodeRoot);

                //treenodeRoot.Image = this.ImageList.Images["Root"];
                InitLayerTreeByXmlNode(treenodeRoot, xmlnodeRoot);


            }
        }
        //根据配置文件显示图层树
        private void InitLayerTreeByXmlNode(DevExpress.XtraTreeList.Nodes.TreeListNode treenode, XmlNode xmlnode)
        {

            for (int iChildIndex = 0; iChildIndex < xmlnode.ChildNodes.Count; iChildIndex++)
            {
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
                if (!Fan.Plugin.ModuleCommon.ListUserdataPriID.Contains(sNodeKey))
                {
                    continue;
                }
                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                DevExpress.XtraTreeList.Nodes.TreeListNode treenodeChild = new DevExpress.XtraTreeList.Nodes.TreeListNode();
                //treenodeChild.Name = sNodeKey;
                //treenodeChild.Text = sNodeText;

                //treenodeChild.DataKey = xmlElementChild;
                treenodeChild.Tag = xmlElementChild.Name;
                

                treenode.Nodes.Add(treenodeChild);

                //递归
                if (xmlElementChild.Name != "Layer")
                {
                    InitLayerTreeByXmlNode(treenodeChild, xmlElementChild as XmlNode);
                }

                InitializeNodeImage(treenodeChild);
            }

        }
        /// <summary>
        /// 通过传入节点的tag，选择对应的图标        
        /// </summary>
        /// <param name="treenode"></param>
        private void InitializeNodeImage(DevExpress.XtraTreeList.Nodes.TreeListNode treenode)
        {
            //switch (treenode.Tag.ToString())
            //{
            //    case "Root":
            //        treenode.Image = this.ImageList.Images["Root"];
            //        treenode.CheckBoxVisible = false;
            //        break;
            //    case "SDE":
            //        treenode.Image = this.ImageList.Images["SDE"];
            //        break;
            //    case "PDB":
            //        treenode.Image = this.ImageList.Images["PDB"];
            //        break;
            //    case "FD":
            //        treenode.Image = this.ImageList.Images["FD"];
            //        break;
            //    case "FC":
            //        treenode.Image = this.ImageList.Images["FC"];
            //        break;
            //    case "TA":
            //        treenode.Image = this.ImageList.Images["TA"];
            //        break;
            //    case "DIR":
            //        treenode.Image = this.ImageList.Images["DIR"];
            //        //treenode.CheckBoxVisible = false;
            //        break;
            //    case "DataDIR":
            //        treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
            //        break;
            //    case "DataDIR&AllOpened":
            //        treenode.Image = this.ImageList.Images["DataDIROpen"];
            //        break;
            //    case "DataDIR&Closed":
            //        treenode.Image = this.ImageList.Images["DataDIRClosed"];
            //        break;
            //    case "DataDIR&HalfOpened":
            //        treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
            //        break;
            //    case "Layer":
            //        XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
            //        if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
            //        {
            //            string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

            //            switch (strFeatureType)
            //            {
            //                case "esriGeometryPoint":
            //                    treenode.Image = this.ImageList.Images["_point"];
            //                    break;
            //                case "esriGeometryPolyline":
            //                    treenode.Image = this.ImageList.Images["_line"];
            //                    break;
            //                case "esriGeometryPolygon":
            //                    treenode.Image = this.ImageList.Images["_polygon"];
            //                    break;
            //                case "esriFTAnnotation":
            //                    treenode.Image = this.ImageList.Images["_annotation"];
            //                    break;
            //                case "esriFTDimension":
            //                    treenode.Image = this.ImageList.Images["_Dimension"];
            //                    break;
            //                case "esriGeometryMultiPatch":
            //                    treenode.Image = this.ImageList.Images["_MultiPatch"];
            //                    break;
            //                default:
            //                    treenode.Image = this.ImageList.Images["Layer"];
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            treenode.Image = this.ImageList.Images["Layer"];
            //        }
            //        break;
            //    case "RC":
            //        treenode.Image = this.ImageList.Images["RC"];
            //        break;
            //    case "RD":
            //        treenode.Image = this.ImageList.Images["RD"];
            //        break;
            //    case "SubType":
            //        treenode.Image = this.ImageList.Images["SubType"];
            //        break;
            //    default:
            //        break;
            //}//end switch
        }


      
        //双击选择叶子节点 
       // private void advTreeLayerList_NodeDoubleClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
       // {
       //     m_NodeKey = "";
       //     if (e.Node == null)
       //         return;
       //     if (e.Node.Tag.ToString() != "Layer")//不是叶子节点 返回
       //     {
       //         return;
       //     }

       //     GetNodeKey(e.Node);
       //     if (string.IsNullOrEmpty(m_NodeKey))
       //         return;
       //     this.DialogResult = DialogResult.OK;
       //     this.Close();
       //}

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //m_NodeKey = "";
            //if (advTreeLayerList.SelectedNode == null)
            //    return;
            //if (advTreeLayerList.SelectedNode.Tag.ToString() != "Layer")//不是叶子节点 返回
            //{
            //    return;
            //}

            //GetNodeKey(advTreeLayerList.SelectedNode);
            //if (string.IsNullOrEmpty(m_NodeKey))
            //    return;
            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }


        //通过NODE 得到NODYKEY
        private void GetNodeKey(DevExpress.XtraTreeList.Nodes.TreeListNode Node)
        {
            labelErr.Text = "";
            XmlNode xmlnode = (XmlNode)Node.Tag;
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
                    if (xmlelement["IsQuery"].Value== "False")
                    {
                        labelErr.Text = "该图层不可查询!";
                        return;
                    }
                }
            }
            if (xmlelement.HasAttribute("NodeKey"))
            {
                m_NodeKey = xmlelement.GetAttribute("NodeKey");
                //m_NodeText = Node.Text;
            }
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; ;
            this.Close();
        }

    }
}
