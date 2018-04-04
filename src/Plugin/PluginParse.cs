using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

using Fan.Plugin.Interface;

namespace Fan.Plugin.Parse
{
    #region 插件接口获取访问与动态加载、解析
    //插件接口容器
    public class PluginCollection : CollectionBase
    {
        //构造函数
        public PluginCollection() { }
        public PluginCollection(PluginCollection value)
        {
            this.AddRange(value);
        }
        public PluginCollection(IPlugin[] value)
        {
            this.AddRange(value);
        }
        public PluginCollection(IPlugin value)
        {
            this.Add(value);
        }
        public IPlugin this[int index]
        {
            get
            {
                return (IPlugin)(this.List[index]);
            }
        }
        public void AddRange(IPlugin[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }
        public void AddRange(PluginCollection value)
        {
            for (int i = 0; i < value.Capacity; i++)
            {
                this.Add((IPlugin)value.List[i]);
            }
        }
        public int Add(IPlugin value)
        {
            return this.List.Add(value);
        }
        public void Insert(int index, IPlugin value)
        {
            this.List.Insert(index, value);
        }
        public void Remove(IPlugin value)
        {
            this.List.Remove(value);
        }
        public int IndexOf(IPlugin value)
        {
            return this.List.IndexOf(value);
        }
        public bool Contains(IPlugin value)
        {
            return this.List.Contains(value);
        }
        public void CopyTo(IPlugin[] value, int index)
        {
            this.CopyTo(value, index);
        }
        public IPlugin[] ToArray()
        {
            IPlugin[] array = new IPlugin[this.Count];
            this.CopyTo(array, 0);
            return array;
        }
        public new PluginCollectionEnumerator GetEnumerator()
        {
            return new PluginCollectionEnumerator(this);
        }
    }
    /// <summary>
    /// 插件接口迭代器
    /// </summary>
    public class PluginCollectionEnumerator : IEnumerator
    {
        private IEnumerable temp;
        private IEnumerator enumerator;
        public PluginCollectionEnumerator(PluginCollection PluginCollection)
        {
            temp = (IEnumerable)PluginCollection;
            enumerator = temp.GetEnumerator();
        }
        public object Current
        {
            get { return enumerator.Current; }
        }
        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }
        public void Reset()
        {
            enumerator.Reset();
        }
    }
    /// <summary>
    /// 插件接口分类解析
    /// </summary>
    public class ParsePluginCol
    {
        public ParsePluginCol(PluginCollection pluginCol)
        {
            m_AllPlugin.ArraylstCommandCategory = new ArrayList();
            m_AllPlugin.dicCommands = new Dictionary<string, ICommandRef>();
            m_AllPlugin.dicControls = new Dictionary<string, IControlRef>();
            m_AllPlugin.dicDockableWindows = new Dictionary<string, IDockableWindowRef>();
            m_AllPlugin.dicMenus = new Dictionary<string, IMenuRef>();
            m_AllPlugin.dicPlugins = new Dictionary<string, IPlugin>();
            m_AllPlugin.dicToolBars = new Dictionary<string, IToolBarRef>();
            m_AllPlugin.dicTools = new Dictionary<string, IToolRef>();
            GetPluginArray(pluginCol);
        }
        private PluginStruct m_AllPlugin = new PluginStruct(); 
        public PluginStruct AllPlugin
        {
            get { return m_AllPlugin; }
        }
        private void GetPluginArray(PluginCollection pluginCol)
        {
            if (pluginCol == null) return;
            foreach (IPlugin plugin in pluginCol)
            {
                try
                {
                    if (!m_AllPlugin.dicPlugins.ContainsKey(plugin.Name))
                    {
                        m_AllPlugin.dicPlugins.Add(plugin.Name, plugin);
                    }
                    ICommandRef cmd = plugin as ICommandRef;
                    if (cmd != null)
                    {
                        if (!m_AllPlugin.dicCommands.ContainsKey(cmd.ToString()))
                        {
                            m_AllPlugin.dicCommands.Add(cmd.Name, cmd);
                        }

                        if (cmd.Category != null)
                        {
                            if (!m_AllPlugin.ArraylstCommandCategory.Contains(cmd.Category))
                            {
                                m_AllPlugin.ArraylstCommandCategory.Add(cmd.Category);
                            }
                        }
                    }
                    IToolRef atool = plugin as IToolRef;
                    if (atool != null)
                    {
                        if (!m_AllPlugin.dicTools.ContainsKey(atool.Name))
                        {
                            m_AllPlugin.dicTools.Add(atool.Name, atool);
                        }

                        if (atool.Category != null)
                        {
                            if (!m_AllPlugin.ArraylstCommandCategory.Contains(atool.Category))
                            {
                                m_AllPlugin.ArraylstCommandCategory.Add(atool.Category);
                            }
                        }
                    }
                    IMenuRef aMenu = plugin as IMenuRef;
                    if (aMenu != null)
                    {
                        if (!m_AllPlugin.dicMenus.ContainsKey(aMenu.Name))
                        {
                            m_AllPlugin.dicMenus.Add(aMenu.Name, aMenu);
                        }
                    }
                    IToolBarRef aToolBar = plugin as IToolBarRef;
                    if (aToolBar != null)
                    {
                        if (!m_AllPlugin.dicToolBars.ContainsKey(aToolBar.Name))
                        {
                            m_AllPlugin.dicToolBars.Add(aToolBar.Name, aToolBar);
                        }
                    }
                    IDockableWindowRef aDockableWindow = plugin as IDockableWindowRef;
                    if (aDockableWindow != null)
                    {
                        if (!m_AllPlugin.dicDockableWindows.ContainsKey(aDockableWindow.Name))
                        {
                            m_AllPlugin.dicDockableWindows.Add(aDockableWindow.Name, aDockableWindow);
                        }
                    }
                    IControlRef aControl = plugin as IControlRef;
                    if (aControl != null)
                    {
                        if (!m_AllPlugin.dicControls.ContainsKey(aControl.Name))
                        {
                            m_AllPlugin.dicControls.Add(aControl.Name, aControl);
                        }
                    }
                }
                catch (Exception err)
                {
                    DataBase.Log.LogManager.WriteSysLog(err, string.Format("Function Name:ParsePluginCol.GetPluginArray"));
                }
            }
        }
    }
    /// <summary>
    /// 根据反射机制获取插件并装入插件接口容器
    /// </summary>
    public class PluginHandle
    {
        public PluginHandle(string strPluginFolder)
        {
            _pluginFolder = strPluginFolder;
        }
        private string _pluginFolder;
        public string PluginFolderPath
        {
            get { return _pluginFolder;}
        }
        private void GetPluginObject(PluginCollection PluginCol, Type type)
        {
            IPlugin plugin = null;
            try
            {
                plugin = Activator.CreateInstance(type) as IPlugin;
                if (plugin != null)
                {
                    if (!PluginCol.Contains(plugin))
                    {
                        PluginCol.Add(plugin);
                    }
                }
            }
            catch (Exception ex)
            {
                DataBase.Log.LogManager.WriteSysLog(ex, string.Format("Function Name:PluginParse.GetPluginObject"));
            }
        }
        public PluginCollection GetPluginFromDLL()
        {
            //插件接口容器
            PluginCollection PluginCol = new PluginCollection();
            //检查插件文件夹是否存在
            if (!Directory.Exists(_pluginFolder))
            {
                Directory.CreateDirectory(_pluginFolder);
            }
            //获取DLL
            string[] dllFiles = Directory.GetFiles(_pluginFolder, "*.dll");
            foreach (string file in dllFiles)
            {
                Assembly assembly = Assembly.LoadFrom(file);
                if (assembly != null)
                {
                    Type[] types = null;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch(Exception ex)
                    {
                        DataBase.Log.LogManager.WriteSysLog(ex, string.Format("Function Name:PluginParse.GetPluginFromDLL"));
                    }
                    if (types == null) continue;
                    foreach (Type type in types)
                    {
                        Type[] interfaces = type.GetInterfaces();
                        foreach (Type ainterface in interfaces)
                        {
                            switch (ainterface.FullName)
                            {
                                case "Fan.Plugin.Interface.ICommandRef":
                                case "Fan.Plugin.Interface.IToolRef":
                                case "Fan.Plugin.Interface.IMenuRef":
                                case "Fan.Plugin.Interface.IToolBarRef":
                                case "Fan.Plugin.Interface.IDockableWindowRef":
                                case "Fan.Plugin.Interface.IControlRef":
                                    GetPluginObject(PluginCol, type);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            return PluginCol;
        }
    }
    #endregion
    public enum PluginControlType
    {
        RibbonPageCategory=1,
        RibbonPage=2,
        RibbonPageGroup=3,
        BarButtonItem=4,
    }
    public struct PluginStruct
    {
        public Dictionary<string, IPlugin> dicPlugins;
        public Dictionary<string, ICommandRef> dicCommands;
        public Dictionary<string, IToolRef> dicTools;
        public Dictionary<string, IMenuRef> dicMenus;
        public Dictionary<string, IToolBarRef> dicToolBars;
        public Dictionary<string, IDockableWindowRef> dicDockableWindows;
        public Dictionary<string, IControlRef> dicControls;
        public ArrayList ArraylstCommandCategory;
    }
}
