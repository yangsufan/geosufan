using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

namespace GeoUtilities
{
    /// <summary>
    /// 右键菜单扩展图层 chenyafei
    /// </summary>
    public class ControlsExpand : Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppArcGISRef myHook;
        public ControlsExpand()
        {

            base._Name = "GeoUtilities.ControlsExpand";
            base._Caption = "扩展图层";
            base._Tooltip = "扩展图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "扩展所有图层";
        }
        public override bool Enabled
        {
            get
            {
                if (myHook.MapControl == null || myHook.TOCControl == null) return false;
                if (myHook.MapControl.LayerCount == 0) return false;
                ILayer nLayer = myHook.MapControl.CustomProperty as ILayer;
                if (nLayer is IFeatureLayer)
                {
                    IFeatureLayer tLayer = nLayer as IFeatureLayer;
                    if (tLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        return false;
                    }
                }
                return true ;
            }
        }
        public override void OnClick()
        {
            int playerCount = myHook.MapControl.Map.LayerCount;//获得MapControl上图层的数量
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();

             ILayer mLayer = myHook.MapControl.CustomProperty  as ILayer ;
             if (mLayer is IGroupLayer)
             {
                 IGroupLayer pGLayer = mLayer as IGroupLayer;
                 if (pGLayer.Expanded == false)
                 {
                     pGLayer.Expanded = true;
                 }
                 ExpandGroupLayer(pGLayer);
             }
             else if (mLayer is IFeatureLayer)
             {
                 pFeatureLayer = mLayer as IFeatureLayer;
                 ExpandLayer(pFeatureLayer);
             }
             if (this.WriteLog)
             {
                 Plugin.LogTable.Writelog(Message);//xisheng 2011.07.08 增加日志
             }
            myHook.TOCControl.Update();
        }
        private void ExpandGroupLayer(IGroupLayer pGroupLayer)
        {
            ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
            for (int i = 0; i < pComLayer.Count; i++)
            {
                IFeatureLayer pFlayer = pComLayer.get_Layer(i) as IFeatureLayer;
                ExpandLayer(pFlayer);
            }
        }
        private void ExpandLayer(IFeatureLayer pFeatureLayer)
        {
            ILegendInfo pLegendInfo = null;
            ILegendGroup pLegendGroup = new LegendGroupClass();
            pLegendInfo = pFeatureLayer as ILegendInfo;
            if (pLegendInfo == null) return;
            pLegendGroup = pLegendInfo.get_LegendGroup(0);
            pLegendGroup.Visible = true;
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = myHook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppArcGISRef;
            if (myHook.MapControl == null) return;
        }
    }
}
