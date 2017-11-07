namespace GeoSysUpdate
{
    partial class ucProjectInfo
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucProjectInfo));
            this.treeProjectInfo = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.IconContainer = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treeProjectInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // treeProjectInfo
            // 
            this.treeProjectInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.treeProjectInfo.AllowDrop = true;
            this.treeProjectInfo.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.treeProjectInfo.BackgroundStyle.Class = "TreeBorderKey";
            this.treeProjectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProjectInfo.Location = new System.Drawing.Point(0, 0);
            this.treeProjectInfo.Name = "treeProjectInfo";
            this.treeProjectInfo.NodesConnector = this.nodeConnector1;
            this.treeProjectInfo.NodeStyle = this.elementStyle1;
            this.treeProjectInfo.PathSeparator = ";";
            this.treeProjectInfo.Size = new System.Drawing.Size(260, 361);
            this.treeProjectInfo.Styles.Add(this.elementStyle1);
            this.treeProjectInfo.SuspendPaint = false;
            this.treeProjectInfo.TabIndex = 0;
            this.treeProjectInfo.Text = "advTree1";
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
            // IconContainer
            // 
            this.IconContainer.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconContainer.ImageStream")));
            this.IconContainer.TransparentColor = System.Drawing.Color.Transparent;
            this.IconContainer.Images.SetKeyName(0, "");
            this.IconContainer.Images.SetKeyName(1, "");
            this.IconContainer.Images.SetKeyName(2, "");
            this.IconContainer.Images.SetKeyName(3, "");
            this.IconContainer.Images.SetKeyName(4, "");
            this.IconContainer.Images.SetKeyName(5, "");
            this.IconContainer.Images.SetKeyName(6, "");
            this.IconContainer.Images.SetKeyName(7, "");
            this.IconContainer.Images.SetKeyName(8, "");
            this.IconContainer.Images.SetKeyName(9, "");
            this.IconContainer.Images.SetKeyName(10, "");
            this.IconContainer.Images.SetKeyName(11, "");
            this.IconContainer.Images.SetKeyName(12, "trash.ico");
            this.IconContainer.Images.SetKeyName(13, "flag.ico");
            this.IconContainer.Images.SetKeyName(14, "pictures.ico");
            this.IconContainer.Images.SetKeyName(15, "refresh.ico");
            this.IconContainer.Images.SetKeyName(16, "export.ico");
            // 
            // ucProjectInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeProjectInfo);
            this.Name = "ucProjectInfo";
            this.Size = new System.Drawing.Size(260, 361);
            ((System.ComponentModel.ISupportInitialize)(this.treeProjectInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.AdvTree.AdvTree treeProjectInfo;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private System.Windows.Forms.ImageList IconContainer;
    }
}
