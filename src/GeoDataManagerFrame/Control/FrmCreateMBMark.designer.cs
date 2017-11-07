namespace GeoDataManagerFrame
{
    partial class FrmCreateMBMark
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
            this.txtMBMName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnXOK = new DevComponents.DotNetBar.ButtonX();
            this.btnXCancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // txtMBMName
            // 
            // 
            // 
            // 
            this.txtMBMName.Border.Class = "TextBoxBorder";
            this.txtMBMName.Location = new System.Drawing.Point(105, 12);
            this.txtMBMName.Name = "txtMBMName";
            this.txtMBMName.Size = new System.Drawing.Size(178, 21);
            this.txtMBMName.TabIndex = 0;
            this.txtMBMName.TextChanged += new System.EventHandler(this.txtMBMName_TextChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(1, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(98, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "地图书签名称：";
            // 
            // btnXOK
            // 
            this.btnXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXOK.Location = new System.Drawing.Point(123, 52);
            this.btnXOK.Name = "btnXOK";
            this.btnXOK.Size = new System.Drawing.Size(75, 23);
            this.btnXOK.TabIndex = 2;
            this.btnXOK.Text = "确定";
            this.btnXOK.Click += new System.EventHandler(this.btnXOK_Click);
            // 
            // btnXCancel
            // 
            this.btnXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnXCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnXCancel.Location = new System.Drawing.Point(208, 52);
            this.btnXCancel.Name = "btnXCancel";
            this.btnXCancel.Size = new System.Drawing.Size(75, 23);
            this.btnXCancel.TabIndex = 0;
            this.btnXCancel.Text = "取消";
            this.btnXCancel.Click += new System.EventHandler(this.btnXCancel_Click);
            // 
            // FrmCreateMBMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 87);
            this.Controls.Add(this.btnXCancel);
            this.Controls.Add(this.btnXOK);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.txtMBMName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCreateMBMark";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建地图书签";
            this.Load += new System.EventHandler(this.FrmCreateMBMark_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmCreateMBMark_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtMBMName;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnXOK;
        private DevComponents.DotNetBar.ButtonX btnXCancel;
    }
}