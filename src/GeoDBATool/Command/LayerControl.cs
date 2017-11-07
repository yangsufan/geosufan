using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    /// <summary>
    /// 目标图层 陈亚飞添加20101124
    /// </summary>
    public class LayerControl : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef myHook;
        private DevComponents.DotNetBar.ComboBoxItem _ComboBoxLayer;

        private IMap _map;               //一定将map设为全局变量否则IActiveViewEvents_Event相关事件无法响应

        public LayerControl()
        {
            base._Name = "GeoDBATool.LayerControl";
            base._Caption = "目标图层";
            base._Tooltip = "目标图层";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "目标图层";
        }

        public override bool Enabled
        {
            get
            {
                if (myHook == null) return false;
                if (_ComboBoxLayer == null)
                {
                    //获取图层控制下拉框
                    foreach (KeyValuePair<DevComponents.DotNetBar.BaseItem, string> kvCmd in Plugin.ModuleCommon.DicBaseItems)
                    {
                        if (kvCmd.Value == "GeoDBATool.LayerControl")
                        {
                            DevComponents.DotNetBar.ComboBoxItem aComboBoxItem = kvCmd.Key as DevComponents.DotNetBar.ComboBoxItem;
                            if (aComboBoxItem != null)
                            {
                                aComboBoxItem.ComboWidth = 150;
                                _ComboBoxLayer = aComboBoxItem;
                                _ComboBoxLayer.SelectedIndexChanged += new EventHandler(ComboBoxLayer_SelectedIndexChanged);
                                break;
                            }
                        }
                    }
                }

                return true;
            }
        }

        public override void OnClick()
        {

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            myHook = hook as Plugin.Application.IAppGISRef;

            _map = myHook.ArcGisMapControl.ActiveView.FocusMap;
            ((IActiveViewEvents_Event)_map).ItemAdded += new IActiveViewEvents_ItemAddedEventHandler(LayerControl_ItemAdded);
            ((IActiveViewEvents_Event)_map).ItemDeleted += new IActiveViewEvents_ItemDeletedEventHandler(LayerControl_ItemDeleted);
        }

        private void ComboBoxLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //List的图层循序与ComboBoxLayer的图层循序一致(在加载时设置)
            List<ILayer> plistLay = _ComboBoxLayer.Tag as List<ILayer>;
            if (plistLay == null) return;
            ModData.m_CurLayer=plistLay[_ComboBoxLayer.SelectedIndex];
        }

        private void LayerControl_ItemAdded(object Item)
        {
            if (_ComboBoxLayer == null) return;
            GetLayers();
        }

        private void LayerControl_ItemDeleted(object Item)
        {
            if (_ComboBoxLayer == null) return;
            GetLayers();
        }

        public void GetLayers()
        {
            _ComboBoxLayer.Items.Clear();
            List<ILayer> plistLay = new List<ILayer>();// ModPublic.LoadAllEditLyr(myHook.MapControl.Map);
            for (int i = 0; i < myHook.MapControl.LayerCount; i++)
            {
                ILayer pLayer = myHook.MapControl.Map.get_Layer(i);
                if (pLayer is IGroupLayer) return;
                if (!plistLay.Contains(pLayer))
                {
                    plistLay.Add(pLayer);
                    _ComboBoxLayer.Items.Add(plistLay[i].Name);
                }
            }
            if (plistLay.Count != 0)
            {
                _ComboBoxLayer.Tag = plistLay;
                _ComboBoxLayer.SelectedIndex = 0;
            }
            //_ComboBoxLayer.s
                _ComboBoxLayer.Refresh();
        }
    }
}
