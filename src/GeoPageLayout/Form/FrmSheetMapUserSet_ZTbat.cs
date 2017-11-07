using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml;
using ESRI.ArcGIS.Carto;
namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110914
    /// 说明：标准图幅地图输出设置窗体-专题图批量
    /// </summary>
    public partial class FrmSheetMapUserSet_ZTbat : DevComponents.DotNetBar.Office2007Form
    {
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        Form pMainForm;
        IMap pMap;
        AxMapControl pAxMapControl;
        string xzq_xianMC;
        private List<DevComponents.AdvTree.Node> dataNode;
        public string ZTMC
        {
            get { return cBoxZT.Text; }
        }
        public FrmSheetMapUserSet_ZTbat(AxMapControl inAxMapControl,Form inForm,string xzqmc)
        {
            InitializeComponent();
            pAxMapControl = inAxMapControl;
            pMainForm = inForm;
            xzq_xianMC = xzqmc;
            //if (advNC != null && advNC.Count > 0)
            //{
            //    foreach(DevComponents.AdvTree.Node advN in advNC)
            //    {
            //        DevComponents.Editors.ComboItem cItem = new DevComponents.Editors.ComboItem();
            //        cItem.Text = advN.Text;
            //        cItem.Tag = advN;
            //        cBoxZT.Items.Add(cItem);
            //    }
            //    if (cBoxZT.Items.Count > 0)
            //        cBoxZT.SelectedIndex = 0;
            //}
            List<string> tdlyZTs = ModGetData.GetZT("TDLY");
            foreach (string tdlyzt in tdlyZTs)
            {
                cBoxZT.Items.Add(tdlyzt);
            }
            if (cBoxZT.Items.Count > 0)
                cBoxZT.SelectedIndex = 0;
            cBoxScale.Items.Add("1:10000");
            cBoxScale.Items.Add("1:50000");
            cBoxScale.SelectedIndex = 0;
        }

        private void txtResolution_TextChanged(object sender, EventArgs e)
        {
 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cBoxScale.Text == ""||cBoxZT.Text=="")
                return;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("选择比例尺为:" + cBoxScale.Text + ",专题为:" + cBoxZT.Text);
            }
            //DevComponents.Editors.ComboItem dataItem = cBoxZT.SelectedItem as DevComponents.Editors.ComboItem;
            //DevComponents.AdvTree.Node dataNode = dataItem.Tag as DevComponents.AdvTree.Node;
            bool isSpecial = ModGetData.IsMapSpecial();
            if (isSpecial)
            {
                pMap = new MapClass();
                ModGetData.AddMapOfByXZQ(pMap, "TDLY", cBoxZT.Text, pAxMapControl.Map, xzq_xianMC);

                ModuleMap.LayersComposeEx(pMap);//图层排序
            }
            else
            {
                IObjectCopy pOC = new ObjectCopyClass();
                pMap = pOC.Copy(pAxMapControl.Map) as IMap;//复制地图
            }
            this.DialogResult = DialogResult.OK;
        }
        public IMap Map
        {
            get { return pMap; }
        }
        public int GetScale
        {
            get { return Convert.ToInt32(cBoxScale.Text.Split(':')[1]); }
        }
        public string GetZTMC
        {
            get { return cBoxZT.Text; }
        }
        private void initMap(IMapLayers pMapLayer, DevComponents.AdvTree.Node advN)
        {
            if (!advN.HasChildNodes && advN.Checked)
            {
                string tag = advN.Tag as string;
                if (tag == "Layer")
                {
                    //获取xml节点
                    if (advN.DataKey != null)
                    {
                        XmlNode layerNode = advN.DataKey as XmlNode;
                        string nodeKey = "";
                        if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                        {
                            nodeKey = layerNode.Attributes["NodeKey"].Value;
                        }
                        ILayer addLayer = ModGetData.GetLayerByNodeKey(pAxMapControl.Map, nodeKey);
                        pMapLayer.InsertLayer(addLayer, false, pMapLayer.LayerCount);
                    }
                }
                else if (tag == "OutLayer")
                { }
                return;
            }
            else if (advN.HasChildNodes)
            {
                foreach (DevComponents.AdvTree.Node avN in advN.Nodes)
                {
                    if (avN.Text.Contains(xzq_xianMC))
                    {
                        ILayer pLyr = getLayer(avN);
                        if (pLyr != null)
                            pMapLayer.InsertLayer(pLyr, false, pMapLayer.LayerCount);
                    }
                }
            }

        }
        private void initMap123(IMapLayers pMapLayer, DevComponents.AdvTree.Node advN)
        {
            if (!advN.HasChildNodes && advN.Checked)
            {
                string tag = advN.Tag as string;
                if (tag == "Layer")
                {
                    //获取xml节点
                    if (advN.DataKey != null)
                    {
                        XmlNode layerNode = advN.DataKey as XmlNode;
                        string nodeKey = "";
                        if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                        {
                            nodeKey = layerNode.Attributes["NodeKey"].Value;
                        }
                        ILayer addLayer = ModGetData.GetLayerByNodeKey(pAxMapControl.Map, nodeKey);
                        pMapLayer.InsertLayer(addLayer, false, pMapLayer.LayerCount);
                    }
                }
                else if (tag == "OutLayer")
                { }
                return;
            }
            else if(advN.HasChildNodes)
            {
                foreach (DevComponents.AdvTree.Node avN in advN.Nodes)
                {
                    if (avN.Text.Contains(xzq_xianMC))
                    {
                        ILayer pLyr = getLayer(avN);
                        if (pLyr != null)
                            pMapLayer.InsertLayer(pLyr, false, pMapLayer.LayerCount);
                    }
                }
            }

        }
        private ILayer getLayer(DevComponents.AdvTree.Node advN)//支持两层结构递归调用
        {
            
            if (!advN.HasChildNodes && advN.Checked)
            {
                string tag = advN.Tag as string;
                ILayer addLayer = null; ;
                if (tag == "Layer")
                {
                    //获取xml节点
                    if (advN.DataKey != null)
                    {
                        XmlNode layerNode = advN.DataKey as XmlNode;
                        string nodeKey = "";
                        if ((layerNode as XmlElement).HasAttribute("NodeKey"))
                        {
                            nodeKey = layerNode.Attributes["NodeKey"].Value;
                        }
                        addLayer = ModGetData.GetLayerByNodeKey(pAxMapControl.Map, nodeKey);
                    }
                }
                else if (tag == "OutLayer")
                { }
                return addLayer;//返回路径1
            }
            else if (advN.HasChildNodes)
            {
                IGroupLayer pGpLayer = new GroupLayerClass();
                pGpLayer.Name = advN.Text;
                foreach (DevComponents.AdvTree.Node avN in advN.Nodes)
                {
                    ILayer pLyr = getLayer(avN);
                    if (pLyr != null)
                      pGpLayer.Add(getLayer(avN));
                }
                return pGpLayer as ILayer;//返回路径2
            }
            return null;//返回路径3

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消批量标准分幅出图");
            }
            this.Close();
        }

        
        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void FrmSheetMapUserSet_Load(object sender, EventArgs e)
        {

        }
        //初始化比例尺列表控件
        private void initcBoxScale()
        {
            cBoxScale.Items.Clear();
          string schemaPath = Application.StartupPath + "\\..\\Res\\Xml\\TFHQuery.xml";
            if (!File.Exists(schemaPath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(schemaPath);

                XmlNode xn1 = cXmlDoc.FirstChild;
                XmlNode xn2 = xn1.NextSibling;
                foreach (XmlNode xn in xn2.ChildNodes)
                {
                    string xnattr = xn.Attributes["ItemName"].Value;
                    cBoxScale.Items.Add(xnattr);
                }
                cXmlDoc = null;
                if (cBoxScale.Items.Count > 0)
                    cBoxScale.SelectedIndex = 0;

            }
        }
        private void FrmSheetMapUserSet_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

    }
}
