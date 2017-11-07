using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;


namespace GeoUtilities
{
    public class ControlsLayerAttribute : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsLayerAttribute()
        {
            base._Name = "GeoUtilities.ControlsLayerAttribute";
            base._Caption = "要素属性表";
            base._Tooltip = "显示要素属性";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "显示要素属性";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer mLayer = _AppHk.MapControl.CustomProperty as ILayer ;
                if (mLayer == null) return false;
                if (!(mLayer is IFeatureLayer)) return false;
                return true;
            }
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
            frmAttributeTB AttributeTB = new frmAttributeTB();
            ILayer pLayer = null;
            IFeatureLayer pFeatureLayer = null;
            IFeatureClass pFeatrueClass = null;
            pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
            pFeatureLayer = pLayer as IFeatureLayer;
            if (pFeatureLayer == null) return;
            pFeatrueClass = pFeatureLayer.FeatureClass;
            AttributeTB.FeatureClass = pFeatrueClass;
            AttributeTB.Fields = pFeatrueClass.Fields;
            AttributeTB.MapControl = _AppHk.MapControl;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("打开图层属性表");//xisheng 2011.07.08 增加日志
            }
            AttributeTB.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
