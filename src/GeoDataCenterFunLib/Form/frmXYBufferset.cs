using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

/*-----------------------------------------------------------
 added by xisheng 20110805 缓冲坐标窗体文件 frmXYBufferset.cs
 -----------------------------------------------------------*/
namespace GeoDataCenterFunLib
{
    public partial class frmXYBufferset : DevComponents.DotNetBar.Office2007Form
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
        public frmXYBufferset()
        {
            InitializeComponent();
        }
        public SysCommon.BottomQueryBar QueryBar
        {
            get;
            set;
        }
        private IMapControlDefault _axmapcontrol = null;
		ISpatialReference earthref = null;//20110802
        ISpatialReference flatref = null;//20110802 xisheng
        ESRI.ArcGIS.Geometry.IPoint point=null;
        private frmBufferSet m_frmBufferSet = null;
        private frmQuery m_frmQuery;
        private Form m_mainFrm;
        private enumQueryMode m_enumQueryMode;


        public frmXYBufferset(IMapControlDefault axmapcontrol,Form mainform)
        {
            _axmapcontrol = axmapcontrol;
            m_mainFrm = mainform;
            m_enumQueryMode = enumQueryMode.Visiable;
            InitializeComponent();
        }
		/// 将经纬度点转换为平面坐标。20110802 xisheng 
        private IPoint GetProject(double x, double y)
        {

            IPoint pt = new PointClass();

            pt.PutCoords(x, y);

            IGeometry geo = (IGeometry)pt;
            geo.SpatialReference = earthref;
            geo.Project(flatref);


            return pt;
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Visible = false; 
            if (_axmapcontrol != null)
            {
                try
                {
                    /*xisheng 20110802 */

                    ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
                    earthref = pSpatRefFac.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

                    point = new ESRI.ArcGIS.Geometry.PointClass();
                    double x1 = Convert.ToDouble(textBoxX.Text);
                    double y1 = Convert.ToDouble(textBoxY.Text);
                    if (_axmapcontrol.MapUnits == esriUnits.esriMeters)
                    {
                        flatref = _axmapcontrol.Map.SpatialReference;
                        point.PutCoords(x1, y1);//yjl20110812
                    }
                    else if (_axmapcontrol.MapUnits == esriUnits.esriDecimalDegrees)
                    {
                        point.PutCoords(x1, y1);
                    }
                   // ESRI.ArcGIS.Geometry.IPoint pPoint = _axmapcontrol.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
                    //end 0802
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog(this.Text);//xisheng 日志记录 0928;
                    }
                    if (m_frmQuery == null)
                    {
                        m_frmQuery = new frmQuery(_axmapcontrol,m_enumQueryMode);
                        m_frmQuery.Owner = m_mainFrm;
                        m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
                    }
                    if (m_frmBufferSet != null)
                    {
                        m_frmBufferSet.setBufferGeometry(null);
                        m_frmBufferSet = null;
                    }
                    m_frmBufferSet = new frmBufferSet(point as IGeometry, _axmapcontrol.Map, m_frmQuery);
                    m_frmBufferSet.FormClosed += new FormClosedEventHandler(frmBufferSet_FormClosed);
                    _axmapcontrol.CenterAt(point);
                    IGeometry pGeometry = m_frmBufferSet.GetBufferGeometry();
                    if (pGeometry == null || m_frmBufferSet.Res == false) return;
                    
                    //m_frmQuery.Show();
                    ///ZQ 20111119  modify
                    //m_frmQuery.FillData(_axmapcontrol.ActiveView.FocusMap, pGeometry, m_frmBufferSet.pesriSpatialRelEnum);
                    QueryBar.m_pMapControl = _axmapcontrol;
                    QueryBar.EmergeQueryData(_axmapcontrol.ActiveView.FocusMap, pGeometry, m_frmBufferSet.pesriSpatialRelEnum);
                    try
                    {
                        DevComponents.DotNetBar.Bar pBar = QueryBar.Parent.Parent as DevComponents.DotNetBar.Bar;
                        if (pBar != null)
                        {
                            pBar.AutoHide = false;
                            //pBar.SelectedDockTab = 1;
                            int tmpindex = pBar.Items.IndexOf("dockItemDataCheck");
                            pBar.SelectedDockTab = tmpindex;
                        }
                    }
                    catch
                    { }
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
            if (m_frmBufferSet!=null) m_frmBufferSet.setBufferGeometry(null);//added by chulili 20110731
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

        private void textBoxX_TextChanged(object sender, EventArgs e)
        {
            CheckNumber(textBoxX.Text);
        }

        private void textBoxY_TextChanged(object sender, EventArgs e)
        {
            CheckNumber(textBoxY.Text);
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

        private void frmXYLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//按回车跟确定一样效果 20110803
            {
                buttonOK_Click(sender, e);
            }
        }

        private void frmXYLocation_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void frmXYBufferset_Load(object sender, EventArgs e)
        {
            if (_axmapcontrol != null)
            {
                if (_axmapcontrol.MapUnits == esriUnits.esriMeters)
                {
                    this.labelX1.Visible = false;
                    this.labelX.Text = "X坐标:";
                    this.labelY.Text = "Y坐标:";
                }
                else if (_axmapcontrol.MapUnits == esriUnits.esriDecimalDegrees)
                {
                    this.labelX1.Visible = true;
                    this.labelX.Text = "经度:";
                    this.labelY.Text = "纬度:";
                }
            }
        }
    }
}
