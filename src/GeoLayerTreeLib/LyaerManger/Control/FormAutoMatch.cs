using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;

namespace GeoLayerTreeLib.LayerManager
{
    public partial class FormAutoMatch : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace _tmpWorkspace = null;        //工作库连接
        private DevComponents.AdvTree.Node _node = null;    //用户所要更改的节点
        private string _xmlpath = "";
        private UcDataLib _pUC = null;  //added by chulili 20110909 添加控件变量，用来调用卸载节点的函数，节点修改了，老节点对应的图层要从视图中卸载掉
        public FormAutoMatch()
        {
            InitializeComponent();
        }
        public FormAutoMatch(UcDataLib pUC, string xmlpath, IWorkspace pWKS, DevComponents.AdvTree.Node pnode)
        {
            InitializeComponent();
            _pUC = pUC;
            _xmlpath = xmlpath;
            _tmpWorkspace = pWKS;
            _node = pnode;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_pUC != null)
            {
                _pUC.RemoveNodeFromMap(_node );
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_xmlpath);
            AutoMatchRender(_node, pXmldoc);
            pXmldoc.Save(_xmlpath);
            SysCommon.ModSysSetting.IsLayerTreeChanged = true;
            if (_pUC != null)
            {
                _pUC.AddNodeToMap(_node );
            }
            this.Hide();
            this.DialogResult = DialogResult.OK;
        }
        //自动匹配符号，递归调用
        private void AutoMatchRender(DevComponents.AdvTree.Node pNode, XmlDocument pXmldoc)
        {
            Exception eError = null;
            if (pNode == null)
                return;
            //获取xml节点名称
            string strtag = pNode.Tag.ToString();
            string strNodeName = strtag;
            //如果是数据集节点，需处理一下,tag赋值后会有变动
            if (strtag.Contains("DataDIR"))
            {
                strNodeName = "DataDIR";
            }
            switch (strNodeName)
            {
                case "Root":
                case "DIR":
                case "DataDIR":
                    {
                        if (pNode.Nodes.Count > 0)
                        {
                            for (int i = 0; i < pNode.Nodes.Count; i++)
                            {
                                DevComponents.AdvTree.Node pTmpnode = pNode.Nodes[i];
                                AutoMatchRender(pTmpnode, pXmldoc);
                                

                            }
                            //string strNodeKey = pNode.Name;
                            //XmlNode pNewXmlNode = pXmldoc.SelectSingleNode("//" + strNodeName + "[@NodeKey='" + strNodeKey + "']");
                            //if (pNewXmlNode != null)
                            //{
                            //    pNode.DataKey = pNewXmlNode as object;
                            //}
                        }
                    }
                    break;
                case "Layer":
                    {
                        //获取xml中对应节点
                        ModuleMap.AutoMatchLayerConfig(_tmpWorkspace, pNode, pXmldoc, this.chkAutoMatchScale.Checked, this.chkAutoMatchRender.Checked, this.chkAutoMatchLabel.Checked, this.chkAutoMatchFilter.Checked);
                    }
                    break;

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FormAutoMatch_Load(object sender, EventArgs e)
        {
            chkAutoMatchFilter.Checked = true;
            chkAutoMatchLabel.Checked = true;
            chkAutoMatchRender.Checked = true;
            chkAutoMatchScale.Checked = false;
        }
    }
}
