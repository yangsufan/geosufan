using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.SystemUI;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20111027
    /// 说明：选择范围专题图
    /// </summary>
   public class ControlsSelOutmapZT : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       public Plugin.Application.IAppFormRef m_frmhook;
       private ICommand _cmd = null;
       public ControlsSelOutmapZT()
        {
            base._Name = "GeoPageLayout.ControlsSelOutmapZD";
            base._Caption = "选择范围专题图";
            base._Tooltip = "选择范围专题图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "选择范围专题图";
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
            //    log.Writelog("选择要素范围出图");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("选择范围专题图 提示'当前没有调阅数据!'", m_Hook.tipRichBox);
                }
                return;
            }
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("选择范围专题图", m_Hook.tipRichBox);
                }
                _cmd = new CommandSelOutmapZT();
                CommandSelOutmapZT TempCommand = _cmd as CommandSelOutmapZT;
                TempCommand.WriteLog = this.WriteLog;//ygc 2012-9-12 是否写日志
                TempCommand.OnCreate(m_Hook.MapControl);
                TempCommand.OnClick();
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
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
