namespace GeoDataCenterFunLib
{
    partial class frmQueryDomain
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
            this.btnQuery = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.rdbInaccurate = new System.Windows.Forms.RadioButton();
            this.rdbAccurate = new System.Windows.Forms.RadioButton();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.ComField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.chax = new DevComponents.DotNetBar.LabelX();
            this.txtLayer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtXZQ = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnSelectXZQ = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuery.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnQuery.Location = new System.Drawing.Point(194, 165);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtBoxName);
            this.groupPanel1.Controls.Add(this.rdbInaccurate);
            this.groupPanel1.Controls.Add(this.rdbAccurate);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Location = new System.Drawing.Point(12, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(347, 82);
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
            this.groupPanel1.TabIndex = 5;
            // 
            // txtBoxName
            // 
            this.txtBoxName.Location = new System.Drawing.Point(64, 43);
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(274, 21);
            this.txtBoxName.TabIndex = 13;
            // 
            // rdbInaccurate
            // 
            this.rdbInaccurate.AutoSize = true;
            this.rdbInaccurate.BackColor = System.Drawing.Color.Transparent;
            this.rdbInaccurate.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbInaccurate.Location = new System.Drawing.Point(132, 10);
            this.rdbInaccurate.Name = "rdbInaccurate";
            this.rdbInaccurate.Size = new System.Drawing.Size(71, 16);
            this.rdbInaccurate.TabIndex = 11;
            this.rdbInaccurate.Text = "模糊查询";
            this.rdbInaccurate.UseVisualStyleBackColor = false;
            // 
            // rdbAccurate
            // 
            this.rdbAccurate.AutoSize = true;
            this.rdbAccurate.BackColor = System.Drawing.Color.Transparent;
            this.rdbAccurate.Checked = true;
            this.rdbAccurate.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbAccurate.Location = new System.Drawing.Point(21, 10);
            this.rdbAccurate.Name = "rdbAccurate";
            this.rdbAccurate.Size = new System.Drawing.Size(71, 16);
            this.rdbAccurate.TabIndex = 10;
            this.rdbAccurate.TabStop = true;
            this.rdbAccurate.Text = "精确查询";
            this.rdbAccurate.UseVisualStyleBackColor = false;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(3, 40);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(69, 22);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "小班号：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(278, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.ComField);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.Controls.Add(this.chax);
            this.groupPanel2.Controls.Add(this.txtLayer);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(12, 100);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(273, 10);
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
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 7;
            this.groupPanel2.Text = "图层信息";
            this.groupPanel2.Visible = false;
            // 
            // ComField
            // 
            this.ComField.DisplayMember = "Text";
            this.ComField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ComField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComField.FormattingEnabled = true;
            this.ComField.ItemHeight = 15;
            this.ComField.Location = new System.Drawing.Point(3, 91);
            this.ComField.Name = "ComField";
            this.ComField.Size = new System.Drawing.Size(261, 21);
            this.ComField.TabIndex = 3;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(3, 62);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "查询字段：";
            // 
            // chax
            // 
            this.chax.BackColor = System.Drawing.Color.Transparent;
            this.chax.Location = new System.Drawing.Point(3, 6);
            this.chax.Name = "chax";
            this.chax.Size = new System.Drawing.Size(113, 23);
            this.chax.TabIndex = 1;
            this.chax.Text = "查询图层名称：";
            // 
            // txtLayer
            // 
            // 
            // 
            // 
            this.txtLayer.Border.Class = "TextBoxBorder";
            this.txtLayer.Location = new System.Drawing.Point(3, 35);
            this.txtLayer.Name = "txtLayer";
            this.txtLayer.Size = new System.Drawing.Size(261, 21);
            this.txtLayer.TabIndex = 0;
            this.txtLayer.TextChanged += new System.EventHandler(this.txtLayer_TextChanged);
            this.txtLayer.Click += new System.EventHandler(this.txtLayer_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 102);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(119, 23);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "行政区选择（可选）";
            // 
            // txtXZQ
            // 
            // 
            // 
            // 
            this.txtXZQ.Border.Class = "TextBoxBorder";
            this.txtXZQ.Enabled = false;
            this.txtXZQ.Location = new System.Drawing.Point(12, 131);
            this.txtXZQ.Name = "txtXZQ";
            this.txtXZQ.Size = new System.Drawing.Size(257, 21);
            this.txtXZQ.TabIndex = 9;
            // 
            // btnSelectXZQ
            // 
            this.btnSelectXZQ.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectXZQ.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectXZQ.Location = new System.Drawing.Point(278, 132);
            this.btnSelectXZQ.Name = "btnSelectXZQ";
            this.btnSelectXZQ.Size = new System.Drawing.Size(75, 23);
            this.btnSelectXZQ.TabIndex = 10;
            this.btnSelectXZQ.Text = "选择行政区";
            this.btnSelectXZQ.Click += new System.EventHandler(this.btnSelectXZQ_Click);
            // 
            // frmQueryDomain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 200);
            this.Controls.Add(this.btnSelectXZQ);
            this.Controls.Add(this.txtXZQ);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnQuery);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQueryDomain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "林班查询";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnQuery;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.RadioButton rdbInaccurate;
        private System.Windows.Forms.RadioButton rdbAccurate;
        private System.Windows.Forms.TextBox txtBoxName;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLayer;
        private DevComponents.DotNetBar.Controls.ComboBoxEx ComField;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX chax;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtXZQ;
        private DevComponents.DotNetBar.ButtonX btnSelectXZQ;
    }
}