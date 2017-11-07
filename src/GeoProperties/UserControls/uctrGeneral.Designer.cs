namespace GeoProperties.UserControls
{
    partial class uctrGeneral
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
            this.panelGeneral = new DevComponents.DotNetBar.PanelEx();
            this.grpScaleRange = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblMaxScale = new DevComponents.DotNetBar.LabelX();
            this.cboMaxScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblInBeyond = new DevComponents.DotNetBar.LabelX();
            this.lblMinScale = new DevComponents.DotNetBar.LabelX();
            this.cboMinScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.lblOutBeyond = new DevComponents.DotNetBar.LabelX();
            this.rbtnRangeScale = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.rbtnAllScale = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lblScale = new DevComponents.DotNetBar.LabelX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.trackBar = new DevComponents.DotNetBar.Controls.Slider();
            this.inPutTrans = new System.Windows.Forms.NumericUpDown();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtDescription = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblDescription = new DevComponents.DotNetBar.LabelX();
            this.chkLayerVisible = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.txtLayerName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblLayerName = new DevComponents.DotNetBar.LabelX();
            this.panelGeneral.SuspendLayout();
            this.grpScaleRange.SuspendLayout();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inPutTrans)).BeginInit();
            this.SuspendLayout();
            // 
            // panelGeneral
            // 
            this.panelGeneral.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelGeneral.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelGeneral.Controls.Add(this.grpScaleRange);
            this.panelGeneral.Controls.Add(this.panelEx2);
            this.panelGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGeneral.Location = new System.Drawing.Point(0, 0);
            this.panelGeneral.Name = "panelGeneral";
            this.panelGeneral.Size = new System.Drawing.Size(429, 310);
            this.panelGeneral.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelGeneral.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelGeneral.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelGeneral.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelGeneral.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelGeneral.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelGeneral.Style.GradientAngle = 90;
            this.panelGeneral.TabIndex = 2;
            // 
            // grpScaleRange
            // 
            this.grpScaleRange.BackColor = System.Drawing.Color.Transparent;
            this.grpScaleRange.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpScaleRange.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpScaleRange.Controls.Add(this.lblMaxScale);
            this.grpScaleRange.Controls.Add(this.cboMaxScale);
            this.grpScaleRange.Controls.Add(this.lblInBeyond);
            this.grpScaleRange.Controls.Add(this.lblMinScale);
            this.grpScaleRange.Controls.Add(this.cboMinScale);
            this.grpScaleRange.Controls.Add(this.lblOutBeyond);
            this.grpScaleRange.Controls.Add(this.rbtnRangeScale);
            this.grpScaleRange.Controls.Add(this.rbtnAllScale);
            this.grpScaleRange.Controls.Add(this.lblScale);
            this.grpScaleRange.DrawTitleBox = false;
            this.grpScaleRange.Location = new System.Drawing.Point(0, 164);
            this.grpScaleRange.Name = "grpScaleRange";
            this.grpScaleRange.Size = new System.Drawing.Size(425, 125);
            // 
            // 
            // 
            this.grpScaleRange.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpScaleRange.Style.BackColorGradientAngle = 90;
            this.grpScaleRange.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpScaleRange.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpScaleRange.Style.BorderBottomWidth = 1;
            this.grpScaleRange.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpScaleRange.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpScaleRange.Style.BorderLeftWidth = 1;
            this.grpScaleRange.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpScaleRange.Style.BorderRightWidth = 1;
            this.grpScaleRange.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpScaleRange.Style.BorderTopWidth = 1;
            this.grpScaleRange.Style.CornerDiameter = 4;
            this.grpScaleRange.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpScaleRange.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpScaleRange.TabIndex = 9;
            this.grpScaleRange.Text = "比例尺范围";
            // 
            // lblMaxScale
            // 
            this.lblMaxScale.Location = new System.Drawing.Point(212, 78);
            this.lblMaxScale.Name = "lblMaxScale";
            this.lblMaxScale.Size = new System.Drawing.Size(87, 21);
            this.lblMaxScale.TabIndex = 8;
            this.lblMaxScale.Text = "(最大比例尺)";
            // 
            // cboMaxScale
            // 
            this.cboMaxScale.DisplayMember = "Text";
            this.cboMaxScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboMaxScale.FormattingEnabled = true;
            this.cboMaxScale.ItemHeight = 15;
            this.cboMaxScale.Location = new System.Drawing.Point(61, 76);
            this.cboMaxScale.Name = "cboMaxScale";
            this.cboMaxScale.Size = new System.Drawing.Size(145, 21);
            this.cboMaxScale.TabIndex = 7;
            this.cboMaxScale.SelectedIndexChanged += new System.EventHandler(this.cboMaxScale_SelectedIndexChanged);
            // 
            // lblInBeyond
            // 
            this.lblInBeyond.Location = new System.Drawing.Point(19, 78);
            this.lblInBeyond.Name = "lblInBeyond";
            this.lblInBeyond.Size = new System.Drawing.Size(42, 20);
            this.lblInBeyond.TabIndex = 6;
            this.lblInBeyond.Text = "下限：";
            // 
            // lblMinScale
            // 
            this.lblMinScale.Location = new System.Drawing.Point(212, 51);
            this.lblMinScale.Name = "lblMinScale";
            this.lblMinScale.Size = new System.Drawing.Size(87, 21);
            this.lblMinScale.TabIndex = 5;
            this.lblMinScale.Text = "(最小比例尺)";
            // 
            // cboMinScale
            // 
            this.cboMinScale.DisplayMember = "Text";
            this.cboMinScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboMinScale.FormattingEnabled = true;
            this.cboMinScale.ItemHeight = 15;
            this.cboMinScale.Location = new System.Drawing.Point(61, 49);
            this.cboMinScale.Name = "cboMinScale";
            this.cboMinScale.Size = new System.Drawing.Size(145, 21);
            this.cboMinScale.TabIndex = 4;
            this.cboMinScale.SelectedIndexChanged += new System.EventHandler(this.cboMinScale_SelectedIndexChanged);
            // 
            // lblOutBeyond
            // 
            this.lblOutBeyond.Location = new System.Drawing.Point(19, 50);
            this.lblOutBeyond.Name = "lblOutBeyond";
            this.lblOutBeyond.Size = new System.Drawing.Size(42, 16);
            this.lblOutBeyond.TabIndex = 3;
            this.lblOutBeyond.Text = "上限：";
            // 
            // rbtnRangeScale
            // 
            this.rbtnRangeScale.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rbtnRangeScale.Location = new System.Drawing.Point(208, 21);
            this.rbtnRangeScale.Name = "rbtnRangeScale";
            this.rbtnRangeScale.Size = new System.Drawing.Size(187, 17);
            this.rbtnRangeScale.TabIndex = 2;
            this.rbtnRangeScale.Text = "比例尺如下范围图层可显示";
            this.rbtnRangeScale.CheckedChanged += new System.EventHandler(this.rbtnRangeScale_CheckedChanged_1);
            // 
            // rbtnAllScale
            // 
            this.rbtnAllScale.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton;
            this.rbtnAllScale.Location = new System.Drawing.Point(15, 22);
            this.rbtnAllScale.Name = "rbtnAllScale";
            this.rbtnAllScale.Size = new System.Drawing.Size(165, 17);
            this.rbtnAllScale.TabIndex = 1;
            this.rbtnAllScale.Text = "所有比例尺图层均可显示";
            // 
            // lblScale
            // 
            this.lblScale.Location = new System.Drawing.Point(13, -1);
            this.lblScale.Name = "lblScale";
            this.lblScale.Size = new System.Drawing.Size(254, 23);
            this.lblScale.TabIndex = 0;
            this.lblScale.Text = "指定比例尺范围，当前图层在范围内可显示：";
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx2.Controls.Add(this.trackBar);
            this.panelEx2.Controls.Add(this.inPutTrans);
            this.panelEx2.Controls.Add(this.labelX1);
            this.panelEx2.Controls.Add(this.txtDescription);
            this.panelEx2.Controls.Add(this.lblDescription);
            this.panelEx2.Controls.Add(this.chkLayerVisible);
            this.panelEx2.Controls.Add(this.txtLayerName);
            this.panelEx2.Controls.Add(this.lblLayerName);
            this.panelEx2.Location = new System.Drawing.Point(3, 3);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(422, 159);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 8;
            this.panelEx2.Text = "panelEx2";
            // 
            // trackBar
            // 
            this.trackBar.LabelPosition = DevComponents.DotNetBar.eSliderLabelPosition.Right;
            this.trackBar.Location = new System.Drawing.Point(159, 124);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(244, 23);
            this.trackBar.TabIndex = 12;
            this.trackBar.Value = 0;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // inPutTrans
            // 
            this.inPutTrans.Location = new System.Drawing.Point(53, 124);
            this.inPutTrans.Name = "inPutTrans";
            this.inPutTrans.Size = new System.Drawing.Size(100, 21);
            this.inPutTrans.TabIndex = 10;
            this.inPutTrans.ValueChanged += new System.EventHandler(this.inPutTrans_ValueChanged);
            this.inPutTrans.KeyUp += new System.Windows.Forms.KeyEventHandler(this.inPutTrans_KeyUp);
            this.inPutTrans.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inPutTrans_KeyPress);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(4, 126);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(59, 19);
            this.labelX1.TabIndex = 9;
            this.labelX1.Text = "透明度：";
            // 
            // txtDescription
            // 
            // 
            // 
            // 
            this.txtDescription.Border.Class = "TextBoxBorder";
            this.txtDescription.Location = new System.Drawing.Point(56, 31);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(347, 78);
            this.txtDescription.TabIndex = 6;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(4, 35);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(55, 23);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "描  述：";
            // 
            // chkLayerVisible
            // 
            this.chkLayerVisible.Location = new System.Drawing.Point(319, 5);
            this.chkLayerVisible.Name = "chkLayerVisible";
            this.chkLayerVisible.Size = new System.Drawing.Size(75, 23);
            this.chkLayerVisible.TabIndex = 2;
            this.chkLayerVisible.Text = "图层可见";
            this.chkLayerVisible.CheckedChanged += new System.EventHandler(this.chkLayerVisible_CheckedChanged);
            // 
            // txtLayerName
            // 
            // 
            // 
            // 
            this.txtLayerName.Border.Class = "TextBoxBorder";
            this.txtLayerName.Location = new System.Drawing.Point(56, 6);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(257, 21);
            this.txtLayerName.TabIndex = 1;
            // 
            // lblLayerName
            // 
            this.lblLayerName.Location = new System.Drawing.Point(4, 7);
            this.lblLayerName.Name = "lblLayerName";
            this.lblLayerName.Size = new System.Drawing.Size(59, 19);
            this.lblLayerName.TabIndex = 0;
            this.lblLayerName.Text = "图层名：";
            // 
            // uctrGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGeneral);
            this.Name = "uctrGeneral";
            this.Size = new System.Drawing.Size(429, 310);
            this.Load += new System.EventHandler(this.uctrGeneral_Load_1);
            this.panelGeneral.ResumeLayout(false);
            this.grpScaleRange.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inPutTrans)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelGeneral;
        private DevComponents.DotNetBar.Controls.GroupPanel grpScaleRange;
        private DevComponents.DotNetBar.LabelX lblMaxScale;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboMaxScale;
        private DevComponents.DotNetBar.LabelX lblInBeyond;
        private DevComponents.DotNetBar.LabelX lblMinScale;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboMinScale;
        private DevComponents.DotNetBar.LabelX lblOutBeyond;
        private DevComponents.DotNetBar.Controls.CheckBoxX rbtnRangeScale;
        private DevComponents.DotNetBar.Controls.CheckBoxX rbtnAllScale;
        private DevComponents.DotNetBar.LabelX lblScale;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDescription;
        private DevComponents.DotNetBar.LabelX lblDescription;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkLayerVisible;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLayerName;
        private DevComponents.DotNetBar.LabelX lblLayerName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.NumericUpDown inPutTrans;
        private DevComponents.DotNetBar.Controls.Slider trackBar;
    }
}
