using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace GeoDataChecker
{
    public partial class frmBatchDataCheck : DevComponents.DotNetBar.Office2007Form
    {
        Thread _Thread;
        public Thread MThread
        {
            set
            {
                _Thread = value;
            }
            get
            {
                return _Thread;
            }
        }
        private Dictionary<string, string> pDicConn = new Dictionary<string, string>();
        public Dictionary<string, string> DicConn
        {
            set
            {
               pDicConn = value;
            }
            get
            {
                return pDicConn;
            }
        }

        private SysCommon.DataBase.SysDataBase SysDB;
        private SysCommon.DataBase.SysDataBase SysDataBaseLog;

        public frmBatchDataCheck()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            object[] TagDBType = new object[] { "GDB", "PDB","SDE","SHP" };
            comboBoxOrgType.Items.AddRange(TagDBType);
            comboBoxOrgType.SelectedIndex = 0;
            GeoDataChecker._PrgressBarOut = this.progressBarX1;
            GeoDataChecker._ProgressBarInner = this.progressBarX2;
            GeoDataChecker._CheckForm = (Form)this;
            GeoDataChecker._Label = this.labelX2;
        }

        private void btnOrg_Click(object sender, EventArgs e)
        {
            switch (comboBoxOrgType.Text)
            {
                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            if (!listViewEx.Items.ContainsKey(pFolderBrowser.SelectedPath))
                            {
                                ListViewItem aItem = listViewEx.Items.Add(pFolderBrowser.SelectedPath, pFolderBrowser.SelectedPath, "");
                                aItem.Tag = "GDB";
                                aItem.Checked = true;
                                aItem.ToolTipText = pFolderBrowser.SelectedPath;
                            }
                            else
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                            }
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件!");
                        }
                    }
                    break;

                case "PDB":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.CheckFileExists = true;
                    OpenFile.CheckPathExists = true;
                    OpenFile.Title = "选择PDB数据";
                    OpenFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    OpenFile.Multiselect = true;
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < OpenFile.FileNames.Length; i++)
                        {
                            if (!listViewEx.Items.ContainsKey(OpenFile.FileNames[i]))
                            {
                                ListViewItem aItem = listViewEx.Items.Add(OpenFile.FileNames[i], OpenFile.FileNames[i], "");
                                aItem.Tag = "PDB";
                                aItem.Checked = true;
                                aItem.ToolTipText = OpenFile.FileNames[i];
                            }
                            else
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                            }
                        }
                    }
                    break;
                case "SDE":
                    FrmSDEConnSet pFrmSDEConnSet = new FrmSDEConnSet();
                    if (pFrmSDEConnSet.ShowDialog() == DialogResult.OK)
                    {
                        if (!listViewEx.Items.ContainsKey(pFrmSDEConnSet.SDEParaStr))
                        {
                            ListViewItem aItem = listViewEx.Items.Add(pFrmSDEConnSet.SDEParaStr, pFrmSDEConnSet.SDEParaStr, "");
                            aItem.Tag = "SDE";
                            aItem.Checked = true;
                            aItem.ToolTipText = pFrmSDEConnSet.SDEParaStr;
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                        }
                    }
                    break;
                case "SHP":
                    FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {

                        if (!listViewEx.Items.ContainsKey(folderDialog.SelectedPath))
                            {
                                ListViewItem aItem = listViewEx.Items.Add(folderDialog.SelectedPath, folderDialog.SelectedPath, "");
                                aItem.Tag = "SHP";
                                aItem.Checked = true;
                                aItem.ToolTipText = folderDialog.SelectedPath;
                            }
                            else
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                            }
                    }
                    break;

                default:
                    break;
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = true;
            }
        }

        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = !aItem.Checked;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            listViewEx.Items.Clear();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string logPath = txtLog.Text;
            if (logPath.Trim() == string.Empty)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择日志输出路径!");
                return;
            }
            if (File.Exists(logPath))
            {
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "数据文件已存在!\n" + logPath))
                {
                    File.Delete(logPath);
                }
                else
                {
                    return;
                }
            }

            GeoDataChecker.DoDispose();
            Exception errEx = null;
            //连接配置参数信息
            SysDB = new SysCommon.DataBase.SysDataBase();
            SysDB.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TopologyCheckClass.GeoDataCheckParaPath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out errEx);
            //SysDB.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + GeoDataChecker.GeoCheckParaPath, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out errEx);
            if (SysDB.DbConn == null)
            {
                return;
            }

            if (!File.Exists(GeoDataChecker.DBSchemaPath)) return;

            if (!File.Exists(GeoDataChecker.GeoCheckXmlPath)) return;

            //生成日志信息EXCEL格式
            SysDataBaseLog = new SysCommon.DataBase.SysDataBase();
            SysDataBaseLog.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + logPath + "; Extended Properties=Excel 8.0;", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out errEx);
            if (SysDataBaseLog.DbConn == null)
            {
                return;
            }
            string strCreateTableSQL = @" CREATE TABLE ";
            strCreateTableSQL += @" 错误日志 ";
            strCreateTableSQL += @" ( ";
            strCreateTableSQL += @" 检查功能名 VARCHAR, ";
            strCreateTableSQL += @" 错误类型 VARCHAR, ";
            strCreateTableSQL += @" 错误描述 VARCHAR, ";
            strCreateTableSQL += @" 数据图层1 VARCHAR, ";
            strCreateTableSQL += @" 数据OID1 VARCHAR, ";
            strCreateTableSQL += @" 数据图层2 VARCHAR, ";
            strCreateTableSQL += @" 数据OID2 VARCHAR, ";
            strCreateTableSQL += @" 定位点X VARCHAR , ";
            strCreateTableSQL += @" 定位点Y VARCHAR , ";
            strCreateTableSQL += @" 检查时间 VARCHAR ,";
            strCreateTableSQL += @" 数据文件路径 VARCHAR ";
            strCreateTableSQL += @" ) ";

            SysDataBaseLog.UpdateTable(strCreateTableSQL, out errEx);
            if (errEx != null)
            {
                SysDataBaseLog.CloseDbConnection();
                return;
            }

            # region 库体结构检查

            foreach (ListViewItem aItem in listViewEx.CheckedItems)
            {
                if (!pDicConn.ContainsKey(aItem.Text.Trim()))
                {
                    pDicConn.Add(aItem.Text.Trim(), aItem.Tag.ToString());
                }
                ////获取数据连接信息
                //SysCommon.Gis.SysGisDB sysGisDB = new SysCommon.Gis.SysGisDB();

                //switch (aItem.Tag.ToString())
                //{
                //    case "GDB":
                //        if (!Directory.Exists(aItem.Text)) continue;
                //        DirectoryInfo directoryInfo = new DirectoryInfo(aItem.Text);
                //        sysGisDB.SetWorkspace(aItem.Text, SysCommon.enumWSType.GDB, out errEx);
                //        break;
                //    case "PDB":
                //        if (!File.Exists(aItem.Text)) continue;
                //        FileInfo fileInfo = new FileInfo(aItem.Text);
                //        sysGisDB.SetWorkspace(aItem.Text, SysCommon.enumWSType.PDB, out errEx);
                //        break;
                //    case "SDE":
                //        string pSdeParaStr = aItem.Text;
                //        string[] paraArr = new string[5];
                //        paraArr = pSdeParaStr.Split(new char[] { ';' });
                //        string pServer = paraArr[0];   //服务器
                //        string pInstance = paraArr[1]; //实例名
                //        string pUser = paraArr[2];     //用户
                //        string pPassword = paraArr[3]; //密码
                //        string pVersion = paraArr[4];  //版本
                //        sysGisDB.SetWorkspace(pServer, pInstance, "", pUser, pPassword, pVersion, out errEx);
                //        break;
                //    case "SHP":
                //        if (!Directory.Exists(aItem.Text)) continue;
                //        IWorkspaceFactory pWSFactory = new ShapefileWorkspaceFactory();
                //        sysGisDB.SetWorkspace(aItem.Text, SysCommon.enumWSType.SHP, out errEx);

                //        break;
                //    default:
                //        return;
                //}
                //if (sysGisDB.WorkSpace == null) continue;

                //labelX2.Text = "正在进行：库体结构检查.......";

                ////初始化检查
                //GeoDataChecker.GetInstance().InitialCheckPara(sysGisDB.WorkSpace, SysDB.DbConn, GeoDataChecker.DBSchemaPath, SysDataBaseLog.DbConn, "错误日志", GeoDataChecker.GeoCheckXmlPath);
                ////库体结构检查
                //GeoStructChecker clsGeoStructChecker = new GeoStructChecker();
                //clsGeoStructChecker.OnCreate(GeoDataChecker.DataCheckHook);
                //clsGeoStructChecker.DataErrTreat += new DataErrTreatHandle(GeoDataChecker.GeoDataChecker_DataErrTreat);
                //clsGeoStructChecker.OnDataCheck();

                ////关闭连接
                //sysGisDB.Dispose();
            }
            #endregion

            #region  其他类型批量检查

            //其他类型的检查,使用线程
            _Thread = new Thread(new ThreadStart(delegate { DataCheck(SysDB, SysDataBaseLog); }));
            ////执行批量检查
            this.btnOk.Enabled = false;
            _Thread.Start();
            #endregion

        }

        private void DataCheck(SysCommon.DataBase.SysDataBase sysDataBase, SysCommon.DataBase.SysDataBase sysDataBaseLog)
        {
            Exception errEx = null;
            foreach (KeyValuePair<string, string> aItem in pDicConn)
            {
                //获取数据连接信息
                SysCommon.Gis.SysGisDB sysGisDB = new SysCommon.Gis.SysGisDB();
                switch (aItem.Value)
                {
                    case "GDB":
                        if (!Directory.Exists(aItem.Key)) continue;
                        DirectoryInfo directoryInfo = new DirectoryInfo(aItem.Key);
                        sysGisDB.SetWorkspace(aItem.Key, SysCommon.enumWSType.GDB, out errEx);
                        break;
                    case "PDB":
                        if (!File.Exists(aItem.Key)) continue;
                        FileInfo fileInfo = new FileInfo(aItem.Key);
                        sysGisDB.SetWorkspace(aItem.Key, SysCommon.enumWSType.PDB, out errEx);
                        break;
                    case "SDE":
                        string pSdeParaStr = aItem.Key;
                        string[] paraArr = new string[5];
                        paraArr = pSdeParaStr.Split(new char[] { ';' });
                        string pServer = paraArr[0];   //服务器
                        string pInstance = paraArr[1]; //实例名
                        string pUser = paraArr[2];     //用户
                        string pPassword = paraArr[3]; //密码
                        string pVersion = paraArr[4];  //版本
                        sysGisDB.SetWorkspace(pServer, pInstance, "", pUser, pPassword, pVersion, out errEx);
                        break;
                    case "SHP":
                        if (!Directory.Exists(aItem.Key)) continue;
                        IWorkspaceFactory pWSFactory = new ShapefileWorkspaceFactory();
                        sysGisDB.SetWorkspace(aItem.Key, SysCommon.enumWSType.SHP, out errEx);
                        break;
                    default:
                        return;
                }
                if (sysGisDB.WorkSpace == null) continue;
                //初始化检查
                GeoDataChecker.GetInstance().InitialCheckPara(sysGisDB.WorkSpace, sysDataBase.DbConn, GeoDataChecker.DBSchemaPath, sysDataBaseLog.DbConn, "错误日志", GeoDataChecker.GeoCheckXmlPath);

                GeoBatchCheck geoBatchCheck = new GeoBatchCheck();
                geoBatchCheck.OnCreate(GeoDataChecker.DataCheckHook);
                geoBatchCheck.DataErrTreat += new DataErrTreatHandle(GeoDataChecker.GeoDataChecker_DataErrTreat);
                geoBatchCheck.ProgressShow += new ProgressChangeHandle(GeoDataChecker.GeoDataChecker_ProgressShow);
                geoBatchCheck.OnDataCheck();

                //关闭连接
                sysGisDB.Dispose();
            }
            //sysDataBase.CloseDbConnection();
            //sysDataBaseLog.CloseDbConnection();
            this.Invoke(new ShowError(showErrorForm));

            _Thread.Abort();
            
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.OverwritePrompt = false;
            saveFile.Title = "保存为EXCEL格式";
            saveFile.Filter = "EXCEL格式(*.xls)|*.xls";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                txtLog.Text = saveFile.FileName;
            }
        }

        private void frmBatchDataCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Thread != null)
            {
                if (_Thread.ThreadState == ThreadState.Running)
                {
                    _Thread.Suspend();
                }
                if (_Thread.ThreadState != ThreadState.Stopped)
                {
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "正在进行检查，是否停止？") == true)
                    {
                        _Thread.Resume();
                        _Thread.Abort();
                        _Thread = null;
                        if (SysDB != null)
                        {
                            if (SysDB.DbConn.State != ConnectionState.Closed)
                            {
                                SysDB.DbConn.Close();
                            }
                        }
                        
                        if (SysDataBaseLog != null)
                        {
                            if (SysDataBaseLog.DbConn.State != ConnectionState.Closed)
                            {
                                SysDataBaseLog.DbConn.Close();
                            }
                        }
                        this.Close();
                    }
                    else
                    {
                        _Thread.Resume();
                        e.Cancel = true;
                    }
                }
            }
        }

        private delegate void ShowError();
        private void showErrorForm()
        {
            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "检查完成！");
            this.labelX2.Text = "检查出错误:"+GeoDataChecker.EERrorCount+"条!";
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.buttonX1.Enabled = false;
            this.buttonX2.Enabled = true;
            if (_Thread != null&&_Thread.ThreadState==ThreadState.Running)
            {
                //暂停
                _Thread.Suspend();
            }
            
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.buttonX2.Enabled = false;
            this.buttonX1.Enabled = true;
            if (_Thread != null && _Thread.ThreadState == ThreadState.Suspended)
            {
                //继续
                _Thread.Resume();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_Thread != null)
            {
                if (_Thread.ThreadState == ThreadState.Stopped)
                {
                    Exception errEx = null;
                    # region 库体结构检查

                    foreach (ListViewItem aItem in listViewEx.CheckedItems)
                    {
                        //获取数据连接信息
                        SysCommon.Gis.SysGisDB sysGisDB = new SysCommon.Gis.SysGisDB();

                        switch (aItem.Tag.ToString())
                        {
                            case "GDB":
                                if (!Directory.Exists(aItem.Text)) continue;
                                DirectoryInfo directoryInfo = new DirectoryInfo(aItem.Text);
                                sysGisDB.SetWorkspace(aItem.Text, SysCommon.enumWSType.GDB, out errEx);
                                break;
                            case "PDB":
                                if (!File.Exists(aItem.Text)) continue;
                                FileInfo fileInfo = new FileInfo(aItem.Text);
                                sysGisDB.SetWorkspace(aItem.Text, SysCommon.enumWSType.PDB, out errEx);
                                break;
                            case "SDE":
                                string pSdeParaStr = aItem.Text;
                                string[] paraArr = new string[5];
                                paraArr = pSdeParaStr.Split(new char[] { ';' });
                                string pServer = paraArr[0];   //服务器
                                string pInstance = paraArr[1]; //实例名
                                string pUser = paraArr[2];     //用户
                                string pPassword = paraArr[3]; //密码
                                string pVersion = paraArr[4];  //版本
                                sysGisDB.SetWorkspace(pServer, pInstance, "", pUser, pPassword, pVersion, out errEx);
                                break;
                            case "SHP":
                                if (!Directory.Exists(aItem.Text)) continue;
                                IWorkspaceFactory pWSFactory = new ShapefileWorkspaceFactory();
                                sysGisDB.SetWorkspace(aItem.Text, SysCommon.enumWSType.SHP, out errEx);

                                break;
                            default:
                                return;
                        }
                        if (sysGisDB.WorkSpace == null) continue;

                        labelX2.Text = "正在进行：库体结构检查.......";

                        //初始化检查
                        GeoDataChecker.GetInstance().InitialCheckPara(sysGisDB.WorkSpace, SysDB.DbConn, GeoDataChecker.DBSchemaPath, SysDataBaseLog.DbConn, "错误日志", GeoDataChecker.GeoCheckXmlPath);
                        //库体结构检查
                        GeoStructChecker clsGeoStructChecker = new GeoStructChecker();
                        clsGeoStructChecker.OnCreate(GeoDataChecker.DataCheckHook);
                        clsGeoStructChecker.DataErrTreat += new DataErrTreatHandle(GeoDataChecker.GeoDataChecker_DataErrTreat);
                        clsGeoStructChecker.OnDataCheck();

                        //关闭连接
                        sysGisDB.Dispose();
                    }
                    this.labelX2.Text = "检查完成，总共检查出错误:"+GeoDataChecker.EERrorCount+"条!";

                    //释放excel连接
                    SysDataBaseLog.CloseDbConnection();
                    #endregion

                    timer1.Enabled = false;
                    _Thread = null;
                }
            }
        }
      
    }
}