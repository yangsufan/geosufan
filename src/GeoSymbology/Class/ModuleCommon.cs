using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace GeoSymbology
{
    public class ModuleCommon
    {
        private ISymbologyControl m_SymbologyControl;

        public ModuleCommon()
        {
            m_SymbologyControl = new SymbologyControlClass();
        }

        /// <summary>
        /// 获取要色方案集合
        /// </summary>
        /// <param name="width">色带图片的宽度</param>
        /// <param name="height">色带图片的高度</param>
        /// <param name="category">色带的类型</param>
        /// <returns></returns>
        public List<ColorItem> GetColorScheme(int width, int height, string category)
        {
            string sInstall = ModuleCommon.ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            if (sInstall == "") //added by chulili 2012-11-13 平台由ArcGIS9.3换成ArcGIS10，相应的注册表路径要修改
            {
                sInstall=ModuleCommon.ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (sInstall == "")
            {
                sInstall = ModuleCommon.ReadRegistry("SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }   //added by chulili 2012-11-13  end
            m_SymbologyControl.LoadStyleFile(sInstall + "\\Styles\\ESRI.ServerStyle");

            ISymbologyStyleClass styleClass = m_SymbologyControl.GetStyleClass(esriSymbologyStyleClass.esriStyleClassColorRamps);
            int count = styleClass.get_ItemCount("");

            List<ColorItem> colorScheme = new List<ColorItem>();
            for (int i = 0; i < count; i++)
            {
                IStyleGalleryItem pItem = styleClass.GetItem(i);
                if (category.Contains(pItem.Category))
                {
                    stdole.IPictureDisp picture = styleClass.PreviewItem(pItem, width, height);
                    Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
                    ColorItem colorItem = new ColorItem();
                    colorItem.ColorImage = image;
                    colorItem.Name = pItem.Name;
                    colorItem.ColorRamp = pItem.Item as IColorRamp;
                    colorScheme.Add(colorItem);
                }
            }

            return colorScheme;
        }

        public static int ImageWidth = 74;
        public static int ImageHeight = 34;

        public static ISymbol CreateMarkerSym(double angle, double size, IColor pColor, double xOffset, double yOffset)
        {
            if (size <= 0)
                throw new Exception("请指定符号的大小！");
            if (pColor == null)
                throw new Exception("请指定符号的颜色！");

            IMarkerSymbol pSymbol = new SimpleMarkerSymbolClass();
            pSymbol.Angle = angle;
            pSymbol.Color = pColor;
            pSymbol.Size = size;
            pSymbol.XOffset = xOffset;
            pSymbol.YOffset = yOffset;

            return pSymbol as ISymbol;
        }

        public static ISymbol CreateLineSym(IColor pColor, double width, double offset)
        {
            if (width <= 0)
                throw new Exception("请指定线符号宽度！");
            if (pColor == null)
                throw new Exception("请指定线符号颜色！");
            ILineSymbol pSymbol = new SimpleLineSymbolClass();
            pSymbol.Color = pColor;
            pSymbol.Width = width;
            //ILineProperties pLineProp = pSymbol as ILineProperties;
            //pLineProp.Offset = offset;
            return pSymbol as ISymbol;
        }

        public static ISymbol CreateFillSym(IColor pColor, double outLineWidth, IColor outLineColor)
        {
            if (pColor == null)
                throw new Exception("请指定面符号颜色！");

            IFillSymbol pSymbol = new SimpleFillSymbolClass();
            pSymbol.Color = pColor;

            if (outLineWidth >= 0.01)
            {
                ILineSymbol pOutLine = new SimpleLineSymbolClass();
                pOutLine.Width = outLineWidth;
                if (outLineColor == null)
                    throw new Exception("请指定外边框颜色！");
                pOutLine.Color = outLineColor;
                pSymbol.Outline = pOutLine;
            }
            return pSymbol as ISymbol;
        }

        public static ISymbol CreateSymbol(esriSymbologyStyleClass _SymbologyStyleClass)
        {
            ISymbol _Symbol = null;
            switch (_SymbologyStyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    _Symbol = CreateMarkerSym();
                    break;
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    _Symbol = CreateLineSym();
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    _Symbol = CreateFillSym();
                    break;
                default:
                    _Symbol = null;
                    break;
            }
            return _Symbol;
        }

        private static ISymbol CreateMarkerSym()
        {
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 0;
            pColor.Green = 0;
            pColor.Blue = 0;
            return ModuleCommon.CreateMarkerSym(0, 2, pColor, 0, 0);
        }

        private static ISymbol CreateLineSym()
        {
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 0;
            pColor.Green = 0;
            pColor.Blue = 0;
            return ModuleCommon.CreateLineSym(pColor, 1, 0);
        }

        private static ISymbol CreateFillSym()
        {
            IRgbColor pOutlineColor = new RgbColorClass();
            pOutlineColor.Red = 0;
            pOutlineColor.Green = 0;
            pOutlineColor.Blue = 0;

            IRgbColor pFillColor = new RgbColorClass();
            pFillColor.Red = 128;
            pFillColor.Green = 255;
            pFillColor.Blue = 128;
            return ModuleCommon.CreateFillSym(pFillColor, 0.1, pOutlineColor);
        }

        /// <summary>
        /// 将符号转化为图片
        /// </summary>
        /// <param name="pSymbol">符号</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns>图片</returns>
        public static Image Symbol2Picture(ISymbol pSymbol, int width, int height)
        {
            if (pSymbol == null || width < 1 || height < 1) return null;

            //根据高宽创建图象
            Bitmap bmp = new Bitmap(width, height);
            Graphics gImage = Graphics.FromImage(bmp);
            gImage.Clear(Color.White);
            double dpi = gImage.DpiX;
            ESRI.ArcGIS.Geometry.IEnvelope pEnvelope = new ESRI.ArcGIS.Geometry.EnvelopeClass();
            pEnvelope.PutCoords(0, 0, (double)bmp.Width, (double)bmp.Height);

            tagRECT deviceRect;
            deviceRect.left = 0;
            deviceRect.right = bmp.Width;
            deviceRect.top = 0;
            deviceRect.bottom = bmp.Height;

            IDisplayTransformation pDisplayTransformation = new DisplayTransformationClass();
            pDisplayTransformation.VisibleBounds = pEnvelope;
            pDisplayTransformation.Bounds = pEnvelope;
            pDisplayTransformation.set_DeviceFrame(ref deviceRect);
            pDisplayTransformation.Resolution = dpi;

            System.IntPtr hdc = new IntPtr();
            hdc = gImage.GetHdc();

            ESRI.ArcGIS.Geometry.IGeometry pGeo = CreateSymShape(pSymbol, pEnvelope);

            //将符号的形状绘制到图象中
            pSymbol.SetupDC((int)hdc, pDisplayTransformation);
            pSymbol.Draw(pGeo);
            pSymbol.ResetDC();
            gImage.ReleaseHdc(hdc);
            gImage.Dispose();
            return bmp;

            ////IStyleGalleryClass pStyleClass = null;
            ////if (pSymbol is IMarkerSymbol)
            ////    pStyleClass = new ESRI.ArcGIS.Carto.MarkerSymbolStyleGalleryClassClass();
            ////else if (pSymbol is ILineSymbol)
            ////    pStyleClass = new ESRI.ArcGIS.Carto.LineSymbolStyleGalleryClassClass();
            ////else if (pSymbol is IFillSymbol)
            ////    pStyleClass = new ESRI.ArcGIS.Carto.FillSymbolStyleGalleryClassClass();
            ////else
            ////    return null;

            //IStyleGalleryItem pStyleItem = new ServerStyleGalleryItemClass();
            //pStyleItem.Name = "tempSymbol";
            //pStyleItem.Item = pSymbol;

            //Bitmap bitmap = new Bitmap(width, height);
            //System.Drawing.Graphics pGraphics = System.Drawing.Graphics.FromImage(bitmap);
            //tagRECT rect = new tagRECT();
            //rect.right = bitmap.Width;
            //rect.bottom = bitmap.Height;
            ////生成预览
            //IntPtr hdc = new IntPtr();
            //hdc = pGraphics.GetHdc();

            //pStyleClass.Preview(pStyleItem, hdc.ToInt32(), ref rect);

            //pGraphics.ReleaseHdc(hdc);
            //pGraphics.Dispose();

            //return bitmap;
        }

        private static IGeometry CreateSymShape(ISymbol pSymbol, IEnvelope pEnvelope)
        {// 根据传入的符号以及外包矩形区域返回对应的几何空间实体（点，线、面）
            //判断是否为“点”符号
            if (pSymbol is IMarkerSymbol)
            {
                return (pEnvelope as IArea).Centroid;
            }
            else if (pSymbol is ILineSymbol)
            {

                IPoint pPointFrom = new PointClass();
                pPointFrom.PutCoords(pEnvelope.XMin, (pEnvelope.YMin + pEnvelope.YMax) / 2);
                IPoint pPointTo = new PointClass();
                pPointTo.PutCoords(pEnvelope.XMax, (pEnvelope.YMin + pEnvelope.YMax) / 2);

                IPolyline pPolyline = new PolylineClass();
                pPolyline.FromPoint = pPointFrom;
                pPolyline.ToPoint = pPointTo;
                return pPolyline;
            }
            else if (pSymbol is ITextSymbol)
            {
                IPolyline pPolyline = new PolylineClass();
                pPolyline.FromPoint = pEnvelope.LowerLeft;
                pPolyline.ToPoint = pEnvelope.UpperRight;
                return pPolyline;
            }
            else
            {
                return pEnvelope as ESRI.ArcGIS.Geometry.IGeometry;
            }
        }

        /// <summary>
        /// 读取符号库路径
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string ReadRegistry(string sKey)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sKey, true);
            if (rk == null) return "";

            return (string)rk.GetValue("InstallDir");
        }

        public static double GetSymbolSize(ISymbol pSymbol)
        {
            double size = 0;
            if (pSymbol is IMarkerSymbol)
            {
                IMarkerSymbol pMarkerSymbol = pSymbol as IMarkerSymbol;
                size = pMarkerSymbol.Size;
            }
            else if (pSymbol is ILineSymbol)
            {
                ILineSymbol pLineSymbol = pSymbol as ILineSymbol;
                size = pLineSymbol.Width;
            }
            else if (pSymbol is IFillSymbol)
            {
                IFillSymbol pFillSymbol = pSymbol as IFillSymbol;
                size = pFillSymbol.Outline.Width;
            }

            return size;
        }

        public static IColor GetColor(ISymbol pSymbol)
        {
            IColor pRGBColor = new RgbColorClass();
            if (pSymbol is IMarkerSymbol)
            {
                IMarkerSymbol pMarkerSymbol = pSymbol as IMarkerSymbol;
                pRGBColor = pMarkerSymbol.Color as IColor;
            }
            else if (pSymbol is ILineSymbol)
            {
                ILineSymbol pLineSymbol = pSymbol as ILineSymbol;
                pRGBColor = pLineSymbol.Color as IColor;
            }
            else if (pSymbol is IFillSymbol)
            {
                IFillSymbol pFillSymbol = pSymbol as IFillSymbol;
                pRGBColor = pFillSymbol.Color as IColor;
            }

            return pRGBColor;
        }

        public static void ChangeSymbolSize(ISymbol pSymbol, double size)
        {
            if (pSymbol is IMarkerSymbol)
            {
                IMarkerSymbol pMarkerSymbol = pSymbol as IMarkerSymbol;
                pMarkerSymbol.Size = size;
            }
            else if (pSymbol is ILineSymbol)
            {
                ILineSymbol pLineSymbol = pSymbol as ILineSymbol;
                pLineSymbol.Width = size;
            }
        }

        public static void ChangeSymbolColor(ISymbol pSymbol, IColor pColor)
        {
            if (pSymbol is IMarkerSymbol)
            {
                IMarkerSymbol pMarkerSymbol = pSymbol as IMarkerSymbol;
                pMarkerSymbol.Color = pColor;
            }
            else if (pSymbol is ILineSymbol)
            {
                ILineSymbol pLineSymbol = pSymbol as ILineSymbol;
                pLineSymbol.Color = pColor;
            }
            else if (pSymbol is IFillSymbol)
            {
                IFillSymbol pFillSymbol = pSymbol as IFillSymbol;
                pFillSymbol.Color = pColor;
            }
        }

        public static ESRI.ArcGIS.Carto.IFeatureRenderer GetRendererFromLayer(
            ESRI.ArcGIS.Carto.IFeatureLayer pLayer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            if (pLayer == null) return CreateUVRenderer(_SymbologyStyleClass);
            ESRI.ArcGIS.Carto.IGeoFeatureLayer pGeoLayer = pLayer as ESRI.ArcGIS.Carto.IGeoFeatureLayer;
            if (pGeoLayer == null || pGeoLayer.Renderer == null) return CreateUVRenderer(_SymbologyStyleClass);
            return pGeoLayer.Renderer;
        }
        //简单渲染
        public static ESRI.ArcGIS.Carto.IFeatureRenderer CreateSimpleRenderer(esriSymbologyStyleClass _SymbologyStyleClass)
        {
            ESRI.ArcGIS.Carto.ISimpleRenderer pSimpleRenderer = new ESRI.ArcGIS.Carto.SimpleRendererClass();
            pSimpleRenderer.Description = "";
            pSimpleRenderer.Label = "";
            pSimpleRenderer.Symbol = ModuleCommon.CreateSymbol(_SymbologyStyleClass);
            return pSimpleRenderer as ESRI.ArcGIS.Carto.IFeatureRenderer;
        }
        //唯一值渲染
        public static ESRI.ArcGIS.Carto.IFeatureRenderer CreateUVRenderer(esriSymbologyStyleClass _SymbologyStyleClass)
        {
            ESRI.ArcGIS.Carto.IUniqueValueRenderer pUVRenderer = new ESRI.ArcGIS.Carto.UniqueValueRendererClass();
            pUVRenderer.FieldCount = 0;
            
            pUVRenderer.DefaultLabel = "<All Other Values>";
            pUVRenderer.ColorScheme = "";            
            pUVRenderer.DefaultSymbol = ModuleCommon.CreateSymbol(_SymbologyStyleClass);
            return pUVRenderer as ESRI.ArcGIS.Carto.IFeatureRenderer;
        }
        //分级颜色渲染
        public static ESRI.ArcGIS.Carto.IFeatureRenderer CreateBreakColorRenderer(esriSymbologyStyleClass _SymbologyStyleClass)
        {
            ESRI.ArcGIS.Carto.IClassBreaksRenderer pBreakRenderer = new ESRI.ArcGIS.Carto.ClassBreaksRendererClass();
            pBreakRenderer.Field = "<NONE>";
            pBreakRenderer.MinimumBreak = 0;
            pBreakRenderer.BreakCount = 0;
            pBreakRenderer.BackgroundSymbol = null;
            ESRI.ArcGIS.Carto.IClassBreaksUIProperties pUIProp = pBreakRenderer as ESRI.ArcGIS.Carto.IClassBreaksUIProperties;
            pUIProp.ColorRamp = "";

            return pBreakRenderer as ESRI.ArcGIS.Carto.IFeatureRenderer;
        }
        //分级大小渲染
        public static ESRI.ArcGIS.Carto.IFeatureRenderer CreateBreakSizeRenderer(esriSymbologyStyleClass _SymbologyStyleClass)
        {
            ESRI.ArcGIS.Carto.IClassBreaksRenderer pBreakRenderer = new ESRI.ArcGIS.Carto.ClassBreaksRendererClass();
            pBreakRenderer.Field = "<NONE>";
            pBreakRenderer.MinimumBreak = 0;
            pBreakRenderer.BreakCount = 0;
            if (_SymbologyStyleClass == esriSymbologyStyleClass.esriStyleClassFillSymbols)
                pBreakRenderer.BackgroundSymbol = ModuleCommon.CreateSymbol(esriSymbologyStyleClass.esriStyleClassMarkerSymbols) as IFillSymbol;
            else
                pBreakRenderer.BackgroundSymbol = null;

            return pBreakRenderer as ESRI.ArcGIS.Carto.IFeatureRenderer ;
        }
        //图表渲染
        public static ESRI.ArcGIS.Carto.IFeatureRenderer CreateChartRenderer(esriSymbologyStyleClass _SymbologyStyleClass)
        {
            ESRI.ArcGIS.Carto.IChartRenderer pCharRenderer = new ESRI.ArcGIS.Carto.ChartRendererClass();
            pCharRenderer.BaseSymbol = ModuleCommon.CreateSymbol(_SymbologyStyleClass);
            return pCharRenderer as ESRI.ArcGIS.Carto.IFeatureRenderer;
        }
        public static ESRI.ArcGIS.Carto.IRasterRenderer CreateStretchColorRampRenderer()
        {
            ESRI.ArcGIS.Carto.IRasterStretchColorRampRenderer pStretchRenderer = new ESRI.ArcGIS.Carto.RasterStretchColorRampRendererClass ();


            return pStretchRenderer as ESRI.ArcGIS.Carto.IRasterRenderer ;
        }
        public static ESRI.ArcGIS.Carto.IRasterRenderer CreateClassifyColorRampRenderer()
        {
            ESRI.ArcGIS.Carto.IRasterClassifyColorRampRenderer pClassifyRenderer = new ESRI.ArcGIS.Carto.RasterClassifyColorRampRendererClass();


            return pClassifyRenderer as ESRI.ArcGIS.Carto.IRasterRenderer;
        }
        public static ESRI.ArcGIS.Carto.IRasterRenderer CreateUniqueValueRasterRenderer()
        {
            ESRI.ArcGIS.Carto.IRasterUniqueValueRenderer pUniqueRenderer = new ESRI.ArcGIS.Carto.RasterUniqueValueRendererClass();


            return pUniqueRenderer as ESRI.ArcGIS.Carto.IRasterRenderer;
        }
        public static ESRI.ArcGIS.Carto.IRasterRenderer CreateRGBRenderer()
        {
            ESRI.ArcGIS.Carto.IRasterRGBRenderer pRGBRenderer = new ESRI.ArcGIS.Carto.RasterRGBRendererClass();


            return pRGBRenderer as ESRI.ArcGIS.Carto.IRasterRenderer;
        }
      

        public static List<FieldInfo> GetFieldsFromLayer(ESRI.ArcGIS.Carto.IFeatureLayer pLayer,bool IsNumricField)
        {
            //FieldInfo[] fields = new FieldInfo[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    if (i == 0)
            //    {
            //        fields[i] = new FieldInfo();
            //        fields[i].FieldName = "<NONE>";
            //        fields[i].FieldDesc = "<NONE>";
            //        fields[i].FieldType = "<NONE>";
            //    }
            //    else
            //    {
            //        fields[i] = new FieldInfo();
            //        fields[i].FieldName = "Field" + i.ToString();
            //        fields[i].FieldDesc = "Field" + i.ToString();
            //        fields[i].FieldType = "Field" + i.ToString();
            //    }
            //}
            //return fields;

            ESRI.ArcGIS.Geodatabase.IFeatureClass pClass = pLayer.FeatureClass;
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo noneField = new FieldInfo();
            noneField.FieldName = "<NONE>";
            noneField.FieldDesc = "<NONE>";
            noneField.FieldType = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate;
            fields.Add(noneField);

            for (int i = 0; i < pClass.Fields.FieldCount; i++)
            {
                FieldInfo field = new FieldInfo();
                ESRI.ArcGIS.Geodatabase.IField pField = pClass.Fields.get_Field(i);
                if (IsNumricField)
                {
                    //if (pField.VarType > 1 && pField.VarType < 6 && pField.Type != ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID)
                    if((int)pField.Type<4)
                    {
                        field.FieldName = pField.Name;
                        field.FieldDesc = pField.AliasName;
                        field.FieldType = pField.Type;
                        fields.Add(field);
                    }
                }
                else
                {
                    //if ((pField.VarType > 1 && pField.VarType < 6 && pField.Type != ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID) 
                    //    || pField.Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString)
                    if((int)pField.Type<5)
                    {
                        field.FieldName = pField.Name;
                        field.FieldDesc = pField.AliasName;
                        field.FieldType = pField.Type;
                        fields.Add(field);
                    }
                }
            }
            return fields;
        }
        public static ESRI.ArcGIS.Carto.IRasterRenderer LoadRasterRendererFromByte(byte[] _RendererValue, string _RendererType)
        {
            if (_RendererValue == null || _RendererValue.Length == 0) return null;
            try
            {
                enumRasterRendererType pRendererType = (enumRasterRendererType)Enum.Parse(typeof(enumRasterRendererType), _RendererType);
                ESRI.ArcGIS.Carto.IRasterRenderer pRasterRenderer = null;
                switch (pRendererType)
                {

                    case enumRasterRendererType.StretchColorRampRenderer:
                        pRasterRenderer = new ESRI.ArcGIS.Carto.RasterStretchColorRampRendererClass ();
                        break;
                    case enumRasterRendererType.ClassifyColorRampRenderer:
                        pRasterRenderer =new ESRI.ArcGIS.Carto.RasterClassifyColorRampRendererClass();
                        break;
                    case enumRasterRendererType.UniqueValueRenderer:
                        pRasterRenderer =new ESRI.ArcGIS.Carto.RasterUniqueValueRendererClass ();
                        break;
                    case enumRasterRendererType.RGBRenderer:
                        pRasterRenderer =new ESRI.ArcGIS.Carto.RasterRGBRendererClass ();
                        break;
                }

                if (pRasterRenderer == null) return null;

                IMemoryBlobStreamVariant pMemoryBlobStreamVariant = new MemoryBlobStreamClass();
                pMemoryBlobStreamVariant.ImportFromVariant((object)_RendererValue);
                IStream pStream = pMemoryBlobStreamVariant as IStream;
                IPersistStream pPersistStream = pRasterRenderer as IPersistStream;
                pPersistStream.Load(pStream);
                pRasterRenderer = pPersistStream as ESRI.ArcGIS.Carto.IRasterRenderer ;
                return pRasterRenderer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static ESRI.ArcGIS.Carto.IFeatureRenderer LoadRendererFromByte(byte[] _RendererValue, string _RendererType)
        {
            if (_RendererValue == null || _RendererValue.Length == 0) return null;
            try
            {
                enumRendererType pRendererType = (enumRendererType)Enum.Parse(typeof(enumRendererType), _RendererType);
                ESRI.ArcGIS.Carto.IFeatureRenderer pRenderer = null;
                switch (pRendererType)
                {
                    case enumRendererType.SimpleRenderer:
                        pRenderer = new ESRI.ArcGIS.Carto.SimpleRendererClass();
                        break;
                    case enumRendererType.UniqueValueRenderer:
                        pRenderer = new ESRI.ArcGIS.Carto.UniqueValueRendererClass();
                        break;
                    case enumRendererType.BreakColorRenderer:
                        pRenderer = new ESRI.ArcGIS.Carto.ClassBreaksRendererClass();
                        break;
                    case enumRendererType.BreakSizeRenderer:
                        pRenderer = new ESRI.ArcGIS.Carto.ClassBreaksRendererClass();
                        break;
                }

                if (pRenderer == null) return null;

                IMemoryBlobStreamVariant pMemoryBlobStreamVariant = new MemoryBlobStreamClass();
                pMemoryBlobStreamVariant.ImportFromVariant((object)_RendererValue);
                IStream pStream = pMemoryBlobStreamVariant as IStream;
                IPersistStream pPersistStream = pRenderer as IPersistStream;
                pPersistStream.Load(pStream);
                pRenderer = pPersistStream as ESRI.ArcGIS.Carto.IFeatureRenderer;
                return pRenderer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void SaveRasterRendererToByte(ref byte[] _RendererValue, ref string _RendererType, IRasterRendererUI  pRasterRendererUI)
        {
            if (pRasterRendererUI == null)
            {
                _RendererValue = null;
                _RendererType = enumRasterRendererType.StretchColorRampRenderer.ToString();
                return;
            }

            _RendererType = pRasterRendererUI.RasterRendererType.ToString();

            try
            {
                IPersistStream pPersistStream = pRasterRendererUI.RasterRenderer  as IPersistStream;
                IStream pStream = new XMLStreamClass();
                pPersistStream.Save(pStream, 0);
                IXMLStream pXMLStream = pStream as IXMLStream;
                _RendererValue = pXMLStream.SaveToBytes();
            }
            catch (Exception ex)
            {
                _RendererValue = null;
                _RendererType = enumRasterRendererType.StretchColorRampRenderer.ToString();
            }
        }
        public static void SaveRendererToByte(ref byte[] _RendererValue, ref string _RendererType, IRendererUI pRendererUI)
        {
            if (pRendererUI == null)
            {
                _RendererValue = null;
                _RendererType = enumRendererType.SimpleRenderer.ToString();
                return;
            }

            _RendererType = pRendererUI.RendererType.ToString();

            try
            {
                IPersistStream pPersistStream = pRendererUI.Renderer as IPersistStream;
                IStream pStream = new XMLStreamClass();
                pPersistStream.Save(pStream, 0);
                IXMLStream pXMLStream = pStream as IXMLStream;
                _RendererValue = pXMLStream.SaveToBytes();
            }
            catch (Exception ex)
            {
                _RendererValue = null;
                _RendererType = enumRendererType.SimpleRenderer.ToString();
            }
        }

        public static System.Drawing.Color GetWindowsColor(IColor pColor)
        {
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.RGB = pColor.RGB;

            System.Drawing.Color color = Color.FromArgb(pRGBColor.Red, pRGBColor.Green, pRGBColor.Blue);
            
            return color;
        }

        public static IColor GetESRIColor(Color color)
        {
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = color.R;
            pRGBColor.Green = color.G;
            pRGBColor.Blue = color.B;
            return pRGBColor;
        }
    }
}
