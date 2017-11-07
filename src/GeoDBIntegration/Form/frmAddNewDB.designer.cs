namespace GeoDBIntegration
{
    partial class frmAddNewDB
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddNewDB));
            this.superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txt_DBName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.combox_DBType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.combox_DBFormat = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btn_cancle = new DevComponents.DotNetBar.ButtonX();
            this.btn_OK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnSelectAll = new DevComponents.DotNetBar.ButtonX();
            this.btnSelectNot = new DevComponents.DotNetBar.ButtonX();
            this.lvDataset = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.txt_servername = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.btn_test = new DevComponents.DotNetBar.ButtonX();
            this.txtVersion = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.txtDataBase = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX13 = new DevComponents.DotNetBar.LabelX();
            this.txtPassWord = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX15 = new DevComponents.DotNetBar.LabelX();
            this.labelX16 = new DevComponents.DotNetBar.LabelX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.btnHistoryCon = new DevComponents.DotNetBar.ButtonX();
            this.cmbDataset = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txt_metatype = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.btn_metatest = new DevComponents.DotNetBar.ButtonX();
            this.txt_metapassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txt_metauser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txt_metaServer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.txtRootPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.cmbScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.名称ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvRootPath = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.chkDBIsUpdate = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel3.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(11, 2);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(79, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "工程名称  ：";
            // 
            // txt_DBName
            // 
            // 
            // 
            // 
            this.txt_DBName.Border.Class = "TextBoxBorder";
            this.txt_DBName.Location = new System.Drawing.Point(96, 2);
            this.txt_DBName.MaxLength = 30;
            this.txt_DBName.Name = "txt_DBName";
            this.txt_DBName.Size = new System.Drawing.Size(312, 21);
            this.txt_DBName.TabIndex = 0;
            // 
            // combox_DBType
            // 
            this.combox_DBType.DisplayMember = "Text";
            this.combox_DBType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_DBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combox_DBType.FormattingEnabled = true;
            this.combox_DBType.ItemHeight = 15;
            this.combox_DBType.Location = new System.Drawing.Point(97, 27);
            this.combox_DBType.Name = "combox_DBType";
            this.combox_DBType.Size = new System.Drawing.Size(341, 21);
            this.combox_DBType.TabIndex = 1;
            this.combox_DBType.SelectedIndexChanged += new System.EventHandler(this.combox_DBType_SelectedIndexChanged);
            // 
            // combox_DBFormat
            // 
            this.combox_DBFormat.DisplayMember = "Text";
            this.combox_DBFormat.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.combox_DBFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combox_DBFormat.FormattingEnabled = true;
            this.combox_DBFormat.ItemHeight = 15;
            this.combox_DBFormat.Location = new System.Drawing.Point(96, 53);
            this.combox_DBFormat.Name = "combox_DBFormat";
            this.combox_DBFormat.Size = new System.Drawing.Size(341, 21);
            this.combox_DBFormat.TabIndex = 2;
            this.combox_DBFormat.SelectedIndexChanged += new System.EventHandler(this.combox_DBFormat_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 26);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(79, 23);
            this.labelX2.TabIndex = 6;
            this.labelX2.Text = "数据库类型：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(11, 51);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(79, 23);
            this.labelX3.TabIndex = 7;
            this.labelX3.Text = "数据库平台：";
            // 
            // btn_cancle
            // 
            this.btn_cancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_cancle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_cancle.Location = new System.Drawing.Point(384, 424);
            this.btn_cancle.Name = "btn_cancle";
            this.btn_cancle.Size = new System.Drawing.Size(53, 23);
            this.btn_cancle.TabIndex = 17;
            this.btn_cancle.Text = "取消";
            this.btn_cancle.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_OK.Location = new System.Drawing.Point(325, 424);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(53, 23);
            this.btn_OK.TabIndex = 16;
            this.btn_OK.Text = "确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.btnSelectAll);
            this.groupPanel3.Controls.Add(this.btnSelectNot);
            this.groupPanel3.Controls.Add(this.lvDataset);
            this.groupPanel3.Controls.Add(this.labelX8);
            this.groupPanel3.Controls.Add(this.txt_servername);
            this.groupPanel3.Controls.Add(this.labelX5);
            this.groupPanel3.Controls.Add(this.btn_test);
            this.groupPanel3.Controls.Add(this.txtVersion);
            this.groupPanel3.Controls.Add(this.labelX4);
            this.groupPanel3.Controls.Add(this.btnServer);
            this.groupPanel3.Controls.Add(this.txtDataBase);
            this.groupPanel3.Controls.Add(this.labelX13);
            this.groupPanel3.Controls.Add(this.txtPassWord);
            this.groupPanel3.Controls.Add(this.txtUser);
            this.groupPanel3.Controls.Add(this.txtServer);
            this.groupPanel3.Controls.Add(this.labelX15);
            this.groupPanel3.Controls.Add(this.labelX16);
            this.groupPanel3.Controls.Add(this.labelX18);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(11, 107);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(426, 220);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 29;
            this.groupPanel3.Text = "数据库连接设置";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectAll.Location = new System.Drawing.Point(362, 145);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(52, 21);
            this.btnSelectAll.TabIndex = 52;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectNot
            // 
            this.btnSelectNot.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectNot.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectNot.Location = new System.Drawing.Point(362, 172);
            this.btnSelectNot.Name = "btnSelectNot";
            this.btnSelectNot.Size = new System.Drawing.Size(52, 21);
            this.btnSelectNot.TabIndex = 51;
            this.btnSelectNot.Text = "反选";
            this.btnSelectNot.Click += new System.EventHandler(this.btnSelectNot_Click);
            // 
            // lvDataset
            // 
            // 
            // 
            // 
            this.lvDataset.Border.Class = "ListViewBorder";
            this.lvDataset.CheckBoxes = true;
            this.lvDataset.Location = new System.Drawing.Point(59, 87);
            this.lvDataset.Name = "lvDataset";
            this.lvDataset.Size = new System.Drawing.Size(292, 106);
            this.lvDataset.TabIndex = 50;
            this.lvDataset.UseCompatibleStateImageBehavior = false;
            this.lvDataset.View = System.Windows.Forms.View.List;
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            this.labelX8.Location = new System.Drawing.Point(2, 87);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(56, 18);
            this.labelX8.TabIndex = 49;
            this.labelX8.Text = "数据集 :";
            // 
            // txt_servername
            // 
            // 
            // 
            // 
            this.txt_servername.Border.Class = "TextBoxBorder";
            this.txt_servername.Location = new System.Drawing.Point(270, 3);
            this.txt_servername.Name = "txt_servername";
            this.txt_servername.Size = new System.Drawing.Size(144, 21);
            this.txt_servername.TabIndex = 4;
            this.txt_servername.WatermarkText = "数据库服务端口号";
            this.txt_servername.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(217, 3);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(56, 18);
            this.labelX5.TabIndex = 48;
            this.labelX5.Text = "服  务 :";
            // 
            // btn_test
            // 
            this.btn_test.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_test.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_test.Location = new System.Drawing.Point(362, 87);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(52, 21);
            this.btn_test.TabIndex = 10;
            this.btn_test.Text = "测试";
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(270, 58);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(144, 21);
            this.txtVersion.TabIndex = 9;
            this.txtVersion.WatermarkText = "版本信息";
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(217, 60);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(56, 18);
            this.labelX4.TabIndex = 44;
            this.labelX4.Text = "版  本 :";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnServer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnServer.Location = new System.Drawing.Point(180, 30);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(26, 21);
            this.btnServer.TabIndex = 6;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(59, 30);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(117, 21);
            this.txtDataBase.TabIndex = 5;
            this.txtDataBase.WatermarkText = "数据库名或本地库路径";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.BackColor = System.Drawing.Color.Transparent;
            this.labelX13.Location = new System.Drawing.Point(3, 30);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(56, 18);
            this.labelX13.TabIndex = 15;
            this.labelX13.Text = "数据库 :";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(59, 57);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(144, 21);
            this.txtPassWord.TabIndex = 8;
            this.txtPassWord.WatermarkText = "访问密码";
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(270, 30);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(144, 21);
            this.txtUser.TabIndex = 7;
            this.txtUser.WatermarkText = "访问用户名";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(59, 3);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(147, 21);
            this.txtServer.TabIndex = 3;
            this.txtServer.WatermarkText = "服务器ip地址或服务名";
            this.txtServer.WordWrap = false;
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.BackColor = System.Drawing.Color.Transparent;
            this.labelX15.Location = new System.Drawing.Point(3, 57);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(56, 18);
            this.labelX15.TabIndex = 4;
            this.labelX15.Text = "密  码 :";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            this.labelX16.BackColor = System.Drawing.Color.Transparent;
            this.labelX16.Location = new System.Drawing.Point(217, 30);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(56, 18);
            this.labelX16.TabIndex = 3;
            this.labelX16.Text = "用  户 :";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            this.labelX18.BackColor = System.Drawing.Color.Transparent;
            this.labelX18.Location = new System.Drawing.Point(3, 6);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(56, 18);
            this.labelX18.TabIndex = 1;
            this.labelX18.Text = "服务器 :";
            // 
            // btnHistoryCon
            // 
            this.btnHistoryCon.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnHistoryCon.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnHistoryCon.Image = ((System.Drawing.Image)(resources.GetObject("btnHistoryCon.Image")));
            this.btnHistoryCon.Location = new System.Drawing.Point(415, 2);
            this.btnHistoryCon.Name = "btnHistoryCon";
            this.btnHistoryCon.Size = new System.Drawing.Size(22, 22);
            this.btnHistoryCon.TabIndex = 51;
            this.btnHistoryCon.Tooltip = "历史连接";
            this.btnHistoryCon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnHistoryCon_MouseClick);
            // 
            // cmbDataset
            // 
            this.cmbDataset.DisplayMember = "Text";
            this.cmbDataset.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDataset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataset.FormattingEnabled = true;
            this.cmbDataset.ItemHeight = 15;
            this.cmbDataset.Location = new System.Drawing.Point(517, 294);
            this.cmbDataset.Name = "cmbDataset";
            this.cmbDataset.Size = new System.Drawing.Size(184, 21);
            this.cmbDataset.TabIndex = 50;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(12, 492);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(70, 10);
            this.splitContainer1.SplitterDistance = 41;
            this.splitContainer1.TabIndex = 30;
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.txt_metatype);
            this.groupPanel1.Controls.Add(this.labelX6);
            this.groupPanel1.Controls.Add(this.btn_metatest);
            this.groupPanel1.Controls.Add(this.txt_metapassword);
            this.groupPanel1.Controls.Add(this.txt_metauser);
            this.groupPanel1.Controls.Add(this.txt_metaServer);
            this.groupPanel1.Controls.Add(this.labelX9);
            this.groupPanel1.Controls.Add(this.labelX10);
            this.groupPanel1.Controls.Add(this.labelX11);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(11, 336);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(426, 79);
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
            this.groupPanel1.TabIndex = 30;
            this.groupPanel1.Text = "元信息库连接设置";
            // 
            // txt_metatype
            // 
            // 
            // 
            // 
            this.txt_metatype.Border.Class = "TextBoxBorder";
            this.txt_metatype.Enabled = false;
            this.txt_metatype.Location = new System.Drawing.Point(56, 0);
            this.txt_metatype.Name = "txt_metatype";
            this.txt_metatype.Size = new System.Drawing.Size(120, 21);
            this.txt_metatype.TabIndex = 11;
            this.txt_metatype.Text = "Oracle";
            this.txt_metatype.WatermarkText = "服务器ip地址或服务名";
            this.txt_metatype.WordWrap = false;
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Location = new System.Drawing.Point(3, 3);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(56, 18);
            this.labelX6.TabIndex = 48;
            this.labelX6.Text = "类  型 :";
            // 
            // btn_metatest
            // 
            this.btn_metatest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_metatest.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_metatest.Location = new System.Drawing.Point(362, 3);
            this.btn_metatest.Name = "btn_metatest";
            this.btn_metatest.Size = new System.Drawing.Size(52, 21);
            this.btn_metatest.TabIndex = 15;
            this.btn_metatest.Text = "测试";
            this.btn_metatest.Click += new System.EventHandler(this.btn_metatest_Click);
            // 
            // txt_metapassword
            // 
            // 
            // 
            // 
            this.txt_metapassword.Border.Class = "TextBoxBorder";
            this.txt_metapassword.Location = new System.Drawing.Point(237, 27);
            this.txt_metapassword.Name = "txt_metapassword";
            this.txt_metapassword.PasswordChar = '*';
            this.txt_metapassword.Size = new System.Drawing.Size(114, 21);
            this.txt_metapassword.TabIndex = 14;
            this.txt_metapassword.WatermarkText = "访问密码";
            // 
            // txt_metauser
            // 
            // 
            // 
            // 
            this.txt_metauser.Border.Class = "TextBoxBorder";
            this.txt_metauser.Location = new System.Drawing.Point(56, 27);
            this.txt_metauser.Name = "txt_metauser";
            this.txt_metauser.Size = new System.Drawing.Size(120, 21);
            this.txt_metauser.TabIndex = 13;
            this.txt_metauser.WatermarkText = "访问用户名";
            // 
            // txt_metaServer
            // 
            // 
            // 
            // 
            this.txt_metaServer.Border.Class = "TextBoxBorder";
            this.txt_metaServer.Location = new System.Drawing.Point(237, 0);
            this.txt_metaServer.Name = "txt_metaServer";
            this.txt_metaServer.Size = new System.Drawing.Size(114, 21);
            this.txt_metaServer.TabIndex = 12;
            this.txt_metaServer.WatermarkText = "Oracle服务名";
            this.txt_metaServer.WordWrap = false;
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            this.labelX9.Location = new System.Drawing.Point(184, 30);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(56, 18);
            this.labelX9.TabIndex = 4;
            this.labelX9.Text = "密  码 :";
            // 
            // labelX10
            // 
            this.labelX10.AutoSize = true;
            this.labelX10.BackColor = System.Drawing.Color.Transparent;
            this.labelX10.Location = new System.Drawing.Point(3, 27);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(56, 18);
            this.labelX10.TabIndex = 3;
            this.labelX10.Text = "用  户 :";
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            this.labelX11.BackColor = System.Drawing.Color.Transparent;
            this.labelX11.Location = new System.Drawing.Point(184, 3);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(56, 18);
            this.labelX11.TabIndex = 1;
            this.labelX11.Text = "服务器 :";
            // 
            // txtRootPath
            // 
            // 
            // 
            // 
            this.txtRootPath.Border.Class = "TextBoxBorder";
            this.txtRootPath.Location = new System.Drawing.Point(537, 267);
            this.txtRootPath.Name = "txtRootPath";
            this.txtRootPath.Size = new System.Drawing.Size(300, 21);
            this.txtRootPath.TabIndex = 52;
            this.txtRootPath.WatermarkText = "ftp栅格根目录";
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            this.labelX12.BackColor = System.Drawing.Color.Transparent;
            this.labelX12.Location = new System.Drawing.Point(452, 327);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(56, 18);
            this.labelX12.TabIndex = 51;
            this.labelX12.Text = "根目录 :";
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(11, 80);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(79, 23);
            this.labelX7.TabIndex = 32;
            this.labelX7.Text = "比 例 尺  ：";
            // 
            // cmbScale
            // 
            this.cmbScale.DisplayMember = "Text";
            this.cmbScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.ItemHeight = 15;
            this.cmbScale.Location = new System.Drawing.Point(96, 80);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(341, 21);
            this.cmbScale.TabIndex = 33;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.名称ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(69, 26);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // 名称ToolStripMenuItem
            // 
            this.名称ToolStripMenuItem.Name = "名称ToolStripMenuItem";
            this.名称ToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            // 
            // lvRootPath
            // 
            // 
            // 
            // 
            this.lvRootPath.Border.Class = "ListViewBorder";
            this.lvRootPath.CheckBoxes = true;
            this.lvRootPath.Location = new System.Drawing.Point(523, 330);
            this.lvRootPath.Name = "lvRootPath";
            this.lvRootPath.Size = new System.Drawing.Size(292, 55);
            this.lvRootPath.TabIndex = 52;
            this.lvRootPath.UseCompatibleStateImageBehavior = false;
            this.lvRootPath.View = System.Windows.Forms.View.List;
            // 
            // chkDBIsUpdate
            // 
            this.chkDBIsUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDBIsUpdate.Location = new System.Drawing.Point(12, 425);
            this.chkDBIsUpdate.Name = "chkDBIsUpdate";
            this.chkDBIsUpdate.Size = new System.Drawing.Size(183, 24);
            this.chkDBIsUpdate.TabIndex = 53;
            this.chkDBIsUpdate.Text = "不用于更新";
            this.chkDBIsUpdate.Visible = false;
            // 
            // frmAddNewDB
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 457);
            this.Controls.Add(this.chkDBIsUpdate);
            this.Controls.Add(this.btnHistoryCon);
            this.Controls.Add(this.labelX12);
            this.Controls.Add(this.lvRootPath);
            this.Controls.Add(this.txtRootPath);
            this.Controls.Add(this.cmbDataset);
            this.Controls.Add(this.cmbScale);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btn_cancle);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.combox_DBFormat);
            this.Controls.Add(this.txt_DBName);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.combox_DBType);
            this.Controls.Add(this.labelX2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddNewDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增数据源：";
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.SuperTooltip superTooltip;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_DBName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx combox_DBType;
        private DevComponents.DotNetBar.Controls.ComboBoxEx combox_DBFormat;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btn_cancle;
        private DevComponents.DotNetBar.ButtonX btn_OK;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.ButtonX btnServer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private DevComponents.DotNetBar.LabelX labelX13;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private DevComponents.DotNetBar.LabelX labelX15;
        private DevComponents.DotNetBar.LabelX labelX16;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.ButtonX btn_test;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_servername;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX btn_metatest;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_metapassword;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_metauser;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_metaServer;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX11;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_metatype;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbScale;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDataset;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX12;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRootPath;
        private DevComponents.DotNetBar.Controls.ListViewEx lvDataset;
        private DevComponents.DotNetBar.ButtonX btnHistoryCon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 名称ToolStripMenuItem;
        private DevComponents.DotNetBar.Controls.ListViewEx lvRootPath;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkDBIsUpdate;
        private DevComponents.DotNetBar.ButtonX btnSelectAll;
        private DevComponents.DotNetBar.ButtonX btnSelectNot;
    }
}