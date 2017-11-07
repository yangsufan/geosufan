namespace GeoDBATool
{
    partial class FrmTopoCheckSet
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
            this.txtCheckDataPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnScanDataPath = new DevComponents.DotNetBar.ButtonX();
            this.btnCheck = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rdbTempData = new System.Windows.Forms.RadioButton();
            this.rdbGDB = new System.Windows.Forms.RadioButton();
            this.rdbMDB = new System.Windows.Forms.RadioButton();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbLayer = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.progressStep = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCheckDataPath
            // 
            // 
            // 
            // 
            this.txtCheckDataPath.Border.Class = "TextBoxBorder";
            this.txtCheckDataPath.Enabled = false;
            this.txtCheckDataPath.Location = new System.Drawing.Point(59, 56);
            this.txtCheckDataPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckDataPath.Name = "txtCheckDataPath";
            this.txtCheckDataPath.Size = new System.Drawing.Size(195, 21);
            this.txtCheckDataPath.TabIndex = 0;
            this.txtCheckDataPath.Click += new System.EventHandler(this.txtCheckDataPath_Click);
            // 
            // btnScanDataPath
            // 
            this.btnScanDataPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanDataPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanDataPath.Location = new System.Drawing.Point(258, 55);
            this.btnScanDataPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnScanDataPath.Name = "btnScanDataPath";
            this.btnScanDataPath.Size = new System.Drawing.Size(28, 22);
            this.btnScanDataPath.TabIndex = 2;
            this.btnScanDataPath.Text = "...";
            this.btnScanDataPath.Click += new System.EventHandler(this.btnScanDataPath_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCheck.Location = new System.Drawing.Point(170, 119);
            this.btnCheck.Margin = new System.Windows.Forms.Padding(2);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(56, 22);
            this.btnCheck.TabIndex = 4;
            this.btnCheck.Text = "检查";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(231, 119);
            this.btnExit.Margin = new System.Windows.Forms.Padding(2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(56, 22);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.rdbTempData);
            this.groupPanel1.Controls.Add(this.rdbGDB);
            this.groupPanel1.Controls.Add(this.rdbMDB);
            this.groupPanel1.Location = new System.Drawing.Point(11, 11);
            this.groupPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(277, 34);
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
            this.groupPanel1.TabIndex = 6;
            // 
            // rdbTempData
            // 
            this.rdbTempData.AutoSize = true;
            this.rdbTempData.BackColor = System.Drawing.Color.Transparent;
            this.rdbTempData.Location = new System.Drawing.Point(202, 6);
            this.rdbTempData.Margin = new System.Windows.Forms.Padding(2);
            this.rdbTempData.Name = "rdbTempData";
            this.rdbTempData.Size = new System.Drawing.Size(59, 16);
            this.rdbTempData.TabIndex = 3;
            this.rdbTempData.TabStop = true;
            this.rdbTempData.Text = "临时库";
            this.rdbTempData.UseVisualStyleBackColor = false;
            this.rdbTempData.CheckedChanged += new System.EventHandler(this.rdbTempData_CheckedChanged);
            // 
            // rdbGDB
            // 
            this.rdbGDB.AutoSize = true;
            this.rdbGDB.BackColor = System.Drawing.Color.Transparent;
            this.rdbGDB.Location = new System.Drawing.Point(98, 6);
            this.rdbGDB.Margin = new System.Windows.Forms.Padding(2);
            this.rdbGDB.Name = "rdbGDB";
            this.rdbGDB.Size = new System.Drawing.Size(77, 16);
            this.rdbGDB.TabIndex = 2;
            this.rdbGDB.Text = "GDB文件夹";
            this.rdbGDB.UseVisualStyleBackColor = false;
            // 
            // rdbMDB
            // 
            this.rdbMDB.AutoSize = true;
            this.rdbMDB.BackColor = System.Drawing.Color.Transparent;
            this.rdbMDB.Checked = true;
            this.rdbMDB.Location = new System.Drawing.Point(7, 6);
            this.rdbMDB.Margin = new System.Windows.Forms.Padding(2);
            this.rdbMDB.Name = "rdbMDB";
            this.rdbMDB.Size = new System.Drawing.Size(65, 16);
            this.rdbMDB.TabIndex = 0;
            this.rdbMDB.TabStop = true;
            this.rdbMDB.Text = "MDB文件";
            this.rdbMDB.UseVisualStyleBackColor = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(13, 59);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(64, 17);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "数据源：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(13, 90);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(70, 17);
            this.labelX2.TabIndex = 7;
            this.labelX2.Text = "小班层：";
            // 
            // cmbLayer
            // 
            this.cmbLayer.DisplayMember = "Text";
            this.cmbLayer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLayer.FormattingEnabled = true;
            this.cmbLayer.ItemHeight = 15;
            this.cmbLayer.Location = new System.Drawing.Point(59, 89);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Size = new System.Drawing.Size(227, 21);
            this.cmbLayer.TabIndex = 8;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // progressStep
            // 
            this.progressStep.BackColor = System.Drawing.Color.White;
            this.progressStep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressStep.Location = new System.Drawing.Point(1, 146);
            this.progressStep.Name = "progressStep";
            this.progressStep.Size = new System.Drawing.Size(291, 10);
            this.progressStep.TabIndex = 9;
            this.progressStep.Visible = false;
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(1, 124);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(164, 16);
            this.lblTips.TabIndex = 7;
            // 
            // FrmTopoCheckSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 155);
            this.Controls.Add(this.progressStep);
            this.Controls.Add(this.cmbLayer);
            this.Controls.Add(this.txtCheckDataPath);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnScanDataPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTopoCheckSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拓扑检查";
            this.Load += new System.EventHandler(this.FrmTopoCheckSet_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtCheckDataPath;
        private DevComponents.DotNetBar.ButtonX btnScanDataPath;
        private DevComponents.DotNetBar.ButtonX btnCheck;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.RadioButton rdbMDB;
        private System.Windows.Forms.RadioButton rdbGDB;
        private System.Windows.Forms.RadioButton rdbTempData;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbLayer;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressStep;
        private DevComponents.DotNetBar.LabelX lblTips;
    }
}