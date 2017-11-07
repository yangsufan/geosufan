using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
namespace GeoDataManagerFrame
{
    public class DataStatCustomize : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public DataStatCustomize()
        {
            base._Name = "GeoDataManagerFrame.DataStatCustomize";
            base._Caption = "自定义统计";
            base._Tooltip = "自定义统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "自定义统计";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            if (m_Hook.ArcGisMapControl == null)
                return;
            if (m_Hook.ArcGisMapControl.Map == null)
                return;
            IMap pMap = m_Hook.ArcGisMapControl.Map;
            if (pMap.LayerCount == 0)
                return;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 增加日志
            }
            FormStatCustomize form = new FormStatCustomize();
            form.InitForm(pMap);
            form.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
