namespace GeoEdit
{
    partial class frmConflictSet
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rbCurVersion = new System.Windows.Forms.RadioButton();
            this.rbDbVersion = new System.Windows.Forms.RadioButton();
            this.chbMerge = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.rbCurVersion);
            this.groupPanel1.Controls.Add(this.rbDbVersion);
            this.groupPanel1.Location = new System.Drawing.Point(12, 12);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(268, 122);
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
            this.groupPanel1.TabIndex = 0;
            this.groupPanel1.Text = "要素冲突版本选择";
            // 
            // rbCurVersion
            // 
            this.rbCurVersion.AutoSize = true;
            this.rbCurVersion.Location = new System.Drawing.Point(29, 51);
            this.rbCurVersion.Name = "rbCurVersion";
            this.rbCurVersion.Size = new System.Drawing.Size(95, 16);
            this.rbCurVersion.TabIndex = 1;
            this.rbCurVersion.TabStop = true;
            this.rbCurVersion.Text = "使用当前版本";
            this.rbCurVersion.UseVisualStyleBackColor = true;
            // 
            // rbDbVersion
            // 
            this.rbDbVersion.AutoSize = true;
            this.rbDbVersion.Location = new System.Drawing.Point(29, 16);
            this.rbDbVersion.Name = "rbDbVersion";
            this.rbDbVersion.Size = new System.Drawing.Size(107, 16);
            this.rbDbVersion.TabIndex = 0;
            this.rbDbVersion.TabStop = true;
            this.rbDbVersion.Text = "使用数据库版本";
            this.rbDbVersion.UseVisualStyleBackColor = true;
            // 
            // chbMerge
            // 
            this.chbMerge.Location = new System.Drawing.Point(13, 150);
            this.chbMerge.Name = "chbMerge";
            this.chbMerge.Size = new System.Drawing.Size(160, 23);
            this.chbMerge.TabIndex = 1;
            this.chbMerge.Text = "是否融合冲突的几何形状";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(205, 184);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmConflictSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 210);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chbMerge);
            this.Controls.Add(this.groupPanel1);
            this.MaximizeBox = false;
            this.Name = "frmConflictSet";
            this.ShowIcon = false;
            this.Text = "冲突处理设置";
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.CheckBoxX chbMerge;
        private System.Windows.Forms.RadioButton rbCurVersion;
        private System.Windows.Forms.RadioButton rbDbVersion;
        private DevComponents.DotNetBar.ButtonX btnOK;
    }
}