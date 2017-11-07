namespace GeoDBATool
{
    partial class FrmImportData
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
            this.listViewEx = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnOrg = new DevComponents.DotNetBar.ButtonX();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxOrgType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewEx
            // 
            // 
            // 
            // 
            this.listViewEx.Border.Class = "ListViewBorder";
            this.listViewEx.CheckBoxes = true;
            this.listViewEx.Location = new System.Drawing.Point(5, 30);
            this.listViewEx.Name = "listViewEx";
            this.listViewEx.ShowItemToolTips = true;
            this.listViewEx.Size = new System.Drawing.Size(324, 113);
            this.listViewEx.TabIndex = 4;
            this.listViewEx.UseCompatibleStateImageBehavior = false;
            this.listViewEx.View = System.Windows.Forms.View.List;
            // 
            // btnOrg
            // 
            this.btnOrg.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOrg.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOrg.Location = new System.Drawing.Point(335, 3);
            this.btnOrg.Name = "btnOrg";
            this.btnOrg.Size = new System.Drawing.Size(63, 21);
            this.btnOrg.TabIndex = 0;
            this.btnOrg.Text = "浏 览";
            this.btnOrg.Click += new System.EventHandler(this.btnOrg_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(335, 30);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(63, 21);
            this.btnSelAll.TabIndex = 1;
            this.btnSelAll.Text = "全 选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnSelReverse
            // 
            this.btnSelReverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelReverse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelReverse.Location = new System.Drawing.Point(335, 57);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(63, 21);
            this.btnSelReverse.TabIndex = 2;
            this.btnSelReverse.Text = "反 选";
            this.btnSelReverse.Click += new System.EventHandler(this.btnSelReverse_Click);
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Location = new System.Drawing.Point(335, 84);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(63, 21);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "清 除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.Location = new System.Drawing.Point(2, 6);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(74, 18);
            this.labelX14.TabIndex = 11;
            this.labelX14.Text = " 类    型 :";
            // 
            // comboBoxOrgType
            // 
            this.comboBoxOrgType.DisplayMember = "Text";
            this.comboBoxOrgType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxOrgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOrgType.FormattingEnabled = true;
            this.comboBoxOrgType.ItemHeight = 15;
            this.comboBoxOrgType.Location = new System.Drawing.Point(80, 3);
            this.comboBoxOrgType.Name = "comboBoxOrgType";
            this.comboBoxOrgType.Size = new System.Drawing.Size(249, 21);
            this.comboBoxOrgType.TabIndex = 10;
            this.comboBoxOrgType.SelectedIndexChanged += new System.EventHandler(this.comboBoxOrgType_SelectedIndexChanged);
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.comboBoxOrgType);
            this.groupPanel2.Controls.Add(this.labelX14);
            this.groupPanel2.Controls.Add(this.btnDel);
            this.groupPanel2.Controls.Add(this.btnSelReverse);
            this.groupPanel2.Controls.Add(this.btnSelAll);
            this.groupPanel2.Controls.Add(this.btnOrg);
            this.groupPanel2.Controls.Add(this.listViewEx);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(6, 12);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(407, 172);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
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
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 10;
            this.groupPanel2.Text = "源数据选择";
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(266, 190);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Text = "确 定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(342, 190);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(70, 25);
            this.btnCancle.TabIndex = 13;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // FrmImportData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 221);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImportData";
            this.ShowIcon = false;
            this.Text = "数据入库";
            this.Load += new System.EventHandler(this.FrmImportData_Load);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx;
        private DevComponents.DotNetBar.ButtonX btnOrg;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxOrgType;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.ButtonX btnCancle;
    }
}