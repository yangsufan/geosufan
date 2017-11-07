namespace GeoProperties.UserControls
{
    partial class uctrFields
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
            this.lstFieldsView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.lblFields = new DevComponents.DotNetBar.LabelX();
            this.btnSelectAll = new DevComponents.DotNetBar.ButtonX();
            this.btnClearAll = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.lblDisplayFields = new DevComponents.DotNetBar.LabelX();
            this.cboFields = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panelFields = new DevComponents.DotNetBar.PanelEx();
            this.panelEx1.SuspendLayout();
            this.panelFields.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstFieldsView
            // 
            // 
            // 
            // 
            this.lstFieldsView.Border.Class = "ListViewBorder";
            this.lstFieldsView.CheckBoxes = true;
            this.lstFieldsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lstFieldsView.FullRowSelect = true;
            this.lstFieldsView.Location = new System.Drawing.Point(3, 78);
            this.lstFieldsView.Name = "lstFieldsView";
            this.lstFieldsView.Size = new System.Drawing.Size(459, 176);
            this.lstFieldsView.TabIndex = 0;
            this.lstFieldsView.UseCompatibleStateImageBehavior = false;
            this.lstFieldsView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Alias";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Length";
            this.columnHeader4.Width = 65;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Precision";
            this.columnHeader5.Width = 90;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Scale";
            // 
            // lblFields
            // 
            this.lblFields.Location = new System.Drawing.Point(14, 50);
            this.lblFields.Name = "lblFields";
            this.lblFields.Size = new System.Drawing.Size(302, 23);
            this.lblFields.TabIndex = 1;
            this.lblFields.Text = "选择可显示的字段，单击别名列更改任何字段的别名。";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelectAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelectAll.Location = new System.Drawing.Point(14, 258);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClearAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClearAll.Location = new System.Drawing.Point(95, 258);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 23);
            this.btnClearAll.TabIndex = 3;
            this.btnClearAll.Text = "全不选";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.lblDisplayFields);
            this.panelEx1.Controls.Add(this.cboFields);
            this.panelEx1.Location = new System.Drawing.Point(3, 3);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(459, 40);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 4;
            // 
            // lblDisplayFields
            // 
            this.lblDisplayFields.Location = new System.Drawing.Point(11, 10);
            this.lblDisplayFields.Name = "lblDisplayFields";
            this.lblDisplayFields.Size = new System.Drawing.Size(75, 23);
            this.lblDisplayFields.TabIndex = 1;
            this.lblDisplayFields.Text = "显示字段：";
            // 
            // cboFields
            // 
            this.cboFields.DisplayMember = "Text";
            this.cboFields.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboFields.FormattingEnabled = true;
            this.cboFields.ItemHeight = 15;
            this.cboFields.Location = new System.Drawing.Point(92, 10);
            this.cboFields.Name = "cboFields";
            this.cboFields.Size = new System.Drawing.Size(261, 21);
            this.cboFields.TabIndex = 0;
            // 
            // panelFields
            // 
            this.panelFields.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelFields.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelFields.Controls.Add(this.panelEx1);
            this.panelFields.Controls.Add(this.btnClearAll);
            this.panelFields.Controls.Add(this.btnSelectAll);
            this.panelFields.Controls.Add(this.lblFields);
            this.panelFields.Controls.Add(this.lstFieldsView);
            this.panelFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFields.Location = new System.Drawing.Point(0, 0);
            this.panelFields.Name = "panelFields";
            this.panelFields.Size = new System.Drawing.Size(465, 285);
            this.panelFields.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelFields.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelFields.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelFields.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelFields.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelFields.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelFields.Style.GradientAngle = 90;
            this.panelFields.TabIndex = 0;
            // 
            // uctrFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelFields);
            this.Name = "uctrFields";
            this.Size = new System.Drawing.Size(465, 285);
            this.panelEx1.ResumeLayout(false);
            this.panelFields.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx lstFieldsView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private DevComponents.DotNetBar.LabelX lblFields;
        private DevComponents.DotNetBar.ButtonX btnSelectAll;
        private DevComponents.DotNetBar.ButtonX btnClearAll;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX lblDisplayFields;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboFields;
        private DevComponents.DotNetBar.PanelEx panelFields;

    }
}
