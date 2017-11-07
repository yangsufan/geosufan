using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Error;

namespace GeoDataCenterFunLib
{
    public class ControlsContainQueryTool : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsContainQueryTool()
        {
            base._Name = "GeoDataCenterFunLib.ControlsContainQueryTool";
            base._Caption = "包含查询";
            base._Tooltip = "包含查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "包含查询";
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
          
            if (m_Hook.ArcGisMapControl.Map.LayerCount == 0)
            {
                MessageBox.Show("当前没有调阅数据！", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            try
            {
                frmQuerySet fmQS = new frmQuerySet(m_Hook.ArcGisMapControl.Map, queryType.Contain);
                if (fmQS.ShowDialog() != DialogResult.OK)
                    return;
                frmQuery fmQuery = new frmQuery(m_Hook.MapControl, enumQueryMode.Visiable, false);
                SysCommon.CProgress pgss = new SysCommon.CProgress("正在查询，请稍候...");
                pgss.EnableCancel = false;
                pgss.ShowDescription = false;
                pgss.FakeProgress = true;
                pgss.TopMost = true;
                pgss.ShowProgress();
                Application.DoEvents();
                fmQuery.FillData(fmQS.lstQueryedLayer, fmQS.GeometryBag, queryType.Contain);
                pgss.Close();
                Application.DoEvents();
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog(Caption);
                }
                fmQuery.Show(m_frmhook.MainForm);

            }
            catch (Exception exError)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
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
