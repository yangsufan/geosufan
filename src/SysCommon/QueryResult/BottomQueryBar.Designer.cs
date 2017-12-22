namespace Fan.Common
{
    partial class BottomQueryBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BottomQueryBar));
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "earth");
            this.ImageList.Images.SetKeyName(1, "Root");
            this.ImageList.Images.SetKeyName(2, "DIR");
            this.ImageList.Images.SetKeyName(3, "DataDIRHalfOpen");
            this.ImageList.Images.SetKeyName(4, "DataDIRClosed");
            this.ImageList.Images.SetKeyName(5, "DataDIROpen");
            this.ImageList.Images.SetKeyName(6, "Layer");
            this.ImageList.Images.SetKeyName(7, "_annotation");
            this.ImageList.Images.SetKeyName(8, "_Dimension");
            this.ImageList.Images.SetKeyName(9, "_line");
            this.ImageList.Images.SetKeyName(10, "_MultiPatch");
            this.ImageList.Images.SetKeyName(11, "_point");
            this.ImageList.Images.SetKeyName(12, "_polygon");
            this.ImageList.Images.SetKeyName(13, "annotation");
            this.ImageList.Images.SetKeyName(14, "Dimension");
            this.ImageList.Images.SetKeyName(15, "line");
            this.ImageList.Images.SetKeyName(16, "MultiPatch");
            this.ImageList.Images.SetKeyName(17, "point");
            this.ImageList.Images.SetKeyName(18, "polygon");
            this.ImageList.Images.SetKeyName(19, "PublicVersion");
            this.ImageList.Images.SetKeyName(20, "PersonalVersion");
            this.ImageList.Images.SetKeyName(21, "INVISIBLE");
            this.ImageList.Images.SetKeyName(22, "VISIBLE");
            // 
            // BottomQueryBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.Name = "BottomQueryBar";
            this.Size = new System.Drawing.Size(176, 179);
            this.ResumeLayout(false);

        }

        #endregion
        
        public System.Windows.Forms.ImageList ImageList;
    }
}
