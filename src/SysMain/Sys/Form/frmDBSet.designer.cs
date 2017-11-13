namespace GeoDatabaseManager
{
    partial class frmDBSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDBSet));
            this.panelEx = new DevExpress.XtraEditors.PanelControl();
            this.picImg = new System.Windows.Forms.PictureBox();
            this.labelX2 = new DevExpress.XtraEditors.LabelControl();
            this.labelX1 = new DevExpress.XtraEditors.LabelControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider();
            this.panelEx1 = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelX3 = new DevExpress.XtraEditors.LabelControl();
            this.labelX4 = new DevExpress.XtraEditors.LabelControl();
            this.buttonXCancel = new DevExpress.XtraEditors.SimpleButton();
            this.buttonXOK = new DevExpress.XtraEditors.SimpleButton();
            this.ConSet = new SysCommon.Gis.UIDataConnect();
            this.groupPanel1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx)).BeginInit();
            this.panelEx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).BeginInit();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx
            // 
            this.panelEx.Controls.Add(this.picImg);
            this.panelEx.Controls.Add(this.labelX2);
            this.panelEx.Controls.Add(this.labelX1);
            this.panelEx.Location = new System.Drawing.Point(5, 1);
            this.panelEx.Name = "panelEx";
            this.panelEx.Size = new System.Drawing.Size(532, 72);
            this.panelEx.TabIndex = 64;
            // 
            // picImg
            // 
            this.picImg.Image = ((System.Drawing.Image)(resources.GetObject("picImg.Image")));
            this.picImg.Location = new System.Drawing.Point(444, 11);
            this.picImg.Name = "picImg";
            this.picImg.Size = new System.Drawing.Size(50, 49);
            this.picImg.TabIndex = 3;
            this.picImg.TabStop = false;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(48, 35);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(216, 14);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "主要是对系统运维配置及临时库进行设置";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(20, 11);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(156, 14);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "设置系统所必备的数据库连接";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // panelEx1
            // 
            this.panelEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx1.Controls.Add(this.pictureBox1);
            this.panelEx1.Controls.Add(this.labelX3);
            this.panelEx1.Controls.Add(this.labelX4);
            this.panelEx1.Location = new System.Drawing.Point(2, 5);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(566, 96);
            this.panelEx1.TabIndex = 65;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(490, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 57);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(12, 38);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(216, 14);
            this.labelX3.TabIndex = 1;
            this.labelX3.Text = "主要是对系统运维配置及临时库进行设置";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(12, 10);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(156, 14);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "设置系统所必备的数据库连接";
            // 
            // buttonXCancel
            // 
            this.buttonXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCancel.Location = new System.Drawing.Point(463, 383);
            this.buttonXCancel.Name = "buttonXCancel";
            this.buttonXCancel.Size = new System.Drawing.Size(87, 27);
            this.buttonXCancel.TabIndex = 68;
            this.buttonXCancel.Text = "取 消";
            this.buttonXCancel.Click += new System.EventHandler(this.buttonXCancel_Click);
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.Location = new System.Drawing.Point(369, 383);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(87, 27);
            this.buttonXOK.TabIndex = 67;
            this.buttonXOK.Text = "确 定";
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // ConSet
            // 
            this.ConSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConSet.DataBase = "";
            this.ConSet.DatabaseType = "ORACLE";
            this.ConSet.Location = new System.Drawing.Point(0, 1);
            this.ConSet.Name = "ConSet";
            this.ConSet.Password = "";
            this.ConSet.Server = "";
            this.ConSet.Service = "5151";
            this.ConSet.Size = new System.Drawing.Size(559, 324);
            this.ConSet.strTitle = "数据连接";
            this.ConSet.TabIndex = 0;
            this.ConSet.User = "";
            this.ConSet.Version = "SDE.DEFAULT";
            // 
            // groupPanel1
            // 
            this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel1.Controls.Add(this.ConSet);
            this.groupPanel1.Location = new System.Drawing.Point(2, 80);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(566, 294);
            this.groupPanel1.TabIndex = 66;
            // 
            // frmDBSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 413);
            this.Controls.Add(this.buttonXCancel);
            this.Controls.Add(this.buttonXOK);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDBSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库连接信息";
            ((System.ComponentModel.ISupportInitialize)(this.panelEx)).EndInit();
            this.panelEx.ResumeLayout(false);
            this.panelEx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEx1)).EndInit();
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupPanel1)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelEx;
        private System.Windows.Forms.PictureBox picImg;
        private DevExpress.XtraEditors.LabelControl labelX2;
        private DevExpress.XtraEditors.LabelControl labelX1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private DevExpress.XtraEditors.PanelControl panelEx1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DevExpress.XtraEditors.LabelControl labelX3;
        private DevExpress.XtraEditors.LabelControl labelX4;
        private DevExpress.XtraEditors.SimpleButton buttonXCancel;
        private DevExpress.XtraEditors.SimpleButton buttonXOK;
        private DevExpress.XtraEditors.GroupControl groupPanel1;
        private SysCommon.Gis.UIDataConnect ConSet;
    }
}