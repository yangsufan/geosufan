using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Display;

namespace GeoProperties.UserControls
{
    public partial class ucSpatialIndex : UserControl
    {
        private ILayer m_pLyr = null;

        public ucSpatialIndex(ILayer pLyr)
        {
            InitializeComponent();

            m_pLyr = pLyr;
            bool blnFeatureLyr = false;

            //
            if (pLyr is IFeatureLayer)
            {
                IFeatureLayer pFeaLyr = pLyr as IFeatureLayer;
                IFeatureClass pFeaCls = pFeaLyr.FeatureClass;
                if (pFeaCls.FeatureType == esriFeatureType.esriFTSimple || pFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    m_pFeaCls = pFeaCls;
                    blnFeatureLyr = true;
                }
            }

            //设置界面
            if (!blnFeatureLyr)
            {
                this.btnCalculate.Enabled = false;
                this.btnDel.Enabled = false;
            }

        }

        private IFeatureClass m_pFeaCls = null;
        public IFeatureClass FeaCls
        {
            set { m_pFeaCls = value; }
        }

        private void InitGridSize()
        {
            if (m_pFeaCls == null) return;

            //获得igeometrydef
            string strShapeName = m_pFeaCls.ShapeFieldName;
            IField pShapeField =m_pFeaCls.Fields.get_Field(m_pFeaCls.FindField(m_pFeaCls.ShapeFieldName));
            if (pShapeField == null) return;
            IGeometryDef pGeometryDef = pShapeField.GeometryDef as IGeometryDef;

            //获得索引信息
            int intGridCount = pGeometryDef.GridCount;
            for (int i = 0; i < pGeometryDef.GridCount; i++)
            {
                double dblGridSize = 0;
                dblGridSize = pGeometryDef.get_GridSize(i);

                switch (i)
                {
                    case 0:
                        this.txtGridSize1.Text = dblGridSize.ToString();
                        break;
                    case 1:
                        this.txtGridSize2.Text = dblGridSize.ToString();
                        break;
                    case 2:
                        this.txtGridSize2.Text = dblGridSize.ToString();
                        break;
                }
            }
        }

        private void InitRasterInfo()
        {
            if (m_pLyr == null) return;
            ILayer pLayer = m_pLyr;

            if (!(pLayer is IRasterLayer) && !(pLayer is IRasterCatalogLayer)) return;

            //获得当前图层的符号
            IRasterRenderer pRasterRender = null;
            if (pLayer is IRasterLayer)
            {
                IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                pRasterRender = pRasterLayer.Renderer;

            }
            else if (pLayer is IRasterCatalogLayer)
            {
                IRasterCatalogLayer pRasterCatalogLayer = pLayer as IRasterCatalogLayer;
                pRasterRender = pRasterCatalogLayer.Renderer;
            }
            else
            {
                return;
            }

            if (pRasterRender == null) return;
            IRasterDisplayProps pRasterDisplayProps = pRasterRender as IRasterDisplayProps;
            if (pRasterDisplayProps == null) return;
            IColor pColor = pRasterDisplayProps.NoDataColor;

            //设置界面
            this.colorNoData.SelectedColor = System.Drawing.ColorTranslator.FromOle(pColor.RGB);
            this.sliderTransparency.Value = (int)pColor.Transparency;
        }

        private void ucSpatialIndex_Load(object sender, EventArgs e)
        {
            InitGridSize();
            InitRasterInfo();
        }



        public void SetGridSize()
        {
            //简单判断下
            try
            {
                if ((m_dblTempGridIndex1 != 0) && (m_dblTempGridIndex1 <= m_dblTempGridIndex2)) return;
                if ((m_dblTempGridIndex2 != 0) && (m_dblTempGridIndex2 < m_dblTempGridIndex3)) return;

                if (m_pFeaCls == null) return;

                //获得igeometrydef
                string strShapeName = m_pFeaCls.ShapeFieldName;
                IField pShapeField = m_pFeaCls.Fields.get_Field(m_pFeaCls.FindField(m_pFeaCls.ShapeFieldName));
                if (pShapeField == null) return;
                IGeometryDefEdit pGeometryDefEdit = pShapeField.GeometryDef as IGeometryDefEdit;

                //根据设置值进行设置
                if (m_dblTempGridIndex1 == 0)
                {
                    pGeometryDefEdit.GridCount_2 = 0;
                    return;
                }
                if (m_dblTempGridIndex2 == 0)
                {
                    pGeometryDefEdit.GridCount_2 = 1;
                    pGeometryDefEdit.set_GridSize(0, m_dblTempGridIndex1);
                    return;
                }

                if (m_dblTempGridIndex3 == 0)
                {
                    pGeometryDefEdit.GridCount_2 = 2;
                    pGeometryDefEdit.set_GridSize(0, m_dblTempGridIndex1);
                    pGeometryDefEdit.set_GridSize(1, m_dblTempGridIndex2);

                    return;
                }

                pGeometryDefEdit.GridCount_2 = 3;
                pGeometryDefEdit.set_GridSize(0, m_dblTempGridIndex1);
                pGeometryDefEdit.set_GridSize(1, m_dblTempGridIndex2);
                pGeometryDefEdit.set_GridSize(2, m_dblTempGridIndex3);
            }
            catch (Exception ex)
            {
                MessageBox.Show("空间索引设置出现错误" + Environment.NewLine + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void SetRasterNoDataColor()
        {
            try
            {
                if (m_pLyr == null) return;
                ILayer pLayer = m_pLyr;

                if (!(pLayer is IRasterLayer) && !(pLayer is IRasterCatalogLayer)) return;

                //获得当前图层的符号
                IRasterRenderer pRasterRender = null;
                if (pLayer is IRasterLayer)
                {
                    IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                    pRasterRender = pRasterLayer.Renderer;

                }
                else if (pLayer is IRasterCatalogLayer)
                {
                    IRasterCatalogLayer pRasterCatalogLayer = pLayer as IRasterCatalogLayer;
                    pRasterRender = pRasterCatalogLayer.Renderer;
                }
                else
                {
                    return;
                }

                if (pRasterRender == null) return;
                IRasterDisplayProps pRasterDisplayProps = pRasterRender as IRasterDisplayProps;
                if (pRasterDisplayProps == null) return;

                //获得当前的颜色值 并设置
                IColor pColor = ConvertColorToIColor(this.colorNoData.SelectedColor);
                pColor.Transparency = (byte)this.sliderTransparency.Value;

                pRasterDisplayProps.NoDataColor = pColor;
            }
            catch
            {
                
            }
        }

        //删除
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (m_pFeaCls == null) return;

            this.txtGridSize1.Text = "0";
            this.txtGridSize2.Text = "0";
            this.txtGridSize3.Text = "0";
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //计算默认索引
            try
            {
                ESRI.ArcGIS.DataManagementTools.CalculateDefaultGridIndex vCalculateDefault = new ESRI.ArcGIS.DataManagementTools.CalculateDefaultGridIndex();
                vCalculateDefault.in_features = m_pFeaCls;

                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true;

                gp.Execute(vCalculateDefault, null);

                this.txtGridSize1.Text = vCalculateDefault.grid_index1.ToString();
                this.txtGridSize2.Text = vCalculateDefault.grid_index2.ToString();
                this.txtGridSize3.Text = vCalculateDefault.grid_index3.ToString();
            }
            catch
            {
            }

            this.Cursor = Cursors.Default;
        }

        private double m_dblTempGridIndex1 = 0;
        private void txtGridSize1_TextChanged(object sender, EventArgs e)
        {
            double dblTemp=0;
            if(!double.TryParse(this.txtGridSize1.Text,out dblTemp))
            {
                this.txtGridSize1.Text=m_dblTempGridIndex1.ToString();
                return;
            }

            m_dblTempGridIndex1=dblTemp;
        }
        private double m_dblTempGridIndex2 = 0;
        private void txtGridSize2_TextChanged(object sender, EventArgs e)
        {
            double dblTemp=0;
            if(!double.TryParse(this.txtGridSize2.Text,out dblTemp))
            {
                this.txtGridSize2.Text=m_dblTempGridIndex2.ToString();
                return;
            }

            m_dblTempGridIndex2=dblTemp;
        }

        private double m_dblTempGridIndex3 = 0;
        private void txtGridSize3_TextChanged(object sender, EventArgs e)
        {
            double dblTemp=0;
            if(!double.TryParse(this.txtGridSize3.Text,out dblTemp))
            {
                this.txtGridSize3.Text=m_dblTempGridIndex3.ToString();
                return;
            }

            m_dblTempGridIndex3=dblTemp;
        }

        private void btnSetNoDataColor_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 转为ESRI的颜色值
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IColor ConvertColorToIColor(System.Drawing.Color color)
        {

            IColor pColor = new RgbColorClass();

            pColor.RGB = color.B * 65536 + color.G * 256 + color.R;

            return pColor;

        }

    }
}
