using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Controls;
using System.Windows.Forms;

using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
namespace GeoDataManagerFrame
{
    public class SetEagleEyeMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SetEagleEyeMap()
        {
            base._Name = "GeoDataManagerFrame.SetEagleEyeMap";
            base._Caption = "鹰眼图设置";
            base._Tooltip = "鹰眼图设置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "鹰眼图设置";
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
            //    log.Writelog("鹰眼图设置");
            //}
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(Caption);
                }
                FrmEagleEyeSet fmMBMM = new FrmEagleEyeSet(m_Hook.MapControl.Map);
                //int fmMBMMX = m_frmhook.MainForm.Location.X + m_frmhook.MainForm.Width - fmMBMM.Width;
                //int fmMBMMY = m_frmhook.MainForm.Location.Y + 100;
                //fmMBMM.SetDesktopLocation(fmMBMMX, fmMBMMY);
                if (fmMBMM.ShowDialog() == DialogResult.OK)
                {

                    FormCollection pFC = Application.OpenForms;//鹰眼底图设置之后,打开的鹰眼视图更换为新的底图0617yjl
                    foreach (Form pFm in pFC)
                    {
                        if (pFm.Name == "FrmEagleEye")
                        {
                            (pFm as FrmEagleEye).InitEagleMap();
                        }
                    }
 
                }
            }
            catch
            { 
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