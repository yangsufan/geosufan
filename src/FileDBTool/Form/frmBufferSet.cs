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

namespace FileDBTool
{
    /// <summary>
    /// 缓冲半径的设置窗体
    /// </summary>
    public partial class frmBufferSet : DevComponents.DotNetBar.Office2007Form
    {
        private IGeometry m_pGeometry;
        private IGeometry m_pBufferGeometry;

        private IMap m_pMap;
        private IPolygon m_pPolygon;
        private IScreenDisplay m_pScreenDisplay;
        private bool m_bOk;
        public bool Res
        {
            get
            {
                return m_bOk;
            }
        }

        //“Enter”和“BackSpace”键常量
        const char KEY_Enter = (char)Keys.Enter;
        const char KEY_BackSpace = (char)Keys.Back;

        //允许输入的值
        private char[] arrayChar = new char[] { '0','1','2','3','4','5','6','7','8','9','.'};

        private IActiveViewEvents_Event m_pActiveViewEvents;

        public frmBufferSet(IGeometry pGeometry, IMap pMap)
        {
            //几何体对象 和 屏幕显示
            m_pMap = pMap;
            m_pGeometry = pGeometry;
            m_bOk = false;
            IActiveView pActiveView = pMap as IActiveView;

            m_pActiveViewEvents = pActiveView as IActiveViewEvents_Event;
            m_pScreenDisplay = pActiveView.ScreenDisplay;
            try
            {
                m_pActiveViewEvents.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler ( m_pActiveViewEvents_AfterDraw );

            }
            catch 
            {
            }
            //初始化控件 及 TrackBar
            InitializeComponent();
            InitializeTrackBar(m_pGeometry);

            this.TopMost = true;
        }

        void frmBufferSet_Disposed(object sender, EventArgs e)
        {
            m_bOk = false;  
        }

        //加载
        private void frmBufferSet_Load(object sender, EventArgs e)
        {
            this.Text = "缓冲半径设置";
            this.trackBar.TickFrequency = 100;
        }

        //被擦去时画出
        private void m_pActiveViewEvents_AfterDraw(IDisplay Display, esriViewDrawPhase phase )
        {
            if (this.IsDisposed == true) return;
            if (phase == esriViewDrawPhase.esriViewForeground) drawgeometryXOR(null, m_pScreenDisplay);
        }

        //根据几何类型设定初始值
        private void InitializeTrackBar(IGeometry pGeometry)
        {                       
            trackBar.Minimum = 0;
            trackBar.Maximum = 1000;
            trackBar.Value = 0;

            switch (pGeometry.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:

                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 100;
                    break;
                case esriGeometryType.esriGeometryPolyline:

                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 100;
                    break;

                case esriGeometryType.esriGeometryPolygon:

                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 100;
                    break;

                case esriGeometryType.esriGeometryBag:
                    trackBar.SmallChange = 1;
                    trackBar.LargeChange = 100;
                    break;

                default:
                    MessageBox.Show("不正确的几何类型!", "系统提示");
                    this.Dispose(true);
                    return;
            }
        }



        //获取当前的缓冲后的几何体
        private void get_BufferGeometry()
        {
            //存在上次的缓冲几何体 则先抹去其图形
            if (m_pBufferGeometry != null) drawgeometryXOR(m_pBufferGeometry as IPolygon, m_pScreenDisplay);
            
            //获取缓冲半径
            double dBufferSize = ((double)trackBar.Value /*/ 10*/);
            dBufferSize = dBufferSize < 1 ? 1 : dBufferSize;
            //if ( dBufferSize == 0.0) dBufferSize = 0.001;

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
            return m_pBufferGeometry;
        }

        /// <summary>
        /// 绘制pGeometry的图形
        /// </summary>
        /// <param name="pGeometry"> 几何体实例</param>
        /// <param name="pScreenDisplay"> 当前屏幕显示</param>
        private void drawgeometryXOR(IPolygon pPolygon, IScreenDisplay pScreenDisplay)
        {
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            ISimpleLineSymbol pLineSymbol = new SimpleLineSymbolClass();

            try
            {
                //颜色对象
                IRgbColor pRGBColor = new RgbColorClass();
                pRGBColor.UseWindowsDithering = false;
                pRGBColor.Red = 45;
                pRGBColor.Green = 45;
                pRGBColor.Blue = 45;

                //填充符号以及画笔
                ISymbol pSymbol = pFillSymbol as ISymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
                pFillSymbol.Color = pRGBColor;

                //边缘线颜色以及画笔
                ISymbol pLSymbol = pLineSymbol as ISymbol;
                pLSymbol.ROP2 = esriRasterOpCode.esriROPXOrPen;
                pRGBColor.Red = 145;
                pRGBColor.Green = 145;
                pRGBColor.Blue = 145;
                pLineSymbol.Color = (IColor)pRGBColor;

                pLineSymbol.Width = 0.8;
                pLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
                pFillSymbol.Outline = pLineSymbol;

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
                MessageBox.Show("没有进行缓冲处理的集合实体!", "系统提示");
                return;
            }

            get_BufferGeometry();
            m_bOk = true;
            m_pGeometry = null;
            m_pPolygon = null;
            IActiveView pActiveView = m_pMap as IActiveView;
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, pActiveView.Extent);
            this.Close();//.Hide();
        }

        //trackBar 值改变事件，响应txtBufferValue的显示
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //trackBar变为doule型再传递
                int dValue = Convert.ToInt32(trackBar.Value /*/ 10.0 */);
                txtBufferValue.Text = dValue < 1 ? "1" : dValue.ToString();

                //将改变响应到 作为返回值的m_pBufferGeometry;
                get_BufferGeometry();
            }
            catch
            {

            }
        }

        //txtBufferValue 键按下事件 ，校验并响应改变
        private void txtBufferValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textBox非数字.和退格不响应，8是退格键

            try
            {
                
                if (e.KeyChar == '\r')//KEY_Enter.CompareTo(e.KeyChar) == 0)
                {
                    int iValue = (int)(Convert.ToDouble(txtBufferValue.Text));
                    if (iValue > trackBar.Maximum)
                    {
                        SysCommon.Error.frmErrorHandle infoFrm = new SysCommon.Error.frmErrorHandle("提示", "缓冲半径超过了最大值！");
                        infoFrm.ShowDialog(this);
                       // trackBar.Maximum = iValue / 2 * 3;
                        trackBar.TickFrequency = Convert.ToInt32(trackBar.Maximum);
                        iValue = trackBar.Maximum;
                    }
                    if (iValue < 0)
                    {
                        SysCommon.Error.frmErrorHandle infoFrm = new SysCommon.Error.frmErrorHandle("提示", "缓冲半径不能为负数！");
                        infoFrm.ShowDialog(this);
                        txtBufferValue.Text = Convert.ToString(0);
                        iValue = 0;
                    }
                    trackBar.Value = iValue;

                }
            
            }
            catch
            {
                SysCommon.Error.frmErrorHandle infoFrm = new SysCommon.Error.frmErrorHandle("提示", "无效的字符串，请输入数字！");
                infoFrm.ShowDialog(this);
                return;
            }
        }

        private void cmd_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(true);
            this.Refresh();
            m_pBufferGeometry = null;
            m_bOk = false;
            drawgeometryXOR(m_pPolygon, m_pScreenDisplay);
        }
    }
}