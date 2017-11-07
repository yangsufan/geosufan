using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataCenterFunLib
{
    public class ControlsClearMouseCurStat : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsClearMouseCurStat()
        {
            base._Name = "GeoDataCenterFunLib.ControlsClearMouseCurStat";
            base._Caption = "Õ£÷πΩªª•";
            base._Tooltip = "Õ£÷πΩªª•";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "Õ£÷π Û±ÍΩªª•";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerDefault;
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

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
            if (_cmd == null || _AppHk == null) return;
            if (_AppHk.CurrentControl == null) return;

            if (_AppHk.CurrentControl is IMapControl2)
            {
                _AppHk.MapControl.CurrentTool = null;
                _AppHk.MapControl.ActiveView.Refresh();
            }
            else
            {
                _AppHk.PageLayoutControl.CurrentTool = null;
                _AppHk.PageLayoutControl.ActiveView.Refresh();
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null)
                return;

            _cmd = new ControlsSelectToolClass();
            _cmd.OnCreate(_AppHk.MapControl);

        }
    }
}
