using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using SysCommon.Error;
using System.IO;
using System.Xml;
namespace GeoDataManagerFrame
{
    public class CommandSheetQuery : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public CommandSheetQuery()
        {
            base._Name = "GeoDataManagerFrame.CommandSheetQuery";
            base._Caption = "图幅号查询";
            base._Tooltip = "图幅号查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "";
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentControl == null) return false;
                if (m_Hook.CurrentControl is ISceneControl) return false;
                return true;
            }
        }
        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            //LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            //if (log != null)
            //{
            //    log.Writelog("地名地址查询");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
          
            FrmSheetMapNoSet fmMBMM = new FrmSheetMapNoSet(m_Hook.ArcGisMapControl);
            //int fmMBMMX = m_frmhook.MainForm.Location.X + m_frmhook.MainForm.Width - fmMBMM.Width;
            //int fmMBMMY = m_frmhook.MainForm.Location.Y + 100;
            //fmMBMM.SetDesktopLocation(fmMBMMX, fmMBMMY);
            fmMBMM.Show(m_frmhook.MainForm);
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