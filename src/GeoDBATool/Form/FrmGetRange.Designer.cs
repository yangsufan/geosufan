namespace GeoDBATool
{
    partial class FrmGetRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetRange));
            this.RadioBtnSelectRange = new System.Windows.Forms.RadioButton();
            this.RadioBtnInputRange = new System.Windows.Forms.RadioButton();
            this.RadioBtnDrawRange = new System.Windows.Forms.RadioButton();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.btnAddData = new DevComponents.DotNetBar.ButtonX();
            this.btnSelectRange = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.FeaturecomboBox = new System.Windows.Forms.ComboBox();
            this.DataLoad = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // RadioBtnSelectRange
            // 
            this.RadioBtnSelectRange.AutoSize = true;
            this.RadioBtnSelectRange.ForeColor = System.Drawing.SystemColors.Desktop;
            this.RadioBtnSelectRange.Location = new System.Drawing.Point(5, 63);
            this.RadioBtnSelectRange.Name = "RadioBtnSelectRange";
            this.RadioBtnSelectRange.Size = new System.Drawing.Size(119, 16);
            this.RadioBtnSelectRange.TabIndex = 36;
            this.RadioBtnSelectRange.TabStop = true;
            this.RadioBtnSelectRange.Text = "从界面上选择范围";
            this.RadioBtnSelectRange.UseVisualStyleBackColor = true;
            this.RadioBtnSelectRange.Click += new System.EventHandler(this.RadioBtnSelectRange_Click);
            this.RadioBtnSelectRange.CheckedChanged += new System.EventHandler(this.RadioBtnSelectRange_CheckedChanged);
            // 
            // RadioBtnInputRange
            // 
            this.RadioBtnInputRange.AutoSize = true;
            this.RadioBtnInputRange.ForeColor = System.Drawing.SystemColors.Desktop;
            this.RadioBtnInputRange.Location = new System.Drawing.Point(3, 32);
            this.RadioBtnInputRange.Name = "RadioBtnInputRange";
            this.RadioBtnInputRange.Size = new System.Drawing.Size(185, 16);
            this.RadioBtnInputRange.TabIndex = 33;
            this.RadioBtnInputRange.TabStop = true;
            this.RadioBtnInputRange.Text = "从外部TXT文件中获取导入范围";
            this.RadioBtnInputRange.UseVisualStyleBackColor = true;
            this.RadioBtnInputRange.Click += new System.EventHandler(this.RadioBtnInputRange_Click);
            this.RadioBtnInputRange.CheckedChanged += new System.EventHandler(this.RadioBtnInputRange_CheckedChanged);
            // 
            // RadioBtnDrawRange
            // 
            this.RadioBtnDrawRange.AutoSize = true;
            this.RadioBtnDrawRange.ForeColor = System.Drawing.SystemColors.Desktop;
            this.RadioBtnDrawRange.Location = new System.Drawing.Point(3, 3);
            this.RadioBtnDrawRange.Name = "RadioBtnDrawRange";
            this.RadioBtnDrawRange.Size = new System.Drawing.Size(179, 16);
            this.RadioBtnDrawRange.TabIndex = 32;
            this.RadioBtnDrawRange.TabStop = true;
            this.RadioBtnDrawRange.Text = "在图形控件上依照底图画范围";
            this.RadioBtnDrawRange.UseVisualStyleBackColor = true;
            this.RadioBtnDrawRange.Click += new System.EventHandler(this.RadioBtnDrawRange_Click);
            this.RadioBtnDrawRange.CheckedChanged += new System.EventHandler(this.RadioBtnDrawRange_CheckedChanged);
            // 
            // groupPanel3
            // 
            this.groupPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.RadioBtnDrawRange);
            this.groupPanel3.Controls.Add(this.RadioBtnInputRange);
            this.groupPanel3.Controls.Add(this.RadioBtnSelectRange);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(287, -8);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(191, 0);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
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
            this.groupPanel3.TabIndex = 40;
            this.groupPanel3.Text = "范围获取方式";
            this.groupPanel3.Visible = false;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.axMapControl1.Location = new System.Drawing.Point(14, 79);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(334, 292);
            this.axMapControl1.TabIndex = 37;
            this.axMapControl1.OnAfterDraw += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnAfterDrawEventHandler(this.axMapControl1_OnAfterDraw);
            // 
            // btnAddData
            // 
            this.btnAddData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddData.Location = new System.Drawing.Point(12, 15);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(92, 23);
            this.btnAddData.TabIndex = 41;
            this.btnAddData.Text = "打开范围数据";
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // btnSelectRange
            // 
            this.btnSelectRange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectRange.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectRange.Location = new System.Drawing.Point(124, 15);
            this.btnSelectRange.Name = "btnSelectRange";
            this.btnSelectRange.Size = new System.Drawing.Size(66, 23);
            this.btnSelectRange.TabIndex = 42;
            this.btnSelectRange.Text = "选择范围";
            this.btnSelectRange.Click += new System.EventHandler(this.btnSelectRange_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(219, 52);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 23);
            this.btnOK.TabIndex = 43;
            this.btnOK.Text = "确  定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(287, 52);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(61, 23);
            this.btnCancle.TabIndex = 44;
            this.btnCancle.Text = "取  消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // FeaturecomboBox
            // 
            this.FeaturecomboBox.Enabled = false;
            this.FeaturecomboBox.FormattingEnabled = true;
            this.FeaturecomboBox.Location = new System.Drawing.Point(15, 53);
            this.FeaturecomboBox.Name = "FeaturecomboBox";
            this.FeaturecomboBox.Size = new System.Drawing.Size(107, 20);
            this.FeaturecomboBox.TabIndex = 45;
            // 
            // DataLoad
            // 
            this.DataLoad.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DataLoad.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DataLoad.Enabled = false;
            this.DataLoad.Location = new System.Drawing.Point(126, 52);
            this.DataLoad.Name = "DataLoad";
            this.DataLoad.Size = new System.Drawing.Size(66, 23);
            this.DataLoad.TabIndex = 42;
            this.DataLoad.Text = "加载";
            this.DataLoad.Click += new System.EventHandler(this.Load_Click);
            // 
            // FrmGetRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 388);
            this.Controls.Add(this.FeaturecomboBox);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.DataLoad);
            this.Controls.Add(this.btnSelectRange);
            this.Controls.Add(this.btnAddData);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.axMapControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetRange";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "获取图幅数据范围";
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected internal System.Windows.Forms.RadioButton RadioBtnSelectRange;
        private System.Windows.Forms.RadioButton RadioBtnInputRange;
        private System.Windows.Forms.RadioButton RadioBtnDrawRange;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private DevComponents.DotNetBar.ButtonX btnAddData;
        private DevComponents.DotNetBar.ButtonX btnSelectRange;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private System.Windows.Forms.ComboBox FeaturecomboBox;
        private DevComponents.DotNetBar.ButtonX DataLoad;
    }
}