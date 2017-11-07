using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;

namespace GeoUtilities
{
    /// <summary>
    /// 缓冲半径的设置窗体
    /// </summary>
    public partial class frmBufferSet : DevComponents.DotNetBar.Office2007Form
    {
        private IGeometry m_pGeometry;
        private IGeometry m_pBufferGeometry;
        public void setBufferGeometry(IGeometry pGeo)
        {
            m_pBufferGeometry = pGeo;
        }
        //缓冲大小
        private double dBufferSize=1;
        public double BufferSize
        {
            get { return dBufferSize; }
            set { dBufferSize = value; }
        }
        private IMap m_pMap;
        IActiveView pActiveView = null;
        private IPolygon m_pPolygon;
        private IScreenDisplay m_pScreenDisplay;
        private bool m_bOk;
		private bool m_TextChange=false;//记录滚动条是否到了最大值//xisheng 20110802
        private int iValueLast;//记录上次 20110802
        public bool Res
        {
            get
            {
                return m_bOk;
            }
        }
        ///ZQ  201119 add
        private esriSpatialRelEnum m_esriSpatialRelEnum = esriSpatialRelEnum.esriSpatialRelIntersects;
        public esriSpatialRelEnum pesriSpatialRelEnum
        {
            get
            {
                return m_esriSpatialRelEnum;
            }
        }
        ///end
        //“Enter”和“BackSpace”键常量
        const char KEY_Enter = (char)Keys.Enter;
        const char KEY_BackSpace = (char)Keys.Back;

        //允许输入的值
        private char[] arrayChar = new char[] { '0','1','2','3','4','5','6','7','8','9','.'};
        //ZQ 2011 1126 modify
        //private IActiveViewEvents_Event m_pActiveViewEvents;
        private DevComponents.DotNetBar.Office2007Form m_Form;
        public frmBufferSet(IGeometry pGeometry, IMap pMap,DevComponents.DotNetBar.Office2007Form pForm)
        {
            //几何体对象 和 屏幕显示
            m_pMap = pMap;
            m_pGeometry = pGeometry;
            m_bOk = false;
            pActiveView = pMap as IActiveView;
            m_Form = pForm;
            //m_pActiveViewEvents = pActiveView as IActiveViewEvents_Event;
            m_pScreenDisplay = pActiveView.ScreenDisplay;
            //ZQ 2011 1126 modify
            SysCommon.ScreenDraw.list.Add(BufferSetAfterDraw);
            //try
            //{
            //    m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler ( m_pActiveViewEvents_AfterDraw );

            //}
            //catch 
            //{
            //}
            //初始化控件 及 TrackBar
            InitializeComponent();
			this.groupBox1.Text = "输入缓冲半径";//20110802 xisheng
        }

        void frmBufferSet_Disposed(object sender, EventArgs e)
        {
            m_bOk = false;  
        }

        //加载
        private void frmBufferSet_Load(object sender, EventArgs e)
        {
            InitializeTrackBar(m_pGeometry);
            this.TopMost = true;
            this.Text = "缓冲半径设置";
            this.trackBar.TickFrequency = trackBar.Maximum / 10;//20110802 xisheng
            ///ZQ 20111119  add
            cmbSpatialRel.SelectedIndex = 0;
        }
        //ZQ 2011 1126 modify
        //被擦去时画出
         internal void BufferSetAfterDraw(IDisplay Display, esriViewDrawPhase phase )
        //private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase )
        {
            if ( m_Form.IsDisposed == true || m_pBufferGeometry == null)
            {
                m_pPolygon = null;
                return;
            }
            if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR(null, m_pScreenDisplay);
        }

        //根据几何类型设定初始值
        private void InitializeTrackBar(IGeometry pGeometry)
        {                       
            trackBar.Minimum = 1;
            trackBar.Maximum = 10000;//0802 xisheng
            trackBar.Value = dBufferSize <=1 ? 10: int.Parse(dBufferSize.ToString());

            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:

                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 10000;//0802 xisheng
                    break;
                case esriGeometryType.esriGeometryPolyline:

                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 10000;
                    break;

                case esriGeometryType.esriGeometryPolygon:

                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 10000;
                    break;

                case esriGeometryType.esriGeometryBag:
                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 10000;
                    break;

                default:
                    MessageBox.Show("不正确的几何类型!", "提示");
                    this.Dispose(true);
                    return;
            }
        }



        //获取当前的缓冲后的几何体
        private void get_BufferGeometry()
        {
            Error_Lable.Visible = false;
            Error_Lable.Text = "";
            //存在上次的缓冲几何体 则先抹去其图形
            if (m_pBufferGeometry != null) drawgeometryXOR(m_pBufferGeometry as IPolygon, m_pScreenDisplay);
            
            //获取缓冲半径
            dBufferSize = Convert.ToDouble(txtBufferValue.Text);/*/ 10*/ ; //20110802 xisheng
            dBufferSize = dBufferSize < 1 ? 1 : dBufferSize;//设置缓冲值不能设置成0 xisheng 20110722
            if ( dBufferSize == 0.0) dBufferSize = 0.001;
            //转换如果是经纬度的地图 xisheng 20110731
            UnitConverter punitConverter = new UnitConverterClass();
            if (m_pMap.MapUnits == esriUnits.esriDecimalDegrees)
            {
                dBufferSize = punitConverter.ConvertUnits(dBufferSize, esriUnits.esriMeters, esriUnits.esriDecimalDegrees);
            }//转换如果是经纬度的地图 xisheng 20110731
            //==如果直接操作，则原pGeometry会被改变
            //进行克隆，获取topo实例
            IClone pClone = (IClone)m_pGeometry;
            ITopologicalOperator pTopo;
            if (m_pGeometry.GeometryType != esriGeometryType.esriGeometryBag)
            {
                pTopo = pClone.Clone() as ITopologicalOperator;

                //topo非空则进行缓冲，获取缓冲后的 m_pBufferGeometry
                if(pTopo != null)  m_pBufferGeometry = pTopo.Buffer(dBufferSize);  
            }
            else
            {
                IGeometryCollection pGeometryBag = (IGeometryCollection)pClone.Clone();
                pTopo = (ITopologicalOperator)pGeometryBag.get_Geometry(0);
                IGeometry pUnionGeom = pTopo.Buffer(dBufferSize);
                for (int i = 1; i < pGeometryBag.GeometryCount; i++)
                {
                    pTopo = (ITopologicalOperator)pGeometryBag.get_Geometry(i);
                    IGeometry pTempGeom = pTopo.Buffer(dBufferSize);
                    pTopo = (ITopologicalOperator)pUnionGeom;
                    pUnionGeom = pTopo.Union(pTempGeom);
                }
                m_pBufferGeometry = pUnionGeom;
            }
            // m_pBufferGeometry为空，直接返回
            if (m_pBufferGeometry == null) return;

            //对 m_pBufferGeometry的topo进行简化再绘出
            pTopo = m_pBufferGeometry as ITopologicalOperator;
            if(pTopo != null)  pTopo.Simplify();

            IPolygon pPolygon = m_pBufferGeometry as IPolygon;

            drawgeometryXOR(pPolygon, m_pScreenDisplay);
        }

        //窗体模态显示,得到几何对象
        public IGeometry GetBufferGeometry()
        {
            this.ShowDialog();
            if (m_bOk)
            {
                //SysCommon.Gis.ModGisPub.DoDrawRangeNoRefresh(m_pMap, m_pBufferGeometry);
                pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
            }
            return m_pBufferGeometry;
        }
        /// <summary>
        /// 绘制pGeometry的图形
        /// </summary>
        /// <param name="pGeometry"> 几何体实例</param>
        /// <param name="pScreenDisplay"> 当前屏幕显示</param>
        private void drawgeometryXOR(IPolygon pPolygon, IScreenDisplay pScreenDisplay)
        {
            if (this.IsDisposed && m_bOk == false)//如果窗体关闭或者取消 就不绘制 xisheng 2011.06.28
            {
                return;
            }
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

                pScreenDisplay.StartDrawing(m_pScreenDisplay.hDC, -1);  //esriScreenCache.esriNoScreenCache -1
                pScreenDisplay.SetSymbol(pSymbol);

                //不存在已画出的多边形
                if (pPolygon != null)
                {
                    pScreenDisplay.DrawPolygon(pPolygon);
                    m_pPolygon = pPolygon;
                }
                //存在已画出的多边形
                else
                {
                    if (m_pPolygon != null)
                    {
                        pScreenDisplay.DrawPolygon(m_pPolygon);
                    }
                }
               
                pScreenDisplay.FinishDrawing();
            }
            catch (Exception ex)
            {
                MessageBox.Show("绘制缓冲范围出错:" + ex.Message, "提示");
                pFillSymbol = null;
            }
        }

        //刷新时画出
        public override void Refresh()
        {
            base.Refresh();
            drawgeometryXOR(m_pPolygon, m_pScreenDisplay);

            IActiveView pActiveView = m_pMap as IActiveView;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);
        }

        private void frmBufferSet_KeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Escape )
            {
                this.Dispose ( true );
                this.Refresh ();
                m_pBufferGeometry = null;
                m_bOk = false;
            }
        }

        //确定 按钮
        private void cmd_OK_Click(object sender, EventArgs e)
        {
            if (m_pGeometry == null)
            {
                MessageBox.Show("没有进行缓冲处理的集合实体!", "提示");
                return;
            }

            get_BufferGeometry();
            m_bOk = true;
            m_pGeometry = null;
            //m_pPolygon = null;
            IActiveView pActiveView = m_pMap as IActiveView;
            //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, pActiveView.Extent);
            this.Hide();
       
        }

        //trackBar 值改变事件，响应txtBufferValue的显示
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //trackBar变为doule型再传递
                int dValue = Convert.ToInt32(trackBar.Value /*/ 10.0 */);
				if (m_TextChange)
                {
                    m_TextChange = false;
                    return;//如果更改text造成的使滚动条到最大值不改变原有text xisheng 20110802

                } txtBufferValue.Text = dValue < 1 ? "1" : dValue.ToString();

                /*xisheng delete 20110802
                //将改变响应到 作为返回值的m_pBufferGeometry;
                get_BufferGeometry();
                this.Refresh();
                 */

            }
            catch
            {

            }
        }

        private void cmd_Cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(true);
            this.Close();
            this.DialogResult = DialogResult.Cancel;
            //this.Refresh();
            //m_pBufferGeometry = null;
            //m_bOk = false;
            //drawgeometryXOR(m_pPolygon, m_pScreenDisplay);changed by xisheng
        }

        //added by xisheng 06.28
        private void txtBufferValue_TextChanged(object sender, EventArgs e)
        {
            try
            {
               	m_TextChange = false;//20110802 xisheng
                Error_Lable.Visible = false;
                //增加输入上限与正确与否的设置 xisheng 2011.07.11
                int iValue = (int)(Convert.ToDouble(txtBufferValue.Text));
				iValueLast = iValue;//记录上次  20110802 xisheng
                if (iValue > trackBar.Maximum)
                {
                //    this.Error_Lable.Text = "缓冲半径超过了最大值";
                //    this.Error_Lable.Visible = true;
                //    txtBufferValue.Text = Convert.ToString(trackBar.Maximum);//20110802 xisheng
                    iValue = trackBar.Maximum;
                    m_TextChange = true;
                }
                if (iValue < 0)
                {
                    this.Error_Lable.Text = "缓冲半径不能为负数";
                    this.Error_Lable.Visible = true;
                    //infoFrm.ShowDialog(this);
                    txtBufferValue.Text = Convert.ToString(1);
                    iValue = 1;
                }
                trackBar.Value = iValue;
       
				//将改变响应到 作为返回值的m_pBufferGeometry; added by xisheng 20110802
                get_BufferGeometry();
                this.Refresh();
            }
            catch(Exception ex)
            {
                if (ex.Message.ToString() == "异常来自 HRESULT:0x80040238")
                {
                    this.Error_Lable.Text = "无效绘制区域，超出有效操作空间";
                }
                else
                {
                    this.Error_Lable.Text = "无效输入，请输入正整数！";
                }
                this.Error_Lable.Visible = true;
                txtBufferValue.Text = Convert.ToString(iValueLast);//20110802 xisheng
                return;
            }

        }

        private void frmBufferSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Dispose(true);
            this.Refresh();
            m_pBufferGeometry = null;
            m_bOk = false;
            SysCommon.ScreenDraw.list.Remove(BufferSetAfterDraw);
        }
        /// <summary>
        /// ZQ 20111119  add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSpatialRel_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cmbSpatialRel.SelectedItem.ToString())
            {
                case"相交":
                    m_esriSpatialRelEnum = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
                case"相接":
                    m_esriSpatialRelEnum = esriSpatialRelEnum.esriSpatialRelTouches;
                    break;
                case "穿越":
                    m_esriSpatialRelEnum = esriSpatialRelEnum.esriSpatialRelCrosses;
                    break;
                case "包含":
                    m_esriSpatialRelEnum = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
            }
        }
    }
}