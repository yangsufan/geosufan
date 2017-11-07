namespace FileDBTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmQueryAll));
            this.cmbScale = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.cmbTime = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.dgResult = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.txtMapNO = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rdInputRange = new System.Windows.Forms.RadioButton();
            this.txtInputRange = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.rbDrawRange = new System.Windows.Forms.RadioButton();
            this.btnInputRange = new DevComponents.DotNetBar.ButtonX();
            this.btnDrawRange = new DevComponents.DotNetBar.ButtonX();
            this.btnSearch = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgResult)).BeginInit();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbScale
            // 
            this.cmbScale.DisplayMember = "Text";
            this.cmbScale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbScale.FormattingEnabled = true;
            this.cmbScale.ItemHeight = 15;
            this.cmbScale.Location = new System.Drawing.Point(92, 59);
            this.cmbScale.Name = "cmbScale";
            this.cmbScale.Size = new System.Drawing.Size(365, 21);
            this.cmbScale.TabIndex = 5;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(21, 57);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(73, 23);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "比 例 尺：";
            // 
            // cmbTime
            // 
            this.cmbTime.DisplayMember = "Text";
            this.cmbTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTime.FormattingEnabled = true;
            this.cmbTime.ItemHeight = 15;
            this.cmbTime.Location = new System.Drawing.Point(92, 88);
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.Size = new System.Drawing.Size(315, 21);
            this.cmbTime.TabIndex = 7;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(21, 86);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 6;
            this.labelX4.Text = "时    间：";
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
            this.dgResult.Location = new System.Drawing.Point(92, 115);
            this.dgResult.Name = "dgResult";
            this.dgResult.RowTemplate.Height = 23;
            this.dgResult.Size = new System.Drawing.Size(365, 233);
            this.dgResult.TabIndex = 8;
            this.dgResult.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgResult_MouseDoubleClick);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(342, 444);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(55, 23);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "确 定";
            this.btnOK.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(408, 444);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(55, 23);
            this.btnCancle.TabIndex = 12;
            this.btnCancle.Text = "取　消";
            this.btnCancle.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // txtMapNO
            // 
            // 
            // 
            // 
            this.txtMapNO.Border.Class = "TextBoxBorder";
            this.txtMapNO.Location = new System.Drawing.Point(92, 3);
            this.txtMapNO.Name = "txtMapNO";
            this.txtMapNO.Size = new System.Drawing.Size(365, 21);
            this.txtMapNO.TabIndex = 13;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.BackColor = System.Drawing.Color.Transparent;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(83, 16);
            this.radioButton1.TabIndex = 14;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "关 键 字：";
            this.radioButton1.UseVisualStyleBackColor = false;
            // 
            // rdInputRange
            // 
            this.rdInputRange.AutoSize = true;
            this.rdInputRange.BackColor = System.Drawing.Color.Transparent;
            this.rdInputRange.Location = new System.Drawing.Point(3, 34);
            this.rdInputRange.Name = "rdInputRange";
            this.rdInputRange.Size = new System.Drawing.Size(83, 16);
            this.rdInputRange.TabIndex = 15;
            this.rdInputRange.TabStop = true;
            this.rdInputRange.Text = "导入范围：";
            this.rdInputRange.UseVisualStyleBackColor = false;
            // 
            // txtInputRange
            // 
            // 
            // 
            // 
            this.txtInputRange.Border.Class = "TextBoxBorder";
            this.txtInputRange.Location = new System.Drawing.Point(92, 30);
            this.txtInputRange.Name = "txtInputRange";
            this.txtInputRange.Size = new System.Drawing.Size(157, 21);
            this.txtInputRange.TabIndex = 16;
            // 
            // rbDrawRange
            // 
            this.rbDrawRange.AutoSize = true;
            this.rbDrawRange.BackColor = System.Drawing.Color.Transparent;
            this.rbDrawRange.Location = new System.Drawing.Point(320, 34);
            this.rbDrawRange.Name = "rbDrawRange";
            this.rbDrawRange.Size = new System.Drawing.Size(71, 16);
            this.rbDrawRange.TabIndex = 17;
            this.rbDrawRange.TabStop = true;
            this.rbDrawRange.Text = "任意范围";
            this.rbDrawRange.UseVisualStyleBackColor = false;
            this.rbDrawRange.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // btnInputRange
            // 
            this.btnInputRange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnInputRange.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnInputRange.Location = new System.Drawing.Point(255, 28);
            this.btnInputRange.Name = "btnInputRange";
            this.btnInputRange.Size = new System.Drawing.Size(40, 23);
            this.btnInputRange.TabIndex = 19;
            this.btnInputRange.Text = "...";
            // 
            // btnDrawRange
            // 
            this.btnDrawRange.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDrawRange.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnDrawRange.Location = new System.Drawing.Point(392, 30);
            this.btnDrawRange.Name = "btnDrawRange";
            this.btnDrawRange.Size = new System.Drawing.Size(65, 23);
            this.btnDrawRange.TabIndex = 20;
            this.btnDrawRange.Text = "构造范围";
            // 
            // btnSearch
            // 
            this.btnSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearch.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnSearch.Location = new System.Drawing.Point(413, 86);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(44, 23);
            this.btnSearch.TabIndex = 21;
            this.btnSearch.Text = "查　询";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.radioButton1);
            this.groupPanel1.Controls.Add(this.btnSearch);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.btnDrawRange);
            this.groupPanel1.Controls.Add(this.cmbScale);
            this.groupPanel1.Controls.Add(this.btnInputRange);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.rbDrawRange);
            this.groupPanel1.Controls.Add(this.cmbTime);
            this.groupPanel1.Controls.Add(this.txtInputRange);
            this.groupPanel1.Controls.Add(this.dgResult);
            this.groupPanel1.Controls.Add(this.rdInputRange);
            this.groupPanel1.Controls.Add(this.txtMapNO);
            this.groupPanel1.Location = new System.Drawing.Point(3, 80);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(466, 358);
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
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel1.TabIndex = 23;
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(39, 32);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(297, 42);
            this.labelX1.TabIndex = 24;
            this.labelX1.Text = "根据关键字［图幅号、快图号、控制点名］或范围查找数据库中所有匹配的项";
            this.labelX1.WordWrap = true;
            // 
            // labelX2
            // 
            this.labelX2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX2.Location = new System.Drawing.Point(3, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(99, 23);
            this.labelX2.TabIndex = 25;
            this.labelX2.Text = "查找指定数据";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(342, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 50);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // FrmQueryAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 471);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "FrmQueryAll";
            this.ShowIcon = false;
            this.Text = "数据查找";
            ((System.ComponentModel.ISupportInitialize)(this.dgResult)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbScale;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbTime;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgResult;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.Controls.TextBoxX txtMapNO;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton rdInputRange;
        private DevComponents.DotNetBar.Controls.TextBoxX txtInputRange;
        private System.Windows.Forms.RadioButton rbDrawRange;
        private DevComponents.DotNetBar.ButtonX btnInputRange;
        private DevComponents.DotNetBar.ButtonX btnDrawRange;
        private DevComponents.DotNetBar.ButtonX btnSearch;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}