namespace GeoDataManagerFrame
{
    partial class FrmResultDataManage
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
            this.UcResultDataManager = new GeoDataManagerFrame.UCResultDataManager();
            this.SuspendLayout();
            // 
            // UcResultDataManager
            // 
            this.UcResultDataManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UcResultDataManager.Location = new System.Drawing.Point(0, 0);
            this.UcResultDataManager.Name = "UcResultDataManager";
            this.UcResultDataManager.Size = new System.Drawing.Size(612, 486);
            this.UcResultDataManager.TabIndex = 0;
            // 
            // FrmResultDataManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 486);
            this.Controls.Add(this.UcResultDataManager);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmResultDataManage";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "成果数据管理";
            this.ResumeLayout(false);

        }

        #endregion

        private UCResultDataManager UcResultDataManager;

    }
}