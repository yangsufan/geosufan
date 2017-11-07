using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto ;


namespace GeoDataCenterFunLib
{
    partial class frmBufferSet
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
            //m_pActiveViewEvents.AfterDraw -= new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.txtBufferValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar = new DevComponents.DotNetBar.Controls.Slider();
            this.cmd_OK = new DevComponents.DotNetBar.ButtonX();
            this.cmd_Cancel = new DevComponents.DotNetBar.ButtonX();
            this.Error_Lable = new System.Windows.Forms.Label();
            this.cmbSpatialRel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbIntersect = new DevComponents.Editors.ComboItem();
            this.cmbContains = new DevComponents.Editors.ComboItem();
            this.groupBox2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupBox1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupBox1.Controls.Add(this.txtBufferValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.trackBar);
            this.groupBox1.DrawTitleBox = false;
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 84);
            // 
            // 
            // 
            this.groupBox1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupBox1.Style.BackColorGradientAngle = 90;
            this.groupBox1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupBox1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox1.Style.BorderBottomWidth = 1;
            this.groupBox1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupBox1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox1.Style.BorderLeftWidth = 1;
            this.groupBox1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox1.Style.BorderRightWidth = 1;
            this.groupBox1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox1.Style.BorderTopWidth = 1;
            this.groupBox1.Style.CornerDiameter = 4;
            this.groupBox1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupBox1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupBox1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupBox1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupBox1.TabIndex = 3;
            this.groupBox1.Text = "缓冲半径　(有效范围：从0.0到 1000.0)";
            // 
            // txtBufferValue
            // 
            this.txtBufferValue.Location = new System.Drawing.Point(7, 18);
            this.txtBufferValue.Name = "txtBufferValue";
            this.txtBufferValue.Size = new System.Drawing.Size(68, 21);
            this.txtBufferValue.TabIndex = 7;
            this.txtBufferValue.TextChanged += new System.EventHandler(this.txtBufferValue_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(78, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "米";
            // 
            // trackBar
            // 
            this.trackBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.trackBar.BackColor = System.Drawing.Color.Transparent;
            this.trackBar.LabelVisible = false;
            this.trackBar.Location = new System.Drawing.Point(101, 7);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(280, 45);
            this.trackBar.TabIndex = 5;
            this.trackBar.Value = 0;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // cmd_OK
            // 
            this.cmd_OK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmd_OK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmd_OK.Location = new System.Drawing.Point(238, 161);
            this.cmd_OK.Name = "cmd_OK";
            this.cmd_OK.Size = new System.Drawing.Size(75, 23);
            this.cmd_OK.TabIndex = 4;
            this.cmd_OK.Text = "确 定";
            this.cmd_OK.Click += new System.EventHandler(this.cmd_OK_Click);
            // 
            // cmd_Cancel
            // 
            this.cmd_Cancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cmd_Cancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cmd_Cancel.Location = new System.Drawing.Point(323, 161);
            this.cmd_Cancel.Name = "cmd_Cancel";
            this.cmd_Cancel.Size = new System.Drawing.Size(75, 23);
            this.cmd_Cancel.TabIndex = 5;
            this.cmd_Cancel.Text = "取 消";
            this.cmd_Cancel.Click += new System.EventHandler(this.cmd_Cancel_Click);
            // 
            // Error_Lable
            // 
            this.Error_Lable.AutoSize = true;
            this.Error_Lable.Location = new System.Drawing.Point(8, 167);
            this.Error_Lable.Name = "Error_Lable";
            this.Error_Lable.Size = new System.Drawing.Size(41, 12);
            this.Error_Lable.TabIndex = 7;
            this.Error_Lable.Text = "label1";
            this.Error_Lable.Visible = false;
            // 
            // cmbSpatialRel
            // 
            this.cmbSpatialRel.DisplayMember = "Text";
            this.cmbSpatialRel.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSpatialRel.FormattingEnabled = true;
            this.cmbSpatialRel.ItemHeight = 15;
            this.cmbSpatialRel.Items.AddRange(new object[] {
            this.cmbIntersect,
            this.cmbContains});
            this.cmbSpatialRel.Location = new System.Drawing.Point(109, 5);
            this.cmbSpatialRel.Name = "cmbSpatialRel";
            this.cmbSpatialRel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbSpatialRel.Size = new System.Drawing.Size(219, 21);
            this.cmbSpatialRel.TabIndex = 8;
            this.cmbSpatialRel.SelectedIndexChanged += new System.EventHandler(this.cmbSpatialRel_SelectedIndexChanged);
            // 
            // cmbIntersect
            // 
            this.cmbIntersect.Text = "相交";
            // 
            // cmbContains
            // 
            this.cmbContains.Text = "包含";
            // 
            // groupBox2
            // 
            this.groupBox2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupBox2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbSpatialRel);
            this.groupBox2.DrawTitleBox = false;
            this.groupBox2.Location = new System.Drawing.Point(8, 91);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 59);
            // 
            // 
            // 
            this.groupBox2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupBox2.Style.BackColorGradientAngle = 90;
            this.groupBox2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupBox2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox2.Style.BorderBottomWidth = 1;
            this.groupBox2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupBox2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox2.Style.BorderLeftWidth = 1;
            this.groupBox2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox2.Style.BorderRightWidth = 1;
            this.groupBox2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupBox2.Style.BorderTopWidth = 1;
            this.groupBox2.Style.CornerDiameter = 4;
            this.groupBox2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupBox2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupBox2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupBox2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            this.groupBox2.TabIndex = 9;
            this.groupBox2.Text = "空间关系";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(34, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "空间条件：";
            // 
            // frmBufferSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(410, 198);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Error_Lable);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmd_Cancel);
            this.Controls.Add(this.cmd_OK);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBufferSet";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "缓冲半径设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBufferSet_FormClosed);
            this.Load += new System.EventHandler(this.frmBufferSet_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBufferSet_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupBox1;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.Slider trackBar;
        private System.Windows.Forms.TextBox txtBufferValue;
        private DevComponents.DotNetBar.ButtonX cmd_OK;
        private DevComponents.DotNetBar.ButtonX cmd_Cancel;
        private System.Windows.Forms.Label Error_Lable;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSpatialRel;
        private DevComponents.Editors.ComboItem cmbIntersect;
        private DevComponents.Editors.ComboItem cmbContains;
        private DevComponents.DotNetBar.Controls.GroupPanel groupBox2;
        private System.Windows.Forms.Label label1;
    }
}