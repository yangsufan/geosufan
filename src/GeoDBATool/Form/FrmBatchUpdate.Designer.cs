namespace GeoDBATool
{
    partial class FrmBatchUpdate
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
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.textBoxUptData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.comboBoxUptDataType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.btnUptData = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.textBoxUptRange = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.comboBoxUptRangeType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.buttonUptRange = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(137, 186);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 28);
            this.btnOK.TabIndex = 43;
            this.btnOK.Text = "更  新";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(231, 186);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 44;
            this.btnCancel.Text = "取  消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.labelX8);
            this.groupPanel2.Controls.Add(this.textBoxUptData);
            this.groupPanel2.Controls.Add(this.comboBoxUptDataType);
            this.groupPanel2.Controls.Add(this.labelX14);
            this.groupPanel2.Controls.Add(this.btnUptData);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(12, 96);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(418, 80);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
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
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 48;
            this.groupPanel2.Text = "更新数据选择";
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            this.labelX8.Location = new System.Drawing.Point(9, 33);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(74, 18);
            this.labelX8.TabIndex = 13;
            this.labelX8.Text = " 数 据 库 :";
            // 
            // textBoxUptData
            // 
            // 
            // 
            // 
            this.textBoxUptData.Border.Class = "TextBoxBorder";
            this.textBoxUptData.Location = new System.Drawing.Point(84, 30);
            this.textBoxUptData.Name = "textBoxUptData";
            this.textBoxUptData.Size = new System.Drawing.Size(296, 21);
            this.textBoxUptData.TabIndex = 12;
            // 
            // comboBoxUptDataType
            // 
            this.comboBoxUptDataType.DisplayMember = "Text";
            this.comboBoxUptDataType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxUptDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUptDataType.FormattingEnabled = true;
            this.comboBoxUptDataType.ItemHeight = 15;
            this.comboBoxUptDataType.Location = new System.Drawing.Point(84, 3);
            this.comboBoxUptDataType.Name = "comboBoxUptDataType";
            this.comboBoxUptDataType.Size = new System.Drawing.Size(322, 21);
            this.comboBoxUptDataType.TabIndex = 10;
            this.comboBoxUptDataType.SelectedIndexChanged += new System.EventHandler(this.comboBoxUptDataType_SelectedIndexChanged);
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.Location = new System.Drawing.Point(10, 6);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(74, 18);
            this.labelX14.TabIndex = 11;
            this.labelX14.Text = " 类    型 :";
            // 
            // btnUptData
            // 
            this.btnUptData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUptData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUptData.Location = new System.Drawing.Point(380, 30);
            this.btnUptData.Name = "btnUptData";
            this.btnUptData.Size = new System.Drawing.Size(26, 21);
            this.btnUptData.TabIndex = 0;
            this.btnUptData.Text = "...";
            this.btnUptData.Click += new System.EventHandler(this.btnUptData_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.textBoxUptRange);
            this.groupPanel1.Controls.Add(this.comboBoxUptRangeType);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.buttonUptRange);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(12, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(418, 80);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
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
            this.groupPanel1.TabIndex = 48;
            this.groupPanel1.Text = "更新范围选择";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(9, 33);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(62, 18);
            this.labelX1.TabIndex = 13;
            this.labelX1.Text = " 数   据:";
            // 
            // textBoxUptRange
            // 
            // 
            // 
            // 
            this.textBoxUptRange.Border.Class = "TextBoxBorder";
            this.textBoxUptRange.Location = new System.Drawing.Point(84, 30);
            this.textBoxUptRange.Name = "textBoxUptRange";
            this.textBoxUptRange.Size = new System.Drawing.Size(296, 21);
            this.textBoxUptRange.TabIndex = 12;
            // 
            // comboBoxUptRangeType
            // 
            this.comboBoxUptRangeType.DisplayMember = "Text";
            this.comboBoxUptRangeType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxUptRangeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUptRangeType.FormattingEnabled = true;
            this.comboBoxUptRangeType.ItemHeight = 15;
            this.comboBoxUptRangeType.Location = new System.Drawing.Point(84, 3);
            this.comboBoxUptRangeType.Name = "comboBoxUptRangeType";
            this.comboBoxUptRangeType.Size = new System.Drawing.Size(322, 21);
            this.comboBoxUptRangeType.TabIndex = 10;
            this.comboBoxUptRangeType.SelectedIndexChanged += new System.EventHandler(this.comboBoxUptRangeType_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(10, 6);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 18);
            this.labelX2.TabIndex = 11;
            this.labelX2.Text = " 类    型 :";
            // 
            // buttonUptRange
            // 
            this.buttonUptRange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonUptRange.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonUptRange.Location = new System.Drawing.Point(380, 30);
            this.buttonUptRange.Name = "buttonUptRange";
            this.buttonUptRange.Size = new System.Drawing.Size(26, 21);
            this.buttonUptRange.TabIndex = 0;
            this.buttonUptRange.Text = "...";
            this.buttonUptRange.Click += new System.EventHandler(this.buttonUptRange_Click);
            // 
            // FrmBatchUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 225);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBatchUpdate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "范围更新";
            this.Load += new System.EventHandler(this.FrmBatchUpdate_Load);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxUptData;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxUptDataType;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.ButtonX btnUptData;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxUptRange;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxUptRangeType;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX buttonUptRange;
    }
}