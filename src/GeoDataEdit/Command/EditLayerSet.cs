using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using SysCommon.Error;
namespace GeoDataEdit
{
    public class EditLayerSet : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public EditLayerSet()
        {
            base._Name = "GeoDataEdit.EditLayerSet";
            base._Caption = "编辑图层设置";
            base._Tooltip = "编辑图层设置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "编辑图层设置";
        }
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook.MapControl.Map.LayerCount == 0)
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
            }
        }
        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            //LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

            //if (log != null)
            //{
            //    log.Writelog("鹰眼图设置");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                FrmEditLayerSet fmMBMM = new FrmEditLayerSet(m_Hook.MapControl);
                fmMBMM.WriteLog = this.WriteLog;
                //int fmMBMMX = m_frmhook.MainForm.Location.X + m_frmhook.MainForm.Width - fmMBMM.Width;
                //int fmMBMMY = m_frmhook.MainForm.Location.Y + 100;
                //fmMBMM.SetDesktopLocation(fmMBMMX, fmMBMMY);
                if (fmMBMM.ShowDialog() == DialogResult.OK)
                {


 
                }
            }
            catch(Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "配置文件editlayer.xml出错！缺少相关的属性。");
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