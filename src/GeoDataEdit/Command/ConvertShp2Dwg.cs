using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
namespace GeoDataEdit
{
    public class ConvertShp2Dwg : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ConvertShp2Dwg()
        {
            base._Name = "GeoDataEdit.ConvertShp2Dwg";
            base._Caption = "shp和dwg转换";
            base._Tooltip = "shp和dwg转换";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "shp和dwg转换";
        }
        public override bool Enabled
        {
            get
            {
                if (m_frmhook == null) return false;
                //if (m_Hook.CurrentControl == null) return false;
                //if (m_Hook.CurrentControl is ISceneControl) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (m_frmhook == null)
                return;
          
            try
            {
                frmShp2Dwg fmMBMM = new frmShp2Dwg();
                fmMBMM.WriteLog = this.WriteLog;
                fmMBMM.ShowDialog(); 
            }
            catch
            { 
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}