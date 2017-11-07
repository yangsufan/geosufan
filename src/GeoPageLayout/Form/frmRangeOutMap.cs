using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

/*-----------------------------------------------------------
 added by xisheng 20110730 范围出图窗体文件 frmRangeOutMap.cs
 -----------------------------------------------------------*/

namespace GeoPageLayout
{
    public partial class frmRangeOutMap : DevComponents.DotNetBar.Office2007Form
    {
        public frmRangeOutMap()
        {
            InitializeComponent();
        }

          private IMapControlDefault _axmapcontrol = null;
          private Form m_mainFrm;
          private IEnvelope m_Polygon;
          private IActiveViewEvents_Event m_pActiveViewEvents;
          private FrmPageLayout frm = null;
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
        public frmRangeOutMap(IMapControlDefault axmapcontrol,Form mainform)
        {
            m_mainFrm = mainform;
            _axmapcontrol = axmapcontrol;
            InitializeComponent();
            m_pActiveViewEvents = _axmapcontrol.ActiveView as IActiveViewEvents_Event;
            try
            {
                m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(m_pActiveViewEvents_AfterDraw);

            }
            catch
            {
            }
        }

          //擦去重绘
          private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
          {
              if (frm != null && !frm.IsDisposed)
              {
                  drawgeometryXOR(null,_axmapcontrol.ActiveView.ScreenDisplay);
              }

          }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (_axmapcontrol != null)
            {
                try
                {
                    IEnvelope penvelope = new EnvelopeClass();
                    penvelope.XMin = Convert.ToDouble(textBoxWest.Text);
                    penvelope.XMax = Convert.ToDouble(textBoxEast.Text);
                    penvelope.YMax = Convert.ToDouble(textBoxNorth.Text);
                    penvelope.YMin = Convert.ToDouble(textBoxSouth.Text);
                    IPoint pmiddle = new PointClass();
                    pmiddle.PutCoords((penvelope.XMax + penvelope.XMin) / 2, (penvelope.YMax + penvelope.YMin) / 2);//中心点

                    IGeometry pGeometry = penvelope as IGeometry;

                    /*-----xisheng 20110802------*/
                    ISpatialReference earthref = null;
                    ISpatialReference flatref = null;
                    ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
                    earthref = pSpatRefFac.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

                    if (_axmapcontrol.MapUnits == esriUnits.esriMeters)
                    {
                        flatref = _axmapcontrol.Map.SpatialReference;
                        pGeometry.SpatialReference = flatref;//yjl20110812 modify
                        //pGeometry.Project(flatref);
                    }
                    /*-----xisheng 20110802------*/

                    if (pGeometry == null)
                        return;
                    if (pGeometry.GeometryType != esriGeometryType.esriGeometryEnvelope)
                        return;
                    if (pGeometry.Envelope.Width < 0)
                        return;
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("坐标范围制图");
                    }
                    drawgeometryXOR(pGeometry as IEnvelope, _axmapcontrol.ActiveView.ScreenDisplay);
                    ESRI.ArcGIS.Carto.IMap pMap = _axmapcontrol.Map;
                    SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
                    pgss.EnableCancel = false;
                    pgss.ShowDescription = false;
                    pgss.FakeProgress = true;
                    pgss.TopMost = true;
                    pgss.ShowProgress();
                    Application.DoEvents();
                    frm = new FrmPageLayout(pMap, pGeometry);
                    frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                    frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                    frm.typeZHT = 2;
                    frm.Show();
                    pgss.Close();
                    _axmapcontrol.CurrentTool = null;
                    Application.DoEvents();




                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "错误：" + ex.Message);
                }
            }
        }
        //窗体关闭时 刷新前景
        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Polygon = null;
            _axmapcontrol.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }
        //绘制导入的范围
        private void drawgeometryXOR(IEnvelope pPolygon, IScreenDisplay pScreenDisplay)
        {
            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                ISymbol pSymbol = (ISymbol)pFillSymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pRGBColor.Red = 255;
                pRGBColor.Green = 170;
                pRGBColor.Blue = 0;
                pLineSymbol.Color = pRGBColor;

                pLineSymbol.Width = 0.8;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

                pFillSymbol.Color = pRGBColor;
                pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;

                pScreenDisplay.StartDrawing(_axmapcontrol.ActiveView.ScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawRectangle(pPolygon);
                    m_Polygon = pPolygon;
                }
                //存在已画出的多边形
                else
                {
                    if (m_Polygon != null)
                    {
                        pScreenDisplay.DrawRectangle(m_Polygon);
                    }
                }

                pScreenDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示");
                pFillSymbol = null;
            }
        }
      

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
        private void textBoxNorth_TextChanged(object sender, EventArgs e)
        {

            CheckNumber(textBoxNorth.Text);
        }
       
        //保证输入为数字
        private void CheckNumber(string s)
        {
            this.Error_Lable.Visible = false;
            try
            {
                int iValue = (int)(Convert.ToDouble(s));
            }
            catch
            {
                this.Error_Lable.Text = "无效输入，请输入数字！";
                this.Error_Lable.Visible = true;
                return;
            }
        }

        private void textBoxWest_TextChanged(object sender, EventArgs e)
        {
            CheckNumber(textBoxWest.Text);
        }

        private void textBoxEast_TextChanged(object sender, EventArgs e)
        {
            CheckNumber(textBoxEast.Text);
        }

        private void textBoxSouth_TextChanged(object sender, EventArgs e)
        {
            CheckNumber(textBoxSouth.Text);
        }
        private void frmRangeLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//按回车跟确定一样效果 20110803
            {
                buttonOK_Click(sender, e);
            }
        }
    }
}
