using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSymbology.Class
{
    public class LineSymbolObject:SymbolObject
    {
        public override ESRI.ArcGIS.Controls.esriSymbologyStyleClass StyleClass
        {
            get { return ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassLineSymbols; }
        }

        private double m_Width;
        private double m_Offset;
        private System.Drawing.Color m_LineColor;

        [System.ComponentModel.DisplayName("Ïß¿í")]
        public double Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        [System.ComponentModel.DisplayName("Æ«ÒÆ")]
        public double Offset
        {
            get { return m_Offset; }
            set { m_Offset = value; }
        }

        [System.ComponentModel.DisplayName("ÑÕÉ«")]
        public System.Drawing.Color LineColor
        {
            get { return m_LineColor; }
            set { m_LineColor = value; }
        }

        public LineSymbolObject()
        {
            m_Width = 1;
            m_Offset = 0;
            m_LineColor = System.Drawing.Color.Black;
        }

        public override void InitClassSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            if (pSymbol is ESRI.ArcGIS.Display.ILineSymbol)
            {
                ESRI.ArcGIS.Display.ILineSymbol pLineSymbol = pSymbol as ESRI.ArcGIS.Display.ILineSymbol;
                m_Width = pLineSymbol.Width;
                m_Offset = 0;
                m_LineColor = ModuleCommon.GetWindowsColor(pLineSymbol.Color);
            }
            else
            {
                m_Width = 1;
                m_Offset = 0;
                m_LineColor = System.Drawing.Color.Black;
            }
        }

        public override void ReGenerateSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            if (pSymbol!=null&&pSymbol is ESRI.ArcGIS.Display.ILineSymbol)
            {
                ESRI.ArcGIS.Display.ILineSymbol pLineSymbol = pSymbol as ESRI.ArcGIS.Display.ILineSymbol;
                pLineSymbol.Width = m_Width;
                //pLineSymbol.Offset = m_Offset;
                pLineSymbol.Color = ModuleCommon.GetESRIColor(m_LineColor);
            }
        }

        public override string[] PropertyNames
        {
            get { return new string[] { "Width", "Offset", "LineColor" }; }
        }
    }
}
