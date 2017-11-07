namespace GeoPageLayout
{
    partial class frmRangeOutMap
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
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.Error_Lable = new System.Windows.Forms.Label();
            this.groupBoxLocation = new System.Windows.Forms.GroupBox();
            this.textBoxNorth = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxEast = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelNorth = new DevComponents.DotNetBar.LabelX();
            this.labelEast = new DevComponents.DotNetBar.LabelX();
            this.textBoxSouth = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.textBoxWest = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelSouth = new DevComponents.DotNetBar.LabelX();
            this.labelWest = new DevComponents.DotNetBar.LabelX();
            this.groupBoxLocation.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(211, 134);
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
            this.buttonCancel.Location = new System.Drawing.Point(295, 134);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Error_Lable
            // 
            this.Error_Lable.AutoSize = true;
            this.Error_Lable.Location = new System.Drawing.Point(14, 167);
            this.Error_Lable.Name = "Error_Lable";
            this.Error_Lable.Size = new System.Drawing.Size(41, 12);
            this.Error_Lable.TabIndex = 8;
            this.Error_Lable.Text = "label1";
            this.Error_Lable.Visible = false;
            // 
            // groupBoxLocation
            // 
            this.groupBoxLocation.Controls.Add(this.textBoxNorth);
            this.groupBoxLocation.Controls.Add(this.textBoxEast);
            this.groupBoxLocation.Controls.Add(this.labelNorth);
            this.groupBoxLocation.Controls.Add(this.labelEast);
            this.groupBoxLocation.Controls.Add(this.textBoxSouth);
            this.groupBoxLocation.Controls.Add(this.textBoxWest);
            this.groupBoxLocation.Controls.Add(this.labelSouth);
            this.groupBoxLocation.Controls.Add(this.labelWest);
            this.groupBoxLocation.Location = new System.Drawing.Point(12, 11);
            this.groupBoxLocation.Name = "groupBoxLocation";
            this.groupBoxLocation.Size = new System.Drawing.Size(358, 117);
            this.groupBoxLocation.TabIndex = 9;
            this.groupBoxLocation.TabStop = false;
            this.groupBoxLocation.Text = "请输入定位范围";
            // 
            // textBoxNorth
            // 
            // 
            // 
            // 
            this.textBoxNorth.Border.Class = "TextBoxBorder";
            this.textBoxNorth.Location = new System.Drawing.Point(119, 21);
            this.textBoxNorth.Name = "textBoxNorth";
            this.textBoxNorth.Size = new System.Drawing.Size(129, 21);
            this.textBoxNorth.TabIndex = 2;
            this.textBoxNorth.Text = "0";
            // 
            // textBoxEast
            // 
            // 
            // 
            // 
            this.textBoxEast.Border.Class = "TextBoxBorder";
            this.textBoxEast.Location = new System.Drawing.Point(234, 51);
            this.textBoxEast.Name = "textBoxEast";
            this.textBoxEast.Size = new System.Drawing.Size(114, 21);
            this.textBoxEast.TabIndex = 3;
            this.textBoxEast.Text = "0";
            // 
            // labelNorth
            // 
            this.labelNorth.Location = new System.Drawing.Point(71, 21);
            this.labelNorth.Name = "labelNorth";
            this.labelNorth.Size = new System.Drawing.Size(42, 23);
            this.labelNorth.TabIndex = 2;
            this.labelNorth.Text = "终点Y:";
            // 
            // labelEast
            // 
            this.labelEast.Location = new System.Drawing.Point(189, 52);
            this.labelEast.Name = "labelEast";
            this.labelEast.Size = new System.Drawing.Size(48, 23);
            this.labelEast.TabIndex = 3;
            this.labelEast.Text = "终点X:";
            // 
            // textBoxSouth
            // 
            // 
            // 
            // 
            this.textBoxSouth.Border.Class = "TextBoxBorder";
            this.textBoxSouth.Location = new System.Drawing.Point(119, 82);
            this.textBoxSouth.Name = "textBoxSouth";
            this.textBoxSouth.Size = new System.Drawing.Size(129, 21);
            this.textBoxSouth.TabIndex = 4;
            this.textBoxSouth.Text = "0";
            // 
            // textBoxWest
            // 
            // 
            // 
            // 
            this.textBoxWest.Border.Class = "TextBoxBorder";
            this.textBoxWest.Location = new System.Drawing.Point(53, 51);
            this.textBoxWest.Name = "textBoxWest";
            this.textBoxWest.Size = new System.Drawing.Size(114, 21);
            this.textBoxWest.TabIndex = 1;
            this.textBoxWest.Text = "0";
            // 
            // labelSouth
            // 
            this.labelSouth.Location = new System.Drawing.Point(71, 82);
            this.labelSouth.Name = "labelSouth";
            this.labelSouth.Size = new System.Drawing.Size(42, 23);
            this.labelSouth.TabIndex = 0;
            this.labelSouth.Text = "起点Y:";
            // 
            // labelWest
            // 
            this.labelWest.Location = new System.Drawing.Point(11, 52);
            this.labelWest.Name = "labelWest";
            this.labelWest.Size = new System.Drawing.Size(46, 23);
            this.labelWest.TabIndex = 0;
            this.labelWest.Text = "起点X:";
            // 
            // frmRangeOutMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 167);
            this.Controls.Add(this.groupBoxLocation);
            this.Controls.Add(this.Error_Lable);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRangeOutMap";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坐标范围制图";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmRangeLocation_KeyDown);
            this.groupBoxLocation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private System.Windows.Forms.Label Error_Lable;
        private System.Windows.Forms.GroupBox groupBoxLocation;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxNorth;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxEast;
        private DevComponents.DotNetBar.LabelX labelNorth;
        private DevComponents.DotNetBar.LabelX labelEast;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxSouth;
        private DevComponents.DotNetBar.Controls.TextBoxX textBoxWest;
        private DevComponents.DotNetBar.LabelX labelSouth;
        private DevComponents.DotNetBar.LabelX labelWest;
    }
}