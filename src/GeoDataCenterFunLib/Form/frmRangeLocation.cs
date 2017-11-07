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

/*-----------------------------------------------------------
 added by xisheng 20110730 范围定位窗体文件 frmRangeLocation.cs
 -----------------------------------------------------------*/

namespace GeoDataCenterFunLib
{
    public partial class frmRangeLocation : DevComponents.DotNetBar.Office2007Form
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
        public frmRangeLocation()
        {
            InitializeComponent();
        }

          private IMapControlDefault _axmapcontrol = null;
          public frmRangeLocation(IMapControlDefault axmapcontrol)
        {
            _axmapcontrol = axmapcontrol;
            InitializeComponent();
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (_axmapcontrol != null)
            {
                try
                {
                    IEnvelope penvelope = new EnvelopeClass();
                    penvelope.XMin = Convert.ToDouble(textBoxWest.Text);
                    penvelope.XMax = Convert.ToDouble(textBoxEast.Text);
                    penvelope.YMax = Convert.ToDouble(textBoxNorth.Text);
                    penvelope.YMin = Convert.ToDouble(textBoxSouth.Text);
               		IGeometry geo=penvelope as IGeometry;
                    /*-----xisheng 20110802------*/
                    ISpatialReference earthref = null;
                    ISpatialReference flatref = null;
                    ISpatialReferenceFactory2 pSpatRefFac = new SpatialReferenceEnvironmentClass();
                    earthref = pSpatRefFac.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);

                    if (_axmapcontrol.MapUnits == esriUnits.esriMeters)
                    {
                        flatref = _axmapcontrol.Map.SpatialReference;
                        geo.SpatialReference = flatref;//yjl20110812 modify支持平面
                        //geo.Project(flatref);
                    }
                    /*-----xisheng 20110802------*/
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("使用范围定位");
                    }
                _axmapcontrol.Extent = penvelope;
                _axmapcontrol.ActiveView.Refresh();
                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "错误：" + ex.Message);
                }
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
