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
    public class ControlsVisibleAllLayer2 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef myHook;
        public ControlsVisibleAllLayer2()
        {

            base._Name = "GeoUtilities.ControlsVisibleAllLayer2";
            base._Caption = "显示所有图层";
            base._Tooltip = "显示所有图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "显示所有图层";
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
            bool changed = false;
            for (int i = 0; i < playerCount; i++)
            {
                ILayer player = myHook.MapControl.Map.get_Layer(i);
                if (player is IGroupLayer)
                {
                    //如果是GroupLayer
                    IGroupLayer pGroupLayer = player as IGroupLayer;
                    if (!pGroupLayer.Visible )
                    {
                        pGroupLayer.Visible = true;
                        changed = true;
                    }
                    bool bRes=VisibleGroupLayer(pGroupLayer);
                    if (bRes)
                    {
                        changed = true;
                    }
                }
                else if (player is IFeatureLayer || player is IRasterLayer || player is IRasterCatalog)
                {
                    if (!player.Visible)
                    {
                        player.Visible = true;
                        changed = true;
                    }
                }
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Message);//xisheng 2011.07.08 增加日志
            }
            if (changed)    //是否更改了图层的可见性，未更改则不刷新
            {
                myHook.MapControl.ActiveView.Refresh();
            }
        }
        //使图层组内所有图层可见
        private bool VisibleGroupLayer(IGroupLayer pGroupLayer)
        {
            bool changed = false;
            ICompositeLayer pComLayer = pGroupLayer as ICompositeLayer;
            if (pComLayer != null)
            {
                for (int i = 0; i < pComLayer.Count; i++)
                {
                    ILayer player = pComLayer.get_Layer(i);
                    if (!player.Visible)
                    {
                        player.Visible = true;
                        changed = true;
                    }
                    if (player is IGroupLayer)
                    {
                        bool bRes=VisibleGroupLayer(player as IGroupLayer);
                        if (bRes)
                        {
                            changed = true;
                        }
                    }
                }
            }
            return changed;
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