namespace GeoStatistics
{
    partial class frmFields
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
            this.lstFields = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.colText = new System.Windows.Forms.ColumnHeader();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnAllselection = new DevComponents.DotNetBar.ButtonX();
            this.btnReturnselection = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // lstFields
            // 
            this.lstFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.lstFields.Border.Class = "ListViewBorder";
            this.lstFields.CheckBoxes = true;
            this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colText});
            this.lstFields.FullRowSelect = true;
            this.lstFields.Location = new System.Drawing.Point(4, 12);
            this.lstFields.MultiSelect = false;
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(318, 309);
            this.lstFields.TabIndex = 3;
            this.lstFields.UseCompatibleStateImageBehavior = false;
            this.lstFields.View = System.Windows.Forms.View.Details;
            // 
            // colText
            // 
            this.colText.Text = "项目名称";
            this.colText.Width = 236;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(247, 327);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(170, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(71, 25);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAllselection
            // 
            this.btnAllselection.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAllselection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllselection.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAllselection.Location = new System.Drawing.Point(5, 327);
            this.btnAllselection.Name = "btnAllselection";
            this.btnAllselection.Size = new System.Drawing.Size(71, 25);
            this.btnAllselection.TabIndex = 8;
            this.btnAllselection.Text = "全选";
            this.btnAllselection.Click += new System.EventHandler(this.btnAllselection_Click);
            // 
            // btnReturnselection
            // 
            this.btnReturnselection.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReturnselection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReturnselection.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReturnselection.Location = new System.Drawing.Point(82, 327);
            this.btnReturnselection.Name = "btnReturnselection";
            this.btnReturnselection.Size = new System.Drawing.Size(71, 25);
            this.btnReturnselection.TabIndex = 9;
            this.btnReturnselection.Text = "反选";
            this.btnReturnselection.Click += new System.EventHandler(this.btnReturnselection_Click);
            // 
            // frmFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 358);
            this.Controls.Add(this.btnReturnselection);
            this.Controls.Add(this.btnAllselection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstFields);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "frmFields";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "字段选择";
            this.Load += new System.EventHandler(this.frmFields_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx lstFields;
        private System.Windows.Forms.ColumnHeader colText;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnAllselection;
        private DevComponents.DotNetBar.ButtonX btnReturnselection;
    }
}