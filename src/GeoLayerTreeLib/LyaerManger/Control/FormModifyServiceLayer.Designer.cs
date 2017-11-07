namespace GeoLayerTreeLib
{
    partial class FormModifyServiceLayer
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
            this.txtServiceLocation = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelServiceLocation = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.txtLayerName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelServiceName = new DevComponents.DotNetBar.LabelX();
            this.txtServiceName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.chkView = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.chkLoad = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.SuspendLayout();
            // 
            // txtServiceLocation
            // 
            // 
            // 
            // 
            this.txtServiceLocation.Border.Class = "TextBoxBorder";
            this.txtServiceLocation.Location = new System.Drawing.Point(72, 61);
            this.txtServiceLocation.Multiline = true;
            this.txtServiceLocation.Name = "txtServiceLocation";
            this.txtServiceLocation.Size = new System.Drawing.Size(230, 21);
            this.txtServiceLocation.TabIndex = 28;
            this.txtServiceLocation.WordWrap = false;
            // 
            // labelServiceLocation
            // 
            this.labelServiceLocation.Location = new System.Drawing.Point(7, 62);
            this.labelServiceLocation.Name = "labelServiceLocation";
            this.labelServiceLocation.Size = new System.Drawing.Size(68, 19);
            this.labelServiceLocation.TabIndex = 32;
            this.labelServiceLocation.Text = "服务地址：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(227, 118);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(144, 118);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 30;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtLayerName
            // 
            // 
            // 
            // 
            this.txtLayerName.Border.Class = "TextBoxBorder";
            this.txtLayerName.Location = new System.Drawing.Point(72, 36);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(230, 21);
            this.txtLayerName.TabIndex = 27;
            this.txtLayerName.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(9, 37);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(65, 19);
            this.labelX5.TabIndex = 29;
            this.labelX5.Text = "图层名：";
            // 
            // labelServiceName
            // 
            this.labelServiceName.Location = new System.Drawing.Point(7, 88);
            this.labelServiceName.Name = "labelServiceName";
            this.labelServiceName.Size = new System.Drawing.Size(68, 19);
            this.labelServiceName.TabIndex = 32;
            this.labelServiceName.Text = "服务名称：";
            // 
            // txtServiceName
            // 
            // 
            // 
            // 
            this.txtServiceName.Border.Class = "TextBoxBorder";
            this.txtServiceName.Location = new System.Drawing.Point(72, 87);
            this.txtServiceName.Multiline = true;
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(230, 21);
            this.txtServiceName.TabIndex = 28;
            this.txtServiceName.WordWrap = false;
            // 
            // chkView
            // 
            this.chkView.Location = new System.Drawing.Point(93, 11);
            this.chkView.Name = "chkView";
            this.chkView.Size = new System.Drawing.Size(82, 24);
            this.chkView.TabIndex = 67;
            this.chkView.Text = "显示";
            // 
            // chkLoad
            // 
            this.chkLoad.Location = new System.Drawing.Point(12, 11);
            this.chkLoad.Name = "chkLoad";
            this.chkLoad.Size = new System.Drawing.Size(79, 24);
            this.chkLoad.TabIndex = 66;
            this.chkLoad.Text = "加载";
            // 
            // FormModifyServiceLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 152);
            this.Controls.Add(this.chkView);
            this.Controls.Add(this.chkLoad);
            this.Controls.Add(this.txtServiceName);
            this.Controls.Add(this.labelServiceName);
            this.Controls.Add(this.txtServiceLocation);
            this.Controls.Add(this.labelServiceLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtLayerName);
            this.Controls.Add(this.labelX5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormModifyServiceLayer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改服务层";
            this.Load += new System.EventHandler(this.FormModifyServiceLayer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtServiceLocation;
        private DevComponents.DotNetBar.LabelX labelServiceLocation;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLayerName;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelServiceName;
        private DevComponents.DotNetBar.Controls.TextBoxX txtServiceName;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkView;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkLoad;
    }
}