using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SysCommon.Error;
namespace GeoUtilities
{
   public  class ControlsQueryAspectCommand : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public ControlsQueryAspectCommand()
        {
            base._Name = "GeoUtilities.ControlsQueryAspectCommand";
            base._Caption = "坡向查询";
            base._Tooltip = "坡向查询";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "坡向查询";
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
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
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
                frmQuerySlope pfrmQuerySlope = new frmQuerySlope("坡向");
                pfrmQuerySlope.WriteLog = WriteLog;//ygc 2012-9-12 是否写日志
                pfrmQuerySlope.pMapControlDefault = m_Hook.MapControl;
                pfrmQuerySlope.initialization();
                pfrmQuerySlope.Show();

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
