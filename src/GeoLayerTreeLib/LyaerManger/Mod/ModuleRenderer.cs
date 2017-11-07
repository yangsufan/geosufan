using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace GeoLayerTreeLib.LayerManager
{

    public static class ModuleRenderer
    {
        /// <summary>
        /// 设置NoData的颜色值
        /// </summary>
        /// <param name="pLayer"></param>
        public static void SetRasterDefaultNoDataColor(ILayer pLayer)
        {
            if (pLayer == null) return;
            //获得当前图层的符号
            IRasterRenderer pRasterRender = null;
            if (pLayer is IRasterLayer)
            {
                IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                pRasterRender = pRasterLayer.Renderer;

            }
            else if (pLayer is IRasterCatalogLayer)
            {
                IRasterCatalogLayer pRasterCatalogLayer = pLayer as IRasterCatalogLayer;
                pRasterRender = pRasterCatalogLayer.Renderer;
            }
            else
            {
                return;
            }

            if (pRasterRender == null) return;

            //设置NoData颜色值
            IColor pColor = null;
            if (pRasterRender is IRasterRGBRenderer) //默认是白色   //changed by chulili 20110825 0是透明 全部默认Nodata颜色是透明
            {
                pColor = GetColorRGB(255, 255,255, 0);
            }
            else if (pRasterRender is IRasterUniqueValueRenderer)//默认是白色
            {
                pColor = GetColorRGB(255, 255, 255, 0);
            }
            else if (pRasterRender is IRasterStretchColorRampRenderer)//默认是透明色 0是透明
            {
                pColor = GetColorRGB(255, 255, 255, 0);
            }
            else if (pRasterRender is IRasterClassifyColorRampRenderer)//默认是白色 
            {
                pColor = GetColorRGB(255, 255, 255, 0);
            }
            else
            {
                return;
            }

            //设置颜色
            IRasterDisplayProps pRasterDisplayProps = pRasterRender as IRasterDisplayProps;
            if (pRasterDisplayProps == null) return;
            pRasterDisplayProps.NoDataColor = pColor;

        }

        /// <summary>
        /// 获得RGB颜色
        /// </summary>
        /// <param name="intRed">红色</param>
        /// <param name="intGreen">绿色</param>
        /// <param name="intBlue">蓝色</param>
        /// <param name="intTrans">透明度</param>
        /// <returns></returns>
        public static IColor GetColorRGB(int intRed, int intGreen, int intBlue, int intTransparency)
        {
            IColor pTempColor = new RgbColorClass();
            IRgbColor pRGBColor = pTempColor as IRgbColor;
            pRGBColor.Red = intRed;
            pRGBColor.Green = intGreen;
            pRGBColor.Blue = intBlue;
            pRGBColor.Transparency = (byte)intTransparency;

            return pTempColor;
        }
        public static IFeatureRenderer DoWithRenderer(IWorkspace pConfigWks, string rendererKey, string rendererSchema, IFeatureLayer featureLayer)
        {
            IFeatureRenderer featureRenderer = null;
            try
            {
                string sql = "ID='" + rendererKey + "'";
                object obj = GetRendererFromBlob(sql,pConfigWks);
                if (featureLayer is IFDOGraphicsLayer)
                {
                    string sAnnoRendererColor = (string)obj;
                    ISymbolSubstitution pSymbolSubstitution = featureLayer as ISymbolSubstitution;
                    pSymbolSubstitution.SubstituteType = esriSymbolSubstituteType.esriSymbolSubstituteColor;
                    pSymbolSubstitution.MassColor = GetRgbColorFromRgbString(sAnnoRendererColor);
                }
                else
                {
                    featureRenderer = (IFeatureRenderer)obj;
                }
            }
            catch (Exception ex)
            {
            }
            return featureRenderer;
        }

        //shduan 20110721 add将栅格符号反序列化
        public static IRasterRenderer DoWithRasterRenderer(IWorkspace pConfigWks, string rendererKey, string rendererSchema, IRasterLayer pRLayer)
        {
            IRasterRenderer pRRenderer = null;
            try
            {
                string sql = "ID='" + rendererKey + "'";
                object obj = GetRendererFromBlob(sql,pConfigWks);

                pRRenderer = (IRasterRenderer)obj;
            }
            catch (Exception ex)
            {
            }
            return pRRenderer;
        }
        public static IColor ConvertColorToIColor(System.Drawing.Color color)
        {

            IColor pColor = new RgbColorClass();

            pColor.RGB = color.B * 65536 + color.G * 256 + color.R;

            return pColor;

        }


        public static IRgbColor GetRgbColorFromRgbString(string sColor)
        {
            if (sColor == "") return null;
            string[] rgbs = sColor.Split(',');

            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = Convert.ToInt32(rgbs[0]);
            pRGBColor.Green = Convert.ToInt32(rgbs[1]);
            pRGBColor.Blue = Convert.ToInt32(rgbs[2]);
            if (rgbs.Length > 3) pRGBColor.Transparency = Convert.ToByte(rgbs[3]);
            return pRGBColor;
        }
        //从数据库中读取图层设置
        public static object GetLayerConfigFromBlob(string sql, IWorkspace pConfigWks)
        {
            Exception err = null;
            SysCommon.Gis.IGisTable pGISTable = new SysCommon.Gis.SysGisTable(pConfigWks);

            //获取Layer
            byte[] LayerByte = pGISTable.GetFieldValue("Render", "LayerConfig", sql, out err) as byte[];

            //是否得到了序列化的blob，没得到，则返回NULL
            if (LayerByte == null) return null;
            //从数据库中得到图层的类型
            string strLayerType = pGISTable.GetFieldValue("Render", "LayerType", sql, out err).ToString();

            ILayer pLayer = null;
            if (strLayerType == "" || strLayerType == null) return null;
            switch (strLayerType)
            {
                case "FDOGraphicsLayer":    //注记层
                    pLayer = new FDOGraphicsLayerClass();
                    break;
                case "DimensionLayer":      //标注层
                    pLayer = new DimensionLayerClass();
                    break;
                case "GdbRasterCatalogLayer":       //影像层(RC)
                    pLayer = new GdbRasterCatalogLayerClass();
                    break;
                case "RasterLayer":         //影像数据集(RD)
                    pLayer = new RasterLayerClass();
                    break;
                case "FeatureLayer":        //普通地物层
                    pLayer = new FeatureLayerClass();
                    break;
                default:
                    pLayer = new FeatureLayerClass();
                    break;
            }
            IMemoryBlobStreamVariant pMemoryBlobStreamVariant = new MemoryBlobStreamClass();
            pMemoryBlobStreamVariant.ImportFromVariant((object)LayerByte);
            IStream pStream = pMemoryBlobStreamVariant as IStream;

            if (pLayer != null && pStream!=null)
            {
                IPersistStream pPersistStream = pLayer as IPersistStream;
                try
                {
                    pPersistStream.Load(pStream);
                }
                catch
                {
                    return null;
                }
                pLayer = pPersistStream as ILayer ;
            }
            return pLayer;  
        }
        /// <summary>
        /// 得到Renderer从数据库中
        /// </summary>
        /// <returns></returns>
        public static object GetRendererFromBlob(string sql, IWorkspace pConfigWks)
        {
            Exception err = null;
            SysCommon.Gis.IGisTable pGISTable = new SysCommon.Gis.SysGisTable(pConfigWks);

            //获取Renderer
            byte[] renderByte = pGISTable.GetFieldValue("Render", "Render", sql, out err) as byte[];

            //是否得到了序列化的blob，没得到，则返回NULL
            if (renderByte == null) return null;

            IMemoryBlobStreamVariant pMemoryBlobStreamVariant = new MemoryBlobStreamClass();
            pMemoryBlobStreamVariant.ImportFromVariant((object)renderByte);
            IStream pStream = pMemoryBlobStreamVariant as IStream;

            //从数据库中得到符号的类型  shduan 20110721 增加RasterRenderer
            string strRenderType = pGISTable.GetFieldValue("Render", "RenderType", sql, out err).ToString();
            string strLyrType = pGISTable.GetFieldValue("Render", "LayerType", sql, out err).ToString();
            if (strLyrType == "RasterLayer")
            {
                IRasterRenderer pRRenderer = null;
                switch (strRenderType)
                {
                    case "RasterClassifyColorRampRenderer":
                        pRRenderer = new RasterClassifyColorRampRendererClass();
                        break;
                    case "RasterUniqueValueRenderer":
                        pRRenderer = new RasterUniqueValueRendererClass();
                        break;
                    case "RasterDiscreteColorRenderer":
                        pRRenderer = new RasterDiscreteColorRendererClass();
                        break;
                    case "RasterRGBRenderer":
                        pRRenderer = new RasterRGBRendererClass();
                        break;
                    case "RasterStretchColorRampRenderer":
                        pRRenderer = new RasterStretchColorRampRendererClass();
                        break;
                }
                IPersistStream pPersistStream = pRRenderer as IPersistStream;
                pPersistStream.Load(pStream);
                //pRRenderer = pPersistStream as IRasterRenderer;

                return pRRenderer;
            }
            else
            {
                IFeatureRenderer pFRenderer = null;
                switch (strRenderType)
                {
                    case "AnnoColor":
                        string sAnnoColor = Encoding.Default.GetString(renderByte);
                        return (object)sAnnoColor;
                    case "SimpleRenderer":
                        pFRenderer = new SimpleRendererClass();
                        break;
                    case "UniqueValueRenderer":
                        pFRenderer = new UniqueValueRendererClass();
                        break;
                    case "ClassBreaksRenderer":
                        pFRenderer = new ClassBreaksRendererClass();
                        break;
                    case "ProportionalSymbolRenderer":
                        pFRenderer = new ProportionalSymbolRendererClass();
                        break;
                    case "ChartRenderer":
                        pFRenderer = new ChartRendererClass();
                        break;
                }
                IPersistStream pPersistStream = pFRenderer as IPersistStream;
                pPersistStream.Load(pStream);
                pFRenderer = pPersistStream as IFeatureRenderer;

                return pFRenderer;
            }
        }
        public static IFeatureRenderer  HideSymbolOfLayer(IGeoFeatureLayer pGeoFeatureLayer)
        {
            IFeatureRenderer pFeatureRenderer = pGeoFeatureLayer.Renderer;
            
            ISymbol pSymbol = null;
            if (pFeatureRenderer is ISimpleRenderer)
            {
                ISimpleRenderer pSimpleRender = pFeatureRenderer as ISimpleRenderer;
                pSimpleRender.Symbol = null;
                //pSymbol = pSimpleRender.Symbol;
                //SetSymbolNoColor(pGeoFeatureLayer, ref pSymbol);
                //pSimpleRender.Symbol = pSymbol;
                return pSimpleRender as IFeatureRenderer;
            }
            else if (pFeatureRenderer is IUniqueValueRenderer)
            {
                IUniqueValueRenderer pUniqueRender = pFeatureRenderer as IUniqueValueRenderer;

                for (int i = 0; i < pUniqueRender.ValueCount; i++)
                {
                    string sValue = pUniqueRender.get_Value(i);
                    pUniqueRender.set_Symbol(sValue, null);
                    //pSymbol = pUniqueRender.get_Symbol(sValue);
                    //SetSymbolNoColor(pGeoFeatureLayer, ref pSymbol);
                    //pUniqueRender.set_Symbol(sValue, pSymbol);
                }
                return  pUniqueRender as IFeatureRenderer;
            }
            else if (pFeatureRenderer is IClassBreaksRenderer)
            {
                IClassBreaksRenderer pClassRenderer = pFeatureRenderer as IClassBreaksRenderer;
                for (int i = 0; i < pClassRenderer.BreakCount; i++)
                {
                    pClassRenderer.set_Symbol(i, null);
                }
                pClassRenderer.BackgroundSymbol = null;
                return pClassRenderer as IFeatureRenderer;
            }
            else if (pFeatureRenderer is IProportionalSymbolRenderer)
            {
               
            }
            else if (pFeatureRenderer is IChartRenderer)
            {
                IChartRenderer pChartRenderer = pFeatureRenderer as IChartRenderer;
                pChartRenderer.BaseSymbol = null;
                pChartRenderer.ChartSymbol = null;
            }
            return null;
        }
        public static void SetSymbolNoColor(IGeoFeatureLayer pGeoFeatureLayer,ref  ISymbol pSymbol)
        {
            ILineSymbol pLineSymbol = null;
            IRgbColor pRgbColor = null;           


            switch (pGeoFeatureLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    IMarkerSymbol pMarkSysmbol = pSymbol as IMarkerSymbol;
                    
                    pRgbColor = new RgbColorClass();
                    pRgbColor.NullColor = true;
                    pMarkSysmbol.Color = pRgbColor;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pLineSymbol = pSymbol as ILineSymbol;
                    pRgbColor = new RgbColorClass();
                    pRgbColor.NullColor = true;
                    pLineSymbol.Color = pRgbColor;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    IFillSymbol pFillSymbol = pSymbol as IFillSymbol;
                    if (pFillSymbol != null)
                    {
                        pLineSymbol = pFillSymbol.Outline;
                        pRgbColor = new RgbColorClass();
                        pRgbColor.NullColor = true;
                        pFillSymbol.Color = pRgbColor;
                        pLineSymbol.Color = pRgbColor;
                        pFillSymbol.Outline = pLineSymbol;
                    }
                    break;

                default:
                    return;
            }
        }
        //目前只支持简单符号和质地填充符号
        public static IFeatureRenderer SetColorOfRenderer(IGeoFeatureLayer pGeoFeatureLayer, IRgbColor pFillRgbColor,
            IRgbColor pBorderColor, double PointSize, double LineWidth)
        {
            IFeatureRenderer pFeatureRenderer = pGeoFeatureLayer.Renderer;
            ISymbol pSymbol = null;
            if (pFeatureRenderer is ISimpleRenderer)
            {
                ISimpleRenderer pSimpleRender = pFeatureRenderer as ISimpleRenderer;
                pSymbol = pSimpleRender.Symbol;
                SetSymbolColor(pGeoFeatureLayer, ref pSymbol, pFillRgbColor, pBorderColor, PointSize, LineWidth);
                pSimpleRender.Symbol = pSymbol;
                return pSimpleRender as IFeatureRenderer;
            }
            else if (pFeatureRenderer is IUniqueValueRenderer)
            {
                IUniqueValueRenderer pUniqueRender = pFeatureRenderer as IUniqueValueRenderer;
                for (int i = 0; i < pUniqueRender.ValueCount; i++)
                {
                    string sValue = pUniqueRender.get_Value(i);
                    pSymbol = pUniqueRender.get_Symbol(sValue);
                    SetSymbolColor(pGeoFeatureLayer, ref pSymbol, pFillRgbColor, pBorderColor, PointSize, LineWidth);
                    pUniqueRender.set_Symbol(sValue, pSymbol);
                }
                return pUniqueRender as IFeatureRenderer;
            }
            else if (pFeatureRenderer is IClassBreaksRenderer)
            {

            }
            else if (pFeatureRenderer is IProportionalSymbolRenderer)
            {

            }
            else if (pFeatureRenderer is IChartRenderer)
            {

            }
            return null;
        }

        //为符号设置颜色
        private static void SetSymbolColor(IGeoFeatureLayer pGeoFeatureLayer, ref ISymbol pSymbol, IRgbColor pFillRgbColor,
            IRgbColor pBorderRgbColor, double PointSize, double LineWidth)
        {
            ILineSymbol pLineSymbol = null;
            switch (pGeoFeatureLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    IMarkerSymbol pMarkSysmbol = pSymbol as IMarkerSymbol;
                    if (pMarkSysmbol != null && pBorderRgbColor != null)
                    {
                        pMarkSysmbol.Color = pBorderRgbColor;
                        if (PointSize > 0)
                            pMarkSysmbol.Size = PointSize * 72 / 25.4;
                    }
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pLineSymbol = pSymbol as ILineSymbol;
                    if (pLineSymbol != null)
                    {
                        if (pBorderRgbColor != null) pLineSymbol.Color = pBorderRgbColor;
                        if (LineWidth > 0)
                            pLineSymbol.Width = LineWidth * 72 / 25.4;
                    }
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    IFillSymbol pFillSymbol = pSymbol as IFillSymbol;
                    if (pFillSymbol != null)
                    {
                        pLineSymbol = pFillSymbol.Outline;
                        if (pFillRgbColor != null)
                        {
                            pFillRgbColor.NullColor = pFillSymbol.Color.NullColor;
                            pFillRgbColor.UseWindowsDithering = pFillSymbol.Color.UseWindowsDithering;
                            pFillRgbColor.Transparency = pFillSymbol.Color.Transparency;
                            pFillSymbol.Color = pFillRgbColor;
                        }
                        if (pBorderRgbColor != null)
                        {
                            pBorderRgbColor.NullColor = pLineSymbol.Color.NullColor;
                            pBorderRgbColor.UseWindowsDithering = pLineSymbol.Color.UseWindowsDithering;
                            pBorderRgbColor.Transparency = pLineSymbol.Color.Transparency;


                            pLineSymbol.Color = pBorderRgbColor;
                        }
                        if (LineWidth > 0)
                            pLineSymbol.Width = LineWidth * 72 / 25.4;
                        pFillSymbol.Outline = pLineSymbol;
                    }
                    break;

                default:
                    return;
            }
        }
    }
}
