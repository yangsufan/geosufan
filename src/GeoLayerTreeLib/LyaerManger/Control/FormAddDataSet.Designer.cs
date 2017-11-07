namespace GeoLayerTreeLib.LayerManager
{
    partial class FormAddDataSet
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
            this.btnAddDataSet = new DevComponents.DotNetBar.ButtonX();
            this.txtDataSet = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // txtComment
            // 
            // 
            // 
            // 
            this.txtComment.Border.Class = "TextBoxBorder";
            this.txtComment.Location = new System.Drawing.Point(63, 38);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(239, 110);
            this.txtComment.TabIndex = 28;
            this.txtComment.WordWrap = false;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(3, 38);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(54, 20);
            this.labelX3.TabIndex = 32;
            this.labelX3.Text = "备  注：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(237, 154);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddDataSet
            // 
            this.btnAddDataSet.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddDataSet.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddDataSet.Location = new System.Drawing.Point(166, 154);
            this.btnAddDataSet.Name = "btnAddDataSet";
            this.btnAddDataSet.Size = new System.Drawing.Size(65, 23);
            this.btnAddDataSet.TabIndex = 30;
            this.btnAddDataSet.Text = "添加";
            this.btnAddDataSet.Click += new System.EventHandler(this.btnAddDataSet_Click);
            // 
            // txtDataSet
            // 
            // 
            // 
            // 
            this.txtDataSet.Border.Class = "TextBoxBorder";
            this.txtDataSet.Location = new System.Drawing.Point(63, 13);
            this.txtDataSet.Name = "txtDataSet";
            this.txtDataSet.Size = new System.Drawing.Size(239, 21);
            this.txtDataSet.TabIndex = 27;
            this.txtDataSet.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(3, 15);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(54, 19);
            this.labelX5.TabIndex = 29;
            this.labelX5.Text = "名  称：";
            // 
            // FormAddDataSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 185);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddDataSet);
            this.Controls.Add(this.txtDataSet);
            this.Controls.Add(this.labelX5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddDataSet";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加数据集";
            this.Load += new System.EventHandler(this.FormAddDataSet_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtComment;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnAddDataSet;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDataSet;
        private DevComponents.DotNetBar.LabelX labelX5;
    }
}