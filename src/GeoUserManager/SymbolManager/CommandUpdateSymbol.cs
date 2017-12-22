using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GeoUserManager
{
    public class CommandUpdateSymbol : Fan.Plugin.Interface.CommandRefBase
    {
        private Fan.Plugin.Application.IAppGisUpdateRef _AppHk;

        public CommandUpdateSymbol()
        {
            base._Name = "GeoUserManager.CommandUpdateSymbol";
            base._Caption = "更新符号";
            base._Tooltip = "更新符号";
            base._Checked = false;
            base._Visible = true;
            base._Enabled =false;
            base._Message = "更新符号";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                return true;
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
            //获得符号信息 通过mxd
            System.Windows.Forms.OpenFileDialog dir = new System.Windows.Forms.OpenFileDialog();
            dir.FileName = "";
            dir.Filter = "ArcGis地图文档(*.mxd)|*.mxd";
            dir.Multiselect = false;
            if (dir.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            string strMxdPath = dir.FileName;
            IMapDocument pMapDoc = new MapDocumentClass();
            if (!pMapDoc.get_IsMapDocument(strMxdPath)) return;

            pMapDoc.Open(strMxdPath, "");
            
            //获得所有的符号信息 并进行序列化
            if (SaveMxdSymbolToXML(pMapDoc, System.Windows.Forms.Application.StartupPath + "\\..\\Template\\SymbolInfo.xml"))
            {
                Fan.Common.Error.ErrorHandle.ShowInform("提示", "符号信息更新成功！");
            }
            else
            {
                Fan.Common.Error.ErrorHandle.ShowInform("提示", "无法更新符号信息！");
            }
            
        }

        public override void OnCreate(Fan.Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Fan.Plugin.Application.IAppGisUpdateRef;
        }

        private bool SaveMxdSymbolToXML(IMapDocument pMapDoc, string strXMLFile)
        {
            if (pMapDoc == null) return false;
            if (!System.IO.File.Exists(strXMLFile)) return false;

            //打开xml文档
            try
            {
                System.Xml.XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.Load(strXMLFile);

                IMap pMap = pMapDoc.get_Map(0);
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    ILayer pLyr = pMap.get_Layer(i);
                    if (pLyr is IGeoFeatureLayer)
                    {
                        IGeoFeatureLayer pGeoFeaLyr = pLyr as IGeoFeatureLayer;
                        string strLyrName = pLyr.Name;

                        //如果有数据源 就以要素类的名称为准 如果没有就以图层名为准
                        if (pGeoFeaLyr.FeatureClass != null)
                        {
                            IDataset pDataset = pGeoFeaLyr.FeatureClass as IDataset;
                            strLyrName = pDataset.Name;
                        }

                        strLyrName = strLyrName.Substring(strLyrName.IndexOf('.') + 1);
                        IFeatureRenderer pFeaRender = pGeoFeaLyr.Renderer;
                        UpdateSymbolInfo(strLyrName, pXmlDoc, pFeaRender);
                    }
                }

                pXmlDoc.Save(strXMLFile);
            }
            catch
            {

                return false;
            }

            return true;
        }

        //更新xml接点
        private void UpdateSymbolInfo(string strLyrName,XmlDocument pXmlDoc,IFeatureRenderer pFeaRender)
        {
            if (pXmlDoc == null) return;

            XmlElement pElement = pXmlDoc.SelectSingleNode("//" + strLyrName) as XmlElement;
            XmlElement pRoot = pXmlDoc.SelectSingleNode("//SYMBOLINFO") as XmlElement;
            if (pElement == null)
            {
                pElement = pXmlDoc.CreateElement(strLyrName);
                pRoot.AppendChild(pElement);
            }

            string strXml = "";
            Fan.Common.XML.XMLClass.XmlSerializer(pFeaRender, "", out strXml);
            pElement.RemoveAll();
            pElement.AppendChild(pXmlDoc.CreateTextNode(strXml));

        }

    }
}
