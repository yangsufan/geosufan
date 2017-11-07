using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;

namespace GeoUtilities
{
    /// <summary>
    /// 右键菜单折叠图层   chenyafei
    /// </summary>
    public class ControlsUnVisibleAllLayer : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef myHook;
        public ControlsUnVisibleAllLayer()
        {

            base._Name = "GeoUtilities.ControlsUnVisibleAllLayer";
            base._Caption = "关闭所有图层";
            base._Tooltip = "关闭所有图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "关闭所有图层";
        }
        public override bool Enabled
        {
            get
            {
                if (myHook.MapControl == null || myHook.TOCControl == null) return false;
                if (myHook.MapControl.LayerCount == 0) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            int playerCount = myHook.MapControl.Map.LayerCount;//获得MapControl上图层的数量
            for (int i = 0; i < playerCount; i++)
            {
                ILayer player = myHook.MapControl.Map.get_Layer(i);
                if (player is IGroupLayer)
                {
                    //如果是GroupLayer
                    IGroupLayer pGroupLayer = player as IGroupLayer;
                    UnVisibleGroupLayer(pGroupLayer);
                }
                else if (player is IFeatureLayer || player is IRasterLayer || player is IRasterCatalog)
                {
                    player.Visible = false;
                }
            }
            Plugin.LogTable.Writelog(Message);//xisheng 2011.07.08 增加日志
            myHook.MapControl.ActiveView.Refresh();
        }
        //使图层组内所有图层可见
        private void UnVisibleGroupLayer(IGroupLayer pGroupLayer)
        {
            ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
            if (pComLayer != null)
            {
                for (int i = 0; i < pComLayer.Count; i++)
                {
                    ILayer player = pComLayer.get_Layer(i);
                    player.Visible = false;
                    if (player is IGroupLayer)
                    {
                        UnVisibleGroupLayer(player as IGroupLayer);
                    }
                }
            }
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
