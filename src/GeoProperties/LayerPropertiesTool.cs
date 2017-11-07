using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using System.Xml;

namespace GeoProperties
{
    public class LayerPropertiesTool:BaseCommand
    {
        private IHookHelper m_pHookHelper;
        //private XmlDocument m_pXmlDoc;
        private frmLayerProperties layerProDialog;
        private IMap m_pMap;
        private IActiveView m_pActiveView;
        private ILayer m_pLayer;
        IMapControlDefault m_MapControl;//xisheng 20111117 传递m_MapControl以便刷新

        public LayerPropertiesTool()
        {
        }

        public ILayer CurLayer
        {
            set { m_pLayer = value; }
        }

        public override void OnCreate(object hook)
        {
            if (m_pHookHelper == null)
            {
                m_pHookHelper = new HookHelperClass();
            }
            m_pHookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;//xisheng 20111117 传递m_MapControl以便刷新
            m_pMap = m_pHookHelper.FocusMap;
            m_pActiveView = m_pHookHelper.ActiveView;
            //m_pLayer = m_pMap.get_Layer(0);
        }

        public override void OnClick()
        {
            if (m_pLayer == null) return;
            // 获取图层NodeKey ygc 2012-20-11
            ILayerGeneralProperties pLayerGenPro = m_pLayer as ILayerGeneralProperties;
            string sValue = pLayerGenPro.LayerDescription;
            string NodeKey = "";
            if (sValue != "")
            {
                XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(sValue);
                XmlNode node = docXml.SelectSingleNode("//Layer");
                NodeKey = node.Attributes["NodeKey"].Value.ToString();
            }//end
            //xisheng 20111117 传递m_MapControl以便刷新*********************************
            if (layerProDialog == null)
            {
                //layerProDialog = new frmLayerProperties(m_pLayer, m_pActiveView, true);
                layerProDialog = new frmLayerProperties(m_pLayer, m_MapControl, true,NodeKey);
                layerProDialog.Show();
            }
            else if (layerProDialog.Visible == false)
            {
                //layerProDialog = new frmLayerProperties(m_pLayer, m_pActiveView, true);
                layerProDialog = new frmLayerProperties(m_pLayer, m_MapControl, true,NodeKey);
                layerProDialog.Show();
            }
            ////xisheng 20111117 传递m_MapControl以便刷新****************************end
        }
    }
}
