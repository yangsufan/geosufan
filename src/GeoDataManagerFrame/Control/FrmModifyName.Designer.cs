namespace GeoDataManagerFrame
{
    partial class FrmModifyName
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.bttSure = new DevComponents.DotNetBar.ButtonX();
            this.bttCancel = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(96, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "成果数据名称：";
            // 
            // txtName
            // 
            // 
            // 
            // 
            this.txtName.Border.Class = "TextBoxBorder";
            this.txtName.Location = new System.Drawing.Point(101, 21);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(258, 21);
            this.txtName.TabIndex = 1;
            // 
            // bttSure
            // 
            this.bttSure.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttSure.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttSure.Location = new System.Drawing.Point(174, 61);
            this.bttSure.Name = "bttSure";
            this.bttSure.Size = new System.Drawing.Size(70, 24);
            this.bttSure.TabIndex = 10;
            this.bttSure.Text = "确定";
            this.bttSure.Click += new System.EventHandler(this.bttSure_Click);
            // 
            // bttCancel
            // 
            this.bttCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttCancel.Location = new System.Drawing.Point(289, 61);
            this.bttCancel.Name = "bttCancel";
            this.bttCancel.Size = new System.Drawing.Size(70, 24);
            this.bttCancel.TabIndex = 11;
            this.bttCancel.Text = "取消";
            this.bttCancel.Click += new System.EventHandler(this.bttCancel_Click);
            // 
            // FrmModifyName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 95);
            this.Controls.Add(this.bttCancel);
            this.Controls.Add(this.bttSure);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.labelX1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmModifyName";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改成果数据名称";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtName;
        private DevComponents.DotNetBar.ButtonX bttSure;
        private DevComponents.DotNetBar.ButtonX bttCancel;
    }
}