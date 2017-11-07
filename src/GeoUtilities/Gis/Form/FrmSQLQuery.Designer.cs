namespace GeoUtilities
{
    partial class FrmSQLQuery
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmbselmode = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmblayersel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.listViewField = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnAnd = new DevComponents.DotNetBar.ButtonX();
            this.btnLike = new DevComponents.DotNetBar.ButtonX();
            this.btnNot = new DevComponents.DotNetBar.ButtonX();
            this.btnOr = new DevComponents.DotNetBar.ButtonX();
            this.btnIs = new DevComponents.DotNetBar.ButtonX();
            this.btnPercent = new DevComponents.DotNetBar.ButtonX();
            this.btn2Ultra = new DevComponents.DotNetBar.ButtonX();
            this.btn1Ultra = new DevComponents.DotNetBar.ButtonX();
            this.btnNotEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnSmallerEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnBiggerEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnEqual = new DevComponents.DotNetBar.ButtonX();
            this.btnSmaller = new DevComponents.DotNetBar.ButtonX();
            this.btnBigger = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnDisplayValue = new DevComponents.DotNetBar.ButtonX();
            this.listViewValue = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.groupPanel5 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.richTextExpression = new System.Windows.Forms.RichTextBox();
            this.btnSample = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveResults = new DevComponents.DotNetBar.ButtonX();
            this.btnLoadResults = new DevComponents.DotNetBar.ButtonX();
            this.btnValidateExpression = new DevComponents.DotNetBar.ButtonX();
            this.btnClearExpression = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.comboExtent = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.groupPanel4.SuspendLayout();
            this.groupPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.cmbselmode);
            this.groupPanel1.Controls.Add(this.cmblayersel);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(12, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(429, 76);
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
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 24;
            // 
            // cmbselmode
            // 
            this.cmbselmode.DisplayMember = "Text";
            this.cmbselmode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbselmode.FormattingEnabled = true;
            this.cmbselmode.ItemHeight = 15;
            this.cmbselmode.Location = new System.Drawing.Point(91, 38);
            this.cmbselmode.Name = "cmbselmode";
            this.cmbselmode.Size = new System.Drawing.Size(303, 21);
            this.cmbselmode.TabIndex = 3;
            this.cmbselmode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbselmode_KeyPress);
            // 
            // cmblayersel
            // 
            this.cmblayersel.DisplayMember = "Text";
            this.cmblayersel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmblayersel.FormattingEnabled = true;
            this.cmblayersel.ItemHeight = 15;
            this.cmblayersel.Location = new System.Drawing.Point(91, 13);
            this.cmblayersel.Name = "cmblayersel";
            this.cmblayersel.Size = new System.Drawing.Size(303, 21);
            this.cmblayersel.TabIndex = 2;
            this.cmblayersel.SelectedIndexChanged += new System.EventHandler(this.cmblayersel_SelectedIndexChanged);
            this.cmblayersel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmblayersel_KeyPress);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(24, 38);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "选择方式：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(24, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "图层选择：";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.listViewField);
            this.groupPanel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(12, 94);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(146, 173);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 25;
            this.groupPanel2.Text = "字段";
            // 
            // listViewField
            // 
            // 
            // 
            // 
            this.listViewField.Border.Class = "ListViewBorder";
            this.listViewField.Location = new System.Drawing.Point(7, 3);
            this.listViewField.MultiSelect = false;
            this.listViewField.Name = "listViewField";
            this.listViewField.Size = new System.Drawing.Size(126, 141);
            this.listViewField.TabIndex = 6;
            this.listViewField.UseCompatibleStateImageBehavior = false;
            this.listViewField.View = System.Windows.Forms.View.Details;
            this.listViewField.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewField_MouseDoubleClick);
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.btnAnd);
            this.groupPanel3.Controls.Add(this.btnLike);
            this.groupPanel3.Controls.Add(this.btnNot);
            this.groupPanel3.Controls.Add(this.btnOr);
            this.groupPanel3.Controls.Add(this.btnIs);
            this.groupPanel3.Controls.Add(this.btnPercent);
            this.groupPanel3.Controls.Add(this.btn2Ultra);
            this.groupPanel3.Controls.Add(this.btn1Ultra);
            this.groupPanel3.Controls.Add(this.btnNotEqual);
            this.groupPanel3.Controls.Add(this.btnSmallerEqual);
            this.groupPanel3.Controls.Add(this.btnBiggerEqual);
            this.groupPanel3.Controls.Add(this.btnEqual);
            this.groupPanel3.Controls.Add(this.btnSmaller);
            this.groupPanel3.Controls.Add(this.btnBigger);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(164, 94);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(130, 173);
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
            this.groupPanel3.TabIndex = 26;
            this.groupPanel3.Text = "操作符";
            // 
            // btnAnd
            // 
            this.btnAnd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAnd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAnd.Location = new System.Drawing.Point(44, 121);
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.Size = new System.Drawing.Size(35, 23);
            this.btnAnd.TabIndex = 13;
            this.btnAnd.Text = "And";
            this.btnAnd.Click += new System.EventHandler(this.btnAnd_Click);
            // 
            // btnLike
            // 
            this.btnLike.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLike.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLike.Location = new System.Drawing.Point(3, 121);
            this.btnLike.Name = "btnLike";
            this.btnLike.Size = new System.Drawing.Size(35, 23);
            this.btnLike.TabIndex = 12;
            this.btnLike.Text = "Like";
            this.btnLike.Click += new System.EventHandler(this.btnLike_Click);
            // 
            // btnNot
            // 
            this.btnNot.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNot.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNot.Location = new System.Drawing.Point(85, 92);
            this.btnNot.Name = "btnNot";
            this.btnNot.Size = new System.Drawing.Size(35, 23);
            this.btnNot.TabIndex = 11;
            this.btnNot.Text = "Not";
            this.btnNot.Click += new System.EventHandler(this.btnNot_Click);
            // 
            // btnOr
            // 
            this.btnOr.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOr.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOr.Location = new System.Drawing.Point(44, 92);
            this.btnOr.Name = "btnOr";
            this.btnOr.Size = new System.Drawing.Size(35, 23);
            this.btnOr.TabIndex = 10;
            this.btnOr.Text = "Or";
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            // 
            // btnIs
            // 
            this.btnIs.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnIs.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnIs.Location = new System.Drawing.Point(3, 92);
            this.btnIs.Name = "btnIs";
            this.btnIs.Size = new System.Drawing.Size(35, 23);
            this.btnIs.TabIndex = 9;
            this.btnIs.Text = "Is";
            this.btnIs.Click += new System.EventHandler(this.btnIs_Click);
            // 
            // btnPercent
            // 
            this.btnPercent.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPercent.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPercent.Location = new System.Drawing.Point(86, 63);
            this.btnPercent.Name = "btnPercent";
            this.btnPercent.Size = new System.Drawing.Size(35, 23);
            this.btnPercent.TabIndex = 8;
            this.btnPercent.Text = "%";
            this.btnPercent.Click += new System.EventHandler(this.btnPercent_Click);
            // 
            // btn2Ultra
            // 
            this.btn2Ultra.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn2Ultra.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn2Ultra.Location = new System.Drawing.Point(44, 63);
            this.btn2Ultra.Name = "btn2Ultra";
            this.btn2Ultra.Size = new System.Drawing.Size(35, 23);
            this.btn2Ultra.TabIndex = 7;
            this.btn2Ultra.Text = "[]";
            this.btn2Ultra.Click += new System.EventHandler(this.btn2Ultra_Click);
            // 
            // btn1Ultra
            // 
            this.btn1Ultra.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn1Ultra.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn1Ultra.Location = new System.Drawing.Point(3, 63);
            this.btn1Ultra.Name = "btn1Ultra";
            this.btn1Ultra.Size = new System.Drawing.Size(35, 23);
            this.btn1Ultra.TabIndex = 6;
            this.btn1Ultra.Text = "－";
            this.btn1Ultra.Click += new System.EventHandler(this.btn1Ultra_Click);
            // 
            // btnNotEqual
            // 
            this.btnNotEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNotEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNotEqual.Location = new System.Drawing.Point(85, 34);
            this.btnNotEqual.Name = "btnNotEqual";
            this.btnNotEqual.Size = new System.Drawing.Size(35, 23);
            this.btnNotEqual.TabIndex = 5;
            this.btnNotEqual.Text = "<>";
            this.btnNotEqual.Click += new System.EventHandler(this.btnNotEqual_Click);
            // 
            // btnSmallerEqual
            // 
            this.btnSmallerEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSmallerEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSmallerEqual.Location = new System.Drawing.Point(44, 34);
            this.btnSmallerEqual.Name = "btnSmallerEqual";
            this.btnSmallerEqual.Size = new System.Drawing.Size(35, 23);
            this.btnSmallerEqual.TabIndex = 4;
            this.btnSmallerEqual.Text = "<=";
            this.btnSmallerEqual.Click += new System.EventHandler(this.btnSmallerEqual_Click);
            // 
            // btnBiggerEqual
            // 
            this.btnBiggerEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBiggerEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBiggerEqual.Location = new System.Drawing.Point(3, 34);
            this.btnBiggerEqual.Name = "btnBiggerEqual";
            this.btnBiggerEqual.Size = new System.Drawing.Size(35, 23);
            this.btnBiggerEqual.TabIndex = 3;
            this.btnBiggerEqual.Text = ">=";
            this.btnBiggerEqual.Click += new System.EventHandler(this.btnBiggerEqual_Click);
            // 
            // btnEqual
            // 
            this.btnEqual.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEqual.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnEqual.Location = new System.Drawing.Point(85, 3);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(35, 23);
            this.btnEqual.TabIndex = 2;
            this.btnEqual.Text = "=";
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // btnSmaller
            // 
            this.btnSmaller.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSmaller.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSmaller.Location = new System.Drawing.Point(44, 3);
            this.btnSmaller.Name = "btnSmaller";
            this.btnSmaller.Size = new System.Drawing.Size(35, 23);
            this.btnSmaller.TabIndex = 1;
            this.btnSmaller.Text = "<";
            this.btnSmaller.Click += new System.EventHandler(this.btnSmaller_Click);
            // 
            // btnBigger
            // 
            this.btnBigger.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBigger.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBigger.Location = new System.Drawing.Point(3, 3);
            this.btnBigger.Name = "btnBigger";
            this.btnBigger.Size = new System.Drawing.Size(35, 23);
            this.btnBigger.TabIndex = 0;
            this.btnBigger.Text = ">";
            this.btnBigger.Click += new System.EventHandler(this.btnBigger_Click);
            // 
            // groupPanel4
            // 
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.btnDisplayValue);
            this.groupPanel4.Controls.Add(this.listViewValue);
            this.groupPanel4.DrawTitleBox = false;
            this.groupPanel4.Location = new System.Drawing.Point(300, 94);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(141, 173);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel4.TabIndex = 27;
            this.groupPanel4.Text = "值";
            // 
            // btnDisplayValue
            // 
            this.btnDisplayValue.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDisplayValue.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDisplayValue.Location = new System.Drawing.Point(16, 128);
            this.btnDisplayValue.Name = "btnDisplayValue";
            this.btnDisplayValue.Size = new System.Drawing.Size(98, 19);
            this.btnDisplayValue.TabIndex = 1;
            this.btnDisplayValue.Text = "列出可能的值";
            this.btnDisplayValue.Click += new System.EventHandler(this.btnDisplayValue_Click);
            // 
            // listViewValue
            // 
            // 
            // 
            // 
            this.listViewValue.Border.Class = "ListViewBorder";
            this.listViewValue.Location = new System.Drawing.Point(7, 3);
            this.listViewValue.MultiSelect = false;
            this.listViewValue.Name = "listViewValue";
            this.listViewValue.Size = new System.Drawing.Size(118, 123);
            this.listViewValue.TabIndex = 0;
            this.listViewValue.UseCompatibleStateImageBehavior = false;
            this.listViewValue.View = System.Windows.Forms.View.Details;
            this.listViewValue.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewValue_MouseDoubleClick);
            // 
            // groupPanel5
            // 
            this.groupPanel5.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel5.Controls.Add(this.richTextExpression);
            this.groupPanel5.Controls.Add(this.btnSample);
            this.groupPanel5.Controls.Add(this.btnSaveResults);
            this.groupPanel5.Controls.Add(this.btnLoadResults);
            this.groupPanel5.Controls.Add(this.btnValidateExpression);
            this.groupPanel5.Controls.Add(this.btnClearExpression);
            this.groupPanel5.DrawTitleBox = false;
            this.groupPanel5.Location = new System.Drawing.Point(12, 268);
            this.groupPanel5.Name = "groupPanel5";
            this.groupPanel5.Size = new System.Drawing.Size(429, 164);
            // 
            // 
            // 
            this.groupPanel5.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel5.Style.BackColorGradientAngle = 90;
            this.groupPanel5.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel5.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderBottomWidth = 1;
            this.groupPanel5.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel5.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderLeftWidth = 1;
            this.groupPanel5.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderRightWidth = 1;
            this.groupPanel5.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel5.Style.BorderTopWidth = 1;
            this.groupPanel5.Style.CornerDiameter = 4;
            this.groupPanel5.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel5.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel5.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel5.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel5.TabIndex = 28;
            this.groupPanel5.Text = "生成表达式";
            // 
            // richTextExpression
            // 
            this.richTextExpression.Location = new System.Drawing.Point(3, 0);
            this.richTextExpression.Name = "richTextExpression";
            this.richTextExpression.Size = new System.Drawing.Size(417, 104);
            this.richTextExpression.TabIndex = 23;
            this.richTextExpression.Text = "";
            // 
            // btnSample
            // 
            this.btnSample.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSample.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSample.Location = new System.Drawing.Point(334, 110);
            this.btnSample.Name = "btnSample";
            this.btnSample.Size = new System.Drawing.Size(75, 23);
            this.btnSample.TabIndex = 5;
            this.btnSample.Text = "示  例";
            this.btnSample.Click += new System.EventHandler(this.btnSample_Click);
            // 
            // btnSaveResults
            // 
            this.btnSaveResults.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveResults.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveResults.Location = new System.Drawing.Point(253, 110);
            this.btnSaveResults.Name = "btnSaveResults";
            this.btnSaveResults.Size = new System.Drawing.Size(75, 23);
            this.btnSaveResults.TabIndex = 4;
            this.btnSaveResults.Text = "保存结果集";
            this.btnSaveResults.Click += new System.EventHandler(this.btnSaveResults_Click);
            // 
            // btnLoadResults
            // 
            this.btnLoadResults.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLoadResults.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLoadResults.Location = new System.Drawing.Point(172, 110);
            this.btnLoadResults.Name = "btnLoadResults";
            this.btnLoadResults.Size = new System.Drawing.Size(75, 23);
            this.btnLoadResults.TabIndex = 3;
            this.btnLoadResults.Text = "加载结果集";
            this.btnLoadResults.Click += new System.EventHandler(this.btnLoadResults_Click);
            // 
            // btnValidateExpression
            // 
            this.btnValidateExpression.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnValidateExpression.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnValidateExpression.Location = new System.Drawing.Point(91, 110);
            this.btnValidateExpression.Name = "btnValidateExpression";
            this.btnValidateExpression.Size = new System.Drawing.Size(75, 23);
            this.btnValidateExpression.TabIndex = 2;
            this.btnValidateExpression.Text = "验证表达式";
            this.btnValidateExpression.Click += new System.EventHandler(this.btnValidateExpression_Click);
            // 
            // btnClearExpression
            // 
            this.btnClearExpression.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClearExpression.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClearExpression.Location = new System.Drawing.Point(10, 110);
            this.btnClearExpression.Name = "btnClearExpression";
            this.btnClearExpression.Size = new System.Drawing.Size(75, 23);
            this.btnClearExpression.TabIndex = 1;
            this.btnClearExpression.Text = "清除表达式";
            this.btnClearExpression.Click += new System.EventHandler(this.btnClearExpression_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(294, 438);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 25);
            this.btnOK.TabIndex = 29;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(373, 438);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 25);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // comboExtent
            // 
            this.comboExtent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboExtent.AutoCompleteCustomSource.AddRange(new string[] {
            "全图范围",
            "当前视图范围",
            "选择要素范围"});
            this.comboExtent.DisplayMember = "Text";
            this.comboExtent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboExtent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboExtent.FormattingEnabled = true;
            this.comboExtent.ItemHeight = 15;
            this.comboExtent.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2,
            this.comboItem3});
            this.comboExtent.Location = new System.Drawing.Point(51, 440);
            this.comboExtent.Name = "comboExtent";
            this.comboExtent.Size = new System.Drawing.Size(112, 21);
            this.comboExtent.TabIndex = 31;
            this.comboExtent.SelectedIndexChanged += new System.EventHandler(this.comboExtent_SelectedIndexChanged);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "全图范围";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "当前视图范围";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "选择要素范围";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(10, 440);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(46, 23);
            this.labelX3.TabIndex = 32;
            this.labelX3.Text = "范围：";
            // 
            // FrmSQLQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 472);
            this.Controls.Add(this.comboExtent);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupPanel5);
            this.Controls.Add(this.groupPanel4);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSQLQuery";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL查询";
            this.Load += new System.EventHandler(this.FrmSQLQuery_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSQLQuery_KeyDown);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel4.ResumeLayout(false);
            this.groupPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbselmode;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmblayersel;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewField;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.ButtonX btnBigger;
        private DevComponents.DotNetBar.ButtonX btnAnd;
        private DevComponents.DotNetBar.ButtonX btnLike;
        private DevComponents.DotNetBar.ButtonX btnNot;
        private DevComponents.DotNetBar.ButtonX btnOr;
        private DevComponents.DotNetBar.ButtonX btnIs;
        private DevComponents.DotNetBar.ButtonX btnPercent;
        private DevComponents.DotNetBar.ButtonX btn2Ultra;
        private DevComponents.DotNetBar.ButtonX btn1Ultra;
        private DevComponents.DotNetBar.ButtonX btnNotEqual;
        private DevComponents.DotNetBar.ButtonX btnSmallerEqual;
        private DevComponents.DotNetBar.ButtonX btnBiggerEqual;
        private DevComponents.DotNetBar.ButtonX btnEqual;
        private DevComponents.DotNetBar.ButtonX btnSmaller;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.ButtonX btnDisplayValue;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewValue;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel5;
        private DevComponents.DotNetBar.ButtonX btnSample;
        private DevComponents.DotNetBar.ButtonX btnSaveResults;
        private DevComponents.DotNetBar.ButtonX btnLoadResults;
        private DevComponents.DotNetBar.ButtonX btnValidateExpression;
        private DevComponents.DotNetBar.ButtonX btnClearExpression;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.RichTextBox richTextExpression;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboExtent;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
    }
}