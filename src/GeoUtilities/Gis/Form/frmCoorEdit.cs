using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;

namespace GeoUtilities
{
    public partial class frmCoorEdit : DevComponents.DotNetBar.Office2007Form
    {
        private IMapControlDefault m_pMap = null;

        private IGeometry m_pGeometry = null;
        public IGeometry Geometry
        {
            get { return m_pGeometry; }
            set { m_pGeometry = value; }
        }

        public frmCoorEdit(IMapControlDefault pMap)
        {
            InitComponent();
            m_pMap = pMap;
        }

        public frmCoorEdit()
        {
            InitComponent();
        }

        private void InitComponent()
        {
            InitializeComponent();
        }

        private void frmCoorEdit_Load(object sender, EventArgs e)
        {
            if (m_pMap == null) return;

            IActiveView pAv=m_pMap.ActiveView as IActiveView;
            this.txtMinX.Text = pAv.Extent.XMin.ToString();
            this.txtMinY.Text = pAv.Extent.YMin.ToString();
            this.txtMaxX.Text = pAv.Extent.XMax.ToString();
            this.txtMaxY.Text = pAv.Extent.YMax.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_pGeometry =GetGeometryByCoor();

            if (m_pGeometry.IsEmpty)
            {
                MessageBox.Show("输入坐标范围为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private IGeometry GetGeometryByCoor()
        {
            //直接获得坐标范围
            double dblMinX = 0;
            double dblMaxX = 0;
            double dblMinY = 0;
            double dblMaxY = 0;

            double.TryParse(this.txtMinX.Text, out dblMinX);
            double.TryParse(this.txtMaxX.Text, out dblMaxX);
            double.TryParse(this.txtMinY.Text, out dblMinY);
            double.TryParse(this.txtMaxY.Text, out dblMaxY);

            //定义范围
            IPointCollection pPntCollection = new PolygonClass();
            object obj = System.Type.Missing;
            IPoint pPnt1 = new PointClass();
            pPnt1.PutCoords(dblMinX, dblMinY);
            pPntCollection.AddPoint(pPnt1, ref obj, ref obj);
            IPoint pPnt2 = new PointClass();
            pPnt2.PutCoords(dblMaxX, dblMinY);
            pPntCollection.AddPoint(pPnt2, ref obj, ref obj);
            IPoint pPnt3 = new PointClass();
            pPnt3.PutCoords(dblMaxX, dblMaxY);
            pPntCollection.AddPoint(pPnt3, ref obj, ref obj);
            IPoint pPnt4 = new PointClass();
            pPnt4.PutCoords(dblMinX, dblMaxY);
            pPntCollection.AddPoint(pPnt4, ref obj, ref obj);

            //
            IPolygon2 pPolygon2 = pPntCollection as IPolygon2;
            pPolygon2.Close();

            if (pPolygon2.IsEmpty)
            {
                return null;
            }

            if (m_pMap != null && m_pMap.SpatialReference != null) pPolygon2.Project(m_pMap.SpatialReference);
            return pPolygon2 as IGeometry;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #region 输入控制
        private double m_dblMinX = 0;
        private void txtMinX_TextChanged(object sender, EventArgs e)
        {
            double dblTemp=0;
            if (!double.TryParse(this.txtMinX.Text, out dblTemp))
            {
                this.txtMinX.Text = m_dblMinX.ToString();
                return;
            }

            m_dblMinX = dblTemp;
        }

        private double m_dlbMinY = 0;
        private void txtMinY_TextChanged(object sender, EventArgs e)
        {
            double dblTemp = 0;
            if (!double.TryParse(this.txtMinY.Text, out dblTemp))
            {
                this.txtMinY.Text = m_dlbMinY.ToString();
                return;
            }

            m_dlbMinY = dblTemp;
        }

        private double m_dblMaxX = 0;
        private void txtMaxX_TextChanged(object sender, EventArgs e)
        {
            double dblTemp = 0;
            if (!double.TryParse(this.txtMaxX.Text, out dblTemp))
            {
                this.txtMaxX.Text = m_dblMaxX.ToString();
                return;
            }

            m_dblMaxX = dblTemp;
        }

        private double m_dblMaxY = 0;
        private void txtMaxY_TextChanged(object sender, EventArgs e)
        {
            double dblTemp = 0;
            if (!double.TryParse(this.txtMaxY.Text, out dblTemp))
            {
                this.txtMaxY.Text = m_dblMaxY.ToString();
                return;
            }

            m_dblMaxY = dblTemp;
        }
        #endregion


        private IElement m_pElement = null;
        private void btnDraw_Click(object sender, EventArgs e)
        {
            m_pGeometry = GetGeometryByCoor();

            if (m_pGeometry.IsEmpty)
            {
                MessageBox.Show("输入坐标范围为空。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //删除掉上一次的
            if (m_pMap == null) return;
            IGraphicsContainer pMapGraphics = (IGraphicsContainer)m_pMap.Map;
            if (m_pElement != null) pMapGraphics.DeleteElement(m_pElement);

            //绘制
            m_pElement=SysCommon.Gis.ModGisPub.DoDrawGeometry(m_pMap, m_pGeometry, 128, 128, 128, false);
        }

        private void frmCoorEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_pMap == null) return;
            IGraphicsContainer pMapGraphics = (IGraphicsContainer)m_pMap.Map;
            if (m_pElement != null) pMapGraphics.DeleteElement(m_pElement);
        }
    }
}