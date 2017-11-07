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

namespace GeoLayerTreeLib
{
    public partial class FormModifyServiceLayer : DevComponents.DotNetBar.Office2007Form
    {
        public string _Layername = "";
        private DevComponents.AdvTree.Node _curNode = null;
        private string _xmlpath = "";
        private IWorkspace _tmpWorkspace = null;
        public bool _Load = false;
        public bool _View = false;
        public bool _Changed = false;
        private string _Key = "";

        private string _ServiceType = "";
        public DevComponents.AdvTree.Node CurNode
        {
            get { return _curNode; }
        }
        public FormModifyServiceLayer()
        {
            InitializeComponent();
        }
        public FormModifyServiceLayer(string xmlPath,IWorkspace pTmpwks,DevComponents.AdvTree.Node pNode)
        {
            _tmpWorkspace = pTmpwks;
            _xmlpath = xmlPath;
            _curNode = pNode;
            InitializeComponent();
        }
        //public FormModifyServiceLayer(UcDataLib pUC,string xmlpath,IWorkspace  pWKS, DevComponents.AdvTree.Node pnode, bool isadd, ImageList pList,string strType)
        //{
        //    InitializeComponent();
        //    _pUC = pUC;
        //    _xmlpath = xmlpath;
        //    _tmpWorkspace = pWKS;
        //    //cyf 20110613 add
        //    if (_tmpWorkspace == null) return;
        //    //end
        //    _isAdd = isadd;
        //    _node = pnode;
        //    _ImgList = pList;
        //    _ServiceType = strType;
        //}
        //public FormAddServiceLayer(string Layername, string ServiceLocation, string ServiceName)
        //{
        //    InitializeComponent();
        //    _Layername = Layername;
        //    _ServiceLocation = ServiceLocation;
        //    _ServiceName = ServiceName;
        //    isUpdate = true;
        //}

        private void FormModifyServiceLayer_Load(object sender, EventArgs e)
        {
            XmlNode pXmlNode = _curNode.DataKey as XmlNode;
            if (pXmlNode == null)
            {
                return;
            }
            XmlElement pXmlEle = pXmlNode as XmlElement;
            string strLayerName = pXmlEle.GetAttribute("NodeText");
            string strLocation = pXmlEle.GetAttribute("ConnectKey");
            string strSelectnode = pXmlEle.GetAttribute("FeatureDatasetName");
            string strLoad = pXmlEle.GetAttribute("Load");
            string strView = pXmlEle.GetAttribute("View");
            _Key = pXmlEle.GetAttribute("NodeKey");
            this.txtLayerName.Text = strLayerName;
            this.txtServiceLocation.Text = strLocation;
            this.txtServiceName.Text = strSelectnode;
            this.txtServiceLocation.Enabled = false;
            this.txtServiceName.Enabled = false;

            if (strLoad == "1")
            {
                this.chkLoad.Checked = true;
                _Load = true;
            }
            else
            {
                this.chkLoad.Checked = false;
            }
            if (strView == "1")
            {
                this.chkView.Checked = true;
                _View = true;
            }
            else
            {
                this.chkView.Checked = false;
            }
            this.txtLayerName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {

            if (_Layername != this.txtLayerName.Text)
            {
                _Layername = this.txtLayerName.Text;
                _Changed = true;
            }
            if (this.chkLoad.Checked != _Load)
            {
                _Load = this.chkLoad.Checked;
                _Changed = true;
            }
            if (this.chkView.Checked != _View)
            {
                _View = this.chkView.Checked;
                _Changed = true;
            }
            if (this.chkLoad.Checked)
            {
                _curNode.Checked = true;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_xmlpath);
            string strSearch = "//Layer[@NodeKey='" + _Key + "']";
            XmlNode pxmlnode = pXmldoc.SelectSingleNode(strSearch);
            XmlElement pXmlele = pxmlnode as XmlElement;
            pXmlele.SetAttribute("NodeText", _Layername);
            if (_Load)
            {
                pXmlele.SetAttribute("Load", "1");
            }
            else
            {
                pXmlele.SetAttribute("Load", "0");
            }
            if (_View)
            {
                pXmlele.SetAttribute("View", "1");
            }
            else
            {
                pXmlele.SetAttribute("View", "0");
            }
            
            pXmldoc.Save(_xmlpath);
            _curNode.DataKey = pxmlnode as object;
            this.DialogResult = DialogResult.OK;
            this.Close();
            
        }
    }
}