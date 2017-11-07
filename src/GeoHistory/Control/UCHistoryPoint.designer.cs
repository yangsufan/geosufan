namespace GeoHistory.Control
{
    partial class UCHistoryPoint
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dTimePost = new System.Windows.Forms.DateTimePicker();
            this.btnQuery = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(49, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 81);
            this.label2.TabIndex = 65;
            this.label2.Text = "提示：\r\n1.点击激活显示历史图形的地图窗口\r\n2.选择要查询的日期点；\r\n3.点击查询将结果显示在激活的地图窗口中。";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 64;
            this.label1.Text = "选择日期点：";
            // 
            // dTimePost
            // 
            this.dTimePost.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dTimePost.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTimePost.Location = new System.Drawing.Point(49, 89);
            this.dTimePost.Name = "dTimePost";
            this.dTimePost.Size = new System.Drawing.Size(163, 21);
            this.dTimePost.TabIndex = 63;
            this.dTimePost.Tag = "提交时间";
            // 
            // btnQuery
            // 
            this.btnQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuery.Location = new System.Drawing.Point(50, 129);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(162, 27);
            this.btnQuery.TabIndex = 66;
            this.btnQuery.Text = "查    询";
            //this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // UCHistoryPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dTimePost);
            this.Name = "UCHistoryPoint";
            this.Size = new System.Drawing.Size(260, 313);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dTimePost;
        private DevComponents.DotNetBar.ButtonX btnQuery;
    }
}
