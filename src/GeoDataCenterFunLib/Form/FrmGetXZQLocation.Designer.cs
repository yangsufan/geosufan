namespace GeoDataCenterFunLib
{
    partial class FrmGetXZQLocation
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbXiang = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cbXian = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cbShi = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cbsheng = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.cbcun = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.cbcun);
            this.groupBox1.Controls.Add(this.labelX5);
            this.groupBox1.Controls.Add(this.cbXiang);
            this.groupBox1.Controls.Add(this.labelX4);
            this.groupBox1.Controls.Add(this.cbXian);
            this.groupBox1.Controls.Add(this.labelX3);
            this.groupBox1.Controls.Add(this.cbShi);
            this.groupBox1.Controls.Add(this.labelX2);
            this.groupBox1.Controls.Add(this.cbsheng);
            this.groupBox1.Controls.Add(this.labelX1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(236, 270);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "行政区";
            // 
            // cbXiang
            // 
            this.cbXiang.DisplayMember = "Text";
            this.cbXiang.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbXiang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbXiang.FormattingEnabled = true;
            this.cbXiang.ItemHeight = 19;
            this.cbXiang.Location = new System.Drawing.Point(9, 186);
            this.cbXiang.Margin = new System.Windows.Forms.Padding(2);
            this.cbXiang.Name = "cbXiang";
            this.cbXiang.Size = new System.Drawing.Size(218, 25);
            this.cbXiang.TabIndex = 7;
            this.cbXiang.SelectedIndexChanged += new System.EventHandler(this.cbXiang_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(9, 163);
            this.labelX4.Margin = new System.Windows.Forms.Padding(2);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(216, 18);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "乡级行政区：";
            // 
            // cbXian
            // 
            this.cbXian.DisplayMember = "Text";
            this.cbXian.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbXian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbXian.FormattingEnabled = true;
            this.cbXian.ItemHeight = 19;
            this.cbXian.Location = new System.Drawing.Point(9, 138);
            this.cbXian.Margin = new System.Windows.Forms.Padding(2);
            this.cbXian.Name = "cbXian";
            this.cbXian.Size = new System.Drawing.Size(218, 25);
            this.cbXian.TabIndex = 5;
            this.cbXian.SelectedIndexChanged += new System.EventHandler(this.cbXian_SelectedIndexChanged);
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(9, 115);
            this.labelX3.Margin = new System.Windows.Forms.Padding(2);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(216, 18);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "县级行政区：";
            // 
            // cbShi
            // 
            this.cbShi.DisplayMember = "Text";
            this.cbShi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbShi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbShi.FormattingEnabled = true;
            this.cbShi.ItemHeight = 19;
            this.cbShi.Location = new System.Drawing.Point(9, 90);
            this.cbShi.Margin = new System.Windows.Forms.Padding(2);
            this.cbShi.Name = "cbShi";
            this.cbShi.Size = new System.Drawing.Size(218, 25);
            this.cbShi.TabIndex = 3;
            this.cbShi.SelectedIndexChanged += new System.EventHandler(this.cbShi_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(9, 67);
            this.labelX2.Margin = new System.Windows.Forms.Padding(2);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(216, 18);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "市级行政区：";
            // 
            // cbsheng
            // 
            this.cbsheng.DisplayMember = "Text";
            this.cbsheng.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbsheng.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsheng.FormattingEnabled = true;
            this.cbsheng.ItemHeight = 19;
            this.cbsheng.Location = new System.Drawing.Point(9, 42);
            this.cbsheng.Margin = new System.Windows.Forms.Padding(2);
            this.cbsheng.Name = "cbsheng";
            this.cbsheng.Size = new System.Drawing.Size(218, 25);
            this.cbsheng.TabIndex = 1;
            this.cbsheng.SelectedIndexChanged += new System.EventHandler(this.cbsheng_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(9, 19);
            this.labelX1.Margin = new System.Windows.Forms.Padding(2);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(216, 18);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "省级行政区：";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(99, 274);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 26);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(165, 274);
            this.btnCancle.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(61, 26);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // cbcun
            // 
            this.cbcun.DisplayMember = "Text";
            this.cbcun.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbcun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbcun.FormattingEnabled = true;
            this.cbcun.ItemHeight = 19;
            this.cbcun.Location = new System.Drawing.Point(9, 238);
            this.cbcun.Margin = new System.Windows.Forms.Padding(2);
            this.cbcun.Name = "cbcun";
            this.cbcun.Size = new System.Drawing.Size(218, 25);
            this.cbcun.TabIndex = 9;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(9, 215);
            this.labelX5.Margin = new System.Windows.Forms.Padding(2);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(216, 18);
            this.labelX5.TabIndex = 8;
            this.labelX5.Text = "村级行政区：";
            // 
            // FrmGetXZQLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 311);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGetXZQLocation";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "行政区选择";
            this.Load += new System.EventHandler(this.FrmGetXZQLocation_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbXian;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbShi;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbsheng;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbXiang;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbcun;
        private DevComponents.DotNetBar.LabelX labelX5;
    }
}