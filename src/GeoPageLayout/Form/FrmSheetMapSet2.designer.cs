namespace GeoPageLayout
{
    partial class FrmSheetMapSet2
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTitle = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cBoxSecret = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem9 = new DevComponents.Editors.ComboItem();
            this.comboItem10 = new DevComponents.Editors.ComboItem();
            this.comboItem11 = new DevComponents.Editors.ComboItem();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtJTBWN = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBN = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBEN = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBE = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBWS = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBS = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBES = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtJTBW = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtTime = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtVersionYear = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtCartoGroup = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cBoxCoordinate = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem12 = new DevComponents.Editors.ComboItem();
            this.cBoxElevation = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label6 = new System.Windows.Forms.Label();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtGWY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtGWX = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtJCHY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtHTY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtContourIntvl = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtCLY = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(277, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "1:";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(476, 394);
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
            this.btnCancel.Location = new System.Drawing.Point(557, 394);
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
            this.Item500,
            this.Item1000,
            this.Item2000});
            this.cBoxScale.Location = new System.Drawing.Point(300, 260);
            this.cBoxScale.Name = "cBoxScale";
            this.cBoxScale.Size = new System.Drawing.Size(121, 21);
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
            this.label2.Location = new System.Drawing.Point(254, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "图幅号:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(216, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "比例尺:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(254, 7);
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
            this.txtTitle.Text = "广州市基础地形图";
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
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(473, 32);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(37, 18);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "密级:";
            // 
            // txtJTBWN
            // 
            // 
            // 
            // 
            this.txtJTBWN.Border.Class = "TextBoxBorder";
            this.txtJTBWN.Location = new System.Drawing.Point(40, -2);
            this.txtJTBWN.Name = "txtJTBWN";
            this.txtJTBWN.Size = new System.Drawing.Size(70, 21);
            this.txtJTBWN.TabIndex = 12;
            // 
            // txtJTBN
            // 
            // 
            // 
            // 
            this.txtJTBN.Border.Class = "TextBoxBorder";
            this.txtJTBN.Location = new System.Drawing.Point(110, -2);
            this.txtJTBN.Name = "txtJTBN";
            this.txtJTBN.Size = new System.Drawing.Size(70, 21);
            this.txtJTBN.TabIndex = 13;
            // 
            // txtJTBEN
            // 
            // 
            // 
            // 
            this.txtJTBEN.Border.Class = "TextBoxBorder";
            this.txtJTBEN.Location = new System.Drawing.Point(180, -2);
            this.txtJTBEN.Name = "txtJTBEN";
            this.txtJTBEN.Size = new System.Drawing.Size(70, 21);
            this.txtJTBEN.TabIndex = 14;
            // 
            // txtJTBE
            // 
            // 
            // 
            // 
            this.txtJTBE.Border.Class = "TextBoxBorder";
            this.txtJTBE.Location = new System.Drawing.Point(180, 19);
            this.txtJTBE.Name = "txtJTBE";
            this.txtJTBE.Size = new System.Drawing.Size(70, 21);
            this.txtJTBE.TabIndex = 15;
            // 
            // txtJTBWS
            // 
            // 
            // 
            // 
            this.txtJTBWS.Border.Class = "TextBoxBorder";
            this.txtJTBWS.Location = new System.Drawing.Point(40, 40);
            this.txtJTBWS.Name = "txtJTBWS";
            this.txtJTBWS.Size = new System.Drawing.Size(70, 21);
            this.txtJTBWS.TabIndex = 16;
            // 
            // txtJTBS
            // 
            // 
            // 
            // 
            this.txtJTBS.Border.Class = "TextBoxBorder";
            this.txtJTBS.Location = new System.Drawing.Point(110, 40);
            this.txtJTBS.Name = "txtJTBS";
            this.txtJTBS.Size = new System.Drawing.Size(70, 21);
            this.txtJTBS.TabIndex = 17;
            // 
            // txtJTBES
            // 
            // 
            // 
            // 
            this.txtJTBES.Border.Class = "TextBoxBorder";
            this.txtJTBES.Location = new System.Drawing.Point(180, 40);
            this.txtJTBES.Name = "txtJTBES";
            this.txtJTBES.Size = new System.Drawing.Size(70, 21);
            this.txtJTBES.TabIndex = 18;
            // 
            // txtJTBW
            // 
            // 
            // 
            // 
            this.txtJTBW.Border.Class = "TextBoxBorder";
            this.txtJTBW.Location = new System.Drawing.Point(40, 19);
            this.txtJTBW.Name = "txtJTBW";
            this.txtJTBW.Size = new System.Drawing.Size(70, 21);
            this.txtJTBW.TabIndex = 19;
            // 
            // txtTime
            // 
            // 
            // 
            // 
            this.txtTime.Border.Class = "TextBoxBorder";
            this.txtTime.Location = new System.Drawing.Point(40, 260);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(117, 21);
            this.txtTime.TabIndex = 20;
            this.txtTime.Text = "1995年5月XXX测图";
            // 
            // txtVersionYear
            // 
            // 
            // 
            // 
            this.txtVersionYear.Border.Class = "TextBoxBorder";
            this.txtVersionYear.Location = new System.Drawing.Point(40, 323);
            this.txtVersionYear.Name = "txtVersionYear";
            this.txtVersionYear.Size = new System.Drawing.Size(58, 21);
            this.txtVersionYear.TabIndex = 21;
            this.txtVersionYear.Text = "1996";
            // 
            // txtCartoGroup
            // 
            // 
            // 
            // 
            this.txtCartoGroup.Border.Class = "TextBoxBorder";
            this.txtCartoGroup.Location = new System.Drawing.Point(9, 164);
            this.txtCartoGroup.Multiline = true;
            this.txtCartoGroup.Name = "txtCartoGroup";
            this.txtCartoGroup.Size = new System.Drawing.Size(18, 90);
            this.txtCartoGroup.TabIndex = 22;
            this.txtCartoGroup.Text = "广州市测绘局";
            // 
            // cBoxCoordinate
            // 
            this.cBoxCoordinate.DisplayMember = "Text";
            this.cBoxCoordinate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxCoordinate.FormattingEnabled = true;
            this.cBoxCoordinate.ItemHeight = 15;
            this.cBoxCoordinate.Items.AddRange(new object[] {
            this.comboItem12});
            this.cBoxCoordinate.Location = new System.Drawing.Point(40, 281);
            this.cBoxCoordinate.Name = "cBoxCoordinate";
            this.cBoxCoordinate.Size = new System.Drawing.Size(117, 21);
            this.cBoxCoordinate.TabIndex = 24;
            this.cBoxCoordinate.Text = "1980年西安坐标系";
            // 
            // comboItem12
            // 
            this.comboItem12.Text = "西安80,高斯-克吕格投影";
            // 
            // cBoxElevation
            // 
            this.cBoxElevation.DisplayMember = "Text";
            this.cBoxElevation.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cBoxElevation.FormattingEnabled = true;
            this.cBoxElevation.ItemHeight = 15;
            this.cBoxElevation.Location = new System.Drawing.Point(40, 302);
            this.cBoxElevation.Name = "cBoxElevation";
            this.cBoxElevation.Size = new System.Drawing.Size(117, 21);
            this.cBoxElevation.TabIndex = 25;
            this.cBoxElevation.Text = "1985国家高程基准";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(104, 326);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 26;
            this.label6.Text = "年版图式";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txtGWY);
            this.groupPanel1.Controls.Add(this.txtGWX);
            this.groupPanel1.Controls.Add(this.label12);
            this.groupPanel1.Controls.Add(this.label11);
            this.groupPanel1.Controls.Add(this.label8);
            this.groupPanel1.Controls.Add(this.label7);
            this.groupPanel1.Controls.Add(this.label5);
            this.groupPanel1.Controls.Add(this.txtJCHY);
            this.groupPanel1.Controls.Add(this.txtHTY);
            this.groupPanel1.Controls.Add(this.pictureBox2);
            this.groupPanel1.Controls.Add(this.label10);
            this.groupPanel1.Controls.Add(this.label9);
            this.groupPanel1.Controls.Add(this.txtContourIntvl);
            this.groupPanel1.Controls.Add(this.txtCLY);
            this.groupPanel1.Controls.Add(this.label1);
            this.groupPanel1.Controls.Add(this.label6);
            this.groupPanel1.Controls.Add(this.txtMapNO);
            this.groupPanel1.Controls.Add(this.cBoxElevation);
            this.groupPanel1.Controls.Add(this.cBoxScale);
            this.groupPanel1.Controls.Add(this.cBoxCoordinate);
            this.groupPanel1.Controls.Add(this.label2);
            this.groupPanel1.Controls.Add(this.txtVersionYear);
            this.groupPanel1.Controls.Add(this.txtTime);
            this.groupPanel1.Controls.Add(this.txtJTBW);
            this.groupPanel1.Controls.Add(this.label3);
            this.groupPanel1.Controls.Add(this.txtJTBES);
            this.groupPanel1.Controls.Add(this.txtCartoGroup);
            this.groupPanel1.Controls.Add(this.txtJTBS);
            this.groupPanel1.Controls.Add(this.txtTitle);
            this.groupPanel1.Controls.Add(this.txtJTBWS);
            this.groupPanel1.Controls.Add(this.label4);
            this.groupPanel1.Controls.Add(this.txtJTBE);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Controls.Add(this.txtJTBEN);
            this.groupPanel1.Controls.Add(this.cBoxSecret);
            this.groupPanel1.Controls.Add(this.txtJTBN);
            this.groupPanel1.Controls.Add(this.txtJTBWN);
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(636, 375);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(446, 304);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 38;
            this.label8.Text = "检查员:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(446, 283);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "绘图员:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(446, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "测量员:";
            // 
            // txtJCHY
            // 
            // 
            // 
            // 
            this.txtJCHY.Border.Class = "TextBoxBorder";
            this.txtJCHY.Location = new System.Drawing.Point(499, 302);
            this.txtJCHY.Name = "txtJCHY";
            this.txtJCHY.Size = new System.Drawing.Size(75, 21);
            this.txtJCHY.TabIndex = 35;
            // 
            // txtHTY
            // 
            // 
            // 
            // 
            this.txtHTY.Border.Class = "TextBoxBorder";
            this.txtHTY.Location = new System.Drawing.Point(499, 281);
            this.txtHTY.Name = "txtHTY";
            this.txtHTY.Size = new System.Drawing.Size(75, 21);
            this.txtHTY.TabIndex = 34;
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(246, 306);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "米";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(160, 306);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "等高距为";
            // 
            // txtContourIntvl
            // 
            // 
            // 
            // 
            this.txtContourIntvl.Border.Class = "TextBoxBorder";
            this.txtContourIntvl.Location = new System.Drawing.Point(213, 302);
            this.txtContourIntvl.Name = "txtContourIntvl";
            this.txtContourIntvl.Size = new System.Drawing.Size(27, 21);
            this.txtContourIntvl.TabIndex = 30;
            this.txtContourIntvl.Text = "1";
            // 
            // txtCLY
            // 
            // 
            // 
            // 
            this.txtCLY.Border.Class = "TextBoxBorder";
            this.txtCLY.Location = new System.Drawing.Point(499, 260);
            this.txtCLY.Name = "txtCLY";
            this.txtCLY.Size = new System.Drawing.Size(75, 21);
            this.txtCLY.TabIndex = 28;
            // 
            // FrmSheetMapSet2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 428);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSheetMapSet2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置标准图幅信息";
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTitle;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxSecret;
        private DevComponents.Editors.ComboItem comboItem9;
        private DevComponents.Editors.ComboItem comboItem10;
        private DevComponents.Editors.ComboItem comboItem11;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBWN;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBN;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBEN;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBE;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBWS;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBS;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBES;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJTBW;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTime;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersionYear;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCartoGroup;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxCoordinate;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cBoxElevation;
        private System.Windows.Forms.Label label6;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCLY;
        private System.Windows.Forms.Label label9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtContourIntvl;
        private DevComponents.Editors.ComboItem comboItem12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureBox2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtJCHY;
        private DevComponents.DotNetBar.Controls.TextBoxX txtHTY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGWY;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGWX;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
    }
}