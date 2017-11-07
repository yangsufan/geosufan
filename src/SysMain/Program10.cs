using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using System.Diagnostics;
using ESRI.ArcGIS.Carto;

using System.IO;

using Microsoft.Win32;

namespace GeoDatabaseManager
{
    static class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new GeoDatabaseManager.LicenseInitializer();
        /// <summary>
        /// 应用程序的主入口点。


        /// </summary>
        [STAThread]
        static void Main()
        {
            //cyf 20110612 modify:将许可授权文件由ArcEngine改为ArcINfo
            //ESRI License Initializer generated code.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);



            IAoInitialize m_AoInitialize = new AoInitialize();
            m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);//esriLicenseProductCodeArcInfo);//.esriLicenseProductCodeEngineGeoDB);
            m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);//esriLicenseProductCodeArcInfo);//.esriLicenseProductCodeEngineGeoDB);
            //end
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //added by chulili 20110714检测有没有安装arcengine
            RegistryKey LocalMachineRegKey = Registry.LocalMachine;

            RegistryKey InstallDirRegKey = LocalMachineRegKey.OpenSubKey(@"SOFTWARE\ESRI\CoreRuntime");
            if (InstallDirRegKey == null)
            {
                InstallDirRegKey = LocalMachineRegKey.OpenSubKey(@"SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            }
            if (InstallDirRegKey == null)
            {
                InstallDirRegKey = LocalMachineRegKey.OpenSubKey(@"SOFTWARE\\ESRI\\Desktop10.0\\CoreRuntime");
            }

            if (InstallDirRegKey == null)
            {

                MessageBox.Show("没有安装ArcGIS Runtime或ArcGIS Desktop！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }

            //end add
            ///软件狗的检测
            //GeoOneKey key = new GeoOneKey();

            //if (key.addkey() != -1)
            //{
                bool b = GetProcessSysmain();
                if (b == true)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "该程序已启动！");
                    MessageBox.Show("该程序已启动！请查看程序是否正在运行，或请在进程中关闭。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                else
                {
                    //deleted by chulili 20110531 删除psc的数据库连接窗体，仅适用南京的数据库连接窗体
                    //**********************************************************************************************
                    //guozhegng 2011-2-25 系统维护库的连接正确性判断以及库体结构完整性判断//
                    //#region 系统维护库的连接正确性判断以及库体结构完整性判断
                    //Exception ex = null;
                    //clsAddAppDBConnection addAppDB = new clsAddAppDBConnection();
                    //string sConnect = addAppDB.GetAppDBConInfo(out ex);
                    //if (string.IsNullOrEmpty(sConnect))
                    //{
                    //    sConnect = addAppDB.SetAppDBConInfo(out ex);
                    //}
                    //if (!string.IsNullOrEmpty(sConnect))
                    //{
                    //    addAppDB.JudgeAppDbConfiguration(sConnect, out ex);
                    //    if (ex != null)
                    //    {
                    //        //if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "系统维护库库体结构错误：" + ex.Message + ",\n是否重新配置系统维护库连接信息？"))
                    //        if (MessageBox.Show("系统维护库库体结构错误：" + ex.Message + ",\n是否重新配置系统维护库连接信息？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    //        {
                    //            sConnect = addAppDB.SetAppDBConInfo(out ex);
                    //        }
                    //        else
                    //        {
                    //            Application.Exit();
                    //            return;
                    //        }

                    //    }
                    //}
                    //else
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接信息失败");
                    //    Application.Exit();
                    //    return;
                    //}
                    //#endregion
                    //end  deleted by chulili 20110531  

                    //登陆主窗体条件设置
                    if (ModuleOperator.CheckLogin())
                    {
                        ISelectionEnvironmentThreshold threshold = new SelectionEnvironmentClass();
                        threshold.WarningThreshold = 2000;
                        Application.Run(new frmMain());
                    }
                }

            m_AoInitialize.Shutdown();
            m_AoInitialize = null;
            m_AOLicenseInitializer.ShutdownApplication();
            m_AOLicenseInitializer = null;
        }

        private static  bool GetProcessSysmain()
        {
            bool state = false;
            Process[] pro = System.Diagnostics.Process.GetProcessesByName("SysMain");
            if (pro.Length > 1)
            {
                state = true;
            }
            return state;
        }
    }
}