namespace GeoArcScene3DAnalyse
{
    partial class frm3DLineOfSight
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtObsOffset = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTarOffset = new System.Windows.Forms.TextBox();
            this.checkBoxCurv = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxOpen = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "观察者高度：";
            // 
            // txtObsOffset
            // 
            this.txtObsOffset.Location = new System.Drawing.Point(98, 44);
            this.txtObsOffset.Name = "txtObsOffset";
            this.txtObsOffset.Size = new System.Drawing.Size(181, 21);
            this.txtObsOffset.TabIndex = 10;
            this.txtObsOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtObsOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCellSize_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "被观察点高度：";
            // 
            // txtTarOffset
            // 
            this.txtTarOffset.Location = new System.Drawing.Point(98, 93);
            this.txtTarOffset.Name = "txtTarOffset";
            this.txtTarOffset.Size = new System.Drawing.Size(181, 21);
            this.txtTarOffset.TabIndex = 12;
            this.txtTarOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTarOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // checkBoxCurv
            // 
            this.checkBoxCurv.AutoSize = true;
            this.checkBoxCurv.Location = new System.Drawing.Point(26, 143);
            this.checkBoxCurv.Name = "checkBoxCurv";
            this.checkBoxCurv.Size = new System.Drawing.Size(132, 16);
            this.checkBoxCurv.TabIndex = 13;
            this.checkBoxCurv.Text = "应用曲率和折射校正";
            this.checkBoxCurv.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(285, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "米";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(285, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "米";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "输入表面集：";
            // 
            // comboBoxOpen
            // 
            this.comboBoxOpen.FormattingEnabled = true;
            this.comboBoxOpen.Location = new System.Drawing.Point(98, 6);
            this.comboBoxOpen.Name = "comboBoxOpen";
            this.comboBoxOpen.Size = new System.Drawing.Size(181, 20);
            this.comboBoxOpen.TabIndex = 17;
            this.comboBoxOpen.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpen_SelectedIndexChanged);
            this.comboBoxOpen.DropDown += new System.EventHandler(this.comboBoxOpen_DropDown);
            // 
            // frm3DLineOfSight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 171);
            this.Controls.Add(this.comboBoxOpen);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxCurv);
            this.Controls.Add(this.txtTarOffset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtObsOffset);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm3DLineOfSight";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "两点间通视分析";
            this.Load += new System.EventHandler(this.frm3DLineOfSight_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm3DLineOfSight_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox checkBoxCurv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxOpen;
        public System.Windows.Forms.TextBox txtObsOffset;
        public System.Windows.Forms.TextBox txtTarOffset;
    }
}