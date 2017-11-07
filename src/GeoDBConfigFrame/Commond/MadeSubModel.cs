using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//编辑专题脚本
namespace GeoDBConfigFrame
{
    public class MadeSubModel : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public MadeSubModel()
        {
            base._Name = "GeoDBConfigFrame.MadeSubModel";
            base._Caption = "编辑专题脚本";
            base._Tooltip = "编辑专题脚本";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "编辑专题脚本";
        }

        public override void OnClick()
        {
            //从全国行政区代码表中查找到当前需要切换的代码。复制到数据单元表中

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("编辑专题脚本");
                }

                SubIndexScript dlg = new SubIndexScript();
                dlg.Show();
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
