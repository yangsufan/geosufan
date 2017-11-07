using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace GeoUtilities
{
    public class ControlsZoomToVisibleScale1 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        public ControlsZoomToVisibleScale1()
        {
            base._Name = "GeoUtilities.ControlsZoomToVisibleScale1";
            base._Caption = "缩放到图层";
            base._Tooltip = "缩放到图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "缩放到图层";
        }
        public override bool Visible
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    if (pAppFormRef.LayerAdvTree != null)
                    {
                        DevComponents.AdvTree.AdvTree pTree = pAppFormRef.LayerAdvTree as DevComponents.AdvTree.AdvTree;
                        if (pTree.SelectedNode != null)
                        {
                            DevComponents.AdvTree.Node pNode = pTree.SelectedNode;
                            if (pNode != null)
                            {
                                if (pNode.Tag != null)
                                {
                                    string strtag = pNode.Tag.ToString();
                                    if (strtag.ToLower().Contains("layer"))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }
        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                ILayer mLayer = _AppHk.MapControl.CustomProperty as ILayer;
                if (mLayer == null) return false;
                bool bVisible = false;
                try
                {
                    bVisible = SysCommon.ModuleMap.GetScaleVisibleOfLayer(_AppHk.MapControl.Map.MapScale, mLayer);
                }
                catch
                { }
                if (bVisible)
                {
                    return false;
                }
                if (mLayer is IDynamicLayer) return false;
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
            try
            {
                ILayer pLayer = null;
                pLayer = (ILayer)_AppHk.MapControl.CustomProperty;
                if (pLayer == null) return;
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.08 增加日志
                }
                IActiveView pActiveView = _AppHk.MapControl.Map as IActiveView;
                double OldScale = _AppHk.MapControl.Map.MapScale;
                pActiveView.Extent = pLayer.AreaOfInterest;
                //added by chulili 20111117 缩放前视图比例尺距离哪个比例尺最近，就缩放到哪个比例尺
                double dMaxScale = pLayer.MaximumScale;
                double dMinScale = pLayer.MinimumScale;
                if (dMaxScale > 0 && dMinScale > 0) //最大比例尺和最小比例尺都设置了
                {
                    if (Math.Abs(dMaxScale - OldScale) > Math.Abs(dMinScale - OldScale))    //距离哪个比例尺最近，就缩放到哪个
                    {
                        _AppHk.MapControl.Map.MapScale = dMinScale;
                        _AppHk.MapControl.Map.MapScale = dMinScale; //added by chulili 20111130 设置一遍有偏差，设置两遍几乎没有偏差
                    }
                    else
                    {
                        _AppHk.MapControl.Map.MapScale = dMaxScale;
                        _AppHk.MapControl.Map.MapScale = dMaxScale;
                    }
                }
                else
                {
                    if (dMaxScale <= 0) //未设置最大比例尺
                    {
                        if (dMinScale > 0)
                        {
                            _AppHk.MapControl.Map.MapScale = dMinScale;
                            _AppHk.MapControl.Map.MapScale = dMinScale;
                        }
                    }
                    else if (dMinScale <= 0)    //未设置最小比例尺
                    {
                        _AppHk.MapControl.Map.MapScale = dMaxScale;
                        _AppHk.MapControl.Map.MapScale = dMaxScale;
                    }
                }
                //end added by chulili
                //if (pLayer.MinimumScale > 0)
                //    _AppHk.MapControl.Map.MapScale = pLayer.MinimumScale;//yjl20111014 add zoomtovisible 
                pActiveView.Refresh();
            }
            catch (Exception eError)
            {
                if (SysCommon.Log.Module.SysLog == null) SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eError);
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
        }
    }
}
