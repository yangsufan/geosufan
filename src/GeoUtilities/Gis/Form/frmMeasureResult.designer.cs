namespace GeoUtilities
{
    partial class frmMeasureResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMeasureResult));
            this.lblResult = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolMeasureLine = new System.Windows.Forms.ToolStripButton();
            this.toolMeasureArea = new System.Windows.Forms.ToolStripButton();
            this.toolMeasureAngle = new System.Windows.Forms.ToolStripButton();
            this.toolMeasureFeature = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSnapToFeature = new System.Windows.Forms.ToolStripButton();
            this.toolShowSum = new System.Windows.Forms.ToolStripButton();
            this.toolUnitsMenu = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolM = new System.Windows.Forms.ToolStripMenuItem();
            this.toolKM = new System.Windows.Forms.ToolStripMenuItem();
            this.toolMu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolHectares = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolClear = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.SystemColors.Window;
            this.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.Location = new System.Drawing.Point(0, 25);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(343, 176);
            this.lblResult.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolMeasureLine,
            this.toolMeasureArea,
            this.toolMeasureAngle,
            this.toolMeasureFeature,
            this.toolStripSeparator1,
            this.toolSnapToFeature,
            this.toolShowSum,
            this.toolUnitsMenu,
            this.toolStripSeparator2,
            this.toolClear});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(343, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolMeasureLine
            // 
            this.toolMeasureLine.AutoSize = false;
            this.toolMeasureLine.CheckOnClick = true;
            this.toolMeasureLine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolMeasureLine.Image = ((System.Drawing.Image)(resources.GetObject("toolMeasureLine.Image")));
            this.toolMeasureLine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolMeasureLine.ImageTransparentColor = System.Drawing.Color.White;
            this.toolMeasureLine.Name = "toolMeasureLine";
            this.toolMeasureLine.Size = new System.Drawing.Size(24, 24);
            this.toolMeasureLine.ToolTipText = "测量线段";
            this.toolMeasureLine.Click += new System.EventHandler(this.toolMeasureLine_Click);
            // 
            // toolMeasureArea
            // 
            this.toolMeasureArea.AutoSize = false;
            this.toolMeasureArea.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolMeasureArea.Image = ((System.Drawing.Image)(resources.GetObject("toolMeasureArea.Image")));
            this.toolMeasureArea.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolMeasureArea.ImageTransparentColor = System.Drawing.Color.White;
            this.toolMeasureArea.Name = "toolMeasureArea";
            this.toolMeasureArea.Size = new System.Drawing.Size(24, 24);
            this.toolMeasureArea.ToolTipText = "测量多边形";
            this.toolMeasureArea.Click += new System.EventHandler(this.toolMeasureArea_Click);
            // 
            // toolMeasureAngle
            // 
            this.toolMeasureAngle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolMeasureAngle.Image = ((System.Drawing.Image)(resources.GetObject("toolMeasureAngle.Image")));
            this.toolMeasureAngle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolMeasureAngle.Name = "toolMeasureAngle";
            this.toolMeasureAngle.Size = new System.Drawing.Size(28, 22);
            this.toolMeasureAngle.ToolTipText = "测量角度";
            this.toolMeasureAngle.Click += new System.EventHandler(this.toolMeasureAngle_Click);
            // 
            // toolMeasureFeature
            // 
            this.toolMeasureFeature.AutoSize = false;
            this.toolMeasureFeature.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolMeasureFeature.Image = ((System.Drawing.Image)(resources.GetObject("toolMeasureFeature.Image")));
            this.toolMeasureFeature.ImageTransparentColor = System.Drawing.Color.White;
            this.toolMeasureFeature.Name = "toolMeasureFeature";
            this.toolMeasureFeature.Size = new System.Drawing.Size(24, 24);
            this.toolMeasureFeature.Text = "toolStripButton3";
            this.toolMeasureFeature.ToolTipText = "测量要素形状";
            this.toolMeasureFeature.Visible = false;
            this.toolMeasureFeature.Click += new System.EventHandler(this.toolMeasureFeature_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolSnapToFeature
            // 
            this.toolSnapToFeature.AutoSize = false;
            this.toolSnapToFeature.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSnapToFeature.Image = ((System.Drawing.Image)(resources.GetObject("toolSnapToFeature.Image")));
            this.toolSnapToFeature.ImageTransparentColor = System.Drawing.Color.White;
            this.toolSnapToFeature.Name = "toolSnapToFeature";
            this.toolSnapToFeature.Size = new System.Drawing.Size(24, 24);
            this.toolSnapToFeature.ToolTipText = "捕捉开关";
            this.toolSnapToFeature.Click += new System.EventHandler(this.toolSnapToFeature_Click);
            // 
            // toolShowSum
            // 
            this.toolShowSum.AutoSize = false;
            this.toolShowSum.CheckOnClick = true;
            this.toolShowSum.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolShowSum.Image = ((System.Drawing.Image)(resources.GetObject("toolShowSum.Image")));
            this.toolShowSum.ImageTransparentColor = System.Drawing.Color.White;
            this.toolShowSum.Name = "toolShowSum";
            this.toolShowSum.Size = new System.Drawing.Size(24, 24);
            this.toolShowSum.ToolTipText = "是否显示总和";
            this.toolShowSum.Click += new System.EventHandler(this.toolShowSum_Click);
            // 
            // toolUnitsMenu
            // 
            this.toolUnitsMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolUnitsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolM,
            this.toolKM,
            this.toolMu,
            this.toolHectares});
            this.toolUnitsMenu.Image = ((System.Drawing.Image)(resources.GetObject("toolUnitsMenu.Image")));
            this.toolUnitsMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolUnitsMenu.Name = "toolUnitsMenu";
            this.toolUnitsMenu.Size = new System.Drawing.Size(45, 22);
            this.toolUnitsMenu.Text = "单位";
            this.toolUnitsMenu.ToolTipText = "设置单位";
            // 
            // toolM
            // 
            this.toolM.Name = "toolM";
            this.toolM.Size = new System.Drawing.Size(152, 22);
            this.toolM.Text = "米";
            this.toolM.Click += new System.EventHandler(this.toolM_Click);
            // 
            // toolKM
            // 
            this.toolKM.Name = "toolKM";
            this.toolKM.Size = new System.Drawing.Size(152, 22);
            this.toolKM.Text = "千米";
            this.toolKM.Click += new System.EventHandler(this.toolKM_Click_1);
            // 
            // toolMu
            // 
            this.toolMu.Enabled = false;
            this.toolMu.Name = "toolMu";
            this.toolMu.Size = new System.Drawing.Size(152, 22);
            this.toolMu.Text = "亩";
            this.toolMu.Click += new System.EventHandler(this.toolMu_Click);
            // 
            // toolHectares
            // 
            this.toolHectares.Enabled = false;
            this.toolHectares.Name = "toolHectares";
            this.toolHectares.Size = new System.Drawing.Size(152, 22);
            this.toolHectares.Text = "公顷";
            this.toolHectares.Click += new System.EventHandler(this.toolHectares_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolClear
            // 
            this.toolClear.AutoSize = false;
            this.toolClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolClear.Image = ((System.Drawing.Image)(resources.GetObject("toolClear.Image")));
            this.toolClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolClear.ImageTransparentColor = System.Drawing.Color.White;
            this.toolClear.Name = "toolClear";
            this.toolClear.Size = new System.Drawing.Size(24, 24);
            this.toolClear.ToolTipText = "清除当前结果";
            this.toolClear.Click += new System.EventHandler(this.toolClear_Click);
            // 
            // frmMeasureResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 201);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMeasureResult";
            this.ShowIcon = false;
            this.Text = "量算工具";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMeasureResult_FormClosed);
            this.Load += new System.EventHandler(this.frmMeasureResult_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolMeasureLine;
        private System.Windows.Forms.ToolStripButton toolMeasureArea;
        private System.Windows.Forms.ToolStripButton toolMeasureFeature;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolSnapToFeature;
        private System.Windows.Forms.ToolStripButton toolShowSum;
        private System.Windows.Forms.ToolStripDropDownButton toolUnitsMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolClear;
        private System.Windows.Forms.ToolStripMenuItem toolM;
        private System.Windows.Forms.ToolStripMenuItem toolKM;
        private System.Windows.Forms.ToolStripButton toolMeasureAngle;
        private System.Windows.Forms.ToolStripMenuItem toolMu;
        private System.Windows.Forms.ToolStripMenuItem toolHectares;

    }
}