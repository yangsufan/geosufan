using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using System.IO;
using System.Xml;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;

namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：地图鹰眼窗体
    /// </summary>
    public partial class FrmLegend : DevComponents.DotNetBar.Office2007Form
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
        AxMapControl pAxMapCtrl;//主窗体控件
        Plugin.Interface.CommandRefBase pCommand;
        ///ZQ 20110927   modify   构造函数添加系统库
        public FrmLegend(AxMapControl inAxMapCtrl, Plugin.Interface.CommandRefBase inCommand, IWorkspace pWorkspace)
        {
            InitializeComponent();
            this.axTOCControl1.SetBuddyControl(inAxMapCtrl);
            this.axTOCControl1.Refresh();
            this.Height = inAxMapCtrl.Height ;
            
        }
        

        private void FrmEagleEye_Load(object sender, EventArgs e)
        {
            
        }

        private void FrmEagleEye_FormClosed(object sender, FormClosedEventArgs e)
        {
        }
      }
}
