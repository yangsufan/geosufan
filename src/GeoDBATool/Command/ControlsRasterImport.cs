using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Data;
using System.Xml;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.Geoprocessor;

namespace GeoDBATool
{
    /// <summary>
    /// 栅格数据入库 陈亚飞
    /// </summary>
    public class ControlsRasterImport: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;        //主功能应用app        
        Plugin.Application.IAppFormRef pAppFormRef;          //应用程序主窗体
        //private System.Windows.Forms.Timer _timer;           //计时器

        //初始化按钮信息
        public ControlsRasterImport()
        {
            base._Name = "GeoDBATool.ControlsRasterImport";
            base._Caption = "栅格数据入库";
            base._Tooltip = "栅格数据入库";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "栅格数据入库";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110626 modify m_Hook.ProjectTree.SelectedNode.DataKeyString != "RC" && 
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "RD") return false;
                //end
                return true;
            }
        }

        public override string Message
        {
            get
            {
                pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            Exception eError = null;
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.CheckFileExists = true;
            OpenFile.CheckPathExists = true;
            OpenFile.Title = "选择栅格数据";
            OpenFile.Filter = "tif数据(*.tif)|*.tif|img数据(*.img)|*.img";
            OpenFile.Multiselect =true;
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string[] fileArr = OpenFile.FileNames;

                ///获得树图上选择的工程节点
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
                //cyf  20110609 modify:
                //string pProjectname = pCurNode.Name;
                string pProjectname = pDBProNode.Text;
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
                //System.Xml.XmlElement DbTypeElem = Projectnode.SelectSingleNode(".//内容//栅格数据库") as System.Xml.XmlElement;
                //string dbType = DbTypeElem.GetAttribute("存储类型");   //栅格编目、栅格数据集
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

                //获得栅格目录，栅格数据集名称
                //System.Xml.XmlElement DbcataLogNameElem = Projectnode.SelectSingleNode(".//栅格数据库/连接信息/库体") as System.Xml.XmlElement;
                //if (DbcataLogNameElem == null) return;
                //string pDesRasterCollName = DbcataLogNameElem.GetAttribute("名称");    //栅格数据名称
                //if (pDesRasterCollName == "") return;
                //end

                //获得连接信息
                System.Xml.XmlElement DbConnElem = Projectnode.SelectSingleNode(".//栅格数据库/连接信息") as System.Xml.XmlElement;
                string pType = DbConnElem.GetAttribute("类型");
                string pServer = DbConnElem.GetAttribute("服务器");
                string pInstance = DbConnElem.GetAttribute("服务名");
                string pDataBase = DbConnElem.GetAttribute("数据库");
                string pUser = DbConnElem.GetAttribute("用户");
                string pPassword = DbConnElem.GetAttribute("密码");
                string pVersion = DbConnElem.GetAttribute("版本");
                //cyf 20110609 add
                string pConnectInfo = "";         //连接信息字符串
                pConnectInfo = pType + "|" + pServer + "|" + pInstance + "|" + pDataBase + "|" + pUser + "|" + pPassword + "|" + pVersion;
                //end
                //获得目标数据库的参数
                //System.Xml.XmlElement DbParaElem = Projectnode.SelectSingleNode(".//栅格数据库/参数设置") as System.Xml.XmlElement;
                //string resampleTypeStr = DbParaElem.GetAttribute("重采样类型");
                //string compressionTypeStr = DbParaElem.GetAttribute("压缩类型");
                //string pyramid = DbParaElem.GetAttribute("金字塔");
                //string tileH = DbParaElem.GetAttribute("瓦片高度");
                //string tileW = DbParaElem.GetAttribute("瓦片宽度");
                //string bandNum = DbParaElem.GetAttribute("波段");
                //rstResamplingTypes resampleType = GetResampleTpe(resampleTypeStr);
                //esriRasterCompressionType compressionType = GetCompression(compressionTypeStr);
                //if (tileH == "")
                //{
                //    tileH = "128";
                //}
                //if (tileW == "")
                //{
                //    tileW = "128";
                //}
                //if (pyramid == "")
                //{
                //    pyramid = "6";
                //}

                //设置数据库连接信息
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

                //Thread aThread = null;
                //cyf 20110609 add:读取栅格数据入库日志，检查是否存在已经入库的数据
                SysCommon.DataBase.SysTable pSysTable = null;
                Dictionary<string, string> DicbeInDb = new Dictionary<string, string>();  //用来已经入库的栅格数据的源路径和目标连接信息
                if (File.Exists(ModData.RasterInDBLog))
                {
                    //若存在栅格数据入库日志，则将入库过的数据的路径保存起来
                    pSysTable = new SysCommon.DataBase.SysTable();
                    pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ModData.RasterInDBLog + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接栅格入库日志表失败！");
                        return;
                    }
                    //获得栅格入库日志表
                    DataTable pTable = pSysTable.GetTable("rasterfileinfo", out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取栅格入库日志表失败！");
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    for (int i = 0; i < pTable.Rows.Count; i++)
                    {
                        string pFileName = pTable.Rows[i][0].ToString();
                        string pDesPath = pTable.Rows[i][3].ToString();
                        if (!DicbeInDb.ContainsKey(pFileName))
                        {
                            DicbeInDb.Add(pFileName, pDesPath);
                        }
                    }
                    //关闭连接
                    pSysTable.CloseDbConnection();
                    //cyf 20110610 modify
                    if (pTable.Rows.Count > 0)
                    {
                        //存在已经入库的数据
                        RasterErrInfoFrm pRasterErrInfoFrm = new RasterErrInfoFrm(pTable);
                        if (pRasterErrInfoFrm.ShowDialog() != DialogResult.OK)
                        {
                            //若不继续入库，则返回
                            return;
                        }
                    }
                    //end
                }
                //end

                ///进行数据入库
                //cyf 20110609
                //连接栅格数据入库日志表
                if (!File.Exists(ModData.RasterInDBLog))
                {
                    if (File.Exists(ModData.RasterInDBTemp))
                    {
                        File.Copy(ModData.RasterInDBTemp, ModData.RasterInDBLog);
                    }
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有找到日志表模板：" + ModData.RasterInDBTemp);

                    return;
				}
                pSysTable = new SysCommon.DataBase.SysTable();
                pSysTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ModData.RasterInDBLog + ";Persist Security Info=True", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接栅格入库日志表失败！");
                    return;
                }
                //添加进度条
                if (pAppFormRef.ProgressBar != null)
                {
                    pAppFormRef.ProgressBar.Visible = true;
                    pAppFormRef.ProgressBar.Maximum = fileArr.Length;
                    pAppFormRef.ProgressBar.Minimum = 0;
                    pAppFormRef.ProgressBar.Value = 0;
                    Application.DoEvents();
                }
                //end
                //List<string> fileList = new List<string>();   //要进行入库的文件名
                for (int i = 0; i < fileArr.Length; i++)
                {
                    //if (!fileList.Contains(fileArr[i]))
                    //{
                    //    fileList.Add(fileArr[i]);
                    //}
                    //cyf 20110609 add:对入库过的数据进行判断和筛选
                    if (DicbeInDb.ContainsKey(fileArr[i]))
                    {
                        //已经进行入库
                        if (DicbeInDb[fileArr[i]] == pConnectInfo)
                        {
                            //如果源的路径与目标的路径都相同，则说明已经入库了
                            //进度条加1
                            if (pAppFormRef.ProgressBar != null)
                            {
                                pAppFormRef.ProgressBar.Value++;
                                Application.DoEvents();
                            }
                            continue;
                        }
                    }
                    //end
                    //cyf 20110610 add:文字提示
                    if (pAppFormRef != null)
                    {
                        pAppFormRef.OperatorTips = "正在进行数据" + fileArr[i] + "入库...";
                        Application.DoEvents();
                    }
                    //end
                    //cyf 20110610 adds
                    string Starime =DateTime.Now.ToLongDateString()+" "+ DateTime.Now.ToLongTimeString();  //开始时间
                    //end
                    string fileFullName = fileArr[i];
                    if (dbType == "栅格数据集")
                    {
                        //栅格数据集入库
                        InputRasterDataset(pDesRasterCollName, fileFullName, pSysDt.WorkSpace, out eError);
                    }
                    else if (dbType == "栅格编目")
                    {
                        //栅格编目数据入库
                        InputRasterCatalogData(pDesRasterCollName, fileFullName, pSysDt.WorkSpace, out eError);
                    }
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "栅格数据入库出错!\n" + eError.Message);
                        return;
                    }
                    //若入库成功后，更新则将入库成功的数据添加到栅格数据日志表中
                    //cyf 20110609 插入栅格数据日志表中
                   string insertStr = "insert into rasterfileinfo values('" + fileArr[i] + "','"+Starime+"','" +DateTime.Now.ToLongDateString()+" "+DateTime.Now.ToLongTimeString()+ "','"+pConnectInfo+"')";
                    pSysTable.UpdateTable(insertStr, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "插入栅格入库日志表失败！");
                        continue;
                    }
                    //进度条加1
                    if (pAppFormRef.ProgressBar != null)
                    {
                        pAppFormRef.ProgressBar.Value++;
                        Application.DoEvents();
                    }
                    //end
                }
                //cyf  20110609 add:
                //关闭连接
                pSysTable.CloseDbConnection();
                //end
                //if (dbType == "栅格编目")
                //{
                //    InputRasterCatalogData(pDesRasterCollName, fileList, pSysDt.WorkSpace, out eError);
                //}
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "栅格数据入库完成!");

                //cyf 20110609 add
                if (pAppFormRef.ProgressBar != null)
                {
                    pAppFormRef.ProgressBar.Visible = false;
                    //cyf 20110610 add:清空文字提示
                    pAppFormRef.OperatorTips = "";
                    //end
                }
                //end
                //cyf 20110610 add:栅格正常结束的日志
                if (File.Exists(ModData.RasterInDBLog))
                {
                    //File.Delete(ModData.RasterInDBLog);
                }
                //end
            }
        }

        #region 委托

        private delegate void ImportRaster(string pDesRasterCollName, string[] fileArr, IWorkspace pWorkSpace, out Exception eError);
        /// <summary>
        /// 栅格数据入库
        /// </summary>
        /// <param name="pDesRasterCollName"></param>
        /// <param name="fileArr"></param>
        /// <param name="pWorkSpace"></param>
        /// <param name="eError"></param>
        private void ImportRasteData(string dbType,string pDesRasterCollName,string[] fileArr,IWorkspace pWorkSpace,out Exception eError)
        {
            eError = null;
           if(pAppFormRef ==null) return;
           // //进度条显示
           //pAppFormRef.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { true});
            for (int i = 0; i < fileArr.Length; i++)
            {
                //if (!fileList.Contains(fileArr[i]))
                //{
                //    fileList.Add(fileArr[i]);
                //}

                string fileFullName = fileArr[i];
                if (dbType == "栅格数据集")
                {
                    InputRasterDataset(pDesRasterCollName, fileFullName, pWorkSpace, out eError);
                }
                else if (dbType == "栅格编目")
                {
                    InputRasterCatalogData(pDesRasterCollName, fileFullName, pWorkSpace, out eError);
                }
                if (eError != null)
                {
                    pAppFormRef.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "错误", "栅格数据入库出错!\n" + eError.Message });
                    return;
                }
            }
            ////隐藏进度条
            //pAppFormRef.MainForm.Invoke(new ShowProgress(ShowProgressBar), new object[] { false});
            pAppFormRef.MainForm.Invoke(new ShowForm(ShowErrForm), new object[] { "提示", "栅格数据入库完成!" });
        }

      
        //弹出提示对话框
        private delegate void ShowForm(string strCaption, string strText);
        private void ShowErrForm(string strCaption, string strText)
        {
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(strCaption, strText);
        }
        //控制进度条显示
        private delegate void ShowProgress(bool bVisible);
        private void ShowProgressBar(bool bVisible)
        {
            if (bVisible == true)
            {
                pAppFormRef.ProgressBar.Visible = true;
                //this.Cursor = Cursors.WaitCursor; //获取鼠标的形状,为沙漏形状

            }
            else
            {
                pAppFormRef.ProgressBar.Visible = false;
                //this.Cursor = Cursors.Default; //获取鼠标为正常形状
            }
        }

        //修改进度条
        private delegate void ChangeProgress(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value);
        private void ChangeProgressBar(DevComponents.DotNetBar.ProgressBarItem pProgressBar, int min, int max, int value)
        {
            if (min != -1)
            {
                pProgressBar.Minimum = min;
            }
            if (max != -1)
            {
                pProgressBar.Maximum = max;
            }
            pProgressBar.Value = value;
            pProgressBar.Refresh();
        }

        #endregion

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
        }

        /// <summary>
        /// 导入数据（栅格数据集）
        /// </summary>
        /// <param name="RDDatasetName">栅格数据集名称</param>
        /// <param name="filePath">源数据路径</param>
        /// <param name="pWorkspace">栅格数据集工作空间</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool InputRasterDataset(string RDDatasetName, string filePath, IWorkspace pWorkspace, out Exception eError)
        {
            eError = null;
            FileInfo pFileInfo = new FileInfo(filePath);
            string pFileDic = pFileInfo.DirectoryName;               //文件目录
            string pFileName = pFileInfo.Name;                     //文件名
            try
            {
                //IMosaicRaster pMosaicR = null;
                //目标栅格数据集
                IRasterDataset pObjRasterDataset = GetRasterDataset(RDDatasetName, pWorkspace, out eError);
                if (pObjRasterDataset == null) return false;
                //IRaster pObjRaster = pObjRasterDataset.CreateDefaultRaster();
                //if(pObjRaster!=null)
                //{
                //    pMosaicR = pObjRaster as IMosaicRaster;
                //}

                //栅格数据工作空间
                IWorkspaceFactory pOrgRasterWsFac = new RasterWorkspaceFactoryClass();
                IWorkspace pWS = pOrgRasterWsFac.OpenFromFile(pFileDic, 0);
                IRasterWorkspace2 pRasterWS = pWS as IRasterWorkspace2;
                if (pRasterWS == null) return false;
                IRasterDataset pOrgRDataset = pRasterWS.OpenRasterDataset(pFileName);
                IRaster pOrgRaster = pOrgRDataset.CreateDefaultRaster();

                //load raster data to exist raster dataset
                IRasterLoader pRasterLoad = new RasterLoaderClass();
                if (pOrgRaster != null)
                {
                    //if(pMosaicR!=null)
                    //{
                    //    //若目标要素不为空，则进行拼接
                    //    //pMosaicR.MosaicOperatorType = rstMosaicOperatorType.MT_LAST;
                    //    pMosaicR.OrderByField = "Name";
                    //    pMosaicR.Ascending = true;
                    //    pMosaicR.MosaicColormapMode = rstMosaicColormapMode.MM_MATCH;
                    //    IMosaicOperator pMosaicOpe=pMosaicR.MosaicOperator;
                        
                    //}
                    pRasterLoad.Background = 0;     //background value be ignored when loading
                    pRasterLoad.PixelAlignmentTolerance = 0;     //重采样的容差
                    pRasterLoad.MosaicColormapMode = rstMosaicColormapMode.MM_LAST;  //拼接的颜色采用 last map color

                    pRasterLoad.Load(pObjRasterDataset, pOrgRaster);
                }
                Marshal.ReleaseComObject(pRasterLoad);
            }
            catch (System.Exception ex)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************

                eError = ex;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 导入数据（栅格目录库）
        /// </summary>
        /// <param name="RCDatasetName">栅格目录名称</param>
        /// <param name="filepaths">源数据文件列表</param>
        /// <param name="pWorkspace">栅格目录工作空间</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public bool InputRasterCatalogData(string RCDatasetName, List<string> filepaths,IWorkspace pWorkspace, out Exception eError)
        {
            eError = null;
            string NameList = String.Empty;
            foreach (string filepath in filepaths)
            {
                if (NameList.Length == 0)
                {
                    NameList = filepath;
                }
                else
                {
                    NameList = NameList + " ; " + filepath;
                }
            }
            return InputRasterCatalogData(RCDatasetName, NameList,pWorkspace, out eError);
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

        /// <summary>
        /// 获得栅格数据集
        /// </summary>
        /// <param name="name">栅格数据集名称</param>
        /// <param name="pWorkspace">geodatabase数据库工作空间</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private IRasterDataset GetRasterDataset(string name,IWorkspace pWorkspace,out Exception eError)
        {
            eError = null;

            IRasterWorkspaceEx pRasterWSEx = (IRasterWorkspaceEx)pWorkspace;
            try
            {   //要素集可能不存在，做一次保护
                return pRasterWSEx.OpenRasterDataset(name);
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
                return null;
            }
        }

        /// <summary>
        /// 获得重采样类型
        /// </summary>
        /// <returns></returns>
        private rstResamplingTypes GetResampleTpe(string str)
        {
            rstResamplingTypes resampleType = rstResamplingTypes.RSP_NearestNeighbor;
            switch (str)
            {
                case "邻近法":
                    resampleType = rstResamplingTypes.RSP_NearestNeighbor;
                    break;
                case "双线性内插法":
                    resampleType = rstResamplingTypes.RSP_BilinearInterpolation;
                    break;
                case "立方卷积法":
                    resampleType = rstResamplingTypes.RSP_CubicConvolution;
                    break;
            }
            return resampleType;
        }

        /// <summary>
        /// 获得压缩类型
        /// </summary>
        /// <returns></returns>
        private esriRasterCompressionType GetCompression(string str)
        {
            esriRasterCompressionType compressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
            switch (str)
            {
                case "LZ77":
                    compressionType = esriRasterCompressionType.esriRasterCompressionLZ77;
                    break;
                case "JPEG":
                    compressionType = esriRasterCompressionType.esriRasterCompressionJPEG;
                    break;
                case "JPEG2000":
                    compressionType = esriRasterCompressionType.esriRasterCompressionJPEG2000;
                    break;
                case "PackBits":
                    compressionType = esriRasterCompressionType.esriRasterCompressionPackBits;
                    break;
                case "LZW":
                    compressionType = esriRasterCompressionType.esriRasterCompressionLZW;
                    break;
            }
            return compressionType;
        }
    }
}

