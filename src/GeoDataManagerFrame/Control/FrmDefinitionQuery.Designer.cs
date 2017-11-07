namespace GeoDataManagerFrame
{
    partial class FrmDefinitionQuery
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
            this.RichTxtCondition = new System.Windows.Forms.RichTextBox();
            this.btnSetCondition = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveSolution = new DevComponents.DotNetBar.ButtonX();
            this.btnOpenSolution = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtLayerName = new System.Windows.Forms.TextBox();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 58);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(103, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "显示条件：";
            // 
            // RichTxtCondition
            // 
            this.RichTxtCondition.Enabled = false;
            this.RichTxtCondition.Location = new System.Drawing.Point(12, 82);
            this.RichTxtCondition.Name = "RichTxtCondition";
            this.RichTxtCondition.Size = new System.Drawing.Size(448, 114);
            this.RichTxtCondition.TabIndex = 1;
            this.RichTxtCondition.Text = "";
            // 
            // btnSetCondition
            // 
            this.btnSetCondition.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetCondition.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSetCondition.Location = new System.Drawing.Point(12, 212);
            this.btnSetCondition.Name = "btnSetCondition";
            this.btnSetCondition.Size = new System.Drawing.Size(75, 23);
            this.btnSetCondition.TabIndex = 2;
            this.btnSetCondition.Text = "条件设置";
            this.btnSetCondition.Click += new System.EventHandler(this.btnSetCondition_Click);
            // 
            // btnSaveSolution
            // 
            this.btnSaveSolution.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveSolution.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveSolution.Location = new System.Drawing.Point(122, 212);
            this.btnSaveSolution.Name = "btnSaveSolution";
            this.btnSaveSolution.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSolution.TabIndex = 3;
            this.btnSaveSolution.Text = "保存方案";
            this.btnSaveSolution.Click += new System.EventHandler(this.btnSaveSolution_Click);
            // 
            // btnOpenSolution
            // 
            this.btnOpenSolution.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenSolution.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpenSolution.Location = new System.Drawing.Point(201, 212);
            this.btnOpenSolution.Name = "btnOpenSolution";
            this.btnOpenSolution.Size = new System.Drawing.Size(75, 23);
            this.btnOpenSolution.TabIndex = 4;
            this.btnOpenSolution.Text = "打开方案";
            this.btnOpenSolution.Click += new System.EventHandler(this.btnOpenSolution_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 7);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(94, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "单击选择图层：";
            // 
            // txtLayerName
            // 
            this.txtLayerName.Location = new System.Drawing.Point(12, 31);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(448, 21);
            this.txtLayerName.TabIndex = 6;
            this.txtLayerName.Click += new System.EventHandler(this.txtLayerName_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(385, 212);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(305, 212);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmDefinitionQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 251);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.txtLayerName);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnOpenSolution);
            this.Controls.Add(this.btnSaveSolution);
            this.Controls.Add(this.btnSetCondition);
            this.Controls.Add(this.RichTxtCondition);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDefinitionQuery";
            this.ShowIcon = false;
            this.Text = "自定义显示";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.RichTextBox RichTxtCondition;
        private DevComponents.DotNetBar.ButtonX btnSetCondition;
        private DevComponents.DotNetBar.ButtonX btnSaveSolution;
        private DevComponents.DotNetBar.ButtonX btnOpenSolution;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.TextBox txtLayerName;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnOK;
    }
}