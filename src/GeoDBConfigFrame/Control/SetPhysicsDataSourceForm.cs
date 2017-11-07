using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;
using SysCommon;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;

//*********************************************************************************
//** 文件名：SetPhysicsDataSourceForm.cs
//** CopyRight (c) 武汉吉奥信息工程技术有限公司软件工程中心
//** 创建人：席胜
//** 日  期：20011-03-18
//** 修改人：
//** 日  期：
//** 描  述：
//**
//** 版  本：1.0
//*********************************************************************************
namespace GeoDBConfigFrame
{
    public partial class SetPhysicsDataSourceForm : DevComponents.DotNetBar.Office2007Form
    {
        private enumWSType m_wsType;            //数据库类型

        GetDataTreeInitIndex m_dIndex = new GetDataTreeInitIndex();
        public SetPhysicsDataSourceForm()
        {
            InitializeComponent();
            //初始化加载配置文件中记录的信息
            string strDbType = m_dIndex.GetDbValue("dbType");
            //string strServerPath = m_dIndex.GetDbValue("dbServerPath");
            //txtServer.Text = strServerPath;
            cboDataType.Text = strDbType;

            if (strDbType.Equals("SDE"))
            {
                labelX5.Text = "服务器：";
                btnServer.Visible = false;
                btnServer.Enabled = false;
                //txtService.Enabled = true;
                txtDataBase.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                //txtVersion.Enabled = true;

                m_wsType = enumWSType.SDE;

                txtDataBase.Text = m_dIndex.GetDbValue("dbServerName");
                txtUser.Text = m_dIndex.GetDbValue("dbUser");
                txtPassWord.Text = m_dIndex.GetDbValue("dbPassword");
                txtService.Text = m_dIndex.GetDbValue("dbService");
                txtVersion.Text = m_dIndex.GetDbValue("dbVersion");
            }
            else if (strDbType.Equals("PDB"))
            {
                labelX5.Text = "数据库：";
                btnServer.Visible = true;
                btnServer.Enabled = true;
                txtService.Enabled = false;
                txtUser.Enabled = false;
                txtDataBase.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;

                m_wsType = enumWSType.PDB;
            }
            else if (strDbType.Equals("GDB"))
            {
                labelX5.Text = "数据库：";
                btnServer.Visible = true;
                btnServer.Enabled = true;
                txtService.Enabled = false;
                txtUser.Enabled = false;
                txtDataBase.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;

                m_wsType = enumWSType.GDB;
            }
            
            InitializeComDSname();
            InitializeComboBox();
        }
        //加载数据源名称
        private void InitializeComDSname()
        {
            comboBoxDsName.Items.Clear();
            string mypath = m_dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 数据源名称 from 物理数据源表";
            GeoDataCenterDbFun db=new GeoDataCenterDbFun();
            List<string> list = db.GetDataReaderFromMdb(strCon, strExp);
            for (int i = 0; i < list.Count; i++)
            {
                comboBoxDsName.Items.Add(list[i]);
            }
            if (comboBoxDsName.Items.Count > 0)
            {
                comboBoxDsName.SelectedIndex = 0;
            }

        }
        //加载数据源类型
        private void InitializeComboBox()
        {
            cboDataType.Items.Add("SDE");
            cboDataType.Items.Add("PDB");
            cboDataType.Items.Add("GDB");
            if (cboDataType.Text.Equals("SDE"))
            {
                cboDataType.SelectedIndex = 0;
            }
            else if (cboDataType.Text.Equals("PDB"))
            {
                cboDataType.SelectedIndex = 1;
            }
            else if (cboDataType.Text.Equals("GDB"))
            {
                cboDataType.SelectedIndex = 2;
            }
            else
            {
                cboDataType.SelectedIndex = 0;
            }

        }
        //数据源类型发生变化
        private void cboDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDataType.Text.Equals("SDE"))
            {
                labelX5.Text = "服务器名称:";
                btnServer.Visible = false;
                btnServer.Enabled = false;
                txtServer.Text = "";
                //txtService.Enabled = true;
                txtDataBase.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                //txtVersion.Enabled = true;
                //if (comboBoxDsName.Items.Count > 0)
                //{
                //    comboBoxDsName.SelectedIndex = 0;
                //}
                m_wsType = enumWSType.SDE;
            }
            else if (cboDataType.Text.Equals("PDB"))
            {
                labelX5.Text = "数据库:";
                btnServer.Visible = true;
                btnServer.Enabled = true;
                txtService.Enabled = false;
                txtUser.Enabled = false;
                txtDataBase.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;
                //if (comboBoxDsName.Items.Count > 0)
                //{
                //    comboBoxDsName.SelectedIndex = 0;
                //}
                m_wsType = enumWSType.PDB;
            }
            else if (cboDataType.Text.Equals("GDB"))
            {
                labelX5.Text = "数据库:";
                btnServer.Visible = true;
                btnServer.Enabled = true;
                txtService.Enabled = false;
                txtUser.Enabled = false;
                txtDataBase.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;
                //if (comboBoxDsName.Items.Count > 0)
                //{
                //    comboBoxDsName.SelectedIndex = 0;
                //}
                m_wsType = enumWSType.GDB;
            }
        }

        //打开mdb文件
        private void btnServer_Click(object sender, EventArgs e)
        {
            if (cboDataType.Text.Equals("PDB"))
            {
                //如果目标库体为PDB时这个按钮生效
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "选择Personal Geodatabase数据库";
                OpenFile.Filter = "数据库(*.mdb)|*.mdb";

                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    txtServer.Text = OpenFile.FileName;
                    btnServer.Tooltip = OpenFile.FileName;
                }
            }
            else if (cboDataType.Text.Equals("GDB"))
            {
                FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
                if (FolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!FolderBrowser.SelectedPath.Contains(".gdb"))
                    {
                        MessageBox.Show("请选择File Geodatabase数据库");
                        return;
                    }
                    txtServer.Text = FolderBrowser.SelectedPath;
                    btnServer.Tooltip = FolderBrowser.SelectedPath;
                }
            }
        }

        private void comboBoxDsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            if (comboBoxDsName.Text.Trim() == "")
            {
                
                cboDataType.Text = "";
                txtUser.Text = "";
                txtPassWord.Text = "";
                txtServer.Text =
                txtDataBase.Text = "";
                return;
            }
            else
            {
                string strExp = "select * from 物理数据源表 where 数据源名称='" + comboBoxDsName.Text + "'";
                string mypath = m_dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                if (dt.Rows.Count == 0)
                    return;
                //cboDataType.Text = dt.Rows[0]["数据源类型"].ToString();
                txtUser.Text = dt.Rows[0]["用户"].ToString();
                txtPassWord.Text = dt.Rows[0]["密码"].ToString();
                cboDataType.SelectedItem = dt.Rows[0]["数据源类型"] ;
                if (dt.Rows[0]["数据源类型"].Equals("SDE"))
                {
                   //SDE
                    txtServer.Text = dt.Rows[0]["服务器"].ToString();
                    txtDataBase.Text = dt.Rows[0]["数据库"].ToString();
                   
                }
                else
                {
                    txtServer.Text = dt.Rows[0]["数据库"].ToString();
                    txtDataBase.Text = "";
                }
            }
        }

        //新建数据源
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                if (comboBoxDsName.Text == "")
                {
                    MessageBox.Show("数据源名称不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string mypath = m_dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select count(*) from 物理数据源表 where 数据源名称='" + comboBoxDsName.Text+ "'";
                int count=db.GetCountFromMdb(strCon, strExp);
                if (count > 0)
                {
                    MessageBox.Show("数据源名称已存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (labelX5.Text == "数据库:")
                {
                    strExp = string.Format("insert into 物理数据源表(数据源名称,服务器,数据库,用户,密码,数据源类型) values('{0}','{1}','{2}','{3}','{4}','{5}')",
                    comboBoxDsName.Text, "", txtServer.Text, "", "", cboDataType.Text);
                }
                else
                {
                    strExp = string.Format("insert into 物理数据源表(数据源名称,服务器,数据库,用户,密码,数据源类型) values('{0}','{1}','{2}','{3}','{4}','{5}')",
                        comboBoxDsName.Text, txtServer.Text, txtDataBase.Text, txtUser.Text, txtPassWord.Text, cboDataType.Text);
                }
                db.ExcuteSqlFromMdb(strCon, strExp);
                MessageBox.Show("新建成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InitializeComDSname();
            }
            catch
            {
                MessageBox.Show("新建失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //删除数据源
        private void btnDel_Click(object sender, EventArgs e)
        {
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            try
            {
                if (comboBoxDsName.Text == "")
                {
                    MessageBox.Show("数据源名称不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string mypath = m_dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
               // string strExp = "select count(*) from 物理数据源表 where 数据源名称=‘" + comboBoxDsName.Text + "’ ";
                //int i = db.GetCountFromMdb(strCon, strExp);
                //if (i == 0)
                //{

                //    MessageBox.Show("数据源不存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                string strExp = "delete from 物理数据源表 where  数据源名称='" + comboBoxDsName.Text + "'";
                db.ExcuteSqlFromMdb(strCon, strExp);
                strExp = "delete from 数据编码表 where 数据源名称='" + comboBoxDsName.Text + "'";//added by yjl remove noexist source data
                db.ExcuteSqlFromMdb(strCon, strExp);
                strExp = "delete from 逻辑数据源表 where  数据源名称='" + comboBoxDsName.Text + "'";
                InitializeComDSname();
                MessageBox.Show("删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message+",删除失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        //测试
        private void buttonXTest_Click(object sender, EventArgs e)
        {
            if (cboDataType.Text == "SDE")
            {
                try
                {//Workspace
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                    //PropertySet
                    IPropertySet pPropertySet;
                    pPropertySet = new PropertySetClass();
                    //pPropertySet.SetProperty("Service", comboBoxDsName.Text);
                    pPropertySet.SetProperty("Server", txtServer.Text);
                    pPropertySet.SetProperty("Database", txtDataBase.Text);
                    pPropertySet.SetProperty("Instance", "5151");//"port:" + txtService.Text
                    pPropertySet.SetProperty("user", txtUser.Text);
                    pPropertySet.SetProperty("password", txtPassWord.Text);
                    pPropertySet.SetProperty("version", "sde.DEFAULT");
                    IWorkspace pws = pWorkspaceFactory.Open(pPropertySet, 0);
                    MessageBox.Show("连接成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { MessageBox.Show("连接失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            if(cboDataType.Text == "PDB")
            {
                if(txtServer.Text=="")
                {
                    MessageBox.Show("服务器名为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  return;                
                }

                try
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                   IWorkspace pws= pWorkspaceFactory.OpenFromFile(txtServer.Text, 0);
                   MessageBox.Show("连接成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               catch { MessageBox.Show("连接失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            if (cboDataType.Text == "GDB")
            {
                if (txtServer.Text == "")
                {
                    MessageBox.Show("服务器名为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    IWorkspace pws = pWorkspaceFactory.OpenFromFile(txtServer.Text, 0);
                    MessageBox.Show("连接成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch { MessageBox.Show("连接失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }



        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}