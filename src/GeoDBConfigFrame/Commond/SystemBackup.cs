using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

using GeoDataCenterFunLib;
namespace GeoDBConfigFrame
{
    public class SystemBackup : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public SystemBackup()
        {
            base._Name = "GeoDBConfigFrame.SystemBackup";
            base._Caption = "系统备份";
            base._Tooltip = "系统备份";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "系统备份";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            SaveFileDialog sfd = new SaveFileDialog();
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
                        vProgress.SetProgress("备份SDE数据库");
                        sfd.Filter = "DMP文件|*.dmp";
                        vProgress.SetProgress("备份SDE数据库" + Mod.User+"用户");
                        ModSystemExport.ExeCommand("Exp " + Mod.User + "/" + Mod.Password + "@" + Mod.Database + " file=" + fileName.Substring(0, fileName.IndexOf(".")) + "[" + Mod.User + "].dmp log=" + fileName.Substring(0, fileName.IndexOf(".")) + "[" + Mod.User+"]");
                        System.Threading.Thread.Sleep(20000);
                        vProgress.SetProgress("备份SDE数据库sde用户");
                        ModSystemExport.ExeCommand("Exp sde/sde@" + Mod.Database + " file=" + fileName.Substring(0, fileName.IndexOf(".")) + "_sde.dmp log=" + fileName.Substring(0, fileName.IndexOf("."))+"_sde");
                        System.Threading.Thread.Sleep(20000);
                        break;
                    case "PDB":
                        vProgress.SetProgress("备份Access数据库");
                        sfd.Filter = "Access文件|*.mdb";
                        File.Copy(Mod.Server,fileName);
                        break;
                    case "GDB":
                        vProgress.SetProgress("备份GDB数据库");
                        sfd.Filter = "GDB文件|*.gdb";
                        CopyGdb(Mod.Server, fileName);
                        break;
                    default:
                        break;
                }
                vProgress.Close();
                
            }

        }
        private void CopyGdb(string strSourceGDB, string strTargetGDB)
        {
            DirectoryInfo Dinfo = new DirectoryInfo(strSourceGDB);
            Directory.CreateDirectory(strTargetGDB);
            foreach (FileInfo Finfo in Dinfo.GetFiles("*.*"))
            {
                File.Copy(Finfo.FullName,strTargetGDB+"\\"+Finfo.Name );
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