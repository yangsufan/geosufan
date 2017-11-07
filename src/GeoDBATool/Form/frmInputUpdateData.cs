using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞 添加
    /// </summary>
    public partial class frmInputUpdateData : DevComponents.DotNetBar.Office2007Form
    {
        Plugin.Application.IAppGISRef m_Hook;//主功能窗体
        private System.Windows.Forms.Timer _timer;
        private clsSubmitByDGML pClsSubmitByDGML;
        public frmInputUpdateData(Plugin.Application.IAppGISRef pHook)
        {
            InitializeComponent();

            m_Hook = pHook;

            //初始化数据库类型
            cmbType.Items.Clear();
            cmbHistoType.Items.Clear();
            comBoxType.Items.Clear();
            //目标数据库
            cmbType.Items.AddRange(new object[] { "GDB", "PDB", "SDE" });
            if (cmbType.Items.Count > 0)
            {
                cmbType.SelectedIndex = 0;
            }
            //历史库
            cmbHistoType.Items.AddRange(new object[] { "GDB", "PDB", "SDE" });
            if (cmbHistoType.Items.Count > 0)
            {
                cmbHistoType.SelectedIndex = 0;
            }
            //FID库 
            comBoxType.Items.AddRange(new object[] { "SQL", "ACCESS", "ORACLE" });
            comBoxType.Text = "ACCESS";

            //初始化库体连接信息
            InitiaDBConn(m_Hook);
        }
        //从XML中读取相关信息在界面上表现出来
        private void InitiaDBConn(Plugin.Application.IAppGISRef pHook)
        {
            XmlNode ProNode = pHook.ProjectTree.SelectedNode.Tag as XmlNode;
            //读取现势库信息，并且在界面上表现出来
            XmlElement userDBElem = ProNode.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
            cmbType.Text = userDBElem.GetAttribute("类型").ToString().Trim();//类型
            txtServer.Text = userDBElem.GetAttribute("服务器").ToString().Trim();
            txtInstance.Text = userDBElem.GetAttribute("服务名").ToString().Trim();
            txtDB.Text = userDBElem.GetAttribute("数据库").ToString().Trim();
            txtUser.Text = userDBElem.GetAttribute("用户").ToString().Trim();
            txtPassword.Text = userDBElem.GetAttribute("密码").ToString().Trim();
            txtVersion.Text = userDBElem.GetAttribute("版本").ToString().Trim();
            //读取历史库信息并且在界面上表现出来
            XmlElement historyDBElem = ProNode.SelectSingleNode(".//内容//历史库//连接信息") as XmlElement;
            cmbHistoType.Text = historyDBElem.GetAttribute("类型").ToString().Trim();
            txtHistoServer.Text = historyDBElem.GetAttribute("服务器").ToString().Trim();
            txtHistoInstance.Text = historyDBElem.GetAttribute("服务名").ToString().Trim();
            txtHistoDB.Text = historyDBElem.GetAttribute("数据库").ToString().Trim();
            txtHistoUser.Text = historyDBElem.GetAttribute("用户").ToString().Trim();
            txtHistoPassword.Text = historyDBElem.GetAttribute("密码").ToString().Trim();
            txtHistoVersion.Text = historyDBElem.GetAttribute("版本").ToString().Trim();
            //读取FID记录表信息并且在界面上表现出来
            XmlElement FIDDBElem = ProNode.SelectSingleNode(".//内容//FID记录表//连接信息") as XmlElement;
            comBoxType.Text = FIDDBElem.GetAttribute("类型").ToString().Trim();
            textBoxXServer.Text = FIDDBElem.GetAttribute("服务名").ToString().Trim();
            textBoxXDB.Text = FIDDBElem.GetAttribute("数据库").ToString().Trim();
            textBoxXUser.Text = FIDDBElem.GetAttribute("用户").ToString().Trim();
            textBoxXPassword.Text = FIDDBElem.GetAttribute("密码").ToString().Trim();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text  = "";
            txtInstance.Text  = "";
            txtDB.Text="";
            txtUser.Text="";
            txtPassword.Text="";
            txtVersion.Text="SDE.DEFAULT";
            
            if (cmbType.Text == "PDB")
            {
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtDB.Enabled = true;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtVersion.Enabled = false;

                btnDB.Visible = true;
                btnDB.Enabled = true;
            }
            else if (cmbType.Text  == "GDB")
            {
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtDB.Enabled = true;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtVersion.Enabled = false;

                btnDB.Visible = true;
                btnDB.Enabled = true;
            }
            else if (cmbType.Text  == "SDE")
            {
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtDB.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtVersion.Enabled = true;

                btnDB.Visible = false;
                btnDB.Enabled = false;
            }
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxXServer.Text = "";
            textBoxXDB.Text = "";
            textBoxXUser.Text = "";
            textBoxXServer.Text = "";

            if (comBoxType.Text == "ACCESS")
            {
                textBoxXServer.Enabled=false;
                textBoxXDB.Enabled = true;
                textBoxXUser.Enabled = false;
                textBoxXPassword.Enabled = false;

                buttonXFile.Visible = true;
                buttonXFile.Enabled = true;
                textBoxXDB.Width = textBoxXUser.Width - buttonXFile.Width;
            }
            else if (comBoxType.Text == "SQL")
            {
                textBoxXServer.Enabled = true;
                textBoxXDB.Enabled = true;
                textBoxXUser.Enabled = true;
                textBoxXPassword.Enabled = true;

                buttonXFile.Visible = false;
                buttonXFile.Enabled = false;
                textBoxXDB.Width = textBoxXUser.Width;
            }
            else if (comBoxType.Text == "ORACLE")
            {
                textBoxXServer.Enabled =false;
                textBoxXDB.Enabled = true;
                textBoxXUser.Enabled = true;
                textBoxXPassword.Enabled = true;

                buttonXFile.Visible = false ;
                buttonXFile.Enabled = false ;
                textBoxXDB.Width = textBoxXUser.Width;
            }
        }

        private void btnDGML_Click(object sender, EventArgs e)
        {
            lVFileNames.Items.Clear();
            OpenFileDialog Ofd = new OpenFileDialog();
            Ofd.Title = "选择文件";
            Ofd.Filter = "文件(*.xml)|*.xml";
            Ofd.Multiselect = true;
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                if (Ofd.FileNames != null && Ofd.FileNames.Length > 0)
                {
                    ListViewItem[] lvItem = new ListViewItem[Ofd.FileNames.Length];
                    for (int i = 0; i < Ofd.FileNames.Length; i++)
                    {
                        lvItem[i] = new ListViewItem();
                        lvItem[i].Text = Ofd.FileNames[i];
                        lvItem[i].ToolTipText = Ofd.FileNames[i];
                    }
                    lVFileNames.Items.AddRange(lvItem);

                    //从XML文件中读取信息来初始化连接信息
                    //XmlDocument DGMLXMl = new XmlDocument();
                    //DGMLXMl.Load(Ofd.FileNames[0]);
                    ////目标库连接节点
                    //XmlElement DBNode = DGMLXMl.SelectSingleNode(".//DGML//MetaInfo//ProjectInfo//OutDataBase") as XmlElement;
                    //cmbType.Text = DBNode.GetAttribute("DBType").Trim();//类型
                    //txtServer.Text = DBNode.GetAttribute("Server").Trim();//服务名
                    //txtInstance.Text = DBNode.GetAttribute("ServiceName").Trim();//实例名
                    //txtDB.Text = DBNode.GetAttribute("DataBase").Trim();//数据库
                    //txtUser.Text = DBNode.GetAttribute("User").Trim();//用户
                    //txtPassword.Text = DBNode.GetAttribute("Password").Trim();//密码
                    //txtVersion.Text = DBNode.GetAttribute("Version").Trim();//版本
                }
            }
        }

        private void btnDB_Click(object sender, EventArgs e)
        {
            if (cmbType.Text == "PDB")
            {
                OpenFileDialog Ofd = new OpenFileDialog();
                Ofd.Title = "选择文件";
                Ofd.Filter = "文件(*.mdb)|*.mdb";
                Ofd.Multiselect = false;
                if (Ofd.ShowDialog() == DialogResult.OK)
                {
                    txtDB.Text = Ofd.FileName;
                }
            }
            if (cmbType.Text == "GDB")
            {
                FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
                if (FolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!FolderBrowser.SelectedPath.Contains(".gdb"))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB型的数据库！");
                        return;
                    }
                    txtDB.Text = FolderBrowser.SelectedPath;
                }
            }
        }

        private void buttonXFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog Ofd = new OpenFileDialog();
            Ofd.Title = "选择文件";
            Ofd.Filter = "文件(*.mdb)|*.mdb";
            Ofd.Multiselect = false;
            if (Ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxXDB.Text = Ofd.FileName;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
            //DGML文档集合
           string[] fileNames = new string[lVFileNames.Items.Count];
            for (int i = 0; i < lVFileNames.Items.Count; i++)
            {
               
                fileNames[i] = lVFileNames.Items[i].Text;
            }
            //目标库体连接
            //if (cmbType.Text.Trim() == "SDE")
            //{
            //    pSysDT.SetWorkspace(txtServer.Text.Trim(), txtInstance.Text.Trim(), txtDB.Text.Trim(), txtUser.Text.Trim(), txtPassword.Text.Trim(), txtVersion.Text.Trim(), out eError);
            //}
            //else if (cmbType.Text.Trim() == "PDB")
            //{
            //    pSysDT.SetWorkspace(txtDB.Text.Trim(), SysCommon.enumWSType.PDB, out eError);
            //}
            //else if (cmbType.Text.Trim() == "GDB")
            //{
            //    pSysDT.SetWorkspace(txtDB.Text.Trim(), SysCommon.enumWSType.GDB, out eError);
            //}
            //if (eError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标数据库连接出错！");
            //    return;
            //}

            //FID库连接
            string connectionStr = "";
            SysCommon.enumDBConType dbConType=new SysCommon.enumDBConType();
            SysCommon.enumDBType dbType=new SysCommon.enumDBType();
            if (comBoxType.Text.Trim()== "ACCESS")
            {
                connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxXDB.Text.Trim() + ";Mode=Share Deny None;Persist Security Info=False";
                dbConType = SysCommon.enumDBConType.OLEDB;
                dbType = SysCommon.enumDBType.ACCESS;
                //pTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            }
            else if (comBoxType.Text.Trim() == "ORACLE")
            {
                //connectionStr = "data source=" + pServe + ";User Id=" + pUser + ";Password=" + pPassword+";";
                connectionStr = "Data Source=" + textBoxXDB.Text.Trim() + ";Persist Security Info=True;User ID=" + textBoxXUser.Text.Trim() + ";Password=" + textBoxXPassword.Text.Trim() + ";Unicode=True";
                dbConType = SysCommon.enumDBConType.ORACLE;
                dbType = SysCommon.enumDBType.ORACLE;
                //pTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
            }
            else if (comBoxType.Text.Trim() == "SQL")
            {
                connectionStr = "Data Source=" + textBoxXDB.Text.Trim() + ";Initial Catalog=" + textBoxXServer.Text.Trim() + ";User ID=" + textBoxXUser.Text.Trim() + ";Password=" + textBoxXPassword.Text.Trim();
                dbConType = SysCommon.enumDBConType.SQL;
                dbType = SysCommon.enumDBType.SQLSERVER;
                //pTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.SQL, SysCommon.enumDBType.SQLSERVER, out eError);
            }

            if (fileNames == null || fileNames.Length == 0) return;

            if (dbConType.GetHashCode().ToString() == "" || dbType.GetHashCode().ToString() == "" || connectionStr == "") return;
            //SysCommon.DataBase.SysTable pTable = frmSubmitData.v_Table;//获取FID库体连接
            //SysCommon.Gis.SysGisDataSet pSysDT = frmSubmitData.v_SysDT;//获取目标数据库连接
            Plugin.Application.IAppFormRef pAppForm = m_Hook as Plugin.Application.IAppFormRef;

            //进行数据提交
            pAppForm.OperatorTips = "根据DGML进行数据提交...";
            pClsSubmitByDGML = new clsSubmitByDGML(fileNames, connectionStr, dbConType, dbType, cmbType.Text.Trim(), txtServer.Text.Trim(), txtInstance.Text.Trim(), txtDB.Text.Trim(), txtUser.Text.Trim(), txtPassword.Text.Trim(), txtVersion.Text.Trim(),cmbHistoType.Text.Trim(),txtHistoServer.Text.Trim(),txtHistoInstance.Text.Trim(),txtHistoDB.Text.Trim(),txtHistoUser.Text.Trim(),txtHistoPassword.Text.Trim(),txtHistoVersion.Text.Trim(), m_Hook);
            //pClsSubmitByDGML.SubmitThread();
            Thread pThread = new Thread(new ThreadStart(pClsSubmitByDGML.SubmitThread));
            pClsSubmitByDGML.CurrentThread = pThread;
            m_Hook.CurrentThread = pThread;
            pThread.Start();


            //利用计时器刷新mapcontrol
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 800;
            _timer.Enabled = true;
            _timer.Tick += new EventHandler(Timer_Tick);

        }

        //利用计时器刷新mapcontrol
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pClsSubmitByDGML.CurrentThread.ThreadState == ThreadState.Stopped)
            {
                if (pClsSubmitByDGML.Res == true)
                {
                    #region 将界面上的信息写入XML里面保存起来
                    //XmlDocument ProXMlDoc = new XmlDocument();
                    //ProXMlDoc.Load(ModData.v_projectXML);
                    XmlNode ProjectNode = m_Hook.ProjectTree.SelectedNode.Tag as XmlNode;//ProXMlDoc.SelectSingleNode(".//工程[@名称='" + m_Hook.ProjectTree.SelectedNode.Name + "']");
                    //将界面上的现势库信息写入到XML中
                    XmlElement userDBElem = ProjectNode.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
                    userDBElem.SetAttribute("类型", cmbType.Text.ToString().Trim());
                    userDBElem.SetAttribute("服务器", txtServer.Text.Trim());
                    userDBElem.SetAttribute("服务名", txtInstance.Text.ToString().Trim());
                    userDBElem.SetAttribute("数据库", txtDB.Text.Trim());
                    userDBElem.SetAttribute("用户", txtUser.Text.Trim());
                    userDBElem.SetAttribute("密码", txtPassword.Text.Trim());
                    userDBElem.SetAttribute("版本", txtVersion.Text.Trim());
                    //将界面上的历史库信息写入到XML中
                    XmlElement historyDBElem = ProjectNode.SelectSingleNode(".//内容//历史库//连接信息") as XmlElement;
                    historyDBElem.SetAttribute("类型", cmbHistoType.Text.ToString().Trim());
                    historyDBElem.SetAttribute("服务器", txtHistoServer.Text.Trim());
                    historyDBElem.SetAttribute("服务名", txtHistoInstance.Text.ToString().Trim());
                    historyDBElem.SetAttribute("数据库", txtHistoDB.Text.Trim());
                    historyDBElem.SetAttribute("用户", txtHistoUser.Text.Trim());
                    historyDBElem.SetAttribute("密码", txtHistoPassword.Text.Trim());
                    historyDBElem.SetAttribute("版本", txtHistoVersion.Text.Trim());
                    //将界面上的FID记录表库体信息写入到XML中
                    XmlElement FIDDBElem = ProjectNode.SelectSingleNode(".//内容//FID记录表//连接信息") as XmlElement;
                    FIDDBElem.SetAttribute("类型", comBoxType.Text.ToString().Trim());
                    FIDDBElem.SetAttribute("服务名", textBoxXServer.Text.ToString().Trim());
                    FIDDBElem.SetAttribute("数据库", textBoxXDB.Text.Trim());
                    FIDDBElem.SetAttribute("用户", textBoxXUser.Text.Trim());
                    FIDDBElem.SetAttribute("密码", textBoxXPassword.Text.Trim());

                    ProjectNode.OwnerDocument.Save(ModData.v_projectXML);
                    #endregion
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                    this.Show();
                _timer.Enabled = false;
            }
        }


        private void cmbHistoType_SelectedIndexChanged(object sender, EventArgs e)
        {
           txtHistoServer.Text="";
           txtHistoInstance.Text = "";
           txtHistoDB .Text = "";
           txtHistoUser .Text = "";
           txtHistoPassword .Text = "";
           txtHistoVersion.Text = "SDE.DEFAULT";

           if (cmbHistoType.Text == "PDB" || cmbHistoType.Text == "GDB")
            {
                txtHistoServer.Enabled = false;
                txtHistoInstance.Enabled = false;
                txtHistoDB.Enabled = true;
                txtHistoUser.Enabled = false;
                txtHistoPassword.Enabled = false;
                txtHistoVersion.Enabled = false;

               btnHistoDB .Visible = true;
               btnHistoDB.Enabled = true;
            }
            else if (cmbHistoType.Text == "SDE")
            {
                txtHistoServer.Enabled = true;
                txtHistoInstance.Enabled = true;
                txtHistoDB.Enabled = true;
                txtHistoUser.Enabled = true;
                txtHistoPassword.Enabled = true;
                txtHistoVersion.Enabled = true;

                btnHistoDB.Visible = false;
                btnHistoDB.Enabled = false;
            }
        }

        private void btnHistoDB_Click(object sender, EventArgs e)
        {
            if (cmbHistoType.Text == "PDB")
            {
                OpenFileDialog Ofd = new OpenFileDialog();
                Ofd.Title = "选择文件";
                Ofd.Filter = "文件(*.mdb)|*.mdb";
                Ofd.Multiselect = false;
                if (Ofd.ShowDialog() == DialogResult.OK)
                {
                   txtHistoDB.Text = Ofd.FileName;
                }
            }
            if (cmbHistoType.Text == "GDB")
            {
                FolderBrowserDialog FolderBrowser = new FolderBrowserDialog();
                if (FolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!FolderBrowser.SelectedPath.Contains(".gdb"))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB型的数据库！");
                        return;
                    }
                    txtHistoDB.Text = FolderBrowser.SelectedPath;
                }
            }
        }
    }
}