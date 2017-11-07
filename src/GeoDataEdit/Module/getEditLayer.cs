using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataEdit
{
    class getEditLayer
    {
        //从xml文件读取layer
        private static string getLayerName()
        {
            string cPath = Application.StartupPath + "\\..\\Res\\Xml\\EditLayer.xml";
            if (!File.Exists(cPath))
            {
                return "";
            }
            XmlDocument cXmlDoc = new XmlDocument();
            string strLayer = "";
            if (cXmlDoc != null)
            {
                cXmlDoc.Load(cPath);

                XmlNodeList xnl = cXmlDoc.GetElementsByTagName("EditLayerInfo");
                if (xnl.Item(0) != null)
                {
                    if ((xnl.Item(0) as XmlElement).HasAttribute("FCName"))
                    {
                        strLayer = xnl.Item(0).Attributes["FCName"].Value;
                    }
                }
            }
            return strLayer;
        }
        //判断layer是否在当前地图
        public static ILayer isExistLayer(IMap inMap)
        {
            string strLayerName = "";
            try
            {
                 strLayerName = getLayerName();
            }
            catch
            {

            }
            ILayer res = null;
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        //added by chulili 20110729 错误保护
                        if (pFLayer == null)
                        { continue; }
                        if ((pFLayer.FeatureClass as IDataset).Name == strLayerName)
                        {
                            res = pCLayer.get_Layer(j);

                            break;
                        }

                    }
                }
                else//不是grouplayer
                {
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    //added by chulili 20110729 错误保护
                    if (pFLayer == null)
                    { continue; }
                    if ((pFLayer.FeatureClass as IDataset).Name == strLayerName)
                    {
                        res = pLayer;

                        break;
                    }
                }
            }
          
            return res;
        }
    }
}
