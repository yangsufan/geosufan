using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using System.Diagnostics;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS;

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
            //ESRI License Initializer generated code.
            if (!RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("没有安装ArcGIS","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            //扩展许可判断
            //IAoInitialize m_AoInitialize = new AoInitialize();
            //esriLicenseStatus status=m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            //m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (GetProcessSysmain())
            {
                MessageBox.Show("该程序已启动！请查看程序是否正在运行，或请在进程中关闭。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                //登陆主窗体条件设置
                if (ModuleOperator.CheckLogin())
                {
                    ISelectionEnvironmentThreshold threshold = new SelectionEnvironmentClass();
                    threshold.WarningThreshold = 2000;
                    Application.Run(new frmMain());
                }
            }
            //m_AoInitialize.Shutdown();
            //m_AoInitialize = null;
            m_AOLicenseInitializer.ShutdownApplication();
            m_AOLicenseInitializer = null;
        }
        /// <summary>
        /// 判断当前系统是否已运行
        /// </summary>
        /// <returns></returns>
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