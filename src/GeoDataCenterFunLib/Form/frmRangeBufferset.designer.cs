namespace GeoDataCenterFunLib
{
    partial class frmRangeBufferset
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
            this.groupBoxLocation = new System.Windows.Forms.GroupBox();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.textBoxNorth = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxEast = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelNorth = new DevComponents.DotNetBar.LabelX();
            this.labelEast = new DevComponents.DotNetBar.LabelX();
            this.textBoxSouth = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxWest = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelSouth = new DevComponents.DotNetBar.LabelX();
            this.labelWest = new DevComponents.DotNetBar.LabelX();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.Error_Lable = new System.Windows.Forms.Label();
            this.groupBoxLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLocation
            // 
            this.groupBoxLocation.Controls.Add(this.labelX1);
            this.groupBoxLocation.Controls.Add(this.textBoxNorth);
            this.groupBoxLocation.Controls.Add(this.textBoxEast);
            this.groupBoxLocation.Controls.Add(this.labelNorth);
            this.groupBoxLocation.Controls.Add(this.labelEast);
            this.groupBoxLocation.Controls.Add(this.textBoxSouth);
            this.groupBoxLocation.Controls.Add(this.textBoxWest);
            this.groupBoxLocation.Controls.Add(this.labelSouth);
            this.groupBoxLocation.Controls.Add(this.labelWest);
            this.groupBoxLocation.Location = new System.Drawing.Point(12, 12);
            this.groupBoxLocation.Name = "groupBoxLocation";
            this.groupBoxLocation.Size = new System.Drawing.Size(358, 141);
            this.groupBoxLocation.TabIndex = 0;
            this.groupBoxLocation.TabStop = false;
            this.groupBoxLocation.Text = "请输入坐标范围";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(10, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(338, 15);
            this.labelX1.TabIndex = 6;
            this.labelX1.Text = "中国地理范围:(东经73.66°-135.04° 北纬3.86°-53.55°)";
            // 
            // textBoxNorth
            // 
            // 
            // 
            // 
            this.textBoxNorth.Border.Class = "TextBoxBorder";
            this.textBoxNorth.Location = new System.Drawing.Point(116, 44);
            this.textBoxNorth.Name = "textBoxNorth";
            this.textBoxNorth.Size = new System.Drawing.Size(129, 21);
            this.textBoxNorth.TabIndex = 2;
            this.textBoxNorth.Text = "0";
            this.textBoxNorth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRangeLocation_KeyDown);
            this.textBoxNorth.TextChanged += new System.EventHandler(this.textBoxNorth_TextChanged);
            // 
            // textBoxEast
            // 
            // 
            // 
            // 
            this.textBoxEast.Border.Class = "TextBoxBorder";
            this.textBoxEast.Location = new System.Drawing.Point(213, 74);
            this.textBoxEast.Name = "textBoxEast";
            this.textBoxEast.Size = new System.Drawing.Size(129, 21);
            this.textBoxEast.TabIndex = 3;
            this.textBoxEast.Text = "0";
            this.textBoxEast.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRangeLocation_KeyDown);
            this.textBoxEast.TextChanged += new System.EventHandler(this.textBoxEast_TextChanged);
            // 
            // labelNorth
            // 
            this.labelNorth.Location = new System.Drawing.Point(89, 44);
            this.labelNorth.Name = "labelNorth";
            this.labelNorth.Size = new System.Drawing.Size(21, 23);
            this.labelNorth.TabIndex = 2;
            this.labelNorth.Text = "北:";
            // 
            // labelEast
            // 
            this.labelEast.Location = new System.Drawing.Point(186, 75);
            this.labelEast.Name = "labelEast";
            this.labelEast.Size = new System.Drawing.Size(21, 23);
            this.labelEast.TabIndex = 3;
            this.labelEast.Text = "东:";
            // 
            // textBoxSouth
            // 
            // 
            // 
            // 
            this.textBoxSouth.Border.Class = "TextBoxBorder";
            this.textBoxSouth.Location = new System.Drawing.Point(116, 105);
            this.textBoxSouth.Name = "textBoxSouth";
            this.textBoxSouth.Size = new System.Drawing.Size(129, 21);
            this.textBoxSouth.TabIndex = 4;
            this.textBoxSouth.Text = "0";
            this.textBoxSouth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRangeLocation_KeyDown);
            this.textBoxSouth.TextChanged += new System.EventHandler(this.textBoxSouth_TextChanged);
            // 
            // textBoxWest
            // 
            // 
            // 
            // 
            this.textBoxWest.Border.Class = "TextBoxBorder";
            this.textBoxWest.Location = new System.Drawing.Point(35, 74);
            this.textBoxWest.Name = "textBoxWest";
            this.textBoxWest.Size = new System.Drawing.Size(129, 21);
            this.textBoxWest.TabIndex = 1;
            this.textBoxWest.Text = "0";
            this.textBoxWest.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRangeLocation_KeyDown);
            this.textBoxWest.TextChanged += new System.EventHandler(this.textBoxWest_TextChanged);
            // 
            // labelSouth
            // 
            this.labelSouth.Location = new System.Drawing.Point(89, 105);
            this.labelSouth.Name = "labelSouth";
            this.labelSouth.Size = new System.Drawing.Size(21, 23);
            this.labelSouth.TabIndex = 0;
            this.labelSouth.Text = "南:";
            // 
            // labelWest
            // 
            this.labelWest.Location = new System.Drawing.Point(8, 75);
            this.labelWest.Name = "labelWest";
            this.labelWest.Size = new System.Drawing.Size(21, 23);
            this.labelWest.TabIndex = 0;
            this.labelWest.Text = "西:";
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(209, 162);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(294, 162);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Error_Lable
            // 
            this.Error_Lable.AutoSize = true;
            this.Error_Lable.Location = new System.Drawing.Point(16, 166);
            this.Error_Lable.Name = "Error_Lable";
            this.Error_Lable.Size = new System.Drawing.Size(41, 12);
            this.Error_Lable.TabIndex = 8;
            this.Error_Lable.Text = "label1";
            this.Error_Lable.Visible = false;
            // 
            // frmRangeBufferset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 190);
            this.Controls.Add(this.Error_Lable);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxLocation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRangeBufferset";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入范围缓冲查询";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRangeLocation_KeyDown);
            this.groupBoxLocation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLocation;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxSouth;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxWest;
        private DevComponents.DotNetBar.LabelX labelSouth;
        private DevComponents.DotNetBar.LabelX labelWest;
        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxNorth;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxEast;
        private DevComponents.DotNetBar.LabelX labelNorth;
        private DevComponents.DotNetBar.LabelX labelEast;
        private System.Windows.Forms.Label Error_Lable;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}