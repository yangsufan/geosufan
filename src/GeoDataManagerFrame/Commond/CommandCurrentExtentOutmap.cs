using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace GeoDataManagerFrame
{
   public class CommandCurrentExtentOutmap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       public Plugin.Application.IAppFormRef m_frmhook;
       public CommandCurrentExtentOutmap()
        {
            base._Name = "GeoDataManagerFrame.CommandCurrentExtentOutmap";
            base._Caption = "当前范围出图";
            base._Tooltip = "当前范围出图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "当前范围出图";
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
            //    log.Writelog("当前范围出图");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            m_Hook.MapControl.CurrentTool = null;
            IEnvelope pCurrentExtent=(m_Hook.MapControl.Map as IActiveView).Extent;
            SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载制图界面，请稍候...");
            pgss.EnableCancel = false;
            pgss.ShowDescription = false;
            pgss.FakeProgress = true;
            pgss.TopMost = true;
            pgss.ShowProgress();
            Application.DoEvents();
            GeoPageLayout.FrmPageLayout fmPageLayout = new GeoPageLayout.FrmPageLayout(m_Hook.MapControl.Map, pCurrentExtent);
            fmPageLayout.WriteLog = this.WriteLog;//2012-9-12 是否写日志
            fmPageLayout.typeZHT = 2;
            fmPageLayout.Show();
            pgss.Close();

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
