using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Analyst3D;

namespace GeoArcScene3DAnalyse
{
    public partial class frm3DSpacedistance : DevComponents.DotNetBar.Office2007Form
    {
        public IPoint m_Point;
        public IElement pElementOne;
        public double SpaceLength;
        public frm3DSpacedistance()
        {
            InitializeComponent();
            labeSpace1.Text = "";
            labeVer3.Text = "";
            labSum4.Text = "";
            labLevel2.Text = "";
        }
        ESRI.ArcGIS.Controls.ISceneControl m_pCurrentSceneControl = null;
        public ESRI.ArcGIS.Controls.ISceneControl CurrentSceneControl
        {
            set { m_pCurrentSceneControl = value; }
        }

        private void frm3DSpacedistance_FormClosing(object sender, FormClosingEventArgs e)
        {
            IGraphicsLayer player = m_pCurrentSceneControl.Scene.BasicGraphicsLayer;
            IGraphicsContainer3D pgraphiccontainer3d = (IGraphicsContainer3D)player;
            pgraphiccontainer3d.DeleteAllElements();
            m_pCurrentSceneControl.SceneGraph.RefreshViewers();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pgraphiccontainer3d);
            Dispose();
        }

    }
}
