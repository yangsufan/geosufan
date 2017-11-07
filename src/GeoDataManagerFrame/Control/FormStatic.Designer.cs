namespace GeoDataManagerFrame
{
    partial class FormStatic
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
            this.comboBoxExTopic = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.buttonXstatic = new DevComponents.DotNetBar.ButtonX();
            this.buttonXQuit = new DevComponents.DotNetBar.ButtonX();
            this.listViewStatType = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxExTopic
            // 
            this.comboBoxExTopic.DisplayMember = "Text";
            this.comboBoxExTopic.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExTopic.FormattingEnabled = true;
            this.comboBoxExTopic.ItemHeight = 15;
            this.comboBoxExTopic.Location = new System.Drawing.Point(46, 12);
            this.comboBoxExTopic.Name = "comboBoxExTopic";
            this.comboBoxExTopic.Size = new System.Drawing.Size(218, 21);
            this.comboBoxExTopic.TabIndex = 0;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(37, 25);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "专题";
            // 
            // buttonXstatic
            // 
            this.buttonXstatic.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXstatic.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXstatic.Location = new System.Drawing.Point(37, 173);
            this.buttonXstatic.Name = "buttonXstatic";
            this.buttonXstatic.Size = new System.Drawing.Size(80, 25);
            this.buttonXstatic.TabIndex = 4;
            this.buttonXstatic.Text = "汇总";
            this.buttonXstatic.Click += new System.EventHandler(this.buttonXstatic_Click);
            // 
            // buttonXQuit
            // 
            this.buttonXQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXQuit.Location = new System.Drawing.Point(150, 173);
            this.buttonXQuit.Name = "buttonXQuit";
            this.buttonXQuit.Size = new System.Drawing.Size(80, 25);
            this.buttonXQuit.TabIndex = 5;
            this.buttonXQuit.Text = "退出";
            this.buttonXQuit.Click += new System.EventHandler(this.buttonXQuit_Click);
            // 
            // listViewStatType
            // 
            this.listViewStatType.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.listViewStatType.AllowDrop = true;
            this.listViewStatType.CheckBoxes = true;
            this.listViewStatType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listViewStatType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewStatType.GridLines = true;
            this.listViewStatType.Location = new System.Drawing.Point(0, 0);
            this.listViewStatType.Name = "listViewStatType";
            this.listViewStatType.Size = new System.Drawing.Size(227, 105);
            this.listViewStatType.TabIndex = 9;
            this.listViewStatType.UseCompatibleStateImageBehavior = false;
            this.listViewStatType.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "汇总类型";
            this.columnHeader2.Width = 130;
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.listViewStatType);
            this.groupPanel1.Location = new System.Drawing.Point(18, 45);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(233, 111);
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
            // 
            // FormStatic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 212);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.buttonXQuit);
            this.Controls.Add(this.buttonXstatic);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.comboBoxExTopic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStatic";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "汇总设置";
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExTopic;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonXstatic;
        private DevComponents.DotNetBar.ButtonX buttonXQuit;
        private System.Windows.Forms.ListView listViewStatType;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
    }
}