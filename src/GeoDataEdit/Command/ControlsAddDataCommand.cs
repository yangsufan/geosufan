using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataEdit.Command
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout
    /// </summary>
    [Guid("7522f8d8-f31c-4de6-85d8-9fca49c1f0fe")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoDataEdit.Command.ControlsAddDataCommand")]
    public sealed class ControlsAddDataCommand : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
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

        private IHookHelper m_hookHelper = null;
        private AxMapControl m_AxMapControl;
        public ControlsAddDataCommand(AxMapControl pAxMapControl)
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "加载数据";  //localizable text 
            base.m_message = "加载数据";  //localizable text
            base.m_toolTip = "";  //localizable text
            base.m_name = "AddData";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
            m_AxMapControl = pAxMapControl;
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
          ESRI.ArcGIS.SystemUI.ICommand pCommand= new ControlsAddDataCommandClass();
          pCommand.OnCreate(m_AxMapControl.Object);
          pCommand.OnClick();
          LayersCompose(m_AxMapControl);

        }

        /// <summary>
        /// 对mapcontrol上的图层进行排序
        /// </summary>
        /// <param name="vAxMapControl"></param>
        private void LayersCompose(AxMapControl pMapcontrol)
        {
            IMap pMap = pMapcontrol.Map;
            int[] iLayerIndex = new int[2] { 0, 0 };
            int[] iFeatureLayerIndex = new int[3] { 0, 0, 0 };

            int iCount = pMapcontrol.LayerCount;
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = pMap.get_Layer(iIndex) as IFeatureLayer;

                if (pFeatureLayer == null) return;
                switch (pFeatureLayer.FeatureClass.FeatureType)
                {
                    case esriFeatureType.esriFTDimension:
                        pMap.MoveLayer(pFeatureLayer, iLayerIndex[0]);
                        iLayerIndex[0] = iLayerIndex[0] + 1;
                        break;
                    case esriFeatureType.esriFTAnnotation:

                        pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1]);
                        iLayerIndex[1] = iLayerIndex[1] + 1;
                        break;
                    case esriFeatureType.esriFTSimple:

                        switch (pFeatureLayer.FeatureClass.ShapeType)
                        {
                            case esriGeometryType.esriGeometryPoint:
                                pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0]);
                                iFeatureLayerIndex[0] = iFeatureLayerIndex[0] + 1;
                                break;
                            case esriGeometryType.esriGeometryLine:
                            case esriGeometryType.esriGeometryPolyline:
                                pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0] + iFeatureLayerIndex[1]);
                                iFeatureLayerIndex[1] = iFeatureLayerIndex[1] + 1;
                                break;
                            case esriGeometryType.esriGeometryPolygon:
                                pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0] + iFeatureLayerIndex[1] + iFeatureLayerIndex[2]);
                                iFeatureLayerIndex[2] = iFeatureLayerIndex[2] + 1;
                                break;
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
