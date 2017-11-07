using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace GeoUtilities
{
    public partial class FrmDisplayPrj : DevComponents.DotNetBar.Office2007Form
    {
        private IMap pMap;//地图
        private ISpatialReference pSR;//空间参考
        public bool hasSet = false;
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
        public FrmDisplayPrj(IMap inMap)
        {
            InitializeComponent();
            try
            {
                pMap = inMap;
                pSR = pMap.SpatialReference;
                txtName.Text = pSR.Name;
                fillRtxt();
            }
            catch
            { 
            }
        }
        public FrmDisplayPrj()
        {
            InitializeComponent();         
        }
        private void fillRtxt()
        {
            rTextPrj.Clear();
            if (pSR is IProjectedCoordinateSystem)
            {
                IProjectedCoordinateSystem5 iPCS = pSR as IProjectedCoordinateSystem5;
                rTextPrj.AppendText("投影："+iPCS.Projection.Name+"\r\n");
                rTextPrj.AppendText("东偏："+iPCS.FalseEasting.ToString("f6")+"\r\n");
                rTextPrj.AppendText("北偏：" + iPCS.FalseNorthing.ToString("f6") + "\r\n");
                rTextPrj.AppendText("中央经线：" + iPCS.get_CentralMeridian(true).ToString("f6") + "\r\n");
				try
                {
                rTextPrj.AppendText("比例因子：" + iPCS.ScaleFactor.ToString("f6") + "\r\n");
                rTextPrj.AppendText("纬线原点：" + iPCS.LatitudeOfOrigin.ToString("f6") + "\r\n");
				}
                catch { }
                rTextPrj.AppendText("单位：" + iPCS.CoordinateUnit.Name.ToString()+"\r\n\r\n");
                IGeographicCoordinateSystem iGCS = iPCS.GeographicCoordinateSystem;
                rTextPrj.AppendText("大地坐标系：" + iGCS.Name.ToString() + "\r\n");
                rTextPrj.AppendText("角度单位：" + iGCS.CoordinateUnit.Name.ToString() + "\r\n");
                rTextPrj.AppendText("本初子午线：" + iGCS.PrimeMeridian.Name.ToString() + "\r\n");
                rTextPrj.AppendText("水准面：" + iGCS.Datum.Name.ToString() + "\r\n");
                rTextPrj.AppendText(" 椭球：" + iGCS.Datum.Spheroid.Name.ToString() + "\r\n");
                rTextPrj.AppendText("  长轴：" + iGCS.Datum.Spheroid.SemiMajorAxis.ToString("F18") + "\r\n");
                rTextPrj.AppendText("  短轴：" + iGCS.Datum.Spheroid.SemiMinorAxis.ToString("F18") + "\r\n");
                rTextPrj.AppendText("  扁率倒数：" + (1 / iGCS.Datum.Spheroid.Flattening).ToString("F18") + "\r\n");

            }
            else if (pSR is IGeographicCoordinateSystem)
            {
                IGeographicCoordinateSystem iGCS = pSR as IGeographicCoordinateSystem;
                rTextPrj.AppendText("大地坐标系：" + iGCS.Name.ToString() + "\r\n");
                rTextPrj.AppendText("角度单位：" + iGCS.CoordinateUnit.Name.ToString() + "\r\n");
                rTextPrj.AppendText("本初子午线：" + iGCS.PrimeMeridian.Name.ToString() + "\r\n");
                rTextPrj.AppendText("水准面：" + iGCS.Datum.Name.ToString() + "\r\n");
                rTextPrj.AppendText(" 椭球：" + iGCS.Datum.Spheroid.Name.ToString() + "\r\n");
                rTextPrj.AppendText("  长轴：" + iGCS.Datum.Spheroid.SemiMajorAxis.ToString("F18") + "\r\n");
                rTextPrj.AppendText("  短轴：" + iGCS.Datum.Spheroid.SemiMinorAxis.ToString("F18") + "\r\n");
                rTextPrj.AppendText("  扁率倒数：" + (1/iGCS.Datum.Spheroid.Flattening).ToString("F18") + "\r\n");

            }
            else
            {
                rTextPrj.AppendText("未知投影\r\n");
            }
        }
        private void btnPrjPath_Click(object sender, EventArgs e)
        {
            //选择prj文件
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Multiselect = false;
            filedialog.Title = "选择prj文件";
            filedialog.Filter = "*.prj|*.prj";
            if (filedialog.ShowDialog() == DialogResult.OK)
            {
                string str = filedialog.FileName;
                this.txtPrjPath.Text = str;
            
                ESRI.ArcGIS.Geometry.ISpatialReferenceFactory pPrjFac = new ESRI.ArcGIS.Geometry.SpatialReferenceEnvironmentClass();
                pSR = pPrjFac.CreateESRISpatialReferenceFromPRJFile(txtPrjPath.Text);
                if (pSR == null)
                    return;
                if (pSR.Name == txtName.Text)
                    return;
                txtName.Text = pSR.Name;
                fillRtxt();
                
			}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string oriName = pMap.SpatialReference.Name;
            if (pMap.SpatialReference.Name != pSR.Name)
            {
                pMap.SpatialReference = pSR;//设置地图投影
                hasSet = true;
                labelM.Text = "设置成功！";
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("设置地图空间参考,原空间参考为:" + oriName + ",目标空间参考为:" + pSR.Name);
                }
            }
			this.DialogResult = DialogResult.OK;//xisheng 20110801
        }

        private void btnOK_Leave(object sender, EventArgs e)
        {
            labelM.Text = "";
			this.DialogResult = DialogResult.Cancel;//xisheng 20110801
        }
    }
}
