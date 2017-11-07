using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using System.Windows.Forms;

namespace GeoPageLayout
{
    /// <summary>
    /// This class creates a command that will export the active view to any supported format.
    /// </summary>
    [Guid("3fdb66c0-7e1f-463d-99f4-55a3c6d3093c")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoPageLayout.CommandExportActiveView")]
    public sealed class CommandExportActiveView : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion


        /* The delegate functions below allow us to harness some of the functions in the win32
         * and user32 libraries built in to Windows.  For instance, since many of the export
         * and printing functions require a Device Context, the getDC and releaseDC functions
         * can be used to obtain and release the DC for the device.  The GetDeviceCaps function
         * is an informational function - it can return information about different capabilities
         * of a device.  Here we use it to get the screen's available resolution, so that we 
         * can properly translate between the activeview's "exportframe" and the exporter.
         */

        /* GDI delegate to GetDeviceCaps function */
        [DllImport("GDI32.dll")]
        public static extern int GetDeviceCaps(int hdc, int nIndex);

        /* User32 delegates to getDC and ReleaseDC */
        [DllImport("User32.dll")]
        public static extern int GetDC(int hWnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(int hWnd, int hDC);
        
        //[DllImport("user32.dll", SetLastError = true)]
        //static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

        /* constants used for user32 calls */
        const uint SPI_GETFONTSMOOTHING = 74;
        const uint SPI_SETFONTSMOOTHING = 75;
        const uint SPIF_UPDATEINIFILE = 0x1;


        /* A reference to the application itself using HookHelper.
        * This will allow this code to work in either engine or
        * desktop application - the hookhelper handles calls to 
        * ActiveView, FocusMap, etc.
        */
        private IHookHelper m_hookHelper;
        private int dpi=300;
        private int ResampleRatio=1;
        private string fType="";
        private string fOutFilename="";
        private bool bClipToGraphicsExtent=false;
        public int DPI
        {
            set
            {
                dpi = value;
            }
        }
        public int Ratio
        {
            set
            {
                ResampleRatio = value;
            }
        }
        public string OutFilename
        {
            set
            {
                fOutFilename = value;
            }
        }
        public string Type
        {
            set
            {
                fType = value;
            }
        }
        public bool ClipToGraphicsExtent
        {
            set
            {
                bClipToGraphicsExtent = value;
            }
        }

        public CommandExportActiveView(int indpi, int inResampleRetio, string inOutFiname, string inType, bool inClipToGraphicsExtent)
        {
            /* Class constructor */
            base.m_category = "制图";
            base.m_caption = "输出地图";
            base.m_message = "输出地图";
            base.m_toolTip = "输出地图";
            base.m_name = base.m_category + "_" + base.m_caption;

            dpi = indpi;
            ResampleRatio = inResampleRetio;
            fOutFilename = inOutFiname;
            fType = inType;
            bClipToGraphicsExtent = inClipToGraphicsExtent;
            try
            {
                //load the bitmap for the icon
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
            {
                m_hookHelper = new HookHelperClass();
            }
            m_hookHelper.Hook = hook;

        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            /* The OnClick method calls the ExportActiveViewParameterized function with some parameters
             * which you can of course change.  The first parameter is resolution in dpi, the second is the resample ratio 
             * from 1(best quality) to 5 (fastest).  the third is a string which represents which export type you want to 
             * output (JPEG, PDF, etc.), the fourth is the directory to which you'd like to write, and the last is a 
             * boolean which determines whether or not the output will be clipped to graphics extent (for layouts).
             */
                    //string exportFolder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
					ExportActiveViewParameterized (dpi, ResampleRatio, fType, fOutFilename, false);
        }

        #endregion
        public IActiveView ExportActiveView
        {
            get;
            set;
        }
        private void ExportActiveViewParameterized(long iOutputResolution, long lResampleRatio, string ExportType, string sOutputName, Boolean bClipToGraphicsExtent)
        {
            /* EXPORT PARAMETER: (iOutputResolution) the resolution requested.
             * EXPORT PARAMETER: (lResampleRatio) Output Image Quality of the export.  The value here will only be used if the export
             * object is a format that allows setting of Output Image Quality, i.e. a vector exporter.
             * The value assigned to ResampleRatio should be in the range 1 to 5.
             * 1 corresponds to "Best", 5 corresponds to "Fast"
             * EXPORT PARAMETER: (ExportType) a string which contains the export type to create.
             * EXPORT PARAMETER: (sOutputDir) a string which contains the directory to output to.
             * EXPORT PARAMETER: (bClipToGraphicsExtent) Assign True or False to determine if export image will be clipped to the graphic 
             * extent of layout elements.  This value is ignored for data view exports
             */

            /* Exports the Active View of the document to selected output format. */
            IActiveView docActiveView =null;
            if (m_hookHelper == null)
                docActiveView = ExportActiveView;
            else
                docActiveView = m_hookHelper.ActiveView;
            
            IExport docExport;
            long iPrevOutputImageQuality;
            IOutputRasterSettings docOutputRasterSettings;
            IEnvelope PixelBoundsEnv;
            tagRECT exportRECT;
            tagRECT DisplayBounds;
            IDisplayTransformation docDisplayTransformation;
            IPageLayout docPageLayout;
            IEnvelope docMapExtEnv;
            long hdc;
            long tmpDC;
            string sNameRoot;
            long iScreenResolution;
            bool bReenable = false;


            IEnvelope docGraphicsExtentEnv;
            IUnitConverter pUnitConvertor;

            if (GetFontSmoothing())
            {
                /* font smoothing is on, disable it and set the flag to reenable it later. */
                bReenable = true;
                DisableFontSmoothing();
                if (GetFontSmoothing())
                {
                    //font smoothing is NOT successfully disabled, error out.
                    return;
                }
                //else font smoothing was successfully disabled.
            }


            // The Export*Class() type initializes a new export class of the desired type.

            if (ExportType == "PDF")
            { 
                docExport = new ExportPDFClass();
				IExportPDF pExPDF = docExport as IExportPDF;
                pExPDF.EmbedFonts = true;
            }
            else if (ExportType == "EPS")
            {
                docExport = new ExportPSClass();
            }
            else if (ExportType == "AI")
            {
                docExport = new ExportAIClass();
            }
            else if (ExportType == "BMP")
            {

                docExport = new ExportBMPClass();
            }
            else if (ExportType == "TIFF")
            {
                docExport = new ExportTIFFClass();
            }
            else if (ExportType == "SVG")
            {
                docExport = new ExportSVGClass();
            }
            else if (ExportType == "PNG")
            {
                docExport = new ExportPNGClass();
            }
            else if (ExportType == "GIF")
            {
                docExport = new ExportGIFClass();
            }
            else if (ExportType == "EMF")
            {
                docExport = new ExportEMFClass();
            }
            else if (ExportType == "JPG")
            {
                docExport = new ExportJPEGClass();
            }
            else
            {
                MessageBox.Show("Unsupported export type " + ExportType + ", defaulting to EMF.");
                ExportType = "EMF";
                docExport = new ExportEMFClass();
            }

            
            //  save the previous output image quality, so that when the export is complete it will be set back.
            docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
            iPrevOutputImageQuality = docOutputRasterSettings.ResampleRatio;


            if (docExport is IExportImage)
            {
                // always set the output quality of the DISPLAY to 1 for image export formats
                SetOutputQuality(docActiveView, 1);
            }
            else
            {
                // for vector formats, assign the desired ResampleRatio to control drawing of raster layers at export time   
                SetOutputQuality(docActiveView, lResampleRatio);
            }

            //set the name root for the export
            sNameRoot = docActiveView.FocusMap.Name;

            //set the export filename (which is the nameroot + the appropriate file extension)
            docExport.ExportFileName = sOutputName;

            
            /* Get the device context of the screen */
            tmpDC = GetDC(0);
            /* Get the screen resolution. */
            iScreenResolution = GetDeviceCaps((int)tmpDC, 88); //88 is the win32 const for Logical pixels/inch in X)
            /* release the DC. */
            ReleaseDC(0, (int)tmpDC);
            docExport.Resolution = iOutputResolution;


            if (docActiveView is IPageLayout)
            {
                //get the bounds of the "exportframe" of the active view.
                DisplayBounds = docActiveView.ExportFrame;
                //set up pGraphicsExtent, used if clipping to graphics extent.
                docGraphicsExtentEnv = GetGraphicsExtent(docActiveView);
            }
            else
            {
                //Get the bounds of the deviceframe for the screen.
                docDisplayTransformation = docActiveView.ScreenDisplay.DisplayTransformation;
                DisplayBounds = docDisplayTransformation.get_DeviceFrame();
            }

            PixelBoundsEnv = new Envelope() as IEnvelope;

            if (bClipToGraphicsExtent && (docActiveView is IPageLayout))
            {
                docGraphicsExtentEnv = GetGraphicsExtent(docActiveView);
                docPageLayout = docActiveView as PageLayout;
                pUnitConvertor = new UnitConverter();
            
                //assign the x and y values representing the clipped area to the PixelBounds envelope
                PixelBoundsEnv.XMin = 0;
                PixelBoundsEnv.YMin = 0;
                PixelBoundsEnv.XMax = pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.XMax, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution - pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.XMin, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution;
                PixelBoundsEnv.YMax = pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.YMax, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution - pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.YMin, docPageLayout.Page.Units, esriUnits.esriInches) * docExport.Resolution;

                //'assign the x and y values representing the clipped export extent to the exportRECT
                exportRECT.bottom = (int)(PixelBoundsEnv.YMax) + 1;
                exportRECT.left = (int)(PixelBoundsEnv.XMin);
                exportRECT.top = (int)(PixelBoundsEnv.YMin);
                exportRECT.right = (int)(PixelBoundsEnv.XMax) + 1;

                //since we're clipping to graphics extent, set the visible bounds.
                docMapExtEnv = docGraphicsExtentEnv;
            }
            else
            {
                double tempratio = iOutputResolution / iScreenResolution;
                double tempbottom = DisplayBounds.bottom * tempratio;
                double tempright = DisplayBounds.right * tempratio;
                //'The values in the exportRECT tagRECT correspond to the width
                //and height to export, measured in pixels with an origin in the top left corner.
                exportRECT.bottom = (int)Math.Truncate(tempbottom);
                exportRECT.left = 0;
                exportRECT.top = 0;
                exportRECT.right = (int)Math.Truncate(tempright);


                //populate the PixelBounds envelope with the values from exportRECT.
                // We need to do this because the exporter object requires an envelope object
                // instead of a tagRECT structure.
                PixelBoundsEnv.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);

                //since it's a page layout or an unclipped page layout we don't need docMapExtEnv.
                docMapExtEnv = null;
            }

            // Assign the envelope object to the exporter object's PixelBounds property.  The exporter object
            // will use these dimensions when allocating memory for the export file.
            docExport.PixelBounds = PixelBoundsEnv;

            // call the StartExporting method to tell docExport you're ready to start outputting.
            hdc = docExport.StartExporting();
          
            // Redraw the active view, rendering it to the exporter object device context instead of the app display.
            // We pass the following values:
            //  * hDC is the device context of the exporter object.
            //  * exportRECT is the tagRECT structure that describes the dimensions of the view that will be rendered.
            // The values in exportRECT should match those held in the exporter object's PixelBounds property.
            //  * docMapExtEnv is an envelope defining the section of the original image to draw into the export object.
            docActiveView.Output((int)hdc, (int)docExport.Resolution, ref exportRECT, docMapExtEnv, null);

            //finishexporting, then cleanup.
            docExport.FinishExporting();
            docExport.Cleanup();
            
            //MessageBox.Show("输出完成 " + sOutputName, "地图输出");
            
            //set the output quality back to the previous value
            SetOutputQuality(docActiveView, iPrevOutputImageQuality);
            if (bReenable)
            {
                /* reenable font smoothing if we disabled it before */
                EnableFontSmoothing();
                bReenable = false;
                if (!GetFontSmoothing())
                {
                    //error: cannot reenable font smoothing.
                    MessageBox.Show("Unable to reenable Font Smoothing", "Font Smoothing error"); 
                }
            }


            docMapExtEnv = null;
            PixelBoundsEnv = null;
        }


        private void SetOutputQuality(IActiveView docActiveView, long iResampleRatio)
        {
            /* This function sets OutputImageQuality for the active view.  If the active view is a pagelayout, then
             * it must also set the output image quality for EACH of the Maps in the pagelayout.
             */
            IGraphicsContainer oiqGraphicsContainer;
            IElement oiqElement;
            IOutputRasterSettings docOutputRasterSettings;
            IMapFrame docMapFrame;
            IActiveView TmpActiveView;

            if (docActiveView is IMap)
            {
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
            }
            else if (docActiveView is IPageLayout)
            {
                //assign ResampleRatio for PageLayout
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
                //and assign ResampleRatio to the Maps in the PageLayout
                oiqGraphicsContainer = docActiveView as IGraphicsContainer;
                oiqGraphicsContainer.Reset();

                oiqElement = oiqGraphicsContainer.Next();
                while (oiqElement != null)
                {
                    if (oiqElement is IMapFrame)
                    {
                        docMapFrame = oiqElement as IMapFrame;
                        TmpActiveView = docMapFrame.Map as IActiveView;
                        docOutputRasterSettings = TmpActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                        docOutputRasterSettings.ResampleRatio = (int)iResampleRatio;
                    }
                    oiqElement = oiqGraphicsContainer.Next();
                }

                docMapFrame = null;
                oiqGraphicsContainer = null;
                TmpActiveView = null;
            }
            docOutputRasterSettings = null;

        }

        private IEnvelope GetGraphicsExtent(IActiveView docActiveView)
        {
            /* Gets the combined extent of all the objects in the map. */
            IEnvelope GraphicsBounds;
            IEnvelope GraphicsEnvelope;
            IGraphicsContainer oiqGraphicsContainer;
            IPageLayout docPageLayout;
            IDisplay GraphicsDisplay;
            IElement oiqElement;

            GraphicsBounds = new EnvelopeClass();
            GraphicsEnvelope = new EnvelopeClass();
            docPageLayout = docActiveView as IPageLayout;
            GraphicsDisplay = docActiveView.ScreenDisplay;
            oiqGraphicsContainer = docActiveView as IGraphicsContainer;
            oiqGraphicsContainer.Reset();

            oiqElement = oiqGraphicsContainer.Next();
            while (oiqElement != null)
            {
                oiqElement.QueryBounds(GraphicsDisplay, GraphicsEnvelope);
                GraphicsBounds.Union(GraphicsEnvelope);
                oiqElement = oiqGraphicsContainer.Next();
            }

            return GraphicsBounds;

        }

        private void DisableFontSmoothing()
        {
            bool iResult;
            int pv = 0;

            /* call to systemparametersinfo to set the font smoothing value */
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 0, ref pv, SPIF_UPDATEINIFILE);
        }

        private void EnableFontSmoothing()
        {
            bool iResult;
            int pv = 0;

            /* call to systemparametersinfo to set the font smoothing value */
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 1, ref pv, SPIF_UPDATEINIFILE);
            
        }

        private Boolean GetFontSmoothing()
        {
            bool iResult;
            int pv = 0;

            /* call to systemparametersinfo to get the font smoothing value */
            iResult = SystemParametersInfo(SPI_GETFONTSMOOTHING, 0, ref pv, 0);

            if (pv > 0)
            {
                //pv > 0 means font smoothing is ON.
                return true;
            }
            else
            {
                //pv == 0 means font smoothing is OFF.
                return false;
            }

        }


    }
}
