using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;


namespace GeoUserManager
{
    public class CommandSymbol : Fan.Plugin.Interface.CommandRefBase
    {
        private Fan.Plugin.Application.IAppGisUpdateRef _AppHk;
        public CommandSymbol()
        {
            base._Name = "GeoUserManager.CommandSymbol";
            base._Caption = "符号设置";
            base._Tooltip = "符号设置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "符号设置";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
                if (pLayer == null) return false;
                if (pLayer is ESRI.ArcGIS.Carto.IFeatureLayer)
                {
                    IFeatureLayer pFeaLyr = pLayer as IFeatureLayer;
                    if (pFeaLyr.FeatureClass != null)
                    {
                        if (pFeaLyr.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                }
                else if(pLayer is ESRI.ArcGIS.Carto.RasterLayerClass)
                {
                    return true;
                }

                return false;
            }
        }
        public override string Message
        {
            get
            {
                Fan.Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Fan.Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Fan.Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Fan.Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            ILayer pCurLayer = (ILayer)_AppHk.MapControl.CustomProperty;
            if (pCurLayer == null) return;
            if (pCurLayer is ESRI.ArcGIS.Carto.IFeatureLayer)
            {
                IFeatureLayer pLayer = pCurLayer as IFeatureLayer ;
                if (pLayer == null) return;

                try
                {
                    GeoSymbology.frmSymbology frm = new GeoSymbology.frmSymbology(pLayer);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        ESRI.ArcGIS.Carto.IGeoFeatureLayer pGeoLayer = pLayer as ESRI.ArcGIS.Carto.IGeoFeatureLayer;
                        pGeoLayer.Renderer = frm.FeatureRenderer();
                        _AppHk.MapControl.ActiveView.Refresh();
                        _AppHk.TOCControl.Update();

                        //保存当前的符号信息
                        string strLyrName = pLayer.Name;
                        if (pGeoLayer.FeatureClass != null)
                        {
                            IDataset pDataset = pGeoLayer.FeatureClass as IDataset;
                            strLyrName = pDataset.Name;
                        }
                        strLyrName = strLyrName.Substring(strLyrName.IndexOf('.') + 1);

                        XmlDocument vDoc = new XmlDocument();
                        vDoc.Load(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");
                        UpdateSymbolInfo(strLyrName, vDoc, pGeoLayer.Renderer);
                        vDoc.Save(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");
                    }
                }
                catch(Exception err)
                {

                    return;
                }
            }
            else if (pCurLayer is ESRI.ArcGIS.Carto.RasterLayerClass)
            {
                GeoSymbology.frmDEMSymbology frm = new GeoSymbology.frmDEMSymbology(pCurLayer);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    //yjl20110826 add
                    IRasterLayer pRasterLayer = pCurLayer as IRasterLayer;
                    IRasterRenderer pRR = pRasterLayer.Renderer;
                    pRasterLayer.Renderer = frm.RasterRenderer();
                    if (pRR.Updated)
                        pRR.Update();
                    _AppHk.MapControl.ActiveView.Refresh();
                    _AppHk.TOCControl.Update();
                    //保存当前的符号信息
                    string strLyrName = pCurLayer.Name;
                    if (pRasterLayer.Raster!= null)
                    {
                        IDataset pDataset = pRasterLayer as IDataset;
                        strLyrName = pDataset.Name;
                    }
                    strLyrName = strLyrName.Substring(strLyrName.IndexOf('.') + 1);

                    XmlDocument vDoc = new XmlDocument();
                    vDoc.Load(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");
                    UpdateSymbolInfo(strLyrName, vDoc, pRasterLayer.Renderer);
                    vDoc.Save(System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml");
                }
            }
        }

        public override void OnCreate(Fan.Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Fan.Plugin.Application.IAppGisUpdateRef;

        }

        //更新xml节点，yjl20110826 modify ifeaturerenderer->object for raster
        private void UpdateSymbolInfo(string strLyrName, XmlDocument pXmlDoc, object pFeaRender)
        {
            if (pXmlDoc == null) return;

            XmlElement pElement = pXmlDoc.SelectSingleNode("//" + strLyrName) as XmlElement;
            if (pElement == null)
            {
                pElement = pXmlDoc.CreateElement(strLyrName);
                pXmlDoc.DocumentElement.AppendChild(pElement);//yjl20110820 modify 否则报【此文档已具有“DocumentElement”节点】错误
            }

            string strXml = "";
            Fan.Common.XML.XMLClass.XmlSerializer(pFeaRender, "", out strXml);
            pElement.RemoveAll();
            pElement.AppendChild(pXmlDoc.CreateTextNode(strXml));

        }
    }
}
