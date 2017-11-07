namespace GeoDBATool
{
    partial class FrmDeleteTempData
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cbProName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbLayerName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cbXianName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cbShiName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnClearAll = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 24);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(97, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "工程名称：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 84);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(97, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "图层名称：";
            // 
            // cbProName
            // 
            this.cbProName.DisplayMember = "Text";
            this.cbProName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbProName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProName.FormattingEnabled = true;
            this.cbProName.ItemHeight = 19;
            this.cbProName.Location = new System.Drawing.Point(6, 53);
            this.cbProName.Name = "cbProName";
            this.cbProName.Size = new System.Drawing.Size(304, 25);
            this.cbProName.TabIndex = 2;
            this.cbProName.SelectedIndexChanged += new System.EventHandler(this.cbProName_SelectedIndexChanged);
            // 
            // cbLayerName
            // 
            this.cbLayerName.DisplayMember = "Text";
            this.cbLayerName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbLayerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLayerName.FormattingEnabled = true;
            this.cbLayerName.ItemHeight = 19;
            this.cbLayerName.Location = new System.Drawing.Point(6, 113);
            this.cbLayerName.Name = "cbLayerName";
            this.cbLayerName.Size = new System.Drawing.Size(304, 25);
            this.cbLayerName.TabIndex = 3;
            this.cbLayerName.SelectedIndexChanged += new System.EventHandler(this.cbLayerName_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelX1);
            this.groupBox1.Controls.Add(this.cbLayerName);
            this.groupBox1.Controls.Add(this.labelX2);
            this.groupBox1.Controls.Add(this.cbProName);
            this.groupBox1.Location = new System.Drawing.Point(0, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 180);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图层选择";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelX4);
            this.groupBox2.Controls.Add(this.labelX3);
            this.groupBox2.Controls.Add(this.cbXianName);
            this.groupBox2.Controls.Add(this.cbShiName);
            this.groupBox2.Location = new System.Drawing.Point(322, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 180);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择县级单位";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(6, 84);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(97, 23);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "县";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(6, 24);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(97, 23);
            this.labelX3.TabIndex = 5;
            this.labelX3.Text = "市";
            // 
            // cbXianName
            // 
            this.cbXianName.DisplayMember = "Text";
            this.cbXianName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbXianName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbXianName.FormattingEnabled = true;
            this.cbXianName.ItemHeight = 19;
            this.cbXianName.Location = new System.Drawing.Point(6, 113);
            this.cbXianName.Name = "cbXianName";
            this.cbXianName.Size = new System.Drawing.Size(243, 25);
            this.cbXianName.TabIndex = 4;
            // 
            // cbShiName
            // 
            this.cbShiName.DisplayMember = "Text";
            this.cbShiName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbShiName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShiName.FormattingEnabled = true;
            this.cbShiName.ItemHeight = 19;
            this.cbShiName.Location = new System.Drawing.Point(6, 53);
            this.cbShiName.Name = "cbShiName";
            this.cbShiName.Size = new System.Drawing.Size(243, 25);
            this.cbShiName.TabIndex = 3;
            this.cbShiName.SelectedIndexChanged += new System.EventHandler(this.cbShiName_SelectedIndexChanged);
            // 
            // btnClearAll
            // 
            this.btnClearAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClearAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClearAll.Location = new System.Drawing.Point(386, 198);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(95, 39);
            this.btnClearAll.TabIndex = 6;
            this.btnClearAll.Text = "清除全部";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(487, 198);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(95, 39);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClear.Location = new System.Drawing.Point(285, 198);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 39);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "清除";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FrmDeleteTempData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 246);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDeleteTempData";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "清除临时库";
            this.Load += new System.EventHandler(this.FrmDeleteTempData_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbProName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbLayerName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbShiName;
        private DevComponents.DotNetBar.ButtonX btnClearAll;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnClear;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbXianName;
    }
}