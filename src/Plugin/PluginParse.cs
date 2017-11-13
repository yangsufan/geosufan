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

namespace Plugin.Parse
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

    //插件接口迭代器
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

    //插件接口分类解析
    public class ParsePluginCol
    {
        // plugin集合
        private Dictionary<string, IPlugin> dicPlugins = new Dictionary<string, IPlugin>();

        //command集合
        private Dictionary<string, ICommandRef> dicCommands = new Dictionary<string, ICommandRef>();

        //tool集合
        private Dictionary<string, IToolRef> dicTools = new Dictionary<string, IToolRef>();

        //memu集合
        private Dictionary<string, IMenuRef> dicMenus = new Dictionary<string, IMenuRef>();

        //toolbar集合
        private Dictionary<string, IToolBarRef> dicToolBars = new Dictionary<string, IToolBarRef>();

        //DockableWindow集合
        private Dictionary<string, IDockableWindowRef> dicDockableWindows = new Dictionary<string, IDockableWindowRef>();

        //control集合
        private Dictionary<string, IControlRef> dicControls = new Dictionary<string, IControlRef>();

        //类型集合
        private ArrayList ArraylstCommandCategory = new ArrayList();

        public Dictionary<string, IPlugin> GetPlugins
        {
            get { return this.dicPlugins; }
        }

        public Dictionary<string, ICommandRef> GetCommands
        {
            get { return this.dicCommands; }
        }

        public Dictionary<string, IToolRef> GetTools
        {
            get { return this.dicTools; }
        }

        public Dictionary<string, IMenuRef> GetMenus
        {
            get { return this.dicMenus; }
        }

        public Dictionary<string, IToolBarRef> GetToolBars
        {
            get { return this.dicToolBars; }
        }

        public Dictionary<string, IDockableWindowRef> GetDockableWindows
        {
            get { return this.dicDockableWindows; }
        }

        public Dictionary<string, IControlRef> GetControls
        {
            get { return this.dicControls; }
        }

        public ArrayList GetCommandCategory
        {
            get { return this.ArraylstCommandCategory; }
        }

        public void GetPluginArray(PluginCollection pluginCol)
        {

            foreach (IPlugin plugin in pluginCol)
            {
                try
                {
                    if (!dicPlugins.ContainsKey(plugin.ToString()))
                    {
                        dicPlugins.Add(plugin.ToString(), plugin);
                    }

                    ICommandRef cmd = plugin as ICommandRef;
                    if (cmd != null)
                    {
                        if (!dicCommands.ContainsKey(cmd.ToString()))
                        {
                            dicCommands.Add(cmd.ToString(), cmd);
                        }

                        if (cmd.Category != null)
                        {
                            if (!ArraylstCommandCategory.Contains(cmd.Category))
                            {
                                ArraylstCommandCategory.Add(cmd.Category);
                            }
                        }
                    }


                    IToolRef atool = plugin as IToolRef;
                    if (atool != null)
                    {
                        if (!dicTools.ContainsKey(atool.ToString()))
                        {
                            dicTools.Add(atool.ToString(), atool);
                        }

                        if (atool.Category != null)
                        {
                            if (!ArraylstCommandCategory.Contains(atool.Category))
                            {
                                ArraylstCommandCategory.Add(atool.Category);
                            }
                        }
                    }


                    IMenuRef aMenu = plugin as IMenuRef;
                    if (aMenu != null)
                    {
                        if (!dicMenus.ContainsKey(aMenu.ToString()))
                        {
                            dicMenus.Add(aMenu.ToString(), aMenu);
                        }
                    }

                    IToolBarRef aToolBar = plugin as IToolBarRef;
                    if (aToolBar != null)
                    {
                        if (!dicToolBars.ContainsKey(aToolBar.ToString()))
                        {
                            dicToolBars.Add(aToolBar.ToString(), aToolBar);
                        }
                    }

                    IDockableWindowRef aDockableWindow = plugin as IDockableWindowRef;
                    if (aDockableWindow != null)
                    {
                        if (!dicDockableWindows.ContainsKey(aDockableWindow.ToString()))
                        {
                            dicDockableWindows.Add(aDockableWindow.ToString(), aDockableWindow);
                        }
                    }

                    IControlRef aControl = plugin as IControlRef;
                    if (aControl != null)
                    {
                        if (!dicControls.ContainsKey(aControl.ToString()))
                        {
                            dicControls.Add(aControl.ToString(), aControl);
                        }
                    }
                }
                catch (Exception err)
                {
                    SysCommon.ModSysSetting.WriteLog("GetPluginArray 函数内错误，信息：" + err.Message);//@日志测试
                }
            }
        }
    }

    //根据反射机制获取插件并装入插件接口容器
    public class PluginHandle
    {
        private string _pluginFolder;
        public string PluginFolderPath
        {
            get
            {
                return _pluginFolder;
            }
            set
            {
                _pluginFolder = value;
            }
        }

        private void GetPluginObject(PluginCollection PluginCol, Type type)
        {
            IPlugin plugin = null;
            try
            {
                plugin = Activator.CreateInstance(type) as IPlugin;
            }
            catch
            {
            }
            finally
            {
                if (plugin != null)
                {
                    if (!PluginCol.Contains(plugin))
                    {
                        PluginCol.Add(plugin);
                    }
                }
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
                    catch
                    {

                    }

                    if (types == null) continue;
                    foreach (Type type in types)
                    {
                        Type[] interfaces = type.GetInterfaces();
                        foreach (Type ainterface in interfaces)
                        {
                            switch (ainterface.FullName)
                            {
                                case "Plugin.Interface.ICommandRef":
                                case "Plugin.Interface.IToolRef":
                                case "Plugin.Interface.IMenuRef":
                                case "Plugin.Interface.IToolBarRef":
                                case "Plugin.Interface.IDockableWindowRef":
                                case "Plugin.Interface.IControlRef":
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
}
