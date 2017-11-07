using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.SpatialAnalystTools;
using ESRI.ArcGIS.Geoprocessor;
//////GP工具函数    张琪  20110627
namespace GeoArcScene3DAnalyse
{
    class ClsGPTool
    {

        private bool RunTool(Geoprocessor geoprocessor, IGPProcess process)
        {
            // Set the overwrite output option to true
            geoprocessor.OverwriteOutput = true;

            try
            {
                geoprocessor.Execute(process, null);
                return true;

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return false;
            }


        }
        /// <summary>
        /// 通过GP调用CutFill类进行填挖方分析
        /// </summary>
        /// <param name="BeforeSuface"></param>
        /// <param name="AfterSuface"></param>
        /// <param name="out_raster"></param>
        /// <param name="zFactor"></param>
        public void CutFillGP(string BeforeSuface, string AfterSuface, string out_raster, double zFactor)  // 填挖方分析    张琪  20110627
        {
            CutFill pCutFill = new CutFill();
            pCutFill.in_after_surface = AfterSuface;
            pCutFill.in_before_surface = BeforeSuface;
            pCutFill.out_raster = out_raster;
            pCutFill.z_factor = zFactor;
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            if (!RunTool(gp, pCutFill))
            {
                return;
            }

            return;

        }
        /// <summary>
        /// 通过GP调用Slope类进行坡度分析
        /// </summary>
        /// <param name="InRasterPath"></param>
        /// <param name="OutRasterPath"></param>
        /// <param name="Unit"></param>
        /// <param name="zFactor"></param>
        public void SlopeAnalyseGP(string InRasterPath, string OutRasterPath, string Unit, double zFactor)//通过GP调用Slope类进行坡度分析(未实现TIN数据的坡度分析)
        {
            Slope pSlope = new Slope();                                                                   // 张琪  20110609
            pSlope.in_raster = InRasterPath;//要扩展名完整的路径
            pSlope.out_raster = OutRasterPath;//要扩展名的完整路径(GRID除外)
            pSlope.output_measurement = Unit;
            pSlope.z_factor = zFactor;
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            if (!RunTool(gp, pSlope))
            {
                return;
            }

            return;
        }
        /// <summary>
        /// 通过用Gp实现通视分析
        /// </summary>
        /// <param name="observer_features"></param>
        /// <param name="in_raster"></param>
        /// <param name="out_raster"></param>
        /// <param name="curvature"></param>
        /// <param name="zFactor"></param>
        public void ViewshedAnalyseGP(string observer_features, string in_raster, string out_raster, string curvature, double zFactor)  // 张琪  20110611
        {
            Viewshed pViewshed = new Viewshed();
            pViewshed.in_observer_features = observer_features;
            pViewshed.in_raster = in_raster;
            pViewshed.out_raster = out_raster;
            pViewshed.z_factor = zFactor;
            pViewshed.curvature_correction = curvature;
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;

            if (!RunTool(gp, pViewshed))
            {
                return;
            }

            return;

        }
    }
}
