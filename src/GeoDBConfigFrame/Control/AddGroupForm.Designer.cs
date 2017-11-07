namespace GeoDBConfigFrame
{
    partial class AddGroupForm
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
            this.label = new System.Windows.Forms.Label();
            this.textBoxGroupName = new System.Windows.Forms.TextBox();
            this.butnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.fromgroupBox = new System.Windows.Forms.GroupBox();
            this.fromtreeView = new System.Windows.Forms.TreeView();
            this.togroupBox = new System.Windows.Forms.GroupBox();
            this.totreeView = new System.Windows.Forms.TreeView();
            this.fromgroupBox.SuspendLayout();
            this.togroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label.Location = new System.Drawing.Point(9, 12);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(47, 12);
            this.label.TabIndex = 0;
            this.label.Text = "组名称:";
            // 
            // textBoxGroupName
            // 
            this.textBoxGroupName.Location = new System.Drawing.Point(63, 8);
            this.textBoxGroupName.Name = "textBoxGroupName";
            this.textBoxGroupName.Size = new System.Drawing.Size(231, 21);
            this.textBoxGroupName.TabIndex = 1;
            // 
            // butnCancel
            // 
            this.butnCancel.Location = new System.Drawing.Point(244, 313);
            this.butnCancel.Name = "butnCancel";
            this.butnCancel.Size = new System.Drawing.Size(68, 23);
            this.butnCancel.TabIndex = 5;
            this.butnCancel.Text = "取消";
            this.butnCancel.UseVisualStyleBackColor = true;
            this.butnCancel.Click += new System.EventHandler(this.butnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(160, 313);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(148, 174);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(35, 26);
            this.btnOut.TabIndex = 9;
            this.btnOut.Text = "<<";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(148, 112);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(35, 26);
            this.btnIn.TabIndex = 8;
            this.btnIn.Text = ">>";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // fromgroupBox
            // 
            this.fromgroupBox.Controls.Add(this.fromtreeView);
            this.fromgroupBox.Location = new System.Drawing.Point(1, 32);
            this.fromgroupBox.Name = "fromgroupBox";
            this.fromgroupBox.Size = new System.Drawing.Size(148, 273);
            this.fromgroupBox.TabIndex = 10;
            this.fromgroupBox.TabStop = false;
            this.fromgroupBox.Text = "标准图层";
            // 
            // fromtreeView
            // 
            this.fromtreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fromtreeView.Location = new System.Drawing.Point(3, 17);
            this.fromtreeView.Name = "fromtreeView";
            this.fromtreeView.Size = new System.Drawing.Size(142, 253);
            this.fromtreeView.TabIndex = 7;
            // 
            // togroupBox
            // 
            this.togroupBox.Controls.Add(this.totreeView);
            this.togroupBox.Location = new System.Drawing.Point(183, 35);
            this.togroupBox.Name = "togroupBox";
            this.togroupBox.Size = new System.Drawing.Size(148, 273);
            this.togroupBox.TabIndex = 11;
            this.togroupBox.TabStop = false;
            this.togroupBox.Text = "组内图层";
            // 
            // totreeView
            // 
            this.totreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totreeView.Location = new System.Drawing.Point(3, 17);
            this.totreeView.Name = "totreeView";
            this.totreeView.Size = new System.Drawing.Size(142, 253);
            this.totreeView.TabIndex = 8;
            // 
            // AddGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 342);
            this.Controls.Add(this.togroupBox);
            this.Controls.Add(this.fromgroupBox);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.butnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.textBoxGroupName);
            this.Controls.Add(this.label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddGroupForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "组内容信息";
            this.fromgroupBox.ResumeLayout(false);
            this.togroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        public System.Windows.Forms.TextBox textBoxGroupName;
        private System.Windows.Forms.Button butnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.GroupBox fromgroupBox;
        private System.Windows.Forms.TreeView fromtreeView;
        private System.Windows.Forms.GroupBox togroupBox;
        public System.Windows.Forms.TreeView totreeView;
    }
}