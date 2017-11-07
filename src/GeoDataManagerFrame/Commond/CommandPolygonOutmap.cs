using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.ADF;

namespace GeoDataManagerFrame
{
   public class CommandPolygonOutmap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       public Plugin.Application.IAppFormRef m_frmhook;
       private ICommand _cmd = null;
       public CommandPolygonOutmap()
        {
            base._Name = "GeoDataManagerFrame.CommandPolygonOutmap";
            base._Caption = "多边形范围出图";
            base._Tooltip = "多边形范围出图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "多边形范围出图";
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
            //    log.Writelog("多边形范围出图");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _cmd = new GeoPageLayout.CommandPolygonOutmap();
            CommandPolygonOutmap tempCommand = _cmd as CommandPolygonOutmap;
            tempCommand.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            tempCommand.OnCreate(m_Hook);
            m_Hook.MapControl.CurrentTool = tempCommand as ITool;
            //m_Hook.MapControl.MousePointer = esriControlsMousePointer.esriPointerCrosshair;//yjl0616鼠标指针
            //SetControl pSetControl = m_Hook.MainUserControl as SetControl ;
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
