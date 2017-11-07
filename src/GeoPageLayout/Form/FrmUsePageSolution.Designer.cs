namespace GeoPageLayout
{
    partial class FrmUsePageSolution
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApply = new DevComponents.DotNetBar.ButtonX();
            this.btnQuit = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cbPageName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.txtRemark = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnApply.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnApply.Location = new System.Drawing.Point(349, 193);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 33);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "应用";
            // 
            // btnQuit
            // 
            this.btnQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuit.Location = new System.Drawing.Point(445, 193);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(75, 33);
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "退出";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(114, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "方案名称：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(12, 72);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "备注信息：";
            // 
            // cbPageName
            // 
            this.cbPageName.DisplayMember = "Text";
            this.cbPageName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbPageName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPageName.FormattingEnabled = true;
            this.cbPageName.ItemHeight = 19;
            this.cbPageName.Location = new System.Drawing.Point(12, 41);
            this.cbPageName.Name = "cbPageName";
            this.cbPageName.Size = new System.Drawing.Size(508, 25);
            this.cbPageName.TabIndex = 4;
            this.cbPageName.SelectedIndexChanged += new System.EventHandler(this.cbPageName_SelectedIndexChanged);
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(12, 101);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(508, 86);
            this.txtRemark.TabIndex = 5;
            this.txtRemark.Text = "";
            // 
            // FrmUsePageSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 238);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.cbPageName);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnApply);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUsePageSolution";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "应用制图方案";
            this.Load += new System.EventHandler(this.FrmUsePageSolution_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnApply;
        private DevComponents.DotNetBar.ButtonX btnQuit;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbPageName;
        private System.Windows.Forms.RichTextBox txtRemark;

    }
}