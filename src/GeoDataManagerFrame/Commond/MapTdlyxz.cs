using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;

namespace GeoDataManagerFrame
{
   public class MapTdlyxz : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       public Plugin.Application.IAppFormRef m_frmhook;
        public MapTdlyxz()
        {
            base._Name = "GeoDataManagerFrame.MapTdlyxz";
            base._Caption = "土地利用现状图";
            base._Tooltip = "土地利用现状图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "土地利用现状图";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            if (log != null)
            {
                log.Writelog("生成土地利用现状图");
            }
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
      //?      GeoPageLayout.GeoPageLayout pPageLayout = new GeoPageLayout.GeoPageLayout(m_Hook.ArcGisMapControl.Map);
       //?     pPageLayout.MapTdlyxz();
      // ?     pPageLayout = null;
            SetControl pSetControl = m_Hook.MainUserControl as SetControl ;
            //pSetControl.InitOutPutResultTree();
            

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
