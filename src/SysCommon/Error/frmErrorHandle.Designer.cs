namespace Fan.Common.Error
{
    partial class frmErrorHandle
    {
        /// <summary>
        /// 必需的设计器变量。

        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。

        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。

        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorHandle));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.buttonX = new DevExpress.XtraEditors.SimpleButton();
            this.labelX = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(14, 35);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(30, 42);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // buttonX
            // 
            this.buttonX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX.Location = new System.Drawing.Point(306, 101);
            this.buttonX.Name = "buttonX";
            this.buttonX.Size = new System.Drawing.Size(87, 27);
            this.buttonX.TabIndex = 4;
            this.buttonX.Text = "返 回";
            this.buttonX.Click += new System.EventHandler(this.buttonX_Click);
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(51, 21);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(0, 14);
            this.labelX.TabIndex = 3;
            // 
            // frmErrorHandle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 142);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.buttonX);
            this.Controls.Add(this.labelX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmErrorHandle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private DevExpress.XtraEditors.SimpleButton buttonX;
        private DevExpress.XtraEditors.LabelControl labelX;
    }
}