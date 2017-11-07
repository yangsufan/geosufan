using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsHideAllCommand : Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsHideAllCommand()
        {
            base._Name = "GeoUtilities.ControlsHideAllCommand";
            base._Caption = "关闭";
            base._Tooltip = "关闭所有图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "关闭视图上所有图层数据";
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
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
           for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
           {
               ILayer GetLayer = _AppHk.MapControl.get_Layer(i);
               GetLayer.Visible = false;
               if (GetLayer is IGroupLayer)
               {
                   SetGroupLayerToVisualUnenable(GetLayer as IGroupLayer);
               }
           }
           if (this.WriteLog)
           {
               Plugin.LogTable.Writelog(Message);//xisheng 2011.07.08 增加日志
           }

           _AppHk.MapControl.ActiveView.Refresh();
           _AppHk.TOCControl.Update();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }

        private void SetGroupLayerToVisualUnenable(IGroupLayer in_GroupLayer)
        {
            ICompositeLayer pComLayer = in_GroupLayer as ICompositeLayer;
            try
            {
                int iCount = pComLayer.Count;
                for (int i = 0; i < iCount; i++)
                {
                    ILayer pGetLayer = pComLayer.get_Layer(i);
                    pGetLayer.Visible = false;
                }

            }
            catch (Exception eError)
            {
                if (null == SysCommon.Log.Module.SysLog) SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
            }
        }
    }
}
