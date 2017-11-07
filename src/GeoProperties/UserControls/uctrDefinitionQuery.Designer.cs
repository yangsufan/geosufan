namespace GeoProperties.UserControls
{
    partial class uctrDefinitionQuery
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
            this.panelDefinition = new DevComponents.DotNetBar.PanelEx();
            this.btnOpenShow = new DevComponents.DotNetBar.ButtonX();
            this.btnSaveShow = new DevComponents.DotNetBar.ButtonX();
            this.txtDefinitionQuery = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.lblDefinitionQuery = new DevComponents.DotNetBar.LabelX();
            this.btnQueryBuilder = new DevComponents.DotNetBar.ButtonX();
            this.panelDefinition.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDefinition
            // 
            this.panelDefinition.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelDefinition.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelDefinition.Controls.Add(this.btnOpenShow);
            this.panelDefinition.Controls.Add(this.btnSaveShow);
            this.panelDefinition.Controls.Add(this.txtDefinitionQuery);
            this.panelDefinition.Controls.Add(this.lblDefinitionQuery);
            this.panelDefinition.Controls.Add(this.btnQueryBuilder);
            this.panelDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefinition.Location = new System.Drawing.Point(0, 0);
            this.panelDefinition.Name = "panelDefinition";
            this.panelDefinition.Size = new System.Drawing.Size(465, 285);
            this.panelDefinition.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelDefinition.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelDefinition.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelDefinition.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelDefinition.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelDefinition.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelDefinition.Style.GradientAngle = 90;
            this.panelDefinition.TabIndex = 0;
            // 
            // btnOpenShow
            // 
            this.btnOpenShow.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOpenShow.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOpenShow.Location = new System.Drawing.Point(261, 153);
            this.btnOpenShow.Name = "btnOpenShow";
            this.btnOpenShow.Size = new System.Drawing.Size(101, 23);
            this.btnOpenShow.TabIndex = 7;
            this.btnOpenShow.Text = "打开定义显示";
            this.btnOpenShow.Click += new System.EventHandler(this.btnOpenShow_Click);
            // 
            // btnSaveShow
            // 
            this.btnSaveShow.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSaveShow.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSaveShow.Location = new System.Drawing.Point(154, 153);
            this.btnSaveShow.Name = "btnSaveShow";
            this.btnSaveShow.Size = new System.Drawing.Size(101, 23);
            this.btnSaveShow.TabIndex = 6;
            this.btnSaveShow.Text = "保存定义显示";
            this.btnSaveShow.Click += new System.EventHandler(this.btnSaveShow_Click);
            // 
            // txtDefinitionQuery
            // 
            // 
            // 
            // 
            this.txtDefinitionQuery.Border.Class = "TextBoxBorder";
            this.txtDefinitionQuery.Location = new System.Drawing.Point(4, 26);
            this.txtDefinitionQuery.Multiline = true;
            this.txtDefinitionQuery.Name = "txtDefinitionQuery";
            this.txtDefinitionQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDefinitionQuery.Size = new System.Drawing.Size(358, 120);
            this.txtDefinitionQuery.TabIndex = 5;
            // 
            // lblDefinitionQuery
            // 
            this.lblDefinitionQuery.Location = new System.Drawing.Point(6, 5);
            this.lblDefinitionQuery.Name = "lblDefinitionQuery";
            this.lblDefinitionQuery.Size = new System.Drawing.Size(66, 23);
            this.lblDefinitionQuery.TabIndex = 4;
            this.lblDefinitionQuery.Text = "定义显示：";
            // 
            // btnQueryBuilder
            // 
            this.btnQueryBuilder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQueryBuilder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQueryBuilder.Location = new System.Drawing.Point(8, 153);
            this.btnQueryBuilder.Name = "btnQueryBuilder";
            this.btnQueryBuilder.Size = new System.Drawing.Size(101, 23);
            this.btnQueryBuilder.TabIndex = 3;
            this.btnQueryBuilder.Text = "设置条件";
            this.btnQueryBuilder.Click += new System.EventHandler(this.btnQueryBuilder_Click);
            // 
            // uctrDefinitionQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDefinition);
            this.Name = "uctrDefinitionQuery";
            this.Size = new System.Drawing.Size(465, 285);
            this.Load += new System.EventHandler(this.uctrDefinitionQuery_Load);
            this.panelDefinition.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelDefinition;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDefinitionQuery;
        private DevComponents.DotNetBar.LabelX lblDefinitionQuery;
        private DevComponents.DotNetBar.ButtonX btnQueryBuilder;
        private DevComponents.DotNetBar.ButtonX btnSaveShow;
        private DevComponents.DotNetBar.ButtonX btnOpenShow;
    }
}
