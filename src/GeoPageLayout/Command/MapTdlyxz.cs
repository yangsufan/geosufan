using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;

namespace GeoPageLayout
{
    /// <summary>
    /// Summary description for CommandSelOutmap.
    /// </summary>
    [Guid("7569408E-569E-404D-8BF9-B41F3E8C4967")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoPageLayout.CommandSelOutmap")]
    public sealed class CommandSelOutmap : BaseCommand
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
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper;

        public CommandSelOutmap()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "GeoPageLayout_CommandSelOutmap";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

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

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            // TODO:  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_hookHelper.Hook == null) return;

            List<IGeometry> vTemp = GetDataGeometry(m_hookHelper.FocusMap);
            if (vTemp == null) return;
            ESRI.ArcGIS.Geometry.IGeometry pGeometry = GetUnion(vTemp);

            if (pGeometry == null) return;

            ESRI.ArcGIS.Carto.IMap pMap = m_hookHelper.FocusMap;
            GeoPageLayout gpl = new GeoPageLayout(pMap, pGeometry);
            gpl.typePageLayout = 2;
            gpl.MapOut();
            gpl = null;
        }

        #endregion


        private List<ESRI.ArcGIS.Geometry.IGeometry> GetDataGeometry(IMap pMap)
        {
            List<IGeometry> lstGeometrys = new List<IGeometry>();

            //IGeometry pTempGeo = null;
            if (pMap.SelectionCount < 1)
            {
                System.Windows.Forms.MessageBox.Show("需要选择范围");
                return null;
            }

            IActiveView pAv = pMap as IActiveView;
            ISelection pSelection = pMap.FeatureSelection;
            if (pSelection == null) return null;

            IEnumFeature pEnumFea = pSelection as IEnumFeature;
            IFeature pFea = pEnumFea.Next();
            while (pFea != null)
            {
                if (pFea.ShapeCopy != null)
                {
                    if (pFea.Shape.GeometryType != esriGeometryType.esriGeometryPolygon) continue;
                    lstGeometrys.Add(pFea.ShapeCopy);
                }
                pFea = pEnumFea.Next();
            }

            return lstGeometrys;
        }

        /// <summary>
        /// union几个面要素
        /// </summary>
        /// <param name="lstGeometry">需要操作的面要素集合</param>
        /// <returns>返回union后的图形</returns>
        public static IGeometry GetUnion(List<IGeometry> lstGeometry)
        {
            IGeometryBag pGeoBag = new GeometryBagClass();
            IGeometryCollection pGeoCol = pGeoBag as IGeometryCollection;
           
            if (lstGeometry.Count < 1) return null;
            if (lstGeometry[0].SpatialReference != null)
            {
                pGeoBag.SpatialReference = lstGeometry[0].SpatialReference;
            }

            object obj = System.Type.Missing;
            for (int i = 0; i < lstGeometry.Count; i++)
            {
                IGeometry pTempGeo = lstGeometry[i];
                pGeoCol.AddGeometry(pTempGeo, ref obj, ref obj);
            }


            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeoBag as IEnumGeometry);
            IGeometry pGeo = pTopo as IGeometry;

            return pGeo;
        }

    }
}
