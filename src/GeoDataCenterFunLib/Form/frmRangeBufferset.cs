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

/*-----------------------------------------------------------
 added by xisheng 20110730 范围定位窗体文件 frmRangeBufferset.cs
 -----------------------------------------------------------*/

namespace GeoDataCenterFunLib
{
    public partial class frmRangeBufferset : DevComponents.DotNetBar.Office2007Form
    {
        public frmRangeBufferset()
        {
            InitializeComponent();
        }

          private IMapControlDefault _axmapcontrol = null;
          private frmBufferSet m_frmBufferSet = null;
          private frmQuery m_frmQuery;
          private Form m_mainFrm;
          private enumQueryMode m_enumQueryMode;
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
          public frmRangeBufferset(IMapControlDefault axmapcontrol,Form mainform)
        {
            m_mainFrm = mainform;
            _axmapcontrol = axmapcontrol;
            m_enumQueryMode = enumQueryMode.Visiable;
            InitializeComponent();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (_axmapcontrol != null)
            {
                try
                {               
                    IEnvelope penvelope=new EnvelopeClass();
                    penvelope.XMin = Convert.ToDouble(textBoxWest.Text);
                    penvelope.XMax = Convert.ToDouble(textBoxEast.Text);
                    penvelope.YMax = Convert.ToDouble(textBoxNorth.Text);
                    penvelope.YMin = Convert.ToDouble(textBoxSouth.Text);
                    IPoint pmiddle=new PointClass();
                    pmiddle.PutCoords((penvelope.XMax + penvelope.XMin) / 2, (penvelope.YMax + penvelope.YMin) / 2);//中心点
                    /*********************************将Envelope转成Polygon*/
                    IPolygon pon = new PolygonClass();
                    IPointCollection ptcoll = new PolygonClass();
                    IPoint a=new PointClass();IPoint b=new PointClass();IPoint c=new PointClass();IPoint d=new PointClass();
                    a.PutCoords(penvelope.XMin, penvelope.YMax);
                    b.PutCoords(penvelope.XMax, penvelope.YMax);
                    c.PutCoords(penvelope.XMax, penvelope.YMin);
                    d.PutCoords(penvelope.XMin, penvelope.YMin);
                    object missing = Type.Missing;
                    ptcoll.AddPoint(a, ref missing, ref missing);
                    ptcoll.AddPoint(b, ref missing, ref missing);
                    ptcoll.AddPoint(c, ref missing, ref missing);
                    ptcoll.AddPoint(d, ref missing, ref missing);
                    pon = ptcoll as IPolygon;
                    /*********************************/
               		IGeometry geo=new PolygonClass();

                    geo = pon as IGeometry;

                    /*-----xisheng 20110802------*/
                    ISpatialReference earthref = null;
                    ISpatialReference flatref = null;
                    ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
                    earthref = pSpatRefFac.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog(this.Text);//xisheng 日志记录 0928;
                    }
                    if (_axmapcontrol.MapUnits == esriUnits.esriMeters)
                    {
                        flatref = _axmapcontrol.Map.SpatialReference;
                        geo.SpatialReference = flatref;//yjl20110812
                        //geo.Project(flatref);
                    }
                    /*-----xisheng 20110802------*/


                   //创建Topo对象，简化后统一空间参考
                    ITopologicalOperator pTopo = (ITopologicalOperator)geo;
                    pTopo.Simplify();
                    geo.Project(_axmapcontrol.Map.SpatialReference);
                    
                    
                    if (m_frmQuery == null)
                    {
                        m_frmQuery = new frmQuery(_axmapcontrol, m_enumQueryMode);
                        m_frmQuery.Owner = m_mainFrm;
                        m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
                    }

                    //清除上次的所有元素
                    (_axmapcontrol.Map as IGraphicsContainer).DeleteAllElements();
                    m_frmBufferSet = new frmBufferSet(geo, _axmapcontrol.Map, m_frmQuery);
                    m_frmBufferSet.FormClosed += new FormClosedEventHandler(frmBufferSet_FormClosed);
                    _axmapcontrol.CenterAt(pmiddle);
                    IGeometry pGeometry = m_frmBufferSet.GetBufferGeometry();
                    if (pGeometry == null || m_frmBufferSet.Res == false) return;

                    m_frmQuery.Show();
                    ///ZQ 20111119  modify
                    m_frmQuery.FillData(_axmapcontrol.ActiveView.FocusMap, pGeometry,m_frmBufferSet.pesriSpatialRelEnum);
                    this.Close();

                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "错误：" + ex.Message);
                }
            }
        }

        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_enumQueryMode = m_frmQuery.QueryMode;
            m_frmQuery = null;
            if (m_frmBufferSet!=null) m_frmBufferSet.setBufferGeometry(null);
            //this.Visible = true;
            //this.Focus();
            //_axmapcontrol.ActiveView.Refresh();
        }
        private void frmBufferSet_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmBufferSet.setBufferGeometry(null);//added by chulili 20110731
            //this.Visible = true;
            //this.Focus();
            _axmapcontrol.ActiveView.Refresh();
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
