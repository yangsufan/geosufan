namespace GeoDataManagerFrame
{
    partial class FrmImpLandUseReport
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
            this.buttonXOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonXQuit = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbAreaUnit = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.chkZTGH = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkTDLY = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtFractionNum = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.buttonBrowse = new DevComponents.DotNetBar.ButtonX();
            this.textBoxResultPath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOK.Location = new System.Drawing.Point(38, 156);
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
            this.buttonXQuit.Location = new System.Drawing.Point(201, 156);
            this.buttonXQuit.Name = "buttonXQuit";
            this.buttonXQuit.Size = new System.Drawing.Size(70, 23);
            this.buttonXQuit.TabIndex = 7;
            this.buttonXQuit.Text = "退出";
            this.buttonXQuit.Click += new System.EventHandler(this.buttonXQuit_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(18, 43);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(70, 16);
            this.labelX2.TabIndex = 0;
            this.labelX2.Text = "统计年度：";
            // 
            // cmbYear
            // 
            this.cmbYear.DisplayMember = "Text";
            this.cmbYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.ItemHeight = 15;
            this.cmbYear.Location = new System.Drawing.Point(90, 40);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(197, 21);
            this.cmbYear.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(18, 69);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(82, 19);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "面积单位：";
            // 
            // cmbAreaUnit
            // 
            this.cmbAreaUnit.DisplayMember = "Text";
            this.cmbAreaUnit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAreaUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAreaUnit.FormattingEnabled = true;
            this.cmbAreaUnit.ItemHeight = 15;
            this.cmbAreaUnit.Location = new System.Drawing.Point(90, 67);
            this.cmbAreaUnit.Name = "cmbAreaUnit";
            this.cmbAreaUnit.Size = new System.Drawing.Size(197, 21);
            this.cmbAreaUnit.TabIndex = 1;
            // 
            // chkZTGH
            // 
            this.chkZTGH.Location = new System.Drawing.Point(144, 12);
            this.chkZTGH.Name = "chkZTGH";
            this.chkZTGH.Size = new System.Drawing.Size(150, 20);
            this.chkZTGH.TabIndex = 13;
            this.chkZTGH.Text = "森林资源总体规划数据";
            // 
            // chkTDLY
            // 
            this.chkTDLY.Location = new System.Drawing.Point(17, 12);
            this.chkTDLY.Name = "chkTDLY";
            this.chkTDLY.Size = new System.Drawing.Size(124, 21);
            this.chkTDLY.TabIndex = 12;
            this.chkTDLY.Text = "森林资源现状数据";
            this.chkTDLY.CheckValueChanged += new System.EventHandler(this.chkTDLY_CheckValueChanged);
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(18, 95);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(82, 19);
            this.labelX3.TabIndex = 0;
            this.labelX3.Text = "小 数 位：";
            // 
            // txtFractionNum
            // 
            // 
            // 
            // 
            this.txtFractionNum.Border.Class = "TextBoxBorder";
            this.txtFractionNum.Location = new System.Drawing.Point(90, 94);
            this.txtFractionNum.Name = "txtFractionNum";
            this.txtFractionNum.Size = new System.Drawing.Size(197, 21);
            this.txtFractionNum.TabIndex = 14;
            this.txtFractionNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFractionNum_KeyPress);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonBrowse.Location = new System.Drawing.Point(286, 121);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(24, 20);
            this.buttonBrowse.TabIndex = 17;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxResultPath
            // 
            // 
            // 
            // 
            this.textBoxResultPath.Border.Class = "TextBoxBorder";
            this.textBoxResultPath.Location = new System.Drawing.Point(90, 121);
            this.textBoxResultPath.Name = "textBoxResultPath";
            this.textBoxResultPath.Size = new System.Drawing.Size(197, 21);
            this.textBoxResultPath.TabIndex = 16;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(18, 116);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(68, 30);
            this.labelX5.TabIndex = 15;
            this.labelX5.Text = "输出目录：";
            // 
            // FrmImpLandUseReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 185);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxResultPath);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.txtFractionNum);
            this.Controls.Add(this.chkZTGH);
            this.Controls.Add(this.chkTDLY);
            this.Controls.Add(this.buttonXQuit);
            this.Controls.Add(this.buttonXOK);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cmbAreaUnit);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.labelX2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmImpLandUseReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "统计设置";
            this.Load += new System.EventHandler(this.FrmImpLandUseReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonXOK;
        private DevComponents.DotNetBar.ButtonX buttonXQuit;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYear;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbAreaUnit;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkZTGH;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkTDLY;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFractionNum;
        private DevComponents.DotNetBar.ButtonX buttonBrowse;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxResultPath;
        private DevComponents.DotNetBar.LabelX labelX5;
    }
}