namespace GeoHistory
{
    partial class FrmHistoryMapView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHistoryMapView));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.xTabHis = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ucHistoryPoint1 = new GeoHistory.Control.UCHistoryPoint();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ucHistoryMap1 = new GeoHistory.Control.UCHistoryMap();
            this.ucHistoryMap2 = new GeoHistory.Control.UCHistoryMap();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolAddLayer = new System.Windows.Forms.ToolStripButton();
            this.toolAnalysisChange = new System.Windows.Forms.ToolStripButton();
            this.tsbDefault = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tsbPan = new System.Windows.Forms.ToolStripButton();
            this.tsbLastView = new System.Windows.Forms.ToolStripButton();
            this.tsbNextView = new System.Windows.Forms.ToolStripButton();
            this.tsbFullExtent = new System.Windows.Forms.ToolStripButton();
            this.tsbIdentity = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.xTabHis.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.xTabHis);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(907, 432);
            this.splitContainer1.SplitterDistance = 249;
            this.splitContainer1.TabIndex = 0;
            // 
            // xTabHis
            // 
            this.xTabHis.Controls.Add(this.tabPage1);
            this.xTabHis.Controls.Add(this.tabPage3);
            this.xTabHis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xTabHis.Location = new System.Drawing.Point(0, 0);
            this.xTabHis.Name = "xTabHis";
            this.xTabHis.SelectedIndex = 0;
            this.xTabHis.Size = new System.Drawing.Size(249, 432);
            this.xTabHis.TabIndex = 0;
            this.xTabHis.SelectedIndexChanged += new System.EventHandler(this.xTabHis_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ucHistoryPoint1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(241, 406);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "日期点";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ucHistoryPoint1
            // 
            this.ucHistoryPoint1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.ucHistoryPoint1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHistoryPoint1.Location = new System.Drawing.Point(3, 3);
            this.ucHistoryPoint1.Name = "ucHistoryPoint1";
            this.ucHistoryPoint1.Size = new System.Drawing.Size(235, 400);
            this.ucHistoryPoint1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.axTOCControl1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(241, 406);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "图层";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(3, 3);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(235, 400);
            this.axTOCControl1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ucHistoryMap1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ucHistoryMap2);
            this.splitContainer2.Size = new System.Drawing.Size(654, 407);
            this.splitContainer2.SplitterDistance = 322;
            this.splitContainer2.TabIndex = 1;
            // 
            // ucHistoryMap1
            // 
            this.ucHistoryMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHistoryMap1.Location = new System.Drawing.Point(0, 0);
            this.ucHistoryMap1.Name = "ucHistoryMap1";
            this.ucHistoryMap1.Size = new System.Drawing.Size(322, 407);
            this.ucHistoryMap1.TabIndex = 0;
            this.ucHistoryMap1.Load += new System.EventHandler(this.ucHistoryMap1_Load);
            this.ucHistoryMap1.Enter += new System.EventHandler(this.ucHistoryMap1_Enter);
            // 
            // ucHistoryMap2
            // 
            this.ucHistoryMap2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHistoryMap2.Location = new System.Drawing.Point(0, 0);
            this.ucHistoryMap2.Name = "ucHistoryMap2";
            this.ucHistoryMap2.Size = new System.Drawing.Size(328, 407);
            this.ucHistoryMap2.TabIndex = 1;
            this.ucHistoryMap2.Enter += new System.EventHandler(this.ucHistoryMap2_Enter);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAddLayer,
            this.toolAnalysisChange,
            this.tsbDefault,
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.tsbPan,
            this.tsbLastView,
            this.tsbNextView,
            this.tsbFullExtent,
            this.tsbIdentity,
            this.tsbExport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(654, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolAddLayer
            // 
            this.toolAddLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAddLayer.Image = ((System.Drawing.Image)(resources.GetObject("toolAddLayer.Image")));
            this.toolAddLayer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAddLayer.Name = "toolAddLayer";
            this.toolAddLayer.Size = new System.Drawing.Size(23, 22);
            this.toolAddLayer.Text = "加载数据";
            this.toolAddLayer.Click += new System.EventHandler(this.toolAddLayer_Click);
            // 
            // toolAnalysisChange
            // 
            this.toolAnalysisChange.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolAnalysisChange.Image = ((System.Drawing.Image)(resources.GetObject("toolAnalysisChange.Image")));
            this.toolAnalysisChange.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAnalysisChange.Name = "toolAnalysisChange";
            this.toolAnalysisChange.Size = new System.Drawing.Size(23, 22);
            this.toolAnalysisChange.Text = "图形对比";
            this.toolAnalysisChange.Click += new System.EventHandler(this.toolAnalysisChange_Click);
            // 
            // tsbDefault
            // 
            this.tsbDefault.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDefault.Image = ((System.Drawing.Image)(resources.GetObject("tsbDefault.Image")));
            this.tsbDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDefault.Name = "tsbDefault";
            this.tsbDefault.Size = new System.Drawing.Size(23, 22);
            this.tsbDefault.Text = "toolStripButton2";
            this.tsbDefault.ToolTipText = "默认工具";
            this.tsbDefault.Click += new System.EventHandler(this.tsbDefault_Click);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tsbZoomIn.Image")));
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsbZoomIn.Text = "toolStripButton3";
            this.tsbZoomIn.ToolTipText = "放大";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbZoomIn_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tsbZoomOut.Image")));
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsbZoomOut.Text = "toolStripButton1";
            this.tsbZoomOut.ToolTipText = "缩小";
            this.tsbZoomOut.Click += new System.EventHandler(this.tsbZoomOut_Click);
            // 
            // tsbPan
            // 
            this.tsbPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPan.Image = ((System.Drawing.Image)(resources.GetObject("tsbPan.Image")));
            this.tsbPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPan.Name = "tsbPan";
            this.tsbPan.Size = new System.Drawing.Size(23, 22);
            this.tsbPan.Text = "toolStripButton4";
            this.tsbPan.ToolTipText = "漫游";
            this.tsbPan.Click += new System.EventHandler(this.tsbPan_Click);
            // 
            // tsbLastView
            // 
            this.tsbLastView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLastView.Image = ((System.Drawing.Image)(resources.GetObject("tsbLastView.Image")));
            this.tsbLastView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLastView.Name = "tsbLastView";
            this.tsbLastView.Size = new System.Drawing.Size(23, 22);
            this.tsbLastView.Text = "toolStripButton5";
            this.tsbLastView.ToolTipText = "前一视图";
            this.tsbLastView.Click += new System.EventHandler(this.tsbLastView_Click);
            // 
            // tsbNextView
            // 
            this.tsbNextView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNextView.Image = ((System.Drawing.Image)(resources.GetObject("tsbNextView.Image")));
            this.tsbNextView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNextView.Name = "tsbNextView";
            this.tsbNextView.Size = new System.Drawing.Size(23, 22);
            this.tsbNextView.Text = "toolStripButton6";
            this.tsbNextView.ToolTipText = "后一视图";
            this.tsbNextView.Click += new System.EventHandler(this.tsbNextView_Click);
            // 
            // tsbFullExtent
            // 
            this.tsbFullExtent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFullExtent.Image = ((System.Drawing.Image)(resources.GetObject("tsbFullExtent.Image")));
            this.tsbFullExtent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFullExtent.Name = "tsbFullExtent";
            this.tsbFullExtent.Size = new System.Drawing.Size(23, 22);
            this.tsbFullExtent.Text = "toolStripButton7";
            this.tsbFullExtent.ToolTipText = "全图";
            this.tsbFullExtent.Click += new System.EventHandler(this.tsbFullExtent_Click);
            // 
            // tsbIdentity
            // 
            this.tsbIdentity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbIdentity.Image = ((System.Drawing.Image)(resources.GetObject("tsbIdentity.Image")));
            this.tsbIdentity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbIdentity.Name = "tsbIdentity";
            this.tsbIdentity.Size = new System.Drawing.Size(23, 22);
            this.tsbIdentity.Text = "toolStripButton8";
            this.tsbIdentity.ToolTipText = "查询要素信息";
            this.tsbIdentity.Click += new System.EventHandler(this.tsbIdentity_Click);
            // 
            // tsbExport
            // 
            this.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExport.Image = ((System.Drawing.Image)(resources.GetObject("tsbExport.Image")));
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(23, 22);
            this.tsbExport.Text = "导出历史数据";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // FrmHistoryMapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 432);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmHistoryMapView";
            this.ShowIcon = false;
            this.Text = "历史回溯";
            this.Load += new System.EventHandler(this.FrmHistoryMapView_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.xTabHis.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl xTabHis;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbZoomOut;
        private GeoHistory.Control.UCHistoryPoint ucHistoryPoint1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private GeoHistory.Control.UCHistoryMap ucHistoryMap1;
        private GeoHistory.Control.UCHistoryMap ucHistoryMap2;
        private System.Windows.Forms.ToolStripButton tsbDefault;
        private System.Windows.Forms.ToolStripButton tsbZoomIn;
        private System.Windows.Forms.ToolStripButton tsbPan;
        private System.Windows.Forms.ToolStripButton tsbLastView;
        private System.Windows.Forms.ToolStripButton tsbNextView;
        private System.Windows.Forms.ToolStripButton tsbFullExtent;
        private System.Windows.Forms.ToolStripButton tsbIdentity;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton toolAddLayer;
        private System.Windows.Forms.ToolStripButton toolAnalysisChange;
    }
}