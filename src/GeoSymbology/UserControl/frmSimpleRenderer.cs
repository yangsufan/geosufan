using System;
using System.Collections.Generic;
using System.Text;
using DevDNB = DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace GeoSymbology
{
    public class frmSimpleRenderer : System.Windows.Forms.UserControl, IEditItem, IRendererUI
    {
        private esriSymbologyStyleClass m_SymbologyStyleClass;

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDesc;
        private DevComponents.DotNetBar.Controls.TextBoxX txtName;
        private DevComponents.DotNetBar.LabelX labelPreviewFore;
        private DevComponents.DotNetBar.LabelX labelX1;
    
        private void InitializeComponent()
        {
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.txtDesc = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelPreviewFore = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.txtDesc);
            this.panelEx1.Controls.Add(this.txtName);
            this.panelEx1.Controls.Add(this.labelPreviewFore);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(420, 350);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // txtDesc
            // 
            // 
            // 
            // 
            this.txtDesc.Border.Class = "TextBoxBorder";
            this.txtDesc.Location = new System.Drawing.Point(117, 150);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(238, 98);
            this.txtDesc.TabIndex = 5;
            // 
            // txtName
            // 
            // 
            // 
            // 
            this.txtName.Border.Class = "TextBoxBorder";
            this.txtName.Location = new System.Drawing.Point(117, 96);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(238, 21);
            this.txtName.TabIndex = 4;
            // 
            // labelPreviewFore
            // 
            this.labelPreviewFore.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewFore.Location = new System.Drawing.Point(117, 31);
            this.labelPreviewFore.Margin = new System.Windows.Forms.Padding(0);
            this.labelPreviewFore.Name = "labelPreviewFore";
            this.labelPreviewFore.Size = new System.Drawing.Size(80, 40);
            this.labelPreviewFore.TabIndex = 3;
            this.labelPreviewFore.Click += new System.EventHandler(this.Control_Click);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(37, 152);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(44, 18);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "ÃèÊö£º";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(37, 97);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(44, 18);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "Ãû³Æ£º";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(37, 42);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(44, 18);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "·ûºÅ£º";
            // 
            // frmSimpleRenderer
            // 
            this.Controls.Add(this.panelEx1);
            this.Name = "frmSimpleRenderer";
            this.Size = new System.Drawing.Size(420, 350);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        public frmSimpleRenderer()
        {
            InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            m_EditObject = null;
        }

        private void Control_Click(object sender, EventArgs e)
        {
            DevDNB.LabelX label = sender as DevDNB.LabelX;
            switch (label.Name)
            {
                case "labelPreviewFore":
                    m_EditObject = label;
                    Form.frmSymbolEdit symbolEdit = new GeoSymbology.Form.frmSymbolEdit(this, label.Tag as ISymbol, "");
                    symbolEdit.ShowDialog();
                    break;
            }
        }

        public void InitRendererObject(List<FieldInfo> pFields, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            InitRendererObject(pRenderer, _SymbologyStyleClass);
        }

        public void InitRendererObject(IFeatureLayer pFeatureLayer, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            InitRendererObject(pRenderer, _SymbologyStyleClass);
        }

        private void InitRendererObject(IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            m_SymbologyStyleClass = _SymbologyStyleClass;
            ISimpleRenderer pSimpleRenderer = pRenderer as ISimpleRenderer;
            txtName.Text = pSimpleRenderer.Label;
            txtDesc.Text = pSimpleRenderer.Description;
            if (pSimpleRenderer.Symbol == null)
                labelPreviewFore.Tag = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
            else
                labelPreviewFore.Tag = pSimpleRenderer.Symbol;
            labelPreviewFore.Image = ModuleCommon.Symbol2Picture(labelPreviewFore.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
        }

        public IFeatureRenderer Renderer
        {
            get
            {
                ISimpleRenderer pRenderer = new SimpleRendererClass();
                pRenderer.Label = txtName.Text;
                pRenderer.Description = txtDesc.Text;
                pRenderer.Symbol = labelPreviewFore.Tag as ISymbol;
                return pRenderer as IFeatureRenderer;
            }
        }

        public enumRendererType RendererType
        {
            get { return enumRendererType.SimpleRenderer; }
        }

        #region IEditItem ³ÉÔ±

        private object m_EditObject;

        public void DoAfterEdit(object newValue, System.Windows.Forms.DialogResult result, string editType)
        {
            if (m_EditObject is DevDNB.LabelX)
            {
                labelPreviewFore.Tag = newValue;
                if (labelPreviewFore.Image != null)
                {
                    labelPreviewFore.Image.Dispose();
                    labelPreviewFore.Image = null;
                }
                labelPreviewFore.Image = ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
            }
            m_EditObject = null;
        }

        #endregion
    }
}
