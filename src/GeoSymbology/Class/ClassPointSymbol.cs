using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSymbology.Class
{
    public abstract class SymbolObject
    {
        public static SymbolObject CreateClassSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            SymbolObject pSymbolObject = null;
            if (pSymbol is ESRI.ArcGIS.Display.IMarkerSymbol)
            {
                pSymbolObject = new PointSymbolObject();
                pSymbolObject.InitClassSymbol(pSymbol);
            }
            else if (pSymbol is ESRI.ArcGIS.Display.ILineSymbol)
            {
                pSymbolObject = new LineSymbolObject();
                pSymbolObject.InitClassSymbol(pSymbol);
            }
            else if (pSymbol is ESRI.ArcGIS.Display.IFillSymbol)
            {
                pSymbolObject = new FillSymbolObject();
                pSymbolObject.InitClassSymbol(pSymbol);
            }

            return pSymbolObject;
        }
        [System.ComponentModel.Browsable(false)]
        public abstract ESRI.ArcGIS.Controls.esriSymbologyStyleClass StyleClass { get;}
        public abstract void InitClassSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol);
        public abstract void ReGenerateSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol);
        [System.ComponentModel.Browsable(false)]
        public abstract string[] PropertyNames { get;}
    }
    public class PointSymbolObject:SymbolObject
    {
        public override ESRI.ArcGIS.Controls.esriSymbologyStyleClass StyleClass
        {
            get { return ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassMarkerSymbols; }
        }

        private double m_Size;
        private double m_XOffset;
        private double m_YOffset;
        private double m_Angle;
        private System.Drawing.Color m_PointColor;

        [System.ComponentModel.DisplayName("符号大小")]
        public double Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        [System.ComponentModel.DisplayName("X偏移")]
        public double XOffset
        {
            get { return m_XOffset; }
            set { m_XOffset = value; }
        }

        [System.ComponentModel.DisplayName("Y偏移")]
        public double YOffset
        {
            get { return m_YOffset; }
            set { m_YOffset = value; }
        }

        [System.ComponentModel.DisplayName("旋转角度")]
        public double Angle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }

        [System.ComponentModel.DisplayName("颜色")]
        public System.Drawing.Color PointColor
        {
            get { return m_PointColor; }
            set { m_PointColor = value; }
        }

        public PointSymbolObject()
        {
            m_Size = 2;
            m_XOffset = 0;
            m_YOffset = 0;
            m_Angle = 0;
            m_PointColor = System.Drawing.Color.Black;
        }

        #region SymbolObject 成员

        public override void InitClassSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            if (pSymbol is ESRI.ArcGIS.Display.IMarkerSymbol)
            {
                ESRI.ArcGIS.Display.IMarkerSymbol pMarkerSymbol = pSymbol as ESRI.ArcGIS.Display.IMarkerSymbol;
                m_Size = pMarkerSymbol.Size;
                m_XOffset = pMarkerSymbol.XOffset;
                m_YOffset = pMarkerSymbol.YOffset;
                m_Angle = pMarkerSymbol.Angle;
                m_PointColor = ModuleCommon.GetWindowsColor(pMarkerSymbol.Color);
            }
            else
            {
                m_Size = 2;
                m_XOffset = 0;
                m_YOffset = 0;
                m_Angle = 0;
                m_PointColor = System.Drawing.Color.Black;
            }
        }

        public override void ReGenerateSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            if (pSymbol != null && pSymbol is ESRI.ArcGIS.Display.IMarkerSymbol)
            {
                ESRI.ArcGIS.Display.IMarkerSymbol pMarkerSymbol = pSymbol as ESRI.ArcGIS.Display.IMarkerSymbol;
                pMarkerSymbol.Size = m_Size;
                pMarkerSymbol.XOffset = m_XOffset;
                pMarkerSymbol.YOffset = m_YOffset;
                pMarkerSymbol.Angle = m_Angle;
                pMarkerSymbol.Color = ModuleCommon.GetESRIColor(m_PointColor);
            }
        }

        public override string[] PropertyNames
        {
            get { return new string[] { "Size","XOffset","YOffset","Angle","PointColor"}; }
        }

        #endregion
    }
}
