using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using DevComponents.DotNetBar;
namespace GeoDataChecker
{
    /// <summary>
    /// 初始化要素数据集，当MAP上操作的数据是离散的量，而并非是在要素数据集下，就进行创建
    /// </summary>
    public class InitializationFeatureDataset : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;


        public InitializationFeatureDataset()
        {
            base._Name = "GeoDataChecker.InitializationFeatureDataset";
            base._Caption = "预处理";
            base._Tooltip = "设置检查所需的要素数据集合";
            base._Checked = false;
            base._Visible = false;
            base._Enabled = true;
            base._Message = "预处理数据";
            
        }
        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    else
                    {
                        if (SetCheckState.CheckState)
                        {
                            base._Enabled = true;
                            return true;
                        }
                        else
                        {
                            base._Enabled = false;
                            return false;
                        }
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override void OnClick()
        {

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef hook = _AppHk as Plugin.Application.IAppFormRef;
            FrmInitiFeatureDataset frm_dataset = new FrmInitiFeatureDataset(hook);
            frm_dataset.ShowInTaskbar = false;
            frm_dataset.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;

        }
    }
}
