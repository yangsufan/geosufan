using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoPageLayout
{
   /// <summary>
   /// ZQ 20111008  add  批量范围专题图   
   /// </summary>
    public class ControlsBlockOutMapRaster:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsBlockOutMapRaster()
        {
            base._Name = "GeoPageLayout.ControlsBlockOutMap";
            base._Caption = "栅格批量范围专题图";
            base._Tooltip = "栅格批量范围专题图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "栅格批量范围专题图";
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
            FrmBlockOutMapRaster pFrmBlockOutMap = new FrmBlockOutMapRaster();
            if(pFrmBlockOutMap.ShowDialog()!=DialogResult.OK)
                return;
            try
            {
                GeoPageLayout geoPageLayout = new GeoPageLayout();
                geoPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                geoPageLayout.pageLayoutExtentRasterBat(m_Hook.ArcGisMapControl.Map, pFrmBlockOutMap.m_QueryResult, pFrmBlockOutMap.ExtentFC, pFrmBlockOutMap.OutputPath);
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
