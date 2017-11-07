namespace GeoLayerTreeLib.LayerManager
{
    partial class FormAddFolder
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
            this.txtComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnAddFolder = new DevComponents.DotNetBar.ButtonX();
            this.txtFolder = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtScale = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbDataType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.cmbXZQ = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.cmbDIRType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.cmbYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.SuspendLayout();
            // 
            // txtComment
            // 
            // 
            // 
            // 
            this.txtComment.Border.Class = "TextBoxBorder";
            this.txtComment.Location = new System.Drawing.Point(68, 146);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(298, 54);
            this.txtComment.TabIndex = 22;
            this.txtComment.WordWrap = false;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(5, 149);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(69, 20);
            this.labelX3.TabIndex = 26;
            this.labelX3.Text = "备    注：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(299, 206);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddFolder.Location = new System.Drawing.Point(228, 206);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(65, 23);
            this.btnAddFolder.TabIndex = 24;
            this.btnAddFolder.Text = "添加";
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // txtFolder
            // 
            // 
            // 
            // 
            this.txtFolder.Border.Class = "TextBoxBorder";
            this.txtFolder.Location = new System.Drawing.Point(68, 10);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(298, 21);
            this.txtFolder.TabIndex = 21;
            this.txtFolder.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(5, 11);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(66, 19);
            this.labelX5.TabIndex = 23;
            this.labelX5.Text = "名    称：";
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(5, 38);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(66, 19);
            this.labelX1.TabIndex = 23;
            this.labelX1.Text = "比 例 尺：";
            // 
            // txtScale
            // 
            // 
            // 
            // 
            this.txtScale.Border.Class = "TextBoxBorder";
            this.txtScale.Location = new System.Drawing.Point(68, 37);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(298, 21);
            this.txtScale.TabIndex = 21;
            this.txtScale.WordWrap = false;
            this.txtScale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtScale_KeyPress);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(5, 65);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(66, 19);
            this.labelX2.TabIndex = 23;
            this.labelX2.Text = "数据类型：";
            // 
            // cmbDataType
            // 
            this.cmbDataType.DisplayMember = "Text";
            this.cmbDataType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.ItemHeight = 15;
            this.cmbDataType.Location = new System.Drawing.Point(68, 64);
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.Size = new System.Drawing.Size(298, 21);
            this.cmbDataType.TabIndex = 27;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(5, 119);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(66, 19);
            this.labelX4.TabIndex = 23;
            this.labelX4.Text = "行 政 区：";
            this.labelX4.Visible = false;
            // 
            // cmbXZQ
            // 
            this.cmbXZQ.DisplayMember = "Text";
            this.cmbXZQ.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbXZQ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXZQ.FormattingEnabled = true;
            this.cmbXZQ.ItemHeight = 15;
            this.cmbXZQ.Location = new System.Drawing.Point(68, 118);
            this.cmbXZQ.Name = "cmbXZQ";
            this.cmbXZQ.Size = new System.Drawing.Size(298, 21);
            this.cmbXZQ.TabIndex = 27;
            this.cmbXZQ.Visible = false;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(5, 92);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(66, 19);
            this.labelX6.TabIndex = 23;
            this.labelX6.Text = "专题类型：";
            // 
            // cmbDIRType
            // 
            this.cmbDIRType.DisplayMember = "Text";
            this.cmbDIRType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDIRType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDIRType.FormattingEnabled = true;
            this.cmbDIRType.ItemHeight = 15;
            this.cmbDIRType.Location = new System.Drawing.Point(68, 91);
            this.cmbDIRType.Name = "cmbDIRType";
            this.cmbDIRType.Size = new System.Drawing.Size(298, 21);
            this.cmbDIRType.TabIndex = 27;
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(5, 119);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(66, 19);
            this.labelX7.TabIndex = 23;
            this.labelX7.Text = "年    度：";
            // 
            // cmbYear
            // 
            this.cmbYear.DisplayMember = "Text";
            this.cmbYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.ItemHeight = 15;
            this.cmbYear.Location = new System.Drawing.Point(68, 119);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(298, 21);
            this.cmbYear.TabIndex = 27;
            // 
            // FormAddFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 235);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.cmbXZQ);
            this.Controls.Add(this.cmbDIRType);
            this.Controls.Add(this.cmbDataType);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.btnAddFolder);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.labelX5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddFolder";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加专题";
            this.Load += new System.EventHandler(this.FormAddFolder_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtComment;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnAddFolder;
        private DevComponents.DotNetBar.Controls.TextBoxX txtFolder;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX txtScale;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDataType;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbXZQ;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbDIRType;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbYear;

    }
}