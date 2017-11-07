namespace GeoSysUpdate
{
    partial class frmXZQZTStatistical
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.cmboxExtent = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmboxType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnDeleteSolution = new DevComponents.DotNetBar.ButtonX();
            this.btnCustomStatistics = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnExportStatistical = new DevComponents.DotNetBar.ButtonX();
            this.btnStatistical = new DevComponents.DotNetBar.ButtonX();
            this.btnExportImage = new DevComponents.DotNetBar.ButtonX();
            this.dataGViewTable = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.CumNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumTYType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabResult = new DevComponents.DotNetBar.TabControl();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.tbiGrid = new DevComponents.DotNetBar.TabItem(this.components);
            this.tabControlPanel3 = new DevComponents.DotNetBar.TabControlPanel();
            this.ChartColumnar = new FanG.Chartlet();
            this.btnChartPie = new DevComponents.DotNetBar.ButtonX();
            this.btnChartColumnar = new DevComponents.DotNetBar.ButtonX();
            this.tbiColumnar = new DevComponents.DotNetBar.TabItem(this.components);
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGViewTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabResult)).BeginInit();
            this.tabResult.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            this.tabControlPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.cmboxExtent);
            this.groupPanel1.Controls.Add(this.cmboxType);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(636, 66);
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
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far;
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "统计设置";
            // 
            // cmboxExtent
            // 
            this.cmboxExtent.DisplayMember = "Text";
            this.cmboxExtent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmboxExtent.FormattingEnabled = true;
            this.cmboxExtent.ItemHeight = 15;
            this.cmboxExtent.Location = new System.Drawing.Point(379, 9);
            this.cmboxExtent.Name = "cmboxExtent";
            this.cmboxExtent.Size = new System.Drawing.Size(217, 21);
            this.cmboxExtent.TabIndex = 3;
            this.cmboxExtent.SelectedIndexChanged += new System.EventHandler(this.cmboxExtent_SelectedIndexChanged);
            // 
            // cmboxType
            // 
            this.cmboxType.DisplayMember = "Text";
            this.cmboxType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmboxType.FormattingEnabled = true;
            this.cmboxType.ItemHeight = 15;
            this.cmboxType.Location = new System.Drawing.Point(74, 9);
            this.cmboxType.Name = "cmboxType";
            this.cmboxType.Size = new System.Drawing.Size(229, 21);
            this.cmboxType.TabIndex = 2;
            this.cmboxType.SelectedIndexChanged += new System.EventHandler(this.cmboxType_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(311, 11);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(66, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "统计范围：";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(10, 11);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(69, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "专题类型：";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.btnDeleteSolution);
            this.groupPanel2.Controls.Add(this.btnCustomStatistics);
            this.groupPanel2.Controls.Add(this.btnCancle);
            this.groupPanel2.Controls.Add(this.btnExportStatistical);
            this.groupPanel2.Controls.Add(this.btnStatistical);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupPanel2.Location = new System.Drawing.Point(0, 416);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(636, 60);
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
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far;
            this.groupPanel2.TabIndex = 1;
            this.groupPanel2.Text = "输出设置";
            // 
            // btnDeleteSolution
            // 
            this.btnDeleteSolution.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDeleteSolution.Location = new System.Drawing.Point(103, 3);
            this.btnDeleteSolution.Name = "btnDeleteSolution";
            this.btnDeleteSolution.Size = new System.Drawing.Size(103, 23);
            this.btnDeleteSolution.TabIndex = 3;
            this.btnDeleteSolution.Text = "管理统计方案";
            this.btnDeleteSolution.Click += new System.EventHandler(this.btnDeleteSolution_Click);
            // 
            // btnCustomStatistics
            // 
            this.btnCustomStatistics.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCustomStatistics.Location = new System.Drawing.Point(214, 3);
            this.btnCustomStatistics.Name = "btnCustomStatistics";
            this.btnCustomStatistics.Size = new System.Drawing.Size(103, 23);
            this.btnCustomStatistics.TabIndex = 2;
            this.btnCustomStatistics.Text = "自定义统计方案";
            this.btnCustomStatistics.Click += new System.EventHandler(this.btnCustomStatistics_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.Location = new System.Drawing.Point(542, 3);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(79, 23);
            this.btnCancle.TabIndex = 0;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnExportStatistical
            // 
            this.btnExportStatistical.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportStatistical.Location = new System.Drawing.Point(431, 3);
            this.btnExportStatistical.Name = "btnExportStatistical";
            this.btnExportStatistical.Size = new System.Drawing.Size(87, 23);
            this.btnExportStatistical.TabIndex = 0;
            this.btnExportStatistical.Text = "导出统计表";
            this.btnExportStatistical.Click += new System.EventHandler(this.btnExportStatistical_Click);
            // 
            // btnStatistical
            // 
            this.btnStatistical.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStatistical.Location = new System.Drawing.Point(343, 3);
            this.btnStatistical.Name = "btnStatistical";
            this.btnStatistical.Size = new System.Drawing.Size(79, 23);
            this.btnStatistical.TabIndex = 0;
            this.btnStatistical.Text = "统计";
            this.btnStatistical.Click += new System.EventHandler(this.btnStatistical_Click);
            // 
            // btnExportImage
            // 
            this.btnExportImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportImage.Location = new System.Drawing.Point(540, 290);
            this.btnExportImage.Name = "btnExportImage";
            this.btnExportImage.Size = new System.Drawing.Size(84, 28);
            this.btnExportImage.TabIndex = 1;
            this.btnExportImage.Text = "导出统计图";
            this.btnExportImage.Click += new System.EventHandler(this.btnExportImage_Click);
            // 
            // dataGViewTable
            // 
            this.dataGViewTable.AllowUserToAddRows = false;
            this.dataGViewTable.AllowUserToDeleteRows = false;
            this.dataGViewTable.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGViewTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGViewTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGViewTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CumNumber,
            this.CumType,
            this.CumTYType,
            this.CumArea,
            this.CumPercentage});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGViewTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGViewTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGViewTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGViewTable.Location = new System.Drawing.Point(1, 1);
            this.dataGViewTable.Name = "dataGViewTable";
            this.dataGViewTable.ReadOnly = true;
            this.dataGViewTable.RowTemplate.Height = 23;
            this.dataGViewTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGViewTable.Size = new System.Drawing.Size(634, 322);
            this.dataGViewTable.TabIndex = 0;
            // 
            // CumNumber
            // 
            this.CumNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumNumber.DataPropertyName = "CumNumber";
            this.CumNumber.HeaderText = "序号";
            this.CumNumber.Name = "CumNumber";
            this.CumNumber.ReadOnly = true;
            this.CumNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CumType
            // 
            this.CumType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumType.DataPropertyName = "CumType";
            this.CumType.HeaderText = "统计类型";
            this.CumType.Name = "CumType";
            this.CumType.ReadOnly = true;
            this.CumType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CumTYType
            // 
            this.CumTYType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumTYType.HeaderText = "地类名称";
            this.CumTYType.Name = "CumTYType";
            this.CumTYType.ReadOnly = true;
            this.CumTYType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CumArea
            // 
            this.CumArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumArea.DataPropertyName = "CumArea";
            this.CumArea.HeaderText = "面积（平方米）";
            this.CumArea.Name = "CumArea";
            this.CumArea.ReadOnly = true;
            this.CumArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CumPercentage
            // 
            this.CumPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CumPercentage.DataPropertyName = "Percent";
            this.CumPercentage.HeaderText = "占百地分比(%)";
            this.CumPercentage.Name = "CumPercentage";
            this.CumPercentage.ReadOnly = true;
            this.CumPercentage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabResult
            // 
            this.tabResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabResult.CanReorderTabs = true;
            this.tabResult.Controls.Add(this.tabControlPanel1);
            this.tabResult.Controls.Add(this.tabControlPanel3);
            this.tabResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResult.Location = new System.Drawing.Point(0, 66);
            this.tabResult.Name = "tabResult";
            this.tabResult.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabResult.SelectedTabIndex = 0;
            this.tabResult.Size = new System.Drawing.Size(636, 350);
            this.tabResult.TabIndex = 3;
            this.tabResult.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabResult.Tabs.Add(this.tbiGrid);
            this.tabResult.Tabs.Add(this.tbiColumnar);
            this.tabResult.Text = "tabControl1";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Controls.Add(this.dataGViewTable);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(636, 324);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tbiGrid;
            // 
            // tbiGrid
            // 
            this.tbiGrid.AttachedControl = this.tabControlPanel1;
            this.tbiGrid.Name = "tbiGrid";
            this.tbiGrid.Text = "统计表";
            this.tbiGrid.Click += new System.EventHandler(this.tbiGrid_Click);
            // 
            // tabControlPanel3
            // 
            this.tabControlPanel3.Controls.Add(this.ChartColumnar);
            this.tabControlPanel3.Controls.Add(this.btnChartPie);
            this.tabControlPanel3.Controls.Add(this.btnChartColumnar);
            this.tabControlPanel3.Controls.Add(this.btnExportImage);
            this.tabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel3.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel3.Name = "tabControlPanel3";
            this.tabControlPanel3.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel3.Size = new System.Drawing.Size(636, 324);
            this.tabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel3.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel3.Style.GradientAngle = 90;
            this.tabControlPanel3.TabIndex = 3;
            this.tabControlPanel3.TabItem = this.tbiColumnar;
            // 
            // ChartColumnar
            // 
            this.ChartColumnar.Alpha3D = ((byte)(255));
            this.ChartColumnar.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_2D_Aurora_FlatCrystal_Glow_NoBorder;
            this.ChartColumnar.AutoBarWidth = true;
            this.ChartColumnar.Background.Highlight = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(238)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            this.ChartColumnar.Background.Lowlight = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ChartColumnar.Background.Paper = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ChartColumnar.ChartTitle.BackColor = System.Drawing.Color.White;
            this.ChartColumnar.ChartTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.ChartTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.ChartColumnar.ChartTitle.OffsetY = 0;
            this.ChartColumnar.ChartTitle.Show = true;
            this.ChartColumnar.ChartTitle.Text = "Please bind a data source with BindChartData()!";
            this.ChartColumnar.ChartType = FanG.Chartlet.ChartTypes.Bar;
            this.ChartColumnar.ClientClick = "";
            this.ChartColumnar.ClientMouseMove = "";
            this.ChartColumnar.ClientMouseOut = "";
            this.ChartColumnar.ClientMouseOver = "";
            this.ChartColumnar.ClientUseMap = "";
            this.ChartColumnar.Colorful = true;
            this.ChartColumnar.ColorGuider.BackColor = System.Drawing.Color.White;
            this.ChartColumnar.ColorGuider.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.ColorGuider.ForeColor = System.Drawing.Color.Black;
            this.ChartColumnar.ColorGuider.Show = true;
            this.ChartColumnar.CopyrightText = "Provided by Chartlet.cn";
            this.ChartColumnar.Crystal.Contraction = 1;
            this.ChartColumnar.Crystal.CoverFull = true;
            this.ChartColumnar.Crystal.Direction = FanG.Chartlet.Direction.TopBottom;
            this.ChartColumnar.Crystal.Enable = true;
            this.ChartColumnar.Depth3D = 10;
            this.ChartColumnar.Dimension = FanG.Chartlet.ChartDimensions.Chart2D;
            this.ChartColumnar.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartColumnar.Fill.Color1 = System.Drawing.Color.Empty;
            this.ChartColumnar.Fill.Color2 = System.Drawing.Color.Empty;
            this.ChartColumnar.Fill.Color3 = System.Drawing.Color.Empty;
            this.ChartColumnar.Fill.ColorStyle = FanG.Chartlet.ColorStyles.Aurora;
            this.ChartColumnar.Fill.ShiftStep = 0;
            this.ChartColumnar.Fill.TextureEnable = false;
            this.ChartColumnar.Fill.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.ChartColumnar.GroupSize = 0;
            this.ChartColumnar.ImageBorder = 0;
            this.ChartColumnar.ImageFolder = "Chartlet";
            this.ChartColumnar.ImageStyle = "";
            this.ChartColumnar.InflateBottom = 0;
            this.ChartColumnar.InflateLeft = 0;
            this.ChartColumnar.InflateRight = 0;
            this.ChartColumnar.InflateTop = 0;
            this.ChartColumnar.LineConnectionRadius = 10;
            this.ChartColumnar.LineConnectionType = FanG.Chartlet.LineConnectionTypes.Round;
            this.ChartColumnar.Location = new System.Drawing.Point(1, 1);
            this.ChartColumnar.MaxValueY = 0D;
            this.ChartColumnar.MinValueY = 0D;
            this.ChartColumnar.Name = "ChartColumnar";
            this.ChartColumnar.OutputFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            this.ChartColumnar.RootPath = "C:\\\\";
            this.ChartColumnar.RoundRadius = 2;
            this.ChartColumnar.RoundRectangle = false;
            this.ChartColumnar.Shadow.Alpha = ((byte)(192));
            this.ChartColumnar.Shadow.Angle = 60F;
            this.ChartColumnar.Shadow.Color = System.Drawing.Color.Black;
            this.ChartColumnar.Shadow.Distance = 0;
            this.ChartColumnar.Shadow.Enable = true;
            this.ChartColumnar.Shadow.Hollow = false;
            this.ChartColumnar.Shadow.Radius = 3;
            this.ChartColumnar.ShowCopyright = false;
            this.ChartColumnar.ShowErrorInfo = true;
            this.ChartColumnar.Size = new System.Drawing.Size(634, 283);
            this.ChartColumnar.Stroke.Color1 = System.Drawing.Color.Empty;
            this.ChartColumnar.Stroke.Color2 = System.Drawing.Color.Empty;
            this.ChartColumnar.Stroke.Color3 = System.Drawing.Color.Empty;
            this.ChartColumnar.Stroke.ColorStyle = FanG.Chartlet.ColorStyles.None;
            this.ChartColumnar.Stroke.ShiftStep = 0;
            this.ChartColumnar.Stroke.TextureEnable = false;
            this.ChartColumnar.Stroke.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.ChartColumnar.Stroke.Width = 0;
            this.ChartColumnar.TabIndex = 4;
            this.ChartColumnar.Tips.BackColor = System.Drawing.Color.White;
            this.ChartColumnar.Tips.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.Tips.ForeColor = System.Drawing.Color.Black;
            this.ChartColumnar.Tips.Show = true;
            this.ChartColumnar.XLabels.BackColor = System.Drawing.Color.White;
            this.ChartColumnar.XLabels.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.XLabels.ForeColor = System.Drawing.Color.Black;
            this.ChartColumnar.XLabels.RotateAngle = 30F;
            this.ChartColumnar.XLabels.SampleSize = 1;
            this.ChartColumnar.XLabels.Show = true;
            this.ChartColumnar.XLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.XLabels.UnitText = "XLabelsUnit";
            this.ChartColumnar.XLabels.ValueFormat = "0.0";
            this.ChartColumnar.YLabels.BackColor = System.Drawing.Color.White;
            this.ChartColumnar.YLabels.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.YLabels.ForeColor = System.Drawing.Color.Black;
            this.ChartColumnar.YLabels.Show = true;
            this.ChartColumnar.YLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ChartColumnar.YLabels.UnitText = "YLabelsUnit";
            this.ChartColumnar.YLabels.ValueFormat = "0";
            // 
            // btnChartPie
            // 
            this.btnChartPie.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChartPie.Location = new System.Drawing.Point(341, 290);
            this.btnChartPie.Name = "btnChartPie";
            this.btnChartPie.Size = new System.Drawing.Size(84, 28);
            this.btnChartPie.TabIndex = 3;
            this.btnChartPie.Text = "饼状图";
            this.btnChartPie.Click += new System.EventHandler(this.btnChartPie_Click);
            // 
            // btnChartColumnar
            // 
            this.btnChartColumnar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnChartColumnar.Enabled = false;
            this.btnChartColumnar.Location = new System.Drawing.Point(442, 290);
            this.btnChartColumnar.Name = "btnChartColumnar";
            this.btnChartColumnar.Size = new System.Drawing.Size(84, 28);
            this.btnChartColumnar.TabIndex = 2;
            this.btnChartColumnar.Text = "柱状图";
            this.btnChartColumnar.Click += new System.EventHandler(this.btnChartColumnar_Click);
            // 
            // tbiColumnar
            // 
            this.tbiColumnar.AttachedControl = this.tabControlPanel3;
            this.tbiColumnar.Name = "tbiColumnar";
            this.tbiColumnar.Text = "统计图";
            this.tbiColumnar.Click += new System.EventHandler(this.tbiColumnar_Click);
            // 
            // frmXZQZTStatistical
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 476);
            this.Controls.Add(this.tabResult);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmXZQZTStatistical";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "行政区专题统计";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGViewTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabResult)).EndInit();
            this.tabResult.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        //private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmboxType;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmboxExtent;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnExportStatistical;
        private DevComponents.DotNetBar.ButtonX btnStatistical;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGViewTable;
        private DevComponents.DotNetBar.TabControl tabResult;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tbiGrid;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel3;
        private DevComponents.DotNetBar.TabItem tbiColumnar;
        private DevComponents.DotNetBar.ButtonX btnExportImage;
        private DevComponents.DotNetBar.ButtonX btnCustomStatistics;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumTYType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumPercentage;
        private DevComponents.DotNetBar.ButtonX btnChartPie;
        private DevComponents.DotNetBar.ButtonX btnChartColumnar;
        private FanG.Chartlet ChartColumnar;
        private DevComponents.DotNetBar.ButtonX btnDeleteSolution;
    }
}