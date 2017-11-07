using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.ADF;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.SystemUI;

namespace GeoDataManagerFrame
{
   public class CommandSelOutmap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       public Plugin.Application.IAppFormRef m_frmhook;
       private ICommand _cmd = null;
       public CommandSelOutmap()
        {
            base._Name = "GeoDataManagerFrame.CommandSelOutmap";
            base._Caption = "选择要素范围出图";
            base._Tooltip = "选择要素范围出图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "选择要素范围出图";
        }
       public override bool Enabled
       {
           get
           {
               try
               {
                   if (m_Hook.CurrentControl is ISceneControl) return false;
                   if (m_Hook.MapControl.LayerCount == 0)
                   {
                       base._Enabled = false;
                       return false;
                   }

                   base._Enabled = true;
                   return true;
               }
               catch
               {
                   base._Enabled = false;
                   return false;
               }
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
                    Plugin.LogTable.Writelog("选择要素范围出图 提示'当前没有调阅数据!'", m_Hook.tipRichBox);
                }
                return;
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("选择要素范围出图", m_Hook.tipRichBox);
            }
            _cmd = new GeoPageLayout.CommandSelOutmap();
            CommandSelOutmap TempCommand = _cmd as CommandSelOutmap;
            TempCommand.WriteLog = this.WriteLog;//ygc 2012-9-12 是否写日志
            TempCommand.OnCreate(m_Hook);
            TempCommand.OnClick();
       
            SetControl pSetControl = m_Hook.MainUserControl as SetControl ;
            if (pSetControl != null)
            {
                pSetControl.InitOutPutResultTree();
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
