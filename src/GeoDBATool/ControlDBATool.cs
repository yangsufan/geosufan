using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Xml;
using SysCommon.Authorize;

using ESRI.ArcGIS.Geodatabase;

namespace GeoDBATool
{
    public class ControlDBATool : Plugin.Interface.ControlRefBase
    {
        private Plugin.Application.IAppFormRef _hook;
        private UserControlDBATool _ControlDBATool;

        //构造函数
        public ControlDBATool()
        {
            base._Name = "GeoDBATool.ControlDBATool";
            base._Caption = "数据库工程管理";
            base._Visible = false;
            base._Enabled = false;
        }

        public override bool Visible
        {
            get
            {
                try
                {
                    if (_hook == null)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    if (_hook.CurrentSysName != base._Name)
                    {
                        base._Visible = false;
                        _ControlDBATool.Visible = false;
                        ModData.v_AppGIS.StatusBar.Visible = false;
                        return false;
                    }

                    base._Visible = true;
                    _ControlDBATool.Visible = true;
                    ModData.v_AppGIS.StatusBar.Visible = true;
                    return true;
                }
                catch
                {
                    base._Visible = false;
                    return false;
                }
            }
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_hook != null)
                    {
                        if (_hook.CurrentSysName != base._Name)
                        {
                            base._Enabled = false;
                            _ControlDBATool.Enabled = false;
                            ModData.v_AppGIS.StatusBar.Enabled = false;
                            return false;
                        }

                        base._Enabled = true;
                        _ControlDBATool.Enabled = true;
                        ModData.v_AppGIS.StatusBar.Enabled = true;
                        return true;
                    }
                    else
                    {
                        base._Enabled = false;
                        return false;
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            _hook = hook as Plugin.Application.IAppFormRef;

            if (_hook == null) return;
            //changed by chulili 20110711添加输入参数
            //ModData.v_AppGIS = new Plugin.Application.AppGIS(_hook.MainForm, _hook.ControlContainer, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath);
            ModData.v_AppGIS = new Plugin.Application.AppGIS(_hook.MainForm, _hook.ControlContainer, _hook.ListUserPrivilegeID, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath, _hook.ConnUser);
            
            _ControlDBATool = new UserControlDBATool(this.Name, this.Caption);

            _ControlDBATool.Show();

            _hook.MainForm.Controls.Add(_ControlDBATool);
            _hook.MainForm.Controls.Add(ModData.v_AppGIS.StatusBar);
            _hook.MainForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);  //窗体关闭时间
            _ControlDBATool.EnabledChanged += new EventHandler(_ControlDBATool_EnabledChanged);  // Enable事件，用来触发数据库工程树图界面的初始化

            ModData.v_AppGIS.RefScaleCmb.SelectedIndexChanged += new EventHandler(RefScaleCmb_SelectedIndexChanged);
            ModData.v_AppGIS.CurScaleCmb.SelectedIndexChanged += new EventHandler(CurScaleCmb_SelectedIndexChanged);

            //*****************************************
            // guozheng added system Function Log
            if (ModData.SysLog == null)
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
            }
            //*****************************************

            //添加回车事件自定义比例尺
            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = ModData.v_AppGIS.CurScaleCmb.ComboBoxEx;
            vComboEx.KeyDown += new KeyEventHandler(vComboEx_KeyDown);
            if (_ControlDBATool.Enabled)
            {
                //若Enable为true，则根据xml初始化数据库工程树图节点
                //清空工程树图和图层
                ModData.v_AppGIS.ProjectTree.Nodes.Clear();
                ModData.v_AppGIS.MapControl.Map.ClearLayers();
                ModData.v_AppGIS.MapControl.ActiveView.Refresh();
                ModData.v_AppGIS.TOCControl.Update();
                InitialDBProject();
                //end
            }
        }

        //响应回车时间 改变当前显示比例尺
        void vComboEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = sender as DevComponents.DotNetBar.Controls.ComboBoxEx;
            string strScale = vComboEx.Text;
            double dblSacle = 0;

            try
            {
                if (double.TryParse(strScale, out dblSacle))
                {
                    ModData.v_AppGIS.MapControl.Map.MapScale = dblSacle;
                    ModData.v_AppGIS.MapControl.ActiveView.Refresh();
                }
                else
                {
                    vComboEx.Text = ModData.v_AppGIS.MapControl.Map.MapScale.ToString();
                }
            }
            catch
            {
            }
        }
        //在退出系统前如正在处理数据应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Plugin.Application.IAppGISRef pApp = ModData.v_AppGIS as Plugin.Application.IAppGISRef;
            if (pApp == null)
            {
                //******************************************************************************
                //guozheng added
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Teminate();
                //******************************************************************************
                return;
            }
            if (pApp.CurrentThread != null)
            {
                pApp.CurrentThread.Suspend();
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "当前任务正在进行,是否终止退出?") == true)
                {
                    //******************************************************************************
                    //guozheng added
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Teminate();
                    //******************************************************************************
                    try
                    {
                        pApp.CurrentThread.Abort();
                    } catch { }  //cyf 20110711 modify
                }
                else
                {
                    pApp.CurrentThread.Resume();
                    e.Cancel = true;
                }
            }
            else
            {
                //******************************************************************************
                //guozheng added
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Teminate();
                //******************************************************************************
            }
        }

        //参考比例尺事件　陈亚飞添加
        private void RefScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModData.v_AppGIS.MapControl.Map.ReferenceScale = Convert.ToDouble(ModData.v_AppGIS.RefScaleCmb.SelectedItem.ToString().Trim());
            }
            catch
            {
                ModData.v_AppGIS.RefScaleCmb.Text = ModData.v_AppGIS.MapControl.Map.ReferenceScale.ToString("0");
            }
            ModData.v_AppGIS.MapControl.ActiveView.Refresh();
        }
        //当前比例尺事件 陈亚飞添加
        private void CurScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModData.v_AppGIS.MapControl.Map.MapScale = Convert.ToDouble(ModData.v_AppGIS.CurScaleCmb.SelectedItem.ToString().Trim());
            }
            catch
            {
                ModData.v_AppGIS.CurScaleCmb.Text = ModData.v_AppGIS.MapControl.Map.MapScale.ToString("0");
            }
            ModData.v_AppGIS.MapControl.ActiveView.Refresh();
        }

        //初始化数据供工程树图  chenyafei  add 20110215 
        private void _ControlDBATool_EnabledChanged(object sender, EventArgs e)
        {
            //cyf 20110612  :取消从集成管理子系统进入数据更新子系统中工程树图的更新
            if (_ControlDBATool.Enabled)
            {
                //若Enable为true，则根据xml初始化数据库工程树图节点
                //清空工程树图和图层
                ModData.v_AppGIS.ProjectTree.Nodes.Clear();
                ModData.v_AppGIS.MapControl.Map.ClearLayers();
                ModData.v_AppGIS.MapControl.ActiveView.Refresh();
                ModData.v_AppGIS.TOCControl.Update();
                InitialDBProject();
                //end
            }
            else
            {
                //否则清空树图节点
                ModData.v_AppGIS.ProjectTree.Nodes.Clear();
            }
            //end
        }

        //初始化数据库工程树图  cyf 20110624
        public  void InitialDBProject()
        {
            Exception pError=null;
            if (_hook.LstRoleInfo == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取当前登录用户对应的角色信息失败！");
                return;
            }
            IWorkspace pWs = null;    //系统维护库工作空间
            pWs = GetSysDBWs(out pError);
            if (pWs == null || pError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败！\n原因：" + pError.Message);
                return;
            }
            //获得数据权限ID集合
            List<string> LstPriID = new List<string>();  //角色对应的数据权限ID集合
            LstPriID=GetDataPriID(_hook.LstRoleInfo,pWs, out pError);
            if (pError != null || LstPriID == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据权限ID集合失败！\n原因：" + pError.Message);
                return;
            }
            if (LstPriID.Count == 0)
                return;
            //根据权限ID信息，将角色对应的权限信息保存起来
            Dictionary<string, Dictionary<string,string>> DicDataPriInfo = new Dictionary<string, Dictionary<string, string>>();//数据信息（数据源、数据集、要素类）
            DicDataPriInfo = GetPriInfo(pWs, LstPriID,out pError);  //cyf 20110626
            if (pError != null || DicDataPriInfo == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据权限信息失败！\n原因：" + pError.Message);
                return;
            }
            if(DicDataPriInfo.Count==0) return;
            //将数据权限信息，存储在工程树图界面xml中
            SaveInfoToXml(pWs, DicDataPriInfo, out pError);
            if (pError != null) return;
            //根据工程树图xml刷新工程树图界面
            if (File.Exists(ModData.v_projectDetalXML))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ModData.v_projectDetalXML);
                ModData.v_AppGIS.DBXmlDocument = xml;

               ModDBOperator.RefreshProjectTree(ModData.v_AppGIS.DBXmlDocument, ModData.v_AppGIS.ProjectTree, out pError);
            }

            #region 原有代码
            //刷新工程xml
            //SaveAndRefreshXml(_hook.LstRoleInfo, out pError);
            //if (pError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "初始化工程树图失败，原因：\n" + pError.Message);
            //    return;
            //}
            ////根据工程xml刷新树图
            //if (File.Exists(ModData.v_projectXML))
            //{
            //    XmlDocument xml = new XmlDocument();
            //    xml.Load(ModData.v_projectXML);
            //    ModData.v_AppGIS.DBXmlDocument = xml;

            //    ProjectXml.AddTreeNodeByXML(ModData.v_AppGIS.DBXmlDocument, ModData.v_AppGIS.ProjectTree);
            //}
            #endregion
        }

       
        // *----------------------------------------------------------------------------------------
        // *功能函数：获取系统维护库对应的工作空间
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-24
        // *参    数：异常（返回值）
        // *返    回：返回系统维护库工作空间
        private IWorkspace GetSysDBWs(out Exception outError)
        {
            outError = null;
            IWorkspace pWs = null;      //系统维护库工作空间
            #region 连接系统维护库
            //获得系统维护库的连接信息
            if (_hook.TempWksInfo.Wks == null)
            {
                //判断配置文件是否存在
                bool blnCanConnect = false;
                SysCommon.Gis.SysGisDB vgisDb = new SysCommon.Gis.SysGisDB();
                if (File.Exists(ModData.v_ConfigPath))
                {
                    //获得系统维护库连接信息
                    SysCommon.Authorize.AuthorizeClass.GetConnectInfo(ModData.v_ConfigPath, out ModData.Server, out ModData.Instance, out ModData.Database, out ModData.User, out ModData.Password, out ModData.Version, out ModData.dbType);
                    //连接系统维护库
                    blnCanConnect = CanOpenConnect(vgisDb, ModData.dbType, ModData.Server, ModData.Instance, ModData.Database, ModData.User, ModData.Password, ModData.Version);
                }
                else
                {
                    outError = new Exception("缺失系统维护库连接信息文件：" + ModData.v_ConfigPath + "/n请重新配置");
                    return null ;
                }
                if (!blnCanConnect)
                {
                    outError = new Exception("系统能够维护库连接失败，请检查!");
                    return  null;
                }
                pWs = vgisDb.WorkSpace;
            }
            else
            {
                pWs = _hook.TempWksInfo.Wks;
            }
            if (pWs == null)
            {
                outError = new Exception("系统能够维护库连接失败，请检查!");
                return null ;
            }
            #endregion
            ModData.TempWks = pWs;  //将系统维护库的链接信息保存起来
            return pWs;
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：查询系统维护库信息，读取角色对应的数据权限
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-24
        // *参    数：角色信息集合列表，系统维护哭工作空间，异常（输出）
        // *返    回：返回角色对应的数据权限的ID集合
        private List<string> GetDataPriID(List<Role> LstRole, IWorkspace pWs,out Exception outError)
        {
            outError = null;
            string pRoleIDWhere = "";                           //用户所对应的所有的角色ID过滤条件
            List<string> returnLstPriID = new List<string>();   //保存返回值
            IFeatureWorkspace pFeaWs = pWs as IFeatureWorkspace;
            if (pFeaWs == null)
            {
                outError = new Exception("获取系统维护库工作空间失败！");
                return null;
            }
            //遍历角色信息，组织角色过滤条件
            foreach (Role pRole in LstRole)
            {
                string pRoleID = pRole.IDStr;    //角色ID
                if (pRoleID.Trim() != "")
                {
                    pRoleIDWhere += "'" + pRoleID + "',";
                }
            }
            if (pRoleIDWhere.Trim() == "")
            {
                outError = new Exception("获取当前登录用户对应的角色信息失败！");
                return null;
            } 
            pRoleIDWhere = "(" + pRoleIDWhere.Substring(0, pRoleIDWhere.Length - 1) + ")";

            //查询角色数据权限表，获取角色对应的数据权限的ID集合
            ICursor pCursor = null;
            //获得角色数据权限表的游标
            pCursor = ModDBOperator.GetCursor(pFeaWs, "ROLE_DATA", "DATAPRI_ID", "ROLEID in " + pRoleIDWhere, out outError);
            if (outError != null || pCursor==null)
            {
                return null;
            }
            IRow pRow = pCursor.NextRow();//查询到的行
            while (pRow != null)
            {
                string mPriID = pRow.get_Value(0).ToString();       //数据权限ID
                if (!returnLstPriID.Contains(mPriID))
                {
                    returnLstPriID.Add(mPriID);
                }
                pRow = pCursor.NextRow();
            }
            //释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
            return returnLstPriID;
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：根据权限ID信息，将角色对应的权限信息保存起来
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-24
        // *参    数：系统维护库工作空间、异常（输出）
        // *返    回：返回角色对应的数据权限信息集合(数据源、数据集、图层名集合)
        private Dictionary<string, Dictionary<string, string>> GetPriInfo(IWorkspace pWs, List<string> LstPriID, out Exception outError)
        {
            outError = null;
            Dictionary<string, Dictionary<string, string>> dicDataInfo = new Dictionary<string, Dictionary<string, string>>();
            XmlDocument docXml = new XmlDocument();  //图层树xml
            //湖区图层树xml
            docXml = GetAllPriXml(pWs, out outError);
            if (outError != null || docXml == null)
            {
                return null;
            }
            //遍历数据权限ID集合
            foreach (string mPriID in LstPriID)
            {
                //根据数据权限获得图层layer节点
                XmlNode pNode = null;   //Layer节点
                try { pNode = docXml.DocumentElement.SelectSingleNode(".//Layer[@NodeKey='" + mPriID + "']"); }
                catch { }
                if (pNode == null) continue;
                XmlElement pElem = pNode as XmlElement;
                if (pElem == null) continue;
                string pDBSourceID = "";                      //数据源ID
                string pFeaDTName = "";                       //数据集
                string pFeaclsName = "";                      //要素类名称
                pDBSourceID = pElem.GetAttribute("ConnectKey").Trim();
                pFeaDTName = pElem.GetAttribute("FeatureDatasetName").Trim();
                pFeaclsName = pElem.GetAttribute("Code").Trim();
                string DataType = pElem.GetAttribute("DataType").Trim();
           
                if (pFeaclsName == "") continue;
                string tmpname = pFeaclsName;//过滤掉不是地形数据的 20111025 xisheng
                if (pFeaclsName.Contains("."))
                {
                    pFeaclsName = tmpname.Substring(tmpname.LastIndexOf(".") + 1);
                }
                if ( DataType != "FC")  //changed by chulili 20120830 DLGK的限制删除掉（建矢量库的标准规范，山西项目未作此要求）
                    continue;
                //cyf 20110626 add:
                string dbTypeID = "";     //数据库类型
                //执行查询，获取数据库类型
                IFeatureWorkspace pFeaWS = pWs as IFeatureWorkspace;
                if (pFeaWS == null)
                {
                    outError = new Exception("获取系统维护库工作空间失败！");
                    return null;
                }
                ICursor pCursor = ModDBOperator.GetCursor(pFeaWS, "DATABASEMD", "DATABASETYPEID", "ID=" + pDBSourceID, out outError);
                if (pCursor == null || outError != null)
                {
                    return null;
                }
                IRow pRow = pCursor.NextRow();//查询到的行
                //cyf 20110630 modify
                if (pRow == null)
                {
                    //outError = new Exception("查询数据源项目信息表失败！");
                    //return null;
                    continue;
                }
                //end
                dbTypeID = pRow.get_Value(0).ToString();    //数据库类型
                //end
                //组织数据源、数据集、要素类
                if (dbTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                {
                    if (!dicDataInfo.ContainsKey(pDBSourceID))
                    {
                        Dictionary<string, string> dicDTFeaClsInf = new Dictionary<string, string>();
                        dicDTFeaClsInf.Add(pFeaDTName, pFeaclsName);
                        dicDataInfo.Add(pDBSourceID, dicDTFeaClsInf);
                    }
                    else
                    {
                        if (!dicDataInfo[pDBSourceID].ContainsKey(pFeaDTName))
                        {

                            dicDataInfo[pDBSourceID].Add(pFeaDTName, pFeaclsName);
                        }
                        else
                        {
                            if (!dicDataInfo[pDBSourceID][pFeaDTName].Contains(pFeaclsName))
                            {
                                pFeaclsName = dicDataInfo[pDBSourceID][pFeaDTName] + "," + pFeaclsName;
                                dicDataInfo[pDBSourceID].Remove(pFeaDTName);
                                dicDataInfo[pDBSourceID].Add(pFeaDTName, pFeaclsName);
                            }
                        }
                    }
                }
                //cyf 20110626 modify:添加对栅格数据库的信息组织
                else if (dbTypeID == enumInterDBType.高程数据库.GetHashCode().ToString() || dbTypeID == enumInterDBType.影像数据库.GetHashCode().ToString())
                {
                    if (!dicDataInfo.ContainsKey(pDBSourceID))
                    {
                        Dictionary<string, string> dicDTFeaClsInf = new Dictionary<string, string>();
                        dicDTFeaClsInf.Add(pFeaclsName, "");
                        dicDataInfo.Add(pDBSourceID, dicDTFeaClsInf);
                    }
                    else
                    {
                        if (!dicDataInfo[pDBSourceID].ContainsKey(pFeaclsName))
                        {

                            dicDataInfo[pDBSourceID].Add(pFeaclsName, "");
                        }
                    }
                }
                //end
            }
            return dicDataInfo;
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：根据权限ID信息，读取数据库中的图层树XML并进行解析
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-24
        // *参    数：系统维护库工作空间，异常（输出）
        // *返    回：返回图层树XML
        private XmlDocument GetAllPriXml(IWorkspace pWs, out Exception outError)
        {
            outError = null;
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(pWs);
            //读取数据库图层数表格，并将图层书xml进行解析
            object objXml = sysTable.GetFieldValue("LAYERTREE_XML", "LAYERTREE", "NAME='LAYERTREE'", out outError);
            if (outError != null)
            {
                outError = new Exception("读取图层树xml表格‘LAYERTREE_XML’信息出错，\n原因：" + outError.Message);
                return null; ;
            }
            if (objXml == null)
            {
                outError = new Exception("获取图层树xml文件失败！");
                return null;
            }
            //获取图层树xml文档
            XmlDocument pXml = objXml as XmlDocument;
            return pXml;
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：根据将数据库相关信息存储到xml中
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-24
        // *参    数：系统维护库工作空间，数据权限信息（数据源ID，数据集名称，图层名），异常（输出）
        private void SaveInfoToXml(IWorkspace pWs, Dictionary<string, Dictionary<string, string>> dicDataPriInfo, out Exception outError)
        {
            outError = null;
            string proName = "";       //项目名称
            string dbFormatID = "";    //数据库格式:PDB\GDB\SDE
            string connectionInfo = "";//数据库链接信息
            string pDBPara = "";       //数据库的相关参数
            string dbTypeID = "";      //数据库类型：框架要素库、文件库、高程库、影像库、。。。
            string scales = "";         //数据集比例尺
            string pDtNames = "";       //数据集名称，以逗号隔开

            string pServer = "";       //服务器
            string pInstance = "";     //服务名
            string pDB = "";           //数据库
            string pUser = "";         //用户
            string pPassword = "";     //密码
            string pVersion = "";      //版本

            outError = null;
            IFeatureWorkspace pFeaWS = pWs as IFeatureWorkspace;
            if (pFeaWS == null)
            {
                outError = new Exception("获取系统维护库工作空间失败！");
                return;
            }
            //cyf 20110626 add:若存在工程树图xml,则删除xml
            if (File.Exists(ModData.v_projectDetalXML))
            {
                File.Delete(ModData.v_projectDetalXML);
            }
            //end

            //遍历数据权限信息，将数据权限信息存储在xml中
            foreach (KeyValuePair<string, Dictionary<string, string>> dataPriInfo in dicDataPriInfo)
            {
                string pDBSourceID = dataPriInfo.Key; ;  //数据源ID
                Dictionary<string, string> dicFeaDTInfo = new Dictionary<string, string>();   //数据集信息
                dicFeaDTInfo = dataPriInfo.Value;
                XmlNode pContentNode = null;             //内容
                //将数据源以及连接信息存储在xml中
                #region 将数据源以及连接信息存储在xml中
                //查询目录权限表
                string pFieldNames = "ID,DATABASENAME,DATABASETYPEID,DATAFORMATID,SCALE,CONNECTIONINFO,DBPARA";
                //执行查询
                ICursor pCursor = ModDBOperator.GetCursor(pFeaWS, "DATABASEMD", pFieldNames, "ID=" + pDBSourceID, out outError);
                if (pCursor == null || outError != null)
                {
                    return;
                }
                IRow pRow = pCursor.NextRow();//查询到的行
                if (pRow == null)
                {
                    outError = new Exception("查询数据源项目信息表失败！");
                    return;
                }
                proName = pRow.get_Value(1).ToString().Trim();
                dbTypeID = pRow.get_Value(2).ToString().Trim();
                dbFormatID = pRow.get_Value(3).ToString().Trim();
                scales= pRow.get_Value(4).ToString().Trim();
                connectionInfo = pRow.get_Value(5).ToString().Trim();
                pDBPara = pRow.get_Value(6).ToString().Trim();
                #region 获取数据库连接信息
                //获得数据库连接参数
                string[] strArr = connectionInfo.Split(new char[] { '|' });
                if (strArr.Length != 7)
                {
                    outError = new Exception("连接字符串设置有误！");
                    return;
                }
                pServer = strArr[0];
                pInstance = strArr[1];
                pDB = strArr[2];
                pUser = strArr[3];
                pPassword = strArr[4];
                pVersion = strArr[5];
                pDtNames = strArr[6];
                #endregion

                //加载xml文件
                if (!File.Exists(ModData.v_projectDetalXMLTemp))
                {
                    outError = new Exception("缺失模板文件,请检查!");
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(ModData.v_projectDetalXML))  //cyf  20110626 delete
                {
                    //若工程xml文件不存在，则创建xml文件
                    xmlDoc.LoadXml("<工程管理></工程管理>");
                    xmlDoc.Save(ModData.v_projectDetalXML);
                }
                xmlDoc.Load(ModData.v_projectDetalXML);

                #region 设置工程节点属性
                //若存在当前工程节点，则移除当前节点
                XmlNode ProNode = null;//当前工程节点
                try { ProNode = xmlDoc.DocumentElement.SelectSingleNode(".//工程[@编号='" + pDBSourceID + "']"); }
                catch { }
                if (ProNode != null)
                {
                    //若该工程节点已经存在，则将该节点删除
                    xmlDoc.DocumentElement.RemoveChild(ProNode);
                    xmlDoc.Save(ModData.v_projectDetalXML);
                }

                //创建工程管理节点下面的子节点 “工程”节点
                XmlElement proElement = xmlDoc.CreateElement("工程");//SelectNode(xmlDoc.DocumentElement, "工程") as XmlElement;//
                xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

                //加载工程模板xml文件
                XmlDocument xmlDocTemple = new XmlDocument();
                xmlDocTemple.Load(ModData.v_projectDetalXMLTemp);
                //获得xml模板文件中的“工程”节点
                XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
                //将模板文件中的“工程”节点引入到新创建的xml文件中
                XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
                //设置工程节点的属性信息
                (DBXmlNodeNew as XmlElement).SetAttribute("编号", pDBSourceID);
                (DBXmlNodeNew as XmlElement).SetAttribute("名称", proName);
                //用设置好的节点替换原有的节点
                xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);
                #endregion
                //内容节点
                pContentNode = DBXmlNodeNew.SelectSingleNode(".//内容");

                #region 设置数据库节点属性，依据不同的数据库类型，设置数据库是否可见
                if (dbTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                {
                    XmlNode RasterNode = pContentNode.SelectSingleNode(".//栅格数据库");
                    if (RasterNode != null)
                    {
                        XmlElement RasterElem = RasterNode as XmlElement;
                        if (RasterElem != null)
                        {
                            RasterElem.SetAttribute("是否显示", "false");
                        }
                    }
                }
                else if (dbTypeID == enumInterDBType.高程数据库.GetHashCode().ToString() || dbTypeID == enumInterDBType.影像数据库.GetHashCode().ToString())
                {
                    XmlNode mNode = pContentNode.SelectSingleNode(".//现势库");
                    if (mNode != null)
                    {
                        XmlElement mElem = mNode as XmlElement;
                        if (mElem != null)
                        {
                            mElem.SetAttribute("是否显示", "false");
                        }
                    }
                    mNode = pContentNode.SelectSingleNode(".//历史库");
                    if (mNode != null)
                    {
                        XmlElement mElem = mNode as XmlElement;
                        if (mElem != null)
                        {
                            mElem.SetAttribute("是否显示", "false");
                        }
                    }
                }

                #endregion

                #region 遍历数据库节点，设置连接信息节点属性，将数据库连接信息存储在XML中
                foreach (XmlNode subNode in pContentNode.ChildNodes)
                {
                    //cyf 20110628 modify :将图幅范围信息写入数据库
                    if (dbTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                    {
                        if (subNode.Name == "图幅工作库")
                        {
                            XmlElement rangElem = null;//范围信息节点
                            try { rangElem = subNode.SelectSingleNode(".//范围信息") as XmlElement; }
                            catch { }
                            if (rangElem != null)
                            {
                                //设置图幅范围信息属性
                                rangElem.SetAttribute("范围", pDBPara);
                            }
                            continue;
                        }
                    }
                    //end
                    string sVisible = (subNode as XmlElement).GetAttribute("是否显示");
                    if (sVisible == bool.FalseString.ToLower()) continue;
                    
                    XmlElement subElement = null;//连接信息节点
                    try { subElement = subNode.FirstChild as XmlElement; }
                    catch { }
                    if (subElement != null)
                    {
                        #region 设置连接信息节点属性
                        if (dbFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("类型", "PDB");
                            subElement.SetAttribute("数据库", pDB);

                        }
                        else if (dbFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("类型", "GDB");
                            subElement.SetAttribute("数据库", pDB);
                        }
                        else if (dbFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("类型", "SDE");
                            subElement.SetAttribute("服务器", pServer);
                            subElement.SetAttribute("服务名", pInstance);
                            subElement.SetAttribute("数据库", pDB);
                            subElement.SetAttribute("用户", pUser);
                            subElement.SetAttribute("密码", pPassword);
                            subElement.SetAttribute("版本", pVersion);
                        }
                        else if (dbFormatID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("类型", "Access");
                            subElement.SetAttribute("数据库", pDB);
                        }
                        else if (dbFormatID == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("类型", "Oracle");
                            //subElement.SetAttribute("服务器", pPassword);
                            subElement.SetAttribute("数据库", pDB);
                            subElement.SetAttribute("用户", pUser);
                            subElement.SetAttribute("密码", pPassword);
                        }
                        else if (dbFormatID == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("类型", "SQL Server");
                            subElement.SetAttribute("服务器", pServer);
                            subElement.SetAttribute("数据库", pDB);
                            subElement.SetAttribute("用户", pUser);
                            subElement.SetAttribute("密码", pPassword);
                        }
                        else if (dbFormatID == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
                        {
                            subElement.SetAttribute("服务器", pServer);
                            subElement.SetAttribute("用户", pUser);
                            subElement.SetAttribute("密码", pPassword);
                        }
                        #endregion
                    }
                }
                #endregion
                #endregion
                //遍历数据集信息,将数据集信息存储在xml中
                foreach (KeyValuePair<string, string> FeaDTInfo in dicFeaDTInfo)
                {
                    string pFeaDTName = FeaDTInfo.Key;       //数据集名称以“，”隔开
                    string pFeaClsNames = FeaDTInfo.Value;   //要素类名称，
                    //将数据集和要素类的信息存储在xml中
                   
                        //高程库和影像库没有数据集的概念，数据集可以为空
                        if (pDtNames.Trim() == "")
                        {
                            outError = new Exception("系统维护库中数据集名称信息或者比例尺信息缺失，\n工程ID为：" + pDBSourceID);
                            return;
                        } 
                        if (!pDtNames.Contains(pFeaDTName)) 
                        {
                            outError = new Exception("系统维护库中不存在名为：" + pFeaDTName + "的数据集，\n工程ID为：" + pDBSourceID);
                            continue;
                        }
                   
                    string[] feaArr = pDtNames.Split(',');       //数据集名称数组
                    string[] ScaleArr = scales.Split(',');       //比例尺数组
                 
                    if (dbTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                    {
                        //cyf 20110705 modify
                        //创建现势库及其子节点
                        CreateSubXmlNode(xmlDoc, pContentNode, "现势库", pFeaDTName, pFeaClsNames, feaArr, ScaleArr, out outError);
                        //创建临时库及其子节点
                        CreateSubXmlNode(xmlDoc, pContentNode, "临时库", pFeaDTName, pFeaClsNames, feaArr, ScaleArr, out outError);
                        //end
                        //创建历史库及其子节点
                        CreateSubXmlNode(xmlDoc, pContentNode, "历史库", pFeaDTName, pFeaClsNames, feaArr, ScaleArr, out outError);
                        //end
                    }
                    else if (dbTypeID == enumInterDBType.高程数据库.GetHashCode().ToString() || dbTypeID == enumInterDBType.影像数据库.GetHashCode().ToString())
                    {
                       //cyf 20110705 modify
                        //创建栅格数据库xml子节点并且设置子节点的属性
                        CreateRasterSubXmlNode(xmlDoc, pContentNode, pFeaDTName, pDBPara, feaArr, out outError);
                        //end
                    }
                }
                xmlDoc.Save(ModData.v_projectDetalXML);
            }
        }
        // *----------------------------------------------------------------------------------------
        // *功能函数：创建矢量数据库（现势库、历史库）xml子节点并且设置子节点的属性
        // *开 发 者：陈亚飞
        // *开发日期：2011-07-05
        // *参    数：xml文档、内容xml节点，库体xml节点名称，数据集节点名称属性，图层名节点名称属性，数据集名称数组，比例尺数组，异常
        private void CreateSubXmlNode(XmlDocument xmlDoc, XmlNode pContentNode, string xmlNodeName, string pFeaDTName, string pFeaClsNames,string[] feaArr, string[] ScaleArr,out Exception outError)
        {
            outError = null;
            XmlNode pDBNode = null;               //现势库节点
            XmlNode pFeaDTNode = null;            //数据集节点
            string pFeaDTTempName = "";          //数据集名称
            // 存储数据集节点的属性信息以及数据集节点子节点的相关信息
            #region 创建或获取数据库节点
            try
            {
                pDBNode = pContentNode.SelectSingleNode(".//" + xmlNodeName);     //现势库节点
            } catch
            {
                outError = new Exception("获取现势库节点失败！");
                return;
            }
            //cyf 20110705 add
            if (pDBNode == null)
            {
                //若数据库不存在，则创建数据库子节点
                XmlElement dbElem = xmlDoc.CreateElement(xmlNodeName);
                pDBNode = dbElem as XmlNode;
                pContentNode.AppendChild(pDBNode);
            }
            //end
            #endregion

            if (xmlNodeName == "现势库")
            {
                pFeaDTTempName = pFeaDTName;
            }
            else if (xmlNodeName == "历史库")
            {
                pFeaDTTempName = pFeaDTName + "_GOH";
            }
            else if (xmlNodeName == "临时库")
            {
                pFeaDTTempName = pFeaDTName + "_GOT";
            }
            #region 创建或获取数据集节点
            try
            {
                pFeaDTNode = pDBNode.SelectSingleNode(".//数据集[@名称='" + pFeaDTTempName + "']");
            } catch { }
            if (pFeaDTNode == null)
            {
                //若该数据集节点不存在，
                //创建现势库下面的子节点 “数据集”节点
                XmlElement FeaDTElement = xmlDoc.CreateElement("数据集");
                pFeaDTNode = FeaDTElement as XmlNode;
                pDBNode.AppendChild(pFeaDTNode);
            }
            #endregion

            #region 设置数据集的属性信息
            //设置数据库节点的“存储类型”属性
            (pFeaDTNode as XmlElement).SetAttribute("名称", pFeaDTTempName);
            string pScale = "";        //当前比例尺信息
            if (feaArr.Length == ScaleArr.Length)
            {
                for (int i = 0; i < feaArr.Length; i++)
                {
                    if (feaArr[i] == pFeaDTName)
                    {
                        pScale = ScaleArr[i];
                        break;
                    }
                }
            }
            (pFeaDTNode as XmlElement).SetAttribute("比例尺", pScale);
            #endregion

            #region 创建或获取图层名节点
            XmlNode feaclsNode = null;        //图层名节点
            try { feaclsNode = pFeaDTNode.SelectSingleNode(".//图层名"); } catch { }
            if (feaclsNode == null)
            {
                //cyf 20110625 modify
                //创建数据集下面的图层名节点
                XmlElement FeaClsElement = xmlDoc.CreateElement("图层名");
                feaclsNode = FeaClsElement as XmlNode;
                pFeaDTNode.AppendChild(feaclsNode);
                //end
            }
            #endregion

            #region 设置图层名节点属性
            XmlElement feaclsElem = feaclsNode as XmlElement;
            if (feaclsElem == null) return;
            if (xmlNodeName == "现势库")
            {
                //现势库
                feaclsElem.SetAttribute("名称", pFeaClsNames);
            }
            else if (xmlNodeName == "历史库")
            {
                //历史库
                string pHisFeaClsNames = "";   //历史库图层名
                string[] feaclsArr = pFeaClsNames.Split(',');
                for (int i = 0; i < feaclsArr.Length; i++)
                {
                    pHisFeaClsNames +=feaclsArr[i] + "_GOH,";
                }
                if (pHisFeaClsNames.Trim() != "")
                {
                    pHisFeaClsNames = pHisFeaClsNames.Substring(0, pHisFeaClsNames.Length - 1);
                }
                feaclsElem.SetAttribute("名称", pHisFeaClsNames);
            }
            else if (xmlNodeName == "临时库")
            {
                //临时库
                string pTmpFeaClsNames = "";   //临时库图层名
                string[] feaclsArr = pFeaClsNames.Split(',');
                for (int i = 0; i < feaclsArr.Length; i++)
                {
                    pTmpFeaClsNames += feaclsArr[i] + "_GOT,";
                }
                if (pTmpFeaClsNames.Trim() != "")
                {
                    pTmpFeaClsNames = pTmpFeaClsNames.Substring(0, pTmpFeaClsNames.Length - 1);
                }
                feaclsElem.SetAttribute("名称", pTmpFeaClsNames);
            }
            #endregion
        }
        // *----------------------------------------------------------------------------------------
        // *功能函数：创建栅格数据库xml子节点并且设置子节点的属性
        // *开 发 者：陈亚飞
        // *开发日期：2011-07-05
        // *参    数：xml文档、内容xml节点，数据集节点名称属性，栅格参数信息，数据集名称数组，异常
        private void CreateRasterSubXmlNode(XmlDocument xmlDoc, XmlNode pContentNode, string pFeaDTName, string pDBPara, string[] feaArr, out Exception outError)
        {
            outError = null;
            XmlNode pRasterDBNode = null;            //栅格数据库节点
            XmlNode pRasterFeaDTNode = null;            //图层节点
            string dbtypeStr = "";        //存储类型
            string pResampleStr = "";       //重采样方法
            string pCompressionStr = "";  //压缩类型
            string pPyramidStr = "";      //金字塔
            string pTileHStr = "";        //瓦片高度
            string pTileWStr = "";        //瓦片宽度
            string pBandStr = "";         //波段

            // 存储图层点的属性信息以及图层节点子节点的相关信息
            #region 创建或获取数据库节点
            try
            {
                pRasterDBNode = pContentNode.SelectSingleNode(".//栅格数据库");     //现势库节点
            } catch
            {
                outError = new Exception("获取栅格数据库节点失败！");
                return;
            }
            //cyf 20110705 add
            if (pRasterDBNode == null)
            {
                //若数据库不存在，则创建数据库子节点
                XmlElement dbElem = xmlDoc.CreateElement("栅格数据库");
                pRasterDBNode = dbElem as XmlNode;
                pContentNode.AppendChild(pRasterDBNode);
            }
            //end
            #endregion
            #region 创建或获取图层节点
            try
            {
                pRasterFeaDTNode = pRasterDBNode.SelectSingleNode(".//图层[@名称='" + pFeaDTName + "']");
            } catch { }
            if (pRasterFeaDTNode == null)
            {
                //若该数据集节点不存在，
                //创建现势库下面的子节点 “数据集”节点
                XmlElement RasterFeaElement = xmlDoc.CreateElement("图层");
                pRasterFeaDTNode = RasterFeaElement as XmlNode;
                pRasterDBNode.AppendChild(pRasterFeaDTNode);
            }
            #endregion

            #region 获得栅格数据库参数
            if (pDBPara != "")
            {
                //cyf 20110629 modify
                string[] pDTPara = pDBPara.Split(',');
                for (int i = 0; i < pDTPara.Length; i++)
                {
                    if (feaArr[i] == pFeaDTName)
                    {
                        string[] paraArr = pDTPara[i].Split(new char[] { '|' });
                        if (paraArr.Length != 7)
                        {
                            outError = new Exception("栅格数据库参数设置有误！");
                            return;
                        }
                        dbtypeStr = paraArr[0];
                        pResampleStr = paraArr[1];
                        pCompressionStr = paraArr[2];
                        pPyramidStr = paraArr[3];
                        pTileHStr = paraArr[4];
                        pTileWStr = paraArr[5];
                        pBandStr = paraArr[6];
                    }
                }
            }
            #endregion

            #region 设置图层属性
            //设置数据库节点的“存储类型”属性
            (pRasterFeaDTNode as XmlElement).SetAttribute("名称", pFeaDTName);
            (pRasterFeaDTNode as XmlElement).SetAttribute("存储类型", dbtypeStr);

            //栅格数据参数节点
            XmlNode rasterParaNode = null;      //栅格参数设置节点
            try { rasterParaNode = pRasterFeaDTNode.SelectSingleNode(".//栅格参数设置"); } catch { }
            if (rasterParaNode == null)
            {
                //若该栅格参数设置节点不存在，则创建栅格参数设置节点
                XmlElement RasterParaElement = xmlDoc.CreateElement("栅格参数设置");
                rasterParaNode = RasterParaElement as XmlNode;
                pRasterFeaDTNode.AppendChild(rasterParaNode);
            }
            XmlElement rasterParaEle = rasterParaNode as XmlElement;
            if (rasterParaEle == null) return;
            //设置栅格数据参数
            rasterParaEle.SetAttribute("重采样类型", pResampleStr);
            rasterParaEle.SetAttribute("压缩类型", pCompressionStr);
            rasterParaEle.SetAttribute("金字塔", pPyramidStr);
            rasterParaEle.SetAttribute("瓦片高度", pTileHStr);
            rasterParaEle.SetAttribute("瓦片宽度", pTileWStr);
            rasterParaEle.SetAttribute("波段", pBandStr);
            #endregion
        }

        // *----------------------------------------------------------------------------------------
        // *功能函数：根据用户的角色权限读取数据源目录，并刷新xml
        // *开 发 者：陈亚飞
        // *开发日期：2011-06-12
        // *参    数：当前用户对应的角色列表，返回异常信息
        private void SaveAndRefreshXml(List<Role> LstRole, out Exception outError)
        {
            outError = null;
            IWorkspace pWs = null;      //系统维护库工作空间
            //获取系统维护库工作空间
            pWs = GetSysDBWs(out outError);
            if (outError != null || pWs == null)
            {
                return;
            }
            string pRoleIDWhere = "";  //用户所对应的所有的角色ID
            //遍历角色信息，加载每一个角色的数据源目录
            foreach (Role pRole in LstRole)
            {
                string pRoleID = pRole.IDStr;    //角色ID
                if (pRoleID.Trim() != "")
                {
                    pRoleIDWhere += "'" + pRoleID + "',";
                }
            }
            if (pRoleIDWhere.Trim() == "")
            {
                outError = new Exception("获取当前登录用户对应的角色信息失败！");
                return;
            }

            pRoleIDWhere = "(" + pRoleIDWhere.Substring(0, pRoleIDWhere.Length - 1) + ")";
            pRoleIDWhere = "ROLEID in " + pRoleIDWhere;

            IFeatureWorkspace pFeaWs = pWs as IFeatureWorkspace;
            if (pFeaWs == null)
            {
                outError = new Exception("系统能够维护库连接失败，请检查!");
                return;
            }
            //ITable pTable = pFeaWs.OpenTable("ROLE_DBSOURCE"); //目录权限表
            //查询目录权限表
            IQueryDef pQueryDef = pFeaWs.CreateQueryDef();
            pQueryDef.Tables = "ROLE_DBSOURCE,DATABASEMD";
            pQueryDef.SubFields = "DATABASEMD.ID,DATABASEMD.DATABASENAME,DATABASEMD.DATABASETYPEID,DATABASEMD.DATAFORMATID,DATABASEMD.SCALE,DATABASEMD.CONNECTIONINFO,DATABASEMD.DBPARA";
            pQueryDef.WhereClause = "ROLE_DBSOURCE.DBSOURCE_ID=DATABASEMD.ID and " + "ROLE_DBSOURCE." + pRoleIDWhere;
            //执行查询
            ICursor pCursor = null;
            try
            {
                pCursor = pQueryDef.Evaluate();
                if (pCursor == null)
                {
                    outError = new Exception("查询目录权限表失败，请检查!");
                    return;
                }
            }
            catch (Exception ex)
            {
                outError = new Exception("查询目录权限表失败，请检查:" + ex.Message);
                return;
            }
            IRow pRow = pCursor.NextRow();//查询到的行
            //cyf 20110613 modify
            if (File.Exists(ModData.v_projectXML))
            {
                File.Delete(ModData.v_projectXML);
            }
            //遍历查询到的行，将每一行的记录写入到XML中
            while (pRow != null)
            {
                string proID = "";         //项目ID
                string proName = "";       //项目名称
                string dbFormatID = "";    //数据库格式:PDB\GDB\SDE
                string scale = "";         //数据库比例尺
                string connectionInfo = "";//数据库链接信息
                string pDBPara = "";       //数据库的相关参数
                string dbTypeID = "";      //数据库类型：框架要素库、文件库、高程库、影像库、。。。
                proID = pRow.get_Value(0).ToString().Trim();
                proName = pRow.get_Value(1).ToString().Trim();
                dbTypeID = pRow.get_Value(2).ToString().Trim();
                dbFormatID = pRow.get_Value(3).ToString().Trim();
                scale = pRow.get_Value(4).ToString().Trim();
                connectionInfo = pRow.get_Value(5).ToString().Trim();
                pDBPara = pRow.get_Value(6).ToString().Trim();
                //将相关信息写到XML里面,刷新xml
                if (dbTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                {
                    //刷新框架要素库xml
                    ModDBOperator.RefeshFeaXml(ModData.v_projectXML, ModData.v_projectXMLTemp, proID, proName, scale, dbFormatID, connectionInfo, pDBPara, out outError);
                }
                else if (dbTypeID == enumInterDBType.高程数据库.GetHashCode().ToString() || dbTypeID == enumInterDBType.影像数据库.GetHashCode().ToString())
                {
                    //刷新高程、影像XML文件
                    ModDBOperator.RefeshRasterXml(ModData.v_projectXML, ModData.v_projectXMLTemp, proID, proName, scale, dbFormatID, connectionInfo, pDBPara, out outError);
                }
                pRow = pCursor.NextRow();
            }

            //cyf 20110613 释放游标
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        //测试链接信息是否可用
        private bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
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
    }
}
