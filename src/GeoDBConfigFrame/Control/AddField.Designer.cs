namespace GeoDBConfigFrame
{
    partial class AddField
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labname = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.textBoxDefault = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDescribe = new System.Windows.Forms.TextBox();
            this.checkBoxChange = new System.Windows.Forms.CheckBox();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.comBoxName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labname
            // 
            this.labname.AutoSize = true;
            this.labname.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.labname.Location = new System.Drawing.Point(14, 15);
            this.labname.Name = "labname";
            this.labname.Size = new System.Drawing.Size(59, 12);
            this.labname.TabIndex = 0;
            this.labname.Text = "字段名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "字段类型:";
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "字母",
            "数字"});
            this.comboBoxType.Location = new System.Drawing.Point(70, 82);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(142, 20);
            this.comboBoxType.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "字段长度:";
            // 
            // textBoxLength
            // 
            this.textBoxLength.Location = new System.Drawing.Point(70, 113);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(142, 21);
            this.textBoxLength.TabIndex = 1;
            // 
            // textBoxDefault
            // 
            this.textBoxDefault.Location = new System.Drawing.Point(70, 162);
            this.textBoxDefault.Name = "textBoxDefault";
            this.textBoxDefault.Size = new System.Drawing.Size(142, 21);
            this.textBoxDefault.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "缺省值:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(14, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "字段描述:";
            // 
            // textBoxDescribe
            // 
            this.textBoxDescribe.Location = new System.Drawing.Point(70, 46);
            this.textBoxDescribe.Name = "textBoxDescribe";
            this.textBoxDescribe.Size = new System.Drawing.Size(142, 21);
            this.textBoxDescribe.TabIndex = 1;
            // 
            // checkBoxChange
            // 
            this.checkBoxChange.AutoSize = true;
            this.checkBoxChange.Location = new System.Drawing.Point(70, 140);
            this.checkBoxChange.Name = "checkBoxChange";
            this.checkBoxChange.Size = new System.Drawing.Size(72, 16);
            this.checkBoxChange.TabIndex = 5;
            this.checkBoxChange.Text = "可变长度";
            this.checkBoxChange.UseVisualStyleBackColor = true;
            this.checkBoxChange.CheckedChanged += new System.EventHandler(this.checkBoxChange_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(37, 200);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(127, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // comBoxName
            // 
            this.comBoxName.FormattingEnabled = true;
            this.comBoxName.Items.AddRange(new object[] {
            "业务代码",
            "年度",
            "专题类型",
            "行政代码",
            "比例尺"});
            this.comBoxName.Location = new System.Drawing.Point(70, 12);
            this.comBoxName.Name = "comBoxName";
            this.comBoxName.Size = new System.Drawing.Size(142, 20);
            this.comBoxName.TabIndex = 7;
            // 
            // AddField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 236);
            this.Controls.Add(this.comBoxName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkBoxChange);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxDefault);
            this.Controls.Add(this.textBoxLength);
            this.Controls.Add(this.textBoxDescribe);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labname);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddField";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加/编辑字段";
            this.Load += new System.EventHandler(this.AddField_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.TextBox textBoxDefault;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDescribe;
        private System.Windows.Forms.CheckBox checkBoxChange;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private System.Windows.Forms.ComboBox comBoxName;
    }
}