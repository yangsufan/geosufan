namespace GeoDataCenterFunLib
{
    partial class FormFlexcell
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFlexcell));
            this.buttonXSave = new DevComponents.DotNetBar.ButtonX();
            this.buttonXSaveAs = new DevComponents.DotNetBar.ButtonX();
            this.buttonXtoExcel = new DevComponents.DotNetBar.ButtonX();
            this.axGrid1 = new AxFlexCell.AxGrid();
            ((System.ComponentModel.ISupportInitialize)(this.axGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonXSave
            // 
            this.buttonXSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXSave.Location = new System.Drawing.Point(14, 11);
            this.buttonXSave.Name = "buttonXSave";
            this.buttonXSave.Size = new System.Drawing.Size(90, 21);
            this.buttonXSave.TabIndex = 1;
            this.buttonXSave.Text = "保存";
            this.buttonXSave.Click += new System.EventHandler(this.buttonXSave_Click);
            // 
            // buttonXSaveAs
            // 
            this.buttonXSaveAs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXSaveAs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXSaveAs.Location = new System.Drawing.Point(120, 11);
            this.buttonXSaveAs.Name = "buttonXSaveAs";
            this.buttonXSaveAs.Size = new System.Drawing.Size(90, 21);
            this.buttonXSaveAs.TabIndex = 2;
            this.buttonXSaveAs.Text = "另存为";
            this.buttonXSaveAs.Click += new System.EventHandler(this.buttonXSaveAs_Click);
            // 
            // buttonXtoExcel
            // 
            this.buttonXtoExcel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXtoExcel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXtoExcel.Location = new System.Drawing.Point(228, 11);
            this.buttonXtoExcel.Name = "buttonXtoExcel";
            this.buttonXtoExcel.Size = new System.Drawing.Size(91, 21);
            this.buttonXtoExcel.TabIndex = 3;
            this.buttonXtoExcel.Text = "导出Excel文件";
            this.buttonXtoExcel.Click += new System.EventHandler(this.buttonXtoExcel_Click);
            // 
            // axGrid1
            // 
            this.axGrid1.Enabled = true;
            this.axGrid1.Location = new System.Drawing.Point(17, 47);
            this.axGrid1.Name = "axGrid1";
            this.axGrid1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGrid1.OcxState")));
            this.axGrid1.Size = new System.Drawing.Size(302, 212);
            this.axGrid1.TabIndex = 0;
            this.axGrid1.GetCellText += new AxFlexCell.@__Grid_GetCellTextEventHandler(this.axGrid1_GetCellText);
            this.axGrid1.SetCellText += new AxFlexCell.@__Grid_SetCellTextEventHandler(this.axGrid1_SetCellText);
            // 
            // FormFlexcell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 281);
            this.Controls.Add(this.buttonXtoExcel);
            this.Controls.Add(this.buttonXSaveAs);
            this.Controls.Add(this.buttonXSave);
            this.Controls.Add(this.axGrid1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormFlexcell";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XtraForm1";
            this.Load += new System.EventHandler(this.FormFlexcell_Load);
            this.Resize += new System.EventHandler(this.FormFlexcell_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.axGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxFlexCell.AxGrid axGrid1;
        private DevComponents.DotNetBar.ButtonX buttonXSave;
        private DevComponents.DotNetBar.ButtonX buttonXSaveAs;
        private DevComponents.DotNetBar.ButtonX buttonXtoExcel;
    }
}