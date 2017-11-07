namespace GeoArcScene3DAnalyse
{
    partial class frm3DSpacedistance
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
            this.labeSpace1 = new System.Windows.Forms.Label();
            this.labeVer3 = new System.Windows.Forms.Label();
            this.labLevel2 = new System.Windows.Forms.Label();
            this.labSum4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labeSpace1
            // 
            this.labeSpace1.AutoSize = true;
            this.labeSpace1.Location = new System.Drawing.Point(102, 80);
            this.labeSpace1.Name = "labeSpace1";
            this.labeSpace1.Size = new System.Drawing.Size(0, 12);
            this.labeSpace1.TabIndex = 0;
            // 
            // labeVer3
            // 
            this.labeVer3.AutoSize = true;
            this.labeVer3.Location = new System.Drawing.Point(102, 44);
            this.labeVer3.Name = "labeVer3";
            this.labeVer3.Size = new System.Drawing.Size(0, 12);
            this.labeVer3.TabIndex = 1;
            // 
            // labLevel2
            // 
            this.labLevel2.AutoSize = true;
            this.labLevel2.Location = new System.Drawing.Point(102, 9);
            this.labLevel2.Name = "labLevel2";
            this.labLevel2.Size = new System.Drawing.Size(0, 12);
            this.labLevel2.TabIndex = 2;
            // 
            // labSum4
            // 
            this.labSum4.AutoSize = true;
            this.labSum4.Location = new System.Drawing.Point(102, 116);
            this.labSum4.Name = "labSum4";
            this.labSum4.Size = new System.Drawing.Size(0, 12);
            this.labSum4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "当前水平距离：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "当前垂直距离：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "当前空间距离：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "总空间距离：";
            // 
            // frm3DSpacedistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 137);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labSum4);
            this.Controls.Add(this.labLevel2);
            this.Controls.Add(this.labeVer3);
            this.Controls.Add(this.labeSpace1);
            this.MaximizeBox = false;
            this.Name = "frm3DSpacedistance";
            this.ShowIcon = false;
            this.Text = "空间距离";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm3DSpacedistance_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labeSpace1;
        public System.Windows.Forms.Label labeVer3;
        public System.Windows.Forms.Label labLevel2;
        public System.Windows.Forms.Label labSum4;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;

    }
}