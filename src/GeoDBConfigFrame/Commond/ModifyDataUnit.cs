using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
//using ModifyDataUnitControl;
using GeoDataCenterFunLib;

//修改数据单元
namespace GeoDBConfigFrame
{
    public class ModifyDataUnit : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public ModifyDataUnit()
        {
            base._Name = "GeoDBConfigFrame.ModifyDataUnit";
            base._Caption = "修改数据单元";
            base._Tooltip = "修改数据单元";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "修改数据单元";
        }

        public override void OnClick()
        {
            //从全国行政区代码表中查找到当前需要切换的代码。复制到数据单元表中
            ModifyDataUnitControl dlg = new ModifyDataUnitControl();
            dlg.Show();
            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("修改数据单元");

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
