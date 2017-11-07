using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoDBConfigFrame
{
    public class ControlsParameters : Plugin.Interface.CommandRefBase
    {
        private UserControl ucCtl = null;
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        private Plugin.Application.IAppFormRef _hook;

        public ControlsParameters()
        {
            base._Name = "GeoDBConfigFrame.ControlsParameters";
            base._Caption = "参数管理";
            base._Tooltip = "参数管理";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "参数管理";
        }

        public override bool Enabled
        {
            get
            {
                if (ModFrameData.v_AppPrivileges == null)
                    return false;
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
                Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Plugin.Application.IAppPrivilegesRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppPrivilegesRef pAppFormRef = m_Hook as Plugin.Application.IAppPrivilegesRef;
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
                ucCtl = ModFrameData.v_AppPrivileges.MainUserControl;
            }
            if (ucCtl != null)
            {
                ucCtl.Visible = true;
                ucCtl.Enabled = true;
                _hook.MainForm.Controls.SetChildIndex(ucCtl, 0);
                //for (int i = 1; i < _hook.MainForm.Controls.Count; i++)
                //{
                //    if (_hook.MainForm.Controls[i].Name.Equals("UCDataSourceManger"))
                //    {
                //        _hook.MainForm.Controls[i].Visible = false;
                //        break;
                //    }
                //}
            }
            if (this.WriteLog )
            {
                Plugin.LogTable.Writelog("数据字典管理");//xisheng 2011.07.09 增加日志
            }
          
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
