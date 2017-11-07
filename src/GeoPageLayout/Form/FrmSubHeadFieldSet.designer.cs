namespace GeoPageLayout
{
    partial class FrmSubHeadFieldSet
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
            this.btnXCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnXOK = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cboFields = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // btnXCancel
            // 
            this.btnXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXCancel.Location = new System.Drawing.Point(164, 57);
            this.btnXCancel.Name = "btnXCancel";
            this.btnXCancel.Size = new System.Drawing.Size(50, 23);
            this.btnXCancel.TabIndex = 4;
            this.btnXCancel.Text = "取消";
            this.btnXCancel.Click += new System.EventHandler(this.btnXCancel_Click);
            // 
            // btnXOK
            // 
            this.btnXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnXOK.Location = new System.Drawing.Point(108, 57);
            this.btnXOK.Name = "btnXOK";
            this.btnXOK.Size = new System.Drawing.Size(50, 23);
            this.btnXOK.TabIndex = 6;
            this.btnXOK.Text = "确定";
            this.btnXOK.Click += new System.EventHandler(this.btnXOK_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(8, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(113, 23);
            this.labelX1.TabIndex = 5;
            this.labelX1.Text = "选择字段:";
            // 
            // cboFields
            // 
            this.cboFields.DisplayMember = "Text";
            this.cboFields.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFields.FormattingEnabled = true;
            this.cboFields.ItemHeight = 15;
            this.cboFields.Location = new System.Drawing.Point(8, 30);
            this.cboFields.Name = "cboFields";
            this.cboFields.Size = new System.Drawing.Size(206, 21);
            this.cboFields.TabIndex = 0;
            this.cboFields.TextChanged += new System.EventHandler(this.cboLayers_TextChanged);
            // 
            // FrmSubHeadFieldSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(218, 88);
            this.Controls.Add(this.cboFields);
            this.Controls.Add(this.btnXCancel);
            this.Controls.Add(this.btnXOK);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSubHeadFieldSet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmEditLayerSet_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnXCancel;
        private DevComponents.DotNetBar.ButtonX btnXOK;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboFields;
    }
}