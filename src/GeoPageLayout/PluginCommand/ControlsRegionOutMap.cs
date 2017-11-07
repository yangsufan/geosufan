using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using GeoDataCenterFunLib;

namespace GeoPageLayout
{
    public class ControlsRegionOutMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       private Plugin.Application.IAppFormRef m_frmhook;
       Plugin.Application.IApplicationRef pHook;
       public ControlsRegionOutMap()
        {
            base._Name = "GeoPageLayout.ControlsRegionOutMap";
            base._Caption = "行政区专题图";
            base._Tooltip = "行政区专题图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "行政区专题图";
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
            //    log.Writelog("标准图幅");
            //}
            //if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            //{
            //    MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    if (this.WriteLog)
            //    {
            //        Plugin.LogTable.Writelog("标准分幅制图 提示‘当前没有调阅数据！’", m_Hook.tipRichBox);
            //    }
            //    return;
            //}
            //ISpatialReference pSpatialRefrence = m_Hook.ArcGisMapControl.SpatialReference;
            //if (!(pSpatialRefrence is IProjectedCoordinateSystem))
            //{
            //    //MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //   //Plugin.LogTable.Writelog("标准分幅制图 提示‘请设置地图的投影坐标！’", m_Hook.tipRichBox);
            //    //return;
            //}
            //if (this.WriteLog)
            //{
            //    Plugin.LogTable.Writelog("标准分幅制图", m_Hook.tipRichBox);
            //}
            //m_Hook.ArcGisMapControl.CurrentTool = null;
            try
            {
                //FrmSheetMapUserSet fmSMUS = new
                //     FrmSheetMapUserSet(m_Hook.ArcGisMapControl, m_frmhook.MainForm, pHook, SheetType.foundationTerrain);
                //fmSMUS.Show(m_frmhook.MainForm);
                //ygc 2013-01-28 屏蔽原有行政区选择方式
                FrmGetXZQLocation newfrom = new FrmGetXZQLocation();
                newfrom.m_DefaultMap = m_Hook.MapControl;
                newfrom.m_IsClose = true;
                if (newfrom.ShowDialog() != DialogResult.OK) return;
                newfrom.drawgeometryXOR(newfrom .m_pGeometry);
                FrmPageLayout frmPage = new FrmPageLayout(m_Hook.MapControl.Map, newfrom.m_pGeometry, true);
                frmPage.Show();

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
            pHook = hook;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
