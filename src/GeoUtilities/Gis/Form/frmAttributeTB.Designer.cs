namespace GeoUtilities
{
    partial class frmAttributeTB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAttributeTB));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnClearChecked = new DevComponents.DotNetBar.ButtonX();
            this.comboExtent = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem3 = new DevComponents.Editors.ComboItem();
            this.listViewFields = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnDisplaySelect = new DevComponents.DotNetBar.ButtonX();
            this.btnDisplayAll = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.progressStep = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.btnClose = new DevComponents.DotNetBar.ButtonX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.labRecord = new DevComponents.DotNetBar.LabelX();
            this.lab = new DevComponents.DotNetBar.LabelX();
            this.txtPage = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.bttLast = new DevComponents.DotNetBar.ButtonX();
            this.bttFirst = new DevComponents.DotNetBar.ButtonX();
            this.bttNext = new DevComponents.DotNetBar.ButtonX();
            this.bttAfter = new DevComponents.DotNetBar.ButtonX();
            this.TimerShow = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.splitContainer1.Panel2.Controls.Add(this.progressStep);
            this.splitContainer1.Panel2.Controls.Add(this.btnClose);
            this.splitContainer1.Panel2.Controls.Add(this.lblTips);
            this.splitContainer1.Panel2.Controls.Add(this.labRecord);
            this.splitContainer1.Panel2.Controls.Add(this.lab);
            this.splitContainer1.Panel2.Controls.Add(this.txtPage);
            this.splitContainer1.Panel2.Controls.Add(this.bttLast);
            this.splitContainer1.Panel2.Controls.Add(this.bttFirst);
            this.splitContainer1.Panel2.Controls.Add(this.bttNext);
            this.splitContainer1.Panel2.Controls.Add(this.bttAfter);
            this.splitContainer1.Size = new System.Drawing.Size(717, 504);
            this.splitContainer1.SplitterDistance = 475;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.splitContainer2.Panel1.Controls.Add(this.btnClearChecked);
            this.splitContainer2.Panel1.Controls.Add(this.comboExtent);
            this.splitContainer2.Panel1.Controls.Add(this.listViewFields);
            this.splitContainer2.Panel1.Controls.Add(this.labelX2);
            this.splitContainer2.Panel1.Controls.Add(this.btnDisplaySelect);
            this.splitContainer2.Panel1.Controls.Add(this.btnDisplayAll);
            this.splitContainer2.Panel1.Controls.Add(this.labelX1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(717, 475);
            this.splitContainer2.SplitterDistance = 162;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnClearChecked
            // 
            this.btnClearChecked.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClearChecked.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClearChecked.Image = ((System.Drawing.Image)(resources.GetObject("btnClearChecked.Image")));
            this.btnClearChecked.Location = new System.Drawing.Point(12, 40);
            this.btnClearChecked.Name = "btnClearChecked";
            this.btnClearChecked.Size = new System.Drawing.Size(136, 23);
            this.btnClearChecked.TabIndex = 3;
            this.btnClearChecked.Text = "清除选择";
            this.btnClearChecked.Click += new System.EventHandler(this.btnClearChecked_Click);
            // 
            // comboExtent
            // 
            this.comboExtent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboExtent.AutoCompleteCustomSource.AddRange(new string[] {
            "全图范围",
            "当前视图范围",
            "选择要素范围"});
            this.comboExtent.DisplayMember = "Text";
            this.comboExtent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboExtent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboExtent.FormattingEnabled = true;
            this.comboExtent.ItemHeight = 15;
            this.comboExtent.Items.AddRange(new object[] {
            this.comboItem2,
            this.comboItem1,
            this.comboItem3});
            this.comboExtent.Location = new System.Drawing.Point(35, 445);
            this.comboExtent.Name = "comboExtent";
            this.comboExtent.Size = new System.Drawing.Size(112, 21);
            this.comboExtent.TabIndex = 4;
            this.comboExtent.Visible = false;
            this.comboExtent.SelectedIndexChanged += new System.EventHandler(this.comboBoxEx1_SelectedIndexChanged);
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "当前视图范围";
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "全图范围";
            // 
            // comboItem3
            // 
            this.comboItem3.Text = "选择要素范围";
            // 
            // listViewFields
            // 
            this.listViewFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFields.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.listViewFields.Border.Class = "ListViewBorder";
            this.listViewFields.CheckBoxes = true;
            this.listViewFields.Location = new System.Drawing.Point(2, 98);
            this.listViewFields.Name = "listViewFields";
            this.listViewFields.Size = new System.Drawing.Size(155, 372);
            this.listViewFields.TabIndex = 2;
            this.listViewFields.UseCompatibleStateImageBehavior = false;
            this.listViewFields.View = System.Windows.Forms.View.List;
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(48, 451);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(46, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "范围：";
            this.labelX2.Visible = false;
            // 
            // btnDisplaySelect
            // 
            this.btnDisplaySelect.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDisplaySelect.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDisplaySelect.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplaySelect.Image")));
            this.btnDisplaySelect.Location = new System.Drawing.Point(11, 69);
            this.btnDisplaySelect.Name = "btnDisplaySelect";
            this.btnDisplaySelect.Size = new System.Drawing.Size(136, 23);
            this.btnDisplaySelect.TabIndex = 1;
            this.btnDisplaySelect.Text = "选择字段显示";
            this.btnDisplaySelect.Click += new System.EventHandler(this.btnDisplaySelect_Click);
            // 
            // btnDisplayAll
            // 
            this.btnDisplayAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDisplayAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDisplayAll.Image = ((System.Drawing.Image)(resources.GetObject("btnDisplayAll.Image")));
            this.btnDisplayAll.Location = new System.Drawing.Point(12, 12);
            this.btnDisplayAll.Name = "btnDisplayAll";
            this.btnDisplayAll.Size = new System.Drawing.Size(136, 23);
            this.btnDisplayAll.TabIndex = 0;
            this.btnDisplayAll.Text = "全部选择";
            this.btnDisplayAll.Click += new System.EventHandler(this.btnDisplayAll_Click);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(80, 453);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(67, 20);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "显示进度：";
            this.labelX1.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewX1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.39019F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(549, 473);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.Size = new System.Drawing.Size(543, 467);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellDoubleClick);
            // 
            // progressStep
            // 
            this.progressStep.BackColor = System.Drawing.Color.White;
            this.progressStep.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressStep.Location = new System.Drawing.Point(3, 4);
            this.progressStep.Name = "progressStep";
            this.progressStep.Size = new System.Drawing.Size(160, 16);
            this.progressStep.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClose.Location = new System.Drawing.Point(658, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 20);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关 闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTips
            // 
            this.lblTips.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTips.BackColor = System.Drawing.Color.Transparent;
            this.lblTips.Location = new System.Drawing.Point(555, 5);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(97, 15);
            this.lblTips.TabIndex = 3;
            this.lblTips.Text = "共计0个";
            // 
            // labRecord
            // 
            this.labRecord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.labRecord.BackColor = System.Drawing.Color.Transparent;
            this.labRecord.Location = new System.Drawing.Point(461, 5);
            this.labRecord.Name = "labRecord";
            this.labRecord.Size = new System.Drawing.Size(67, 14);
            this.labRecord.TabIndex = 6;
            // 
            // lab
            // 
            this.lab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lab.BackColor = System.Drawing.Color.Transparent;
            this.lab.Location = new System.Drawing.Point(375, 4);
            this.lab.Name = "lab";
            this.lab.Size = new System.Drawing.Size(92, 16);
            this.lab.TabIndex = 5;
            this.lab.Text = "当前显示记录：";
            // 
            // txtPage
            // 
            // 
            // 
            // 
            this.txtPage.Border.Class = "TextBoxBorder";
            this.txtPage.Location = new System.Drawing.Point(238, 2);
            this.txtPage.Name = "txtPage";
            this.txtPage.ReadOnly = true;
            this.txtPage.Size = new System.Drawing.Size(63, 21);
            this.txtPage.TabIndex = 4;
            // 
            // bttLast
            // 
            this.bttLast.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttLast.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttLast.Enabled = false;
            this.bttLast.Location = new System.Drawing.Point(341, 1);
            this.bttLast.Name = "bttLast";
            this.bttLast.Size = new System.Drawing.Size(28, 21);
            this.bttLast.TabIndex = 3;
            this.bttLast.Text = ">>";
            this.bttLast.Tooltip = "最后一页";
            this.bttLast.Click += new System.EventHandler(this.bttLast_Click);
            // 
            // bttFirst
            // 
            this.bttFirst.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttFirst.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttFirst.Enabled = false;
            this.bttFirst.Location = new System.Drawing.Point(169, 1);
            this.bttFirst.Name = "bttFirst";
            this.bttFirst.Size = new System.Drawing.Size(28, 21);
            this.bttFirst.TabIndex = 0;
            this.bttFirst.Text = "<<";
            this.bttFirst.Tooltip = "第一页";
            this.bttFirst.Click += new System.EventHandler(this.bttFirst_Click);
            // 
            // bttNext
            // 
            this.bttNext.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttNext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.bttNext.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttNext.Enabled = false;
            this.bttNext.Location = new System.Drawing.Point(307, 1);
            this.bttNext.Name = "bttNext";
            this.bttNext.Size = new System.Drawing.Size(28, 21);
            this.bttNext.TabIndex = 2;
            this.bttNext.Text = ">";
            this.bttNext.Tooltip = "下一页";
            this.bttNext.Click += new System.EventHandler(this.bttNext_Click);
            // 
            // bttAfter
            // 
            this.bttAfter.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttAfter.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttAfter.Enabled = false;
            this.bttAfter.Location = new System.Drawing.Point(203, 1);
            this.bttAfter.Name = "bttAfter";
            this.bttAfter.Size = new System.Drawing.Size(28, 21);
            this.bttAfter.TabIndex = 1;
            this.bttAfter.Text = "<";
            this.bttAfter.Tooltip = "上一页";
            this.bttAfter.Click += new System.EventHandler(this.bttAfter_Click);
            // 
            // TimerShow
            // 
            this.TimerShow.Interval = 600;
            this.TimerShow.Tick += new System.EventHandler(this.TimerShow_Tick);
            // 
            // frmAttributeTB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(717, 504);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAttributeTB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "要素属性表";
            this.Load += new System.EventHandler(this.frmAttributeTB_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewFields;
        private DevComponents.DotNetBar.ButtonX btnDisplaySelect;
        private DevComponents.DotNetBar.ButtonX btnDisplayAll;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnClose;
        private System.Windows.Forms.Timer TimerShow;
        private DevComponents.DotNetBar.ButtonX btnClearChecked;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboExtent;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.ComboItem comboItem3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevComponents.DotNetBar.ButtonX bttLast;
        private DevComponents.DotNetBar.ButtonX bttNext;
        private DevComponents.DotNetBar.ButtonX bttAfter;
        private DevComponents.DotNetBar.ButtonX bttFirst;
        private DevComponents.DotNetBar.Controls.TextBoxX txtPage;
        private DevComponents.DotNetBar.LabelX lab;
        private DevComponents.DotNetBar.LabelX labRecord;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressStep;
    }
}