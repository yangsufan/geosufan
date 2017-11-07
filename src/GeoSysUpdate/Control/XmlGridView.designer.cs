namespace GeoSysUpdate
{
    partial class XmlGridView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webXmlView = new System.Windows.Forms.WebBrowser();
            this.grdTableView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdTableView)).BeginInit();
            this.SuspendLayout();
            // 
            // webXmlView
            // 
            this.webXmlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webXmlView.Location = new System.Drawing.Point(0, 0);
            this.webXmlView.MinimumSize = new System.Drawing.Size(20, 20);
            this.webXmlView.Name = "webXmlView";
            this.webXmlView.Size = new System.Drawing.Size(315, 310);
            this.webXmlView.TabIndex = 0;
            // 
            // grdTableView
            // 
            this.grdTableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTableView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTableView.Location = new System.Drawing.Point(0, 0);
            this.grdTableView.Name = "grdTableView";
            this.grdTableView.Size = new System.Drawing.Size(315, 310);
            this.grdTableView.TabIndex = 1;
            // 
            // XmlGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdTableView);
            this.Controls.Add(this.webXmlView);
            this.Name = "XmlGridView";
            this.Size = new System.Drawing.Size(315, 310);
            ((System.ComponentModel.ISupportInitialize)(this.grdTableView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webXmlView;
        private System.Windows.Forms.DataGridView grdTableView;
    }
}
