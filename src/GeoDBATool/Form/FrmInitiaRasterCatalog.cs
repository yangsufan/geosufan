using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.esriSystem;
using System.Xml;

namespace GeoDBATool
{
    //cyf 20110615 add:根据不同方式来初始化RasteCatalog
    public partial class FrmInitiaRasterCatalog : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_WS = null;  //系统维护库工作空间
        private Plugin.Application.IAppGISRef m_Hook;        //主功能应用app  
        Plugin.Application.IAppFormRef m_AppForm;            //主窗体  
        EnumRasterOperateType m_Operate = EnumRasterOperateType.Input;   //cyf 20110620 栅格数据的操作类型：入库、更新
        public string m_pRootPathfst = "";
        //构造
        public FrmInitiaRasterCatalog(IWorkspace pWs, Plugin.Application.IAppGISRef pHook, EnumRasterOperateType pOperate)
        {
            InitializeComponent();
            m_Hook = pHook;
            if (m_Hook == null) return;
            m_WS = pWs;

            //初始化界面
            m_AppForm = pHook as Plugin.Application.IAppFormRef;
            if (m_AppForm == null) return;
            m_Operate = pOperate;

            #region 查询系统维护库，获得是否存在指定目录
            //cyf 20110626 modify
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode;     //当前树节点，栅格数据节点
            DevComponents.AdvTree.Node pDBProNode = m_Hook.ProjectTree.SelectedNode;   //获得工程树图节点
            while (pDBProNode.Parent != null)
            {
                pDBProNode = pDBProNode.Parent;
            }
            if (pDBProNode.DataKeyString != "project")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取工程树图节点失败！");
                return;
            }
            //获得栅格数据库类型
            XmlElement pCurElem = null;  //图层xml节点
            try { pCurElem = pCurNode.Tag as XmlElement; }
            catch { }
            if (pCurElem == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取图层xml节点失败！");
                return;
            }
            string pDesRasterCollName = pCurElem.GetAttribute("名称").Trim();   //栅格数据名称
            //end
            //end
            //cyf  20110609 modify: 20110626 modify :修改工程树图节点; 
            Exception eError = null;
            string pWhereStr = "ProjectID=" + pDBProNode.Name + " and RasterLayerName='" + pDesRasterCollName + "'";
            ICursor pCursor = ModDBOperator.GetCursor(pWs as IFeatureWorkspace, "RasterCatalogLayerInfo", "RootPath", pWhereStr, out eError);
            if (pCursor == null || eError != null) return;
            
            IRow pRow = pCursor.NextRow();
            if (pRow != null)
                m_pRootPathfst = pRow.get_Value(0).ToString().Trim();
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            #endregion
            //cyf 20110620 add
            if (m_Operate == EnumRasterOperateType.Update)
            {
                if (m_pRootPathfst == "") return;
                rBServeData.Visible = false;
                rBServeData.Enabled = false;
                rBOtherData.Checked = true;
            }
            else
            {
                if (m_pRootPathfst != "")
                {
                    rBServeData.Visible = true;
                    rBServeData.Enabled = true;
                    rBServeData.Checked = true;
                }
                else
                {
                    rBServeData.Visible = true;
                    rBServeData.Enabled = false;
                    rBOtherData.Checked = true;
                }
            }
            //end
        }

        private void btnBroswer_Click(object sender, EventArgs e)
        {
             FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
             if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
             {
                 string pRootFile = pFolderBrowserDialog.SelectedPath;// 选择的文件夹路径
                 this.txtData.Text = pRootFile;
             }
        }

        private void rBServeData_CheckedChanged(object sender, EventArgs e)
        {
            if (rBServeData.Checked)
            {
                txtData.Enabled = false;
                btnBroswer.Enabled = false;
            }
            else
            {
                txtData.Enabled = true;
                btnBroswer.Enabled = true;
            }
        }
        //1、若加载外部数据，则首先将数据上传到FTP上（1、连接ftp；2、上传文件）
        //2、将ftp的数据录入到RasterCatalog中
        #region //原有用FTP的上传的确定事件 deled by xisheng 0915
        /*private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            ///获得树图上选择的工程节点
            #region 获得工程树图上的参数信息
            //cyf 20110626 modify
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode;     //当前树节点，栅格数据节点
            DevComponents.AdvTree.Node pDBProNode = m_Hook.ProjectTree.SelectedNode;   //获得工程树图节点
            while (pDBProNode.Parent != null)
            {
                pDBProNode = pDBProNode.Parent;
            }
            if (pDBProNode.DataKeyString != "project")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取工程树图节点失败！");
                return;
            }
            //end
            //cyf  20110609 modify: 20110626 modify :修改工程树图节点
            string pProjectID = pDBProNode.Name;                 //数据源ID
            string pProjectname = pDBProNode.Text;               //数据源名称
            //end
            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            //cyf
            if (Projectnode == null)
            {
                return;
            }
            //end
            //cyf 20110626 modify:获取栅格数据图层的存储类型
            //获得栅格数据库类型
            XmlElement pCurElem = null;  //图层xml节点
            try { pCurElem = pCurNode.Tag as XmlElement; }
            catch { }
            if (pCurElem == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取图层xml节点失败！");
                return;
            }
            string dbType = pCurElem.GetAttribute("存储类型").Trim();           //栅格数据存储类型
            string pDesRasterCollName = pCurElem.GetAttribute("名称").Trim();   //栅格数据名称
            //end

            //获得连接信息
            System.Xml.XmlElement DbConnElem = Projectnode.SelectSingleNode(".//内容//栅格数据库/连接信息") as System.Xml.XmlElement;
            string pType = DbConnElem.GetAttribute("类型");
            string pServer = DbConnElem.GetAttribute("服务器");
            string pInstance = DbConnElem.GetAttribute("服务名");
            string pDataBase = DbConnElem.GetAttribute("数据库");
            string pUser = DbConnElem.GetAttribute("用户");
            string pPassword = DbConnElem.GetAttribute("密码");
            string pVersion = DbConnElem.GetAttribute("版本");
            //cyf 20110609 add
            string pConnectInfo = "";         //连接信息字符串
            pConnectInfo = pType + "," + pServer + "," + pInstance + "," + pDataBase + "," + pUser + "," + pPassword + "," + pVersion;
            //end
            #endregion
            string pFtpConn = "";                 //ftp连接信息，包括ftp地址、用户名、密码
            string pftpServer = "";               //ftp服务地址
            string pftpUser = "";                 //ftp用户
            string pftpPassword = "";             //ftp用户密码
            IFeatureWorkspace pFeaWs = m_WS as IFeatureWorkspace;
            if (pFeaWs == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败！");
                return;
            }
            #region 查询系统维护库  cyf 20110629  modify
            ICursor pCursor = ModDBOperator.GetCursor(pFeaWs, "DATABASEMD", "ftpConn", "ID=" + pProjectID, out eError);
            if (pCursor == null || eError != null) return;
            IRow pRow = pCursor.NextRow();
            if (pRow == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库中数据源信息失败，数据源ID为：" + pProjectID);
                return;
            }
            pFtpConn = pRow.get_Value(0).ToString().Trim();
            if (pFtpConn == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据源的ftp连接信息失败！");
                return;
            }
            #endregion
            //cyf 20110629
            #region 获得ftp的连接信息,并连接Ftp
            string pErrorStr = "";           //错误信息
            string[] strConnArr = pFtpConn.Split('|');
            if (strConnArr.Length != 3)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据源的ftp连接信息失败！");
                return;
            }
            //通过连接信息数组，获取连接信息服务地址
            pftpServer = strConnArr[0].Trim();
            pftpUser = strConnArr[1].Trim();
            pftpPassword = strConnArr[2].Trim();
            FTP_Class Ftp = new FTP_Class(pftpServer, pftpUser, pftpPassword);
            Ftp.Connecttest(pftpServer, pftpUser, pftpPassword, out pErrorStr);
            if (pErrorStr != "Succeed")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接FTP失败!");
                return;
            }
            #endregion

            #region 获取FTP目录下所有的文件夹（包含所有子目录）
            List<string> LstAllDir = new List<string>(); //ftp根目录下所有文件夹数组(包含子目录)

            LstAllDir = Ftp.GetSubDirectory("", true, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取ftp根目录文件夹失败!");
                return;
            }
            #endregion
            #region 查询系统维护库，获得FTP根目录以及子目录（根目录的下级目录）的信息  cyf 20110629 add:
            string pWhereStr = "ProjectID=" + pProjectID + " and RasterLayerName='" + pDesRasterCollName + "'";
            pCursor = ModDBOperator.GetCursor(pFeaWs, "RasterCatalogLayerInfo", "RootPath", pWhereStr, out eError);
            if (pCursor == null || eError != null) return;
			string pRootPath="";
            pRow = pCursor.NextRow();
            Dictionary<string, string> DicRootPath = new Dictionary<string, string>();        //保存该栅格编目的根目录
            while (pRow != null)
            {
                pRootPath = pRow.get_Value(0).ToString().Trim();        //根目录
                if (!LstAllDir.Contains(pRootPath))
                {
                    //不包含该目录
                    pRow = pCursor.NextRow();
                    continue;
                }
                //获取跟目录下面对应的所有的子目录(下级目录)
                string[] DirArr = Ftp.GetFloder(pRootPath, out pErrorStr);
                if (DirArr != null)
                {
                    //遍历子目录名称
                    for (int i = 0; i < DirArr.Length; i++)
                    {
                        string subDir = DirArr[i].Trim();  //子目录名称
                        if (subDir.Length != 3) continue;
                        if (!DicRootPath.ContainsKey(subDir))
                        {
                            DicRootPath.Add(subDir, pRootPath + "/" + subDir);
                        }
                    }
                }
                pRow = pCursor.NextRow();
            }
            #endregion
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            if (m_AppForm.ProgressBar != null)
            {
                m_AppForm.ProgressBar.Visible = true;
            }
            //组织需要上传的数据，
            Dictionary<string, Dictionary<string, string>> dicMapDir = new Dictionary<string, Dictionary<string, string>>();
            if (rBServeData.Checked)
            {
                //从服务器上传数据 cyf 20110629 
                //获得目录下所有的文件夹（包括所有子目录）,并按照一定的格式来进行组织，方便入库操作
                dicMapDir = ManageFtpRaster(Ftp, DicRootPath, out eError);
                if (eError != null || dicMapDir == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "组织文件失败！\n原因：" + eError.Message);
                    return;
                }
                //end
                //cyf 20110629 modify
                //foreach (string pRootPath in LstRoorPath)
                //{
                //    //选择服务器上的数据
                //    if (!LstAllDir.Contains(pRootPath))
                //    {
                //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "ftp上不存在文件夹：" + pRootPath);
                //        return;
                //    }
                //}
            }
            else
            {
                //从外部数据上传数据 cyf 20110629 delete
                //if (!LstAllDir.Contains(pRootPath))
                //{
                //    //在ftp上创建文件夹
                //    Ftp.MakeDir(pRootPath, out pErrorStr);
                //    if (pErrorStr != "Succeed")
                //    {
                //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建文件夹：" + "ftp://" + pftpServer + "/" + pRootPath + ",失败！");
                //        return;
                //    }
                //    //cyf 20110620 add:在ftp上创建文件夹后，将文件夹的信息保存在列表中
                //    if (LstAllDir.Contains(pRootPath))
                //    {
                //        LstAllDir.Add(pRootPath);
                //    }
                //    ///end
                //}
                //将文件上传到FTP文件夹上
                //获取外部数据
                string pOrgRoot = this.txtData.Text.Trim(); //外部数据根目录
                DirectoryInfo pDirectoryInfo = new DirectoryInfo(pOrgRoot);
                //FileInfo[] pFileInfoArr = pDirectoryInfo.GetFiles("*.tif", SearchOption.AllDirectories);
                //获得源数据根目录以及子目录项的所有的文件
                FileInfo[] pFileInfoArr = pDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                //添加进度条
                if (m_AppForm.ProgressBar != null)
                {
                    m_AppForm.ProgressBar.Maximum = pFileInfoArr.Length;
                    m_AppForm.ProgressBar.Minimum = 0;
                    m_AppForm.ProgressBar.Value = 0;
                    Application.DoEvents();
                }
                if (m_AppForm != null)
                {
                    m_AppForm.OperatorTips = "将外部数据上传到ftp服务器上...";
                    Application.DoEvents();
                }

                #region 遍历源数据文件，并将源数据文件上传
                //保存外部上传的数据
                Dictionary<string, string> dicTransferDataDir = new Dictionary<string, string>();
                for (int i = 0; i < pFileInfoArr.Length; i++)
                {
                    FileInfo pFileInfo = pFileInfoArr[i];
                    string pFileName = pFileInfo.FullName;  //源文件名（包含路径的完整文件名）
                    string pureName = pFileInfo.Name;       //源文件名
                    //if (!pFileName.ToLower().EndsWith("tif") && !pFileName.ToLower().EndsWith("img") && !pFileName.ToLower().EndsWith("sid"))
                    //{
                    //    //如果不是TIF文件和IMG文件则不参与检查
                    //    //如果不是TIF文件和IMG文件则不参与检查
                    //    //进度条加1
                    //    if (m_AppForm.ProgressBar != null)
                    //    {
                    //        m_AppForm.ProgressBar.Value++;
                    //        Application.DoEvents();
                    //    }
                    //    continue;
                    //}

                    string pfileDirecName = pureName.Substring(0, pureName.IndexOf('.'));   //不带后缀的文件名
                    if (pfileDirecName.Length != 13 && pfileDirecName.Length != 19)//changed by xisheng 2011.07.18
                    {
                        //说明文件命名不规范
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "源文件：" + pfileDirecName + ",命名不规范,请检查！");
                        return;
                    }
                    //cyf 20110629 add
                    string pDesDirName = pfileDirecName.Substring(0, 3);     //该文件应存放在FTP上的父目录
                    if (!DicRootPath.ContainsKey(pDesDirName))
                    {
                        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "FTP上不存在父目录:" + pDesDirName);//deleted by xisheng 20110721
                        if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "FTP上不存在父目录，是否创建？"))//added by xisheng 20110721 创建新目录
                        {
                            Ftp.MakeDir(pRootPath+"/"+pDesDirName, out pErrorStr);
                            if (pErrorStr != "Succeed")
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建文件夹：" + "ftp://" + pftpServer +"/"+pRootPath+ "/" + pDesDirName + ",失败！");
                                return;
                            }
                            DicRootPath.Add(pDesDirName, pRootPath + "/" + pDesDirName);//创建文件夹成功后，添加到列表中
                        }
						//    if (LstAllDir.Contains(pRootPath))
                        //    {
                        //        LstAllDir.Add(pRootPath);
                        //    }
                        //}
                        //else
                        //{
                        continue;
                        //}
                    }
                    pRootPath = DicRootPath[pDesDirName];    //文件夹存储的上级目录
                    //end
                    #region 首先在FTP上创建存储源文件的上级父目录
                    if (m_AppForm != null)
                    {
                        m_AppForm.OperatorTips = "在ftp的文件夹：" + pRootPath + "下创建目录:" + pfileDirecName + "...";
                        Application.DoEvents();
                    }
                    //cyf 20110620 add
                    if (LstAllDir.Contains(pRootPath + "/" + pfileDirecName))
                    {
                        //eError = new Exception("ftp服务器上存在同名的文件夹：" + pRootPath + "/" + pfileDirecName + "，\n说明该数据已经上传！");
                        //进度条加1
                        //if (m_AppForm.ProgressBar != null)
                        //{
                        //    m_AppForm.ProgressBar.Value++;
                        //    Application.DoEvents();
                        //}
                        //continue;
                    }
                    else
                    {
                        //在ftp上创建文件夹
                        Ftp.MakeDir(pRootPath + "/" + pfileDirecName, out pErrorStr);
                        if (pErrorStr != "Succeed")
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建文件夹：\n" + "ftp://" + pftpServer + "/" + pRootPath + "/" + pfileDirecName + ",失败！");
                            return;
                        }
                        //cyf 20110620 add:将创建的文件夹保存在列表中
                        if (!LstAllDir.Contains(pRootPath + "/" + pfileDirecName))
                        {
                            LstAllDir.Add(pRootPath + "/" + pfileDirecName);
                        }
                    }
                    #endregion
                    #region 上传文件到FTP
                    if (m_AppForm != null)
                    {
                        m_AppForm.OperatorTips = "正在上传文件" + pureName + "...";
                        Application.DoEvents();
                    }
                    //将文件上传至创建的文件夹下面
                    Ftp.Upload(pFileName, pRootPath + "/" + pfileDirecName, pureName, out pErrorStr);
                    if (pErrorStr != "Succeed")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "上传文件失败！");
                        return;
                    }
                    #endregion
                    //组织外部上传的数据，以便进行入库
                    if (!dicTransferDataDir.ContainsKey(pDesDirName))
                    {
                        dicTransferDataDir.Add(pDesDirName, DicRootPath[pDesDirName]);
                    }
                    //进度条加1
                    if (m_AppForm.ProgressBar != null)
                    {
                        m_AppForm.ProgressBar.Value++;
                        Application.DoEvents();
                    }
                }
                #endregion
                dicMapDir = ManageFtpRaster(Ftp, dicTransferDataDir, out eError);//函数修改
                if (eError != null || dicTransferDataDir == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "组织文件失败！\n原因：" + eError.Message);
                    return;
                }
            }
            //以ftp上的数据作为源数据，进行入库，入库到RasterCatalog里面
            #region 1、设置目标数据库连接信息
            SysCommon.Gis.SysGisDataSet pSysDt = new SysCommon.Gis.SysGisDataSet();
            if (pType.ToUpper() == "PDB")
            {
                pSysDt.SetWorkspace(pDataBase, SysCommon.enumWSType.PDB, out eError);
            }
            else if (pType.ToUpper() == "GDB")
            {
                pSysDt.SetWorkspace(pDataBase, SysCommon.enumWSType.GDB, out eError);
            }
            else if (pType.ToUpper() == "SDE")
            {
                pSysDt.SetWorkspace(pServer, pInstance, "", pUser, pPassword, pVersion, out eError);
                pDesRasterCollName = pUser + "." + pDesRasterCollName;
            }
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！");
                return;
            }
            #endregion
            #region 获得ftp的对应目录下的所有的文件列表,并将数据录入到现势库中
            //用来保存同一个图幅数据的不同年份和不同路径集合（图幅号，年份，路径）
            //添加文字提示
            if (m_AppForm != null)
            {
                m_AppForm.OperatorTips = "将服务器上的数据进行入库...";
                Application.DoEvents();
            }
            //添加进度条
            if (m_AppForm.ProgressBar != null)
            {
                m_AppForm.ProgressBar.Maximum = dicMapDir.Count;
                m_AppForm.ProgressBar.Minimum = 0;
                m_AppForm.ProgressBar.Value = 0;
                Application.DoEvents();
            }

            //遍历不同的图幅号，进行入库
            foreach (KeyValuePair<string, Dictionary<string, string>> pMapInfo in dicMapDir)
            {
                string pMapID = pMapInfo.Key;   //图幅号
                //获得图幅号对应的时间最新的数据的时间（查询入库的数据和数据库，找到最新的）
                bool beEqual = false;
                string pLatestTime = GetLatestTime(pMapID, pMapInfo.Value, pProjectID, out beEqual, out eError);
                if (eError != null || pLatestTime=="-1") return;
                //遍历同一个图幅号对应的不同年份的数据，进行分门别类的入库
                foreach (KeyValuePair<string, string> pTimeItemInfo in pMapInfo.Value)
                {
                    if (pTimeItemInfo.Value.Trim() == "") continue;
                    string pDesRasterName = "";         //栅格编目名称

                    if (pTimeItemInfo.Key == pLatestTime)
                    {
                        if (beEqual)
                        {
                            //cyf =0说明现势库中已经存在该最新的数据
                            continue;
                        }
                        //最新的数据入到现势库，其他的数据入到历史库
                        pDesRasterName = pDesRasterCollName;
                        //现势库中该数据若存在对应的旧数据，则需要将旧的数据移植到历史库中
                        //通过读入图幅日志索引表来判断是否存在旧的数据
                        TransferData(pMapID, pDesRasterName, pSysDt.WorkSpace, pLatestTime,pProjectID,out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "栅格编目数据从现势库移植到历史库失败!\n" + eError.Message);
                            return;
                        }
                        //更新图幅日志索引表，将以前在旧的数据对应现势库图层名的日志更改为对应历史库图层名
                        string updateStr = "update RasterIndexTable set RasterLayerName ='" + pDesRasterCollName + "_goh' where RasterLayerName ='" + pDesRasterCollName + "' and MapID='" + pMapID + "' and ProID="+pProjectID;
                        try { m_WS.ExecuteSQL(updateStr); }
                        catch
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新图幅索引表失败！");
                            return;
                        }
                    }
                    else
                    {
                        //查询数据库，看该数据是否已经入库
                        pDesRasterName = pDesRasterCollName + "_GOH";
                    }
                    //获取文件夹下面的数据文件
                    string[] pDesFile = Ftp.GetFileList(pTimeItemInfo.Value, out pErrorStr); //获得文件夹下面的所有的文件列表
                    //遍历数据文件列表，将数据文件进行入库
                    for (int k = 0; k < pDesFile.Length; k++)
                    {
                        string pTempDesFile = pDesFile[k];  //数据文件名
                        if (!pTempDesFile.ToLower().EndsWith("tif") && !pTempDesFile.ToLower().EndsWith("img") && !pTempDesFile.ToLower().EndsWith("sid")) continue;
                        if (pTempDesFile.Substring(0, pTempDesFile.IndexOf('.')).Length != 19)
                        {
                            //数据命名不规范
                            continue;
                        }
                        #region 判断数据文件是否已经入库，若已经入库，则不进行入库  cyf 20110620
                        //若在图幅索引表中存在该记录说明，说明已经进行过入库
                        bool BeExist = false;
                        pWhereStr = "FilePath='" + pTimeItemInfo.Value + "/" + pTempDesFile + "' and RasterLayerName='" + pDesRasterName + "' and ProID="+pProjectID;
                        BeExist = BeExistDataInDB("RasterIndexTable", pWhereStr, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            return;
                        }
                        if (BeExist)
                        {
                            //说明该数据已经入库
                            continue;
                        }
                        #endregion

                        string pDesFullPath = "\\\\" + pftpServer + "\\" + pTimeItemInfo.Value + "\\" + pTempDesFile;
                        pDesFullPath = pDesFullPath.Replace('/', '\\');
                        if (m_AppForm != null)
                        {
                            m_AppForm.OperatorTips = "正在进行数据‘" + pTempDesFile + "’的入库...";
                            Application.DoEvents();
                        }
                        //栅格编目数据入库
                        InputRasterCatalogData(pDesRasterName, pDesFullPath, pSysDt.WorkSpace, out eError);
                        if (eError != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "栅格数据入库出错!\n" + eError.Message);
                            return;
                        }
                        //入库完成后，将相关信息写入到日志记录表
                        #region 更新图幅日志索引表
                        string pRasterType = "";  //栅格类型，用两位数表示
                        string pPixel = "";       //分辨率,用三位数表示
                        string pTime = "";       //数据生产年份，用四位数表示
                        string pFtpConn1 = "";    //ftp地址
                        string pFilePath = "";   //文件路径（完整路径）
                        string pOperatoer = "";  //操作员
 						string insertStr="";
                            pMapID = pTempDesFile.Substring(0, 10);
                            pFtpConn1 = pftpServer;
                            pFilePath = pTimeItemInfo.Value + "/" + pTempDesFile;
                            pOperatoer = this.txtOperator.Text.Trim();
                            //获得OBJECTID字段值
                            long pOBJECTID = ModDBOperator.GetMaxID(pFeaWs, "RasterIndexTable", "OBJECTID", out eError);
                            if (eError != null || pOBJECTID == -1) return;
                            if (pTempDesFile.Length == 19)//国家局标准命名 //xisheng 2011.07.19
                            {
                                pRasterType = pTempDesFile.Substring(10, 2);
                                pPixel = pTempDesFile.Substring(12, 3);
                                pTime = pTempDesFile.Substring(15, 4);
                                insertStr= "insert into RasterIndexTable values(" + pOBJECTID + ",'" + pMapID + "','" + pRasterType + "','" + pPixel + "'," + pTime + ",'" + pFilePath + "','" + pOperatoer + "','" + pFtpConn1 + "'," + pProjectID + ",'" + pDesRasterName + "')";
                            }
                            else//非国家局标准命名 //xisheng 2011.07.19
                            {
                                pRasterType = pTempDesFile.Substring(10);
                                insertStr = "insert into RasterIndexTable(OBJECTID,MapID,RasterType,FilePath,Operator,FileConn,PROID,RASTERLAYERNAME) values(" + pOBJECTID + ",'" + pMapID + "','" + pRasterType + "','" + pFilePath + "','" + pOperatoer + "','" + pFtpConn1 + "'," + pProjectID + ",'" + pDesRasterName + "')";
                            }
                        try { m_WS.ExecuteSQL(insertStr); }
                        catch
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "插入图幅索引表失败！");
                            return;
                        }
                        #endregion
                    }
                }
                //进度条加1
                if (m_AppForm.ProgressBar != null)
                {
                    m_AppForm.ProgressBar.Value++;
                    Application.DoEvents();
                }
            }
            //end
            #endregion
            //关闭窗口
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功！");
            m_AppForm.ProgressBar.Visible = false;
            this.Close();
        }*/
        #endregion

        //修改后确定事件 add by xisheng 0915
        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            ///获得树图上选择的工程节点
            #region 获得工程树图上的参数信息
            //cyf 20110626 modify
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode;     //当前树节点，栅格数据节点
            DevComponents.AdvTree.Node pDBProNode = m_Hook.ProjectTree.SelectedNode;   //获得工程树图节点
            while (pDBProNode.Parent != null)
            {
                pDBProNode = pDBProNode.Parent;
            }
            if (pDBProNode.DataKeyString != "project")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取工程树图节点失败！");
                return;
            }
            //end
            //cyf  20110609 modify: 20110626 modify :修改工程树图节点
            string pProjectID = pDBProNode.Name;                 //数据源ID
            string pProjectname = pDBProNode.Text;               //数据源名称
            //end
            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            //cyf
            if (Projectnode == null)
            {
                return;
            }
            //end
            //cyf 20110626 modify:获取栅格数据图层的存储类型
            //获得栅格数据库类型
            XmlElement pCurElem = null;  //图层xml节点
            try { pCurElem = pCurNode.Tag as XmlElement; }
            catch { }
            if (pCurElem == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取图层xml节点失败！");
                return;
            }
            string dbType = pCurElem.GetAttribute("存储类型").Trim();           //栅格数据存储类型
            string pDesRasterCollName = pCurElem.GetAttribute("名称").Trim();   //栅格数据名称
            //end

            //获得连接信息
            System.Xml.XmlElement DbConnElem = Projectnode.SelectSingleNode(".//内容//栅格数据库/连接信息") as System.Xml.XmlElement;
            string pType = DbConnElem.GetAttribute("类型");
            string pServer = DbConnElem.GetAttribute("服务器");
            string pInstance = DbConnElem.GetAttribute("服务名");
            string pDataBase = DbConnElem.GetAttribute("数据库");
            string pUser = DbConnElem.GetAttribute("用户");
            string pPassword = DbConnElem.GetAttribute("密码");
            string pVersion = DbConnElem.GetAttribute("版本");
            //cyf 20110609 add
            string pConnectInfo = "";         //连接信息字符串
            pConnectInfo = pType + "," + pServer + "," + pInstance + "," + pDataBase + "," + pUser + "," + pPassword + "," + pVersion;
            //end
            #endregion
            List<string> LstAllDir = new List<string>();
            IFeatureWorkspace pFeaWs = m_WS as IFeatureWorkspace;
            if (pFeaWs == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败！");
                return;
            }

            #region 查询系统维护库，获得根目录以及子目录（根目录的下级目录）的信息
            string pWhereStr = "ProjectID=" + pProjectID + " and RasterLayerName='" + pDesRasterCollName + "'";
            ICursor pCursor = ModDBOperator.GetCursor(pFeaWs, "RasterCatalogLayerInfo", "RootPath", pWhereStr, out eError);
            if (pCursor == null || eError != null) return;
            string pRootPath = ""; string pRootPathfst = "";
            IRow pRow = pCursor.NextRow();
            if (pRow != null)
                pRootPathfst = pRow.get_Value(0).ToString().Trim();
            Dictionary<string, string> DicRootPath = new Dictionary<string, string>();        //保存该栅格编目的根目录
            while (pRow != null)
            {
                pRootPath = pRow.get_Value(0).ToString().Trim();        //根目录

                //获取跟目录下面对应的所有的子目录(下级目录)
                string[] DirArr = Directory.GetDirectories(pRootPath); ;
                if (DirArr != null)
                {
                    //遍历子目录名称
                    for (int i = 0; i < DirArr.Length; i++)
                    {
                        string subDir = (DirArr[i].Trim());  //子目录名称;
                        subDir = subDir.Substring(subDir.LastIndexOf("\\") + 1);
                        if (subDir.Length != 3) continue;
                        if (!DicRootPath.ContainsKey(subDir))
                        {
                            DicRootPath.Add(subDir, pRootPath + "\\" + subDir);
                        }
                    }
                }

                pRow = pCursor.NextRow();
            }
            #endregion
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            if (m_AppForm.ProgressBar != null)
            {
                m_AppForm.ProgressBar.Visible = true;
            }
            #region 设置目标数据库连接信息
            SysCommon.Gis.SysGisDataSet pSysDt = new SysCommon.Gis.SysGisDataSet();
            if (pType.ToUpper() == "PDB")
            {
                pSysDt.SetWorkspace(pDataBase, SysCommon.enumWSType.PDB, out eError);
            }
            else if (pType.ToUpper() == "GDB")
            {
                pSysDt.SetWorkspace(pDataBase, SysCommon.enumWSType.GDB, out eError);
            }
            else if (pType.ToUpper() == "SDE")
            {
                pSysDt.SetWorkspace(pServer, pInstance, "", pUser, pPassword, pVersion, out eError);
                pDesRasterCollName = pUser + "." + pDesRasterCollName;
            }
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！");
                return;
            }
            #endregion

            string pOrgRoot = this.txtData.Text.Trim(); //外部数据根目录
            
           
            #region 没有设置一个服务器目录，直接将选择目录下所有文件入库
            if (pRootPathfst == "")
            {
                if (!Directory.Exists(pOrgRoot))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "路径不存在，请检查！");
                    return;
                }
                DirectoryInfo pDirectoryInfo = new DirectoryInfo(pOrgRoot);
                FileInfo[] pFileInfoArr = pDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                List<string> Typelist=new List<string>();
                Typelist.Add(".IMG");Typelist.Add(".SID");Typelist.Add(".TIF");
               List<string> Filelist=GetTheFiles(Typelist,pFileInfoArr);
                //添加文字提示
                if (m_AppForm != null)
                {
                    m_AppForm.OperatorTips = "将选择上的数据进行入库...";
                    Application.DoEvents();
                }

                //添加进度条
                if (m_AppForm.ProgressBar != null)
                {
                    m_AppForm.ProgressBar.Maximum = Filelist.Count;
                    m_AppForm.ProgressBar.Minimum = 0;
                    m_AppForm.ProgressBar.Value = 0;
                    Application.DoEvents();
                }
                for (int i = 0; i < Filelist.Count; i++)
                {
                    string pFileName = Filelist[i];  //源文件名（包含路径的完整文件名）
                    string pureName = pFileName.Substring(pFileName.LastIndexOf("\\")+1);       //源文件名



                    if (m_AppForm != null)
                    {
                        m_AppForm.OperatorTips = "正在进行数据‘" + pureName + "’的入库...";
                        Application.DoEvents();
                    }
                    //栅格编目数据入库
                    InputRasterCatalogData(pDesRasterCollName, pFileName, pSysDt.WorkSpace, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "栅格数据入库失败!\n" + eError.Message);
                        return;
                    }
                    //进度条加1
                    if (m_AppForm.ProgressBar != null)
                    {
                        m_AppForm.ProgressBar.Value++;
                        Application.DoEvents();
                    }

                }

            }
            #endregion
            #region 选了服务器路径，先上传到服务器路径，然后从服务器LOAD到RC中
            else
            {
                Dictionary<string, Dictionary<string, string>> dicMapDir = new Dictionary<string, Dictionary<string, string>>();
                if (rBServeData.Checked)
                {
                    //组织需要上传的数据，

                    //从服务器上传数据 cyf 20110629 
                    //获得目录下所有的文件夹（包括所有子目录）,并按照一定的格式来进行组织，方便入库操作
                    dicMapDir = ManageRaster(DicRootPath, out eError);
                    if (eError != null || dicMapDir == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "组织文件失败！\n原因：" + eError.Message);
                        return;
                    }
                }
                else //外部数据
                {
                    if (!Directory.Exists(pOrgRoot))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "路径不存在，请检查！");
                        return;
                    }
                    DirectoryInfo pDirectoryInfo = new DirectoryInfo(pOrgRoot);
                    //获得源数据根目录以及子目录项的所有的文件
                    FileInfo[] pFileInfoArr = pDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

                    //添加进度条
                    if (m_AppForm.ProgressBar != null)
                    {
                        m_AppForm.ProgressBar.Maximum = pFileInfoArr.Length;
                        m_AppForm.ProgressBar.Minimum = 0;
                        m_AppForm.ProgressBar.Value = 0;
                        Application.DoEvents();
                    }
                    if (m_AppForm != null)
                    {
                        m_AppForm.OperatorTips = "将外部数据上传到服务器上...";
                        Application.DoEvents();
                    }

                    #region 遍历源数据文件，并将源数据文件上传
                    //保存外部上传的数据
                    Dictionary<string, string> dicTransferDataDir = new Dictionary<string, string>();
                    for (int i = 0; i < pFileInfoArr.Length; i++)
                    {
                        FileInfo pFileInfo = pFileInfoArr[i];
                        string pFileName = pFileInfo.FullName;  //源文件名（包含路径的完整文件名）
                        string pureName = pFileInfo.Name;       //源文件名
                        string pfileDirecName = pureName.Substring(0, pureName.IndexOf('.'));   //不带后缀的文件名
                        if (pfileDirecName.Length != 13 && pfileDirecName.Length != 19)//changed by xisheng 2011.07.18
                        {
                            //说明文件命名不规范
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "源文件：" + pfileDirecName + ",命名不规范,请检查！");
                            return;
                        }
                        //cyf 20110629 add
                        string pDesDirName = pfileDirecName.Substring(0, 3);     //该文件应存放在FTP上的父目录
                        if (!DicRootPath.ContainsKey(pDesDirName))
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "FTP上不存在父目录:" + pDesDirName);//deleted by xisheng 20110721
                            if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "不存在父目录，是否创建？"))//added by xisheng 20110721 创建新目录
                            {

                                try
                                {
                                    Directory.CreateDirectory(pRootPathfst + "\\" + pDesDirName);
                                }
                                catch
                                {
                                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建文件夹：" + pRootPath + "\\" + pDesDirName + ",失败！");
                                    return;
                                }
                                DicRootPath.Add(pDesDirName, pRootPath + "\\" + pDesDirName);//创建文件夹成功后，添加到列表中
                            }
                            continue;
                        }
                        pRootPath = DicRootPath[pDesDirName];    //文件夹存储的上级目录
                        //end
                        #region 首先在创建存储源文件的上级父目录
                        if (!Directory.Exists(pRootPath + "\\" + pfileDirecName))
                        {
                            if (m_AppForm != null)
                            {
                                m_AppForm.OperatorTips = "在文件夹：" + pRootPath + "下创建目录:" + pfileDirecName + "...";
                                Application.DoEvents();
                            }
                            //创建文件夹
                            try
                            {
                                Directory.CreateDirectory(pRootPath + "\\" + pfileDirecName);
                            }
                            catch
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建文件夹：" + pRootPath + "\\" + pfileDirecName + ",失败！");
                                return;
                            }
                        }
                        //cyf 20110620 add:将创建的文件夹保存在列表中
                        if (!LstAllDir.Contains(pRootPath + "\\" + pfileDirecName))
                        {
                            LstAllDir.Add(pRootPath + "\\" + pfileDirecName);
                        }

                        #endregion
                        
                        #region 上传文件到指定目录
                        if (m_AppForm != null)
                        {
                            m_AppForm.OperatorTips = "正在上传文件" + pureName + "...";
                            Application.DoEvents();
                        }
                        //将文件上传至创建的文件夹下面
                        try
                        {
                            File.Copy(pFileName, pRootPath + "\\" + pfileDirecName + "\\" + pureName, true);
                        }
                        catch
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "上传文件失败！");
                            return;
                        }
                        #endregion
                        //组织外部上传的数据，以便进行入库
                        if (!dicTransferDataDir.ContainsKey(pDesDirName))
                        {
                            dicTransferDataDir.Add(pDesDirName, DicRootPath[pDesDirName]);
                        }
                        //进度条加1
                        if (m_AppForm.ProgressBar != null)
                        {
                            m_AppForm.ProgressBar.Value++;
                            Application.DoEvents();
                        }
                    }
                    #endregion
                    dicMapDir = ManageRaster(dicTransferDataDir, out eError);//函数修改
                    if (eError != null || dicTransferDataDir == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "组织文件失败！\n原因：" + eError.Message);
                        return;
                    }
                }
                //以上的数据作为源数据，进行入库，入库到RasterCatalog里面
                #region 获得对应目录下的所有的文件列表,并将数据录入到现势库中
                //用来保存同一个图幅数据的不同年份和不同路径集合（图幅号，年份，路径）
                //添加文字提示
                if (m_AppForm != null)
                {
                    m_AppForm.OperatorTips = "将服务器上的数据进行入库...";
                    Application.DoEvents();
                }
                //添加进度条
                if (m_AppForm.ProgressBar != null)
                {
                    m_AppForm.ProgressBar.Maximum = dicMapDir.Count;
                    m_AppForm.ProgressBar.Minimum = 0;
                    m_AppForm.ProgressBar.Value = 0;
                    Application.DoEvents();
                }

                //遍历不同的图幅号，进行入库
                foreach (KeyValuePair<string, Dictionary<string, string>> pMapInfo in dicMapDir)
                {
                    string pMapID = pMapInfo.Key;   //图幅号
                    //获得图幅号对应的时间最新的数据的时间（查询入库的数据和数据库，找到最新的）
                    bool beEqual = false;
                    string pLatestTime = GetLatestTime(pMapID, pMapInfo.Value, pProjectID, out beEqual, out eError);
                    if (eError != null || pLatestTime == "-1") return;
                    //遍历同一个图幅号对应的不同年份的数据，进行分门别类的入库
                    foreach (KeyValuePair<string, string> pTimeItemInfo in pMapInfo.Value)
                    {
                        if (pTimeItemInfo.Value.Trim() == "") continue;
                        string pDesRasterName = "";         //栅格编目名称

                        if (pTimeItemInfo.Key == pLatestTime)
                        {
                            if (beEqual)
                            {
                                //cyf =0说明现势库中已经存在该最新的数据
                                continue;
                            }
                            //最新的数据入到现势库，其他的数据入到历史库
                            pDesRasterName = pDesRasterCollName;
                            //现势库中该数据若存在对应的旧数据，则需要将旧的数据移植到历史库中
                            //通过读入图幅日志索引表来判断是否存在旧的数据
                            TransferData(pMapID, pDesRasterName, pSysDt.WorkSpace, pLatestTime, pProjectID, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "栅格编目数据从现势库移植到历史库失败!\n" + eError.Message);
                                return;
                            }
                            //更新图幅日志索引表，将以前在旧的数据对应现势库图层名的日志更改为对应历史库图层名
                            string updateStr = "update RasterIndexTable set RasterLayerName ='" + pDesRasterCollName + "_goh' where RasterLayerName ='" + pDesRasterCollName + "' and MapID='" + pMapID + "' and ProID=" + pProjectID;
                            try { m_WS.ExecuteSQL(updateStr); }
                            catch
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新图幅索引表失败！");
                                return;
                            }
                        }
                        else
                        {
                            //查询数据库，看该数据是否已经入库
                            pDesRasterName = pDesRasterCollName + "_GOH";
                        }
                        //获取文件夹下面的数据文件
                        //string[] pDesFile = Ftp.GetFileList(pTimeItemInfo.Value, out pErrorStr); //获得文件夹下面的所有的文件列表
                        string[] pDesFile = Directory.GetFiles(pTimeItemInfo.Value, "*.*", SearchOption.AllDirectories);
                        //遍历数据文件列表，将数据文件进行入库
                        for (int k = 0; k < pDesFile.Length; k++)
                        {
                            string pTempDesFile = pDesFile[k];  //数据文件名
                            pTempDesFile = Path.GetFileName(pTempDesFile);
                            if (!pTempDesFile.ToLower().EndsWith("tif") && !pTempDesFile.ToLower().EndsWith("img") && !pTempDesFile.ToLower().EndsWith("sid")) continue;
                            if (pTempDesFile.Substring(0, pTempDesFile.IndexOf('.')).Length != 19 && pTempDesFile.Substring(0, pTempDesFile.IndexOf('.')).Length != 13)
                            {
                                //数据命名不规范
                                continue;
                            }
                            #region 判断数据文件是否已经入库，若已经入库，则不进行入库  cyf 20110620
                            //若在图幅索引表中存在该记录说明，说明已经进行过入库
                            bool BeExist = false;
                            pWhereStr = "FilePath='" + pTimeItemInfo.Value + "\\" + pTempDesFile + "' and RasterLayerName='" + pDesRasterName + "' and ProID=" + pProjectID;
                            BeExist = BeExistDataInDB("RasterIndexTable", pWhereStr, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                return;
                            }
                            if (BeExist)
                            {
                                //说明该数据已经入库
                                continue;
                            }
                            #endregion

                            string pDesFullPath = pTimeItemInfo.Value + "\\" + pTempDesFile;
                            pDesFullPath = pDesFullPath.Replace('/', '\\');
                            if (m_AppForm != null)
                            {
                                m_AppForm.OperatorTips = "正在进行数据‘" + pTempDesFile + "’的入库...";
                                Application.DoEvents();
                            }
                            //栅格编目数据入库
                            InputRasterCatalogData(pDesRasterName, pDesFullPath, pSysDt.WorkSpace, out eError);
                            if (eError != null)
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "栅格数据入库出错!\n" + eError.Message);
                                return;
                            }
                            //入库完成后，将相关信息写入到日志记录表
                            #region 更新图幅日志索引表
                            string pRasterType = "";  //栅格类型，用两位数表示
                            string pPixel = "";       //分辨率,用三位数表示
                            string pTime = "";       //数据生产年份，用四位数表示
                            string pFtpConn1 = "";    //ftp地址
                            string pFilePath = "";   //文件路径（完整路径）
                            string pOperatoer = "";  //操作员
                            string insertStr = "";
                            pMapID = pTempDesFile.Substring(0, 10);
                            pFilePath = pTimeItemInfo.Value + "\\" + pTempDesFile;
                            pOperatoer = this.txtOperator.Text.Trim();
                            //获得OBJECTID字段值
                            long pOBJECTID = ModDBOperator.GetMaxID(pFeaWs, "RasterIndexTable", "OBJECTID", out eError);
                            if (eError != null || pOBJECTID == -1) return;
                            if (pTempDesFile.Length == 19)//国家局标准命名 //xisheng 2011.07.19
                            {
                                pRasterType = pTempDesFile.Substring(10, 2);
                                pPixel = pTempDesFile.Substring(12, 3);
                                pTime = pTempDesFile.Substring(15, 4);
                                insertStr = "insert into RasterIndexTable values(" + pOBJECTID + ",'" + pMapID + "','" + pRasterType + "','" + pPixel + "'," + pTime + ",'" + pFilePath + "','" + pOperatoer + "','" + pFtpConn1 + "'," + pProjectID + ",'" + pDesRasterName + "')";
                            }
                            else//非国家局标准命名 //xisheng 2011.07.19
                            {
                                pRasterType = pTempDesFile.Substring(10);
                                insertStr = "insert into RasterIndexTable(OBJECTID,MapID,RasterType,FilePath,Operator,FileConn,PROID,RASTERLAYERNAME) values(" + pOBJECTID + ",'" + pMapID + "','" + pRasterType + "','" + pFilePath + "','" + pOperatoer + "','" + pFtpConn1 + "'," + pProjectID + ",'" + pDesRasterName + "')";
                            }
                            try { m_WS.ExecuteSQL(insertStr); }
                            catch
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "插入图幅索引表失败！");
                                return;
                            }
                            #endregion
                        }
                    }
                    //进度条加1
                    if (m_AppForm.ProgressBar != null)
                    {
                        m_AppForm.ProgressBar.Value++;
                        Application.DoEvents();
                    }
                }
                //end
                #endregion

            }
            #endregion

            //关闭窗口
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功！");
            m_AppForm.ProgressBar.Visible = false;
            this.Close();
        }
        //xisheng 20110917 add：获得指定后缀名的
        public List<string> GetTheFiles(List<string> Type,FileInfo[] Fileinfo)
        {
            List<string> Filelist=new List<string>();
            for (int i = 0; i < Fileinfo.Length; i++)
            {
                FileInfo pFileInfo = Fileinfo[i];
                string pureName = pFileInfo.Extension;       //源文件名
                if (Type.Contains(pureName.ToUpper()))
                {
                    if (!Filelist.Contains(pFileInfo.FullName))
                        Filelist.Add(pFileInfo.FullName);
                }
            }
            return Filelist;
        }
        
        // xisheng 20110917 add：获得子文件夹
        private string[] GetSubDirectory(string path)
        {
            string[] list;
            if (Directory.Exists(path))
            {
                list = Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories);
                return list;
            }
            else
                return null;
        }
        //xisheng 20110915 add：组织需要上传的数据的相关信息
        private Dictionary<string, Dictionary<string, string>> ManageRaster(Dictionary<string, string> dicRootPath, out Exception eError)
        {
            eError = null;
            Dictionary<string, Dictionary<string, string>> dicMapDir = new Dictionary<string, Dictionary<string, string>>();
            foreach (KeyValuePair<string, string> pathInfo in dicRootPath)
            {
                string[] LstSubDir;//该指定文件夹目录下的所有的文件夹
                LstSubDir = GetSubDirectory(pathInfo.Value);
                if (LstSubDir.Length == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取指定目录文件夹：" + pathInfo.Value + ",下的所有的文件夹失败!");
                    return null;
                }
                #region 遍历文件夹,对文件夹按照数据来进行组织
                for (int j = 0; j < LstSubDir.Length; j++)
                {
                    string parentFileName = LstSubDir[j].Substring(LstSubDir[j].LastIndexOf('\\') + 1);
                    if (parentFileName.Length == 19)//判断若为19位就分年份什么的存储 xisheng 2011.07.19
                    {
                        //说明不为空的文件夹


                        string pMapID = "";      //图幅号，用前十位数表示
                        string pRasterType = "";  //栅格类型，用两位数表示
                        string pPixel = "";       //分辨率,用三位数表示
                        string pTime = "";       //数据生产年份，用四位数表示
                        pMapID = parentFileName.Substring(0, 10);
                        pRasterType = parentFileName.Substring(10, 2);
                        pPixel = parentFileName.Substring(12, 3);
                        pTime = parentFileName.Substring(15, 4);
                        //将需要入库的数据父目录根据数据的不同年份进行存储
                        if (!dicMapDir.ContainsKey(pMapID))
                        {
                            Dictionary<string, string> dicTimePath = new Dictionary<string, string>();
                            if (!dicTimePath.ContainsKey(pTime))
                            {
                                dicTimePath.Add(pTime, LstSubDir[j]);
                            }
                            dicMapDir.Add(pMapID, dicTimePath);
                        }
                        else
                        {
                            if (!dicMapDir[pMapID].ContainsKey(pTime))
                            {
                                dicMapDir[pMapID].Add(pTime, LstSubDir[j]);
                            }
                        }
                    }
                    else if (parentFileName.Length == 13)//广州13位影像数据将时间定死 xisheng 2011.07.19
                    {
                        string pMapID = parentFileName.Substring(0, 10);
                        if (!dicMapDir.ContainsKey(pMapID))
                        {
                            Dictionary<string, string> dicTimePath = new Dictionary<string, string>();
                            if (!dicTimePath.ContainsKey("2011"))
                            {
                                dicTimePath.Add("2011", LstSubDir[j]);
                            }
                            dicMapDir.Add(pMapID, dicTimePath);
                        }
                        else
                        {
                            if (!dicMapDir[pMapID].ContainsKey("2011"))
                            {
                                dicMapDir[pMapID].Add("2011", LstSubDir[j]);
                            }
                        }
                    }
                }
                #endregion
            }
            return dicMapDir;
        }

        //cyf 20110629 add:组织需要上传的数据的相关信息
        private Dictionary<string, Dictionary<string, string>> ManageFtpRaster(FTP_Class Ftp, string pRootPath, out Exception eError)
        {
            Dictionary<string, Dictionary<string, string>> dicMapDir = new Dictionary<string, Dictionary<string, string>>();
            List<string> LstSubDir = new List<string>();  //该指定文件夹目录下的所有的文件夹
            LstSubDir = Ftp.GetSubDirectory(pRootPath, true, out eError);
            if (eError != null || LstSubDir == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取ftp指定目录文件夹：" + pRootPath + ",下的所有的文件夹失败!");
                return null;
            }
                #region 遍历文件夹,对文件夹按照数据来进行组织
                for (int j = 0; j < LstSubDir.Count; j++)
                {
                    string parentFileName = LstSubDir[j].Substring(LstSubDir[j].LastIndexOf('/') + 1);
                    if (parentFileName.Length != 19)
                    {
                        //说明为空的文件夹
                        continue;
                    }
                    string pMapID = "";      //图幅号，用前十位数表示
                    string pRasterType = "";  //栅格类型，用两位数表示
                    string pPixel = "";       //分辨率,用三位数表示
                    string pTime = "";       //数据生产年份，用四位数表示
                    pMapID = parentFileName.Substring(0, 10);
                    pRasterType = parentFileName.Substring(10, 2);
                    pPixel = parentFileName.Substring(12, 3);
                    pTime = parentFileName.Substring(15, 4);
                    //将需要入库的数据父目录根据数据的不同年份进行存储
                    if (!dicMapDir.ContainsKey(pMapID))
                    {
                        Dictionary<string, string> dicTimePath = new Dictionary<string, string>();
                        if (!dicTimePath.ContainsKey(pTime))
                        {
                            dicTimePath.Add(pTime, LstSubDir[j]);
                        }
                        dicMapDir.Add(pMapID, dicTimePath);
                    }
                    else
                    {
                        if (!dicMapDir[pMapID].ContainsKey(pTime))
                        {
                            dicMapDir[pMapID].Add(pTime, LstSubDir[j]);
                        }
                    }
                }
                #endregion
            return dicMapDir;
        }

        //cyf 20110629 add：组织需要上传的数据的相关信息
        private Dictionary<string, Dictionary<string, string>> ManageFtpRaster( FTP_Class Ftp,Dictionary<string, string> dicRootPath, out Exception eError)
        {
            eError = null;
            Dictionary<string, Dictionary<string, string>> dicMapDir = new Dictionary<string, Dictionary<string, string>>();
            foreach (KeyValuePair<string, string> pathInfo in dicRootPath)
            {
                List<string> LstSubDir = new List<string>();  //该指定文件夹目录下的所有的文件夹
                LstSubDir = Ftp.GetSubDirectory(pathInfo.Value, true, out eError);
                if (eError != null || LstSubDir == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取ftp指定目录文件夹：" + pathInfo.Value + ",下的所有的文件夹失败!");
                    return null ;
                }
                #region 遍历文件夹,对文件夹按照数据来进行组织
                for (int j = 0; j < LstSubDir.Count; j++)
                {
                    string parentFileName = LstSubDir[j].Substring(LstSubDir[j].LastIndexOf('/') + 1);
                    if (parentFileName.Length == 19)//判断若为19位就分年份什么的存储 xisheng 2011.07.19
                    {
                        //说明不为空的文件夹


                        string pMapID = "";      //图幅号，用前十位数表示
                        string pRasterType = "";  //栅格类型，用两位数表示
                        string pPixel = "";       //分辨率,用三位数表示
                        string pTime = "";       //数据生产年份，用四位数表示
                        pMapID = parentFileName.Substring(0, 10);
                        pRasterType = parentFileName.Substring(10, 2);
                        pPixel = parentFileName.Substring(12, 3);
                        pTime = parentFileName.Substring(15, 4);
                        //将需要入库的数据父目录根据数据的不同年份进行存储
                        if (!dicMapDir.ContainsKey(pMapID))
                        {
                            Dictionary<string, string> dicTimePath = new Dictionary<string, string>();
                            if (!dicTimePath.ContainsKey(pTime))
                            {
                                dicTimePath.Add(pTime, LstSubDir[j]);
                            }
                            dicMapDir.Add(pMapID, dicTimePath);
                        }
                        else
                        {
                            if (!dicMapDir[pMapID].ContainsKey(pTime))
                            {
                                dicMapDir[pMapID].Add(pTime, LstSubDir[j]);
                            }
                        }
                    }
                    else if (parentFileName.Length == 13)//广州13位影像数据将时间定死 xisheng 2011.07.19
                    {
                        string pMapID = parentFileName.Substring(0, 10);
                        if (!dicMapDir.ContainsKey(pMapID))
                        {
                            Dictionary<string, string> dicTimePath = new Dictionary<string, string>();
                            if (!dicTimePath.ContainsKey("2011"))
                            {
                                dicTimePath.Add("2011", LstSubDir[j]);
                            }
                            dicMapDir.Add(pMapID, dicTimePath);
                        }
                        else
                        {
                            if (!dicMapDir[pMapID].ContainsKey("2011"))
                            {
                                dicMapDir[pMapID].Add("2011", LstSubDir[j]);
                            }
                        }
                    }
                   
                }
                #endregion
            }
            return dicMapDir;
        }

        //cyf add 20110619  content:获得年份最新的年份（键值）,参数：数据的年份和对应的数据路径
        private string GetLatestTime(string pMapID,Dictionary<string, string> DicTimePathInfo,string projectID,out bool beEqual,out Exception eError)
        {
            eError = null;
            beEqual = false;
            string pLatestTime = "";   //最新的年份
            foreach (KeyValuePair<string, string> item in DicTimePathInfo)
            {
                string pTime = item.Key;
                if (pLatestTime == "")
                {
                    pLatestTime = pTime;
                }
                if (Convert.ToInt64(pLatestTime) < Convert.ToInt64(pTime))
                {
                    pLatestTime = pTime;
                }
            }
            //用当前最新的时间跟数据库中的时间进行比较，返回最新的时间 cyf 20110629
            IFeatureWorkspace pFeaWS = ModData.TempWks as IFeatureWorkspace;
            if (pFeaWS == null)
            {
                eError = new Exception("连接系统维护库失败!");
                return "-1";
            }
            long pDBLatestTime = GetMaxField(pFeaWS, "RasterIndexTable", "MapID='" + pMapID + "' and ProID=" + projectID, "UpdateTime", out eError);
            if (pDBLatestTime != -1)
            {
                if (Convert.ToInt64(pLatestTime) <pDBLatestTime)
                {
                    pLatestTime = pDBLatestTime.ToString();
                }
                else if (Convert.ToInt64(pLatestTime) == pDBLatestTime)
                {
                    beEqual = true;
                }
            }
            return pLatestTime;
        }

        //cyf 20110619 add :将现势库中旧的数据移植到历史库中，并且删除旧的数据
        //参数：图幅号，栅格编目现势图层名，工作空间，栅格数据存储工作空间，异常（输出）
        private void TransferData(string pMapID, string pDesRasterCollName, IWorkspace pWs, string pLasterTime,string projectID, out Exception outError)
        {
            outError = null;
            IFeatureWorkspace pFeaWs = m_WS as IFeatureWorkspace;
            if (pFeaWs != null)
            {
                #region cyf 20110620 统计当图幅号为当前数据图幅号时，图幅索引表中最新的年份
                //获得该图幅数据在数据库中最新的年份，并与当前数据进行比较，若新与当前数据，则返回  cyf 20110629
                //long pMaxTime = GetMaxField(pFeaWs, "RasterIndexTable", "MapID='" + pMapID + "' and RasterLayerName='" + pDesRasterCollName + "'", "UpdateTime",out outError);
                //if (outError != null)
                //{
                //    outError = new Exception("获取该图幅数据对应的最大值失败！");
                //    return;
                //}
                //if (pMaxTime >= Convert.ToInt64(pLasterTime))
                //{
                //    //说明数据库中的数据比上传时ftp的数据更新，无需更新
                //    outError = new Exception("数据库中已经存在更新的数据："+pMapID+","+pLasterTime);
                //    return;
                //}
                #endregion

                IQueryDef pQueryDef = pFeaWs.CreateQueryDef();
                pQueryDef.Tables = "RasterIndexTable";
                pQueryDef.SubFields = "FilePath,FileConn";
                pQueryDef.WhereClause = "MapID='" + pMapID + "' and RasterLayerName='" + pDesRasterCollName + "' and ProID=" + projectID;
                ICursor pCursor = null;
                try { pCursor = pQueryDef.Evaluate(); }
                catch
                {
                    outError = new Exception("查询数据图幅日志索引表失败！");
                    return;
                }
                if (pCursor == null)
                {
                    outError = new Exception("查询数据图幅日志索引表失败！");
                    return;
                }
                IRow pRow = pCursor.NextRow();
                //遍历图幅对应的旧的数据，将旧的数据录入到历史库中
                while (pRow != null)
                {
                    string pFullPath = pRow.get_Value(0).ToString();  //目标文件完整路径
                    string pConn = pRow.get_Value(1).ToString();     //目标文件服务器IP地址
                    string pDesFullPath = "\\\\" + pConn + "\\" + pFullPath;   //目标文件远程路径
                    pDesFullPath = pDesFullPath.Replace('/', '\\');

                    //现势库中删除栅格编目数据
                    IFeatureWorkspace mFeaRasteWs = pWs as IFeatureWorkspace;
                    if (mFeaRasteWs == null)
                    {
                        outError = new Exception("获取现势库工作空间失败！");
                        return;
                    }
                    //删除现势库中的数据
                    DeleteRows(mFeaRasteWs, pDesRasterCollName, pFullPath, out outError);
                    if (outError != null)
                    {
                        outError = new Exception("删除现势库中数据出错!\n" + outError.Message);
                        return;
                    }
                    //对应的栅格编目数据入历史库 
                    #region 若历史库中已经存在的该数据，则不用入库  cyf s20110620
                    //cyf 20110620 add;判断数据库中是否存在当前数据
                    bool beExist = false;   //表示数据是否存在
                    beExist=BeExistDataInDB("RasterIndexTable", "FilePath='" + pFullPath + "' and RasterLayerName='" + pDesRasterCollName + "_goh' and ProID="+projectID,out outError);
                    if(outError!=null)
                    {
                        return;
                    }
                    if (beExist)
                    {
                        //说明该数据已经入库
                        continue;
                    }
                    //end
                    #endregion
                    InputRasterCatalogData(pDesRasterCollName + "_GOH", pDesFullPath, pWs, out outError);
                    if (outError != null)
                    {
                        outError = new Exception("栅格数据入历史库出错!\n" + outError.Message);
                        return;
                    }
                    pRow = pCursor.NextRow();

                }
                //释放游标
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            }
        }
        // *-------------------------------------------------------------------------------------------------------
        // *功能函数：数据库中是否存在该数据，
        // *开 发 者：陈亚飞
        // *开发日期：20110620
        // *参    数：表格所在工作空间，表格名称，自动编号的字段名，异常
        // *返    回：若存在则返回true;若不存在，则返回false
        private bool BeExistDataInDB(string pTableName,string WhereStr,out Exception outError)
        {
            bool BeExist = false ;
            outError = null;
            IFeatureWorkspace mFeaWS = m_WS as IFeatureWorkspace;
            if (mFeaWS == null)
            {
                outError = new Exception("获取系统维护库工作空间失败！");
                return false;
            }
            ITable pTable = mFeaWS.OpenTable(pTableName);
            IQueryFilter pFilter = new QueryFilterClass();
            pFilter.WhereClause = WhereStr;
            ICursor mCursor = pTable.Search(pFilter, false);
            if (mCursor == null)
            {
                outError = new Exception("查询系统维护库失败！");
                return false;
            }
            IRow mRow = mCursor.NextRow();
            if (mRow != null)
            {
                //说明该数据已经入库
                BeExist = true;
            }
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(mCursor);
            return BeExist;
        }

        // *-------------------------------------------------------------------------------------------------------
        // *功能函数：获取字段的最大值
        // *开 发 者：陈亚飞
        // *开发日期：20110620
        // *参    数：表格所在工作空间，表格名称，自动编号的字段名，异常
        // *返    回：返回该字段的下一个值
        public long GetMaxField(IFeatureWorkspace pFeaWs, string pTableName,string whereStr, string pFiledName, out Exception outError)
        {
            outError = null;
            long ReturnMaxID = -1;
            //获取表格
            ITable pTable = pFeaWs.OpenTable(pTableName);
            IQueryFilter pFilter = new QueryFilterClass();
            if (whereStr != "")
            {
                pFilter.WhereClause = whereStr;
            }
            try
            {
                //表格行数
                long count = Convert.ToInt64(pTable.RowCount(pFilter).ToString());
                if (count > 0)
                {
                    //若表格行数部位0行，则统计表格中该字段的最大值
                    IDataStatistics pDataSta = new DataStatisticsClass();
                    pDataSta.Field = pFiledName;
                    pDataSta.Cursor = pTable.Search(pFilter, false);
                    IStatisticsResults pStaRes = null;
                    pStaRes = pDataSta.Statistics;
                    count = (long)pStaRes.Maximum;
                    //下一个值为最大值+1
                    ReturnMaxID = count;
                }
                return ReturnMaxID;
            }
            catch (Exception eError)
            {
                outError = new Exception("获取自动编号的下一个值失败！");
                return -1;
            }
        }

        //cyf 20110619 add:删除现势库中旧的数据
        //参数：栅格数据存储工作空间,栅格现势库图层名，栅格数据名称字段值，异常（输出）
        private void DeleteRows(IFeatureWorkspace pFeaWs,string pRasterName,string pFieldFullName, out Exception outError)
        {
            outError = null;
            //获取不带完整路径的数据文件名称字段值
            int index = pFieldFullName.LastIndexOf('/');
            string pFieldName = pFieldFullName.Substring(index + 1);
            ITable pTable = null;
            try { pTable = pFeaWs.OpenTable(pRasterName); }
            catch
            {
                outError = new Exception("获取RasterCatalog现势库失败！");
                return;
            }
            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "Name='"+pFieldName+"'";
            try
            {
                //删除对应的旧的数据
                pTable.DeleteSearchedRows(pQueryFilter);
            }
            catch
            {
                outError = new Exception("删除RasterCatalog现势库中旧的数据失败！");
                return;
            }

            //删除索引表对应的记录
            //string deleteStr="delete from RasterIndexTable where 
        }

        /// <summary>
        /// 导入数据（栅格目录库）
        /// </summary>
        /// <param name="RCDatasetName">栅格目录名称</param>
        /// <param name="filepaths">源数据路径</param>
        /// <param name="pWorkspace">栅格目录工作空间</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool InputRasterCatalogData(string RCDatasetName, string filepaths, IWorkspace pWorkspace, out Exception eError)
        {
            eError = null;

            try
            {
                IRasterCatalogLoader pRCLoader = new RasterCatalogLoaderClass();
                pRCLoader.Workspace = pWorkspace;
                pRCLoader.LoadDatasets(RCDatasetName, filepaths, null);
                //Marshal.ReleaseComObject(pRCLoader);
                return true;
            }
            catch (Exception eX)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(eX, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(eX, null, DateTime.Now);
                }
                //********************************************************************

                eError = eX;
                return false;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
