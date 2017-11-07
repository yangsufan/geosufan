using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
namespace GeoDataManagerFrame
{
    public class DataStat : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public DataStat()
        {
            base._Name = "GeoDataManagerFrame.DataStat";
            base._Caption = "数据统计";
            base._Tooltip = "数据统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据统计";
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
            FormStatic form = new FormStatic();
            form.InitForm(pMap );
            form.ShowDialog();
            SetControl pSetControl = m_Hook.MainUserControl as SetControl;
            //pSetControl.InitOutPutResultTree();

            

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
