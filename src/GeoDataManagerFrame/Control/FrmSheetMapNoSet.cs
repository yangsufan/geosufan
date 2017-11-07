using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using System.IO;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using System.Xml;
namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.28
    /// 说明：标准图幅查询定位窗体
    /// </summary>
    public partial class FrmSheetMapNoSet : DevComponents.DotNetBar.Office2007Form
    {
        public string MapNo="";
        public int Scale = 50000;
        AxMapControl pAxMapControl;
   
        public FrmSheetMapNoSet(AxMapControl inpAxMapControl)
        {
            InitializeComponent();
            //isOK = false;
            //cBoxScale.SelectedIndex = 0;
            //curMapName = incurMapName;
            pAxMapControl = inpAxMapControl;
            try
            {

                initcBoxScale();
            }
            catch
            {

            }
           
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
                else
                    label1.Text = "请检查配置文件\\Res\\Xml\\TFHQuery.xml";
            }
        }
        private void txtResolution_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled=true;
            if(txtMapNo.Text=="")
                btnOK.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtMapNo.Text == "" || cBoxScale.Text == "")
                return;
            string[] oMapNo = txtMapNo.Text.Split(' ');
            for (int q = 0; q < oMapNo.Length; q++)
            {
                MapNo += oMapNo[q];
            }
            Scale = Convert.ToInt32(cBoxScale.Text.Split(':')[1]);
            try
            {
                string cPath = Application.StartupPath + "\\..\\Res\\Xml\\TFHQuery.xml";
                if (!File.Exists(cPath))
                {
                    return;
                }
                XmlDocument cXmlDoc = new XmlDocument();
                string dataName = "";
                if (cXmlDoc != null)
                {
                    cXmlDoc.Load(cPath);

                    XmlNodeList xnl = cXmlDoc.GetElementsByTagName("GisDoc");
                    foreach (XmlNode xn in xnl.Item(0).ChildNodes)
                    {
                        if (xn.Attributes["ItemName"].Value == "1:" + Scale.ToString())
                        {
                            dataName = xn.Attributes["DataName"].Value;
                            break;
                        }

                    }
                }

                IGraphicsContainer psGra = pAxMapControl.Map as IGraphicsContainer;
                IGeometry pGeometry = getExtentByTFNO(MapNo, pAxMapControl.Map,
                                  Scale > 2000, dataName);
                if (pGeometry != null)
                {
                    pAxMapControl.Extent = pGeometry.Envelope;
                    drawPolygonElement(pGeometry as IPolygon, psGra);
                }


                pAxMapControl.Refresh();
            }
            catch
            { 
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
        //根据 图幅号 搜索 接合表图层 得到相应的 范围
        private IGeometry getExtentByTFNO(string inTFNO, IMap inMap, bool isLScale, string strLayerName)
        {
            IGeometry psGeometry = null;
            for (int i = 0; i < inMap.LayerCount; i++)
            {

                ILayer pLayer = inMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pCLayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < pCLayer.Count; j++)
                    {
                         string dName=((pCLayer.get_Layer(j) as IFeatureLayer).FeatureClass as IDataset).Name;
                        if(dName.Contains("."))
                            dName=dName.Split('.')[1];
                        if (dName != strLayerName)
                            continue;
                        
                        IFeatureLayer pFLayer = pCLayer.get_Layer(j) as IFeatureLayer;
                        psGeometry = getExtentFromFL(pFLayer, inTFNO, isLScale);
                        if (psGeometry != null)
                            break;

                    }
                }
                else//不是grouplayer
                {
                    string dName=((pLayer as IFeatureLayer).FeatureClass as IDataset).Name;
                    if(dName.Contains("."))
                        dName=dName.Split('.')[1];
                    if (dName != strLayerName)
                        continue;
                    IFeatureLayer pFLayer = pLayer as IFeatureLayer;
                    psGeometry = getExtentFromFL(pFLayer, inTFNO, isLScale);
                    if (psGeometry != null)
                        break;
                }
            }
            return psGeometry;
        }
        //搜索FC,定位图幅号得范围，the caller is getExtentByTFNO(string inTFNO, IMap inMap,bool isLScale)
        private IGeometry getExtentFromFL(IFeatureLayer pFLayer, string inTFNO, bool isLScale)
        {
            if (isLScale)
            {
                try
                {
                    IFeatureClass pFC = pFLayer.FeatureClass;
                    //int dexScale = pFC.FindField("比例尺分母");
                    int dexTFH = pFC.FindField("图幅号");
                    //if (pFC.Search(null, false).NextFeature().get_Value(dexTFH).ToString().Substring(5, 1) == inTFNO.Substring(5, 1))
                    //{

                        IFeatureCursor pFCursor = pFC.Search(null, false);
                        IFeature pFeature = pFCursor.NextFeature();
                        while (pFeature != null)
                        {
                            string[] oTFH=pFeature.get_Value(dexTFH).ToString().Split(' ');
                            string rTFH="";
                            for(int q=0;q<oTFH.Length;q++)
                            {
                                rTFH +=oTFH[q];
                            }
                            if (rTFH == inTFNO)
                                return pFeature.ShapeCopy;
                            pFeature = pFCursor.NextFeature();
                        }
                    //}
                }
                catch
                {

                }
            }
            return null;
        }
        //在mapcontrol上画多边形
        private void drawPolygonElement(IPolygon pPolygon, IGraphicsContainer pGra)
        {
            if (pPolygon == null)
                return;
            pGra.DeleteAllElements();
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();
            IFillShapeElement pPolygonElement = new PolygonElementClass();
            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                //pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 0;
                pRGBColor.Green = 0;
                pRGBColor.Blue = 255;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 2;
                //pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;
                pRGBColor.Transparency = 0;
                pFillSymbol.Color = pRGBColor;
                //pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
                (pPolygonElement as IElement).Geometry = pPolygon;
                pPolygonElement.Symbol = pFillSymbol;
                pGra.AddElement(pPolygonElement as IElement, 0);





            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制范围出错:" + ex.Message, "提示");
                pFillSymbol = null;
            }
        }

        private void FrmSheetMapNoSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            (pAxMapControl.Map as IGraphicsContainer).DeleteAllElements();
            pAxMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            //Application.DoEvents();
        }

        private void FrmSheetMapNoSet_Load(object sender, EventArgs e)
        {

        }

        


       

    }
}
