using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;

namespace GeoStatistics.Command
{
    /// <summary>
    /// Summary description for ToolAreaStatistics.
    /// </summary>
    [Guid("2AA55347-A5DA-4fa8-9E63-1E14D2EBC863")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("GeoStatistics.ToolSelAreaStatistics")]
    public sealed class ToolSelAreaStatistics : BaseTool
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
        private IMapControlDefault m_MapControl = null;
        private IActiveView m_pActiveView = null;
        public ToolSelAreaStatistics()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "Statistics"; //localizable text 
            base.m_caption = "要素范围统计";  //localizable text 
            base.m_message = "要素范围统计";  //localizable text
            base.m_toolTip = "要素范围统计";  //localizable text
            base.m_name = "ToolSelAreaStatistics";   //unique id, non-localizable (e.g. "MyCategory_MyTool")
            try
            {
                //
                // TODO: change resource name if necessary
                //
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
            m_pActiveView = m_hookHelper.FocusMap as IActiveView;
            // TODO:  Add ToolAreaStatistics.OnCreate implementation
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            if (m_hookHelper.FocusMap == null) return;

            List<IGeometry> vTemp = GetDataGeometry(m_hookHelper.FocusMap);
            if (vTemp == null) return;
            ESRI.ArcGIS.Geometry.IGeometry pGeometry = GetUnion(vTemp);

            if (pGeometry == null) return;
            //DrawGeometry(pGeometry);
            frmAreaStatistics frm = new frmAreaStatistics();
            frm.CurGeometry = pGeometry;
            frm.CurMap = m_hookHelper.Hook as IMapControlDefault;
           // frm.SetSliderValue(false);
            frm.InitFrm();
            frm.ShowDialog();
        }
       


        private List<ESRI.ArcGIS.Geometry.IGeometry> GetDataGeometry(IMap pMap)
        {
            List<IGeometry> lstGeometrys = new List<IGeometry>();

            //IGeometry pTempGeo = null;
            if (pMap.SelectionCount < 1)
            {
                System.Windows.Forms.MessageBox.Show("请使用地图工具条中的【选择要素】工具在当前地图中选择要素！");
                return null;
            }

            IActiveView pAv = pMap as IActiveView;
            ISelection pSelection = pMap.FeatureSelection;
            if (pSelection == null) return null;

            IGeometry pgeometry = null;
            IEnumFeature pEnumFea = pSelection as IEnumFeature;
            IFeature pFea = pEnumFea.Next();
            while (pFea != null)
            {
                if (pFea.ShapeCopy != null)
                {
                //    if (pFea.Shape.GeometryType != esriGeometryType.esriGeometryPolygon)
                //    {
                //        pFea = pEnumFea.Next();
                //        continue;
                //    }
                    if (pgeometry == null || pgeometry.GeometryType == pFea.Shape.GeometryType)
                    {
                        pgeometry = pFea.ShapeCopy;
                    }
                    else
                    {
                        pFea = pEnumFea.Next();
                        continue;
                    }
                    lstGeometrys.Add(pgeometry);
                   // return lstGeometrys;//考虑到各种要素都有 所以就只获得第一个了
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

            //ITopologicalOperator pTopo = new PolygonClass();
            //pTopo.ConstructUnion(pGeoBag as IEnumGeometry);
            //IGeometry pGeo = pTopo as IGeometry;

            return pGeoBag as IGeometry;
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
        }

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolAreaStatistics.OnMouseMove implementation
        }

        public override void OnMouseUp(int Button, int Shift, int X, int Y)
        {
            // TODO:  Add ToolAreaStatistics.OnMouseUp implementation
        }
        #endregion
    }
}
