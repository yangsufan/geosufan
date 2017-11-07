using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto ;


namespace GeoUtilities
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBufferValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.cmd_OK = new System.Windows.Forms.Button();
            this.cmd_Cancel = new System.Windows.Forms.Button();
            this.Error_Lable = new System.Windows.Forms.Label();
            this.cmbSpatialRel = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.cmbIntersect = new DevComponents.Editors.ComboItem();
            this.cmbTouches = new DevComponents.Editors.ComboItem();
            this.cmbCrosses = new DevComponents.Editors.ComboItem();
            this.cmbContains = new DevComponents.Editors.ComboItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox1.Controls.Add(this.txtBufferValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.trackBar);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(390, 91);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "缓冲半径　(有效范围：从0.0到 1000.0)";
            // 
            // txtBufferValue
            // 
            this.txtBufferValue.Location = new System.Drawing.Point(9, 40);
            this.txtBufferValue.Name = "txtBufferValue";
            this.txtBufferValue.Size = new System.Drawing.Size(70, 21);
            this.txtBufferValue.TabIndex = 7;
            this.txtBufferValue.TextChanged += new System.EventHandler(this.txtBufferValue_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.AliceBlue;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(82, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "米";
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.Color.LightSteelBlue;
            this.trackBar.Location = new System.Drawing.Point(109, 29);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(275, 45);
            this.trackBar.TabIndex = 5;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // cmd_OK
            // 
            this.cmd_OK.Location = new System.Drawing.Point(284, 150);
            this.cmd_OK.Name = "cmd_OK";
            this.cmd_OK.Size = new System.Drawing.Size(52, 26);
            this.cmd_OK.TabIndex = 4;
            this.cmd_OK.Text = "确 定";
            this.cmd_OK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmd_OK.UseVisualStyleBackColor = true;
            this.cmd_OK.Click += new System.EventHandler(this.cmd_OK_Click);
            // 
            // cmd_Cancel
            // 
            this.cmd_Cancel.Location = new System.Drawing.Point(342, 150);
            this.cmd_Cancel.Name = "cmd_Cancel";
            this.cmd_Cancel.Size = new System.Drawing.Size(50, 26);
            this.cmd_Cancel.TabIndex = 5;
            this.cmd_Cancel.Text = "取 消";
            this.cmd_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmd_Cancel.UseVisualStyleBackColor = true;
            this.cmd_Cancel.Click += new System.EventHandler(this.cmd_Cancel_Click);
            // 
            // Error_Lable
            // 
            this.Error_Lable.AutoSize = true;
            this.Error_Lable.Location = new System.Drawing.Point(8, 156);
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
            this.cmbTouches,
            this.cmbCrosses,
            this.cmbContains});
            this.cmbSpatialRel.Location = new System.Drawing.Point(109, 14);
            this.cmbSpatialRel.Name = "cmbSpatialRel";
            this.cmbSpatialRel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbSpatialRel.Size = new System.Drawing.Size(162, 21);
            this.cmbSpatialRel.TabIndex = 8;
            this.cmbSpatialRel.SelectedIndexChanged += new System.EventHandler(this.cmbSpatialRel_SelectedIndexChanged);
            // 
            // cmbIntersect
            // 
            this.cmbIntersect.Text = "相交";
            // 
            // cmbTouches
            // 
            this.cmbTouches.Text = "相接";
            // 
            // cmbCrosses
            // 
            this.cmbCrosses.Text = "穿越";
            // 
            // cmbContains
            // 
            this.cmbContains.Text = "包含";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbSpatialRel);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox2.Location = new System.Drawing.Point(8, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(390, 42);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "空间关系";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.AliceBlue;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(34, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "空间条件：";
            // 
            // frmBufferSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(410, 179);
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
            this.Load += new System.EventHandler(this.frmBufferSet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBufferSet_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBufferSet_KeyDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.TextBox txtBufferValue;
        private System.Windows.Forms.Button cmd_OK;
        private System.Windows.Forms.Button cmd_Cancel;
        private System.Windows.Forms.Label Error_Lable;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSpatialRel;
        private DevComponents.Editors.ComboItem cmbIntersect;
        private DevComponents.Editors.ComboItem cmbTouches;
        private DevComponents.Editors.ComboItem cmbCrosses;
        private DevComponents.Editors.ComboItem cmbContains;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
    }
}