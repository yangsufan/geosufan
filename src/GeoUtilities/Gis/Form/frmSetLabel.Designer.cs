namespace GeoUtilities
{
    partial class frmSetLabel
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnConcel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtMaxLabelScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rdbPerShape = new System.Windows.Forms.RadioButton();
            this.rdbPerPart = new System.Windows.Forms.RadioButton();
            this.rdbPerName = new System.Windows.Forms.RadioButton();
            this.txtMinLabelScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.FontBar = new DevComponents.DotNetBar.Bar();
            this.btnBold = new DevComponents.DotNetBar.ButtonItem();
            this.btnItalic = new DevComponents.DotNetBar.ButtonItem();
            this.btnUnderLine = new DevComponents.DotNetBar.ButtonItem();
            this.CmbFontName = new DevComponents.DotNetBar.ComboBoxItem();
            this.CmbFontSize = new DevComponents.DotNetBar.ComboBoxItem();
            this.FontColorPicker = new DevComponents.DotNetBar.ColorPickerDropDown();
            this.LabelText = new DevComponents.DotNetBar.LabelX();
            this.CmbFields = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.chkIsLabel = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel1.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontBar)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(325, 244);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnConcel
            // 
            this.btnConcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConcel.Location = new System.Drawing.Point(396, 244);
            this.btnConcel.Name = "btnConcel";
            this.btnConcel.Size = new System.Drawing.Size(54, 23);
            this.btnConcel.TabIndex = 3;
            this.btnConcel.Text = "取消";
            this.btnConcel.Click += new System.EventHandler(this.btnConcel_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtMaxLabelScale);
            this.groupPanel1.Controls.Add(this.groupPanel3);
            this.groupPanel1.Controls.Add(this.txtMinLabelScale);
            this.groupPanel1.Controls.Add(this.labelX8);
            this.groupPanel1.Controls.Add(this.labelX9);
            this.groupPanel1.Controls.Add(this.groupPanel2);
            this.groupPanel1.Controls.Add(this.CmbFields);
            this.groupPanel1.Controls.Add(this.labelX7);
            this.groupPanel1.Controls.Add(this.chkIsLabel);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(5, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(455, 236);
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
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 45;
            this.groupPanel1.Text = "图层标注";
            // 
            // txtMaxLabelScale
            // 
            this.txtMaxLabelScale.AcceptsTab = true;
            // 
            // 
            // 
            this.txtMaxLabelScale.Border.Class = "TextBoxBorder";
            this.txtMaxLabelScale.Location = new System.Drawing.Point(113, 70);
            this.txtMaxLabelScale.Name = "txtMaxLabelScale";
            this.txtMaxLabelScale.Size = new System.Drawing.Size(166, 21);
            this.txtMaxLabelScale.TabIndex = 50;
            this.txtMaxLabelScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxLabelScale_KeyPress);
            // 
            // groupPanel3
            // 
            this.groupPanel3.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.rdbPerShape);
            this.groupPanel3.Controls.Add(this.rdbPerPart);
            this.groupPanel3.Controls.Add(this.rdbPerName);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(290, 5);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(158, 205);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 45;
            this.groupPanel3.Text = "标注位置";
            // 
            // rdbPerShape
            // 
            this.rdbPerShape.AutoSize = true;
            this.rdbPerShape.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbPerShape.Location = new System.Drawing.Point(20, 25);
            this.rdbPerShape.Name = "rdbPerShape";
            this.rdbPerShape.Size = new System.Drawing.Size(119, 16);
            this.rdbPerShape.TabIndex = 2;
            this.rdbPerShape.Text = "每个要素标注一次";
            this.rdbPerShape.UseVisualStyleBackColor = true;
            // 
            // rdbPerPart
            // 
            this.rdbPerPart.AutoSize = true;
            this.rdbPerPart.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbPerPart.Location = new System.Drawing.Point(20, 46);
            this.rdbPerPart.Name = "rdbPerPart";
            this.rdbPerPart.Size = new System.Drawing.Size(119, 16);
            this.rdbPerPart.TabIndex = 1;
            this.rdbPerPart.Text = "每个部分标注一次";
            this.rdbPerPart.UseVisualStyleBackColor = true;
            // 
            // rdbPerName
            // 
            this.rdbPerName.AutoSize = true;
            this.rdbPerName.Checked = true;
            this.rdbPerName.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbPerName.Location = new System.Drawing.Point(20, 4);
            this.rdbPerName.Name = "rdbPerName";
            this.rdbPerName.Size = new System.Drawing.Size(119, 16);
            this.rdbPerName.TabIndex = 0;
            this.rdbPerName.TabStop = true;
            this.rdbPerName.Text = "每个名称标注一次";
            this.rdbPerName.UseVisualStyleBackColor = true;
            // 
            // txtMinLabelScale
            // 
            // 
            // 
            // 
            this.txtMinLabelScale.Border.Class = "TextBoxBorder";
            this.txtMinLabelScale.Location = new System.Drawing.Point(113, 39);
            this.txtMinLabelScale.Name = "txtMinLabelScale";
            this.txtMinLabelScale.Size = new System.Drawing.Size(166, 21);
            this.txtMinLabelScale.TabIndex = 49;
            this.txtMinLabelScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMinLabelScale_KeyPress);
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            this.labelX8.Location = new System.Drawing.Point(12, 68);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(79, 21);
            this.labelX8.TabIndex = 48;
            this.labelX8.Text = "最大比例尺：";
            // 
            // labelX9
            // 
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            this.labelX9.Location = new System.Drawing.Point(12, 41);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(79, 21);
            this.labelX9.TabIndex = 47;
            this.labelX9.Text = "最小比例尺：";
            // 
            // groupPanel2
            // 
            this.groupPanel2.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.FontBar);
            this.groupPanel2.Controls.Add(this.LabelText);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(8, 99);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(274, 111);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 45;
            this.groupPanel2.Text = "标注字体";
            // 
            // FontBar
            // 
            this.FontBar.AccessibleDescription = "FontBar (FontBar)";
            this.FontBar.AccessibleName = "FontBar";
            this.FontBar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.FontBar.BackColor = System.Drawing.Color.Transparent;
            this.FontBar.DockedBorderStyle = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.FontBar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnBold,
            this.btnItalic,
            this.btnUnderLine,
            this.CmbFontName,
            this.CmbFontSize,
            this.FontColorPicker});
            this.FontBar.Location = new System.Drawing.Point(6, 47);
            this.FontBar.Name = "FontBar";
            this.FontBar.Size = new System.Drawing.Size(256, 34);
            this.FontBar.Stretch = true;
            this.FontBar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.FontBar.TabIndex = 4;
            this.FontBar.TabStop = false;
            this.FontBar.Text = "FontBar";
            // 
            // btnBold
            // 
            this.btnBold.FontBold = true;
            this.btnBold.Name = "btnBold";
            this.btnBold.Text = "B";
            this.btnBold.Tooltip = "是否粗体";
            this.btnBold.Click += new System.EventHandler(this.btnBold_Click);
            // 
            // btnItalic
            // 
            this.btnItalic.FontItalic = true;
            this.btnItalic.Name = "btnItalic";
            this.btnItalic.PersonalizedMenus = DevComponents.DotNetBar.ePersonalizedMenus.DisplayOnHover;
            this.btnItalic.Text = "I";
            this.btnItalic.Tooltip = "是否倾斜";
            this.btnItalic.Click += new System.EventHandler(this.btnItalic_Click);
            // 
            // btnUnderLine
            // 
            this.btnUnderLine.FontUnderline = true;
            this.btnUnderLine.Name = "btnUnderLine";
            this.btnUnderLine.Text = "U";
            this.btnUnderLine.Tooltip = "是否下划线";
            this.btnUnderLine.Click += new System.EventHandler(this.btnUnderLine_Click);
            // 
            // CmbFontName
            // 
            this.CmbFontName.DropDownHeight = 106;
            this.CmbFontName.DropDownWidth = 200;
            this.CmbFontName.ItemHeight = 17;
            this.CmbFontName.Name = "CmbFontName";
            this.CmbFontName.Tooltip = "字体名称";
            this.CmbFontName.SelectedIndexChanged += new System.EventHandler(this.CmbFontName_SelectedIndexChanged);
            // 
            // CmbFontSize
            // 
            this.CmbFontSize.DropDownHeight = 106;
            this.CmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.CmbFontSize.DropDownWidth = 120;
            this.CmbFontSize.ItemHeight = 17;
            this.CmbFontSize.Name = "CmbFontSize";
            this.CmbFontSize.Tooltip = "字体大小";
            this.CmbFontSize.SelectedIndexChanged += new System.EventHandler(this.CmbFontSize_SelectedIndexChanged);
            this.CmbFontSize.ComboBoxTextChanged += new System.EventHandler(this.CmbFontSize_ComboBoxTextChanged);
            // 
            // FontColorPicker
            // 
            this.FontColorPicker.Name = "FontColorPicker";
            this.FontColorPicker.Text = "颜色";
            this.FontColorPicker.Tooltip = "字体颜色";
            this.FontColorPicker.SelectedColorChanged += new System.EventHandler(this.FontColorPicker_SelectedColorChanged);
            // 
            // LabelText
            // 
            this.LabelText.BackColor = System.Drawing.Color.Transparent;
            this.LabelText.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabelText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.LabelText.Location = new System.Drawing.Point(52, 0);
            this.LabelText.Name = "LabelText";
            this.LabelText.Size = new System.Drawing.Size(159, 48);
            this.LabelText.TabIndex = 0;
            this.LabelText.Text = "      标注示例";
            // 
            // CmbFields
            // 
            this.CmbFields.DisplayMember = "Text";
            this.CmbFields.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CmbFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFields.FormattingEnabled = true;
            this.CmbFields.ItemHeight = 15;
            this.CmbFields.Location = new System.Drawing.Point(153, 8);
            this.CmbFields.Name = "CmbFields";
            this.CmbFields.Size = new System.Drawing.Size(126, 21);
            this.CmbFields.TabIndex = 1;
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            this.labelX7.Location = new System.Drawing.Point(119, 6);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(46, 23);
            this.labelX7.TabIndex = 0;
            this.labelX7.Text = "字段:";
            // 
            // chkIsLabel
            // 
            this.chkIsLabel.BackColor = System.Drawing.Color.Transparent;
            this.chkIsLabel.Location = new System.Drawing.Point(5, 7);
            this.chkIsLabel.Name = "chkIsLabel";
            this.chkIsLabel.Size = new System.Drawing.Size(100, 24);
            this.chkIsLabel.TabIndex = 46;
            this.chkIsLabel.Text = "是否标注";
            // 
            // frmSetLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(462, 275);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnConcel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetLabel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置标注";
            this.Load += new System.EventHandler(this.frmSetLabel_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FontBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnConcel;
        //private DevComponents.DotNetBar.ButtonItem btnBold;
        //private DevComponents.DotNetBar.ButtonItem btnItalic;
        //private DevComponents.DotNetBar.ComboBoxItem CmbFontName;
        //private DevComponents.DotNetBar.ComboBoxItem CmbFontSize;
        //private DevComponents.DotNetBar.ButtonItem btnUnderLine;
        //private DevComponents.DotNetBar.ColorPickerDropDown FontColorPicker;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMaxLabelScale;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private System.Windows.Forms.RadioButton rdbPerShape;
        private System.Windows.Forms.RadioButton rdbPerPart;
        private System.Windows.Forms.RadioButton rdbPerName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMinLabelScale;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Bar FontBar;
        private DevComponents.DotNetBar.ButtonItem btnBold;
        private DevComponents.DotNetBar.ButtonItem btnItalic;
        private DevComponents.DotNetBar.ButtonItem btnUnderLine;
        private DevComponents.DotNetBar.ComboBoxItem CmbFontName;
        private DevComponents.DotNetBar.ComboBoxItem CmbFontSize;
        private DevComponents.DotNetBar.ColorPickerDropDown FontColorPicker;
        private DevComponents.DotNetBar.LabelX LabelText;
        private DevComponents.DotNetBar.Controls.ComboBoxEx CmbFields;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkIsLabel;
    }
}