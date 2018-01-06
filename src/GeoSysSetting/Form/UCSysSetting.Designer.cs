namespace GeoSysSetting.SubControl
{
    partial class UCSysSetting
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.buttonModify = new DevComponents.DotNetBar.ButtonX();
            this.comboBoxDataType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.textBoxSettingDescription = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxSettingName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxSettingValue = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonXExport = new DevComponents.DotNetBar.ButtonX();
            this.buttonXAdd = new DevComponents.DotNetBar.ButtonX();
            this.buttonXOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonXImport = new DevComponents.DotNetBar.ButtonX();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.btnDelete = new DevComponents.DotNetBar.ButtonX();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelEx1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView);
            this.splitContainer1.Size = new System.Drawing.Size(850, 500);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnDelete);
            this.panelEx1.Controls.Add(this.buttonModify);
            this.panelEx1.Controls.Add(this.comboBoxDataType);
            this.panelEx1.Controls.Add(this.textBoxSettingDescription);
            this.panelEx1.Controls.Add(this.textBoxSettingName);
            this.panelEx1.Controls.Add(this.textBoxSettingValue);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.buttonXExport);
            this.panelEx1.Controls.Add(this.buttonXAdd);
            this.panelEx1.Controls.Add(this.buttonXOK);
            this.panelEx1.Controls.Add(this.buttonXImport);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(224, 500);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // buttonModify
            // 
            this.buttonModify.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonModify.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonModify.Location = new System.Drawing.Point(14, 350);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new System.Drawing.Size(92, 26);
            this.buttonModify.TabIndex = 9;
            this.buttonModify.Text = "修改";
            this.buttonModify.Click += new System.EventHandler(this.buttonModify_Click);
            // 
            // comboBoxDataType
            // 
            this.comboBoxDataType.DisplayMember = "Text";
            this.comboBoxDataType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDataType.FormattingEnabled = true;
            this.comboBoxDataType.ItemHeight = 15;
            this.comboBoxDataType.Location = new System.Drawing.Point(79, 88);
            this.comboBoxDataType.Name = "comboBoxDataType";
            this.comboBoxDataType.Size = new System.Drawing.Size(115, 21);
            this.comboBoxDataType.TabIndex = 8;
            this.comboBoxDataType.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataType_SelectedIndexChanged);
            // 
            // textBoxSettingDescription
            // 
            // 
            // 
            // 
            this.textBoxSettingDescription.Border.Class = "TextBoxBorder";
            this.textBoxSettingDescription.Location = new System.Drawing.Point(17, 144);
            this.textBoxSettingDescription.Multiline = true;
            this.textBoxSettingDescription.Name = "textBoxSettingDescription";
            this.textBoxSettingDescription.Size = new System.Drawing.Size(192, 100);
            this.textBoxSettingDescription.TabIndex = 7;
            // 
            // textBoxSettingName
            // 
            // 
            // 
            // 
            this.textBoxSettingName.Border.Class = "TextBoxBorder";
            this.textBoxSettingName.Location = new System.Drawing.Point(79, 27);
            this.textBoxSettingName.Name = "textBoxSettingName";
            this.textBoxSettingName.Size = new System.Drawing.Size(115, 21);
            this.textBoxSettingName.TabIndex = 6;
            // 
            // textBoxSettingValue
            // 
            // 
            // 
            // 
            this.textBoxSettingValue.Border.Class = "TextBoxBorder";
            this.textBoxSettingValue.Location = new System.Drawing.Point(79, 57);
            this.textBoxSettingValue.Name = "textBoxSettingValue";
            this.textBoxSettingValue.Size = new System.Drawing.Size(115, 21);
            this.textBoxSettingValue.TabIndex = 6;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(17, 120);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(66, 26);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "参数说明：";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(17, 86);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(66, 24);
            this.labelX5.TabIndex = 3;
            this.labelX5.Text = "参数类型：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(17, 55);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(66, 24);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "参 数 值：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(17, 25);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(66, 25);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "参数名称：";
            // 
            // buttonXExport
            // 
            this.buttonXExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXExport.Location = new System.Drawing.Point(117, 309);
            this.buttonXExport.Name = "buttonXExport";
            this.buttonXExport.Size = new System.Drawing.Size(92, 26);
            this.buttonXExport.TabIndex = 1;
            this.buttonXExport.Text = "导出参数";
            this.buttonXExport.Click += new System.EventHandler(this.buttonXExport_Click);
            // 
            // buttonXAdd
            // 
            this.buttonXAdd.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXAdd.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXAdd.Location = new System.Drawing.Point(117, 267);
            this.buttonXAdd.Name = "buttonXAdd";
            this.buttonXAdd.Size = new System.Drawing.Size(92, 26);
            this.buttonXAdd.TabIndex = 0;
            this.buttonXAdd.Text = "添加参数";
            this.buttonXAdd.Click += new System.EventHandler(this.buttonXAdd_Click);
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOK.Location = new System.Drawing.Point(14, 267);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(92, 26);
            this.buttonXOK.TabIndex = 0;
            this.buttonXOK.Text = "保存";
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // buttonXImport
            // 
            this.buttonXImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXImport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXImport.Location = new System.Drawing.Point(14, 309);
            this.buttonXImport.Name = "buttonXImport";
            this.buttonXImport.Size = new System.Drawing.Size(92, 26);
            this.buttonXImport.TabIndex = 0;
            this.buttonXImport.Text = "导入参数";
            this.buttonXImport.Click += new System.EventHandler(this.buttonXImport_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader5});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(622, 500);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "系统参数";
            this.columnHeader1.Width = 128;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "参数值";
            this.columnHeader2.Width = 137;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "参数类型";
            this.columnHeader4.Width = 120;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "参数说明";
            this.columnHeader5.Width = 235;
            // 
            // btnDelete
            // 
            this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDelete.Location = new System.Drawing.Point(117, 353);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(92, 23);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "删除参数";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // UCSysSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCSysSetting";
            this.Size = new System.Drawing.Size(850, 500);
            this.Load += new System.EventHandler(this.UCSysSetting_Load);
            this.Resize += new System.EventHandler(this.UCSysSetting_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonXExport;
        private DevComponents.DotNetBar.ButtonX buttonXImport;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxSettingDescription;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxSettingValue;
        private DevComponents.DotNetBar.ButtonX buttonXAdd;
        private DevComponents.DotNetBar.ButtonX buttonXOK;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDataType;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxSettingName;
        private DevComponents.DotNetBar.ButtonX buttonModify;
        private DevComponents.DotNetBar.ButtonX btnDelete;
    }
}
