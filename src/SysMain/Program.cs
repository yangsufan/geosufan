using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS;

namespace GDBM
{
    static class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new GDBM.LicenseInitializer();
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
            //登陆主窗体条件设置
            if (ModuleOperator.CheckLogin())
            {
                ISelectionEnvironmentThreshold threshold = new SelectionEnvironmentClass();
                threshold.WarningThreshold = 2000;
                Application.Run(new frmMain());
            }
            //m_AoInitialize.Shutdown();
            //m_AoInitialize = null;
            m_AOLicenseInitializer.ShutdownApplication();
            m_AOLicenseInitializer = null;
        }
    }
}