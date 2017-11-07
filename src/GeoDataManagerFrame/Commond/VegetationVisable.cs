using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.Carto;
using System.Xml;

//显示/隐藏植被层 added by  xisheng 20110802
namespace GeoDataManagerFrame
{
    public class VegetationVisable : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public readonly static string m_QueryPath = Application.StartupPath + "\\..\\Template\\QueryConfig.Xml";
        public VegetationVisable()
        {
            base._Name = "GeoDataManagerFrame.VegetationVisable";
            base._Caption = "隐藏植被层";
            base._Tooltip = "隐藏植被层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "隐藏植被层";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            XmlDocument pXmlDoc = new XmlDocument();
            try
            {
                pXmlDoc.Load(m_QueryPath);
                bool IsNull = true;
                //构成xml节点，根据NodeKey在节点里查询
                XmlNodeList nodelist = pXmlDoc.SelectNodes("//HideDataConfig/HideItem[@ItemText='植被']/LayerItem");
                if (nodelist == null)
                {
                    pXmlDoc = null;
                    return;
                }

                if (base._Checked == false)
                {
                    foreach (XmlNode pxmlnode in nodelist)//循环遍历植被层是否在图层中存在
                    {
                        string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
                        ILayer pLayer = GetLayerByNodeKey(m_Hook.MapControl.Map, strNodeKey);
                        if (pLayer == null)
                            continue;
                        pLayer.Visible = false;
                        IsNull = false;//如果存在一个图层则置于false，这样好改变按钮是否选中。
                    }
                    if (!IsNull)
                        base._Checked = true;
                }
                else
                {
                    foreach (XmlNode pxmlnode in nodelist)
                    {
                        string strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
                        ILayer pLayer = GetLayerByNodeKey(m_Hook.MapControl.Map, strNodeKey);
                        if (pLayer == null)
                            continue;
                        pLayer.Visible = true;
                        
                    }
                    base._Checked = false;
                }
                
                m_Hook.MapControl.ActiveView.Refresh();
                pXmlDoc = null;
            }
            catch
            { }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }

        //added by chulili 20110730 根据nodeKey查找图层
        public static ILayer GetLayerByNodeKey(IMap pMap, string strNodeKey)
        {
            if (pMap == null) return null;
            if (strNodeKey.Equals(string.Empty)) return null;

            ILayer pSearchLayer = null;
            //循环子节点，比对NodeKey
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                ILayer pLayer = pMap.get_Layer(i);
                if (pLayer != null)
                {   //调用查找图层的函数
                    pSearchLayer = GetLayerByNodeKey(pMap, pLayer, strNodeKey);
                    if (pSearchLayer != null)
                    {
                        return pSearchLayer;
                    }
                }

            }
            return null;
        }
        //added by chulili 20110730根据NodeKey查找图层 递归调用
        private static ILayer GetLayerByNodeKey(IMap pMap, ILayer pLayer, string strNodeKey)
        {
            if (pMap==null)
                return null;
            if (pLayer==null)
                return null;
            if (strNodeKey.Equals(string.Empty)) return null;
            ILayer pSearchLayer = null;
            //遍历子节点
            IGroupLayer pGrouplayer = pLayer as IGroupLayer;
            if (pGrouplayer != null)
            {
                ICompositeLayer pComLayer = pGrouplayer as ICompositeLayer;
                for (int i = 0; i < pComLayer.Count; i++)
                {
                    ILayer pTmpLayer = pComLayer.get_Layer(i);
                    pSearchLayer = GetLayerByNodeKey(pMap, pTmpLayer, strNodeKey);
                    if (pSearchLayer != null)
                    {
                        return pSearchLayer;
                    }
                }
            }
            else
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                string strNodeXml = pLayerGenPro.LayerDescription;
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(strNodeXml );
                //构成xml节点，根据NodeKey在节点里查询
                string strSearch = "//Layer[@NodeKey=" + "'"+strNodeKey +"'" + "]";
                XmlNode pNode = pXmlDoc.SelectSingleNode(strSearch);
                if (pNode != null)
                {
                    pXmlDoc = null;
                    return pLayer;
                }
            }
            return null;
        }

    }
}
