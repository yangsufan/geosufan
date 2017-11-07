namespace GeoDataCenterFunLib
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.CmbFields = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.FontBar = new DevComponents.DotNetBar.Bar();
            this.btnBold = new DevComponents.DotNetBar.ButtonItem();
            this.btnItalic = new DevComponents.DotNetBar.ButtonItem();
            this.btnUnderLine = new DevComponents.DotNetBar.ButtonItem();
            this.CmbFontName = new DevComponents.DotNetBar.ComboBoxItem();
            this.CmbFontSize = new DevComponents.DotNetBar.ComboBoxItem();
            this.FontColorPicker = new DevComponents.DotNetBar.ColorPickerDropDown();
            this.LabelText = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnConcel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontBar)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.CmbFields);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(12, 3);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(295, 87);
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
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "标注字段";
            // 
            // CmbFields
            // 
            this.CmbFields.DisplayMember = "Text";
            this.CmbFields.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CmbFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFields.FormattingEnabled = true;
            this.CmbFields.ItemHeight = 15;
            this.CmbFields.Location = new System.Drawing.Point(75, 20);
            this.CmbFields.Name = "CmbFields";
            this.CmbFields.Size = new System.Drawing.Size(204, 21);
            this.CmbFields.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(13, 20);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(56, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "字段名称";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.FontBar);
            this.groupPanel2.Controls.Add(this.LabelText);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(12, 109);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(295, 111);
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
            this.groupPanel2.TabIndex = 1;
            this.groupPanel2.Text = "标注风格";
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
            this.FontBar.Location = new System.Drawing.Point(13, 47);
            this.FontBar.Name = "FontBar";
            this.FontBar.Size = new System.Drawing.Size(266, 32);
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
            this.CmbFontName.Name = "CmbFontName";
            this.CmbFontName.Tooltip = "字体名称";
            this.CmbFontName.SelectedIndexChanged += new System.EventHandler(this.CmbFontName_SelectedIndexChanged);
            // 
            // CmbFontSize
            // 
            this.CmbFontSize.DropDownHeight = 106;
            this.CmbFontSize.DropDownWidth = 120;
            this.CmbFontSize.Name = "CmbFontSize";
            this.CmbFontSize.Tooltip = "字体大小";
            this.CmbFontSize.SelectedIndexChanged += new System.EventHandler(this.CmbFontSize_SelectedIndexChanged);
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
            this.LabelText.Location = new System.Drawing.Point(12, 3);
            this.LabelText.Name = "LabelText";
            this.LabelText.Size = new System.Drawing.Size(264, 48);
            this.LabelText.TabIndex = 0;
            this.LabelText.Text = "吉奥信息技术有限公司";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(182, 226);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnConcel
            // 
            this.btnConcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConcel.Location = new System.Drawing.Point(253, 226);
            this.btnConcel.Name = "btnConcel";
            this.btnConcel.Size = new System.Drawing.Size(54, 23);
            this.btnConcel.TabIndex = 3;
            this.btnConcel.Text = "取消";
            this.btnConcel.Click += new System.EventHandler(this.btnConcel_Click);
            // 
            // frmSetLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(319, 257);
            this.Controls.Add(this.btnConcel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetLabel";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "设置标注";
            this.Load += new System.EventHandler(this.frmSetLabel_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FontBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx CmbFields;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.LabelX LabelText;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnConcel;
        private DevComponents.DotNetBar.Bar FontBar;
        private DevComponents.DotNetBar.ButtonItem btnBold;
        private DevComponents.DotNetBar.ButtonItem btnItalic;
        private DevComponents.DotNetBar.ComboBoxItem CmbFontName;
        private DevComponents.DotNetBar.ComboBoxItem CmbFontSize;
        private DevComponents.DotNetBar.ButtonItem btnUnderLine;
        private DevComponents.DotNetBar.ColorPickerDropDown FontColorPicker;
    }
}