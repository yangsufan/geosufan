namespace GeoDBATool
{
    partial class FrmLogicCheckSet
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
            this.rdbGDB = new System.Windows.Forms.RadioButton();
            this.rdbShpefile = new System.Windows.Forms.RadioButton();
            this.rdbMDB = new System.Windows.Forms.RadioButton();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnScanLog = new DevComponents.DotNetBar.ButtonX();
            this.rdbTempData = new System.Windows.Forms.RadioButton();
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
            this.txtCheckDataPath.Location = new System.Drawing.Point(12, 134);
            this.txtCheckDataPath.Name = "txtCheckDataPath";
            this.txtCheckDataPath.Size = new System.Drawing.Size(389, 25);
            this.txtCheckDataPath.TabIndex = 0;
            // 
            // btnScanDataPath
            // 
            this.btnScanDataPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanDataPath.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanDataPath.Location = new System.Drawing.Point(407, 136);
            this.btnScanDataPath.Name = "btnScanDataPath";
            this.btnScanDataPath.Size = new System.Drawing.Size(75, 23);
            this.btnScanDataPath.TabIndex = 2;
            this.btnScanDataPath.Text = "...";
            this.btnScanDataPath.Click += new System.EventHandler(this.btnScanDataPath_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCheck.Location = new System.Drawing.Point(326, 177);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 4;
            this.btnCheck.Text = "开始检查";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(407, 177);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
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
            this.groupPanel1.Controls.Add(this.rdbShpefile);
            this.groupPanel1.Controls.Add(this.rdbMDB);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(492, 92);
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
            this.groupPanel1.Text = "检查数据类型";
            // 
            // rdbGDB
            // 
            this.rdbGDB.AutoSize = true;
            this.rdbGDB.BackColor = System.Drawing.Color.Transparent;
            this.rdbGDB.Location = new System.Drawing.Point(272, 19);
            this.rdbGDB.Name = "rdbGDB";
            this.rdbGDB.Size = new System.Drawing.Size(97, 19);
            this.rdbGDB.TabIndex = 2;
            this.rdbGDB.Text = "GDB文件夹";
            this.rdbGDB.UseVisualStyleBackColor = false;
            // 
            // rdbShpefile
            // 
            this.rdbShpefile.AutoSize = true;
            this.rdbShpefile.BackColor = System.Drawing.Color.Transparent;
            this.rdbShpefile.Location = new System.Drawing.Point(114, 19);
            this.rdbShpefile.Name = "rdbShpefile";
            this.rdbShpefile.Size = new System.Drawing.Size(130, 19);
            this.rdbShpefile.TabIndex = 1;
            this.rdbShpefile.Text = "ShapeFile文件";
            this.rdbShpefile.UseVisualStyleBackColor = false;
            // 
            // rdbMDB
            // 
            this.rdbMDB.AutoSize = true;
            this.rdbMDB.BackColor = System.Drawing.Color.Transparent;
            this.rdbMDB.Checked = true;
            this.rdbMDB.Location = new System.Drawing.Point(9, 19);
            this.rdbMDB.Name = "rdbMDB";
            this.rdbMDB.Size = new System.Drawing.Size(82, 19);
            this.rdbMDB.TabIndex = 0;
            this.rdbMDB.TabStop = true;
            this.rdbMDB.Text = "MDB文件";
            this.rdbMDB.UseVisualStyleBackColor = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 105);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(102, 23);
            this.labelX1.TabIndex = 7;
            this.labelX1.Text = "检查数据：";
            // 
            // btnScanLog
            // 
            this.btnScanLog.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanLog.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanLog.Location = new System.Drawing.Point(245, 177);
            this.btnScanLog.Name = "btnScanLog";
            this.btnScanLog.Size = new System.Drawing.Size(75, 23);
            this.btnScanLog.TabIndex = 9;
            this.btnScanLog.Text = "查看日志";
            this.btnScanLog.Click += new System.EventHandler(this.btnScanLog_Click);
            // 
            // rdbTempData
            // 
            this.rdbTempData.AutoSize = true;
            this.rdbTempData.BackColor = System.Drawing.Color.Transparent;
            this.rdbTempData.Location = new System.Drawing.Point(404, 19);
            this.rdbTempData.Name = "rdbTempData";
            this.rdbTempData.Size = new System.Drawing.Size(73, 19);
            this.rdbTempData.TabIndex = 3;
            this.rdbTempData.TabStop = true;
            this.rdbTempData.Text = "临时库";
            this.rdbTempData.UseVisualStyleBackColor = false;
            this.rdbTempData.CheckedChanged += new System.EventHandler(this.rdbTempData_CheckedChanged);
            // 
            // FrmLogicCheckSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 206);
            this.Controls.Add(this.btnScanLog);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnScanDataPath);
            this.Controls.Add(this.txtCheckDataPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogicCheckSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "森林数据逻辑检查";
            this.Load += new System.EventHandler(this.FrmLogicCheckSet_Load);
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
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.RadioButton rdbMDB;
        private System.Windows.Forms.RadioButton rdbGDB;
        private System.Windows.Forms.RadioButton rdbShpefile;
        private DevComponents.DotNetBar.ButtonX btnScanLog;
        private System.Windows.Forms.RadioButton rdbTempData;
    }
}