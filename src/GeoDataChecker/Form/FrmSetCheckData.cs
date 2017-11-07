using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry; 
namespace GeoDataChecker
{
    public partial class SetJoinCheck : DevComponents.DotNetBar.Office2007RibbonForm
    {
        private Plugin.Application.IAppArcGISRef _AppHk;//引入将要用到的MAP
        public SetJoinCheck(Plugin.Application.IAppArcGISRef AppHk)
        {
            _AppHk = AppHk;
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();//关闭窗体
        }

        private void ribbonClientPanel1_Click(object sender, EventArgs e)
        {
            btn_check.Focus();
        }

        private void btn_check_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = @"d:\";//默认打开D盘
            open.Filter = "*.mdb|*.mdb";
            open.ShowDialog();
            txt_path.Text = open.FileName;
            
        }

        /// <summary>
        /// 判断打开的MDB是不是一个已经过处理的要素集合
        /// </summary>
        private void EnterDataset()
        {
            if (txt_path.Text != "本地数据集合")
            {
                IWorkspaceFactory W_space = new AccessWorkspaceFactoryClass();
                IWorkspace space = W_space.OpenFromFile(txt_path.Text, 0);
                IEnumDataset dataset= space.get_Datasets(esriDatasetType.esriDTAny);//得到MDB里的所有集合名称
                dataset.Reset();
                IDataset set = dataset.Next();
                while (set != null)
                {
                    if (set is IFeatureClass)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "您打开的数据，存在离散数据，请打开要素集合数据！");
                        break;
                    }
                    set = dataset.Next();
                }
                AddLayer(space);
                
            }
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            EnterDataset();
        }

        private void AddLayer(IWorkspace space)
        {
            IFeatureWorkspace F_work = space as IFeatureWorkspace;
            
            IFeatureLayer f_layer = new FeatureLayerClass();
            IGroupLayer G_layer = new GroupLayerClass();
            G_layer.Name = "Geo_GroupLayer_check";

            IEnumDataset dataset = space.get_Datasets(esriDatasetType.esriDTFeatureDataset);//得到MDB里的所有集合名称
            dataset.Reset();
            IDataset set = dataset.Next();
            while (set != null)
            {

                IFeatureDataset F_dataset = set as IFeatureDataset;
                IEnumDataset dataset_each = F_dataset.Subsets;
                dataset_each.Reset();
                IDataset dataset_class = dataset_each.Next();

                if (dataset_class == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "这个集合当中没有任何的数据，请重新选择要素集合！");
                    return;
                }

                while (dataset_class != null)
                {
                    if (dataset_class is IFeatureClass)
                    {
                        f_layer = new FeatureLayerClass();
                        IFeatureClass class_each = F_work.OpenFeatureClass(dataset_class.Name); 
                        f_layer.FeatureClass = class_each;
                        f_layer.Name = dataset_class.Name;

                        G_layer.Add(f_layer as ILayer);
                    }
                    dataset_class = dataset_each.Next();
                }
                
                set = dataset.Next();
            }
            if (G_layer != null)
            {
                this.Close();
                _AppHk.MapControl.Map.AddLayer(G_layer);
                _AppHk.MapControl.ActiveView.Refresh();
            }

        }
    }
}