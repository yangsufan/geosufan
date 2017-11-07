using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoSysSetting
{
    public class ControlLogManager : Plugin.Interface.CommandRefBase
    {
        private GeoSysSetting.SubControl.UCLogManager ucCtl = null;
        private Plugin.Application.IAppFormRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlLogManager()
        {
            base._Name = "GeoSysSetting.ControlLogManager";
            base._Caption = "日志管理";
            base._Tooltip = "日志管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "日志管理";
        }

        public override bool Enabled
        {
            get
            {
                if (_hook == null) return true;
                //根据时间刷新一下改空间是否显示
                if (_hook.Visible == false && this.ucCtl != null)
                {
                    this.ucCtl.Visible = false;
                }
                return true;
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
            Exception eError;
            //changed by chulili20110718 先将目录管理界面不可见，再响应界面切换
            for (int i = 0; i < _hook.MainForm.Controls.Count; i++)
            {
                if (_hook.MainForm.Controls[i].Name.Equals("UCDataSourceManger"))
                {
                    _hook.MainForm.Controls[i].Enabled = false;
                    break;
                }
            }
            //end added by chulili 
            if (ucCtl == null)
            {
                ucCtl = new SubControl.UCLogManager ();
                ucCtl.Dock = DockStyle.Fill;



                _hook.MainForm.Controls.Add(ucCtl);
            }
            else
            {
                ucCtl.RefreshLog();
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
            }
            ucCtl.Visible = true;
            _hook.MainForm.Controls.SetChildIndex(ucCtl, 0);

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppFormRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }

    }
}
