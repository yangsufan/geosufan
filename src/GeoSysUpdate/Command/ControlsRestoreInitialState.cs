using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

namespace GeoSysUpdate
{
    public class ControlsRestoreInitialState : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsRestoreInitialState()
        {
            base._Name = "GeoSysUpdate.ControlsRestoreInitialState";
            base._Caption = "恢复初始状态";
            base._Tooltip = "恢复初始状态";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "恢复初始状态";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                //if (_AppHk.MapControl.LayerCount == 0) return false;
                //ILayer pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
                //if (pLayer == null) return false;
                //if (pLayer.Name == "示意图" ||pLayer.Name=="Default" || pLayer.Name.StartsWith("MapFrame_")) return false;
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
            IMapControlDefault pMapControl = _hook.MapControl as IMapControlDefault;
            string layerTreeXmlPath = _hook.LayerTreeXmlPath;
            pMapControl.Map.ClearLayers();
            SysCommon.CProgress vProgress = null;
            try
            {
                //从数据库中拷贝图层树
                vProgress = new SysCommon.CProgress("恢复图层目录初始状态");
                vProgress.EnableCancel = false;
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                vProgress.SetProgress("读取图层目录");
                SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(ModData.v_AppGisUpdate.CurWksInfo.Wks, layerTreeXmlPath);
                GeoLayerTreeLib.LayerManager.UcDataLib pUcDataLib = _hook.LayerTree as GeoLayerTreeLib.LayerManager.UcDataLib;
                vProgress.SetProgress("恢复图层目录");
                pUcDataLib.LoadData();
                vProgress.Close();
            }
            catch
            {
                vProgress.Close();
            }
            SysCommon.ModSysSetting.IsConfigLayerTreeChanged = false;

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
