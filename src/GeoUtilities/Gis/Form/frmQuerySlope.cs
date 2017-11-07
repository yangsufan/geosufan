using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Analyst3D;

namespace GeoUtilities
{
    public partial class frmQuerySlope : DevComponents.DotNetBar.Office2007Form
    {
        //当前需要进行统计的SurFace
        public ISurface m_SurFace;
        ////统计范围
        //private IPolygon m_StaPolygon = null;
        public ILayer m_Layer;//当前选中的图层
        private ToolQuerySlope pToolQuerySlope = new ToolQuerySlope();
        private ESRI.ArcGIS.Geometry.IPoint m_SlopePoint = new PointClass();
        private const string sPolyOutlineName = "_POLYOUTLINE_";
        private ISpatialReference m_SpatialReference;
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public frmQuerySlope(string strText)
        {
            InitializeComponent();
            this.Text = strText+"查询";
            m_SurFace = null;
            m_SlopePoint = null;
            labelX2.Text = strText + "值：";
        }
        ESRI.ArcGIS.Controls.IMapControlDefault m_pMapControlDefault = null;
        public ESRI.ArcGIS.Controls.IMapControlDefault pMapControlDefault//获取sceneControl 的值 
        {
            set { m_pMapControlDefault = value; }                  //张琪// 20110630
        }
        //初始化 comboBoxOpen选项
        public void initialization()
        {
            m_SpatialReference = m_pMapControlDefault.Map.SpatialReference;
            if (m_pMapControlDefault.Map.LayerCount == 0)
            {
                return;
            }
            for (int i = 0; i < m_pMapControlDefault.Map.LayerCount; i++)
            {
                ILayer pLayer = m_pMapControlDefault.Map.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is IGroupLayer)
                    {
                        ICompositeLayer pComLyr = pLayer as ICompositeLayer;
                        for (int j = 0; j < pComLyr.Count; j++)
                        {
                            ILayer pTempLyr = pComLyr.get_Layer(j);
                            if (pTempLyr is IRasterLayer)
                            {
                                comboBoxOpen.Items.Add(pTempLyr.Name);
                                //comboBoxOpen_SelectedIndexChanged();

                            }
                            else if (pTempLyr is ITinLayer)
                            {
                                comboBoxOpen.Items.Add(pTempLyr.Name);
                            }
                            if (comboBoxOpen.Items.Count > 0)
                            {
                                comboBoxOpen.SelectedIndex = 0;
                                break;
                            }
                        }

                    }
                    else
                    {
                        if (pLayer is IRasterLayer)
                        {
                            comboBoxOpen.Items.Add(pLayer.Name);
                            //comboBoxOpen_SelectedIndexChanged();

                        }
                        else if (pLayer is ITinLayer)
                        {
                            comboBoxOpen.Items.Add(pLayer.Name);
                        }
                        if (comboBoxOpen.Items.Count > 0)
                        {
                            comboBoxOpen.SelectedIndex = 0;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 通过图层名获取图层
        /// </summary>
        /// <param name="sLayerName"></param>
        /// <returns></returns>
        private ILayer GetLayerByName(ref string sLayerName)
        {
            int i;
            int iCount;
            iCount = m_pMapControlDefault.Map.LayerCount;
            for (i = 0; i < iCount; i++)
            {
                ILayer iLayer = m_pMapControlDefault.Map.get_Layer(i);
                if (iLayer is IGroupLayer)
                {
                    ICompositeLayer pComLyr = iLayer as ICompositeLayer;
                    for (int j = 0; j < pComLyr.Count; j++)
                    {
                        ILayer pTempLyr = pComLyr.get_Layer(j);
                        if (pTempLyr.Name == sLayerName)
                        {
                            return iLayer = pTempLyr;
                        }
                    }

                }
                else
                {
                    if (iLayer.Name == sLayerName)
                    {
                        return iLayer;
                    }
                }
            }

            return null;
        }
        private void comboBoxOpen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxOpen.Items.Count > 0)
                {
                    label.Text = "";
                    DrawGeo.Enabled = false;
                    string LayerName = comboBoxOpen.SelectedItem.ToString();
                    ILayer pLayer = GetLayerByName(ref LayerName);

                    if (pLayer is IRasterLayer)//读取Raster数据的ISurface
                    {
                        IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                        IGeoDataset pDataset = pRasterLayer as IGeoDataset;
                        ISpatialReference psr = pDataset.SpatialReference;
                        //   ZQ   20110808   为处理地理坐标系与投影坐标系单位的转换
                        if (psr is IProjectedCoordinateSystem)
                        {
                            label.Text = "该数据为地理坐标，计算结果无效仅供参考！";

                        }
                        else if (psr is IGeographicCoordinateSystem)
                        {
                            //m_pMapControlDefault.MapUnits.ToString();
                        }
                        //
                        IRasterSurface pRasterSurface = new RasterSurfaceClass();
                        pRasterSurface.PutRaster(pRasterLayer.Raster, 0);
                        m_SurFace = pRasterSurface as ISurface;

                    }
                    else if (pLayer is ITinLayer)//读取TIN数据的ISurface
                    {
                        ITinLayer pTinLayer = pLayer as ITinLayer;
                        IGeoDataset pDataset = pTinLayer as IGeoDataset;
                        ISpatialReference psr = pDataset.SpatialReference;

                        if (!(psr is IProjectedCoordinateSystem))
                        {
                            label.Text = "该数据为地理坐标，计算结果无效仅供参考！";

                        }
                        m_SurFace = pTinLayer.Dataset as ISurface;


                    }
                    if (m_SurFace == null)
                    {
                        MessageBox.Show("请选择需要进行分析的DEM数据", "提示！");
                        return;
                    }

                    DrawGeo.Enabled = true;
                }
            }
            catch
            {
                return;
            }
        }

        private void comboBoxOpen_DropDown(object sender, EventArgs e)
        {
            comboBoxOpen.Items.Clear();
            //m_StaPolygon = null;
            ILayer pLayer;
            for (int i = 0; i < m_pMapControlDefault.Map.LayerCount; i++)
            {
                pLayer = m_pMapControlDefault.Map.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is IGroupLayer)
                    {

                        ICompositeLayer pComLyr = pLayer as ICompositeLayer;
                        for (int j = 0; j < pComLyr.Count; j++)
                        {
                            ILayer pTempLyr = pComLyr.get_Layer(j);
                            if (pTempLyr is IRasterLayer)
                            {
                                comboBoxOpen.Items.Add(pTempLyr.Name);

                            }
                            else if (pTempLyr is ITinLayer)
                            {
                                comboBoxOpen.Items.Add(pTempLyr.Name);

                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (pLayer is IRasterLayer)
                        {
                            comboBoxOpen.Items.Add(pLayer.Name);

                        }
                        else if (pLayer is ITinLayer)
                        {
                            comboBoxOpen.Items.Add(pLayer.Name);

                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }

            }
        }

        private void DrawGeo_Click(object sender, EventArgs e)
        {
            //新建三维绘制工具
            ClsMarkDraw.DeleteAllElementsWithName(m_pMapControlDefault.Map, sPolyOutlineName);
            m_pMapControlDefault.Refresh(esriViewDrawPhase.esriViewBackground, null, null);
            pToolQuerySlope.EndDtrawd += new myEventHandlerSlope(pToolQuerySlope_EndDtrawd);
            pToolQuerySlope.pSurface = m_SurFace;
            ICommand pCommad = pToolQuerySlope;
            pCommad.OnCreate(m_pMapControlDefault.Object);
            m_pMapControlDefault.CurrentTool = pCommad as ITool;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(this.Text);
            }
            
        }

        private void pToolQuerySlope_EndDtrawd(bool BeginDraw)
        {
            if (BeginDraw)
            {
                m_SlopePoint = null;
                m_SlopePoint = pToolQuerySlope.m_Geometry;
                if (this.Text == "坡度查询")
                {
                    txtSlope.Text = m_SurFace.GetSlopeDegrees(m_SlopePoint).ToString();
                }
                else if(this.Text=="坡向查询")
                {
                    txtSlope.Text = m_SurFace.GetAspectDegrees(m_SlopePoint).ToString();
                }
            }
        }

        private void frmQuerySlope_FormClosing(object sender, FormClosingEventArgs e)
        {

            ClsMarkDraw.DeleteAllElementsWithName(m_pMapControlDefault.Map, sPolyOutlineName);//清除绘制的要素
            if(m_SpatialReference!=null)
            {
                m_pMapControlDefault.SpatialReference = m_SpatialReference;
            }
            m_pMapControlDefault.Refresh(esriViewDrawPhase.esriViewBackground, null, null);
            m_SurFace = null;
            m_pMapControlDefault.CurrentTool = null;
            this.Dispose();
        }
    }
}
