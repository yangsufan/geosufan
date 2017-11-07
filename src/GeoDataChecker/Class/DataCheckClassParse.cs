using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml;

namespace GeoDataChecker
{
    //插件接口容器
    public class DataCheckPluginCollection : CollectionBase
    {
        //构造函数
        public DataCheckPluginCollection() { }
        public DataCheckPluginCollection(DataCheckPluginCollection value)
        {
            this.AddRange(value);
        }
        public DataCheckPluginCollection(IDataCheck[] value)
        {
            this.AddRange(value);
        }
        public DataCheckPluginCollection(IDataCheck value)
        {
            this.Add(value);
        }

        public IDataCheck this[int index]
        {
            get
            {
                return (IDataCheck)(this.List[index]);
            }
        }

        public void AddRange(IDataCheck[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(DataCheckPluginCollection value)
        {
            for (int i = 0; i < value.Capacity; i++)
            {
                this.Add((IDataCheck)value.List[i]);
            }
        }

        public int Add(IDataCheck value)
        {
            return this.List.Add(value);
        }

        public void Insert(int index, IDataCheck value)
        {
            this.List.Insert(index, value);
        }

        public void Remove(IDataCheck value)
        {
            this.List.Remove(value);
        }

        public int IndexOf(IDataCheck value)
        {
            return this.List.IndexOf(value);
        }

        public bool Contains(IDataCheck value)
        {
            return this.List.Contains(value);
        }

        public void CopyTo(IDataCheck[] value, int index)
        {
            this.CopyTo(value, index);
        }

        public IDataCheck[] ToArray()
        {
            IDataCheck[] array = new IDataCheck[this.Count];
            this.CopyTo(array, 0);
            return array;
        }

        public new DataCheckPluginCollectionEnumerator GetEnumerator()
        {
            return new DataCheckPluginCollectionEnumerator(this);
        }
    }

    //插件接口迭代器
    public class DataCheckPluginCollectionEnumerator : IEnumerator
    {
        private IEnumerable temp;
        private IEnumerator enumerator;

        public DataCheckPluginCollectionEnumerator(DataCheckPluginCollection PluginCollection)
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
    //根据反射机制获取插件并装入插件接口容器
    public class DataCheckPluginHandle
    {
        private string _pluginFolder;
        private string _searchPattern;
        public DataCheckPluginHandle(string pluginFolder, string searchPattern)
        {
            _pluginFolder = pluginFolder;
            _searchPattern = searchPattern;
        }

        private void GetPluginObject(DataCheckPluginCollection PluginCol, Type type)
        {
            IDataCheck plugin = null;
            try
            {
                plugin = Activator.CreateInstance(type) as IDataCheck;
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

        public DataCheckPluginCollection GetPluginFromDLL()
        {
            //插件接口容器
            DataCheckPluginCollection PluginCol = new DataCheckPluginCollection();

            //检查插件文件夹是否存在
            if (!Directory.Exists(_pluginFolder))
            {
                Directory.CreateDirectory(_pluginFolder);
            }

            //获取DLL
            string[] dllFiles = Directory.GetFiles(_pluginFolder, _searchPattern);
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
                                case "GeoDataChecker.IDataCheckRealize":
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
}
