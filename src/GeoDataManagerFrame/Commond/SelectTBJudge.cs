using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;


//加载地图文档
namespace GeoDataManagerFrame
{
    public class SelectTBJudge : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SelectTBJudge()
        {
            base._Name = "GeoDataManagerFrame.SelectTBJudge";
            base._Caption = "变化图斑研判";
            base._Tooltip = "变化图斑研判";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "变化图斑研判";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();

            LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
            string strLog = "选择图斑研判";
            vProgress.SetProgress("选择图斑研判");
            if (log != null)
            {
                log.Writelog(strLog);
            }
            SetControl pSetControl = (SetControl)m_Hook.MainUserControl;
            pSetControl.SelectJudge(vProgress);
            vProgress.Close();
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
