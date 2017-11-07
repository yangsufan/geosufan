using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using ESRI.ArcGIS.Carto;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoPageLayout
{
    public partial class FrmUser : DevComponents.DotNetBar.Office2007Form
    
    {
        private IMap cMap = null;
        private XmlDocument cXmlDoc = null;
        string cPath = Application.StartupPath + "\\..\\Template\\PageLayout.xml";
        string ztqc = "";
        List<string> namelist = new List<string>();
        IWorkspace srcWs = null;
        GeoPageLayout pgpl = null;

        public FrmUser(IMap pMap,GeoPageLayout gpl)
        {
            InitializeComponent();
            cMap = pMap;
            pgpl = gpl;
        }

        private void FrmUser_Load(object sender, EventArgs e)
        {
            this.Text = "出图对话框";
            this.gpPanel1.Text = "出图图层";
            this.labelX1.Text = "专题类型";
            this.labelX2.Text = "行政区";
            this.btnOK.Text = "图形输出";
            this.btnXCancel.Text = "退出";

            //从地图控件获取专题类型并添加到用户设置窗体的combox里
            for (int i = 0; i < cMap.LayerCount; i++)
            {
                if (cMap.get_Layer(i) is IGroupLayer)
                {
                    int idex=cMap.get_Layer(i).Name.IndexOf('_');
                    string ztlx = cMap.get_Layer(i).Name;//.Substring(0, idex);

                    cBoxExZTLX.Items.Add(ztlx);
                   

                }
            }
            cBoxExZTLX.SelectedIndex = 0;

            //ICompositeLayer cmp = cMap.get_Layer(0) as ICompositeLayer;
            //for (int j = 0; j < cmp.Count; j++)
            //{
            //    listView1.Items.Add(cmp.get_Layer(j).Name);
            //}




       

        }

        private void cBoxExZTLX_SelectedIndexChanged(object sender, EventArgs e)
        {

            

            if (cBoxExZTLX.Text == "") return;
            for (int i = 0; i < cMap.LayerCount; i++)
            {
                if (cMap.get_Layer(i) is IGroupLayer)
                {
                    int idex = cMap.get_Layer(i).Name.IndexOf('_');
                    string ztlx = cMap.get_Layer(i).Name;//.Substring(0, idex);

                    if (cBoxExZTLX.Text != ztlx) continue;
                    namelist.Clear();
                    listView1.Items.Clear();
                    ICompositeLayer cmp = cMap.get_Layer(i) as ICompositeLayer;
                    for (int j = 0; j < cmp.Count; j++)
                    {
                        listView1.Items.Add(cmp.get_Layer(j).Name);
                       
                    }

                }
            }

            cXmlDoc = new XmlDocument();
            if (cXmlDoc != null && File.Exists(cPath))
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("SubMapType");

                foreach (XmlNode xn in xnl)
                {
                    if ((xn as XmlElement).GetAttribute("ItemName").ToString() != "森林资源现状图") continue;
                    for (int i = 0; i < xn.ChildNodes.Count; i++)
                    {
                        for (int k = 0; k < listView1.Items.Count; k++)
                        {
                            if (xn.ChildNodes[i].Attributes["sItemName"].Value == listView1.Items[k].Text)
                                listView1.Items[k].Checked = true;
                        }
                    }


                }


                cXmlDoc = null;
            }





            
        }

        private void btnXCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
          
            if (cBoxExZTLX.Text == "") return;
            for (int a = 0; a < listView1.Items.Count; a++)
            {
                if (listView1.Items[a].Checked)
                {

                    pgpl.listview.Add(listView1.Items[a].Text);
                    pgpl.isNeed = true;
                }

            }
            pgpl.ztlx = cBoxExZTLX.Text.Replace(':', '：');//.Trim().Replace('【','[');
            //GeoPageLayout.ztlx=GeoPageLayout.ztlx.Replace('】',']');

            pgpl.stcxzq = cBoxExXZQ.Text;
            this.Close();
            Application.DoEvents();

        }


       
                        
               

    }
}