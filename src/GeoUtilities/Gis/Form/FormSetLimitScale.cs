using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public partial class FormSetLimitScale : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.Node _node = null;    //用户所要更改的节点
        private string _LayerTreexmlpath = "";
        private IMapControlDefault _MapControl = null;
       
        public FormSetLimitScale()
        {
            InitializeComponent();
        }
        public FormSetLimitScale( DevComponents.AdvTree.Node pnode,IMapControlDefault pMapControl,string LayerTreeXmlPath)
        {
            InitializeComponent();
            _node = pnode;
            _MapControl = pMapControl;
            _LayerTreexmlpath = LayerTreeXmlPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            double dMinScale = 0;
            double dMaxScale = 0;
            if (rdbLimitScale.Checked)
            {
                if (txtMinScale.Text != "")
                {
                    string strMin = txtMinScale.Text;
                    try
                    {
                        dMinScale = double.Parse(strMin);
                    }
                    catch
                    { }
                }
                if (txtMaxScale.Text != "")
                {
                    string strMax = txtMaxScale.Text;
                    try
                    {
                        dMaxScale = double.Parse(strMax);
                    }
                    catch
                    { }
                }
            }
            //避免在代码中频繁打开XML文档，执行函数前打开一次
            XmlDocument pXmldoc = new XmlDocument();
            if (_LayerTreexmlpath != "")
            {
                pXmldoc.Load(_LayerTreexmlpath);
            }
            SetLimitScaleToNode(_node,_MapControl,dMinScale,dMaxScale,pXmldoc);
            if (_LayerTreexmlpath != "")
            {
                pXmldoc.Save(_LayerTreexmlpath);
            }
            pXmldoc = null;
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }
        private void SetLimitScaleToNode(DevComponents.AdvTree.Node pNode,IMapControlDefault pMapControl,double dMinScale,double dMaxScale,XmlDocument pXmldoc)
        {
            if (pNode.Nodes.Count > 0)
            {
                for (int i = 0; i < pNode.Nodes.Count; i++)
                {
                    DevComponents.AdvTree.Node pTmpNode = pNode.Nodes[i];
                    SetLimitScaleToNode(pTmpNode, pMapControl, dMinScale, dMaxScale, pXmldoc);
                }
            }
            else
            {
                if (pNode.Tag != null)
                {
                    string strTag = pNode.Tag.ToString();
                    if (strTag.Contains("Layer"))
                    {
                        string strNodeKey = pNode.Name;
                        ILayer pLayer = SysCommon.ModuleMap.GetLayerByNodeKey(null, _MapControl.Map, strNodeKey, null);
                        if (pLayer != null)
                        {
                            pLayer.MinimumScale = dMinScale;
                            pLayer.MaximumScale = dMaxScale;
                        }
                        //为了卸载并加载后，比例尺也使用新比例尺，修改XML中的信息
                        if (pXmldoc != null)
                        {                            
                            string strSearch = "//Layer[@NodeKey='" + strNodeKey + "']";
                            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
                            if (pXmlnode == null)
                            {
                                return;
                            }
                            XmlNode nodeShow = pXmlnode["AboutShow"];
                            //为AboutShow节点写属性
                            XmlElement eleShow = nodeShow as XmlElement;
                            if (nodeShow == null)
                            {
                                eleShow = pXmldoc.CreateElement("AboutShow");
                                nodeShow = pXmlnode.AppendChild(eleShow as XmlNode);
                            }
                            if (eleShow != null)
                            {
                                eleShow.SetAttribute("MaxScale", dMaxScale.ToString());
                                eleShow.SetAttribute("MinScale", dMinScale.ToString());

                            }
                            //added by chulili 20110630 非常重要，只有这样关联上，图层节点才能在视图浏览中正确显示
                            //pNode.DataKey = pXmlnode as object;
                            SysCommon.ModuleMap.SetDataKey(pNode, pXmlnode);
                        }
                        
                    }
                }
            }
        }
        private void rdbLimitScale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbLimitScale.Checked)//选择限制比例尺
            {
                labelMaxScale.Enabled = true;
                labelMinScale.Enabled = true;
                txtMaxScale.Enabled = true;
                txtMinScale.Enabled = true;
            }
            else//任何比例尺都可见
            {
                labelMaxScale.Enabled = false;
                labelMinScale.Enabled = false;
                txtMaxScale.Enabled = false;
                txtMinScale.Enabled = false;  
            }
        }

        private void txtMinScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }


        private void txtMaxScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }

        private void FormSetLimitScale_Load(object sender, EventArgs e)
        {
            if (_node != null)
            {
                if (_node.Tag != null)
                {
                    string strTag = _node.Tag.ToString();
                    if (strTag.ToLower().Contains("layer"))
                    {
                        if (_MapControl.CustomProperty != null)
                        {
                            ILayer pLayer = _MapControl.CustomProperty as ILayer;
                            double dMaxScale = pLayer.MaximumScale;
                            double dMinScale = pLayer.MinimumScale;
                            if (dMaxScale > 0 || dMinScale > 0)
                            {
                                rdbLimitScale.Select();
                                txtMaxScale.Text = dMaxScale.ToString();
                                txtMinScale.Text = dMinScale.ToString();
                                
                            }
                        }
                    }
                }
            }
        }
       
    }
}
