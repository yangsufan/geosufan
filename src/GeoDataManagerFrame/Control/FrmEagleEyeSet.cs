using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geometry;

namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：地图鹰眼设置窗体
    /// </summary>
    public partial class FrmEagleEyeSet : DevComponents.DotNetBar.Office2007Form
    {
        IMap pMap = null;
        public FrmEagleEyeSet(IMap inMap)
        {
            InitializeComponent();
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            cboLayers.Items.Add(pCLayer.get_Layer(j).Name); 

                    }
                }
                else//不是grouplayer
                {
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    if (pFLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                          cboLayers.Items.Add(pLayer.Name); 
                }
            }
            //if (cboLayers.Items.Count > 0)
            //    cboLayers.SelectedIndex = 0;
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\EagleEye.xml";
            if (!File.Exists(cPath))
            {
                return;
            }
            XmlDocument cXmlDoc = new XmlDocument();

            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("EagleEyeInfo");
                cboLayers.Text = xnl.Item(0).Attributes["LayerName"].Value;
            }
            
            cXmlDoc = null;
        }

        private void btnXOK_Click(object sender, EventArgs e)
        {
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\EagleEye.xml";
            if (!File.Exists(cPath))
            {
                return;
            }
             XmlDocument cXmlDoc = new XmlDocument();

             if (cXmlDoc != null)
             {
                 cXmlDoc.Load(cPath);

                 XmlNodeList xnl = cXmlDoc.GetElementsByTagName("EagleEyeInfo");
                 xnl.Item(0).Attributes["LayerName"].Value = cboLayers.Text;
             }

             cXmlDoc.Save(cPath);
        }

        private void FrmEagleEyeSet_Load(object sender, EventArgs e)
        {
            
        }

        private void cboLayers_TextChanged(object sender, EventArgs e)
        {
            btnXOK.Enabled = true;
            if (cboLayers.Text == "")
                btnXOK.Enabled = false;
        }

        private void btnXCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
