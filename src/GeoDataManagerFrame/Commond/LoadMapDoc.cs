using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;

//加载地图文档
namespace GeoDataManagerFrame
{
    public class LoadMapDoc : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public LoadMapDoc()
        {
            base._Name = "GeoDataManagerFrame.LoadMapDoc";
            base._Caption = "加载地图文档";
            base._Tooltip = "加载地图文档";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载地图文档";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "xml文件|*.xml";
            dlg.ShowDialog();
            string xmlpath = dlg.FileName;
            if (xmlpath.Equals(""))
                return;
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();

            LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
            string strLog = "加载地图文档,原始文件路径:" + xmlpath;
            vProgress.SetProgress("开始加载地图文档");
            if (log != null)
            {
                log.Writelog(strLog);
            }

            string strWorkFile = Application.StartupPath + "\\..\\Temp\\CurPrj.xml";
           
            System.IO.File.Copy(xmlpath, strWorkFile, true);
            
            SetControl pSetControl=(SetControl )m_Hook.MainUserControl;

            //清空前次加载的图层

            pSetControl.m_SaveXmlFileName = xmlpath;
            pSetControl.LoadDatafromXml(strWorkFile, vProgress);
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
