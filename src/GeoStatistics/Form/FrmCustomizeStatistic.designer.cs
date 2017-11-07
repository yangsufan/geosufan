namespace GeoStatistics
{
    partial class FrmCustomizeStatistic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCustomizeStatistic));
            this.txtBoxLayer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupPanelAutoConfig = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnShowXitems = new DevComponents.DotNetBar.ButtonX();
            this.ListContents = new System.Windows.Forms.DataGridView();
            this.ColumnSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.label17 = new System.Windows.Forms.Label();
            this.cmbRegion = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDigits = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbYfactor3 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbYfactor2 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label16 = new System.Windows.Forms.Label();
            this.cmbYfactor1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbXfactor = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExportImage = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.cmbUnit = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanelXfactors = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.chkboxXfactorItems = new System.Windows.Forms.CheckBox();
            this.dataGridXfactors = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartlet = new FanG.Chartlet();
            this.btnStatistic = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbY = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbContent = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbChartType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.dGridViewStatisticRes = new System.Windows.Forms.DataGridView();
            this.advTreeLayers = new DevComponents.AdvTree.AdvTree();
            this.node2 = new DevComponents.AdvTree.Node();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.rowMergeView1 = new RowMergeView();
            this.groupPanelAutoConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListContents)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.groupPanelXfactors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridXfactors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGridViewStatisticRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowMergeView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBoxLayer
            // 
            this.txtBoxLayer.Location = new System.Drawing.Point(50, 12);
            this.txtBoxLayer.Name = "txtBoxLayer";
            this.txtBoxLayer.ReadOnly = true;
            this.txtBoxLayer.Size = new System.Drawing.Size(342, 21);
            this.txtBoxLayer.TabIndex = 54;
            this.txtBoxLayer.Text = "点击选择 目标图层";
            this.txtBoxLayer.Click += new System.EventHandler(this.txtBoxLayer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 53;
            this.label2.Text = "图层:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupPanelAutoConfig
            // 
            this.groupPanelAutoConfig.BackColor = System.Drawing.Color.Transparent;
            this.groupPanelAutoConfig.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelAutoConfig.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelAutoConfig.Controls.Add(this.btnShowXitems);
            this.groupPanelAutoConfig.Controls.Add(this.ListContents);
            this.groupPanelAutoConfig.Controls.Add(this.groupPanel1);
            this.groupPanelAutoConfig.Controls.Add(this.cmbXfactor);
            this.groupPanelAutoConfig.Controls.Add(this.label13);
            this.groupPanelAutoConfig.Controls.Add(this.label12);
            this.groupPanelAutoConfig.Controls.Add(this.label11);
            this.groupPanelAutoConfig.Controls.Add(this.label10);
            this.groupPanelAutoConfig.Controls.Add(this.label3);
            this.groupPanelAutoConfig.DrawTitleBox = false;
            this.groupPanelAutoConfig.Location = new System.Drawing.Point(1, 34);
            this.groupPanelAutoConfig.Name = "groupPanelAutoConfig";
            this.groupPanelAutoConfig.Size = new System.Drawing.Size(390, 169);
            // 
            // 
            // 
            this.groupPanelAutoConfig.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelAutoConfig.Style.BackColorGradientAngle = 90;
            this.groupPanelAutoConfig.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelAutoConfig.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderBottomWidth = 1;
            this.groupPanelAutoConfig.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelAutoConfig.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderLeftWidth = 1;
            this.groupPanelAutoConfig.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderRightWidth = 1;
            this.groupPanelAutoConfig.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelAutoConfig.Style.BorderTopWidth = 1;
            this.groupPanelAutoConfig.Style.CornerDiameter = 4;
            this.groupPanelAutoConfig.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelAutoConfig.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelAutoConfig.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelAutoConfig.TabIndex = 55;
            this.groupPanelAutoConfig.Text = "统计选项";
            this.groupPanelAutoConfig.Click += new System.EventHandler(this.groupPanelAutoConfig_Click);
            // 
            // btnShowXitems
            // 
            this.btnShowXitems.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnShowXitems.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnShowXitems.Font = new System.Drawing.Font("宋体", 4.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnShowXitems.Location = new System.Drawing.Point(369, 9);
            this.btnShowXitems.Name = "btnShowXitems";
            this.btnShowXitems.Size = new System.Drawing.Size(16, 17);
            this.btnShowXitems.TabIndex = 70;
            this.btnShowXitems.Text = "...";
            this.btnShowXitems.Click += new System.EventHandler(this.btnShowXitems_Click);
            // 
            // ListContents
            // 
            this.ListContents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ListContents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSelected,
            this.ColumnContent,
            this.ColumnUnit});
            this.ListContents.Location = new System.Drawing.Point(159, 37);
            this.ListContents.Margin = new System.Windows.Forms.Padding(2);
            this.ListContents.Name = "ListContents";
            this.ListContents.RowHeadersVisible = false;
            this.ListContents.RowTemplate.Height = 25;
            this.ListContents.Size = new System.Drawing.Size(227, 102);
            this.ListContents.TabIndex = 69;
            this.ListContents.CurrentCellChanged += new System.EventHandler(this.ListContents_CurrentCellChanged);
            // 
            // ColumnSelected
            // 
            this.ColumnSelected.HeaderText = "";
            this.ColumnSelected.Name = "ColumnSelected";
            this.ColumnSelected.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnSelected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColumnSelected.Width = 28;
            // 
            // ColumnContent
            // 
            this.ColumnContent.HeaderText = "因子";
            this.ColumnContent.Name = "ColumnContent";
            this.ColumnContent.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnContent.Width = 62;
            // 
            // ColumnUnit
            // 
            this.ColumnUnit.HeaderText = "单位";
            this.ColumnUnit.Name = "ColumnUnit";
            this.ColumnUnit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnUnit.Width = 80;
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.label17);
            this.groupPanel1.Controls.Add(this.cmbRegion);
            this.groupPanel1.Controls.Add(this.label5);
            this.groupPanel1.Controls.Add(this.cmbDigits);
            this.groupPanel1.Controls.Add(this.label14);
            this.groupPanel1.Controls.Add(this.cmbYfactor3);
            this.groupPanel1.Controls.Add(this.label15);
            this.groupPanel1.Controls.Add(this.cmbYfactor2);
            this.groupPanel1.Controls.Add(this.label16);
            this.groupPanel1.Controls.Add(this.cmbYfactor1);
            this.groupPanel1.Controls.Add(this.label18);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(-1, -8);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(138, 158);
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
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 68;
            this.groupPanel1.Click += new System.EventHandler(this.groupPanel1_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 133);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 12);
            this.label17.TabIndex = 65;
            this.label17.Text = "小数位数:";
            // 
            // cmbRegion
            // 
            this.cmbRegion.DisplayMember = "Text";
            this.cmbRegion.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.ItemHeight = 15;
            this.cmbRegion.Location = new System.Drawing.Point(72, 16);
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(58, 21);
            this.cmbRegion.TabIndex = 66;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 65;
            this.label5.Text = "纵";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // cmbDigits
            // 
            this.cmbDigits.DisplayMember = "Text";
            this.cmbDigits.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDigits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDigits.FormattingEnabled = true;
            this.cmbDigits.ItemHeight = 15;
            this.cmbDigits.Location = new System.Drawing.Point(72, 130);
            this.cmbDigits.Name = "cmbDigits";
            this.cmbDigits.Size = new System.Drawing.Size(58, 21);
            this.cmbDigits.TabIndex = 66;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 62);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 65;
            this.label14.Text = "轴";
            this.label14.Click += new System.EventHandler(this.label14_Click);
            // 
            // cmbYfactor3
            // 
            this.cmbYfactor3.DisplayMember = "Text";
            this.cmbYfactor3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbYfactor3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYfactor3.FormattingEnabled = true;
            this.cmbYfactor3.ItemHeight = 15;
            this.cmbYfactor3.Location = new System.Drawing.Point(33, 98);
            this.cmbYfactor3.Name = "cmbYfactor3";
            this.cmbYfactor3.Size = new System.Drawing.Size(97, 21);
            this.cmbYfactor3.TabIndex = 66;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 77);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 65;
            this.label15.Text = "因";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // cmbYfactor2
            // 
            this.cmbYfactor2.DisplayMember = "Text";
            this.cmbYfactor2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbYfactor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYfactor2.FormattingEnabled = true;
            this.cmbYfactor2.ItemHeight = 15;
            this.cmbYfactor2.Location = new System.Drawing.Point(33, 71);
            this.cmbYfactor2.Name = "cmbYfactor2";
            this.cmbYfactor2.Size = new System.Drawing.Size(97, 21);
            this.cmbYfactor2.TabIndex = 66;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 91);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 65;
            this.label16.Text = "子";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // cmbYfactor1
            // 
            this.cmbYfactor1.DisplayMember = "Text";
            this.cmbYfactor1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbYfactor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYfactor1.FormattingEnabled = true;
            this.cmbYfactor1.ItemHeight = 15;
            this.cmbYfactor1.Location = new System.Drawing.Point(33, 44);
            this.cmbYfactor1.Name = "cmbYfactor1";
            this.cmbYfactor1.Size = new System.Drawing.Size(97, 21);
            this.cmbYfactor1.TabIndex = 66;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 20);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 12);
            this.label18.TabIndex = 65;
            this.label18.Text = "统计单位:";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // cmbXfactor
            // 
            this.cmbXfactor.DisplayMember = "Text";
            this.cmbXfactor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbXfactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXfactor.FormattingEnabled = true;
            this.cmbXfactor.ItemHeight = 15;
            this.cmbXfactor.Location = new System.Drawing.Point(205, 9);
            this.cmbXfactor.Name = "cmbXfactor";
            this.cmbXfactor.Size = new System.Drawing.Size(158, 21);
            this.cmbXfactor.TabIndex = 66;
            this.cmbXfactor.SelectedIndexChanged += new System.EventHandler(this.cmbXfactor_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(142, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 12);
            this.label13.TabIndex = 65;
            this.label13.Text = "容";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(142, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 12);
            this.label12.TabIndex = 65;
            this.label12.Text = "内";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(142, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 65;
            this.label11.Text = "计";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(142, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 65;
            this.label10.Text = "统";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(140, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 65;
            this.label3.Text = "横轴因子:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(326, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(66, 23);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExportImage
            // 
            this.btnExportImage.Location = new System.Drawing.Point(1, 211);
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(66, 23);
            this.btnExportImage.TabIndex = 56;
            this.btnExportImage.Text = "输出图形";
            this.btnExportImage.UseVisualStyleBackColor = true;
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(74, 211);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(66, 23);
            this.btnExportExcel.TabIndex = 57;
            this.btnExportExcel.Text = "输出表格";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // cmbUnit
            // 
            this.cmbUnit.DisplayMember = "Text";
            this.cmbUnit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnit.FormattingEnabled = true;
            this.cmbUnit.ItemHeight = 15;
            this.cmbUnit.Location = new System.Drawing.Point(1, 211);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Size = new System.Drawing.Size(101, 21);
            this.cmbUnit.TabIndex = 66;
            this.cmbUnit.Visible = false;
            this.cmbUnit.SelectedIndexChanged += new System.EventHandler(this.cmbUnit_SelectedIndexChanged);
            // 
            // groupPanelXfactors
            // 
            this.groupPanelXfactors.BackColor = System.Drawing.Color.Transparent;
            this.groupPanelXfactors.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanelXfactors.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanelXfactors.Controls.Add(this.button1);
            this.groupPanelXfactors.Controls.Add(this.chkboxXfactorItems);
            this.groupPanelXfactors.Controls.Add(this.dataGridXfactors);
            this.groupPanelXfactors.DrawTitleBox = false;
            this.groupPanelXfactors.Location = new System.Drawing.Point(652, 291);
            this.groupPanelXfactors.Name = "groupPanelXfactors";
            this.groupPanelXfactors.Size = new System.Drawing.Size(153, 146);
            // 
            // 
            // 
            this.groupPanelXfactors.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanelXfactors.Style.BackColorGradientAngle = 90;
            this.groupPanelXfactors.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanelXfactors.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelXfactors.Style.BorderBottomWidth = 1;
            this.groupPanelXfactors.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanelXfactors.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelXfactors.Style.BorderLeftWidth = 1;
            this.groupPanelXfactors.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelXfactors.Style.BorderRightWidth = 1;
            this.groupPanelXfactors.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanelXfactors.Style.BorderTopWidth = 1;
            this.groupPanelXfactors.Style.CornerDiameter = 4;
            this.groupPanelXfactors.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanelXfactors.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanelXfactors.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanelXfactors.TabIndex = 68;
            this.groupPanelXfactors.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(85, 124);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 20);
            this.button1.TabIndex = 71;
            this.button1.Text = "关闭";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkboxXfactorItems
            // 
            this.chkboxXfactorItems.AutoSize = true;
            this.chkboxXfactorItems.Location = new System.Drawing.Point(2, 128);
            this.chkboxXfactorItems.Margin = new System.Windows.Forms.Padding(2);
            this.chkboxXfactorItems.Name = "chkboxXfactorItems";
            this.chkboxXfactorItems.Size = new System.Drawing.Size(48, 16);
            this.chkboxXfactorItems.TabIndex = 70;
            this.chkboxXfactorItems.Text = "全选";
            this.chkboxXfactorItems.UseVisualStyleBackColor = true;
            this.chkboxXfactorItems.CheckedChanged += new System.EventHandler(this.chkboxXfactorItems_CheckedChanged);
            // 
            // dataGridXfactors
            // 
            this.dataGridXfactors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridXfactors.ColumnHeadersVisible = false;
            this.dataGridXfactors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1});
            this.dataGridXfactors.Location = new System.Drawing.Point(0, -1);
            this.dataGridXfactors.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridXfactors.Name = "dataGridXfactors";
            this.dataGridXfactors.RowHeadersVisible = false;
            this.dataGridXfactors.RowTemplate.Height = 27;
            this.dataGridXfactors.Size = new System.Drawing.Size(149, 126);
            this.dataGridXfactors.TabIndex = 69;
            this.dataGridXfactors.CurrentCellChanged += new System.EventHandler(this.ListContents_CurrentCellChanged);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewCheckBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "因子";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.Width = 110;
            // 
            // chartlet
            // 
            this.chartlet.Alpha3D = ((byte)(255));
            this.chartlet.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_2D_Aurora_FlatCrystal_Glow_NoBorder;
            this.chartlet.AutoBarWidth = true;
            this.chartlet.Background.Highlight = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(238)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            this.chartlet.Background.Lowlight = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.chartlet.Background.Paper = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chartlet.ChartTitle.BackColor = System.Drawing.Color.White;
            this.chartlet.ChartTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.ChartTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.chartlet.ChartTitle.OffsetY = 0;
            this.chartlet.ChartTitle.Show = true;
            this.chartlet.ChartTitle.Text = "Please bind a data source with BindChartData()!";
            this.chartlet.ChartType = FanG.Chartlet.ChartTypes.Bar;
            this.chartlet.ClientClick = "";
            this.chartlet.ClientMouseMove = "";
            this.chartlet.ClientMouseOut = "";
            this.chartlet.ClientMouseOver = "";
            this.chartlet.ClientUseMap = "";
            this.chartlet.Colorful = true;
            this.chartlet.ColorGuider.BackColor = System.Drawing.Color.White;
            this.chartlet.ColorGuider.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.ColorGuider.ForeColor = System.Drawing.Color.Black;
            this.chartlet.ColorGuider.Show = true;
            this.chartlet.CopyrightText = "Provided by Chartlet.cn";
            this.chartlet.Crystal.Contraction = 1;
            this.chartlet.Crystal.CoverFull = true;
            this.chartlet.Crystal.Direction = FanG.Chartlet.Direction.TopBottom;
            this.chartlet.Crystal.Enable = true;
            this.chartlet.Depth3D = 10;
            this.chartlet.Dimension = FanG.Chartlet.ChartDimensions.Chart2D;
            this.chartlet.Fill.Color1 = System.Drawing.Color.Empty;
            this.chartlet.Fill.Color2 = System.Drawing.Color.Empty;
            this.chartlet.Fill.Color3 = System.Drawing.Color.Empty;
            this.chartlet.Fill.ColorStyle = FanG.Chartlet.ColorStyles.Aurora;
            this.chartlet.Fill.ShiftStep = 0;
            this.chartlet.Fill.TextureEnable = false;
            this.chartlet.Fill.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.chartlet.GroupSize = 0;
            this.chartlet.ImageBorder = 0;
            this.chartlet.ImageFolder = "Chartlet";
            this.chartlet.ImageStyle = "";
            this.chartlet.InflateBottom = 0;
            this.chartlet.InflateLeft = 0;
            this.chartlet.InflateRight = 0;
            this.chartlet.InflateTop = 0;
            this.chartlet.LineConnectionRadius = 10;
            this.chartlet.LineConnectionType = FanG.Chartlet.LineConnectionTypes.Round;
            this.chartlet.Location = new System.Drawing.Point(398, 34);
            this.chartlet.Margin = new System.Windows.Forms.Padding(4);
            this.chartlet.MaxValueY = 0;
            this.chartlet.MinValueY = 0;
            this.chartlet.Name = "chartlet";
            this.chartlet.OutputFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            this.chartlet.RootPath = "C:\\\\";
            this.chartlet.RoundRadius = 2;
            this.chartlet.RoundRectangle = false;
            this.chartlet.Shadow.Alpha = ((byte)(192));
            this.chartlet.Shadow.Angle = 60F;
            this.chartlet.Shadow.Color = System.Drawing.Color.Black;
            this.chartlet.Shadow.Distance = 0;
            this.chartlet.Shadow.Enable = true;
            this.chartlet.Shadow.Hollow = false;
            this.chartlet.Shadow.Radius = 3;
            this.chartlet.ShowCopyright = false;
            this.chartlet.ShowErrorInfo = true;
            this.chartlet.Size = new System.Drawing.Size(452, 200);
            this.chartlet.Stroke.Color1 = System.Drawing.Color.Empty;
            this.chartlet.Stroke.Color2 = System.Drawing.Color.Empty;
            this.chartlet.Stroke.Color3 = System.Drawing.Color.Empty;
            this.chartlet.Stroke.ColorStyle = FanG.Chartlet.ColorStyles.None;
            this.chartlet.Stroke.ShiftStep = 0;
            this.chartlet.Stroke.TextureEnable = false;
            this.chartlet.Stroke.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.chartlet.Stroke.Width = 0;
            this.chartlet.TabIndex = 69;
            this.chartlet.Tips.BackColor = System.Drawing.Color.White;
            this.chartlet.Tips.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.Tips.ForeColor = System.Drawing.Color.Black;
            this.chartlet.Tips.Show = true;
            this.chartlet.XLabels.BackColor = System.Drawing.Color.White;
            this.chartlet.XLabels.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.XLabels.ForeColor = System.Drawing.Color.Black;
            this.chartlet.XLabels.RotateAngle = 10F;
            this.chartlet.XLabels.SampleSize = 1;
            this.chartlet.XLabels.Show = true;
            this.chartlet.XLabels.UnitFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.XLabels.UnitText = "XLabelsUnit";
            this.chartlet.XLabels.ValueFormat = "0.0";
            this.chartlet.YLabels.BackColor = System.Drawing.Color.White;
            this.chartlet.YLabels.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.YLabels.ForeColor = System.Drawing.Color.Black;
            this.chartlet.YLabels.Show = true;
            this.chartlet.YLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.YLabels.UnitText = "YLabelsUnit";
            this.chartlet.YLabels.ValueFormat = "0";
            this.chartlet.Click += new System.EventHandler(this.chartlet_Click);
            // 
            // btnStatistic
            // 
            this.btnStatistic.Location = new System.Drawing.Point(254, 209);
            this.btnStatistic.Name = "btnStatistic";
            this.btnStatistic.Size = new System.Drawing.Size(66, 23);
            this.btnStatistic.TabIndex = 56;
            this.btnStatistic.Text = "统计";
            this.btnStatistic.UseVisualStyleBackColor = true;
            this.btnStatistic.Click += new System.EventHandler(this.btnStatistic_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(604, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(58, 20);
            this.btnRefresh.TabIndex = 56;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cmbY
            // 
            this.cmbY.DisplayMember = "Text";
            this.cmbY.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbY.FormattingEnabled = true;
            this.cmbY.ItemHeight = 15;
            this.cmbY.Location = new System.Drawing.Point(398, 7);
            this.cmbY.Name = "cmbY";
            this.cmbY.Size = new System.Drawing.Size(61, 21);
            this.cmbY.TabIndex = 66;
            // 
            // cmbContent
            // 
            this.cmbContent.DisplayMember = "Text";
            this.cmbContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbContent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbContent.FormattingEnabled = true;
            this.cmbContent.ItemHeight = 15;
            this.cmbContent.Location = new System.Drawing.Point(464, 7);
            this.cmbContent.Name = "cmbContent";
            this.cmbContent.Size = new System.Drawing.Size(61, 21);
            this.cmbContent.TabIndex = 66;
            // 
            // cmbChartType
            // 
            this.cmbChartType.DisplayMember = "Text";
            this.cmbChartType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartType.FormattingEnabled = true;
            this.cmbChartType.ItemHeight = 15;
            this.cmbChartType.Location = new System.Drawing.Point(530, 7);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(61, 21);
            this.cmbChartType.TabIndex = 66;
            this.cmbChartType.SelectedIndexChanged += new System.EventHandler(this.cmbChartType_SelectedIndexChanged);
            // 
            // dGridViewStatisticRes
            // 
            this.dGridViewStatisticRes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGridViewStatisticRes.Location = new System.Drawing.Point(153, 213);
            this.dGridViewStatisticRes.Name = "dGridViewStatisticRes";
            this.dGridViewStatisticRes.RowTemplate.Height = 23;
            this.dGridViewStatisticRes.Size = new System.Drawing.Size(13, 18);
            this.dGridViewStatisticRes.TabIndex = 72;
            this.dGridViewStatisticRes.Visible = false;
            // 
            // advTreeLayers
            // 
            this.advTreeLayers.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLayers.AllowDrop = true;
            this.advTreeLayers.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLayers.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLayers.Location = new System.Drawing.Point(50, 12);
            this.advTreeLayers.Name = "advTreeLayers";
            this.advTreeLayers.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node2});
            this.advTreeLayers.NodesConnector = this.nodeConnector2;
            this.advTreeLayers.NodeStyle = this.elementStyle2;
            this.advTreeLayers.PathSeparator = ";";
            this.advTreeLayers.Size = new System.Drawing.Size(24, 128);
            this.advTreeLayers.Styles.Add(this.elementStyle2);
            this.advTreeLayers.TabIndex = 73;
            this.advTreeLayers.Text = "advTree1";
            this.advTreeLayers.Visible = false;
            this.advTreeLayers.Leave += new System.EventHandler(this.advTreeLayers_Leave);
            this.advTreeLayers.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeLayers_NodeClick);
            // 
            // node2
            // 
            this.node2.Name = "node2";
            this.node2.Text = "node2";
            // 
            // nodeConnector2
            // 
            this.nodeConnector2.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "earth");
            this.ImageList.Images.SetKeyName(1, "Root");
            this.ImageList.Images.SetKeyName(2, "DIR");
            this.ImageList.Images.SetKeyName(3, "DataDIRHalfOpen");
            this.ImageList.Images.SetKeyName(4, "DataDIRClosed");
            this.ImageList.Images.SetKeyName(5, "DataDIROpen");
            this.ImageList.Images.SetKeyName(6, "Layer");
            this.ImageList.Images.SetKeyName(7, "_annotation");
            this.ImageList.Images.SetKeyName(8, "_Dimension");
            this.ImageList.Images.SetKeyName(9, "_line");
            this.ImageList.Images.SetKeyName(10, "_MultiPatch");
            this.ImageList.Images.SetKeyName(11, "_point");
            this.ImageList.Images.SetKeyName(12, "_polygon");
            this.ImageList.Images.SetKeyName(13, "annotation");
            this.ImageList.Images.SetKeyName(14, "Dimension");
            this.ImageList.Images.SetKeyName(15, "line");
            this.ImageList.Images.SetKeyName(16, "MultiPatch");
            this.ImageList.Images.SetKeyName(17, "point");
            this.ImageList.Images.SetKeyName(18, "polygon");
            this.ImageList.Images.SetKeyName(19, "PublicVersion");
            this.ImageList.Images.SetKeyName(20, "PersonalVersion");
            this.ImageList.Images.SetKeyName(21, "INVISIBLE");
            this.ImageList.Images.SetKeyName(22, "VISIBLE");
            // 
            // rowMergeView1
            // 
            this.rowMergeView1.ColumnHeadersHeight = 40;
            this.rowMergeView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.rowMergeView1.Location = new System.Drawing.Point(1, 241);
            this.rowMergeView1.MergeColumnHeaderBackColor = System.Drawing.SystemColors.Control;
            this.rowMergeView1.MergeColumnNames = ((System.Collections.Generic.List<string>)(resources.GetObject("rowMergeView1.MergeColumnNames")));
            this.rowMergeView1.Name = "rowMergeView1";
            this.rowMergeView1.RowTemplate.Height = 23;
            this.rowMergeView1.Size = new System.Drawing.Size(849, 266);
            this.rowMergeView1.TabIndex = 71;
            // 
            // FrmCustomizeStatistic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 508);
            this.Controls.Add(this.advTreeLayers);
            this.Controls.Add(this.dGridViewStatisticRes);
            this.Controls.Add(this.rowMergeView1);
            this.Controls.Add(this.groupPanelXfactors);
            this.Controls.Add(this.chartlet);
            this.Controls.Add(this.cmbChartType);
            this.Controls.Add(this.cmbContent);
            this.Controls.Add(this.cmbY);
            this.Controls.Add(this.btnExportExcel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnStatistic);
            this.Controls.Add(this.btnExportImage);
            this.Controls.Add(this.cmbUnit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupPanelAutoConfig);
            this.Controls.Add(this.txtBoxLayer);
            this.Controls.Add(this.label2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCustomizeStatistic";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "定制统计";
            this.Load += new System.EventHandler(this.FrmCustomizeStatistic_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmCustomizeStatistic_FormClosed);
            this.Click += new System.EventHandler(this.FrmCustomizeStatistic_Click);
            this.groupPanelAutoConfig.ResumeLayout(false);
            this.groupPanelAutoConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ListContents)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.groupPanelXfactors.ResumeLayout(false);
            this.groupPanelXfactors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridXfactors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGridViewStatisticRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowMergeView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxLayer;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelAutoConfig;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbXfactor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.Label label17;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRegion;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDigits;
        private System.Windows.Forms.Label label14;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYfactor3;
        private System.Windows.Forms.Label label15;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYfactor2;
        private System.Windows.Forms.Label label16;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYfactor1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExportImage;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.DataGridView ListContents;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbUnit;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanelXfactors;
        private FanG.Chartlet chartlet;
        private System.Windows.Forms.Button btnStatistic;
        private System.Windows.Forms.CheckBox chkboxXfactorItems;
        private System.Windows.Forms.Button button1;
        private DevComponents.DotNetBar.ButtonX btnShowXitems;
        private System.Windows.Forms.Button btnRefresh;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbY;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbContent;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbChartType;
        private RowMergeView rowMergeView1;
        private System.Windows.Forms.DataGridView dGridViewStatisticRes;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUnit;
        private DevComponents.AdvTree.AdvTree advTreeLayers;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        public System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.DataGridView dataGridXfactors;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        //private AxFlexCell.AxGrid axGrid1;


    }
}