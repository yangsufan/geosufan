namespace Fan.Common.Error
{
    partial class frmInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInformation));
            this.labelX = new DevExpress.XtraEditors.LabelControl();
            this.buttonXOk = new DevExpress.XtraEditors.SimpleButton();
            this.buttonXCancel = new DevExpress.XtraEditors.SimpleButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(63, 24);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(277, 66);
            this.labelX.TabIndex = 0;
            // 
            // buttonXOk
            // 
            this.buttonXOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOk.Location = new System.Drawing.Point(184, 96);
            this.buttonXOk.Name = "buttonXOk";
            this.buttonXOk.Size = new System.Drawing.Size(75, 23);
            this.buttonXOk.TabIndex = 5;
            this.buttonXOk.Click += new System.EventHandler(this.buttonXOk_Click);
            // 
            // buttonXCancel
            // 
            this.buttonXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCancel.Location = new System.Drawing.Point(265, 96);
            this.buttonXCancel.Name = "buttonXCancel";
            this.buttonXCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonXCancel.TabIndex = 6;
            this.buttonXCancel.Click += new System.EventHandler(this.buttonXCancel_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(12, 24);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(26, 36);
            this.pictureBox.TabIndex = 7;
            this.pictureBox.TabStop = false;
            // 
            // frmInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 122);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.buttonXCancel);
            this.Controls.Add(this.buttonXOk);
            this.Controls.Add(this.labelX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInformation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提示";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelX;
        private DevExpress.XtraEditors.SimpleButton buttonXOk;
        private DevExpress.XtraEditors.SimpleButton buttonXCancel;
        private System.Windows.Forms.PictureBox pictureBox;
    }
}