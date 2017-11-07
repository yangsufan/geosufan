namespace GeoDBATool
{
    partial class ControlHistoryBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlHistoryBar));
            this.BarHistory = new DevComponents.DotNetBar.Bar();
            this.btnAddHistoryData = new DevComponents.DotNetBar.ButtonItem();
            this.sliderItem = new DevComponents.DotNetBar.SliderItem();
            this.comboBoxItem = new DevComponents.DotNetBar.ComboBoxItem();
            this.btnCompare = new DevComponents.DotNetBar.ButtonItem();
            this.btnRenderHistoryData = new DevComponents.DotNetBar.ButtonItem();
            this.btnStract = new DevComponents.DotNetBar.ButtonItem();
            this.btnSelFeatures = new DevComponents.DotNetBar.ButtonItem();
            this.btnSelFeaturesByCondition = new DevComponents.DotNetBar.ButtonItem();
            this.btnBrowse = new DevComponents.DotNetBar.ButtonItem();
            this.dockContainerItem1 = new DevComponents.DotNetBar.DockContainerItem();
            this.dockContainerItem2 = new DevComponents.DotNetBar.DockContainerItem();
            ((System.ComponentModel.ISupportInitialize)(this.BarHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // BarHistory
            // 
            this.BarHistory.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnAddHistoryData,
            this.sliderItem,
            this.comboBoxItem,
            this.btnCompare,
            this.btnRenderHistoryData,
            this.btnStract,
            this.btnSelFeatures,
            this.btnSelFeaturesByCondition,
            this.btnBrowse});
            this.BarHistory.Location = new System.Drawing.Point(3, 19);
            this.BarHistory.Name = "BarHistory";
            this.BarHistory.Size = new System.Drawing.Size(748, 26);
            this.BarHistory.Stretch = true;
            this.BarHistory.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            this.BarHistory.TabIndex = 0;
            this.BarHistory.TabStop = false;
            // 
            // btnAddHistoryData
            // 
            this.btnAddHistoryData.Image = ((System.Drawing.Image)(resources.GetObject("btnAddHistoryData.Image")));
            this.btnAddHistoryData.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnAddHistoryData.ImagePaddingHorizontal = 8;
            this.btnAddHistoryData.Name = "btnAddHistoryData";
            this.btnAddHistoryData.Tooltip = "加载历史数据";
            this.btnAddHistoryData.Click += new System.EventHandler(this.btnAddHistoryData_Click);
            // 
            // sliderItem
            // 
            this.sliderItem.BeginGroup = true;
            this.sliderItem.Name = "sliderItem";
            this.sliderItem.Text = "浏览";
            this.sliderItem.Value = 0;
            this.sliderItem.Width = 300;
            this.sliderItem.ValueChanged += new System.EventHandler(this.sliderItem_ValueChanged);
            // 
            // comboBoxItem
            // 
            this.comboBoxItem.ComboWidth = 150;
            this.comboBoxItem.DropDownHeight = 106;
            this.comboBoxItem.Name = "comboBoxItem";
            this.comboBoxItem.SelectedIndexChanged += new System.EventHandler(this.comboBoxItem_SelectedIndexChanged);
            // 
            // btnCompare
            // 
            this.btnCompare.Image = global::GeoDBATool.Properties.Resources.CompareHistoryData;
            this.btnCompare.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnCompare.ImagePaddingHorizontal = 8;
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Text = "btnCompare";
            this.btnCompare.Tooltip = "对比浏览";
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnRenderHistoryData
            // 
            this.btnRenderHistoryData.Image = global::GeoDBATool.Properties.Resources.RenderHistoryData;
            this.btnRenderHistoryData.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnRenderHistoryData.ImagePaddingHorizontal = 8;
            this.btnRenderHistoryData.Name = "btnRenderHistoryData";
            this.btnRenderHistoryData.Text = "btnRenderHistoryData";
            this.btnRenderHistoryData.Tooltip = "更新变化渲染";
            this.btnRenderHistoryData.Click += new System.EventHandler(this.btnRenderHistoryData_Click);
            // 
            // btnStract
            // 
            this.btnStract.Image = global::GeoDBATool.Properties.Resources.StractHistoryData;
            this.btnStract.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnStract.ImagePaddingHorizontal = 8;
            this.btnStract.Name = "btnStract";
            this.btnStract.Text = "btnStract";
            this.btnStract.Tooltip = "提取数据";
            this.btnStract.Click += new System.EventHandler(this.btnStract_Click);
            // 
            // btnSelFeatures
            // 
            this.btnSelFeatures.BeginGroup = true;
            this.btnSelFeatures.Image = ((System.Drawing.Image)(resources.GetObject("btnSelFeatures.Image")));
            this.btnSelFeatures.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnSelFeatures.ImagePaddingHorizontal = 8;
            this.btnSelFeatures.Name = "btnSelFeatures";
            this.btnSelFeatures.Text = "btnSelFeatures";
            this.btnSelFeatures.Tooltip = "要素选择";
            this.btnSelFeatures.Click += new System.EventHandler(this.btnSelFeatures_Click);
            // 
            // btnSelFeaturesByCondition
            // 
            this.btnSelFeaturesByCondition.Image = global::GeoDBATool.Properties.Resources.SelFeaturesByCondition;
            this.btnSelFeaturesByCondition.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnSelFeaturesByCondition.ImagePaddingHorizontal = 8;
            this.btnSelFeaturesByCondition.Name = "btnSelFeaturesByCondition";
            this.btnSelFeaturesByCondition.Text = "btnSelFeaturesByCondition";
            this.btnSelFeaturesByCondition.Tooltip = "要素查询选择";
            this.btnSelFeaturesByCondition.Click += new System.EventHandler(this.btnSelFeaturesByCondition_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Image = global::GeoDBATool.Properties.Resources.BrowseHistoryData;
            this.btnBrowse.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.NotSet;
            this.btnBrowse.ImagePaddingHorizontal = 8;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Text = "btnBrowse";
            this.btnBrowse.Tooltip = "选择要素浏览";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dockContainerItem1
            // 
            this.dockContainerItem1.Name = "dockContainerItem1";
            this.dockContainerItem1.Text = "dockContainerItem1";
            // 
            // dockContainerItem2
            // 
            this.dockContainerItem2.Name = "dockContainerItem2";
            this.dockContainerItem2.Text = "dockContainerItem2";
            // 
            // ControlHistoryBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BarHistory);
            this.Name = "ControlHistoryBar";
            this.Size = new System.Drawing.Size(773, 74);
            ((System.ComponentModel.ISupportInitialize)(this.BarHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar BarHistory;
        private DevComponents.DotNetBar.ButtonItem btnAddHistoryData;
        private DevComponents.DotNetBar.SliderItem sliderItem;
        private DevComponents.DotNetBar.ComboBoxItem comboBoxItem;
        private DevComponents.DotNetBar.ButtonItem btnStract;
        private DevComponents.DotNetBar.ButtonItem btnRenderHistoryData;
        private DevComponents.DotNetBar.ButtonItem btnSelFeatures;
        private DevComponents.DotNetBar.ButtonItem btnBrowse;
        private DevComponents.DotNetBar.ButtonItem btnCompare;
        private DevComponents.DotNetBar.ButtonItem btnSelFeaturesByCondition;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem1;
        private DevComponents.DotNetBar.DockContainerItem dockContainerItem2;
    }
}
