namespace GeoDBATool
{
    partial class FrmCheck
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.CheckGDB = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.CheckMDB = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.CheckShapeFile = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtTemplate = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtTest = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnScanTemplate = new DevComponents.DotNetBar.ButtonX();
            this.btnScanTestData = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.CheckGDB);
            this.groupPanel1.Controls.Add(this.CheckMDB);
            this.groupPanel1.Controls.Add(this.CheckShapeFile);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(434, 53);
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
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 1;
            this.groupPanel1.Text = "数据库类型";
            // 
            // CheckGDB
            // 
            this.CheckGDB.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.CheckGDB.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckGDB.BackgroundStyle.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckGDB.BackgroundStyle.BorderColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckGDB.BackgroundStyle.BorderColorLight2 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckGDB.Location = new System.Drawing.Point(268, 3);
            this.CheckGDB.Name = "CheckGDB";
            this.CheckGDB.Size = new System.Drawing.Size(106, 23);
            this.CheckGDB.TabIndex = 2;
            this.CheckGDB.Text = "GDB数据";
            this.CheckGDB.Click += new System.EventHandler(this.CheckGDB_Click);
            // 
            // CheckMDB
            // 
            this.CheckMDB.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.CheckMDB.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckMDB.BackgroundStyle.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckMDB.BackgroundStyle.BorderColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckMDB.BackgroundStyle.BorderColorLight2 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckMDB.Location = new System.Drawing.Point(139, 3);
            this.CheckMDB.Name = "CheckMDB";
            this.CheckMDB.Size = new System.Drawing.Size(106, 23);
            this.CheckMDB.TabIndex = 2;
            this.CheckMDB.Text = "MDB数据";
            this.CheckMDB.Click += new System.EventHandler(this.CheckMDB_Click);
            // 
            // CheckShapeFile
            // 
            this.CheckShapeFile.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.CheckShapeFile.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckShapeFile.BackgroundStyle.BorderColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckShapeFile.BackgroundStyle.BorderColorLight = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckShapeFile.BackgroundStyle.BorderColorLight2 = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.CheckShapeFile.Checked = true;
            this.CheckShapeFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckShapeFile.CheckValue = "Y";
            this.CheckShapeFile.Location = new System.Drawing.Point(9, 3);
            this.CheckShapeFile.Name = "CheckShapeFile";
            this.CheckShapeFile.Size = new System.Drawing.Size(106, 23);
            this.CheckShapeFile.TabIndex = 0;
            this.CheckShapeFile.Text = "ShapeFile数据";
            this.CheckShapeFile.Click += new System.EventHandler(this.CheckShapeFile_Click);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(12, 69);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "模板数据：";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(12, 98);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "检测数据：";
            // 
            // txtTemplate
            // 
            // 
            // 
            // 
            this.txtTemplate.Border.Class = "TextBoxBorder";
            this.txtTemplate.Location = new System.Drawing.Point(74, 71);
            this.txtTemplate.Name = "txtTemplate";
            this.txtTemplate.ReadOnly = true;
            this.txtTemplate.Size = new System.Drawing.Size(271, 21);
            this.txtTemplate.TabIndex = 4;
            // 
            // txtTest
            // 
            // 
            // 
            // 
            this.txtTest.Border.Class = "TextBoxBorder";
            this.txtTest.Location = new System.Drawing.Point(74, 102);
            this.txtTest.Name = "txtTest";
            this.txtTest.ReadOnly = true;
            this.txtTest.Size = new System.Drawing.Size(271, 21);
            this.txtTest.TabIndex = 5;
            // 
            // btnScanTemplate
            // 
            this.btnScanTemplate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanTemplate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanTemplate.Location = new System.Drawing.Point(352, 68);
            this.btnScanTemplate.Name = "btnScanTemplate";
            this.btnScanTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnScanTemplate.TabIndex = 6;
            this.btnScanTemplate.Text = "...";
            this.btnScanTemplate.Click += new System.EventHandler(this.btnScanTemplate_Click);
            // 
            // btnScanTestData
            // 
            this.btnScanTestData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnScanTestData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnScanTestData.Location = new System.Drawing.Point(352, 102);
            this.btnScanTestData.Name = "btnScanTestData";
            this.btnScanTestData.Size = new System.Drawing.Size(75, 23);
            this.btnScanTestData.TabIndex = 7;
            this.btnScanTestData.Text = "...";
            this.btnScanTestData.Click += new System.EventHandler(this.btnScanTestData_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(270, 144);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "检测";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(352, 144);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 9;
            this.btnCancle.Text = "退出";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // FrmCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 179);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnScanTestData);
            this.Controls.Add(this.btnScanTemplate);
            this.Controls.Add(this.txtTest);
            this.Controls.Add(this.txtTemplate);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCheck";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmCheck_Load);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.CheckBoxX CheckGDB;
        private DevComponents.DotNetBar.Controls.CheckBoxX CheckMDB;
        private DevComponents.DotNetBar.Controls.CheckBoxX CheckShapeFile;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTemplate;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTest;
        private DevComponents.DotNetBar.ButtonX btnScanTemplate;
        private DevComponents.DotNetBar.ButtonX btnScanTestData;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.ButtonX btnCancle;
    }
}