namespace FileDBTool
{
    partial class FrmProjectSetting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewMeta = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.listViewData = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.MapRangeBox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.ProjectRangeBox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonX3 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX4 = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMeta)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewMeta
            // 
            this.dataGridViewMeta.AllowUserToAddRows = false;
            this.dataGridViewMeta.AllowUserToDeleteRows = false;
            this.dataGridViewMeta.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewMeta.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewMeta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewMeta.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMeta.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewMeta.Location = new System.Drawing.Point(3, 32);
            this.dataGridViewMeta.Name = "dataGridViewMeta";
            this.dataGridViewMeta.RowTemplate.Height = 23;
            this.dataGridViewMeta.Size = new System.Drawing.Size(367, 523);
            this.dataGridViewMeta.TabIndex = 0;
            this.dataGridViewMeta.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMeta_CellValueChanged);
            this.dataGridViewMeta.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewMeta_Scroll);
            this.dataGridViewMeta.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewMeta_CellValidating);
            this.dataGridViewMeta.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMeta_CellEndEdit);
            this.dataGridViewMeta.CurrentCellChanged += new System.EventHandler(this.dataGridViewMeta_CurrentCellChanged);
            this.dataGridViewMeta.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewMeta_DataError);
            this.dataGridViewMeta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewMeta_KeyPress);
            this.dataGridViewMeta.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewMeta_ColumnWidthChanged);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(442, 576);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(59, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "保 存";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(507, 576);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(61, 23);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取　消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(60, 620);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(79, 21);
            this.dateTimePicker1.TabIndex = 3;
            this.dateTimePicker1.VisibleChanged += new System.EventHandler(this.dateTimePicker1_VisibleChanged);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(4, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 23);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "数据文件：";
            // 
            // listViewData
            // 
            this.listViewData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.listViewData.Border.Class = "ListViewBorder";
            this.listViewData.Location = new System.Drawing.Point(3, 29);
            this.listViewData.MultiSelect = false;
            this.listViewData.Name = "listViewData";
            this.listViewData.ShowItemToolTips = true;
            this.listViewData.Size = new System.Drawing.Size(184, 526);
            this.listViewData.TabIndex = 5;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            this.listViewData.View = System.Windows.Forms.View.SmallIcon;
            this.listViewData.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewData_ItemSelectionChanged);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(0, 0);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(61, 26);
            this.buttonX1.TabIndex = 6;
            this.buttonX1.Text = "浏  览";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(696, 610);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupPanel1.Size = new System.Drawing.Size(77, 81);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 7;
            this.groupPanel1.Text = "选择要上传的数据文件";
            // 
            // MapRangeBox
            // 
            this.MapRangeBox.DisplayMember = "Text";
            this.MapRangeBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.MapRangeBox.FormattingEnabled = true;
            this.MapRangeBox.ItemHeight = 15;
            this.MapRangeBox.Location = new System.Drawing.Point(167, 622);
            this.MapRangeBox.Name = "MapRangeBox";
            this.MapRangeBox.Size = new System.Drawing.Size(31, 21);
            this.MapRangeBox.TabIndex = 8;
            this.MapRangeBox.Visible = false;
            this.MapRangeBox.SelectedValueChanged += new System.EventHandler(this.MapRangeBox_SelectedValueChanged);
            // 
            // ProjectRangeBox
            // 
            this.ProjectRangeBox.DisplayMember = "Text";
            this.ProjectRangeBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ProjectRangeBox.FormattingEnabled = true;
            this.ProjectRangeBox.ItemHeight = 15;
            this.ProjectRangeBox.Location = new System.Drawing.Point(204, 620);
            this.ProjectRangeBox.Name = "ProjectRangeBox";
            this.ProjectRangeBox.Size = new System.Drawing.Size(31, 21);
            this.ProjectRangeBox.TabIndex = 9;
            this.ProjectRangeBox.Visible = false;
            this.ProjectRangeBox.SelectedValueChanged += new System.EventHandler(this.ProjectRangeBox_SelectedValueChanged);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.splitContainer1);
            this.panelEx1.Location = new System.Drawing.Point(0, 12);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(567, 558);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 10;
            this.panelEx1.Text = "panelEx1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonX3);
            this.splitContainer1.Panel1.Controls.Add(this.listViewData);
            this.splitContainer1.Panel1.Controls.Add(this.buttonX1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonX2);
            this.splitContainer1.Panel2.Controls.Add(this.buttonX4);
            this.splitContainer1.Panel2.Controls.Add(this.labelX2);
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewMeta);
            this.splitContainer1.Size = new System.Drawing.Size(567, 558);
            this.splitContainer1.SplitterDistance = 190;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonX3
            // 
            this.buttonX3.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX3.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX3.Location = new System.Drawing.Point(78, 3);
            this.buttonX3.Name = "buttonX3";
            this.buttonX3.Size = new System.Drawing.Size(82, 23);
            this.buttonX3.TabIndex = 11;
            this.buttonX3.Text = "删除选中项";
            this.buttonX3.Click += new System.EventHandler(this.buttonX3_Click);
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX2.Location = new System.Drawing.Point(239, 3);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Size = new System.Drawing.Size(59, 23);
            this.buttonX2.TabIndex = 4;
            this.buttonX2.Text = "设　置";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX4
            // 
            this.buttonX4.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX4.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX4.Location = new System.Drawing.Point(313, 3);
            this.buttonX4.Name = "buttonX4";
            this.buttonX4.Size = new System.Drawing.Size(56, 23);
            this.buttonX4.TabIndex = 3;
            this.buttonX4.Text = "应  用";
            this.buttonX4.Click += new System.EventHandler(this.buttonX4_Click);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(3, 3);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(82, 23);
            this.labelX2.TabIndex = 1;
            this.labelX2.Text = "元信息列表：";
            // 
            // cmbScale
            // 
            this.cmbScale.DisplayMember = "Text";
            this.cmbScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.ItemHeight = 15;
            this.cmbScale.Location = new System.Drawing.Point(275, 625);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(67, 21);
            this.cmbScale.TabIndex = 11;
            this.cmbScale.SelectedValueChanged += new System.EventHandler(this.cmbScale_SelectedValueChanged);
            // 
            // FrmProjectSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 607);
            this.Controls.Add(this.cmbScale);
            this.Controls.Add(this.ProjectRangeBox);
            this.Controls.Add(this.MapRangeBox);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmProjectSetting";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMeta)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewMeta;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ListViewEx listViewData;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx MapRangeBox;
        private DevComponents.DotNetBar.Controls.ComboBoxEx ProjectRangeBox;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX buttonX3;
        private DevComponents.DotNetBar.ButtonX buttonX4;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbScale;
    }
}