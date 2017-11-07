namespace GeoUtilities
{
    partial class FormSetLimitScale
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rdbLimitScale = new System.Windows.Forms.RadioButton();
            this.rdbAnyScale = new System.Windows.Forms.RadioButton();
            this.txtMaxScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtMinScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelMinScale = new DevComponents.DotNetBar.LabelX();
            this.labelMaxScale = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel4
            // 
            this.groupPanel4.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.rdbLimitScale);
            this.groupPanel4.Controls.Add(this.rdbAnyScale);
            this.groupPanel4.Controls.Add(this.txtMaxScale);
            this.groupPanel4.Controls.Add(this.txtMinScale);
            this.groupPanel4.Controls.Add(this.labelMinScale);
            this.groupPanel4.Controls.Add(this.labelMaxScale);
            this.groupPanel4.Location = new System.Drawing.Point(8, 12);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(237, 128);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel4.TabIndex = 62;
            // 
            // rdbLimitScale
            // 
            this.rdbLimitScale.AutoSize = true;
            this.rdbLimitScale.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbLimitScale.Location = new System.Drawing.Point(3, 37);
            this.rdbLimitScale.Name = "rdbLimitScale";
            this.rdbLimitScale.Size = new System.Drawing.Size(155, 16);
            this.rdbLimitScale.TabIndex = 79;
            this.rdbLimitScale.Text = "仅以下比例尺范围内可见";
            this.rdbLimitScale.UseVisualStyleBackColor = true;
            this.rdbLimitScale.CheckedChanged += new System.EventHandler(this.rdbLimitScale_CheckedChanged);
            // 
            // rdbAnyScale
            // 
            this.rdbAnyScale.AutoSize = true;
            this.rdbAnyScale.Checked = true;
            this.rdbAnyScale.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbAnyScale.Location = new System.Drawing.Point(3, 9);
            this.rdbAnyScale.Name = "rdbAnyScale";
            this.rdbAnyScale.Size = new System.Drawing.Size(107, 16);
            this.rdbAnyScale.TabIndex = 78;
            this.rdbAnyScale.TabStop = true;
            this.rdbAnyScale.Text = "任何比例尺可见";
            this.rdbAnyScale.UseVisualStyleBackColor = true;
            // 
            // txtMaxScale
            // 
            this.txtMaxScale.AcceptsTab = true;
            // 
            // 
            // 
            this.txtMaxScale.Border.Class = "TextBoxBorder";
            this.txtMaxScale.Enabled = false;
            this.txtMaxScale.Location = new System.Drawing.Point(96, 94);
            this.txtMaxScale.Name = "txtMaxScale";
            this.txtMaxScale.Size = new System.Drawing.Size(120, 21);
            this.txtMaxScale.TabIndex = 73;
            this.txtMaxScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMaxScale_KeyPress);
            // 
            // txtMinScale
            // 
            // 
            // 
            // 
            this.txtMinScale.Border.Class = "TextBoxBorder";
            this.txtMinScale.Enabled = false;
            this.txtMinScale.Location = new System.Drawing.Point(95, 64);
            this.txtMinScale.Name = "txtMinScale";
            this.txtMinScale.Size = new System.Drawing.Size(120, 21);
            this.txtMinScale.TabIndex = 72;
            this.txtMinScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMinScale_KeyPress);
            // 
            // labelMinScale
            // 
            this.labelMinScale.Enabled = false;
            this.labelMinScale.Location = new System.Drawing.Point(22, 63);
            this.labelMinScale.Name = "labelMinScale";
            this.labelMinScale.Size = new System.Drawing.Size(80, 24);
            this.labelMinScale.TabIndex = 76;
            this.labelMinScale.Text = "最小比例尺：";
            // 
            // labelMaxScale
            // 
            this.labelMaxScale.Enabled = false;
            this.labelMaxScale.Location = new System.Drawing.Point(22, 92);
            this.labelMaxScale.Name = "labelMaxScale";
            this.labelMaxScale.Size = new System.Drawing.Size(80, 24);
            this.labelMaxScale.TabIndex = 77;
            this.labelMaxScale.Text = "最大比例尺：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(173, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 23);
            this.btnCancel.TabIndex = 64;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(87, 148);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 63;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormSetLimitScale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 176);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupPanel4);
            this.DoubleBuffered = true;
            this.Name = "FormSetLimitScale";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置限制比例尺";
            this.Load += new System.EventHandler(this.FormSetLimitScale_Load);
            this.groupPanel4.ResumeLayout(false);
            this.groupPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMaxScale;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMinScale;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.LabelX labelMinScale;
        private DevComponents.DotNetBar.LabelX labelMaxScale;
        private System.Windows.Forms.RadioButton rdbLimitScale;
        private System.Windows.Forms.RadioButton rdbAnyScale;
    }
}