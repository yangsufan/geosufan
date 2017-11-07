using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.IO;
//using ESRI.ArcGIS.MapControl;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;

namespace GeoDataExport
{
    public partial class frmAddData : DevComponents.DotNetBar.Office2007Form
    {
        private object[] DataName;
       // private IHookHelper m_pHookHelper;
        private IMap m_pMap;
        private IActiveView m_pActiveView;
        
        public IFeatureLayer m_pFeaturelayer;
        public frmAddData(IMap pmap)
        {
           // m_pHookHelper = pHookHelper;
            m_pMap = pmap;
            m_pActiveView = (IActiveView)pmap;
            //m_pFeaturelayer = pfeaturelayer;
            InitializeComponent();
            
        }
        /// <summary>
        /// 加载目录树
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="directoryInfo"></param>
        public void LoadDirectories(DevComponents.AdvTree.Node Parent,DirectoryInfo directoryInfo)
        {
            //获取当前文件夹的子文件夹
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directoryInfos)
            {
                //如果属性是隐藏，继续下面的操作
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();//创建node节点，并初始化
                if (dir.Name.Contains(".gdb"))//如果是gdb类型文件夹，则按如下方式添加节点
                {
                    node.Tag = dir;
                    node.Name = dir.Name;
                    node.Text = dir.Name.Substring(0,dir.Name.Length-4);
                    node.ImageIndex = 7;
                    Parent.Nodes.Add(node);
                }
                else//非gdb类型文件夹，如下方式添加节点
                {
                    node.Tag = dir;
                    node.Name = dir.Name;
                    node.Text = dir.Name;
                    node.ImageIndex = 5;
                    node.ImageExpandedIndex = 6;
                    Parent.Nodes.Add(node);//父节点加载子节点
                }
                node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Visible;//设置节点的可以展开
            }
            //获取当前文件夹的文件
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                //如果文件的扩展名不为空，则继续下面操作
                if (file.Extension != "" && file.Extension != null)
                {
                    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                    //获取文件扩展名并转换为小写
                    string fileExtension = file.Extension.Substring(1).ToLower();
                    //根据扩展名分类添加节点
                    switch (fileExtension)
                    {
                        case "mdb":
                            node.Tag = file;
                            node.Text = file.Name;
                            node.Name = file.Name;
                            node.ImageIndex = 7;
                            Parent.Nodes.Add(node);
                            node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Visible;
                            break;
                        case "shp":
                            string dataPath = file.FullName;
                            //获取shp文件的FeatureClass
                            IFeatureClass pFeatureClass = GetShpFeatureClass(dataPath);
                            node.Tag = pFeatureClass;
                            node.Text = file.Name;
                            node.Name = file.Name;
                            Parent.Nodes.Add(node);
                            //根据不同的类型加载不同的图标
                            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            {
                                node.ImageIndex = 9;
                            }
                            else if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                node.ImageIndex = 10;
                            }
                            else if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                            {
                                node.ImageIndex = 11;
                            }
                            break;
                        case "dwg":
                            node.Tag = file;
                            node.Text = file.Name;
                            node.Name = file.Name;
                            node.ImageIndex = 13;
                            Parent.Nodes.Add(node);
                            node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Visible;
                            break;
                        case "tif":
                        case "jpg":
                        case "bmp":
                            node.Tag = file;
                            node.Text = file.Name;
                            node.Name = file.Name;
                            node.ImageIndex = 15;
                            Parent.Nodes.Add(node);
                            node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Visible;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 加载ListView
        /// </summary>
        /// <param name="directoryInfo"></param>
        private void LoadListView(DirectoryInfo directoryInfo)
        {
            //获取当前文件夹的子文件夹
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo dir in directoryInfos)
            {
                if ((dir.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) continue;
                ListViewItem item = lvwData.Items.Add(dir.Name);//ListView添加item
                item.Name = dir.Name;
                item.Tag = dir;
                //如果是gdb类型文件夹，则按如下方式设置item属性
                if (dir.Name.Contains(".gdb"))
                {
                    item.Text = dir.Name.Substring(0, dir.Name.Length - 4);
                    item.ImageIndex = 5;
                }
                else
                {
                    item.ImageIndex = 4;
                    item.Text = dir.Name;
                }
            }
            //获取当前文件夹的文件
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                //如果文件的扩展名不为空，则继续下面操作
                if (file.Extension != "" && file.Extension != null)
                {
                    //获取文件扩展名并转换为小写
                    string fileExtension = file.Extension.Substring(1).ToLowerInvariant();
                    ListViewItem item = null;
                    switch (fileExtension)
                    {
                        case "mdb":
                            item = lvwData.Items.Add(file.Name, 5);
                            item.Name = file.Name;
                            item.Text = file.Name;
                            item.Tag = file;
                            break;
                        case "shp":
                            string dataPath = file.FullName;
                            //获取shp文件的FeatureClass
                            IFeatureClass pFeatureClass = GetShpFeatureClass(dataPath);
                            item = lvwData.Items.Add(file.Name);
                            item.Name = file.Name;
                            item.Text = file.Name;
                            item.Tag = pFeatureClass;
                            //根据不同的类型加载不同的图标
                            if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                            {
                                item.ImageIndex = 7;
                            }
                            else if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                            {
                                item.ImageIndex = 8;
                            }
                            else if (pFeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                            {
                                item.ImageIndex = 9;
                            }
                            break;
                        case "dwg":
                            item = lvwData.Items.Add(file.Name, 11);
                            item.Name = file.Name;
                            item.Text = file.Name;
                            item.Tag = file;
                            break;
                        case "tif":
                        case "jpg":
                        case "bmp":
                            string rasterPath = file.FullName;
                            //获取栅格文件的RasterDataset
                            IRasterDataset pRasterDataset;
                            pRasterDataset = GetRasterDataset(rasterPath, fileExtension);

                            item = lvwData.Items.Add(file.Name);
                            item.Name = file.Name;
                            item.Text = file.Name;
                            item.Tag = pRasterDataset;
                            item.ImageIndex = 13;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private IRasterDataset GetRasterDataset(string dataPath,string dataType)
        {
            int index = dataPath.LastIndexOf("\\");
            string filePath = dataPath.Substring(0, index);
            string fileName = dataPath.Substring(index + 1);
            //获取shp的Workspace
            IWorkspace pWorkspace = GetWorkspace(filePath, dataType);
            IRasterWorkspace pRasterWorkspace;
            pRasterWorkspace = (IRasterWorkspace)pWorkspace;
            IRasterDataset pRasterDataset;
            pRasterDataset = pRasterWorkspace.OpenRasterDataset(fileName);
            return pRasterDataset;
        }
       /// <summary>
        /// 获取Workspace
       /// </summary>
       /// <param name="dataPath"></param>
       /// <param name="dataType"></param>
       /// <returns></returns>
        public IWorkspace GetWorkspace(string dataPath,string dataType)
        {
            IWorkspaceFactory pWorkspaceFactory=null;
            IWorkspace pWorkspace=null;
            IPropertySet pPropertySet = new PropertySetClass();
            int index;
            string filePath="";
            string dataTypeName = dataType.ToLower();
            //根据不同的数据类型获取不同的WorkspaceFactory，设置不同的IPropertySet参数
            switch (dataTypeName)
            {
                //mdb类型的dataPath为mdb本身的全路径
                case "mdb":
                    pPropertySet.SetProperty("DATABASE", dataPath);
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    break;
                //shp类型的dataPath为其父文件夹的全路径
                case "shp":
                    index = dataPath.LastIndexOf("\\");
                    filePath = dataPath.Substring(0, index);
                    pPropertySet.SetProperty("DATABASE", filePath);
                    pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                    break;
                //dwg类型的dataPath为其父文件夹的全路径
                case "dwg":
                    index = dataPath.LastIndexOf("\\");
                    filePath = dataPath.Substring(0, index);
                    pPropertySet.SetProperty("DATABASE", filePath);
                    pWorkspaceFactory = new CadWorkspaceFactoryClass();
                    break;
                //gdb类型的dataPath为gdb文件夹的全路径
                case "gdb":
                    pPropertySet.SetProperty("DATABASE", dataPath);
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    break;
                case "jpg":
                case "bmp":
                case "tif":
                    index = dataPath.LastIndexOf("\\");
                    filePath = dataPath.Substring(0, index);
                    pPropertySet.SetProperty("DATABASE", filePath);
                    pWorkspaceFactory = new RasterWorkspaceFactoryClass();
                    break;
                default:
                    break;
            }
            pPropertySet.SetProperty("DATABASE", dataPath);
            pWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
            return pWorkspace;
        }
        /// <summary>
        /// 获取IEnumDataset
        /// </summary>
        /// <param name="dataPath"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private IEnumDataset GetEnumDataset(string dataPath,string dataType)
        {
            //获取Workspace
            IWorkspace pWorkspace = GetWorkspace(dataPath,dataType);
            //根据Workspace得到所有的EnumDataset
            IEnumDataset pEnumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTAny) as IEnumDataset;
            return pEnumDataset;
        }
        /// <summary>
        /// 获取shp的FeatureClass
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        private IFeatureClass GetShpFeatureClass(string dataPath)
        {
            int index = dataPath.LastIndexOf("\\");
            string filePath = dataPath.Substring(0, index);
            string fileName = dataPath.Substring(index + 1);
            //获取shp的Workspace
            IWorkspace pWorkspace = GetWorkspace(filePath, "shp");
            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IFeatureLayer pFeatureLayer = new FeatureLayerClass();
            //获取shp的FeatureClass，其打开FeatureClass的名字是shp文件本身名字
            pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(fileName);
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            return pFeatureClass;
        }
        /// <summary>
        /// 获取CAD的FeatureClassContainer
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private IFeatureClassContainer GetFeatureClassContainer(FileInfo file)
        {
            string dwgPath = file.FullName;
            int indexdwg = dwgPath.LastIndexOf("\\");
            string filedwgPath = dwgPath.Substring(0, indexdwg);
            string filedwgName = dwgPath.Substring(indexdwg + 1);
            //获取CAD文件的Workspace
            IWorkspace dwgWorkspace = GetWorkspace(filedwgPath, "dwg");
            IFeatureWorkspace pFeatureWorkspace = dwgWorkspace as IFeatureWorkspace;
            IFeatureDataset pFeatureDataset;
            //获取CAD类型的FeatureDataset,打开CAD类型FeatureDataset的名字是CAD文件本身名字
            pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(filedwgName);
            IFeatureClassContainer pFeatureClassContainer = null;
            pFeatureClassContainer = pFeatureDataset as IFeatureClassContainer;
            return pFeatureClassContainer;
        }
        /// <summary>
        /// 获取advTree的DatasetNode节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="dataPath"></param>
        /// <param name="dataType"></param>
        private void getDatasetNode(DevComponents.AdvTree.Node parentNode,string dataPath, string dataType)
        {
            //获取EnumDataset
            IEnumDataset pEnumDataset = GetEnumDataset(dataPath, dataType);
            pEnumDataset.Reset();
            //遍历DataSet内的各个图层
            IDataset pDataset = pEnumDataset.Next();
            //判断Dataset类型，设置不同的图标与属性
            while (pDataset is IFeatureDataset)
            {
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Tag = pDataset;
                node.Text = pDataset.Name;
                node.Name = pDataset.Name;
                node.ImageIndex = 8;
                parentNode.Nodes.Add(node);
                node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Visible;
                pDataset = pEnumDataset.Next();
            }
            while (pDataset is IFeatureClass)
            {
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pDataset as IFeatureClass;
                node.Tag = pDataset;
                node.Text = pDataset.Name;
                node.Name = pDataset.Name;
                parentNode.Nodes.Add(node);
                //根据FeatureClass的不同类型，设置不同的图标
                switch (pFeatureLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        node.ImageIndex = 9;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        node.ImageIndex = 10;
                        break;
                    case esriGeometryType.esriGeometryPoint:
                        node.ImageIndex = 11;
                        break;
                    default:
                        break;
                }
                pDataset = pEnumDataset.Next();
            }
        }
        /// <summary>
        ///  获取ListViewEx的DatasetItem
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="dataPath"></param>
        /// <param name="dataType"></param>
        private void getDatasetItem(DevComponents.AdvTree.Node parentNode, string dataPath, string dataType)
        {
            //获取EnumDataset
            IEnumDataset pEnumDataset = GetEnumDataset(dataPath, dataType);
            pEnumDataset.Reset();
            //遍历DataSet内的各个图层
            IDataset pDataset = pEnumDataset.Next() as IDataset;
            //判断Dataset类型，设置不同的图标与属性
            while (pDataset is IFeatureDataset)
            {
                ListViewItem dirItem = new ListViewItem();
                dirItem = lvwData.Items.Add(pDataset.Name,6);
                dirItem.Name = pDataset.Name;
                dirItem.Text = pDataset.Name;
                dirItem.Tag = pDataset;
                pDataset = pEnumDataset.Next();
            }
            while (pDataset is IFeatureClass)
            {
                ListViewItem dirItem = new ListViewItem();
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pDataset as IFeatureClass;
                dirItem = lvwData.Items.Add(pDataset.Name);
                dirItem.Name = pDataset.Name;
                dirItem.Text = pDataset.Name;
                dirItem.Tag = pDataset;
                //根据FeatureClass的不同类型，设置不同的图标
                switch (pFeatureLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        dirItem.ImageIndex = 7;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        dirItem.ImageIndex = 8;
                        break;
                    case esriGeometryType.esriGeometryPoint:
                        dirItem.ImageIndex = 9;
                        break;
                    default:
                        break;
                }
                pDataset = pEnumDataset.Next();
            }
        }
       /// <summary>
        /// 获取advTree的FeatureClassNode节点
       /// </summary>
       /// <param name="parentNode"></param>
        private void getFeatureClassNode(DevComponents.AdvTree.Node parentNode)
        {
            IFeatureDataset pFeatureDataset = null;
            //根据节点的Tag属性获取FeatureDataset
            pFeatureDataset = (IFeatureDataset)parentNode.Tag;
            IEnumDataset pEnDS = pFeatureDataset.Subsets;
            pEnDS.Reset();
            //遍历DataSet内的各个图层
            IDataset pDS = pEnDS.Next();
            while (pDS is IFeatureClass)
            {
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pDS as IFeatureClass;
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Tag = pDS;//把Dataset赋值给item的Tag属性
                node.Name = pDS.Name;
                node.Text = pDS.Name;
                parentNode.Nodes.Add(node);
                //根据FeatureClass的不同类型，设置不同的图标
                switch (pFeatureLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        node.ImageIndex = 9;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        node.ImageIndex = 10;
                        break;
                    case esriGeometryType.esriGeometryPoint:
                        node.ImageIndex = 11;
                        break;
                    default:
                        break;
                }
                pDS = pEnDS.Next();
            }
        }
        /// <summary>
        /// 获取ListViewEx的FeatureClassItem
        /// </summary>
        /// <param name="parentNode"></param>
        private void getFeatureClassItem(DevComponents.AdvTree.Node parentNode)
        {
            IFeatureDataset pFeatureDataset;
            //根据节点的Tag属性获取FeatureDataset
            pFeatureDataset = (IFeatureDataset)parentNode.Tag;
            IEnumDataset pEnDS = pFeatureDataset.Subsets;
            pEnDS.Reset();
            //遍历DataSet内的各个图层
            IDataset pDS = pEnDS.Next();
            while (pDS is IFeatureClass)
            {
                ListViewItem dirItem = new ListViewItem();
                IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pDS as IFeatureClass;
                dirItem = lvwData.Items.Add(pDS.Name);
                dirItem.Name = pDS.Name;
                dirItem.Text = pDS.Name;
                dirItem.Tag = pDS;//把Dataset赋值给item的Tag属性
                //根据FeatureClass的不同类型，设置不同的图标
                switch (pFeatureLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        dirItem.ImageIndex = 7;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        dirItem.ImageIndex = 8;
                        break;
                    case esriGeometryType.esriGeometryPoint:
                        dirItem.ImageIndex = 9;
                        break;
                    default:
                        break;
                }
                pDS = pEnDS.Next();
            }
        }
        /// <summary>
        /// 获取CAD的FeatureClassNode
        /// </summary>
        /// <param name="parentNode"></param>
        private void getDwgFCNode(DevComponents.AdvTree.Node parentNode)
        {
            //获取当前的CAD文件
            FileInfo file = (FileInfo)parentNode.Tag;
            IFeatureClassContainer pFeatureClassContainer = null;
            //获取CAD文件的FeatureClassContainer
            pFeatureClassContainer = GetFeatureClassContainer(file);
            for (int i = 0; i < pFeatureClassContainer.ClassCount - 1; i++)
            {
                IFeatureClass pFeatureClass;
                //获取FeatureClassContainer中的FeatureClass
                pFeatureClass = pFeatureClassContainer.get_Class(i);
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Tag = pFeatureClass;
                node.Name = pFeatureClass.AliasName;
                node.Text = pFeatureClass.AliasName;
                parentNode.Nodes.Add(node);
                //根据FeatureClass的不同类型，设置不同的图标
                switch (pFeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        node.ImageIndex = 9;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        node.ImageIndex = 10;
                        break;
                    case esriGeometryType.esriGeometryPoint:
                        node.ImageIndex = 11;
                        break;
                    case esriGeometryType.esriGeometryMultiPatch:
                        node.ImageIndex = 14;
                        break;
                    default:
                        break;
                }
            }
        }        
        /// <summary>
        /// 获取CAD的FeatureClassItem
        /// </summary>
        /// <param name="parentNode"></param>
        private void getDwgFCItem(DevComponents.AdvTree.Node parentNode)
        {
            //获取当前的CAD文件
            FileInfo file = (FileInfo)parentNode.Tag;
            IFeatureClassContainer pFeatureClassContainer = null;
            //获取CAD文件的FeatureClassContainer
            pFeatureClassContainer = GetFeatureClassContainer(file);
            for (int i = 0; i < pFeatureClassContainer.ClassCount - 1; i++)
            {
                IFeatureClass pFeatureClass;
                //获取FeatureClassContainer中的FeatureClass
                pFeatureClass = pFeatureClassContainer.get_Class(i);
                ListViewItem item = new ListViewItem();
                item.Tag = pFeatureClass;
                item.Name = pFeatureClass.AliasName;
                item.Text = pFeatureClass.AliasName;
                lvwData.Items.Add(item);
                //根据FeatureClass的不同类型，设置不同的图标
                switch (pFeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        item.ImageIndex = 7;
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        item.ImageIndex = 8;
                        break;
                    case esriGeometryType.esriGeometryPoint:
                        item.ImageIndex = 9;
                        break;
                    case esriGeometryType.esriGeometryMultiPatch:
                        item.ImageIndex = 12;
                        break;
                    default:
                        break;
                }
            }
        }

        private void getRasterBandNode(DevComponents.AdvTree.Node parentNode, string dataType)
        {
            //获取当前的栅格文件
            FileInfo file = (FileInfo)parentNode.Tag;
            string dataPath = file.FullName;
            IRasterDataset pRasterDataset;
            pRasterDataset = GetRasterDataset(file.FullName, dataType);
            IRasterLayer pRasterLayer = new RasterLayerClass();
            pRasterLayer.CreateFromDataset(pRasterDataset);
            IRaster pRaster = pRasterLayer.Raster;
            IRasterBandCollection pRasterBandCol = (IRasterBandCollection)pRaster;

            IRasterBand pRasterBand = null;
            for (int i = 0; i < pRasterBandCol.Count; i++)
            {
                pRasterBand = pRasterBandCol.Item(i);
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Tag = pRasterBand;
                node.Name = pRasterBand.Bandname;
                node.Text = pRasterBand.Bandname;
                node.ImageIndex = 16;
                parentNode.Nodes.Add(node);
            }
        }

        private void getRasterBandItem(DevComponents.AdvTree.Node parentNode, string dataType)
        {
            //获取当前的栅格文件
            FileInfo file = (FileInfo)parentNode.Tag;
            string dataPath = file.FullName;
            IRasterDataset pRasterDataset;
            pRasterDataset = GetRasterDataset(file.FullName, dataType);
            IRasterLayer pRasterLayer = new RasterLayerClass();
            pRasterLayer.CreateFromDataset(pRasterDataset);
            IRaster pRaster = pRasterLayer.Raster;
            IRasterBandCollection pRasterBandCol = (IRasterBandCollection)pRaster;

            IRasterBand pRasterBand = null;
            for (int i = 0; i < pRasterBandCol.Count; i++)
            {
                pRasterBand = pRasterBandCol.Item(i);
                ListViewItem item = new ListViewItem();
                item.Tag = pRasterBand;
                item.Name = pRasterBand.Bandname;
                item.Text = pRasterBand.Bandname;
                item.ImageIndex = 14;
                lvwData.Items.Add(item);
            }
        }
        /// <summary>
        /// 返回Dataset与FeatureClass
        /// </summary>
        /// <returns></returns>
        public object[] GetData()
        {
            return DataName;
        }
        
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            //如果当前选择的irems数量大于0，则继续下面操作
            if (lvwData.SelectedItems.Count > 0)
            {
                //获取当前树目录的SelectedNode
                DevComponents.AdvTree.Node node = treData.SelectedNode;
                ListViewItem item = lvwData.SelectedItems[0];
                //根据不同的图标进行不同的操作
                switch (item.ImageIndex)
                {
                    //如果是数据层，则获取所有选择的item，据此加载选择的数据
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        DataName = new object[lvwData.SelectedItems.Count];
                        for (int i = 0; i < lvwData.SelectedItems.Count; i++)
                        {
                            DataName[i] = lvwData.SelectedItems[i];
                        }
                        this.DialogResult = DialogResult.OK;
                        
                        break;
                    default:
                        //其他情况，则根据选择的第一个item，展开当前选择节点，并设置SelectedNode为目标item对应的节点
                        node.Expanded = true;
                        for (int i = 0; i < node.Nodes.Count; i++)
                        {
                            if (node.Nodes[i].Tag.ToString() == item.Name)
                            {
                                treData.SelectedNode = node.Nodes[i];
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 窗体启动加载
        /// </summary>
        private void formLoad()
        {
            //获取系统驱动器
            DriveInfo[] drives = DriveInfo.GetDrives();
            treData.BeginUpdate();
            DevComponents.AdvTree.Node RootNode = new DevComponents.AdvTree.Node();
            //设置跟节点
            RootNode.Text = "我的电脑";
            RootNode.ImageIndex = 0;
            treData.Nodes.Add(RootNode);
            RootNode.Expanded = true;
            //遍历所有的驱动器，并添加到跟节点中
            foreach (DriveInfo drive in drives)
            {
                DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                node.Tag = drive;
                node.Name = drive.Name;
                node.Text = drive.Name;
                RootNode.Nodes.Add(node);
                if (drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable)
                {
                    node.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Visible;
                }
                //判断驱动器类型，根据类型设置不同的图标
                switch (drive.DriveType)
                {
                    case DriveType.CDRom:
                        {
                            node.ImageIndex = 1;
                            node.ImageExpandedIndex = 1;
                            break;
                        }
                    case DriveType.Removable:
                        {
                            node.ImageIndex = 3;
                            node.ImageExpandedIndex = 3;
                            break;
                        }
                    case DriveType.Fixed:
                        node.ImageIndex = 2;
                        node.ImageExpandedIndex = 2;
                        break;
                    case DriveType.Network:
                        node.ImageIndex = 4;
                        node.ImageExpandedIndex = 4;
                        break;
                    default:
                        break;
                }

            }
            treData.SelectedNode = RootNode;//根据当前选择的节点加载ListViewEx中的Item
            treData.EndUpdate();
        }

        private void AddData_Load(object sender, EventArgs e)
        {
            cboType.Text = "Datasets and Layers(*.lyr)";
            formLoad();
        }

        private void ExListView_DoubleClick(object sender, EventArgs e)
        {
            //如果当前选择的irems数量大于0，则继续下面操作
            if (lvwData.SelectedItems.Count > 0)
            {
                DevComponents.AdvTree.Node node = treData.SelectedNode;
                //获取当前树目录的SelectedNode
                ListViewItem item = lvwData.SelectedItems[0];
                //根据不同的图标进行不同的操作
                switch (item.ImageIndex)
                {
                    //如果是数据层，则获取选择的item，据此加载选择的数据
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 12:
                    case 14:
                        DataName=new object[lvwData.SelectedItems.Count];
                        for (int i = 0; i < lvwData.SelectedItems.Count; i++)
                        {
                            DataName[i] = lvwData.SelectedItems[i];
                        }
                        this.DialogResult = DialogResult.OK;
                 
                        break;
                    default:
                        //其他情况，则根据选择的第一个item，展开当前选择节点，并设置SelectedNode为目标item对应的节点
                        node.Expanded = true;
                        for (int i = 0; i < node.Nodes.Count; i++)
                        {
                            if (node.Nodes[i].Name.ToString() == item.Name)
                            {
                                treData.SelectedNode = node.Nodes[i];
                            }
                        }
                        break;
                }
            }
        }

        private void advTreeView_AfterNodeSelect(object sender, DevComponents.AdvTree.AdvTreeNodeEventArgs e)
        {
            //ListView清空
            lvwData.Items.Clear();
            DevComponents.AdvTree.Node ParentNode = e.Node;
            //根据当前选择的节点，调用不同的加载方法
            //当前节点是驱动器
            if (ParentNode.Tag is DriveInfo)
            {
                DriveInfo driveInfo = (DriveInfo)ParentNode.Tag;
                LoadListView(driveInfo.RootDirectory);
            }
            //当前节点是文件夹
            else if (ParentNode.Tag is DirectoryInfo)
            {
                DirectoryInfo dir = (DirectoryInfo)ParentNode.Tag;
                //FDB类型的文件夹
                if (dir.Name.Contains(".gdb"))
                {
                    getDatasetItem(ParentNode, dir.FullName, "gdb");
                }
                LoadListView(dir);
            }
            //当前节点时根节点
            else if (ParentNode.Tag == null)
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.DriveType == DriveType.Fixed || drive.DriveType == DriveType.Removable)
                    {
                        ListViewItem item = lvwData.Items.Add(drive.Name, 1);
                        item.Name = drive.Name;
                        item.Tag = drive;
                    }
                }
            }
            //当前节点是文件，不同类型选取不同加载方法
            else if (ParentNode.Tag is FileInfo)
            {
                FileInfo file = (FileInfo)ParentNode.Tag;
                string fileExtension = file.Extension.Substring(1).ToLower();
                switch (fileExtension)
                {
                    case "mdb":
                        getDatasetItem(ParentNode, file.FullName, "mdb");
                        break;
                    case "dwg":
                        getDwgFCItem(ParentNode);
                        break;
                    case "jpg":
                    case "bmp":
                    case "tif":
                        getRasterBandItem(ParentNode, fileExtension);
                        break;
                    default:
                        break;
                }
            }
            //当前节点是FeatureDataset
            else if (ParentNode.Tag is IFeatureDataset)
            {
                getFeatureClassItem(ParentNode);
            }
            else
                return;
        }

        private void advTreeView_BeforeExpand(object sender, DevComponents.AdvTree.AdvTreeNodeCancelEventArgs e)
        {
            DevComponents.AdvTree.Node Parent = e.Node;
            //如果当前节点的子节点个数大于0，返回，说明当前节点的子节点已经加载
            if (Parent.Nodes.Count > 0) return;
            //当前节点是驱动器
            else if (Parent.Tag is DriveInfo)
            {
                treData.BeginUpdate();
                DriveInfo driveInfo = (DriveInfo)Parent.Tag;
                LoadDirectories(Parent, driveInfo.RootDirectory);
                Parent.ExpandVisibility = DevComponents.AdvTree.eNodeExpandVisibility.Auto;
                treData.EndUpdate(true);
            }
            //当前节点是文件夹
            else if (Parent.Tag is DirectoryInfo)
            {
                DirectoryInfo dir = (DirectoryInfo)Parent.Tag;
                //当前节点是FDB类型的文件夹
                if (dir.Name.Contains(".gdb"))
                {
                    getDatasetNode(Parent, dir.FullName, "gdb");
                }
                else
                {
                    LoadDirectories(Parent, dir);
                }
            }
            //当前节点是文件，不同类型选取不同加载方法
            else if (Parent.Tag is FileInfo)
            {
                FileInfo file = (FileInfo)Parent.Tag;
                string fileExtension = file.Extension.Substring(1).ToLower();
                switch (fileExtension)
                {
                    case "mdb":
                        getDatasetNode(Parent, file.FullName, "mdb");
                        break;
                    case "dwg":
                        getDwgFCNode(Parent);
                        break;
                    case "jpg":
                    case "bmp":
                    case "tif":
                        getRasterBandNode(Parent,fileExtension);
                        break;
                    default:
                        break;
                }
            }
            //当前节点是FeatureDataset
            else if (Parent.Tag is IFeatureDataset)
            {
                getFeatureClassNode(Parent);
            }
            else
                return;
        }

        private void UpOn_Click(object sender, EventArgs e)
        {
            //获取当前的SelectedNode
            DevComponents.AdvTree.Node node = treData.SelectedNode;
            //如果当前的SelectedNode节点的父节点为空，返回；否则，设置当前选择节点为SelectedNode的父节点
            if (node.Parent == null) return;
            treData.SelectedNode = node.Parent;
        }

        private void ExListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //获取ListView中选择item的个数
            int count = lvwData.SelectedItems.Count;
            string selectedItemsName = "";
            ListViewItem item;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    item = lvwData.SelectedItems[i];
                    selectedItemsName += item.Text + ";";
                    string itemsName = selectedItemsName.Substring(0,selectedItemsName.Length-1);
                    txtName.Text = itemsName;//把所有选择的item的名字显示到相应的TextBox中，并用‘；’分隔开
                }
            }
            else
            {
                txtName.Text = "";
            }
        }

        /// <summary>
        /// 加载FeatureClass
        /// </summary>
        /// <param name="dataName"></param>
        private void AddFeatureClass(object featureClass,IMap pMap)
        {
            //获取FeatureClass
            IFeatureClass pFeatureClass = featureClass as IFeatureClass;
           IFeatureLayer pFeatureLayer;
            pFeatureLayer = new FeatureLayerClass();
            pFeatureLayer.FeatureClass = pFeatureClass;
           pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
           pMap.AddLayer(pFeatureLayer);//加载FeatureClass到Map中
            m_pFeaturelayer=pFeatureLayer;
            m_pActiveView.Refresh();
        }
       /* private void FeatureLayer(object featureClass)
        {
            object[] dataName = GetData();

            IFeatureClass pFeatureClass = featureClass as IFeatureClass;
            // IFeatureLayer pFeaturelayer;
            // pFeaturelayer = new FeatureLayerClass();
            // pFeaturelayer.FeatureClass = pFeatureClass;
            // pFeaturelayer.Name = pFeaturelayer.FeatureClass.AliasName;
            m_pFeaturelayer.FeatureClass = pFeatureClass;
        }*/
        /// <summary>
        /// 加载Dataset 
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <param name="dsName"></param>
        private void AddDataset(object dataSet, IMap pMap)
        {
            //获取FeatureDataset
            IFeatureDataset pFeatureDataset = (IFeatureDataset)dataSet;
            IWorkspace pWorkspace = pFeatureDataset.Workspace;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureLayer pFeatureLayer;
            pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            IEnumDataset pEnDS = pFeatureDataset.Subsets;
            pEnDS.Reset();
            //遍历Dataset
            IDataset pDS = pEnDS.Next();
            while (pDS is IFeatureClass)
            {
                pFeatureLayer = new FeatureLayerClass();
                pFeatureLayer.FeatureClass = pFeatureWorkspace.OpenFeatureClass(pDS.Name);
                pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                m_pFeaturelayer = pFeatureLayer;
                pMap.AddLayer(pFeatureLayer);//加载Dataset中的FeatureClass
                pDS = pEnDS.Next();
            }
            m_pActiveView.Refresh();//刷新

        }
        /// <summary>
        /// 加载CAD数据
        /// </summary>
        /// <param name="fileData"></param>
        private void AddCADData(object fileData, IMap pMap)
        {
            //获取CAD文件
            FileInfo file = (FileInfo)fileData;
            string dwgPath = file.FullName;//得到CAD文件的全路径
            int indexdwg = dwgPath.LastIndexOf("\\");
            string filedwgPath = dwgPath.Substring(0, indexdwg);
            string filedwgName = dwgPath.Substring(indexdwg + 1);
            //获取CAD文件的Workspace
            IWorkspace dwgWorkspace = GetWorkspace(filedwgPath, "dwg");
            IFeatureWorkspace pFeatureWorkspace = dwgWorkspace as IFeatureWorkspace;
            IFeatureDataset pFeatureDataset;
            IFeatureLayer pFeatureLayer = null;
            //得到CAD文件的FeatureDataset
            pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(filedwgName);

            IFeatureClassContainer pFeatureClassContainer = null;
            pFeatureClassContainer = pFeatureDataset as IFeatureClassContainer;
            //遍历并打开CAD文件
            for (int i = 0; i < pFeatureClassContainer.ClassCount - 1; i++)
            {
                IFeatureClass pFeatureClass;
                //得到FeatureClass
                pFeatureClass = pFeatureClassContainer.get_Class(i);
                if (pFeatureClass.FeatureType == esriFeatureType.esriFTCoverageAnnotation)
                {
                    pFeatureLayer = new CadAnnotationLayerClass();
                }
                else
                {
                    pFeatureLayer = new FeatureLayerClass();
                }
                pFeatureLayer.FeatureClass = pFeatureClass;
                pFeatureLayer.Name = pFeatureClass.AliasName;
                m_pFeaturelayer = pFeatureLayer;
                pMap.AddLayer(pFeatureLayer);
                m_pActiveView.Refresh();
            }
        }
        /* private void getCADlayer(object fileData)
         {
             //获取CAD文件
             FileInfo file = (FileInfo)fileData;
             string dwgPath = file.FullName;//得到CAD文件的全路径
             int indexdwg = dwgPath.LastIndexOf("\\");
             string filedwgPath = dwgPath.Substring(0, indexdwg);
             string filedwgName = dwgPath.Substring(indexdwg + 1);
             //获取CAD文件的Workspace
             IWorkspace dwgWorkspace = GetWorkspace(filedwgPath, "dwg");
             IFeatureWorkspace pFeatureWorkspace = dwgWorkspace as IFeatureWorkspace;
             IFeatureDataset pFeatureDataset;
             //IFeatureLayer pFeatureLayer = null;
             //得到CAD文件的FeatureDataset
             pFeatureDataset = pFeatureWorkspace.OpenFeatureDataset(filedwgName);

             IFeatureClassContainer pFeatureClassContainer = null;
             pFeatureClassContainer = pFeatureDataset as IFeatureClassContainer;
             //遍历并打开CAD文件
             for (int i = 0; i < pFeatureClassContainer.ClassCount - 1; i++)
             {
                 IFeatureClass pFeatureClass;
                 //得到FeatureClass
                 pFeatureClass = pFeatureClassContainer.get_Class(i);
                 /*if (pFeatureClass.FeatureType == esriFeatureType.esriFTCoverageAnnotation)
                 {
                     pFeatureLayer = new CadAnnotationLayerClass();
                 }
                 else
                 {
                     pFeatureLayer = new FeatureLayerClass();
                 }
                 //pFeatureLayer.FeatureClass = pFeatureClass;
                 m_pFeaturelayer.FeatureClass = pFeatureClass;
                 // pFeatureLayer.Name = pFeatureClass.AliasName;
                 // pMap.AddLayer(pFeatureLayer);
                 //m_pActiveView.Refresh();
             }
         }*/
        private void AddRasterDataset(object rasterDataset,IMap pMap)
        {
            IRasterDataset pRasterDataset;
            pRasterDataset = (IRasterDataset)rasterDataset;
            IRasterLayer pRasterLayer = new RasterLayerClass();
            pRasterLayer.CreateFromDataset(pRasterDataset);
            pMap.AddLayer(pRasterLayer);
        }

        private void AddRasterBand(object rasterBand,IMap pMap)
        {
            IRasterBand pRasterBand;
            pRasterBand = (IRasterBand)rasterBand;
            IRasterDataset pRasterDataset;
            pRasterDataset = (IRasterDataset)pRasterBand;
            IRasterLayer pRasterLayer=new RasterLayerClass();
            pRasterLayer.CreateFromDataset(pRasterDataset);
            pMap.AddLayer(pRasterLayer);
        }
      /*  public void GetFeatureLayer()
        {
            object[] dataName = GetData();//获取选择打开的Item
            for (int i = 0; i < dataName.Length; i++)
            {
                ListViewItem dataItem = dataName[i] as ListViewItem;
                //根据item对应的不同图标，选择不同的加载方式
                switch (dataItem.ImageIndex)
                {
                    /* case 6:
                         AddDataset(dataItem.Tag);
                         break;
                      
                    case 11:
                        getCADlayer(dataItem.Tag);
                        break;
                    default:
                        FeatureLayer(dataItem.Tag);
                        break;
                }
            }
        }*/
        public void FormAddData()
        {
            //如果返回OK，则加载相应图层
                object[] dataName = GetData();//获取选择打开的Item
                for (int i = 0; i < dataName.Length; i++)
                {
                    ListViewItem dataItem = dataName[i] as ListViewItem;
                    //根据item对应的不同图标，选择不同的加载方式
                    switch (dataItem.ImageIndex)
                    {
                        case 6:
                            AddDataset(dataItem.Tag, m_pMap);
                            break;
                        case 11:
                            AddCADData(dataItem.Tag, m_pMap);
                            break;
                        case 13:
                            AddRasterDataset(dataItem.Tag, m_pMap);
                            break;
                        case 14:
                            AddRasterBand(dataItem.Tag,m_pMap);
                            break;
                        default:
                            AddFeatureClass(dataItem.Tag, m_pMap);
                            break;
                    }
                }
                m_pActiveView.Extent = m_pActiveView.FullExtent;           
        }
    }
}