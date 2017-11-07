namespace GeoDBConfigFrame
{
    partial class AddLayerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.textBoxDescri = new System.Windows.Forms.TextBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.numericTranspar = new System.Windows.Forms.NumericUpDown();
            this.textBoxGroupType = new System.Windows.Forms.TextBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.ComboBoxName = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericTranspar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "组名称:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "图层名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "图层描述:";
            // 
            // groupType
            // 
            this.groupType.DisplayMember = "Text";
            this.groupType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.groupType.FormattingEnabled = true;
            this.groupType.Location = new System.Drawing.Point(82, 20);
            this.groupType.Name = "groupType";
            this.groupType.Size = new System.Drawing.Size(173, 22);
            this.groupType.TabIndex = 30;
            // 
            // textBoxDescri
            // 
            this.textBoxDescri.Location = new System.Drawing.Point(82, 87);
            this.textBoxDescri.Name = "textBoxDescri";
            this.textBoxDescri.Size = new System.Drawing.Size(172, 21);
            this.textBoxDescri.TabIndex = 32;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(195, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 23);
            this.btnCancel.TabIndex = 42;
            this.btnCancel.Text = "取 消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.Location = new System.Drawing.Point(111, 204);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 41;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "透明度:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "显示比例:";
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.DisplayMember = "Text";
            this.comboBoxScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(82, 116);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.Size = new System.Drawing.Size(173, 22);
            this.comboBoxScale.TabIndex = 30;
            // 
            // numericTranspar
            // 
            this.numericTranspar.Location = new System.Drawing.Point(212, 144);
            this.numericTranspar.Name = "numericTranspar";
            this.numericTranspar.Size = new System.Drawing.Size(43, 21);
            this.numericTranspar.TabIndex = 43;
            // 
            // textBoxGroupType
            // 
            this.textBoxGroupType.Location = new System.Drawing.Point(82, 21);
            this.textBoxGroupType.Name = "textBoxGroupType";
            this.textBoxGroupType.Size = new System.Drawing.Size(172, 21);
            this.textBoxGroupType.TabIndex = 44;
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(73, 144);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(133, 45);
            this.trackBar.TabIndex = 45;
            this.trackBar.TickFrequency = 10;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // ComboBoxName
            // 
            this.ComboBoxName.FormattingEnabled = true;
            this.ComboBoxName.Location = new System.Drawing.Point(82, 56);
            this.ComboBoxName.Name = "ComboBoxName";
            this.ComboBoxName.Size = new System.Drawing.Size(173, 20);
            this.ComboBoxName.TabIndex = 46;
            this.ComboBoxName.TextChanged += new System.EventHandler(this.ComboBoxName_TextChanged);
            // 
            // AddLayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 254);
            this.Controls.Add(this.ComboBoxName);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.textBoxGroupType);
            this.Controls.Add(this.numericTranspar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBoxDescri);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxScale);
            this.Controls.Add(this.groupType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddLayerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图层信息";
            ((System.ComponentModel.ISupportInitialize)(this.numericTranspar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx groupType;
        private System.Windows.Forms.TextBox textBoxDescri;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxScale;
        private System.Windows.Forms.NumericUpDown numericTranspar;
        private System.Windows.Forms.TextBox textBoxGroupType;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.ComboBox ComboBoxName;
    }
}