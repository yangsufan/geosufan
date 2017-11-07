namespace GeoPageLayout
{
    partial class FrmZDMapSet
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
            this.label1 = new DevComponents.DotNetBar.LabelX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.rTxtTitle = new System.Windows.Forms.RichTextBox();
            this.rTxtLeftLower = new System.Windows.Forms.RichTextBox();
            this.rTxtRightLower = new System.Windows.Forms.RichTextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtCartoGroup = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(216, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "1:";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(406, 328);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(487, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtCartoGroup);
            this.groupPanel1.Controls.Add(this.txtScale);
            this.groupPanel1.Controls.Add(this.rTxtTitle);
            this.groupPanel1.Controls.Add(this.rTxtLeftLower);
            this.groupPanel1.Controls.Add(this.rTxtRightLower);
            this.groupPanel1.Controls.Add(this.pictureBox2);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(562, 322);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 28;
            this.groupPanel1.Text = "地图信息";
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(241, 254);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(100, 21);
            this.txtScale.TabIndex = 40;
            this.txtScale.Text = "500";
            // 
            // rTxtTitle
            // 
            this.rTxtTitle.Location = new System.Drawing.Point(197, 3);
            this.rTxtTitle.Name = "rTxtTitle";
            this.rTxtTitle.Size = new System.Drawing.Size(185, 22);
            this.rTxtTitle.TabIndex = 39;
            this.rTxtTitle.Text = "宗地图";
            // 
            // rTxtLeftLower
            // 
            this.rTxtLeftLower.Location = new System.Drawing.Point(23, 254);
            this.rTxtLeftLower.Name = "rTxtLeftLower";
            this.rTxtLeftLower.Size = new System.Drawing.Size(182, 39);
            this.rTxtLeftLower.TabIndex = 37;
            this.rTxtLeftLower.Text = "制图日期：\n审核日期：             ";
            this.rTxtLeftLower.TextChanged += new System.EventHandler(this.rTxtRightLower_TextChanged);
            // 
            // rTxtRightLower
            // 
            this.rTxtRightLower.Location = new System.Drawing.Point(390, 254);
            this.rTxtRightLower.Name = "rTxtRightLower";
            this.rTxtRightLower.Size = new System.Drawing.Size(163, 39);
            this.rTxtRightLower.TabIndex = 36;
            this.rTxtRightLower.Text = "制图人：\n审核人：";
            this.rTxtRightLower.TextChanged += new System.EventHandler(this.rTxtLeftLower_TextChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GeoPageLayout.Properties.Resources.GeoPageLayout_gzdt;
            this.pictureBox2.Location = new System.Drawing.Point(23, 31);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(531, 217);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // txtCartoGroup
            // 
            // 
            // 
            // 
            this.txtCartoGroup.Border.Class = "TextBoxBorder";
            this.txtCartoGroup.Location = new System.Drawing.Point(-1, 139);
            this.txtCartoGroup.Multiline = true;
            this.txtCartoGroup.Name = "txtCartoGroup";
            this.txtCartoGroup.Size = new System.Drawing.Size(18, 109);
            this.txtCartoGroup.TabIndex = 41;
            this.txtCartoGroup.Text = "XX市林业资源局";
            // 
            // FrmZDMapSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 358);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmZDMapSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "宗地图";
            this.Load += new System.EventHandler(this.FrmZDMapSet_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX label1;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.RichTextBox rTxtLeftLower;
        private System.Windows.Forms.RichTextBox rTxtRightLower;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.RichTextBox rTxtTitle;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCartoGroup;
    }
}