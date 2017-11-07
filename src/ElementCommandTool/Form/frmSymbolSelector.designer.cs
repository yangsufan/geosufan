namespace ElementCommandTool
{
    partial class frmSymbolSelector
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSymbolSelector));
            this.btnAddStyle = new System.Windows.Forms.Button();
            this.cmbStyle = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.axSymbologyControl1 = new ESRI.ArcGIS.Controls.AxSymbologyControl();
            this.grpSymbol = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpPoint = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.numPtYOffSet = new System.Windows.Forms.NumericUpDown();
            this.numPtXOffSet = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnPointColor = new System.Windows.Forms.Button();
            this.numPtSize = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grpText = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.fillGroup = new System.Windows.Forms.GroupBox();
            this.l_ColorForPolygon = new System.Windows.Forms.Label();
            this.cmb_ColorForPolygon = new System.Windows.Forms.Button();
            this.txt_LineWidth = new System.Windows.Forms.TextBox();
            this.l_ColorForLine = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cmb_ColorForLine = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.anGle = new System.Windows.Forms.NumericUpDown();
            this.cmbTextFont = new System.Windows.Forms.ComboBox();
            this.toolBarAlign = new System.Windows.Forms.ToolBar();
            this.toolLeft = new System.Windows.Forms.ToolBarButton();
            this.toolCenter = new System.Windows.Forms.ToolBarButton();
            this.toolRight = new System.Windows.Forms.ToolBarButton();
            this.toolFull = new System.Windows.Forms.ToolBarButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.toolBarStyle = new System.Windows.Forms.ToolBar();
            this.toolBlod = new System.Windows.Forms.ToolBarButton();
            this.toolItalic = new System.Windows.Forms.ToolBarButton();
            this.toolUnderline = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnTextColor = new System.Windows.Forms.Button();
            this.numTextSize = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.grpLine = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.numLineOffSet = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.btnLineColor = new System.Windows.Forms.Button();
            this.numLineWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpPoly = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnOutLineColor = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPolyColor = new System.Windows.Forms.Button();
            this.numOutLineWidth = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colorAnno = new ElementCommandTool.ColorButton();
            this.colorPoint = new ElementCommandTool.ColorButton();
            this.colorBorder = new ElementCommandTool.ColorButton();
            this.colorPolygon = new ElementCommandTool.ColorButton();
            this.colorLine = new ElementCommandTool.ColorButton();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axSymbologyControl1)).BeginInit();
            this.grpSymbol.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPtYOffSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPtXOffSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPtSize)).BeginInit();
            this.grpText.SuspendLayout();
            this.fillGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anGle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTextSize)).BeginInit();
            this.grpLine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineOffSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLineWidth)).BeginInit();
            this.grpPoly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutLineWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddStyle
            // 
            this.btnAddStyle.Image = ((System.Drawing.Image)(resources.GetObject("btnAddStyle.Image")));
            this.btnAddStyle.Location = new System.Drawing.Point(492, 2);
            this.btnAddStyle.Name = "btnAddStyle";
            this.btnAddStyle.Size = new System.Drawing.Size(23, 23);
            this.btnAddStyle.TabIndex = 9;
            this.btnAddStyle.UseVisualStyleBackColor = true;
            this.btnAddStyle.Click += new System.EventHandler(this.btnAddStyle_Click);
            // 
            // cmbStyle
            // 
            this.cmbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStyle.FormattingEnabled = true;
            this.cmbStyle.Location = new System.Drawing.Point(65, 5);
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.Size = new System.Drawing.Size(422, 20);
            this.cmbStyle.TabIndex = 8;
            this.cmbStyle.SelectedIndexChanged += new System.EventHandler(this.cmbStyle_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "符号库：";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.axSymbologyControl1);
            this.groupPanel1.Location = new System.Drawing.Point(13, 31);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(311, 337);
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
            this.groupPanel1.TabIndex = 10;
            this.groupPanel1.Text = "符号";
            // 
            // axSymbologyControl1
            // 
            this.axSymbologyControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axSymbologyControl1.Location = new System.Drawing.Point(0, 0);
            this.axSymbologyControl1.Name = "axSymbologyControl1";
            this.axSymbologyControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSymbologyControl1.OcxState")));
            this.axSymbologyControl1.Size = new System.Drawing.Size(305, 313);
            this.axSymbologyControl1.TabIndex = 0;
            this.axSymbologyControl1.OnItemSelected += new ESRI.ArcGIS.Controls.ISymbologyControlEvents_Ax_OnItemSelectedEventHandler(this.axSymbologyControl1_OnItemSelected);
            // 
            // grpSymbol
            // 
            this.grpSymbol.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpSymbol.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpSymbol.Controls.Add(this.pictureBox1);
            this.grpSymbol.Location = new System.Drawing.Point(353, 31);
            this.grpSymbol.Name = "grpSymbol";
            this.grpSymbol.Size = new System.Drawing.Size(162, 142);
            // 
            // 
            // 
            this.grpSymbol.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpSymbol.Style.BackColorGradientAngle = 90;
            this.grpSymbol.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpSymbol.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSymbol.Style.BorderBottomWidth = 1;
            this.grpSymbol.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpSymbol.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSymbol.Style.BorderLeftWidth = 1;
            this.grpSymbol.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSymbol.Style.BorderRightWidth = 1;
            this.grpSymbol.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpSymbol.Style.BorderTopWidth = 1;
            this.grpSymbol.Style.CornerDiameter = 4;
            this.grpSymbol.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpSymbol.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpSymbol.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpSymbol.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpSymbol.TabIndex = 11;
            this.grpSymbol.Text = "符号预览";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(156, 118);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // grpPoint
            // 
            this.grpPoint.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpPoint.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpPoint.Controls.Add(this.colorPoint);
            this.grpPoint.Controls.Add(this.numPtYOffSet);
            this.grpPoint.Controls.Add(this.numPtXOffSet);
            this.grpPoint.Controls.Add(this.label11);
            this.grpPoint.Controls.Add(this.label10);
            this.grpPoint.Controls.Add(this.btnPointColor);
            this.grpPoint.Controls.Add(this.numPtSize);
            this.grpPoint.Controls.Add(this.label4);
            this.grpPoint.Controls.Add(this.label5);
            this.grpPoint.Location = new System.Drawing.Point(349, 182);
            this.grpPoint.Name = "grpPoint";
            this.grpPoint.Size = new System.Drawing.Size(167, 184);
            // 
            // 
            // 
            this.grpPoint.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpPoint.Style.BackColorGradientAngle = 90;
            this.grpPoint.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpPoint.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoint.Style.BorderBottomWidth = 1;
            this.grpPoint.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpPoint.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoint.Style.BorderLeftWidth = 1;
            this.grpPoint.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoint.Style.BorderRightWidth = 1;
            this.grpPoint.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoint.Style.BorderTopWidth = 1;
            this.grpPoint.Style.CornerDiameter = 4;
            this.grpPoint.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpPoint.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpPoint.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpPoint.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpPoint.TabIndex = 12;
            this.grpPoint.Text = "点符号";
            // 
            // numPtYOffSet
            // 
            this.numPtYOffSet.DecimalPlaces = 1;
            this.numPtYOffSet.Location = new System.Drawing.Point(61, 95);
            this.numPtYOffSet.Name = "numPtYOffSet";
            this.numPtYOffSet.Size = new System.Drawing.Size(56, 21);
            this.numPtYOffSet.TabIndex = 17;
            this.numPtYOffSet.ValueChanged += new System.EventHandler(this.numPtYOffSet_ValueChanged);
            // 
            // numPtXOffSet
            // 
            this.numPtXOffSet.DecimalPlaces = 1;
            this.numPtXOffSet.Location = new System.Drawing.Point(61, 68);
            this.numPtXOffSet.Name = "numPtXOffSet";
            this.numPtXOffSet.Size = new System.Drawing.Size(56, 21);
            this.numPtXOffSet.TabIndex = 16;
            this.numPtXOffSet.ValueChanged += new System.EventHandler(this.numPtXOffSet_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 12);
            this.label11.TabIndex = 15;
            this.label11.Text = "Y偏移：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "X偏移：";
            // 
            // btnPointColor
            // 
            this.btnPointColor.Location = new System.Drawing.Point(63, 12);
            this.btnPointColor.Name = "btnPointColor";
            this.btnPointColor.Size = new System.Drawing.Size(42, 23);
            this.btnPointColor.TabIndex = 13;
            this.btnPointColor.UseVisualStyleBackColor = true;
            // 
            // numPtSize
            // 
            this.numPtSize.DecimalPlaces = 1;
            this.numPtSize.Location = new System.Drawing.Point(61, 41);
            this.numPtSize.Name = "numPtSize";
            this.numPtSize.Size = new System.Drawing.Size(56, 21);
            this.numPtSize.TabIndex = 12;
            this.numPtSize.ValueChanged += new System.EventHandler(this.numPtSize_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "大小：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "颜色：";
            // 
            // grpText
            // 
            this.grpText.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpText.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpText.Controls.Add(this.label17);
            this.grpText.Controls.Add(this.fillGroup);
            this.grpText.Controls.Add(this.anGle);
            this.grpText.Controls.Add(this.colorAnno);
            this.grpText.Controls.Add(this.cmbTextFont);
            this.grpText.Controls.Add(this.toolBarAlign);
            this.grpText.Controls.Add(this.toolBarStyle);
            this.grpText.Controls.Add(this.btnTextColor);
            this.grpText.Controls.Add(this.numTextSize);
            this.grpText.Controls.Add(this.label15);
            this.grpText.Controls.Add(this.label14);
            this.grpText.Controls.Add(this.label12);
            this.grpText.Location = new System.Drawing.Point(348, 182);
            this.grpText.Name = "grpText";
            this.grpText.Size = new System.Drawing.Size(181, 184);
            // 
            // 
            // 
            this.grpText.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpText.Style.BackColorGradientAngle = 90;
            this.grpText.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpText.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpText.Style.BorderBottomWidth = 1;
            this.grpText.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpText.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpText.Style.BorderLeftWidth = 1;
            this.grpText.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpText.Style.BorderRightWidth = 1;
            this.grpText.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpText.Style.BorderTopWidth = 1;
            this.grpText.Style.CornerDiameter = 4;
            this.grpText.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpText.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpText.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpText.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpText.TabIndex = 18;
            this.grpText.Text = "注记符号";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Location = new System.Drawing.Point(92, 6);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 12);
            this.label17.TabIndex = 48;
            this.label17.Text = "角度:";
            // 
            // fillGroup
            // 
            this.fillGroup.Controls.Add(this.l_ColorForPolygon);
            this.fillGroup.Controls.Add(this.cmb_ColorForPolygon);
            this.fillGroup.Controls.Add(this.txt_LineWidth);
            this.fillGroup.Controls.Add(this.l_ColorForLine);
            this.fillGroup.Controls.Add(this.label16);
            this.fillGroup.Controls.Add(this.cmb_ColorForLine);
            this.fillGroup.Controls.Add(this.label13);
            this.fillGroup.Location = new System.Drawing.Point(3, 100);
            this.fillGroup.Name = "fillGroup";
            this.fillGroup.Size = new System.Drawing.Size(163, 60);
            this.fillGroup.TabIndex = 1;
            this.fillGroup.TabStop = false;
            this.fillGroup.Text = "填充";
            this.fillGroup.Visible = false;
            // 
            // l_ColorForPolygon
            // 
            this.l_ColorForPolygon.BackColor = System.Drawing.SystemColors.ControlText;
            this.l_ColorForPolygon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.l_ColorForPolygon.Location = new System.Drawing.Point(116, 14);
            this.l_ColorForPolygon.Name = "l_ColorForPolygon";
            this.l_ColorForPolygon.Size = new System.Drawing.Size(35, 18);
            this.l_ColorForPolygon.TabIndex = 54;
            // 
            // cmb_ColorForPolygon
            // 
            this.cmb_ColorForPolygon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmb_ColorForPolygon.Location = new System.Drawing.Point(72, 11);
            this.cmb_ColorForPolygon.Name = "cmb_ColorForPolygon";
            this.cmb_ColorForPolygon.Size = new System.Drawing.Size(41, 23);
            this.cmb_ColorForPolygon.TabIndex = 53;
            this.cmb_ColorForPolygon.Text = "填充颜色";
            this.cmb_ColorForPolygon.UseVisualStyleBackColor = true;
            this.cmb_ColorForPolygon.Click += new System.EventHandler(this.cmb_ColorForPolygon_Click);
            // 
            // txt_LineWidth
            // 
            this.txt_LineWidth.Location = new System.Drawing.Point(66, 35);
            this.txt_LineWidth.Name = "txt_LineWidth";
            this.txt_LineWidth.Size = new System.Drawing.Size(48, 21);
            this.txt_LineWidth.TabIndex = 49;
            // 
            // l_ColorForLine
            // 
            this.l_ColorForLine.AutoSize = true;
            this.l_ColorForLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.l_ColorForLine.Location = new System.Drawing.Point(45, 9);
            this.l_ColorForLine.Name = "l_ColorForLine";
            this.l_ColorForLine.Size = new System.Drawing.Size(23, 12);
            this.l_ColorForLine.TabIndex = 52;
            this.l_ColorForLine.Text = "___";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(5, 39);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 12);
            this.label16.TabIndex = 48;
            this.label16.Text = "边框宽度:";
            // 
            // cmb_ColorForLine
            // 
            this.cmb_ColorForLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmb_ColorForLine.Location = new System.Drawing.Point(4, 11);
            this.cmb_ColorForLine.Name = "cmb_ColorForLine";
            this.cmb_ColorForLine.Size = new System.Drawing.Size(39, 23);
            this.cmb_ColorForLine.TabIndex = 51;
            this.cmb_ColorForLine.Text = "边框颜色";
            this.cmb_ColorForLine.UseVisualStyleBackColor = true;
            this.cmb_ColorForLine.Click += new System.EventHandler(this.cmb_ColorForLine_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(121, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 50;
            this.label13.Text = "毫米";
            // 
            // anGle
            // 
            this.anGle.Location = new System.Drawing.Point(127, 3);
            this.anGle.Name = "anGle";
            this.anGle.Size = new System.Drawing.Size(46, 21);
            this.anGle.TabIndex = 47;
            // 
            // cmbTextFont
            // 
            this.cmbTextFont.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cmbTextFont.FormattingEnabled = true;
            this.cmbTextFont.Location = new System.Drawing.Point(97, 30);
            this.cmbTextFont.Name = "cmbTextFont";
            this.cmbTextFont.Size = new System.Drawing.Size(76, 20);
            this.cmbTextFont.TabIndex = 25;
            this.cmbTextFont.ValueMemberChanged += new System.EventHandler(this.cmbTextFont_ValueMemberChanged);
            // 
            // toolBarAlign
            // 
            this.toolBarAlign.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolBarAlign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolBarAlign.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolLeft,
            this.toolCenter,
            this.toolRight,
            this.toolFull});
            this.toolBarAlign.ButtonSize = new System.Drawing.Size(23, 22);
            this.toolBarAlign.Divider = false;
            this.toolBarAlign.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBarAlign.DropDownArrows = true;
            this.toolBarAlign.ImageList = this.imageList2;
            this.toolBarAlign.Location = new System.Drawing.Point(38, 80);
            this.toolBarAlign.Name = "toolBarAlign";
            this.toolBarAlign.ShowToolTips = true;
            this.toolBarAlign.Size = new System.Drawing.Size(97, 27);
            this.toolBarAlign.TabIndex = 24;
            this.toolBarAlign.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarAlign_ButtonClick);
            // 
            // toolLeft
            // 
            this.toolLeft.ImageKey = "LeftAlign.bmp";
            this.toolLeft.Name = "toolLeft";
            this.toolLeft.ToolTipText = "左对齐";
            // 
            // toolCenter
            // 
            this.toolCenter.ImageKey = "CenterAlign.bmp";
            this.toolCenter.Name = "toolCenter";
            this.toolCenter.ToolTipText = "居中";
            // 
            // toolRight
            // 
            this.toolRight.ImageKey = "RightAlign.bmp";
            this.toolRight.Name = "toolRight";
            this.toolRight.ToolTipText = "右对齐";
            // 
            // toolFull
            // 
            this.toolFull.ImageKey = "JustifiedAlign.bmp";
            this.toolFull.Name = "toolFull";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "LeftAlign.bmp");
            this.imageList2.Images.SetKeyName(1, "CenterAlign.bmp");
            this.imageList2.Images.SetKeyName(2, "RightAlign.bmp");
            this.imageList2.Images.SetKeyName(3, "JustifiedAlign.bmp");
            // 
            // toolBarStyle
            // 
            this.toolBarStyle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.toolBarStyle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolBarStyle.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBlod,
            this.toolItalic,
            this.toolUnderline});
            this.toolBarStyle.ButtonSize = new System.Drawing.Size(23, 20);
            this.toolBarStyle.Divider = false;
            this.toolBarStyle.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBarStyle.DropDownArrows = true;
            this.toolBarStyle.ImageList = this.imageList1;
            this.toolBarStyle.Location = new System.Drawing.Point(39, 59);
            this.toolBarStyle.Name = "toolBarStyle";
            this.toolBarStyle.ShowToolTips = true;
            this.toolBarStyle.Size = new System.Drawing.Size(70, 27);
            this.toolBarStyle.TabIndex = 23;
            this.toolBarStyle.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarStyle_ButtonClick);
            // 
            // toolBlod
            // 
            this.toolBlod.ImageKey = "Bold.bmp";
            this.toolBlod.Name = "toolBlod";
            // 
            // toolItalic
            // 
            this.toolItalic.ImageKey = "Italic.bmp";
            this.toolItalic.Name = "toolItalic";
            // 
            // toolUnderline
            // 
            this.toolUnderline.ImageKey = "Underline.bmp";
            this.toolUnderline.Name = "toolUnderline";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Bold.bmp");
            this.imageList1.Images.SetKeyName(1, "Italic.bmp");
            this.imageList1.Images.SetKeyName(2, "Underline.bmp");
            // 
            // btnTextColor
            // 
            this.btnTextColor.Location = new System.Drawing.Point(38, 1);
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.Size = new System.Drawing.Size(42, 23);
            this.btnTextColor.TabIndex = 22;
            this.btnTextColor.UseVisualStyleBackColor = true;
            // 
            // numTextSize
            // 
            this.numTextSize.Location = new System.Drawing.Point(39, 29);
            this.numTextSize.Name = "numTextSize";
            this.numTextSize.Size = new System.Drawing.Size(56, 21);
            this.numTextSize.TabIndex = 21;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Location = new System.Drawing.Point(3, 59);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 12);
            this.label15.TabIndex = 20;
            this.label15.Text = "样式:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(2, 35);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(35, 12);
            this.label14.TabIndex = 19;
            this.label14.Text = "大小:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(1, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 18;
            this.label12.Text = "颜色:";
            // 
            // grpLine
            // 
            this.grpLine.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpLine.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpLine.Controls.Add(this.colorLine);
            this.grpLine.Controls.Add(this.numLineOffSet);
            this.grpLine.Controls.Add(this.label9);
            this.grpLine.Controls.Add(this.btnLineColor);
            this.grpLine.Controls.Add(this.numLineWidth);
            this.grpLine.Controls.Add(this.label3);
            this.grpLine.Controls.Add(this.label2);
            this.grpLine.Location = new System.Drawing.Point(349, 182);
            this.grpLine.Name = "grpLine";
            this.grpLine.Size = new System.Drawing.Size(167, 184);
            // 
            // 
            // 
            this.grpLine.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpLine.Style.BackColorGradientAngle = 90;
            this.grpLine.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpLine.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLine.Style.BorderBottomWidth = 1;
            this.grpLine.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpLine.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLine.Style.BorderLeftWidth = 1;
            this.grpLine.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLine.Style.BorderRightWidth = 1;
            this.grpLine.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpLine.Style.BorderTopWidth = 1;
            this.grpLine.Style.CornerDiameter = 4;
            this.grpLine.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpLine.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpLine.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpLine.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpLine.TabIndex = 19;
            this.grpLine.Text = "线符号";
            // 
            // numLineOffSet
            // 
            this.numLineOffSet.DecimalPlaces = 1;
            this.numLineOffSet.Location = new System.Drawing.Point(60, 80);
            this.numLineOffSet.Name = "numLineOffSet";
            this.numLineOffSet.Size = new System.Drawing.Size(56, 21);
            this.numLineOffSet.TabIndex = 13;
            this.numLineOffSet.ValueChanged += new System.EventHandler(this.numLineOffSet_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "偏移：";
            // 
            // btnLineColor
            // 
            this.btnLineColor.Location = new System.Drawing.Point(60, 21);
            this.btnLineColor.Name = "btnLineColor";
            this.btnLineColor.Size = new System.Drawing.Size(42, 23);
            this.btnLineColor.TabIndex = 11;
            this.btnLineColor.UseVisualStyleBackColor = true;
            // 
            // numLineWidth
            // 
            this.numLineWidth.DecimalPlaces = 1;
            this.numLineWidth.Location = new System.Drawing.Point(60, 54);
            this.numLineWidth.Name = "numLineWidth";
            this.numLineWidth.Size = new System.Drawing.Size(56, 21);
            this.numLineWidth.TabIndex = 10;
            this.numLineWidth.ValueChanged += new System.EventHandler(this.numLineWidth_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "宽度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "颜色：";
            // 
            // grpPoly
            // 
            this.grpPoly.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpPoly.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpPoly.Controls.Add(this.colorBorder);
            this.grpPoly.Controls.Add(this.colorPolygon);
            this.grpPoly.Controls.Add(this.btnOutLineColor);
            this.grpPoly.Controls.Add(this.label8);
            this.grpPoly.Controls.Add(this.btnPolyColor);
            this.grpPoly.Controls.Add(this.numOutLineWidth);
            this.grpPoly.Controls.Add(this.label6);
            this.grpPoly.Controls.Add(this.label7);
            this.grpPoly.Location = new System.Drawing.Point(349, 182);
            this.grpPoly.Name = "grpPoly";
            this.grpPoly.Size = new System.Drawing.Size(167, 184);
            // 
            // 
            // 
            this.grpPoly.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpPoly.Style.BackColorGradientAngle = 90;
            this.grpPoly.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpPoly.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoly.Style.BorderBottomWidth = 1;
            this.grpPoly.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpPoly.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoly.Style.BorderLeftWidth = 1;
            this.grpPoly.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoly.Style.BorderRightWidth = 1;
            this.grpPoly.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpPoly.Style.BorderTopWidth = 1;
            this.grpPoly.Style.CornerDiameter = 4;
            this.grpPoly.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grpPoly.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.grpPoly.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpPoly.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpPoly.TabIndex = 19;
            this.grpPoly.Text = "面符号";
            // 
            // btnOutLineColor
            // 
            this.btnOutLineColor.Location = new System.Drawing.Point(84, 48);
            this.btnOutLineColor.Name = "btnOutLineColor";
            this.btnOutLineColor.Size = new System.Drawing.Size(42, 23);
            this.btnOutLineColor.TabIndex = 13;
            this.btnOutLineColor.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "边线颜色：";
            // 
            // btnPolyColor
            // 
            this.btnPolyColor.Location = new System.Drawing.Point(84, 20);
            this.btnPolyColor.Name = "btnPolyColor";
            this.btnPolyColor.Size = new System.Drawing.Size(42, 23);
            this.btnPolyColor.TabIndex = 11;
            this.btnPolyColor.UseVisualStyleBackColor = true;
            // 
            // numOutLineWidth
            // 
            this.numOutLineWidth.DecimalPlaces = 1;
            this.numOutLineWidth.Location = new System.Drawing.Point(84, 82);
            this.numOutLineWidth.Name = "numOutLineWidth";
            this.numOutLineWidth.Size = new System.Drawing.Size(42, 21);
            this.numOutLineWidth.TabIndex = 10;
            this.numOutLineWidth.ValueChanged += new System.EventHandler(this.numOutLineWidth_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "边线宽度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "颜色：";
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(367, 374);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 23);
            this.btnOk.TabIndex = 20;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(448, 374);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colorAnno
            // 
            this.colorAnno.Automatic = "Automatic";
            this.colorAnno.Color = System.Drawing.Color.Transparent;
            this.colorAnno.Location = new System.Drawing.Point(39, 0);
            this.colorAnno.MoreColors = "More Colors...";
            this.colorAnno.Name = "colorAnno";
            this.colorAnno.Size = new System.Drawing.Size(51, 23);
            this.colorAnno.TabIndex = 26;
            this.colorAnno.Text = "透明";
            this.colorAnno.UseVisualStyleBackColor = true;
            this.colorAnno.Changed += new System.EventHandler(this.colorAnno_Changed);
            // 
            // colorPoint
            // 
            this.colorPoint.Automatic = "Automatic";
            this.colorPoint.Color = System.Drawing.Color.Transparent;
            this.colorPoint.Location = new System.Drawing.Point(61, 12);
            this.colorPoint.MoreColors = "More Colors...";
            this.colorPoint.Name = "colorPoint";
            this.colorPoint.Size = new System.Drawing.Size(75, 23);
            this.colorPoint.TabIndex = 27;
            this.colorPoint.Text = "透明";
            this.colorPoint.UseVisualStyleBackColor = true;
            this.colorPoint.Changed += new System.EventHandler(this.colorPoint_Changed);
            // 
            // colorBorder
            // 
            this.colorBorder.Automatic = "Automatic";
            this.colorBorder.Color = System.Drawing.Color.Transparent;
            this.colorBorder.Location = new System.Drawing.Point(75, 48);
            this.colorBorder.MoreColors = "More Colors...";
            this.colorBorder.Name = "colorBorder";
            this.colorBorder.Size = new System.Drawing.Size(75, 23);
            this.colorBorder.TabIndex = 30;
            this.colorBorder.Text = "透明";
            this.colorBorder.UseVisualStyleBackColor = true;
            this.colorBorder.Changed += new System.EventHandler(this.colorBorder_Changed);
            // 
            // colorPolygon
            // 
            this.colorPolygon.Automatic = "Automatic";
            this.colorPolygon.Color = System.Drawing.Color.Transparent;
            this.colorPolygon.Location = new System.Drawing.Point(75, 19);
            this.colorPolygon.MoreColors = "More Colors...";
            this.colorPolygon.Name = "colorPolygon";
            this.colorPolygon.Size = new System.Drawing.Size(75, 23);
            this.colorPolygon.TabIndex = 29;
            this.colorPolygon.Text = "透明";
            this.colorPolygon.UseVisualStyleBackColor = true;
            this.colorPolygon.Changed += new System.EventHandler(this.colorPolygon_Changed);
            // 
            // colorLine
            // 
            this.colorLine.Automatic = "Automatic";
            this.colorLine.Color = System.Drawing.Color.Transparent;
            this.colorLine.Location = new System.Drawing.Point(60, 21);
            this.colorLine.MoreColors = "More Colors...";
            this.colorLine.Name = "colorLine";
            this.colorLine.Size = new System.Drawing.Size(75, 23);
            this.colorLine.TabIndex = 28;
            this.colorLine.Text = "透明";
            this.colorLine.UseVisualStyleBackColor = true;
            this.colorLine.Changed += new System.EventHandler(this.colorLine_Changed);
            // 
            // frmSymbolSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 406);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grpSymbol);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnAddStyle);
            this.Controls.Add(this.cmbStyle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpText);
            this.Controls.Add(this.grpPoint);
            this.Controls.Add(this.grpPoly);
            this.Controls.Add(this.grpLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSymbolSelector";
            this.ShowIcon = false;
            this.Text = "符号选择器";
            this.Load += new System.EventHandler(this.frmSymbolSelector_Load);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axSymbologyControl1)).EndInit();
            this.grpSymbol.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpPoint.ResumeLayout(false);
            this.grpPoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPtYOffSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPtXOffSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPtSize)).EndInit();
            this.grpText.ResumeLayout(false);
            this.grpText.PerformLayout();
            this.fillGroup.ResumeLayout(false);
            this.fillGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anGle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTextSize)).EndInit();
            this.grpLine.ResumeLayout(false);
            this.grpLine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLineOffSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLineWidth)).EndInit();
            this.grpPoly.ResumeLayout(false);
            this.grpPoly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOutLineWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddStyle;
        private System.Windows.Forms.ComboBox cmbStyle;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpSymbol;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevComponents.DotNetBar.Controls.GroupPanel grpPoint;
        private System.Windows.Forms.NumericUpDown numPtYOffSet;
        private System.Windows.Forms.NumericUpDown numPtXOffSet;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnPointColor;
        private System.Windows.Forms.NumericUpDown numPtSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.Controls.GroupPanel grpText;
        private System.Windows.Forms.ComboBox cmbTextFont;
        private System.Windows.Forms.ToolBar toolBarAlign;
        private System.Windows.Forms.ToolBarButton toolLeft;
        private System.Windows.Forms.ToolBarButton toolCenter;
        private System.Windows.Forms.ToolBarButton toolRight;
        private System.Windows.Forms.ToolBarButton toolFull;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolBar toolBarStyle;
        private System.Windows.Forms.ToolBarButton toolBlod;
        private System.Windows.Forms.ToolBarButton toolItalic;
        private System.Windows.Forms.ToolBarButton toolUnderline;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnTextColor;
        private System.Windows.Forms.NumericUpDown numTextSize;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private DevComponents.DotNetBar.Controls.GroupPanel grpLine;
        private System.Windows.Forms.NumericUpDown numLineOffSet;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnLineColor;
        private System.Windows.Forms.NumericUpDown numLineWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.GroupPanel grpPoly;
        private System.Windows.Forms.Button btnOutLineColor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnPolyColor;
        private System.Windows.Forms.NumericUpDown numOutLineWidth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private ElementCommandTool.ColorButton colorAnno;
        private ElementCommandTool.ColorButton colorPoint;
        private ElementCommandTool.ColorButton colorLine;
        private ElementCommandTool.ColorButton colorPolygon;
        private ElementCommandTool.ColorButton colorBorder;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private ESRI.ArcGIS.Controls.AxSymbologyControl axSymbologyControl1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown anGle;
        private System.Windows.Forms.Label l_ColorForPolygon;
        private System.Windows.Forms.Button cmb_ColorForPolygon;
        private System.Windows.Forms.Label l_ColorForLine;
        private System.Windows.Forms.Button cmb_ColorForLine;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt_LineWidth;
        private System.Windows.Forms.GroupBox fillGroup;
        private System.Windows.Forms.Label label17;
    }
}