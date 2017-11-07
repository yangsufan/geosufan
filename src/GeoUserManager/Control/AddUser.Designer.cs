namespace GeoUserManager
{
    partial class AddUser
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnGetExtent = new DevComponents.DotNetBar.ButtonX();
            this.cbDepartment = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.checkBoxDate = new System.Windows.Forms.CheckBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.txtTrueName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.comboSex = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtExportArea = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.txtPosition = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtPassword = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnAddUser = new DevComponents.DotNetBar.ButtonX();
            this.txtUser = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.btnGetExtent);
            this.panelEx1.Controls.Add(this.cbDepartment);
            this.panelEx1.Controls.Add(this.labelX10);
            this.panelEx1.Controls.Add(this.checkBoxDate);
            this.panelEx1.Controls.Add(this.dateTimePicker);
            this.panelEx1.Controls.Add(this.txtTrueName);
            this.panelEx1.Controls.Add(this.labelX6);
            this.panelEx1.Controls.Add(this.btnCancel);
            this.panelEx1.Controls.Add(this.comboSex);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Controls.Add(this.txtComment);
            this.panelEx1.Controls.Add(this.labelX9);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.txtExportArea);
            this.panelEx1.Controls.Add(this.labelX7);
            this.panelEx1.Controls.Add(this.txtPosition);
            this.panelEx1.Controls.Add(this.labelX2);
            this.panelEx1.Controls.Add(this.txtPassword);
            this.panelEx1.Controls.Add(this.labelX1);
            this.panelEx1.Controls.Add(this.btnAddUser);
            this.panelEx1.Controls.Add(this.txtUser);
            this.panelEx1.Controls.Add(this.labelX5);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(384, 325);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // btnGetExtent
            // 
            this.btnGetExtent.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetExtent.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetExtent.Location = new System.Drawing.Point(311, 160);
            this.btnGetExtent.Name = "btnGetExtent";
            this.btnGetExtent.Size = new System.Drawing.Size(65, 23);
            this.btnGetExtent.TabIndex = 25;
            this.btnGetExtent.Text = "选择范围";
            this.btnGetExtent.Click += new System.EventHandler(this.btnGetExtent_Click);
            // 
            // cbDepartment
            // 
            this.cbDepartment.DisplayMember = "Text";
            this.cbDepartment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDepartment.FormattingEnabled = true;
            this.cbDepartment.ItemHeight = 15;
            this.cbDepartment.Location = new System.Drawing.Point(69, 135);
            this.cbDepartment.Name = "cbDepartment";
            this.cbDepartment.Size = new System.Drawing.Size(305, 21);
            this.cbDepartment.TabIndex = 24;
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(9, 135);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(68, 20);
            this.labelX10.TabIndex = 23;
            this.labelX10.Text = "科    室：";
            // 
            // checkBoxDate
            // 
            this.checkBoxDate.AutoSize = true;
            this.checkBoxDate.Location = new System.Drawing.Point(350, 195);
            this.checkBoxDate.Name = "checkBoxDate";
            this.checkBoxDate.Size = new System.Drawing.Size(15, 14);
            this.checkBoxDate.TabIndex = 22;
            this.checkBoxDate.UseVisualStyleBackColor = true;
            this.checkBoxDate.CheckedChanged += new System.EventHandler(this.checkBoxDate_CheckedChanged);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Enabled = false;
            this.dateTimePicker.Location = new System.Drawing.Point(69, 189);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(275, 21);
            this.dateTimePicker.TabIndex = 21;
            // 
            // txtTrueName
            // 
            // 
            // 
            // 
            this.txtTrueName.Border.Class = "TextBoxBorder";
            this.txtTrueName.Location = new System.Drawing.Point(69, 33);
            this.txtTrueName.Name = "txtTrueName";
            this.txtTrueName.Size = new System.Drawing.Size(305, 21);
            this.txtTrueName.TabIndex = 1;
            this.txtTrueName.WordWrap = false;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(9, 33);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(68, 20);
            this.labelX6.TabIndex = 20;
            this.labelX6.Text = "姓    名：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(309, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // comboSex
            // 
            this.comboSex.DisplayMember = "Text";
            this.comboSex.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSex.FormattingEnabled = true;
            this.comboSex.ItemHeight = 15;
            this.comboSex.Location = new System.Drawing.Point(69, 83);
            this.comboSex.Name = "comboSex";
            this.comboSex.Size = new System.Drawing.Size(305, 21);
            this.comboSex.TabIndex = 4;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(9, 83);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(68, 20);
            this.labelX4.TabIndex = 16;
            this.labelX4.Text = "性    别：";
            // 
            // txtComment
            // 
            // 
            // 
            // 
            this.txtComment.Border.Class = "TextBoxBorder";
            this.txtComment.Location = new System.Drawing.Point(69, 216);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(305, 70);
            this.txtComment.TabIndex = 8;
            this.txtComment.WordWrap = false;
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(9, 190);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(68, 20);
            this.labelX9.TabIndex = 14;
            this.labelX9.Text = "有效期至：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(9, 220);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 20);
            this.labelX3.TabIndex = 14;
            this.labelX3.Text = "备    注：";
            // 
            // txtExportArea
            // 
            // 
            // 
            // 
            this.txtExportArea.Border.Class = "TextBoxBorder";
            this.txtExportArea.Enabled = false;
            this.txtExportArea.Location = new System.Drawing.Point(69, 164);
            this.txtExportArea.Name = "txtExportArea";
            this.txtExportArea.Size = new System.Drawing.Size(236, 21);
            this.txtExportArea.TabIndex = 6;
            this.txtExportArea.WordWrap = false;
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(9, 164);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(68, 20);
            this.labelX7.TabIndex = 12;
            this.labelX7.Text = "提取范围：";
            // 
            // txtPosition
            // 
            // 
            // 
            // 
            this.txtPosition.Border.Class = "TextBoxBorder";
            this.txtPosition.Location = new System.Drawing.Point(69, 108);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(305, 21);
            this.txtPosition.TabIndex = 6;
            this.txtPosition.WordWrap = false;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(9, 108);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 20);
            this.labelX2.TabIndex = 12;
            this.labelX2.Text = "职    称：";
            // 
            // txtPassword
            // 
            // 
            // 
            // 
            this.txtPassword.Border.Class = "TextBoxBorder";
            this.txtPassword.Location = new System.Drawing.Point(69, 58);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(305, 21);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.WordWrap = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(9, 58);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 20);
            this.labelX1.TabIndex = 10;
            this.labelX1.Text = "密    码：";
            // 
            // btnAddUser
            // 
            this.btnAddUser.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddUser.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddUser.Location = new System.Drawing.Point(240, 292);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(65, 23);
            this.btnAddUser.TabIndex = 17;
            this.btnAddUser.Text = "添加";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // txtUser
            // 
            // 
            // 
            // 
            this.txtUser.Border.Class = "TextBoxBorder";
            this.txtUser.Location = new System.Drawing.Point(69, 8);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(305, 21);
            this.txtUser.TabIndex = 0;
            this.txtUser.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(9, 8);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(68, 20);
            this.labelX5.TabIndex = 7;
            this.labelX5.Text = "用    户：";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // AddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 325);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加用户";
            this.Load += new System.EventHandler(this.AddUser_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX btnAddUser;
        private DevComponents.DotNetBar.Controls.TextBoxX txtComment;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPosition;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPassword;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboSex;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.Controls.TextBoxX txtTrueName;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.TextBoxX txtUser;
        private DevComponents.DotNetBar.LabelX labelX5;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private DevComponents.DotNetBar.Controls.TextBoxX txtExportArea;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX9;
        private System.Windows.Forms.CheckBox checkBoxDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbDepartment;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.ButtonX btnGetExtent;
    }
}