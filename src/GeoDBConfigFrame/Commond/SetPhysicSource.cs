using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using GeoDataCenterFunLib;

namespace GeoDBConfigFrame
{
   public class SetPhysicSource : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public SetPhysicSource()
        {
            base._Name = "GeoDBConfigFrame.SetPhysicSource";
            base._Caption = "配置图形数据源";
            base._Tooltip = "配置图形数据源";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "配置图形数据源";
        }

        public override void OnClick()
        {
            //从全国行政区代码表中查找到当前需要切换的代码。复制到数据单元表中

            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("配置物理数据源");

                }
                SetPhysicsDataSourceForm frm=new SetPhysicsDataSourceForm();
                frm.Show();
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
