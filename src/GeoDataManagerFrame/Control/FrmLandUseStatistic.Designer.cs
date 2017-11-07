namespace GeoDataManagerFrame
{
    partial class FrmLandUseStatistic
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbXZQ = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.buttonXOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonXQuit = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmbAreaUnit = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtFractionNum = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.textBoxResultPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonBrowse = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(13, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 30);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "行 政 区：";
            // 
            // cmbXZQ
            // 
            this.cmbXZQ.DisplayMember = "Text";
            this.cmbXZQ.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbXZQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXZQ.FormattingEnabled = true;
            this.cmbXZQ.ItemHeight = 15;
            this.cmbXZQ.Location = new System.Drawing.Point(73, 8);
            this.cmbXZQ.Name = "cmbXZQ";
            this.cmbXZQ.Size = new System.Drawing.Size(206, 21);
            this.cmbXZQ.TabIndex = 1;
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOK.Location = new System.Drawing.Point(68, 152);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(70, 23);
            this.buttonXOK.TabIndex = 6;
            this.buttonXOK.Text = "统计";
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // buttonXQuit
            // 
            this.buttonXQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXQuit.Location = new System.Drawing.Point(164, 152);
            this.buttonXQuit.Name = "buttonXQuit";
            this.buttonXQuit.Size = new System.Drawing.Size(70, 23);
            this.buttonXQuit.TabIndex = 7;
            this.buttonXQuit.Text = "退出";
            this.buttonXQuit.Click += new System.EventHandler(this.buttonXQuit_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(13, 31);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 30);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "年    度：";
            // 
            // cmbYear
            // 
            this.cmbYear.DisplayMember = "Text";
            this.cmbYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.ItemHeight = 15;
            this.cmbYear.Location = new System.Drawing.Point(73, 36);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(206, 21);
            this.cmbYear.TabIndex = 1;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(13, 59);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(68, 30);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "单    位：";
            // 
            // cmbAreaUnit
            // 
            this.cmbAreaUnit.DisplayMember = "Text";
            this.cmbAreaUnit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAreaUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAreaUnit.FormattingEnabled = true;
            this.cmbAreaUnit.ItemHeight = 15;
            this.cmbAreaUnit.Location = new System.Drawing.Point(73, 64);
            this.cmbAreaUnit.Name = "cmbAreaUnit";
            this.cmbAreaUnit.Size = new System.Drawing.Size(206, 21);
            this.cmbAreaUnit.TabIndex = 1;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(13, 87);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(68, 30);
            this.labelX4.TabIndex = 0;
            this.labelX4.Text = "小 数 位：";
            // 
            // txtFractionNum
            // 
            // 
            // 
            // 
            this.txtFractionNum.Border.Class = "TextBoxBorder";
            this.txtFractionNum.Location = new System.Drawing.Point(73, 92);
            this.txtFractionNum.Name = "txtFractionNum";
            this.txtFractionNum.Size = new System.Drawing.Size(206, 21);
            this.txtFractionNum.TabIndex = 8;
            this.txtFractionNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFractionNum_KeyPress);
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(13, 116);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(68, 30);
            this.labelX5.TabIndex = 0;
            this.labelX5.Text = "输出目录：";
            // 
            // textBoxResultPath
            // 
            // 
            // 
            // 
            this.textBoxResultPath.Border.Class = "TextBoxBorder";
            this.textBoxResultPath.Location = new System.Drawing.Point(73, 121);
            this.textBoxResultPath.Name = "textBoxResultPath";
            this.textBoxResultPath.Size = new System.Drawing.Size(206, 21);
            this.textBoxResultPath.TabIndex = 8;
            this.textBoxResultPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFractionNum_KeyPress);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonBrowse.Location = new System.Drawing.Point(280, 122);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowse.TabIndex = 9;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // FrmLandUseStatistic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 185);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxResultPath);
            this.Controls.Add(this.txtFractionNum);
            this.Controls.Add(this.buttonXQuit);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.buttonXOK);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.cmbAreaUnit);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.cmbXZQ);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLandUseStatistic";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "统计设置";
            this.Load += new System.EventHandler(this.FrmImpLandUseReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbXZQ;
        private DevComponents.DotNetBar.ButtonX buttonXOK;
        private DevComponents.DotNetBar.ButtonX buttonXQuit;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYear;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbAreaUnit;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFractionNum;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxResultPath;
        private DevComponents.DotNetBar.ButtonX buttonBrowse;
    }
}