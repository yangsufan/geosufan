using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevComponents.DotNetBar;
using System.IO;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoDBATool
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
    public class frmDBsConnSet : DevComponents.DotNetBar.Office2007Form
    {
		private DevComponents.DotNetBar.ItemPanel itemPanel1;
		private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
		private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private ControlContainerItem controlContainerItem1;
        private PanelEx panelEx1;
        private ButtonX btnCancel;
        private ButtonX btnOK;
        private DevComponents.DotNetBar.Controls.GroupPanel groupUserDBSet;
        private DevComponents.DotNetBar.Controls.GroupPanel groupHisDBSet;
        public ButtonX btnServer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataBase;
        private LabelX labelX13;
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersion;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassWord;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInstance;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServer;
        private LabelX labelX14;
        private LabelX labelX15;
        private LabelX labelX16;
        private LabelX labelX17;
        private LabelX labelX18;
		private System.ComponentModel.IContainer components;

        private Plugin.Application.IAppGISRef m_Hook = null;

        ///构造现势库连接信息
        ///
        private CustomDBConnInfomation m_UserDBInfo = new CustomDBConnInfomation("现势库");
        private CustomDBConnInfomation.DBSet m_UserDBInfoConn = new CustomDBConnInfomation.DBSet();

        ///构造业务对象连接信息
        ///
        CustomDBConnInfomation m_CaseDBInfo = new CustomDBConnInfomation("业务对象");

        ///构造更新环境连接信息
        ///
        CustomDBConnInfomation m_UpdateDBInfo = new CustomDBConnInfomation("更新环境");
        private DevComponents.DotNetBar.Controls.TextBoxX txtVersionHistory;
        private LabelX labelX1;
        private ButtonX btnDBHistory;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPasswordHistory;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUserHistory;
        private LabelX labelX8;
        private LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDBHistory;
        private LabelX labelX10;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInstanceHistory;
        private LabelX labelX11;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServerHistory;
        private LabelX labelX12;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxTypeHistory;
        private LabelX labelX2;
        private ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx2;
        private LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comBoxType;
        private LabelX labelX4;

        ///构造历史库连接信息
        ///
        CustomDBConnInfomation m_HisDBInfo = new CustomDBConnInfomation("历史库");

		public frmDBsConnSet()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

        public frmDBsConnSet(Plugin.Application.IAppGISRef Hook)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            m_Hook = Hook;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.controlContainerItem1 = new DevComponents.DotNetBar.ControlContainerItem();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.groupUserDBSet = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.comBoxType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.comboBoxEx2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
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
            this.groupHisDBSet = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtVersionHistory = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnDBHistory = new DevComponents.DotNetBar.ButtonX();
            this.txtPasswordHistory = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtUserHistory = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txtDBHistory = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.txtInstanceHistory = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.txtServerHistory = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX12 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxTypeHistory = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.panelEx1.SuspendLayout();
            this.groupUserDBSet.SuspendLayout();
            this.groupHisDBSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemPanel1
            // 
            this.itemPanel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanel1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanel1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanel1.BackgroundStyle.PaddingBottom = 1;
            this.itemPanel1.BackgroundStyle.PaddingLeft = 1;
            this.itemPanel1.BackgroundStyle.PaddingRight = 1;
            this.itemPanel1.BackgroundStyle.PaddingTop = 1;
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(0, 0);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(144, 314);
            this.itemPanel1.TabIndex = 5;
            this.itemPanel1.Text = "itemPanel1";
            this.itemPanel1.ItemClick += new System.EventHandler(this.itemPanel1_ItemClick);
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.ExpandableControl = this.itemPanel1;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(144, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(10, 314);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 6;
            this.expandableSplitter1.TabStop = false;
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.controlContainerItem1});
            this.contextMenuBar1.Location = new System.Drawing.Point(67, 258);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(90, 23);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.contextMenuBar1.TabIndex = 7;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // controlContainerItem1
            // 
            this.controlContainerItem1.AllowItemResize = true;
            this.controlContainerItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.controlContainerItem1.Name = "controlContainerItem1";
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnOK);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx1.Location = new System.Drawing.Point(154, 267);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(429, 47);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnCancel.Location = new System.Drawing.Point(357, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 25);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.btnOK.Location = new System.Drawing.Point(283, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 25);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupUserDBSet
            // 
            this.groupUserDBSet.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupUserDBSet.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupUserDBSet.Controls.Add(this.comBoxType);
            this.groupUserDBSet.Controls.Add(this.labelX4);
            this.groupUserDBSet.Controls.Add(this.buttonX1);
            this.groupUserDBSet.Controls.Add(this.comboBoxEx2);
            this.groupUserDBSet.Controls.Add(this.labelX3);
            this.groupUserDBSet.Controls.Add(this.btnServer);
            this.groupUserDBSet.Controls.Add(this.txtDataBase);
            this.groupUserDBSet.Controls.Add(this.labelX13);
            this.groupUserDBSet.Controls.Add(this.txtVersion);
            this.groupUserDBSet.Controls.Add(this.txtPassWord);
            this.groupUserDBSet.Controls.Add(this.txtUser);
            this.groupUserDBSet.Controls.Add(this.txtInstance);
            this.groupUserDBSet.Controls.Add(this.txtServer);
            this.groupUserDBSet.Controls.Add(this.labelX14);
            this.groupUserDBSet.Controls.Add(this.labelX15);
            this.groupUserDBSet.Controls.Add(this.labelX16);
            this.groupUserDBSet.Controls.Add(this.labelX17);
            this.groupUserDBSet.Controls.Add(this.labelX18);
            this.groupUserDBSet.DrawTitleBox = false;
            this.groupUserDBSet.Location = new System.Drawing.Point(161, 1);
            this.groupUserDBSet.Name = "groupUserDBSet";
            this.groupUserDBSet.Size = new System.Drawing.Size(420, 261);
            // 
            // 
            // 
            this.groupUserDBSet.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupUserDBSet.Style.BackColorGradientAngle = 90;
            this.groupUserDBSet.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupUserDBSet.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupUserDBSet.Style.BorderBottomWidth = 1;
            this.groupUserDBSet.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupUserDBSet.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupUserDBSet.Style.BorderLeftWidth = 1;
            this.groupUserDBSet.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupUserDBSet.Style.BorderRightWidth = 1;
            this.groupUserDBSet.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupUserDBSet.Style.BorderTopWidth = 1;
            this.groupUserDBSet.Style.CornerDiameter = 4;
            this.groupUserDBSet.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupUserDBSet.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupUserDBSet.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupUserDBSet.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupUserDBSet.TabIndex = 9;
            this.groupUserDBSet.Text = "现势库连接设置";
            // 
            // comBoxType
            // 
            this.comBoxType.DisplayMember = "Text";
            this.comBoxType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.ItemHeight = 15;
            this.comBoxType.Location = new System.Drawing.Point(88, 12);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(295, 21);
            this.comBoxType.TabIndex = 1;
            this.comBoxType.SelectedIndexChanged += new System.EventHandler(this.comBoxType_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(10, 15);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(74, 18);
            this.labelX4.TabIndex = 41;
            this.labelX4.Text = " 类    型 :";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(88, 201);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(43, 21);
            this.buttonX1.TabIndex = 9;
            this.buttonX1.Text = "连接";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // comboBoxEx2
            // 
            this.comboBoxEx2.DisplayMember = "Text";
            this.comboBoxEx2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEx2.FormattingEnabled = true;
            this.comboBoxEx2.ItemHeight = 15;
            this.comboBoxEx2.Location = new System.Drawing.Point(137, 201);
            this.comboBoxEx2.Name = "comboBoxEx2";
            this.comboBoxEx2.Size = new System.Drawing.Size(246, 21);
            this.comboBoxEx2.TabIndex = 10;
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Location = new System.Drawing.Point(16, 204);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 18);
            this.labelX3.TabIndex = 37;
            this.labelX3.Text = "数 据 集 :";
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnServer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnServer.Location = new System.Drawing.Point(357, 93);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(26, 21);
            this.btnServer.TabIndex = 5;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txtDataBase
            // 
            // 
            // 
            // 
            this.txtDataBase.Border.Class = "TextBoxBorder";
            this.txtDataBase.Location = new System.Drawing.Point(88, 93);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Size = new System.Drawing.Size(270, 21);
            this.txtDataBase.TabIndex = 4;
            this.txtDataBase.WatermarkText = "本地库路径";
            // 
            // labelX13
            // 
            this.labelX13.AutoSize = true;
            this.labelX13.Location = new System.Drawing.Point(16, 96);
            this.labelX13.Name = "labelX13";
            this.labelX13.Size = new System.Drawing.Size(68, 18);
            this.labelX13.TabIndex = 31;
            this.labelX13.Text = "数 据 库 :";
            // 
            // txtVersion
            // 
            // 
            // 
            // 
            this.txtVersion.Border.Class = "TextBoxBorder";
            this.txtVersion.Location = new System.Drawing.Point(88, 174);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(295, 21);
            this.txtVersion.TabIndex = 8;
            this.txtVersion.Text = "SDE.DEFAULT";
            // 
            // txtPassWord
            // 
            // 
            // 
            // 
            this.txtPassWord.Border.Class = "TextBoxBorder";
            this.txtPassWord.Location = new System.Drawing.Point(88, 147);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(295, 21);
            this.txtPassWord.TabIndex = 7;
            this.txtPassWord.WatermarkText = "SDE访问密码";
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(88, 120);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(295, 21);
            this.txtUser.TabIndex = 6;
            this.txtUser.WatermarkText = "SDE访问用户名";
            // 
            // txtInstance
            // 
            // 
            // 
            // 
            this.txtInstance.Border.Class = "TextBoxBorder";
            this.txtInstance.Location = new System.Drawing.Point(88, 66);
            this.txtInstance.Name = "txtInstance";
            this.txtInstance.Size = new System.Drawing.Size(295, 21);
            this.txtInstance.TabIndex = 3;
            this.txtInstance.WatermarkText = "数据库实例名称";
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.Border.Class = "TextBoxBorder";
            this.txtServer.Location = new System.Drawing.Point(88, 39);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(295, 21);
            this.txtServer.TabIndex = 2;
            this.txtServer.WatermarkText = "服务器IP地址或机器名";
            this.txtServer.WordWrap = false;
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.Location = new System.Drawing.Point(10, 177);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(74, 18);
            this.labelX14.TabIndex = 25;
            this.labelX14.Text = " 版    本 :";
            // 
            // labelX15
            // 
            this.labelX15.AutoSize = true;
            this.labelX15.Location = new System.Drawing.Point(10, 150);
            this.labelX15.Name = "labelX15";
            this.labelX15.Size = new System.Drawing.Size(74, 18);
            this.labelX15.TabIndex = 24;
            this.labelX15.Text = " 密    码 :";
            // 
            // labelX16
            // 
            this.labelX16.AutoSize = true;
            this.labelX16.Location = new System.Drawing.Point(9, 123);
            this.labelX16.Name = "labelX16";
            this.labelX16.Size = new System.Drawing.Size(74, 18);
            this.labelX16.TabIndex = 23;
            this.labelX16.Text = " 用    户 :";
            // 
            // labelX17
            // 
            this.labelX17.AutoSize = true;
            this.labelX17.Location = new System.Drawing.Point(16, 69);
            this.labelX17.Name = "labelX17";
            this.labelX17.Size = new System.Drawing.Size(68, 18);
            this.labelX17.TabIndex = 22;
            this.labelX17.Text = "实 例 名 :";
            // 
            // labelX18
            // 
            this.labelX18.AutoSize = true;
            this.labelX18.Location = new System.Drawing.Point(16, 42);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(68, 18);
            this.labelX18.TabIndex = 21;
            this.labelX18.Text = "服 务 器 :";
            // 
            // groupHisDBSet
            // 
            this.groupHisDBSet.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupHisDBSet.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupHisDBSet.Controls.Add(this.txtVersionHistory);
            this.groupHisDBSet.Controls.Add(this.labelX1);
            this.groupHisDBSet.Controls.Add(this.btnDBHistory);
            this.groupHisDBSet.Controls.Add(this.txtPasswordHistory);
            this.groupHisDBSet.Controls.Add(this.txtUserHistory);
            this.groupHisDBSet.Controls.Add(this.labelX8);
            this.groupHisDBSet.Controls.Add(this.labelX9);
            this.groupHisDBSet.Controls.Add(this.txtDBHistory);
            this.groupHisDBSet.Controls.Add(this.labelX10);
            this.groupHisDBSet.Controls.Add(this.txtInstanceHistory);
            this.groupHisDBSet.Controls.Add(this.labelX11);
            this.groupHisDBSet.Controls.Add(this.txtServerHistory);
            this.groupHisDBSet.Controls.Add(this.labelX12);
            this.groupHisDBSet.Controls.Add(this.comboBoxTypeHistory);
            this.groupHisDBSet.Controls.Add(this.labelX2);
            this.groupHisDBSet.DrawTitleBox = false;
            this.groupHisDBSet.Location = new System.Drawing.Point(161, 1);
            this.groupHisDBSet.Name = "groupHisDBSet";
            this.groupHisDBSet.Size = new System.Drawing.Size(420, 261);
            // 
            // 
            // 
            this.groupHisDBSet.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupHisDBSet.Style.BackColorGradientAngle = 90;
            this.groupHisDBSet.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupHisDBSet.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupHisDBSet.Style.BorderBottomWidth = 1;
            this.groupHisDBSet.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupHisDBSet.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupHisDBSet.Style.BorderLeftWidth = 1;
            this.groupHisDBSet.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupHisDBSet.Style.BorderRightWidth = 1;
            this.groupHisDBSet.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupHisDBSet.Style.BorderTopWidth = 1;
            this.groupHisDBSet.Style.CornerDiameter = 4;
            this.groupHisDBSet.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupHisDBSet.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupHisDBSet.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupHisDBSet.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupHisDBSet.TabIndex = 10;
            this.groupHisDBSet.Text = "历史库连接设置";
            // 
            // txtVersionHistory
            // 
            // 
            // 
            // 
            this.txtVersionHistory.Border.Class = "TextBoxBorder";
            this.txtVersionHistory.Location = new System.Drawing.Point(89, 174);
            this.txtVersionHistory.Name = "txtVersionHistory";
            this.txtVersionHistory.Size = new System.Drawing.Size(295, 21);
            this.txtVersionHistory.TabIndex = 18;
            this.txtVersionHistory.Text = "SDE.DEFAULT";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(9, 177);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 18);
            this.labelX1.TabIndex = 28;
            this.labelX1.Text = " 版    本 :";
            // 
            // btnDBHistory
            // 
            this.btnDBHistory.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDBHistory.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDBHistory.Location = new System.Drawing.Point(358, 93);
            this.btnDBHistory.Name = "btnDBHistory";
            this.btnDBHistory.Size = new System.Drawing.Size(26, 21);
            this.btnDBHistory.TabIndex = 15;
            this.btnDBHistory.Text = "...";
            this.btnDBHistory.Click += new System.EventHandler(this.btnDBHistory_Click);
            // 
            // txtPasswordHistory
            // 
            // 
            // 
            // 
            this.txtPasswordHistory.Border.Class = "TextBoxBorder";
            this.txtPasswordHistory.Location = new System.Drawing.Point(89, 147);
            this.txtPasswordHistory.Name = "txtPasswordHistory";
            this.txtPasswordHistory.PasswordChar = '*';
            this.txtPasswordHistory.Size = new System.Drawing.Size(295, 21);
            this.txtPasswordHistory.TabIndex = 17;
            // 
            // txtUserHistory
            // 
            // 
            // 
            // 
            this.txtUserHistory.Border.Class = "TextBoxBorder";
            this.txtUserHistory.Location = new System.Drawing.Point(89, 120);
            this.txtUserHistory.Name = "txtUserHistory";
            this.txtUserHistory.Size = new System.Drawing.Size(295, 21);
            this.txtUserHistory.TabIndex = 16;
            // 
            // labelX8
            // 
            this.labelX8.AutoSize = true;
            this.labelX8.Location = new System.Drawing.Point(9, 150);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(74, 18);
            this.labelX8.TabIndex = 27;
            this.labelX8.Text = " 密    码 :";
            // 
            // labelX9
            // 
            this.labelX9.AutoSize = true;
            this.labelX9.Location = new System.Drawing.Point(9, 123);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(74, 18);
            this.labelX9.TabIndex = 25;
            this.labelX9.Text = " 用 户 名 :";
            // 
            // txtDBHistory
            // 
            // 
            // 
            // 
            this.txtDBHistory.Border.Class = "TextBoxBorder";
            this.txtDBHistory.Location = new System.Drawing.Point(89, 93);
            this.txtDBHistory.Name = "txtDBHistory";
            this.txtDBHistory.Size = new System.Drawing.Size(268, 21);
            this.txtDBHistory.TabIndex = 14;
            // 
            // labelX10
            // 
            this.labelX10.AutoSize = true;
            this.labelX10.Location = new System.Drawing.Point(9, 96);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(74, 18);
            this.labelX10.TabIndex = 23;
            this.labelX10.Text = " 数 据 库 :";
            // 
            // txtInstanceHistory
            // 
            // 
            // 
            // 
            this.txtInstanceHistory.Border.Class = "TextBoxBorder";
            this.txtInstanceHistory.Location = new System.Drawing.Point(89, 66);
            this.txtInstanceHistory.Name = "txtInstanceHistory";
            this.txtInstanceHistory.Size = new System.Drawing.Size(295, 21);
            this.txtInstanceHistory.TabIndex = 13;
            // 
            // labelX11
            // 
            this.labelX11.AutoSize = true;
            this.labelX11.Location = new System.Drawing.Point(9, 69);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(74, 18);
            this.labelX11.TabIndex = 26;
            this.labelX11.Text = " 实 例 名 :";
            // 
            // txtServerHistory
            // 
            // 
            // 
            // 
            this.txtServerHistory.Border.Class = "TextBoxBorder";
            this.txtServerHistory.Location = new System.Drawing.Point(89, 39);
            this.txtServerHistory.Name = "txtServerHistory";
            this.txtServerHistory.Size = new System.Drawing.Size(295, 21);
            this.txtServerHistory.TabIndex = 12;
            // 
            // labelX12
            // 
            this.labelX12.AutoSize = true;
            this.labelX12.Location = new System.Drawing.Point(9, 42);
            this.labelX12.Name = "labelX12";
            this.labelX12.Size = new System.Drawing.Size(74, 18);
            this.labelX12.TabIndex = 24;
            this.labelX12.Text = " 服 务 器 :";
            // 
            // comboBoxTypeHistory
            // 
            this.comboBoxTypeHistory.DisplayMember = "Text";
            this.comboBoxTypeHistory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxTypeHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTypeHistory.FormattingEnabled = true;
            this.comboBoxTypeHistory.ItemHeight = 15;
            this.comboBoxTypeHistory.Location = new System.Drawing.Point(89, 12);
            this.comboBoxTypeHistory.Name = "comboBoxTypeHistory";
            this.comboBoxTypeHistory.Size = new System.Drawing.Size(295, 21);
            this.comboBoxTypeHistory.TabIndex = 11;
            this.comboBoxTypeHistory.SelectedIndexChanged += new System.EventHandler(this.comboBoxTypeHistory_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(9, 15);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 18);
            this.labelX2.TabIndex = 22;
            this.labelX2.Text = " 类    型 :";
            // 
            // frmDBsConnSet
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(583, 314);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this.itemPanel1);
            this.Controls.Add(this.groupUserDBSet);
            this.Controls.Add(this.groupHisDBSet);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(432, 269);
            this.Name = "frmDBsConnSet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "属性设置";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.groupUserDBSet.ResumeLayout(false);
            this.groupUserDBSet.PerformLayout();
            this.groupHisDBSet.ResumeLayout(false);
            this.groupHisDBSet.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmDBsConnSet());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
            object[] TagDBType = new object[] { "GDB", "SDE", "PDB" };
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;

            ///填充现势库信息
            ///
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点
            string pProjectname = pCurNode.Parent.Text;

            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            #region 填充现势库信息，并写入变量UserDBInfo、DBConn中
            System.Xml.XmlElement ProjectUserDBConnEle = ProjectNodeElement.SelectSingleNode(".//现势库/连接信息") as System.Xml.XmlElement;
            string DBType = ProjectUserDBConnEle.GetAttribute("类型");
            m_UserDBInfoConn._Key = DBType;
            if (DBType == "PDB")
            {
                comBoxType.SelectedIndex = 2;
                string path = ProjectUserDBConnEle.GetAttribute("数据库");

                this.txtDataBase.Text = path;
                m_UserDBInfoConn._DBDataBase = path;
                btnServer.Tooltip = path;
            }
            else if (DBType == "GDB")
            {
                comBoxType.SelectedIndex = 0;
                string path = ProjectUserDBConnEle.GetAttribute("数据库");

                this.txtDataBase.Text = path;
                m_UserDBInfoConn._DBDataBase = path;
                btnServer.Tooltip = path;
            }
            else
            {
                comBoxType.SelectedIndex = 1;
                this.txtServer.Text = ProjectUserDBConnEle.GetAttribute("服务器");
                m_UserDBInfoConn._DBServer = ProjectUserDBConnEle.GetAttribute("服务器");

                this.txtInstance.Text = ProjectUserDBConnEle.GetAttribute("服务名");
                m_UserDBInfoConn._DBInstance = ProjectUserDBConnEle.GetAttribute("服务名");

                this.txtDataBase.Text = ProjectUserDBConnEle.GetAttribute("数据库");
                m_UserDBInfoConn._DBDataBase = ProjectUserDBConnEle.GetAttribute("数据库");

                this.txtUser.Text = ProjectUserDBConnEle.GetAttribute("用户");
                m_UserDBInfoConn._DBUser = ProjectUserDBConnEle.GetAttribute("用户");

                this.txtPassWord.Text = ProjectUserDBConnEle.GetAttribute("密码");
                m_UserDBInfoConn._DBPsd = ProjectUserDBConnEle.GetAttribute("密码");

                this.txtVersion.Text = ProjectUserDBConnEle.GetAttribute("版本");
                m_UserDBInfoConn._DBVersion = ProjectUserDBConnEle.GetAttribute("版本"); ;
            } 
            #endregion

            #region 填充历史信息

            comboBoxTypeHistory.Items.Add("PDB");
            comboBoxTypeHistory.Items.Add("GDB");
            comboBoxTypeHistory.Items.Add("SDE");
            comboBoxTypeHistory.SelectedIndex = 1;

            ///填充历史库信息
            ///
            System.Xml.XmlElement ProjectHisDBConnEle = ProjectNodeElement.SelectSingleNode(".//历史库/连接信息") as System.Xml.XmlElement;
            string HisDBType = ProjectHisDBConnEle.GetAttribute("类型");
            comboBoxTypeHistory.Text = HisDBType;
            if (HisDBType == "PDB")
            {
                
                string path = ProjectHisDBConnEle.GetAttribute("数据库");

                this.txtDBHistory.Text = path;
                btnDBHistory.Tooltip = path;
            }
            else if (HisDBType == "GDB")
            {
                string path = ProjectHisDBConnEle.GetAttribute("数据库");

                this.txtDBHistory.Text = path;
                btnDBHistory.Tooltip = path;
            }
            else if (HisDBType == "SDE")
            {
                this.txtServerHistory.Text = ProjectHisDBConnEle.GetAttribute("服务器");

                this.txtInstanceHistory.Text = ProjectHisDBConnEle.GetAttribute("服务名");

                this.txtDBHistory.Text = ProjectHisDBConnEle.GetAttribute("数据库");

                this.txtUserHistory.Text = ProjectHisDBConnEle.GetAttribute("用户");

                this.txtPasswordHistory.Text = ProjectHisDBConnEle.GetAttribute("密码");

                this.txtVersionHistory.Text = ProjectHisDBConnEle.GetAttribute("版本");
            } 
            #endregion


			itemPanel1.BeginUpdate();
            ///加载现势库设置节点
            ///
            ButtonItem buttonUserDB = new ButtonItem("现势库", "现势库");
            buttonUserDB.OptionGroup = "DBsSet";
            itemPanel1.Items.Add(buttonUserDB);
            buttonUserDB.Checked = true;

            /////加载业务对象设置节点
            //ButtonItem buttonCaseObject = new ButtonItem("业务对象", "业务对象");
            //buttonCaseObject.OptionGroup = "DBsSet";
            //itemPanel1.Items.Add(buttonCaseObject);

            /////加载更新环境设置节点
            /////
            //ButtonItem buttonUpdateEnv = new ButtonItem("更新环境", "更新环境");
            //buttonUpdateEnv.OptionGroup = "DBsSet";
            //itemPanel1.Items.Add(buttonUpdateEnv);

            ///加载历史库设置节点
            ///
            ButtonItem buttonHisDB = new ButtonItem("历史库", "历史库");
            buttonHisDB.OptionGroup = "DBsSet";
            itemPanel1.Items.Add(buttonHisDB);

			itemPanel1.EndUpdate();
            //guozheng 2011-2-25 added 修改：读取界面上的库体节点选择名称初始化界面
            string sSelectNodeName = ModData.v_AppGIS.ProjectTree.SelectedNode.Text;
            if (sSelectNodeName == "现势库")
            {
                buttonUserDB.Checked = true;
                buttonHisDB.Checked = false;
                groupUserDBSet.Visible = true;
                groupHisDBSet.Visible = false;
            }
            else if (sSelectNodeName == "历史库")
            {
                buttonUserDB.Checked = false;
                buttonHisDB.Checked = true;
                groupHisDBSet.Visible = true;
                groupUserDBSet.Visible = false;
            }
            else
            {
            }
            ///从已有的文件中读取信息
            ///展现到界面上


		}

		private void itemPanel1_ItemClick(object sender, System.EventArgs e)
		{
			ButtonItem button = sender as ButtonItem;
			if(button==null)
				return;
            switch (button.Name)
            {
                case "现势库":
                    groupUserDBSet.Visible = true;

                    groupHisDBSet.Visible = false;
                    break;
                case "历史库":
                    groupHisDBSet.Visible = true;

                    groupUserDBSet.Visible = false;
                    break;

                default:
                    break;
            }
            //eTabStripStyle style = (eTabStripStyle)Enum.Parse(typeof(eTabStripStyle), button.Name);
			itemPanel1.RecalcLayout();
		}

        private void btnServer_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB");
                            return;
                        }
                        txtDataBase.Text = pFolderBrowser.SelectedPath;
                    }
                    break;

                case "PDB":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {

            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点
            string pProjectname = pCurNode.Parent.Text;

            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;
            //guozheng 修改，增加判断，是修改用户库还是修改历史库
            //还要增加连接信息的判断
            if (this.groupUserDBSet.Visible)
            {
                if (!GeoDataBaseConnectTest(true)) return;
                #region 写入现势库信息
                ///写入现势库信息
                System.Xml.XmlElement ProjectConnEle = ProjectNodeElement.SelectSingleNode(".//现势库/连接信息") as System.Xml.XmlElement;
                ///设置数据库连接类型
                if (comBoxType.SelectedIndex == 2)
                {
                    ProjectConnEle.SetAttribute("类型", "PDB");
                    ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);

                }
                else if (comBoxType.SelectedIndex == 0)
                {
                    ProjectConnEle.SetAttribute("类型", "GDB");
                    ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
                }
                else if (comBoxType.SelectedIndex == 1)
                {
                    ProjectConnEle.SetAttribute("类型", "SDE");
                    ProjectConnEle.SetAttribute("服务器", txtServer.Text);
                    ProjectConnEle.SetAttribute("服务名", txtInstance.Text);
                    ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
                    ProjectConnEle.SetAttribute("用户", txtUser.Text);
                    ProjectConnEle.SetAttribute("密码", txtPassWord.Text);
                    ProjectConnEle.SetAttribute("版本", txtVersion.Text);
                }

                //设置数据集名称
                //if (comboBoxEx2.Text.Trim() == "")
                //{
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择数据集！");
                //    return;
                //}
                System.Xml.XmlElement ProjectUserDSEle = ProjectConnEle.SelectSingleNode(".//库体") as System.Xml.XmlElement;
                ProjectUserDSEle.SetAttribute("名称", comboBoxEx2.Text.Trim());
                #endregion
            }
            if (this.groupHisDBSet.Visible)
            {
                if (!GeoDataBaseConnectTest(false)) return;
                #region 写入历史库连接信息
                System.Xml.XmlElement ProjectHisConnEle = ProjectNodeElement.SelectSingleNode(".//历史库/连接信息") as System.Xml.XmlElement;
                ///设置数据库连接类型
                if (comboBoxTypeHistory.Text == "PDB")
                {
                    ProjectHisConnEle.SetAttribute("类型", "PDB");
                    ProjectHisConnEle.SetAttribute("数据库", txtDBHistory.Text);

                }
                else if (comboBoxTypeHistory.Text == "GDB")
                {
                    ProjectHisConnEle.SetAttribute("类型", "GDB");
                    ProjectHisConnEle.SetAttribute("数据库", txtDBHistory.Text);
                }
                else
                {
                    ProjectHisConnEle.SetAttribute("类型", "SDE");
                    ProjectHisConnEle.SetAttribute("服务器", txtServerHistory.Text);
                    ProjectHisConnEle.SetAttribute("服务名", txtInstanceHistory.Text);
                    ProjectHisConnEle.SetAttribute("数据库", txtDBHistory.Text);
                    ProjectHisConnEle.SetAttribute("用户", txtUserHistory.Text);
                    ProjectHisConnEle.SetAttribute("密码", txtPasswordHistory.Text);
                    ProjectHisConnEle.SetAttribute("版本", txtVersionHistory.Text);
                }
                #endregion
            }


            m_Hook.DBXmlDocument.Save(ModData.v_projectXML);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxTypeHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServerHistory.Text = "";
            txtInstanceHistory.Text = "";
            txtDBHistory.Text = "";
            txtUserHistory.Text = "";
            txtPasswordHistory.Text = "";
            if (comboBoxTypeHistory.Text != "SDE")
            {
                btnDBHistory.Visible = true;
                txtDBHistory.Size = new Size(txtServerHistory.Size.Width - btnDBHistory.Size.Width, txtDBHistory.Size.Height);
                txtServerHistory.Enabled = false;
                txtInstanceHistory.Enabled = false;
                txtUserHistory.Enabled = false;
                txtPasswordHistory.Enabled = false;
                txtVersionHistory.Enabled = false;
            }
            else
            {
                btnDBHistory.Visible = false;
                txtDBHistory.Size = new Size(txtServerHistory.Size.Width, txtDBHistory.Size.Height);
                txtServerHistory.Enabled = true;
                txtInstanceHistory.Enabled = true;
                txtUserHistory.Enabled = true;
                txtPasswordHistory.Enabled = true;
                txtVersionHistory.Enabled = true;

            }
        }

        private void btnDBHistory_Click(object sender, EventArgs e)
        {
            switch (comboBoxTypeHistory.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB");
                            return;
                        }
                        txtDBHistory.Text = pFolderBrowser.SelectedPath;
                    }
                    break;

                case "PDB":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDBHistory.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (!GeoDataBaseConnectTest(true)) return;
            IWorkspace DesWorkspace = null;
            IWorkspaceFactory pWorkspaceFactory = null;
            IPropertySet pPropertySet = new PropertySetClass();

            comboBoxEx2.Items.Clear();

            try
            {
                if (comBoxType.SelectedIndex == 1)  //SDE库
                {
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                    pPropertySet.SetProperty("SERVER", txtServer.Text);
                    pPropertySet.SetProperty("INSTANCE", txtInstance.Text);
                    pPropertySet.SetProperty("USER", txtUser.Text);
                    pPropertySet.SetProperty("PASSWORD", txtPassWord.Text);
                    pPropertySet.SetProperty("VERSION", txtVersion.Text);

                }
                else if (comBoxType.SelectedIndex == 0)   //GDB库
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pPropertySet.SetProperty("DATABASE", txtDataBase.Text);
                }
                else if(comBoxType.SelectedIndex==2)   //PDB库体
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    pPropertySet.SetProperty("DATABASE", txtDataBase.Text);
                }

                DesWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                if (DesWorkspace == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！");
                    return ;
                }

                IEnumDatasetName pEnumDatasetName = DesWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);

                if (pEnumDatasetName == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "找不到数据集！");
                    return; 
                }

                IDatasetName pDatasetName = pEnumDatasetName.Next();

                while (pDatasetName != null)
                {

                    comboBoxEx2.Items.Add(pDatasetName.Name);
                    pDatasetName = pEnumDatasetName.Next();
                }

                if (comboBoxEx2.Items.Count > 0)
                {
                    comboBoxEx2.SelectedIndex = 0;
                }
            }
            catch(Exception er)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                //********************************************************************

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！");
                return;
            }
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text = "";
            txtInstance.Text = "";
            txtDataBase.Text = "";
            txtUser.Text = "";
            txtPassWord.Text = "";
            if (comBoxType.Text != "SDE")
            {
                btnServer.Visible = true;
                txtDataBase.Size = new Size(txtServer.Size.Width - btnServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnServer.Visible = false;
                txtDataBase.Size = new Size(txtServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                txtVersion.Enabled = true;

            }
        }

        /// <summary>
        ///  2011-2-25 guozheng added，判断填写的库体连接信息是否正确，不正确函数中有界面提示
        /// </summary>
        /// <param name="IsUserDB">是否是用户库</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool GeoDataBaseConnectTest(bool IsUserDB)
        {
            try
            {
                if (IsUserDB)
                {
                    //////判断用户库连接测试
                    #region 判断用户库连接测试
                    switch (this.comBoxType.Text.Trim())
                    {
                        case "SDE":
                            if (string.IsNullOrEmpty(this.txtServer.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE服务名");
                                this.txtServer.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtInstance.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE实例名");
                                this.txtInstance.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtUser.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE用户名");
                                this.txtUser.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtPassWord.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE用户密码");
                                this.txtPassWord.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtVersion.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE版本");
                                this.txtVersion.Focus();
                                return false;
                            }
                            
                            IPropertySet pPropSet = new PropertySetClass();
                            IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                            pPropSet.SetProperty("SERVER", this.txtServer.Text);
                            pPropSet.SetProperty("INSTANCE", this.txtInstance.Text);
                            pPropSet.SetProperty("DATABASE", this.txtDataBase.Text);
                            pPropSet.SetProperty("USER", this.txtUser.Text);
                            pPropSet.SetProperty("PASSWORD", this.txtPassWord.Text);
                            pPropSet.SetProperty("VERSION", this.txtVersion.Text);
                            try
                            {
                                IWorkspace _Workspace = pSdeFact.Open(pPropSet, 0);
                                pPropSet = null;
                                pSdeFact = null;
                                return true;
                            }
                            catch(Exception eError)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接SDE失败，\n原因："+eError.Message);
                                if (ModData.SysLog == null) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                ModData.SysLog.Write(eError);
                                return false;
                            }
                            break;
                        case "PDB":
                            if (string.IsNullOrEmpty(this.txtDataBase.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请指定一个PDB文件");
                                this.txtDataBase.Focus();
                                return false;
                            }
                            if (!System.IO.File.Exists(this.txtDataBase.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "本地PDB文件：" + this.txtDataBase.Text + " 不存在");
                                this.txtDataBase.Focus();
                                return false;
                            }
                            break;
                        case "GDB":
                            if (string.IsNullOrEmpty(this.txtDataBase.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请指定一个GDB文件库");
                                this.txtDataBase.Focus();
                                return false;
                            }
                            if (!System.IO.Directory.Exists(this.txtDataBase.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "本地GDB文件库：" + this.txtDataBase.Text + " 不存在");
                                this.txtDataBase.Focus();
                                return false;
                            }
                            break;
                        default:
                            break;


                    }
                    #endregion

                }
                else
                {
                    //////判断历史库连接测试
                    #region 判断历史库连接测试
                    switch (this.comboBoxTypeHistory.Text.Trim())
                    {
                        case "SDE":
                            if (string.IsNullOrEmpty(this.txtServerHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE服务名");
                                this.txtServerHistory.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtInstanceHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE实例名");
                                this.txtInstanceHistory.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtUserHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE用户名");
                                this.txtUserHistory.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtPasswordHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE用户密码");
                                this.txtPasswordHistory.Focus();
                                return false;
                            }
                            if (string.IsNullOrEmpty(this.txtVersionHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入SDE版本");
                                this.txtVersionHistory.Focus();
                                return false;
                            }
                            
                            IPropertySet pPropSet = new PropertySetClass();
                            IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                            pPropSet.SetProperty("SERVER", this.txtServerHistory.Text);
                            pPropSet.SetProperty("INSTANCE", this.txtInstanceHistory.Text);
                            pPropSet.SetProperty("DATABASE", this.txtDBHistory.Text);
                            pPropSet.SetProperty("USER", this.txtUserHistory.Text);
                            pPropSet.SetProperty("PASSWORD", this.txtPasswordHistory.Text);
                            pPropSet.SetProperty("VERSION", this.txtVersionHistory.Text);

                            try
                            {
                                IWorkspace _Workspace = pSdeFact.Open(pPropSet, 0);
                                pPropSet = null;
                                pSdeFact = null;
                                return true;
                            }
                            catch (Exception eError)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接SDE失败，\n原因：" + eError.Message);
                                if (ModData.SysLog == null) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                ModData.SysLog.Write(eError);
                                return false;
                            }
                            break;
                        case "PDB":
                            if (string.IsNullOrEmpty(this.txtDBHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请指定一个PDB文件");
                                this.txtDBHistory.Focus();
                                return false;
                            }
                            if (!System.IO.File.Exists(this.txtDBHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "本地PDB文件：" + this.txtDBHistory.Text + " 不存在");
                                this.txtDBHistory.Focus();
                                return false;
                            }
                            break;
                        case "GDB":
                            if (string.IsNullOrEmpty(this.txtDBHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请指定一个GDB文件库");
                                this.txtDBHistory.Focus();
                                return false;
                            }
                            if (!System.IO.Directory.Exists(this.txtDBHistory.Text))
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "本地GDB文件库：" + this.txtDBHistory.Text + " 不存在");
                                this.txtDBHistory.Focus();
                                return false;
                            }
                            break;
                        default:
                            break;


                    }
                    #endregion
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
	}

    /// <summary>
    /// 数据库连接信息结构
    /// </summary>
    public struct CustomDBConnInfomation
    { 
        /// <summary>
        /// 数据库关键字
        /// </summary>
        public  string _KeyName;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string _DBType;

        public  struct DBSet
        {

            public string _Key;
            /// <summary>
            /// 服务器名称
            /// </summary>
            public string _DBServer;

            /// <summary>
            /// 实例名
            /// </summary>
            public string _DBInstance;

            /// <summary>
            /// 数据库名称
            /// </summary>
            public string _DBDataBase;

            /// <summary>
            /// 用户名
            /// </summary>
            public string _DBUser;

            /// <summary>
            /// 访问密码
            /// </summary>
            public string _DBPsd;

            /// <summary>
            /// 版本
            /// </summary>
            public string _DBVersion;

            public DBSet(string Key,string DBServer, string DBInstance, string DBDataBase, string DBUser, string DBPsd, string DBVersion)
            {
                _Key = Key;
                _DBServer = DBServer;
                _DBInstance = DBInstance;
                _DBDataBase = DBDataBase;
                _DBUser = DBUser;
                _DBPsd = DBPsd;
                _DBVersion = DBVersion;

            }
        }

        /// <summary>
        /// 数据集名称
        /// </summary>
        public string _DSName;

        public  CustomDBConnInfomation(string Key)
        {
            _KeyName = Key;
            _DBType = "";
            _DSName = "";
        }
    }
}
