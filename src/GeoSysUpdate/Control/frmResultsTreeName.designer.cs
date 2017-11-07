namespace GeoSysUpdate
{
    partial class frmResultsTreeName
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.lab = new System.Windows.Forms.Label();
            this.bttOk = new System.Windows.Forms.Button();
            this.btnCanlce = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(82, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(198, 21);
            this.txtName.TabIndex = 0;
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // lab
            // 
            this.lab.AutoSize = true;
            this.lab.Location = new System.Drawing.Point(12, 15);
            this.lab.Name = "lab";
            this.lab.Size = new System.Drawing.Size(65, 12);
            this.lab.TabIndex = 1;
            this.lab.Text = "创建名称：";
            // 
            // bttOk
            // 
            this.bttOk.Location = new System.Drawing.Point(153, 43);
            this.bttOk.Name = "bttOk";
            this.bttOk.Size = new System.Drawing.Size(57, 23);
            this.bttOk.TabIndex = 2;
            this.bttOk.Text = "确 定";
            this.bttOk.UseVisualStyleBackColor = true;
            this.bttOk.Click += new System.EventHandler(this.bttOk_Click);
            // 
            // btnCanlce
            // 
            this.btnCanlce.Location = new System.Drawing.Point(216, 43);
            this.btnCanlce.Name = "btnCanlce";
            this.btnCanlce.Size = new System.Drawing.Size(57, 23);
            this.btnCanlce.TabIndex = 3;
            this.btnCanlce.Text = "取 消";
            this.btnCanlce.UseVisualStyleBackColor = true;
            this.btnCanlce.Click += new System.EventHandler(this.btnCanlce_Click);
            // 
            // frmResultsTreeName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 73);
            this.Controls.Add(this.btnCanlce);
            this.Controls.Add(this.bttOk);
            this.Controls.Add(this.lab);
            this.Controls.Add(this.txtName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmResultsTreeName";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建目录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lab;
        private System.Windows.Forms.Button bttOk;
        private System.Windows.Forms.Button btnCanlce;
    }
}