using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoProperties.UserControls
{
    public partial class uctrSource : UserControl
    {
        ILayer m_pLayer;

        public uctrSource(ILayer pLayer)
        {
            m_pLayer = pLayer;

            try
            {
                InitializeComponent();
                GetLayerExtent();
                GetDataSource();
            }
            catch
            {
                
            }
        }

        private void GetLayerExtent()
        {
            IEnvelope pEnvelop = (m_pLayer as IGeoDataset).Extent;
            lblWest.Text = pEnvelop.XMin.ToString();
            lblEast.Text = pEnvelop.XMax.ToString();
            lblNorth.Text = pEnvelop.YMax.ToString();
            lblSouth.Text = pEnvelop.YMin.ToString();
        }
        private void GetDataSource()
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)m_pLayer;
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            IDataset pFeatureDataset = pFeatureClass as IDataset;
            ISpatialReference pSpatialRef;
            pSpatialRef= (pFeatureClass as IGeoDataset).SpatialReference;
            
            //判断如果转换成投影坐标是否成功 xisheng 20111122
            IProjectedCoordinateSystem pProjectedCoordinateSystem = ((pFeatureClass as IGeoDataset).SpatialReference) as IProjectedCoordinateSystem;
            IGeographicCoordinateSystem pGeographicCoordinateSystem = null ;
            if (pProjectedCoordinateSystem == null)
            {
                pGeographicCoordinateSystem = ((pFeatureClass as IGeoDataset).SpatialReference) as IGeographicCoordinateSystem;
            }
            else
            {
                pGeographicCoordinateSystem = pProjectedCoordinateSystem.GeographicCoordinateSystem;
            }
            //**************************************************end
            string strDataType = pFeatureLayer.DataSourceType;
            string strLocation = pFeatureDataset.Workspace.PathName;
            string strFeatureDSName = pFeatureDataset.Name;
            string strFCName = pFeatureClass.AliasName;
            string strFeatureType = pFeatureClass.FeatureType.ToString().Substring(6);
            string strGeometryType = pFeatureClass.ShapeType.ToString().Substring(12);

            if (pProjectedCoordinateSystem != null)
            {
                string strPCSName = pProjectedCoordinateSystem.Name;
                string strProjection = pProjectedCoordinateSystem.Projection.Name;
                string strFalseEasting = pProjectedCoordinateSystem.FalseEasting.ToString();
                string strFalseNorthing = pProjectedCoordinateSystem.FalseNorthing.ToString();
                string strCentralMeridian = pProjectedCoordinateSystem.get_CentralMeridian(true).ToString();
                string strScaleFactor = pProjectedCoordinateSystem.ScaleFactor.ToString();
                string strLinearUnit = pProjectedCoordinateSystem.CoordinateUnit.Name;

                string strGCSName = pGeographicCoordinateSystem.Name;
                string strDatum = pGeographicCoordinateSystem.Datum.Name;
                string strPrimeMeridian = pGeographicCoordinateSystem.PrimeMeridian.Name;
                string strAngularUnit = pGeographicCoordinateSystem.CoordinateUnit.Name;
                txtDataSource.Text =
                    "Data Type:\t\t\t" + strDataType +
                    "\r\n位置:\t\t\t" + strLocation +
                    "\r\n要素集:\t\t" + strFeatureDSName +
                    "\r\n要素类:\t\t\t" + strFCName +
                    "\r\n要素类型:\t\t\t" + strFeatureType +
                    "\r\n几何类型:\t\t\t" + strGeometryType +
                    "\r\n\r\n投影坐标系信息:\t" + strPCSName +
                    "\r\n投影:\t\t\t" + strProjection +
                    "\r\nFalse_Easting:\t\t\t" + strFalseEasting +
                    "\r\nFalse_Northing:\t\t\t" + strFalseNorthing +
                    "\r\nCentral_Meridian:\t\t" + strCentralMeridian +
                    "\r\nScale_Factor:\t\t\t" + strScaleFactor +
                    "\r\nLinear Unit:\t\t\t" + strLinearUnit +
                    "\r\n\r\nGeographic Coordinate System:\t" + strGCSName +
                    "\r\nDatum:\t\t\t\t" + strDatum +
                    "\r\nPrime Meridian:\t\t\t" + strPrimeMeridian +
                    "\r\nAngular Unit:\t\t\t" + strAngularUnit + "\r\n";
            }
            else
            {
                string strGCSName = pGeographicCoordinateSystem.Name;
                string strDatum = pGeographicCoordinateSystem.Datum.Name;
                string strPrimeMeridian = pGeographicCoordinateSystem.PrimeMeridian.Name;
                string strAngularUnit = pGeographicCoordinateSystem.CoordinateUnit.Name;
                txtDataSource.Text =
                    "Data Type:\t\t\t" + strDataType +
                    "\r\nLocation:\t\t\t" + strLocation +
                    "\r\nFeature Dataset:\t\t" + strFeatureDSName +
                    "\r\nFeature Class:\t\t\t" + strFCName +
                    "\r\nFeature Type:\t\t\t" + strFeatureType +
                    "\r\nGeometry Type:\t\t\t" + strGeometryType +
                    "\r\n\r\nGeographic Coordinate System:\t" + strGCSName +
                    "\r\nDatum:\t\t\t\t" + strDatum +
                    "\r\nPrime Meridian:\t\t\t" + strPrimeMeridian +
                    "\r\nAngular Unit:\t\t\t" + strAngularUnit + "\r\n";
            }
        }

        private string MapUnitChinese(esriUnits mapUnit)
        {
            string mapUnitChinese = "";
            switch (mapUnit)
            {
                case esriUnits.esriCentimeters:
                    mapUnitChinese = "厘米";
                    break;
                case esriUnits.esriDecimeters:
                    mapUnitChinese = "分米";
                    break;
                case esriUnits.esriKilometers:
                    mapUnitChinese = "千米";
                    break;
                case esriUnits.esriMeters:
                    mapUnitChinese = "米";
                    break;
                case esriUnits.esriMillimeters:
                    mapUnitChinese = "毫米";
                    break;
                case esriUnits.esriInches:
                    mapUnitChinese = "英寸";
                    break;
                case esriUnits.esriUnknownUnits:
                    mapUnitChinese = "未知单位";
                    break;
                default:
                    mapUnitChinese = "";
                    break;
            }
            return mapUnitChinese;
        }

        private void btnSetDataSource_Click(object sender, EventArgs e)
        {

        }
    }
}