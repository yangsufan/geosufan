using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110928
    /// 说明：单个宗地图
    /// </summary>
   public class ControlsMapOutZD : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
       private Plugin.Application.IAppFormRef m_frmhook;
       Plugin.Application.IApplicationRef pHook;
       private ICommand _cmd = null;
       public ControlsMapOutZD()
        {
            base._Name = "GeoPageLayout.ControlsMapOutCZDJ";
            base._Caption = "宗地图";
            base._Tooltip = "宗地图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "宗地图";
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
               Plugin.LogTable.Writelog("标准分幅制图 提示‘当前没有调阅数据！’", m_Hook.tipRichBox);
                return;
            }
            ISpatialReference pSpatialRefrence = m_Hook.ArcGisMapControl.SpatialReference;
            if (!(pSpatialRefrence is IProjectedCoordinateSystem))
            {
                //MessageBox.Show("请设置地图的投影坐标！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               //Plugin.LogTable.Writelog("标准分幅制图 提示‘请设置地图的投影坐标！’", m_Hook.tipRichBox);
                //return;
            }
            Plugin.LogTable.Writelog("宗地图", m_Hook.tipRichBox);
            _cmd = new CommandSelOutmapZD();
            _cmd.OnCreate(m_Hook.MapControl);
            _cmd.OnClick();
                

           
            

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
