using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using SysCommon.Gis;
using System.Xml;
using System.IO;

namespace GeoSysSetting
{
    public partial class FormFullMapConfig : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace _TmpWorkSpace = null;//业务库工作空间
        private string _LayerTreePath = Application.StartupPath + "\\..\\res\\xml\\临时全图图层树.xml";     //图层目录文件路径
        private XmlDocument _LayerTreeXmldoc = null;
        private string _SelectLayerKey = "";
        public FormFullMapConfig(IWorkspace pTmpWorkSpace)
        {
            InitializeComponent();
            _TmpWorkSpace = pTmpWorkSpace;
            if (pTmpWorkSpace != null)
            {
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(pTmpWorkSpace, _LayerTreePath);
            }
        }

        private void FormFullMapConfig_Load(object sender, EventArgs e)
        {
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
                DevComponents.AdvTree.Node treenodeRoot = new DevComponents.AdvTree.Node();
                treenodeRoot.Name = "Root";
                treenodeRoot.Text = sNodeText;

                treenodeRoot.Tag = "Root";
                treenodeRoot.DataKey = xmlelementRoot;
                treenodeRoot.Expanded = true;
                this.advTreeLayerList.Nodes.Add(treenodeRoot);

                treenodeRoot.Image = this.ImageList.Images["Root"];
                InitLayerTreeByXmlNode(treenodeRoot, xmlnodeRoot);


            }
            string strMinX = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "X最小值");//MinX yjl20120813 modify 
            string strMinY = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "Y最小值");//MinY
            string strMaxX = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "X最大值");//MaxX
            string strMaxY = SysCommon.ModSysSetting.GetSysSettingValue(Plugin.ModuleCommon.TmpWorkSpace, "Y最大值");//MaxY
            txtBoxXmax.Text = strMaxX; txtBoxXmax.Enabled = false;
            txtBoxXmin.Text = strMinX; txtBoxXmin.Enabled = false;
            txtBoxYmax.Text = strMaxY; txtBoxYmax.Enabled = false;
            txtBoxYmin.Text = strMinY; txtBoxYmin.Enabled = false;
            
        }
        //根据配置文件显示图层树
        private void InitLayerTreeByXmlNode(DevComponents.AdvTree.Node treenode, XmlNode xmlnode)
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

                string sNodeText = xmlElementChild.GetAttribute("NodeText");

                DevComponents.AdvTree.Node treenodeChild = new DevComponents.AdvTree.Node();
                treenodeChild.Name = sNodeKey;
                treenodeChild.Text = sNodeText;

                treenodeChild.DataKey = xmlElementChild;
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
        private void InitializeNodeImage(DevComponents.AdvTree.Node treenode)
        {
            switch (treenode.Tag.ToString())
            {
                case "Root":
                    treenode.Image = this.ImageList.Images["Root"];
                    treenode.CheckBoxVisible = false;
                    break;
                case "SDE":
                    treenode.Image = this.ImageList.Images["SDE"];
                    break;
                case "PDB":
                    treenode.Image = this.ImageList.Images["PDB"];
                    break;
                case "FD":
                    treenode.Image = this.ImageList.Images["FD"];
                    break;
                case "FC":
                    treenode.Image = this.ImageList.Images["FC"];
                    break;
                case "TA":
                    treenode.Image = this.ImageList.Images["TA"];
                    break;
                case "DIR":
                    treenode.Image = this.ImageList.Images["DIR"];
                    //treenode.CheckBoxVisible = false;
                    break;
                case "DataDIR":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "DataDIR&AllOpened":
                    treenode.Image = this.ImageList.Images["DataDIROpen"];
                    break;
                case "DataDIR&Closed":
                    treenode.Image = this.ImageList.Images["DataDIRClosed"];
                    break;
                case "DataDIR&HalfOpened":
                    treenode.Image = this.ImageList.Images["DataDIRHalfOpen"];
                    break;
                case "Layer":
                    XmlNode xmlnodeChild = (XmlNode)treenode.DataKey;
                    if (xmlnodeChild != null && xmlnodeChild.Attributes["FeatureType"] != null)
                    {
                        string strFeatureType = xmlnodeChild.Attributes["FeatureType"].Value;

                        switch (strFeatureType)
                        {
                            case "esriGeometryPoint":
                                treenode.Image = this.ImageList.Images["_point"];
                                break;
                            case "esriGeometryPolyline":
                                treenode.Image = this.ImageList.Images["_line"];
                                break;
                            case "esriGeometryPolygon":
                                treenode.Image = this.ImageList.Images["_polygon"];
                                break;
                            case "esriFTAnnotation":
                                treenode.Image = this.ImageList.Images["_annotation"];
                                break;
                            case "esriFTDimension":
                                treenode.Image = this.ImageList.Images["_Dimension"];
                                break;
                            case "esriGeometryMultiPatch":
                                treenode.Image = this.ImageList.Images["_MultiPatch"];
                                break;
                            default:
                                treenode.Image = this.ImageList.Images["Layer"];
                                break;
                        }
                    }
                    else
                    {
                        treenode.Image = this.ImageList.Images["Layer"];
                    }
                    break;
                case "RC":
                    treenode.Image = this.ImageList.Images["RC"];
                    break;
                case "RD":
                    treenode.Image = this.ImageList.Images["RD"];
                    break;
                case "SubType":
                    treenode.Image = this.ImageList.Images["SubType"];
                    break;
                default:
                    break;
            }//end switch
        }

        private void advTreeLayerList_AfterNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            DevComponents.AdvTree.AdvTree tree = sender as DevComponents.AdvTree.AdvTree;
            DevComponents.AdvTree.Node node = tree.SelectedNode;
            switch (node.Tag.ToString())
            {
                case "DIR":
                case "DataDIR":
                case "Root":
                    break;
                case "Layer":
                    if (_SelectLayerKey != node.Name)
                    {
                        string strNodeKey = node.Name;
                        IFeatureClass pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(_TmpWorkSpace, _LayerTreePath, strNodeKey);
                        GetFullMapConfig(pFeatureClass);
                        _SelectLayerKey = node.Name;
                    }
                    break;
            }


            
        }
        //从要素类中获取全图范围
        private void GetFullMapConfig(IFeatureClass pFeacls)
        {
            if(pFeacls ==null)
            {
                return;
            }
            IGeoDataset  pDataset = pFeacls as IGeoDataset;
            if (pDataset != null)
            {
                IEnvelope pENV= pDataset.Extent;
                try
                {
                    txtBoxXmaxNew.Text = pENV.XMax.ToString();
                    txtBoxXminNew.Text = pENV.XMin.ToString();
                    txtBoxYmaxNew.Text = pENV.YMax.ToString();
                    txtBoxYminNew.Text = pENV.YMin.ToString();
                }
                catch
                { }
            }
        }
        private void SaveFullMapConfig()
        {
            if (txtBoxYminNew.Text != "")
            {
                try
                {
                    double dtmp = double.Parse(txtBoxYminNew.Text);
                    SaveConfig("Y最小值", txtBoxYminNew.Text);
                }
                catch
                { }
            }
            if (txtBoxYmaxNew.Text != "")
            {
                try
                {
                    double dtmp = double.Parse(txtBoxYmaxNew.Text);
                    SaveConfig("Y最大值", txtBoxYmaxNew.Text);
                }
                catch
                { }
            }
            if (txtBoxXminNew.Text != "")
            {
                try
                {
                    double dtmp = double.Parse(txtBoxXminNew.Text);
                    SaveConfig("X最小值", txtBoxXminNew.Text);
                }
                catch
                { }
            }
            if (txtBoxXmaxNew.Text != "")
            {
                try
                {
                    double dtmp = double.Parse(txtBoxXmaxNew.Text);
                    SaveConfig("X最大值", txtBoxXmaxNew.Text);
                }
                catch
                { }
            }
        }
        private void SaveConfig(string strConfigName,string strConfigValue)
        {
            SysGisTable mSystable = new SysCommon.Gis.SysGisTable(_TmpWorkSpace);
            Dictionary<string, object> pDic = new Dictionary<string, object>();
            //参数名
            pDic.Add("SETTINGNAME", strConfigName);

            pDic.Add("SETTINGVALUE", strConfigValue);

            Exception err = null;
            bool bRes = false;
            if (mSystable.ExistData("SYSSETTING", "SETTINGNAME='" + strConfigName + "'"))
            {
                bRes = mSystable.UpdateRow("SYSSETTING", "SETTINGNAME='" + strConfigName + "'", pDic, out err);
            }
            else
            {
                bRes = mSystable.NewRow("SYSSETTING", pDic, out err);
            }


            mSystable = null;
 
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveFullMapConfig();
            this.DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
