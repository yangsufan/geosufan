namespace GeoDBATool
{
    partial class FrmQueryAll
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.cmbRange = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.cmbMatch = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmbField = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.dgResult = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.btnStop = new DevComponents.DotNetBar.ButtonX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.txtValue = new DevComponents.DotNetBar.Controls.TextBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.dgResult)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(3, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "检 索 值:";
            // 
            // cmbRange
            // 
            this.cmbRange.DisplayMember = "Text";
            this.cmbRange.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbRange.FormattingEnabled = true;
            this.cmbRange.ItemHeight = 15;
            this.cmbRange.Location = new System.Drawing.Point(79, 43);
            this.cmbRange.Name = "cmbRange";
            this.cmbRange.Size = new System.Drawing.Size(475, 21);
            this.cmbRange.TabIndex = 3;
            this.cmbRange.SelectedIndexChanged += new System.EventHandler(this.cmbRange_SelectedIndexChanged);
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(3, 41);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "检索范围:";
            // 
            // cmbMatch
            // 
            this.cmbMatch.DisplayMember = "Text";
            this.cmbMatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMatch.FormattingEnabled = true;
            this.cmbMatch.ItemHeight = 15;
            this.cmbMatch.Location = new System.Drawing.Point(79, 72);
            this.cmbMatch.Name = "cmbMatch";
            this.cmbMatch.Size = new System.Drawing.Size(475, 21);
            this.cmbMatch.TabIndex = 5;
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(3, 70);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "匹配方式:";
            // 
            // cmbField
            // 
            this.cmbField.DisplayMember = "Text";
            this.cmbField.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbField.FormattingEnabled = true;
            this.cmbField.ItemHeight = 15;
            this.cmbField.Location = new System.Drawing.Point(79, 101);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(475, 21);
            this.cmbField.TabIndex = 7;
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(3, 99);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "检索字段:";
            // 
            // dgResult
            // 
            this.dgResult.AllowUserToAddRows = false;
            this.dgResult.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgResult.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgResult.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgResult.Location = new System.Drawing.Point(3, 128);
            this.dgResult.Name = "dgResult";
            this.dgResult.RowTemplate.Height = 23;
            this.dgResult.Size = new System.Drawing.Size(551, 315);
            this.dgResult.TabIndex = 8;
            this.dgResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgResult_MouseDoubleClick);
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSearch.Location = new System.Drawing.Point(434, 449);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(55, 23);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "搜 索";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnStop
            // 
            this.btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStop.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStop.Location = new System.Drawing.Point(701, 514);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(56, 23);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "停 止";
            // 
            // labelX5
            // 
            this.labelX5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX5.Location = new System.Drawing.Point(12, 449);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(274, 23);
            this.labelX5.TabIndex = 11;
            this.labelX5.Text = "...";
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(495, 449);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(55, 23);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "退 出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtValue
            // 
            // 
            // 
            // 
            this.txtValue.Border.Class = "TextBoxBorder";
            this.txtValue.Location = new System.Drawing.Point(79, 16);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(475, 21);
            this.txtValue.TabIndex = 13;
            // 
            // FrmQueryAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 476);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dgResult);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.cmbMatch);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cmbRange);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmQueryAll";
            this.ShowIcon = false;
            this.Text = "全文检索";
            ((System.ComponentModel.ISupportInitialize)(this.dgResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbRange;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbMatch;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbField;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgResult;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private DevComponents.DotNetBar.ButtonX btnStop;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.TextBoxX txtValue;
    }
}