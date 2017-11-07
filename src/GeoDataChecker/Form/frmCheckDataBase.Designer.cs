namespace GeoDataChecker
{
    partial class frmCheckDataBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCheckDataBase));
            this.LST_Check = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btn_check = new DevComponents.DotNetBar.ButtonX();
            this.btn_cancle = new DevComponents.DotNetBar.ButtonX();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LST_Check
            // 
            // 
            // 
            // 
            this.LST_Check.Border.Class = "ListViewBorder";
            this.LST_Check.Dock = System.Windows.Forms.DockStyle.Top;
            this.LST_Check.FullRowSelect = true;
            this.LST_Check.GridLines = true;
            this.LST_Check.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.LST_Check.Location = new System.Drawing.Point(0, 0);
            this.LST_Check.MultiSelect = false;
            this.LST_Check.Name = "LST_Check";
            this.LST_Check.Size = new System.Drawing.Size(292, 180);
            this.LST_Check.TabIndex = 0;
            this.LST_Check.UseCompatibleStateImageBehavior = false;
            this.LST_Check.View = System.Windows.Forms.View.List;
            this.LST_Check.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.LST_Check_ItemCheck);
            // 
            // btn_check
            // 
            this.btn_check.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_check.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_check.Location = new System.Drawing.Point(136, 195);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(75, 23);
            this.btn_check.TabIndex = 1;
            this.btn_check.Text = "确 定";
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // btn_cancle
            // 
            this.btn_cancle.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.btn_cancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_cancle.Location = new System.Drawing.Point(215, 195);
            this.btn_cancle.Name = "btn_cancle";
            this.btn_cancle.Size = new System.Drawing.Size(75, 23);
            this.btn_cancle.TabIndex = 2;
            this.btn_cancle.Text = "取 消";
            this.btn_cancle.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(3, 186);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(26, 36);
            this.pictureBox.TabIndex = 7;
            this.pictureBox.TabStop = false;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(35, 200);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(95, 18);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "选择库体检查！";
            // 
            // frmCheckDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 226);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btn_cancle);
            this.Controls.Add(this.btn_check);
            this.Controls.Add(this.LST_Check);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 260);
            this.MinimumSize = new System.Drawing.Size(300, 260);
            this.Name = "frmCheckDataBase";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择库体进行检查";
            this.Load += new System.EventHandler(this.frmCheckDataBase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ListViewEx LST_Check;
        private DevComponents.DotNetBar.ButtonX btn_check;
        private DevComponents.DotNetBar.ButtonX btn_cancle;
        private System.Windows.Forms.PictureBox pictureBox;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}