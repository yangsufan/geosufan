namespace GeoArcScene3DAnalyse
{
    partial class frmViewshedAnalyse
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxOpenraster = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxOpenfeatures = new System.Windows.Forms.ComboBox();
            this.txtZFactor = new System.Windows.Forms.TextBox();
            this.txtSave = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCellSize = new System.Windows.Forms.TextBox();
            this.labePrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "输入表面：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "观测点：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Z因子：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "输出像元大小：";
            // 
            // comboBoxOpenraster
            // 
            this.comboBoxOpenraster.FormattingEnabled = true;
            this.comboBoxOpenraster.Location = new System.Drawing.Point(92, 15);
            this.comboBoxOpenraster.Name = "comboBoxOpenraster";
            this.comboBoxOpenraster.Size = new System.Drawing.Size(228, 20);
            this.comboBoxOpenraster.TabIndex = 5;
            this.comboBoxOpenraster.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpenraster_SelectedIndexChanged);
            this.comboBoxOpenraster.DropDown += new System.EventHandler(this.comboBoxOpenraster_DropDown);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(35, 88);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "使用地球曲率";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "输出栅格：";
            // 
            // comboBoxOpenfeatures
            // 
            this.comboBoxOpenfeatures.FormattingEnabled = true;
            this.comboBoxOpenfeatures.Location = new System.Drawing.Point(92, 53);
            this.comboBoxOpenfeatures.Name = "comboBoxOpenfeatures";
            this.comboBoxOpenfeatures.Size = new System.Drawing.Size(228, 20);
            this.comboBoxOpenfeatures.TabIndex = 8;
            this.comboBoxOpenfeatures.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpenfeatures_SelectedIndexChanged);
            this.comboBoxOpenfeatures.DropDown += new System.EventHandler(this.comboBoxOpenfeatures_DropDown);
            // 
            // txtZFactor
            // 
            this.txtZFactor.Location = new System.Drawing.Point(92, 119);
            this.txtZFactor.Name = "txtZFactor";
            this.txtZFactor.Size = new System.Drawing.Size(169, 21);
            this.txtZFactor.TabIndex = 9;
            this.txtZFactor.Text = "1";
            this.txtZFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtZFactor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtZFactor_KeyPress);
            // 
            // txtSave
            // 
            this.txtSave.Location = new System.Drawing.Point(92, 193);
            this.txtSave.Name = "txtSave";
            this.txtSave.ReadOnly = true;
            this.txtSave.Size = new System.Drawing.Size(169, 21);
            this.txtSave.TabIndex = 12;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(267, 193);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(53, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "浏览";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSure
            // 
            this.btnSure.Enabled = false;
            this.btnSure.Location = new System.Drawing.Point(196, 230);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(53, 23);
            this.btnSure.TabIndex = 18;
            this.btnSure.Text = "确定";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(267, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(53, 23);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtCellSize
            // 
            this.txtCellSize.Location = new System.Drawing.Point(92, 153);
            this.txtCellSize.Name = "txtCellSize";
            this.txtCellSize.Size = new System.Drawing.Size(169, 21);
            this.txtCellSize.TabIndex = 20;
            this.txtCellSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCellSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCellSize_KeyPress);
            // 
            // labePrompt
            // 
            this.labePrompt.AutoSize = true;
            this.labePrompt.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labePrompt.ForeColor = System.Drawing.Color.Red;
            this.labePrompt.Location = new System.Drawing.Point(3, 244);
            this.labePrompt.Name = "labePrompt";
            this.labePrompt.Size = new System.Drawing.Size(0, 12);
            this.labePrompt.TabIndex = 59;
            // 
            // frmViewshedAnalyse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 256);
            this.Controls.Add(this.labePrompt);
            this.Controls.Add(this.txtCellSize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSave);
            this.Controls.Add(this.txtZFactor);
            this.Controls.Add(this.comboBoxOpenfeatures);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBoxOpenraster);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "frmViewshedAnalyse";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "通视分析";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxOpenraster;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxOpenfeatures;
        private System.Windows.Forms.TextBox txtZFactor;
        private System.Windows.Forms.TextBox txtSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtCellSize;
        private System.Windows.Forms.Label labePrompt;
    }
}