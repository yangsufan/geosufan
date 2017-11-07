namespace GeoProperties.UserControls
{
    partial class ucSpatialIndex
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSpatialIndex));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.sliderTransparency = new DevComponents.DotNetBar.Controls.Slider();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.colorNoData = new DevComponents.DotNetBar.ColorPickerButton();
            this.grpDataSource = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtGridSize3 = new System.Windows.Forms.TextBox();
            this.txtGridSize1 = new System.Windows.Forms.TextBox();
            this.txtGridSize2 = new System.Windows.Forms.TextBox();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.btnCalculate = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panelEx1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.grpDataSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.groupPanel1);
            this.panelEx1.Controls.Add(this.grpDataSource);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(394, 263);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.sliderTransparency);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.colorNoData);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(3, 141);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(388, 79);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 13;
            this.groupPanel1.Text = "影像无效值设置";
            // 
            // sliderTransparency
            // 
            this.sliderTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderTransparency.Location = new System.Drawing.Point(191, 15);
            this.sliderTransparency.Name = "sliderTransparency";
            this.sliderTransparency.Size = new System.Drawing.Size(181, 23);
            this.sliderTransparency.TabIndex = 11;
            this.sliderTransparency.Text = "透明";
            this.sliderTransparency.Value = 0;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(13, 15);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(103, 23);
            this.labelX4.TabIndex = 10;
            this.labelX4.Text = "无效值颜色设置：";
            // 
            // colorNoData
            // 
            this.colorNoData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.colorNoData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.colorNoData.Image = ((System.Drawing.Image)(resources.GetObject("colorNoData.Image")));
            this.colorNoData.Location = new System.Drawing.Point(122, 15);
            this.colorNoData.Name = "colorNoData";
            this.colorNoData.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.colorNoData.Size = new System.Drawing.Size(63, 23);
            this.colorNoData.TabIndex = 9;
            // 
            // grpDataSource
            // 
            this.grpDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDataSource.BackColor = System.Drawing.Color.Transparent;
            this.grpDataSource.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpDataSource.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpDataSource.Controls.Add(this.txtGridSize3);
            this.grpDataSource.Controls.Add(this.txtGridSize1);
            this.grpDataSource.Controls.Add(this.txtGridSize2);
            this.grpDataSource.Controls.Add(this.btnDel);
            this.grpDataSource.Controls.Add(this.btnCalculate);
            this.grpDataSource.Controls.Add(this.labelX3);
            this.grpDataSource.Controls.Add(this.labelX2);
            this.grpDataSource.Controls.Add(this.labelX1);
            this.grpDataSource.DrawTitleBox = false;
            this.grpDataSource.Location = new System.Drawing.Point(3, 3);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new System.Drawing.Size(388, 137);
            // 
            // 
            // 
            this.grpDataSource.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpDataSource.Style.BackColorGradientAngle = 90;
            this.grpDataSource.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpDataSource.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderBottomWidth = 1;
            this.grpDataSource.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpDataSource.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderLeftWidth = 1;
            this.grpDataSource.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderRightWidth = 1;
            this.grpDataSource.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderTopWidth = 1;
            this.grpDataSource.Style.CornerDiameter = 4;
            this.grpDataSource.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpDataSource.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpDataSource.TabIndex = 12;
            this.grpDataSource.Text = "空间索引";
            // 
            // txtGridSize3
            // 
            this.txtGridSize3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGridSize3.Location = new System.Drawing.Point(75, 61);
            this.txtGridSize3.Name = "txtGridSize3";
            this.txtGridSize3.Size = new System.Drawing.Size(294, 21);
            this.txtGridSize3.TabIndex = 14;
            this.txtGridSize3.TextChanged += new System.EventHandler(this.txtGridSize3_TextChanged);
            // 
            // txtGridSize1
            // 
            this.txtGridSize1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGridSize1.Location = new System.Drawing.Point(75, 6);
            this.txtGridSize1.Name = "txtGridSize1";
            this.txtGridSize1.Size = new System.Drawing.Size(294, 21);
            this.txtGridSize1.TabIndex = 12;
            this.txtGridSize1.TextChanged += new System.EventHandler(this.txtGridSize1_TextChanged);
            // 
            // txtGridSize2
            // 
            this.txtGridSize2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGridSize2.Location = new System.Drawing.Point(75, 34);
            this.txtGridSize2.Name = "txtGridSize2";
            this.txtGridSize2.Size = new System.Drawing.Size(294, 21);
            this.txtGridSize2.TabIndex = 13;
            this.txtGridSize2.TextChanged += new System.EventHandler(this.txtGridSize2_TextChanged);
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Location = new System.Drawing.Point(292, 88);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 8;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalculate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCalculate.Location = new System.Drawing.Point(211, 88);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 23);
            this.btnCalculate.TabIndex = 6;
            this.btnCalculate.Text = "计算";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(22, 58);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "Grid 3:";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(22, 31);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "Grid 2:";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(22, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Grid 1:";
            // 
            // ucSpatialIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "ucSpatialIndex";
            this.Size = new System.Drawing.Size(394, 263);
            this.Load += new System.EventHandler(this.ucSpatialIndex_Load);
            this.panelEx1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.grpDataSource.ResumeLayout(false);
            this.grpDataSource.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpDataSource;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX btnCalculate;
        private System.Windows.Forms.TextBox txtGridSize3;
        private System.Windows.Forms.TextBox txtGridSize1;
        private System.Windows.Forms.TextBox txtGridSize2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ColorPickerButton colorNoData;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private DevComponents.DotNetBar.Controls.Slider sliderTransparency;
    }
}
