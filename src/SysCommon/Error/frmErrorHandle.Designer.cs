namespace SysCommon.Error
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
            this.buttonX = new DevComponents.DotNetBar.ButtonX();
            this.labelX = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(12, 30);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(26, 36);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // buttonX
            // 
            this.buttonX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX.Location = new System.Drawing.Point(262, 87);
            this.buttonX.Name = "buttonX";
            this.buttonX.Size = new System.Drawing.Size(75, 23);
            this.buttonX.TabIndex = 4;
            this.buttonX.Text = "返 回";
            this.buttonX.Click += new System.EventHandler(this.buttonX_Click);
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(44, 18);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(293, 63);
            this.labelX.TabIndex = 3;
            this.labelX.WordWrap = true;
            // 
            // frmErrorHandle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 122);
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

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private DevComponents.DotNetBar.ButtonX buttonX;
        private DevComponents.DotNetBar.LabelX labelX;
    }
}