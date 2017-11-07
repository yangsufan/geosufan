using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using SysCommon;
namespace GeoSysSetting
{
   public class CommandExit:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFormRef _hook;

        public CommandExit()
        {
            base._Name = "GeoSysSetting.CommandExit";
            base._Caption = "退出系统";
            base._Tooltip = "退出系统";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "退出系统";
        }
        public override void OnClick()
        {
            //if (MessageBox.Show("是否确定要退出系统！", "退出确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //{
            //    Application.ExitThread();
            //    Application.Exit();
            //    Process[] pro = System.Diagnostics.Process.GetProcessesByName("SysMain");
            //    foreach (Process pc in pro)
            //    {
            //        pc.Kill();
            //    }

            //}
            FrmExit pFrm = new FrmExit();
            DialogResult pRes= pFrm.ShowDialog();
            pFrm = null;
            if (pRes == DialogResult.Yes)
            {
                Application.ExitThread();
                Application.Exit();
                Process[] pro = System.Diagnostics.Process.GetProcessesByName("GeoDatabaseManager");
                foreach (Process pc in pro)
                {
                    pc.Kill();
                }
            }
            else if (pRes == DialogResult.No)
            {
                string exepath = Application.StartupPath;
                string strExecutablePath = Application.ExecutablePath;
                Application.ExitThread();
                Application.Exit();
                //Process[] pro = System.Diagnostics.Process.GetProcessesByName("GeoDatabaseManager");
                //foreach (Process pc in pro)
                //{
                //    pc.Kill();
                //}

                string picPath;
                Process p = new Process(); picPath = string.Concat(System.IO.Path.GetDirectoryName(strExecutablePath), "\\GeoDatabaseManager.exe");
                System.Diagnostics.Process.Start(picPath);
            }
            else
            {
 
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _hook = hook as Plugin.Application.IAppFormRef;


        }
    }
}
