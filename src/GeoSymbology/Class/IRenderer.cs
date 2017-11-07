using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;

namespace GeoSymbology
{
    public interface IRendererUI
    {
        void InitRendererObject(ESRI.ArcGIS.Carto.IFeatureLayer pFeatureLayer, ESRI.ArcGIS.Carto.IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass);
        void InitRendererObject(List<FieldInfo> pFields, ESRI.ArcGIS.Carto.IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass);
        ESRI.ArcGIS.Carto.IFeatureRenderer Renderer { get;}
        enumRendererType RendererType { get;}
    }
    public interface IRasterRendererUI
    {
        void InitRasterRendererObject(ESRI.ArcGIS.Carto.ILayer  pLayer, ESRI.ArcGIS.Carto.IRasterRenderer  pRasterRenderer);
        void InitRasterRendererObject(List<FieldInfo> pFields, ESRI.ArcGIS.Carto.IRasterRenderer pRasterRenderer);
        ESRI.ArcGIS.Carto.IRasterRenderer  RasterRenderer { get; }
        enumRasterRendererType RasterRendererType { get; }
    }

    public enum enumRendererType
    {
        SimpleRenderer,
        UniqueValueRenderer,
        BreakColorRenderer,
        BreakSizeRenderer,
        ChartRenderer
    }
    public enum enumRasterRendererType
    {
        RGBRenderer,
        UniqueValueRenderer,
        StretchColorRampRenderer,
        ClassifyColorRampRenderer
    }
    public interface IEditItem
    {
        void DoAfterEdit(object newValue, System.Windows.Forms.DialogResult result, string editType);
    }

    public class ColorItem
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        private System.Drawing.Image m_ColorImage;
        public System.Drawing.Image ColorImage
        {
            get { return m_ColorImage; }
            set { m_ColorImage = value; }
        }
        private IColorRamp m_ColorRamp;
        public IColorRamp ColorRamp
        {
            get { return m_ColorRamp; }
            set { m_ColorRamp = value; }
        }

        public ColorItem()
        {
        }

        public override string ToString()
        {
            return m_Name;
        }
    }

    public class FieldInfo
    {
        private string m_FieldName;
        public string FieldName
        {
            get { return m_FieldName; }
            set { m_FieldName = value; }
        }
        private string m_FieldDesc;
        public string FieldDesc
        {
            get { return m_FieldDesc; }
            set { m_FieldDesc = value; }
        }
        private ESRI.ArcGIS.Geodatabase.esriFieldType m_FieldType;
        public ESRI.ArcGIS.Geodatabase.esriFieldType FieldType
        {
            get { return m_FieldType; }
            set { m_FieldType = value; }
        }

        public override string ToString()
        {
            return m_FieldName;
        }
    }
}
