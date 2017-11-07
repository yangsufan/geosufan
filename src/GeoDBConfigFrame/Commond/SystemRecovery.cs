using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

using GeoDataCenterFunLib;
namespace GeoDBConfigFrame
{
    public class SystemRecovery : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public SystemRecovery()
        {
            base._Name = "GeoDBConfigFrame.SystemRecovery";
            base._Caption = "系统恢复";
            base._Tooltip = "系统恢复";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "系统恢复";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;

            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Title = "选择保存的路径";
            sfd.Filter = "Dmp文件|*dmp";
            sfd.RestoreDirectory = false;

            if (File.Exists(Mod.v_ConfigPath))
            {
                //工作库
                SysCommon.Gis.SysGisDB vgisDb = new SysCommon.Gis.SysGisDB();
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(Mod.v_ConfigPath, out Mod.Server, out Mod.Instance, out Mod.Database, out Mod.User, out Mod.Password, out Mod.Version, out Mod.dbType);
            }
            switch (Mod.dbType)
            {
                case "SDE":
                    sfd.Filter = "DMP文件|*.dmp";
                    break;
                case "PDB":
                    sfd.Filter = "Access文件|*.mdb";
                    break;
                case "GDB":
                    sfd.Filter = "GDB文件|*.gdb";
                    break;
                default:
                    break;
            }
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
                vProgress.EnableCancel = false;
                vProgress.ShowDescription = true;
                vProgress.FakeProgress = true;
                vProgress.TopMost = true;
                vProgress.ShowProgress();
                string fileName = sfd.FileName;
                switch (Mod.dbType)
                {

                    case "SDE":
                        vProgress.SetProgress("恢复SDE数据库" + Mod.User+"用户");
                        int userlen = fileName.LastIndexOf("]") - fileName.LastIndexOf("[")-1;
                        string username = fileName.Substring(fileName.LastIndexOf("[") + 1, userlen);
                        string truefile = fileName.Substring(0, fileName.LastIndexOf("["));
                        ModSystemExport.ExeCommand("Imp " + Mod.User + "/" + Mod.Password + "@" + Mod.Database + " file=" + fileName + " fromuser=" + username + " touser=" + Mod.User + " log=" + truefile+"_imp");
                        System.Threading.Thread.Sleep(20000);
                        vProgress.SetProgress("恢复SDE数据库sde用户");
                        ModSystemExport.ExeCommand("Imp sde/sde@" + Mod.Database + " fromuser=sde file=" + truefile + "_sde.dmp touser=sde log=" + truefile + "_sde_imp");
                        System.Threading.Thread.Sleep(20000);
                        break;
                    case "PDB":
                        vProgress.SetProgress("恢复Access数据库");
                        File.Delete(Mod.Server);
                        File.Copy( fileName,Mod.Server);
                        break;
                    case "GDB":
                        vProgress.SetProgress("恢复GDB数据库");
                        CopyGdb( fileName,Mod.Server);
                        break;
                    default:
                        break;
                }
                vProgress.Close();
            }



        }
        private void CopyGdb(string strSourceGDB, string strTargetGDB)
        {
            Directory.Delete(strTargetGDB, true);
            DirectoryInfo Dinfo = new DirectoryInfo(strSourceGDB);
            Directory.CreateDirectory(strTargetGDB);
            foreach (FileInfo Finfo in Dinfo.GetFiles("*.*"))
            {
                File.Copy(Finfo.FullName, strTargetGDB + "\\" + Finfo.Name);
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