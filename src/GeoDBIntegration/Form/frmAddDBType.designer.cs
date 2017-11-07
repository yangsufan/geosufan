namespace GeoDBIntegration
{
    partial class frmAddDBType
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
            this.txtDBType = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.comBoxScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.superTooltip = new DevComponents.DotNetBar.SuperTooltip();
            this.SuspendLayout();
            // 
            // txtDBType
            // 
            // 
            // 
            // 
            this.txtDBType.Border.Class = "TextBoxBorder";
            this.txtDBType.Location = new System.Drawing.Point(53, 9);
            this.txtDBType.MaxLength = 30;
            this.txtDBType.Name = "txtDBType";
            this.txtDBType.Size = new System.Drawing.Size(176, 21);
            this.txtDBType.TabIndex = 31;
            this.txtDBType.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
            this.txtDBType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProjectName_KeyPress);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(1, 12);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(56, 18);
            this.labelX4.TabIndex = 32;
            this.labelX4.Text = "名  称 :";
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(163, 39);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(65, 23);
            this.btnCancle.TabIndex = 34;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(88, 39);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(65, 23);
            this.btnOk.TabIndex = 33;
            this.btnOk.Text = "确 定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // comBoxScale
            // 
            this.comBoxScale.DisplayMember = "Text";
            this.comBoxScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comBoxScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxScale.FormattingEnabled = true;
            this.comBoxScale.ItemHeight = 15;
            this.comBoxScale.Location = new System.Drawing.Point(89, 190);
            this.comBoxScale.Name = "comBoxScale";
            this.comBoxScale.Size = new System.Drawing.Size(176, 21);
            this.comBoxScale.TabIndex = 35;
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(36, 193);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(56, 18);
            this.labelX2.TabIndex = 36;
            this.labelX2.Text = "比例尺 :";
            // 
            // frmAddDBType
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 68);
            this.Controls.Add(this.comBoxScale);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtDBType);
            this.Controls.Add(this.labelX4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddDBType";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加数据库类型";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevComponents.DotNetBar.Controls.TextBoxX txtDBType;
        public DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.ButtonX btnCancle;
        public DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comBoxScale;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.SuperTooltip superTooltip;
    }
}