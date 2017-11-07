namespace GeoDBATool
{
    partial class FrmInputTempData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cbSelectTempData = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnAddData = new DevComponents.DotNetBar.ButtonX();
            this.btnDelete = new DevComponents.DotNetBar.ButtonX();
            this.btnClear = new DevComponents.DotNetBar.ButtonX();
            this.btnDoInput = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.dgvInputList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rbMDBFile = new System.Windows.Forms.RadioButton();
            this.rbGDBFile = new System.Windows.Forms.RadioButton();
            this.rbShapeFile = new System.Windows.Forms.RadioButton();
            this.ProName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SourceLayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetLayerName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LayerMarker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InputLog = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputList)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(100, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "工程名称";
            // 
            // cbSelectTempData
            // 
            this.cbSelectTempData.DisplayMember = "Text";
            this.cbSelectTempData.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbSelectTempData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectTempData.FormattingEnabled = true;
            this.cbSelectTempData.ItemHeight = 19;
            this.cbSelectTempData.Location = new System.Drawing.Point(12, 41);
            this.cbSelectTempData.Name = "cbSelectTempData";
            this.cbSelectTempData.Size = new System.Drawing.Size(205, 25);
            this.cbSelectTempData.TabIndex = 1;
            this.cbSelectTempData.SelectedIndexChanged += new System.EventHandler(this.cbSelectTempData_SelectedIndexChanged);
            // 
            // btnAddData
            // 
            this.btnAddData.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddData.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddData.Location = new System.Drawing.Point(16, 215);
            this.btnAddData.Name = "btnAddData";
            this.btnAddData.Size = new System.Drawing.Size(99, 33);
            this.btnAddData.TabIndex = 2;
            this.btnAddData.Text = "添加";
            this.btnAddData.Click += new System.EventHandler(this.btnAddData_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDelete.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDelete.Location = new System.Drawing.Point(121, 215);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(99, 33);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnClear.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnClear.Location = new System.Drawing.Point(15, 266);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(99, 33);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDoInput
            // 
            this.btnDoInput.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDoInput.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDoInput.Location = new System.Drawing.Point(120, 266);
            this.btnDoInput.Name = "btnDoInput";
            this.btnDoInput.Size = new System.Drawing.Size(99, 33);
            this.btnDoInput.TabIndex = 7;
            this.btnDoInput.Text = "入库";
            this.btnDoInput.Click += new System.EventHandler(this.btnDoInput_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(785, 356);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(99, 33);
            this.btnExit.TabIndex = 8;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.dgvInputList);
            this.groupBox.Location = new System.Drawing.Point(244, 12);
            this.groupBox.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox.Name = "groupBox";
            this.groupBox.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox.Size = new System.Drawing.Size(639, 337);
            this.groupBox.TabIndex = 9;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "文件列表";
            // 
            // dgvInputList
            // 
            this.dgvInputList.AllowUserToAddRows = false;
            this.dgvInputList.AllowUserToDeleteRows = false;
            this.dgvInputList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvInputList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProName,
            this.SourceLayerName,
            this.TargetLayerName,
            this.LayerMarker,
            this.InputState,
            this.InputLog});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInputList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInputList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInputList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgvInputList.Location = new System.Drawing.Point(4, 22);
            this.dgvInputList.Name = "dgvInputList";
            this.dgvInputList.RowHeadersVisible = false;
            this.dgvInputList.RowTemplate.Height = 27;
            this.dgvInputList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInputList.Size = new System.Drawing.Size(631, 311);
            this.dgvInputList.TabIndex = 0;
            this.dgvInputList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvInputList_DataError);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.rbMDBFile);
            this.groupPanel1.Controls.Add(this.rbGDBFile);
            this.groupPanel1.Controls.Add(this.rbShapeFile);
            this.groupPanel1.Location = new System.Drawing.Point(12, 85);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupPanel1.ShowFocusRectangle = true;
            this.groupPanel1.Size = new System.Drawing.Size(205, 100);
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
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.TabIndex = 10;
            this.groupPanel1.Text = "入库数据类型";
            // 
            // rbMDBFile
            // 
            this.rbMDBFile.AutoSize = true;
            this.rbMDBFile.Location = new System.Drawing.Point(3, 53);
            this.rbMDBFile.Name = "rbMDBFile";
            this.rbMDBFile.Size = new System.Drawing.Size(97, 19);
            this.rbMDBFile.TabIndex = 2;
            this.rbMDBFile.Text = "MDB数据库";
            this.rbMDBFile.UseVisualStyleBackColor = true;
            // 
            // rbGDBFile
            // 
            this.rbGDBFile.AutoSize = true;
            this.rbGDBFile.Location = new System.Drawing.Point(3, 28);
            this.rbGDBFile.Name = "rbGDBFile";
            this.rbGDBFile.Size = new System.Drawing.Size(97, 19);
            this.rbGDBFile.TabIndex = 1;
            this.rbGDBFile.Text = "GDB数据库";
            this.rbGDBFile.UseVisualStyleBackColor = true;
            // 
            // rbShapeFile
            // 
            this.rbShapeFile.AutoSize = true;
            this.rbShapeFile.Checked = true;
            this.rbShapeFile.Location = new System.Drawing.Point(3, 3);
            this.rbShapeFile.Name = "rbShapeFile";
            this.rbShapeFile.Size = new System.Drawing.Size(98, 19);
            this.rbShapeFile.TabIndex = 0;
            this.rbShapeFile.TabStop = true;
            this.rbShapeFile.Text = "Shape文件";
            this.rbShapeFile.UseVisualStyleBackColor = true;
            // 
            // ProName
            // 
            this.ProName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProName.DataPropertyName = "ProName";
            this.ProName.HeaderText = "工程名称";
            this.ProName.Name = "ProName";
            this.ProName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProName.Width = 73;
            // 
            // SourceLayerName
            // 
            this.SourceLayerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SourceLayerName.DataPropertyName = "SourceLayerName";
            this.SourceLayerName.HeaderText = "源图层名称";
            this.SourceLayerName.Name = "SourceLayerName";
            this.SourceLayerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SourceLayerName.Width = 88;
            // 
            // TargetLayerName
            // 
            this.TargetLayerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TargetLayerName.DataPropertyName = "TargetLayerName";
            this.TargetLayerName.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.TargetLayerName.HeaderText = "目标图层名称";
            this.TargetLayerName.MaxDropDownItems = 100;
            this.TargetLayerName.Name = "TargetLayerName";
            this.TargetLayerName.Width = 66;
            // 
            // LayerMarker
            // 
            this.LayerMarker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LayerMarker.DataPropertyName = "LayerMarker";
            this.LayerMarker.HeaderText = "图层描述";
            this.LayerMarker.Name = "LayerMarker";
            this.LayerMarker.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LayerMarker.Width = 52;
            // 
            // InputState
            // 
            this.InputState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.InputState.DataPropertyName = "InputState";
            this.InputState.HeaderText = "入库状态";
            this.InputState.Name = "InputState";
            this.InputState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InputState.Width = 52;
            // 
            // InputLog
            // 
            this.InputLog.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.InputLog.DataPropertyName = "InputLog";
            this.InputLog.HeaderText = "详细日志";
            this.InputLog.Name = "InputLog";
            this.InputLog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InputLog.Width = 52;
            // 
            // FrmInputTempData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 401);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupBox);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDoInput);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAddData);
            this.Controls.Add(this.cbSelectTempData);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInputTempData";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "临时库入库";
            this.Load += new System.EventHandler(this.FrmInputTempData_Load);
            this.groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputList)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbSelectTempData;
        private DevComponents.DotNetBar.ButtonX btnAddData;
        private DevComponents.DotNetBar.ButtonX btnDelete;
        private DevComponents.DotNetBar.ButtonX btnClear;
        private DevComponents.DotNetBar.ButtonX btnDoInput;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private System.Windows.Forms.GroupBox groupBox;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgvInputList;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.RadioButton rbMDBFile;
        private System.Windows.Forms.RadioButton rbGDBFile;
        private System.Windows.Forms.RadioButton rbShapeFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SourceLayerName;
        private System.Windows.Forms.DataGridViewComboBoxColumn TargetLayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LayerMarker;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputState;
        private System.Windows.Forms.DataGridViewTextBoxColumn InputLog;
    }
}