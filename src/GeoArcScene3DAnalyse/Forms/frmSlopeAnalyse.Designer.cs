namespace GeoArcScene3DAnalyse
{
    partial class frmSlopeAnalyse
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
            this.comboBoxOpen = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioBtnDegree = new System.Windows.Forms.RadioButton();
            this.radioBtn = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtZFactor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCellSize = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSave = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入表面：";
            // 
            // comboBoxOpen
            // 
            this.comboBoxOpen.FormattingEnabled = true;
            this.comboBoxOpen.Location = new System.Drawing.Point(84, 18);
            this.comboBoxOpen.Name = "comboBoxOpen";
            this.comboBoxOpen.Size = new System.Drawing.Size(212, 20);
            this.comboBoxOpen.TabIndex = 1;
            this.comboBoxOpen.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpen_SelectedIndexChanged);
            this.comboBoxOpen.DropDown += new System.EventHandler(this.comboBoxOpen_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "输出度量：";
            // 
            // radioBtnDegree
            // 
            this.radioBtnDegree.AutoSize = true;
            this.radioBtnDegree.Checked = true;
            this.radioBtnDegree.Location = new System.Drawing.Point(112, 49);
            this.radioBtnDegree.Name = "radioBtnDegree";
            this.radioBtnDegree.Size = new System.Drawing.Size(35, 16);
            this.radioBtnDegree.TabIndex = 4;
            this.radioBtnDegree.TabStop = true;
            this.radioBtnDegree.Text = "度";
            this.radioBtnDegree.UseVisualStyleBackColor = true;
            this.radioBtnDegree.CheckedChanged += new System.EventHandler(this.radioBtnDegree_CheckedChanged);
            // 
            // radioBtn
            // 
            this.radioBtn.AutoSize = true;
            this.radioBtn.Location = new System.Drawing.Point(214, 49);
            this.radioBtn.Name = "radioBtn";
            this.radioBtn.Size = new System.Drawing.Size(59, 16);
            this.radioBtn.TabIndex = 5;
            this.radioBtn.Text = "百分比";
            this.radioBtn.UseVisualStyleBackColor = true;
            this.radioBtn.CheckedChanged += new System.EventHandler(this.radioBtn_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Z因子：";
            // 
            // txtZFactor
            // 
            this.txtZFactor.Location = new System.Drawing.Point(84, 79);
            this.txtZFactor.Name = "txtZFactor";
            this.txtZFactor.Size = new System.Drawing.Size(212, 21);
            this.txtZFactor.TabIndex = 7;
            this.txtZFactor.Text = "1";
            this.txtZFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtZFactor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZFactor_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "输出像元大小：";
            // 
            // txtCellSize
            // 
            this.txtCellSize.Location = new System.Drawing.Point(84, 124);
            this.txtCellSize.Name = "txtCellSize";
            this.txtCellSize.Size = new System.Drawing.Size(212, 21);
            this.txtCellSize.TabIndex = 9;
            this.txtCellSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCellSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCellSize_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "输出栅格：";
            // 
            // txtSave
            // 
            this.txtSave.Location = new System.Drawing.Point(84, 167);
            this.txtSave.Name = "txtSave";
            this.txtSave.ReadOnly = true;
            this.txtSave.Size = new System.Drawing.Size(160, 21);
            this.txtSave.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(250, 165);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "浏览";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(250, 208);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSure
            // 
            this.btnSure.Enabled = false;
            this.btnSure.Location = new System.Drawing.Point(182, 208);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(53, 23);
            this.btnSure.TabIndex = 17;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // frmSlopeAnalyse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 243);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCellSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtZFactor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radioBtn);
            this.Controls.Add(this.radioBtnDegree);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxOpen);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmSlopeAnalyse";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坡度分析";

    
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxOpen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioBtnDegree;
        private System.Windows.Forms.RadioButton radioBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtZFactor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCellSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSure;
    }
}