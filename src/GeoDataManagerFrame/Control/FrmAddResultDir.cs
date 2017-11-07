using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
using SysCommon.Error;

using System.Diagnostics;
using  System.IO;
namespace GeoDataManagerFrame
{
    public partial class FrmAddResultDir : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_Workspace;
        public FrmAddResultDir(IWorkspace Workspace)
        {
            InitializeComponent();
            m_Workspace = Workspace;

   

            if (m_Workspace != null)
            {
                SysGisTable sysTable = new SysGisTable(m_Workspace);

                ///获得对应成果数据上传的目标路径
                //  string strUploading = null;
                //   int iCount = 0;
                Exception eError = null;
                if (sysTable.ExistData("RESULTDIR", null))
                {
                    List<object> ListDatasource = sysTable.GetFieldValues("RESULTDIR", "COMPUTERIP", "", out eError);
                    foreach (object datasource in ListDatasource)
                    {
                        this.comboBoxExIP.Items.Add(datasource.ToString());
                    }

                }

            }
        }

        //写入记录
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            //获取对话框中的内容
            string strComputerIp = comboBoxExIP.Text;

            //获取文件夹名称
            string strFileName = textBox_File.Text;
            //获取登录电脑名和密码
            string strUserName = txtUserName.Text;
            string strPass = txtUserPass.Text;

            Exception eError = null;

            //记录到对应的数据库中,插入记录
            if (m_Workspace != null)
            {
                //判断名称是否存在
                SysGisTable sysTable = new SysGisTable(m_Workspace);
                string strSearch = "PATHNAME='" + strFileName + "'";
                if (sysTable.ExistData("RESULTDIR", strSearch))
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "已存在相同的文件名,请输入其他名称！");
                    return;
                }
                //if (strUserName == "")
                //{
                //    MessageBox.Show("请输入登录目标电脑的用户名！","提示");
                //    return;
                //}
                if (!Connect(strComputerIp, strUserName, strPass))
                {
                    MessageBox.Show("不能连接目标电脑成果目录，请核对登录目标电脑用户名和登录密码后再式！","提示");
                    return;
                }
                //判断目标电脑上是否存在该文件夹 ,不存在就创建
                //本机创建 复制到目标电脑上
                if (Ping(strComputerIp))
                {

                    //新建本机上
                    string FilePath = Application.StartupPath + "\\" + strFileName;
                    string strFromFilepath = @"\\" + strComputerIp + "\\SG-RESULT\\";

                    FilePath = strFromFilepath + strFileName;
                    try
                    {
                        Directory.CreateDirectory(FilePath);
                    }
                    catch(Exception  err)
                    {
                        MessageBox.Show(err.Message);
                        return;
                    }

                    Dictionary<string, object> dicProjectGroup = new Dictionary<string, object>();
                    dicProjectGroup.Add("COMPUTERIP", strComputerIp);
                    dicProjectGroup.Add("DATADIR", FilePath);
                    dicProjectGroup.Add("USER_", strUserName);
                    dicProjectGroup.Add("PASSWORD_", strPass);
                    dicProjectGroup.Add("PATHNAME", strFileName);
                    sysTable.NewRow("RESULTDIR", dicProjectGroup, out eError);
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void buttonXQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region
        public static bool Ping(string remoteHost)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                //string dosLine = @"ping -n 1 " + remoteHost;
                string dosLine = @"ping -n 1 " + remoteHost;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(500);
                }
                string pingResult = proc.StandardOutput.ReadToEnd();
                if (pingResult.IndexOf("(0% loss)") != -1 || pingResult.IndexOf("(0% 丢失)") != -1)
                {
                    Flag = true;
                }
                proc.StandardOutput.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    proc.Close();
                    proc.Dispose();
                }
                catch
                {
                }
            }
            return Flag;
        }

        public static bool Connect(string remoteHost, string userName, string passWord)
        {
            if (!Ping(remoteHost))
            {
                return false;
            }
            bool Flag = true;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe"; //设定程序名
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true; //重定向标准输入
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true; //重定向错误输出
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "";
                if (userName != "")
                {
                    dosLine = @"net use \\" + remoteHost + " " + passWord + " " + " /user:" + userName + ">NUL";
                }
                else
                {
                    dosLine = @"net use \\" + remoteHost + " >NUL";
                }
                proc.StandardInput.WriteLine(dosLine);  //执行的命令

                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(100);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                if (errormsg != "")
                {
                    Flag = false;
                }
                proc.StandardError.Close();
            }
            catch (Exception ex)
            {
                Flag = false;
            }
            finally
            {
                try
                {
                    proc.Close();
                    proc.Dispose();
                }
                catch
                {
                }
            }
            return Flag;
        }
        #endregion

    }

}
