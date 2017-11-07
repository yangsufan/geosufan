using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Display;
//using ESRI.ArcGIS.Geometry;
//using ESRI.ArcGIS.esriSystem;

using GeoDataCenterFunLib;

namespace GeoDataEdit
{
    public class BatchFillField : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;

        public BatchFillField()
        {
            base._Name = "GeoDataEdit.BatchFillField";
            base._Caption = "批量赋值";
            base._Tooltip = "批量赋值";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "批量赋值";
            //base._Image = "";
            //base._Category = "";
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
        public override bool Enabled
        {
            get
            {
                try
                {
                    //if (_AppHk.MapControl.Map.LayerCount == 0)
                    //{
                    //    base._Enabled = false;
                    //    return false;
                    //}

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
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
            GeoDataEdit.frmBatchFillField frmBFF = new frmBatchFillField();
            frmBFF.ShowDialog();
           
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null)
                return;
        }
    }
}
