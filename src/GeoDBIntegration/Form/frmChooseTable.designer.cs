namespace GeoDBIntegration
{
    partial class frmChooseTable
    {
        /// <summary>
        /// 必需的设计器变量。        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。        /// </summary>
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
        /// 使用代码编辑器修改此方法的内容。        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.list_Table = new System.Windows.Forms.CheckedListBox();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btn_SelectAll = new DevComponents.DotNetBar.ButtonX();
            this.btn_SelectRev = new DevComponents.DotNetBar.ButtonX();
            this.btn_SelectClear = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(256, 502);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 25);
            this.btnOk.TabIndex = 39;
            this.btnOk.Text = "确 定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(332, 502);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(70, 25);
            this.btnCancle.TabIndex = 40;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // list_Table
            // 
            this.list_Table.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.list_Table.FormattingEnabled = true;
            this.list_Table.Location = new System.Drawing.Point(31, 28);
            this.list_Table.Name = "list_Table";
            this.list_Table.Size = new System.Drawing.Size(371, 468);
            this.list_Table.TabIndex = 41;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(5, 4);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(133, 18);
            this.labelX2.TabIndex = 42;
            this.labelX2.Text = "请选择需要创建的表：";
            // 
            // btn_SelectAll
            // 
            this.btn_SelectAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelectAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelectAll.Location = new System.Drawing.Point(4, 31);
            this.btn_SelectAll.Name = "btn_SelectAll";
            this.btn_SelectAll.Size = new System.Drawing.Size(21, 84);
            this.btn_SelectAll.TabIndex = 43;
            this.btn_SelectAll.Text = "全选";
            this.btn_SelectAll.Click += new System.EventHandler(this.btn_SelectAll_Click);
            // 
            // btn_SelectRev
            // 
            this.btn_SelectRev.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelectRev.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelectRev.Location = new System.Drawing.Point(4, 121);
            this.btn_SelectRev.Name = "btn_SelectRev";
            this.btn_SelectRev.Size = new System.Drawing.Size(21, 84);
            this.btn_SelectRev.TabIndex = 44;
            this.btn_SelectRev.Text = "反选";
            this.btn_SelectRev.Click += new System.EventHandler(this.btn_SelectRev_Click);
            // 
            // btn_SelectClear
            // 
            this.btn_SelectClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_SelectClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_SelectClear.Location = new System.Drawing.Point(5, 211);
            this.btn_SelectClear.Name = "btn_SelectClear";
            this.btn_SelectClear.Size = new System.Drawing.Size(21, 84);
            this.btn_SelectClear.TabIndex = 45;
            this.btn_SelectClear.Text = "清空";
            this.btn_SelectClear.Click += new System.EventHandler(this.btn_SelectClear_Click);
            // 
            // frmChooseTable
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 533);
            this.Controls.Add(this.btn_SelectClear);
            this.Controls.Add(this.btn_SelectRev);
            this.Controls.Add(this.btn_SelectAll);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.list_Table);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancle);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChooseTable";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建库体";
            this.ResumeLayout(false);

        }

        #endregion

        public DevComponents.DotNetBar.ButtonX btnOk;
        public DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.SuperTooltip superTooltip;
        private System.Windows.Forms.CheckedListBox list_Table;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btn_SelectAll;
        private DevComponents.DotNetBar.ButtonX btn_SelectRev;
        private DevComponents.DotNetBar.ButtonX btn_SelectClear;
    }
}