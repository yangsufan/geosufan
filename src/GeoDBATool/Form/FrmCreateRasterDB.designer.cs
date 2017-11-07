namespace GeoDBATool
{
    partial class FrmCreateRasterDB
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbRasterType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pListViewDT = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnAddList = new DevComponents.DotNetBar.ButtonX();
            this.rbcatalog = new System.Windows.Forms.RadioButton();
            this.rbdataset = new System.Windows.Forms.RadioButton();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbRasterSpaRef = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.cmbGeoSpaRef = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.comBoxType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtDataBase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.txtVersion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtPassWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtInstance = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX17 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.labelXErr = new DevComponents.DotNetBar.LabelX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.txtRasterName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBand = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGeoSpati = new DevComponents.DotNetBar.ButtonX();
            this.btnRasterSpati = new DevComponents.DotNetBar.ButtonX();
            this.txtGeoSpati = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtRasterSpati = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX19 = new DevComponents.DotNetBar.LabelX();
            this.btnRuleFile = new DevComponents.DotNetBar.ButtonX();
            this.textRuleFilePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.cmbRasterPixeType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.tileW = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.tileH = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.cmbResampleType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbCompression = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txtPyramid = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbRasterType
            // 
            this.cmbRasterType.DisplayMember = "Text";
            this.cmbRasterType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRasterType.FormattingEnabled = true;
            this.cmbRasterType.ItemHeight = 15;
            this.cmbRasterType.Location = new System.Drawing.Point(131, 474);
            this.cmbRasterType.Name = "cmbRasterType";
            this.cmbRasterType.Size = new System.Drawing.Size(220, 21);
            this.cmbRasterType.TabIndex = 0;
            this.cmbRasterType.SelectedIndexChanged += new System.EventHandler(this.cmbRasterType_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(34, 478);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(101, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "数 据 类 型：";
            // 
            // pListViewDT
            // 
            // 
            // 
            // 
            this.pListViewDT.Border.Class = "ListViewBorder";
            this.pListViewDT.Location = new System.Drawing.Point(9, 526);
            this.pListViewDT.Name = "pListViewDT";
            this.pListViewDT.Size = new System.Drawing.Size(328, 62);
            this.pListViewDT.TabIndex = 2;
            this.pListViewDT.UseCompatibleStateImageBehavior = false;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(551, 386);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(53, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "完 成";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(623, 386);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(52, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddList
            // 
            this.btnAddList.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddList.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddList.Location = new System.Drawing.Point(292, 501);
            this.btnAddList.Name = "btnAddList";
            this.btnAddList.Size = new System.Drawing.Size(53, 23);
            this.btnAddList.TabIndex = 5;
            this.btnAddList.Text = "添 加";
            this.btnAddList.Click += new System.EventHandler(this.btnAddList_Click);
            // 
            // rbcatalog
            // 
            this.rbcatalog.AutoSize = true;
            this.rbcatalog.Location = new System.Drawing.Point(114, 24);
            this.rbcatalog.Name = "rbcatalog";
            this.rbcatalog.Size = new System.Drawing.Size(71, 16);
            this.rbcatalog.TabIndex = 6;
            this.rbcatalog.TabStop = true;
            this.rbcatalog.Text = "栅格编目";
            this.rbcatalog.UseVisualStyleBackColor = true;
            this.rbcatalog.CheckedChanged += new System.EventHandler(this.rbcatalog_CheckedChanged);
            // 
            // rbdataset
            // 
            this.rbdataset.AutoSize = true;
            this.rbdataset.Location = new System.Drawing.Point(210, 25);
            this.rbdataset.Name = "rbdataset";
            this.rbdataset.Size = new System.Drawing.Size(83, 16);
            this.rbdataset.TabIndex = 7;
            this.rbdataset.TabStop = true;
            this.rbdataset.Text = "栅格数据集";
            this.rbdataset.UseVisualStyleBackColor = true;
            this.rbdataset.CheckedChanged += new System.EventHandler(this.rbdataset_CheckedChanged);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(18, 20);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(93, 23);
            this.labelX2.TabIndex = 9;
            this.labelX2.Text = "栅格空间参考：";
            // 
            // cmbRasterSpaRef
            // 
            this.cmbRasterSpaRef.DisplayMember = "Text";
            this.cmbRasterSpaRef.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRasterSpaRef.FormattingEnabled = true;
            this.cmbRasterSpaRef.ItemHeight = 15;
            this.cmbRasterSpaRef.Location = new System.Drawing.Point(355, 519);
            this.cmbRasterSpaRef.Name = "cmbRasterSpaRef";
            this.cmbRasterSpaRef.Size = new System.Drawing.Size(210, 21);
            this.cmbRasterSpaRef.TabIndex = 8;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(17, 20);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(90, 23);
            this.labelX3.TabIndex = 10;
            this.labelX3.Text = "存 储 类 型：";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(18, 49);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(94, 23);
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "几何空间参考：";
            // 
            // cmbGeoSpaRef
            // 
            this.cmbGeoSpaRef.DisplayMember = "Text";
            this.cmbGeoSpaRef.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGeoSpaRef.FormattingEnabled = true;
            this.cmbGeoSpaRef.ItemHeight = 15;
            this.cmbGeoSpaRef.Location = new System.Drawing.Point(354, 546);
            this.cmbGeoSpaRef.Name = "cmbGeoSpaRef";
            this.cmbGeoSpaRef.Size = new System.Drawing.Size(210, 21);
            this.cmbGeoSpaRef.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnServer);
            this.groupBox1.Controls.Add(this.comBoxType);
            this.groupBox1.Controls.Add(this.labelX4);
            this.groupBox1.Controls.Add(this.txtDataBase);
            this.groupBox1.Controls.Add(this.labelX13);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.txtPassWord);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.txtInstance);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.labelX14);
            this.groupBox1.Controls.Add(this.labelX15);
            this.groupBox1.Controls.Add(this.labelX16);
            this.groupBox1.Controls.Add(this.labelX17);
            this.groupBox1.Controls.Add(this.labelX18);
            this.groupBox1.Location = new System.Drawing.Point(11, 49);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 269);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "栅格数据库连接信息";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnServer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnServer.Location = new System.Drawing.Point(285, 119);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(26, 21);
            this.btnServer.TabIndex = 60;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // comBoxType
            // 
            this.comBoxType.DisplayMember = "Text";
            this.comBoxType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.ItemHeight = 15;
            this.comBoxType.Location = new System.Drawing.Point(97, 17);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(214, 21);
            this.comBoxType.TabIndex = 59;
            this.comBoxType.SelectedIndexChanged += new System.EventHandler(this.comBoxType_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(6, 20);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(87, 18);
            this.labelX4.TabIndex = 58;
            this.labelX4.Text = "数据库类 型：";
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(97, 119);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(186, 21);
            this.txtDataBase.TabIndex = 55;
            this.txtDataBase.WatermarkText = "本地库路径";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.Location = new System.Drawing.Point(6, 122);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(87, 18);
            this.labelX13.TabIndex = 54;
            this.labelX13.Text = "数  据   库：";
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(97, 227);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(214, 21);
            this.txtVersion.TabIndex = 53;
            this.txtVersion.Text = "SDE.DEFAULT";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(97, 187);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(214, 21);
            this.txtPassWord.TabIndex = 52;
            this.txtPassWord.WatermarkText = "SDE访问密码";
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(97, 152);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(214, 21);
            this.txtUser.TabIndex = 51;
            this.txtUser.WatermarkText = "SDE访问用户名";
            // 
            // txtInstance
            // 
            // 
            // 
            // 
            this.txtInstance.Border.Class = "TextBoxBorder";
            this.txtInstance.Location = new System.Drawing.Point(97, 85);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(214, 21);
            this.txtInstance.TabIndex = 50;
            this.txtInstance.WatermarkText = "数据库实例名称";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(97, 48);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(214, 21);
            this.txtServer.TabIndex = 49;
            this.txtServer.WatermarkText = "服务器IP地址或机器名";
            this.txtServer.WordWrap = false;
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.Location = new System.Drawing.Point(6, 230);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(87, 18);
            this.labelX14.TabIndex = 48;
            this.labelX14.Text = "版       本：";
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.Location = new System.Drawing.Point(6, 190);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(87, 18);
            this.labelX15.TabIndex = 47;
            this.labelX15.Text = "密       码：";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            this.labelX16.Location = new System.Drawing.Point(6, 155);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(87, 18);
            this.labelX16.TabIndex = 46;
            this.labelX16.Text = "用       户：";
            // 
            // labelX17
            // 
            this.labelX17.AutoSize = true;
            this.labelX17.Location = new System.Drawing.Point(6, 88);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(87, 18);
            this.labelX17.TabIndex = 45;
            this.labelX17.Text = "实  例   名：";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            this.labelX18.Location = new System.Drawing.Point(6, 51);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(87, 18);
            this.labelX18.TabIndex = 44;
            this.labelX18.Text = "服  务   器：";
            // 
            // labelXErr
            // 
            this.labelXErr.Location = new System.Drawing.Point(10, 353);
            this.labelXErr.Name = "labelXErr";
            this.labelXErr.Size = new System.Drawing.Size(321, 56);
            this.labelXErr.TabIndex = 16;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(12, 324);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(95, 23);
            this.labelX6.TabIndex = 18;
            this.labelX6.Text = "名        称：";
            // 
            // txtRasterName
            // 
            // 
            // 
            // 
            this.txtRasterName.Border.Class = "TextBoxBorder";
            this.txtRasterName.Location = new System.Drawing.Point(109, 324);
            this.txtRasterName.Name = "txtRasterName";
            this.txtRasterName.Size = new System.Drawing.Size(211, 21);
            this.txtRasterName.TabIndex = 19;
            // 
            // txtBand
            // 
            // 
            // 
            // 
            this.txtBand.Border.Class = "TextBoxBorder";
            this.txtBand.Location = new System.Drawing.Point(109, 167);
            this.txtBand.Name = "txtBand";
            this.txtBand.Size = new System.Drawing.Size(210, 21);
            this.txtBand.TabIndex = 21;
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(12, 169);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(93, 23);
            this.labelX7.TabIndex = 20;
            this.labelX7.Text = "波   段   数：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnGeoSpati);
            this.groupBox3.Controls.Add(this.btnRasterSpati);
            this.groupBox3.Controls.Add(this.txtGeoSpati);
            this.groupBox3.Controls.Add(this.txtRasterSpati);
            this.groupBox3.Controls.Add(this.labelX19);
            this.groupBox3.Controls.Add(this.btnRuleFile);
            this.groupBox3.Controls.Add(this.textRuleFilePath);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.labelX2);
            this.groupBox3.Controls.Add(this.labelX5);
            this.groupBox3.Location = new System.Drawing.Point(337, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(341, 356);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "库体参数设置";
            // 
            // btnGeoSpati
            // 
            this.btnGeoSpati.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGeoSpati.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGeoSpati.Location = new System.Drawing.Point(299, 49);
            this.btnGeoSpati.Name = "btnGeoSpati";
            this.btnGeoSpati.Size = new System.Drawing.Size(26, 21);
            this.btnGeoSpati.TabIndex = 44;
            this.btnGeoSpati.Text = "...";
            this.btnGeoSpati.Click += new System.EventHandler(this.btnGeoSpati_Click);
            // 
            // btnRasterSpati
            // 
            this.btnRasterSpati.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRasterSpati.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRasterSpati.Location = new System.Drawing.Point(299, 20);
            this.btnRasterSpati.Name = "btnRasterSpati";
            this.btnRasterSpati.Size = new System.Drawing.Size(26, 21);
            this.btnRasterSpati.TabIndex = 43;
            this.btnRasterSpati.Text = "...";
            this.btnRasterSpati.Click += new System.EventHandler(this.btnRasterSpati_Click);
            // 
            // txtGeoSpati
            // 
            // 
            // 
            // 
            this.txtGeoSpati.Border.Class = "TextBoxBorder";
            this.txtGeoSpati.Location = new System.Drawing.Point(116, 49);
            this.txtGeoSpati.Name = "txtGeoSpati";
            this.txtGeoSpati.Size = new System.Drawing.Size(181, 21);
            this.txtGeoSpati.TabIndex = 42;
            // 
            // txtRasterSpati
            // 
            // 
            // 
            // 
            this.txtRasterSpati.Border.Class = "TextBoxBorder";
            this.txtRasterSpati.Location = new System.Drawing.Point(118, 20);
            this.txtRasterSpati.Name = "txtRasterSpati";
            this.txtRasterSpati.Size = new System.Drawing.Size(181, 21);
            this.txtRasterSpati.TabIndex = 41;
            // 
            // labelX19
            // 
            this.labelX19.Location = new System.Drawing.Point(17, 82);
            this.labelX19.Name = "labelX19";
            this.labelX19.Size = new System.Drawing.Size(94, 23);
            this.labelX19.TabIndex = 40;
            this.labelX19.Text = "库体配置文件：";
            // 
            // btnRuleFile
            // 
            this.btnRuleFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnRuleFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnRuleFile.Location = new System.Drawing.Point(299, 82);
            this.btnRuleFile.Name = "btnRuleFile";
            this.btnRuleFile.Size = new System.Drawing.Size(26, 21);
            this.btnRuleFile.TabIndex = 39;
            this.btnRuleFile.Text = "...";
            this.btnRuleFile.Click += new System.EventHandler(this.btnRuleFile_Click);
            // 
            // textRuleFilePath
            // 
            // 
            // 
            // 
            this.textRuleFilePath.Border.Class = "TextBoxBorder";
            this.textRuleFilePath.Location = new System.Drawing.Point(114, 82);
            this.textRuleFilePath.Name = "textRuleFilePath";
            this.textRuleFilePath.Size = new System.Drawing.Size(185, 21);
            this.textRuleFilePath.TabIndex = 38;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelX20);
            this.groupBox2.Controls.Add(this.labelX12);
            this.groupBox2.Controls.Add(this.cmbRasterPixeType);
            this.groupBox2.Controls.Add(this.tileW);
            this.groupBox2.Controls.Add(this.labelX11);
            this.groupBox2.Controls.Add(this.txtBand);
            this.groupBox2.Controls.Add(this.labelX10);
            this.groupBox2.Controls.Add(this.tileH);
            this.groupBox2.Controls.Add(this.cmbResampleType);
            this.groupBox2.Controls.Add(this.labelX7);
            this.groupBox2.Controls.Add(this.cmbCompression);
            this.groupBox2.Controls.Add(this.labelX9);
            this.groupBox2.Controls.Add(this.txtPyramid);
            this.groupBox2.Controls.Add(this.labelX8);
            this.groupBox2.Location = new System.Drawing.Point(6, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(327, 228);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "栅格数据集参数设置";
            // 
            // labelX20
            // 
            this.labelX20.Location = new System.Drawing.Point(12, 198);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(94, 23);
            this.labelX20.TabIndex = 45;
            this.labelX20.Text = "栅格像素类型：";
            // 
            // labelX12
            // 
            this.labelX12.Location = new System.Drawing.Point(11, 20);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(94, 23);
            this.labelX12.TabIndex = 0;
            this.labelX12.Text = "重采样类型：";
            // 
            // cmbRasterPixeType
            // 
            this.cmbRasterPixeType.DisplayMember = "Text";
            this.cmbRasterPixeType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRasterPixeType.FormattingEnabled = true;
            this.cmbRasterPixeType.ItemHeight = 15;
            this.cmbRasterPixeType.Location = new System.Drawing.Point(109, 198);
            this.cmbRasterPixeType.Name = "cmbRasterPixeType";
            this.cmbRasterPixeType.Size = new System.Drawing.Size(210, 21);
            this.cmbRasterPixeType.TabIndex = 46;
            // 
            // tileW
            // 
            // 
            // 
            // 
            this.tileW.Border.Class = "TextBoxBorder";
            this.tileW.Location = new System.Drawing.Point(109, 137);
            this.tileW.Name = "tileW";
            this.tileW.Size = new System.Drawing.Size(210, 21);
            this.tileW.TabIndex = 9;
            // 
            // labelX11
            // 
            this.labelX11.Location = new System.Drawing.Point(12, 50);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(75, 23);
            this.labelX11.TabIndex = 1;
            this.labelX11.Text = "压缩类型：";
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(12, 79);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(75, 23);
            this.labelX10.TabIndex = 2;
            this.labelX10.Text = "金字塔：";
            // 
            // tileH
            // 
            // 
            // 
            // 
            this.tileH.Border.Class = "TextBoxBorder";
            this.tileH.Location = new System.Drawing.Point(110, 108);
            this.tileH.Name = "tileH";
            this.tileH.Size = new System.Drawing.Size(209, 21);
            this.tileH.TabIndex = 8;
            // 
            // cmbResampleType
            // 
            this.cmbResampleType.DisplayMember = "Text";
            this.cmbResampleType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbResampleType.FormattingEnabled = true;
            this.cmbResampleType.ItemHeight = 15;
            this.cmbResampleType.Location = new System.Drawing.Point(109, 20);
            this.cmbResampleType.Name = "cmbResampleType";
            this.cmbResampleType.Size = new System.Drawing.Size(210, 21);
            this.cmbResampleType.TabIndex = 5;
            // 
            // cmbCompression
            // 
            this.cmbCompression.DisplayMember = "Text";
            this.cmbCompression.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCompression.FormattingEnabled = true;
            this.cmbCompression.ItemHeight = 15;
            this.cmbCompression.Location = new System.Drawing.Point(109, 50);
            this.cmbCompression.Name = "cmbCompression";
            this.cmbCompression.Size = new System.Drawing.Size(210, 21);
            this.cmbCompression.TabIndex = 6;
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(12, 137);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(75, 23);
            this.labelX9.TabIndex = 4;
            this.labelX9.Text = "瓦片宽度：";
            // 
            // txtPyramid
            // 
            // 
            // 
            // 
            this.txtPyramid.Border.Class = "TextBoxBorder";
            this.txtPyramid.Location = new System.Drawing.Point(110, 77);
            this.txtPyramid.Name = "txtPyramid";
            this.txtPyramid.Size = new System.Drawing.Size(209, 21);
            this.txtPyramid.TabIndex = 7;
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(12, 108);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(75, 23);
            this.labelX8.TabIndex = 3;
            this.labelX8.Text = "瓦片高度：";
            // 
            // FrmCreateRasterDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 418);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.labelXErr);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.rbdataset);
            this.Controls.Add(this.rbcatalog);
            this.Controls.Add(this.cmbRasterSpaRef);
            this.Controls.Add(this.cmbGeoSpaRef);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.btnAddList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtRasterName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbRasterType);
            this.Controls.Add(this.pListViewDT);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCreateRasterDB";
            this.ShowIcon = false;
            this.Text = "栅格数据库管理";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRasterType;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ListViewEx pListViewDT;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnAddList;
        private System.Windows.Forms.RadioButton rbcatalog;
        private System.Windows.Forms.RadioButton rbdataset;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRasterSpaRef;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbGeoSpaRef;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInstance;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.LabelX labelX17;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comBoxType;
        private DevComponents.DotNetBar.LabelX labelXErr;
        public DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRasterName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBand;
        private DevComponents.DotNetBar.LabelX labelX7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.Controls.TextBoxX tileW;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.Controls.TextBoxX tileH;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbResampleType;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbCompression;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPyramid;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX19;
        public DevComponents.DotNetBar.ButtonX btnRuleFile;
        public DevComponents.DotNetBar.Controls.TextBoxX textRuleFilePath;
        public DevComponents.DotNetBar.ButtonX btnGeoSpati;
        public DevComponents.DotNetBar.ButtonX btnRasterSpati;
        private DevComponents.DotNetBar.Controls.TextBoxX txtGeoSpati;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRasterSpati;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRasterPixeType;
    }
}

