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
 added by xisheng 20110730 坐标定位窗体文件 frmXYLocation.cs
 -----------------------------------------------------------*/
namespace GeoDataCenterFunLib
{
    public partial class frmXYLocation : DevComponents.DotNetBar.Office2007Form
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
        public frmXYLocation()
        {
            InitializeComponent();
        }
        private AxMapControl _axmapcontrol = null;
		ISpatialReference earthref = null;//20110802
        ISpatialReference flatref = null;//20110802 xisheng
        ESRI.ArcGIS.Geometry.IPoint point=null;

        public PictureBox p1 { get; set; }
        public PictureBox p2 { get; set; }
        public PictureBox p3 { get; set; }
        public PictureBox p4 { get; set; }


        public frmXYLocation(AxMapControl axmapcontrol)
        {
            
            _axmapcontrol = axmapcontrol;
            InitializeComponent();
            _axmapcontrol.OnExtentUpdated += new IMapControlEvents2_Ax_OnExtentUpdatedEventHandler(_axmapcontrol_OnExtentUpdated);
            _axmapcontrol.OnMapReplaced += new IMapControlEvents2_Ax_OnMapReplacedEventHandler(_axmapcontrol_OnMapReplaced);
            _axmapcontrol.OnMouseDown += new IMapControlEvents2_Ax_OnMouseDownEventHandler(_axmapcontrol_OnMouseDown);
            _axmapcontrol.OnMouseUp += new IMapControlEvents2_Ax_OnMouseUpEventHandler(_axmapcontrol_OnMouseUp);
           
            

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
            if (_axmapcontrol != null)
            {
                try
                {
                    /*xisheng 20110802 */
                    p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;
                    ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
                    earthref = pSpatRefFac.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

                    point = new ESRI.ArcGIS.Geometry.PointClass();
                    double x1 = Convert.ToDouble(textBoxX.Text);
                    double y1 = Convert.ToDouble(textBoxY.Text);
                    if (_axmapcontrol.MapUnits == esriUnits.esriMeters)
                    {
                        flatref = _axmapcontrol.Map.SpatialReference;
                        point = GetProject(x1, y1);
                    }
                    else if (_axmapcontrol.MapUnits == esriUnits.esriDecimalDegrees)
                    {
                        point.PutCoords(x1, y1);
                    }
                    //end 0802
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("地图坐标定位到中心点(" + x1 + "," + y1 + ")");//xisheng 日志记录
                    }
                    _axmapcontrol.CenterAt(point);
                    _axmapcontrol.ActiveView.Refresh();

                    int x = Convert.ToInt32(point.X);
                    int y = Convert.ToInt32(point.Y);
                    _axmapcontrol.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(point, out x, out y);
                    p1.Location = new System.Drawing.Point(x - 11, y - 1);
                    p2.Location = new System.Drawing.Point(x - 1, y - 11);
                    p3.Location = new System.Drawing.Point(x + 1, y - 1);
                    p4.Location = new System.Drawing.Point(x - 1, y + 1);
                    p1.Visible = p2.Visible = p3.Visible = p4.Visible = true;

                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "错误：" + ex.Message);
                }
            }
            
        }

        private void _axmapcontrol_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            if (!this.IsDisposed)
            {
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;
                int x = Convert.ToInt32(point.X);
                int y = Convert.ToInt32(point.Y);
                _axmapcontrol.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(point, out x, out y);
                p1.Location = new System.Drawing.Point(x - 11, y - 1);
                p2.Location = new System.Drawing.Point(x - 1, y - 11);
                p3.Location = new System.Drawing.Point(x + 1, y - 1);
                p4.Location = new System.Drawing.Point(x - 1, y + 1);
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = true;
            }

        }
        private void _axmapcontrol_OnMouseUp(object sender, IMapControlEvents2_OnMouseUpEvent e)
        {
            
            if (!this.IsDisposed)
            {
            
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = true;
            }
                
            

        }
        private void _axmapcontrol_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (!this.IsDisposed)
            {
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;
            }
        }

        private void _axmapcontrol_OnMapReplaced(object sender, IMapControlEvents2_OnMapReplacedEvent e)
        {

            if (!this.IsDisposed)
            {
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;
                int x = Convert.ToInt32(point.X);
                int y = Convert.ToInt32(point.Y);
                _axmapcontrol.ActiveView.ScreenDisplay.DisplayTransformation.FromMapPoint(point, out x, out y);
                p1.Location = new System.Drawing.Point(x - 11, y - 1);
                p2.Location = new System.Drawing.Point(x - 1, y - 11);
                p3.Location = new System.Drawing.Point(x + 1, y - 1);
                p4.Location = new System.Drawing.Point(x - 1, y + 1);
                p1.Visible = p2.Visible = p3.Visible = p4.Visible = true;
            }

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
            p1.Visible = p2.Visible = p3.Visible = p4.Visible = false;//关闭十字丝
           // m_frm = null;
            this.Dispose(true);
        }
    }
}
