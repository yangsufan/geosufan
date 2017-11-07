using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace GeoProperties
{
    public class frmLayerProperties : DevComponents.DotNetBar.Office2007Form
    {

        private DevComponents.DotNetBar.ButtonX btnApply;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.DotNetBar.PanelEx panelEx1;

        private DevComponents.DotNetBar.TabControl tabLayerProperties;
        private DevComponents.DotNetBar.TabControlPanel tabControlPanel1;
        private DevComponents.DotNetBar.TabItem tabItem1;
        private UserControls.uctrGeneral m_uctrGeneral;

        private DevComponents.DotNetBar.TabControlPanel tabControlPanel2;
        private DevComponents.DotNetBar.TabItem tabItem2;
        private UserControls.uctrSource m_uctrSource;

        private DevComponents.DotNetBar.TabControlPanel tabControlPanel3;
        private DevComponents.DotNetBar.TabItem tabItem3;
        private UserControls.uctrFields m_uctrFields;

        private DevComponents.DotNetBar.TabControlPanel tabControlPanel4;
        private DevComponents.DotNetBar.TabItem tabItem4;
        private UserControls.uctrDefinitionQuery m_uctrDefQuery;

        private DevComponents.DotNetBar.TabControlPanel tabControlPanel5;
        private DevComponents.DotNetBar.TabItem tabItem5;
        private UserControls.ucSpatialIndex m_ucSpatialIndex;

        public static IActiveView m_pActiveView;
        private ILayer m_pLayer;
        private IFeatureLayer m_pCurrentLayer;
        public  static bool m_featureTrue;
        //private XmlDocument m_pXmlDoc;

        private void InitializeComponent()
        {
            this.btnApply = new DevComponents.DotNetBar.ButtonX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnApply.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnApply.Location = new System.Drawing.Point(381, 318);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 6;
            this.btnApply.Text = "应用";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(300, 318);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(219, 318);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(467, 313);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 7;
            // 
            // frmLayerProperties
            // 
            this.ClientSize = new System.Drawing.Size(467, 343);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLayerProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图层属性";
            this.TopMost = true;
            this.ResumeLayout(false);

        }
        /// <summary>
        /// 初始化TabControl
        /// </summary>
        public void InitTabControl()
        {
            
            #region 常规
            this.tabLayerProperties = new DevComponents.DotNetBar.TabControl();
            this.tabItem1 = new DevComponents.DotNetBar.TabItem();
            this.tabControlPanel1 = new DevComponents.DotNetBar.TabControlPanel();
            this.m_uctrGeneral = new GeoProperties.UserControls.uctrGeneral(m_pLayer);
            // 
            // tabLayerProperties
            // 
            this.tabLayerProperties.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabLayerProperties.CanReorderTabs = true;
            this.tabLayerProperties.Controls.Add(this.tabControlPanel1);
            this.tabLayerProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLayerProperties.Location = new System.Drawing.Point(0, 0);
            this.tabLayerProperties.Name = "tabLayerProperties";
            this.tabLayerProperties.SelectedTabFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.tabLayerProperties.SelectedTabIndex = 0;
            this.tabLayerProperties.Size = new System.Drawing.Size(464, 311);
            //this.tabLayerProperties.TabIndex = 1;
            this.tabLayerProperties.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.tabLayerProperties.Tabs.Add(this.tabItem1);
            this.tabLayerProperties.Text = "tabControl1";
            // 
            // tabItem1
            // 
            this.tabItem1.AttachedControl = this.tabControlPanel1;
            this.tabItem1.Name = "tabItem1";
            this.tabItem1.Text = "常规";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(464, 285);
            this.tabControlPanel1.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel1.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel1.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel1.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel1.Style.GradientAngle = 90;
            //this.tabControlPanel1.TabIndex = 1;
            this.tabControlPanel1.TabItem = this.tabItem1;
            //
            this.m_uctrGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_uctrGeneral.Name = "uctrGeneral";
            this.m_uctrGeneral.TabIndex = 0;
            //this.uctrSource1.Size = new System.Drawing.Size(462, 283);
            //this.uctrSource1.TabIndex = 0;
            this.tabControlPanel1.Controls.Add(this.m_uctrGeneral);

            this.panelEx1.Controls.Add(this.tabLayerProperties);

            #endregion

            #region 数据源
            this.tabItem2 = new DevComponents.DotNetBar.TabItem();
            this.tabControlPanel2 = new DevComponents.DotNetBar.TabControlPanel();
            this.m_uctrSource = new GeoProperties.UserControls.uctrSource(m_pLayer);

            // 
            // tabItem2
            // 
            this.tabItem2.AttachedControl = this.tabControlPanel2;
            this.tabItem2.Name = "tabItem2";
            this.tabItem2.Text = "数据源";
            // 
            // tabControlPanel2
            // 
            this.tabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel2.Name = "tabControlPanel2";
            this.tabControlPanel2.Padding = new System.Windows.Forms.Padding(1);
            //this.tabControlPanel2.Size = new System.Drawing.Size(464, 285);
            this.tabControlPanel2.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel2.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel2.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel2.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel2.Style.GradientAngle = 90;
            //this.tabControlPanel2.TabIndex = 1;
            this.tabControlPanel2.TabItem = this.tabItem2;
            //
            this.m_uctrSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_uctrSource.Name = "uctrSource";
            //this.m_uctrSource.TabIndex = 1;
            //this.uctrSource1.Size = new System.Drawing.Size(462, 283);
            //this.uctrSource1.TabIndex = 0;
            this.tabLayerProperties.Controls.Add(this.tabControlPanel2);
            this.tabControlPanel2.Controls.Add(this.m_uctrSource);

            this.tabLayerProperties.Tabs.Add(this.tabItem2);
            #endregion

            //如果是栅格数据
            if (m_featureTrue)
            {
                #region 字段
                this.tabItem3 = new DevComponents.DotNetBar.TabItem();
                this.tabControlPanel3 = new DevComponents.DotNetBar.TabControlPanel();
                this.m_uctrFields = new GeoProperties.UserControls.uctrFields(m_pLayer);

                // 
                // tabItem3
                // 
                this.tabItem3.AttachedControl = this.tabControlPanel3;
                this.tabItem3.Name = "tabItem3";
                this.tabItem3.Text = "字段";
                // 
                // tabControlPanel3
                // 
                this.tabControlPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
                //this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
                this.tabControlPanel3.Name = "tabControlPanel3";
                this.tabControlPanel3.Padding = new System.Windows.Forms.Padding(1);
                //this.tabControlPanel2.Size = new System.Drawing.Size(464, 285);
                this.tabControlPanel3.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
                this.tabControlPanel3.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
                this.tabControlPanel3.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
                this.tabControlPanel3.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
                this.tabControlPanel3.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                            | DevComponents.DotNetBar.eBorderSide.Bottom)));
                this.tabControlPanel3.Style.GradientAngle = 90;
                //this.tabControlPanel3.TabIndex = 2;
                this.tabControlPanel3.TabItem = this.tabItem3;
                //
                this.m_uctrFields.Dock = System.Windows.Forms.DockStyle.Fill;
                this.m_uctrFields.Name = "uctrFields";
                //this.m_uctrFields.TabIndex = 2;
                //this.uctrSource1.Size = new System.Drawing.Size(462, 283);
                //this.uctrSource1.TabIndex = 0;
                this.tabLayerProperties.Controls.Add(this.tabControlPanel3);
                this.tabControlPanel3.Controls.Add(this.m_uctrFields);

                this.tabLayerProperties.Tabs.Add(this.tabItem3);
                #endregion

                #region 定义查询
                this.tabItem4 = new DevComponents.DotNetBar.TabItem();
                this.tabControlPanel4 = new DevComponents.DotNetBar.TabControlPanel();
                this.m_uctrDefQuery = new GeoProperties.UserControls.uctrDefinitionQuery(m_pLayer);

                // 
                // tabItem4
                // 
                this.tabItem4.AttachedControl = this.tabControlPanel4;
                this.tabItem4.Name = "tabItem4";
                this.tabItem4.Text = "定义查询";
                // 
                // tabControlPanel4
                // 
                this.tabControlPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
                //this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
                this.tabControlPanel4.Name = "tabControlPanel4";
                this.tabControlPanel4.Padding = new System.Windows.Forms.Padding(1);
                //this.tabControlPanel2.Size = new System.Drawing.Size(464, 285);
                this.tabControlPanel4.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
                this.tabControlPanel4.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
                this.tabControlPanel4.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
                this.tabControlPanel4.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
                this.tabControlPanel4.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                            | DevComponents.DotNetBar.eBorderSide.Bottom)));
                this.tabControlPanel4.Style.GradientAngle = 90;
                //this.tabControlPanel4.TabIndex = 1;
                this.tabControlPanel4.TabItem = this.tabItem4;
                //
                this.m_uctrDefQuery.Dock = System.Windows.Forms.DockStyle.Fill;
                this.m_uctrDefQuery.Name = "uctrSource";
                //this.m_uctrDefQuery.TabIndex = 3;
                //this.uctrSource1.Size = new System.Drawing.Size(462, 283);
                //this.uctrSource1.TabIndex = 0;
                this.tabLayerProperties.Controls.Add(this.tabControlPanel4);
                this.tabControlPanel4.Controls.Add(this.m_uctrDefQuery);

                this.tabLayerProperties.Tabs.Add(this.tabItem4);
                #endregion
            }
            
            #region 空间索引
            this.tabItem5 = new DevComponents.DotNetBar.TabItem();
            this.tabControlPanel5 = new DevComponents.DotNetBar.TabControlPanel();
            this.m_ucSpatialIndex = new GeoProperties.UserControls.ucSpatialIndex(m_pLayer);
            this.m_ucSpatialIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // tabItem4
            // 
            this.tabItem5.AttachedControl = this.tabControlPanel5;
            this.tabItem5.Name = "tabItem5";
            this.tabItem5.Text = "其他设置";
            // 
            // tabControlPanel4
            // 
            this.tabControlPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.tabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel5.Name = "tabControlPanel5";
            this.tabControlPanel5.Padding = new System.Windows.Forms.Padding(1);
            //this.tabControlPanel2.Size = new System.Drawing.Size(464, 285);
            this.tabControlPanel5.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(179)))), ((int)(((byte)(231)))));
            this.tabControlPanel5.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(237)))), ((int)(((byte)(254)))));
            this.tabControlPanel5.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.tabControlPanel5.Style.BorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(97)))), ((int)(((byte)(156)))));
            this.tabControlPanel5.Style.BorderSide = ((DevComponents.DotNetBar.eBorderSide)(((DevComponents.DotNetBar.eBorderSide.Left | DevComponents.DotNetBar.eBorderSide.Right)
                        | DevComponents.DotNetBar.eBorderSide.Bottom)));
            this.tabControlPanel5.Style.GradientAngle = 90;
            //this.tabControlPanel4.TabIndex = 1;
            this.tabControlPanel5.TabItem = this.tabItem5;
 
            this.tabLayerProperties.Controls.Add(this.tabControlPanel5);
            this.tabControlPanel5.Controls.Add(this.m_ucSpatialIndex);

            this.tabLayerProperties.Tabs.Add(this.tabItem5);
            #endregion

           


        }

        public frmLayerProperties(ILayer pLayer,IActiveView pActiveView,bool FeatureTure)
        {
            m_pActiveView = pActiveView;
            m_pLayer = pLayer;

           //在这判断是否为影像和适量了
            if (pLayer is IRasterCatalogLayer || pLayer is IRasterLayer)
            {
                FeatureTure = false;
            }
            else
            {
                FeatureTure = true;
            }

            m_featureTrue = FeatureTure;
            InitializeComponent();
            InitTabControl();//添加TabControl
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_uctrGeneral.SaveChangeResult();

            if (m_uctrDefQuery != null)
                m_uctrDefQuery.GetQueryResult();

            if (m_uctrFields != null)
                m_uctrFields.SaveFieldChange();

            //空间索引
            if (m_ucSpatialIndex != null)
            {
                m_ucSpatialIndex.SetGridSize();//空间索引
                m_ucSpatialIndex.SetRasterNoDataColor();//影像无效值设置
            }

            this.Close();
            if (m_pActiveView != null)
            {
                m_pActiveView.Refresh();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //通用
            m_uctrGeneral.SaveChangeResult();

            //定义显示
            if (m_uctrDefQuery != null)
            m_uctrDefQuery.GetQueryResult();

            //字段信息
            if (m_uctrFields != null)
            m_uctrFields.SaveFieldChange();

            //空间索引
            if (m_ucSpatialIndex != null)
            {
                m_ucSpatialIndex.SetGridSize();
                m_ucSpatialIndex.SetRasterNoDataColor();
            }

            if (m_pActiveView != null)
            {
                m_pActiveView.Refresh();
            }     
        }       
    }
}