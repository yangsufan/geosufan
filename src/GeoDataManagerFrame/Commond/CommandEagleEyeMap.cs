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
   public class EagleEyeMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        FrmEagleEye fmEE = null;
        public EagleEyeMap()
        {
            base._Name = "GeoDataManagerFrame.EagleEyeMap";
            base._Caption = "鹰眼图";
            base._Tooltip = "鹰眼图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "鹰眼图";

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
                return;
            }
            if (fmEE==null)//第一次打开鹰眼  第二次关闭鹰眼
            {
                this._Checked = true;
                try
                {
                    System.Drawing.Point p = m_Hook.ArcGisMapControl.PointToScreen(m_Hook.ArcGisMapControl.Location);
                    fmEE = new FrmEagleEye(m_Hook.ArcGisMapControl, this,Plugin.ModuleCommon.TmpWorkSpace);
                    fmEE.WriteLog = this.WriteLog; //ygc 2012-9-12 是否写日志
                    //int fmEEX = m_frmhook.MainForm.Location.X + m_frmhook.MainForm.Width - fmEE.Width;主窗体右下角
                    //int fmEEY = m_frmhook.MainForm.Location.Y + m_frmhook.MainForm.Height - fmEE.Height;
                    int fmEEX = p.X + m_Hook.ArcGisMapControl.Width - fmEE.Width;
                    int fmEEY = p.Y + m_Hook.ArcGisMapControl.Height - fmEE.Height;//地图控件右下角yjl20110820 add
                    fmEE.SetDesktopLocation(fmEEX, fmEEY);
                    fmEE.Show(m_frmhook.MainForm);
                    //changed by chulili 20110729 鹰眼图关闭事件
                    fmEE.FormClosed += new FormClosedEventHandler(FrmEagleEye_FormClosed);
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("打开鹰眼图");
                    }
                }
                catch
                {

                }
            }
            else 
            {
                try
                {
                    fmEE.Close();
                }
                catch
                {

                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
       //added by chulili 20110729 对关闭事件的处理
        private void FrmEagleEye_FormClosed(object sender, FormClosedEventArgs e)
        {
            fmEE = null;
            this._Checked = false;
        }
    }
}