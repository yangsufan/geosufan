namespace Fan.Common
{
    partial class FrmExit
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
            this.label1 = new DevExpress.XtraEditors.LabelControl();
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.buttonChange = new DevExpress.XtraEditors.SimpleButton();
            this.buttonExit = new DevExpress.XtraEditors.SimpleButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(61, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "退出系统或切换子系统？";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonChange);
            this.panel1.Controls.Add(this.buttonExit);
            this.panel1.Location = new System.Drawing.Point(-1, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(283, 54);
            // 
            // 
            // 
            this.panel1.TabIndex = 8;
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.Location = new System.Drawing.Point(188, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 28);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonChange
            // 
            this.buttonChange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonChange.Location = new System.Drawing.Point(101, 12);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(80, 28);
            this.buttonChange.TabIndex = 9;
            this.buttonChange.Text = "切换";
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonExit.Location = new System.Drawing.Point(14, 12);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(80, 28);
            this.buttonExit.TabIndex = 8;
            this.buttonExit.Text = "退出";
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // FrmExit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ClientSize = new System.Drawing.Size(278, 121);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmExit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "退出确认";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl label1;
        private DevExpress.XtraEditors.PanelControl panel1;
        private DevExpress.XtraEditors.SimpleButton buttonCancel;
        private DevExpress.XtraEditors.SimpleButton buttonChange;
        private DevExpress.XtraEditors.SimpleButton buttonExit;
    }
}