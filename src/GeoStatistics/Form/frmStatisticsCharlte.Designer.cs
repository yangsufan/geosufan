namespace GeoStatistics
{
    partial class frmStatisticsCharlte
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnStatisticalTable = new DevComponents.DotNetBar.ButtonX();
            this.txtPageCount = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.bttNextPage = new DevComponents.DotNetBar.ButtonX();
            this.bttBackPage = new DevComponents.DotNetBar.ButtonX();
            this.bttOutput = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.chartlet = new FanG.Chartlet();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.10708F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.892922F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1209, 746);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.btnStatisticalTable);
            this.groupPanel2.Controls.Add(this.txtPageCount);
            this.groupPanel2.Controls.Add(this.bttNextPage);
            this.groupPanel2.Controls.Add(this.bttBackPage);
            this.groupPanel2.Controls.Add(this.bttOutput);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.Controls.Add(this.cmbType);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(4, 683);
            this.groupPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(1201, 59);
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
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 3;
            // 
            // btnStatisticalTable
            // 
            this.btnStatisticalTable.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStatisticalTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatisticalTable.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStatisticalTable.Location = new System.Drawing.Point(1012, 10);
            this.btnStatisticalTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStatisticalTable.Name = "btnStatisticalTable";
            this.btnStatisticalTable.Size = new System.Drawing.Size(156, 38);
            this.btnStatisticalTable.TabIndex = 6;
            this.btnStatisticalTable.Text = "导出统计表";
            this.btnStatisticalTable.Click += new System.EventHandler(this.btnStatisticalTable_Click);
            // 
            // txtPageCount
            // 
            // 
            // 
            // 
            this.txtPageCount.Border.Class = "TextBoxBorder";
            this.txtPageCount.Enabled = false;
            this.txtPageCount.Location = new System.Drawing.Point(269, 16);
            this.txtPageCount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPageCount.Name = "txtPageCount";
            this.txtPageCount.Size = new System.Drawing.Size(80, 25);
            this.txtPageCount.TabIndex = 5;
            // 
            // bttNextPage
            // 
            this.bttNextPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttNextPage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttNextPage.Enabled = false;
            this.bttNextPage.Location = new System.Drawing.Point(156, 16);
            this.bttNextPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bttNextPage.Name = "bttNextPage";
            this.bttNextPage.Size = new System.Drawing.Size(100, 29);
            this.bttNextPage.TabIndex = 4;
            this.bttNextPage.Text = "下页";
            this.bttNextPage.Click += new System.EventHandler(this.bttNextPage_Click);
            // 
            // bttBackPage
            // 
            this.bttBackPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttBackPage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttBackPage.Enabled = false;
            this.bttBackPage.Location = new System.Drawing.Point(27, 16);
            this.bttBackPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bttBackPage.Name = "bttBackPage";
            this.bttBackPage.Size = new System.Drawing.Size(100, 29);
            this.bttBackPage.TabIndex = 3;
            this.bttBackPage.Text = "上页";
            this.bttBackPage.Click += new System.EventHandler(this.bttBackPage_Click);
            // 
            // bttOutput
            // 
            this.bttOutput.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bttOutput.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOutput.Location = new System.Drawing.Point(828, 9);
            this.bttOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bttOutput.Name = "bttOutput";
            this.bttOutput.Size = new System.Drawing.Size(156, 38);
            this.bttOutput.TabIndex = 2;
            this.bttOutput.Text = "导出统计图";
            this.bttOutput.Click += new System.EventHandler(this.bttOutput_Click);
            // 
            // labelX1
            // 
            this.labelX1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(445, 16);
            this.labelX1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(89, 35);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "外观样式：";
            // 
            // cmbType
            // 
            this.cmbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbType.DisplayMember = "Text";
            this.cmbType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.ItemHeight = 15;
            this.cmbType.Location = new System.Drawing.Point(543, 16);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(196, 21);
            this.cmbType.TabIndex = 0;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.chartlet);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(4, 4);
            this.groupPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(1201, 671);
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
            this.groupPanel1.TabIndex = 2;
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
            this.chartlet.ColorGuider.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.ColorGuider.ForeColor = System.Drawing.Color.Black;
            this.chartlet.ColorGuider.Show = true;
            this.chartlet.CopyrightText = "Provided by Chartlet.cn";
            this.chartlet.Crystal.Contraction = 1;
            this.chartlet.Crystal.CoverFull = true;
            this.chartlet.Crystal.Direction = FanG.Chartlet.Direction.TopBottom;
            this.chartlet.Crystal.Enable = true;
            this.chartlet.Depth3D = 10;
            this.chartlet.Dimension = FanG.Chartlet.ChartDimensions.Chart2D;
            this.chartlet.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.chartlet.Location = new System.Drawing.Point(0, 0);
            this.chartlet.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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
            this.chartlet.Size = new System.Drawing.Size(1195, 665);
            this.chartlet.Stroke.Color1 = System.Drawing.Color.Empty;
            this.chartlet.Stroke.Color2 = System.Drawing.Color.Empty;
            this.chartlet.Stroke.Color3 = System.Drawing.Color.Empty;
            this.chartlet.Stroke.ColorStyle = FanG.Chartlet.ColorStyles.None;
            this.chartlet.Stroke.ShiftStep = 0;
            this.chartlet.Stroke.TextureEnable = false;
            this.chartlet.Stroke.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.chartlet.Stroke.Width = 0;
            this.chartlet.TabIndex = 0;
            this.chartlet.Tips.BackColor = System.Drawing.Color.White;
            this.chartlet.Tips.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.Tips.ForeColor = System.Drawing.Color.Black;
            this.chartlet.Tips.Show = true;
            this.chartlet.XLabels.BackColor = System.Drawing.Color.White;
            this.chartlet.XLabels.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.XLabels.ForeColor = System.Drawing.Color.Black;
            this.chartlet.XLabels.RotateAngle = 30F;
            this.chartlet.XLabels.SampleSize = 1;
            this.chartlet.XLabels.Show = true;
            this.chartlet.XLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.XLabels.UnitText = "XLabelsUnit";
            this.chartlet.XLabels.ValueFormat = "0.0";
            this.chartlet.YLabels.BackColor = System.Drawing.Color.White;
            this.chartlet.YLabels.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.YLabels.ForeColor = System.Drawing.Color.Black;
            this.chartlet.YLabels.Show = true;
            this.chartlet.YLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet.YLabels.UnitText = "YLabelsUnit";
            this.chartlet.YLabels.ValueFormat = "00";
            this.chartlet.SizeChanged += new System.EventHandler(this.chartlet_SizeChanged);
            // 
            // frmStatisticsCharlte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 746);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStatisticsCharlte";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "统计图";
            this.Load += new System.EventHandler(this.frmStatisticsCharlte_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStatisticsCharlte_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbType;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX bttOutput;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPageCount;
        private DevComponents.DotNetBar.ButtonX bttNextPage;
        private DevComponents.DotNetBar.ButtonX bttBackPage;
        private DevComponents.DotNetBar.ButtonX btnStatisticalTable;
        private FanG.Chartlet chartlet;
    }
}