namespace GeoDataCenterFunLib
{
    partial class frmPolygonToTxt
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
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.textBoxY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelY = new DevComponents.DotNetBar.LabelX();
            this.labelX = new DevComponents.DotNetBar.LabelX();
            this.btn_SelShp = new DevComponents.DotNetBar.ButtonX();
            this.btn_SelTxt = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(238, 88);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(329, 88);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "退出";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxY
            // 
            // 
            // 
            // 
            this.textBoxY.Border.Class = "TextBoxBorder";
            this.textBoxY.Location = new System.Drawing.Point(76, 49);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.Size = new System.Drawing.Size(301, 21);
            this.textBoxY.TabIndex = 13;
            // 
            // textBoxX
            // 
            // 
            // 
            // 
            this.textBoxX.Border.Class = "TextBoxBorder";
            this.textBoxX.Location = new System.Drawing.Point(76, 22);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.Size = new System.Drawing.Size(301, 21);
            this.textBoxX.TabIndex = 12;
            // 
            // labelY
            // 
            this.labelY.Location = new System.Drawing.Point(12, 51);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(89, 23);
            this.labelY.TabIndex = 10;
            this.labelY.Text = "文本路径:";
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(12, 22);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(58, 23);
            this.labelX.TabIndex = 11;
            this.labelX.Text = "Shp路径:";
            // 
            // btn_SelShp
            // 
            this.btn_SelShp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelShp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelShp.Location = new System.Drawing.Point(383, 22);
            this.btn_SelShp.Name = "btn_SelShp";
            this.btn_SelShp.Size = new System.Drawing.Size(42, 23);
            this.btn_SelShp.TabIndex = 14;
            this.btn_SelShp.Text = "...";
            this.btn_SelShp.Click += new System.EventHandler(this.btn_SelShp_Click);
            // 
            // btn_SelTxt
            // 
            this.btn_SelTxt.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelTxt.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelTxt.Location = new System.Drawing.Point(383, 47);
            this.btn_SelTxt.Name = "btn_SelTxt";
            this.btn_SelTxt.Size = new System.Drawing.Size(42, 23);
            this.btn_SelTxt.TabIndex = 14;
            this.btn_SelTxt.Text = "...";
            this.btn_SelTxt.Click += new System.EventHandler(this.btn_SelTxt_Click);
            // 
            // frmPolygonToTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 120);
            this.Controls.Add(this.btn_SelTxt);
            this.Controls.Add(this.btn_SelShp);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPolygonToTxt";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shp导出坐标串";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxY;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxX;
        private DevComponents.DotNetBar.LabelX labelY;
        private DevComponents.DotNetBar.LabelX labelX;
        private DevComponents.DotNetBar.ButtonX btn_SelShp;
        private DevComponents.DotNetBar.ButtonX btn_SelTxt;
    }
}