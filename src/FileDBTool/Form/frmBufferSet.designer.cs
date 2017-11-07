using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto ;


namespace FileDBTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBufferSet));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBufferValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.cmd_OK = new System.Windows.Forms.Button();
            this.cmd_Cancel = new System.Windows.Forms.Button();
            this.superTooltip1 = new DevComponents.DotNetBar.SuperTooltip();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
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
            this.groupBox1.Size = new System.Drawing.Size(374, 91);
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
            this.txtBufferValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBufferValue_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.AliceBlue;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(82, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 6;
            this.label2.Text = "米";
            // 
            // trackBar
            // 
            this.trackBar.BackColor = System.Drawing.Color.LightSteelBlue;
            this.trackBar.Location = new System.Drawing.Point(109, 29);
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(255, 45);
            this.trackBar.TabIndex = 5;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // cmd_OK
            // 
            this.cmd_OK.Image = ((System.Drawing.Image)(resources.GetObject("cmd_OK.Image")));
            this.cmd_OK.Location = new System.Drawing.Point(246, 104);
            this.cmd_OK.Name = "cmd_OK";
            this.cmd_OK.Size = new System.Drawing.Size(65, 26);
            this.cmd_OK.TabIndex = 4;
            this.cmd_OK.Text = "确 定";
            this.cmd_OK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmd_OK.UseVisualStyleBackColor = true;
            this.cmd_OK.Click += new System.EventHandler(this.cmd_OK_Click);
            // 
            // cmd_Cancel
            // 
            this.cmd_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("cmd_Cancel.Image")));
            this.cmd_Cancel.Location = new System.Drawing.Point(317, 104);
            this.cmd_Cancel.Name = "cmd_Cancel";
            this.cmd_Cancel.Size = new System.Drawing.Size(65, 26);
            this.cmd_Cancel.TabIndex = 5;
            this.cmd_Cancel.Text = "取 消";
            this.cmd_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmd_Cancel.UseVisualStyleBackColor = true;
            this.cmd_Cancel.Click += new System.EventHandler(this.cmd_Cancel_Click);
            // 
            // frmBufferSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(391, 136);
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
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBufferSet_KeyDown);
            this.Load += new System.EventHandler(this.frmBufferSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.TextBox txtBufferValue;
        private System.Windows.Forms.Button cmd_OK;
        private System.Windows.Forms.Button cmd_Cancel;
        private DevComponents.DotNetBar.SuperTooltip superTooltip1;
    }
}