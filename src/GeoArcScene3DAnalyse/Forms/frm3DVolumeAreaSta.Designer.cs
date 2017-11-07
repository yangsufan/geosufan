namespace GeoArcScene3DAnalyse
{
    partial class frm3DVolumeAreaSta
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
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxOpen = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btncount = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtVolumeAbove = new System.Windows.Forms.TextBox();
            this.txtVolumeBelow = new System.Windows.Forms.TextBox();
            this.txtAreaAbove = new System.Windows.Forms.TextBox();
            this.txtAreaBelow = new System.Windows.Forms.TextBox();
            this.txtPara = new System.Windows.Forms.TextBox();
            this.txtArea2DSel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sldPlaneHeight = new System.Windows.Forms.TrackBar();
            this.DrawGeo = new System.Windows.Forms.Button();
            this.chkShowContour = new System.Windows.Forms.CheckBox();
            this.chkShowLWRP = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPlaneHeight = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label = new DevComponents.DotNetBar.LabelX();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sldPlaneHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "输入表面集：";
            // 
            // comboBoxOpen
            // 
            this.comboBoxOpen.FormattingEnabled = true;
            this.comboBoxOpen.Location = new System.Drawing.Point(95, 9);
            this.comboBoxOpen.Name = "comboBoxOpen";
            this.comboBoxOpen.Size = new System.Drawing.Size(315, 20);
            this.comboBoxOpen.TabIndex = 18;
            this.comboBoxOpen.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpen_SelectedIndexChanged);
            this.comboBoxOpen.DropDown += new System.EventHandler(this.comboBoxOpen_DropDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label);
            this.groupBox2.Controls.Add(this.btncount);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtVolumeAbove);
            this.groupBox2.Controls.Add(this.txtVolumeBelow);
            this.groupBox2.Controls.Add(this.txtAreaAbove);
            this.groupBox2.Controls.Add(this.txtAreaBelow);
            this.groupBox2.Controls.Add(this.txtPara);
            this.groupBox2.Controls.Add(this.txtArea2DSel);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(1, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(427, 233);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "统计结果";
            // 
            // btncount
            // 
            this.btncount.Enabled = false;
            this.btncount.Location = new System.Drawing.Point(25, 20);
            this.btncount.Name = "btncount";
            this.btncount.Size = new System.Drawing.Size(74, 23);
            this.btncount.TabIndex = 45;
            this.btncount.Text = "计算统计值";
            this.btncount.UseVisualStyleBackColor = true;
            this.btncount.Click += new System.EventHandler(this.btncount_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(380, 177);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 43;
            this.label15.Text = "立方米";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(183, 177);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 42;
            this.label14.Text = "平方米";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(182, 126);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 41;
            this.label12.Text = "平方米";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(377, 126);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 38;
            this.label13.Text = "立方米";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(378, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 36;
            this.label11.Text = "米";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(182, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 35;
            this.label10.Text = "平方米";
            // 
            // txtVolumeAbove
            // 
            this.txtVolumeAbove.Location = new System.Drawing.Point(229, 174);
            this.txtVolumeAbove.Name = "txtVolumeAbove";
            this.txtVolumeAbove.ReadOnly = true;
            this.txtVolumeAbove.Size = new System.Drawing.Size(143, 21);
            this.txtVolumeAbove.TabIndex = 34;
            this.txtVolumeAbove.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtVolumeBelow
            // 
            this.txtVolumeBelow.Location = new System.Drawing.Point(229, 123);
            this.txtVolumeBelow.Name = "txtVolumeBelow";
            this.txtVolumeBelow.ReadOnly = true;
            this.txtVolumeBelow.Size = new System.Drawing.Size(143, 21);
            this.txtVolumeBelow.TabIndex = 33;
            this.txtVolumeBelow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAreaAbove
            // 
            this.txtAreaAbove.Location = new System.Drawing.Point(24, 174);
            this.txtAreaAbove.Name = "txtAreaAbove";
            this.txtAreaAbove.ReadOnly = true;
            this.txtAreaAbove.Size = new System.Drawing.Size(152, 21);
            this.txtAreaAbove.TabIndex = 32;
            this.txtAreaAbove.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAreaBelow
            // 
            this.txtAreaBelow.Location = new System.Drawing.Point(25, 123);
            this.txtAreaBelow.Name = "txtAreaBelow";
            this.txtAreaBelow.ReadOnly = true;
            this.txtAreaBelow.Size = new System.Drawing.Size(152, 21);
            this.txtAreaBelow.TabIndex = 31;
            this.txtAreaBelow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPara
            // 
            this.txtPara.Location = new System.Drawing.Point(229, 73);
            this.txtPara.Name = "txtPara";
            this.txtPara.ReadOnly = true;
            this.txtPara.Size = new System.Drawing.Size(143, 21);
            this.txtPara.TabIndex = 30;
            this.txtPara.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtArea2DSel
            // 
            this.txtArea2DSel.Location = new System.Drawing.Point(24, 73);
            this.txtArea2DSel.Name = "txtArea2DSel";
            this.txtArea2DSel.ReadOnly = true;
            this.txtArea2DSel.Size = new System.Drawing.Size(152, 21);
            this.txtArea2DSel.TabIndex = 24;
            this.txtArea2DSel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(227, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 12);
            this.label9.TabIndex = 23;
            this.label9.Text = "基准面以上体积：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "基准面以上表面积：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(230, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "基准面以下体积：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "基准面以下表面积：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(227, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "统计范围周长：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "投影面积：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sldPlaneHeight);
            this.groupBox1.Controls.Add(this.DrawGeo);
            this.groupBox1.Controls.Add(this.chkShowContour);
            this.groupBox1.Controls.Add(this.chkShowLWRP);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPlaneHeight);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 101);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数设置";
            // 
            // sldPlaneHeight
            // 
            this.sldPlaneHeight.Enabled = false;
            this.sldPlaneHeight.Location = new System.Drawing.Point(0, 50);
            this.sldPlaneHeight.Name = "sldPlaneHeight";
            this.sldPlaneHeight.Size = new System.Drawing.Size(298, 45);
            this.sldPlaneHeight.TabIndex = 26;
            this.sldPlaneHeight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sldPlaneHeight_MouseUp);
            // 
            // DrawGeo
            // 
            this.DrawGeo.Enabled = false;
            this.DrawGeo.Location = new System.Drawing.Point(316, 50);
            this.DrawGeo.Name = "DrawGeo";
            this.DrawGeo.Size = new System.Drawing.Size(93, 23);
            this.DrawGeo.TabIndex = 25;
            this.DrawGeo.Text = "绘制统计范围";
            this.DrawGeo.UseVisualStyleBackColor = true;
            this.DrawGeo.Click += new System.EventHandler(this.DrawGeo_Click);
            // 
            // chkShowContour
            // 
            this.chkShowContour.AutoSize = true;
            this.chkShowContour.Location = new System.Drawing.Point(325, 26);
            this.chkShowContour.Name = "chkShowContour";
            this.chkShowContour.Size = new System.Drawing.Size(84, 16);
            this.chkShowContour.TabIndex = 22;
            this.chkShowContour.Text = "显示轮廓线";
            this.chkShowContour.UseVisualStyleBackColor = true;
            this.chkShowContour.CheckedChanged += new System.EventHandler(this.chkShowContour_CheckedChanged);
            // 
            // chkShowLWRP
            // 
            this.chkShowLWRP.AutoSize = true;
            this.chkShowLWRP.Checked = true;
            this.chkShowLWRP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowLWRP.Location = new System.Drawing.Point(232, 25);
            this.chkShowLWRP.Name = "chkShowLWRP";
            this.chkShowLWRP.Size = new System.Drawing.Size(84, 16);
            this.chkShowLWRP.TabIndex = 21;
            this.chkShowLWRP.Text = "显示参考面";
            this.chkShowLWRP.UseVisualStyleBackColor = true;
            this.chkShowLWRP.CheckedChanged += new System.EventHandler(this.chkShowLWRP_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(207, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "米";
            // 
            // txtPlaneHeight
            // 
            this.txtPlaneHeight.Location = new System.Drawing.Point(94, 20);
            this.txtPlaneHeight.Name = "txtPlaneHeight";
            this.txtPlaneHeight.ReadOnly = true;
            this.txtPlaneHeight.Size = new System.Drawing.Size(107, 21);
            this.txtPlaneHeight.TabIndex = 19;
            this.txtPlaneHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPlaneHeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPlaneHeight_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "基准面高度：";
            // 
            // label
            // 
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label.Location = new System.Drawing.Point(6, 210);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(310, 23);
            this.label.TabIndex = 46;
            // 
            // frm3DVolumeAreaSta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 377);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxOpen);
            this.Controls.Add(this.label5);
            this.MaximizeBox = false;
            this.Name = "frm3DVolumeAreaSta";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "面积、体积统计";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm3DVolumeAreaSta_FormClosing);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sldPlaneHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxOpen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtPlaneHeight;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox chkShowContour;
        public System.Windows.Forms.CheckBox chkShowLWRP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txtVolumeAbove;
        public System.Windows.Forms.TextBox txtVolumeBelow;
        public System.Windows.Forms.TextBox txtAreaAbove;
        public System.Windows.Forms.TextBox txtAreaBelow;
        public System.Windows.Forms.TextBox txtPara;
        public System.Windows.Forms.TextBox txtArea2DSel;
        private System.Windows.Forms.Button DrawGeo;
        private System.Windows.Forms.TrackBar sldPlaneHeight;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btncount;
        private DevComponents.DotNetBar.LabelX label;
    }
}