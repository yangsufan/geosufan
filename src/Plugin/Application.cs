using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Threading;
using SysCommon.Authorize;
using System.Drawing.Printing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;

using SysCommon.Gis;
using ESRI.ArcGIS.Geometry;

namespace Plugin.Application
{
    #region 接口声明
    public interface IApplicationRef
    {
    }
    /// <summary>
    /// 框架公用属性接口
    /// </summary>
    public interface IAppFormRef : IApplicationRef
    {
        /// <summary>
        /// 窗体
        /// </summary>
        Form MainForm { get; set; }
        /// <summary>
        /// 窗体名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 窗体标题
        /// </summary>
        string Caption { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        bool Visible { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        bool Enabled { get; set; }
        /// <summary>
        /// 工具栏控件容器
        /// </summary>
        Control ControlContainer { get; set; }
        #region 状态栏属性
        /// <summary>
        /// 状态栏
        /// </summary>
        Control StatusBar { get; set; }
        /// <summary>
        /// 操作提示内容
        /// </summary>
        string OperatorTips { get; set; }
        /// <summary>
        /// 进度条
        /// </summary>
        DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar { get; set; }
        //参考比例尺cmb
        DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb { get; set; }
        //当前比例尺cmb
        DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb { get; set; }
        //坐标显示文本框
        DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt { get; set; }
        /// <summary>
        /// 图上点坐标内容
        /// </summary>
        string CoorXY { get; set; }
        /// <summary>
        /// 用户信息内容
        /// </summary>
        string UserInfo { get; set; }
        #endregion
        /// <summary>
        /// 当前使用的系统名称
        /// </summary>
        string CurrentSysName { get; set; }
        /// <summary>
        /// 系统功能树图Xml节点
        /// </summary>
        XmlDocument SystemXml { get; set; }
        /// <summary>
        /// 数据树图XML节点
        /// </summary>
        XmlDocument DataTreeXml { get; set; }
        /// <summary>
        /// 连接信息XML节点
        /// </summary>
        XmlDocument DatabaseInfoXml { get; set; }
        /// <summary>
        /// 右键菜单集合
        /// </summary>
        Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu { get; set; }
        /// <summary>
        /// 系统插件集合
        /// </summary>
        Plugin.Parse.PluginCollection ColParsePlugin { get; set; }
        /// <summary>
        /// 图片资源路径
        /// </summary>
        string ImageResPath { get; set; }
        /// <summary>
        /// 登陆的用户
        /// </summary>
        User ConnUser { get; set; }
        //cyf 20110602 add 登录用户对应的角色信息
        List<Role> LstRoleInfo { get; set; }
        //end
        /// <summary>
        /// 用户权限编号
        /// </summary>
        List<string> ListUserPrivilegeID { get; set; }
        //连接信息
        /// <summary>
        /// 正式库数据库接口信息
        /// </summary>
        ICustomWks CurWksInfo { get; set; }
        /// <summary>
        /// 临时库数据库结构信息
        /// </summary>
        ICustomWks TempWksInfo { get; set; }

        //added by chulili 20111120 
        IMapControlDefault MapControl { get; set; }
        ITOCControlDefault TOCControl { get; set; }
        //end added by chulili 
        object LayerTree { get; set; }
        object LayerAdvTree { get; set; }
        string LayerTreeXmlPath { get; set; }
    }

    //自定义空间数据库工作空间接口
    public struct ICustomWks
    {
        public ESRI.ArcGIS.Geodatabase.IWorkspace Wks;
        public string Server;
        public string Service;
        public string Database;
        public string User;
        public string PassWord;
        public string Version;
        public string DataBase;
        public string DBType;
    }
    /// <summary>
    /// ArcGIS控件属性接口
    /// </summary>
    public interface IAppArcGISRef : IApplicationRef
    {
        /// <summary>
        /// 当前使用的GIS工具
        /// </summary>
        string CurrentTool { get; set; }
        /// <summary>
        /// 存储的文档对象
        /// </summary>
        IMapDocument MapDocument { get; set; }
        /// <summary>
        /// 主控件中的MapControl控件
        /// </summary>
        IMapControlDefault MapControl { get; set; }
        AxMapControl ArcGisMapControl { get; set; }
        /// <summary>
        /// 主控件中的SceneControl控件
        /// </summary>
        ISceneControlDefault SceneControlDefault { get; set; }
        AxSceneControl SceneControl { get; set; }
        /// <summary>
        /// 主控件中的PageLayoutControl控件
        /// </summary>
        IPageLayoutControlDefault PageLayoutControl { get; set; }
        /// <summary>
        /// 主控件中的TOCControl控件
        /// </summary>
        ITOCControlDefault TOCControl { get; set; }
        /// <summary>
        /// 十字丝 xisheng 20110805
        /// </summary>
        PictureBox p1 { get; set; }
        PictureBox p2 { get; set; }
        PictureBox p3{ get; set; }
        PictureBox p4 { get; set; }
        //cyf  20110518 
        /// <summary>
        /// 当前图形显示控件
        /// </summary>
        object CurrentControl { get; set; }
        /// <summary>
        /// 将tab按照系统分类
        /// </summary>
        Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs { get; set; }
        //end
    }
    /// <summary>
    /// GIS系统公用属性接口
    /// </summary>
    public interface IAppGISRef : IAppArcGISRef
    {
        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl MainUserControl { get; set; }
        /// <summary>
        /// 数据处理树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList DataTree { get; set; }
        /// <summary>
        /// 工程信息树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ProjectTree { get; set; }
        /// <summary>
        /// 数据工程配置XML文件
        /// </summary>
        XmlDocument DBXmlDocument { get; set; }
        /// <summary>
        /// 更新对比列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView UpdateGrid { get; set; }
        /// <summary>
        /// 更新对比列表分页信息显示文本框
        /// </summary>
        DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtDisplayPage { get; set; }
        /// <summary>
        /// 更新对比列表按钮
        /// </summary>
        DevExpress.XtraBars.BarButtonItem BtnFirst { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnLast { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnPre { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnNext { get; set; }
        /// <summary>
        /// 线搜索结果列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView PolylineSearchGrid { get; set; }
        /// <summary>
        /// 多边形搜索结果
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView PolygonSearchGrid { get; set; }
        /// <summary>
        /// 接边融合记录结果
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView JoinMergeResultGrid { get; set; }
        /// <summary>
        /// 数据检查列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView DataCheckGrid { get; set; }
        /// <summary>
        /// 处理数据的进程(唯一)
        /// </summary>
        Thread CurrentThread { get; set; }
    }
    /// <summary>
    /// 数据更新系统公用属性接口
    /// </summary>
    public interface IAppSMPDRef : IAppArcGISRef
    {
        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl MainUserControl { get; set; }
        /// <summary>
        /// 更新对比列表分页信息显示文本框
        /// </summary>
        DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtDisplayPage { get; set; }
        /// <summary>
        /// 更新对比列表按钮
        /// </summary>
        DevExpress.XtraBars.BarButtonItem BtnFirst { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnLast { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnPre { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnNext { get; set; }
        /// <summary>
        /// 错误树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ErrTree { get; set; }
        /// <summary>
        /// 工程信息树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ProjectTree { get; set; }
        /// <summary>
        /// 更新对比分析列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView UpdateDataGrid { get; set; }
        /// <summary>
        /// 数据检查列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView DataCheckGrid { get; set; }
        /// <summary>
        /// 数据工程配置XML文件
        /// </summary>
        XmlDocument DBXmlDocument { get; set; }
        /// <summary>
        /// 处理数据的进程(唯一)
        /// </summary>
        Thread CurrentThread { get; set; }
    }
    /// <summary>
    /// 配置系统公用属性接口
    /// </summary>
    public interface IAppPrivilegesRef : IAppFormRef
    {
        //wgf 20110518
        //---------------------------------------------------------
        System.Windows.Forms.TreeView DataTabIndexTree { get; set; }

        System.Windows.Forms.DataGridView GridCtrl { get; set; }

        System.Windows.Forms.RichTextBox tipRichBox { get; set; }
        string strLogFilePath { get; set; }   //日志文件
        //---------------------------------------------------------
        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl MainUserControl { get; set; }
        /// <summary>
        /// 配置管理主树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList MainTree { get; set; }
        /// <summary>
        /// 用户组树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList RoleTree { get; set; }

        /// 用户树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList UserTree { get; set; }
        /// <summary>
        /// 权限树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList PrivilegeTree { get; set; }
        /// <summary>
        /// 处理数据的进程(唯一)
        /// </summary>
        Thread CurrentThread { get; set; }
        //连接信息
        /// <summary>
        /// 正式库数据库接口信息
        /// </summary>
        ICustomWks CurWksInfo { get; set; }
        /// <summary>
        /// 临时库数据库结构信息
        /// </summary>
        ICustomWks TempWksInfo { get; set; }
        bool CurScaleVisible { get; set; }
        bool RefScaleVisible { get; set; }
    }

    /// <summary>
    /// 数据更新系统公用属性接口
    /// </summary>
    public interface IAppGisUpdateRef : IAppArcGISRef
    {
        //wgf 2011-5-18
        //-------------------------------------------------------------------
        //文档窗口
        System.Windows.Forms.RichTextBox DocControl { get; set; }
        /// 数据单元树图
        /// </summary>
        System.Windows.Forms.TreeView DataUnitTree { get; set; }

        /// 资源目录树图
        /// </summary>
        System.Windows.Forms.TreeView DataIndexTree { get; set; }

        /// 地图文档树图
        /// </summary>
        System.Windows.Forms.TreeView MapDocTree { get; set; }

        //文档树
        System.Windows.Forms.TreeView TextDocTree { get; set; }

        //用户成果树
        System.Windows.Forms.TreeView UserResultTree { get; set; }

        DevExpress.XtraBars.Ribbon.RibbonPage IndextabControl { get; set; }

        System.Windows.Forms.RichTextBox tipRichBox { get; set; }
        SysCommon.BottomQueryBar QueryBar { get; set; }

        SysGisDataSet gisDataSet { get; set; }         //数据库操作类 wgf

        string strLogFilePath { get; set; }   //日志文件

        //---------------------------------------------------------------------

        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl MainUserControl { get; set; }

        /// <summary>
        /// 数据处理树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList DataTree { get; set; }

        /// <summary>
        /// 错误树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ErrTree { get; set; }

        /// <summary>
        /// 工程信息树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ProjectTree { get; set; }

        /// <summary>
        /// 更新对比分析列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView UpdateDataGrid { get; set; }
        /// <summary>
        /// 数据工程配置XML文件
        /// </summary>
        XmlDocument DBXmlDocument { get; set; }
        /// <summary>
        /// 处理数据的进程(唯一)
        /// </summary>
        Thread CurrentThread { get; set; }
    }

    /// <summary>
    /// 文件库系统共用属性接口
    /// </summary>
    public interface IAppFileRef : IAppArcGISRef
    {
        //wgf 2011-5-18
        //---------------------------------------------------------
        System.Windows.Forms.TreeView DataTabIndexTree { get; set; }
        System.Windows.Forms.DataGridView GridCtrl { get; set; }
        System.Windows.Forms.RichTextBox tipRichBox { get; set; }
        string strLogFilePath { get; set; }   //日志文件
        //---------------------------------------------------------
        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl MainUserControl { get; set; }
        /// <summary>
        /// 工程信息树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ProjectTree { get; set; }
        /// <summary>
        /// 数据信息列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView DataInfoGrid { get; set; }
        /// <summary>
        /// 元数据列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView MetaDataGrid { get; set; }
        /// <summary>
        /// 系统设置列表
        /// </summary>
        DevExpress.XtraGrid.Views.Grid.GridView SysSettingGrid { get; set; }
        /// <summary>
        /// 系统设置树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList SysSettingTree { get; set; }
        /// <summary>
        /// 数据工程配置XML文件
        /// </summary>
        XmlDocument DBXmlDocument { get; set; }
        /// <summary>
        /// 数据信息列表分页信息显示文本框
        /// </summary>
        DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtDisplayPage { get; set; }
        /// <summary>
        /// 数据信息列表按钮
        /// </summary>
        DevExpress.XtraBars.BarButtonItem BtnFirst { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnLast { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnPre { get; set; }
        DevExpress.XtraBars.BarButtonItem BtnNext { get; set; }
        /// <summary>
        /// 处理数据的进程(唯一)
        /// </summary>
        Thread CurrentThread { get; set; }
    }
    /// <summary>
    /// 数据库集成管理子系统插件接口定义  陈亚飞添加  20100927
    /// </summary>
    public interface IAppDBIntegraRef : IAppArcGISRef
    {
        /// <summary>
        /// 用户控件
        /// </summary>
        UserControl MainUserControl { get; set; }
        /// <summary>
        /// 工程信息树图
        /// </summary>
        DevExpress.XtraTreeList.TreeList ProjectTree { get; set; }
        /// <summary>
        /// 数据工程配置XML文件
        /// </summary>
        XmlDocument DBXmlDocument { get; set; }
        /// <summary>
        /// 处理数据的进程(唯一)
        /// </summary>
        Thread CurrentThread { get; set; }
        //added by chulili 20110722 添加函数 支持状态栏比例尺可见状态设置
        bool CurScaleVisible { get; set; }
        bool RefScaleVisible { get; set; }
        //end added by chulili
    }
    #endregion
    #region 接口实现类
    public class AppForm : IAppFormRef
    {
        //added by chulili 20111120
        private IMapControlDefault _MapControl;                      //MapControl控件
        private ITOCControlDefault _TOCControl;                     //TOCControl控件
        //end added by chulili
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private object _LayerTree;                                      //added by chulili 20111119
        private string _LayerTreeXmlPath;                               //added by chulili 20111215
        private Form _MainForm;                                        // 窗体
        private Control _StatusStrip;                                  // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private string _CurrentTab;                                   // 当前使用的Tab
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;                                  // 图片资源路径
        private User _user;                                          //连接的用户信息 
        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end
        private List<string> _ListUserPrivilegeID;                   //用户权限编号
        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        //构造函数
        public AppForm()
        {
        }

        public AppForm(Form MainForm, Control StatusStrip, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath)
        {
            _MainForm = MainForm;
            _StatusStrip = StatusStrip;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;
        }

        #region IDefAppForm 成员

        public Form MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }
        public object LayerTree
        {
            get
            {
                return _LayerTree;
            }
            set
            {
                _LayerTree = value;
            }
        }
        //added by chulili 2011-11-29 图层目录的AdvTree
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree; ;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //added by chulili 2011-12-15 图层目录配置文件路径
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        //end added by chulili
        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }
        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }
        public string Name
        {
            get
            {
                return _MainForm.Name;
            }
            set
            {
                _MainForm.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainForm.Text;
            }
            set
            {
                _MainForm.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _MainForm.Visible;
            }
            set
            {
                _MainForm.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainForm.Enabled;
            }
            set
            {
                _MainForm.Enabled = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public string CurrentSysName
        {
            get
            {
                return _CurrentTab;
            }
            set
            {
                _CurrentTab = value;
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }
        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end

        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }

        #endregion

        #region 状态栏属性
        private String _OperatorTips;
        public string OperatorTips
        {
            get
            {
                return _OperatorTips;
            }
            set
            {
                _OperatorTips = value;
            }
        }

        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar _ProgressBar;
        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get
            {
                return _ProgressBar;
            }
            set
            {
                _ProgressBar = value;
            }
        }
        //参考比例尺cmb
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox _RefScaleCmb;
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get
            {
                return _RefScaleCmb;
            }
            set
            {
                _RefScaleCmb = value;
            }
        }
        //当前比例尺cmb
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox _CurScaleCmb;
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get
            {
                return _CurScaleCmb;
            }
            set
            {
                _CurScaleCmb = value;
            }
        }
        //坐标显示文本框
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit _CoorTxt;
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get
            {
                return _CoorTxt;
            }
            set
            {
                _CoorTxt = value;
            }
        }

        private String _UserInfo;
        public string UserInfo
        {
            get
            {
                return _UserInfo;
            }
            set
            {
                _UserInfo = value;
            }
        }

        private String _CoorXY;
        public string CoorXY
        {
            get
            {
                return _CoorXY;
            }
            set
            {
                _CoorXY = value;
            }
        }
        #endregion


    }

    public class AppGIS : IAppGISRef, IAppFormRef
    {
        private object _LayerTree;                                      //added by chulili 20111119
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private string _LayerTreeXmlPath;                               //added by chulili 20111215
        private Form _MainForm;                                        // 窗体
        private UserControl _MainUserControl;                         //控件UserControl
        private Control _StatusStrip;                                 // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;                                  // 图片资源路径
        private IMapDocument _MapDocument;                            //存储的文档对象
        private IMapControlDefault _MapControl;                      //MapControl控件
        private AxMapControl _AxMapControl;
        private ISceneControlDefault _SceneControlDefault;
        private AxSceneControl _SceneControl;

        private IPageLayoutControlDefault _PageLayoutControl;        //PageLayoutControl控件
        private ITOCControlDefault _TOCControl;                     //TOCControl控件

        PictureBox _p1;                                               //做十字丝
        PictureBox _p2; 
        PictureBox _p3; 
        PictureBox _p4;

        private string _CurrentTool;                                   // 当前使用的TOOL名称
        //20110518
        private object _CurrentControl;                             //当前图形显示控件    
        private List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> _scaleBoxList;                        //比例尺显示        
        private Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> _dicTabs;          //将tab按照系统分类
        //end

        private DevExpress.XtraTreeList.TreeList _ProjectTree;       // 工程信息树图
        private DevExpress.XtraTreeList.TreeList _DataTree;          // 数据处理树图
        private DevExpress.XtraGrid.Views.Grid.GridView _UpdateGrid;  //更新对比列表
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit _txtDisplayPage;//更新对比列表分页显示文本框
        private DevExpress.XtraBars.BarButtonItem _btnFirst;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnLast;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnPre;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnNext;//更新对比列表分页显示按钮
        private DevExpress.XtraGrid.Views.Grid.GridView _PolylineSearchGrid;  //接边线型记录表
        private DevExpress.XtraGrid.Views.Grid.GridView _PolygonSearchGrid;  //接边多边形记录表
        private DevExpress.XtraGrid.Views.Grid.GridView _JoinMergeResultGrid;  //接边融合结果记录表

        private DevExpress.XtraGrid.Views.Grid.GridView _DataCheckGrid;   //数据检查表格

        private Thread _CurrentThread;                           //处理数据的进程(唯一)
        private XmlDocument _DBXmlDocument;                       //数据工程配置XML文件
        private User _user;                                          //连接的用户信息
        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end
        private List<string> _ListUserPrivilegeID;                   //用户权限编号

        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        private SysCommon.BottomQueryBar _QueryBar; //added by chulili 2012-10-16 入库更新子系统添加查询结果状态栏

        //构造函数
        public AppGIS()
        {
        }

        public AppGIS(Form pForm, Control ControlContainer, List<string> ListUserPrivilegeID, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath, User V_user)
        {
            //沿袭AppFormRef 参数
            _MainForm = pForm;
            _ControlContainer = ControlContainer;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;
            //added by chulili 20110711 添加两个参数
            _ListUserPrivilegeID = ListUserPrivilegeID;
            _user = V_user;
        }


        #region IAppGISDef 成员

        public UserControl MainUserControl
        {
            get
            {
                return _MainUserControl;
            }
            set
            {
                _MainUserControl = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList DataTree
        {
            get
            {
                return _DataTree;
            }
            set
            {
                _DataTree = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList ProjectTree
        {
            get
            {
                return _ProjectTree;
            }
            set
            {
                _ProjectTree = value;
            }
        }

        public XmlDocument DBXmlDocument
        {
            get
            {
                return _DBXmlDocument;
            }
            set
            {
                _DBXmlDocument = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView UpdateGrid
        {
            get
            {
                return _UpdateGrid;
            }
            set
            {
                _UpdateGrid = value;
            }
        }
        public DevExpress.XtraGrid.Views.Grid.GridView PolylineSearchGrid
        {
            get
            {
                return _PolylineSearchGrid;
            }
            set
            {
                _PolylineSearchGrid = value;
            }
        }
        public DevExpress.XtraGrid.Views.Grid.GridView PolygonSearchGrid
        {
            get
            {
                return _PolygonSearchGrid;
            }
            set
            {
                _PolygonSearchGrid = value;
            }
        }
        public DevExpress.XtraGrid.Views.Grid.GridView JoinMergeResultGrid
        {
            get
            {
                return _JoinMergeResultGrid;
            }
            set
            {
                _JoinMergeResultGrid = value;
            }
        }




        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtDisplayPage
        {
            get
            {
                return _txtDisplayPage;
            }
            set
            {
                _txtDisplayPage = value;
            }
        }

        public DevExpress.XtraBars.BarButtonItem BtnFirst
        {
            get
            {
                return _btnFirst;
            }
            set
            {
                _btnFirst = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnLast
        {
            get
            {
                return _btnLast;
            }
            set
            {
                _btnLast = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnPre
        {
            get
            {
                return _btnPre;
            }
            set
            {
                _btnPre = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnNext
        {
            get
            {
                return _btnNext;
            }
            set
            {
                _btnNext = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView DataCheckGrid
        {
            get
            {
                return _DataCheckGrid;
            }
            set
            {
                _DataCheckGrid = value;
            }
        }
        public SysCommon.BottomQueryBar QueryBar
        {
            get
            {
                return _QueryBar;
            }
            set
            {
                _QueryBar = value;
            }
        }

        public string Name
        {
            get
            {
                return _MainUserControl.Name;
            }
            set
            {
                _MainUserControl.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainUserControl.Tag as string;
            }
            set
            {
                _MainUserControl.Tag = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _MainUserControl.Visible;
            }
            set
            {
                _MainUserControl.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainUserControl.Enabled;
            }
            set
            {
                _MainUserControl.Enabled = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }


        #endregion

        #region 状态栏属性
        public string OperatorTips
        {
            get; set;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get; set;
        }

        //参考比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get; set;
        }
        //当前比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get;set;
        }
        //坐标显示文本框
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get; set;
        }


        public string CoorXY
        {
            get; set;
        }

        public string UserInfo
        {
            get; set;
        }

        #endregion

        #region IAppArcGISRef 成员
        public string CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                _CurrentTool = value;
            }
        }

        public IMapDocument MapDocument
        {
            get
            {
                return _MapDocument;
            }
            set
            {
                _MapDocument = value;
            }
        }

        public PictureBox p1
        {
            get
            { return _p1; }
            set
            { _p1 = value; }
        }

        public PictureBox p2
        {
            get
            { return _p2; }
            set
            { _p2 = value; }
        }

        public PictureBox p3
        {
            get
            { return _p3; }
            set
            { _p3 = value; }
        }

        public PictureBox p4
        {
            get
            { return _p4; }
            set
            { _p4 = value; }
        }


        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }

        public AxMapControl ArcGisMapControl
        {
            get
            {
                return _AxMapControl;
            }
            set
            {
                _AxMapControl = value;
            }
        }

        public ISceneControlDefault SceneControlDefault
        {
            get { return _SceneControlDefault; }
            set { _SceneControlDefault = value; }
        }

        public AxSceneControl SceneControl
        {
            get { return _SceneControl; }
            set { _SceneControl = value; }
        }

        public IPageLayoutControlDefault PageLayoutControl
        {
            get
            {
                return _PageLayoutControl;
            }
            set
            {
                _PageLayoutControl = value;
            }
        }

        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }
        public object CurrentControl
        {
            get
            {
                return _CurrentControl;
            }
            set
            {
                _CurrentControl = value;
            }
        }
        public List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> ScaleBoxList
        {
            get
            {
                return _scaleBoxList;
            }
            set
            {
                _scaleBoxList = value;
            }
        }

        public Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs
        {
            get
            {
                return _dicTabs;
            }
            set
            {
                _dicTabs = value;
            }
        }
        #endregion

        #region IAppFormDef 成员

        Form IAppFormRef.MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        string IAppFormRef.CurrentSysName
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }

        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end

        public object LayerTree
        {
            get
            {
                return _LayerTree;
            }
            set
            {
                _LayerTree = value;
            }
        }
        //added by chulili 2011-11-29 图层目录的AdvTree
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree; ;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //added by chulili 2011-12-15 图层目录配置文件路径
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        //end added by chulili
        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }
        #endregion
    }
     
    public class AppSMPD : IAppSMPDRef, IAppFormRef
    {
        private object _LayerTree;                                      //added by chulili 20111119
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private string _LayerTreeXmlPath;                               //added by chulili 20111215
        private Form _MainForm;                                        // 窗体
        private UserControl _MainUserControl;                         //控件UserControl
        private Control _StatusStrip;                                 // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;                                  // 图片资源路径
        private IMapDocument _MapDocument;                            //存储的文档对象
        private IMapControlDefault _MapControl;                      //MapControl控件
        private AxMapControl _AxMapControl;

        private ISceneControlDefault _SceneControlDefault;
        private AxSceneControl _SceneControl;
        private IPageLayoutControlDefault _PageLayoutControl;        //PageLayoutControl控件
        private ITOCControlDefault _TOCControl;                     //TOCControl控件
        private string _CurrentTool;                                   // 当前使用的TOOL名称
        //20110518
        private object _CurrentControl;                             //当前图形显示控件 
        private List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> _scaleBoxList;                        //比例尺显示        
        private Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> _dicTabs;          //将tab按照系统分类
        //end


        private DevExpress.XtraTreeList.TreeList _DataTree;          // 数据处理树图
        private DevExpress.XtraTreeList.TreeList _ErrTree;           // 错误树图
        private DevExpress.XtraTreeList.TreeList _ProjectTree;       // 工程信息树图
        private DevExpress.XtraGrid.Views.Grid.GridView _UpdateDataGrid;   //更新对比表格

        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit _txtDisplayPage;//更新对比列表分页显示文本框
        private DevExpress.XtraBars.BarButtonItem _btnFirst;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnLast;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnPre;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnNext;//更新对比列表分页显示按钮

        private DevExpress.XtraGrid.Views.Grid.GridView _DataCheckGrid;   //数据检查表格
        private XmlDocument _DBXmlDocument;                       //数据工程配置XML文件

        private Thread _CurrentThread;                           //处理数据的进程(唯一)

        private User _user;                                          //连接的用户信息
        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end
        private List<string> _ListUserPrivilegeID;                   //用户权限编号

        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        //构造函数
        public AppSMPD()
        {
        }

        public AppSMPD(Form pForm, Control ControlContainer, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath)
        {
            //沿袭AppFormRef 参数
            _MainForm = pForm;
            _ControlContainer = ControlContainer;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;

        }

        #region IAppSMPDRef 成员
        public UserControl MainUserControl
        {
            get
            {
                return _MainUserControl;
            }
            set
            {
                _MainUserControl = value;
            }
        }

        public string Name
        {
            get
            {
                return _MainUserControl.Name;
            }
            set
            {
                _MainUserControl.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainUserControl.Tag as string;
            }
            set
            {
                _MainUserControl.Tag = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _MainUserControl.Visible;
            }
            set
            {
                _MainUserControl.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainUserControl.Enabled;
            }
            set
            {
                _MainUserControl.Enabled = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList DataTree
        {
            get
            {
                return _DataTree;
            }
            set
            {
                _DataTree = value;
            }
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtDisplayPage
        {
            get
            {
                return _txtDisplayPage;
            }
            set
            {
                _txtDisplayPage = value;
            }
        }

        public DevExpress.XtraBars.BarButtonItem BtnFirst
        {
            get
            {
                return _btnFirst;
            }
            set
            {
                _btnFirst = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnLast
        {
            get
            {
                return _btnLast;
            }
            set
            {
                _btnLast = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnPre
        {
            get
            {
                return _btnPre;
            }
            set
            {
                _btnPre = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnNext
        {
            get
            {
                return _btnNext;
            }
            set
            {
                _btnNext = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList ErrTree
        {
            get
            {
                return _ErrTree;
            }
            set
            {
                _ErrTree = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList ProjectTree
        {
            get
            {
                return _ProjectTree;
            }
            set
            {
                _ProjectTree = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView UpdateDataGrid
        {
            get
            {
                return _UpdateDataGrid;
            }
            set
            {
                _UpdateDataGrid = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView DataCheckGrid
        {
            get
            {
                return _DataCheckGrid;
            }
            set
            {
                _DataCheckGrid = value;
            }
        }

        public XmlDocument DBXmlDocument
        {
            get
            {
                return _DBXmlDocument;
            }
            set
            {
                _DBXmlDocument = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }
        #endregion

        #region 状态栏属性
        public string OperatorTips
        {
            get;set;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get; set;
        }

        //参考比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get; set;
        }
        //当前比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get; set;
        }
        //坐标显示文本框
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get; set;
        }

        public string UserInfo
        {
            get; set;
        }

        public string CoorXY
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }
        #endregion

        #region IAppArcGISRef 成员
        public string CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                _CurrentTool = value;
            }
        }

        public IMapDocument MapDocument
        {
            get
            {
                return _MapDocument;
            }
            set
            {
                _MapDocument = value;
            }
        }

        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }

        public PictureBox p1 { get; set; }
        public PictureBox p2 { get; set; }
        public PictureBox p3 { get; set; }
        public PictureBox p4 { get; set; }
        public AxMapControl ArcGisMapControl
        {
            get
            {
                return _AxMapControl;
            }
            set
            {
                _AxMapControl = value;
            }
        }

        public ISceneControlDefault SceneControlDefault
        {
            get { return _SceneControlDefault; }
            set { _SceneControlDefault = value; }
        }

        public AxSceneControl SceneControl
        {
            get { return _SceneControl; }
            set { _SceneControl = value; }
        }

        public IPageLayoutControlDefault PageLayoutControl
        {
            get
            {
                return _PageLayoutControl;
            }
            set
            {
                _PageLayoutControl = value;
            }
        }

        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }

        //cyf 20110518  
        public object CurrentControl
        {
            get
            {
                return _CurrentControl;
            }
            set
            {
                _CurrentControl = value;
            }
        }
        public List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> ScaleBoxList
        {
            get
            {
                return _scaleBoxList;
            }
            set
            {
                _scaleBoxList = value;
            }
        }

        public Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs
        {
            get
            {
                return _dicTabs;
            }
            set
            {
                _dicTabs = value;
            }
        }
        //end

        #endregion

        #region IAppFormDef 成员

        Form IAppFormRef.MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        string IAppFormRef.CurrentSysName
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }

        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end

        public object LayerTree
        {
            get
            {
                return _LayerTree;
            }
            set
            {
                _LayerTree = value;
            }
        }
        //added by chulili 2011-11-29 图层目录的AdvTree
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree; ;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //added by chulili 2011-12-15 图层目录配置文件路径
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        //end added by chulili
        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }

        #endregion
    }
    public class AppPrivileges : IAppPrivilegesRef
    {
        //added by chulili 20111120
        private IMapControlDefault _MapControl;                      //MapControl控件
        private ITOCControlDefault _TOCControl;                     //TOCControl控件
        //end added by chulili
        private object _LayerTree;                                      //added by chulili 20111119
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private string _LayerTreeXmlPath;                               //added by chulili 20111215
        private Form _MainForm;                                        // 窗体
        private UserControl _MainUserControl;                         //控件UserControl
        private Control _StatusStrip;                                 // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;


        //wgf 2011-5-18
        //--------------------------------------------------
        private System.Windows.Forms.TreeView _DataTabIndexTree;

        private System.Windows.Forms.DataGridView _GridCtrl;

        private System.Windows.Forms.RichTextBox _tipRichBox;

        private string _strLogFilePath;
        //--------------------------------------------------

        private DevExpress.XtraTreeList.TreeList _MainTree;          // 主配置树图
        private DevExpress.XtraTreeList.TreeList _RoleTree;           // 用户组树图
        private DevExpress.XtraTreeList.TreeList _UserTree;       // 用户树图
        private DevExpress.XtraTreeList.TreeList _privilegeTree;       //权限树
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup _CurrentPanel;       // 当前GroupPanel
        private User _user;                                          //连接的用户信息 
        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end

        private Thread _CurrentThread;                           //处理数据的进程(唯一)
        private List<string> _ListUserPrivilegeID;                   //用户权限编号

        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        //构造函数
        public AppPrivileges()
        {
        }

        public AppPrivileges(Form pForm, Control ControlContainer, List<string> ListUserPrivilegeID, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath, User V_user)
        {
            //沿袭AppFormRef 参数
            _MainForm = pForm;
            _ControlContainer = ControlContainer;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;
            _user = V_user;
            _ListUserPrivilegeID = ListUserPrivilegeID;
        }

        #region 成员
        public UserControl MainUserControl
        {
            get
            {
                return _MainUserControl;
            }
            set
            {
                _MainUserControl = value;
            }
        }
        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }
        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }
        public string Name
        {
            get
            {
                return _MainUserControl.Name;
            }
            set
            {
                _MainUserControl.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainUserControl.Tag as string;
            }
            set
            {
                _MainUserControl.Tag = value;
            }
        }

        public bool Visible
        {

            get
            {
                if (_MainUserControl == null) return false;
                return _MainUserControl.Visible;
            }
            set
            {
                if (_MainUserControl == null) return;
                _MainUserControl.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainUserControl.Enabled;
            }
            set
            {
                _MainUserControl.Enabled = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList MainTree
        {
            get
            {
                return _MainTree;
            }
            set
            {
                _MainTree = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList RoleTree
        {
            get
            {
                return _RoleTree;
            }
            set
            {
                _RoleTree = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList UserTree
        {
            get
            {
                return _UserTree;
            }
            set
            {
                _UserTree = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList PrivilegeTree
        {
            get
            {
                return _privilegeTree;
            }
            set
            {
                _privilegeTree = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }
        #endregion

        #region 状态栏属性
        public string OperatorTips
        {
            get;set;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get; set;
        }

        //参考比例尺cmb
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox _RefScaleCmb;
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get
            {
                return _RefScaleCmb;
            }
            set
            {
                _RefScaleCmb = value;
            }
        }
        //参考比例尺是否可见
        public bool RefScaleVisible
        {
            get; set;
        }
        //当前比例尺是否可见
        public bool CurScaleVisible
        {
            get; set;
        }
        //当前比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get; set;
        }
        //end 褚丽丽  从别处拷贝
        //坐标显示文本框
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit _CoorTxt;
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get
            {
                return _CoorTxt;
            }
            set
            {
                _CoorTxt = value;
            }
        }

        public string UserInfo
        {
            get; set;
        }

        public string CoorXY
        {
            get; set;
        }
        #endregion

        #region IAppFormDef 成员

        Form IAppFormRef.MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        string IAppFormRef.CurrentSysName
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }

        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end

        public object LayerTree
        {
            get
            {
                return _LayerTree;
            }
            set
            {
                _LayerTree = value;
            }
        }
        //added by chulili 2011-11-29 图层目录的AdvTree
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree; ;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //added by chulili 2011-12-15 图层目录配置文件路径
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        //end added by chulili
        //wgf 2011-5-18
        //--------------------------------------------
        public System.Windows.Forms.TreeView DataTabIndexTree
        {
            get
            {
                return _DataTabIndexTree;
            }
            set
            {
                _DataTabIndexTree = value;
            }
        }
        public System.Windows.Forms.DataGridView GridCtrl
        {
            get
            {
                return _GridCtrl;
            }
            set
            {
                _GridCtrl = value;
            }
        }

        public System.Windows.Forms.RichTextBox tipRichBox
        {
            get
            {
                return _tipRichBox;
            }
            set
            {
                _tipRichBox = value;
            }
        }

        public string strLogFilePath
        {
            get
            {
                return _strLogFilePath;
            }
            set
            {
                _strLogFilePath = value;
            }
        }

        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }
        //---------------------------------------------

        #endregion
    }

    public class AppGidUpdate : IAppGisUpdateRef, IAppFormRef
    {
        private Form _MainForm;                                        // 窗体
        private UserControl _MainUserControl;                         //控件UserControl
        private Control _StatusStrip;                                 // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;                                  // 图片资源路径
        private IMapDocument _MapDocument;                            //存储的文档对象
        private IMapControlDefault _MapControl;                      //MapControl控件
        private AxMapControl _AxMapControl;
        PictureBox _p1; PictureBox _p2; PictureBox _p3; PictureBox _p4;//十字丝
        private IGeometry _Geometry; //当前绘制的图形要素
        private ISceneControlDefault _SceneControlDefault;
        private AxSceneControl _SceneControl;
        private IPageLayoutControlDefault _PageLayoutControlDefault;        //PageLayoutControl控件
        //private AxPageLayoutControl _PageLayoutControl;                 //PageLayoutControl控件
        private short _CurrentPrintPage = 0;                           //输出文档页数
        private PrintDocument _PrintDocument;                       //打印输出文档
        private ITOCControlDefault _TOCControl;                     //TOCControl控件
        private string _CurrentTool;                                   // 当前使用的TOOL名称
        private object _CurrentControl;                             //当前图形显示控件
        private List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> _scaleBoxList;                        //比例尺显示        
        private Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> _dicTabs;          //将tab按照系统分类

        private string _LayerTreeXmlPath = "";  //added by chulili 20111101 
        private object _LayerTree;  //目录树图 用来存放GeoLayerTreeLib.LayerManager.UcDataLib
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private DevExpress.XtraTreeList.TreeList _DataTree;          // 数据处理树图
        private DevExpress.XtraTreeList.TreeList _XZQTree;          // yjl20110924 add 行政区树图
        private DevExpress.XtraTreeList.TreeList _ResultFileTree;      //added by chulili 20110923 成果列表树图
        private DevExpress.XtraTreeList.TreeList _ErrTree;           // 错误树图
        private DevExpress.XtraTreeList.TreeList _ProjectTree;       // 工程信息树图
        private DevExpress.XtraGrid.Views.Grid.GridView _UpdateDataGrid;   //更新对比表格
        private DevExpress.XtraGrid.Views.Grid.GridView _DataCheckGrid;   //数据检查表格
        private XmlDocument _DBXmlDocument;                       //数据工程配置XML文件
        private User _user;                                          //连接的用户信息 
        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end
        private TreeView _ResultsTree;   //当前成果目录树 xisheng 20120612 当前改为制图文件管理目录树

        private Thread _CurrentThread;                           //处理数据的进程(唯一)

        //wgf20110518
        //--------------------------------------------------------
        //   private IFeatureWorkspace _pFeatureWorkspace;  //arcgis工作空间
        private System.Windows.Forms.TreeView _DataUnitTree;          // 数据单元树图
        private System.Windows.Forms.TreeView _DataIndexTree;          // 资源目录树图
        private System.Windows.Forms.TreeView _MapDocTree;          // 地图文档树图
        private System.Windows.Forms.TreeView _TextDocTree;          // 文档树图
        private System.Windows.Forms.TreeView _UserResultTree;          // 用户成果树图
        private System.Windows.Forms.TreeView _MetadataTree;          // yjl20110926 add 元数据树
        private DevExpress.XtraBars.Ribbon.RibbonPage _IndextabControl;//索引树tabcontrol 席胜
        private System.Windows.Forms.RichTextBox _DocControl; //文档窗口
        private System.Windows.Forms.RichTextBox _tipRichBox;
        private SysCommon.BottomQueryBar _QueryBar; //added by chulili 2012-08-09

        private SysGisDataSet _gisDataSet;           //数据库操作类 wgf
        private string _strLogFilePath;  //日志文件
        //--------------------------------------------------------
        private List<string> _ListUserPrivilegeID;                   //用户权限编号

        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        //构造函数
        public AppGidUpdate()
        {
        }

        public AppGidUpdate(Form pForm, Control ControlContainer, List<string> ListUserPrivilegeID, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath, User V_user)
        {
            //沿袭AppFormRef 参数
            _MainForm = pForm;
            _ControlContainer = ControlContainer;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;
            _user = V_user;
            _ListUserPrivilegeID = ListUserPrivilegeID;
        }

        #region IAppSMPDRef 成员
        public UserControl MainUserControl
        {
            get
            {
                return _MainUserControl;
            }
            set
            {
                _MainUserControl = value;
            }
        }

        public string Name
        {
            get
            {
                return _MainUserControl.Name;
            }
            set
            {
                _MainUserControl.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainUserControl.Tag as string;
            }
            set
            {
                _MainUserControl.Tag = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _MainUserControl.Visible;
            }
            set
            {
                _MainUserControl.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainUserControl.Enabled;
            }
            set
            {
                _MainUserControl.Enabled = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList DataTree
        {
            get
            {
                return _DataTree;
            }
            set
            {
                _DataTree = value;
            }
        }
        public object LayerTree
        {
            get
            {
                return _LayerTree; ;
            }
            set
            {
                _LayerTree = value;
            }
        }
        //added by chulili 2011-11-29 图层目录的AdvTree
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree; ;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //end added by chulili
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        public DevExpress.XtraTreeList.TreeList XZQTree
        {
            get
            {
                return _XZQTree;
            }
            set
            {
                _XZQTree = value;
            }
        }
        public DevExpress.XtraTreeList.TreeList ResultFileTree
        {
            get
            {
                return _ResultFileTree;
            }
            set
            {
                _ResultFileTree = value;
            }
        }
        public DevExpress.XtraTreeList.TreeList ErrTree
        {
            get
            {
                return _ErrTree;
            }
            set
            {
                _ErrTree = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList ProjectTree
        {
            get
            {
                return _ProjectTree;
            }
            set
            {
                _ProjectTree = value;
            }
        }

        //wgf 20110518
        //-------------------------------------------------

        public System.Windows.Forms.RichTextBox DocControl
        {
            get
            {
                return _DocControl;
            }
            set
            {
                _DocControl = value;
            }
        }
        public System.Windows.Forms.TreeView DataUnitTree
        {
            get
            {
                return _DataUnitTree;
            }
            set
            {
                _DataUnitTree = value;
            }
        }

        public System.Windows.Forms.TreeView DataIndexTree
        {
            get
            {
                return _DataIndexTree;
            }
            set
            {
                _DataIndexTree = value;
            }
        }
        public TreeView ResultsTree//制图文件目录 xisheng 20120612
        {
            get
            {
                return _ResultsTree;
            }
            set
            {
                _ResultsTree = value;
            }
        }
        public System.Windows.Forms.TreeView MapDocTree
        {
            get
            {
                return _MapDocTree;
            }
            set
            {
                _MapDocTree = value;
            }
        }

        public System.Windows.Forms.TreeView TextDocTree
        {
            get
            {
                return _TextDocTree;
            }
            set
            {
                _TextDocTree = value;
            }
        }

        public System.Windows.Forms.TreeView UserResultTree
        {
            get
            {
                return _UserResultTree;
            }
            set
            {
                _UserResultTree = value;
            }
        }
        public System.Windows.Forms.TreeView MetadataTree
        {
            get
            {
                return _MetadataTree;
            }
            set
            {
                _MetadataTree = value;
            }
        }

        public DevExpress.XtraBars.Ribbon.RibbonPage IndextabControl
        {
            get
            {
                return _IndextabControl;
            }
            set
            {
                _IndextabControl = value;
            }
        }

        public System.Windows.Forms.RichTextBox tipRichBox
        {
            get
            {
                return _tipRichBox;
            }
            set
            {
                _tipRichBox = value;
            }
        }
        public SysCommon.BottomQueryBar QueryBar
        {
            get
            {
                return _QueryBar;
            }
            set
            {
                _QueryBar = value;
            }
        }

        public string strLogFilePath
        {
            get
            {
                return _strLogFilePath;
            }
            set
            {
                _strLogFilePath = value;
            }
        }


        public SysGisDataSet gisDataSet
        {
            get
            {
                return _gisDataSet;
            }
            set
            {
                _gisDataSet = value;
            }
        }
        //--------------------------------------------------
        public DevExpress.XtraGrid.Views.Grid.GridView UpdateDataGrid
        {
            get
            {
                return _UpdateDataGrid;
            }
            set
            {
                _UpdateDataGrid = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView DataCheckGrid
        {
            get
            {
                return _DataCheckGrid;
            }
            set
            {
                _DataCheckGrid = value;
            }
        }

        public XmlDocument DBXmlDocument
        {
            get
            {
                return _DBXmlDocument;
            }
            set
            {
                _DBXmlDocument = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }
        #endregion

        #region 状态栏属性
        public string OperatorTips
        {
            get;set;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get; set;
        }
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get; set;
        }
        //当前比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get; set;
        }
        //end 褚丽丽  从别处拷贝
        //坐标显示文本框
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit _CoorTxt;
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get
            {
                return _CoorTxt;
            }
            set
            {
                _CoorTxt = value;
            }
        }

        public string UserInfo
        {
            get; set;
        }

        public string CoorXY
        {
            get; set;
        }
        #endregion

        #region IAppArcGISRef 成员
        public string CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                _CurrentTool = value;
            }
        }

        public IMapDocument MapDocument
        {
            get
            {
                return _MapDocument;
            }
            set
            {
                _MapDocument = value;
            }
        }

        public PrintDocument MyDocument
        {
            get
            {
                return _PrintDocument;
            }
            set
            {
                _PrintDocument = value;
            }
        }
        public PictureBox p1
        {
            get
            { return _p1; }
            set
            { _p1 = value; }
        }

        public PictureBox p2
        {
            get
            { return _p2; }
            set
            { _p2 = value; }
        }

        public PictureBox p3
        {
            get
            { return _p3; }
            set
            { _p3 = value; }
        }

        public PictureBox p4
        {
            get
            { return _p4; }
            set
            { _p4 = value; }
        }
        public IGeometry pGeometry
        {
            get
            {
                return _Geometry;
            }
            set
            {
                _Geometry = value;
            }
        }
        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }

        public AxMapControl ArcGisMapControl
        {
            get
            {
                return _AxMapControl;
            }
            set
            {
                _AxMapControl = value;
            }
        }

        public ISceneControlDefault SceneControlDefault
        {
            get { return _SceneControlDefault; }
            set { _SceneControlDefault = value; }
        }

        public AxSceneControl SceneControl
        {
            get { return _SceneControl; }
            set { _SceneControl = value; }
        }

        public IPageLayoutControlDefault PageLayoutControl
        {
            get
            {
                return _PageLayoutControlDefault;
            }
            set
            {
                _PageLayoutControlDefault = value;
            }
        }
        public short CurrentPrintPage
        {
            get
            {
                return _CurrentPrintPage;
            }
            set
            {
                _CurrentPrintPage = value;
            }
        }

        //public AxPageLayoutControl PageLayoutControl
        //{
        //    get
        //    {
        //        return _PageLayoutControl;
        //    }
        //    set
        //    {
        //        _PageLayoutControl = value;
        //    }
        //}

        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }

        public object CurrentControl
        {
            get
            {
                return _CurrentControl;
            }
            set
            {
                _CurrentControl = value;
            }
        }
        public List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> ScaleBoxList
        {
            get
            {
                return _scaleBoxList;
            }
            set
            {
                _scaleBoxList = value;
            }
        }

        public Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs
        {
            get
            {
                return _dicTabs;
            }
            set
            {
                _dicTabs = value;
            }
        }
        #endregion

        #region IAppFormDef 成员

        Form IAppFormRef.MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        string IAppFormRef.CurrentSysName
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }

        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end


        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }
        #endregion
    }

    public class AppFileDB : IAppFileRef, IAppFormRef
    {
        private object _LayerTree;                                      //added by chulili 20111119
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private string _LayerTreeXmlPath;                               //added by chulili 20111215
        //wgf 2011-5-18
        //--------------------------------------------------
        private System.Windows.Forms.TreeView _DataTabIndexTree;

        private System.Windows.Forms.DataGridView _GridCtrl;

        private System.Windows.Forms.RichTextBox _tipRichBox;

        private string _strLogFilePath;
        //--------------------------------------------------

        private Form _MainForm;                                        // 窗体
        private UserControl _MainUserControl;                         //控件UserControl
        private Control _StatusStrip;                                 // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;                                  // 图片资源路径
        private IMapDocument _MapDocument;                            //存储的文档对象
        private IMapControlDefault _MapControl;                      //MapControl控件
        private AxMapControl _AxMapControl;
        private ISceneControlDefault _SceneControlDefault;
        private AxSceneControl _SceneControl;

        private IPageLayoutControlDefault _PageLayoutControl;        //PageLayoutControl控件
        private ITOCControlDefault _TOCControl;                     //TOCControl控件
        private string _CurrentTool;                                   // 当前使用的TOOL名称
        //20110518
        private object _CurrentControl;                             //当前图形显示控件
        private List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> _scaleBoxList;                        //比例尺显示        
        private Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> _dicTabs;          //将tab按照系统分类
        //end


        private DevExpress.XtraTreeList.TreeList _ProjectTree;       // 工程信息树
        private DevExpress.XtraGrid.Views.Grid.GridView _DataInfoGrid;  //数据信息列表
        private DevExpress.XtraGrid.Views.Grid.GridView _MetaDataGrid;  //元数据列表

        private DevExpress.XtraTreeList.TreeList _SysSettingTree;//系统设置树图
        private DevExpress.XtraGrid.Views.Grid.GridView _SysSettingGrid;//系统设置列表

        //private DevExpress.XtraTreeList.TreeList _DataTree;          // 数据处理树图
        //private DevExpress.XtraGrid.Views.Grid.GridView _UpdateGrid;  //更新对比列表
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit _txtDisplayPage;//更新对比列表分页显示文本框
        private DevExpress.XtraBars.BarButtonItem _btnFirst;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnLast;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnPre;//更新对比列表分页显示按钮
        private DevExpress.XtraBars.BarButtonItem _btnNext;//更新对比列表分页显示按钮

        private Thread _CurrentThread;                           //处理数据的进程(唯一)
        private XmlDocument _DBXmlDocument;                       //数据工程配置XML文件
        private User _user;                                          //连接的用户信息

        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end

        private List<string> _ListUserPrivilegeID;                   //用户权限编号

        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        //构造函数
        public AppFileDB()
        {
        }

        public AppFileDB(Form pForm, Control ControlContainer, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath)
        {
            //沿袭AppFormRef 参数
            _MainForm = pForm;
            _ControlContainer = ControlContainer;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;
        }


        #region IAppFileRef 成员

        public UserControl MainUserControl
        {
            get
            {
                return _MainUserControl;
            }
            set
            {
                _MainUserControl = value;
            }
        }


        public DevExpress.XtraTreeList.TreeList ProjectTree
        {
            get
            {
                return _ProjectTree;
            }
            set
            {
                _ProjectTree = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView DataInfoGrid
        {
            get
            {
                return _DataInfoGrid;
            }
            set
            {
                _DataInfoGrid = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView MetaDataGrid
        {
            get
            {
                return _MetaDataGrid;
            }
            set
            {
                _MetaDataGrid = value;
            }
        }

        public DevExpress.XtraGrid.Views.Grid.GridView SysSettingGrid
        {
            get
            {
                return _SysSettingGrid;
            }
            set
            {
                _SysSettingGrid = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList SysSettingTree
        {
            get
            {
                return _SysSettingTree;
            }
            set
            {
                _SysSettingTree = value;
            }
        }

        public XmlDocument DBXmlDocument
        {
            get
            {
                return _DBXmlDocument;
            }
            set
            {
                _DBXmlDocument = value;
            }
        }


        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtDisplayPage
        {
            get
            {
                return _txtDisplayPage;
            }
            set
            {
                _txtDisplayPage = value;
            }
        }

        public DevExpress.XtraBars.BarButtonItem BtnFirst
        {
            get
            {
                return _btnFirst;
            }
            set
            {
                _btnFirst = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnLast
        {
            get
            {
                return _btnLast;
            }
            set
            {
                _btnLast = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnPre
        {
            get
            {
                return _btnPre;
            }
            set
            {
                _btnPre = value;
            }
        }
        public DevExpress.XtraBars.BarButtonItem BtnNext
        {
            get
            {
                return _btnNext;
            }
            set
            {
                _btnNext = value;
            }
        }



        public string Name
        {
            get
            {
                return _MainUserControl.Name;
            }
            set
            {
                _MainUserControl.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainUserControl.Tag as string;
            }
            set
            {
                _MainUserControl.Tag = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _MainUserControl.Visible;
            }
            set
            {
                _MainUserControl.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainUserControl.Enabled;
            }
            set
            {
                _MainUserControl.Enabled = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }
        #endregion

        #region 状态栏属性
        public string OperatorTips
        {
            get;set;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get; set;
        }

        //参考比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get; set;
        }
        //当前比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get; set;
        }
        //坐标显示文本框
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get; set;
        }


        public string CoorXY
        {
            get; set;
        }

        public string UserInfo
        {
            get; set;
        }

        #endregion

        #region IAppArcGISRef 成员
        public string CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                _CurrentTool = value;
            }
        }

        public IMapDocument MapDocument
        {
            get
            {
                return _MapDocument;
            }
            set
            {
                _MapDocument = value;
            }
        }

        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }
        public PictureBox p1 { get; set; }
        public PictureBox p2 { get; set; }
        public PictureBox p3 { get; set; }
        public PictureBox p4 { get; set; }
        public AxMapControl ArcGisMapControl
        {
            get
            {
                return _AxMapControl;
            }
            set
            {
                _AxMapControl = value;
            }
        }

        public ISceneControlDefault SceneControlDefault
        {
            get { return _SceneControlDefault; }
            set { _SceneControlDefault = value; }
        }

        public AxSceneControl SceneControl
        {
            get { return _SceneControl; }
            set { _SceneControl = value; }
        }

        public IPageLayoutControlDefault PageLayoutControl
        {
            get
            {
                return _PageLayoutControl;
            }
            set
            {
                _PageLayoutControl = value;
            }
        }

        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }

        //cyf 20110518  
        public object CurrentControl
        {
            get
            {
                return _CurrentControl;
            }
            set
            {
                _CurrentControl = value;
            }
        }
        public List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> ScaleBoxList
        {
            get
            {
                return _scaleBoxList;
            }
            set
            {
                _scaleBoxList = value;
            }
        }

        public Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs
        {
            get
            {
                return _dicTabs;
            }
            set
            {
                _dicTabs = value;
            }
        }
        //end

        #endregion

        #region IAppFormDef 成员

        Form IAppFormRef.MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        string IAppFormRef.CurrentSysName
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }

        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end

        public object LayerTree
        {
            get
            {
                return _LayerTree;
            }
            set
            {
                _LayerTree = value;
            }
        }
        //added by chulili 2011-11-29 图层目录的AdvTree
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree; ;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //added by chulili 2011-12-15 图层目录配置文件路径
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        //end added by chulili
        //wgf 2011-5-18
        //--------------------------------------------
        public System.Windows.Forms.TreeView DataTabIndexTree
        {
            get
            {
                return _DataTabIndexTree;
            }
            set
            {
                _DataTabIndexTree = value;
            }
        }
        public System.Windows.Forms.DataGridView GridCtrl
        {
            get
            {
                return _GridCtrl;
            }
            set
            {
                _GridCtrl = value;
            }
        }

        public System.Windows.Forms.RichTextBox tipRichBox
        {
            get
            {
                return _tipRichBox;
            }
            set
            {
                _tipRichBox = value;
            }
        }

        public string strLogFilePath
        {
            get
            {
                return _strLogFilePath;
            }
            set
            {
                _strLogFilePath = value;
            }
        }

        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }
        //---------------------------------------------

        #endregion
    }
    /// <summary>
    /// 数据库集成管理子系统接口类实现 陈亚飞添加  20100927
    /// </summary>
    public class AppDBIntegra : IAppDBIntegraRef, IAppFormRef
    {
        private object _LayerAdvTree;  //目录树图 用来存放advTree
        private object _LayerTree;  //目录树图 added by chulili 20111119
        private string _LayerTreeXmlPath;                               //added by chulili 20111215
        private Form _MainForm;                                        // 窗体
        private UserControl _MainUserControl;                         //控件UserControl
        private Control _StatusStrip;                                 // 状态栏
        private Control _ControlContainer;                             // 控件容器
        private XmlDocument _SystemXml;                                // 系统功能树图Xml节点
        private XmlDocument _DataTreeXml;                              // 数据树图XML节点
        private XmlDocument _DatabaseInfoXml;                          // 连接信息XML节点
        private Plugin.Parse.PluginCollection _ColParsePlugin;         // 系统插件集合
        private Dictionary<string, System.Windows.Forms.ContextMenuStrip> _DicContextMenu;           //右键菜单集合
        private string _ImageResPath;                                  // 图片资源路径
        private IMapDocument _MapDocument;                            //存储的文档对象
        private IMapControlDefault _MapControl;                      //MapControl控件
        private AxMapControl _AxMapControl;
        private ISceneControlDefault _SceneControlDefault;
        private AxSceneControl _SceneControl;
        private IPageLayoutControlDefault _PageLayoutControl;        //PageLayoutControl控件
        private ITOCControlDefault _TOCControl;                     //TOCControl控件
        private string _CurrentTool;                                   // 当前使用的TOOL名称
        private DevExpress.XtraTreeList.TreeList _ProjectTree;       // 工程信息树图
        //private DevExpress.XtraGrid.Views.Grid.GridView _UpdateDataGrid;   //更新对比表格

        //private DevExpress.XtraGrid.Views.Grid.GridView _DataCheckGrid;   //数据检查表格
        private XmlDocument _DBXmlDocument;                       //数据工程配置XML文件

        private Thread _CurrentThread;                           //处理数据的进程(唯一)

        private User _user;                                          //连接的用户信息
        //cyf 20110602 add
        private List<Role> _LstRoleInfo;                             //连接的用户对应的角色信息
        //end

        //20110518
        private object _CurrentControl;                             //当前图形显示控件 
        private List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> _scaleBoxList;                        //比例尺显示        
        private Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> _dicTabs;          //将tab按照系统分类
        //end

        private List<string> _ListUserPrivilegeID;                   //用户权限编号

        //正式库
        private ICustomWks _CurWks;
        //临时库
        private ICustomWks _TempWks;

        //构造函数
        public AppDBIntegra()
        {
        }

        public AppDBIntegra(Form pForm, Control ControlContainer, XmlDocument SystemXml, XmlDocument DataTreeXml, XmlDocument DatabaseInfoXml, Plugin.Parse.PluginCollection ColParsePlugin, string ImageResPath, User v_user, List<Role> v_ListRole)
        {
            //沿袭AppFormRef 参数
            _MainForm = pForm;
            _ControlContainer = ControlContainer;
            _SystemXml = SystemXml;
            _DataTreeXml = DataTreeXml;
            _DatabaseInfoXml = DatabaseInfoXml;
            _ColParsePlugin = ColParsePlugin;
            _ImageResPath = ImageResPath;
            _user = v_user;
            _LstRoleInfo = v_ListRole;
        }

        #region IAppDBIntegraRef 成员
        public UserControl MainUserControl
        {
            get
            {
                return _MainUserControl;
            }
            set
            {
                _MainUserControl = value;
            }
        }
        public object LayerTree
        {
            get
            {
                return _LayerTree; ;
            }
            set
            {
                _LayerTree = value;
            }
        }
        public object LayerAdvTree
        {
            get
            {
                return _LayerAdvTree;
            }
            set
            {
                _LayerAdvTree = value;
            }
        }
        //added by chulili 2011-12-15 图层目录配置文件路径
        public string LayerTreeXmlPath
        {
            get
            {
                return _LayerTreeXmlPath; ;
            }
            set
            {
                _LayerTreeXmlPath = value;
            }
        }
        public string Name
        {
            get
            {
                return _MainUserControl.Name;
            }
            set
            {
                _MainUserControl.Name = value;
            }
        }

        public string Caption
        {
            get
            {
                return _MainUserControl.Tag as string;
            }
            set
            {
                _MainUserControl.Tag = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _MainUserControl.Visible;
            }
            set
            {
                _MainUserControl.Visible = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return _MainUserControl.Enabled;
            }
            set
            {
                _MainUserControl.Enabled = value;
            }
        }

        public Dictionary<string, System.Windows.Forms.ContextMenuStrip> DicContextMenu
        {
            get
            {
                return _DicContextMenu;
            }
            set
            {
                _DicContextMenu = value;
            }
        }

        public DevExpress.XtraTreeList.TreeList ProjectTree
        {
            get
            {
                return _ProjectTree;
            }
            set
            {
                _ProjectTree = value;
            }
        }

        public XmlDocument DBXmlDocument
        {
            get
            {
                return _DBXmlDocument;
            }
            set
            {
                _DBXmlDocument = value;
            }
        }

        public Control StatusBar
        {
            get
            {
                return _StatusStrip;
            }
            set
            {
                _StatusStrip = value;
            }
        }

        public Thread CurrentThread
        {
            get
            {
                return _CurrentThread;
            }
            set
            {
                _CurrentThread = value;
            }
        }
        #endregion

        #region 状态栏属性
        public string OperatorTips
        {
            get;set;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemProgressBar ProgressBar
        {
            get; set;
        }
        //参考比例尺是否可见
        public bool RefScaleVisible
        {
            get; set;
        }
        //当前比例尺是否可见
        public bool CurScaleVisible
        {
            get; set;
        }
        //参考比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox RefScaleCmb
        {
            get; set;
        }
        //当前比例尺cmb
        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox CurScaleCmb
        {
            get; set;
        }
        //坐标显示文本框
        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit CoorTxt
        {
            get; set;
        }

        public string UserInfo
        {
            get; set;
        }

        //public string CoorXY
        //{
        //    get
        //    {
        //        return string.Empty;
        //    }
        //    set
        //    {
        //    }
        //}
        //changed by chulili 20110722
        public string CoorXY
        {
            get; set;
        }
        #endregion

        #region IAppArcGISRef 成员
        public string CurrentTool
        {
            get
            {
                return _CurrentTool;
            }
            set
            {
                _CurrentTool = value;
            }
        }

        public IMapDocument MapDocument
        {
            get
            {
                return _MapDocument;
            }
            set
            {
                _MapDocument = value;
            }
        }

        public IMapControlDefault MapControl
        {
            get
            {
                return _MapControl;
            }
            set
            {
                _MapControl = value;
            }
        }

        public PictureBox p1 { get; set; }
        public PictureBox p2 { get; set; }
        public PictureBox p3 { get; set; }
        public PictureBox p4 { get; set; }
        public AxMapControl ArcGisMapControl
        {
            get
            {
                return _AxMapControl;
            }
            set
            {
                _AxMapControl = value;
            }
        }

        public ISceneControlDefault SceneControlDefault
        {
            get { return _SceneControlDefault; }
            set { _SceneControlDefault = value; }
        }

        public AxSceneControl SceneControl
        {
            get { return _SceneControl; }
            set { _SceneControl = value; }
        }

        public IPageLayoutControlDefault PageLayoutControl
        {
            get
            {
                return _PageLayoutControl;
            }
            set
            {
                _PageLayoutControl = value;
            }
        }

        public ITOCControlDefault TOCControl
        {
            get
            {
                return _TOCControl;
            }
            set
            {
                _TOCControl = value;
            }
        }

        //cyf 20110518  
        public object CurrentControl
        {
            get
            {
                return _CurrentControl;
            }
            set
            {
                _CurrentControl = value;
            }
        }
        public List<DevExpress.XtraEditors.Repository.RepositoryItemComboBox> ScaleBoxList
        {
            get
            {
                return _scaleBoxList;
            }
            set
            {
                _scaleBoxList = value;
            }
        }

        public Dictionary<DevExpress.XtraBars.Ribbon.RibbonPage, string> DicTabs
        {
            get
            {
                return _dicTabs;
            }
            set
            {
                _dicTabs = value;
            }
        }
        //end

        #endregion

        #region IAppFormDef 成员

        Form IAppFormRef.MainForm
        {
            get
            {
                return _MainForm;
            }
            set
            {
                _MainForm = value;
            }
        }

        public Control ControlContainer
        {
            get
            {
                return _ControlContainer;
            }
            set
            {
                _ControlContainer = value;
            }
        }

        string IAppFormRef.CurrentSysName
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public XmlDocument SystemXml
        {
            get
            {
                return _SystemXml;
            }
            set
            {
                _SystemXml = value;
            }
        }

        public XmlDocument DataTreeXml
        {
            get
            {
                return _DataTreeXml;
            }
            set
            {
                _DataTreeXml = value;
            }
        }

        public XmlDocument DatabaseInfoXml
        {
            get
            {
                return _DatabaseInfoXml;
            }
            set
            {
                _DatabaseInfoXml = value;
            }
        }

        public Plugin.Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
            set
            {
                _ColParsePlugin = value;
            }
        }

        public string ImageResPath
        {
            get
            {
                return _ImageResPath;
            }
            set
            {
                _ImageResPath = value;
            }
        }

        public User ConnUser
        {
            get { return _user; }
            set { _user = value; }
        }

        //cyf 20110602 add:连接用户对应的角色的属性
        public List<Role> LstRoleInfo
        {
            get { return _LstRoleInfo; }
            set { _LstRoleInfo = value; }
        }
        //end


        public List<string> ListUserPrivilegeID
        {
            get
            {
                return _ListUserPrivilegeID;
            }
            set
            {
                _ListUserPrivilegeID = value;
            }
        }

        //数据连接信息
        public ICustomWks CurWksInfo
        {
            get
            {
                return _CurWks;
            }
            set
            {
                _CurWks = value;
            }
        }
        public ICustomWks TempWksInfo
        {
            get
            {
                return _TempWks;
            }
            set
            {
                _TempWks = value;
            }
        }
        #endregion
    }
    #endregion
}
