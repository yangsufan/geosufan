namespace GeoDataChecker
{
    partial class frmBatchDataCheck
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
            this.components = new System.ComponentModel.Container();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.comboBoxOrgType = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX14 = new DevComponents.DotNetBar.LabelX();
            this.btnDel = new DevComponents.DotNetBar.ButtonX();
            this.btnSelReverse = new DevComponents.DotNetBar.ButtonX();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.btnOrg = new DevComponents.DotNetBar.ButtonX();
            this.listViewEx = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOk = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnLog = new DevComponents.DotNetBar.ButtonX();
            this.txtLog = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.progressBarX2 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupPanel2.SuspendLayout();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.comboBoxOrgType);
            this.groupPanel2.Controls.Add(this.labelX14);
            this.groupPanel2.Controls.Add(this.btnDel);
            this.groupPanel2.Controls.Add(this.btnSelReverse);
            this.groupPanel2.Controls.Add(this.btnSelAll);
            this.groupPanel2.Controls.Add(this.btnOrg);
            this.groupPanel2.Controls.Add(this.listViewEx);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(12, 12);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(407, 263);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 2;
            this.groupPanel2.Text = "数据选择";
            // 
            // comboBoxOrgType
            // 
            this.comboBoxOrgType.DisplayMember = "Text";
            this.comboBoxOrgType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxOrgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOrgType.FormattingEnabled = true;
            this.comboBoxOrgType.ItemHeight = 15;
            this.comboBoxOrgType.Location = new System.Drawing.Point(80, 3);
            this.comboBoxOrgType.Name = "comboBoxOrgType";
            this.comboBoxOrgType.Size = new System.Drawing.Size(318, 21);
            this.comboBoxOrgType.TabIndex = 10;
            // 
            // labelX14
            // 
            this.labelX14.AutoSize = true;
            this.labelX14.Location = new System.Drawing.Point(2, 6);
            this.labelX14.Name = "labelX14";
            this.labelX14.Size = new System.Drawing.Size(74, 18);
            this.labelX14.TabIndex = 11;
            this.labelX14.Text = " 类    型 :";
            // 
            // btnDel
            // 
            this.btnDel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDel.Location = new System.Drawing.Point(335, 111);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(63, 21);
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "清 除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnSelReverse
            // 
            this.btnSelReverse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelReverse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelReverse.Location = new System.Drawing.Point(335, 84);
            this.btnSelReverse.Name = "btnSelReverse";
            this.btnSelReverse.Size = new System.Drawing.Size(63, 21);
            this.btnSelReverse.TabIndex = 2;
            this.btnSelReverse.Text = "反 选";
            this.btnSelReverse.Click += new System.EventHandler(this.btnSelReverse_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(335, 57);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(63, 21);
            this.btnSelAll.TabIndex = 1;
            this.btnSelAll.Text = "全 选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnOrg
            // 
            this.btnOrg.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOrg.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOrg.Location = new System.Drawing.Point(335, 30);
            this.btnOrg.Name = "btnOrg";
            this.btnOrg.Size = new System.Drawing.Size(63, 21);
            this.btnOrg.TabIndex = 0;
            this.btnOrg.Text = "浏览";
            this.btnOrg.Click += new System.EventHandler(this.btnOrg_Click);
            // 
            // listViewEx
            // 
            // 
            // 
            // 
            this.listViewEx.Border.Class = "ListViewBorder";
            this.listViewEx.CheckBoxes = true;
            this.listViewEx.Location = new System.Drawing.Point(5, 30);
            this.listViewEx.Name = "listViewEx";
            this.listViewEx.ShowItemToolTips = true;
            this.listViewEx.Size = new System.Drawing.Size(324, 203);
            this.listViewEx.TabIndex = 4;
            this.listViewEx.UseCompatibleStateImageBehavior = false;
            this.listViewEx.View = System.Windows.Forms.View.List;
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(348, 348);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(70, 25);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取 消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOk
            // 
            this.btnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOk.Location = new System.Drawing.Point(124, 348);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 25);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "开 始";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.btnLog);
            this.groupPanel3.Controls.Add(this.txtLog);
            this.groupPanel3.Controls.Add(this.labelX1);
            this.groupPanel3.DrawTitleBox = false;
            this.groupPanel3.Location = new System.Drawing.Point(12, 281);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(407, 61);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel3.TabIndex = 8;
            this.groupPanel3.Text = "日志输出设置";
            // 
            // btnLog
            // 
            this.btnLog.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnLog.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnLog.Location = new System.Drawing.Point(335, 6);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(63, 21);
            this.btnLog.TabIndex = 0;
            this.btnLog.Text = "浏 览";
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // txtLog
            // 
            // 
            // 
            // 
            this.txtLog.Border.Class = "TextBoxBorder";
            this.txtLog.Location = new System.Drawing.Point(72, 6);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(257, 21);
            this.txtLog.TabIndex = 2;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.labelX1.Location = new System.Drawing.Point(5, 8);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(68, 18);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "输出路径 :";
            // 
            // progressBarX1
            // 
            this.progressBarX1.Location = new System.Drawing.Point(10, 474);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(408, 13);
            this.progressBarX1.TabIndex = 9;
            this.progressBarX1.Text = "progressBarX1";
            // 
            // progressBarX2
            // 
            this.progressBarX2.Location = new System.Drawing.Point(10, 381);
            this.progressBarX2.Name = "progressBarX2";
            this.progressBarX2.Size = new System.Drawing.Size(408, 13);
            this.progressBarX2.TabIndex = 10;
            this.progressBarX2.Text = "progressBarX2";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(10, 400);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(401, 23);
            this.labelX2.TabIndex = 11;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(200, 348);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(70, 25);
            this.buttonX1.TabIndex = 12;
            this.buttonX1.Text = "暂 停";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(276, 348);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(70, 25);
            this.buttonX2.TabIndex = 13;
            this.buttonX2.Text = "继 续";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmBatchDataCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 434);
            this.Controls.Add(this.buttonX2);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.progressBarX2);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchDataCheck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量数据检查";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBatchDataCheck_FormClosing);
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel2.PerformLayout();
            this.groupPanel3.ResumeLayout(false);
            this.groupPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxOrgType;
        private DevComponents.DotNetBar.LabelX labelX14;
        private DevComponents.DotNetBar.ButtonX btnDel;
        private DevComponents.DotNetBar.ButtonX btnSelReverse;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnOrg;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewEx;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnOk;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.ButtonX btnLog;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLog;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX2;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private System.Windows.Forms.Timer timer1;
    }
}