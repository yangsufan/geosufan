namespace GeoDBConfigFrame
{
    partial class SubAttForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textSubCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textSubName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textIndexFile = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMapIndexFile = new System.Windows.Forms.TextBox();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "专题类型:";
            // 
            // textSubCode
            // 
            this.textSubCode.Location = new System.Drawing.Point(106, 20);
            this.textSubCode.Name = "textSubCode";
            this.textSubCode.Size = new System.Drawing.Size(160, 21);
            this.textSubCode.TabIndex = 1;
            this.textSubCode.TextChanged += new System.EventHandler(this.textSubCode_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "专题描述:";
            // 
            // textSubName
            // 
            this.textSubName.Location = new System.Drawing.Point(106, 55);
            this.textSubName.Name = "textSubName";
            this.textSubName.Size = new System.Drawing.Size(160, 21);
            this.textSubName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "脚本文件:";
            // 
            // textIndexFile
            // 
            this.textIndexFile.Location = new System.Drawing.Point(106, 89);
            this.textIndexFile.Name = "textIndexFile";
            this.textIndexFile.Size = new System.Drawing.Size(160, 21);
            this.textIndexFile.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(106, 148);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "配图方案文件:";
            // 
            // textBoxMapIndexFile
            // 
            this.textBoxMapIndexFile.Location = new System.Drawing.Point(106, 121);
            this.textBoxMapIndexFile.Name = "textBoxMapIndexFile";
            this.textBoxMapIndexFile.Size = new System.Drawing.Size(160, 21);
            this.textBoxMapIndexFile.TabIndex = 1;
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(267, 122);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(33, 21);
            this.btnServer.TabIndex = 48;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // SubAttForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 170);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.textBoxMapIndexFile);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textIndexFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textSubName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textSubCode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SubAttForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "专题属性";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textSubCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textSubName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textIndexFile;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMapIndexFile;
        private DevComponents.DotNetBar.ButtonX btnServer;
    }
}