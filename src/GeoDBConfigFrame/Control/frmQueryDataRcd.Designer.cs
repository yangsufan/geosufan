namespace GeoDBConfigFrame
{
    partial class frmQueryDataRcd
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.bttQuery = new DevComponents.DotNetBar.ButtonX();
            this.txtKeys = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labQuery = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.dataGridVRe = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.CumTable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CumName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupPanel2.SuspendLayout();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVRe)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.bttQuery);
            this.groupPanel2.Controls.Add(this.txtKeys);
            this.groupPanel2.Controls.Add(this.labQuery);
            this.groupPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(0, 0);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(503, 52);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
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
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel2.TabIndex = 4;
            // 
            // bttQuery
            // 
            this.bttQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bttQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bttQuery.Location = new System.Drawing.Point(376, 10);
            this.bttQuery.Name = "bttQuery";
            this.bttQuery.Size = new System.Drawing.Size(70, 24);
            this.bttQuery.TabIndex = 6;
            this.bttQuery.Text = "检索";
            this.bttQuery.Click += new System.EventHandler(this.bttQuery_Click);
            // 
            // txtKeys
            // 
            // 
            // 
            // 
            this.txtKeys.Border.Class = "TextBoxBorder";
            this.txtKeys.Location = new System.Drawing.Point(101, 10);
            this.txtKeys.Name = "txtKeys";
            this.txtKeys.Size = new System.Drawing.Size(249, 21);
            this.txtKeys.TabIndex = 3;
            this.txtKeys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKeys_KeyDown);
            // 
            // labQuery
            // 
            this.labQuery.BackColor = System.Drawing.Color.Transparent;
            this.labQuery.Location = new System.Drawing.Point(38, 10);
            this.labQuery.Name = "labQuery";
            this.labQuery.Size = new System.Drawing.Size(72, 23);
            this.labQuery.TabIndex = 0;
            this.labQuery.Text = "关键字：";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.dataGridVRe);
            this.groupPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupPanel1.DrawTitleBox = false;
            this.groupPanel1.Location = new System.Drawing.Point(0, 52);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(503, 337);
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
            this.groupPanel1.TabIndex = 5;
            this.groupPanel1.Text = "检索结果";
            // 
            // dataGridVRe
            // 
            this.dataGridVRe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridVRe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CumTable,
            this.CumCODE,
            this.CumName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridVRe.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridVRe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridVRe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridVRe.Location = new System.Drawing.Point(0, 0);
            this.dataGridVRe.Name = "dataGridVRe";
            this.dataGridVRe.ReadOnly = true;
            this.dataGridVRe.RowTemplate.Height = 23;
            this.dataGridVRe.Size = new System.Drawing.Size(497, 313);
            this.dataGridVRe.TabIndex = 2;
            // 
            // CumTable
            // 
            this.CumTable.HeaderText = "所在表名称";
            this.CumTable.Name = "CumTable";
            this.CumTable.ReadOnly = true;
            this.CumTable.Width = 150;
            // 
            // CumCODE
            // 
            this.CumCODE.HeaderText = "编码";
            this.CumCODE.Name = "CumCODE";
            this.CumCODE.ReadOnly = true;
            this.CumCODE.Width = 150;
            // 
            // CumName
            // 
            this.CumName.HeaderText = "名称";
            this.CumName.Name = "CumName";
            this.CumName.ReadOnly = true;
            this.CumName.Width = 150;
            // 
            // frmQueryDataRcd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 389);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel2);
            this.MaximizeBox = false;
            this.Name = "frmQueryDataRcd";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "检索记录";
            this.groupPanel2.ResumeLayout(false);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVRe)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX bttQuery;
        private DevComponents.DotNetBar.Controls.TextBoxX txtKeys;
        private DevComponents.DotNetBar.LabelX labQuery;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridVRe;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CumName;
    }
}