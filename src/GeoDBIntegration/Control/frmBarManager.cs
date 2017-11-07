using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace GeoDBIntegration
{
    public partial class frmBarManager : System.Windows.Forms.Form
    {
        public frmBarManager()
        {
            InitializeComponent();
        }

        public DevComponents.DotNetBar.DotNetBarManager MainDotNetBarManager
        {
            get
            {
                return this.dotNetBarManager;
            }
        }

        public DevComponents.DotNetBar.Bar ButtomBar
        {
            get
            {
                return this.barTip;
            }
        }
        public DevComponents.DotNetBar.Bar CreateBar(string strName, enumLayType layType)
        {
            DevComponents.DotNetBar.Bar bar = new DevComponents.DotNetBar.Bar();
            bar.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            bar.AlwaysDisplayDockTab = true;
            bar.CanCustomize = false;
            bar.CanDockBottom = false;
            bar.CanDockDocument = true;
            bar.CanDockLeft = false;
            bar.CanDockRight = false;
            bar.CanDockTop = false;
            bar.CanHide = true;
            bar.CanUndock = false;
            bar.DockTabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Top;
            bar.LayoutType = DevComponents.DotNetBar.eLayoutType.DockContainer;
            bar.SelectedDockTab = 0;
            bar.Stretch = true;
            bar.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2003;
            bar.TabStop = false;
            bar.Name = strName;

            DevComponents.DotNetBar.DockSite dockSite = null;
            switch (layType)
            {
                case enumLayType.BOTTOM:
                    dockSite = this.barBottomDockSite;
                    break;
                case enumLayType.FILL:
                    dockSite = this.barFilldockSite;
                    break;
                case enumLayType.LEFT:
                    dockSite = this.barLeftDockSite;
                    break;
                case enumLayType.RIGHT:
                    dockSite = this.barRightDockSite;
                    break;
                case enumLayType.TOP:
                    dockSite = this.barTopDockSite;
                    break;
            }
            if (dockSite != null)
            {
                dockSite.Controls.Add(bar);
                dockSite.DocumentDockContainer.Orientation = DevComponents.DotNetBar.eOrientation.Vertical;
                DevComponents.DotNetBar.DocumentBarContainer aDocumentBarContainer = new DocumentBarContainer(bar, bar.Size.Width, bar.Size.Height);
                dockSite.DocumentDockContainer.Documents.Add(((DevComponents.DotNetBar.DocumentBaseContainer)(aDocumentBarContainer)));
            }
            return bar;
        }

        public DevComponents.DotNetBar.PanelDockContainer CreatePanelDockContainer(string panelName, DevComponents.DotNetBar.Bar bar)
        {
            DevComponents.DotNetBar.PanelDockContainer panelDockContainer = new PanelDockContainer();
            panelDockContainer.Name = panelName;
            panelDockContainer.Style.Alignment = System.Drawing.StringAlignment.Center;
            panelDockContainer.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            panelDockContainer.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            panelDockContainer.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            panelDockContainer.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            panelDockContainer.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            panelDockContainer.Style.GradientAngle = 90;
            panelDockContainer.Dock = DockStyle.Fill;
            bar.Controls.Add(panelDockContainer);

            return panelDockContainer;
        }

        public DevComponents.DotNetBar.DockContainerItem CreateDockContainerItem(string ItemName, string ItemText, DevComponents.DotNetBar.PanelDockContainer panelDockContainer, DevComponents.DotNetBar.Bar bar)
        {
            DevComponents.DotNetBar.DockContainerItem dockContainerItem = new DockContainerItem();
            dockContainerItem.Control = panelDockContainer;
            dockContainerItem.Name = ItemName;
            dockContainerItem.Text = ItemText;

            bar.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] { dockContainerItem });

            return dockContainerItem;
        }
    }
}