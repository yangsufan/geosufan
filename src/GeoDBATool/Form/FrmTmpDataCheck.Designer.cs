namespace GeoDBATool
{
    partial class FrmTmpDataCheck
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.rdbSheng = new System.Windows.Forms.RadioButton();
            this.rdbShi = new System.Windows.Forms.RadioButton();
            this.rdbXian = new System.Windows.Forms.RadioButton();
            this.cmbLayer = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.cmbTmpDB = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dGridCheckRes = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnCheck = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.lblTips = new DevComponents.DotNetBar.LabelX();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.ColRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCheckState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPanel4.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGridCheckRes)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel4
            // 
            this.groupPanel4.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.rdbSheng);
            this.groupPanel4.Controls.Add(this.rdbShi);
            this.groupPanel4.Controls.Add(this.rdbXian);
            this.groupPanel4.Controls.Add(this.cmbLayer);
            this.groupPanel4.Controls.Add(this.labelX9);
            this.groupPanel4.Controls.Add(this.cmbTmpDB);
            this.groupPanel4.Controls.Add(this.labelX8);
            this.groupPanel4.Controls.Add(this.labelX7);
            this.groupPanel4.Location = new System.Drawing.Point(6, 7);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(366, 64);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel4.TabIndex = 62;
            // 
            // rdbSheng
            // 
            this.rdbSheng.AutoSize = true;
            this.rdbSheng.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbSheng.Location = new System.Drawing.Point(88, 39);
            this.rdbSheng.Name = "rdbSheng";
            this.rdbSheng.Size = new System.Drawing.Size(35, 16);
            this.rdbSheng.TabIndex = 68;
            this.rdbSheng.Text = "省";
            this.rdbSheng.UseVisualStyleBackColor = true;
            this.rdbSheng.CheckedChanged += new System.EventHandler(this.rdbRegion_CheckedChanged);
            // 
            // rdbShi
            // 
            this.rdbShi.AutoSize = true;
            this.rdbShi.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbShi.Location = new System.Drawing.Point(163, 39);
            this.rdbShi.Name = "rdbShi";
            this.rdbShi.Size = new System.Drawing.Size(35, 16);
            this.rdbShi.TabIndex = 67;
            this.rdbShi.Text = "市";
            this.rdbShi.UseVisualStyleBackColor = true;
            this.rdbShi.CheckedChanged += new System.EventHandler(this.rdbRegion_CheckedChanged);
            // 
            // rdbXian
            // 
            this.rdbXian.AutoSize = true;
            this.rdbXian.Checked = true;
            this.rdbXian.ForeColor = System.Drawing.Color.MidnightBlue;
            this.rdbXian.Location = new System.Drawing.Point(230, 39);
            this.rdbXian.Name = "rdbXian";
            this.rdbXian.Size = new System.Drawing.Size(35, 16);
            this.rdbXian.TabIndex = 66;
            this.rdbXian.TabStop = true;
            this.rdbXian.Text = "县";
            this.rdbXian.UseVisualStyleBackColor = true;
            this.rdbXian.CheckedChanged += new System.EventHandler(this.rdbRegion_CheckedChanged);
            // 
            // cmbLayer
            // 
            this.cmbLayer.DisplayMember = "Text";
            this.cmbLayer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbLayer.FormattingEnabled = true;
            this.cmbLayer.ItemHeight = 15;
            this.cmbLayer.Location = new System.Drawing.Point(230, 10);
            this.cmbLayer.Name = "cmbLayer";
            this.cmbLayer.Size = new System.Drawing.Size(130, 21);
            this.cmbLayer.TabIndex = 65;
            this.cmbLayer.SelectedIndexChanged += new System.EventHandler(this.cmbLayer_SelectedIndexChanged);
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(192, 12);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(44, 18);
            this.labelX9.TabIndex = 64;
            this.labelX9.Text = "图层：";
            // 
            // cmbTmpDB
            // 
            this.cmbTmpDB.DisplayMember = "Text";
            this.cmbTmpDB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTmpDB.FormattingEnabled = true;
            this.cmbTmpDB.ItemHeight = 15;
            this.cmbTmpDB.Location = new System.Drawing.Point(57, 10);
            this.cmbTmpDB.Name = "cmbTmpDB";
            this.cmbTmpDB.Size = new System.Drawing.Size(130, 21);
            this.cmbTmpDB.TabIndex = 65;
            this.cmbTmpDB.SelectedIndexChanged += new System.EventHandler(this.cmbTmpDB_SelectedIndexChanged);
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(7, 38);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(67, 21);
            this.labelX8.TabIndex = 0;
            this.labelX8.Text = "审核单位：";
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(7, 12);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(66, 18);
            this.labelX7.TabIndex = 64;
            this.labelX7.Text = "临时库：";
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.dGridCheckRes);
            this.groupPanel1.Location = new System.Drawing.Point(6, 75);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(366, 184);
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
            this.groupPanel1.TabIndex = 62;
            // 
            // dGridCheckRes
            // 
            this.dGridCheckRes.AllowUserToAddRows = false;
            this.dGridCheckRes.AllowUserToOrderColumns = true;
            this.dGridCheckRes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGridCheckRes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColRegion,
            this.ColCheckState});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGridCheckRes.DefaultCellStyle = dataGridViewCellStyle3;
            this.dGridCheckRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGridCheckRes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dGridCheckRes.Location = new System.Drawing.Point(0, 0);
            this.dGridCheckRes.Name = "dGridCheckRes";
            this.dGridCheckRes.RowTemplate.Height = 23;
            this.dGridCheckRes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGridCheckRes.Size = new System.Drawing.Size(364, 182);
            this.dGridCheckRes.TabIndex = 1;
            // 
            // btnCheck
            // 
            this.btnCheck.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCheck.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCheck.Location = new System.Drawing.Point(230, 275);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(67, 23);
            this.btnCheck.TabIndex = 63;
            this.btnCheck.Text = "审核通过";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(305, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 23);
            this.btnCancel.TabIndex = 63;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblTips
            // 
            this.lblTips.Location = new System.Drawing.Point(7, 274);
            this.lblTips.Name = "lblTips";
            this.lblTips.Size = new System.Drawing.Size(218, 22);
            this.lblTips.TabIndex = 68;
            // 
            // progressBarX1
            // 
            this.progressBarX1.Location = new System.Drawing.Point(5, 260);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.Size = new System.Drawing.Size(366, 10);
            this.progressBarX1.TabIndex = 67;
            this.progressBarX1.Text = "proBar";
            this.progressBarX1.Visible = false;
            // 
            // ColRegion
            // 
            this.ColRegion.HeaderText = "行政区";
            this.ColRegion.Name = "ColRegion";
            this.ColRegion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColRegion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColRegion.Width = 160;
            // 
            // ColCheckState
            // 
            this.ColCheckState.HeaderText = "审核状态";
            this.ColCheckState.Name = "ColCheckState";
            this.ColCheckState.Width = 160;
            // 
            // FrmTmpDataCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 302);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel4);
            this.Controls.Add(this.lblTips);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTmpDataCheck";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "临时库审核";
            this.Load += new System.EventHandler(this.FrmTmpDataCheck_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTmpDataCheck_FormClosed);
            this.groupPanel4.ResumeLayout(false);
            this.groupPanel4.PerformLayout();
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGridCheckRes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbLayer;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTmpDB;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private System.Windows.Forms.RadioButton rdbSheng;
        private System.Windows.Forms.RadioButton rdbShi;
        private System.Windows.Forms.RadioButton rdbXian;
        private DevComponents.DotNetBar.ButtonX btnCheck;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        public DevComponents.DotNetBar.Controls.DataGridViewX dGridCheckRes;
        private DevComponents.DotNetBar.LabelX lblTips;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRegion;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCheckState;

    }
}