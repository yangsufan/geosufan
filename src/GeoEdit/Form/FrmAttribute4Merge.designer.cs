namespace GeoEdit
{
    partial class FrmAttribute4Merge
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAttribute4Merge));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeview_Name = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.DT_VIEW_Attriubte = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.panelEx1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeview_Name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_VIEW_Attriubte)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.Controls.Add(this.splitContainer1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(338, 201);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            this.panelEx1.Text = "panelEx1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeview_Name);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.DT_VIEW_Attriubte);
            this.splitContainer1.Size = new System.Drawing.Size(338, 201);
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeview_Name
            // 
            this.treeview_Name.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.treeview_Name.AllowDrop = true;
            this.treeview_Name.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.treeview_Name.BackgroundStyle.Class = "TreeBorderKey";
            this.treeview_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeview_Name.Location = new System.Drawing.Point(0, 0);
            this.treeview_Name.Name = "treeview_Name";
            this.treeview_Name.NodesConnector = this.nodeConnector1;
            this.treeview_Name.NodeStyle = this.elementStyle1;
            this.treeview_Name.PathSeparator = ";";
            this.treeview_Name.Size = new System.Drawing.Size(112, 201);
            this.treeview_Name.Styles.Add(this.elementStyle1);
            this.treeview_Name.SuspendPaint = false;
            this.treeview_Name.TabIndex = 0;
            this.treeview_Name.Text = "advTree1";
            this.treeview_Name.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.treeview_Name_NodeDoubleClick);
            this.treeview_Name.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.treeview_Name_NodeClick);
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
            // DT_VIEW_Attriubte
            // 
            this.DT_VIEW_Attriubte.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DT_VIEW_Attriubte.DefaultCellStyle = dataGridViewCellStyle1;
            this.DT_VIEW_Attriubte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DT_VIEW_Attriubte.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DT_VIEW_Attriubte.Enabled = false;
            this.DT_VIEW_Attriubte.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.DT_VIEW_Attriubte.Location = new System.Drawing.Point(0, 0);
            this.DT_VIEW_Attriubte.Name = "DT_VIEW_Attriubte";
            this.DT_VIEW_Attriubte.RowTemplate.Height = 23;
            this.DT_VIEW_Attriubte.Size = new System.Drawing.Size(222, 201);
            this.DT_VIEW_Attriubte.TabIndex = 0;
            this.DT_VIEW_Attriubte.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DT_VIEW_Attriubte_CellClick);
            // 
            // FrmAttribute4Merge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 201);
            this.Controls.Add(this.panelEx1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAttribute4Merge";
            this.ShowInTaskbar = false;
            this.Text = "要素融合选择";
            this.Load += new System.EventHandler(this.FrmAttribute_Load);
            this.panelEx1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeview_Name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DT_VIEW_Attriubte)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public  DevComponents.DotNetBar.Controls.DataGridViewX DT_VIEW_Attriubte;
        public DevComponents.AdvTree.AdvTree treeview_Name;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
    }
}