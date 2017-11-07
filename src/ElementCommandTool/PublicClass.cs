using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;
using ElementCommandTool;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Output;

namespace ElementCommandTool
{
    public static class PublicClass
    {
        public struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("User32", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos([In] ref POINTAPI pos);

        public static IWorkspace g_Workspace;

        /// <summary>
        /// 绘制线、矩形和面的函数
        /// </summary>
        /// <param name="pGeometry"></param>
        /// <param name="pScrDisplay"></param>
        public static void DrawGeometryXOR(IGeometry pGeometry,IScreenDisplay pScrDisplay)
        {
            if (pGeometry == null || pScrDisplay == null || pGeometry.GeometryType == esriGeometryType.esriGeometryPoint) return;

            IFillSymbol pFillSymbol;
            ILineSymbol pLineSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;
            ISymbol pLSymbol;
            IPolygon pPolygon;
            IEnvelope pEnvelope;
            IPolyline pPolyline;
            try
            {
                pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                pRGBColor.Red = 45;
                pRGBColor.Green = 45;
                pRGBColor.Blue = 45;
                
                pFillSymbol = new SimpleFillSymbolClass();
                pSymbol = (ISymbol)pFillSymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
                pFillSymbol.Color = (IColor)pRGBColor;

                pLineSymbol = pFillSymbol.Outline;
                pLSymbol = (ISymbol)pLineSymbol;
                pLSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
                pRGBColor.Red = 145;
                pRGBColor.Green = 145;
                pRGBColor.Blue = 145;
                pLineSymbol.Color = (IColor)pRGBColor;
                pLineSymbol.Width = 0.8;
                pFillSymbol.Outline = pLineSymbol;

                pScrDisplay.StartDrawing(0, -1);  //esriScreenCache.esriNoScreenCache -1
                if (pGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
                {
                    pScrDisplay.SetSymbol((ISymbol)pLineSymbol);
                    pPolyline = (IPolyline)pGeometry;
                    pScrDisplay.DrawPolyline(pPolyline);
                }
                else if (pGeometry.GeometryType == esriGeometryType.esriGeometryEnvelope)
                {
                    pScrDisplay.SetSymbol(pSymbol);
                    pEnvelope = (IEnvelope)pGeometry;
                    pScrDisplay.DrawRectangle(pEnvelope);
                }
                else
                {
                    pScrDisplay.SetSymbol(pSymbol);
                    pPolygon = (IPolygon)pGeometry;
                    pScrDisplay.DrawPolygon(pPolygon);
                }
                pScrDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制缓冲范围出错:" + ex.Message, "提示");
            }
            finally
            {
                pFillSymbol = null;
                pLineSymbol = null;
                pSymbol = null;
                pRGBColor = null;
                pLSymbol = null;
            }
        }

        public static IGeometry GetSelectGeometry(IMap pMap)
        {
            if (pMap == null) return null;
            if (pMap.SelectionCount == 0) return null;

            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeomtryCol = (IGeometryCollection)pGeometryBag;
            IEnumFeature pEnumFeature = (IEnumFeature)pMap.FeatureSelection;
            IFeature pFeature = pEnumFeature.Next();
            
            object obj = System.Reflection.Missing.Value;
            while (pFeature != null)
            {
                pGeomtryCol.AddGeometry(pFeature.ShapeCopy, ref obj, ref obj);
                pFeature = pEnumFeature.Next();
            }
            pGeometryBag.Project(pMap.SpatialReference);
            return (IGeometry)pGeometryBag;
        }

        public static void SetPrinter(ref IPageLayoutControl pPageControl,ref PrintDocument document)
        {
            try
            {
                PageSetupDialog pageSetDlg = new PageSetupDialog();
                pageSetDlg.PageSettings = new PageSettings();
                pageSetDlg.PrinterSettings = new PrinterSettings();
                pageSetDlg.ShowNetwork = true;
                if (pageSetDlg.ShowDialog() == DialogResult.OK)
                {
                    document.PrinterSettings = pageSetDlg.PrinterSettings;
                    document.DefaultPageSettings = pageSetDlg.PageSettings;

                    int i;
                    IEnumerator paperSizes = pageSetDlg.PrinterSettings.PaperSizes.GetEnumerator();
                    paperSizes.Reset();

                    for (i = 0; i < pageSetDlg.PrinterSettings.PaperSizes.Count; ++i)
                    {
                        paperSizes.MoveNext();
                        if (((PaperSize)paperSizes.Current).Kind == document.DefaultPageSettings.PaperSize.Kind)
                        {
                            document.DefaultPageSettings.PaperSize = ((PaperSize)paperSizes.Current);
                            break;
                        }
                    }

                    IPaper paper;
                    paper = new PaperClass(); //create a paper object
                    IPrinter printer;
                    printer = new EmfPrinterClass(); //create a printer object
                    paper.Attach(pageSetDlg.PrinterSettings.GetHdevmode(pageSetDlg.PageSettings).ToInt32(), pageSetDlg.PrinterSettings.GetHdevnames().ToInt32());
                    printer.Paper = paper;
                    pPageControl.Printer = printer;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印设置失败：" + ex.Message, "提示");
            }
        }

        public static IRgbColor GetRGBColor(IColor pColor)
        {
            int lValue = pColor.RGB;
            IRgbColor pRGBColor = new RgbColorClass();
            int iRed = lValue % 0X100;
            int iGreen = (lValue / 0X100) % 0X100;
            int iBlue = (lValue / 0X10000) % 0X100;
            pRGBColor.Red = iRed;
            pRGBColor.Green = iGreen;
            pRGBColor.Blue = iBlue;
            pRGBColor.Transparency = pColor.Transparency;
            return pRGBColor;
        }

        public static IRgbColor GetRGBColor(ColorButton btnColor)
        {
            IRgbColor pRGB = new RgbColorClass();
            pRGB.Red = btnColor.Color.R;
            pRGB.Green = btnColor.Color.G;
            pRGB.Blue = btnColor.Color.B;
            if (btnColor.Color == Color.Transparent) pRGB.Transparency = 0;
            else pRGB.Transparency = 255;

            pRGB.UseWindowsDithering = true;
            return pRGB;
        }

        public static IRgbColor GetRGBColor(int yourRed, int yourGreen, int yourBlue)
        {
            IRgbColor pRGB = new RgbColorClass();
            pRGB.Red = yourRed;
            pRGB.Green = yourGreen;
            pRGB.Blue = yourBlue;
            pRGB.UseWindowsDithering = true;
            return pRGB;
        }

    }
}
