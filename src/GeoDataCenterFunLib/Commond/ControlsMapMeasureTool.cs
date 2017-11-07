using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;

namespace GeoDataCenterFunLib
{
    public class ControlsMapMeasureTool : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;

        public ControlsMapMeasureTool()
        {
            base._Name = "GeoDataCenterFunLib.ControlsMapMeasureTool";
            base._Caption = "量算工具";
            base._Tooltip = "量算工具";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "进行量算";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerPan;
            //base._Image = "";
            //base._Category = "";
        }

        //public override bool Enabled
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (_AppHk.CurrentControl is IPageLayoutControl2)
        //            {
        //                base._Enabled = false;
        //                return false;
        //            }
        //            base._Enabled = true;
        //            return true;
        //        }
        //        catch
        //        {
        //            base._Enabled = false;
        //            return false;
        //        }
        //    }
        //}

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.Map.LayerCount == 0)
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

        public override bool Checked
        {
            get
            {
                if (_AppHk.CurrentTool != this.Name) return false;
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
            if (_tool == null || _cmd == null || _AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            _AppHk.MapControl.CurrentTool = _tool;
            _AppHk.CurrentTool = this.Name;
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            _tool = new ControlsMapMeasureToolDefClass();
            _cmd = _tool as ICommand;
            _cmd.OnCreate(_AppHk);
        }
    }
}
