namespace GeoCustomExport
{
    partial class FrmEditLabel
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
            this.labelSoure = new System.Windows.Forms.Label();
            this.txt_Source = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelTarget = new System.Windows.Forms.Label();
            this.txt_Target = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btn_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.btn_Ok = new DevComponents.DotNetBar.ButtonX();
            this.cb_Source = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // labelSoure
            // 
            this.labelSoure.AutoSize = true;
            this.labelSoure.Location = new System.Drawing.Point(4, 30);
            this.labelSoure.Name = "labelSoure";
            this.labelSoure.Size = new System.Drawing.Size(47, 12);
            this.labelSoure.TabIndex = 0;
            this.labelSoure.Text = "源图层:";
            // 
            // txt_Source
            // 
            // 
            // 
            // 
            this.txt_Source.Border.Class = "TextBoxBorder";
            this.txt_Source.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_Source.Location = new System.Drawing.Point(51, 24);
            this.txt_Source.Name = "txt_Source";
            this.txt_Source.ReadOnly = true;
            this.txt_Source.Size = new System.Drawing.Size(116, 21);
            this.txt_Source.TabIndex = 1;
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Location = new System.Drawing.Point(180, 30);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(59, 12);
            this.labelTarget.TabIndex = 0;
            this.labelTarget.Text = "目标图层:";
            // 
            // txt_Target
            // 
            // 
            // 
            // 
            this.txt_Target.Border.Class = "TextBoxBorder";
            this.txt_Target.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txt_Target.Location = new System.Drawing.Point(240, 24);
            this.txt_Target.Name = "txt_Target";
            this.txt_Target.Size = new System.Drawing.Size(116, 21);
            this.txt_Target.TabIndex = 1;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Cancel.Location = new System.Drawing.Point(299, 62);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(53, 23);
            this.btn_Cancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.btn_Cancel.TabIndex = 13;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btn_Ok.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btn_Ok.Location = new System.Drawing.Point(240, 62);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(53, 23);
            this.btn_Ok.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.btn_Ok.TabIndex = 12;
            this.btn_Ok.Text = "确定";
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // cb_Source
            // 
            this.cb_Source.DisplayMember = "Text";
            this.cb_Source.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_Source.FormattingEnabled = true;
            this.cb_Source.ItemHeight = 15;
            this.cb_Source.Location = new System.Drawing.Point(51, 25);
            this.cb_Source.Name = "cb_Source";
            this.cb_Source.Size = new System.Drawing.Size(116, 21);
            this.cb_Source.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.cb_Source.TabIndex = 14;
            this.cb_Source.SelectedIndexChanged += new System.EventHandler(this.cb_Source_SelectedIndexChanged);
            // 
            // FrmEditLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 89);
            this.Controls.Add(this.cb_Source);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.txt_Target);
            this.Controls.Add(this.labelTarget);
            this.Controls.Add(this.txt_Source);
            this.Controls.Add(this.labelSoure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEditLabel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "修改名称";
            this.Load += new System.EventHandler(this.FrmEditLabel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSoure;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_Source;
        private System.Windows.Forms.Label labelTarget;
        private DevComponents.DotNetBar.Controls.TextBoxX txt_Target;
        private DevComponents.DotNetBar.ButtonX btn_Cancel;
        private DevComponents.DotNetBar.ButtonX btn_Ok;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cb_Source;
    }
}