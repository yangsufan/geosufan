using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.Generic;

using System.Windows.Forms;
using ESRI.ArcGIS.Display;


/*-----------------------------------------------------------
 added by xisheng 20110806 范围出图菜单文件 CommandRangeOutmap.cs
 -----------------------------------------------------------*/
namespace GeoPageLayout
{

    public sealed class CommandRangeOutmap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public CommandRangeOutmap()
        {
            base._Name = "GeoPageLayout.CommandRangeOutmap";
            base._Caption = "坐标范围制图";
            base._Tooltip = "坐标范围制图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "坐标范围制图";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            frmRangeOutMap frm = new frmRangeOutMap(m_Hook.MapControl, m_frmhook.MainForm);
            frm.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
            frm.Show();

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
