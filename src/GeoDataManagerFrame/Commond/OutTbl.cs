using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
namespace GeoDataManagerFrame
{
    public class OutTbl : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public OutTbl()
        {
            base._Name = "GeoDataManagerFrame.OutTbl";
            base._Caption = "表格输出";
            base._Tooltip = "表格输出";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "表格输出";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            if (m_Hook.ArcGisMapControl == null)
                return;

            FormLandUseReport form = new FormLandUseReport();
            form.ShowDialog();
            SetControl pSetControl = m_Hook.MainUserControl as SetControl;
            //if (pSetControl!=null) pSetControl.InitOutPutResultTree();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
