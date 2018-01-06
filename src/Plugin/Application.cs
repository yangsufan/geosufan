using System;
using System.Windows.Forms;
using Fan.Common;
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
        /// 窗体
        /// </summary>
        Form MainForm { get;}
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
        Parse.PluginCollection ColParsePlugin { get; set; }
        /// <summary>
        /// 登陆的用户
        /// </summary>
        User ConnUser { get;  }
    }
    #endregion
    #region 接口实现类
    public class AppForm : IAppFormRef
    {
        private Form _MainForm;                                      
        private Parse.PluginCollection _ColParsePlugin;      
        public AppForm()
        {

        }
        public AppForm(Form MainForm, Parse.PluginCollection ColParsePlugin):this()
        {
            _MainForm = MainForm;
            _ColParsePlugin = ColParsePlugin;
        }
        #region IDefAppForm 成员
        public Form MainForm
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
            set
            {
                _ColParsePlugin = value;
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
