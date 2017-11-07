using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;

namespace GeoUtilities
{
    public class ControlsExportByZone : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        public ControlsExportByZone()
        {
            base._Name = "GeoUtilities.ControlsExportByZone";
            base._Caption = "按行政区提取";
            base._Tooltip = "按行政区提取";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "按行政区提取";
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if (_AppHk.MapControl == null) return;

            //获得行政区名称及范围 
            IGeometry pGeoZone = null;
            //需要添加获得行政区范围的代码


            //根据行政区获得图幅号 需要添加5000图幅的要素类名称
            string strMapName = "";//50000图幅要素类名称 
            IList<string> lstMapNOs = GeoUtilities.GeoModule.GetMapNOsByGeometry(_AppHk.MapControl.Map,strMapName, pGeoZone);
            if (lstMapNOs.Count < 1)
            {
                MessageBox.Show("获得图幅为空。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //弹出提取框
            GeoUtilities.Gis.Form.frmExportDataByMapNO vfrmExport = new GeoUtilities.Gis.Form.frmExportDataByMapNO(lstMapNOs,false);
            vfrmExport.ShowDialog();
            vfrmExport = null;

            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Tooltip);
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
