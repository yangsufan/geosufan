namespace GeoProperties.UserControls
{
    partial class uctrSource
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelSource = new DevComponents.DotNetBar.PanelEx();
            this.grpDataSource = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnSetDataSource = new DevComponents.DotNetBar.ButtonX();
            this.txtDataSource = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.grpExtent = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lblWest = new DevComponents.DotNetBar.LabelX();
            this.lblEast = new DevComponents.DotNetBar.LabelX();
            this.lblSouth = new DevComponents.DotNetBar.LabelX();
            this.lblNorth = new DevComponents.DotNetBar.LabelX();
            this.lblRight = new DevComponents.DotNetBar.LabelX();
            this.lblButtom = new DevComponents.DotNetBar.LabelX();
            this.lblLefe = new DevComponents.DotNetBar.LabelX();
            this.lblTop = new DevComponents.DotNetBar.LabelX();
            this.panelSource.SuspendLayout();
            this.grpDataSource.SuspendLayout();
            this.grpExtent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelSource
            // 
            this.panelSource.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelSource.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelSource.Controls.Add(this.grpDataSource);
            this.panelSource.Controls.Add(this.grpExtent);
            this.panelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSource.Location = new System.Drawing.Point(0, 0);
            this.panelSource.Name = "panelSource";
            this.panelSource.Size = new System.Drawing.Size(465, 285);
            this.panelSource.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelSource.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelSource.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelSource.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelSource.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelSource.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelSource.Style.GradientAngle = 90;
            this.panelSource.TabIndex = 0;
            // 
            // grpDataSource
            // 
            this.grpDataSource.BackColor = System.Drawing.Color.Transparent;
            this.grpDataSource.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpDataSource.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpDataSource.Controls.Add(this.btnSetDataSource);
            this.grpDataSource.Controls.Add(this.txtDataSource);
            this.grpDataSource.DrawTitleBox = false;
            this.grpDataSource.Location = new System.Drawing.Point(3, 106);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new System.Drawing.Size(459, 176);
            // 
            // 
            // 
            this.grpDataSource.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpDataSource.Style.BackColorGradientAngle = 90;
            this.grpDataSource.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpDataSource.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderBottomWidth = 1;
            this.grpDataSource.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpDataSource.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderLeftWidth = 1;
            this.grpDataSource.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderRightWidth = 1;
            this.grpDataSource.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpDataSource.Style.BorderTopWidth = 1;
            this.grpDataSource.Style.CornerDiameter = 4;
            this.grpDataSource.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpDataSource.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpDataSource.TabIndex = 11;
            this.grpDataSource.Text = "数据源";
            // 
            // btnSetDataSource
            // 
            this.btnSetDataSource.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSetDataSource.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSetDataSource.Location = new System.Drawing.Point(313, 129);
            this.btnSetDataSource.Name = "btnSetDataSource";
            this.btnSetDataSource.Size = new System.Drawing.Size(113, 23);
            this.btnSetDataSource.TabIndex = 1;
            this.btnSetDataSource.Text = "设置数据源";
            this.btnSetDataSource.Visible = false;
            this.btnSetDataSource.Click += new System.EventHandler(this.btnSetDataSource_Click);
            // 
            // txtDataSource
            // 
            this.txtDataSource.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.txtDataSource.Border.Class = "TextBoxBorder";
            this.txtDataSource.Location = new System.Drawing.Point(4, 0);
            this.txtDataSource.Multiline = true;
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.ReadOnly = true;
            this.txtDataSource.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDataSource.Size = new System.Drawing.Size(447, 149);
            this.txtDataSource.TabIndex = 0;
            this.txtDataSource.WordWrap = false;
            // 
            // grpExtent
            // 
            this.grpExtent.BackColor = System.Drawing.Color.Transparent;
            this.grpExtent.CanvasColor = System.Drawing.SystemColors.Control;
            this.grpExtent.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.grpExtent.Controls.Add(this.lblWest);
            this.grpExtent.Controls.Add(this.lblEast);
            this.grpExtent.Controls.Add(this.lblSouth);
            this.grpExtent.Controls.Add(this.lblNorth);
            this.grpExtent.Controls.Add(this.lblRight);
            this.grpExtent.Controls.Add(this.lblButtom);
            this.grpExtent.Controls.Add(this.lblLefe);
            this.grpExtent.Controls.Add(this.lblTop);
            this.grpExtent.DrawTitleBox = false;
            this.grpExtent.Location = new System.Drawing.Point(3, 3);
            this.grpExtent.Name = "grpExtent";
            this.grpExtent.Size = new System.Drawing.Size(459, 91);
            // 
            // 
            // 
            this.grpExtent.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.grpExtent.Style.BackColorGradientAngle = 90;
            this.grpExtent.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.grpExtent.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpExtent.Style.BorderBottomWidth = 1;
            this.grpExtent.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.grpExtent.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpExtent.Style.BorderLeftWidth = 1;
            this.grpExtent.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpExtent.Style.BorderRightWidth = 1;
            this.grpExtent.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.grpExtent.Style.BorderTopWidth = 1;
            this.grpExtent.Style.CornerDiameter = 4;
            this.grpExtent.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grpExtent.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.grpExtent.TabIndex = 10;
            this.grpExtent.Text = "四至";
            // 
            // lblWest
            // 
            this.lblWest.Location = new System.Drawing.Point(46, 22);
            this.lblWest.Name = "lblWest";
            this.lblWest.Size = new System.Drawing.Size(122, 17);
            this.lblWest.TabIndex = 8;
            // 
            // lblEast
            // 
            this.lblEast.Location = new System.Drawing.Point(304, 23);
            this.lblEast.Name = "lblEast";
            this.lblEast.Size = new System.Drawing.Size(122, 17);
            this.lblEast.TabIndex = 7;
            // 
            // lblSouth
            // 
            this.lblSouth.Location = new System.Drawing.Point(177, 45);
            this.lblSouth.Name = "lblSouth";
            this.lblSouth.Size = new System.Drawing.Size(122, 17);
            this.lblSouth.TabIndex = 6;
            // 
            // lblNorth
            // 
            this.lblNorth.Location = new System.Drawing.Point(177, -1);
            this.lblNorth.Name = "lblNorth";
            this.lblNorth.Size = new System.Drawing.Size(122, 17);
            this.lblNorth.TabIndex = 4;
            // 
            // lblRight
            // 
            this.lblRight.Location = new System.Drawing.Point(280, 22);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(29, 23);
            this.lblRight.TabIndex = 3;
            this.lblRight.Text = "右：";
            // 
            // lblButtom
            // 
            this.lblButtom.Location = new System.Drawing.Point(151, 47);
            this.lblButtom.Name = "lblButtom";
            this.lblButtom.Size = new System.Drawing.Size(31, 15);
            this.lblButtom.TabIndex = 2;
            this.lblButtom.Text = "下：";
            // 
            // lblLefe
            // 
            this.lblLefe.Location = new System.Drawing.Point(23, 21);
            this.lblLefe.Name = "lblLefe";
            this.lblLefe.Size = new System.Drawing.Size(29, 23);
            this.lblLefe.TabIndex = 1;
            this.lblLefe.Text = "左：";
            // 
            // lblTop
            // 
            this.lblTop.Location = new System.Drawing.Point(151, 0);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(31, 18);
            this.lblTop.TabIndex = 0;
            this.lblTop.Text = "上：";
            // 
            // uctrSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelSource);
            this.Name = "uctrSource";
            this.Size = new System.Drawing.Size(465, 285);
            this.panelSource.ResumeLayout(false);
            this.grpDataSource.ResumeLayout(false);
            this.grpExtent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelSource;
        private DevComponents.DotNetBar.Controls.GroupPanel grpDataSource;
        private DevComponents.DotNetBar.ButtonX btnSetDataSource;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataSource;
        private DevComponents.DotNetBar.Controls.GroupPanel grpExtent;
        private DevComponents.DotNetBar.LabelX lblEast;
        private DevComponents.DotNetBar.LabelX lblSouth;
        private DevComponents.DotNetBar.LabelX lblNorth;
        private DevComponents.DotNetBar.LabelX lblRight;
        private DevComponents.DotNetBar.LabelX lblButtom;
        private DevComponents.DotNetBar.LabelX lblLefe;
        private DevComponents.DotNetBar.LabelX lblTop;
        private DevComponents.DotNetBar.LabelX lblWest;
    }
}
