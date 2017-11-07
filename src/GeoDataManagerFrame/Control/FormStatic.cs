using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Carto;
using System.IO;

using GeoDataCenterFunLib;
namespace GeoDataManagerFrame
{
    public partial class FormStatic : DevComponents.DotNetBar.Office2007Form
    {
        private IMap m_Map;
        private string m_XmlPath = Application.StartupPath + @"\..\Template\StatInfo.Xml";
        public FormStatic()
        {
            InitializeComponent();
            InitStaticTypeFromXml();
        }
        public void InitForm(IMap pMap)
        {
            m_Map = pMap;
            if (pMap == null)
                return;
            
            int iLayerCount = pMap.LayerCount;
            string layername = "";
            this.comboBoxExTopic.Items.Clear();
            for (int ii = 0; ii < iLayerCount; ii++)
            {
                ILayer layer = pMap.get_Layer(ii);
                if (layer is IGroupLayer )
                {
                    layername = layer.Name;
                    this.comboBoxExTopic.Items.Add(layername);
                }
            }
        }

        private void buttonXstatic_Click(object sender, EventArgs e)
        {
            
            ListView.CheckedListViewItemCollection pCollection = this.listViewStatType.CheckedItems;
            if (pCollection.Count == 0)
            {
                MessageBox.Show("未选中任何汇总类型！");
                return;
            }
            if (this.comboBoxExTopic.Text.Equals(""))
            {
                MessageBox.Show("未选中任何专题！");
                return;
            }
            string sTopicName = this.comboBoxExTopic.Text;
            //初始化进度条
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();

            foreach (ListViewItem item in this.listViewStatType.CheckedItems)
            {
                string statType = item.Text;
                string statKey = GetKeyofstatType(statType);

                switch (statKey)
                {
                    case "LandUseCur":
                        vProgress.SetProgress("进行森林资源现状汇总...");
                        ModStatReport.LandUseCurSum(m_Map, sTopicName, m_XmlPath, "LandUseCur", vProgress);
                        break;
                    default:
                        break;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private string  GetKeyofstatType(string StatType)
        {
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc == null)
                return "";
            if (!File.Exists(m_XmlPath))
                return "";
            xmldoc.Load(m_XmlPath);
            string strSearch = "//StatType [@ItemName='"+StatType+"']";
            XmlNode xmlNodetype = xmldoc.SelectSingleNode(strSearch);
            if (xmlNodetype == null)
                return "";
            if (xmlNodetype.NodeType == XmlNodeType.Element)
            {
                XmlElement pEle = xmlNodetype as XmlElement;
                string sKey = pEle.GetAttribute("Key");
                return sKey;
            }
            return "";
        }

        private void InitStaticTypeFromXml()
        {
            XmlDocument xmldoc = new XmlDocument();
            if (xmldoc==null)
                return;
            if (!File.Exists(m_XmlPath))
                return;

            xmldoc.Load(m_XmlPath);
            string strSearchRoot = "//StatRoot";
            string strRootPath = "";
            XmlNode xmlNodeRoot = xmldoc.SelectSingleNode(strSearchRoot);
            if (xmlNodeRoot == null)
                return;
            XmlNodeList xmllist = xmlNodeRoot.ChildNodes as XmlNodeList;
            foreach(XmlNode staticnode in xmllist )
            {
                if (staticnode.NodeType != XmlNodeType.Element)
                    continue;
                XmlElement pEle = staticnode as XmlElement;
                string statictype = pEle.GetAttribute("ItemName");
                this.listViewStatType.Items.Add(statictype);
            }
        }

        private void buttonXQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        //private void comboBoxExTopic_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string TopicName = this.comboBoxExTopic.Text;
        //    string XZQname = "";
        //    int index1, index2;
        //    index1 = TopicName.IndexOf("_")+1;
        //    index2 = TopicName.LastIndexOf("年") - 4;
        //    XZQname = TopicName.Substring(index1,index2-index1);
        //    this.labelXxzq.Text = XZQname;
            
        //}

    }
}