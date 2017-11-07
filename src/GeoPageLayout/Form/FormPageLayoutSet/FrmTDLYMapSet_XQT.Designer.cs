namespace GeoPageLayout
{
    partial class FrmTDLYMapSet_XQT
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
            this.cBoxScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.Item10000 = new DevComponents.Editors.ComboItem();
            this.Item25000 = new DevComponents.Editors.ComboItem();
            this.Item50000 = new DevComponents.Editors.ComboItem();
            this.txtTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.checkBoxTuli = new System.Windows.Forms.CheckBox();
            this.txtCartoGroup = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cBoxSecret = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.rTxtLeftLower = new System.Windows.Forms.RichTextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.chkBoxSecurity = new System.Windows.Forms.CheckBox();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(239, 256);
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
            this.btnOK.Location = new System.Drawing.Point(427, 361);
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
            this.btnCancel.Location = new System.Drawing.Point(508, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cBoxScale
            // 
            this.cBoxScale.DisplayMember = "Text";
            this.cBoxScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxScale.FormattingEnabled = true;
            this.cBoxScale.ItemHeight = 15;
            this.cBoxScale.Items.AddRange(new object[] {
            this.Item10000,
            this.Item25000,
            this.Item50000});
            this.cBoxScale.Location = new System.Drawing.Point(262, 254);
            this.cBoxScale.Name = "cBoxScale";
            this.cBoxScale.Size = new System.Drawing.Size(121, 21);
            this.cBoxScale.TabIndex = 4;
            this.cBoxScale.Text = "10000";
            this.cBoxScale.SelectedIndexChanged += new System.EventHandler(this.cBoxScale_SelectedIndexChanged);
            // 
            // Item10000
            // 
            this.Item10000.Text = "10000";
            // 
            // Item25000
            // 
            this.Item25000.Text = "25000";
            // 
            // Item50000
            // 
            this.Item50000.Text = "50000";
            // 
            // txtTitle
            // 
            // 
            // 
            // 
            this.txtTitle.Border.Class = "TextBoxBorder";
            this.txtTitle.Location = new System.Drawing.Point(184, 2);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(210, 21);
            this.txtTitle.TabIndex = 7;
            this.txtTitle.Text = "森林资源现状辖区图";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.chkBoxSecurity);
            this.groupPanel1.Controls.Add(this.checkBoxTuli);
            this.groupPanel1.Controls.Add(this.txtCartoGroup);
            this.groupPanel1.Controls.Add(this.cBoxSecret);
            this.groupPanel1.Controls.Add(this.rTxtLeftLower);
            this.groupPanel1.Controls.Add(this.pictureBox2);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.cBoxScale);
            this.groupPanel1.Controls.Add(this.txtTitle);
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(583, 355);
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
            // checkBoxTuli
            // 
            this.checkBoxTuli.AutoSize = true;
            this.checkBoxTuli.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxTuli.Checked = true;
            this.checkBoxTuli.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTuli.Location = new System.Drawing.Point(502, 259);
            this.checkBoxTuli.Name = "checkBoxTuli";
            this.checkBoxTuli.Size = new System.Drawing.Size(72, 16);
            this.checkBoxTuli.TabIndex = 44;
            this.checkBoxTuli.Text = "生成图例";
            this.checkBoxTuli.UseVisualStyleBackColor = false;
            // 
            // txtCartoGroup
            // 
            // 
            // 
            // 
            this.txtCartoGroup.Border.Class = "TextBoxBorder";
            this.txtCartoGroup.Location = new System.Drawing.Point(3, 141);
            this.txtCartoGroup.Multiline = true;
            this.txtCartoGroup.Name = "txtCartoGroup";
            this.txtCartoGroup.Size = new System.Drawing.Size(18, 107);
            this.txtCartoGroup.TabIndex = 39;
            this.txtCartoGroup.Text = "XX市林业资源局";
            // 
            // cBoxSecret
            // 
            this.cBoxSecret.DisplayMember = "Text";
            this.cBoxSecret.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxSecret.FormattingEnabled = true;
            this.cBoxSecret.ItemHeight = 15;
            this.cBoxSecret.Items.AddRange(new object[] {
            this.comboItem9,
            this.comboItem10,
            this.comboItem11,
            this.comboItem1});
            this.cBoxSecret.Location = new System.Drawing.Point(494, 3);
            this.cBoxSecret.Name = "cBoxSecret";
            this.cBoxSecret.Size = new System.Drawing.Size(80, 21);
            this.cBoxSecret.TabIndex = 38;
            this.cBoxSecret.Text = "内部用图";
            // 
            // comboItem9
            // 
            this.comboItem9.Text = "秘密";
            // 
            // comboItem10
            // 
            this.comboItem10.Text = "机密";
            // 
            // comboItem11
            // 
            this.comboItem11.Text = "绝密";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "内部用图";
            // 
            // rTxtLeftLower
            // 
            this.rTxtLeftLower.Location = new System.Drawing.Point(27, 254);
            this.rTxtLeftLower.Name = "rTxtLeftLower";
            this.rTxtLeftLower.Size = new System.Drawing.Size(163, 66);
            this.rTxtLeftLower.TabIndex = 36;
            this.rTxtLeftLower.Text = "2009年5月XXX测图\n1980年西安坐标系\n1985年国家高程基准";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GeoPageLayout.Properties.Resources.GeoPageLayout_gzdt;
            this.pictureBox2.Location = new System.Drawing.Point(27, 30);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(547, 218);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // chkBoxSecurity
            // 
            this.chkBoxSecurity.AutoSize = true;
            this.chkBoxSecurity.BackColor = System.Drawing.Color.Transparent;
            this.chkBoxSecurity.Checked = true;
            this.chkBoxSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxSecurity.Location = new System.Drawing.Point(440, 8);
            this.chkBoxSecurity.Name = "chkBoxSecurity";
            this.chkBoxSecurity.Size = new System.Drawing.Size(48, 16);
            this.chkBoxSecurity.TabIndex = 46;
            this.chkBoxSecurity.Text = "密级";
            this.chkBoxSecurity.UseVisualStyleBackColor = false;
            this.chkBoxSecurity.CheckedChanged += new System.EventHandler(this.chkBoxSecurity_CheckedChanged);
            // 
            // FrmTDLYMapSet_XQT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 386);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTDLYMapSet_XQT";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "森林资源现状辖区图";
            this.Load += new System.EventHandler(this.FrmTDLYMapSet_XQT_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX label1;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxScale;
        private DevComponents.Editors.ComboItem Item10000;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTitle;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private DevComponents.Editors.ComboItem Item25000;
        private DevComponents.Editors.ComboItem Item50000;
        private System.Windows.Forms.RichTextBox rTxtLeftLower;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxSecret;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem10;
        private DevComponents.Editors.ComboItem comboItem11;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCartoGroup;
        private System.Windows.Forms.CheckBox checkBoxTuli;
        private System.Windows.Forms.CheckBox chkBoxSecurity;
    }
}