namespace GeoArcScene3DAnalyse
{
    partial class frmCuttfillAnalyse
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFortraster = new System.Windows.Forms.ComboBox();
            this.comboBoxToraster = new System.Windows.Forms.ComboBox();
            this.txtZFactor = new System.Windows.Forms.TextBox();
            this.txtCellSize = new System.Windows.Forms.TextBox();
            this.txtSave = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "填挖前表面：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Z因子：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "输出像元大小：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "输出栅格：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "填挖后表面：";
            // 
            // comboBoxFortraster
            // 
            this.comboBoxFortraster.FormattingEnabled = true;
            this.comboBoxFortraster.Location = new System.Drawing.Point(86, 6);
            this.comboBoxFortraster.Name = "comboBoxFortraster";
            this.comboBoxFortraster.Size = new System.Drawing.Size(228, 20);
            this.comboBoxFortraster.TabIndex = 10;
            this.comboBoxFortraster.SelectedIndexChanged += new System.EventHandler(this.comboBoxFortraster_SelectedIndexChanged);
            this.comboBoxFortraster.DropDown += new System.EventHandler(this.comboBoxFortraster_DropDown);
            // 
            // comboBoxToraster
            // 
            this.comboBoxToraster.FormattingEnabled = true;
            this.comboBoxToraster.Location = new System.Drawing.Point(86, 49);
            this.comboBoxToraster.Name = "comboBoxToraster";
            this.comboBoxToraster.Size = new System.Drawing.Size(228, 20);
            this.comboBoxToraster.TabIndex = 11;
            this.comboBoxToraster.SelectedIndexChanged += new System.EventHandler(this.comboBoxToraster_SelectedIndexChanged);
            this.comboBoxToraster.DropDown += new System.EventHandler(this.comboBoxToraster_DropDown);
            // 
            // txtZFactor
            // 
            this.txtZFactor.Location = new System.Drawing.Point(86, 85);
            this.txtZFactor.Name = "txtZFactor";
            this.txtZFactor.Size = new System.Drawing.Size(169, 21);
            this.txtZFactor.TabIndex = 12;
            this.txtZFactor.Text = "1";
            this.txtZFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtZFactor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZFactor_KeyPress);
            // 
            // txtCellSize
            // 
            this.txtCellSize.Location = new System.Drawing.Point(86, 128);
            this.txtCellSize.Name = "txtCellSize";
            this.txtCellSize.Size = new System.Drawing.Size(169, 21);
            this.txtCellSize.TabIndex = 21;
            this.txtCellSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCellSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCellSize_KeyPress);
            // 
            // txtSave
            // 
            this.txtSave.Location = new System.Drawing.Point(86, 166);
            this.txtSave.Name = "txtSave";
            this.txtSave.ReadOnly = true;
            this.txtSave.Size = new System.Drawing.Size(169, 21);
            this.txtSave.TabIndex = 22;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(261, 164);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 23);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "浏览";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSure
            // 
            this.btnSure.Enabled = false;
            this.btnSure.Location = new System.Drawing.Point(191, 201);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(53, 23);
            this.btnSure.TabIndex = 24;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(261, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmCuttfillAnalyse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 229);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.txtCellSize);
            this.Controls.Add(this.txtZFactor);
            this.Controls.Add(this.comboBoxToraster);
            this.Controls.Add(this.comboBoxFortraster);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCuttfillAnalyse";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "填挖";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxFortraster;
        private System.Windows.Forms.ComboBox comboBoxToraster;
        private System.Windows.Forms.TextBox txtZFactor;
        private System.Windows.Forms.TextBox txtCellSize;
        private System.Windows.Forms.TextBox txtSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Button btnCancel;
    }
}