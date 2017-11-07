namespace GeoDataEdit
{
    partial class frmShp2Dwg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShp2Dwg));
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtSource = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtTarget = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmdSource = new DevComponents.DotNetBar.ButtonX();
            this.cmbTarget = new DevComponents.DotNetBar.ButtonX();
            this.cmdOK = new DevComponents.DotNetBar.ButtonX();
            this.cmdCancel = new DevComponents.DotNetBar.ButtonX();
            this.lstLyrFile = new System.Windows.Forms.ListView();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoS2D = new System.Windows.Forms.RadioButton();
            this.rdoM2D = new System.Windows.Forms.RadioButton();
            this.rdoD2S = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 64);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(56, 21);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "源数据：";
            // 
            // txtSource
            // 
            // 
            // 
            // 
            this.txtSource.Border.Class = "TextBoxBorder";
            this.txtSource.Location = new System.Drawing.Point(74, 64);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(328, 21);
            this.txtSource.TabIndex = 1;
            // 
            // txtTarget
            // 
            // 
            // 
            // 
            this.txtTarget.Border.Class = "TextBoxBorder";
            this.txtTarget.Location = new System.Drawing.Point(74, 249);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(328, 21);
            this.txtTarget.TabIndex = 5;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(12, 249);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(70, 21);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "目标数据：";
            // 
            // cmdSource
            // 
            this.cmdSource.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdSource.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdSource.Location = new System.Drawing.Point(408, 64);
            this.cmdSource.Name = "cmdSource";
            this.cmdSource.Size = new System.Drawing.Size(32, 21);
            this.cmdSource.TabIndex = 6;
            this.cmdSource.Text = "...";
            this.cmdSource.Click += new System.EventHandler(this.cmdSource_Click);
            // 
            // cmbTarget
            // 
            this.cmbTarget.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmbTarget.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmbTarget.Location = new System.Drawing.Point(408, 249);
            this.cmbTarget.Name = "cmbTarget";
            this.cmbTarget.Size = new System.Drawing.Size(32, 21);
            this.cmbTarget.TabIndex = 7;
            this.cmbTarget.Text = "...";
            this.cmbTarget.Click += new System.EventHandler(this.cmbTarget_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdOK.Location = new System.Drawing.Point(282, 279);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(76, 27);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "确定";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmdCancel.Location = new System.Drawing.Point(364, 279);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(76, 27);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "取消";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lstLyrFile
            // 
            this.lstLyrFile.CheckBoxes = true;
            this.lstLyrFile.Location = new System.Drawing.Point(12, 91);
            this.lstLyrFile.Name = "lstLyrFile";
            this.lstLyrFile.Size = new System.Drawing.Size(390, 152);
            this.lstLyrFile.TabIndex = 11;
            this.lstLyrFile.UseCompatibleStateImageBehavior = false;
            this.lstLyrFile.View = System.Windows.Forms.View.Details;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(12, 81);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 16;
            // 
            // rdoS2D
            // 
            this.rdoS2D.AutoSize = true;
            this.rdoS2D.Checked = true;
            this.rdoS2D.Location = new System.Drawing.Point(17, 20);
            this.rdoS2D.Name = "rdoS2D";
            this.rdoS2D.Size = new System.Drawing.Size(71, 16);
            this.rdoS2D.TabIndex = 17;
            this.rdoS2D.TabStop = true;
            this.rdoS2D.Text = "shp→dwg";
            this.rdoS2D.UseVisualStyleBackColor = true;
            this.rdoS2D.CheckedChanged += new System.EventHandler(this.rdoS2D_CheckedChanged);
            // 
            // rdoM2D
            // 
            this.rdoM2D.AutoSize = true;
            this.rdoM2D.Location = new System.Drawing.Point(165, 20);
            this.rdoM2D.Name = "rdoM2D";
            this.rdoM2D.Size = new System.Drawing.Size(71, 16);
            this.rdoM2D.TabIndex = 18;
            this.rdoM2D.Text = "mdb→dwg";
            this.rdoM2D.UseVisualStyleBackColor = true;
            this.rdoM2D.CheckedChanged += new System.EventHandler(this.rdoM2D_CheckedChanged);
            // 
            // rdoD2S
            // 
            this.rdoD2S.AutoSize = true;
            this.rdoD2S.Location = new System.Drawing.Point(313, 20);
            this.rdoD2S.Name = "rdoD2S";
            this.rdoD2S.Size = new System.Drawing.Size(71, 16);
            this.rdoD2S.TabIndex = 19;
            this.rdoD2S.Text = "dwg→shp";
            this.rdoD2S.UseVisualStyleBackColor = true;
            this.rdoD2S.CheckedChanged += new System.EventHandler(this.rdoD2S_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoM2D);
            this.groupBox1.Controls.Add(this.rdoD2S);
            this.groupBox1.Controls.Add(this.rdoS2D);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 46);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "转换类型";
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(408, 123);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(31, 21);
            this.btnSelAll.TabIndex = 21;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSelReverse
            // 
            this.btnSelReverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelReverse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelReverse.Location = new System.Drawing.Point(408, 158);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(31, 21);
            this.btnSelReverse.TabIndex = 22;
            this.btnSelReverse.Text = "反选";
            this.btnSelReverse.Click += new System.EventHandler(this.btnSelReverse_Click);
            // 
            // frmShp2Dwg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 318);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnSelReverse);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.lstLyrFile);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmbTarget);
            this.Controls.Add(this.cmdSource);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShp2Dwg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据转换";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSource;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTarget;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX cmdSource;
        private DevComponents.DotNetBar.ButtonX cmbTarget;
        private DevComponents.DotNetBar.ButtonX cmdOK;
        private DevComponents.DotNetBar.ButtonX cmdCancel;
        private System.Windows.Forms.ListView lstLyrFile;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoS2D;
        private System.Windows.Forms.RadioButton rdoM2D;
        private System.Windows.Forms.RadioButton rdoD2S;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;
    }
}