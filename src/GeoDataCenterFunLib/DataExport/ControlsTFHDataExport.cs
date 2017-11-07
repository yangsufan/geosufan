using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

using ESRI.ArcGIS.Carto;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
///////ZQ  20111007   行政区数据提取
namespace GeoDataCenterFunLib 
{
    public class ControlsTFHDataExport : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        DevComponents.AdvTree.AdvTree m_xzqTree = null;
        private String m_layerNodeKey =System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\接图表图层信息.xml";
        public ControlsTFHDataExport()
        {
            base._Name = "GeoDataCenterFunLib.ControlsTFHDataExport";
            base._Caption = "行政区图幅数据输出";
            base._Tooltip = "行政区图幅数据输出";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区图幅数据输出";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.CurrentControl is ESRI.ArcGIS.Controls.ISceneControl) return false;
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                try
                {
                    if (m_xzqTree.SelectedNode.Tag.ToString() != "County" && m_xzqTree.SelectedNode.Tag.ToString() != "Town" && m_xzqTree.SelectedNode.Tag.ToString() != "Village") return false;
                }
                catch { }
                //else { return false; }
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        //xisheng 20111103
        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {//XZQLocation
            if (_hook == null) return;
            if (_hook.MainUserControl == null) return;

            IGeometry xzqGeo = ModGetData.getExtentByXZQ(m_xzqTree.SelectedNode);
            if (xzqGeo == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到相应的行政区范围！");
                return;
            }
            ///ZQ  20111027 add  判断数据字典是否初始化
            if (SysCommon.ModField._DicFieldName.Count == 0)
            {
                SysCommon.ModField.InitNameDic(Plugin.ModuleCommon.TmpWorkSpace, SysCommon.ModField._DicFieldName, "属性对照表");
            }
            IList<string> listMapNo = new List<string>();
            IEnvelope pExtent = xzqGeo.Envelope;
            pExtent.Expand(1.5, 1.5, true);//跟地位效果一样 xisheng 20111104
            (_AppHk.ArcGisMapControl.Map as IActiveView).Extent = pExtent;
            //ZQ    20110914    modify   改变显示方式
            //drawPolygonElement(pGeometry as IPolygon, psGra);
            _AppHk.ArcGisMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, null);
            //end
            //ZQ   20110809   modify   先刷新后闪烁问题
            _AppHk.ArcGisMapControl.ActiveView.ScreenDisplay.UpdateWindow();
            //end
            _AppHk.ArcGisMapControl.FlashShape(xzqGeo, 3, 200, null);
            //这里需要修改NodeKey 的值      ZQ  20110801     Add
            try
            {
                string strMapNoField = "";
                string NodeKey = GetNodeKey("1:50000", out strMapNoField).ToString();//不同比例尺的接图表的ID号
                IFeatureClass pFeatureClass = ModQuery.GetFeatureClassByNodeKey(NodeKey);
                if (strMapNoField == "") return;
                if (pFeatureClass == null) { return; }
                ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = xzqGeo;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                IFeatureCursor pFeaCursor = pFeatureClass.Search(pSpatialFilter, false);
                IFeature pFeature = pFeaCursor.NextFeature();
                while (pFeature != null)
                {
                    //ZQ   20110807  modify
                    listMapNo.Add((pFeature.get_Value(pFeature.Table.FindField(strMapNoField))).ToString());
                    //end
                    pFeature = pFeaCursor.NextFeature();
                }

                pFeaCursor = null;
                GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(listMapNo, true, false, false, false, _AppHk.ArcGisMapControl as IMapControlDefault, _hook.CurWksInfo.Wks);
                pfrmExportDataByMapNO.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGisUpdateRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
            m_xzqTree = _hook.XZQTree;
        }

        /// <summary>
        /// 读取接图表图层.xml来获取图层NodeKey     ZQ   20110805  add
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        private string GetNodeKey(string Number,out string strMapNoField)
        {
            string strNodeKey = "";
            strMapNoField = "";
            XmlDocument pXmlDocument = new XmlDocument();
            if (!File.Exists(m_layerNodeKey))
            {
                return strNodeKey = "";
            }
            pXmlDocument.Load(m_layerNodeKey);
            XmlNode pxmlnode = null;
            switch (Number)
            {
                case "1:1000000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:1000000']");
                    break;
                case "1:250000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:250000']");
                    break;
                case "1:200000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:200000']");
                    break;
                case "1:100000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:100000']");
                    break;
                case "1:50000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:50000']");
                    break;
                case "1:25000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:25000']");
                    break;
                case "1:10000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:10000']");
                    break;
                case "1:5000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:5000']");
                    break;
                case "1:2000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:2000']");
                    break;
                case "1:1000":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:1000']");
                    break;
                case "1:500":
                    pxmlnode = pXmlDocument.SelectSingleNode("GisDoc/Layer[@ItemName='1:500']");
                    break;
            }
            if (pxmlnode == null)
            {
                strMapNoField = "";
                return strNodeKey = "";
            }
            strNodeKey = pxmlnode.Attributes["NodeKey"].Value.ToString();
            strMapNoField = pxmlnode.Attributes["MapNo"].Value.ToString();
            return strNodeKey;
        }
    }
}
