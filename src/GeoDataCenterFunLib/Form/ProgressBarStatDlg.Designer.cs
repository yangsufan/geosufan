namespace GeoDataCenterFunLib
{
    partial class ProgressBarStatDlg
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
            this.components = new System.ComponentModel.Container();
            this.labelPro = new System.Windows.Forms.Label();
            this.Mytimer = new System.Windows.Forms.Timer(this.components);
            this.myprogressBar = new GeoDataCenterFunLib.MyprogressBar();
            this.SuspendLayout();
            // 
            // labelPro
            // 
            this.labelPro.AutoSize = true;
            this.labelPro.Location = new System.Drawing.Point(12, 9);
            this.labelPro.Name = "labelPro";
            this.labelPro.Size = new System.Drawing.Size(0, 12);
            this.labelPro.TabIndex = 2;
            // 
            // Mytimer
            // 
            this.Mytimer.Tick += new System.EventHandler(this.Mytimer_Tick);
            // 
            // myprogressBar
            // 
            this.myprogressBar.BackColor = System.Drawing.Color.Transparent;
            this.myprogressBar.EndColor = System.Drawing.Color.ForestGreen;
            this.myprogressBar.Location = new System.Drawing.Point(2, 24);
            this.myprogressBar.Name = "myprogressBar";
            this.myprogressBar.Size = new System.Drawing.Size(264, 25);
            this.myprogressBar.StartColor = System.Drawing.Color.Red;
            this.myprogressBar.TabIndex = 1;
            // 
            // ProgressBarStatDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(267, 55);
            this.Controls.Add(this.labelPro);
            this.Controls.Add(this.myprogressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressBarStatDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProgressBarStatDlg";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProgressBarStatDlg_FormClosed);
            this.Load += new System.EventHandler(this.ProgressBarStatDlg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MyprogressBar myprogressBar;
        private System.Windows.Forms.Label labelPro;
        private System.Windows.Forms.Timer Mytimer;
    }
}