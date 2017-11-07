using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
//入库数据分析
namespace GeoDBConfigFrame
{
    public class AnalyseInLibMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;

        public AnalyseInLibMap()
        {
            base._Name = "GeoDBConfigFrame.AnalyseInLibMap";
            base._Caption = "入库数据分析";
            base._Tooltip = "入库数据分析";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "入库数据分析";
        }

        public override void OnClick()
        {
            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("入库图形数据分析");

                }
            }
            //从全国行政区代码表中查找到当前需要切换的代码。复制到数据单元表中
            frmAnalyseInLibMap frm = new frmAnalyseInLibMap();
            frm.Show();
           
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef; 
        }
    }
}
