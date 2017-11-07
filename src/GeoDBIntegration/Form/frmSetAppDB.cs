using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data.OracleClient;

namespace GeoDBIntegration
{
    public partial class frmSetAppDB : DevComponents.DotNetBar.Office2007Form
    {
        private string m_Server;
        private string m_User;
        private string m_Password;

        public string Server
        {
            get { return this.m_Server; }
            set { this.m_Server = value; }
        }
        public string User
        {
            get { return this.m_User; }
            set { this.m_User = value; }
        }
        public string Password
        {
            get { return this.m_Password; }
            set { this.m_Password = value; }
        }

        public frmSetAppDB()
        {
            InitializeComponent();
            this.m_Server = string.Empty;
            this.m_User = string.Empty;
            this.m_Password = string.Empty;
            this.checkBox1.Checked = true;
        }
        public frmSetAppDB(string server, string user, string password)
        {
            InitializeComponent();
            this.txtServer.Text = server;
            this.txtUser.Text = user;
            this.txtPassword.Text = password;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.m_Password = this.txtPassword.Text;
            this.m_Server = this.txtServer.Text;
            this.m_User = this.txtUser.Text;
            //////guozheng 2011-2-14 added
            if (this.checkBox1.Checked)
            {
                ////////选中“创建系统维护库库体”，在测试通过的Oracle连接新中创建系统维护库库体结构（根据模板）
                Exception ex=null;
                ExecuteSQLFile(this.m_Server, this.m_User, this.m_Password, ModuleData.v_SystemFunctionDBSchema, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建系统维护库结构失败,/n原因：" + ex.Message);
                    return;
                }
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        //////////测试Oracle连接
        private void buttonX3_Click(object sender, EventArgs e)
        {
            //////////测试Oracle连接
            Exception ex =null;
            if (!TestOracleConnect(out ex))
            {
                if (ex != null) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接失败,/n原因：" + ex.Message); return; }
                else { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接失败"); return; }
            }
            //this.checkBox1.Enabled = true;
            this.buttonX2.Enabled = true;
           
        }
        private bool TestOracleConnect(out Exception ex)
        {
            ex = null;
            string sConinfo = "Data Source=" + this.txtServer.Text + ";Persist Security Info=True;User ID=" + this.txtUser.Text + ";Password=" + this.txtPassword.Text.Trim() + ";Unicode=True";
            OracleConnection OracleCon = new OracleConnection(sConinfo);
            try
            {
                OracleCon.Open();
                return true;
            }
            catch(Exception eError)
            {
                ex = eError;
                return false;
            }
            finally
            {
                if (OracleCon.State==ConnectionState.Open)
                    OracleCon.Close();               
            }
        }
        
        
        /// <summary>
        /// guozheng 2011-2-14 added  执行一个SQL脚本文件
        /// </summary>
        /// <param name="OracleServer">Oracle服务名</param>
        /// <param name="OracleUser">Oracle用户名</param>
        /// <param name="OraclePass">Oracle用户密码</param>
        /// <param name="SqlFileName">SQL脚本文件</param>
        /// <param name="ex">输出：错误信息</param>
        private void ExecuteSQLFile(string OracleServer, string OracleUser, string OraclePass, string SqlFileName, out Exception ex)
        {
            ex = null;
            if (string.IsNullOrEmpty(OracleServer) || string.IsNullOrEmpty(OracleUser) || string.IsNullOrEmpty(OraclePass))
            {
                ex = new Exception("Oracle连接信息不完整");
                return;
            }
            //////////////读取SQL脚本文件
            if (!File.Exists(SqlFileName))
            {
                ex = new Exception("SQL脚本文件“" + SqlFileName + "”不存在！");
                return;
            }
            try
            {
                //将SQL脚本文件复制到C盘根目录下执行，避免安装路径中存在空格造成的错误
                if (File.Exists(@"C:\SystemFunctionDBConfiguration.sql"))
                    File.Delete(@"C:\SystemFunctionDBConfiguration.sql");
                File.Copy(ModuleData.v_SystemFunctionDBSchema, @"C:\SystemFunctionDBConfiguration.sql");
                System.Diagnostics.Process   Process1=new   System.Diagnostics.Process(); 
                Process1.StartInfo.FileName="sqlplus";
                Process1.StartInfo.Arguments = OracleUser+"/"+OraclePass+"@"+OracleServer+"   @C:\\SystemFunctionDBConfiguration.sql";  
                Process1.StartInfo.WindowStyle   =   System.Diagnostics.ProcessWindowStyle.Hidden; 
                Process1.Start();         //开始

                //while (!Process1.HasExited)                     //等待导出的完成 
                //{

                //}
                Process1.WaitForExit();
                if (Process1.ExitCode != 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "执行系统维护库脚本文件失败");
                    return;
                }
             
            }
            catch(Exception eError)
            {
                ex = eError;
                return;
            }
            finally
            {
                try
                {
                    if (File.Exists(@"C:\SystemFunctionDBConfiguration.sql"))
                        File.Delete(@"C:\SystemFunctionDBConfiguration.sql");
                }
                catch
                {
                }
            }
        }

        private void frmSetAppDB_Load(object sender, EventArgs e)
        {
            this.txtPassword.Text = this.m_Password;
            this.txtServer.Text = this.m_Server;
            this.txtUser.Text = this.m_User;
        }

    }
}