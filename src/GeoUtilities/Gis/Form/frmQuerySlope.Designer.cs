namespace GeoUtilities
{
    partial class frmQuerySlope
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
            this.comboBoxOpen = new System.Windows.Forms.ComboBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.DrawGeo = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtSlope = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.label = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // comboBoxOpen
            // 
            this.comboBoxOpen.FormattingEnabled = true;
            this.comboBoxOpen.Location = new System.Drawing.Point(92, 7);
            this.comboBoxOpen.Name = "comboBoxOpen";
            this.comboBoxOpen.Size = new System.Drawing.Size(255, 20);
            this.comboBoxOpen.TabIndex = 28;
            this.comboBoxOpen.SelectedIndexChanged += new System.EventHandler(this.comboBoxOpen_SelectedIndexChanged);
            this.comboBoxOpen.DropDown += new System.EventHandler(this.comboBoxOpen_DropDown);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 7);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(79, 23);
            this.labelX1.TabIndex = 30;
            this.labelX1.Text = "选择要素集：";
            // 
            // DrawGeo
            // 
            this.DrawGeo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DrawGeo.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DrawGeo.Location = new System.Drawing.Point(272, 47);
            this.DrawGeo.Name = "DrawGeo";
            this.DrawGeo.Size = new System.Drawing.Size(75, 23);
            this.DrawGeo.TabIndex = 31;
            this.DrawGeo.Text = "取点工具";
            this.DrawGeo.Click += new System.EventHandler(this.DrawGeo_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(30, 49);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(56, 23);
            this.labelX2.TabIndex = 32;
            this.labelX2.Text = "坡度值：";
            // 
            // txtSlope
            // 
            // 
            // 
            // 
            this.txtSlope.Border.Class = "TextBoxBorder";
            this.txtSlope.Location = new System.Drawing.Point(93, 47);
            this.txtSlope.Name = "txtSlope";
            this.txtSlope.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtSlope.Size = new System.Drawing.Size(146, 21);
            this.txtSlope.TabIndex = 33;
            this.txtSlope.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(241, 50);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(21, 23);
            this.labelX3.TabIndex = 34;
            this.labelX3.Text = "度";
            // 
            // label
            // 
            this.label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label.Location = new System.Drawing.Point(-3, 72);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(265, 23);
            this.label.TabIndex = 35;
            // 
            // frmQuerySlope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 90);
            this.Controls.Add(this.label);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.txtSlope);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.DrawGeo);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.comboBoxOpen);
            this.MaximizeBox = false;
            this.Name = "frmQuerySlope";
            this.ShowIcon = false;
            this.Text = "坡度查询";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmQuerySlope_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxOpen;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX DrawGeo;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSlope;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX label;
    }
}