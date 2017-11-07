namespace GeoDataManagerFrame
{
    partial class frmFeatureClassMetaInput
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.btnUpdata = new DevComponents.DotNetBar.ButtonX();
            this.dataGrid = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOtherSelected = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnAllSelected = new DevComponents.DotNetBar.ButtonX();
            this.bttAllRemove = new DevComponents.DotNetBar.ButtonX();
            this.bttRemove = new DevComponents.DotNetBar.ButtonX();
            this.bttOpenFolder = new DevComponents.DotNetBar.ButtonX();
            this.bttOpenFile = new DevComponents.DotNetBar.ButtonX();
            this.CmnSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CmnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CmnFeatureClassName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CmnState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.btnUpdata);
            this.groupPanel1.Controls.Add(this.dataGrid);
            this.groupPanel1.Controls.Add(this.btnCancel);
            this.groupPanel1.Controls.Add(this.btnOtherSelected);
            this.groupPanel1.Controls.Add(this.btnOK);
            this.groupPanel1.Controls.Add(this.btnAllSelected);
            this.groupPanel1.Controls.Add(this.bttAllRemove);
            this.groupPanel1.Controls.Add(this.bttRemove);
            this.groupPanel1.Controls.Add(this.bttOpenFolder);
            this.groupPanel1.Controls.Add(this.bttOpenFile);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 0);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(579, 453);
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
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 2;
            this.groupPanel1.TitleImagePosition = DevComponents.DotNetBar.eTitleImagePosition.Center;
            // 
            // btnUpdata
            // 
            this.btnUpdata.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUpdata.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUpdata.Location = new System.Drawing.Point(499, 314);
            this.btnUpdata.Name = "btnUpdata";
            this.btnUpdata.Size = new System.Drawing.Size(65, 23);
            this.btnUpdata.TabIndex = 31;
            this.btnUpdata.Text = "更新";
            this.btnUpdata.Click += new System.EventHandler(this.btnUpdata_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CmnSelect,
            this.CmnPath,
            this.CmnFeatureClassName,
            this.CmnState});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGrid.Location = new System.Drawing.Point(3, 3);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowTemplate.Height = 23;
            this.dataGrid.Size = new System.Drawing.Size(486, 441);
            this.dataGrid.TabIndex = 30;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(499, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOtherSelected
            // 
            this.btnOtherSelected.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOtherSelected.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOtherSelected.Location = new System.Drawing.Point(499, 238);
            this.btnOtherSelected.Name = "btnOtherSelected";
            this.btnOtherSelected.Size = new System.Drawing.Size(65, 23);
            this.btnOtherSelected.TabIndex = 26;
            this.btnOtherSelected.Text = "反选";
            this.btnOtherSelected.Click += new System.EventHandler(this.btnOtherSelected_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(499, 285);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "上传";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAllSelected
            // 
            this.btnAllSelected.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAllSelected.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAllSelected.Location = new System.Drawing.Point(499, 209);
            this.btnAllSelected.Name = "btnAllSelected";
            this.btnAllSelected.Size = new System.Drawing.Size(65, 23);
            this.btnAllSelected.TabIndex = 25;
            this.btnAllSelected.Text = "全选";
            this.btnAllSelected.Click += new System.EventHandler(this.btnAllSelected_Click);
            // 
            // bttAllRemove
            // 
            this.bttAllRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttAllRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttAllRemove.Location = new System.Drawing.Point(499, 159);
            this.bttAllRemove.Name = "bttAllRemove";
            this.bttAllRemove.Size = new System.Drawing.Size(65, 23);
            this.bttAllRemove.TabIndex = 24;
            this.bttAllRemove.Text = "全部移除";
            this.bttAllRemove.Click += new System.EventHandler(this.bttAllRemove_Click);
            // 
            // bttRemove
            // 
            this.bttRemove.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttRemove.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttRemove.Location = new System.Drawing.Point(499, 130);
            this.bttRemove.Name = "bttRemove";
            this.bttRemove.Size = new System.Drawing.Size(65, 23);
            this.bttRemove.TabIndex = 23;
            this.bttRemove.Text = "移除";
            this.bttRemove.Click += new System.EventHandler(this.bttRemove_Click);
            // 
            // bttOpenFolder
            // 
            this.bttOpenFolder.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpenFolder.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpenFolder.Location = new System.Drawing.Point(499, 82);
            this.bttOpenFolder.Name = "bttOpenFolder";
            this.bttOpenFolder.Size = new System.Drawing.Size(65, 23);
            this.bttOpenFolder.TabIndex = 22;
            this.bttOpenFolder.Text = "文件夹";
            this.bttOpenFolder.Click += new System.EventHandler(this.bttOpenFolder_Click);
            // 
            // bttOpenFile
            // 
            this.bttOpenFile.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttOpenFile.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttOpenFile.Location = new System.Drawing.Point(500, 53);
            this.bttOpenFile.Name = "bttOpenFile";
            this.bttOpenFile.Size = new System.Drawing.Size(65, 23);
            this.bttOpenFile.TabIndex = 21;
            this.bttOpenFile.Text = "文件";
            this.bttOpenFile.Click += new System.EventHandler(this.bttOpenFile_Click);
            // 
            // CmnSelect
            // 
            this.CmnSelect.HeaderText = "选择";
            this.CmnSelect.Name = "CmnSelect";
            this.CmnSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CmnSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CmnSelect.Width = 55;
            // 
            // CmnPath
            // 
            this.CmnPath.HeaderText = "数据库元数据信息";
            this.CmnPath.Name = "CmnPath";
            this.CmnPath.ReadOnly = true;
            this.CmnPath.Width = 180;
            // 
            // CmnFeatureClassName
            // 
            this.CmnFeatureClassName.HeaderText = "数据库名称";
            this.CmnFeatureClassName.Name = "CmnFeatureClassName";
            // 
            // CmnState
            // 
            this.CmnState.HeaderText = "状态";
            this.CmnState.Name = "CmnState";
            this.CmnState.ReadOnly = true;
            this.CmnState.Width = 105;
            // 
            // frmFeatureClassMetaInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 453);
            this.Controls.Add(this.groupPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFeatureClassMetaInput";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库元数据信息录入";
            this.Load += new System.EventHandler(this.frmFeatureClassMetaInput_Load);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGrid;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOtherSelected;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnAllSelected;
        private DevComponents.DotNetBar.ButtonX bttAllRemove;
        private DevComponents.DotNetBar.ButtonX bttRemove;
        private DevComponents.DotNetBar.ButtonX bttOpenFolder;
        private DevComponents.DotNetBar.ButtonX bttOpenFile;
        private DevComponents.DotNetBar.ButtonX btnUpdata;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CmnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn CmnPath;
        private System.Windows.Forms.DataGridViewComboBoxColumn CmnFeatureClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CmnState;
    }
}