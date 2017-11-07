namespace GeoDBIntegration
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddGroup));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbRoleName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbGroupType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnAddRole = new DevComponents.DotNetBar.ButtonX();
            this.txtRole = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.cmbRoleName);
            this.panelEx1.Controls.Add(this.cmbGroupType);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.txtComment);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.btnAddRole);
            this.panelEx1.Controls.Add(this.txtRole);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(302, 233);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(11, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(74, 19);
            this.labelX2.TabIndex = 24;
            this.labelX2.Text = "角色列表：";
            // 
            // cmbRoleName
            // 
            this.cmbRoleName.DisplayMember = "Text";
            this.cmbRoleName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRoleName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoleName.FormattingEnabled = true;
            this.cmbRoleName.ItemHeight = 15;
            this.cmbRoleName.Location = new System.Drawing.Point(83, 12);
            this.cmbRoleName.Name = "cmbRoleName";
            this.cmbRoleName.Size = new System.Drawing.Size(198, 21);
            this.cmbRoleName.TabIndex = 23;
            this.cmbRoleName.SelectedIndexChanged += new System.EventHandler(this.cmbRoleName_SelectedIndexChanged);
            // 
            // cmbGroupType
            // 
            this.cmbGroupType.DisplayMember = "Text";
            this.cmbGroupType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGroupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupType.FormattingEnabled = true;
            this.cmbGroupType.ItemHeight = 15;
            this.cmbGroupType.Location = new System.Drawing.Point(83, 79);
            this.cmbGroupType.Name = "cmbGroupType";
            this.cmbGroupType.Size = new System.Drawing.Size(198, 21);
            this.cmbGroupType.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 79);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 19);
            this.labelX1.TabIndex = 22;
            this.labelX1.Text = "角色类型：";
            // 
            // txtComment
            // 
            // 
            // 
            // 
            this.txtComment.Border.Class = "TextBoxBorder";
            this.txtComment.Location = new System.Drawing.Point(83, 106);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(198, 84);
            this.txtComment.TabIndex = 2;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(11, 106);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 20);
            this.labelX3.TabIndex = 20;
            this.labelX3.Text = "备    注：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(211, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 25);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddRole
            // 
            this.btnAddRole.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddRole.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddRole.Location = new System.Drawing.Point(135, 193);
            this.btnAddRole.Name = "btnAddRole";
            this.btnAddRole.Size = new System.Drawing.Size(70, 25);
            this.btnAddRole.TabIndex = 3;
            this.btnAddRole.Text = "确 定";
            this.btnAddRole.Click += new System.EventHandler(this.btnAddRole_Click);
            // 
            // txtRole
            // 
            // 
            // 
            // 
            this.txtRole.Border.Class = "TextBoxBorder";
            this.txtRole.Location = new System.Drawing.Point(83, 46);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(198, 21);
            this.txtRole.TabIndex = 0;
            this.txtRole.WordWrap = false;
            this.txtRole.TextChanged += new System.EventHandler(this.txtRole_TextChanged);
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(12, 49);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(74, 19);
            this.labelX5.TabIndex = 7;
            this.labelX5.Text = "角色名称：";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // AddGroup
            // 
            this.AcceptButton = this.btnAddRole;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 233);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddGroup";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加角色";
            this.Load += new System.EventHandler(this.AddGroup_Load);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtRole;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.ButtonX btnAddRole;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.TextBoxX txtComment;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbGroupType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRoleName;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}