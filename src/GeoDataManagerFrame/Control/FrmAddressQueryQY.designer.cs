namespace GeoDataManagerFrame
{
    partial class FrmAddressQueryQY
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
            this.listViewD = new System.Windows.Forms.ListView();
            this.btnQuery = new DevComponents.DotNetBar.ButtonX();
            this.txtDM = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.dataGridViewR = new System.Windows.Forms.DataGridView();
            this.btnExportExcel = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewR)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewD
            // 
            this.listViewD.Location = new System.Drawing.Point(47, 123);
            this.listViewD.Name = "listViewD";
            this.listViewD.Size = new System.Drawing.Size(248, 88);
            this.listViewD.TabIndex = 4;
            this.listViewD.UseCompatibleStateImageBehavior = false;
            this.listViewD.Visible = false;
            // 
            // btnQuery
            // 
            this.btnQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuery.Location = new System.Drawing.Point(372, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 11;
            this.btnQuery.Text = "搜索";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // txtDM
            // 
            // 
            // 
            // 
            this.txtDM.Border.Class = "TextBoxBorder";
            this.txtDM.Location = new System.Drawing.Point(1, 12);
            this.txtDM.Name = "txtDM";
            this.txtDM.Size = new System.Drawing.Size(365, 21);
            this.txtDM.TabIndex = 19;
            this.txtDM.TextChanged += new System.EventHandler(this.txtDM_TextChanged);
            // 
            // dataGridViewR
            // 
            this.dataGridViewR.AllowUserToAddRows = false;
            this.dataGridViewR.AllowUserToDeleteRows = false;
            this.dataGridViewR.AllowUserToOrderColumns = true;
            this.dataGridViewR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewR.Location = new System.Drawing.Point(1, 39);
            this.dataGridViewR.Name = "dataGridViewR";
            this.dataGridViewR.ReadOnly = true;
            this.dataGridViewR.RowTemplate.Height = 23;
            this.dataGridViewR.Size = new System.Drawing.Size(365, 172);
            this.dataGridViewR.TabIndex = 21;
            this.dataGridViewR.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewR_RowHeaderMouseDoubleClick);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportExcel.Location = new System.Drawing.Point(372, 41);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(75, 23);
            this.btnExportExcel.TabIndex = 23;
            this.btnExportExcel.Text = "导出为Exel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // FrmAddressQueryQY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 219);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.dataGridViewR);
            this.Controls.Add(this.txtDM);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.listViewD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddressQueryQY";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "区域地名地址查询对话框";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewD;
        private DevComponents.DotNetBar.ButtonX btnQuery;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDM;
        private System.Windows.Forms.DataGridView dataGridViewR;
        private DevComponents.DotNetBar.ButtonX btnExportExcel;
    }
}