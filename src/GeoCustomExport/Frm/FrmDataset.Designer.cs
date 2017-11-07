namespace GeoCustomExport
{
    partial class FrmDataset
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
            this.lstFeaDataset = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.colText = new System.Windows.Forms.ColumnHeader();
            this.btn_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.btn_OK = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // lstFeaDataset
            // 
            // 
            // 
            // 
            this.lstFeaDataset.Border.Class = "ListViewBorder";
            this.lstFeaDataset.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstFeaDataset.CheckBoxes = true;
            this.lstFeaDataset.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colText});
            this.lstFeaDataset.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstFeaDataset.FullRowSelect = true;
            this.lstFeaDataset.Location = new System.Drawing.Point(0, 0);
            this.lstFeaDataset.MultiSelect = false;
            this.lstFeaDataset.Name = "lstFeaDataset";
            this.lstFeaDataset.Size = new System.Drawing.Size(146, 229);
            this.lstFeaDataset.TabIndex = 3;
            this.lstFeaDataset.UseCompatibleStateImageBehavior = false;
            this.lstFeaDataset.View = System.Windows.Forms.View.Details;
            // 
            // colText
            // 
            this.colText.Text = "数据集名称";
            this.colText.Width = 150;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Cancel.Location = new System.Drawing.Point(152, 52);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(51, 23);
            this.btn_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_OK.Location = new System.Drawing.Point(152, 12);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(51, 23);
            this.btn_OK.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // FrmDataset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 229);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.lstFeaDataset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDataset";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据集";
            this.Load += new System.EventHandler(this.FrmDataset_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx lstFeaDataset;
        private System.Windows.Forms.ColumnHeader colText;
        private DevComponents.DotNetBar.ButtonX btn_Cancel;
        private DevComponents.DotNetBar.ButtonX btn_OK;

    }
}