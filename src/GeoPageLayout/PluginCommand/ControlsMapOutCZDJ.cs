using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110928
    /// 说明：单个城镇地籍图
    /// </summary>
   public class ControlsMapOutCZDJ : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       private Plugin.Application.IAppFormRef m_frmhook;
       Plugin.Application.IApplicationRef pHook;
       public ControlsMapOutCZDJ()
        {
            base._Name = "GeoPageLayout.ControlsMapOutCZDJ";
            base._Caption = "城镇地籍图";
            base._Tooltip = "城镇地籍图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "城镇地籍图";
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

            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("标准分幅制图 提示‘当前没有调阅数据！’", m_Hook.tipRichBox);
                }
                return;
            }
            ISpatialReference pSpatialRefrence = m_Hook.ArcGisMapControl.SpatialReference;
            if (!(pSpatialRefrence is IProjectedCoordinateSystem))
            {
                //MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               //Plugin.LogTable.Writelog("标准分幅制图 提示‘请设置地图的投影坐标！’", m_Hook.tipRichBox);
                //return;
            }
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("城镇地籍图", m_Hook.tipRichBox);
                }
                m_Hook.ArcGisMapControl.CurrentTool = null;
                FrmSheetMapUserSet fmSMUS = new
                     FrmSheetMapUserSet(m_Hook.ArcGisMapControl, m_frmhook.MainForm, pHook, SheetType.urbanCadastre);
                fmSMUS.Show(m_frmhook.MainForm);
                fmSMUS.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志

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
