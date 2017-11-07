using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;


//ygc 2012-12-26逻辑检查设置
namespace GeoDBATool
{
    public partial class FrmLogicCheckSet : DevComponents.DotNetBar.Office2007Form
    {
        public FrmLogicCheckSet()
        {
            InitializeComponent();
        }
        private string CheckDataPatn = "";
        private static string m_LogFilePath = Application.StartupPath + "\\..\\Log\\LogicCheckResult.txt";
        //选择检查数据路径
        private void btnScanDataPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            FolderBrowserDialog ScanGDB = new FolderBrowserDialog();
            openFile.Title = "打开检查数据";
            CheckDataPatn = "";
            if (rdbGDB.Checked == true)
            {
                ScanGDB.Description = "选择GDB路径";
            }
            else  if (rdbShpefile.Checked == true)
            {
                openFile.Filter = "*.shp|*.shp";
            }
            else if (rdbMDB.Checked == true)
            {
                openFile.Filter = "*.mdb|*.mdb";
            }
            else
            {
                MessageBox.Show("请选择检查数据类型！","提示",MessageBoxButtons .OK,MessageBoxIcon.Information);
                return;
            }
            if (rdbGDB.Checked != true)
            {
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    txtCheckDataPath.Text = "";
                    CheckDataPatn = openFile.FileName;
                    txtCheckDataPath.Text = CheckDataPatn;
                }
                else
                { return; }
            }
            else
            {
                if (ScanGDB.ShowDialog() == DialogResult.OK)
                {
                    txtCheckDataPath.Text = "";
                    CheckDataPatn = ScanGDB.SelectedPath;
                    txtCheckDataPath.Text = CheckDataPatn;
                }
                else
                { return; }
            }
            
        }

        private void FrmLogicCheckSet_Load(object sender, EventArgs e)
        {
        }
        //根据文件路径打开文件 ygc 2012-8-29
        private IWorkspace GetWorkspace(string filePath)
        {
            IWorkspace pWorkspace = null;
            string FileType = filePath.Substring(filePath.Length - 4, 4);
            switch (FileType)
            {
                case ".shp":
                    IWorkspaceFactory pShpWorkSpaceFactory = new ShapefileWorkspaceFactory();
                    try
                    {
                        pWorkspace = pShpWorkSpaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(filePath), 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pShpWorkSpaceFactory);
                    }
                    break;
                case ".mdb":
                    IPropertySet pPropertySet = new PropertySetClass();
                    pPropertySet.SetProperty("DATABASE", filePath);
                    IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    try
                    {
                        pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pWorkspaceFactory);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pPropertySet);
                    }
                    break;
                case ".gdb":
                    IWorkspaceFactory pGDBWorkSpace = new FileGDBWorkspaceFactoryClass();
                    try
                    {
                        pWorkspace = pGDBWorkSpace.OpenFromFile(filePath, 0);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "错误");
                        return null;
                    }
                    finally
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pGDBWorkSpace);
                    }
                    break;
            }
            return pWorkspace;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            IWorkspace pWorkspace=null ;
            Exception ex = null;
            if (rdbTempData.Checked == false)
            {
                if (CheckDataPatn == "") return;
                pWorkspace = GetWorkspace(CheckDataPatn);
            
                string LayerName = "";
                if (rdbShpefile.Checked == true)
                {
                    LayerName = CheckDataPatn.Substring(CheckDataPatn.LastIndexOf("\\") + 1, CheckDataPatn.Length - 5 - CheckDataPatn.LastIndexOf("\\"));
                    clsLogicCheck.m_LayerName = LayerName;
                }
                Dictionary<IFeatureClass, Dictionary<string, int>> dicCheckResult = clsLogicCheck.GetLogicCheck(pWorkspace, out ex);
                string Log = clsLogicCheck.m_Log;
                WriteLog(Log);
                FrmLogicCheckResult newfrm = new FrmLogicCheckResult();
                newfrm.m_DicErrorData = dicCheckResult;
                if (Log != "")
                {
                    if (MessageBox.Show("改检查中存在错误，是否查看日志信息！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("notepad.exe", m_LogFilePath);
                    }
                }
                newfrm.Show();
            }
            else
            {
                Dictionary<IFeatureClass, Dictionary<string, int>> dicCheckResult = clsLogicCheck.GetLogicCheck( out ex);
                string Log = clsLogicCheck.m_Log;
                WriteLog(Log);
                FrmLogicCheckResult newfrm = new FrmLogicCheckResult();
                newfrm.m_DicErrorData = dicCheckResult;
                if (Log != "")
                {
                    if (MessageBox.Show("改检查中存在错误，是否查看日志信息！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("notepad.exe", m_LogFilePath);
                    }
                }
                newfrm.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //打开日志文件
        private void btnScanLog_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", m_LogFilePath);
        }
        public static void WriteLog(string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (!File.Exists(m_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(m_LogFilePath);
                pFileStream.Close();
            }
            else
            {
                File.Delete(m_LogFilePath);
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            System.IO.FileStream fs = new System.IO.FileStream(m_LogFilePath, FileMode.Create, FileAccess.Write);
            fs.Close();

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string strread = strLog + "     " + strTime + "      \r\n";
            StreamWriter sw = new StreamWriter(m_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            sw = null;
        }

        private void rdbTempData_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTempData.Checked == true)
            {
                btnScanDataPath.Enabled = false;
            }
            else
            {
                btnScanDataPath.Enabled = true;
            }
        }
    }
}
