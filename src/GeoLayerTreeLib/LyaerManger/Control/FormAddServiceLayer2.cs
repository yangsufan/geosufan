using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SysCommon.Gis;
using SysCommon.Error;
using SysCommon.Authorize;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using ESRI.ArcGIS.GISClient;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;

namespace GeoLayerTreeLib.LayerManager
{
    /// <summary>
    /// 通过列举服务地址下所有的服务，来添加图层
    /// </summary>
    public partial class FormAddServiceLayer2 : DevComponents.DotNetBar.Office2007Form
    {
        bool isUpdate = false;                  //判断当前是否为更新
        public string _Layername = "";
        public string _ServiceLocation = "";
        public string _ServiceName = "";
        private DevComponents.AdvTree.Node _curNode = null;
        private UcDataLib _pUC = null;  //added by chulili 20110909 添加控件变量，用来调用卸载节点的函数，节点修改了，老节点对应的图层要从视图中卸载掉
        private bool _isAdd = true;
        private DevComponents.AdvTree.Node _node = null;
        private ImageList _ImgList;
        private string _xmlpath = "";
        private IWorkspace _tmpWorkspace = null;

        private string _ServiceType = "";
        public DevComponents.AdvTree.Node CurNode
        {
            get { return _curNode; }
        }
        public FormAddServiceLayer2()
        {
            InitializeComponent();
        }
        public FormAddServiceLayer2(UcDataLib pUC, string xmlpath, IWorkspace pWKS, DevComponents.AdvTree.Node pnode, bool isadd, ImageList pList, string strType)
        {
            InitializeComponent();
            _pUC = pUC;
            _xmlpath = xmlpath;
            _tmpWorkspace = pWKS;
            //cyf 20110613 add
            if (_tmpWorkspace == null) return;
            //end
            _isAdd = isadd;
            _node = pnode;
            _ImgList = pList;
            _ServiceType = strType;
        }
        public FormAddServiceLayer2(string Layername, string ServiceLocation, string ServiceName)
        {
            InitializeComponent();
            _Layername = Layername;
            _ServiceLocation = ServiceLocation;
            _ServiceName = ServiceName;
            isUpdate = true;
        }

        private void FormAddServiceLayer_Load(object sender, EventArgs e)
        {
            if (isUpdate)
            {
                btnAddServiceLayer.Text = "修改";
            }
            else
            {
                btnAddServiceLayer.Text = "添加";
            }
            this.txtLayerName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddServiceLayer_Click(object sender, EventArgs e)
        {
            if (this.txtLayerName.Text == "")
            {
                MessageBox.Show("请输入图层名称!");
                return;
            }
            if (this.txtServiceLocation.Text == "")
            {
                MessageBox.Show("请输入服务地址!");
                return;
            }
            if (advTree1.SelectedNode == null)
            {
                MessageBox.Show("请选择一个服务节点!");
                return;
            }
            XmlDocument layerxmldoc = new XmlDocument();
            layerxmldoc.Load(_xmlpath);

            DevComponents.AdvTree.Node addSubNode = new DevComponents.AdvTree.Node();
            addSubNode.Text = this.txtLayerName.Text;
            addSubNode.Tag = "Layer";
            string nodekey = Guid.NewGuid().ToString();
            addSubNode.Name = nodekey;

            addSubNode.Image = _ImgList.Images["Layer"];
            addSubNode.CheckBoxVisible = true;

            XmlElement childele = layerxmldoc.CreateElement("Layer");

            childele.SetAttribute("NodeText", this.txtLayerName.Text);
            childele.SetAttribute("NodeKey", nodekey);
            childele.SetAttribute("Enabled", "true");
            childele.SetAttribute("ConnectKey", this.txtServiceLocation.Text);
            childele.SetAttribute("FeatureDatasetName", advTree1.SelectedNode.Text);//this.txtServiceName.Text
            if (_ServiceType == "WMS")
            {
                childele.SetAttribute("DataType", "WMSSERVICE");
            }
            else
            {
                childele.SetAttribute("DataType", "SERVICE");
            }
            childele.SetAttribute("FeatureType", "");
            childele.SetAttribute("Code", "");

            childele.SetAttribute("Load", "0");

            addSubNode.CheckState = CheckState.Unchecked;
            childele.SetAttribute("View", "1");

            string strTag = _node.Tag.ToString();
            string NodeName = strTag;
            if (NodeName.Contains("DataDIR"))//yjl20120814 add because 'DataDIR&AllOpened'
                NodeName = "DataDIR";
            string strSearch = "//" + NodeName + "[@NodeKey='" + _node.Name + "']";
            XmlNode pxmlnode = layerxmldoc.SelectSingleNode(strSearch);
            pxmlnode.AppendChild(childele as XmlNode);
            XmlNode pChildnode = childele as XmlNode;

            XmlElement eleShow = null;
            XmlNode nodeShow = null;
            eleShow = layerxmldoc.CreateElement("AboutShow");
            nodeShow = pChildnode.AppendChild(eleShow as XmlNode);

            eleShow.SetAttribute("Renderer", "");
            eleShow.SetAttribute("MaxScale", "");
            eleShow.SetAttribute("MinScale", "");

            layerxmldoc.Save(_xmlpath);

            XmlNode pTMPnode = layerxmldoc.SelectSingleNode("//Layer[@NodeKey='" + nodekey + "']");
            addSubNode.DataKey = pTMPnode as object;
            _node.Nodes.Add(addSubNode);//在树图中添加advnode
            _node.DataKey = pTMPnode.ParentNode as object;

            _curNode = addSubNode;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //pHostOrUrl   服务地址
        public void GetWMSServerLayer(string pHostOrUrl, bool pIsLAN)
        {
            IWMSConnectionFactory pWmsFac = new WMSConnectionFactory();
            IWMSConnection pWmsConn = null;
            IPropertySet pProSet = new PropertySet();
            if (pIsLAN)
                pProSet.SetProperty("machine", pHostOrUrl);
            else
                pProSet.SetProperty("url", pHostOrUrl);
            ///连接服务                
            try
            {
                pWmsConn = pWmsFac.Open(pProSet, 0, null);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pWmsFac);
                pWmsFac = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pProSet);
                pProSet = null;
                if (pWmsConn != null)
                {
                    ///获取所有的服务名称

                    IWMSConnectionName pWmsConnectionName = pWmsConn.FullName as IWMSConnectionName;
                    ILayerFactory pLayerFactory = new EngineWMSMapLayerFactoryClass();
                    if (pLayerFactory.get_CanCreate(pWmsConnectionName))
                    {
                        IEnumLayer pEnumLayer = pLayerFactory.Create(pWmsConnectionName);
                        pEnumLayer.Reset();
                        ILayer pLayer = pEnumLayer.Next();
                        while (pLayer != null)
                        {
                            if (pLayer is IWMSMapLayer)
                            {
                                IWMSMapLayer pWmsMapLayer = pLayer as IWMSMapLayer;
                                IWMSGroupLayer pWmsGroupLayer = pWmsMapLayer as IWMSGroupLayer;
                                DevComponents.AdvTree.Node nService = new DevComponents.AdvTree.Node();
                                nService.Text = pLayer.Name;
                                int iService = advTree1.Nodes.Add(nService);//一级服务名
                                for (int j = 0; j < pWmsGroupLayer.Count; j++)
                                {
                                    ILayer pTmpLayer = pWmsGroupLayer.get_Layer(j);
                                    IWMSGroupLayer pTmpWmsGroupLayer = pTmpLayer as IWMSGroupLayer;
                                    if (pTmpWmsGroupLayer != null)
                                    {
                                        DevComponents.AdvTree.Node nMap = new DevComponents.AdvTree.Node();
                                        nMap.Text = pTmpLayer.Name;
                                        int iMap = nService.Nodes.Add(nMap);//二级地图名

                                        for (int k = 0; k < pTmpWmsGroupLayer.Count; k++)
                                        {
                                            ILayer pTmpTmplayer = pTmpWmsGroupLayer.get_Layer(k);
                                            DevComponents.AdvTree.Node nLayer = new DevComponents.AdvTree.Node();
                                            nLayer.Text = pTmpTmplayer.Name;
                                            nMap.Nodes.Add(nLayer);//三级图层名
                                        }
                                    }
                                }
                            }
                            pLayer = pEnumLayer.Next();
                        }
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pLayerFactory);
                    pLayerFactory = null;
                }
            }
            catch
            { }
            advTree1.ExpandAll();
        }

        private void btnGetService_Click(object sender, EventArgs e)
        {
            if (this.txtServiceLocation.Text == "")
            {
                MessageBox.Show("请输入服务地址!");
                return;
            }
            advTree1.Nodes.Clear();
            GetWMSServerLayer(txtServiceLocation.Text, false);
        }

        private void txtServiceLocation_TextChanged(object sender, EventArgs e)
        {
            if (this.txtServiceLocation.Text != "")
            {
                advTree1.Nodes.Clear();
                GetWMSServerLayer(txtServiceLocation.Text, false);
            }
        }

    }
}