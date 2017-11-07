namespace SysCommon.Error
{
    partial class frmErrorHandleEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmErrorHandleEx));
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.buttonX = new DevComponents.DotNetBar.ButtonX();
            this.labelX = new DevComponents.DotNetBar.LabelX();
            this.listErrorInfo = new DevComponents.DotNetBar.Controls.ListViewEx();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(19, 9);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(26, 36);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // buttonX
            // 
            this.buttonX.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX.Location = new System.Drawing.Point(276, 207);
            this.buttonX.Name = "buttonX";
            this.buttonX.Size = new System.Drawing.Size(75, 23);
            this.buttonX.TabIndex = 4;
            this.buttonX.Text = "返 回";
            this.buttonX.Click += new System.EventHandler(this.buttonX_Click);
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(49, 12);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(293, 29);
            this.labelX.TabIndex = 3;
            this.labelX.WordWrap = true;
            // 
            // listErrorInfo
            // 
            this.listErrorInfo.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            // 
            // 
            // 
            this.listErrorInfo.Border.Class = "ListViewBorder";
            this.listErrorInfo.Location = new System.Drawing.Point(19, 53);
            this.listErrorInfo.Name = "listErrorInfo";
            this.listErrorInfo.Size = new System.Drawing.Size(333, 143);
            this.listErrorInfo.TabIndex = 6;
            this.listErrorInfo.UseCompatibleStateImageBehavior = false;
            this.listErrorInfo.View = System.Windows.Forms.View.List;
            // 
            // frmErrorHandleEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 240);
            this.Controls.Add(this.listErrorInfo);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.buttonX);
            this.Controls.Add(this.labelX);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmErrorHandleEx";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private DevComponents.DotNetBar.ButtonX buttonX;
        private DevComponents.DotNetBar.LabelX labelX;
        private DevComponents.DotNetBar.Controls.ListViewEx listErrorInfo;
    }
}