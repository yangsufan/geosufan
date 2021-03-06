﻿using System;
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
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
//剖面分析   张琪   20110630
namespace GeoArcScene3DAnalyse
{
    public partial class frm3DSection : DevComponents.DotNetBar.Office2007Form
    {
        //当前需要进行统计的SurFace
        public ISurface m_SurFace;
        ////统计范围
        //private IPolygon m_StaPolygon = null;
        public ILayer m_Layer;//当前选中的图层
        private  Tool3DDrawGeo pTool3DDrawGeo = new Tool3DDrawGeo();
        private IGeometry  m_SectionLine = new PolylineClass();
        private const string sPolyOutlineName = "_POLYOUTLINE_";
        private bool _Writelog = true;
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
        public frm3DSection()
        {
            InitializeComponent();
            m_SurFace = null;
            //m_StaPolygon = null;
            DrawGeo.Enabled = false;
            axBtnOK.Enabled = false;
            m_SectionLine = null;
        }
        ESRI.ArcGIS.Controls.ISceneControl m_pCurrentSceneControl = null;
        public ESRI.ArcGIS.Controls.ISceneControl CurrentSceneControl//获取sceneControl 的值 
        {
            set { m_pCurrentSceneControl = value; }                  //张琪// 20110630
        }
        //初始化 comboBoxOpen选项
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
                //用于绘制三维效果
                Cls3DMarkDraw.AddSimpleGraphic(pGeometry, Cls3DMarkDraw.getRGB(71, 61, 255), 4, sPolyOutlineName, m_pCurrentSceneControl.Scene, pGroup);
                m_pCurrentSceneControl.SceneGraph.RefreshViewers();

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

                        if (!(psr is IProjectedCoordinateSystem))
                        {
                           
                            label.Text = "该数据为地理坐标，计算结果无效仅供参考！";

                        }
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
            Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, sPolyOutlineName);
            m_pCurrentSceneControl.SceneGraph.RefreshViewers();
            pTool3DDrawGeo.EndDtrawd += new myEventHandler(pTool3DDrawGeo_EndDtrawd);
            pTool3DDrawGeo.GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine;
            pTool3DDrawGeo.pSurface = m_SurFace;
            ICommand pCommad = pTool3DDrawGeo;
            pCommad.OnCreate(m_pCurrentSceneControl.Object);
            m_pCurrentSceneControl.CurrentTool = pCommad as ITool;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("两点间剖面分析,表面集为:" + comboBoxOpen.Text);//xisheng 日志记录07.08
            }
        }

        private void pTool3DDrawGeo_EndDtrawd(bool BeginDraw)
        {
            if (BeginDraw)
            {
                m_SectionLine = pTool3DDrawGeo.m_Geometry as IPolyline;
                m_pCurrentSceneControl.CurrentTool = null;
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
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("输出剖面分析剖面图,路径为:" + pSaveFileDialog.FileName);//xisheng 日志记录07.08
            }
            MessageBox.Show("剖面图保存成功！", "提示！");
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frm3DSection_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cls3DMarkDraw.DeleteAllElementsWithName(m_pCurrentSceneControl.Scene, sPolyOutlineName);//清除绘制的要素
            m_SurFace = null;
            m_pCurrentSceneControl.CurrentTool = null;
            this.Dispose();
        }

     
      
    }
}
