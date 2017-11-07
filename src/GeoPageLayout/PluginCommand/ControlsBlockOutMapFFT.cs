using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoPageLayout
{
   /// <summary>
   /// yjl 20111028  add  批量范围专题图  分幅图 
   /// </summary>
    public class ControlsBlockOutMapFFT:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsBlockOutMapFFT()
        {
            base._Name = "GeoPageLayout.ControlsBlockOutMapFFT";
            base._Caption = "矢量批量范围专题图";
            base._Tooltip = "矢量批量范围专题图";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "矢量批量范围专题图";
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
           
            FrmBlockOutMapFFT pFrmBlockOutMap = new FrmBlockOutMapFFT();
            if (pFrmBlockOutMap.ShowDialog() != DialogResult.OK)
                return;
            //设置主界面鼠标指针为等待
            Cursor cur = m_frmhook.MainForm.Cursor;
            m_frmhook.MainForm.Cursor = Cursors.WaitCursor;
            try
            {
                GeoPageLayout geoPageLayout = new GeoPageLayout();
                geoPageLayout.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                geoPageLayout.pageLayoutExtentBatFFT(m_Hook.ArcGisMapControl.Map, pFrmBlockOutMap.m_QueryResult, pFrmBlockOutMap.ExtentFC, pFrmBlockOutMap.OutputPath);
                
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
            }
            m_frmhook.MainForm.Cursor = cur;
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
