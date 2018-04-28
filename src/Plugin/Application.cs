using System;
using System.Windows.Forms;
using Fan.Common;
using Fan.Plugin.Parse;
namespace Fan.Plugin.Application
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
        /// 主窗体
        /// </summary>
        BaseRibbonForm MainForm { get;}
        /// <summary>
        /// 窗体名称
        /// </summary>
        string Name { get;}
        /// <summary>
        /// 窗体标题
        /// </summary>
        string Caption { get; }
        /// <summary>
        /// 系统插件集合
        /// </summary>
        PluginCollection ColParsePlugin { get; }
        /// <summary>
        /// 登陆的用户
        /// </summary>
        User ConnUser { get;  }
    }
    #endregion
    #region 接口实现类
    public class AppForm : IAppFormRef
    {
        private BaseRibbonForm _MainForm;                                      
        private PluginCollection _ColParsePlugin;      
        public AppForm()
        {

        }
        public AppForm(BaseRibbonForm MainForm, PluginCollection ColParsePlugin):this()
        {
            _MainForm = MainForm;
            _ColParsePlugin = ColParsePlugin;
        }
        #region AppForm 成员
        public BaseRibbonForm MainForm
        {
            get{return _MainForm;}
        }
        public string Name
        {
            get{return _MainForm.Name;}
        }
        public string Caption
        {
            get{return _MainForm.Text;}
        }
        public Parse.PluginCollection ColParsePlugin
        {
            get
            {
                return _ColParsePlugin;
            }
        }
        public User ConnUser
        {
            get;
        }
        #endregion
    }
    #endregion
}
