namespace GeoUserManager
{
    partial class SelectExportExtent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectExportExtent));
            this.advXZQ = new DevComponents.AdvTree.AdvTree();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancle = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.advXZQ)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // advXZQ
            // 
            this.advXZQ.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advXZQ.AllowDrop = true;
            this.advXZQ.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advXZQ.BackgroundStyle.Class = "TreeBorderKey";
            this.advXZQ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advXZQ.ImageList = this.imageList;
            this.advXZQ.Location = new System.Drawing.Point(3, 17);
            this.advXZQ.Name = "advXZQ";
            this.advXZQ.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advXZQ.NodesConnector = this.nodeConnector1;
            this.advXZQ.NodeStyle = this.elementStyle1;
            this.advXZQ.PathSeparator = ";";
            this.advXZQ.Size = new System.Drawing.Size(304, 364);
            this.advXZQ.Styles.Add(this.elementStyle1);
            this.advXZQ.TabIndex = 0;
            this.advXZQ.Text = "advTree1";
            this.advXZQ.AfterCheck += new DevComponents.AdvTree.AdvTreeCellEventHandler(this.advXZQ_AfterCheck);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            this.imageList.Images.SetKeyName(11, "");
            this.imageList.Images.SetKeyName(12, "");
            this.imageList.Images.SetKeyName(13, "");
            this.imageList.Images.SetKeyName(14, "folder 拷贝333.png");
            this.imageList.Images.SetKeyName(15, "");
            this.imageList.Images.SetKeyName(16, "");
            this.imageList.Images.SetKeyName(17, "");
            this.imageList.Images.SetKeyName(18, "");
            this.imageList.Images.SetKeyName(19, "");
            this.imageList.Images.SetKeyName(20, "");
            this.imageList.Images.SetKeyName(21, "image.png");
            this.imageList.Images.SetKeyName(22, "table.ico");
            this.imageList.Images.SetKeyName(23, "table.ico");
            this.imageList.Images.SetKeyName(24, "经纬网.ico");
            this.imageList.Images.SetKeyName(25, "city.png");
            this.imageList.Images.SetKeyName(26, "county.png");
            this.imageList.Images.SetKeyName(27, "province.png");
            this.imageList.Images.SetKeyName(28, "1_s1.png");
            this.imageList.Images.SetKeyName(29, "2_s1.png");
            this.imageList.Images.SetKeyName(30, "3_1_s1.png");
            this.imageList.Images.SetKeyName(31, "3_s1.png");
            this.imageList.Images.SetKeyName(32, "4_1_s1.png");
            this.imageList.Images.SetKeyName(33, "4_s1.png");
            this.imageList.Images.SetKeyName(34, "book_next.png");
            this.imageList.Images.SetKeyName(35, "page_world.png");
            this.imageList.Images.SetKeyName(36, "page_green.png");
            this.imageList.Images.SetKeyName(37, "PublishPlanHS.png");
            this.imageList.Images.SetKeyName(38, "world.png");
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.advXZQ);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 384);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "行政区";
            // 
            // btnCancle
            // 
            this.btnCancle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancle.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancle.Location = new System.Drawing.Point(225, 398);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(82, 29);
            this.btnCancle.TabIndex = 2;
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(137, 398);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(82, 29);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SelectExportExtent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 439);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SelectExportExtent";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提取范围权限设置";
            ((System.ComponentModel.ISupportInitialize)(this.advXZQ)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree advXZQ;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.ButtonX btnCancle;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private System.Windows.Forms.ImageList imageList;
    }
}