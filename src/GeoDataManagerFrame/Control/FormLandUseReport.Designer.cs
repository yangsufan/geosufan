namespace GeoDataManagerFrame
{
    partial class FormLandUseReport
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxExDataSource = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxExDLGrade = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.comboItem4 = new DevComponents.Editors.ComboItem();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxExAreaName = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.buttonXOK = new DevComponents.DotNetBar.ButtonX();
            this.buttonXQuit = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(26, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(71, 31);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "汇总数据源";
            // 
            // comboBoxExDataSource
            // 
            this.comboBoxExDataSource.DisplayMember = "Text";
            this.comboBoxExDataSource.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExDataSource.FormattingEnabled = true;
            this.comboBoxExDataSource.ItemHeight = 15;
            this.comboBoxExDataSource.Location = new System.Drawing.Point(102, 21);
            this.comboBoxExDataSource.Name = "comboBoxExDataSource";
            this.comboBoxExDataSource.Size = new System.Drawing.Size(149, 21);
            this.comboBoxExDataSource.TabIndex = 1;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(26, 65);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(66, 21);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "地类级别";
            // 
            // comboBoxExDLGrade
            // 
            this.comboBoxExDLGrade.DisplayMember = "Text";
            this.comboBoxExDLGrade.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExDLGrade.FormattingEnabled = true;
            this.comboBoxExDLGrade.ItemHeight = 15;
            this.comboBoxExDLGrade.Items.AddRange(new object[] {
            this.comboItem3,
            this.comboItem4});
            this.comboBoxExDLGrade.Location = new System.Drawing.Point(102, 60);
            this.comboBoxExDLGrade.Name = "comboBoxExDLGrade";
            this.comboBoxExDLGrade.Size = new System.Drawing.Size(148, 21);
            this.comboBoxExDLGrade.TabIndex = 3;
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "一级";
            // 
            // comboItem4
            // 
            this.comboItem4.Text = "二级";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(26, 99);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(61, 27);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "面积单位";
            // 
            // comboBoxExAreaName
            // 
            this.comboBoxExAreaName.DisplayMember = "Text";
            this.comboBoxExAreaName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxExAreaName.FormattingEnabled = true;
            this.comboBoxExAreaName.ItemHeight = 15;
            this.comboBoxExAreaName.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.comboBoxExAreaName.Location = new System.Drawing.Point(102, 99);
            this.comboBoxExAreaName.Name = "comboBoxExAreaName";
            this.comboBoxExAreaName.Size = new System.Drawing.Size(149, 21);
            this.comboBoxExAreaName.TabIndex = 5;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "平方米";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "公顷";
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOK.Location = new System.Drawing.Point(37, 139);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(94, 25);
            this.buttonXOK.TabIndex = 6;
            this.buttonXOK.Text = "统计";
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // buttonXQuit
            // 
            this.buttonXQuit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXQuit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXQuit.Location = new System.Drawing.Point(149, 140);
            this.buttonXQuit.Name = "buttonXQuit";
            this.buttonXQuit.Size = new System.Drawing.Size(94, 25);
            this.buttonXQuit.TabIndex = 7;
            this.buttonXQuit.Text = "退出";
            this.buttonXQuit.Click += new System.EventHandler(this.buttonXQuit_Click);
            // 
            // FormLandUseReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 182);
            this.Controls.Add(this.buttonXQuit);
            this.Controls.Add(this.buttonXOK);
            this.Controls.Add(this.comboBoxExAreaName);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.comboBoxExDLGrade);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.comboBoxExDataSource);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLandUseReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "统计设置";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExDataSource;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExDLGrade;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxExAreaName;
        private DevComponents.DotNetBar.ButtonX buttonXOK;
        private DevComponents.DotNetBar.ButtonX buttonXQuit;
        private DevComponents.Editors.ComboItem comboItem3;
        private DevComponents.Editors.ComboItem comboItem4;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
    }
}