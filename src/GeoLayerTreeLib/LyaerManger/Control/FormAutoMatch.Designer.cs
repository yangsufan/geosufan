namespace GeoLayerTreeLib.LayerManager
{
    partial class FormAutoMatch
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
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanelAutoConfig = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkAutoMatchRender = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkAutoMatchScale = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkAutoMatchFilter = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkAutoMatchLabel = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanelAutoConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(98, 66);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 47;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(24, 66);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 46;
            this.btnOK.Text = "匹配";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupPanelAutoConfig
            // 
            this.groupPanelAutoConfig.BackColor = System.Drawing.Color.Transparent;
            this.groupPanelAutoConfig.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelAutoConfig.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelAutoConfig.Controls.Add(this.chkAutoMatchRender);
            this.groupPanelAutoConfig.Controls.Add(this.chkAutoMatchScale);
            this.groupPanelAutoConfig.Controls.Add(this.chkAutoMatchFilter);
            this.groupPanelAutoConfig.Controls.Add(this.chkAutoMatchLabel);
            this.groupPanelAutoConfig.DrawTitleBox = false;
            this.groupPanelAutoConfig.Location = new System.Drawing.Point(3, 12);
            this.groupPanelAutoConfig.Name = "groupPanelAutoConfig";
            this.groupPanelAutoConfig.Size = new System.Drawing.Size(181, 41);
            // 
            // 
            // 
            this.groupPanelAutoConfig.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelAutoConfig.Style.BackColorGradientAngle = 90;
            this.groupPanelAutoConfig.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelAutoConfig.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderBottomWidth = 1;
            this.groupPanelAutoConfig.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelAutoConfig.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderLeftWidth = 1;
            this.groupPanelAutoConfig.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderRightWidth = 1;
            this.groupPanelAutoConfig.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderTopWidth = 1;
            this.groupPanelAutoConfig.Style.CornerDiameter = 4;
            this.groupPanelAutoConfig.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelAutoConfig.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelAutoConfig.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelAutoConfig.TabIndex = 50;
            // 
            // chkAutoMatchRender
            // 
            this.chkAutoMatchRender.Location = new System.Drawing.Point(5, 9);
            this.chkAutoMatchRender.Name = "chkAutoMatchRender";
            this.chkAutoMatchRender.Size = new System.Drawing.Size(78, 24);
            this.chkAutoMatchRender.TabIndex = 54;
            this.chkAutoMatchRender.Text = "匹配符号";
            // 
            // chkAutoMatchScale
            // 
            this.chkAutoMatchScale.Location = new System.Drawing.Point(83, 9);
            this.chkAutoMatchScale.Name = "chkAutoMatchScale";
            this.chkAutoMatchScale.Size = new System.Drawing.Size(91, 24);
            this.chkAutoMatchScale.TabIndex = 53;
            this.chkAutoMatchScale.Text = "匹配比例尺";
            // 
            // chkAutoMatchFilter
            // 
            this.chkAutoMatchFilter.Location = new System.Drawing.Point(242, 11);
            this.chkAutoMatchFilter.Name = "chkAutoMatchFilter";
            this.chkAutoMatchFilter.Size = new System.Drawing.Size(100, 24);
            this.chkAutoMatchFilter.TabIndex = 49;
            this.chkAutoMatchFilter.Text = "匹配过滤条件";
            this.chkAutoMatchFilter.Visible = false;
            // 
            // chkAutoMatchLabel
            // 
            this.chkAutoMatchLabel.Location = new System.Drawing.Point(172, 10);
            this.chkAutoMatchLabel.Name = "chkAutoMatchLabel";
            this.chkAutoMatchLabel.Size = new System.Drawing.Size(80, 24);
            this.chkAutoMatchLabel.TabIndex = 49;
            this.chkAutoMatchLabel.Text = "匹配标注";
            this.chkAutoMatchLabel.Visible = false;
            // 
            // FormAutoMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 93);
            this.Controls.Add(this.groupPanelAutoConfig);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAutoMatch";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动匹配";
            this.Load += new System.EventHandler(this.FormAutoMatch_Load);
            this.groupPanelAutoConfig.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelAutoConfig;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAutoMatchFilter;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAutoMatchLabel;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAutoMatchRender;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkAutoMatchScale;
    }
}