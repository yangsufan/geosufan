using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Analyst3D;
//剖面分析   张琪   20110808
namespace GeoUtilities
{
    public partial class frm3DSection : DevComponents.DotNetBar.Office2007Form
    {
        //当前需要进行统计的SurFace
        public ISurface m_SurFace;
        ////统计范围
        //private IPolygon m_StaPolygon = null;
        public ILayer m_Layer;//当前选中的图层
        private  ToolDrawGeo pToolDrawGeo = new ToolDrawGeo();
        private IGeometry  m_SectionLine = new PolylineClass();
        private const string sPolyOutlineName = "_POLYOUTLINE_";
        private ISpatialReference m_SpatialReference;
        public frm3DSection()
        {
            InitializeComponent();
            m_SurFace = null;
            //m_StaPolygon = null;
            DrawGeo.Enabled = false;
            axBtnOK.Enabled = false;
            m_SectionLine = null;
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
            if ( m_pMapControlDefault.Map.LayerCount == 0)
            {
                return;
            }
            for (int i = 0; i < m_pMapControlDefault.Map.LayerCount; i++)
            {
                ILayer pLayer = m_pMapControlDefault.Map.get_Layer(i);
                if (pLayer.Valid)
                {
                    if(pLayer is IGroupLayer)
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
        private void DrawSectionPart()
        {
            try
            {
                if (m_SectionLine == null)
                {
                    return;
                }
                if (m_SurFace == null)
                {
                    return;
                }

                IGeometry pGeometry = new PolylineClass();
                object pObject = 1;
                m_SurFace.InterpolateShape(m_SectionLine, out pGeometry, ref pObject);
                IGroupElement pGroup = null;
                ////用于绘制三维效果
                ClsMarkDraw.AddSimpleGraphic(pGeometry, ClsMarkDraw.getRGB(71, 61, 255), 2, sPolyOutlineName, m_pMapControlDefault.Map, pGroup);
                m_pMapControlDefault.Refresh(esriViewDrawPhase.esriViewBackground, null, null);

                double dblLength;
                object StepSize = Type.Missing; ;
                m_SurFace.QuerySurfaceLength(pGeometry, out  dblLength, ref StepSize);
                txtSurfaceLengh.Text = dblLength.ToString();
                IPolyline pLine = pGeometry as IPolyline;
                txtPreject.Text = pLine.Length.ToString();
                txtZmax.Text = pLine.Envelope.ZMax.ToString();
                txtZmin.Text = pLine.Envelope.ZMin.ToString();
                IPointCollection pPointCol = pLine as IPointCollection;
                IPolyline pPolyline2;
                IPointCollection pPointCol2 = new PolylineClass();
                int lLOOP;
                double dblminY, dblPery, dblPerx, dblMaxY;
                dblminY = pLine.Envelope.ZMin;
                dblMaxY = pLine.Envelope.ZMax;
                if ((pLine.Envelope.ZMax - pLine.Envelope.ZMin) == 0)
                {
                    return;
                }
                if (pLine.Length == 0)
                {
                    return;
                }

                //重新绘制底图
                Image pImage = new Bitmap(442, 230);
                Graphics m_mouse = Graphics.FromImage(pImage);
                //m_mouse.FillRectangle(Brushes.White,0,0,460,240);
                Pen pen1 = new Pen(Color.Black, 1);

                //绘制横坐标、纵坐标及其他两条边
                m_mouse.DrawLine(pen1, new PointF(55, 210), new PointF(55, 6));
                m_mouse.DrawLine(pen1, new PointF(55, 210), new PointF(431, 210));
                m_mouse.DrawLine(pen1, new PointF(55, 6), new PointF(431, 6));
                m_mouse.DrawLine(pen1, new PointF(431, 6), new PointF(431, 210));

                //Brush pBrush = new HatchBrush(HatchStyle.Percent70, Color.WhiteSmoke);
                m_mouse.FillRectangle(Brushes.White, 56, 7, 375, 203);//填充区域及背景图
                // 
                dblPery = 200 / (pLine.Envelope.ZMax - pLine.Envelope.ZMin);//将纵坐标的高度分线段的高程差段
                dblPerx = 368 / pLine.Length;//将横坐标的宽度分成线段长度份
                double x, y, x1, y1;
                x = 0;
                y = 0;
                object before = Type.Missing;
                object after = Type.Missing;
                Pen pen3 = new Pen(Color.Red, 2);

                Font drawFont = new Font("宋体", 8.0f, FontStyle.Regular);
                StringFormat drawFormat = new StringFormat();
                Brush drawBrush =new SolidBrush(Color.Black);
                
                drawFormat.FormatFlags = StringFormatFlags.DisplayFormatControl;
                //绘制横坐标刻度值
                for (int i = 0; i <= 4; i++)
                {
                    if (i == 0)
                    {
                        m_mouse.DrawString(i.ToString(), drawFont, Brushes.Black, new PointF(52, 214), drawFormat);
                    }
                    else
                    {
                        Single Lenvlae = Convert.ToSingle((pLine.Length / 4) * i);
                        m_mouse.DrawLine(pen1, new PointF(55 + 92 * i, 210), new PointF(55 + 92 * i, 214));
                        m_mouse.DrawString(Lenvlae.ToString(), drawFont, drawBrush, new PointF(20 + 92 * i, 215), drawFormat);
                    }
                }

                //绘制纵坐标刻度值
                for (int i = 1; i <= 10; i++)
                {
                    m_mouse.DrawLine(pen1, new PointF(55, 210 - 20 * i), new PointF(51, 210 - 20 * i));
                    Pen pen = new Pen(Color.Black, 1);
                    pen.DashStyle = DashStyle.Dot;
                    m_mouse.DrawLine(pen, new PointF(55, 210 - 20 * i), new PointF(431, 210 - 20 * i));
                    Single Lenvlae = Convert.ToSingle((((dblMaxY - dblminY) / 10) * i) + dblminY);
                    m_mouse.DrawString(Lenvlae.ToString(), drawFont, Brushes.Black, new PointF(1, 210 - 21 * i), drawFormat);
                }

                for (lLOOP = 0; lLOOP < pPointCol.PointCount; lLOOP++)
                {
                    pPointCol2.AddPoint(pPointCol.get_Point(lLOOP), ref before, ref after);
                    pPolyline2 = pPointCol2 as IPolyline;
                    x1 = pPolyline2.Length * dblPerx + 57;//37为坐标图在pictureBox1中离左边的距离
                    y1 = 210 - ((pPointCol.get_Point(lLOOP).Z - pLine.Envelope.ZMin) * dblPery);
                    if (lLOOP == 0)//绘制起点
                    {
                        SolidBrush pSolidBrush = new SolidBrush(Color.Blue);
                        m_mouse.FillEllipse(pSolidBrush, Convert.ToSingle(x1 - 2), Convert.ToSingle(y1 - 2), 6, 6);
                    }
                    if (lLOOP > 0)
                    {
                        //绘制沿线高程变化线
                        m_mouse.DrawLine(pen3, new PointF(Convert.ToSingle(x), Convert.ToSingle(y)), new PointF(Convert.ToSingle(x1), Convert.ToSingle(y1)));
                    }

                    if (lLOOP == pPointCol.PointCount - 1)//绘制终点
                    {
                        SolidBrush pSolidBrush = new SolidBrush(Color.Green);
                        m_mouse.FillEllipse(pSolidBrush, Convert.ToSingle(x1 - 2), Convert.ToSingle(y1-2), 6, 6);
                    }
                    x = x1;
                    y = y1;
                }

                pictureBox1.Image = pImage;
                m_mouse.Dispose();
                drawFont.Dispose();
                drawBrush.Dispose();
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
                        else if(psr is IGeographicCoordinateSystem )
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

        private void frm3DSection_Load(object sender, EventArgs e)
        {


            Image pImage = new Bitmap(440, 230);

            Graphics m_mouse = Graphics.FromImage(pImage);
            Pen pen1 = new Pen(Color.Black, 1);

            m_mouse.DrawLine(pen1, new PointF(35, 210), new PointF(35, 8));
            //m_mouse.DrawLine(pen1, new PointF(15, 10), new PointF(10,15 ));
            //m_mouse.DrawLine(pen1, new PointF(15, 10), new PointF(20, 15));
            m_mouse.DrawLine(pen1, new PointF(35, 210), new PointF(437, 210));
            m_mouse.DrawLine(pen1, new PointF(35, 7), new PointF(438, 7));
            m_mouse.DrawLine(pen1, new PointF(438, 7), new PointF(438, 210));

            m_mouse.FillRectangle(Brushes.White, 36, 8, 401, 202);
            Font drawFont = new Font("宋体", 8.0f, FontStyle.Regular);
            StringFormat drawFormat = new StringFormat();
            //SolidBrush drawBrush = new SolidBrush(Color.Black);
            drawFormat.FormatFlags = StringFormatFlags.DisplayFormatControl;
            
            //绘制横坐标刻度
            for (int i = 0; i <= 4; i++)
            {
                if (i == 0)
                {
                    m_mouse.DrawString(i.ToString(), drawFont, Brushes.Black , new PointF(30, 214), drawFormat);
                }
                else
                {
                    m_mouse.DrawLine(pen1, new PointF(35 + 99 * i, 210), new PointF(35 + 99 * i, 214));
                    m_mouse.DrawString(i.ToString(), drawFont, Brushes.Black, new PointF(30 + 99 * i, 215), drawFormat);
                }
            }
            //绘制纵坐标刻度
            for (int i = 1; i <= 10; i++)
            {
                m_mouse.DrawLine(pen1, new PointF(35, 210 - 19 * i), new PointF(31, 210 - 19 * i));
                Pen pen = new Pen(Color.Black, 1);
                pen.DashStyle = DashStyle.Dot;
                m_mouse.DrawLine(pen, new PointF(35, 210 - 19 * i), new PointF(438, 210 - 19 * i));
            }

            pictureBox1.Image = pImage;
            m_mouse.Dispose();
            drawFont.Dispose();
           
        }

        private void DrawGeo_Click(object sender, EventArgs e)
        {
            //新建三维绘制工具
            ClsMarkDraw.DeleteAllElementsWithName(m_pMapControlDefault.Map, sPolyOutlineName);
            m_pMapControlDefault.Refresh(esriViewDrawPhase.esriViewBackground, null, null);
            pToolDrawGeo.EndDtrawd += new myEventHandler(pToolDrawGeo_EndDtrawd);
            pToolDrawGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine;
            pToolDrawGeo.pSurface = m_SurFace;
            ICommand pCommad = pToolDrawGeo;
            pCommad.OnCreate(m_pMapControlDefault.Object);
            m_pMapControlDefault.CurrentTool = pCommad as ITool;

        }

        private void pToolDrawGeo_EndDtrawd(bool BeginDraw)
        {
            if (BeginDraw)
            {
                m_SectionLine = pToolDrawGeo.m_Geometry as IPolyline;
                
                m_pMapControlDefault.CurrentTool = null;
                DrawSectionPart();
                axBtnOK.Enabled = true;
            }
        }
        private void axBtnOK_Click(object sender, EventArgs e)
        {
            SaveFileDialog pSaveFileDialog = new SaveFileDialog();
            pSaveFileDialog.Filter = "*.bmp(BMP文件)|*.bmp";//保存GRID格式的数据无需加扩展名(*.grid)(*.img)
            if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
            {
                
                return;
            }
            if (pictureBox1.Image==null)
            {
                return;
            }
            File.Delete(pSaveFileDialog.FileName);
            pictureBox1.Image.Save(pSaveFileDialog.FileName);
            MessageBox.Show("剖面图保存成功！", "提示！");
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClsMarkDraw.DeleteAllElementsWithName(m_pMapControlDefault.Map, sPolyOutlineName);//清除绘制的要素
            if (m_SpatialReference != null)
            {
                m_pMapControlDefault.Map.SpatialReference = m_SpatialReference;
            }
            m_pMapControlDefault.Refresh(esriViewDrawPhase.esriViewBackground, null, null);
            m_SurFace = null;
            m_pMapControlDefault.CurrentTool = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frm3DSection_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClsMarkDraw.DeleteAllElementsWithName(m_pMapControlDefault.Map, sPolyOutlineName);//清除绘制的要素
            if(m_SpatialReference!=null)
            {
            m_pMapControlDefault.Map.SpatialReference = m_SpatialReference;
            }
            m_pMapControlDefault.Refresh(esriViewDrawPhase.esriViewBackground, null, null);
            m_SurFace = null;
            m_pMapControlDefault.CurrentTool = null;
            this.Dispose();
        }

     
      
    }
}
