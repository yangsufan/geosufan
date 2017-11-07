namespace GeoDataCenterFunLib
{
    partial class frmQuerySet
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
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.cboxSearchLayer = new System.Windows.Forms.ComboBox();
            this.rdSelect = new System.Windows.Forms.RadioButton();
            this.rdAll = new System.Windows.Forms.RadioButton();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.lstLayer = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(103, 306);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.Location = new System.Drawing.Point(170, 306);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(61, 23);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "退出";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // cboxSearchLayer
            // 
            this.cboxSearchLayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSearchLayer.FormattingEnabled = true;
            this.cboxSearchLayer.Location = new System.Drawing.Point(27, 12);
            this.cboxSearchLayer.Name = "cboxSearchLayer";
            this.cboxSearchLayer.Size = new System.Drawing.Size(168, 20);
            this.cboxSearchLayer.TabIndex = 9;
            this.cboxSearchLayer.SelectedIndexChanged += new System.EventHandler(this.cboxSearchLayer_SelectedIndexChanged);
            // 
            // rdSelect
            // 
            this.rdSelect.AutoSize = true;
            this.rdSelect.BackColor = System.Drawing.Color.Transparent;
            this.rdSelect.Location = new System.Drawing.Point(27, 38);
            this.rdSelect.Name = "rdSelect";
            this.rdSelect.Size = new System.Drawing.Size(83, 16);
            this.rdSelect.TabIndex = 11;
            this.rdSelect.TabStop = true;
            this.rdSelect.Text = "选择的要素";
            this.rdSelect.UseVisualStyleBackColor = false;
            // 
            // rdAll
            // 
            this.rdAll.AutoSize = true;
            this.rdAll.BackColor = System.Drawing.Color.Transparent;
            this.rdAll.Location = new System.Drawing.Point(124, 38);
            this.rdAll.Name = "rdAll";
            this.rdAll.Size = new System.Drawing.Size(71, 16);
            this.rdAll.TabIndex = 12;
            this.rdAll.TabStop = true;
            this.rdAll.Text = "所有要素";
            this.rdAll.UseVisualStyleBackColor = false;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.cboxSearchLayer);
            this.groupPanel1.Controls.Add(this.rdAll);
            this.groupPanel1.Controls.Add(this.rdSelect);
            this.groupPanel1.Location = new System.Drawing.Point(3, 11);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(228, 90);
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
            this.groupPanel1.TabIndex = 13;
            this.groupPanel1.Text = "查询图层";
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.btnSelReverse);
            this.groupPanel2.Controls.Add(this.btnSelAll);
            this.groupPanel2.Controls.Add(this.lstLayer);
            this.groupPanel2.Location = new System.Drawing.Point(3, 107);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(228, 193);
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
            this.groupPanel2.TabIndex = 14;
            this.groupPanel2.Text = "被查询图层";
            // 
            // btnSelReverse
            // 
            this.btnSelReverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelReverse.Location = new System.Drawing.Point(134, 137);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(61, 23);
            this.btnSelReverse.TabIndex = 4;
            this.btnSelReverse.Text = "反选";
            this.btnSelReverse.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.Location = new System.Drawing.Point(67, 137);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(61, 23);
            this.btnSelAll.TabIndex = 3;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstLayer
            // 
            // 
            // 
            // 
            this.lstLayer.Border.Class = "ListViewBorder";
            this.lstLayer.CheckBoxes = true;
            this.lstLayer.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstLayer.GridLines = true;
            this.lstLayer.Location = new System.Drawing.Point(27, 3);
            this.lstLayer.Name = "lstLayer";
            this.lstLayer.Size = new System.Drawing.Size(168, 128);
            this.lstLayer.TabIndex = 0;
            this.lstLayer.UseCompatibleStateImageBehavior = false;
            this.lstLayer.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "图层名";
            this.columnHeader1.Width = 208;
            // 
            // frmQuerySet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 341);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQuerySet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询设置";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private System.Windows.Forms.ComboBox cboxSearchLayer;
        private System.Windows.Forms.RadioButton rdSelect;
        private System.Windows.Forms.RadioButton rdAll;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.ListViewEx lstLayer;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        
    }
}