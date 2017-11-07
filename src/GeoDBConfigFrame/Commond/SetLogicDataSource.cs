using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//批量设置逻辑数据源
namespace GeoDBConfigFrame
{
    public class SetLogicDataSource : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public SetLogicDataSource()
        {
            base._Name = "GeoDBConfigFrame.SetLogicDataSource";
            base._Caption = "批量设置逻辑数据源";
            base._Tooltip = "批量设置逻辑数据源";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "批量设置逻辑数据源";
        }

        public override void OnClick()
        {
            //从全国行政区代码表中查找到当前需要切换的代码。复制到数据单元表中

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("批量设置逻辑数据源");

                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef; 
        }
    }
}
