namespace SceneCommonTools
{
    partial class UCSceneControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCSceneControl));
            this.SceneControlMain = new ESRI.ArcGIS.Controls.AxSceneControl();
            ((System.ComponentModel.ISupportInitialize)(this.SceneControlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // SceneControlMain
            // 
            this.SceneControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SceneControlMain.Location = new System.Drawing.Point(0, 0);
            this.SceneControlMain.Name = "SceneControlMain";
            this.SceneControlMain.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("SceneControlMain.OcxState")));
            this.SceneControlMain.Size = new System.Drawing.Size(150, 150);
            this.SceneControlMain.TabIndex = 1;
            // 
            // UCSceneControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SceneControlMain);
            this.Name = "UCSceneControl";
            ((System.ComponentModel.ISupportInitialize)(this.SceneControlMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxSceneControl SceneControlMain;

    }
}
