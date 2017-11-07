using System;
using System.Collections.Generic;
using System.Text;
using DevDNB = DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;

namespace GeoSymbology
{
    public class frmMFUVRenderer : System.Windows.Forms.UserControl, IEditItem, IRendererUI
    {
        #region InitializeComponent

        private DevComponents.DotNetBar.PanelEx panelEx3;
        private DevComponents.DotNetBar.PanelEx panelEx4;
        private DevComponents.DotNetBar.LabelX labelPreviewFore;
        private DevComponents.DotNetBar.PanelEx panelEx5;
        private DevComponents.DotNetBar.ButtonX buttonDown;
        private DevComponents.DotNetBar.ButtonX buttonUp;
        private DevComponents.DotNetBar.PanelEx panelEx6;
        private DevComponents.DotNetBar.Controls.ListViewEx listValueItem;
        private DevComponents.Editors.IntegerInput integerInput4;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.ButtonX buttonDelAllValue;
        private DevComponents.DotNetBar.ButtonX buttonDelValue;
        private DevComponents.DotNetBar.ButtonX buttonAddFromSymbol;
        private DevComponents.DotNetBar.ButtonX buttonAddValue;
        private DevComponents.DotNetBar.ButtonX buttonAddAllValue;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbField1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbField3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbField2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbColorRamp;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMFUVRenderer));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.panelEx3 = new DevComponents.DotNetBar.PanelEx();
            this.buttonDelAllValue = new DevComponents.DotNetBar.ButtonX();
            this.buttonDelValue = new DevComponents.DotNetBar.ButtonX();
            this.buttonAddFromSymbol = new DevComponents.DotNetBar.ButtonX();
            this.buttonAddValue = new DevComponents.DotNetBar.ButtonX();
            this.buttonAddAllValue = new DevComponents.DotNetBar.ButtonX();
            this.panelEx5 = new DevComponents.DotNetBar.PanelEx();
            this.buttonDown = new DevComponents.DotNetBar.ButtonX();
            this.buttonUp = new DevComponents.DotNetBar.ButtonX();
            this.panelEx6 = new DevComponents.DotNetBar.PanelEx();
            this.listValueItem = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.integerInput4 = new DevComponents.Editors.IntegerInput();
            this.panelEx4 = new DevComponents.DotNetBar.PanelEx();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbColorRamp = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbField1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbField2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbField3 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelPreviewFore = new DevComponents.DotNetBar.LabelX();
            this.panelEx3.SuspendLayout();
            this.panelEx5.SuspendLayout();
            this.panelEx6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.integerInput4)).BeginInit();
            this.panelEx4.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(405, 350);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // panelEx3
            // 
            this.panelEx3.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx3.Controls.Add(this.buttonDelAllValue);
            this.panelEx3.Controls.Add(this.buttonDelValue);
            this.panelEx3.Controls.Add(this.buttonAddFromSymbol);
            this.panelEx3.Controls.Add(this.buttonAddValue);
            this.panelEx3.Controls.Add(this.buttonAddAllValue);
            this.panelEx3.Controls.Add(this.panelEx5);
            this.panelEx3.Controls.Add(this.panelEx4);
            this.panelEx3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx3.Location = new System.Drawing.Point(0, 0);
            this.panelEx3.Name = "panelEx3";
            this.panelEx3.Size = new System.Drawing.Size(465, 370);
            this.panelEx3.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx3.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx3.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx3.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx3.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx3.Style.GradientAngle = 90;
            this.panelEx3.TabIndex = 0;
            // 
            // buttonDelAllValue
            // 
            this.buttonDelAllValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDelAllValue.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDelAllValue.Location = new System.Drawing.Point(279, 339);
            this.buttonDelAllValue.Name = "buttonDelAllValue";
            this.buttonDelAllValue.Size = new System.Drawing.Size(75, 23);
            this.buttonDelAllValue.TabIndex = 56;
            this.buttonDelAllValue.Text = "删除所有值";
            this.buttonDelAllValue.Click += new System.EventHandler(this.Control_Click);
            // 
            // buttonDelValue
            // 
            this.buttonDelValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDelValue.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDelValue.Location = new System.Drawing.Point(192, 339);
            this.buttonDelValue.Name = "buttonDelValue";
            this.buttonDelValue.Size = new System.Drawing.Size(80, 23);
            this.buttonDelValue.TabIndex = 55;
            this.buttonDelValue.Text = "删除值";
            this.buttonDelValue.Click += new System.EventHandler(this.Control_Click);
            // 
            // buttonAddFromSymbol
            // 
            this.buttonAddFromSymbol.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAddFromSymbol.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddFromSymbol.Location = new System.Drawing.Point(374, 339);
            this.buttonAddFromSymbol.Name = "buttonAddFromSymbol";
            this.buttonAddFromSymbol.Size = new System.Drawing.Size(88, 23);
            this.buttonAddFromSymbol.TabIndex = 54;
            this.buttonAddFromSymbol.Text = "从符号库添加";
            this.buttonAddFromSymbol.Visible = false;
            this.buttonAddFromSymbol.Click += new System.EventHandler(this.Control_Click);
            // 
            // buttonAddValue
            // 
            this.buttonAddValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAddValue.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddValue.Location = new System.Drawing.Point(89, 339);
            this.buttonAddValue.Name = "buttonAddValue";
            this.buttonAddValue.Size = new System.Drawing.Size(80, 23);
            this.buttonAddValue.TabIndex = 53;
            this.buttonAddValue.Text = "添加值";
            this.buttonAddValue.Click += new System.EventHandler(this.Control_Click);
            // 
            // buttonAddAllValue
            // 
            this.buttonAddAllValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonAddAllValue.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonAddAllValue.Location = new System.Drawing.Point(3, 339);
            this.buttonAddAllValue.Name = "buttonAddAllValue";
            this.buttonAddAllValue.Size = new System.Drawing.Size(80, 23);
            this.buttonAddAllValue.TabIndex = 52;
            this.buttonAddAllValue.Text = "添加所有值";
            this.buttonAddAllValue.Click += new System.EventHandler(this.Control_Click);
            // 
            // panelEx5
            // 
            this.panelEx5.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx5.Controls.Add(this.buttonDown);
            this.panelEx5.Controls.Add(this.buttonUp);
            this.panelEx5.Controls.Add(this.panelEx6);
            this.panelEx5.Location = new System.Drawing.Point(3, 118);
            this.panelEx5.Name = "panelEx5";
            this.panelEx5.Size = new System.Drawing.Size(459, 215);
            this.panelEx5.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx5.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx5.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx5.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx5.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx5.Style.GradientAngle = 90;
            this.panelEx5.TabIndex = 51;
            // 
            // buttonDown
            // 
            this.buttonDown.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonDown.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDown.Image")));
            this.buttonDown.ImagePosition = DevComponents.DotNetBar.eImagePosition.Bottom;
            this.buttonDown.Location = new System.Drawing.Point(429, 112);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(22, 35);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Click += new System.EventHandler(this.Control_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonUp.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
            this.buttonUp.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
            this.buttonUp.Location = new System.Drawing.Point(429, 68);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(22, 35);
            this.buttonUp.TabIndex = 3;
            this.buttonUp.Click += new System.EventHandler(this.Control_Click);
            // 
            // panelEx6
            // 
            this.panelEx6.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx6.Controls.Add(this.listValueItem);
            this.panelEx6.Controls.Add(this.integerInput4);
            this.panelEx6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEx6.Location = new System.Drawing.Point(0, 0);
            this.panelEx6.Name = "panelEx6";
            this.panelEx6.Size = new System.Drawing.Size(417, 215);
            this.panelEx6.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx6.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx6.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx6.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx6.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx6.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx6.Style.GradientAngle = 90;
            this.panelEx6.TabIndex = 0;
            // 
            // listValueItem
            // 
            // 
            // 
            // 
            this.listValueItem.Border.Class = "ListViewBorder";
            this.listValueItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listValueItem.FullRowSelect = true;
            this.listValueItem.Location = new System.Drawing.Point(0, 0);
            this.listValueItem.Name = "listValueItem";
            this.listValueItem.Size = new System.Drawing.Size(417, 215);
            this.listValueItem.TabIndex = 1;
            this.listValueItem.UseCompatibleStateImageBehavior = false;
            this.listValueItem.View = System.Windows.Forms.View.Details;
            this.listValueItem.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DoListValueItemMouseDoubleClick);
            // 
            // integerInput4
            // 
            // 
            // 
            // 
            this.integerInput4.BackgroundStyle.Class = "DateTimeInputBackground";
            this.integerInput4.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.integerInput4.Location = new System.Drawing.Point(86, 68);
            this.integerInput4.Name = "integerInput4";
            this.integerInput4.ShowUpDown = true;
            this.integerInput4.Size = new System.Drawing.Size(80, 21);
            this.integerInput4.TabIndex = 15;
            // 
            // panelEx4
            // 
            this.panelEx4.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx4.Controls.Add(this.groupPanel1);
            this.panelEx4.Controls.Add(this.groupPanel3);
            this.panelEx4.Controls.Add(this.labelX6);
            this.panelEx4.Controls.Add(this.labelPreviewFore);
            this.panelEx4.Location = new System.Drawing.Point(3, 3);
            this.panelEx4.Name = "panelEx4";
            this.panelEx4.Size = new System.Drawing.Size(459, 109);
            this.panelEx4.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx4.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx4.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx4.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx4.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx4.Style.GradientAngle = 90;
            this.panelEx4.TabIndex = 48;
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.cmbColorRamp);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(210, 2);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(244, 60);
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
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 50;
            this.groupPanel1.Text = "颜色方案";
            // 
            // cmbColorRamp
            // 
            this.cmbColorRamp.DisplayMember = "Text";
            this.cmbColorRamp.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbColorRamp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColorRamp.FormattingEnabled = true;
            this.cmbColorRamp.ItemHeight = 21;
            this.cmbColorRamp.Location = new System.Drawing.Point(8, 2);
            this.cmbColorRamp.Margin = new System.Windows.Forms.Padding(0);
            this.cmbColorRamp.Name = "cmbColorRamp";
            this.cmbColorRamp.Size = new System.Drawing.Size(227, 27);
            this.cmbColorRamp.TabIndex = 49;
            this.cmbColorRamp.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // groupPanel3
            // 
            this.groupPanel3.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.cmbField1);
            this.groupPanel3.Controls.Add(this.cmbField2);
            this.groupPanel3.Controls.Add(this.cmbField3);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(8, 2);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(196, 102);
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
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 50;
            this.groupPanel3.Text = "分类字段";
            // 
            // cmbField1
            // 
            this.cmbField1.DisplayMember = "Text";
            this.cmbField1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbField1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField1.FormattingEnabled = true;
            this.cmbField1.ItemHeight = 15;
            this.cmbField1.Location = new System.Drawing.Point(7, 3);
            this.cmbField1.Name = "cmbField1";
            this.cmbField1.Size = new System.Drawing.Size(184, 21);
            this.cmbField1.TabIndex = 46;
            this.cmbField1.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // cmbField2
            // 
            this.cmbField2.DisplayMember = "Text";
            this.cmbField2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbField2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField2.FormattingEnabled = true;
            this.cmbField2.ItemHeight = 15;
            this.cmbField2.Location = new System.Drawing.Point(7, 29);
            this.cmbField2.Name = "cmbField2";
            this.cmbField2.Size = new System.Drawing.Size(184, 21);
            this.cmbField2.TabIndex = 47;
            this.cmbField2.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // cmbField3
            // 
            this.cmbField3.DisplayMember = "Text";
            this.cmbField3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbField3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbField3.FormattingEnabled = true;
            this.cmbField3.ItemHeight = 15;
            this.cmbField3.Location = new System.Drawing.Point(7, 54);
            this.cmbField3.Name = "cmbField3";
            this.cmbField3.Size = new System.Drawing.Size(184, 21);
            this.cmbField3.TabIndex = 48;
            this.cmbField3.SelectedIndexChanged += new System.EventHandler(this.Control_ValueChanged);
            // 
            // labelX6
            // 
            this.labelX6.AutoSize = true;
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Location = new System.Drawing.Point(291, 59);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(19, 31);
            this.labelX6.TabIndex = 44;
            this.labelX6.Text = "符\r\n号";
            this.labelX6.Visible = false;
            // 
            // labelPreviewFore
            // 
            this.labelPreviewFore.BackColor = System.Drawing.Color.Transparent;
            this.labelPreviewFore.Location = new System.Drawing.Point(315, 54);
            this.labelPreviewFore.Name = "labelPreviewFore";
            this.labelPreviewFore.Size = new System.Drawing.Size(80, 40);
            this.labelPreviewFore.TabIndex = 43;
            this.labelPreviewFore.Visible = false;
            this.labelPreviewFore.Click += new System.EventHandler(this.Control_Click);
            // 
            // frmMFUVRenderer
            // 
            this.Controls.Add(this.panelEx3);
            this.Name = "frmMFUVRenderer";
            this.Size = new System.Drawing.Size(465, 370);
            this.panelEx3.ResumeLayout(false);
            this.panelEx5.ResumeLayout(false);
            this.panelEx6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.integerInput4)).EndInit();
            this.panelEx4.ResumeLayout(false);
            this.panelEx4.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private esriSymbologyStyleClass m_SymbologyStyleClass;
        private bool flag = false;
        private IFeatureLayer m_Layer;

        public frmMFUVRenderer()
        {
            InitializeComponent();
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            m_EditObject = null;
            listValueItem.SmallImageList = new System.Windows.Forms.ImageList();
            listValueItem.SmallImageList.ImageSize = new System.Drawing.Size(ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);

            System.Windows.Forms.ColumnHeader column = new System.Windows.Forms.ColumnHeader();
            column.Name = "Symbol";
            column.Text = "符号";
            column.Width = 80;
            column.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            listValueItem.Columns.Add(column);

            column = new System.Windows.Forms.ColumnHeader();
            column.Name = "Range";
            column.Text = "范围";
            column.Width = 146;
            column.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listValueItem.Columns.Add(column);

            column = new System.Windows.Forms.ColumnHeader();
            column.Name = "Label";
            column.Text = "标签";
            column.Width = 146;
            column.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            listValueItem.Columns.Add(column);

            List<ColorItem> colorRamps = new ModuleCommon().GetColorScheme(207, 20, "Default Schemes;Spatial Ramps");
            for (int i = 0; i < colorRamps.Count; i++)
            {
                DevComponents.Editors.ComboItem item = new DevComponents.Editors.ComboItem();
                item.Image = colorRamps[i].ColorImage;
                item.Text = colorRamps[i].Name;
                item.Tag = colorRamps[i].ColorRamp;
                cmbColorRamp.Items.Add(item);
            }
            flag = true;
            cmbColorRamp.SelectedIndex = 0;
            flag = false;
        }

        private void Control_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Control control = sender as  System.Windows.Forms.Control;
            switch (control.Name)
            {
                case "buttonAddFromSymbol":
                    m_EditObject = control;
                    Form.frmSymbolEdit foreEdit = new GeoSymbology.Form.frmSymbolEdit(this, ModuleCommon.CreateSymbol(m_SymbologyStyleClass), "");
                    foreEdit.ShowDialog();
                    break;
                case "buttonUp":
                    {
                        if (listValueItem.SelectedItems.Count != 1) return;
                        System.Windows.Forms.ListViewItem item = listValueItem.SelectedItems[0];
                        int index = item.Index;
                        if (index <= 1) return;
                        listValueItem.Items.Remove(item);
                        listValueItem.Items.Insert(index - 1,item);
                    }
                    break;
                case "buttonDown":
                    {
                        if (listValueItem.SelectedItems.Count != 1) return;
                        System.Windows.Forms.ListViewItem item = listValueItem.SelectedItems[0];
                        int index = item.Index;
                        if (index == listValueItem.Items.Count - 1) return;
                        listValueItem.Items.Remove(item);
                        listValueItem.Items.Insert(index + 1,item);
                    }
                    break;
                case "buttonAddAllValue":
                    AddAllValue();
                    RefreshColorRamp();//yjl20110826
                    break;
                case "buttonAddValue":
                    AddValue();
                    break;
                case "buttonDelValue":
                    for (int i = listValueItem.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        string nameIndex = listValueItem.SelectedItems[i].Name.Replace("Item", "");
                        if (nameIndex == "Default") continue;
                        listValueItem.Items.RemoveByKey("Item" + nameIndex);
                        listValueItem.SmallImageList.Images.RemoveByKey("Symbol" + nameIndex);
                    }
                    break;
                case "buttonDelAllValue":
                    DeleteAllValue();
                    break;                
                case "labelPreviewFore":
                    m_EditObject = control;
                    Form.frmSymbolEdit foreEdit1 = new GeoSymbology.Form.frmSymbolEdit(this, control.Tag as ISymbol, "");
                    foreEdit1.ShowDialog();
                    break;
            }
        }

        private void DoListValueItemMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.ListViewItem item = listValueItem.GetItemAt(e.X, e.Y);
            if (item == null) return;

            System.Drawing.Rectangle rec = item.GetBounds(System.Windows.Forms.ItemBoundsPortion.Entire);
            if (e.X <= listValueItem.Columns[0].Width)
            {
                //符号编辑
                m_EditObject = item;
                Form.frmSymbolEdit frm = new GeoSymbology.Form.frmSymbolEdit(this, item.Tag as ISymbol, "");
                frm.ShowDialog();
            }
            else if (e.X > listValueItem.Columns[0].Width &&
                e.X <= (listValueItem.Columns[1].Width + listValueItem.Columns[0].Width))
            {
                if (item.Name.Contains("Default")) return;
                m_EditObject = item.SubItems[1];
                //范围编辑
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = listValueItem.Columns[0].Width;
                point.Y = rec.Top + (rec.Height - Form.frmDoubleEdit.FormWidth) / 2;
                point = listValueItem.PointToScreen(point);
                Form.frmStringEdit stringEdit = new GeoSymbology.Form.frmStringEdit(this,
                    item.SubItems[1].Text, point, listValueItem.Columns[1].Width,"");
                stringEdit.Show();
            }
            else if (e.X > (listValueItem.Columns[1].Width + listValueItem.Columns[0].Width)
                && e.X <= (listValueItem.Columns[0].Width + listValueItem.Columns[1].Width + listValueItem.Columns[2].Width))
            {
                if (item.Name.Contains("Default")) return;
                m_EditObject = item.SubItems[2];
                //标签编辑
                System.Drawing.Point point = new System.Drawing.Point();
                point.X = listValueItem.Columns[0].Width + listValueItem.Columns[1].Width;
                point.Y = rec.Top + (rec.Height - Form.frmStringEdit.FormWidth) / 2;
                point = listValueItem.PointToScreen(point);
                Form.frmStringEdit stringEdit = new GeoSymbology.Form.frmStringEdit(this,
                    item.SubItems[2].Text, point, listValueItem.Columns[2].Width, "");
                stringEdit.Show();
            }
        }

        #region IEditItem 成员

        private object m_EditObject;

        public void DoAfterEdit(object newValue, System.Windows.Forms.DialogResult result,string editType)
        {
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                if (editType=="NEW"&&result == System.Windows.Forms.DialogResult.Cancel && m_EditObject is System.Windows.Forms.ListViewItem.ListViewSubItem)
                {
                    System.Windows.Forms.ListViewItem.ListViewSubItem subItem =
                    m_EditObject as System.Windows.Forms.ListViewItem.ListViewSubItem;
                    if (subItem.Name.Contains("Range"))
                    {
                        string itemName = subItem.Name.Replace("Range", "Item");
                        listValueItem.Items.RemoveByKey(itemName);
                    }
                }
                m_EditObject = null;
                return;
            }
            if (m_EditObject is System.Windows.Forms.ListViewItem.ListViewSubItem)
            {
                System.Windows.Forms.ListViewItem.ListViewSubItem subItem =
                    m_EditObject as System.Windows.Forms.ListViewItem.ListViewSubItem;
                if (subItem.Name.Contains("Range"))//范围编辑
                {
                    if (editType == "NEW")
                    {
                        if (listValueItem.Items.ContainsKey("Item" + newValue.ToString()))
                        {
                            string itemName = subItem.Name.Replace("Range", "Item");
                            listValueItem.Items.RemoveByKey(itemName);
                        }
                        else
                        {
                            if (newValue.ToString() != "")
                            {
                                UpdateValue(subItem, newValue.ToString());
                            }
                            else
                            {
                                string itemName = subItem.Name.Replace("Range", "Item");
                                listValueItem.Items.RemoveByKey(itemName);
                            }
                        }
                    }
                    else if (newValue.ToString() != "")
                    {
                        UpdateValue(subItem, newValue.ToString());
                    }
                }
                else if (subItem.Name.Contains("Label"))//标签编辑
                {
                    if (newValue.ToString() != "")
                    {
                        subItem.Tag = newValue;
                        subItem.Text = newValue.ToString();
                    }
                }
            }
            if (m_EditObject is System.Windows.Forms.ListViewItem)
            {
                System.Windows.Forms.ListViewItem item = m_EditObject as System.Windows.Forms.ListViewItem;
                item.Tag = newValue;
                string imageName = item.Name.Replace("Item", "Symbol");
                listValueItem.SmallImageList.Images.RemoveByKey(imageName);
                listValueItem.SmallImageList.Images.Add(imageName, ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                item.ImageKey = imageName;
            }
            if (m_EditObject is DevComponents.DotNetBar.LabelX)
            {
                DevComponents.DotNetBar.LabelX label = m_EditObject as DevComponents.DotNetBar.LabelX;
                switch (label.Name)
                {
                    case "labelPreviewFore":
                        if (label.Image != null)
                        {
                            label.Image.Dispose();
                            label.Image = null;
                        }
                        label.Tag = newValue;
                        label.Image = ModuleCommon.Symbol2Picture(newValue as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);
                        RefreshSymbol();
                        break;
                }
            }
            if (m_EditObject is DevDNB.ButtonX)
            {
                DevDNB.ButtonX button = m_EditObject as DevDNB.ButtonX;
                switch (button.Name)
                {
                    case "buttonAddFromSymbol":

                        break;
                }
            }
            m_EditObject = null;
        }

        #endregion

        public void InitRendererObject(IFeatureLayer pFeatureLayer, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            List<FieldInfo> fields = ModuleCommon.GetFieldsFromLayer(pFeatureLayer, false);
            m_Layer = pFeatureLayer;
            InitRendererObject(fields, pRenderer, _SymbologyStyleClass);
        }

        public void InitRendererObject(List<FieldInfo> pFields, IFeatureRenderer pRenderer, esriSymbologyStyleClass _SymbologyStyleClass)
        {
            flag = true;
            m_SymbologyStyleClass = _SymbologyStyleClass;

            cmbField1.Items.AddRange(pFields.ToArray());
            cmbField2.Items.AddRange(pFields.ToArray());
            cmbField3.Items.AddRange(pFields.ToArray());

            IUniqueValueRenderer pUVRenderer = pRenderer as IUniqueValueRenderer;
            if (pUVRenderer.ColorScheme != "")
                cmbColorRamp.Text = pUVRenderer.ColorScheme;

            cmbField1.Text = "<NONE>";
            cmbField2.Text = "<NONE>";
            cmbField3.Text = "<NONE>";
            if (pUVRenderer.FieldCount > 0)
                cmbField1.Text = pUVRenderer.get_Field(0);
            if (pUVRenderer.FieldCount > 1)
                cmbField2.Text = pUVRenderer.get_Field(1);
            if (pUVRenderer.FieldCount > 2)
                cmbField3.Text = pUVRenderer.get_Field(2);

            if (pUVRenderer.DefaultSymbol != null)
                labelPreviewFore.Tag = pUVRenderer.DefaultSymbol;
            else
                labelPreviewFore.Tag = ModuleCommon.CreateSymbol(m_SymbologyStyleClass);
            labelPreviewFore.Image = ModuleCommon.Symbol2Picture(labelPreviewFore.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight);

            listValueItem.SmallImageList.Images.Add("SymbolDefault", ModuleCommon.Symbol2Picture(labelPreviewFore.Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
            System.Windows.Forms.ListViewItem defaultItem = new System.Windows.Forms.ListViewItem();
            defaultItem.Name = "ItemDefault";
            defaultItem.Text = "";
            defaultItem.ImageKey = "SymbolDefault";
            defaultItem.Tag = labelPreviewFore.Tag;

            System.Windows.Forms.ListViewItem.ListViewSubItem defaultSubItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
            defaultSubItem.Name = "RangeDefault";
            defaultSubItem.Text = "<All Other Values>";
            defaultSubItem.Tag = "<All Other Values>";
            defaultItem.SubItems.Add(defaultSubItem);

            System.Windows.Forms.ListViewItem.ListViewSubItem defaultSubItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
            defaultSubItem1.Name = "LabelDefault";
            defaultSubItem1.Text = "<All Other Values>";
            defaultItem.SubItems.Add(defaultSubItem1);
            listValueItem.Items.Add(defaultItem);

            for (int i = 0; i < pUVRenderer.ValueCount; i++)
            {
                string value = pUVRenderer.get_Value(i);
                ISymbol pSymbol = pUVRenderer.get_Symbol(value);
                string label = pUVRenderer.get_Label(value);
                string itemName = value;//Guid.NewGuid().ToString();
                if (listValueItem.Items.ContainsKey("Item" + itemName)) continue;

                listValueItem.SmallImageList.Images.Add("Symbol" + itemName, ModuleCommon.Symbol2Picture(pSymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + itemName;
                item.Text = "";
                item.ImageKey = "Symbol" + itemName;
                item.Tag = pSymbol;

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Name = "Range" + itemName;
                subItem.Text = value;
                subItem.Tag = value;
                item.SubItems.Add(subItem);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem1.Name = "Label" + itemName;
                subItem1.Text = label;
                item.SubItems.Add(subItem1);

                listValueItem.Items.Add(item);
            }
            flag = false;
        }

        public IFeatureRenderer Renderer
        {
            get 
            {
                if(cmbField1.Text=="<NONE>") return null;

                IUniqueValueRenderer pRenderer = new UniqueValueRendererClass();
                pRenderer.ColorScheme = cmbColorRamp.Text;
                pRenderer.DefaultLabel = listValueItem.Items["ItemDefault"].SubItems[2].Text;
                pRenderer.DefaultSymbol = listValueItem.Items["ItemDefault"].Tag as ISymbol;
                pRenderer.UseDefaultSymbol = true;

                if (cmbField3.Text != "<NONE>")
                {
                    pRenderer.FieldCount = 3;
                    pRenderer.set_Field(0, cmbField1.Text);
                    pRenderer.set_Field(1, cmbField2.Text);
                    pRenderer.set_Field(2, cmbField3.Text);
                }
                else if (cmbField2.Text != "<NONE>")
                {
                    pRenderer.FieldCount = 2;
                    pRenderer.set_Field(0, cmbField1.Text);
                    pRenderer.set_Field(1, cmbField2.Text);
                }
                else
                {
                    pRenderer.FieldCount = 1;
                    pRenderer.set_Field(0, cmbField1.Text);
                }

                int valueIndex = 0;
                for (int i = 0; i < listValueItem.Items.Count; i++)
                {
                    if (listValueItem.Items[i].Name.Contains("Default")) continue;
                    ISymbol pSymbol = listValueItem.Items[i].Tag as ISymbol;
                    string value = listValueItem.Items[i].SubItems[1].Text;
                    string label = listValueItem.Items[i].SubItems[2].Text;

                    pRenderer.AddValue(value, "", pSymbol);
                    pRenderer.set_Label(value, label);
                }
                return pRenderer as IFeatureRenderer;
            }
        }

       public enumRendererType RendererType
        {
            get { return enumRendererType.UniqueValueRenderer; }
        }

        private ISymbol CreateSymbol()
        {
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            ISymbol pSymbol = copy.Copy(labelPreviewFore.Tag) as ISymbol;
            IColorRamp pColorRamp = (cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem).Tag as IColorRamp;
            bool bColorRamp = false;
            pColorRamp.Size = 2;
            pColorRamp.CreateRamp(out bColorRamp);
            IEnumColors pColors = pColorRamp.Colors;
            pColors.Reset();
            IColor pColor = pColors.Next();

            ModuleCommon.ChangeSymbolColor(pSymbol, pColor);
            return pSymbol;
        }

        private ISymbol[] CreateSymbols(int count)
        {
            if (count < 0)
                count = listValueItem.Items.Count;
            IColorRamp pColorRamp = (cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem).Tag as IColorRamp;
            bool bCreateRamp;
            pColorRamp.Size = count <= 1 ? 2 : count;
            pColorRamp.CreateRamp(out bCreateRamp);
            IEnumColors enumColors = pColorRamp.Colors;
            enumColors.Reset();

            ISymbol[] symbols = new ISymbol[count];
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            for (int i = 0; i < count; i++)
            {
                symbols[i] = copy.Copy(labelPreviewFore.Tag) as ISymbol;
                IColor pColor = enumColors.Next();
                if (pColor == null)
                {
                    enumColors.Reset();
                    enumColors.Next();
                }
                ModuleCommon.ChangeSymbolColor(symbols[i], pColor);
            }
            return symbols;
        }

        private void RefreshItems()
        {
            ISymbol[] symbols = CreateSymbols(-1);
            listValueItem.SmallImageList.Images.Clear();
            for (int i = 0; i < listValueItem.Items.Count; i++)
            {
                listValueItem.Items[i].Tag = symbols[i];
                string imageName = listValueItem.Items[i].Name.Replace("Item", "Symbol");
                listValueItem.SmallImageList.Images.Add(imageName, ModuleCommon.Symbol2Picture(
                    listValueItem.Items[i].Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                listValueItem.Items[i].ImageKey = imageName;
            }
            listValueItem.Refresh();
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            if (flag) return;
            flag = true;
            if (cmbField1.Text == "<NONE>")
            {
                cmbField2.Text = "<NONE>";
                cmbField3.Text = "<NONE>";
            }
            if (cmbField2.Text == "<NONE>")
                cmbField3.Text = "<NONE>";
            flag = false;
            System.Windows.Forms.Control control = sender as System.Windows.Forms.Control;
            switch (control.Name)
            {
                case "cmbField1":
                case "cmbField2":
                case "cmbField3":
                    DeleteAllValue();
                    break;
                case "cmbColorRamp":
                    RefreshColorRamp();
                    break;
            }
            
            listValueItem.Refresh();
        }

        private void AddValue()
        {
            ISymbol pSymbol = CreateSymbol();

            string nameIndex = "";

            listValueItem.SmallImageList.Images.Add("Symbol" + nameIndex, ModuleCommon.Symbol2Picture(pSymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
            item.Name = "Item" + nameIndex;
            item.Text = "";
            item.ImageKey = "Symbol" + nameIndex;
            item.Tag = pSymbol;

            System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
            subItem.Name = "Range" + nameIndex;
            subItem.Text = "";
            subItem.Tag = "";
            item.SubItems.Add(subItem);

            System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
            subItem1.Name = "Label" + nameIndex;
            subItem1.Text = "";
            item.SubItems.Add(subItem1);

            listValueItem.Items.Add(item);
            item.EnsureVisible();

            System.Drawing.Rectangle rec = item.GetBounds(System.Windows.Forms.ItemBoundsPortion.Entire);
            m_EditObject = subItem;
            //范围编辑
            System.Drawing.Point point = new System.Drawing.Point();
            point.X = listValueItem.Columns[0].Width;
            point.Y = rec.Top + (rec.Height - Form.frmDoubleEdit.FormWidth) / 2;
            point = listValueItem.PointToScreen(point);
            Form.frmStringEdit stringEdit = new GeoSymbology.Form.frmStringEdit(this,
                item.SubItems[1].Text, point, listValueItem.Columns[1].Width, "NEW");
            stringEdit.Show();
        }

        private void AddAllValue()
        {
            if (m_Layer == null) return;
            ESRI.ArcGIS.Geodatabase.IFeatureClass pFeatureClass = m_Layer.FeatureClass;
            ESRI.ArcGIS.Geodatabase.IQueryFilter pFilter = new ESRI.ArcGIS.Geodatabase.QueryFilterClass();
            List<string> fields = new List<string>();
            if (cmbField1.Text != "<NONE>")
                fields.Add(cmbField1.Text);
            if (cmbField2.Text != "<NONE>")
                fields.Add(cmbField2.Text);
            if (cmbField3.Text != "<NONE>")
                fields.Add(cmbField3.Text);
            if(fields.Count==0) return;

            ESRI.ArcGIS.Geodatabase.ITable pTable = (ESRI.ArcGIS.Geodatabase.ITable)pFeatureClass;
            List<int> fieldsIndex = new List<int>();
            for(int i = 0;i<fields.Count;i++)
                fieldsIndex.Add(pTable.Fields.FindField(fields[i]));

            int count = pTable.RowCount(pFilter);
            

            ISymbol[] allSymbols = CreateSymbols(count);
            ESRI.ArcGIS.Geodatabase.ICursor pCursor = pTable.Search(pFilter, false);
            int rowIndex = 0;
            bool isContinue = false;
            for (ESRI.ArcGIS.Geodatabase.IRow pRow = pCursor.NextRow(); pRow != null; pRow = pCursor.NextRow())
            {
                if (listValueItem.Items.Count > 500&&!isContinue)
                {
                    if (System.Windows.Forms.MessageBox.Show("记录个数大于500，是否继续计算添加？", "提示",
                        System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                    else
                        isContinue = true;
                }
                string value = "";
                for (int i = 0; i < fieldsIndex.Count; i++)
                {
                    string fieldValue = "";
                    if (fieldsIndex[i] == -1)
                    {
                        fieldValue = "<Null>";
                    }
                    else
                    {
                        fieldValue = pRow.get_Value(fieldsIndex[i]).ToString();
                        fieldValue = fieldValue == "" ? "<Null>" : fieldValue;
                    }
                    if (i == 0)
                        value = fieldValue;
                    else
                        value += "," + fieldValue;
                }
                string nameIndex = value;//Guid.NewGuid().ToString();
                if (listValueItem.Items.ContainsKey("Item" + nameIndex)) continue;

                listValueItem.SmallImageList.Images.Add("Symbol" + nameIndex, ModuleCommon.Symbol2Picture(allSymbols[rowIndex], ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem();
                item.Name = "Item" + nameIndex;
                item.Text = "";
                item.ImageKey = "Symbol" + nameIndex;
                item.Tag = allSymbols[rowIndex];

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem.Name = "Range" + nameIndex;
                subItem.Text = value;
                subItem.Tag = value;
                item.SubItems.Add(subItem);

                System.Windows.Forms.ListViewItem.ListViewSubItem subItem1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                subItem1.Name = "Label" + nameIndex;
                subItem1.Text = value;
                item.SubItems.Add(subItem1);

                listValueItem.Items.Add(item);

                rowIndex++;
            }
        }

        private void DeleteAllValue()
        {
            for (int i = listValueItem.Items.Count - 1; i >= 0; i--)
            {
                string imageName = listValueItem.Items[i].Name.Replace("Item", "Symbol");
                if (imageName.Contains("Default")) continue;
                listValueItem.Items.RemoveAt(i);
                listValueItem.SmallImageList.Images.RemoveByKey(imageName);
            }
        }  

        private void UpdateValue(System.Windows.Forms.ListViewItem.ListViewSubItem subItem, string newValue)
        {
            if (listValueItem.Items.ContainsKey("Item" + newValue)) return;

            System.Windows.Forms.ListViewItem item = listValueItem.Items[subItem.Name.Replace("Range", "Item")];
            System.Drawing.Image image = listValueItem.SmallImageList.Images[subItem.Name.Replace("Range", "Symbol")];
            listValueItem.SmallImageList.Images.RemoveByKey(subItem.Name.Replace("Range", "Symbol"));

            item.SubItems[1].Text = newValue;
            if (item.SubItems[2].Text == "")
                item.SubItems[2].Text = newValue;

            item.Name = "Item" + newValue;
            item.SubItems[1].Name = "Range" + newValue;
            item.SubItems[1].Tag = newValue;
            item.SubItems[2].Name = "Label" + newValue;
            item.ImageKey = "Symbol" + newValue;
            listValueItem.SmallImageList.Images.Add("Symbol" + newValue, image);
        }

        private void RefreshColorRamp()
        {//刷新符号颜色
            if (flag) return;
            IColorRamp pColorRamp = (cmbColorRamp.SelectedItem as DevComponents.Editors.ComboItem).Tag as IColorRamp;
            bool bCreateRamp;
            pColorRamp.Size = listValueItem.Items.Count <= 1 ? 2 : listValueItem.Items.Count;
            pColorRamp.CreateRamp(out bCreateRamp);
            IEnumColors enumColors = pColorRamp.Colors;
            enumColors.Reset();
            listValueItem.SmallImageList.Images.Clear();
            for (int i = 0; i < listValueItem.Items.Count; i++)
            {
                IColor pColor = enumColors.Next();
                if (pColor == null)
                {
                    enumColors.Reset();
                    enumColors.Next();
                }

                ModuleCommon.ChangeSymbolColor(listValueItem.Items[i].Tag as ISymbol, pColor);
                listValueItem.SmallImageList.Images.Add(listValueItem.Items[i].Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(listValueItem.Items[i].Tag as ISymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                listValueItem.Items[i].ImageKey = listValueItem.Items[i].Name.Replace("Item", "Symbol");
            }
        }

        private void RefreshSymbol()
        {//刷新符号样式
            if (flag) return;
            ESRI.ArcGIS.esriSystem.IObjectCopy copy = new ESRI.ArcGIS.esriSystem.ObjectCopyClass();
            listValueItem.SmallImageList.Images.Clear();
            for (int i = 0; i < listValueItem.Items.Count; i++)
            {
                ISymbol pSymbol = copy.Copy(labelPreviewFore.Tag) as ISymbol;
                IColor pColor = ModuleCommon.GetColor(listValueItem.Items[i].Tag as ISymbol);
                ModuleCommon.ChangeSymbolColor(pSymbol, pColor);
                listValueItem.Items[i].Tag = pSymbol;
                listValueItem.SmallImageList.Images.Add(listValueItem.Items[i].Name.Replace("Item", "Symbol"),
                    ModuleCommon.Symbol2Picture(pSymbol, ModuleCommon.ImageWidth, ModuleCommon.ImageHeight));
                listValueItem.Items[i].ImageKey = listValueItem.Items[i].Name.Replace("Item", "Symbol");
            }
        }
    }
}
