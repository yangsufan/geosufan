using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;
using Plugin.Interface;
using Plugin.Application;
using Plugin.Parse;

namespace Plugin
{
    #region 从XML读取解析插件UI化,并与插件实现进行绑定
    public static class ModuleCommon
    {
        public static event SysCommon.SysLogInfoChangedHandle SysLogInfoChnaged;
        #region 插件对象集合
        private static Dictionary<string, IPlugin> v_dicPlugins;
        private static Dictionary<string, ICommandRef> v_dicCommands;
        public static Dictionary<string, ICommandRef> DicCommands
        {
            get { return v_dicCommands; }
        }
        private static Dictionary<string, IToolRef> v_dicTools;
        private static Dictionary<string, IToolBarRef> v_dicToolBars;
        private static Dictionary<string, IMenuRef> v_dicMenus;
        private static Dictionary<string, IDockableWindowRef> v_dicDockableWindows;
        private static Dictionary<string, IControlRef> v_dicControls;
        public static Dictionary<string, IControlRef> DicControls
        {
            get
            {
                return v_dicControls;
            }
            set
            {
                v_dicControls = value;
            }
        }
        #endregion
        private static XmlDocument _SysXmlDocument;
        private static string _ResPath;
        private static List<string> _ListUserPrivilegeID;
        public static List<string> ListUserPrivilegeID
        {
            get { return _ListUserPrivilegeID; }
            set { _ListUserPrivilegeID = value; }
        }
        private static List<string> _ListUserdataPriID;
        public static List<string> ListUserdataPriID
        {
            get { return _ListUserdataPriID; }
            set { _ListUserdataPriID = value; }
        }
        private static SysCommon.User _AppUser;
        public static SysCommon.User AppUser
        {
            get { return _AppUser; }
            set { _AppUser = value; }
        }
        private static ESRI.ArcGIS.Geodatabase.IWorkspace _TmpWks ;
        public static ESRI.ArcGIS.Geodatabase.IWorkspace TmpWorkSpace
        {
            get { return _TmpWks; }
            set { _TmpWks = value; }
        }

        //刷新按钮Enable属性
        private static System.Windows.Forms.Timer _Timer = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer RefreshTimer
        {
            get { return _Timer;}
        }
        //插件与界面上对应关系
        private static Dictionary<DevExpress.XtraBars.BarItem, string> _dicBaseItems = new Dictionary<DevExpress.XtraBars.BarItem, string>();
        public static Dictionary<DevExpress.XtraBars.BarItem, string> DicBaseItems
        {
            get { return _dicBaseItems; }
        }

        //记录插件加载过程日志
        private static SysCommon.Log.SysLocalLog _SysLocalLog;
        
        //将tab按照系统分类
        private static Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> _dicTabs = new Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string>();

        public static Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs
        {
            get 
            {
                return _dicTabs;
            }
            set 
            {
                _dicTabs=value;
            }
        }

        //主应用程序APP
        private static Plugin.Application.IAppFormRef _pIAppFrm;

        public static Plugin.Application.IAppFormRef AppFrm
        {
            get
            {
                return _pIAppFrm;
            }
            set
            {
                _pIAppFrm = value;
            }
        }
        //全局记录当前选择的按钮-----应用功能高亮显示
        private static DevExpress.XtraBars.BarButtonItem m_pBaseItem = null;
        //初始化
        public static void IntialModuleCommon(List<string> ListUserPrivilegeID, XmlDocument aXmlDocument, string strResPath, PluginCollection pluginCol, string strLogPath)
        {
            _SysXmlDocument = aXmlDocument;
            _ResPath = strResPath;
            _SysLocalLog = new SysCommon.Log.SysLocalLog(strLogPath);
            _SysLocalLog.CreateLogFile("系统加载插件日志.txt");

            _ListUserPrivilegeID = ListUserPrivilegeID;

            //分类解析插件
            SysCommon.ModSysSetting.WriteLog("分类解析插件"); //@日志测试
            try
            {
                ParsePlugins(pluginCol);
            }
            catch (Exception err)
            {
                SysCommon.ModSysSetting.WriteLog("分类解析插件错误，信息："+err.Message); //@日志测试
            }
            SysCommon.ModSysSetting.WriteLog("分类解析插件结束"); //@日志测试
            //实时刷新命令按钮Enable属性
            _Timer.Interval = 500;
            _Timer.Enabled = true;
            _Timer.Tick+=new EventHandler(Timer_Tick);
        }

        //初始化  added by chulili 20110601 ListUserPrivilegeID在sysmain里面传入一次就可以
        public static void IntialModuleCommon( XmlDocument aXmlDocument, string strResPath, PluginCollection pluginCol, string strLogPath)
        {
            _SysXmlDocument = aXmlDocument;
            _ResPath = strResPath;
            _SysLocalLog = new SysCommon.Log.SysLocalLog(strLogPath);
            _SysLocalLog.CreateLogFile("系统加载插件日志.txt");

            //_ListUserPrivilegeID = ListUserPrivilegeID;

            //分类解析插件
            ParsePlugins(pluginCol);

            //实时刷新命令按钮Enable属性
            _Timer.Interval = 500;
            _Timer.Enabled = true;
            _Timer.Tick += new EventHandler(Timer_Tick);
        }

        /// <summary>
        /// 根据XML初始化主窗体
        /// </summary>
        /// <param name="pApplication"></param>
        /// <returns></returns>
        public static bool LoadFormByXmlNode(IApplicationRef pApplication)
        {
            if (_SysXmlDocument == null || v_dicPlugins == null || pApplication == null) return false;

            _pIAppFrm = pApplication as Plugin.Application.IAppFormRef;
            //根据XML内容进行插件事件绑定
            LoadEventsByXmlNode();

            //根据XML加载系统界面
            return LoadControlsByXmlNode(pApplication);

        }
        public static bool LoadData(IApplicationRef pApplication)
        {
            if (_SysXmlDocument == null || v_dicPlugins == null || pApplication == null) return false;

            _pIAppFrm = pApplication as Plugin.Application.IAppFormRef;

            return LoadDataByXmlNode(pApplication);

        }

        /// <summary>
        /// 根据XML加载系统界面
        /// </summary>
        private static bool LoadControlsByXmlNode(IApplicationRef pApplication)
        {         
            IAppFormRef pAppFormRef = pApplication as IAppFormRef;
            if (pAppFormRef == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "AppFormRef未设置");
                return false;
            }
            if (pAppFormRef.MainForm == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "AppFormRef中变量MainForm未设置");
                return false;
            }
            
            //修改窗体标题
            string xPath = "//Main";
            XmlNode xmlnode = _SysXmlDocument.SelectSingleNode(xPath);
            if (xmlnode == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "XML中未找到Main节点");
                return false;
            }
            XmlElement aXmlElement = xmlnode as XmlElement;
            if (aXmlElement.HasAttribute("Caption"))
            {
                pAppFormRef.Caption = aXmlElement.GetAttribute("Caption");
            }

            //创建RibbonControl
            pAppFormRef.MainForm.Size = new System.Drawing.Size(1028, 742);
            DevExpress.XtraBars.Ribbon.RibbonControl aRibbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
            aRibbonControl.Dock = System.Windows.Forms.DockStyle.Top;
            aRibbonControl.Location = new System.Drawing.Point(4, 1);
            aRibbonControl.Name = "ribbonControlMain";
           // aRibbonControl.Size = new System.Drawing.Size(1028, 102);
            aRibbonControl.Size = new System.Drawing.Size(1028, 140);
            pAppFormRef.ControlContainer = aRibbonControl as Control;

            //创建StartButton
            DevExpress.XtraBars.Ribbon.ApplicationMenu aStartButton = new DevExpress.XtraBars.Ribbon.ApplicationMenu();


            aStartButton.Name = "buttonSystems";



            _SysLocalLog.WriteLocalLog("完成系统主界面初始化");
            if (xmlnode.HasChildNodes == false)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "XML中无相关插件内容");
                return false;
            }
            //右键菜单集合
            Dictionary<string, DevExpress.Utils.ContextButton> dicContextMenu = new Dictionary<string, DevExpress.Utils.ContextButton>();
            string sNodeName = "";
            string sNodeID = "";
            string sNodeCaption = "";
            string sControlType = "";
            string sVisible = "";
            string sEnabled = "";
            string sBackgroudLoad = "";

            //初始化加载控件
            bool bRes = true;

            foreach (XmlNode xmlnodeChild in xmlnode.ChildNodes)
            {
                try
                {
                    XmlElement xmlelementChild = xmlnodeChild as XmlElement;
                    sNodeName = xmlelementChild.GetAttribute("Name").Trim();
                    sNodeID = xmlelementChild.GetAttribute("ID").Trim();
                    sNodeCaption = xmlelementChild.GetAttribute("Caption").Trim();
                    sControlType = xmlelementChild.GetAttribute("ControlType").Trim();
                    sVisible = xmlelementChild.GetAttribute("Visible").Trim();
                    sEnabled = xmlelementChild.GetAttribute("Enabled").Trim();
                    sBackgroudLoad = xmlelementChild.GetAttribute("BackgroudLoad").Trim();  //wgf 20110602 支持System节点后台添加
                    sNodeID = "";
                    if (xmlelementChild.GetAttribute("ID") != null) sNodeID = xmlelementChild.GetAttribute("ID").Trim();

                    if (pAppFormRef.ConnUser.UserCode.ToLower() != "admin")
                    {
                        if (sNodeID == "") continue;
                        if (!_ListUserPrivilegeID.Contains(sNodeID)) continue;
                    }

                    if (sVisible == bool.FalseString.ToLower())  //获取子系统是否加载 对应Visible
                        continue;
                    _SysLocalLog.WriteLocalLog("***************************************");
                    _SysLocalLog.WriteLocalLog("开始加载" + sNodeName);

                    SysCommon.SysLogInfoChangedEvent newEvent = new SysCommon.SysLogInfoChangedEvent("加载" + sNodeCaption + "...");
                    SysLogInfoChnaged(null, newEvent);

                    if (sControlType == "UserControl")
                    {
                        #region 根据XML初始化快捷菜单按钮menuSystemItems子项
                        DevExpress.XtraBars.BarButtonItem aSysItem = new DevExpress.XtraBars.BarButtonItem();
                        aSysItem.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
                        if (File.Exists(_ResPath + "\\" + sNodeName + ".png"))
                        {
                            aSysItem.ImageUri = new DevExpress.Utils.DxImageUri(_ResPath + "\\" + sNodeName + ".png");
                        }
                        aSysItem.Name = sNodeName;
                        aSysItem.Caption = sNodeCaption;
                        if (sBackgroudLoad != bool.FalseString.ToLower())
                        {

                            //快捷菜单按钮menuSystemItems的click方法
                            aSysItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(menuSystemItem_Click);
                            #endregion

                            //判断插件中是否存在该项
                            if (v_dicPlugins.ContainsKey(sNodeName))
                            {
                                //根据IControlRef初始化加载用户自定义控件内容
                                PluginOnCreate(v_dicPlugins[sNodeName], pApplication);
                            }
                            else
                            {
                                bRes = false;
                                _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未找到对应的插件");
                                _SysLocalLog.WriteLocalLog("完成加载" + sNodeName);
                                _SysLocalLog.WriteLocalLog("***************************************");
                                continue;
                            }
                        }
                        _SysLocalLog.WriteLocalLog("完成加载" + sNodeName);
                        _SysLocalLog.WriteLocalLog("***************************************");

                    }
                }
                catch (Exception e)
                {
                    bRes = false;
                    _SysLocalLog.WriteLocalLog("出错：" + e.Message);
                }
            }

            //添加工具、状态栏
            pAppFormRef.MainForm.Controls.Add(aRibbonControl);



            _SysLocalLog.LogClose();
            return bRes;
        }
        /// <summary>
        /// 根据XML加载系统界面
        /// </summary>
        private static bool LoadDataByXmlNode(IApplicationRef pApplication)
        {
            IAppFormRef pAppFormRef = pApplication as IAppFormRef;
            if (pAppFormRef == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "AppFormRef未设置");
                return false;
            }
            if (pAppFormRef.MainForm == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "AppFormRef中变量MainForm未设置");
                return false;
            }
            //修改窗体标题
            string xPath = "//Main";
            XmlNode xmlnode = _SysXmlDocument.SelectSingleNode(xPath);
            if (xmlnode == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "XML中未找到Main节点");
                return false;
            }
           string sNodeName = "";
            string sControlType = "";
            string sVisible = "";

            //初始化加载控件
            bool bRes = true;

            foreach (XmlNode xmlnodeChild in xmlnode.ChildNodes)
            {
                try
                {
                    XmlElement xmlelementChild = xmlnodeChild as XmlElement;
                    sNodeName = xmlelementChild.GetAttribute("Name").Trim();
                    sControlType = xmlelementChild.GetAttribute("ControlType").Trim();
                    if (sVisible == bool.FalseString.ToLower())  //获取子系统是否加载 对应Visible
                        continue;
                    _SysLocalLog.WriteLocalLog("***************************************");
                    _SysLocalLog.WriteLocalLog("开始加载" + sNodeName);
                    if (sControlType == "UserControl")
                    {
                        //判断插件中是否存在该项
                        if (v_dicPlugins.ContainsKey(sNodeName))
                        {
                            //根据IControlRef初始化加载用户自定义控件内容
                            PluginOnLoadData(v_dicPlugins[sNodeName], pApplication);
                        }
                        else
                        {
                            bRes = false;
                            _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未找到对应的插件");
                            _SysLocalLog.WriteLocalLog("完成加载" + sNodeName);
                            _SysLocalLog.WriteLocalLog("***************************************");
                            continue;
                        }
                    }
                   
                    _SysLocalLog.WriteLocalLog("完成加载" + sNodeName);
                    _SysLocalLog.WriteLocalLog("***************************************");

                }
                catch (Exception e)
                {
                    bRes = false;
                    _SysLocalLog.WriteLocalLog("出错：" + e.Message);
                }
            }
            _SysLocalLog.LogClose();
            return bRes;
        }

        /// <summary>
        /// 匹配所有的Xml节点(菜单栏、工具栏、右键菜单)来构造界面
        /// </summary>
        public static bool LoadButtonViewByXmlNode(Control aControl, string xPath, IApplicationRef pApplication)
        {
            if (aControl == null || xPath == string.Empty || _SysXmlDocument == null || v_dicPlugins == null || pApplication == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "存在未设置的变量");
                return false;
            }

            DevExpress.XtraBars.Ribbon.RibbonControl aRibbonControl = aControl as DevExpress.XtraBars.Ribbon.RibbonControl;
            if (aRibbonControl == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "控件容器类型不是RibbonControl");
                return false;
            }

            XmlNode xmlnode = _SysXmlDocument.SelectSingleNode(xPath);
            if (xmlnode == null)
            {
                _SysLocalLog.WriteLocalLog("异常：" + "XML中未找到" + xPath + "节点");
                return false;
            }

            XmlElement XmlElementSys=xmlnode as XmlElement;
            string strNameSys=XmlElementSys.GetAttribute("Name");

            if (xmlnode.HasChildNodes == false)
            {
                _SysLocalLog.WriteLocalLog("异常：" + xPath + "节点下无相关内容");
                return false;
            }
            aRibbonControl.Height = 120;        //added by chulili 限制整栏菜单的高度
            //右键菜单集合
            Dictionary<string, DevExpress.XtraBars.PopupMenu> dicContextMenu = new Dictionary<string, DevExpress.XtraBars.PopupMenu>();
            IAppFormRef pAppFormRef = pApplication as IAppFormRef;
            bool bRes = true;
            foreach (XmlNode xmlnodeChild in xmlnode.ChildNodes)
            {                    
                try
                {
                    XmlElement xmlelementChild = xmlnodeChild as XmlElement;
                    
                    string sNodeName = xmlelementChild.GetAttribute("Name").Trim();
                    string sNodeID = xmlelementChild.GetAttribute("ID").Trim();
                    string sNodeCaption = xmlelementChild.GetAttribute("Caption").Trim();
                    string sControlType = xmlelementChild.GetAttribute("ControlType").Trim();
                    string sVisible = xmlelementChild.GetAttribute("Visible").Trim();
                    string sEnabled = xmlelementChild.GetAttribute("Enabled").Trim();
                    if (pAppFormRef.ConnUser.UserCode.ToLower() != "admin")
                    {
                        if (sNodeID == "")
                        {
                            continue;
                        }
                        if (!_ListUserPrivilegeID.Contains(sNodeID)) continue;
                    }
                    SysCommon.SysLogInfoChangedEvent newEvent = new SysCommon.SysLogInfoChangedEvent("加载" + sNodeCaption + "...");
                    SysLogInfoChnaged(null, newEvent);

                    if (sControlType == "RibbonTabItem")   //菜单类型节点  根据XML中xmlnodeChild相关属性UI化界面Tab
                    {
                        DevExpress.XtraBars.Ribbon.RibbonPage aRibbonPanel = new DevExpress.XtraBars.Ribbon.RibbonPage();
                        aRibbonPanel.Name = "RibbonPanel" + sNodeName;

                        DevExpress.XtraBars.Ribbon.RibbonPage aRibbonTabItem = new DevExpress.XtraBars.Ribbon.RibbonPage();
                        aRibbonTabItem.Name = sNodeName;
                        aRibbonTabItem.Text = sNodeCaption;
                        aRibbonTabItem.Visible = Convert.ToBoolean(sVisible);
                        
                        //if (aRibbonControl.Controls.ContainsKey(aRibbonPanel.Name) || aRibbonControl.Items.Contains(aRibbonTabItem.Name))
                        //{
                        //    bRes = false;
                        //     _SysLocalLog.WriteLocalLog("异常：存在相同名称的RibbonTabItem节点");
                        //    continue;
                        //}
                        
                        //aRibbonControl.Controls.Add(aRibbonPanel);
                        //aRibbonControl.Items.Add(aRibbonTabItem);
                        //aRibbonControl.Expanded = true;

                        _dicTabs.Add(aRibbonTabItem, strNameSys);
                        //绑定菜单事件
                        //aRibbonTabItem.Click += new EventHandler(RibbonTabItem_Click);

                        foreach (XmlNode aXmlnode in xmlnodeChild.ChildNodes)
                        {
                            //if (LoadButtonView(aRibbonPanel, aXmlnode, pApplication, dicContextMenu) == null)
                            //{
                            //    bRes = false;
                            //}
                        }
                        continue;
                    }

                    //右键菜单
                    //if (LoadButtonView(null, xmlnodeChild, pApplication, dicContextMenu) == null)
                    //{
                    //    bRes = false;
                    //}
                }
                catch (Exception e)
                {
                    bRes = false;
                    _SysLocalLog.WriteLocalLog("出错：" + e.Message);
                }
            }

            //IAppFormRef pAppFormRef = pApplication as IAppFormRef;
            if (pAppFormRef != null)
            {
                //pAppFormRef.DicContextMenu = dicContextMenu;
            }
           

            return bRes;
        }
        /// <summary>
        /// 菜单栏、工具栏、右键菜单 不执行PluginOnCreate，通过以下代码直接UI化添加
        /// </summary>
        /// <param name="aControl"></param>
        /// <param name="xmlnodeChild"></param>
        /// <param name="pApplication"></param>
        /// <param name="dicContextMenu"></param>
        /// <returns></returns>
        public static Control LoadButtonView(Control aControl, XmlNode xmlnodeChild, IApplicationRef pApplication, Dictionary<string, System.Windows.Forms.ContextMenu> dicContextMenu)
        {
            string sNodeName = "";
            string sNodeID = "";
            string sNodeCaption = "";
            string sControlType = "";
            string sVisible = "";
            string sEnabled = "";
            string sDockStyle = "";

            XmlElement xmlelementChild = xmlnodeChild as XmlElement;
            IAppFormRef pAppFormRef = pApplication as IAppFormRef;
            sNodeName = xmlelementChild.GetAttribute("Name").Trim();
            sNodeID = xmlelementChild.GetAttribute("ID").Trim();
            sNodeCaption = xmlelementChild.GetAttribute("Caption").Trim();
            sControlType = xmlelementChild.GetAttribute("ControlType").Trim();
            sVisible = xmlelementChild.GetAttribute("Visible").Trim();
            sEnabled = xmlelementChild.GetAttribute("Enabled").Trim();
            if (xmlelementChild.HasAttribute("DockStyle"))
            {
                sDockStyle = xmlelementChild.GetAttribute("DockStyle").Trim();
            }
            if (pAppFormRef.ConnUser.UserCode.ToLower() != "admin")
            {
                if (sNodeID == "") return null;
                if (!_ListUserPrivilegeID.Contains(sNodeID)) return null;
            }
            if (sVisible == bool.FalseString.ToLower()) return null;

            SysCommon.SysLogInfoChangedEvent newEvent = new SysCommon.SysLogInfoChangedEvent("加载" + sNodeCaption + "...");
            SysLogInfoChnaged(null, newEvent);

            //实例化菜单栏、工具栏、右键菜单插件对应UI对象
            //string strType = "DevComponents.DotNetBar." + sControlType + ",DevComponents.DotNetBar2,Version=8.1.0.6,Culture=neutral,PublicKeyToken=null";
            string strType = "DevComponents.DotNetBar." + sControlType + ",DevComponents.DotNetBar2,Version=8.1.0.6";
           
            Type aType = null;
            try
            {
                aType = Type.GetType(strType);
            } catch
            {
                _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未能获取UI对象" + sControlType + "类型");
                return null;
            }
            if (aType == null)
            {
                _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未能获取UI对象" + sControlType + "类型");
                return null;
            }
            Object newObject = Activator.CreateInstance(aType);

            Control aBarControl = newObject as Control;
            if (aBarControl == null)
            {
                _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未能创建UI对象" + sControlType);
                return null;
            }
            aBarControl.Name = sNodeName;
            aBarControl.Text = sNodeCaption;

            aBarControl.Enabled = Convert.ToBoolean(sEnabled);
            aBarControl.Visible = Convert.ToBoolean(sVisible);

            if (sDockStyle != "")
            {
                switch(sDockStyle.ToLower())
                {
                    case "top":
                        aBarControl.Dock = System.Windows.Forms.DockStyle.Top;
                        break;
                    case "bottom":
                        aBarControl.Dock = System.Windows.Forms.DockStyle.Bottom;
                        break;
                    case "left":
                        aBarControl.Dock = System.Windows.Forms.DockStyle.Left;
                        break;
                    case "right":
                        aBarControl.Dock = System.Windows.Forms.DockStyle.Right;
                        break;
                    case "fill":
                        aBarControl.Dock = System.Windows.Forms.DockStyle.Fill;
                        break;
                    default:
                        aBarControl.Dock = System.Windows.Forms.DockStyle.None;
                        break;         
                }
            }

            bool bNotToolBar = false;
            if (sControlType == "ContextMenuBar")
                bNotToolBar = true;

            if (xmlnodeChild.HasChildNodes == false)
            {
                _SysLocalLog.WriteLocalLog("异常：" +sNodeName + "节点下无相关内容");
            }



            return aBarControl;
        }
        //<summary>
        //根据XML匹配添加菜单栏、工具栏、右键菜单等内容子项
        //</summary>
        private static bool AddItemsByXmlNode(object aControl, XmlNode xmlNodeCol, bool bImageAndText, IApplicationRef pApplication)
        {
            if (xmlNodeCol.HasChildNodes == false)
            {
                return false;
            }

            string sNodeNameImage = "";
            string sNodeName = "";
            string sNodeID = "";
            string sNodeCaption = "";
            string sControlType = "";
            string sVisible = "";
            string sEnabled = "";
            string sTips = "";
            string sNewGroup = "";
            string strWritelog = "";

            bool bRes = true;

            foreach (XmlNode aXmlnode in xmlNodeCol.ChildNodes)
            {
                try
                {
                    XmlElement xmlelementChild = aXmlnode as XmlElement;
                    IAppFormRef pAppFormRef = pApplication as IAppFormRef;
                    if(xmlelementChild.Name.ToLower()!="subitems") continue;

                    sNodeName = xmlelementChild.GetAttribute("Name").Trim();
                    sNodeID = xmlelementChild.GetAttribute("ID").Trim();
                    sNodeNameImage = sNodeName;
                    sNodeCaption = xmlelementChild.GetAttribute("Caption").Trim();
                    sControlType = xmlelementChild.GetAttribute("ControlType").Trim();
                    sVisible = xmlelementChild.GetAttribute("Visible").Trim();
                    sEnabled = xmlelementChild.GetAttribute("Enabled").Trim();
                    try
                    {
                        strWritelog = xmlelementChild.GetAttribute("WriteLog").Trim();
                    }
                    catch
                    { }
                    if(xmlelementChild.HasAttribute("Tips"))
                    {
                        sTips = xmlelementChild.GetAttribute("Tips").Trim();
                    }
                    if (pAppFormRef.ConnUser.UserCode.ToLower() != "admin")
                    {
                        if (sNodeID == "") continue;
                        if (!_ListUserPrivilegeID.Contains(sNodeID)) continue;
                    }
                    sNewGroup = xmlelementChild.GetAttribute("NewGroup").Trim();

                    if (sVisible == bool.FalseString.ToLower()) continue;
                    _SysLocalLog.WriteLocalLog("加载" + sNodeName);

                    SysCommon.SysLogInfoChangedEvent newEvent = new SysCommon.SysLogInfoChangedEvent("加载" + sNodeCaption + "...");
                    SysLogInfoChnaged(null, newEvent);

                    //判断插件中是否存在该项(注意：sNodeName中可能是组合字符串——代表要实例化插件用于避免冲突)
                    if (v_dicPlugins.ContainsKey(sNodeName))
                    {
                        // 初始化插件内容
                        PluginOnCreate(v_dicPlugins[sNodeName], pApplication);
                        PluginOnWriteLog(v_dicPlugins[sNodeName], strWritelog);
                    }
                    else
                    {
                        //实例化插件
                        bool bTemp = false;
                        foreach (string key in v_dicPlugins.Keys)
                        {
                            if (sNodeName.Contains(key))    //loged by chulili 20111214 @@重要 为什么用Contains不用Equal
                            {
                                Type aTypeTemp = v_dicPlugins[key].GetType();
                                IPlugin plugin = Activator.CreateInstance(aTypeTemp) as IPlugin;
                                if(plugin!=null)
                                {
                                    v_dicPlugins.Add(sNodeName, plugin);
                                }

                                ICommandRef cmd = plugin as ICommandRef;
                                if (cmd != null)
                                {
                                    v_dicCommands.Add(sNodeName, cmd);
                                }
                                IToolRef atool = plugin as IToolRef;
                                if (atool != null)
                                {
                                    v_dicTools.Add(sNodeName, atool);
                                }

                                // 初始化插件内容
                                PluginOnCreate(v_dicPlugins[sNodeName], pApplication);

                                bTemp = true;
                                sNodeNameImage = key;
                                break;
                            }
                        }

                        if (bTemp == false && aXmlnode.HasChildNodes == false)
                        {
                            bRes = false;
                            _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未找到对应的插件");
                            continue;
                        }
                    }

                    //string strType = "DevComponents.DotNetBar." + sControlType + ",DevComponents.DotNetBar2,Version=8.1.0.6,Culture=neutral,PublicKeyToken=null";
                    string strType = "DevComponents.DotNetBar." + sControlType + ",DevComponents.DotNetBar2,Version=8.1.0.6";
                    Type aType = null;
                    try
                    { aType = Type.GetType(strType); }
                    catch
                    { 
                         bRes = false;
                        _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未能获取UI对象" + sControlType + "类型");
                        continue;
                    }
                    if (aType == null)
                    {
                        bRes = false;
                        _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未能获取UI对象" + sControlType + "类型");
                        continue;
                    }
                    Object newObject = Activator.CreateInstance(aType);

                    DevExpress.XtraBars.BarItem aBaseItem = newObject as DevExpress.XtraBars.BarItem;
                    if (aBaseItem == null)
                    {
                        bRes = false;
                        _SysLocalLog.WriteLocalLog("加载" + sNodeName + "异常：" + "未能创建UI对象" + sControlType);
                        continue;
                    }

                    aBaseItem.Name = sNodeName;
                    aBaseItem.Enabled = Convert.ToBoolean(sEnabled);
                   

                    XmlElement xmlelementTips = (XmlElement)aXmlnode.SelectSingleNode(".//Tips");
                    if(xmlelementTips==null || sTips!="")
                    {
                    }
                    else if(xmlelementTips!=null)
                    {
                        string strbogyImage=xmlelementTips.GetAttribute("BodyImage");
                        string strfooterImage=xmlelementTips.GetAttribute("FooterImage");
                        Image bogyImage = null;
                        Image footerImage=null;
                        if(File.Exists(_ResPath + "\\"+strbogyImage+".png"))
                        {
                            bogyImage=Image.FromFile(_ResPath + "\\"+strbogyImage+".png");
                        }
                        if (File.Exists(_ResPath + "\\" + strfooterImage + ".png"))
                        {
                            footerImage = Image.FromFile(_ResPath + "\\" + strfooterImage + ".png");
                        }
                    }

                    //状态栏提示

                    _dicBaseItems.Add(aBaseItem, sNodeName);






                    
                   DevExpress.XtraBars.Bar aRibbonBar = aControl as DevExpress.XtraBars.Bar;
                    if (aRibbonBar != null)
                    {
                    }

                    DevExpress.XtraBars.BarButtonItem contextMemuButtonItem = aControl as DevExpress.XtraBars.BarButtonItem;
                    if (contextMemuButtonItem != null)
                    {
                    }


                    if (aXmlnode.HasChildNodes == true)
                    {
                        if (AddItemsByXmlNode(aBaseItem, aXmlnode, bImageAndText, pApplication) == false)
                        {
                            bRes = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    bRes = false;
                    _SysLocalLog.WriteLocalLog("出错：" + e.Message);
                }
            }
           

            return bRes;
        }
        /// <summary>
        /// 插件接口分类解析
        /// </summary>
        private static void ParsePlugins(PluginCollection pluginCol)
        {
            if (pluginCol == null) return;

            //分类解析、获取插件
            SysCommon.ModSysSetting.WriteLog("parsePluginCol"); //@日志测试
            ParsePluginCol parsePluginCol = new ParsePluginCol();
            SysCommon.ModSysSetting.WriteLog("GetPluginArray start"); //@日志测试
            parsePluginCol.GetPluginArray(pluginCol);
            SysCommon.ModSysSetting.WriteLog("GetPluginArray end"); //@日志测试

            v_dicPlugins = parsePluginCol.GetPlugins;
            v_dicCommands = parsePluginCol.GetCommands;
            v_dicTools = parsePluginCol.GetTools;
            v_dicToolBars = parsePluginCol.GetToolBars;
            v_dicMenus = parsePluginCol.GetMenus;
            v_dicDockableWindows = parsePluginCol.GetDockableWindows;
            v_dicControls = parsePluginCol.GetControls;
        }
        private static void PluginOnWriteLog(IPlugin plugin, string strWritelog)
        {
            bool Writelog = true;
            try
            {
                Writelog = Convert.ToBoolean(strWritelog);
            }
            catch
            { }
            ICommandRef cmd = plugin as ICommandRef;
            if (cmd != null)
            {
                cmd.WriteLog = Writelog;
            }
            IToolRef atool = plugin as IToolRef;
            if (atool != null)
            {
                atool.WriteLog = Writelog;
            }
        }
        /// <summary>
        /// 初始化插件内容
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="pApplication"></param>
        private static void PluginOnCreate(IPlugin plugin, IApplicationRef pApplication)
        {
            ICommandRef cmd = plugin as ICommandRef;
            if (cmd != null)
            {
                cmd.OnCreate(pApplication);
            }


            IToolRef atool = plugin as IToolRef;
            if (atool != null)
            {
                atool.OnCreate(pApplication);
            }


            IMenuRef aMenu = plugin as IMenuRef;
            if (aMenu != null)
            {
                aMenu.OnCreate(pApplication);
            }

            IToolBarRef aToolBar = plugin as IToolBarRef;
            if (aToolBar != null)
            {
                aToolBar.OnCreate(pApplication);
            }

            IDockableWindowRef aDockableWindow = plugin as IDockableWindowRef;
            if (aDockableWindow != null)
            {
                aDockableWindow.OnCreate(pApplication);
            }

            IControlRef aControl = plugin as IControlRef;
            if (aControl != null)
            {
                aControl.OnCreate(pApplication);
            }
        }
        /// <summary>
        /// 初始化插件内容
        /// </summary>
        /// <param name="plugin"></param>
        /// <param name="pApplication"></param>
        private static void PluginOnLoadData(IPlugin plugin, IApplicationRef pApplication)
        {
            IControlRef aControl = plugin as IControlRef;
            if (aControl != null)
            {
                aControl.LoadData();
            }
        }
        # region 根据XML内容进行插件事件绑定
        /// <summary>
        /// 根据XML关联各插件间事件方法
        /// </summary>
        private static void LoadEventsByXmlNode()
        {
            if (v_dicPlugins == null || _SysXmlDocument == null) return;

            string xPath = "//Event";
            XmlNode xmlnode = _SysXmlDocument.SelectSingleNode(xPath);
            if (xmlnode == null) return;
            if (xmlnode.HasChildNodes == false) return;

            string OrgName = "";
            string OrgEvent = "";
            string ObjName = "";
            string ObjMethod = "";

            try
            {
                foreach (XmlNode xmlnodeChild in xmlnode.ChildNodes)
                {
                    XmlElement xmlelementChild = xmlnodeChild as XmlElement;
                    OrgName = xmlelementChild.GetAttribute("OrgName").Trim();
                    OrgEvent = xmlelementChild.GetAttribute("OrgEvent").Trim();
                    ObjName = xmlelementChild.GetAttribute("ObjName").Trim();
                    ObjMethod = xmlelementChild.GetAttribute("ObjMethod").Trim();

                    //判断插件中是否存在该项
                    if (!v_dicPlugins.ContainsKey(OrgName))
                    {
                        _SysLocalLog.WriteLocalLog("事件绑定异常：" + "不存在插件" + OrgName);
                        continue;
                    }
                    if (!v_dicPlugins.ContainsKey(ObjName))
                    {
                        _SysLocalLog.WriteLocalLog("事件绑定异常：" + "不存在插件" + ObjName);
                        continue;
                    }


                    IPlugin OrgPlugin = v_dicPlugins[OrgName];
                    IPlugin ObjPlugin = v_dicPlugins[ObjName];
                    Type OrgType = OrgPlugin.GetType();
                    Type type=OrgType.GetEvent(OrgEvent).EventHandlerType;
                    if (type == null)
                    {
                        _SysLocalLog.WriteLocalLog("事件绑定异常：" + "未能获取事件" + OrgEvent.ToString());
                        continue;
                    }

                    Delegate newDelegate = Delegate.CreateDelegate(type, ObjPlugin, ObjMethod);
                    if (newDelegate == null)
                    {
                        _SysLocalLog.WriteLocalLog("事件绑定异常：" + "未能获取委托方法" + ObjMethod.ToString());
                        continue;
                    }

                    OrgType.GetEvent(OrgEvent).AddEventHandler(ObjPlugin, newDelegate);
                }
            }
            catch (Exception e)
            {
                _SysLocalLog.WriteLocalLog("事件绑定出错：" + e.Message);
            }
        }
        #endregion

        private static void BaseItem_Click(object sender, EventArgs e)
        {
            DevExpress.XtraBars.BarItem aBaseItem = sender as DevExpress.XtraBars.BarItem;
            string sKey = aBaseItem.Name.ToString().Trim();
            if (m_pBaseItem == null)
            {
                m_pBaseItem = aBaseItem as DevExpress.XtraBars.BarButtonItem;
            }
            else
            {
                if (!m_pBaseItem.Equals(aBaseItem))
                {
                    m_pBaseItem = aBaseItem as DevExpress.XtraBars.BarButtonItem;
                }
            }
            //end
            if (v_dicCommands.ContainsKey(sKey))
            {
                ICommandRef pCommandRef = v_dicCommands[sKey] as ICommandRef;
                pCommandRef.OnClick();
            }

            if (v_dicTools.ContainsKey(sKey))
            {
                IToolRef pToolRef = v_dicTools[sKey] as IToolRef;
                pToolRef.OnClick();
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            if(_dicBaseItems==null) return;
            if (v_dicCommands != null)
            {
                foreach (KeyValuePair<string, ICommandRef> keyvalue in v_dicCommands)
                {
                    foreach(KeyValuePair<DevExpress.XtraBars.BarItem, string> kvCmd in _dicBaseItems)
                    {
                        if (kvCmd.Value == keyvalue.Key)
                        {
                            kvCmd.Key.Enabled = keyvalue.Value.Enabled;
                        }
                    }
                }
            }

            if (v_dicTools != null)
            {
                foreach (KeyValuePair<string, IToolRef> keyvalue in v_dicTools)
                {
                    foreach (KeyValuePair<DevExpress.XtraBars.BarItem, string> kvTool in _dicBaseItems)
                    {
                        if (kvTool.Value == keyvalue.Key)
                        {
                            kvTool.Key.Enabled = keyvalue.Value.Enabled;
                        }
                    }
                }
            }
        }
        //wgf 20110602 左上角系统切换
        private static void menuSystemItem_Click(object sender, EventArgs e)
        {
            DevExpress.XtraBars.BarButtonItem aSysItem = sender as DevExpress.XtraBars.BarButtonItem;
            if (aSysItem == null || _dicTabs == null || _pIAppFrm == null) return;

            _pIAppFrm.CurrentSysName = aSysItem.Name;
            _pIAppFrm.Caption = SysCommon.ModSysSetting.GetSystemName() + aSysItem.Caption; //added by chulili 20111022 修改子系统标题名

            bool bEnable = false;
            bool bVisible = false;

            //刷新菜单列表 wgf 20111109
            int i = 0;
            Mod.WriteLocalLog("_dicTabs start");
            foreach (KeyValuePair<DevExpress.XtraBars.Ribbon.RibbonPage, string> keyValue in _dicTabs)
            {
                if (keyValue.Value == aSysItem.Name)
                {
                    i = i + 1;
                    keyValue.Key.Visible = true;
                }
                else
                {
                    keyValue.Key.Visible = false;
                }
            }
            Mod.WriteLocalLog("_dicTabs end");
            //刷新窗口控件 wgf 20111109
            if (v_dicControls != null)
            {
                foreach (KeyValuePair<string, IControlRef> keyValue in v_dicControls)
                {
                    Mod.WriteLocalLog("bEnable start");

                    bEnable = keyValue.Value.Enabled;
                    Mod.WriteLocalLog("bVisible start");

                    bVisible = keyValue.Value.Visible;
                    Mod.WriteLocalLog("bVisible end" + keyValue.Value.Name );

                    Plugin.Interface.ICommandRef pCmd = keyValue.Value as Plugin.Interface.ICommandRef;
                    if (pCmd != null)
                    {
                        if (keyValue.Key == aSysItem.Name)
                        {
                            pCmd.OnClick();
                        }
                    }
                }
            }
            Mod.WriteLocalLog("v_dicControls end");
        }
        private static void BaseItem_MouseEnter(object sender, EventArgs e)
        {
            string strMessage=string.Empty;
            DevExpress.XtraBars.BarItem aBaseItem = sender as DevExpress.XtraBars.BarItem;
            if (aBaseItem == null) return;

            string sKey = aBaseItem.Name.ToString().Trim();

            if (v_dicCommands.ContainsKey(sKey))
            {
                ICommandRef pCommandRef = v_dicCommands[sKey] as ICommandRef;
                strMessage = pCommandRef.Message;
            }

            if (v_dicTools.ContainsKey(sKey))
            {
                IToolRef pToolRef = v_dicTools[sKey] as IToolRef;
                strMessage = pToolRef.Message;
            }
        }
        private static void BaseItem_MouseLeave(object sender, EventArgs e)
        {
            DevExpress.XtraBars.BarItem aBaseItem = sender as DevExpress.XtraBars.BarItem;
            if (aBaseItem == null) return;

            string sKey = aBaseItem.Name.ToString().Trim();

            if (v_dicCommands.ContainsKey(sKey))
            {
                ICommandRef pCommandRef = v_dicCommands[sKey] as ICommandRef;
                pCommandRef.ClearMessage();
            }

            if (v_dicTools.ContainsKey(sKey))
            {
                IToolRef pToolRef = v_dicTools[sKey] as IToolRef;
                pToolRef.ClearMessage();
            }
        }
    }
    #endregion
}
