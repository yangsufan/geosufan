namespace GeoDBATool
{
    partial class FrmInitiaRasterCatalog
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
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.rBServeData = new System.Windows.Forms.RadioButton();
            this.rBOtherData = new System.Windows.Forms.RadioButton();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnBroswer = new DevComponents.DotNetBar.ButtonX();
            this.txtData = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtOperator = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(6, 47);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(69, 23);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "外部数据：";
            // 
            // rBServeData
            // 
            this.rBServeData.AutoSize = true;
            this.rBServeData.Location = new System.Drawing.Point(6, 3);
            this.rBServeData.Name = "rBServeData";
            this.rBServeData.Size = new System.Drawing.Size(155, 16);
            this.rBServeData.TabIndex = 1;
            this.rBServeData.TabStop = true;
            this.rBServeData.Text = "选择远程服务器上的数据";
            this.rBServeData.UseVisualStyleBackColor = true;
            this.rBServeData.CheckedChanged += new System.EventHandler(this.rBServeData_CheckedChanged);
            // 
            // rBOtherData
            // 
            this.rBOtherData.AutoSize = true;
            this.rBOtherData.Location = new System.Drawing.Point(6, 25);
            this.rBOtherData.Name = "rBOtherData";
            this.rBOtherData.Size = new System.Drawing.Size(95, 16);
            this.rBOtherData.TabIndex = 1;
            this.rBOtherData.TabStop = true;
            this.rBOtherData.Text = "选择外部数据";
            this.rBOtherData.UseVisualStyleBackColor = true;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnBroswer);
            this.groupPanel1.Controls.Add(this.txtData);
            this.groupPanel1.Controls.Add(this.rBServeData);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.rBOtherData);
            this.groupPanel1.Location = new System.Drawing.Point(3, 2);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(338, 105);
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
            this.groupPanel1.TabIndex = 2;
            this.groupPanel1.Text = "请选择入库的数据";
            // 
            // btnBroswer
            // 
            this.btnBroswer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBroswer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBroswer.Location = new System.Drawing.Point(288, 47);
            this.btnBroswer.Name = "btnBroswer";
            this.btnBroswer.Size = new System.Drawing.Size(43, 23);
            this.btnBroswer.TabIndex = 3;
            this.btnBroswer.Text = "选择";
            this.btnBroswer.Click += new System.EventHandler(this.btnBroswer_Click);
            // 
            // txtData
            // 
            // 
            // 
            // 
            this.txtData.Border.Class = "TextBoxBorder";
            this.txtData.Location = new System.Drawing.Point(72, 47);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(216, 21);
            this.txtData.TabIndex = 2;
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(284, 140);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(52, 23);
            this.btnCancle.TabIndex = 4;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(222, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 113);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 5;
            this.labelX1.Text = "操 作 员：";
            // 
            // txtOperator
            // 
            // 
            // 
            // 
            this.txtOperator.Border.Class = "TextBoxBorder";
            this.txtOperator.Location = new System.Drawing.Point(78, 113);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.Size = new System.Drawing.Size(259, 21);
            this.txtOperator.TabIndex = 6;
            // 
            // FrmInitiaRasterCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 165);
            this.Controls.Add(this.txtOperator);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInitiaRasterCatalog";
            this.ShowIcon = false;
            this.Text = "栅格目录入库";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.RadioButton rBServeData;
        private System.Windows.Forms.RadioButton rBOtherData;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX btnBroswer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtData;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtOperator;
    }
}