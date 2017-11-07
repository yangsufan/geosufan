using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
//索引
namespace GeoDBConfigFrame
{
    public class SysHelpIndex : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        public SysHelpIndex()
        {
            base._Name = "GeoDBConfigFrame.SysHelpIndex";
            base._Caption = "索引";
            base._Tooltip = "索引";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "索引";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            string path = Application.StartupPath + "\\..\\Help\\基础地理信息数据库管理系统用户手册.chm";
            HelpNavigator navigator = new HelpNavigator();
            navigator = HelpNavigator.Index;//索引枚举
            if (!File.Exists(path))
            {
                MessageBox.Show("帮助文件不存在 ！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Help.ShowHelp(m_frmhook.MainForm, path, navigator);
            }
        

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
