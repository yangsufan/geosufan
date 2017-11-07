namespace GeoSysUpdate
{
    partial class frmUploadingList
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
            this.listlog = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listlog
            // 
            this.listlog.BackColor = System.Drawing.SystemColors.Control;
            this.listlog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listlog.FormattingEnabled = true;
            this.listlog.ItemHeight = 12;
            this.listlog.Location = new System.Drawing.Point(0, 0);
            this.listlog.Name = "listlog";
            this.listlog.Size = new System.Drawing.Size(243, 160);
            this.listlog.TabIndex = 1;
            // 
            // frmUploadingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 162);
            this.Controls.Add(this.listlog);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUploadingList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传成果文件报告";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listlog;

    }
}