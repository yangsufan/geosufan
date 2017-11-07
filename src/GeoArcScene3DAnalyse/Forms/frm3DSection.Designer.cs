namespace GeoArcScene3DAnalyse
{
    partial class frm3DSection
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
            this.DrawGeo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtZmin = new System.Windows.Forms.TextBox();
            this.txtZmax = new System.Windows.Forms.TextBox();
            this.txtSurfaceLengh = new System.Windows.Forms.TextBox();
            this.txtPreject = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.axBtnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label = new DevComponents.DotNetBar.LabelX();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "输入表面集：";
            // 
            // comboBoxOpen
            // 
            this.comboBoxOpen.FormattingEnabled = true;
            this.comboBoxOpen.Location = new System.Drawing.Point(95, 14);
            this.comboBoxOpen.Name = "comboBoxOpen";
            this.comboBoxOpen.Size = new System.Drawing.Size(256, 20);
            this.comboBoxOpen.TabIndex = 19;
            this.comboBoxOpen.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpen_SelectedIndexChanged);
            this.comboBoxOpen.DropDown += new System.EventHandler(this.comboBoxOpen_DropDown);
            // 
            // DrawGeo
            // 
            this.DrawGeo.Enabled = false;
            this.DrawGeo.Location = new System.Drawing.Point(360, 12);
            this.DrawGeo.Name = "DrawGeo";
            this.DrawGeo.Size = new System.Drawing.Size(93, 23);
            this.DrawGeo.TabIndex = 26;
            this.DrawGeo.Text = "绘制统计范围";
            this.DrawGeo.UseVisualStyleBackColor = true;
            this.DrawGeo.Click += new System.EventHandler(this.DrawGeo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 257);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(440, 233);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(153, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "沿 线 高 程 剖 面 图";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtZmin);
            this.groupBox2.Controls.Add(this.txtZmax);
            this.groupBox2.Controls.Add(this.txtSurfaceLengh);
            this.groupBox2.Controls.Add(this.txtPreject);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(1, 299);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(452, 124);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "统计结果";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(420, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "米";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(420, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "米";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(194, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "米";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(194, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 34;
            this.label3.Text = "米";
            // 
            // txtZmin
            // 
            this.txtZmin.Location = new System.Drawing.Point(252, 88);
            this.txtZmin.Name = "txtZmin";
            this.txtZmin.ReadOnly = true;
            this.txtZmin.Size = new System.Drawing.Size(162, 21);
            this.txtZmin.TabIndex = 33;
            this.txtZmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtZmax
            // 
            this.txtZmax.Location = new System.Drawing.Point(24, 90);
            this.txtZmax.Name = "txtZmax";
            this.txtZmax.ReadOnly = true;
            this.txtZmax.Size = new System.Drawing.Size(164, 21);
            this.txtZmax.TabIndex = 31;
            this.txtZmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSurfaceLengh
            // 
            this.txtSurfaceLengh.Location = new System.Drawing.Point(252, 41);
            this.txtSurfaceLengh.Name = "txtSurfaceLengh";
            this.txtSurfaceLengh.ReadOnly = true;
            this.txtSurfaceLengh.Size = new System.Drawing.Size(162, 21);
            this.txtSurfaceLengh.TabIndex = 30;
            this.txtSurfaceLengh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPreject
            // 
            this.txtPreject.Location = new System.Drawing.Point(24, 41);
            this.txtPreject.Name = "txtPreject";
            this.txtPreject.ReadOnly = true;
            this.txtPreject.Size = new System.Drawing.Size(164, 21);
            this.txtPreject.TabIndex = 24;
            this.txtPreject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(250, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "最低点高程：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "最高点高程：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(250, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 19;
            this.label4.Text = "线段曲面长度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "线段投影长度：";
            // 
            // axBtnOK
            // 
            this.axBtnOK.Enabled = false;
            this.axBtnOK.Location = new System.Drawing.Point(280, 429);
            this.axBtnOK.Name = "axBtnOK";
            this.axBtnOK.Size = new System.Drawing.Size(83, 23);
            this.axBtnOK.TabIndex = 29;
            this.axBtnOK.Text = "输出剖面图";
            this.axBtnOK.UseVisualStyleBackColor = true;
            this.axBtnOK.Click += new System.EventHandler(this.axBtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(400, 429);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label
            // 
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label.Location = new System.Drawing.Point(1, 429);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(264, 23);
            this.label.TabIndex = 31;
            // 
            // frm3DSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 456);
            this.Controls.Add(this.label);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.axBtnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.DrawGeo);
            this.Controls.Add(this.comboBoxOpen);
            this.Controls.Add(this.label5);
            this.MaximizeBox = false;
            this.Name = "frm3DSection";
            this.ShowIcon = false;
            this.Text = "剖面分析";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frm3DSection_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm3DSection_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxOpen;
        private System.Windows.Forms.Button DrawGeo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox txtZmin;
        public System.Windows.Forms.TextBox txtZmax;
        public System.Windows.Forms.TextBox txtSurfaceLengh;
        public System.Windows.Forms.TextBox txtPreject;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button axBtnOK;
        private System.Windows.Forms.Button btnCancel;
        private DevComponents.DotNetBar.LabelX label;
    }
}