using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;

namespace GeoArcScene3DAnalyse
{
    public partial class frm3DVolumeAreaSta : DevComponents.DotNetBar.Office2007Form
    {
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
        private const string sRefPlaneName = "_ReferancePlane_";

        private const  string ContourName = "_Contour_";
        private  bool  m_IsTin;//对于tin数据而言可以统计任意范围的面积和体积
        Tool3DDrawGeo pTool3DDrawGeo = new Tool3DDrawGeo();
        //当前需要进行统计的SurFace
        public ISurface m_SurFace;  
        //统计范围
        private IPolygon m_StaPolygon =null; 
        
        //当前范围
        private IPolygon m_ScenePoly = null;
     
    
        //绘制对象
        //private  WithEvents m_pDrawGeometry 
        public frm3DVolumeAreaSta()
        {
            InitializeComponent();
            m_SurFace = null;
            m_StaPolygon = null;
            DrawGeo.Enabled = false;
            txtPlaneHeight.ReadOnly = true;
            sldPlaneHeight.Enabled = false;
        }
        ESRI.ArcGIS.Controls.ISceneControl m_pCurrentSceneControl = null;
        public ESRI.ArcGIS.Controls.ISceneControl CurrentSceneControl//获取sceneControl 的值 
        {
            set { m_pCurrentSceneControl = value; }                  //张琪// 20110623
        }
        public void initialization()
        {
            if (m_pCurrentSceneControl.Scene.LayerCount == 0)
            {
                return;
            }
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                ILayer pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
                {
                    if (pLayer is ITinLayer)
                    {
                        comboBoxOpen.Items.Add(pLayer.Name);
                        //comboBoxOpen_SelectedIndexChanged();

                    }
                    else if (pLayer is IRasterLayer)
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

        /// <summary>
        /// 通过图层名获取图层
        /// </summary>
        /// <param name="sLayerName"></param>
        /// <returns></returns>
        private ILayer GetLayerByName(ref string sLayerName)
        {
            int i;
            int iCount;
            iCount = m_pCurrentSceneControl.Scene.LayerCount;
            for (i = 0; i < iCount; i++)
            {
                ILayer iLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (iLayer.Name == sLayerName)
                {
                    return iLayer;
                }
            }

            return null;
        }
        /// <summary>
        /// 面积、体积统计及三维效果绘制    张琪  20110629
        /// </summary>
        private void StaTisTic()
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            try
            {
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在计算");
                object before = Type.Missing;
                object after = Type.Missing;
                if (m_StaPolygon == null)
                {
                    txtArea2DSel.Text = m_SurFace.GetProjectedArea(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceAbove).ToString();
                    txtAreaAbove.Text = m_SurFace.GetSurfaceArea(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceAbove).ToString();
                    txtAreaBelow.Text = m_SurFace.GetSurfaceArea(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceBelow).ToString();
                    txtVolumeBelow.Text = m_SurFace.GetVolume(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceBelow).ToString();
                    txtVolumeAbove.Text = m_SurFace.GetVolume(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceAbove).ToString();
                }
                else//在地图上绘制多边形时
                {
                    if (m_IsTin)//为true是指数据为TIN
                    {
                        ITinSurface pTinsurface = m_SurFace as ITinSurface;
                        ITinAdvanced pTinAdvanced = pTinsurface as ITinAdvanced;
                        ////ITriangleLabelInPolyFilter  = new 
                        //ITinFilter pTCFilterBasic = pTCFilter as ITinFilter;
                        //pTCFilterBasic.DataElementsOnly = true;
                        int Criteria = 4;
                        IEnumTinTriangle pEnumTriangles = pTinAdvanced.MakeTriangleEnumerator(m_StaPolygon.Envelope, Criteria, null);
                        object dVolumeBelow = null;
                        object dSurfaceArea = null;
                        object dProjectedArea = null;
                        //根据绘制的多边形和Z值来计算基准面以上的面积和体积
                        pTinsurface.GetPartialVolumeAndArea(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceAbove, pEnumTriangles, ref dVolumeBelow, ref dSurfaceArea, ref dProjectedArea);
                        txtAreaAbove.Text = Convert.ToDouble(dSurfaceArea).ToString();
                        txtVolumeAbove.Text = dVolumeBelow.ToString();
                        txtArea2DSel.Text = dProjectedArea.ToString();
                        //根据绘制的多边形和Z值来计算基准面以下的面积和体积
                        pTinsurface.GetPartialVolumeAndArea(Convert.ToDouble(txtPlaneHeight.Text), esriPlaneReferenceType.esriPlaneReferenceBelow, pEnumTriangles, ref dVolumeBelow, ref dSurfaceArea, ref dProjectedArea);
                        txtAreaBelow.Text = dSurfaceArea.ToString();
                        txtVolumeBelow.Text = dVolumeBelow.ToString();
                    }
                    else
                    {
                        MessageBox.Show("抱歉该功能暂不支持Raster数据","提示！");
                      
                        vProgress.Close();
                        return;
                    }

                }

                IPointCollection pPolygon;
                ESRI.ArcGIS.Geometry.IPoint pPoint = new ESRI.ArcGIS.Geometry.PointClass();
                //IPolygon pOutPoly;
                if (m_StaPolygon == null)//当绘制范围为空，获取整张图的范围
                {
                    pPolygon = new PolygonClass();
                    IZAware pZawre;
                    pPoint.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;
                    pPoint.X = m_SurFace.Domain.Envelope.XMin;
                    pPoint.Y = m_SurFace.Domain.Envelope.YMin;
                    pZawre = pPoint as IZAware;
                    pZawre.ZAware = true;
                    pPoint.Z = Convert.ToDouble(txtPlaneHeight.Text);
                    pPolygon.AddPoint(pPoint, ref before, ref after);

                    pPoint = new ESRI.ArcGIS.Geometry.PointClass();
                    pZawre = pPoint as IZAware;
                    pZawre.ZAware = true;
                    pPoint.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;
                    pPoint.X = m_SurFace.Domain.Envelope.XMax;
                    pPoint.Y = m_SurFace.Domain.Envelope.YMin;
                    pPoint.Z = Convert.ToDouble(txtPlaneHeight.Text);
                    pPolygon.AddPoint(pPoint, ref before, ref after);

                    pPoint = new ESRI.ArcGIS.Geometry.PointClass();
                    pZawre = pPoint as IZAware;
                    pZawre.ZAware = true;
                    pPoint.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;
                    pPoint.X = m_SurFace.Domain.Envelope.XMax;
                    pPoint.Y = m_SurFace.Domain.Envelope.YMax;
                    pPoint.Z = Convert.ToDouble(txtPlaneHeight.Text);
                    pPolygon.AddPoint(pPoint, ref before, ref after);

                    pPoint = new ESRI.ArcGIS.Geometry.PointClass();
                    pZawre = pPoint as IZAware;
                    pZawre.ZAware = true;
                    pPoint.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;
                    pPoint.X = m_SurFace.Domain.Envelope.XMin;
                    pPoint.Y = m_SurFace.Domain.Envelope.YMax;
                    pPoint.Z = Convert.ToDouble(txtPlaneHeight.Text);
                    pPolygon.AddPoint(pPoint, ref before, ref after);

                    pPoint = new ESRI.ArcGIS.Geometry.PointClass();
                    pZawre = pPoint as IZAware;
                    pZawre.ZAware = true;
                    pPoint.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;
                    pPoint.X = m_SurFace.Domain.Envelope.XMin;
                    pPoint.Y = m_SurFace.Domain.Envelope.YMin;
                    pPoint.Z = Convert.ToDouble(txtPlaneHeight.Text);
                    pPolygon.AddPoint(pPoint, ref before, ref after);
                    //获得整张图范围组成的多边形
                    IGeometry pGeometry = pPolygon as IGeometry;
                    pGeometry.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;

                }
                else
                {
                    pPolygon = m_StaPolygon as IPointCollection;
                }

                if (pPolygon == null)
                {
                    vProgress.Close();
                    return;
                }
                //显示参考面
                ShowRefPlane(pPolygon as IPolygon);
                //显示周边
                ShowCountour(pPolygon as IPolygon);
                //获取轮廓线的周长
                m_ScenePoly = pPolygon as IPolygon;
                ITopologicalOperator pTopo = pPolygon as ITopologicalOperator;
                IPolyline pPolyLine = pTopo.Boundary as IPolyline;
                txtPara.Text = pPolyLine.Length.ToString();
                vProgress.Close();
            }
            catch
            {
               
                vProgress.Close();
            }
          
        }
        /// <summary> 
        /// 显示参考面     张琪    20110629
        /// </summary>
        /// <param name="pPoly">参考多边形</param>
        private void ShowRefPlane(IPolygon pPoly)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            try
            {
              
                IGeometry pBoundary = new PolylineClass();
                pBoundary = null;
                IGeometry pPolygon = new PolygonClass();
                pPolygon = null;
                object StepSize = Type.Missing;
                m_SurFace.InterpolateShape(pPoly as IGeometry, out pPolygon, ref StepSize);
                Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, sRefPlaneName);//清除已有的参考面
                if (!chkShowLWRP.Checked)//当为false时不显示参考面
                {
                    m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                    return;
                }
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在绘制参考面");
                ITopologicalOperator pTopo = pPolygon as ITopologicalOperator;
                pBoundary = pTopo.Boundary as IPolyline;
                //设置Z值方向显示
                IZAware pZAware = pBoundary as IZAware;
                pZAware.ZAware = true;
                m_SurFace.InterpolateShape(pBoundary as IGeometry, out pBoundary, ref StepSize);
                m_SurFace.InterpolateShape(pPoly as IGeometry, out pPolygon, ref StepSize);
                IExtrude pExtrude = new GeometryEnvironmentClass();
                //获取参考面的多面体
                IMultiPatch pMultiPatchRefPlaneWall = pExtrude.ExtrudeAbsolute(Convert.ToDouble(txtPlaneHeight.Text), pBoundary as IGeometry) as IMultiPatch;
                pMultiPatchRefPlaneWall.SpatialReference = m_pCurrentSceneControl.Scene.SpatialReference;
                IZ pZ;
                pZAware = pPolygon as IZAware;
                pZAware.ZAware = true;
                pZ = pPolygon as IZ;
                pZ.SetConstantZ(Convert.ToDouble(txtPlaneHeight.Text));
                //用于绘制三维效果
                IGroupElement pGroup = null;
                Cls3DMarkDraw.AddSimpleGraphic(pMultiPatchRefPlaneWall as IGeometry, Cls3DMarkDraw.getRGB(71, 61, 255), 1, sRefPlaneName, m_pCurrentSceneControl.Scene, pGroup);
                Cls3DMarkDraw.AddSimpleGraphic(pPolygon as IGeometry, Cls3DMarkDraw.getRGB(71, 61, 255), 1, sRefPlaneName, m_pCurrentSceneControl.Scene, pGroup);
                m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                vProgress.Close();
            }
            catch
            {
                vProgress.Close();
            }
        }
        /// <summary>
        /// 显示轮廓线   张琪  20110629
        /// </summary>
        /// <param name="pPolygon"></param>
        private void ShowCountour(IPolygon pPolygon)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            try
            {
              
                Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, ContourName);
                if (!chkShowContour.Checked)
                {
                    m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                    return;
                }
                vProgress.EnableCancel = false;//设置进度条
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("正在绘制轮廓线");
                ITopologicalOperator pTopo = pPolygon as ITopologicalOperator;
                IGeometry pPolyLine = new PolylineClass();
                pPolyLine = pTopo.Boundary;
                object StepSize = Type.Missing; ;
                m_SurFace.InterpolateShape(pPolyLine as IGeometry, out pPolyLine, ref StepSize);
                IGroupElement pGroup = null;
                //用于绘制三维效果
                Cls3DMarkDraw.AddSimpleGraphic(pPolyLine as IGeometry, Cls3DMarkDraw.getRGB(30, 255, 255), 4, ContourName, m_pCurrentSceneControl.Scene, pGroup);
                m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                vProgress.Close();
            }
            catch
            {
                vProgress.Close();
            }
        }
        /// <summary>
        /// 下拉获取地图中的图层     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOpen_DropDown(object sender, EventArgs e)
        {
            comboBoxOpen.Items.Clear();
            m_StaPolygon = null;
            ILayer pLayer;
            for (int i = 0; i < m_pCurrentSceneControl.Scene.LayerCount; i++)
            {
                pLayer = m_pCurrentSceneControl.Scene.get_Layer(i);
                if (pLayer.Valid)
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
                else
                {
                    continue;
                }
                  
            }
        }
        /// <summary>
        /// 获取分析图层及Surfer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOpen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxOpen.Items.Count > 0)
                {
                    label.Text = "";
                    Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, sRefPlaneName);
                    Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, ContourName);
                    m_pCurrentSceneControl.SceneGraph.RefreshViewers();
                    m_ScenePoly = null;
                    m_StaPolygon = null;
                    m_SurFace = null;
                    txtArea2DSel.Text = "";
                    txtAreaAbove.Text = "";
                    txtAreaBelow.Text = "";
                    txtPara.Text = "";
                    txtPlaneHeight.Text = "";
                    txtVolumeAbove.Text = "";
                    txtVolumeBelow.Text = "";

                    DrawGeo.Enabled = false;
                    sldPlaneHeight.Enabled = false;
                    btncount.Enabled = false;
                    string LayerName = comboBoxOpen.SelectedItem.ToString();
                    ILayer pLayer = GetLayerByName(ref LayerName);

                    if (pLayer is IRasterLayer)//读取Raster数据的ISurface
                    {
                        IRasterLayer pRasterLayer = pLayer as IRasterLayer;
                        IGeoDataset pDataset = pRasterLayer as IGeoDataset;
                        ISpatialReference psr = pDataset.SpatialReference;

                        if (!(psr is IProjectedCoordinateSystem))
                        {
                            label.Text = "该数据为地理坐标，计算结果无效仅供参考！";

                        }

                        IRasterSurface pRasterSurface = new RasterSurfaceClass();
                        pRasterSurface.PutRaster(pRasterLayer.Raster, 0);
                        m_SurFace = pRasterSurface as ISurface;
                        DrawGeo.Enabled = false;
                        m_IsTin = false;
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
                        m_IsTin = true;
                        DrawGeo.Enabled = true;
                    }
                    if (m_SurFace == null)
                    {
                        MessageBox.Show("请选择需要进行分析的DEM数据", "提示！");
                        return;
                    }
                    if (m_StaPolygon == null)
                    {
                        if (m_SurFace is ITin)
                        {
                            ITin pTin = m_SurFace as ITin;
                            sldPlaneHeight.Maximum = Convert.ToInt32(pTin.Extent.ZMax);
                            sldPlaneHeight.Minimum = Convert.ToInt32(pTin.Extent.ZMin);
                        }
                        else
                        {
                            IRasterSurface pRasterSurface = m_SurFace as IRasterSurface;
                            IRasterStatistics pRasterStatistic = pRasterSurface.RasterBand.Statistics;
                            sldPlaneHeight.Maximum = Convert.ToInt32(pRasterStatistic.Maximum);
                            sldPlaneHeight.Minimum = Convert.ToInt32(pRasterStatistic.Minimum);
                        }
                        double Vale = sldPlaneHeight.Maximum + sldPlaneHeight.Minimum;
                        Vale = Vale / 2;
                        txtPlaneHeight.Text = Vale.ToString();
                        Vale = Convert.ToDouble(txtPlaneHeight.Text);
                        sldPlaneHeight.Value = Convert.ToInt32(Vale);
                    }
                    else
                    {
                        double zrange;
                        zrange = m_StaPolygon.Envelope.ZMax - m_StaPolygon.Envelope.ZMin;
                        sldPlaneHeight.Maximum = Convert.ToInt32(m_StaPolygon.Envelope.ZMax + zrange);
                        sldPlaneHeight.Minimum = Convert.ToInt32(m_StaPolygon.Envelope.ZMin);
                        sldPlaneHeight.Value = Convert.ToInt32(Math.Round(m_StaPolygon.Envelope.ZMin + (zrange * 0.5)));
                        txtPlaneHeight.Text = sldPlaneHeight.Value.ToString();

                    }
                    sldPlaneHeight.Enabled = true;
                    btncount.Enabled = true;
                }

            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 限制用户只能输入"0123456789."
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPlaneHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strnum = "0123456789.";
            if (!char.IsControl(e.KeyChar) && (!strnum.Contains(e.KeyChar.ToString())))
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 控制轮廓线的显隐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowContour_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_ScenePoly == null)
                {
                    return;
                }
                ShowCountour(m_ScenePoly);
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// 控制参考面的显隐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowLWRP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_ScenePoly == null)
                {
                    return;
                }
                ShowRefPlane(m_ScenePoly);
            }
            catch
            {
                return;
            }
        }
       
        private void frm3DVolumeAreaSta_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, sRefPlaneName);
            Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, ContourName);
            m_pCurrentSceneControl.CurrentTool = null;//释放使用工具
            m_ScenePoly = null;
            m_StaPolygon = null;
            m_SurFace = null;
            this.Dispose();
        }
        /// <summary>
        /// 实例化绘制范围工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawGeo_Click(object sender, EventArgs e)
        {
            btncount.Enabled = false;
            Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, sRefPlaneName);
            Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, ContourName);
            m_pCurrentSceneControl.SceneGraph.RefreshViewers();
            //pTool3DDrawGeo.BeginDrawed += new myEventHandler(pTool3DDrawGeo_BeginDrawed);
            pTool3DDrawGeo.EndDtrawd += new myEventHandler(pTool3DDrawGeo_EndDtrawd);
            pTool3DDrawGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
            pTool3DDrawGeo.pSurface = m_SurFace;
            ICommand pCommad = pTool3DDrawGeo;
            pCommad.OnCreate(m_pCurrentSceneControl.Object);
            m_pCurrentSceneControl.CurrentTool = pCommad as ITool;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("局部填挖方分析,表面集为:" + comboBoxOpen.Text + ",基准面高度为:" + txtPlaneHeight.Text);
            }
        }

        private void pTool3DDrawGeo_EndDtrawd(bool BeginDraw)
        {
            if (BeginDraw)
            {
                m_StaPolygon = pTool3DDrawGeo.m_Geometry as IPolygon;
                m_pCurrentSceneControl.CurrentTool = null;
                StaTisTic();
            }
        }
        /// <summary>
        /// 基准面高度变化时，相应的计算统计结果    
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sldPlaneHeight_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                txtPlaneHeight.Text = sldPlaneHeight.Value.ToString();
                StaTisTic();
            }
            catch
            {
                return;
            }
        }

        private void btncount_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_SurFace == null)
                {
                    return;
                }

                StaTisTic();
                btncount.Enabled = false;
            }
            catch
            {
                return;
            }

        }
                                                               
    }
}
