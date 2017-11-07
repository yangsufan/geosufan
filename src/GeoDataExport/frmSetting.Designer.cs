namespace GeoDataExport
{
    partial class frmSetting
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
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtRotate = new DevComponents.Editors.DoubleInput();
            this.txtCentY = new DevComponents.Editors.DoubleInput();
            this.txtCentX = new DevComponents.Editors.DoubleInput();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtYoff = new DevComponents.Editors.DoubleInput();
            this.txtXoff = new DevComponents.Editors.DoubleInput();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmdOK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRotate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCentY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCentX)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtYoff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXoff)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.txtRotate);
            this.groupPanel2.Controls.Add(this.txtCentY);
            this.groupPanel2.Controls.Add(this.txtCentX);
            this.groupPanel2.Controls.Add(this.labelX5);
            this.groupPanel2.Controls.Add(this.labelX4);
            this.groupPanel2.Controls.Add(this.labelX3);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(7, 92);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(415, 91);
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
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.TabIndex = 1;
            this.groupPanel2.Text = "旋转设置";
            // 
            // txtRotate
            // 
            // 
            // 
            // 
            this.txtRotate.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtRotate.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtRotate.Increment = 1;
            this.txtRotate.Location = new System.Drawing.Point(341, 23);
            this.txtRotate.Name = "txtRotate";
            this.txtRotate.Size = new System.Drawing.Size(60, 21);
            this.txtRotate.TabIndex = 9;
            // 
            // txtCentY
            // 
            // 
            // 
            // 
            this.txtCentY.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtCentY.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtCentY.Increment = 1;
            this.txtCentY.Location = new System.Drawing.Point(212, 23);
            this.txtCentY.Name = "txtCentY";
            this.txtCentY.Size = new System.Drawing.Size(60, 21);
            this.txtCentY.TabIndex = 8;
            // 
            // txtCentX
            // 
            // 
            // 
            // 
            this.txtCentX.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtCentX.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtCentX.Increment = 1;
            this.txtCentX.Location = new System.Drawing.Point(82, 23);
            this.txtCentX.Name = "txtCentX";
            this.txtCentX.Size = new System.Drawing.Size(60, 21);
            this.txtCentX.TabIndex = 7;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(280, 23);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(69, 24);
            this.labelX5.TabIndex = 3;
            this.labelX5.Text = "旋转角度：";
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(143, 23);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 24);
            this.labelX4.TabIndex = 2;
            this.labelX4.Text = "旋转原点Y：";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(14, 23);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(72, 24);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "旋转原点X：";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtYoff);
            this.groupPanel1.Controls.Add(this.txtXoff);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(7, 3);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(415, 91);
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
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.TabIndex = 2;
            this.groupPanel1.Text = "平移设置";
            // 
            // txtYoff
            // 
            // 
            // 
            // 
            this.txtYoff.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtYoff.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtYoff.Increment = 1;
            this.txtYoff.Location = new System.Drawing.Point(249, 19);
            this.txtYoff.Name = "txtYoff";
            this.txtYoff.Size = new System.Drawing.Size(129, 21);
            this.txtYoff.TabIndex = 5;
            // 
            // txtXoff
            // 
            // 
            // 
            // 
            this.txtXoff.BackgroundStyle.Class = "DateTimeInputBackground";
            this.txtXoff.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.txtXoff.Increment = 1;
            this.txtXoff.Location = new System.Drawing.Point(58, 19);
            this.txtXoff.Name = "txtXoff";
            this.txtXoff.Size = new System.Drawing.Size(129, 21);
            this.txtXoff.TabIndex = 4;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(205, 20);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(49, 24);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "Y方向：";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(14, 19);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(51, 24);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "X方向：";
            // 
            // cmdOK
            // 
            this.cmdOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdOK.Location = new System.Drawing.Point(339, 197);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(83, 22);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "确定";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 231);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "坐标转换设置";
            this.Load += new System.EventHandler(this.frmSetting_Load);
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRotate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCentY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCentX)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtYoff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXoff)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX cmdOK;
        private DevComponents.Editors.DoubleInput txtRotate;
        private DevComponents.Editors.DoubleInput txtCentY;
        private DevComponents.Editors.DoubleInput txtCentX;
        private DevComponents.Editors.DoubleInput txtYoff;
        private DevComponents.Editors.DoubleInput txtXoff;
    }
}