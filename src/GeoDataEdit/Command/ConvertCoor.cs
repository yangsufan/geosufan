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
    public class ConvertCoor : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ConvertCoor()
        {
            base._Name = "GeoDataEdit.ConvertCoor";
            base._Caption = "坐标转换";
            base._Tooltip = "坐标转换";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "坐标转换";
        }
        public override bool Enabled
        {
            get
            {
                if (m_frmhook == null) return false;
                //if (m_frmhook.CurrentControl == null) return false;
                //if (m_frmhook.CurrentControl is ISceneControl) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (m_frmhook == null)
                return;
         
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(Caption); //ygc 2012-9-14 写日志
                }
                frmCoorTrans fmMBMM = new frmCoorTrans();
                
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