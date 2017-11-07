using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSymbology.UserControl
{
    public class frmSizeBreakRenderer:System.Windows.Forms.UserControl
    {
        private DevComponents.DotNetBar.PanelEx panelContainer;
    
        private void InitializeComponent()
        {
            this.panelContainer = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelContainer.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelContainer.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelEx1";
            this.panelContainer.Size = new System.Drawing.Size(451, 287);
            this.panelContainer.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelContainer.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelContainer.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelContainer.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelContainer.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelContainer.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelContainer.Style.GradientAngle = 90;
            this.panelContainer.TabIndex = 0;
            // 
            // frmSizeBreakRenderer
            // 
            this.Controls.Add(this.panelContainer);
            this.Name = "frmSizeBreakRenderer";
            this.Size = new System.Drawing.Size(451, 287);
            this.ResumeLayout(false);

        }

        public frmSizeBreakRenderer()
        {
            InitializeComponent();
        }
    }
}
