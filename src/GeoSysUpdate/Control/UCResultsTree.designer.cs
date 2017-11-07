namespace GeoSysUpdate.Control
{
    partial class UCResultsTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCResultsTree));
            this.tVResults = new System.Windows.Forms.TreeView();
            this.imageList_Small = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tVResults
            // 
            this.tVResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tVResults.Location = new System.Drawing.Point(0, 0);
            this.tVResults.Name = "tVResults";
            this.tVResults.Size = new System.Drawing.Size(180, 173);
            this.tVResults.TabIndex = 0;
            this.tVResults.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tVResults_NodeMouseClick);
            // 
            // imageList_Small
            // 
            this.imageList_Small.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_Small.ImageStream")));
            this.imageList_Small.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_Small.Images.SetKeyName(0, "Save");
            this.imageList_Small.Images.SetKeyName(1, "Import");
            this.imageList_Small.Images.SetKeyName(2, "Flag");
            this.imageList_Small.Images.SetKeyName(3, "Export");
            this.imageList_Small.Images.SetKeyName(4, "MoveDown");
            this.imageList_Small.Images.SetKeyName(5, "MoveUp");
            this.imageList_Small.Images.SetKeyName(6, "CAD");
            this.imageList_Small.Images.SetKeyName(7, "FC");
            this.imageList_Small.Images.SetKeyName(8, "PDB");
            this.imageList_Small.Images.SetKeyName(9, "SDE");
            this.imageList_Small.Images.SetKeyName(10, "RC");
            this.imageList_Small.Images.SetKeyName(11, "RD");
            this.imageList_Small.Images.SetKeyName(12, "Root");
            this.imageList_Small.Images.SetKeyName(13, "FD");
            this.imageList_Small.Images.SetKeyName(14, "PDB");
            this.imageList_Small.Images.SetKeyName(15, "Dept");
            this.imageList_Small.Images.SetKeyName(16, "Step");
            this.imageList_Small.Images.SetKeyName(17, "SubType");
            this.imageList_Small.Images.SetKeyName(18, "_polygon");
            this.imageList_Small.Images.SetKeyName(19, "_annotation");
            this.imageList_Small.Images.SetKeyName(20, "_Dimension");
            this.imageList_Small.Images.SetKeyName(21, "_line");
            this.imageList_Small.Images.SetKeyName(22, "_MultiPatch");
            this.imageList_Small.Images.SetKeyName(23, "_point");
            this.imageList_Small.Images.SetKeyName(24, "目录管理图标");
            this.imageList_Small.Images.SetKeyName(25, "打开");
            this.imageList_Small.Images.SetKeyName(26, "关闭");
            this.imageList_Small.Images.SetKeyName(27, "属性");
            this.imageList_Small.Images.SetKeyName(28, "图层");
            this.imageList_Small.Images.SetKeyName(29, "元数据");
            this.imageList_Small.Images.SetKeyName(30, "资源节点");
            this.imageList_Small.Images.SetKeyName(31, "ArcMap");
            this.imageList_Small.Images.SetKeyName(32, "excel");
            this.imageList_Small.Images.SetKeyName(33, "word");
            this.imageList_Small.Images.SetKeyName(34, "images");
            this.imageList_Small.Images.SetKeyName(35, "text");
            this.imageList_Small.Images.SetKeyName(36, "pdf");
            this.imageList_Small.Images.SetKeyName(37, "warning");
            // 
            // UCResultsTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tVResults);
            this.Name = "UCResultsTree";
            this.Size = new System.Drawing.Size(180, 173);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tVResults;
        private System.Windows.Forms.ImageList imageList_Small;
    }
}
