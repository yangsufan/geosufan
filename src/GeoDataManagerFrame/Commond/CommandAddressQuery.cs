using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
namespace GeoDataManagerFrame
{
    public class CommandAddressQuery : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public CommandAddressQuery()
        {
            base._Name = "GeoDataManagerFrame.CommandAddressQueryBZW";
            base._Caption = "地名地址查询";
            base._Tooltip = "地名地址查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "地名地址查询";
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
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("一般查询 提示‘当前没有调阅数据！’");
                }
                return;
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("地名地址查询");
            }
            FrmAddressQuery fmMBMM = new FrmAddressQuery(m_Hook.MapControl.Map);
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