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
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            //扩展许可判断
            //IAoInitialize m_AoInitialize = new AoInitialize();
            //esriLicenseStatus status=m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            //m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //check the config file 
            frmDBSet setDbFrom=null;
            if (!System.IO.File.Exists(Fan.Common.ModuleConfig.m_ConnectFileName))
            {
                setDbFrom = new frmDBSet();
                if (setDbFrom.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    Mod.m_SysDbOperate = setDbFrom.Dbop;
                }
            }
            else
            {
                //Read The Connect Info from the config file
                Fan.DataBase.DBConfig dbConfig = new Fan.DataBase.DBConfig();
                dbConfig.ReadConfigFromFile(Fan.Common.ModuleConfig.m_ConnectFileName);
                Fan.DataBase.DBOperatorFactory dbFac = new Fan.DataBase.DBOperatorFactory(dbConfig);
                Mod.m_SysDbOperate=dbFac.GetDbOperate();
                if (Mod.m_SysDbOperate == null||!Mod.m_SysDbOperate.TestConnect())
                {
                    setDbFrom = new frmDBSet();
                    if (setDbFrom.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        Mod.m_SysDbOperate = setDbFrom.Dbop;
                    }
                }
            }
            if (setDbFrom != null) setDbFrom.Dispose();
            frmLogin LoginFrom = new frmLogin();
            //登陆主窗体条件设置
            if (LoginFrom.ShowDialog() == DialogResult.OK)
            {
                LoginFrom.Dispose();
                ISelectionEnvironmentThreshold threshold = new SelectionEnvironmentClass();
                threshold.WarningThreshold = 2000;
                Application.Run(new frmMain());
            }
            else
            {
                return;
            }
            //m_AoInitialize.Shutdown();
            //m_AoInitialize = null;
            m_AOLicenseInitializer.ShutdownApplication();
            m_AOLicenseInitializer = null;
        }
    }
}