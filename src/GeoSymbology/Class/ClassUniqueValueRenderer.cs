using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;

namespace GeoSymbology
{
    public class ClassUniqueValueRenderer:ClassRenderer
    {
        private ComboValue m_Field1;
        /// <summary>
        /// ½¥±ä×Ö¶Î
        /// </summary>
        public ComboValue Field1
        {
            get { return m_Field1; }
            set { m_Field1 = value; }
        }

        private ComboValue m_Field2;
        /// <summary>
        /// ½¥±ä×Ö¶Î
        /// </summary>
        public ComboValue Field2
        {
            get { return m_Field2; }
            set { m_Field2 = value; }
        }

        private ComboValue m_Field3;
        /// <summary>
        /// ½¥±ä×Ö¶Î
        /// </summary>
        public ComboValue Field3
        {
            get { return m_Field3; }
            set { m_Field3 = value; }
        }

        private string m_ColorRamp;
        /// <summary>
        /// ÑÕÉ«·½°¸
        /// </summary>
        public string ColorRamp
        {
            get { return m_ColorRamp; }
            set { m_ColorRamp = value; }
        }

        public ClassUniqueValueRenderer(esriSymbologyStyleClass _SymbologyStyleClass)
            : base()
        {
        }


        public override void DoButtonClick(DevComponents.DotNetBar.ButtonX button)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void DoListValueItemMouseDoubleClick(int x, int y)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override ESRI.ArcGIS.Carto.IFeatureRenderer FeatureRenderer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override void InitRendererObject(ESRI.ArcGIS.Carto.IFeatureRenderer _Renderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        
        public override void RefreshValueItem()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
