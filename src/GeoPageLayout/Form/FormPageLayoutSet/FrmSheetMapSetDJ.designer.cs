namespace GeoPageLayout
{
    partial class FrmSheetMapSetDJ
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
            this.Item500 = new DevComponents.Editors.ComboItem();
            this.Item1000 = new DevComponents.Editors.ComboItem();
            this.Item2000 = new DevComponents.Editors.ComboItem();
            this.txtMapNO = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cBoxSecret = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.txtJTBWN = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBN = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBEN = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBE = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBWS = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBS = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBES = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBW = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtVersionYear = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtCartoGroup = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chkBoxSecurity = new System.Windows.Forms.CheckBox();
            this.rTxtRightLower = new System.Windows.Forms.RichTextBox();
            this.rTxtLeftLower = new System.Windows.Forms.RichTextBox();
            this.checkBoxTuli = new System.Windows.Forms.CheckBox();
            this.txtGWY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtGWX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(304, 280);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "1:";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(480, 421);
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
            this.btnCancel.Location = new System.Drawing.Point(561, 421);
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
            this.cBoxScale.Enabled = false;
            this.cBoxScale.FormattingEnabled = true;
            this.cBoxScale.ItemHeight = 15;
            this.cBoxScale.Items.AddRange(new object[] {
            this.Item500,
            this.Item1000,
            this.Item2000});
            this.cBoxScale.Location = new System.Drawing.Point(329, 280);
            this.cBoxScale.Name = "cBoxScale";
            this.cBoxScale.Size = new System.Drawing.Size(71, 21);
            this.cBoxScale.TabIndex = 4;
            this.cBoxScale.Text = "500";
            this.cBoxScale.SelectedIndexChanged += new System.EventHandler(this.cBoxScale_SelectedIndexChanged);
            // 
            // Item500
            // 
            this.Item500.Text = "500";
            // 
            // Item1000
            // 
            this.Item1000.Text = "1000";
            // 
            // Item2000
            // 
            this.Item2000.Text = "2000";
            // 
            // txtMapNO
            // 
            // 
            // 
            // 
            this.txtMapNO.Border.Class = "TextBoxBorder";
            this.txtMapNO.Location = new System.Drawing.Point(317, 30);
            this.txtMapNO.Name = "txtMapNO";
            this.txtMapNO.Size = new System.Drawing.Size(142, 21);
            this.txtMapNO.TabIndex = 2;
            this.txtMapNO.Text = "20.00-30.00";
            this.txtMapNO.TextChanged += new System.EventHandler(this.txtMapNO_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(268, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "图幅号:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(268, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "图  名:";
            // 
            // txtTitle
            // 
            // 
            // 
            // 
            this.txtTitle.Border.Class = "TextBoxBorder";
            this.txtTitle.Location = new System.Drawing.Point(317, 3);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(142, 21);
            this.txtTitle.TabIndex = 7;
            this.txtTitle.Text = "XX市标准分幅地籍图";
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
            this.comboItem11});
            this.cBoxSecret.Location = new System.Drawing.Point(509, 30);
            this.cBoxSecret.Name = "cBoxSecret";
            this.cBoxSecret.Size = new System.Drawing.Size(65, 21);
            this.cBoxSecret.TabIndex = 11;
            this.cBoxSecret.Text = "秘密";
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
            // txtJTBWN
            // 
            // 
            // 
            // 
            this.txtJTBWN.Border.Class = "TextBoxBorder";
            this.txtJTBWN.Enabled = false;
            this.txtJTBWN.Location = new System.Drawing.Point(40, -2);
            this.txtJTBWN.Name = "txtJTBWN";
            this.txtJTBWN.Size = new System.Drawing.Size(73, 21);
            this.txtJTBWN.TabIndex = 12;
            // 
            // txtJTBN
            // 
            // 
            // 
            // 
            this.txtJTBN.Border.Class = "TextBoxBorder";
            this.txtJTBN.Enabled = false;
            this.txtJTBN.Location = new System.Drawing.Point(113, -2);
            this.txtJTBN.Name = "txtJTBN";
            this.txtJTBN.Size = new System.Drawing.Size(73, 21);
            this.txtJTBN.TabIndex = 13;
            // 
            // txtJTBEN
            // 
            // 
            // 
            // 
            this.txtJTBEN.Border.Class = "TextBoxBorder";
            this.txtJTBEN.Enabled = false;
            this.txtJTBEN.Location = new System.Drawing.Point(186, -2);
            this.txtJTBEN.Name = "txtJTBEN";
            this.txtJTBEN.Size = new System.Drawing.Size(73, 21);
            this.txtJTBEN.TabIndex = 14;
            // 
            // txtJTBE
            // 
            // 
            // 
            // 
            this.txtJTBE.Border.Class = "TextBoxBorder";
            this.txtJTBE.Enabled = false;
            this.txtJTBE.Location = new System.Drawing.Point(186, 19);
            this.txtJTBE.Name = "txtJTBE";
            this.txtJTBE.Size = new System.Drawing.Size(73, 21);
            this.txtJTBE.TabIndex = 15;
            // 
            // txtJTBWS
            // 
            // 
            // 
            // 
            this.txtJTBWS.Border.Class = "TextBoxBorder";
            this.txtJTBWS.Enabled = false;
            this.txtJTBWS.Location = new System.Drawing.Point(40, 40);
            this.txtJTBWS.Name = "txtJTBWS";
            this.txtJTBWS.Size = new System.Drawing.Size(73, 21);
            this.txtJTBWS.TabIndex = 16;
            // 
            // txtJTBS
            // 
            // 
            // 
            // 
            this.txtJTBS.Border.Class = "TextBoxBorder";
            this.txtJTBS.Enabled = false;
            this.txtJTBS.Location = new System.Drawing.Point(113, 40);
            this.txtJTBS.Name = "txtJTBS";
            this.txtJTBS.Size = new System.Drawing.Size(73, 21);
            this.txtJTBS.TabIndex = 17;
            // 
            // txtJTBES
            // 
            // 
            // 
            // 
            this.txtJTBES.Border.Class = "TextBoxBorder";
            this.txtJTBES.Enabled = false;
            this.txtJTBES.Location = new System.Drawing.Point(186, 40);
            this.txtJTBES.Name = "txtJTBES";
            this.txtJTBES.Size = new System.Drawing.Size(73, 21);
            this.txtJTBES.TabIndex = 18;
            // 
            // txtJTBW
            // 
            // 
            // 
            // 
            this.txtJTBW.Border.Class = "TextBoxBorder";
            this.txtJTBW.Enabled = false;
            this.txtJTBW.Location = new System.Drawing.Point(40, 19);
            this.txtJTBW.Name = "txtJTBW";
            this.txtJTBW.Size = new System.Drawing.Size(73, 21);
            this.txtJTBW.TabIndex = 19;
            // 
            // txtVersionYear
            // 
            // 
            // 
            // 
            this.txtVersionYear.Border.Class = "TextBoxBorder";
            this.txtVersionYear.Font = new System.Drawing.Font("宋体", 7F);
            this.txtVersionYear.Location = new System.Drawing.Point(40, 349);
            this.txtVersionYear.Name = "txtVersionYear";
            this.txtVersionYear.Size = new System.Drawing.Size(258, 18);
            this.txtVersionYear.TabIndex = 21;
            this.txtVersionYear.Text = "(单位)于XXXX年测制。";
            // 
            // txtCartoGroup
            // 
            // 
            // 
            // 
            this.txtCartoGroup.Border.Class = "TextBoxBorder";
            this.txtCartoGroup.Location = new System.Drawing.Point(9, 145);
            this.txtCartoGroup.Multiline = true;
            this.txtCartoGroup.Name = "txtCartoGroup";
            this.txtCartoGroup.Size = new System.Drawing.Size(18, 109);
            this.txtCartoGroup.TabIndex = 22;
            this.txtCartoGroup.Text = "XX市林业资源局";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.chkBoxSecurity);
            this.groupPanel1.Controls.Add(this.rTxtRightLower);
            this.groupPanel1.Controls.Add(this.rTxtLeftLower);
            this.groupPanel1.Controls.Add(this.checkBoxTuli);
            this.groupPanel1.Controls.Add(this.txtGWY);
            this.groupPanel1.Controls.Add(this.txtGWX);
            this.groupPanel1.Controls.Add(this.label12);
            this.groupPanel1.Controls.Add(this.label11);
            this.groupPanel1.Controls.Add(this.pictureBox2);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.txtMapNO);
            this.groupPanel1.Controls.Add(this.cBoxScale);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.txtVersionYear);
            this.groupPanel1.Controls.Add(this.txtJTBW);
            this.groupPanel1.Controls.Add(this.txtJTBES);
            this.groupPanel1.Controls.Add(this.txtCartoGroup);
            this.groupPanel1.Controls.Add(this.txtJTBS);
            this.groupPanel1.Controls.Add(this.txtTitle);
            this.groupPanel1.Controls.Add(this.txtJTBWS);
            this.groupPanel1.Controls.Add(this.label4);
            this.groupPanel1.Controls.Add(this.txtJTBE);
            this.groupPanel1.Controls.Add(this.txtJTBEN);
            this.groupPanel1.Controls.Add(this.cBoxSecret);
            this.groupPanel1.Controls.Add(this.txtJTBN);
            this.groupPanel1.Controls.Add(this.txtJTBWN);
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(636, 406);
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
            this.groupPanel1.Click += new System.EventHandler(this.groupPanel1_Click);
            // 
            // chkBoxSecurity
            // 
            this.chkBoxSecurity.AutoSize = true;
            this.chkBoxSecurity.BackColor = System.Drawing.Color.Transparent;
            this.chkBoxSecurity.Checked = true;
            this.chkBoxSecurity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxSecurity.Location = new System.Drawing.Point(461, 35);
            this.chkBoxSecurity.Name = "chkBoxSecurity";
            this.chkBoxSecurity.Size = new System.Drawing.Size(48, 16);
            this.chkBoxSecurity.TabIndex = 47;
            this.chkBoxSecurity.Text = "密级";
            this.chkBoxSecurity.UseVisualStyleBackColor = false;
            this.chkBoxSecurity.CheckedChanged += new System.EventHandler(this.chkBoxSecurity_CheckedChanged);
            // 
            // rTxtRightLower
            // 
            this.rTxtRightLower.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rTxtRightLower.Location = new System.Drawing.Point(406, 260);
            this.rTxtRightLower.Name = "rTxtRightLower";
            this.rTxtRightLower.Size = new System.Drawing.Size(168, 83);
            this.rTxtRightLower.TabIndex = 45;
            this.rTxtRightLower.Text = "测量员：\n绘图员：\n检查员：";
            // 
            // rTxtLeftLower
            // 
            this.rTxtLeftLower.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rTxtLeftLower.Location = new System.Drawing.Point(40, 262);
            this.rTxtLeftLower.Name = "rTxtLeftLower";
            this.rTxtLeftLower.Size = new System.Drawing.Size(258, 81);
            this.rTxtLeftLower.TabIndex = 44;
            this.rTxtLeftLower.Text = "1980年西安坐标系\n1985国家高程基准，等高距为1m。\nGB/T20257.1-2007国家基本比例尺地图图式  第1部分：\n1:500 1:1000 1:2" +
                "000地形图图式 ";
            // 
            // checkBoxTuli
            // 
            this.checkBoxTuli.AutoSize = true;
            this.checkBoxTuli.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxTuli.Location = new System.Drawing.Point(500, 359);
            this.checkBoxTuli.Name = "checkBoxTuli";
            this.checkBoxTuli.Size = new System.Drawing.Size(72, 16);
            this.checkBoxTuli.TabIndex = 43;
            this.checkBoxTuli.Text = "生成图例";
            this.checkBoxTuli.UseVisualStyleBackColor = false;
            // 
            // txtGWY
            // 
            // 
            // 
            // 
            this.txtGWY.Border.Class = "TextBoxBorder";
            this.txtGWY.Location = new System.Drawing.Point(590, 3);
            this.txtGWY.Name = "txtGWY";
            this.txtGWY.Size = new System.Drawing.Size(39, 21);
            this.txtGWY.TabIndex = 42;
            this.txtGWY.Text = "XX";
            this.txtGWY.TextChanged += new System.EventHandler(this.txtGWY_TextChanged);
            // 
            // txtGWX
            // 
            // 
            // 
            // 
            this.txtGWX.Border.Class = "TextBoxBorder";
            this.txtGWX.Location = new System.Drawing.Point(509, 3);
            this.txtGWX.Name = "txtGWX";
            this.txtGWX.Size = new System.Drawing.Size(39, 21);
            this.txtGWX.TabIndex = 41;
            this.txtGWX.Text = "X";
            this.txtGWX.TextChanged += new System.EventHandler(this.txtGWX_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(552, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 40;
            this.label12.Text = "高位Y:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(463, 7);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 39;
            this.label11.Text = "高位X:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GeoPageLayout.Properties.Resources.GeoPageLayout_gzdt;
            this.pictureBox2.Location = new System.Drawing.Point(40, 65);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(534, 189);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // FrmSheetMapSetDJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 449);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSheetMapSetDJ";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置地籍图信息";
            this.Load += new System.EventHandler(this.FrmSheetMapSet2_Load);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX label1;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMapNO;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxScale;
        private DevComponents.Editors.ComboItem Item500;
        private DevComponents.Editors.ComboItem Item1000;
        private DevComponents.Editors.ComboItem Item2000;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTitle;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxSecret;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem10;
        private DevComponents.Editors.ComboItem comboItem11;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBWN;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBN;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBEN;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBE;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBWS;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBS;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBES;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBW;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersionYear;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCartoGroup;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGWY;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGWX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox checkBoxTuli;
        private System.Windows.Forms.RichTextBox rTxtLeftLower;
        private System.Windows.Forms.RichTextBox rTxtRightLower;
        private System.Windows.Forms.CheckBox chkBoxSecurity;
    }
}