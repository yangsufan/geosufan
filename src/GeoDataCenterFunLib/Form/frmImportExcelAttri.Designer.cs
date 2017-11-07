namespace GeoDataCenterFunLib
{
    partial class frmImportExcelAttri
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtBoxLayer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxExcelPath = new System.Windows.Forms.TextBox();
            this.btnOpenExcel = new System.Windows.Forms.Button();
            this.ListFields = new System.Windows.Forms.CheckedListBox();
            this.chkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbKeyField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(200, 448);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 23);
            this.btnOK.TabIndex = 50;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(264, 448);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "关闭";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtBoxLayer
            // 
            this.txtBoxLayer.Location = new System.Drawing.Point(72, 12);
            this.txtBoxLayer.Name = "txtBoxLayer";
            this.txtBoxLayer.ReadOnly = true;
            this.txtBoxLayer.Size = new System.Drawing.Size(252, 21);
            this.txtBoxLayer.TabIndex = 52;
            this.txtBoxLayer.Text = "点击选择 目标图层";
            this.txtBoxLayer.Click += new System.EventHandler(this.txtBoxLayer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 51;
            this.label2.Text = "图层:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 51;
            this.label1.Text = "外业表:";
            // 
            // txtBoxExcelPath
            // 
            this.txtBoxExcelPath.Location = new System.Drawing.Point(72, 39);
            this.txtBoxExcelPath.Name = "txtBoxExcelPath";
            this.txtBoxExcelPath.ReadOnly = true;
            this.txtBoxExcelPath.Size = new System.Drawing.Size(211, 21);
            this.txtBoxExcelPath.TabIndex = 52;
            // 
            // btnOpenExcel
            // 
            this.btnOpenExcel.Location = new System.Drawing.Point(289, 39);
            this.btnOpenExcel.Name = "btnOpenExcel";
            this.btnOpenExcel.Size = new System.Drawing.Size(35, 23);
            this.btnOpenExcel.TabIndex = 50;
            this.btnOpenExcel.Text = "...";
            this.btnOpenExcel.UseVisualStyleBackColor = true;
            this.btnOpenExcel.Click += new System.EventHandler(this.btnOpenExcel_Click);
            // 
            // ListFields
            // 
            this.ListFields.FormattingEnabled = true;
            this.ListFields.Location = new System.Drawing.Point(8, 116);
            this.ListFields.Name = "ListFields";
            this.ListFields.Size = new System.Drawing.Size(316, 292);
            this.ListFields.TabIndex = 53;
            // 
            // chkBoxSelectAll
            // 
            this.chkBoxSelectAll.AutoSize = true;
            this.chkBoxSelectAll.Location = new System.Drawing.Point(14, 450);
            this.chkBoxSelectAll.Name = "chkBoxSelectAll";
            this.chkBoxSelectAll.Size = new System.Drawing.Size(48, 16);
            this.chkBoxSelectAll.TabIndex = 54;
            this.chkBoxSelectAll.Text = "全选";
            this.chkBoxSelectAll.UseVisualStyleBackColor = true;
            this.chkBoxSelectAll.CheckedChanged += new System.EventHandler(this.chkBoxSelectAll_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 51;
            this.label3.Text = "关键字段:";
            // 
            // cmbKeyField
            // 
            this.cmbKeyField.DisplayMember = "Text";
            this.cmbKeyField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbKeyField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyField.FormattingEnabled = true;
            this.cmbKeyField.ItemHeight = 15;
            this.cmbKeyField.Location = new System.Drawing.Point(72, 67);
            this.cmbKeyField.Name = "cmbKeyField";
            this.cmbKeyField.Size = new System.Drawing.Size(252, 21);
            this.cmbKeyField.TabIndex = 64;
            // 
            // progressBarX1
            // 
            this.progressBarX1.Location = new System.Drawing.Point(7, 413);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(319, 10);
            this.progressBarX1.TabIndex = 65;
            this.progressBarX1.Text = "proBar";
            this.progressBarX1.Visible = false;
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(11, 424);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(268, 22);
            this.lblTips.TabIndex = 66;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(329, 12);
            this.label4.TabIndex = 51;
            this.label4.Text = "请确保外业调查表第一行为字段名(字母)，第二行开始为数据";
            // 
            // frmImportExcelAttri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 476);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.cmbKeyField);
            this.Controls.Add(this.chkBoxSelectAll);
            this.Controls.Add(this.ListFields);
            this.Controls.Add(this.txtBoxExcelPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxLayer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOpenExcel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportExcelAttri";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导入外业调查表到图层属性中";
            this.Load += new System.EventHandler(this.frmImportExcelAttri_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtBoxLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxExcelPath;
        private System.Windows.Forms.Button btnOpenExcel;
        private System.Windows.Forms.CheckedListBox ListFields;
        private System.Windows.Forms.CheckBox chkBoxSelectAll;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbKeyField;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.LabelX lblTips;
        private System.Windows.Forms.Label label4;
    }
}