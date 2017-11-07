using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using SysCommon.Gis;

namespace GeoDatabaseManager
{
    /// <summary>
    /// cyf  用户登录   20110520  add
    /// </summary>
    public partial class NewFormLogin : DevComponents.DotNetBar.Office2007Form
    {
        public NewFormLogin()
        {
            InitializeComponent();
        }

        private void NewFormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string pUser = "";             //用户名
            string pPassWord = "";         //密码
            string pUserType = "";         //用户类型
            if (txtUser.Text == "superadmin" && txtPassword.Text == "11111")
            {
                pUser = txtUser.Text.Trim();
                pPassWord = txtPassword.Text.Trim();
                pUserType = "0";
              
            }
            else if (txtUser.Text == "admin" && txtPassword.Text == "11111")
            {
                pUser = txtUser.Text.Trim();
                pPassWord = txtPassword.Text.Trim();
                pUserType = "1";

            }
            else if (txtUser.Text == "commonuser" && txtPassword.Text == "11111")
            {
              
                pUser = txtUser.Text.Trim();
                pPassWord = txtPassword.Text.Trim();
                pUserType = "2";
              
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "用户或者密码不正确！");
                txtUser.Text = "";
                txtPassword.Text ="";
                return;
            }
          
            //不符合用户类型，则返回
            if (pUserType != "0" && pUserType != "1" && pUserType != "2")
                return;

            //将用户信息保存在xml中
            XmlDocument XmlDoc = new XmlDocument();
            XmlNode ele = null;  //数据节点
            if (File.Exists(Mod.v_AppDBConectXml))
            {
                XmlDoc.Load(Mod.v_AppDBConectXml);
                
            }
           
                ele = XmlDoc.SelectSingleNode(".//用户信息");
                if (ele == null)
                {
                    ele = XmlDoc.CreateElement("用户信息");//
                }
            (ele as XmlElement).SetAttribute("user", txtUser.Text.Trim());
            (ele as XmlElement).SetAttribute("password", txtPassword.Text.Trim());
            (ele as XmlElement).SetAttribute("type", pUserType);
            XmlDoc.DocumentElement.AppendChild(ele);
            try
            {
                XmlDoc.Save(Mod.v_AppDBConectXml);
            } catch(Exception ex)
            {
                ex = new Exception("用户信息保存失败");
                return;
            }
            this.Hide();

            if (File.Exists(Mod.v_ConfigPath))
            {
                SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(Mod.v_ConfigPath, out Mod.Server, out Mod.Instance, out Mod.Database, out Mod.User, out Mod.Password, out Mod.Version, out Mod.dbType);
                CanOpenConnect(vgisDb, Mod.dbType, Mod.Server, Mod.Instance, Mod.Database, Mod.User, Mod.Password, Mod.Version);
                Mod.TempWks = vgisDb.WorkSpace;

                //判断现实库的连接
                //string strCurServer, strCurType, strCurInstance, strCurDatabase, strCurUser, strCurPassword, strCurVersion;
                //SysCommon.Authorize.AuthorizeClass.GetCurWks(vgisDb.WorkSpace, out Mod.CurServer, out Mod.CurInstance, out Mod.CurDatabase, out Mod.CurUser, out Mod.CurPassword, out Mod.CurVersion, out Mod.CurdbType);
                //CanOpenConnect(vgisDb, Mod.CurdbType, Mod.CurServer, Mod.CurInstance, Mod.CurDatabase, Mod.CurUser, Mod.CurPassword, Mod.CurVersion);
                //Mod.CurWks = vgisDb.WorkSpace;
            }

            //进入系统
            frmMain pfrmMain = new frmMain(pUser, pPassWord, pUserType);
            pfrmMain.Show();
        }


        //测试链接信息是否可用
        public static bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
