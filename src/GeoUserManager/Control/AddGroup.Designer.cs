namespace GeoUserManager
{
    partial class AddGroup
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.txtComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxProjectgroup = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboBoxRoleType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnAddRole = new DevComponents.DotNetBar.ButtonX();
            this.txtRole = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.txtComment);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.comboBoxProjectgroup);
            this.panelEx1.Controls.Add(this.comboBoxRoleType);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnAddRole);
            this.panelEx1.Controls.Add(this.txtRole);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(318, 192);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // txtComment
            // 
            // 
            // 
            // 
            this.txtComment.Border.Class = "TextBoxBorder";
            this.txtComment.Location = new System.Drawing.Point(69, 95);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(239, 58);
            this.txtComment.TabIndex = 24;
            this.txtComment.WordWrap = false;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(9, 95);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(66, 19);
            this.labelX2.TabIndex = 25;
            this.labelX2.Text = "备    注：";
            // 
            // comboBoxProjectgroup
            // 
            this.comboBoxProjectgroup.DisplayMember = "Text";
            this.comboBoxProjectgroup.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxProjectgroup.FormattingEnabled = true;
            this.comboBoxProjectgroup.ItemHeight = 15;
            this.comboBoxProjectgroup.Location = new System.Drawing.Point(69, 66);
            this.comboBoxProjectgroup.Name = "comboBoxProjectgroup";
            this.comboBoxProjectgroup.Size = new System.Drawing.Size(239, 21);
            this.comboBoxProjectgroup.TabIndex = 23;
            // 
            // comboBoxRoleType
            // 
            this.comboBoxRoleType.DisplayMember = "Text";
            this.comboBoxRoleType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxRoleType.FormattingEnabled = true;
            this.comboBoxRoleType.ItemHeight = 15;
            this.comboBoxRoleType.Location = new System.Drawing.Point(69, 37);
            this.comboBoxRoleType.Name = "comboBoxRoleType";
            this.comboBoxRoleType.Size = new System.Drawing.Size(239, 21);
            this.comboBoxRoleType.TabIndex = 22;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(9, 66);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(66, 19);
            this.labelX1.TabIndex = 21;
            this.labelX1.Text = "项 目 组：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(9, 37);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(66, 19);
            this.labelX3.TabIndex = 20;
            this.labelX3.Text = "角色类型：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(243, 162);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddRole
            // 
            this.btnAddRole.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddRole.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddRole.Location = new System.Drawing.Point(169, 162);
            this.btnAddRole.Name = "btnAddRole";
            this.btnAddRole.Size = new System.Drawing.Size(65, 23);
            this.btnAddRole.TabIndex = 9;
            this.btnAddRole.Text = "添加";
            this.btnAddRole.Click += new System.EventHandler(this.btnAddRole_Click);
            // 
            // txtRole
            // 
            // 
            // 
            // 
            this.txtRole.Border.Class = "TextBoxBorder";
            this.txtRole.Location = new System.Drawing.Point(69, 8);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(239, 21);
            this.txtRole.TabIndex = 0;
            this.txtRole.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(9, 9);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(66, 19);
            this.labelX5.TabIndex = 7;
            this.labelX5.Text = "名    称：";
            // 
            // AddGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 192);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddGroup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加角色";
            this.Load += new System.EventHandler(this.AddGroup_Load);
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnAddRole;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRole;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxProjectgroup;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxRoleType;
        private DevComponents.DotNetBar.Controls.TextBoxX txtComment;
        private DevComponents.DotNetBar.LabelX labelX2;

    }
}