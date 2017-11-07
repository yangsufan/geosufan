using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSymbology.Class
{
    public class FillSymbolObject : SymbolObject
    {
        public override ESRI.ArcGIS.Controls.esriSymbologyStyleClass StyleClass
        {
            get { return ESRI.ArcGIS.Controls.esriSymbologyStyleClass.esriStyleClassFillSymbols; }
        }

        private double m_OutlineWidth;
        private System.Drawing.Color m_FillColor;
        private System.Drawing.Color m_OutlineColor;

        [System.ComponentModel.DisplayName("边线宽度")]
        public double OutlineWidth
        {
            get { return m_OutlineWidth; }
            set { m_OutlineWidth = value; }
        }

        [System.ComponentModel.DisplayName("填充颜色")]
        public System.Drawing.Color FillColor
        {
            get { return m_FillColor; }
            set { m_FillColor = value; }
        }

        [System.ComponentModel.DisplayName("边线颜色")]
        public System.Drawing.Color OutlineColor
        {
            get { return m_OutlineColor; }
            set { m_OutlineColor = value; }
        }

        public FillSymbolObject()
        {
            m_OutlineWidth = 1;
            m_FillColor = System.Drawing.Color.Gray;
            m_OutlineColor = System.Drawing.Color.Black;
        }

        public override void InitClassSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            if (pSymbol is ESRI.ArcGIS.Display.IFillSymbol)
            {
                ESRI.ArcGIS.Display.IFillSymbol pFillSymbol = pSymbol as ESRI.ArcGIS.Display.IFillSymbol;
                m_OutlineWidth = pFillSymbol.Outline.Width;
                m_FillColor = ModuleCommon.GetWindowsColor(pFillSymbol.Color);
                m_OutlineColor = ModuleCommon.GetWindowsColor(pFillSymbol.Outline.Color);
            }
            else
            {
                m_OutlineWidth = 1;
                m_FillColor = System.Drawing.Color.Gray;
                m_OutlineColor = System.Drawing.Color.Black;
            }
        }

        public override void ReGenerateSymbol(ESRI.ArcGIS.Display.ISymbol pSymbol)
        {
            if (pSymbol != null && pSymbol is ESRI.ArcGIS.Display.IFillSymbol)
            {
                ESRI.ArcGIS.Display.IFillSymbol pFillSymbol = pSymbol as ESRI.ArcGIS.Display.IFillSymbol;
                ESRI.ArcGIS.Display.ILineSymbol pLineSymbol = pFillSymbol.Outline;
                pLineSymbol.Width = m_OutlineWidth;
                pLineSymbol.Color = ModuleCommon.GetESRIColor(m_OutlineColor);
                pFillSymbol.Outline = pLineSymbol;
                pFillSymbol.Color = ModuleCommon.GetESRIColor(m_FillColor);
            }
        }

        public override string[] PropertyNames
        {
            get { return new string[] { "OutlineWidth", "OutlineColor", "FillColor" }; }
        }
    }
}
