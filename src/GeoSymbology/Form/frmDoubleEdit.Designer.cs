namespace GeoSymbology.Form
{
    partial class frmDoubleEdit
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
            this.doubleInput = new DevComponents.Editors.DoubleInput();
            ((System.ComponentModel.ISupportInitialize)(this.doubleInput)).BeginInit();
            this.SuspendLayout();
            // 
            // doubleInput
            // 
            // 
            // 
            // 
            this.doubleInput.BackgroundStyle.Class = "DateTimeInputBackground";
            this.doubleInput.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.doubleInput.DisplayFormat = ".####";
            this.doubleInput.Increment = 1;
            this.doubleInput.Location = new System.Drawing.Point(0, 3);
            this.doubleInput.Margin = new System.Windows.Forms.Padding(0);
            this.doubleInput.Name = "doubleInput";
            this.doubleInput.Size = new System.Drawing.Size(150, 21);
            this.doubleInput.TabIndex = 0;
            this.doubleInput.KeyUp += new System.Windows.Forms.KeyEventHandler(this.doubleInput_KeyUp);
            // 
            // frmDoubleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(150, 27);
            this.Controls.Add(this.doubleInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDoubleEdit";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.doubleInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.Editors.DoubleInput doubleInput;
    }
}