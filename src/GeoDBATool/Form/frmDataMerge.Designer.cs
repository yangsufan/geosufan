namespace GeoDBATool
{
    partial class frmDataMerge
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
            this.AdvTreeFeaCls = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.dtGridAttribute = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.AdvTreeFeaCls)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridAttribute)).BeginInit();
            this.SuspendLayout();
            // 
            // AdvTreeFeaCls
            // 
            this.AdvTreeFeaCls.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.AdvTreeFeaCls.AllowDrop = true;
            this.AdvTreeFeaCls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.AdvTreeFeaCls.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.AdvTreeFeaCls.BackgroundStyle.Class = "TreeBorderKey";
            this.AdvTreeFeaCls.Location = new System.Drawing.Point(-2, 0);
            this.AdvTreeFeaCls.Name = "AdvTreeFeaCls";
            this.AdvTreeFeaCls.NodesConnector = this.nodeConnector1;
            this.AdvTreeFeaCls.NodeStyle = this.elementStyle1;
            this.AdvTreeFeaCls.PathSeparator = ";";
            this.AdvTreeFeaCls.Size = new System.Drawing.Size(127, 297);
            this.AdvTreeFeaCls.Styles.Add(this.elementStyle1);
            this.AdvTreeFeaCls.SuspendPaint = false;
            this.AdvTreeFeaCls.TabIndex = 2;
            this.AdvTreeFeaCls.Text = "advTree1";
            this.AdvTreeFeaCls.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.AdvTreeFeaCls_NodeDoubleClick);
            this.AdvTreeFeaCls.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.AdvTreeFeaCls_NodeClick);
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // dtGridAttribute
            // 
            this.dtGridAttribute.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtGridAttribute.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dtGridAttribute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtGridAttribute.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtGridAttribute.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dtGridAttribute.Location = new System.Drawing.Point(125, 0);
            this.dtGridAttribute.Name = "dtGridAttribute";
            this.dtGridAttribute.RowTemplate.Height = 23;
            this.dtGridAttribute.Size = new System.Drawing.Size(240, 294);
            this.dtGridAttribute.TabIndex = 3;
            // 
            // frmDataMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 293);
            this.Controls.Add(this.dtGridAttribute);
            this.Controls.Add(this.AdvTreeFeaCls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataMerge";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "要素融合选择";
            ((System.ComponentModel.ISupportInitialize)(this.AdvTreeFeaCls)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtGridAttribute)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevComponents.AdvTree.AdvTree AdvTreeFeaCls;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.Controls.DataGridViewX dtGridAttribute;

    }
}