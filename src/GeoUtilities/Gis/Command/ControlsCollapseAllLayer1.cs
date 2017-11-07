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
    public class ControlsCollapseAllLayer1 : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef myHook;
        public ControlsCollapseAllLayer1()
        {

            base._Name = "GeoUtilities.ControlsCollapseAllLayer1";
            base._Caption = "折叠所有图层";
            base._Tooltip = "折叠所有图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "折叠所有图层";
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
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            for (int i = 0; i < playerCount; i++)
            {
                ILayer player = myHook.MapControl.Map.get_Layer(i);
                if (player is IGroupLayer)
                {
                    //如果图层是GroupLayer，那么将它的Expanded设置成FALSE
                    IGroupLayer pGroupLayer = player as IGroupLayer;
                    CollapseGroupLayer(pGroupLayer);
                    pGroupLayer.Expanded = false;
                }
                else if (player is IFeatureLayer)
                {
                    //如果不是GroupLayer，利用pLegendGroup.Visible来折叠图层
                    pFeatureLayer = player as IFeatureLayer;
                    CollapseLayer(pFeatureLayer);
                }
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(base._Tooltip);//xisheng 2011.07.08 增加日志
            }
            myHook.TOCControl.Update();
        }
        private void CollapseGroupLayer(IGroupLayer pGroupLauer)
        {
            ICompositeLayer pComLayer = pGroupLauer as ICompositeLayer;
            for (int j = 0; j < pComLayer.Count; j++)
            {
                ILayer mmLayer = pComLayer.get_Layer(j);
                IFeatureLayer pFeatureLayer = mmLayer as IFeatureLayer;
                CollapseLayer(pFeatureLayer);
            }
        }
        private void CollapseLayer(IFeatureLayer pFeatureLayer)
        {
            ILegendInfo pLegendInfo = null;
            ILegendGroup pLegendGroup = new LegendGroupClass();
            if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
            {
                #region 如果图层是注记层
                //IAnnotationLayer pAnnoLayer = pFeatureLayer as IAnnotationLayer;
                //IGeoFeatureLayer pGeoFeaLayer = pFeatureLayer as IGeoFeatureLayer;
                //IFeatureRenderer pRender = pGeoFeaLayer.Renderer;
                //ISymbol pSym = (pRender as IUniqueValueRenderer).DefaultSymbol;
                //ISimpleTextSymbol pTextSym = pSym as ISimpleTextSymbol;
                //ICompositeLayer pComLayer = pAnnoLayer as ICompositeLayer;
                //IGroupLayer pgLayer = new GroupLayerClass();
                //pgLayer.Name = pFeatureLayer.Name;
                //for (int j = 0; j < pComLayer.Count; j++)
                //{
                //    ILayer ppLayer = pComLayer.get_Layer(j);
                //    pgLayer.Add(ppLayer);
                //}
                //myHook.MapControl.Map.DeleteLayer(pFeatureLayer as ILayer);
                //myHook.MapControl.Map.AddLayer(pgLayer);
                //myHook.MapControl.ActiveView.Refresh();
                //pgLayer.Expanded = false;
                //myHook.TOCControl.Update();
                #endregion
            }
            else
            {
                //如果图层时普通的图层
                pLegendInfo = pFeatureLayer as ILegendInfo;
                if (pLegendInfo == null) return;
                for (int i = 0; i < pLegendInfo.LegendGroupCount; i++)//changed by xisheng 2011.06.16
                {//采用唯一值符号多个的一起折叠
                    pLegendGroup = pLegendInfo.get_LegendGroup(i);
                    pLegendGroup.Visible = false;
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
            myHook = hook as Plugin.Application.IAppFormRef;
            if (myHook.MapControl == null) return;
        }
    }
}
