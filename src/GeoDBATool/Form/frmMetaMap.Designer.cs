namespace GeoDBATool
{
    partial class frmMetaMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMetaMap));
            this.comboBoxDataSource = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnSetLabel = new DevComponents.DotNetBar.ButtonX();
            this.axMapControlR = new ESRI.ArcGIS.Controls.AxMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlR)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxDataSource
            // 
            this.comboBoxDataSource.DisplayMember = "Text";
            this.comboBoxDataSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDataSource.FormattingEnabled = true;
            this.comboBoxDataSource.ItemHeight = 15;
            this.comboBoxDataSource.Location = new System.Drawing.Point(88, 12);
            this.comboBoxDataSource.Name = "comboBoxDataSource";
            this.comboBoxDataSource.Size = new System.Drawing.Size(416, 21);
            this.comboBoxDataSource.TabIndex = 39;
            this.comboBoxDataSource.Visible = false;
            this.comboBoxDataSource.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataSource_SelectedIndexChanged);
            this.comboBoxDataSource.DropDown += new System.EventHandler(this.comboBoxDataSource_DropDown);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(18, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 23);
            this.labelX2.TabIndex = 38;
            this.labelX2.Text = "元数据库:";
            this.labelX2.Visible = false;
            // 
            // btnSetLabel
            // 
            this.btnSetLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetLabel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSetLabel.Location = new System.Drawing.Point(12, 10);
            this.btnSetLabel.Name = "btnSetLabel";
            this.btnSetLabel.Size = new System.Drawing.Size(90, 23);
            this.btnSetLabel.TabIndex = 40;
            this.btnSetLabel.Text = "设置标注字段";
            this.btnSetLabel.Click += new System.EventHandler(this.btnSetLabel_Click);
            // 
            // axMapControlR
            // 
            this.axMapControlR.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.axMapControlR.Location = new System.Drawing.Point(0, 39);
            this.axMapControlR.Name = "axMapControlR";
            this.axMapControlR.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControlR.OcxState")));
            this.axMapControlR.Size = new System.Drawing.Size(514, 369);
            this.axMapControlR.TabIndex = 0;
            // 
            // frmMetaMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 408);
            this.Controls.Add(this.axMapControlR);
            this.Controls.Add(this.btnSetLabel);
            this.Controls.Add(this.comboBoxDataSource);
            this.Controls.Add(this.labelX2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmMetaMap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "元数据地图";
            this.Load += new System.EventHandler(this.frmMetaMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControlR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDataSource;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnSetLabel;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControlR;
    }
}