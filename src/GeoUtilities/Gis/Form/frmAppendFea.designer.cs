
namespace GeoUtilities
{
    partial class frmAppendFea
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.sddssfdffds = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnGetNames = new DevComponents.DotNetBar.ButtonX();
            this.btnNoSel = new DevComponents.DotNetBar.ButtonX();
            this.btnSelAll = new DevComponents.DotNetBar.ButtonX();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colCheckedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDatasetNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAliasNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appendFeaTBBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dataBaseSets = new GeoUtilities.Resources.DataBaseSets();
            this.cmdLog = new DevComponents.DotNetBar.ButtonX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.buttonXCancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonXOK = new DevComponents.DotNetBar.ButtonX();
            this.pbCopyRows = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.ucDataconnectTag = new SysCommon.Gis.UIDataConnect();
            this.ucDataConnectSource = new SysCommon.Gis.UIDataConnect();
            this.appendFeaTBBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sddssfdffds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appendFeaTBBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBaseSets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appendFeaTBBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // sddssfdffds
            // 
            this.sddssfdffds.CanvasColor = System.Drawing.SystemColors.Control;
            this.sddssfdffds.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.sddssfdffds.Controls.Add(this.btnGetNames);
            this.sddssfdffds.Controls.Add(this.btnNoSel);
            this.sddssfdffds.Controls.Add(this.btnSelAll);
            this.sddssfdffds.Controls.Add(this.dataGridViewX1);
            this.sddssfdffds.DrawTitleBox = false;
            this.sddssfdffds.Location = new System.Drawing.Point(12, 262);
            this.sddssfdffds.Name = "sddssfdffds";
            this.sddssfdffds.Size = new System.Drawing.Size(519, 191);
            // 
            // 
            // 
            this.sddssfdffds.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.sddssfdffds.Style.BackColorGradientAngle = 90;
            this.sddssfdffds.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.sddssfdffds.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.sddssfdffds.Style.BorderBottomWidth = 1;
            this.sddssfdffds.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.sddssfdffds.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.sddssfdffds.Style.BorderLeftWidth = 1;
            this.sddssfdffds.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.sddssfdffds.Style.BorderRightWidth = 1;
            this.sddssfdffds.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.sddssfdffds.Style.BorderTopWidth = 1;
            this.sddssfdffds.Style.CornerDiameter = 4;
            this.sddssfdffds.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.sddssfdffds.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.sddssfdffds.TabIndex = 29;
            this.sddssfdffds.Text = "指定备份数据";
            // 
            // btnGetNames
            // 
            this.btnGetNames.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnGetNames.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnGetNames.Location = new System.Drawing.Point(435, 140);
            this.btnGetNames.Name = "btnGetNames";
            this.btnGetNames.Size = new System.Drawing.Size(75, 23);
            this.btnGetNames.TabIndex = 3;
            this.btnGetNames.Text = "获取数据";
            this.btnGetNames.Click += new System.EventHandler(this.btnGetNames_Click);
            // 
            // btnNoSel
            // 
            this.btnNoSel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnNoSel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnNoSel.Location = new System.Drawing.Point(69, 141);
            this.btnNoSel.Name = "btnNoSel";
            this.btnNoSel.Size = new System.Drawing.Size(46, 23);
            this.btnNoSel.TabIndex = 2;
            this.btnNoSel.Text = "反选";
            this.btnNoSel.Click += new System.EventHandler(this.btnNoSel_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSelAll.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSelAll.Location = new System.Drawing.Point(17, 141);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(46, 23);
            this.btnSelAll.TabIndex = 1;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.AutoGenerateColumns = false;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckedDataGridViewCheckBoxColumn,
            this.colDatasetNameDataGridViewTextBoxColumn,
            this.colAliasNameDataGridViewTextBoxColumn,
            this.colDataTypeDataGridViewTextBoxColumn});
            this.dataGridViewX1.DataSource = this.appendFeaTBBindingSource1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(17, 3);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.ShowEditingIcon = false;
            this.dataGridViewX1.Size = new System.Drawing.Size(493, 131);
            this.dataGridViewX1.TabIndex = 0;
            // 
            // colCheckedDataGridViewCheckBoxColumn
            // 
            this.colCheckedDataGridViewCheckBoxColumn.DataPropertyName = "colChecked";
            this.colCheckedDataGridViewCheckBoxColumn.FillWeight = 50F;
            this.colCheckedDataGridViewCheckBoxColumn.HeaderText = "选择";
            this.colCheckedDataGridViewCheckBoxColumn.Name = "colCheckedDataGridViewCheckBoxColumn";
            this.colCheckedDataGridViewCheckBoxColumn.Width = 50;
            // 
            // colDatasetNameDataGridViewTextBoxColumn
            // 
            this.colDatasetNameDataGridViewTextBoxColumn.DataPropertyName = "colDatasetName";
            this.colDatasetNameDataGridViewTextBoxColumn.HeaderText = "名称";
            this.colDatasetNameDataGridViewTextBoxColumn.Name = "colDatasetNameDataGridViewTextBoxColumn";
            this.colDatasetNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.colDatasetNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // colAliasNameDataGridViewTextBoxColumn
            // 
            this.colAliasNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAliasNameDataGridViewTextBoxColumn.DataPropertyName = "colAliasName";
            this.colAliasNameDataGridViewTextBoxColumn.HeaderText = "别名";
            this.colAliasNameDataGridViewTextBoxColumn.Name = "colAliasNameDataGridViewTextBoxColumn";
            this.colAliasNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // colDataTypeDataGridViewTextBoxColumn
            // 
            this.colDataTypeDataGridViewTextBoxColumn.DataPropertyName = "colDataType";
            this.colDataTypeDataGridViewTextBoxColumn.HeaderText = "类型";
            this.colDataTypeDataGridViewTextBoxColumn.Name = "colDataTypeDataGridViewTextBoxColumn";
            this.colDataTypeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // appendFeaTBBindingSource1
            // 
            this.appendFeaTBBindingSource1.DataMember = "AppendFeaTB";
            this.appendFeaTBBindingSource1.DataSource = this.dataBaseSets;
            // 
            // dataBaseSets
            // 
            this.dataBaseSets.DataSetName = "DataBaseSets";
            this.dataBaseSets.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cmdLog
            // 
            this.cmdLog.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmdLog.Location = new System.Drawing.Point(294, 459);
            this.cmdLog.Name = "cmdLog";
            this.cmdLog.Size = new System.Drawing.Size(75, 23);
            this.cmdLog.TabIndex = 35;
            this.cmdLog.Text = "查看日志";
            this.cmdLog.Click += new System.EventHandler(this.cmdLog_Click);
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(11, 459);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(277, 23);
            this.lblTips.TabIndex = 34;
            // 
            // buttonXCancel
            // 
            this.buttonXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCancel.Location = new System.Drawing.Point(456, 459);
            this.buttonXCancel.Name = "buttonXCancel";
            this.buttonXCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonXCancel.TabIndex = 33;
            this.buttonXCancel.Text = "取 消";
            this.buttonXCancel.Click += new System.EventHandler(this.buttonXCancel_Click);
            // 
            // buttonXOK
            // 
            this.buttonXOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOK.Location = new System.Drawing.Point(375, 459);
            this.buttonXOK.Name = "buttonXOK";
            this.buttonXOK.Size = new System.Drawing.Size(75, 23);
            this.buttonXOK.TabIndex = 32;
            this.buttonXOK.Text = "确 定";
            this.buttonXOK.Click += new System.EventHandler(this.buttonXOK_Click);
            // 
            // pbCopyRows
            // 
            this.pbCopyRows.Location = new System.Drawing.Point(-1, 485);
            this.pbCopyRows.Name = "pbCopyRows";
            this.pbCopyRows.Size = new System.Drawing.Size(547, 14);
            this.pbCopyRows.TabIndex = 31;
            this.pbCopyRows.Text = "pbCopyRows";
            // 
            // ucDataconnectTag
            // 
            this.ucDataconnectTag.DataBase = "";
            this.ucDataconnectTag.DatabaseType = "ORACLE";
            this.ucDataconnectTag.Location = new System.Drawing.Point(277, 3);
            this.ucDataconnectTag.Name = "ucDataconnectTag";
            this.ucDataconnectTag.Password = "";
            this.ucDataconnectTag.Server = "";
            this.ucDataconnectTag.Service = "";
            this.ucDataconnectTag.Size = new System.Drawing.Size(253, 253);
            this.ucDataconnectTag.strTitle = "目标数据连接";
            this.ucDataconnectTag.TabIndex = 3;
            this.ucDataconnectTag.User = "";
            this.ucDataconnectTag.Version = "SDE.DEFAULT";
            // 
            // ucDataConnectSource
            // 
            this.ucDataConnectSource.DataBase = "";
            this.ucDataConnectSource.DatabaseType = "ORACLE";
            this.ucDataConnectSource.Location = new System.Drawing.Point(12, 3);
            this.ucDataConnectSource.Name = "ucDataConnectSource";
            this.ucDataConnectSource.Password = "";
            this.ucDataConnectSource.Server = "";
            this.ucDataConnectSource.Service = "";
            this.ucDataConnectSource.Size = new System.Drawing.Size(260, 253);
            this.ucDataConnectSource.strTitle = "源数据连接";
            this.ucDataConnectSource.TabIndex = 2;
            this.ucDataConnectSource.User = "";
            this.ucDataConnectSource.Version = "SDE.DEFAULT";
            // 
            // appendFeaTBBindingSource
            // 
            this.appendFeaTBBindingSource.DataMember = "AppendFeaTB";
            this.appendFeaTBBindingSource.DataSource = this.dataBaseSets;
            // 
            // frmAppendFea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 494);
            this.Controls.Add(this.cmdLog);
            this.Controls.Add(this.lblTips);
            this.Controls.Add(this.buttonXCancel);
            this.Controls.Add(this.buttonXOK);
            this.Controls.Add(this.pbCopyRows);
            this.Controls.Add(this.sddssfdffds);
            this.Controls.Add(this.ucDataconnectTag);
            this.Controls.Add(this.ucDataConnectSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAppendFea";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据备份";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAppendFea_FormClosing);
            this.sddssfdffds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appendFeaTBBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataBaseSets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appendFeaTBBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SysCommon.Gis.UIDataConnect ucDataconnectTag;
        private SysCommon.Gis.UIDataConnect ucDataConnectSource;
        private DevComponents.DotNetBar.Controls.GroupPanel sddssfdffds;
        private DevComponents.DotNetBar.ButtonX cmdLog;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.ButtonX buttonXCancel;
        private DevComponents.DotNetBar.ButtonX buttonXOK;
        private DevComponents.DotNetBar.Controls.ProgressBarX pbCopyRows;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.BindingSource appendFeaTBBindingSource;
        private GeoUtilities.Resources.DataBaseSets dataBaseSets;
        private System.Windows.Forms.BindingSource appendFeaTBBindingSource1;
        private DevComponents.DotNetBar.ButtonX btnNoSel;
        private DevComponents.DotNetBar.ButtonX btnSelAll;
        private DevComponents.DotNetBar.ButtonX btnGetNames;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDatasetNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAliasNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataTypeDataGridViewTextBoxColumn;
    }
}