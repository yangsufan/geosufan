namespace GeoProperties.UserControls
{
    partial class FrmSaveSQLSolution
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtSolutionName = new System.Windows.Forms.TextBox();
            this.RtxtCondition = new System.Windows.Forms.RichTextBox();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.RichTxtDescription = new System.Windows.Forms.RichTextBox();
            this.CheckShared = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(13, 13);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(101, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "方案名称:";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(13, 61);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(89, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "SQL查询语句：";
            // 
            // txtSolutionName
            // 
            this.txtSolutionName.Location = new System.Drawing.Point(13, 34);
            this.txtSolutionName.Name = "txtSolutionName";
            this.txtSolutionName.Size = new System.Drawing.Size(330, 21);
            this.txtSolutionName.TabIndex = 3;
            // 
            // RtxtCondition
            // 
            this.RtxtCondition.Enabled = false;
            this.RtxtCondition.Location = new System.Drawing.Point(13, 91);
            this.RtxtCondition.Name = "RtxtCondition";
            this.RtxtCondition.Size = new System.Drawing.Size(330, 66);
            this.RtxtCondition.TabIndex = 5;
            this.RtxtCondition.Text = "";
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(268, 323);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 23);
            this.btnCancle.TabIndex = 6;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(174, 323);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(13, 176);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "功能描述：";
            // 
            // RichTxtDescription
            // 
            this.RichTxtDescription.Location = new System.Drawing.Point(13, 206);
            this.RichTxtDescription.Name = "RichTxtDescription";
            this.RichTxtDescription.Size = new System.Drawing.Size(330, 75);
            this.RichTxtDescription.TabIndex = 9;
            this.RichTxtDescription.Text = "";
            // 
            // CheckShared
            // 
            this.CheckShared.Checked = true;
            this.CheckShared.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckShared.CheckValue = "Y";
            this.CheckShared.Location = new System.Drawing.Point(13, 299);
            this.CheckShared.Name = "CheckShared";
            this.CheckShared.Size = new System.Drawing.Size(101, 23);
            this.CheckShared.TabIndex = 10;
            this.CheckShared.Text = "共享方案";
            // 
            // FrmSaveSQLSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 358);
            this.Controls.Add(this.CheckShared);
            this.Controls.Add(this.RichTxtDescription);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.RtxtCondition);
            this.Controls.Add(this.txtSolutionName);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSaveSQLSolution";
            this.ShowIcon = false;
            this.Text = "保存查询解决方案";
            this.Load += new System.EventHandler(this.FrmSaveSQLSolution_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.TextBox txtSolutionName;
        private System.Windows.Forms.RichTextBox RtxtCondition;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.LabelX labelX3;
        private System.Windows.Forms.RichTextBox RichTxtDescription;
        private DevComponents.DotNetBar.Controls.CheckBoxX CheckShared;
    }
}