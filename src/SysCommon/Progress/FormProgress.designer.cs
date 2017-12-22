namespace Fan.Common.Progress
{
    partial class FormProgress
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
            this.ButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.progressBar1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.labelDesc = new DevExpress.XtraEditors.LabelControl();
            this.labelProgress = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(351, 74);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(73, 29);
            this.ButtonCancel.TabIndex = 13;
            this.ButtonCancel.Text = "取消";
            //this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1, 34);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(423, 29);
            this.progressBar1.Properties.Step = 1;
            this.progressBar1.TabIndex = 14;
            // 
            // labelDesc
            // 
            this.labelDesc.AutoSize = true;
            this.labelDesc.Location = new System.Drawing.Point(16, 11);
            this.labelDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(0, 15);
            this.labelDesc.TabIndex = 15;
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(531, 11);
            this.labelProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelProgress.Size = new System.Drawing.Size(0, 15);
            this.labelProgress.TabIndex = 16;
            //this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelProgress.Click += new System.EventHandler(this.labelProgress_Click);
            // 
            // FormProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(424, 106);
            this.ControlBox = false;
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.labelDesc);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ButtonCancel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormProgress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraEditors.SimpleButton ButtonCancel;
        public DevExpress.XtraEditors.ProgressBarControl progressBar1;
        public DevExpress.XtraEditors.LabelControl labelProgress;
        public DevExpress.XtraEditors.LabelControl labelDesc;
    }
}