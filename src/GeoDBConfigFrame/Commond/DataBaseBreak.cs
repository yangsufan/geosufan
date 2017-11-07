using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
//连接数据库  断开数据库
namespace GeoDBConfigFrame
{
    public class DataBaseBreak : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public DataBaseBreak()
        {
            base._Name = "GeoDBConfigFrame.DataBaseBreak";
            base._Caption = "断开数据库";
            base._Tooltip = "断开数据库";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "断开数据库";
        }

        public override void OnClick()
        {

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("断开数据库");

                }
            }
            //断开连接

            //清空listview内容
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
