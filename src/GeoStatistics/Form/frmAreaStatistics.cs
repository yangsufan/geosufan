using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace GeoStatistics
{
    public partial class frmAreaStatistics : DevComponents.DotNetBar.Office2007Form
    {
        public frmAreaStatistics()
        {
            InitializeComponent();
        }

        private IMapControlDefault m_pMap;
        public IMapControlDefault CurMap
        {
            set { m_pMap = value; }
        }

        private ESRI.ArcGIS.Geometry.IGeometry m_pGeometry;
        public ESRI.ArcGIS.Geometry.IGeometry CurGeometry
        {
            set { m_pGeometry = value; }
        }

        public void InitFrm()
        {
            this.ucArea1.CurGeometry = m_pGeometry;
            this.ucArea1.CurMap = m_pMap;
            this.ucArea1.InitLayers();
            
        }

        private void frmAreaStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {

            this.ucArea1.CurGeometry = null;
            this.ucArea1.boolok = true;
            ///ZQ  2011 1129  移除添加的事件
            SysCommon.ScreenDraw.list.Remove(this.ucArea1.UcAreaAfterDraw);
            (m_pMap.ActiveView).PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }

        public void SetSliderValue(bool p)
        {
            this.ucArea1.CurSliderEnable = p;
        }

        private void frmAreaStatistics_MouseClick(object sender, MouseEventArgs e)
        {
            this.ucArea1.HideLayerTree();
        }
    }
}