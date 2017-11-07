namespace GeoDBATool
{
    partial class frmStractHistorySet
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
            this.txtProjectName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.btnSave = new DevComponents.DotNetBar.ButtonX();
            this.txtSavePath = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.comBoxType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.progressBarXFeat = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.progressBarXLay = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelXMemo = new DevComponents.DotNetBar.LabelX();
            this.listViewEx = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.SuspendLayout();
            // 
            // txtProjectName
            // 
            // 
            // 
            // 
            this.txtProjectName.Border.Class = "TextBoxBorder";
            this.txtProjectName.Location = new System.Drawing.Point(77, 225);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(213, 21);
            this.txtProjectName.TabIndex = 24;
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.Location = new System.Drawing.Point(12, 228);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(68, 18);
            this.labelX4.TabIndex = 30;
            this.labelX4.Text = "输出名称 :";
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(229, 279);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(70, 25);
            this.btnCancle.TabIndex = 27;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(153, 279);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 25);
            this.btnOk.TabIndex = 26;
            this.btnOk.Text = "确 定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSave.Location = new System.Drawing.Point(258, 252);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(32, 21);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "...";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSavePath
            // 
            // 
            // 
            // 
            this.txtSavePath.Border.Class = "TextBoxBorder";
            this.txtSavePath.Location = new System.Drawing.Point(77, 252);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(182, 21);
            this.txtSavePath.TabIndex = 28;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Location = new System.Drawing.Point(12, 255);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 18);
            this.labelX1.TabIndex = 29;
            this.labelX1.Text = "保存路径 :";
            // 
            // comBoxType
            // 
            this.comBoxType.DisplayMember = "Text";
            this.comBoxType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comBoxType.FormattingEnabled = true;
            this.comBoxType.ItemHeight = 15;
            this.comBoxType.Location = new System.Drawing.Point(77, 198);
            this.comBoxType.Name = "comBoxType";
            this.comBoxType.Size = new System.Drawing.Size(213, 21);
            this.comBoxType.TabIndex = 31;
            this.comBoxType.SelectedIndexChanged += new System.EventHandler(this.comBoxType_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.Location = new System.Drawing.Point(12, 201);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 18);
            this.labelX2.TabIndex = 32;
            this.labelX2.Text = "输出类型 :";
            // 
            // progressBarXFeat
            // 
            this.progressBarXFeat.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused;
            this.progressBarXFeat.Location = new System.Drawing.Point(-2, 323);
            this.progressBarXFeat.Name = "progressBarXFeat";
            this.progressBarXFeat.Size = new System.Drawing.Size(306, 10);
            this.progressBarXFeat.TabIndex = 34;
            // 
            // progressBarXLay
            // 
            this.progressBarXLay.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused;
            this.progressBarXLay.Location = new System.Drawing.Point(-2, 310);
            this.progressBarXLay.Name = "progressBarXLay";
            this.progressBarXLay.Size = new System.Drawing.Size(306, 10);
            this.progressBarXLay.TabIndex = 33;
            // 
            // labelXMemo
            // 
            this.labelXMemo.AutoSize = true;
            this.labelXMemo.Location = new System.Drawing.Point(3, 295);
            this.labelXMemo.Name = "labelXMemo";
            this.labelXMemo.Size = new System.Drawing.Size(0, 0);
            this.labelXMemo.TabIndex = 35;
            // 
            // listViewEx
            // 
            // 
            // 
            // 
            this.listViewEx.Border.Class = "ListViewBorder";
            this.listViewEx.CheckBoxes = true;
            this.listViewEx.Location = new System.Drawing.Point(12, 12);
            this.listViewEx.Name = "listViewEx";
            this.listViewEx.ShowItemToolTips = true;
            this.listViewEx.Size = new System.Drawing.Size(278, 180);
            this.listViewEx.TabIndex = 36;
            this.listViewEx.UseCompatibleStateImageBehavior = false;
            this.listViewEx.View = System.Windows.Forms.View.List;
            // 
            // frmStractHistorySet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 333);
            this.Controls.Add(this.listViewEx);
            this.Controls.Add(this.labelXMemo);
            this.Controls.Add(this.progressBarXFeat);
            this.Controls.Add(this.progressBarXLay);
            this.Controls.Add(this.comBoxType);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.txtProjectName);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSavePath);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmStractHistorySet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输出设置";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevComponents.DotNetBar.Controls.TextBoxX txtProjectName;
        public DevComponents.DotNetBar.LabelX labelX4;
        public DevComponents.DotNetBar.ButtonX btnCancle;
        public DevComponents.DotNetBar.ButtonX btnOk;
        public DevComponents.DotNetBar.ButtonX btnSave;
        public DevComponents.DotNetBar.Controls.TextBoxX txtSavePath;
        public DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comBoxType;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarXFeat;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarXLay;
        private DevComponents.DotNetBar.LabelX labelXMemo;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx;
    }
}