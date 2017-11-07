using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geometry;
namespace SceneCommonTools
{
    public partial class UCSceneControl : UserControl
    {
        public UCSceneControl()
        {
            InitializeComponent();

            //添加对UC滚轮的监听
            this.MouseWheel += new MouseEventHandler(UCSceneControl_MouseWheel);
        }

        //处理滚轮事件
        void UCSceneControl_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                System.Drawing.Point pSceLoc = SceneControlMain.PointToScreen(this.SceneControlMain.Location);
                System.Drawing.Point Pt = this.PointToScreen(e.Location);
                if (Pt.X < pSceLoc.X | Pt.X > pSceLoc.X + SceneControlMain.Width | Pt.Y < pSceLoc.Y | Pt.Y > pSceLoc.Y + SceneControlMain.Height) return;

                double scale = 0.2;
                if (e.Delta < 0) scale = -0.2;
                ICamera pCamera = SceneControlMain.Camera;
                IPoint pPtObs = pCamera.Observer;
                IPoint pPtTar = pCamera.Target;
                pPtObs.X += (pPtObs.X - pPtTar.X) * scale;
                pPtObs.Y += (pPtObs.Y - pPtTar.Y) * scale;
                pPtObs.Z += (pPtObs.Z - pPtTar.Z) * scale;
                pCamera.Observer = pPtObs;
                SceneControlMain.SceneGraph.RefreshViewers();

            }
            catch
            {
                return;
            }

        }

        public ESRI.ArcGIS.Controls.AxSceneControl axSceneCtl
        {
            get { return this.SceneControlMain; }
        }
    }
}
