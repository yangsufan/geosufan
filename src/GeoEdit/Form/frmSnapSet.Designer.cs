namespace GeoEdit
{
    partial class frmSnapSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSnapSet));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupPanel = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.panelEx = new DevComponents.DotNetBar.PanelEx();
            this.txtSnapRadius = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.txtCacheRadius = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.dataGridViewX = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.buttonXOk = new DevComponents.DotNetBar.ButtonX();
            this.buttonXCancle = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel.SuspendLayout();
            this.panelEx.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX)).BeginInit();
            this.SuspendLayout();
            // 
            // groupPanel
            // 
            this.groupPanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel.Controls.Add(this.panelEx);
            this.groupPanel.Controls.Add(this.dataGridViewX);
            this.groupPanel.Location = new System.Drawing.Point(12, 12);
            this.groupPanel.Name = "groupPanel";
            this.groupPanel.Size = new System.Drawing.Size(385, 316);
            // 
            // 
            // 
            this.groupPanel.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel.Style.BackColorGradientAngle = 90;
            this.groupPanel.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel.Style.BorderBottomWidth = 1;
            this.groupPanel.Style.BorderColor = System.Drawing.Color.Transparent;
            this.groupPanel.Style.BorderGradientAngle = 0;
            this.groupPanel.Style.BorderLightGradientAngle = 0;
            this.groupPanel.Style.CornerDiameter = 4;
            this.groupPanel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupPanel.TabIndex = 1;
            this.groupPanel.Text = "设置捕捉图层及参数";
            // 
            // panelEx
            // 
            this.panelEx.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx.Controls.Add(this.txtSnapRadius);
            this.panelEx.Controls.Add(this.labelX4);
            this.panelEx.Controls.Add(this.txtCacheRadius);
            this.panelEx.Controls.Add(this.labelX5);
            this.panelEx.Controls.Add(this.labelX3);
            this.panelEx.Controls.Add(this.labelX2);
            this.panelEx.Controls.Add(this.labelX1);
            this.panelEx.Controls.Add(this.pictureBox);
            this.panelEx.Location = new System.Drawing.Point(0, 204);
            this.panelEx.Name = "panelEx";
            this.panelEx.Size = new System.Drawing.Size(385, 94);
            this.panelEx.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx.Style.BackColor1.Color = System.Drawing.Color.Transparent;
            this.panelEx.Style.BackColor2.Color = System.Drawing.SystemColors.GradientInactiveCaption;
            this.panelEx.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx.Style.BorderColor.Color = System.Drawing.SystemColors.ActiveCaption;
            this.panelEx.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx.Style.GradientAngle = 90;
            this.panelEx.TabIndex = 15;
            // 
            // txtSnapRadius
            // 
            // 
            // 
            // 
            this.txtSnapRadius.Border.Class = "TextBoxBorder";
            this.txtSnapRadius.Location = new System.Drawing.Point(85, 52);
            this.txtSnapRadius.Name = "txtSnapRadius";
            this.txtSnapRadius.Size = new System.Drawing.Size(39, 21);
            this.txtSnapRadius.TabIndex = 22;
            this.txtSnapRadius.Text = "20";
            this.txtSnapRadius.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSnapRadius_KeyDown);
            this.txtSnapRadius.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSnapRadius_KeyPress);
            this.txtSnapRadius.TextChanged += new System.EventHandler(this.txtSnapRadius_TextChanged);
            // 
            // labelX4
            // 
            this.labelX4.AutoSize = true;
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(304, 55);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(31, 18);
            this.labelX4.TabIndex = 21;
            this.labelX4.Text = "像素";
            // 
            // txtCacheRadius
            // 
            // 
            // 
            // 
            this.txtCacheRadius.Border.Class = "TextBoxBorder";
            this.txtCacheRadius.Location = new System.Drawing.Point(259, 52);
            this.txtCacheRadius.Name = "txtCacheRadius";
            this.txtCacheRadius.Size = new System.Drawing.Size(39, 21);
            this.txtCacheRadius.TabIndex = 15;
            this.txtCacheRadius.Text = "300";
            this.txtCacheRadius.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCacheRadius_KeyDown);
            this.txtCacheRadius.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCacheRadius_KeyPress);
            this.txtCacheRadius.TextChanged += new System.EventHandler(this.txtCacheRadius_TextChanged);
            // 
            // labelX5
            // 
            this.labelX5.AutoSize = true;
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(194, 55);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(68, 18);
            this.labelX5.TabIndex = 20;
            this.labelX5.Text = "缓冲半径：";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.Location = new System.Drawing.Point(130, 55);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(31, 18);
            this.labelX3.TabIndex = 17;
            this.labelX3.Text = "像素";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(20, 55);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(68, 18);
            this.labelX2.TabIndex = 19;
            this.labelX2.Text = "捕捉半径：";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(49, 15);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(259, 33);
            this.labelX1.TabIndex = 18;
            this.labelX1.Text = "设置合适的捕捉半径，以加快捕捉的效率。";
            this.labelX1.WordWrap = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(19, 13);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(24, 33);
            this.pictureBox.TabIndex = 16;
            this.pictureBox.TabStop = false;
            // 
            // dataGridViewX
            // 
            this.dataGridViewX.AllowUserToAddRows = false;
            this.dataGridViewX.AllowUserToDeleteRows = false;
            this.dataGridViewX.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridViewX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewX.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX.Location = new System.Drawing.Point(0, 3);
            this.dataGridViewX.Name = "dataGridViewX";
            this.dataGridViewX.RowTemplate.Height = 23;
            this.dataGridViewX.Size = new System.Drawing.Size(385, 196);
            this.dataGridViewX.TabIndex = 14;
            // 
            // buttonXOk
            // 
            this.buttonXOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXOk.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXOk.Location = new System.Drawing.Point(230, 334);
            this.buttonXOk.Name = "buttonXOk";
            this.buttonXOk.Size = new System.Drawing.Size(75, 23);
            this.buttonXOk.TabIndex = 2;
            this.buttonXOk.Text = "确定";
            this.buttonXOk.Click += new System.EventHandler(this.buttonXOk_Click);
            // 
            // buttonXCancle
            // 
            this.buttonXCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonXCancle.Location = new System.Drawing.Point(322, 334);
            this.buttonXCancle.Name = "buttonXCancle";
            this.buttonXCancle.Size = new System.Drawing.Size(75, 23);
            this.buttonXCancle.TabIndex = 3;
            this.buttonXCancle.Text = "取消";
            this.buttonXCancle.Click += new System.EventHandler(this.buttonXCancle_Click);
            // 
            // frmSnapSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 367);
            this.Controls.Add(this.buttonXCancle);
            this.Controls.Add(this.buttonXOk);
            this.Controls.Add(this.groupPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSnapSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "捕捉设置";
            this.groupPanel.ResumeLayout(false);
            this.panelEx.ResumeLayout(false);
            this.panelEx.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel;
        private DevComponents.DotNetBar.ButtonX buttonXOk;
        private DevComponents.DotNetBar.ButtonX buttonXCancle;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX;
        private DevComponents.DotNetBar.PanelEx panelEx;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.Controls.TextBoxX txtCacheRadius;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.PictureBox pictureBox;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSnapRadius;
    }
}