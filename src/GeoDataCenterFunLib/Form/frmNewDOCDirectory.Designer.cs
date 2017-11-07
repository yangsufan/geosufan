namespace GeoDataCenterFunLib
{
    partial class frmNewDOCDirectory
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
            this.labelDatasource = new System.Windows.Forms.Label();
            this.labelDirectory = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.textBoxDatasouce = new System.Windows.Forms.TextBox();
            this.btnServer = new DevComponents.DotNetBar.ButtonX();
            this.btnDel = new System.Windows.Forms.Button();
            this.ComBoxName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelDatasource
            // 
            this.labelDatasource.AutoSize = true;
            this.labelDatasource.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDatasource.Location = new System.Drawing.Point(33, 87);
            this.labelDatasource.Name = "labelDatasource";
            this.labelDatasource.Size = new System.Drawing.Size(71, 12);
            this.labelDatasource.TabIndex = 0;
            this.labelDatasource.Text = "数据源路径:";
            // 
            // labelDirectory
            // 
            this.labelDirectory.AutoSize = true;
            this.labelDirectory.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDirectory.Location = new System.Drawing.Point(30, 41);
            this.labelDirectory.Name = "labelDirectory";
            this.labelDirectory.Size = new System.Drawing.Size(71, 12);
            this.labelDirectory.TabIndex = 1;
            this.labelDirectory.Text = "虚拟目录名:";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(51, 137);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "创  建";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(254, 137);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "退  出";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // textBoxDatasouce
            // 
            this.textBoxDatasouce.Location = new System.Drawing.Point(110, 78);
            this.textBoxDatasouce.Name = "textBoxDatasouce";
            this.textBoxDatasouce.Size = new System.Drawing.Size(219, 21);
            this.textBoxDatasouce.TabIndex = 7;
            this.textBoxDatasouce.TextChanged += new System.EventHandler(this.textBoxDatasouce_TextChanged);
            // 
            // btnServer
            // 
            this.btnServer.AccessibleRole = System.Windows.Forms.AccessibleRole.Border;
            this.btnServer.Location = new System.Drawing.Point(335, 78);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(33, 21);
            this.btnServer.TabIndex = 33;
            this.btnServer.Text = "...";
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(154, 137);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "删  除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // ComBoxName
            // 
            this.ComBoxName.FormattingEnabled = true;
            this.ComBoxName.Location = new System.Drawing.Point(110, 38);
            this.ComBoxName.Name = "ComBoxName";
            this.ComBoxName.Size = new System.Drawing.Size(219, 20);
            this.ComBoxName.TabIndex = 34;
            this.ComBoxName.SelectedIndexChanged += new System.EventHandler(this.ComBoxName_SelectedIndexChanged);
            // 
            // frmNewDOCDirectory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 168);
            this.Controls.Add(this.ComBoxName);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.textBoxDatasouce);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.labelDirectory);
            this.Controls.Add(this.labelDatasource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewDOCDirectory";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置文档数据源";
            this.Load += new System.EventHandler(this.frmNewDOCDirectory_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDatasource;
        private System.Windows.Forms.Label labelDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.TextBox textBoxDatasouce;
        private DevComponents.DotNetBar.ButtonX btnServer;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ComboBox ComBoxName;
    }
}