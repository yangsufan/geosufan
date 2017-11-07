namespace SceneCommonTools
{
    partial class FrmAddLayerFromMap
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddLayerFromMap));
            this.btnNoSelAll = new System.Windows.Forms.Button();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.LayerTree = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader2 = new DevComponents.AdvTree.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.LayerTree)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNoSelAll
            // 
            this.btnNoSelAll.Location = new System.Drawing.Point(222, 41);
            this.btnNoSelAll.Name = "btnNoSelAll";
            this.btnNoSelAll.Size = new System.Drawing.Size(39, 23);
            this.btnNoSelAll.TabIndex = 9;
            this.btnNoSelAll.Text = "反选";
            this.btnNoSelAll.UseVisualStyleBackColor = true;
            this.btnNoSelAll.Click += new System.EventHandler(this.btnNoSelAll_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(222, 12);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(39, 23);
            this.btnSelAll.TabIndex = 8;
            this.btnSelAll.Text = "全选";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(190, 283);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(119, 283);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(65, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // LayerTree
            // 
            this.LayerTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.LayerTree.AllowDrop = true;
            this.LayerTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.LayerTree.BackgroundStyle.Class = "TreeBorderKey";
            this.LayerTree.Columns.Add(this.columnHeader1);
            this.LayerTree.Columns.Add(this.columnHeader2);
            this.LayerTree.Location = new System.Drawing.Point(12, 12);
            this.LayerTree.Name = "LayerTree";
            this.LayerTree.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.LayerTree.NodesConnector = this.nodeConnector1;
            this.LayerTree.NodeStyle = this.elementStyle1;
            this.LayerTree.PathSeparator = ";";
            this.LayerTree.Size = new System.Drawing.Size(205, 260);
            this.LayerTree.Styles.Add(this.elementStyle1);
            this.LayerTree.TabIndex = 11;
            this.LayerTree.Text = "advTree1";
            this.LayerTree.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.LayerTree_AfterCheck);
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
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
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "DataDIR");
            this.ImageList.Images.SetKeyName(1, "Layer");
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "图层";
            this.columnHeader1.Width.Absolute = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "选择";
            this.columnHeader2.Width.Absolute = 150;
            // 
            // FrmAddLayerFromMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 311);
            this.Controls.Add(this.LayerTree);
            this.Controls.Add(this.btnNoSelAll);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "FrmAddLayerFromMap";
            this.Text = "选择图层";
            this.Load += new System.EventHandler(this.FrmAddLayerFromMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LayerTree)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNoSelAll;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private DevComponents.AdvTree.AdvTree LayerTree;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.ColumnHeader columnHeader2;
    }
}