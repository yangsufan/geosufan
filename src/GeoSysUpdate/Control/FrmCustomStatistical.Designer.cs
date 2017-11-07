namespace GeoSysUpdate
{
    partial class FrmCustomStatistical
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
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtSolutionName = new System.Windows.Forms.TextBox();
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.CBoxStatisticsField = new System.Windows.Forms.ComboBox();
            this.CBoxClassField = new System.Windows.Forms.ComboBox();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveSolution = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(33, 54);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "统计图层：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(33, 90);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "统计字段：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(33, 129);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 2;
            this.labelX3.Text = "分类字段：";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(13, 23);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(95, 23);
            this.labelX4.TabIndex = 3;
            this.labelX4.Text = "统计方案名称：";
            // 
            // txtSolutionName
            // 
            this.txtSolutionName.Location = new System.Drawing.Point(115, 23);
            this.txtSolutionName.Name = "txtSolutionName";
            this.txtSolutionName.Size = new System.Drawing.Size(323, 21);
            this.txtSolutionName.TabIndex = 4;
            // 
            // txtLayerName
            // 
            this.txtLayerName.Location = new System.Drawing.Point(115, 57);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(323, 21);
            this.txtLayerName.TabIndex = 5;
            this.txtLayerName.Click += new System.EventHandler(this.txtLayerName_Click);
            // 
            // CBoxStatisticsField
            // 
            this.CBoxStatisticsField.FormattingEnabled = true;
            this.CBoxStatisticsField.Location = new System.Drawing.Point(115, 92);
            this.CBoxStatisticsField.Name = "CBoxStatisticsField";
            this.CBoxStatisticsField.Size = new System.Drawing.Size(323, 20);
            this.CBoxStatisticsField.TabIndex = 6;
            this.CBoxStatisticsField.SelectedIndexChanged += new System.EventHandler(this.CBoxStatisticsField_SelectedIndexChanged);
            // 
            // CBoxClassField
            // 
            this.CBoxClassField.FormattingEnabled = true;
            this.CBoxClassField.Location = new System.Drawing.Point(115, 129);
            this.CBoxClassField.Name = "CBoxClassField";
            this.CBoxClassField.Size = new System.Drawing.Size(323, 20);
            this.CBoxClassField.TabIndex = 7;
            this.CBoxClassField.SelectedIndexChanged += new System.EventHandler(this.CBoxClassField_SelectedIndexChanged);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(264, 176);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(362, 176);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 9;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnSaveSolution
            // 
            this.btnSaveSolution.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveSolution.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveSolution.Location = new System.Drawing.Point(143, 176);
            this.btnSaveSolution.Name = "btnSaveSolution";
            this.btnSaveSolution.Size = new System.Drawing.Size(101, 23);
            this.btnSaveSolution.TabIndex = 10;
            this.btnSaveSolution.Text = "保存统计方案";
            this.btnSaveSolution.Click += new System.EventHandler(this.btnSaveSolution_Click);
            // 
            // FrmCustomStatistical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 211);
            this.Controls.Add(this.btnSaveSolution);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.CBoxClassField);
            this.Controls.Add(this.CBoxStatisticsField);
            this.Controls.Add(this.txtLayerName);
            this.Controls.Add(this.txtSolutionName);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCustomStatistical";
            this.ShowIcon = false;
            this.Text = "自定义专题统计";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private System.Windows.Forms.TextBox txtSolutionName;
        private System.Windows.Forms.TextBox txtLayerName;
        private System.Windows.Forms.ComboBox CBoxStatisticsField;
        private System.Windows.Forms.ComboBox CBoxClassField;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnSaveSolution;
    }
}