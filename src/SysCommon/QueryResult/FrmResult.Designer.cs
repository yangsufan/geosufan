namespace SysCommon
{
    partial class FrmResult
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chartlet1 = new FanG.Chartlet();
            this.txtPageCount = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnNextPage = new DevComponents.DotNetBar.ButtonX();
            this.btnLastPage = new DevComponents.DotNetBar.ButtonX();
            this.btnExportExecl = new DevComponents.DotNetBar.ButtonX();
            this.btnExportChart = new DevComponents.DotNetBar.ButtonX();
            this.cbStyle = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chartlet1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtPageCount);
            this.splitContainer1.Panel2.Controls.Add(this.btnNextPage);
            this.splitContainer1.Panel2.Controls.Add(this.btnLastPage);
            this.splitContainer1.Panel2.Controls.Add(this.btnExportExecl);
            this.splitContainer1.Panel2.Controls.Add(this.btnExportChart);
            this.splitContainer1.Panel2.Controls.Add(this.cbStyle);
            this.splitContainer1.Panel2.Controls.Add(this.labelX1);
            this.splitContainer1.Size = new System.Drawing.Size(697, 604);
            this.splitContainer1.SplitterDistance = 521;
            this.splitContainer1.TabIndex = 1;
            // 
            // chartlet1
            // 
            this.chartlet1.Alpha3D = ((byte)(200));
            this.chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_3D_Aurora_FlatCrystal_NoGlow_NoBorder;
            this.chartlet1.AutoBarWidth = true;
            this.chartlet1.Background.Highlight = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(238)))), ((int)(((byte)(237)))), ((int)(((byte)(238)))));
            this.chartlet1.Background.Lowlight = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.chartlet1.Background.Paper = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chartlet1.ChartTitle.BackColor = System.Drawing.Color.White;
            this.chartlet1.ChartTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.ChartTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.chartlet1.ChartTitle.OffsetY = 0;
            this.chartlet1.ChartTitle.Show = true;
            this.chartlet1.ChartTitle.Text = "Please bind a data source with BindChartData()!";
            this.chartlet1.ChartType = FanG.Chartlet.ChartTypes.Bar;
            this.chartlet1.ClientClick = "";
            this.chartlet1.ClientMouseMove = "";
            this.chartlet1.ClientMouseOut = "";
            this.chartlet1.ClientMouseOver = "";
            this.chartlet1.ClientUseMap = "";
            this.chartlet1.Colorful = true;
            this.chartlet1.ColorGuider.BackColor = System.Drawing.Color.White;
            this.chartlet1.ColorGuider.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.ColorGuider.ForeColor = System.Drawing.Color.Black;
            this.chartlet1.ColorGuider.Show = true;
            this.chartlet1.CopyrightText = "Provided by Chartlet.cn";
            this.chartlet1.Crystal.Contraction = 1;
            this.chartlet1.Crystal.CoverFull = true;
            this.chartlet1.Crystal.Direction = FanG.Chartlet.Direction.BottomTop;
            this.chartlet1.Crystal.Enable = true;
            this.chartlet1.Depth3D = 10;
            this.chartlet1.Dimension = FanG.Chartlet.ChartDimensions.Chart3D;
            this.chartlet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartlet1.Fill.Color1 = System.Drawing.Color.Empty;
            this.chartlet1.Fill.Color2 = System.Drawing.Color.Empty;
            this.chartlet1.Fill.Color3 = System.Drawing.Color.Empty;
            this.chartlet1.Fill.ColorStyle = FanG.Chartlet.ColorStyles.Aurora;
            this.chartlet1.Fill.ShiftStep = 0;
            this.chartlet1.Fill.TextureEnable = false;
            this.chartlet1.Fill.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.chartlet1.GroupSize = 0;
            this.chartlet1.ImageBorder = 0;
            this.chartlet1.ImageFolder = "Chartlet";
            this.chartlet1.ImageStyle = "";
            this.chartlet1.InflateBottom = 0;
            this.chartlet1.InflateLeft = 0;
            this.chartlet1.InflateRight = 0;
            this.chartlet1.InflateTop = 0;
            this.chartlet1.LineConnectionRadius = 10;
            this.chartlet1.LineConnectionType = FanG.Chartlet.LineConnectionTypes.Round;
            this.chartlet1.Location = new System.Drawing.Point(0, 0);
            this.chartlet1.MaxValueY = 0;
            this.chartlet1.MinValueY = 0;
            this.chartlet1.Name = "chartlet1";
            this.chartlet1.OutputFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
            this.chartlet1.RootPath = "C:\\\\";
            this.chartlet1.RoundRadius = 2;
            this.chartlet1.RoundRectangle = false;
            this.chartlet1.Shadow.Alpha = ((byte)(192));
            this.chartlet1.Shadow.Angle = 60F;
            this.chartlet1.Shadow.Color = System.Drawing.Color.Black;
            this.chartlet1.Shadow.Distance = 0;
            this.chartlet1.Shadow.Enable = false;
            this.chartlet1.Shadow.Hollow = false;
            this.chartlet1.Shadow.Radius = 3;
            this.chartlet1.ShowCopyright = false;
            this.chartlet1.ShowErrorInfo = true;
            this.chartlet1.Size = new System.Drawing.Size(697, 521);
            this.chartlet1.Stroke.Color1 = System.Drawing.Color.Empty;
            this.chartlet1.Stroke.Color2 = System.Drawing.Color.Empty;
            this.chartlet1.Stroke.Color3 = System.Drawing.Color.Empty;
            this.chartlet1.Stroke.ColorStyle = FanG.Chartlet.ColorStyles.Aurora;
            this.chartlet1.Stroke.ShiftStep = 0;
            this.chartlet1.Stroke.TextureEnable = false;
            this.chartlet1.Stroke.TextureStyle = System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal;
            this.chartlet1.Stroke.Width = 1;
            this.chartlet1.TabIndex = 0;
            this.chartlet1.Tips.BackColor = System.Drawing.Color.White;
            this.chartlet1.Tips.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.Tips.ForeColor = System.Drawing.Color.Black;
            this.chartlet1.Tips.Show = true;
            this.chartlet1.XLabels.BackColor = System.Drawing.Color.White;
            this.chartlet1.XLabels.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.XLabels.ForeColor = System.Drawing.Color.Black;
            this.chartlet1.XLabels.RotateAngle = 30F;
            this.chartlet1.XLabels.SampleSize = 1;
            this.chartlet1.XLabels.Show = true;
            this.chartlet1.XLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.XLabels.UnitText = "XLabelsUnit";
            this.chartlet1.XLabels.ValueFormat = "0.0";
            this.chartlet1.YLabels.BackColor = System.Drawing.Color.White;
            this.chartlet1.YLabels.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.YLabels.ForeColor = System.Drawing.Color.Black;
            this.chartlet1.YLabels.Show = true;
            this.chartlet1.YLabels.UnitFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chartlet1.YLabels.UnitText = "YLabelsUnit";
            this.chartlet1.YLabels.ValueFormat = "0";
            // 
            // txtPageCount
            // 
            // 
            // 
            // 
            this.txtPageCount.Border.Class = "TextBoxBorder";
            this.txtPageCount.Location = new System.Drawing.Point(104, 25);
            this.txtPageCount.Name = "txtPageCount";
            this.txtPageCount.Size = new System.Drawing.Size(54, 21);
            this.txtPageCount.TabIndex = 6;
            // 
            // btnNextPage
            // 
            this.btnNextPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNextPage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNextPage.Location = new System.Drawing.Point(175, 25);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 23);
            this.btnNextPage.TabIndex = 5;
            this.btnNextPage.Text = "下页";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLastPage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLastPage.Location = new System.Drawing.Point(12, 25);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(75, 23);
            this.btnLastPage.TabIndex = 4;
            this.btnLastPage.Text = "上页";
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnExportExecl
            // 
            this.btnExportExecl.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportExecl.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportExecl.Location = new System.Drawing.Point(594, 25);
            this.btnExportExecl.Name = "btnExportExecl";
            this.btnExportExecl.Size = new System.Drawing.Size(75, 23);
            this.btnExportExecl.TabIndex = 3;
            this.btnExportExecl.Text = "导出统计表";
            this.btnExportExecl.Click += new System.EventHandler(this.btnExportExecl_Click);
            // 
            // btnExportChart
            // 
            this.btnExportChart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExportChart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExportChart.Location = new System.Drawing.Point(496, 25);
            this.btnExportChart.Name = "btnExportChart";
            this.btnExportChart.Size = new System.Drawing.Size(92, 23);
            this.btnExportChart.TabIndex = 2;
            this.btnExportChart.Text = "导出统计图";
            this.btnExportChart.Click += new System.EventHandler(this.btnExportChart_Click);
            // 
            // cbStyle
            // 
            this.cbStyle.DisplayMember = "Text";
            this.cbStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbStyle.FormattingEnabled = true;
            this.cbStyle.ItemHeight = 15;
            this.cbStyle.Location = new System.Drawing.Point(337, 27);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new System.Drawing.Size(153, 21);
            this.cbStyle.TabIndex = 1;
            this.cbStyle.SelectedIndexChanged += new System.EventHandler(this.cbStyle_SelectedIndexChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(256, 27);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "外观样式：";
            // 
            // FrmResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 604);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResult";
            this.ShowIcon = false;
            this.Text = "查询结果统计";
            this.Load += new System.EventHandler(this.FrmResult_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.ButtonX btnExportExecl;
        private DevComponents.DotNetBar.ButtonX btnExportChart;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbStyle;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPageCount;
        private DevComponents.DotNetBar.ButtonX btnNextPage;
        private DevComponents.DotNetBar.ButtonX btnLastPage;
        private FanG.Chartlet chartlet1;
    }
}