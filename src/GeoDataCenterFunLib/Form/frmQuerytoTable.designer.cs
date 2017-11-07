namespace GeoDataCenterFunLib
{
    partial class frmQuerytoTable
    {
        /// <summary>
        /// 必需的设计器变量。        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。        /// </summary>
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
        /// 使用代码编辑器修改此方法的内容。        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.barTop = new DevComponents.DotNetBar.Bar();
            this.labelItem = new DevComponents.DotNetBar.LabelItem();
            this.comboBoxFeatureLayer = new DevComponents.DotNetBar.ComboBoxItem();
            this.comboItemTopLay = new DevComponents.Editors.ComboItem();
            this.comboItemVisibleLay = new DevComponents.Editors.ComboItem();
            this.comboItemSelectableLay = new DevComponents.Editors.ComboItem();
            this.comboItemAllLay = new DevComponents.Editors.ComboItem();
            this.barButtom = new DevComponents.DotNetBar.Bar();
            this.labelItemMemo = new DevComponents.DotNetBar.LabelItem();
            this.progressBarItem = new DevComponents.DotNetBar.ProgressBarItem();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.textBoxPage = new DevComponents.DotNetBar.TextBoxItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.buttonLastPage = new DevComponents.DotNetBar.ButtonItem();
            this.buttonNextPage = new DevComponents.DotNetBar.ButtonItem();
            this.buttonAll = new DevComponents.DotNetBar.ButtonItem();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.gridRes = new DevComponents.DotNetBar.Controls.DataGridViewX();
            ((System.ComponentModel.ISupportInitialize)(this.barTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barButtom)).BeginInit();
            this.itemPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRes)).BeginInit();
            this.SuspendLayout();
            // 
            // barTop
            // 
            this.barTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barTop.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem,
            this.comboBoxFeatureLayer});
            this.barTop.Location = new System.Drawing.Point(0, 0);
            this.barTop.Name = "barTop";
            this.barTop.Size = new System.Drawing.Size(762, 28);
            this.barTop.Stretch = true;
            this.barTop.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.barTop.TabIndex = 8;
            this.barTop.TabStop = false;
            // 
            // labelItem
            // 
            this.labelItem.Name = "labelItem";
            this.labelItem.Text = "    图层：";
            this.labelItem.Width = 70;
            // 
            // comboBoxFeatureLayer
            // 
            this.comboBoxFeatureLayer.ComboWidth = 400;
            this.comboBoxFeatureLayer.DropDownHeight = 106;
            this.comboBoxFeatureLayer.ItemHeight = 17;
            this.comboBoxFeatureLayer.Name = "comboBoxFeatureLayer";
            this.comboBoxFeatureLayer.SelectedIndexChanged += new System.EventHandler(this.comboBoxFeatureLayer_SelectedIndexChanged);
            // 
            // comboItemTopLay
            // 
            this.comboItemTopLay.Text = "顶层图层";
            // 
            // comboItemVisibleLay
            // 
            this.comboItemVisibleLay.Text = "可见图层";
            // 
            // comboItemSelectableLay
            // 
            this.comboItemSelectableLay.Text = "可选图层";
            // 
            // comboItemAllLay
            // 
            this.comboItemAllLay.Text = "所有图层";
            // 
            // barButtom
            // 
            this.barButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barButtom.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItemMemo,
            this.progressBarItem,
            this.labelItem1,
            this.textBoxPage,
            this.labelItem2,
            this.buttonLastPage,
            this.buttonNextPage,
            this.buttonAll});
            this.barButtom.Location = new System.Drawing.Point(0, 431);
            this.barButtom.Name = "barButtom";
            this.barButtom.Size = new System.Drawing.Size(762, 27);
            this.barButtom.Stretch = true;
            this.barButtom.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.barButtom.TabIndex = 14;
            this.barButtom.TabStop = false;
            // 
            // labelItemMemo
            // 
            this.labelItemMemo.Name = "labelItemMemo";
            this.labelItemMemo.Width = 120;
            // 
            // progressBarItem
            // 
            this.progressBarItem.ChunkGradientAngle = 0F;
            this.progressBarItem.ColorTable = DevComponents.DotNetBar.eProgressBarItemColor.Paused;
            this.progressBarItem.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.progressBarItem.Name = "progressBarItem";
            this.progressBarItem.RecentlyUsed = false;
            this.progressBarItem.Width = 400;
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "第";
            // 
            // textBoxPage
            // 
            this.textBoxPage.MaxLength = 30;
            this.textBoxPage.Name = "textBoxPage";
            this.textBoxPage.TextBoxWidth = 22;
            this.textBoxPage.WatermarkColor = System.Drawing.SystemColors.GrayText;
            this.textBoxPage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPage_KeyPress);
            this.textBoxPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPage_KeyDown);
            // 
            // labelItem2
            // 
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "页";
            // 
            // buttonLastPage
            // 
            this.buttonLastPage.Name = "buttonLastPage";
            this.buttonLastPage.Text = "上一页";
            this.buttonLastPage.Click += new System.EventHandler(this.buttonLastPage_Click);
            // 
            // buttonNextPage
            // 
            this.buttonNextPage.Name = "buttonNextPage";
            this.buttonNextPage.Text = "下一页";
            this.buttonNextPage.Click += new System.EventHandler(this.buttonNextPage_Click);
            // 
            // buttonAll
            // 
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Text = "列举全部";
            this.buttonAll.Click += new System.EventHandler(this.buttonAll_Click);
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.itemPanel1.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderBottomWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.itemPanel1.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderLeftWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderRightWidth = 1;
            this.itemPanel1.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.itemPanel1.BackgroundStyle.BorderTopWidth = 1;
            this.itemPanel1.BackgroundStyle.PaddingBottom = 1;
            this.itemPanel1.BackgroundStyle.PaddingLeft = 1;
            this.itemPanel1.BackgroundStyle.PaddingRight = 1;
            this.itemPanel1.BackgroundStyle.PaddingTop = 1;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Controls.Add(this.gridRes);
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.Location = new System.Drawing.Point(0, 28);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(762, 403);
            this.itemPanel1.TabIndex = 15;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // gridRes
            // 
            this.gridRes.AllowUserToAddRows = false;
            this.gridRes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridRes.DefaultCellStyle = dataGridViewCellStyle1;
            this.gridRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.gridRes.Location = new System.Drawing.Point(0, 0);
            this.gridRes.Name = "gridRes";
            this.gridRes.RowTemplate.Height = 23;
            this.gridRes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRes.Size = new System.Drawing.Size(762, 403);
            this.gridRes.TabIndex = 4;
            this.gridRes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRes_CellDoubleClick);
            // 
            // frmQuerytoTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 458);
            this.Controls.Add(this.itemPanel1);
            this.Controls.Add(this.barButtom);
            this.Controls.Add(this.barTop);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQuerytoTable";
            this.ShowIcon = false;
            this.Text = "查询结果";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmQuery_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.barTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barButtom)).EndInit();
            this.itemPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar barTop;
        private DevComponents.DotNetBar.LabelItem labelItem;
        private DevComponents.DotNetBar.ComboBoxItem comboBoxFeatureLayer;
        private DevComponents.Editors.ComboItem comboItemTopLay;
        private DevComponents.Editors.ComboItem comboItemVisibleLay;
        private DevComponents.Editors.ComboItem comboItemSelectableLay;
        //private DevComponents.Editors.ComboItem comboItemCurEdit;
        private DevComponents.DotNetBar.Bar barButtom;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.LabelItem labelItemMemo;
        private DevComponents.DotNetBar.ProgressBarItem progressBarItem;
        private DevComponents.Editors.ComboItem comboItemAllLay;
        private DevComponents.DotNetBar.Controls.DataGridViewX gridRes;
        private DevComponents.DotNetBar.ButtonItem buttonLastPage;
        private DevComponents.DotNetBar.ButtonItem buttonNextPage;
        private DevComponents.DotNetBar.ButtonItem buttonAll;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.TextBoxItem textBoxPage;
    }
}